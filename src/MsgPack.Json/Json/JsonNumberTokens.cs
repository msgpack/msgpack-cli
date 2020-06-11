// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON number decoding related binary constants which are stored as native BLOB.
	/// </summary>
	internal static class JsonNumberTokens
	{
		public static readonly byte MinusSign = (byte)'-';
		public static readonly byte PlusSign = (byte)'+';
		public static readonly byte SingleByteUnicodeMinusSign = MinusSign;
		public static readonly byte[][] MultiByteUnicodeMinusSigns =
			new[]
			{
				new byte[] { 0xE2, 0x81, 0xBB }, // U+207B SUPERSCRIPT MINUS SIGN
				new byte[] { 0xE2, 0x82, 0x8B }, // U+208B SUBSCRIPT MINUS SIGN
				new byte[] { 0xE2, 0x88, 0x92 }, // U+2212 MINUS SIGN
				new byte[] { 0xEF, 0xB9, 0xA3 }, // U+FE63 SMALL HYPHEN-MINUS
				new byte[] { 0xEF, 0xBC, 0x8D }, // U+FF0D FULLWIDTH HYPHEN-MINUS
			};
		public static readonly byte SingleByteUnicodePlus = PlusSign;
		public static readonly byte[][] MultiByteUnicodePlusSigns =
			new[]
			{
				new byte[] { 0xE2, 0x81, 0xBA }, // U+207A SUPERSCRIPT PLUS SIGN
				new byte[] { 0xE2, 0x82, 0x8A }, // U+208A SUBSCRIPT PLUS SIGN
				new byte[] { 0xEF, 0xAC, 0xA9 }, // U+FB29 HEBREW LETTER ALTERNATIVE PLUS SIGN
				new byte[] { 0xEF, 0xB9, 0xA2 }, // U+FE62 SMALL PLUS SIGN
				new byte[] { 0xEF, 0xBC, 0x8B }, // U+FE0B FULLWIDTH PLUS SIGN
			};
		public static ReadOnlySpan<byte> Digits => new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
		public static ReadOnlySpan<byte> NonZeroDigits => new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
		public static ReadOnlySpan<byte> ExponentialIndicators => new[] { (byte)'E', (byte)'e' };
		public static ReadOnlySpan<byte> PlusMinusSigns => new[] { (byte)'-', (byte)'+' };
		public static readonly byte[][] MultiByteUnicodePlusMinusSigns =
			new[]
			{
				new byte[] { 0xE2, 0x81, 0xBB }, // U+207B SUPERSCRIPT MINUS SIGN
				new byte[] { 0xE2, 0x82, 0x8B }, // U+208B SUBSCRIPT MINUS SIGN
				new byte[] { 0xE2, 0x88, 0x92 }, // U+2212 MINUS SIGN
				new byte[] { 0xEF, 0xB9, 0xA3 }, // U+FE63 SMALL HYPHEN-MINUS
				new byte[] { 0xEF, 0xBC, 0x8D },  // U+FF0D FULLWIDTH HYPHEN-MINUS
				new byte[] { 0xE2, 0x81, 0xBA }, // U+207A SUPERSCRIPT PLUS SIGN
				new byte[] { 0xE2, 0x82, 0x8A }, // U+208A SUBSCRIPT PLUS SIGN
				new byte[] { 0xEF, 0xAC, 0xA9 }, // U+FB29 HEBREW LETTER ALTERNATIVE PLUS SIGN
				new byte[] { 0xEF, 0xB9, 0xA2 }, // U+FE62 SMALL PLUS SIGN
				new byte[] { 0xEF, 0xBC, 0x8B }, // U+FE0B FULLWIDTH PLUS SIGN
			};
		public static ReadOnlySpan<byte> SingleByteUnicodeExponentialIndicators => ExponentialIndicators;
		public static readonly byte[][] MultiByteUnicodeExponentialIndicators =
			new[]
			{
				new byte[] { 0xE1, 0xB4, 0xB1 }, // U+1D31 MODIFIER LETTER CAPITAL E
				new byte[] { 0xE1, 0xB5, 0x89 }, // U+1D49 MODIFIER LETTER SMALL E
				new byte[] { 0xE2, 0x82, 0x91 }, // U+2091 LATIN SUBSCRIPT SMALL LETTER E
				new byte[] { 0xE2, 0x84, 0xAF }, // U+212F SCRIPT SMALL E
				new byte[] { 0xE2, 0x84, 0xB0 }, // U+2130 SCRIPT CAPITAL E
				new byte[] { 0xE2, 0x85, 0x87 }, // U+2147 DOUBLE-STRUCK ITALIC SMALL E
				new byte[] { 0xE2, 0x92, 0xBA }, // U+24BA CIRCLED LATIN CAPITAL LETTER E
				new byte[] { 0xE2, 0x93, 0x94 }, // U+24D4 CIRCLED LATIN SMALL LETTER E
				new byte[] { 0xEF, 0xBC, 0xA5 }, // U+FF25 FULLWIDTH LATIN CAPITAL LETTER E
				new byte[] { 0xEF, 0xBD, 0x85 }, // U+FF45 FULLWIDTH LATIN SMALL LETTER E
				new byte[] { 0xF0, 0x9D, 0x90, 0x84 }, // U+1D404 MATHEMATICAL BOLD CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x90, 0x9E }, // U+1D41E ATHEMATICAL BOLD SMALL E
				new byte[] { 0xF0, 0x9D, 0x90, 0xB8 }, // U+1D438 MATHEMATICAL ITALIC CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x91, 0x92 }, // U+1D452 MATHEMATICAL ITALIC SMALL E
				new byte[] { 0xF0, 0x9D, 0x91, 0xAC }, // U+1D46C MATHEMATICAL BOLD ITALIC CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x92, 0x86 }, // U+1D486 MATHEMATICAL BOLD ITALIC SMALL E
				new byte[] { 0xF0, 0x9D, 0x93, 0x94 }, // U+1D4D4 MATHEMATICAL BOLD SCRIPT CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x93, 0xAE }, // U+1D4EE MATHEMATICAL BOLD SCRIPT SMALL E
				new byte[] { 0xF0, 0x9D, 0x94, 0x88 }, // U+1D508 MATHEMATICAL FRAKTUR CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x94, 0xA2 }, // U+1D522 MATHEMATICAL FRAKTUR SMALL E
				new byte[] { 0xF0, 0x9D, 0x94, 0xBC }, // U+1D53C MATHEMATICAL DOUBLE-STRUCK CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x95, 0x96 }, // U+1D556 MATHEMATICAL DOUBLE-STRUCK SMALL E
				new byte[] { 0xF0, 0x9D, 0x95, 0xB0 }, // U+1D570 MATHEMATICAL BOLD FRAKTUR CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x96, 0x8A }, // U+1D58A MATHEMATICAL BOLD FRAKTUR SMALL E
				new byte[] { 0xF0, 0x9D, 0x96, 0xA4 }, // U+1D5A4 MATHEMATICAL SANS-SERIF CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x96, 0xBE }, // U+1D5BE MATHEMATICAL SANS-SERIF SMALL E
				new byte[] { 0xF0, 0x9D, 0x97, 0x98 }, // U+1D5D8 MATHEMATICAL SANS-SERIF BOLD CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x97, 0xB2 }, // U+1D5F2 MATHEMATICAL SANS-SERIF BOLD SMALL E
				new byte[] { 0xF0, 0x9D, 0x98, 0x8C }, // U+1D60C MATHEMATICAL SANS-SERIF ITALIC CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x98, 0xA6 }, // U+1D626 MATHEMATICAL SANS-SERIF ITALIC SMALL E
				new byte[] { 0xF0, 0x9D, 0x99, 0x80 }, // U+1D640 MATHEMATICAL SANS-SERIF BOLD ITALIC CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x99, 0x9A }, // U+1D65A MATHEMATICAL SANS-SERIF BOLD ITALIC SMALL E
				new byte[] { 0xF0, 0x9D, 0x99, 0xB4 }, // U+1D674 MATHEMATICAL MONOSPACE CAPITAL E
				new byte[] { 0xF0, 0x9D, 0x9A, 0x8E }, // U+1D68E MATHEMATICAL MONOSPACE SMALL E
				new byte[] { 0xF0, 0x9F, 0x84, 0xB4 }, // U+1F134 SQUARED LATIN CAPITAL LETTER E
			};
	}
}
