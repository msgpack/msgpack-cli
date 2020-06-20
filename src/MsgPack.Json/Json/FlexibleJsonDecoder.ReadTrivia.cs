// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack.Json
{
	internal partial class FlexibleJsonDecoder
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override long ReadTrivia(ref SequenceReader<byte> source)
		{
			ReadOnlySpan<byte> singleByteWhitespaces;
			unsafe
			{
				if ((this.Options.ParseOptions & JsonParseOptions.AllowUnicodeWhitespace) == 0)
				{
					byte* pStandardWhitespaces = stackalloc byte[] { (byte)' ', (byte)'\t', (byte)'\r', (byte)'\n' };
					singleByteWhitespaces = new ReadOnlySpan<byte>(pStandardWhitespaces, 4);
				}
				else
				{
					byte* pSingleByteUnicodeWhitespaces =
						stackalloc byte[]
						{
							(byte)' ', (byte)'\t', (byte)'\r', (byte)'\n',
							(byte)'\u000B', (byte)'\u000C'
						};
					singleByteWhitespaces = new ReadOnlySpan<byte>(pSingleByteUnicodeWhitespaces, 6);
				}
			}

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

			return consumed;
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
			ReadOnlySpan<byte> singleLineCommentStart;
			ReadOnlySpan<byte> newLine;
			unsafe
			{
				byte* pSingleLineCommentStart = stackalloc[] { (byte)'/', (byte)'/' };
				singleLineCommentStart = new ReadOnlySpan<byte>(pSingleLineCommentStart, 2);
				byte* pNewLine = stackalloc[] { (byte)'\r', (byte)'\n' };
				newLine = new ReadOnlySpan<byte>(pNewLine, 2);
			}

			if (((this.Options.ParseOptions & JsonParseOptions.AllowHashSingleLineComment) != 0 && source.IsNext((byte)'#', advancePast: true))
				|| ((this.Options.ParseOptions & JsonParseOptions.AllowDoubleSolidousSingleLineComment) != 0 && source.IsNext(singleLineCommentStart, advancePast: true)))
			{
				if (!source.TryReadToAny(out ReadOnlySequence<byte> line, newLine, advancePastDelimiter: true))
				{
					// to EoF
					consumed += source.Remaining;
					source.Advance(source.Remaining);
					return true;
				}

				consumed += line.Length;
				consumed += source.AdvancePastAny((byte)'\r', (byte)'\n');
				return true;
			}

			return false;
		}

		private bool TryReadMultiLineComment(in SequenceReader<byte> source, ref long consumed)
		{
			if((this.Options.ParseOptions & JsonParseOptions.AllowMultilineComment) == 0)
			{
				return false;
			}

			ReadOnlySpan<byte> multiLineCommentStart;
			ReadOnlySpan<byte> multiLineCommentEnd;
			unsafe
			{
				byte* pMultiLineCommentStart = stackalloc[] { (byte)'/', (byte)'*' };
				multiLineCommentStart = new ReadOnlySpan<byte>(pMultiLineCommentStart, 2);
				byte* pMultiLineCommentEnd = stackalloc[] { (byte)'*', (byte)'/' };
				multiLineCommentEnd = new ReadOnlySpan<byte>(pMultiLineCommentEnd, 2);
			}

			var offset = source.Consumed;

			if (!source.IsNext(multiLineCommentStart, advancePast: true))
			{
				return false;
			}

			if (!source.TryReadToAny(out ReadOnlySequence<byte> comment, multiLineCommentEnd, advancePastDelimiter: true))
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
