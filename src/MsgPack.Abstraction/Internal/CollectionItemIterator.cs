// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	public struct CollectionItemIterator
	{
		public delegate bool CollectionEndDetection(in SequenceReader<byte> source, ref long nextItemIndex, long itemsCount, out int requestHint);

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
		public bool CollectionEnds(in SequenceReader<byte> source)
		{
			var result = this._collectionEnds(source, ref this._nextItemIndex, this._itemsCount, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDetectCollectionEnds(source.Consumed, requestHint);
			}

			return result;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool CollectionEnds(in SequenceReader<byte> source, out int requestHint)
			=> this._collectionEnds(source, ref this._nextItemIndex, this._itemsCount, out requestHint);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool CollectionEnds(ReadOnlyMemory<byte> source, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(source));
			return this.CollectionEnds(reader, out requestHint);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void Drain(in SequenceReader<byte> source)
		{
			if (!this.Drain(source, out var requestHint))
			{
				Throw.InsufficientInputForDrainCollectionItems(source.Consumed, requestHint);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool Drain(in SequenceReader<byte> source, out int requestHint)
		{
			while (!this.CollectionEnds(source, out requestHint))
			{
				if (requestHint != 0)
				{
					return false;
				}
			}

			return true;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool Drain(ref ReadOnlyMemory<byte> source, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(source));
			var ends = this.Drain(reader, out requestHint);
			source = source.Slice((int)reader.Consumed);
			return ends;
		}
	}
}
