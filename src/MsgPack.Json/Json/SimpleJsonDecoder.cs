// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Json
{
	/// <summary>
	///		Simple <see cref="JsonDecoder"/> implementation which is optimized for RFC compliant JSON parsing.
	/// </summary>
	internal sealed class SimpleJsonDecoder : JsonDecoder
	{
		public SimpleJsonDecoder(JsonDecoderOptions options)
			: base(options) { }

		protected override void ReadTrivia(in SequenceReader<byte> source, out ReadOnlySequence<byte> trivia)
		{
			var offset = source.Consumed;
			var consumed = source.AdvancePastAny(JsonTriviaTokens.StandardWhitespaces);
			trivia = source.Sequence.Slice(offset, consumed);
		}

		protected override bool TryReadNull(in SequenceReader<byte> source)
			=> source.IsNext(JsonTokens.Null, advancePast: true);
	}
}
