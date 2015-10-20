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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

namespace MsgPack
{
	/// <summary>
	///		Define bit operations which enforce big endian.
	/// </summary>
	internal static class BigEndianBinary
	{
		public static sbyte ToSByte( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( sbyte ), buffer.Length + ">=" + offset + " + " + sizeof( sbyte ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return ( sbyte )buffer[ offset ];
			}
		}

		public static short ToInt16( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( short ), buffer.Length + ">=" + offset + " + " + sizeof( short ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return ( short )( buffer[ offset ] << 8 | buffer[ 1 + offset ] );
			}
		}

		public static int ToInt32( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( int ), buffer.Length + ">=" + offset + " + " + sizeof( int ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return buffer[ offset ] << 24 | buffer[ 1 + offset ] << 16 | buffer[ 2 + offset ] << 8 | buffer[ 3 + offset ];
			}
		}

		public static long ToInt64( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( long ), buffer.Length + ">=" + offset + " + " + sizeof( long ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return
					( long )buffer[ offset ] << 56 | ( long )buffer[ 1 + offset ] << 48 | ( long )buffer[ 2 + offset ] << 40 | ( long )buffer[ 3 + offset ] << 32
					| ( long )buffer[ 4 + offset ] << 24 | ( long )buffer[ 5 + offset ] << 16 | ( long )buffer[ 6 + offset ] << 8 | buffer[ 7 + offset ];
			}
		}

		public static byte ToByte( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( byte ), buffer.Length + ">=" + offset + " + " + sizeof( byte ) );
#endif // DEBUG && !UNITY

			return buffer[ offset ];
		}

		public static ushort ToUInt16( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( ushort ), buffer.Length + ">=" + offset + " + " + sizeof( ushort ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return ( ushort )( ( buffer[ offset ] << 8 ) | buffer[ 1 + offset ] );
			}
		}

		public static uint ToUInt32( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( uint ), buffer.Length + ">=" + offset + " + " + sizeof( uint ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return ( uint )( ( buffer[ offset ] << 24 ) | buffer[ 1 + offset ] << 16 | buffer[ 2 + offset ] << 8 | buffer[ 3 + offset ] );
			}
		}

		public static ulong ToUInt64( byte[] buffer, int offset )
		{
#if DEBUG && !UNITY
			Contract.Assert( buffer.Length >= offset + sizeof( ulong ), buffer.Length + ">=" + offset + " + " + sizeof( ulong ) );
#endif // DEBUG && !UNITY

			unchecked
			{
				return
					( ulong )( ( long )buffer[ offset ] << 56 | ( long )buffer[ 1 + offset ] << 48 | ( long )buffer[ 2 + offset ] << 40 | ( long )buffer[ 3 + offset ] << 32
					| ( long )buffer[ 4 + offset ] << 24 | ( long )buffer[ 5 + offset ] << 16 | ( long )buffer[ 6 + offset ] << 8 | buffer[ 7 + offset ] );
			}
		}

		public static float ToSingle( byte[] buffer, int offset )
		{
			return new Float32Bits( buffer, offset ).Value;
		}

		public static double ToDouble( byte[] buffer, int offset )
		{
			return new Float64Bits( buffer, offset ).Value;
		}
	}
}
