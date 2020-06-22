// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#warning TODO: Implement generator and reflection with this type!
#warning TODO: Separate factory method from this file for maintenancibility.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements serialization target member extraction logics.
	/// </summary>
	internal readonly struct SerializationTarget
	{
		// Type names to avoid user who doesn't embed "message pack assembly's attributes" in their code directly.
		private static readonly string MessagePackMemberAttributeTypeName = typeof(MessagePackMemberAttribute).FullName!;
		private static readonly string MessagePackIgnoreAttributeTypeName = typeof(MessagePackIgnoreAttribute).FullName!;
		private static readonly string MessagePackDeserializationConstructorAttributeTypeName = typeof(MessagePackDeserializationConstructorAttribute).FullName!;
		private static readonly Assembly ThisAssembly = typeof(SerializationTarget).GetAssembly();

		public Type Type { get; }

		public CollectionTraits CollectionTraits { get; }

		public IReadOnlyList<SerializingMember> Members { get; }

		public ConstructorInfo? DeserializationConstructor { get; }

		private readonly string?[]? _correspondentMemberNames;

		public bool IsConstructorDeserialization => (this.DeserializationConstructor?.GetParameters()?.Length).GetValueOrDefault() > 0;

		public bool CanDeserialize { get; }

		public bool IsCollection => this.CollectionTraits.CollectionType != CollectionKind.NotCollection;

		private SerializationTarget(Type type, CollectionTraits traits, IReadOnlyList<SerializingMember> members, ConstructorInfo? constructor, string?[]? correspondentMemberNames, bool canDeserialize)
		{
			this.Type = type;
			this.CollectionTraits = traits;
			this.Members = members;
			this.DeserializationConstructor = constructor;
			this.CanDeserialize = canDeserialize;
			this._correspondentMemberNames = correspondentMemberNames;
		}

		public SerializerCapabilities GetCapabilities()
			=> TupleItems.IsTuple(this.Type) ?
				(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) :
				this.IsCollection ?
					(
						!this.CanDeserialize ?
							SerializerCapabilities.Serialize :
							this.CollectionTraits.AddMethod == null ?
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) :
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize | SerializerCapabilities.DeserializeTo)
					) :
					(
						!this.CanDeserialize?
							SerializerCapabilities.Serialize :
							this.Type.GetIsValueType() ?
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) :
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize | SerializerCapabilities.DeserializeTo)
					);

		private static string?[]? FindCorrespondentMemberNames(IReadOnlyList<SerializingMember> members, ConstructorInfo? constructor)
		{
			if (constructor == null)
			{
				return null;
			}

			Debug.Assert(!members.Any(m => m.Member == null), "members contains Tuple item.");

			var constructorParameters = constructor.GetParameters();
			return
					constructorParameters.GroupJoin(
						members,
						p => new KeyValuePair<string, Type>(p.Name!, p.ParameterType),
						m => new KeyValuePair<string, Type>(m.Contract.Name, m.Member!.GetMemberValueType()),
						(p, ms) => DetermineCorrespondentMemberName(p, ms),
						MemberConstructorParameterEqualityComparer.Instance
					).ToArray();
		}

		private static string? DetermineCorrespondentMemberName(ParameterInfo parameterInfo, IEnumerable<SerializingMember> members)
		{
			var membersArray = members.ToArray();
			switch (membersArray.Length)
			{
				case 0:
				{
					return null;
				}
				case 1:
				{
					return membersArray[0].MemberName;
				}
				default:
				{
					ThrowAmbigiousMatchException(parameterInfo, membersArray);
					// never
					return default!;
				}
			}
		}

		private static void ThrowAmbigiousMatchException(ParameterInfo parameterInfo, ICollection<SerializingMember> members) =>
			throw new AmbiguousMatchException(
				$"There are multiple candiates for corresponding member for parameter '{parameterInfo}' of constructor. [{String.Join(", ", members.Select(x => x.ToString()))}]"
			);

		public string? GetCorrespondentMemberName(int constructorParameterIndex)
			=> this._correspondentMemberNames?[constructorParameterIndex];

		public static void VerifyType(Type targetType)
		{
			if (targetType.GetIsInterface() || targetType.GetIsAbstract())
			{
				Throw.NotSupportedBecauseCannotInstanciateAbstractType(targetType);
			}
		}

		public static void VerifyCanSerializeTargetType(SerializerGenerationOptions options, Type targetType)
		{
			if (options.DisablesPrivilegedAccess && !targetType.GetIsPublic() && !targetType.GetIsNestedPublic() && !ThisAssembly.Equals(targetType.GetAssembly()))
			{
				Throw.CannotSerializeNonPublicTypeUnlessPrivledgedAccessEnabled(targetType);
			}
		}

		public static SerializationTarget Prepare(DiagnosticListener diag, SerializerGenerationOptions options, Type targetType)
		{
			VerifyCanSerializeTargetType(options, targetType);

			var memberIgnoreList = options.IgnoringMembers.TryGetValue(targetType, out var ignoringMembers) ? ignoringMembers : Enumerable.Empty<string>();
			var getters = GetTargetMembers(targetType, options)
						  .Where(getter => !memberIgnoreList.Contains(getter.MemberName, StringComparer.Ordinal))
						  .OrderBy(entry => entry.Contract.Id)
						  .ToArray();

			if (getters.Length == 0 && !options.SerializableAnywayInterfaceDetector(targetType, options))
			{
				Throw.IsNotSerializableAnyway(targetType);
			}

			var memberCandidates = getters.Where(entry => CheckTargetEligibility(options, entry.Member!)).ToArray();

			if (memberCandidates.Length == 0 && !options.AllowsAsymmetricSerializer)
			{
				ConstructorKind constructorKind;
				var deserializationConstructor = FindDeserializationConstructor(diag, options, targetType, out constructorKind);
				var complementedMembers = ComplementMembers(getters, options, targetType);
				var correspondingMemberNames = FindCorrespondentMemberNames(complementedMembers, deserializationConstructor);
				return
					new SerializationTarget(
						targetType,
						CollectionTraits.NotCollection,
						complementedMembers,
						deserializationConstructor,
						correspondingMemberNames,
						DetermineCanDeserialize(diag, constructorKind, options, targetType, correspondingMemberNames, allowDefault: false)
					);
			}
			else
			{
				bool? canDeserialize;
				ConstructorKind constructorKind;

				// Try to get default constructor.
				var constructor = targetType.GetConstructor(ReflectionAbstractions.EmptyTypes);
				if (constructor == null && !targetType.GetIsValueType())
				{
					// Try to get deserialization constructor.
					var deserializationConstructor = FindDeserializationConstructor(diag, options, targetType, out constructorKind);
					if (deserializationConstructor == null && !options.AllowsAsymmetricSerializer)
					{
						Throw.TargetDoesNotHavePublicDefaultConstructor(targetType);
					}

					constructor = deserializationConstructor;
					canDeserialize = null;
				}
				else if (memberCandidates.Length == 0)
				{
					Debug.Assert(options.AllowsAsymmetricSerializer);
					// Absolutely cannot deserialize in this case.
					canDeserialize = false;
					constructorKind = ConstructorKind.Ambiguous;
				}
				else
				{
					constructorKind = ConstructorKind.Default;
					// Let's prefer annotated constructor here.
					var markedConstructors = FindExplicitDeserializationConstructors(targetType.GetConstructors());
					if (markedConstructors.Count == 1)
					{
						// For backward compatibility, no exceptions are thrown here even if mulitiple deserialization constructor attributes in the type
						// just use default constructor for it.
						constructor = markedConstructors[0];
						constructorKind = ConstructorKind.Marked;
					}

					// OK, appropriate constructor and setters are found.
					canDeserialize = true;
				}

				if (constructor != null && constructor.GetParameters().Any() || options.AllowsAsymmetricSerializer)
				{
					// Recalculate members because getter-only/readonly members should be included for constructor deserialization.
					memberCandidates = getters;
				}

				// Because members' order is equal to declared order is NOT guaranteed, so explicit ordering is required.
				IReadOnlyList<SerializingMember> members;
				if (memberCandidates.All(item => item.Contract.Id == DataMemberContract.UnspecifiedId))
				{
					// Alphabetical order.
					members = memberCandidates.OrderBy(item => item.Contract.Name).ToArray();
				}
				else
				{
					// ID order.
					members = ComplementMembers(memberCandidates, options, targetType);
				}

				var correspondingMemberNames = FindCorrespondentMemberNames(members, constructor);
				return
					new SerializationTarget(
						targetType,
						CollectionTraits.NotCollection,
						members,
						constructor,
						correspondingMemberNames,
						canDeserialize ?? DetermineCanDeserialize(diag, constructorKind, options, targetType, correspondingMemberNames, allowDefault: true)
					);
			}
		}

		private static bool HasAnyCorrespondingMembers(IEnumerable<string?> correspondingMemberNames)
			=> correspondingMemberNames.Count(x => !String.IsNullOrEmpty(x)) > 0;

		private static bool HasDeserializableInterface(Type targetType, SerializerGenerationOptions options)
			=> options.DeserializableInterfaceDetector(targetType, options);

		private static bool DetermineCanDeserialize(DiagnosticSource diag, ConstructorKind kind, SerializerGenerationOptions options, Type targetType, IEnumerable<string?> correspondingMemberNames, bool allowDefault)
		{
			if (HasDeserializableInterface(targetType, options))
			{
				diag.DetectedAsDeserializable(
					new
					{
						targetType,
						constructorKind = "ImplementsInterface",
						allowsDefault = allowDefault
					}
				);
				return true;
			}

			switch (kind)
			{
				case ConstructorKind.Marked:
				{
					diag.DetectedAsDeserializable(
						new
						{
							targetType,
							constructorKind = "MarkedWithAttribute",
							allowsDefault = allowDefault
						}
					);
					return true;
				}
				case ConstructorKind.Parameterful:
				{
					var result = HasAnyCorrespondingMembers(correspondingMemberNames);
					diag.DetectedAsDeserializable(
						new
						{
							targetType,
							constructorKind = "Parameterful",
							allowsDefault = allowDefault
						}
					);
					return result;
				}
				case ConstructorKind.Default:
				{
					diag.DetectedAsDeserializable(
						new
						{
							targetType,
							constructorKind = "Default",
							allowsDefault = allowDefault
						}
					);
					return allowDefault;
				}
				default:
				{
					Debug.Assert(kind == ConstructorKind.None || kind == ConstructorKind.Ambiguous, "kind == ConstructorKind.None || kind == ConstructorKind.Ambiguous : " + kind);
					return false;
				}
			}
		}

		private static MemberInfo[] GetDistinctMembers(Type type)
		{
			var distinctMembers = new List<MemberInfo>();
			var returningMemberNamesSet = new HashSet<string>();
			while (type != typeof(object) && type != null)
			{
				var members =
					type.FindMembers(
						MemberTypes.Field | MemberTypes.Property,
						BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly,
						null,
						null
					);
				foreach (var memberInfo in members)
				{
					if (returningMemberNamesSet.Add(memberInfo.Name)) //HashSet returns true is new key was added
					{
						distinctMembers.Add(memberInfo);
					}
				}

				type = type.GetBaseType();
			}

			return distinctMembers.ToArray();
		}

		private static IEnumerable<SerializingMember> GetTargetMembers(Type type, SerializerGenerationOptions options)
		{
			Debug.Assert(type != null, "type != null");

			var members = GetDistinctMembers(type);
			var filtered =
				members.Where(item =>
					item.GetCustomAttributesData().Any(a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName)
				).ToArray();

			if (filtered.Length > 0)
			{
				return GetAnnotatedMembersWithDuplicationDetection(type, filtered);
			}

			var compatibleMembers = GetCompatibleMembers(type.GetCustomAttributesData(), members, options).ToArray();
			if (compatibleMembers.Any())
			{
				return compatibleMembers;
			}

			return GetPublicUnpreventedMembers(members, options);
		}

		private static IEnumerable<SerializingMember> GetAnnotatedMembersWithDuplicationDetection(Type type, MemberInfo[] filtered)
		{
			var duplicated =
				filtered.FirstOrDefault(
					member => member.GetCustomAttributesData().Any(a => a.GetAttributeType().FullName == MessagePackIgnoreAttributeTypeName)
				);

			if (duplicated != null)
			{
				Throw.MemberIsMarkedWithMemberAndIgnoreAttribute(type, duplicated.Name);
			}

			return
				filtered.Select(
					member =>
					{
						var attribute = member.GetCustomAttributesData().Single(a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName);
						return
							new SerializingMember(
								member,
								new DataMemberContract(
									member,
									(string?)GetAttributeProperty(MessagePackMemberAttributeTypeName, attribute, "Name"),
									(NilImplication)((int?)GetAttributeProperty(MessagePackMemberAttributeTypeName, attribute, "NilImplication")).GetValueOrDefault(),
									(int?)GetAttributeArgument(MessagePackMemberAttributeTypeName, attribute, 0)
								)
							);
					}
				);
		}

		private static object? GetAttributeArgument(string attributeName, CustomAttributeData attribute, int index)
		{
			var arguments = attribute.GetConstructorArguments();
			if (arguments.Count < index)
			{
				// Use default.
				return null;
			}

			return arguments[index].Value;
		}

		private static object? GetAttributeProperty(string attributeName, CustomAttributeData attribute, string propertyName)
		{
			var property = attribute.GetNamedArguments().SingleOrDefault(a => a.GetMemberName() == propertyName);
			if (property.GetMemberName() == null)
			{
				// Use default.
				return null;
			}

			return property.GetTypedValue().Value;
		}

		private static MessagePackMemberAttributeData? ProvideDefaultMessagePackMemberAttributeCompatibility(
			IEnumerable<CustomAttributeData> typeAttributes,
			IEnumerable<CustomAttributeData> memberAttributes
		)
		{
			if (!typeAttributes.Any(a => a.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute"))
			{
				return null;
			}

			var dataMemberAttribute =
				memberAttributes
				.FirstOrDefault(a =>
					a.GetAttributeType()
					.FullName == "System.Runtime.Serialization.DataMemberAttribute"
				);
			if (dataMemberAttribute == null)
			{
				return null;
			}

			var name = default(string?);
			var order = default(int?);
			foreach(var namedArgument in dataMemberAttribute.GetNamedArguments())
			{
				var memberName = namedArgument.GetMemberName();
				if (memberName == "Name")
				{
					name = (string?)namedArgument.GetTypedValue().Value;
				}
				else if(memberName == "Order")
				{
					order = (int?)namedArgument.GetTypedValue().Value;
				}
			}

			return new MessagePackMemberAttributeData(order < 0 ? null : order, name);
		}

		private static IEnumerable<SerializingMember> GetCompatibleMembers(IEnumerable<CustomAttributeData> typeAttributes, MemberInfo[] members, SerializerGenerationOptions options)
			=> members.Select(m => 
				(
					Member: m,
					Attribute: (options.MessagePackMemberAttributeCompatibilityProvider ?? ProvideDefaultMessagePackMemberAttributeCompatibility)
								.Invoke(typeAttributes, m.GetCustomAttributesData())
				)
			).Where(x => x.Attribute != null)
			.Select(x =>
				new SerializingMember(
					x.Member,
					// TODO: NilImplication
					new DataMemberContract(x.Member, x.Attribute.GetValueOrDefault().Name, NilImplication.MemberDefault, x.Attribute.GetValueOrDefault().Id)
				)
			);

		private static IEnumerable<SerializingMember> GetPublicUnpreventedMembers(MemberInfo[] members, SerializerGenerationOptions options)
		{
			return members.Where(
				member =>
					member.GetIsPublic()
					&& !member
#if !UNITY
						.GetCustomAttributesData()
						.Select(data => data.GetAttributeType().FullName!)
#else
						.GetCustomAttributes( true )
						.Select(data => data.GetType().FullName)
#endif // !UNITY
						.Any(t => options.IgnoreAttributeTypeNames.Contains(t))
					&& !IsNonSerializedField(member)
				).Select(member => new SerializingMember(member, new DataMemberContract(member)));
		}

		private static bool IsNonSerializedField(MemberInfo member)
		{
			var asField = member as FieldInfo;
			if (asField == null)
			{
				return false;
			}

			return (asField.Attributes & FieldAttributes.NotSerialized) != 0;
		}

		private static ConstructorInfo? FindDeserializationConstructor(DiagnosticSource diag, SerializerGenerationOptions options, Type targetType, out ConstructorKind constructorKind)
		{
			var constructors = targetType.GetConstructors().ToArray();

			if (constructors.Length == 0)
			{
				if (options.AllowsAsymmetricSerializer)
				{
					constructorKind = ConstructorKind.None;
					return null;
				}
				else
				{
					Throw.TypeCannotBeSerializedBecauseNoMembersAndNoParameterizedPublicConstructors(targetType);
					// never
					constructorKind = default;
					return default;
				}
			}

			// The marked construtor is always preferred.
			var markedConstructors = FindExplicitDeserializationConstructors(constructors);
			switch (markedConstructors.Count)
			{
				case 0:
				{
					break;
				}
				case 1:
				{
					// OK use it for deserialization.
					constructorKind = ConstructorKind.Marked;
					return markedConstructors[0];
				}
				default:
				{
					Throw.TypeCannotBeSerializedBecauseThereAreMultipleMessagePackDeserializationConstrutorAttribute(targetType);
					// never
					constructorKind = default;
					return default;
				}
			}

			// A constructor which has most parameters will be used.
			var mostRichConstructors =
				constructors.GroupBy(ctor => ctor.GetParameters().Length).OrderByDescending(g => g.Key).First().ToArray();

			diag.DeserializationConstructorFound(
				new
				{
					targetType = targetType,
					constructors = mostRichConstructors.Select(x => x.ToString()).ToArray()
				}
			);

			switch (mostRichConstructors.Length)
			{
				case 1:
				{
					if (mostRichConstructors[0].GetParameters().Length == 0)
					{
						if (options.AllowsAsymmetricSerializer)
						{
							constructorKind = ConstructorKind.Default;
							return mostRichConstructors[0];
						}
						else
						{
							Throw.TypeCannotBeSerializedBecauseNoMembersAndNoParameterizedPublicConstructors(targetType);
							// never
							constructorKind = default;
							return default;
						}
					}

					// OK try use it but it may not handle deserialization correctly.
					constructorKind = ConstructorKind.Parameterful;
					return mostRichConstructors[0];
				}
				default:
				{
					if (options.AllowsAsymmetricSerializer)
					{
						constructorKind = ConstructorKind.Ambiguous;
						return null;
					}
					else
					{
						Throw.TypeCannotBeSerializedBecauseNoMembersAndAmbigiousConstructors(targetType, mostRichConstructors);
						// never
						constructorKind = default;
						return default;
					}
				}
			}
		}

		private static IList<ConstructorInfo> FindExplicitDeserializationConstructors(IEnumerable<ConstructorInfo> construtors)
		{
			return
				construtors
				.Where(ctor =>
#if !UNITY
					ctor.GetCustomAttributesData().Any(a =>
					   a.GetAttributeType().FullName
#else
					ctor.GetCustomAttributes( true ).Any( a =>
						a.GetType().FullName
#endif // !UNITY
						== MessagePackDeserializationConstructorAttributeTypeName
				   )
				).ToArray();
		}


		private static bool CheckTargetEligibility(SerializerGenerationOptions options, MemberInfo member)
		{
			Type returnType;

			if (member is PropertyInfo asProperty)
			{
				if (asProperty.GetIndexParameters().Length > 0)
				{
					// Indexer cannot be target except the type itself implements IDictionary or IDictionary<TKey,TValue>
					return false;
				}

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				var setter = asProperty.GetSetMethod(true);
#else
				var setter = asProperty.SetMethod;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
				if (setter != null)
				{
					if (setter.GetIsPublic())
					{
						return true;
					}

					if (!options.DisablesPrivilegedAccess)
					{
						// Can deserialize non-public setter if privileged.
						return true;
					}
				}

				returnType = asProperty.PropertyType;
			}
			else if (member is FieldInfo asField)
			{
				if (!asField.IsInitOnly)
				{
					return true;
				}

				returnType = asField.FieldType;
			}
			else
			{
				Debug.Fail($"Unknown type member '{member}'");
				return true;
			}

			var traits = returnType.GetCollectionTraits(CollectionTraitOptions.WithAddMethod, allowNonCollectionEnumerableTypes: false);
			switch (traits.CollectionType)
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					return traits.AddMethod != null;
				}
				default:
				{
					return false;
				}
			}
		}

		private static IReadOnlyList<SerializingMember> ComplementMembers(IReadOnlyList<SerializingMember> candidates, SerializerGenerationOptions options, Type targetType)
		{
			if (candidates.Count == 0)
			{
				return candidates;
			}

			if (candidates[0].Contract.Id < 0)
			{
				return candidates;
			}

			if (options.OneBoundDataMemberOrder && candidates[0].Contract.Id == 0)
			{
				Throw.DataMemberAttributeCannotBeZeroWhenOneBoundDataMemberOrderIsTrue(targetType, candidates[0].MemberName);
			}

#if !UNITY
			var maxId = candidates.Max(item => item.Contract.Id);
#else
			int maxId = -1;
			foreach ( var id in candidates.Select( item => item.Contract.Id ) )
			{
				maxId = Math.Max( id, maxId );
			}
#endif
			var result = new List<SerializingMember>(maxId + 1);
			for (int source = 0, destination = options.OneBoundDataMemberOrder ? 1 : 0;
				source < candidates.Count;
				source++, destination++)
			{
				Debug.Assert(candidates[source].Contract.Id >= 0, $"candidates[source].Contract.Id ({candidates[source].Contract.Id}) >= 0");

				if (candidates[source].Contract.Id < destination)
				{
					Throw.MemberIdIsDuplicated(candidates[source].Contract.Id, targetType);
				}

				while (candidates[source].Contract.Id > destination)
				{
					result.Add(new SerializingMember());
					destination++;
				}

				result.Add(candidates[source]);
			}

			VerifyNilImplication(targetType, result);
			VerifyKeyUniqueness(result);
			return result;
		}

		private static void VerifyNilImplication(Type type, IEnumerable<SerializingMember> entries)
		{
			foreach (var serializingMember in entries)
			{
				Debug.Assert(serializingMember.Member != null, "VerifyNilImplication is called for Tuple.");

				if (serializingMember.Contract.NilImplication == NilImplication.Null)
				{
					var itemType = serializingMember.Member.GetMemberValueType();

					if (!itemType.IsDefined(typeof(MessagePackNullableAttribute))
						&& !itemType.IsNullableType())
					{
						Throw.ValueTypeCannotBeNull(serializingMember.Member.ToString(), itemType, type);
					}

					bool isReadOnly;
					if (serializingMember.Member is FieldInfo asField)
					{
						isReadOnly = asField.IsInitOnly;
					}
					else
					{
						var asProperty = serializingMember.Member as PropertyInfo;
						Debug.Assert(asProperty != null, serializingMember.Member.ToString());
						isReadOnly = asProperty.GetSetMethod() == null;
					}

					if (isReadOnly)
					{
						Throw.NullIsProhibitedForReadOnlyMember(serializingMember.Member.ToString());
					}
				}
			}
		}

		private static void VerifyKeyUniqueness(IList<SerializingMember> result)
		{
			var duplicated = new Dictionary<string, List<MemberInfo>>();
			var existents = new Dictionary<string, SerializingMember>();
			foreach (var member in result)
			{
				if (member.Contract.Name == null)
				{
					continue;
				}

				Debug.Assert(member.Member != null, "VerifyKeyUniqueness is called for Tuple.");

				try
				{
					existents.Add(member.Contract.Name, member);
				}
				catch (ArgumentException)
				{
					if (duplicated.TryGetValue(member.Contract.Name, out var list))
					{
						list.Add(member.Member);
					}
					else
					{
						duplicated.Add(member.Contract.Name, new List<MemberInfo> { existents[member.Contract.Name].Member!, member.Member });
					}
				}
			}

			if (duplicated.Count > 0)
			{
				Throw.DuplicatedAttributes(duplicated);
			}
		}

		public static SerializationTarget CreateForCollection(Type targetType, CollectionTraits traits, ConstructorInfo collectionConstructor, bool canDeserialize)
			=> new SerializationTarget(targetType, traits, Array.Empty<SerializingMember>(), collectionConstructor, Array.Empty<string>(), canDeserialize);

		public static SerializationTarget CreateForTuple(Type targetType)
			=> new SerializationTarget(
				targetType,
				CollectionTraits.NotCollection,
				TupleItems.GetTupleItemMembers(targetType).Select((m, i) => new SerializingMember(m, GetTupleItemNameFromIndex(i))).ToArray(),
				constructor: null,
				correspondentMemberNames: null,
				canDeserialize: true
			);

		public static string GetTupleItemNameFromIndex(int i)
			=> $"Item{i + 1:D}";

#warning TODO: REMOVE_CODEGEN
#if FEATURE_CODEGEN
		public static bool BuiltInSerializerExists( ISerializerGeneratorConfiguration configuration, Type type, CollectionTraits traits )
		{
			return GenericSerializer.IsSupported( type, traits, configuration.PreferReflectionBasedSerializer ) || SerializerRepository.InternalDefault.ContainsFor( type );
		}
#endif // FEATURE_CODEGEN

		private sealed class MemberConstructorParameterEqualityComparer : EqualityComparer<KeyValuePair<string, Type>>
		{
			public static readonly IEqualityComparer<KeyValuePair<string, Type>> Instance = new MemberConstructorParameterEqualityComparer();

			private MemberConstructorParameterEqualityComparer() { }

			public override bool Equals(KeyValuePair<string, Type> x, KeyValuePair<string, Type> y)
			{
				return String.Equals(x.Key, y.Key, StringComparison.OrdinalIgnoreCase) && x.Value == y.Value;
			}

			public override int GetHashCode(KeyValuePair<string, Type> obj)
			{
				return (obj.Key == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Key)) ^ (obj.Value == null ? 0 : obj.Value.GetHashCode());
			}
		}

		private enum ConstructorKind
		{
			None = 0,
			Marked,
			Default,
			Parameterful,
			Ambiguous
		}
	}
}
