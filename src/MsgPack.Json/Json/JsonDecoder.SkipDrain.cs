// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		public override void Drain(ref SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, out int requestHint, CancellationToken cancellationToken = default)
			=> JsonThrow.DrainIsNotSupported(out requestHint);

		public override void Skip(ref SequenceReader<byte> source, in CollectionContext collectionContext, out int requestHint, CancellationToken cancellationToken = default)
		{
			var originalPosition = source.Consumed;

			if(!this.DecodeItem(ref source, out var decodeItemResult, cancellationToken))
			{
				requestHint = (int)(decodeItemResult.RequestHint & Int32.MaxValue);
				return;
			}

			switch (decodeItemResult.ElementType)
			{
				case ElementType.Array:
				case ElementType.Map:
				{
					// Skip current collection with CollectionIterator.Drain()
					var iterator = decodeItemResult.CollectionIterator;
					if (!iterator.Drain(ref source, out requestHint))
					{
						source.Rewind(source.Consumed - originalPosition);
						return;
					}

					break;
				}
			}

			requestHint = 0;
		}
	}
}
