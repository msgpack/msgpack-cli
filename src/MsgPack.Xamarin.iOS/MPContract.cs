#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion

using System;
using System.Diagnostics;

namespace MsgPack
{
	/// <summary>
	///		System.Contract alternative working on Xamarin.
	/// </summary>
	internal static class MPContract
	{
		[Conditional( "DEBUG" )]
		public static void Assert( bool condition, string userMessage )
		{
			if ( !condition )
			{
				throw new Exception( "Assertion error: " + userMessage );
			}
		}

		[Conditional("__NEVER")]
		public static void EndContractBlock()
		{
			// nop
		}

		[Conditional( "__NEVER" )]
		public static void Requires( bool expression )
		{
			// nop
		}

		[Conditional( "__NEVER" )]
		public static void Ensures( bool expression )
		{
			// nop
		}

		public static T Result<T>()
		{
			return default( T );
		}
	}
}