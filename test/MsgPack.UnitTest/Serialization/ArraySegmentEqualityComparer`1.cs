#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.Linq;

namespace MsgPack.Serialization
{
	public sealed class ArraySegmentEqualityComparer<T> : IEqualityComparer<ArraySegment<T>>
	{
		public ArraySegmentEqualityComparer() { }

		public bool Equals( ArraySegment<T> x, ArraySegment<T> y )
		{
			if ( x.Count == 0 )
			{
				return y.Count == 0;
			}

			return x.Array.Skip( x.Offset ).Take( x.Count ).SequenceEqual( y.Array.Skip( y.Offset ).Take( y.Count ), DictionaryTestHelper.GetEqualityComparer<T>() );
		}

		public int GetHashCode( ArraySegment<T> obj )
		{
			return obj.GetHashCode();
		}
	}
}
