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
using System.Collections;

namespace System.Collections.Generic
{
	/// <summary>
	///		Compatibility dummy object.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	internal sealed class HashSet<T> : IEnumerable<T>
	{
		private readonly Dictionary<T, object> _dictionary;

		public HashSet()
		{
			this._dictionary = new Dictionary<T, object>();
		}

		public bool Add( T item )
		{
			if ( this._dictionary.ContainsKey( item ) )
			{
				return false;
			}
			else
			{
				try
				{
					this._dictionary.Add( item, null );
					return true;
				}
				catch ( ArgumentException )
				{
					return false;
				}
			}
		}

		public bool Remove( T item )
		{
			return this._dictionary.Remove( item );
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this._dictionary.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
