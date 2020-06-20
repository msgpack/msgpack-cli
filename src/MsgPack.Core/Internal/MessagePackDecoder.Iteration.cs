// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		private readonly CollectionItemIterator.CollectionEndDetection _detectCollectionEnds;

		// This is instance method now because CLR/CoreCLR delegates are optmized for instance method invocation.
		// This method might be changed static if C# supports function pointer.
		private bool DetectCollectionEnds(ref SequenceReader<byte> source, ref long nextItemIndex, long itemsCount, out int requestHint)
		{
			requestHint = 0;

			if (nextItemIndex >= itemsCount)
			{
				return false;
			}

			nextItemIndex++;
			return true;
		}

		public override CollectionType DecodeArrayOrMap(ref SequenceReader<byte> source, out CollectionItemIterator iterator, out int requestHint)
		{
			var type = this.PrivateDecodeArrayOrMapHeader(ref source, out _, out var itemsCount, out requestHint);
			if (requestHint != 0)
			{
				iterator = default;
				return default;
			}

			iterator = this.CreateIterator(itemsCount);
			return type;
		}

		private CollectionItemIterator CreateIterator(long itemsCount)
			=> new CollectionItemIterator(this._detectCollectionEnds, itemsCount);

		public override CollectionItemIterator DecodeArray(ref SequenceReader<byte> source, out int requestHint)
		{
			var itemsCount = this.DecodeArrayHeader(ref source, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			return new CollectionItemIterator(this._detectCollectionEnds, itemsCount);
		}

		public override CollectionItemIterator DecodeMap(ref SequenceReader<byte> source, out int requestHint)
		{
			var itemsCount = this.DecodeMapHeader(ref source, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			return new CollectionItemIterator(this._detectCollectionEnds, itemsCount);
		}
	}
}
