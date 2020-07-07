// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.Internal
{
	internal static partial class AsyncDecodeHelpers
	{
		public static async ValueTask<bool> TryDecodeNullAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			bool result;
			while (!TryDecodeNull(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeNull(FormatDecoder decoder, ReadOnlyStreamSequence source, out bool result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = decoder.TryDecodeNull(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<int> DecodeArrayHeaderAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			int result;
			while (!TryDecodeArrayHeader(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeArrayHeader(FormatDecoder decoder, ReadOnlyStreamSequence source, out int result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = decoder.DecodeArrayHeader(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<int> DecodeMapHeaderAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			int result;
			while (!TryDecodeMapHeader(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeMapHeader(FormatDecoder decoder, ReadOnlyStreamSequence source, out int result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = decoder.DecodeArrayHeader(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<CollectionTypeAndCount> DecodeArrayOrMapHeaderAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			CollectionTypeAndCount result;
			while (!TryDecodeArrayOrMapHeader(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeArrayOrMapHeader(FormatDecoder decoder, ReadOnlyStreamSequence source, out CollectionTypeAndCount result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			var type = decoder.DecodeArrayOrMapHeader(ref reader, out var itemsCount, out requestHint);
			result = new CollectionTypeAndCount(type, itemsCount);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<CollectionItemIterator> DecodeArrayAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			CollectionItemIterator result;
			while (!TryDecodeArray(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeArray(FormatDecoder decoder, ReadOnlyStreamSequence source, out CollectionItemIterator result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = decoder.DecodeArray(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<CollectionItemIterator> DecodeMapAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			CollectionItemIterator result;
			while (!TryDecodeMap(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeMap(FormatDecoder decoder, ReadOnlyStreamSequence source, out CollectionItemIterator result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = decoder.DecodeArray(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<CollectionTypeAndIterator> DecodeArrayOrMapAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			CollectionTypeAndIterator result;
			while (!TryDecodeArrayOrMap(decoder, source, out result, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool TryDecodeArrayOrMap(FormatDecoder decoder, ReadOnlyStreamSequence source, out CollectionTypeAndIterator result, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			var type = decoder.DecodeArrayOrMap(ref reader, out var iterator, out requestHint);
			result = new CollectionTypeAndIterator(type, iterator);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask SkipAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CollectionContext collectionContext, CancellationToken cancellationToken)
		{
			while (!TrySkip(decoder, source, collectionContext, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}
		}

		private static bool TrySkip(FormatDecoder decoder, ReadOnlyStreamSequence source, CollectionContext collectionContext, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			decoder.Skip(ref reader, collectionContext, out requestHint, cancellationToken);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask DrainAsync(this FormatDecoder decoder, ReadOnlyStreamSequence source, CollectionContext collectionContext, long itemsCount, CancellationToken cancellationToken)
		{
			while (!TryDrain(decoder, source, collectionContext, itemsCount, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}
		}

		private static bool TryDrain(FormatDecoder decoder, ReadOnlyStreamSequence source, CollectionContext collectionContext, long itemsCount, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			decoder.Drain(ref reader, collectionContext, itemsCount, out requestHint, cancellationToken);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask<bool> CollectionEndsAsync(this CollectionItemIterator iterator, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			bool result;
			while (!TryCollectionEnds(iterator, source, out result, out var requestHint))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		private static bool TryCollectionEnds(CollectionItemIterator iterator, ReadOnlyStreamSequence source, out bool result, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = iterator.CollectionEnds(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}

		public static async ValueTask DrainAsync(this CollectionItemIterator iterator, ReadOnlyStreamSequence source, CancellationToken cancellationToken)
		{
			while (!TryDrain(iterator, source, out var requestHint, cancellationToken))
			{
				await source.FetchAsync(requestHint, cancellationToken).ConfigureAwait(false);
			}
		}

		private static bool TryDrain(CollectionItemIterator iterator, ReadOnlyStreamSequence source, out int requestHint, CancellationToken cancellationToken)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			iterator.Drain(ref reader, out requestHint);
			source.Advance(reader.Consumed);
			return requestHint == 0;
		}
	}
}
