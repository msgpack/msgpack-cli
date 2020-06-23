// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Globalization;
using System.Linq;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Json
{
	internal static class JsonThrow
	{
		public static void TooShortUtf8()
			=> throw new FormatException($"Input UTF-8 sequence is invalid. The sequence unexpectedly ends.");

		public static CollectionType CollectionHeaderDecodingIsNotSupported(out int itemsCount, out int requestHint)
			=> throw new NotSupportedException("JSON does not support collection length.");

		public static int CollectionHeaderDecodingIsNotSupported(out int requestHint)
			=> throw new NotSupportedException("JSON does not support collection length.");

		public static void DrainIsNotSupported(out int requestHint)
			=> throw new NotSupportedException("JSON does not support collection length.");

		public static void MalformedUtf8(in ReadOnlySpan<byte> sequence, long position)
			=> throw new FormatException($"Input UTF-8 has invalid sequence {BitConverter.ToString(sequence.ToArray())} at position {position:#,0}.");

		public static void MalformedUtf8(in ReadOnlySequence<byte> sequence, long position)
			=> throw new FormatException($"Input UTF-8 has invalid sequence {BitConverter.ToString(sequence.ToArray())} at position {position:#,0}.");

		public static void SurrogateCharInUtf8(long position, int codePoint)
			=> throw new FormatException($"Input UTF-8 has surrogate charactor U+{codePoint:X4} at position {position:#,0}.");

		public static void IsNotArrayNorObject(in ReadOnlySequence<byte> sequence, long position)
			=> throw new FormatException($"Char {Stringify(sequence)} at position {position:#,0} is not start of array nor object.");

		public static void IsNotArray(long position)
			=> throw new FormatException($"Char '{{' at position {position:#,0} is not start of array.");

		public static void IsNotObject(long position)
			=> throw new FormatException($"Char '[' at position {position:#,0} is not start of object.");

		public static void IsNotType(Type type, in ReadOnlySequence<byte> unit, long position)
			=> throw new FormatException($"Char {Stringify(unit)} at position {position:#,0} is not valid for {type} value.");

		public static void TooLongNumber(long numberLength, long maxLength, long position)
			=> throw new FormatException($"The number at position {position:#,0} has {numberLength:#,0} charactors, but maximum allowed length is {maxLength:#,0}.");

		public static void IsNotStringStart(long position, ReadOnlySpan<byte> validQuotations)
		{
			if (validQuotations.Length == 1)
			{
				throw new FormatException($"String must starts with '{(char)validQuotations[0]}' (U+00{validQuotations[0]:X2}) at {position:#,0}.");
			}
			else
			{
				throw new FormatException($"String must starts with one of [{String.Join(", ", validQuotations.ToArray().Select(b => $"'{(char)b}'(U + 00{b:X2})"))}] at {position:#,0}.");
			}
		}

		public static void InvalidEscapeSequence(long position, byte escaped)
			=> throw new FormatException($"Escape sequence '\\{(char)escaped}' at {position:#,0} is not valid.");

		public static void InvalidUnicodeEscapeSequence(long position, Span<byte> chars)
			=> throw new FormatException($"Escape sequence '\\u{Encoding.UTF8.GetString(chars)}' at {position:#,0} is not valid.");

		public static void OrphanSurrogate(long position, int codePoint)
			=> throw new FormatException($"Surrogate char U+{codePoint:X4} at {position:#,0} is not valid.");

		public static void InvalidBase64(long position)
			=> throw new FormatException($"String at {position:#,0} is not valid BASE64 sequence.");

		public static void UnexpectedToken(long position, byte token)
			=> throw new FormatException($"A token {(token >= 0x80 ? $"0x{token:X2}" : (token < 0x7F && token >= 0x20 ? $"'{(char)token}'" : $"U+00{token:X2}"))} is not expected at {position:#,0}.");

		private static string Stringify(in ReadOnlySequence<byte> unit)
		{
			var buffer = new StringBuilder();
			foreach (var rune in Encoding.UTF8.GetString(unit).EnumerateRunes())
			{
				var category = Rune.GetUnicodeCategory(rune);
				switch (category)
				{
					case UnicodeCategory.Control:
					case UnicodeCategory.Format:
					case UnicodeCategory.LineSeparator:
					case UnicodeCategory.ModifierLetter:
					case UnicodeCategory.ModifierSymbol:
					case UnicodeCategory.NonSpacingMark:
					case UnicodeCategory.OtherNotAssigned:
					case UnicodeCategory.ParagraphSeparator:
					case UnicodeCategory.SpacingCombiningMark:
					case UnicodeCategory.Surrogate:
					{
						buffer.Append($"<control-charactor>(U+{rune.Value:X4}, {category})");
						break;
					}
					case UnicodeCategory.SpaceSeparator:
					{
						buffer.Append($"'{rune}'(U+{rune.Value:X4}, {category})");
						break;
					}
					default:
					{
						buffer.Append('\'').Append(rune).Append('\'');
						break;
					}
				}
			}

			return buffer.ToString();
		}
	}
}
