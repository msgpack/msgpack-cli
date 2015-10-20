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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITYusing System.Globalization;
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
		/// <summary>
		///		Default instance (null object).
		/// </summary>
		internal static readonly PolymorphismSchema Default = new PolymorphismSchema();

		/// <summary>
		///		ForPolymorphicObject( Type targetType )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectTypeEmbeddingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicObject", new[] { typeof( Type ) } );

		/// <summary>
		///		ForPolymorphicObject( Type targetType, IDictionary{byte, Type} codeTypeMapping )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicObjectCodeTypeMappingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicObject", new[] { typeof( Type ), typeof( IDictionary<string, Type> ) } );

		/// <summary>
		///		ForContextSpecifiedCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedCollectionMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForContextSpecifiedCollection", new[] { typeof( Type ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionTypeEmbeddingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicCollection", new[] { typeof( Type ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		ForPolymorphicCollection( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema itemsSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicCollectionCodeTypeMappingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicCollection", new[] { typeof( Type ), typeof( IDictionary<string, Type> ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		ForContextSpecifiedDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForContextSpecifiedDictionaryMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForContextSpecifiedDictionary", new[] { typeof( Type ), typeof( PolymorphismSchema ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryTypeEmbeddingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicDictionary", new[] { typeof( Type ), typeof( PolymorphismSchema ), typeof( PolymorphismSchema ) } );

		/// <summary>
		///		ForPolymorphicDictionary( Type targetType, IDictionary{byte, Type} codeTypeMapping, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicDictionaryCodeTypeMappingMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicDictionary", new[] { typeof( Type ), typeof( IDictionary<string, Type> ), typeof( PolymorphismSchema ), typeof( PolymorphismSchema ) } );

#if !NETFX_35 && !UNITY
		/// <summary>
		///		ForPolymorphicTuple( Type targetType, PolymorphismSchema[] itemSchemaList )
		/// </summary>
		internal static readonly MethodInfo ForPolymorphicTupleMethod =
			typeof( PolymorphismSchema ).GetMethod( "ForPolymorphicTuple", new[] { typeof( Type ), typeof( PolymorphismSchema[]) } );
#endif // !NETFX_35 && !UNITY

		internal static readonly ConstructorInfo CodeTypeMapConstructor =
			typeof( Dictionary<,> ).MakeGenericType( typeof( string ), typeof( Type ) )
				.GetConstructor( new[] { typeof( int ) } );

		internal static readonly MethodInfo AddToCodeTypeMapMethod =
			typeof( Dictionary<,> ).MakeGenericType( typeof( string ), typeof( Type ) )
				.GetMethod( "Add", new[] { typeof( string ), typeof( Type ) } );


		internal string DebugString
		{
			get
			{
				var buffer = new StringBuilder();
				this.ToDebugString( buffer );
				return buffer.ToString();
			}
		}

		private void ToDebugString( StringBuilder buffer )
		{
			buffer.Append( "{TargetType:" ).Append( this.TargetType ).Append( ", SchemaType:" ).Append( this.PolymorphismType );
			switch ( this.ChildrenType )
			{
				case PolymorphismSchemaChildrenType.CollectionItems:
				{
					buffer.Append( ", CollectionItemsSchema:" );
					if ( this.ItemSchema == null )
					{
						buffer.Append( "null" );
					}
					else
					{
						this.ItemSchema.ToDebugString( buffer );
					}

					break;
				}
				case PolymorphismSchemaChildrenType.DictionaryKeyValues:
				{
					buffer.Append( ", DictinoaryKeysSchema:" );
					if ( this.KeySchema == null )
					{
						buffer.Append( "null" );
					}
					else
					{
						this.KeySchema.ToDebugString( buffer );
					}

					buffer.Append( ", DictinoaryValuesSchema:" );
					if ( this.ItemSchema == null )
					{
						buffer.Append( "null" );
					}
					else
					{
						this.ItemSchema.ToDebugString( buffer );
					}

					break;
				}
#if !NETFX_35 && !UNITY
				case PolymorphismSchemaChildrenType.TupleItems:
				{
					buffer.Append( ", TupleItemsSchema:[" );
					var isFirst = true;
					foreach ( var child in this._children )
					{
						if ( isFirst )
						{
							isFirst = false;
						}
						else
						{
							buffer.Append( ", " );
						}

						if ( child == null )
						{
							buffer.Append( "null" );
						}
						else
						{
							child.ToDebugString( buffer );
						}
					}

					break;
				}
#endif // !NETFX_35 && !UNITY
			}

			buffer.Append( '}' );
		}

		internal static PolymorphismSchema Create(
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
				return Default;
			}

			if ( memberMayBeNull == null )
			{
				// Using default for collection/tuple items.
				return Default;
			}

#if !UNITY
			var member = memberMayBeNull.Value;
#else
			var member = memberMayBeNull;
#endif // !UNITY

			var table = TypeTable.Create( member.Member );

			var traits = member.Member.GetMemberValueType().GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					if ( !table.Member.Exists && !table.CollectionItem.Exists )
					{
						return Default;
					}

					return
						new PolymorphismSchema(
							member.Member.GetMemberValueType(),
							table.Member.PolymorphismType,
							table.Member.CodeTypeMapping,
							PolymorphismSchemaChildrenType.CollectionItems,
							new PolymorphismSchema(
								traits.ElementType,
								table.CollectionItem.PolymorphismType,
								table.CollectionItem.CodeTypeMapping,
								PolymorphismSchemaChildrenType.None
							)
						);
				}
				case CollectionKind.Map:
				{
					if ( !table.Member.Exists && !table.DictionaryKey.Exists && !table.CollectionItem.Exists )
					{
						return Default;
					}

					return
						new PolymorphismSchema(
							member.Member.GetMemberValueType(),
							table.Member.PolymorphismType,
							table.Member.CodeTypeMapping,
							PolymorphismSchemaChildrenType.DictionaryKeyValues,
							new PolymorphismSchema(
								traits.ElementType.GetGenericArguments()[ 0 ],
								table.DictionaryKey.PolymorphismType,
								table.DictionaryKey.CodeTypeMapping,
								PolymorphismSchemaChildrenType.None
							),
							new PolymorphismSchema(
								traits.ElementType.GetGenericArguments()[ 1 ],
								table.CollectionItem.PolymorphismType,
								table.CollectionItem.CodeTypeMapping,
								PolymorphismSchemaChildrenType.None
							)
						);
				}
				default:
				{
#if !NETFX_35 && !UNITY
					if ( TupleItems.IsTuple( member.Member.GetMemberValueType() ) )
					{
						if ( table.TupleItems.Count == 0 )
						{
							return Default;
						}

						var tupleItemTypes = TupleItems.GetTupleItemTypes( member.Member.GetMemberValueType() );
						return
							new PolymorphismSchema(
								member.Member.GetMemberValueType(),
								PolymorphismType.None,
								EmptyMap,
								PolymorphismSchemaChildrenType.TupleItems,
								table.TupleItems
								.Zip(tupleItemTypes, (e,t) => new { Entry = e, ItemType = t })
								.Select( e =>
									new PolymorphismSchema(
										e.ItemType,
										e.Entry.PolymorphismType, 
										e.Entry.CodeTypeMapping,
										PolymorphismSchemaChildrenType.None
									)
								).ToArray()
							);
					}
					else
#endif // !NETFX_35 && !UNITY
					{
						if ( !table.Member.Exists )
						{
							return Default;
						}

						return
							new PolymorphismSchema(
								member.Member.GetMemberValueType(),
								table.Member.PolymorphismType,
								table.Member.CodeTypeMapping,
								PolymorphismSchemaChildrenType.None
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
#if !NETFX_35 && !UNITY
			public readonly IList<TypeTableEntry> TupleItems;
#endif // !NETFX_35 && !UNITY

			private TypeTable(
				TypeTableEntry member,
				TypeTableEntry collectionItem,
				TypeTableEntry dictionaryKey
#if !NETFX_35 && !UNITY
				, IList<TypeTableEntry> tupleItems
#endif // !NETFX_35 && !UNITY
			)
			{
				this.Member = member;
				this.CollectionItem = collectionItem;
				this.DictionaryKey = dictionaryKey;
#if !NETFX_35 && !UNITY
				this.TupleItems = tupleItems;
#endif // !NETFX_35 && !UNITY
			}

			public static TypeTable Create( MemberInfo member )
			{
				return
					new TypeTable(
						TypeTableEntry.Create( member, PolymorphismTarget.Member ),
						TypeTableEntry.Create( member, PolymorphismTarget.CollectionItem ),
						TypeTableEntry.Create( member, PolymorphismTarget.DictionaryKey )
#if !NETFX_35 && !UNITY
						, TypeTableEntry.CreateTupleItems( member )
#endif // !NETFX_35 && !UNITY
					);
			}
		}

		private sealed class TypeTableEntry
		{
#if !NETFX_35 && !UNITY
			private static readonly TypeTableEntry[] EmptyEntries = new TypeTableEntry[ 0 ];
#endif // !NETFX_35 && !UNITY

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

			private TypeTableEntry() { }

			public static TypeTableEntry Create( MemberInfo member, PolymorphismTarget targetType )
			{
				var result = new TypeTableEntry();
				foreach (
					var attribute in
						member.GetCustomAttributes( false )
							.OfType<IPolymorphicHelperAttribute>()
							.Where( a => a.Target == targetType )
				)
				{
					result.Interpret( attribute, member.ToString(), -1 );
				}

				return result;
			}

#if !NETFX_35 && !UNITY
			public static TypeTableEntry[] CreateTupleItems( MemberInfo member )
			{
				if ( !TupleItems.IsTuple( member.GetMemberValueType() ) )
				{
					return EmptyEntries;
				}

				var tupleItems = TupleItems.GetTupleItemTypes( member.GetMemberValueType() );
				var result = Enumerable.Repeat( default( object ), tupleItems.Count ).Select( _ => new TypeTableEntry() ).ToArray();
				foreach (
					var attribute in
						member.GetCustomAttributes( false )
							.OfType<IPolymorphicTupleItemTypeAttribute>()
							.OrderBy( a => a.ItemNumber )
				)
				{
					result[ attribute.ItemNumber - 1 ].Interpret( attribute, member.ToString(), attribute.ItemNumber );
				}

				return result;
			}
#endif // !NETFX_35 && !UNITY

			private void Interpret( IPolymorphicHelperAttribute attribute, string memberName, int tupleItemNumber )
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

					var typeCode = asKnown.TypeCode;
					try
					{
						this._knownTypeMapping.Add( typeCode, asKnown.BindingType );
						return;
					}
					catch ( ArgumentException )
					{
						throw new SerializationException(
							GetCannotDuplicateKnownTypeCodeErrorMessage( attribute, typeCode, memberName, tupleItemNumber )
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

			private static string GetCannotSpecifyKnownTypeAndRuntimeTypeErrorMessage( IPolymorphicHelperAttribute attribute, string memberName, int tupleItemNumber )
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

			private static string GetCannotDuplicateKnownTypeCodeErrorMessage( IPolymorphicHelperAttribute attribute, string typeCode, string memberName, int tupleItemNumber )
			{
				switch ( attribute.Target )
				{
					case PolymorphismTarget.CollectionItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for collection items of member '{1}'.",
								StringEscape.ForDisplay( typeCode ),
								memberName
							);
					}
					case PolymorphismTarget.DictionaryKey:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for dictionary keys of member '{1}'.",
								StringEscape.ForDisplay( typeCode ),
								memberName
							);
					}
					case PolymorphismTarget.TupleItem:
					{
						return
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify multiple types for ext-type code '{0}' for #{1} item of tuple type member '{2}'.",
								StringEscape.ForDisplay( typeCode ),
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
								StringEscape.ForDisplay( typeCode ),
								memberName
							);
					}
				}
			}
		}
	}
}