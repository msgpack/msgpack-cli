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
	///		Simple <see cref="JsonDecoder"/> implementation which can handle various resource consuming relaxations.
	/// </summary>
	internal sealed partial class FlexibleJsonDecoder : JsonDecoder
	{
		public FlexibleJsonDecoder(JsonDecoderOptions options)
			: base(options) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override unsafe bool TryDecodeNull(ref SequenceReader<byte> source, out int requestHint)
		{
			byte* pNull= stackalloc byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
			ReadOnlySpan<byte> @null = new ReadOnlySpan<byte>(pNull, 4);

			if (source.IsNext(@null, advancePast: true))
			{
				requestHint = 0;
				source.Advance(4);
				return true;
			}

			return this.TryDecodeNullSlow(ref source, out requestHint);
		}

		private unsafe bool TryDecodeNullSlow(ref SequenceReader<byte> source, out int requestHint)
		{
			byte* pUndefined = stackalloc byte[] { (byte)'u', (byte)'n', (byte)'d', (byte)'e', (byte)'f', (byte)'i', (byte)'n', (byte)'e', (byte)'d', };
			ReadOnlySpan<byte> undefined = new ReadOnlySpan<byte>(pUndefined, 9);

			if ((this.Options.ParseOptions & JsonParseOptions.AllowUndefined) != 0
				&& source.IsNext(undefined, advancePast: true))
			{
				requestHint = 0;
				source.Advance(9);
				return true;
			}

			requestHint = Math.Max(0, 4 - (int)source.Remaining);
			return false;
		}
	}
}
