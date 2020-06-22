// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	partial class PolymorphismSchema
	{
		private static readonly Func<PolymorphicTypeVerificationContext, bool> DefaultTypeVerfiier = _ => true;

		/// <summary>
		///		Gets a default instance.
		/// </summary>
		public static PolymorphismSchema Default { get; } = new PolymorphismSchema();

		/// <summary>
		///		ForPolymorphicObject( Type targetType )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicObject", new[] { typeof(Type) });

		/// <summary>
		///		ForPolymorphicObject( Type targetType, IDictionary{byte, Type} codeTypeMapping )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicObject", new[] { typeof(Type), typeof(IDictionary<string, Type>) });

		/// <summary>
		///		ForContextSpecifiedCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedCollectionMethod =
			typeof(PolymorphismSchema).GetMethod("ForContextSpecifiedCollection", new[] { typeof(Type), typeof(PolymorphismSchema) });

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicCollection", new[] { typeof(Type), typeof(PolymorphismSchema) });

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicCollection", new[] { typeof(Type), typeof(IDictionary<string, Type>), typeof(PolymorphismSchema) });

		/// <summary>
		///		ForContextSpecifiedDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedDictionaryMethod =
			typeof(PolymorphismSchema).GetMethod("ForContextSpecifiedDictionary", new[] { typeof(Type), typeof(PolymorphismSchema), typeof(PolymorphismSchema) });

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryTypeEmbeddingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicDictionary", new[] { typeof(Type), typeof(PolymorphismSchema), typeof(PolymorphismSchema) });

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryCodeTypeMappingMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicDictionary", new[] { typeof(Type), typeof(IDictionary<string, Type>), typeof(PolymorphismSchema), typeof(PolymorphismSchema) });

#if !NET35 && !UNITY
		/// <summary>
		///		ForPolymorphicTuple( Type targetType, PolymorphismSchema[] itemSchemaList )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicTupleMethod =
			typeof(PolymorphismSchema).GetMethod("ForPolymorphicTuple", new[] { typeof(Type), typeof(PolymorphismSchema[]) });
#endif // !NET35 && !UNITY

		internal static readonly ConstructorInfo CodeTypeMapConstructor =
			typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Type))
				.GetConstructor(new[] { typeof(int) });

		internal static readonly MethodInfo AddToCodeTypeMapMethod =
			typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Type))
				.GetMethod("Add", new[] { typeof(string), typeof(Type) });


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
#if !NET35 && !UNITY
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
#endif // !NET35 && !UNITY
			}

			buffer.Append('}');
		}

		internal static PolymorphismSchema Create(
			DiagnosticSource diag,
			Type type,
#if !UNITY
			SerializingMember? memberMayBeNull
#else
			SerializingMember memberMayBeNull
#endif // !UNITY
			)
		{
			if (type.GetIsValueType())
			{
				diag.DefaultPolymorphicSchemaForValueType(
					new
					{
						targetType =
							memberMayBeNull == null ?
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
							type :
#else
							type.GetTypeInfo() :
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
#if !UNITY
							memberMayBeNull.Value.Member,
#else
							memberMayBeNull.Member,
#endif
						schema = Default
					}
				);
				// Value types will never be polymorphic.
				return Default;
			}

			if (memberMayBeNull == null)
			{
				var schema =
					CreateCore(
						diag,
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						type,
#else
						type.GetTypeInfo(),
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						Default
					);
				diag.PolymorphicSchemaCreatedForRootType(
					new
					{
						targetType =
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
							type,
#else
							type.GetTypeInfo(),
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						schema = schema
					}
				);
				return schema;
			}

#if !UNITY
			var member = memberMayBeNull.Value;
#else
			var member = memberMayBeNull;
#endif // !UNITY

			return
				CreateCore(
					diag,
					member.Member,
					CreateCore(
						diag,
#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						type,
#else
						type.GetTypeInfo(),
#endif // !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
						Default
					)
				);
		}

		private static PolymorphismSchema CreateCore(DiagnosticSource diag, MemberInfo member, PolymorphismSchema defaultSchema)
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
						diag.DefaultPolymorphicSchemaForUnqualifiedCollectionMember(
							new
							{
								targetMember = member,
								schema = defaultSchema
							}
						);
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
					diag.PolymorphicSchemaCreatedForCollectionMember(
						new
						{
							targetMember = member,
							schema = result
						}
					);
					return result;
				}
				case CollectionKind.Map:
				{
					if (!table.Member.Exists && !table.DictionaryKey.Exists && !table.CollectionItem.Exists)
					{
						diag.DefaultPolymorphicSchemaForUnqualifiedDictionaryMember(
							new
							{
								targetMember = member,
								schema = defaultSchema
							}
						);
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

					diag.PolymorphicSchemaCreatedForDictionaryMember(
						new
						{
							targetMember = member,
							schema = result
						}
					);
					return result;
				}
				default:
				{
#if !NET35 && !UNITY
					if (TupleItems.IsTuple(member.GetMemberValueType()))
					{
						if (table.TupleItems.Count == 0)
						{
							diag.DefaultPolymorphicSchemaForUnqualifiedTupleMember(
								new
								{
									targetMember = member,
									schema = defaultSchema
								}
							);
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

						diag.PolymorphicSchemaCreatedForTupleMember(
							new
							{
								targetMember = member,
								schema = result
							}
						);
						return result;
					}
					else
#endif // !NET35 && !UNITY
					{
						if (!table.Member.Exists)
						{
							diag.DefaultPolymorphicSchemaForUnqualifiedObjectMember(
								new
								{
									targetMember = member,
									schema = defaultSchema
								}
							);
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
						diag.PolymorphicSchemaCreatedForObjectMember(
							new
							{
								targetMember = member,
								schema = result
							}
						);
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
#if !NET35 && !UNITY
			public readonly IList<TypeTableEntry> TupleItems;
#endif // !NET35 && !UNITY

			private TypeTable(
				TypeTableEntry member,
				TypeTableEntry collectionItem,
				TypeTableEntry dictionaryKey
#if !NET35 && !UNITY
				, IList<TypeTableEntry> tupleItems
#endif // !NET35 && !UNITY
			)
			{
				this.Member = member;
				this.CollectionItem = collectionItem;
				this.DictionaryKey = dictionaryKey;
#if !NET35 && !UNITY
				this.TupleItems = tupleItems;
#endif // !NET35 && !UNITY
			}

			public static TypeTable Create(MemberInfo member, PolymorphismSchema defaultSchema)
			{
				return
					new TypeTable(
						TypeTableEntry.Create(member, PolymorphismTarget.Member, defaultSchema),
						TypeTableEntry.Create(member, PolymorphismTarget.CollectionItem, defaultSchema.TryGetItemSchema()),
						TypeTableEntry.Create(member, PolymorphismTarget.DictionaryKey, defaultSchema.TryGetKeySchema())
#if !NET35 && !UNITY
						, TypeTableEntry.CreateTupleItems(member)
#endif // !NET35 && !UNITY
					);
			}
		}

		private sealed class TypeTableEntry
		{
#if !NET35 && !UNITY
			private static readonly TypeTableEntry[] EmptyEntries = new TypeTableEntry[0];
#endif // !NET35 && !UNITY

			private readonly Dictionary<string, Type> _knownTypeMapping = new Dictionary<string, Type>();

			public IDictionary<string, Type> CodeTypeMapping { get { return this._knownTypeMapping; } }

			private bool _useTypeEmbedding;

			public PolymorphismType PolymorphismType
			{
				get
				{
					return
						this._useTypeEmbedding
							? PolymorphismType.RuntimeType
							: this._knownTypeMapping.Count > 0
								? PolymorphismType.KnownTypes
								: PolymorphismType.None;
				}
			}

			public bool Exists { get { return this._useTypeEmbedding || this._knownTypeMapping.Count > 0; } }

			public Func<PolymorphicTypeVerificationContext, bool> TypeVerifier { get; private set; }

			private TypeTableEntry() { }

			public static TypeTableEntry Create(MemberInfo member, PolymorphismTarget targetType, PolymorphismSchema defaultSchema)
			{
				var result = new TypeTableEntry();
				var memberName = member.ToString();
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
					result[attribute.ItemNumber - 1].Interpret(attribute, member.ToString(), attribute.ItemNumber);
				}

				return result;
			}
#endif // !NET35 && !UNITY

			private void Interpret(IPolymorphicHelperAttribute attribute, string memberName, int tupleItemNumber)
			{
				var asKnown = attribute as IPolymorphicKnownTypeAttribute;
				if (asKnown != null)
				{
					this.SetKnownType(attribute.Target, memberName, tupleItemNumber, asKnown.TypeCode, asKnown.BindingType);
					return;
				}

#if DEBUG
				Contract.Assert(attribute is IPolymorphicRuntimeTypeAttribute, attribute + " is IPolymorphicRuntimeTypeAttribute");
#endif // DEBUG
				if (this._useTypeEmbedding)
				{
#if DEBUG
					Contract.Assert(attribute.Target == PolymorphismTarget.TupleItem, attribute.Target + " == PolymorphismTarget.TupleItem");
#endif // DEBUG
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Cannot specify multiple '{0}' to #{1} item of tuple type member '{2}'.",
							typeof(MessagePackRuntimeTupleItemTypeAttribute),
							tupleItemNumber,
							memberName
						)
					);
				}

				this.SetRuntimeType(attribute.Target, memberName, tupleItemNumber, GetVerifier(attribute as IPolymorphicRuntimeTypeAttribute));
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

			private void SetRuntimeType(PolymorphismTarget target, string memberName, int tupleItemNumber, Func<PolymorphicTypeVerificationContext, bool> typeVerifier)
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
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to collection items of member '{2}' itself.",
								typeof(MessagePackRuntimeCollectionItemTypeAttribute),
								typeof(MessagePackKnownCollectionItemTypeAttribute),
								memberName
							);
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to dictionary keys of member '{2}' itself.",
								typeof(MessagePackRuntimeDictionaryKeyTypeAttribute),
								typeof(MessagePackKnownDictionaryKeyTypeAttribute),
								memberName
							);
					}
					case PolymorphismTarget.TupleItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to #{2} item of tuple type member '{3}' itself.",
								typeof(MessagePackRuntimeTupleItemTypeAttribute),
								typeof(MessagePackKnownTupleItemTypeAttribute),
								tupleItemNumber,
								memberName
							);
					}
					default:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to member '{2}' itself.",
								typeof(MessagePackRuntimeTypeAttribute),
								typeof(MessagePackKnownTypeAttribute),
								memberName
							);
					}
				}
			}

			private static string GetCannotDuplicateKnownTypeCodeErrorMessage(PolymorphismTarget target, string typeCode, string memberName, int tupleItemNumber)
			{
				switch (target)
				{
					case PolymorphismTarget.CollectionItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for collection items of member '{1}'.",
								StringEscape.ForDisplay(typeCode),
								memberName
							);
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for dictionary keys of member '{1}'.",
								StringEscape.ForDisplay(typeCode),
								memberName
							);
					}
					case PolymorphismTarget.TupleItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for #{1} item of tuple type member '{2}'.",
								StringEscape.ForDisplay(typeCode),
								tupleItemNumber,
								memberName
							);
					}
					default:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for member '{1}'.",
								StringEscape.ForDisplay(typeCode),
								memberName
							);
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
					throw new SerializationException(String.Format(CultureInfo.CurrentCulture, "A public static or instance method named '{0}' with single parameter typed PolymorphicTypeVerificationContext in type '{1}'.", attribute.VerifierMethodName, attribute.VerifierMethodName));
				}

				if (method.IsStatic)
				{
					return method.CreateDelegate(typeof(Func<PolymorphicTypeVerificationContext, bool>)) as Func<PolymorphicTypeVerificationContext, bool>;
				}
				else
				{
					return method.CreateDelegate(typeof(Func<PolymorphicTypeVerificationContext, bool>), Activator.CreateInstance(attribute.VerifierType)) as Func<PolymorphicTypeVerificationContext, bool>;
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
		}
	}
}
