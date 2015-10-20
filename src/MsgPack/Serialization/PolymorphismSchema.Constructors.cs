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
using System.Collections.ObjectModel;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
using System.Linq;

namespace MsgPack.Serialization
{
	partial class PolymorphismSchema
	{
		private static readonly Dictionary<string, Type> EmptyMap = new Dictionary<string, Type>( 0 );
		private static readonly PolymorphismSchema[] EmptyChildren = new PolymorphismSchema[ 0 ];

		// Internal only, for Default
		private PolymorphismSchema()
		{
			this.TargetType = null;
			this.PolymorphismType = PolymorphismType.None;
			this.ChildrenType = PolymorphismSchemaChildrenType.None;
#if !UNITY
			this._codeTypeMapping = new ReadOnlyDictionary<string, Type>( EmptyMap );
			this._children = new ReadOnlyCollection<PolymorphismSchema>( EmptyChildren );
#else
			// Work-around Unity type intiialization issue.
			this._codeTypeMapping = new ReadOnlyDictionary<string, Type>( new Dictionary<string, Type>( 0 ) );
			this._children = new ReadOnlyCollection<PolymorphismSchema>( new PolymorphismSchema[ 0 ] );
#endif
		}

		// Aggregate
		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList )
			: this(
				targetType,
				polymorphismType,
				new ReadOnlyDictionary<string, Type>( EmptyMap ),
				childrenType,
				new ReadOnlyCollection<PolymorphismSchema>(
					( childItemSchemaList ?? EmptyChildren ).Select( x => x ?? Default ).ToArray()
				) 
			) {}

		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			IDictionary<string, Type> codeTypeMapping,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList )
			: this(
				targetType,
				polymorphismType,
				new ReadOnlyDictionary<string, Type>( codeTypeMapping ),
				childrenType,
				new ReadOnlyCollection<PolymorphismSchema>(
					( childItemSchemaList ?? EmptyChildren ).Select( x => x ?? Default ).ToArray()
				)
			) {}

		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			ReadOnlyDictionary<string, Type> codeTypeMapping,
			PolymorphismSchemaChildrenType childrenType,
			ReadOnlyCollection<PolymorphismSchema> childItemSchemaList )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			this.TargetType = targetType;
			this.PolymorphismType = polymorphismType;
			this._codeTypeMapping = codeTypeMapping;
			this.ChildrenType = childrenType;
			this._children = childItemSchemaList;
		}

		// Plane

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicObject( Type targetType )
		{
			return new PolymorphismSchema( targetType, PolymorphismType.RuntimeType, PolymorphismSchemaChildrenType.None );
		}

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code-type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicObject( Type targetType, IDictionary<string, Type> codeTypeMapping )
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.KnownTypes,
					codeTypeMapping,
					PolymorphismSchemaChildrenType.None
				);
		}

		// Collection items

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses declared type or context specified concrete type.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses declared type or context specified concrete type.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForContextSpecifiedCollection( Type targetType, PolymorphismSchema itemSchema )
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.None,
					PolymorphismSchemaChildrenType.CollectionItems,
					itemSchema
				);
		}

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicCollection( Type targetType, PolymorphismSchema itemSchema )
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.RuntimeType,
					PolymorphismSchemaChildrenType.CollectionItems,
					itemSchema
				);
		}

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicCollection(
			Type targetType,
			IDictionary<string, Type> codeTypeMapping,
			PolymorphismSchema itemSchema 
		)
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.KnownTypes,
					codeTypeMapping,
					PolymorphismSchemaChildrenType.CollectionItems,
					itemSchema
				);
		}

		// Dictionary key/values

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses declared type or context specified concrete type.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses declared type or context specified concrete type.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForContextSpecifiedDictionary(
			Type targetType,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema )
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.None,
					PolymorphismSchemaChildrenType.DictionaryKeyValues,
					keySchema,
					valueSchema
				);
		}

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicDictionary(
			Type targetType,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema
		)
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.RuntimeType,
					PolymorphismSchemaChildrenType.DictionaryKeyValues,
					keySchema,
					valueSchema
				);
		}

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static PolymorphismSchema ForPolymorphicDictionary(
			Type targetType,
			IDictionary<string, Type> codeTypeMapping,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema 
		)
		{
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.KnownTypes,
					codeTypeMapping,
					PolymorphismSchemaChildrenType.DictionaryKeyValues,
					keySchema,
					valueSchema
				);
		}

#if !NETFX_35 && !UNITY
		// Tuple items

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for <see cref="Tuple"/> object.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchemaList">The schema for collection items of the serialization target tuple. <c>null</c> or empty indicates all items do not have any polymorphism.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for <see cref="Tuple"/> object.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		/// <exception cref="System.ArgumentException">A count of <paramref name="itemSchemaList"/> does not match for an arity of the tuple type specified as <paramref name="targetType"/>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static PolymorphismSchema ForPolymorphicTuple( Type targetType, PolymorphismSchema[] itemSchemaList )
		{
			VerifyArity( targetType, itemSchemaList );
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.None,
					PolymorphismSchemaChildrenType.TupleItems,
					itemSchemaList
				);
		}

		private static void VerifyArity( Type tupleType, ICollection<PolymorphismSchema> itemSchemaList )
		{
			if ( itemSchemaList == null || itemSchemaList.Count == 0 )
			{
				// OK
				return;
			}

			if ( TupleItems.GetTupleItemTypes( tupleType ).Count != itemSchemaList.Count )
			{
				throw new ArgumentException( "An arity of itemSchemaList does not match for an arity of the tuple.", "itemSchemaList" );
			}
		}
#endif // !NETFX_35 && !UNITY

		internal PolymorphismSchema FilterSelf()
		{
			if ( this == Default )
			{
				return this;
			}

			return new PolymorphismSchema( this.TargetType, PolymorphismType.None, this._codeTypeMapping, this.ChildrenType, this._children );
		}
	}
}