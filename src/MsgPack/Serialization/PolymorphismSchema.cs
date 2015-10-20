#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
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
using System.Diagnostics;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<para>
	///			<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		</para>
	///		<para>
	///			A provider parameter to support polymorphism.
	///		</para>
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[DebuggerDisplay("{DebugString}")]
	public sealed partial class PolymorphismSchema
	{
		/// <summary>
		///		Gets the type of the serialization target.
		/// </summary>
		/// <value>
		///		The type of the serialization target. This value can be <c>null</c>.
		/// </value>
		internal Type TargetType { get; private set; }

		/// <summary>
		///		Gets the type of the polymorphism.
		/// </summary>
		/// <value>
		///		The type of the polymorphism.
		/// </value>
		internal PolymorphismType PolymorphismType { get; private set; }

		private readonly ReadOnlyDictionary<string, Type> _codeTypeMapping;

		/// <summary>
		///		Gets the code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </summary>
		/// <value>
		///		The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </value>
		internal IDictionary<string, Type> CodeTypeMapping { get { return this._codeTypeMapping; } }

		internal bool UseDefault { get { return this.PolymorphismType == PolymorphismType.None; } }
		internal bool UseTypeEmbedding { get { return this.PolymorphismType == PolymorphismType.RuntimeType; } }

		internal PolymorphismSchemaChildrenType ChildrenType { get; private set; }

		private readonly ReadOnlyCollection<PolymorphismSchema> _children;

		/// <summary>
		///		Gets the schema for child items of the serialization target collection/tuple.
		/// </summary>
		/// <value>
		///		The schema for child items of the serialization target collection/tuple.
		/// </value>
		internal IList<PolymorphismSchema> ChildSchemaList
		{
			get { return this._children; }
		}

		/// <summary>
		///		Gets the schema for collection items of the serialization target collection.
		/// </summary>
		/// <value>
		///		The schema for collection items of the serialization target collection.
		/// </value>
		internal PolymorphismSchema ItemSchema
		{
			get
			{
				switch ( this.ChildrenType )
				{
					case PolymorphismSchemaChildrenType.None:
					{
						return null;
					}
					case PolymorphismSchemaChildrenType.CollectionItems:
					{
						return this._children.FirstOrDefault();
					}
					case PolymorphismSchemaChildrenType.DictionaryKeyValues:
					{
						return this._children.Skip( 1 ).FirstOrDefault();
					}
					default:
					{
						throw new NotSupportedException();
					}
				}
			}
		}

		/// <summary>
		///		Gets the schema for dictionary keys of the serialization target collection.
		/// </summary>
		/// <value>
		///		The schema for collection items of the serialization target collection.
		/// </value>
		internal PolymorphismSchema KeySchema
		{
			get
			{
				switch ( this.ChildrenType )
				{
					case PolymorphismSchemaChildrenType.None:
					{
						return null;
					}
					case PolymorphismSchemaChildrenType.DictionaryKeyValues:
					{
						return this._children.FirstOrDefault();
					}
					default:
					{
						throw new NotSupportedException();
					}
				}
			}
		}
#if NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
		private sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
		{
			private readonly IDictionary<TKey, TValue> _underlying;

			ICollection<TKey> IDictionary<TKey, TValue>.Keys
			{
				get { return this._underlying.Keys; }
			}

			ICollection<TValue> IDictionary<TKey, TValue>.Values
			{
				get { return this._underlying.Values; }
			}

			TValue IDictionary<TKey, TValue>.this[ TKey key ]
			{
				get { return this._underlying[ key ]; }
				set
				{
					throw new NotSupportedException();
				}
			}

			int ICollection<KeyValuePair<TKey, TValue>>.Count
			{
				get { return this._underlying.Count; }
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
			{
				get { return true; }
			}

			public ReadOnlyDictionary( IDictionary<TKey, TValue> underlying )
			{
				this._underlying = underlying;
			}

			bool IDictionary<TKey, TValue>.ContainsKey( TKey key )
			{
				return this._underlying.ContainsKey( key );
			}

			bool IDictionary<TKey, TValue>.TryGetValue( TKey key, out TValue value )
			{
				return this._underlying.TryGetValue( key, out value );
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.Contains( KeyValuePair<TKey, TValue> item )
			{
				return this._underlying.Contains( item );
			}

			void ICollection<KeyValuePair<TKey, TValue>>.CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
			{
				this._underlying.CopyTo( array, arrayIndex );
			}

			IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
			{
				return this._underlying.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this._underlying.GetEnumerator();
			}

			void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
			{
				throw new NotSupportedException();
			}

			bool IDictionary<TKey, TValue>.Remove( TKey key )
			{
				throw new NotSupportedException();
			}

			void ICollection<KeyValuePair<TKey, TValue>>.Add( KeyValuePair<TKey, TValue> item )
			{
				throw new NotSupportedException();
			}

			void ICollection<KeyValuePair<TKey, TValue>>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.Remove( KeyValuePair<TKey, TValue> item )
			{
				throw new NotSupportedException();
			}
		}
#endif // NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
	}
}