// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON trivials decoding related binary constants which are stored as native BLOB.
	/// </summary>
	internal static class JsonTriviaTokens
	{
		public static readonly byte[][] MultiByteUnicodeWhitespaces =
			new[]
			{
				new byte[] { 0xC2, 0x85 }, // U+0085 NEXT LINE (NEL)
				new byte[] { 0xC2, 0xA0 }, // U+00A0 NO-BREAK SPACE
				new byte[] { 0xE1, 0x9A, 0x80 }, // U+1680 OGHAM SPACE MARK
				new byte[] { 0xE1, 0x9A, 0x80 }, // U+2000 EN QUAD
				new byte[] { 0xE2, 0x80, 0x81 }, // U+2001 EM QUAD
				new byte[] { 0xE2, 0x80, 0x82 }, // U+2002 EN SPACE
				new byte[] { 0xE2, 0x80, 0x83 }, // U+2003 EM SPACE
				new byte[] { 0xE2, 0x80, 0x84 }, // U+2004 THREE-PER-EM SPACE
				new byte[] { 0xE2, 0x80, 0x85 }, // U+2005 FOUR-PER-EM SPACE
				new byte[] { 0xE2, 0x80, 0x86 }, // U+2006 SIX-PER-EM SPACE
				new byte[] { 0xE2, 0x80, 0x87 }, // U+2007 FIGURE SPACE
				new byte[] { 0xE2, 0x80, 0x88 }, // U+2008 PUNCTUATION SPACE
				new byte[] { 0xE2, 0x80, 0x89 }, // U+2009 THIN SPACE
				new byte[] { 0xE2, 0x80, 0x8A }, // U+200A HAIR SPACE
				new byte[] { 0xE2, 0x80, 0xA8 }, // U+2028 LINE SEPARATOR
				new byte[] { 0xE2, 0x80, 0xA9 }, // U+2029 PARAGRAPH SEPARATOR
				new byte[] { 0xE2, 0x80, 0xAF }, // U+202F NARROW NO-BREAK SPACE
				new byte[] { 0xE2, 0x81, 0x9F }, // U+205F MEDIUM MATHEMATICAL SPACE
				new byte[] { 0xEe, 0x80, 0x80 }, // U+3000 IDEOGRAPHIC SPACE
			};
	}
}
