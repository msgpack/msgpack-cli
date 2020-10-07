// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	public struct CollectionItemIterator
	{
		public delegate bool CollectionEndDetection(ref SequenceReader<byte> source, ref long nextItemIndex, long itemsCount, out int requestHint);

		private readonly long _itemsCount;
		private long _nextItemIndex;
		private readonly CollectionEndDetection _collectionEnds;

		public CollectionItemIterator(
			CollectionEndDetection moveNext,
			long itemsCount
		)
		{
			this._collectionEnds = Ensure.NotNull(moveNext);
			this._itemsCount = itemsCount;
			this._nextItemIndex = 0;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool CollectionEnds(ref SequenceReader<byte> source)
		{
			var result = this._collectionEnds(ref source, ref this._nextItemIndex, this._itemsCount, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDetectCollectionEnds(source.Consumed, requestHint);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool CollectionEnds(ref SequenceReader<byte> source, out int requestHint)
			=> this._collectionEnds(ref source, ref this._nextItemIndex, this._itemsCount, out requestHint);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool CollectionEnds(in ReadOnlySequence<byte> source, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source);
			return this.CollectionEnds(ref reader, out requestHint);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void Drain(ref SequenceReader<byte> source)
		{
			if (!this.Drain(ref source, out var requestHint))
			{
				Throw.InsufficientInputForDrainCollectionItems(source.Consumed, requestHint);
			}
		}

#warning TODO: CancellationToken
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool Drain(ref SequenceReader<byte> source, out int requestHint)
		{
			while (!this.CollectionEnds(ref source, out requestHint))
			{
				if (requestHint != 0)
				{
					return false;
				}
			}

			return true;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool Drain(ref ReadOnlySequence<byte> source, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source);
			var ends = this.Drain(ref reader, out requestHint);
			source = source.Slice((int)reader.Consumed);
			return ends;
		}
	}
}
