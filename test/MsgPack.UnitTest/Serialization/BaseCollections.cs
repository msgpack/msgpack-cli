#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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

#if FEATURE_TAP
#define FEATURE_READONLY_COLLECTIONS
#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	// CollectionTypes for complex serialization tests.

	public abstract class EnumerableBase<T> : IEnumerable<T>
	{
		private readonly List<T> _underlying;

		protected List<T> Underlying
		{
			get { return this._underlying; }
		}

		protected EnumerableBase()
		{
			this._underlying = new List<T>();
		}

		public virtual void Initialize( params T[] initialValues )
		{
			this._underlying.AddRange( initialValues );
		}

		public virtual T[] GetValues()
		{
			return this._underlying.ToArray();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

#if FEATURE_READONLY_COLLECTIONS
	public abstract partial class ReadOnlyCollectionBase<T> : EnumerableBase<T> , IReadOnlyCollection<T>
	{
		protected ReadOnlyCollectionBase() { }
	}

	partial class ReadOnlyCollectionBase<T>
#else
	partial class CollectionBase<T>
#endif // FEATURE_READONLY_COLLECTIONS
	{
		public int Count { get { return this.Underlying.Count; } }
	}

	// ReSharper disable once PartialTypeWithSinglePart
	public abstract partial class CollectionBase<T> :
#if FEATURE_READONLY_COLLECTIONS
		ReadOnlyCollectionBase<T>, 
#else
		EnumerableBase<T>,
#endif // FEATURE_READONLY_COLLECTIONS
		ICollection<T>, ICollection
	{
		private ICollection AsNonGeneric { get { return this.Underlying; } }
		
		bool ICollection<T>.IsReadOnly { get { return false; } }

		object ICollection.SyncRoot { get { return this; } }

		bool ICollection.IsSynchronized { get { return false; } }

		protected CollectionBase() { }

		public void Add( T item )
		{
			this.Underlying.Add( item );
		}

		public void Clear()
		{
			this.Underlying.Clear();
		}

		public bool Contains( T item )
		{
			return this.Underlying.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex )
		{
			this.Underlying.CopyTo( array, arrayIndex );
		}

		public bool Remove( T item )
		{
			return this.Underlying.Remove( item );
		}

		void ICollection.CopyTo( Array array, int index )
		{
			this.AsNonGeneric.CopyTo( array, index );
		}
	}

#if FEATURE_READONLY_COLLECTIONS
	public abstract partial class ReadOnlyListBase<T> : ReadOnlyCollectionBase<T>, IReadOnlyList<T>
	{
		protected ReadOnlyListBase() { }

		public T this[ int index ]
		{
			get { return this.Underlying[ index ]; }
		}
	}
#endif // FEATURE_READONLY_COLLECTIONS

	// ReSharper disable once PartialTypeWithSinglePart
	public abstract partial class ListBase<T> :
		CollectionBase<T>,
		IList<T>,
#if FEATURE_READONLY_COLLECTIONS
		IReadOnlyList<T>,
#endif // FEATURE_READONLY_COLLECTIONS
		IList
	{
		private IList AsNonGeneric { get { return this.Underlying; } }

		public T this[ int index ]
		{
			get { return this.Underlying[ index ]; }
			set { this.Underlying[ index ] = value; }
		}

		object IList.this[ int index ]
		{
			get { return this.AsNonGeneric[ index ]; }
			set { this.AsNonGeneric[ index ] = value; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		protected ListBase() { }

		public int IndexOf( T item )
		{
			return this.Underlying.IndexOf( item );
		}

		public void Insert( int index, T item )
		{
			this.Underlying.Insert( index, item );
		}

		public void RemoveAt( int index )
		{
			this.Underlying.RemoveAt( index );
		}

		int IList.Add( object value )
		{
			return this.AsNonGeneric.Add( value );
		}

		bool IList.Contains( object value )
		{
			return this.AsNonGeneric.Contains( value );
		}

		int IList.IndexOf( object value )
		{
			return this.AsNonGeneric.IndexOf( value );
		}

		void IList.Insert( int index, object value )
		{
			this.AsNonGeneric.Insert( index, value );
		}

		void IList.Remove( object value )
		{
			this.AsNonGeneric.Remove( value );
		}
	}

#if FEATURE_READONLY_COLLECTIONS
	public abstract partial class ReadOnlyDictionaryBase<T> : IReadOnlyDictionary<T, T>
	{
		public IEnumerable<T> Keys
		{
			get { return this._underlying.Keys; }
		}

		public IEnumerable<T> Values
		{
			get { return this._underlying.Values; }
		}

		public T this[ T key ]
		{
			get { return this._underlying[ key ]; }
		}

		protected ReadOnlyDictionaryBase() {}
	}

	partial class ReadOnlyDictionaryBase<T>
#else
	partial class DictionaryBase<T>
#endif // FEATURE_READONLY_COLLECTIONS
	{
		private readonly Dictionary<T, T> _underlying = new Dictionary<T, T>();
		protected Dictionary<T, T> Underlying { get { return this._underlying; } }

		public int Count { get { return this._underlying.Count; } }

		public bool ContainsKey( T key )
		{
			return this._underlying.ContainsKey( key );
		}

		public bool TryGetValue( T key, out T value )
		{
			return this._underlying.TryGetValue( key, out value );
		}

		public IEnumerator<KeyValuePair<T, T>> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	// ReSharper disable once PartialTypeWithSinglePart
	public abstract partial class DictionaryBase<T> :
#if FEATURE_READONLY_COLLECTIONS
		ReadOnlyDictionaryBase<T>,
#endif // FEATURE_READONLY_COLLECTIONS
		IDictionary<T, T>, IDictionary
	{
		private ICollection<KeyValuePair<T, T>> AsCollection { get { return this.Underlying; } }
		private IDictionary AsNonGeneric { get { return this.Underlying; } }

#pragma warning disable 109
		public new T this[ T key ]
		{
			get { return this.Underlying[ key ]; }
			set { this.Underlying[ key ] = value; }
		}

		public new ICollection<T> Keys { get { return this.Underlying.Keys; } }

		public new ICollection<T> Values { get { return this.Underlying.Values; } }
#pragma warning restore 109

		ICollection IDictionary.Keys { get { return this.AsNonGeneric.Keys; } }

		ICollection IDictionary.Values { get { return this.AsNonGeneric.Values; } }

		bool IDictionary.IsReadOnly { get { return false; } }

		bool IDictionary.IsFixedSize { get { return false; } }

		bool ICollection<KeyValuePair<T, T>>.IsReadOnly { get { return false; } }

		object IDictionary.this[ object key ]
		{
			get { return this.AsNonGeneric[ key ]; }
			set { this.AsNonGeneric[ key ] = value; }
		}

		object ICollection.SyncRoot { get { return this; } }

		bool ICollection.IsSynchronized { get { return false; } }

		protected DictionaryBase() { }

		public void Clear()
		{
			this.Underlying.Clear();
		}

		public void Add( T key, T value )
		{
			this.Underlying.Add( key, value );
		}

		public bool Remove( T key )
		{
			return this.Underlying.Remove( key );
		}

		bool IDictionary.Contains( object key )
		{
			return this.AsNonGeneric.Contains( key );
		}

		void IDictionary.Add( object key, object value )
		{
			this.AsNonGeneric.Add( key, value );
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this.AsNonGeneric.GetEnumerator();
		}

		void IDictionary.Remove( object key )
		{
			this.AsNonGeneric.Remove( key );
		}

		void ICollection<KeyValuePair<T, T>>.Add( KeyValuePair<T, T> item )
		{
			this.AsCollection.Add( item );
		}
		
		bool ICollection<KeyValuePair<T, T>>.Contains( KeyValuePair<T, T> item )
		{
			return this.AsCollection.Contains( item );
		}

		void ICollection<KeyValuePair<T, T>>.CopyTo( KeyValuePair<T, T>[] array, int arrayIndex )
		{
			this.AsCollection.CopyTo( array, arrayIndex );
		}

		bool ICollection<KeyValuePair<T, T>>.Remove( KeyValuePair<T, T> item )
		{
			return this.AsCollection.Remove( item );
		}

		void ICollection.CopyTo( Array array, int index )
		{
			this.AsNonGeneric.CopyTo( array, index );
		}
	}

	public abstract class NonGenericEnumerableBase : IEnumerable
	{
		private readonly IList _underlying;

		protected IList Underlying
		{
			get { return this._underlying; }
		}

		protected NonGenericEnumerableBase()
		{
			this._underlying = new List<object>();
		}

		public IEnumerator GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}
	}

	public abstract class NonGenericCollectionBase : NonGenericEnumerableBase, ICollection
	{
		public int Count
		{
			get { return this.Underlying.Count; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false ; }
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}

		protected NonGenericCollectionBase() { }


		public void CopyTo( Array array, int index )
		{
			this.Underlying.CopyTo( array, index );
		}
	}

	public abstract class NonGenericListBase : NonGenericCollectionBase, IList
	{
		public object this[ int index ]
		{
			get { return this.Underlying[ index ]; }
			set { this.Underlying[ index ] = value; }
		}

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		protected NonGenericListBase() { }

		public int Add( object value )
		{
			return this.Underlying.Add( value );
		}

		public void Clear()
		{
			this.Underlying.Clear();
		}

		public bool Contains( object value )
		{
			return this.Underlying.Contains( value );
		}

		public int IndexOf( object value )
		{
			return this.Underlying.IndexOf( value );
		}

		public void Insert( int index, object value )
		{
			this.Underlying.Insert( index, value );
		}

		public void Remove( object value )
		{
			this.Underlying.Remove( value );
		}

		public void RemoveAt( int index )
		{
			this.Underlying.RemoveAt( index );
		}
	}

	public abstract class NonGenericDictionaryBase : IDictionary
	{
		private readonly IDictionary _underlying;

		protected IDictionary Underlying { get { return this._underlying; } }

		public int Count
		{
			get { return this._underlying.Count; }
		}

		public object this[ object key ]
		{
			get { return this._underlying[ key ]; }
			set { this._underlying[ key ] = value; }
		}

		public ICollection Keys
		{
			get { return this._underlying.Keys; }
		}

		public ICollection Values
		{
			get { return this._underlying.Values; }
		}
		
		bool IDictionary.IsFixedSize
		{
			get { return false; }
		}

		bool IDictionary.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}

		protected NonGenericDictionaryBase()
		{
			this._underlying = new Dictionary<object, object>();
		}

		public void Add( object key, object value )
		{
			this._underlying.Add( key, value );
		}

		public void Clear()
		{
			this._underlying.Clear();
		}

		public bool Contains( object key )
		{
			return this._underlying.Contains( key );
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		public void Remove( object key )
		{
			this._underlying.Remove( key );
		}

		public void CopyTo( Array array, int index )
		{
			this._underlying.CopyTo( array, index );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
