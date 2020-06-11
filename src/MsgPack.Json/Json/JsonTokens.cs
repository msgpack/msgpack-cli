// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON tokens as UTF-8 byte blob data.
	/// </summary>
	internal static class JsonTokens
	{
		// Utilizes C# optimization to read data directly from blob metadata without array allocation.
		public static ReadOnlySpan<byte> Null => new[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
		public static ReadOnlySpan<byte> True => new[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
		public static ReadOnlySpan<byte> False => new[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
		public static ReadOnlySpan<byte> ArrayStart => new[] { (byte)'[' };
		public static ReadOnlySpan<byte> ArrayEnd => new[] { (byte)']' };
		public static ReadOnlySpan<byte> MapStart => new[] { (byte)'{' };
		public static ReadOnlySpan<byte> MapEnd => new[] { (byte)'}' };
		public static ReadOnlySpan<byte> Whitespace => new[] { (byte)' ' };
		public static ReadOnlySpan<byte> Comma => new[] { (byte)',' };
		public static ReadOnlySpan<byte> Colon => new[] { (byte)':' };
		public static ReadOnlySpan<byte> Quatation => new[] { (byte)'"' };

		public static ReadOnlySpan<byte> Undefined => new[] { (byte)'u', (byte)'n', (byte)'d', (byte)'e', (byte)'f', (byte)'i', (byte)'n', (byte)'e', (byte)'d' };
	}
}
