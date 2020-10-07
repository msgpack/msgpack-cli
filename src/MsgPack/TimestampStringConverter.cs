// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack
{
	/// <summary>
	///		An internal <see cref="Timestamp"/> parser and stringifier.
	/// </summary>
	internal static partial class TimestampStringConverter
	{
		private const char DateDelimiter = '-';
		private const char TimeDelimiter = ':';
		private const char DateTimeDelimiter = 'T';
		private const char SubsecondDelimiter = '.';
		private const char UtcSign = 'Z';
	}
}
