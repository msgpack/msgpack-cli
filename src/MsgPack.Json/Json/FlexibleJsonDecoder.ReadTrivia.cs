// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Json
{
	internal partial class FlexibleJsonDecoder
	{
		protected override void ReadTrivia(in SequenceReader<byte> source, out ReadOnlySequence<byte> trivia)
		{
			var offset = source.Consumed;
			var singleByteWhitespaces = (this.Options.ParseOptions & JsonParseOptions.AllowUnicodeWhitespace) == 0 ? JsonTriviaTokens.StandardWhitespaces : JsonTriviaTokens.SingleByteUnicodeWhitespaces;

			var consumed = 0L;
			while (!source.End)
			{
				consumed += source.AdvancePastAny(singleByteWhitespaces);

				if (this.TryReadMultiByteUnicodeWhitespaces(source, ref consumed))
				{
					continue;
				}

				// Single line comment
				if (this.TryReadSingleLineComment(source, ref consumed))
				{
					continue;
				}

				// Multi line comment
				if (this.TryReadMultiLineComment(source, ref consumed))
				{
					continue;
				}
			}

			trivia = source.Sequence.Slice(offset, consumed);
		}

		private bool TryReadMultiByteUnicodeWhitespaces(in SequenceReader<byte> source, ref long consumed)
		{
			if ((this.Options.ParseOptions & JsonParseOptions.AllowUnicodeWhitespace) == 0)
			{
				return false;
			}

			foreach (var whitespace in JsonTriviaTokens.MultiByteUnicodeWhitespaces)
			{
				var length = source.AdvancePastAny(whitespace);
				if (length > 0)
				{
					consumed += length;
					return true;
				}
			}

			return false;
		}

		private bool TryReadSingleLineComment(in SequenceReader<byte> source, ref long consumed)
		{
			if (((this.Options.ParseOptions & JsonParseOptions.AllowHashSingleLineComment) != 0 && source.IsNext((byte)'#', advancePast: true))
				|| ((this.Options.ParseOptions & JsonParseOptions.AllowDoubleSolidousSingleLineComment) != 0 && source.IsNext(JsonTriviaTokens.SingleLineCommentStart2, advancePast: true)))
			{
				if (!source.TryReadToAny(out ReadOnlySequence<byte> line, JsonTriviaTokens.NewLine, advancePastDelimiter: true))
				{
					// to EoF
					consumed += source.Remaining;
					source.Advance(source.Remaining);
					return true;
				}

				consumed += line.Length;
				consumed += source.AdvancePastAny(JsonTriviaTokens.NewLine);
				return true;
			}

			return false;
		}

		private bool TryReadMultiLineComment(in SequenceReader<byte> source, ref long consumed)
		{
			var offset = source.Consumed;

			if ((this.Options.ParseOptions & JsonParseOptions.AllowMultilineComment) == 0 || !source.IsNext(JsonTriviaTokens.MultiLineCommentStart, advancePast: true))
			{
				return false;
			}

			if (!source.TryReadToAny(out ReadOnlySequence<byte> comment, JsonTriviaTokens.MultiLineCommentEnd, advancePastDelimiter: true))
			{
				// */ is not found.
				source.Rewind(2);
				return false;
			}

			consumed += comment.Length;
			return true;
		}
	}
}
