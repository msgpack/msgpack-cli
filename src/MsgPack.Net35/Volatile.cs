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


using System;
using System.Threading;

namespace MsgPack
{
	/// <summary>
	///		Polyfill for System.Threading.Volatile.
	/// </summary>
	internal static class Volatile
	{
		public static int Read( ref int field )
		{
#if SILVERLIGHT
			return Interlocked.CompareExchange( ref field, 0, 0 );
#else
			return Thread.VolatileRead( ref field );
#endif // SILVERLIGHT
		}

		public static void Write( ref int field, int value )
		{
#if SILVERLIGHT
			Interlocked.Exchange( ref field, value );
#else
			Thread.VolatileWrite( ref field, value );
#endif // SILVERLIGHT
		}
	}
}
