// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines known escape sequence int UTF-8 and related structures.
	/// </summary>
	internal static partial class JsonEscapeSequence
	{
		public static ReadOnlySpan<byte> Unicode => new[] { (byte)'\\', (byte)'u' };
		public static ReadOnlySpan<byte> ReverseSolidous => new[] { (byte)'\\', (byte)'\\' };
		public static ReadOnlySpan<byte> Quatation => new[] { (byte)'\\', (byte)'"' };
		public static ReadOnlySpan<byte> Tab => new[] { (byte)'\\', (byte)'t' };
		public static ReadOnlySpan<byte> CarriageReturn => new[] { (byte)'\\', (byte)'r' };
		public static ReadOnlySpan<byte> LineFeed => new[] { (byte)'\\', (byte)'n' };
		public static ReadOnlySpan<byte> BackSpace => new[] { (byte)'\\', (byte)'b' };
		public static ReadOnlySpan<byte> FormFeed => new[] { (byte)'\\', (byte)'f' };

		public static readonly StandardFormat UnicodeFormat = new StandardFormat('X', 4);
	}
}
