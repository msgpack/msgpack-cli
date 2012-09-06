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
	public sealed class MillisecondsDateTimeOffsetComparer : EqualityComparer<DateTimeOffset>
	{
		public static readonly MillisecondsDateTimeOffsetComparer Instance = new MillisecondsDateTimeOffsetComparer();

		private MillisecondsDateTimeOffsetComparer() { }

		public sealed override bool Equals( DateTimeOffset x, DateTimeOffset y )
		{
			var xms = new DateTimeOffset( new DateTime( x.DateTime.Ticks / 10000, x.DateTime.Kind ), x.Offset );
			var yms = new DateTimeOffset( new DateTime( y.DateTime.Ticks / 10000, y.DateTime.Kind ), y.Offset );
			return xms.Equals( yms );
		}

		public sealed override int GetHashCode( DateTimeOffset obj )
		{
			return new DateTimeOffset( new DateTime( obj.DateTime.Ticks / 10000, obj.DateTime.Kind ), obj.Offset ).GetHashCode();
		}
	}
}
