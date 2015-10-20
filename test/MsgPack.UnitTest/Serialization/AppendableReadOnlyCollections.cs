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

namespace MsgPack.Serialization
{
	public class AppendableReadOnlyCollection<T> : IReadOnlyCollection<T>
	{
		private readonly List<T> _underlying;

		public int Count
		{
			get { return this._underlying.Count; }
		}

		public AppendableReadOnlyCollection() : this( 0 ) { }

		public AppendableReadOnlyCollection( int initialCapacity )
		{
			this._underlying = new List<T>( initialCapacity );
		}

		public void Add( T item )
		{
			this._underlying.Add( item );
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	public class AppendableReadOnlyList<T> : IReadOnlyList<T>
	{
		private readonly List<T> _underlying;

		public T this[ int index ]
		{
			get { return this._underlying[ index ]; }
		}

		public int Count
		{
			get { return this._underlying.Count; }
		}

		public AppendableReadOnlyList() : this( 0 ) { }

		public AppendableReadOnlyList( int initialCapacity )
		{
			this._underlying = new List<T>( initialCapacity );
		}

		public void Add( T item )
		{
			this._underlying.Add( item );
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	public class AppendableReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> _underlying;

		public IEnumerable<TKey> Keys
		{
			get { return this._underlying.Keys; }
		}

		public IEnumerable<TValue> Values
		{
			get { return this._underlying.Values; }
		}

		public TValue this[ TKey key ]
		{
			get { return this._underlying[ key ]; }
		}

		public int Count
		{
			get { return this._underlying.Count; }
		}

		public AppendableReadOnlyDictionary() : this( 0 ) { }

		public AppendableReadOnlyDictionary( int initialCapacity )
		{
			this._underlying = new Dictionary<TKey, TValue>( initialCapacity );
		}

		public void Add( TKey key, TValue value )
		{
			this._underlying.Add( key, value );
		}

		public bool ContainsKey( TKey key )
		{
			return this._underlying.ContainsKey( key );
		}

		public bool TryGetValue( TKey key, out TValue value )
		{
			return this._underlying.TryGetValue( key, out value );
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}


}
