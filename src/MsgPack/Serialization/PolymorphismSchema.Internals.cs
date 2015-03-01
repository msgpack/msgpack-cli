#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015-2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion -- License Terms --

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	partial class PolymorphismSchema
	{
		/// <summary>
		///		Default instance (null object).
		/// </summary>
		internal static readonly PolymorphismSchema Default = new PolymorphismSchema();

		/// <summary>
		///		.ctor(Type) for leaf objects with type embedding.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForTypeEmbeddingLeaf =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ) } );

		/// <summary>
		///		.ctor(Type, IDictionary{byte, Type}) for leaf objects with type mapping.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForKnownTypeMappingLeaf =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( IDictionary<byte, Type> ) } );

		/// <summary>
		///		.ctor(Type, PolymorphismSchema) for collections with type embedding.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForTypeEmbeddingCollection =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		.ctor(Type, IDictionary{byte, Type}, PolymorphismSchema) for collections with type mapping.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForKnownTypeMappingCollection =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( IDictionary<byte, Type> ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		.ctor(Type, PolymorphismSchema, PolymorphismSchema) for dictionaries with type embedding.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForTypeEmbeddingDictionary =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( PolymorphismSchema ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		.ctor(Type, IDictionary{byte, Type}, PolymorphismSchema, PolymorphismSchema) for dictionaries with type mapping.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForKnownTypeMappingDictionary =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( IDictionary<byte, Type> ), typeof( PolymorphismSchema ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		.ctor(Type, PolymorphismSchema[]) for tuple.
		/// </summary>
		internal static readonly ConstructorInfo ConstructorForTuple =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( PolymorphismSchema[] ) } );

		internal static readonly ConstructorInfo CodeTypeMapConstructor =
			typeof( Dictionary<,> ).MakeGenericType( typeof( byte ), typeof( Type ) )
				.GetConstructor( ReflectionAbstractions.EmptyTypes );

		internal static readonly MethodInfo AddToCodeTypeMapMethod =
			typeof( Dictionary<,> ).MakeGenericType( typeof( byte ), typeof( Type ) )
				.GetMethod( "Add", new[] { typeof( byte ), typeof( Type ) } );


		internal static PolymorphismSchema Create(
			SerializationContext context,
			Type type,
#if !UNITY
 SerializingMember? memberMayBeNull
#else
			SerializingMember memberMayBeNull
#endif // !UNITY
 )
		{
			if ( type.GetIsValueType() )
			{
				// Value types will never be polymorphic.
				return null;
			}

			if ( memberMayBeNull == null )
			{
				// Using default for collection/tuple items.
				return null;
			}

#if !UNITY
			var member = memberMayBeNull.Value;
#else
			var member = memberMayBeNull;
#endif // !UNITY

			var table = TypeTable.Create( context, member.Member );

			var traits = member.Member.GetMemberValueType().GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					return
						new PolymorphismSchema(
							member.Member.GetMemberValueType(),
							table.Member.CodeTypeMapping,
							PolymorphismSchemaChildrenType.CollectionItems,
							new PolymorphismSchema(
								traits.ElementType,
								table.CollectionItem.CodeTypeMapping
							)
						);
				}
				case CollectionKind.Map:
				{
					return 
						new PolymorphismSchema(
							member.Member.GetMemberValueType(),
							table.Member.CodeTypeMapping,
							PolymorphismSchemaChildrenType.DictionaryKeyValues,
							new PolymorphismSchema(
								traits.ElementType.GetGenericArguments()[ 0 ],
								table.DictionaryKey.CodeTypeMapping
							),
							new PolymorphismSchema(
								traits.ElementType.GetGenericArguments()[ 1 ],
								table.CollectionItem.CodeTypeMapping
							)
						);
				}
				default:
				{
#if !WINDOWS_PHONE && !NETFX_35 && !UNITY
					if ( TupleItems.IsTuple( member.Member.GetMemberValueType() ) )
					{
						var tupleItemTypes = TupleItems.GetTupleItemTypes( member.Member.GetMemberValueType() );
						return
							new PolymorphismSchema(
								member.Member.GetMemberValueType(),
								table.Member.CodeTypeMapping,
								PolymorphismSchemaChildrenType.TupleItems,
								table.TupleItems
								.Zip(tupleItemTypes, (e,t) => new { Entry = e, ItemType = t })
								.Select( e =>
									new PolymorphismSchema(
										e.ItemType,
										e.Entry.CodeTypeMapping
									)
								).ToArray()
							);
					}
					else
#endif // !WINDOWS_PHONE && !NETFX_35 && !UNITY
					{
						return
							new PolymorphismSchema(
								member.Member.GetMemberValueType(),
								table.Member.CodeTypeMapping
							);
					}
				}
			}
		}

		private struct TypeTable
		{
			public readonly TypeTableEntry Member;
			public readonly TypeTableEntry CollectionItem;
			public readonly TypeTableEntry DictionaryKey;
			public readonly IList<TypeTableEntry> TupleItems;

			private TypeTable(
				TypeTableEntry member,
				TypeTableEntry collectionItem,
				TypeTableEntry dictionaryKey,
				IList<TypeTableEntry> tupleItems
			)
			{
				this.Member = member;
				this.CollectionItem = collectionItem;
				this.DictionaryKey = dictionaryKey;
				this.TupleItems = tupleItems;
			}

			public static TypeTable Create( SerializationContext context, MemberInfo member )
			{
				return
					new TypeTable(
						TypeTableEntry.Create( context, member, PolymorphismTarget.Member ),
						TypeTableEntry.Create( context, member, PolymorphismTarget.CollectionItem ),
						TypeTableEntry.Create( context, member, PolymorphismTarget.DictionaryKey ),
						TypeTableEntry.CreateTupleItems( context, member )
					);
			}
		}

		private sealed class TypeTableEntry
		{
			private readonly Dictionary<byte, Type> _knownTypeMapping = new Dictionary<byte, Type>();

			public IDictionary<byte, Type> CodeTypeMapping { get { return this._knownTypeMapping; } }

			private bool _useTypeEmbedding;

			private TypeTableEntry() { }

			public static TypeTableEntry Create( SerializationContext context, MemberInfo member, PolymorphismTarget targetType )
			{
				var result = new TypeTableEntry();
				foreach (
					var attribute in
						member.GetCustomAttributes<Attribute>()
							.OfType<IPolymorhicHelperAttribute>()
							.Where( a => a.Target == targetType )
				)
				{
					result.Interpret( context, attribute, member.ToString(), -1 );
				}

				return result;
			}

			public static TypeTableEntry[] CreateTupleItems( SerializationContext context, MemberInfo member )
			{
				var tupleItems = TupleItems.GetTupleItemTypes( member.GetMemberValueType() );
				var result = Enumerable.Repeat( default( object ), tupleItems.Count ).Select( _ => new TypeTableEntry() ).ToArray();
				foreach (
					var attribute in
						member.GetCustomAttributes<Attribute>()
							.OfType<IPolymorphicTupleItemTypeAttribute>()
							.OrderBy( a => a.ItemNumber )
				)
				{
					result[ attribute.ItemNumber - 1 ].Interpret( context, attribute, member.ToString(), attribute.ItemNumber );
				}

				return result;
			}

			private void Interpret( SerializationContext context, IPolymorhicHelperAttribute attribute, string memberName, int tupleItemNumber )
			{
				var asKnown = attribute as IPolymorphicKnownTypeAttribute;
				if ( asKnown != null )
				{
					if ( this._useTypeEmbedding )
					{
						throw new SerializationException(
							GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage( attribute, memberName, tupleItemNumber )
						);
					}

					var bindingCode = asKnown.GetBindingCode( context );
					try
					{
						this._knownTypeMapping.Add( bindingCode, asKnown.BindingType );
						return;
					}
					catch ( ArgumentException )
					{
						throw new SerializationException(
							GetCannotDuplicateKnownTypeCodeErrorMessage( attribute, bindingCode, memberName, tupleItemNumber )
						);
					}
				}

#if DEBUG && !UNITY
				Contract.Assert( attribute is IPolymorphicRuntimeTypeAttribute, attribute + " is IPolymorphicRuntimeTypeAttribute" );
#endif // DEBUG && !UNITY
				if ( this._useTypeEmbedding )
				{
#if DEBUG && !UNITY
					Contract.Assert( attribute.Target == PolymorphismTarget.TupleItem, attribute.Target + " == PolymorphismTarget.TupleItem" );
#endif // DEBUG && !UNITY
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Cannot specify multiple '{0}' to #{1} item of tuple type member '{2}'.",
							typeof( MessagePackRuntimeTupleItemTypeAttribute ),
							tupleItemNumber,
							memberName
						)
					);
				}

				if ( this._knownTypeMapping.Count > 0 )
				{
					throw new SerializationException(
						GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage( attribute, memberName, tupleItemNumber )
					);
				}

				this._useTypeEmbedding = true;
			}

			private static string GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage( IPolymorhicHelperAttribute attribute, string memberName, int tupleItemNumber )
			{
				switch ( attribute.Target )
				{
					case PolymorphismTarget.CollectionItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to collection items of member '{2}' itself.",
								typeof( MessagePackRuntimeCollectionItemTypeAttribute ),
								typeof( MessagePackKnownCollectionItemTypeAttribute ),
								memberName
							);
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to dictionary keys of member '{2}' itself.",
								typeof( MessagePackRuntimeDictionaryKeyTypeAttribute ),
								typeof( MessagePackKnownDictionaryKeyTypeAttribute ),
								memberName
							);
					}
					case PolymorphismTarget.TupleItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to #{2} item of tuple type member '{3}' itself.",
								typeof( MessagePackRuntimeTupleItemTypeAttribute ),
								typeof( MessagePackKnownTupleItemTypeAttribute ),
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
								typeof( MessagePackRuntimeTypeAttribute ),
								typeof( MessagePackKnownTypeAttribute ),
								memberName
							);
					}
				}
			}

			private static string GetCannotDuplicateKnownTypeCodeErrorMessage( IPolymorhicHelperAttribute attribute, byte bindingCode, string memberName, int tupleItemNumber )
			{
				switch ( attribute.Target )
				{
					case PolymorphismTarget.CollectionItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for collection items of member '{1}'.",
								bindingCode,
								memberName
							);
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for dictionary keys of member '{1}'.",
								bindingCode,
								memberName
							);
					}
					case PolymorphismTarget.TupleItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for #{1} item of tuple type member '{2}'.",
								bindingCode,
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
								bindingCode,
								memberName
							);
					}
				}
			}
		}

	}
}