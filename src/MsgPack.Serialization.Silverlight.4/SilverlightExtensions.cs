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
using System.Linq;

namespace MsgPack
{
#if SILVERLIGHT
	internal static class SilverlightExtensions
	{
		public static int FindIndex<T>( this IList<T> source, Predicate<T> predicate )
		{
			for ( int i = 0; i < source.Count; i++ )
			{
				if ( predicate( source[ i ] ) )
				{
					return i;
				}
			}

			return -1;
		}

		public static IEnumerable<Type> FindInterfaces( this Type source, Func<Type,object,bool> filter, object filterCriteria )
		{
			return source.GetInterfaces().Where( type => filter( type, filterCriteria ) );
		}
	}
#endif
}
