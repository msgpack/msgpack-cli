// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#warning TODO: Implement generator and reflection with this type!
#warning TODO: Separate factory method from this file for maintenancibility.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements serialization target member extraction logics.
	/// </summary>
	internal readonly partial struct SerializationTarget
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
						!this.CanDeserialize ?
							SerializerCapabilities.Serialize :
							this.Type.GetIsValueType() ?
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) :
								(SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize | SerializerCapabilities.DeserializeTo)
					);

		public string? GetCorrespondentMemberName(int constructorParameterIndex)
			=> this._correspondentMemberNames?[constructorParameterIndex];

		private static void VerifyType(Type targetType)
		{
			if (targetType.GetIsInterface() || targetType.GetIsAbstract())
			{
				Throw.NotSupportedBecauseCannotInstanciateAbstractType(targetType);
			}
		}

		private static void VerifyCanSerializeTargetType(ISerializerGenerationOptions options, Type targetType)
		{
			VerifyType(targetType);

			if (options.BindingOptions.IsPrivilegedAccessDisabled && !targetType.GetIsPublic() && !targetType.GetIsNestedPublic() && !ThisAssembly.Equals(targetType.GetAssembly()))
			{
				Throw.CannotSerializeNonPublicTypeUnlessPrivledgedAccessEnabled(targetType);
			}
		}

		public static SerializationTarget Prepare(ISerializerGenerationOptions options, Type targetType)
		{
			VerifyCanSerializeTargetType(options, targetType);

			var memberIgnoreList = options.BindingOptions.GetIgnoringMembers(targetType);

			var deserializationConstructor = FindDeserializationConstructor(options, targetType, out var constructorKind);
			var correspondentParameterMapping = new ConstructorParameterMapping(deserializationConstructor?.GetParameters() ?? Array.Empty<ParameterInfo>());

			var getters = GetTargetMembers(targetType, (n, t) => correspondentParameterMapping.GetMappedParameterName(n, t), options, AreInternalsVisibleTo(targetType, options))
						  .Where(getter => !memberIgnoreList.Contains(getter.MemberName, StringComparer.Ordinal))
						  .OrderBy(entry => entry.Contract.Id)
						  .ToArray();

			if (getters.Length == 0 && !options.CompatibilityOptions.SerializableInterfaceDetectors.Any(d => d(targetType, options)))
			{
				Throw.IsNotSerializableAnyway(targetType);
			}

			var memberCandidates = getters.Where(entry => entry.GetterAccessStrategy != MemberAccessStrategy.Skip).ToArray();

			// NEW SPEC Draft
			// 1. If it is "unbalanced" between getters and setters, skip unsettables.
			// 2. If there is an explicit constructor, then use it. If multiple, use .ctor() for backward compatibility [should be error, needs switch]
			// 3. If there is an parameterful consctructor, then use it.
			// 4. Use default consctructor.
			// 5. Deserialize rest members via setters. This is backward-compatible behavior.

#warning If there are any settable members, ctor is ignored in v1??? We can use both of ctor and setters...
			// Needs verification::
			// 1. .ctor -> setters
			// 2. .ctor -> IUnpackable
			// 3. .ctor -> both parameters and setter -- should be once, and the change is backward compatible.

			if (memberCandidates.Length == 0 && !options.AllowsAsymmetricSerializer)
			{
				var complementedMembers = ComplementMembers(getters, options.CompatibilityOptions, targetType);
				var correspondingMemberNames = correspondentParameterMapping.GetCorrespondentMemberNames();
				return
					new SerializationTarget(
						targetType,
						CollectionTraits.NotCollection,
						complementedMembers,
						deserializationConstructor,
						correspondingMemberNames,
						DetermineCanDeserialize(constructorKind, options, targetType, correspondingMemberNames, allowDefault: false)
					);
			}
			else
			{
				bool? canDeserialize;

				// Try to get default constructor.
				var constructor = targetType.GetConstructor(ReflectionAbstractions.EmptyTypes);
				if (constructor == null && !targetType.GetIsValueType())
				{
					// Try to get deserialization constructor.
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
					members = ComplementMembers(memberCandidates, options.CompatibilityOptions, targetType);
				}

				var correspondingMemberNames = correspondentParameterMapping.GetCorrespondentMemberNames();
				return
					new SerializationTarget(
						targetType,
						CollectionTraits.NotCollection,
						members,
						constructor,
						correspondingMemberNames,
						canDeserialize ?? DetermineCanDeserialize(constructorKind, options, targetType, correspondingMemberNames, allowDefault: true)
					);
			}
		}

		private static readonly NaiveConcurrentFifoCache<string, NaiveConcurrentFifoCache<Assembly, bool>> s_areInternalsVisibleToCache =
			new NaiveConcurrentFifoCache<string, NaiveConcurrentFifoCache<Assembly, bool>>(3);

		private static bool AreInternalsVisibleTo(Type type, ISerializerGenerationOptions options)
		{
			if (options.BindingOptions.AssumesInternalsVisibleTo)
			{
				return true;
			}

			var cacheExists = s_areInternalsVisibleToCache.TryGetValue(options.RuntimeCodeGenerationAssemblyName, out var areInternalsVisibleToCacheForGeneratingAssembly);

			if (cacheExists && areInternalsVisibleToCacheForGeneratingAssembly!.TryGetValue(type.Assembly, out var areInternalsVisibleTo))
			{
				return areInternalsVisibleTo;
			}

			if (!cacheExists)
			{
				areInternalsVisibleToCacheForGeneratingAssembly = new NaiveConcurrentFifoCache<Assembly, bool>(10);
				s_areInternalsVisibleToCache.Put(
					options.RuntimeCodeGenerationAssemblyName,
					areInternalsVisibleToCacheForGeneratingAssembly,
					(old, _) => old // discard new one because it must be empty.
				);
			}

			var result =
				type.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>().Any(
					a =>
					{
						var allowedAssembly = new AssemblyName(a.AssemblyName);
						if (allowedAssembly.GetPublicKey() != null)
						{
							return false;
						}

						return allowedAssembly.Name == options.RuntimeCodeGenerationAssemblyName;
					}
				);

			areInternalsVisibleToCacheForGeneratingAssembly!.Put(
				type.Assembly,
				result,
				(old, _) => old // 'old' and 'new' are always same, so pick up old here.
			);
			return result;
		}

		private static bool HasAnyCorrespondingMembers(IEnumerable<string?> correspondingMemberNames)
			=> correspondingMemberNames.Count(x => !String.IsNullOrEmpty(x)) > 0;

		private static bool HasDeserializableInterface(Type targetType, ISerializerGenerationOptions options)
			=> options.CompatibilityOptions.DeserializableInterfaceDetectors.Any(d => d(targetType, options));

		private static bool DetermineCanDeserialize(ConstructorKind kind, ISerializerGenerationOptions options, Type targetType, IEnumerable<string?> correspondingMemberNames, bool allowDefault)
		{
			if (HasDeserializableInterface(targetType, options))
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.DetectedAsDeserializable))
				{
					Diagnostic.GeneratorTrace.DetectedAsDeserializable(
						new
						{
							targetType,
							constructorKind = "ImplementsInterface",
							allowsDefault = allowDefault
						}
					);
				}

				return true;
			}

			switch (kind)
			{
				case ConstructorKind.Marked:
				{
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.DetectedAsDeserializable))
					{
						Diagnostic.GeneratorTrace.DetectedAsDeserializable(
							new
							{
								targetType,
								constructorKind = "MarkedWithAttribute",
								allowsDefault = allowDefault
							}
						);
					}

					return true;
				}
				case ConstructorKind.Parameterful:
				{
					var result = HasAnyCorrespondingMembers(correspondingMemberNames);
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.DetectedAsDeserializable))
					{
						Diagnostic.GeneratorTrace.DetectedAsDeserializable(
							new
							{
								targetType,
								constructorKind = "Parameterful",
								allowsDefault = allowDefault
							}
						);
					}

					return result;
				}
				case ConstructorKind.Default:
				{
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.DetectedAsDeserializable))
					{
						Diagnostic.GeneratorTrace.DetectedAsDeserializable(
							new
							{
								targetType,
								constructorKind = "Default",
								allowsDefault = allowDefault
							}
						);
					}

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

		private static IEnumerable<SerializingMember> GetTargetMembers(
			Type targetType,
			Func<string, Type, string?> correspondentParameterNameProvider,
			ISerializerGenerationOptions options,
			bool isInternalsVisible
		)
		{
			Debug.Assert(targetType != null, "type != null");

			var members = GetDistinctMembers(targetType);
			var filtered =
				members.Where(item =>
					item.GetCustomAttributesData().Any(a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName)
				).ToArray();

			if (filtered.Length > 0)
			{
				return GetAnnotatedMembersWithDuplicationDetection(targetType, filtered, correspondentParameterNameProvider, options, isInternalsVisible);
			}

			var typeAttributes = targetType.GetCustomAttributesData();
			var compatibleMembers = GetCompatibleMembers(targetType, typeAttributes, members, correspondentParameterNameProvider, options, isInternalsVisible).ToArray();
			if (compatibleMembers.Any())
			{
				return compatibleMembers;
			}

			return GetPublicUnpreventedMembers(targetType, typeAttributes, members, correspondentParameterNameProvider, options, isInternalsVisible);
		}

		private static IEnumerable<SerializingMember> GetAnnotatedMembersWithDuplicationDetection(
			Type targetType,
			MemberInfo[] filtered,
			Func<string, Type, string?> correspondentParameterNameProvider,
			ISerializerGenerationOptions options,
			bool isInternalsVisible
		)
		{
			var duplicated =
				filtered.FirstOrDefault(
					member => member.GetCustomAttributesData().Any(a => a.GetAttributeType().FullName == MessagePackIgnoreAttributeTypeName)
				);

			if (duplicated != null)
			{
				Throw.MemberIsMarkedWithMemberAndIgnoreAttribute(targetType, duplicated.Name);
			}

			return
				filtered.Select(
					member =>
					{
						var attribute = member.GetCustomAttributesData().Single(a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName);
						var memberName = (string?)GetAttributeProperty(MessagePackMemberAttributeTypeName, attribute, "Name") ?? member.Name;
						var memberType = member.GetMemberValueType();
						var memberTraits = memberType.GetCollectionTraits(CollectionTraitOptions.Full, options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes);
						var correspondentParameterName = correspondentParameterNameProvider(memberName, memberType);
						return
								new SerializingMember(
								member,
								new DataMemberContract(
									member,
									memberName,
									(NilImplication)((int?)GetAttributeProperty(MessagePackMemberAttributeTypeName, attribute, "NilImplication")).GetValueOrDefault(),
									(int?)GetAttributeArgument(MessagePackMemberAttributeTypeName, attribute, 0)
								),
								memberTraits,
								GetAccessStrategy(targetType, member, memberTraits, options, isInternalsVisible, isSetter: false, correspondentParameterName),
								GetAccessStrategy(targetType, member, memberTraits, options, isInternalsVisible, isSetter: true, correspondentParameterName)
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

		private static IEnumerable<SerializingMember> GetCompatibleMembers(
			Type targetType,
			IEnumerable<CustomAttributeData> typeAttributes,
			MemberInfo[] members,
			Func<string, Type, string?> correspondentParameterNameProvider,
			ISerializerGenerationOptions options,
			bool isInternalsVisible
		)
			=> members.Select(m =>
				(
					Member: m,
					Attribute: (options.CompatibilityOptions.MessagePackMemberAttributeCompatibilityProvider)
								.Invoke(typeAttributes, m.GetCustomAttributesData())
				)
			).Where(x => x.Attribute != null)
			.Select(x =>
				{
					var memberName = x.Attribute.GetValueOrDefault().Name ?? x.Member.Name;
					var memberType = x.Member.GetMemberValueType();
					var memberTraits = memberType.GetCollectionTraits(CollectionTraitOptions.Full, options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes);
					var correspondentParameterName = correspondentParameterNameProvider(memberName, memberType);
					return
						new SerializingMember(
							x.Member,
							new DataMemberContract(
								x.Member,
								memberName,
								x.Attribute.GetValueOrDefault().NilImplication,
								x.Attribute.GetValueOrDefault().Id
							),
							memberTraits,
							GetAccessStrategy(targetType, x.Member, memberTraits, options, isInternalsVisible, isSetter: false, correspondentParameterName),
							GetAccessStrategy(targetType, x.Member, memberTraits, options, isInternalsVisible, isSetter: true, correspondentParameterName)
						);
				}
			);

		private static IEnumerable<SerializingMember> GetPublicUnpreventedMembers(
			Type targetType,
			IEnumerable<CustomAttributeData> typeAttributes,
			MemberInfo[] members,
			Func<string, Type, string?> correspondentParameterNameProvider,
			ISerializerGenerationOptions options,
			bool isInternalsVisible
		)
			=> members.Where(
				member =>
					member.GetIsPublic()
					&& !IsNonSerializedField(member)
					&& options.CompatibilityOptions.IgnoringAttributeCompatibilityProvider(
						typeAttributes, 
						member.GetCustomAttributesData()
					) is null
					&& !IsNonSerializedField(member)
			).Select(
				(member, i) =>
				{
					var memberType = member.GetMemberValueType();
					var memberTraits = memberType.GetCollectionTraits(CollectionTraitOptions.Full, options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes);
					var correspondentParameterName = correspondentParameterNameProvider(member.Name, memberType);
					return
						new SerializingMember(
							member,
							new DataMemberContract(member),
							memberTraits,
							GetAccessStrategy(targetType, member, memberTraits, options, isInternalsVisible, isSetter: false, correspondentParameterName),
							GetAccessStrategy(targetType, member, memberTraits, options, isInternalsVisible, isSetter: true, correspondentParameterName)
						);
				}
			);

		private static bool IsNonSerializedField(MemberInfo member)
		{
			var asField = member as FieldInfo;
			if (asField == null)
			{
				return false;
			}

			return (asField.Attributes & FieldAttributes.NotSerialized) != 0;
		}

		private static ConstructorInfo? FindDeserializationConstructor(ISerializerGenerationOptions options, Type targetType, out ConstructorKind constructorKind)
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

			if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.DeserializationConstructorFound))
			{
				Diagnostic.GeneratorTrace.DeserializationConstructorFound(
					new
					{
						targetType,
						constructors = mostRichConstructors.Select(x => x.ToString()).ToArray()
					}
				);
			}

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

		private static MemberAccessStrategy GetAccessStrategy(
			Type targetType,
			MemberInfo member,
			CollectionTraits memberTraits,
			ISerializerGenerationOptions options,
			bool isInternalsVisible,
			bool isSetter,
			string? correspondentParameterName
		)
		{
			var isPublic = false;
			var isAssembly = false;
			var isReadOnly = false;

			// 1. Check member. Static, literal, or setonly member should be skipped.
			if (member is FieldInfo asField)
			{
				if (asField.IsStatic || asField.IsLiteral)
				{
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipStaticMember))
					{
						Diagnostic.GeneratorTrace.SkipStaticMember(
							new
							{
								targetMember = member
							}
						);
					}

					return MemberAccessStrategy.Skip;
				}

				isPublic = asField.IsPublic;
				isAssembly = asField.IsAssembly || asField.IsFamilyOrAssembly;
				isReadOnly = asField.IsInitOnly;
			}
			else
			{
				var asProperty = member as PropertyInfo;
				Debug.Assert(asProperty != null);

				if (asProperty.GetIndexParameters().Length > 0)
				{
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipIndexer))
					{
						Diagnostic.GeneratorTrace.SkipIndexer(
							new
							{
								targetMember = member
							}
						);
					}

					return MemberAccessStrategy.Skip;
				}

				if (!isSetter)
				{
					var getter = asProperty.GetGetMethod(nonPublic: true);
					if (getter == null)
					{
						if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipSetOnlyProperty))
						{
							Diagnostic.GeneratorTrace.SkipSetOnlyProperty(
								new
								{
									targetMember = member
								}
							);
						}

						return MemberAccessStrategy.Skip;
					}

					if (getter.IsStatic)
					{
						if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipStaticMember))
						{
							Diagnostic.GeneratorTrace.SkipStaticMember(
								new
								{
									targetMember = member
								}
							);
						}

						return MemberAccessStrategy.Skip;
					}

					isPublic = getter.IsPublic;
					isAssembly = getter.IsAssembly || getter.IsFamilyOrAssembly;
				}
				else
				{
					var setter = asProperty.GetSetMethod(nonPublic: true);
					if (setter != null)
					{
						if (setter.IsStatic)
						{
							if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipStaticMember))
							{
								Diagnostic.GeneratorTrace.SkipStaticMember(
									new
									{
										targetMember = member
									}
								);
							}

							return MemberAccessStrategy.Skip;
						}

						isPublic = setter.IsPublic;
						isAssembly = setter.IsAssembly || setter.IsFamilyOrAssembly;
					}
					else
					{
						isReadOnly = true;
					}
				}
			}

			// 2. If field/property is init only, we MAY set it via ctor.
			if (isSetter && isReadOnly)
			{
				return GetAccessStrategyForReadOnlyMember(targetType, member, memberTraits, options, correspondentParameterName);
			}

			// 3. If runtime code generation is disabled, we can only use reflection anyway.
			if (options.IsRuntimeCodeGenerationDisabled)
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseReflectionMemberAccess))
				{
					Diagnostic.GeneratorTrace.UseReflectionMemberAccess(
						new
						{
							targetMember = member
						}
					);
				}

				// Use reflection
				return MemberAccessStrategy.WithReflection;
			}

			// 4. If target is public, we can access it directly.
			if (isPublic)
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseDirectMemberAccessDueToPublic))
				{
					Diagnostic.GeneratorTrace.UseDirectMemberAccessDueToPublic(
						new
						{
							targetMember = member
						}
					);
				}

				return MemberAccessStrategy.Direct;
			}

			// 5. If assembly builder can access to target internals and target's accessibility is assembly, then we can use it directly.
			if (isInternalsVisible)
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseDirectMemberAccessDueToInternalsVisibleTo))
				{
					Diagnostic.GeneratorTrace.UseDirectMemberAccessDueToInternalsVisibleTo(
						new
						{
							targetMember = member,
							generatingAssembly = options.RuntimeCodeGenerationAssemblyName
						}
					);
				}

				return MemberAccessStrategy.Direct;
			}

			// 6. If privileged access is disabled, we can only use constructor.
			if (!options.BindingOptions.IsPrivilegedAccessDisabled)
			{
				if (isSetter && correspondentParameterName != null)
				{
					if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseConstructorForMemberAccessDueToAccessibility))
					{
						Diagnostic.GeneratorTrace.UseConstructorForMemberAccessDueToAccessibility(
							new
							{
								targetMember = member,
								correspondentParameterName
							}
						);
					}

					return MemberAccessStrategy.ViaConstrutor;
				}

				// Skip this member for now -- it leads exception when no accessible members available.
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.SkipMemberDueToAccessibility))
				{
					Diagnostic.GeneratorTrace.SkipMemberDueToAccessibility(
						new
						{
							targetMember = member
						}
					);
				}

			}

#if FEATURE_IGNORE_ACCESS_CHECKS_TO_ATTRIBUTE
			// 7. Use System.Runtime.CompilerServices.IgnoreAccessChecksToAttribute if possible in current platform.
			if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseIgnoreAccessChecksToAttributeForMemberAccess))
			{
				Diagnostic.GeneratorTrace.UseIgnoreAccessChecksToAttributeForMemberAccess(
					new
					{
						targetMember = member
					}
				);
			}

			return MemberAccessStrategy.DirectWithIgnoreAccessChecksToAttribute;
#else

			// 8. Now, we can use delegate.
			//    Delegate creation may throw MemberAccessException when required code access security permission is not grant,
			//    however, in such case, we cannot [de]serialize target type anyway.
			if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseDelegateForMemberAccess))
			{
				Diagnostic.GeneratorTrace.UseDelegateForMemberAccess(
					new
					{
						targetMember = member
					}
				);
			}

			return MemberAccessStrategy.ViaDelegate;
#endif
		}

		private static MemberAccessStrategy GetAccessStrategyForReadOnlyMember(Type targetType, MemberInfo member, CollectionTraits memberTraits, ISerializerGenerationOptions options, string? correspondentParameterName)
		{
			if (correspondentParameterName != null)
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseConstructorForMemberAccessDueToReadOnly))
				{
					Diagnostic.GeneratorTrace.UseConstructorForMemberAccessDueToReadOnly(
						new
						{
							targetMember = member,
							correspondentParameterName
						}
					);
				}

				return MemberAccessStrategy.ViaConstrutor;
			}

			switch (memberTraits.CollectionType)
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					if (memberTraits.AddMethod != null)
					{
						if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.UseCollectionAddForReadOnlyMember))
						{
							Diagnostic.GeneratorTrace.UseCollectionAddForReadOnlyMember(
								new
								{
									targetMember = member,
									correspondentParameterName
								}
							);
						}

						return MemberAccessStrategy.CollectionAdd;
					}

					break;
				}
				default:
				{
					break;
				}
			}

			if (options.AllowsAsymmetricSerializer)
			{
				if (Diagnostic.GeneratorTrace.IsEnabled(Diagnostic.GeneratorTrace.Keys.WillBeAsymmetricDueToReadOnlyMember))
				{
					Diagnostic.GeneratorTrace.WillBeAsymmetricDueToReadOnlyMember(
						new
						{
							targetType,
							targetMember = member,
							correspondentParameterName
						}
					);
				}

				return MemberAccessStrategy.WillBeAsymmetric;
			}
			else
			{
				Throw.CannotSerializeTypeWhichDeclaresReadOnlySignificantNonCollectionMemberWhenAsymmetricSerializerIsNotAllowed(targetType, member);
				return default; // never reaches
			}
		}

		private static IReadOnlyList<SerializingMember> ComplementMembers(IReadOnlyList<SerializingMember> candidates, ISerializationCompatibilityOptions options, Type targetType)
		{
			if (candidates.Count == 0)
			{
				return candidates;
			}

			if (candidates[0].Contract.Id < 0)
			{
				return candidates;
			}

			if (options.UsesOneBoundDataMemberOrder && candidates[0].Contract.Id == 0)
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
			for (int source = 0, destination = options.UsesOneBoundDataMemberOrder ? 1 : 0;
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

		public static SerializationTarget CreateForCollection(Type targetType, CollectionTraits traits, ConstructorInfo? collectionConstructor, bool canDeserialize)
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

		private sealed class ConstructorParameterMapping
		{
			private readonly Dictionary<string, string> _parameterMemberMapping;
			private readonly Dictionary<string, ParameterInfo> _parametersLookup;

			public ConstructorParameterMapping(ParameterInfo[] parameters)
			{
				this._parametersLookup = parameters.ToDictionary(p => p.Name!, StringComparer.OrdinalIgnoreCase);
				this._parameterMemberMapping = new Dictionary<string, string>(parameters.Length);
			}

			public string? GetMappedParameterName(string memberName, Type memberType)
			{
				if (!this._parametersLookup.TryGetValue(memberName, out var parameter) || parameter.ParameterType != memberType)
				{
					return null;
				}

				if (!this._parameterMemberMapping.TryAdd(parameter.Name!, memberName))
				{
					Throw.CannotDetectCorrepondentMemberNameUniquely(
						parameter,
						new [] { this._parameterMemberMapping[parameter.Name!], memberName }
					);
				}

				return parameter.Name;
			}

			public string?[] GetCorrespondentMemberNames()
				=> this._parametersLookup
					.Values
					.OrderBy(p => p.Position)
					.Select(p => this._parameterMemberMapping.TryGetValue(p.Name!, out var memberName) ? memberName : null)
					.ToArray();
		}

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
