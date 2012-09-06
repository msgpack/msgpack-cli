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
using System.Diagnostics;

namespace MsgPack
{
	// Thanks to Mono implementation.

	/// <summary>
	///		Debugger type proxy for <see cref="IDictionary{TKey,TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The key type of the dictionary.</typeparam>
	/// <typeparam name="TValue">The value type of the dictionary.</typeparam>
	internal sealed class DictionaryDebuggerProxy<TKey,TValue>
	{
		private readonly IDictionary<TKey,TValue> _dictionary;

		public DictionaryDebuggerProxy( IDictionary<TKey, TValue> target )
		{
			this._dictionary = target;
		}

		[DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
		public KeyValuePair<TKey,TValue>[] Items
		{
			get
			{
				if ( this._dictionary == null )
				{
					return null;
				}

				var result = new KeyValuePair<TKey, TValue>[ this._dictionary.Count ];
				this._dictionary.CopyTo( result, 0 );
				return result;
			}
		}
	}
}
