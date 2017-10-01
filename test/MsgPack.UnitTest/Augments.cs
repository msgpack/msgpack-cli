#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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

namespace MsgPack
{
	internal static class Augments
	{
		public static T[] ToArray<T>( this ArraySegment<T> source )
		{
			var result = new T[ source.Count ];
			if ( result.Length > 0 )
			{
				Array.Copy( source.Array, source.Offset, result, 0, source.Count );
			}

			return result;
		}

		public static long ToUnixTimeSeconds( this DateTimeOffset source )
		{
			return source.UtcDateTime.Ticks / TimeSpan.TicksPerSecond - 62135596800;
		}
	}
}
