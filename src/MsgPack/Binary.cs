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
using System.Diagnostics.Contracts;
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Defines binary related utilities.
	/// </summary>
	internal static class Binary
	{
		/// <summary>
		///		Singleton empty <see cref="Byte"/>[].
		/// </summary>
		public static readonly byte[] Empty = new byte[ 0 ];

		public static string ToHexString( byte[] blob )
		{
			if ( blob == null || blob.Length == 0 )
			{
				return String.Empty;
			}

			var buffer = new StringBuilder( blob.Length * 2 + 2 );
			ToHexStringCore( blob, buffer );
			return buffer.ToString();
		}

		public static void ToHexString( byte[] blob, StringBuilder buffer )
		{
			if ( blob == null || blob.Length == 0 )
			{
				return;
			}

			ToHexStringCore( blob, buffer );
		}

		private static void ToHexStringCore( byte[] blob, StringBuilder buffer )
		{
			buffer.Append( "0x" );
			foreach ( var b in blob )
			{
				buffer.Append( ToHexChar( b >> 4 ) );
				buffer.Append( ToHexChar( b & 0xF ) );
			}
		}

		private static char ToHexChar( int b )
		{
			if ( b < 10 )
			{
				return unchecked( ( char )( '0' + b ) );
			}
			else
			{
				return unchecked( ( char )( 'A' + ( b - 10 ) ) );
			}
		}
	}
}
