// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_COM_TYPES
using System.Runtime.InteropServices.ComTypes;
#endif // FEATURE_COM_TYPES

namespace MsgPack.Serialization.BuiltinSerializers
{
	internal static class InternalDateTimeExtensions
	{
#if FEATURE_COM_TYPES
		private static readonly DateTime _fileTimeEpocUtc = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime ToDateTime(this FILETIME source)
		{
			// DateTime.FromFileTimeUtc in Mono 2.10.x does not return Utc DateTime (Mono issue #2936), so do convert manually to ensure returned DateTime is UTC.
			return
				_fileTimeEpocUtc.AddTicks(
					unchecked(((long)source.dwHighDateTime << 32) | (source.dwLowDateTime & 0xffffffff))
				);
		}

		public static FILETIME ToWin32FileTimeUtc(this DateTime source)
		{
			var value = source.ToFileTimeUtc();
			return
				new FILETIME
				{
					dwHighDateTime = unchecked((int)(value >> 32)),
					dwLowDateTime = unchecked((int)(value & 0xffffffff))
				};
		}
#endif // FEATURE_COM_TYPES
	}
}
