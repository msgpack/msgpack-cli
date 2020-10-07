// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

#warning TODO: Move to Common/

using System;
using System.Buffers;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	internal static class StringEscape
	{
		public static string ForDisplay(string value)
		{
			if (value == null)
			{
				return String.Empty;
			}

			var buffer = new StringBuilder();
			foreach (Rune r in value.EnumerateRunes())
			{
				if (buffer.Length > 64)
				{
					buffer.Append("...");
					break;
				}

				switch (r.Value)
				{
					case '\0':
					{
						buffer.Append("\\0");
						continue;
					}
					case '\t':
					{
						buffer.Append("\\t");
						continue;
					}
					case '\r':
					{
						buffer.Append("\\r");
						continue;
					}
					case '\n':
					{
						buffer.Append("\\n");
						continue;
					}
					case '\a':
					{
						buffer.Append("\\a");
						continue;
					}
					case '\b':
					{
						buffer.Append("\\b");
						continue;
					}
					case '\f':
					{
						buffer.Append("\\f");
						continue;
					}
					case '\v':
					{
						buffer.Append("\\v");
						continue;
					}
					case '\\':
					{
						buffer.Append("\\\\");
						continue;
					}
					case ' ':
					{
						buffer.Append(' ');
						continue;
					}
				}

				switch (Rune.GetUnicodeCategory(r))
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
					case UnicodeCategory.PrivateUse:
					{
						if (r.Value == ' ')
						{
							goto default;
						}

						buffer.Append("\\u").Append((r.Value).ToString("X4", CultureInfo.InvariantCulture));
						continue;
					}
					default:
					{
						Span<char> c = stackalloc char[2];
						r.TryEncodeToUtf16(c, out var charsWritten);
						buffer.Append(c.Slice(0, charsWritten));
						continue;
					}
				}
			}

			return buffer.ToString();
		}

		public static string Stringify(in ReadOnlySequence<byte> unit)
		{
			var buffer = new StringBuilder();
			foreach (var rune in Encoding.UTF8.GetString(unit.Slice(0, 260).ToArray()).EnumerateRunes())
			{
				if (buffer.Length > 64)
				{
					buffer.Append("...");
					break;
				}

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
