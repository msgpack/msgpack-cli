#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Defines binary related utilities.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class Binary
	{
		/// <summary>
		///		Singleton empty <see cref="Byte"/>[].
		/// </summary>
		public static readonly byte[] Empty = new byte[ 0 ];

		public static string ToHexString( byte[] blob )
		{
			return ToHexString( blob, true );
		}

		public static string ToHexString( byte[] blob, bool withPrefix )
		{
			if ( blob == null || blob.Length == 0 )
			{
				return String.Empty;
			}

			var buffer = new StringBuilder( blob.Length * 2 + (withPrefix ? 2 : 0 ) );
			ToHexStringCore( blob, buffer, withPrefix );
			return buffer.ToString();
		}

		public static void ToHexString( byte[] blob, StringBuilder buffer )
		{
			if ( blob == null || blob.Length == 0 )
			{
				return;
			}

			ToHexStringCore( blob, buffer, true );
		}

		private static void ToHexStringCore( byte[] blob, StringBuilder buffer, bool withPrefix )
		{
			if ( withPrefix )
			{
				buffer.Append( "0x" );
			}

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

		public static int ToBits( float value )
		{
			var bits = new Float32Bits( value );
			var result = default( int );

			// Float32Bits usage is effectively pointer dereference operation rather than shifting operators, so we must consider endianness here.
			if ( BitConverter.IsLittleEndian )
			{
				result = bits.Byte3 << 24;
				result |= bits.Byte2 << 16;
				result |= bits.Byte1 << 8;
				result |= bits.Byte0;
			}
			else
			{
				result = bits.Byte0 << 24;
				result |= bits.Byte1 << 16;
				result |= bits.Byte2 << 8;
				result |= bits.Byte3;
			}

			return result;
		}

		public static long ToBits( double value )
		{
			return BitConverter.DoubleToInt64Bits( value );
		}
	}
}
