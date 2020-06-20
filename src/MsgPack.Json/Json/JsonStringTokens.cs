// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON string decoding related binary constants which are stored as native BLOB.
	/// </summary>
	internal static class JsonStringTokens
	{
		public static ReadOnlySpan<byte> AnyQuotations => new[] { (byte)'"', (byte)'\'' };
		public static ReadOnlySpan<byte> DoubleQuotation => AnyQuotations.Slice(0, 1);
	}
}
