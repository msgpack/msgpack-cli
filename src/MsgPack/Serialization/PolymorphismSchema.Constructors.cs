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

using System;
using System.Collections.Generic;
#if !NETFX_35 && !NETFX_40 && !SILVERLIGHT && !UNITY
using System.Collections.ObjectModel;
#endif // !NETFX_35 && !NETFX_40 && !SILVERLIGHT && !UNITY
using System.ComponentModel;
using System.Linq;

namespace MsgPack.Serialization
{
	partial class PolymorphismSchema
	{
		private static readonly Dictionary<byte, Type> EmptyMap = new Dictionary<byte, Type>( 0 );
		private static readonly PolymorphismSchema[] EmptyChildren = new PolymorphismSchema[ 0 ];

		// Internal only, for Default
		private PolymorphismSchema()
		{
			this.TargetType = null;
			this.CodeTypeMapping = new ReadOnlyDictionary<byte, Type>( EmptyMap );
			this.ChildrenType = PolymorphismSchemaChildrenType.None;
			this._children = new ReadOnlyCollection<PolymorphismSchema>( EmptyChildren );
		}

		// Aggregate
		private PolymorphismSchema(
			Type targetType,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList )
			: this( targetType, new ReadOnlyDictionary<byte, Type>( EmptyMap ), childrenType, childItemSchemaList ) { }

		private PolymorphismSchema(
			Type targetType,
			IDictionary<byte, Type> codeTypeMapping,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			this.TargetType = targetType;
			this.CodeTypeMapping = codeTypeMapping;
			this.ChildrenType = childrenType;
			this._children =
				new ReadOnlyCollection<PolymorphismSchema>(
					( childItemSchemaList ?? EmptyChildren ).Select( x => x ?? Default ).ToArray() 
				);
		}

		// Plane

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for type embedding.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType )
			: this( targetType, PolymorphismSchemaChildrenType.None ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for known type mapping.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, IDictionary<byte, Type> codeTypeMapping )
			: this( targetType, codeTypeMapping, PolymorphismSchemaChildrenType.None ) { }

		// Collection items

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for type embedding.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, PolymorphismSchema itemSchema )
			: this( targetType, PolymorphismSchemaChildrenType.CollectionItems, itemSchema ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for known type mapping.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, IDictionary<byte, Type> codeTypeMapping, PolymorphismSchema itemSchema )
			: this( targetType, codeTypeMapping, PolymorphismSchemaChildrenType.CollectionItems, itemSchema ) { }

		// Dictionary key/values

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for type embedding.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, PolymorphismSchema keySchema, PolymorphismSchema valueSchema )
			: this( targetType, PolymorphismSchemaChildrenType.DictionaryKeyValues, keySchema, valueSchema ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for known type mapping.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, IDictionary<byte, Type> codeTypeMapping, PolymorphismSchema keySchema, PolymorphismSchema valueSchema )
			: this( targetType, codeTypeMapping, PolymorphismSchemaChildrenType.DictionaryKeyValues, keySchema, valueSchema ) { }

		// Tuple items

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for type embedding.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchemaList">The schema for collection items of the serialization target tuple. <c>null</c> or empty indicates all items do not have any polymorphism.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		/// <exception cref="System.ArgumentException">A count of <paramref name="itemSchemaList"/> does not match for an arity of the tuple type specified as <paramref name="targetType"/>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, PolymorphismSchema[] itemSchemaList )
			: this( targetType, PolymorphismSchemaChildrenType.TupleItems, itemSchemaList )
		{
			VerifyArity( targetType, itemSchemaList );
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
	}
}