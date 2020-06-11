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
		public static ReadOnlySpan<byte> SingleQuotationDelimiters => new[] { (byte)'\'', (byte)'\\' };
		public static ReadOnlySpan<byte> DoubleQuotationDelimiters => new[] { (byte)'"', (byte)'\\' };
		public static readonly byte UnescapedCharactorsOffset = UnescapedCharactors[0];
		public static ReadOnlySpan<byte> UnescapedCharactors =>
			new byte[]
			{
				(byte)'"', // U+0022 "
				0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, // U+0023-U+002E
				(byte)'/', // U+002F /
				0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F, // U+0030-U+003F
				0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF, // U+0040-U+004F
				0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xBB, // U+0050-U+005B
				(byte)'\\', // U+005C \
				0xBD, 0xBE, 0xBF, // U+005D-U+005F
				0xC0, 0xC1, // U+0060-U+0061
				(byte)'\b', // U+0062 -> U+0008 Back Space
				0xC3, 0xC4, 0xC5, // U+0063-U+0065
				(byte)'\f', // U+0066 -> U+000C Form Feed
				0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, // UL0067-U+006D
				(byte)'\n', // U+006E -> U+000A Line Feed
				0xCF, // U+006F
				0xD0, 0xD1, // U+0070-U+0071
				(byte)'\r', // U+0072 -> U+000D Carriage Return  
				0xD3, // U+0073
				(byte)'\t', // U+0074 -> U+0009 Tab
			};
	}
}
