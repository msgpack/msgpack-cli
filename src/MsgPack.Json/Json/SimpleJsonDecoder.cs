// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		Simple <see cref="JsonDecoder"/> implementation which is optimized for RFC compliant JSON parsing.
	/// </summary>
	internal sealed class SimpleJsonDecoder : JsonDecoder
	{
		public SimpleJsonDecoder(JsonDecoderOptions options)
			: base(options) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override long ReadTrivia(ref SequenceReader<byte> source)
			=> source.AdvancePastAny((byte)' ', (byte)'\t', (byte)'\r', (byte)'\n');

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override unsafe bool TryDecodeNull(ref SequenceReader<byte> source, out int requestHint)
		{
			byte* pNull = stackalloc byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
			ReadOnlySpan<byte> @null = new ReadOnlySpan<byte>(pNull, 4);

			if (source.IsNext(@null, advancePast: true))
			{
				requestHint = 0;
				source.Advance(4);
				return true;
			}

			requestHint = Math.Max(0, 4 - (int)source.Remaining);
			return false;
		}
	}
}
