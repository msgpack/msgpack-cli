#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	public class SimpleCollection<T> : ICollection<T>
	{
		private readonly List<T> _underlying = new List<T>();
		
		public int Count
		{
			get { return this._underlying.Count; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		public SimpleCollection() { }

		public void Add( T item )
		{
			this._underlying.Add( item );
		}

		public void Clear()
		{
			this._underlying.Clear();
		}

		public bool Contains( T item )
		{
			return this._underlying.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex )
		{
			this._underlying.CopyTo( array, arrayIndex );
		}

		public bool Remove( T item )
		{
			return this._underlying.Remove( item );
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
}
