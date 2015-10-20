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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY
using System.Runtime.InteropServices.ComTypes;
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
	internal static class InternalDateTimeExtensions
	{
#if SILVERLIGHT
	// Porting of reference sources ToBinary().
	// See https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/datetime.cs
		private const UInt64 LocalMask = 0x8000000000000000;
		private const Int64 TicksCeiling = 0x4000000000000000;
		private const Int32 KindShift = 62;

		public static Int64 ToBinary( this DateTime source )
		{
			if ( source.Kind == DateTimeKind.Local )
			{
				// Local times need to be adjusted as you move from one time zone to another, 
				// just as they are when serializing in text. As such the format for local times
				// changes to store the ticks of the UTC time, but with flags that look like a 
				// local date.

				// To match serialization in text we need to be able to handle cases where
				// the UTC value would be out of range. Unused parts of the ticks range are
				// used for this, so that values just past max value are stored just past the
				// end of the maximum range, and values just below minimum value are stored
				// at the end of the ticks area, just below 2^62.
				var offset = TimeZoneInfo.Local.GetUtcOffset( source );
				var ticks = source.Ticks;
				var storedTicks = ticks - offset.Ticks;
				if ( storedTicks < 0 )
				{
					storedTicks = TicksCeiling + storedTicks;
				}

				return storedTicks | ( unchecked( ( Int64 )LocalMask ) );
			}
			else
			{
				return ( Int64 )( ( ulong )source.Ticks | ( ( ulong )source.Kind << KindShift ) );
			}
		}        
		// End porting
#endif // SILVERLIGHT

#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY
		private static readonly DateTime _fileTimeEpocUtc = new DateTime( 1601, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		public static DateTime ToDateTime( this FILETIME source )
		{
			// DateTime.FromFileTimeUtc in Mono 2.10.x does not return Utc DateTime (Mono issue #2936), so do convert manually to ensure returned DateTime is UTC.
			return
				_fileTimeEpocUtc.AddTicks(
					unchecked( ( ( long )source.dwHighDateTime << 32 ) | ( source.dwLowDateTime & 0xffffffff ) )
				);
		}

		public static FILETIME ToWin32FileTimeUtc( this DateTime source )
		{
			var value = source.ToFileTimeUtc();
			return
				new FILETIME
				{
					dwHighDateTime = unchecked( ( int ) ( value >> 32 ) ),
					dwLowDateTime = unchecked( ( int ) ( value & 0xffffffff ) )
				};
		}
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY
	}
}