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

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MsgPack.Serialization;

using NUnitLite.Runner;

namespace MsgPack.NUnitLiteRunner
{
	/// <summary>
	///		Lightweight test runner for NUnit Lite with <c>mono --full-aot</c>.
	/// </summary>
	internal class Program
	{
		private static readonly string AssemblyName = typeof( PackUnpackTest ).Assembly.FullName;

		private static void Main( string[] args )
		{
			var adjustedArgs = new List<string>( args.Length + 1 );
			adjustedArgs.AddRange( args );
			adjustedArgs.Add( AssemblyName );
			PreHeat();
			new TextUI().Execute( adjustedArgs.ToArray() );
		}

		[MethodImpl( MethodImplOptions.NoOptimization )]
		private static void PreHeat()
		{
			new ArraySegmentEqualityComparer<byte>().Equals( default( ArraySegment<byte> ), default( ArraySegment<byte> ) );
			new ArraySegmentEqualityComparer<int>().Equals( default( ArraySegment<int> ), default( ArraySegment<int> ) );
			new ArraySegmentEqualityComparer<decimal>().Equals( default( ArraySegment<decimal> ), default( ArraySegment<decimal> ) );

			new SerializationContext().GetSerializer<KeyValuePair<string, int>>( null );

			// DateTimeOffset is not able to be instanciated via reflection, it might be because it consist of other non-primitive value types (DateTime and TimeSpan).
		}
	}
}
