// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Linq;
using System.Text;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON constant chars in UTF-8 or related structs.
	/// </summary>
	internal static class JsonCharactor
	{
		public static readonly byte[] MustBeEscaped1Byte = new[] { (byte)'\\', (byte)'"' };
		public static readonly ReadOnlyMemory<Rune> ShouldBeEscaped = new[] { (byte)'/', (byte)'\'', (byte)'<', (byte)'>', (byte)'&' }.Select(b => new Rune(b)).ToArray(); // For HTML embedding
		public static ReadOnlySpan<byte> CarriageReturn => new[] { (byte)'\r' };
		public static ReadOnlySpan<byte> LineFeed => new[] { (byte)'\n' };
	}
}
