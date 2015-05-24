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

namespace MsgPack.Serialization
{
	public sealed class MillisecondsDateTimeComparer : IEqualityComparer<DateTime>
	{
		public static readonly MillisecondsDateTimeComparer Instance = new MillisecondsDateTimeComparer();

		private MillisecondsDateTimeComparer() { }

		public bool Equals( DateTime x, DateTime y )
		{
			var xms = new DateTime( x.Year, x.Month, x.Day, x.Hour, x.Minute, x.Second, x.Millisecond, x.Kind );
			var yms = new DateTime( y.Year, y.Month, y.Day, y.Hour, y.Minute, y.Second, y.Millisecond, y.Kind );
			return xms.Equals( yms );
		}

		public int GetHashCode( DateTime obj )
		{
			return new DateTime( obj.Year, obj.Month, obj.Day, obj.Hour, obj.Minute, obj.Second, obj.Millisecond, obj.Kind ).GetHashCode();
		}
	}
}
