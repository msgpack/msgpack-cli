// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	public partial class PolymorphismSchema
	{
		private static readonly Func<PolymorphicTypeVerificationContext, bool> DefaultTypeVerfiier = _ => true;

		/// <summary>
		///		Gets a default instance.
		/// </summary>
		public static PolymorphismSchema Default { get; } = new PolymorphismSchema();

		/// <summary>
		///		ForPolymorphicObject(Type targetType)
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicObject), new[] { typeof(Type) })!;

		/// <summary>
		///		ForPolymorphicObject(Type targetType, IDictionary{byte, Type} codeTypeMapping)
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicObject), new[] { typeof(Type), typeof(IDictionary<string, Type>) })!;

		/// <summary>
		///		ForContextSpecifiedCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedCollectionMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForContextSpecifiedCollection), new[] { typeof(Type), typeof(PolymorphismSchema) })!;

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicCollection), new[] { typeof(Type), typeof(PolymorphismSchema) })!;

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicCollection), new[] { typeof(Type), typeof(IDictionary<string, Type>), typeof(PolymorphismSchema) })!;

		/// <summary>
		///		ForContextSpecifiedDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedDictionaryMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForContextSpecifiedDictionary), new[] { typeof(Type), typeof(PolymorphismSchema), typeof(PolymorphismSchema) })!;

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicDictionary), new[] { typeof(Type), typeof(PolymorphismSchema), typeof(PolymorphismSchema) })!;

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicDictionary), new[] { typeof(Type), typeof(IDictionary<string, Type>), typeof(PolymorphismSchema), typeof(PolymorphismSchema) })!;

#if FEATURE_TUPLE
		/// <summary>
		///		ForPolymorphicTuple( Type targetType, PolymorphismSchema[] itemSchemaList )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicTupleMethod =
			typeof(PolymorphismSchema).GetMethod(nameof(ForPolymorphicTuple), new[] { typeof(Type), typeof(PolymorphismSchema[]) })!;
#endif // FEATURE_TUPLE

		internal static readonly ConstructorInfo CodeTypeMapConstructor =
			typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Type))
				.GetConstructor(new[] { typeof(int) })!;

		internal static readonly MethodInfo AddToCodeTypeMapMethod =
			typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Type))
				.GetMethod("Add", new[] { typeof(string), typeof(Type) })!;

		internal string DebugString
		{
			get
			{
				var buffer = new StringBuilder();
				this.ToDebugString(buffer);
				return buffer.ToString();
			}
		}

		private void ToDebugString(StringBuilder buffer)
		{
			buffer.Append("{TargetType:").Append(this.TargetType).Append(", SchemaType:").Append(this.PolymorphismType);
			switch (this.ChildrenType)
			{
				case PolymorphismSchemaChildrenType.CollectionItems:
				{
					buffer.Append(", CollectionItemsSchema:");
					if (this.ItemSchema == null)
					{
						buffer.Append("null");
					}
					else
					{
						this.ItemSchema.ToDebugString(buffer);
					}

					break;
				}
				case PolymorphismSchemaChildrenType.DictionaryKeyValues:
				{
					buffer.Append(", DictinoaryKeysSchema:");
					if (this.KeySchema == null)
					{
						buffer.Append("null");
					}
					else
					{
						this.KeySchema.ToDebugString(buffer);
					}

					buffer.Append(", DictinoaryValuesSchema:");
					if (this.ItemSchema == null)
					{
						buffer.Append("null");
					}
					else
					{
						this.ItemSchema.ToDebugString(buffer);
					}

					break;
				}
#if FEATURE_TUPLE
				case PolymorphismSchemaChildrenType.TupleItems:
				{
					buffer.Append(", TupleItemsSchema:[");
					var isFirst = true;
					foreach (var child in this._children)
					{
						if (isFirst)
						{
							isFirst = false;
						}
						else
						{
							buffer.Append(", ");
						}

						if (child == null)
						{
							buffer.Append("null");
						}
						else
						{
							child.ToDebugString(buffer);
						}
					}

					break;
				}
#endif // FEATURE_TUPLE
			}

			buffer.Append('}');
		}

		internal static PolymorphismSchema Create(
			Type memberType,
			SerializingMember? memberMayBeNull
		)
		{
			if (memberType.GetIsValueType())
			{
				if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.DefaultSchemaForValueType))
				{
					Diagnostic.PolymorphicTrace.DefaultSchemaForValueType(
						new
						{
							targetType = memberType,
							schema = Default
						}
					);
				}

				// Value types will never be polymorphic.
				return Default;
			}

			if (memberMayBeNull == null)
			{
				var schema =
					CreateCore(
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						memberType,
#else
						type.GetTypeInfo(),
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						Default
					);
				if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.SchemaCreatedForRootType))
				{
					Diagnostic.PolymorphicTrace.SchemaCreatedForRootType(
						new
						{
							targetType = memberType,
							schema
						}
					);
				}

				return schema;
			}

			var member = memberMayBeNull.Value;

			return
				CreateCore(
					member.Member,
					CreateCore(
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						memberType,
#else
						type.GetTypeInfo(),
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						Default
					)
				);
		}

		private static PolymorphismSchema CreateCore(MemberInfo member, PolymorphismSchema defaultSchema)
		{
			var table = TypeTable.Create(member, defaultSchema);

#warning TODO: CollectionTraits can be taken from SerializingMember.
			var traits = member.GetMemberValueType().GetCollectionTraits(CollectionTraitOptions.None, allowNonCollectionEnumerableTypes: false);
			switch (traits.CollectionType)
			{
				case CollectionKind.Array:
				{
					if (!table.Member.Exists && !table.CollectionItem.Exists)
					{
						if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.DefaultSchemaForUnqualifiedCollectionMember))
						{
							Diagnostic.PolymorphicTrace.DefaultSchemaForUnqualifiedCollectionMember(
								new
								{
									targetMember = member,
									schema = defaultSchema
								}
							);
						}

						return defaultSchema;
					}

					Debug.Assert(traits.ElementType != null, "traits.ElementType != null");

#warning TODO: Non-Generic?
					var result =
						new PolymorphismSchema(
							member.GetMemberValueType(),
							table.Member.PolymorphismType,
							table.Member.CodeTypeMapping,
							table.Member.TypeVerifier,
							PolymorphismSchemaChildrenType.CollectionItems,
							new PolymorphismSchema(
								traits.ElementType,
								table.CollectionItem.PolymorphismType,
								table.CollectionItem.CodeTypeMapping,
								table.CollectionItem.TypeVerifier,
								PolymorphismSchemaChildrenType.None
							)
						);

					if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.SchemaCreatedForCollectionMember))
					{
						Diagnostic.PolymorphicTrace.SchemaCreatedForCollectionMember(
							new
							{
								targetMember = member,
								schema = result
							}
						);
					}

					return result;
				}
				case CollectionKind.Map:
				{
					if (!table.Member.Exists && !table.DictionaryKey.Exists && !table.CollectionItem.Exists)
					{
						if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.DefaultSchemaForUnqualifiedDictionaryMember))
						{
							Diagnostic.PolymorphicTrace.DefaultSchemaForUnqualifiedDictionaryMember(
								new
								{
									targetMember = member,
									schema = defaultSchema
								}
							);
						}

						return defaultSchema;
					}

					Debug.Assert(traits.ElementType != null, "traits.ElementType != null");
					var genericArguments = traits.ElementType.GetGenericArguments();

#warning TODO: Non-Generic?
					var result =
						new PolymorphismSchema(
							member.GetMemberValueType(),
							table.Member.PolymorphismType,
							table.Member.CodeTypeMapping,
							table.Member.TypeVerifier,
							PolymorphismSchemaChildrenType.DictionaryKeyValues,
							new PolymorphismSchema(
								genericArguments[0],
								table.DictionaryKey.PolymorphismType,
								table.DictionaryKey.CodeTypeMapping,
								table.DictionaryKey.TypeVerifier,
								PolymorphismSchemaChildrenType.None
							),
							new PolymorphismSchema(
								genericArguments[1],
								table.CollectionItem.PolymorphismType,
								table.CollectionItem.CodeTypeMapping,
								table.CollectionItem.TypeVerifier,
								PolymorphismSchemaChildrenType.None
							)
						);

					if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.SchemaCreatedForDictionaryMember))
					{
						Diagnostic.PolymorphicTrace.SchemaCreatedForDictionaryMember(
							new
							{
								targetMember = member,
								schema = result
							}
						);
					}

					return result;
				}
				default:
				{
#if FEATURE_TUPLE
					if (TupleItems.IsTuple(member.GetMemberValueType()))
					{
						if (table.TupleItems.Count == 0)
						{
							if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.DefaultSchemaForUnqualifiedTupleMember))
							{
								Diagnostic.PolymorphicTrace.DefaultSchemaForUnqualifiedTupleMember(
									new
									{
										targetMember = member,
										schema = defaultSchema
									}
								);
							}

							return defaultSchema;
						}

						var tupleItemTypes = TupleItems.GetTupleItemTypes(member.GetMemberValueType());
						var result =
							new PolymorphismSchema(
								member.GetMemberValueType(),
								PolymorphismType.None,
								EmptyMap,
								DefaultTypeVerfiier,
								PolymorphismSchemaChildrenType.TupleItems,
								table.TupleItems
								.Zip(tupleItemTypes, (e, t) => new { Entry = e, ItemType = t })
								.Select(e =>
								   new PolymorphismSchema(
									   e.ItemType,
									   e.Entry.PolymorphismType,
									   e.Entry.CodeTypeMapping,
									   e.Entry.TypeVerifier,
									   PolymorphismSchemaChildrenType.None
								   )
								).ToArray()
							);

						if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.SchemaCreatedForTupleMember))
						{
							Diagnostic.PolymorphicTrace.SchemaCreatedForTupleMember(
								new
								{
									targetMember = member,
									schema = result
								}
							);
						}

						return result;
					}
					else
#endif // FEATURE_TUPLE
					{
						if (!table.Member.Exists)
						{
							if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.DefaultSchemaForUnqualifiedObjectMember))
							{
								Diagnostic.PolymorphicTrace.DefaultSchemaForUnqualifiedObjectMember(
									new
									{
										targetMember = member,
										schema = defaultSchema
									}
								);
							}
							return defaultSchema;
						}

						var result =
							new PolymorphismSchema(
								member.GetMemberValueType(),
								table.Member.PolymorphismType,
								table.Member.CodeTypeMapping,
								table.Member.TypeVerifier,
								PolymorphismSchemaChildrenType.None
							);

						if (Diagnostic.PolymorphicTrace.IsEnabled(Diagnostic.PolymorphicTrace.Keys.SchemaCreatedForObjectMember))
						{
							Diagnostic.PolymorphicTrace.SchemaCreatedForObjectMember(
								new
								{
									targetMember = member,
									schema = result
								}
							);
						}
						return result;
					}
				}
			}
		}

		private struct TypeTable
		{
			public readonly TypeTableEntry Member;
			public readonly TypeTableEntry CollectionItem;
			public readonly TypeTableEntry DictionaryKey;
#if FEATURE_TUPLE
			public readonly IList<TypeTableEntry> TupleItems;
#endif // FEATURE_TUPLE

			private TypeTable(
				TypeTableEntry member,
				TypeTableEntry collectionItem,
				TypeTableEntry dictionaryKey
#if FEATURE_TUPLE
				, IList<TypeTableEntry> tupleItems
#endif // FEATURE_TUPLE
			)
			{
				this.Member = member;
				this.CollectionItem = collectionItem;
				this.DictionaryKey = dictionaryKey;
#if FEATURE_TUPLE
				this.TupleItems = tupleItems;
#endif // FEATURE_TUPLE
			}

			public static TypeTable Create(MemberInfo member, PolymorphismSchema defaultSchema)
				=> new TypeTable(
					TypeTableEntry.Create(member, PolymorphismTarget.Member, defaultSchema),
					TypeTableEntry.Create(member, PolymorphismTarget.CollectionItem, defaultSchema.TryGetItemSchema()),
					TypeTableEntry.Create(member, PolymorphismTarget.DictionaryKey, defaultSchema.TryGetKeySchema())
#if FEATURE_TUPLE
					, TypeTableEntry.CreateTupleItems(member)
#endif // !FEATURE_TUPLE
				);
		} // TypeTable

		private sealed class TypeTableEntry
		{
#if FEATURE_TUPLE
			private static readonly TypeTableEntry[] EmptyEntries = new TypeTableEntry[0];
#endif // FEATURE_TUPLE

			private readonly Dictionary<string, Type> _knownTypeMapping = new Dictionary<string, Type>();

			public IDictionary<string, Type> CodeTypeMapping => this._knownTypeMapping;

			private bool _useTypeEmbedding;

			public PolymorphismType PolymorphismType
				=> this._useTypeEmbedding ?
					PolymorphismType.RuntimeType :
					this._knownTypeMapping.Count > 0 ?
						PolymorphismType.KnownTypes :
						PolymorphismType.None;

			public bool Exists => this._useTypeEmbedding || this._knownTypeMapping.Count > 0;

			public Func<PolymorphicTypeVerificationContext, bool>? TypeVerifier { get; private set; }

			private TypeTableEntry() { }

			public static TypeTableEntry Create(MemberInfo member, PolymorphismTarget targetType, PolymorphismSchema? defaultSchema)
			{
				var result = new TypeTableEntry();
				var memberName = member.ToString()!;
				foreach (
					var attribute in
						member.GetCustomAttributes(false)
							.OfType<IPolymorphicHelperAttribute>()
							.Where(a => a.Target == targetType)
				)
				{
					// TupleItem schema should never come here, so passing -1 as tupleItemNumber is OK.
					result.Interpret(attribute, memberName, -1);
				}

				if (defaultSchema != null)
				{
					// TupleItem schema should never come here, so passing -1 as tupleItemNumber is OK.
					result.SetDefault(targetType, memberName, -1, defaultSchema);
				}

				return result;
			}

#if !NET35 && !UNITY
			public static TypeTableEntry[] CreateTupleItems(MemberInfo member)
			{
				if (!TupleItems.IsTuple(member.GetMemberValueType()))
				{
					return EmptyEntries;
				}

				var tupleItems = TupleItems.GetTupleItemTypes(member.GetMemberValueType());
				var result = tupleItems.Select(_ => new TypeTableEntry()).ToArray();
				foreach (
					var attribute in
						member.GetCustomAttributes(false)
							.OfType<IPolymorphicTupleItemTypeAttribute>()
							.OrderBy(a => a.ItemNumber)
				)
				{
					result[attribute.ItemNumber - 1].Interpret(attribute, member.ToString()!, attribute.ItemNumber);
				}

				return result;
			}
#endif // !NET35 && !UNITY

			private void Interpret(IPolymorphicHelperAttribute attribute, string memberName, int tupleItemNumber)
			{
				if (attribute is IPolymorphicKnownTypeAttribute asKnown)
				{
					this.SetKnownType(attribute.Target, memberName, tupleItemNumber, asKnown.TypeCode, asKnown.BindingType);
					return;
				}

				var asRuntimeType = attribute as IPolymorphicRuntimeTypeAttribute;
				Debug.Assert(asRuntimeType != null, attribute + " is IPolymorphicRuntimeTypeAttribute");
				if (this._useTypeEmbedding)
				{
					Debug.Assert(attribute.Target == PolymorphismTarget.TupleItem, attribute.Target + " == PolymorphismTarget.TupleItem");

					throw new SerializationException(
						$"Cannot specify multiple '{typeof(MessagePackRuntimeTupleItemTypeAttribute)}' to #{tupleItemNumber} item of tuple type member '{memberName}'."
					);
				}

				this.SetRuntimeType(attribute.Target, memberName, tupleItemNumber, GetVerifier(asRuntimeType));
			}

			private void SetDefault(PolymorphismTarget target, string memberName, int tupleItemNumber, PolymorphismSchema defaultSchema)
			{
				if (this._useTypeEmbedding || this._knownTypeMapping.Count > 0)
				{
					// Default is not required.
					return;
				}

				switch (defaultSchema.PolymorphismType)
				{
					case PolymorphismType.KnownTypes:
					{
						foreach (var typeMapping in defaultSchema.CodeTypeMapping)
						{
							this.SetKnownType(target, memberName, tupleItemNumber, typeMapping.Key, typeMapping.Value);
						}

						break;
					}
					case PolymorphismType.RuntimeType:
					{
						this.SetRuntimeType(target, memberName, tupleItemNumber, defaultSchema.TypeVerifier);
						break;
					}
				}
			}

			private void SetKnownType(PolymorphismTarget target, string memberName, int tupleItemNumber, string typeCode, Type bindingType)
			{
				if (this._useTypeEmbedding)
				{
					throw new SerializationException(
						GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage(target, memberName, tupleItemNumber)
					);
				}

				try
				{
					this._knownTypeMapping.Add(typeCode, bindingType);
					this.TypeVerifier = DefaultTypeVerfiier;
				}
				catch (ArgumentException)
				{
					throw new SerializationException(
						GetCannotDuplicateKnownTypeCodeErrorMessage(target, typeCode, memberName, tupleItemNumber)
					);
				}
			}

			private void SetRuntimeType(PolymorphismTarget target, string memberName, int tupleItemNumber, Func<PolymorphicTypeVerificationContext, bool>? typeVerifier)
			{
				if (this._knownTypeMapping.Count > 0)
				{
					throw new SerializationException(
						GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage(target, memberName, tupleItemNumber)
					);
				}

				this.TypeVerifier = typeVerifier;
				this._useTypeEmbedding = true;
			}
			private static string GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage(PolymorphismTarget target, string memberName, int? tupleItemNumber)
			{
				switch (target)
				{
					case PolymorphismTarget.CollectionItem:
					{
						return $"Cannot specify '{typeof(MessagePackRuntimeCollectionItemTypeAttribute)}' and '{typeof(MessagePackKnownCollectionItemTypeAttribute)}' simultaneously to collection items of member '{memberName}' itself.";
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return $"Cannot specify '{typeof(MessagePackRuntimeDictionaryKeyTypeAttribute)}' and '{typeof(MessagePackKnownDictionaryKeyTypeAttribute)}' simultaneously to dictionary keys of member '{memberName}' itself.";
					}
					case PolymorphismTarget.TupleItem:
					{
						return $"Cannot specify '{typeof(MessagePackRuntimeTupleItemTypeAttribute)}' and '{typeof(MessagePackKnownTupleItemTypeAttribute)}' simultaneously to #{tupleItemNumber} item of tuple type member '{memberName}' itself.";
					}
					default:
					{
						return $"Cannot specify '{typeof(MessagePackRuntimeTypeAttribute)}' and '{typeof(MessagePackKnownTypeAttribute)}' simultaneously to member '{memberName}' itself.";
					}
				}
			}

			private static string GetCannotDuplicateKnownTypeCodeErrorMessage(PolymorphismTarget target, string typeCode, string memberName, int tupleItemNumber)
			{
				switch (target)
				{
					case PolymorphismTarget.CollectionItem:
					{
						return $"Cannot specify multiple types for ext-type code '{StringEscape.ForDisplay(typeCode)}' for collection items of member '{memberName}'.";
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return $"Cannot specify multiple types for ext-type code '{StringEscape.ForDisplay(typeCode)}' for dictionary keys of member '{memberName}'.";
					}
					case PolymorphismTarget.TupleItem:
					{
						return $"Cannot specify multiple types for ext-type code '{StringEscape.ForDisplay(typeCode)}' for #{tupleItemNumber} item of tuple type member '{memberName}'.";
					}
					default:
					{
						return $"Cannot specify multiple types for ext-type code '{StringEscape.ForDisplay(typeCode)}' for member '{memberName}'.";
					}
				}
			}

			private static Func<PolymorphicTypeVerificationContext, bool> GetVerifier(IPolymorphicRuntimeTypeAttribute attribute)
			{
				if (attribute.VerifierType == null)
				{
					// Use default.
					return DefaultTypeVerfiier;
				}

				if (String.IsNullOrEmpty(attribute.VerifierMethodName))
				{
					throw new SerializationException("VerifierMethodName cannot be null nor empty if VerifierType is specified.");
				}

				// Explore [static] bool X(PolymorphicTypeVerificationContext)
				var method = attribute.VerifierType.GetRuntimeMethods().SingleOrDefault(m => IsVerificationMethod(m, attribute.VerifierMethodName));
				if (method == null)
				{
					throw new SerializationException($"A public static or instance method named '{attribute.VerifierMethodName}' with single parameter typed PolymorphicTypeVerificationContext in type '{attribute.VerifierType}'.");
				}

				if (method.IsStatic)
				{
					return method.CreateDelegate<Func<PolymorphicTypeVerificationContext, bool>>();
				}
				else
				{
					return (method.CreateDelegate(typeof(Func<PolymorphicTypeVerificationContext, bool>), Activator.CreateInstance(attribute.VerifierType)) as Func<PolymorphicTypeVerificationContext, bool>)!;
				}
			}

			private static bool IsVerificationMethod(MethodInfo method, string name)
			{
				if (method.ReturnType != typeof(bool))
				{
					return false;
				}

				if (method.Name != name)
				{
					return false;
				}

				var parameters = method.GetParameters();
				return parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(typeof(PolymorphicTypeVerificationContext));
			}
		} // TypeTableEntry
	}
}
