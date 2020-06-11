// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Json
{
	/// <summary>
	///		Defines Unicode related utilities.
	/// </summary>
	internal static class Unicode
	{
		public static bool IsLowSurrogate(int cp)
			=> cp >= 0xDC00u && cp <= 0xDFFFu;

		public static bool IsHighSurrogate(int cp)
			=> cp >= 0xD800u && cp <= 0xDBFFu;
	}
}
