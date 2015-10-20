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

namespace MsgPack
{
#if UNITY && !MSGPACK_UNITY_FULL
	internal sealed class BooleanStack
	{
		private readonly List<Boolean> _list;

		public int Count { get { return this._list.Count; } }

		public BooleanStack( int initialCapacity )
		{
			this._list = new List<Boolean>( initialCapacity );
		}

		public void Push( Boolean value )
		{
			this._list.Add( value );
		}

		public Boolean Peek()
		{
			return this._list[ this._list.Count - 1 ];
		}

		public Boolean Pop()
		{
			var result = this.Peek();
			this._list.RemoveAt( this._list.Count - 1 );
			return result;
		}
	}

	internal sealed class Int64Stack
	{
		private readonly List<Int64> _list;

		public int Count { get { return this._list.Count; } }

		public Int64Stack( int initialCapacity )
		{
			this._list = new List<Int64>( initialCapacity );
		}

		public void Push( Int64 value )
		{
			this._list.Add( value );
		}

		public Int64 Peek()
		{
			return this._list[ this._list.Count - 1 ];
		}

		public Int64 Pop()
		{
			var result = this.Peek();
			this._list.RemoveAt( this._list.Count - 1 );
			return result;
		}
	}

#endif // UNITY && !MSGPACK_UNITY_FULL
}
