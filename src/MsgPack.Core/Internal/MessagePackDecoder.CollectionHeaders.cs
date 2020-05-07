// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		public sealed override CollectionType DecodeArrayOrMapHeader(in SequenceReader<byte> source, out long itemsCount, out int requestHint)
		{
			var consumed = 0L;
			var result = this.PrivateDecodeArrayOrMapHeader(source, ref consumed, out var header, out itemsCount, out requestHint);

			if (itemsCount > Int32.MaxValue)
			{
				MessagePackThrow.TooLargeArrayOrMapLength(header, source.Consumed - consumed, itemsCount);
			}

			return result;
		}

		private CollectionType PrivateDecodeArrayOrMapHeader(in SequenceReader<byte> source, ref long consumed, out byte header, out long itemsCount, out int requestHint)
		{
			var result = this.DecodeArrayOrMapHeaderCore(source, ref consumed, out header, out itemsCount, out requestHint);
			if (result == CollectionType.None)
			{
				MessagePackThrow.TypeIsNotArrayNorMap(header, source.Consumed - consumed);
			}

			return result;
		}

		private CollectionType DecodeArrayOrMapHeaderCore(in SequenceReader<byte> source, ref long consumed, out byte header, out long itemsCount, out int requestHint)
		{
			if (!this.TryPeek(source, out header))
			{
				requestHint = 1;
				itemsCount = 0;
				return default;
			}

			if ((header & MessagePackCode.MinimumFixedArray) == MessagePackCode.MinimumFixedArray)
			{
				itemsCount = (uint)header - MessagePackCode.MinimumFixedArray;
				requestHint = 0;
				source.Advance(1);
				consumed++;
				return CollectionType.Array;
			}

			if ((header & MessagePackCode.MaximumFixedMap) == MessagePackCode.MaximumFixedMap)
			{
				itemsCount = (uint)header - MessagePackCode.MaximumFixedMap;
				requestHint = 0;
				source.Advance(1);
				consumed++;
				return CollectionType.Map;
			}

			int length;
			CollectionType type;
			switch (header)
			{
				case MessagePackCode.Array16:
				{
					length = 3;
					type = CollectionType.Array;
					break;
				}
				case MessagePackCode.Array32:
				{
					length = 5;
					type = CollectionType.Array;
					break;
				}
				case MessagePackCode.Map16:
				{
					length = 3;
					type = CollectionType.Map;
					break;
				}
				case MessagePackCode.Map32:
				{
					length = 5;
					type = CollectionType.Map;
					break;
				}
				default:
				{
					requestHint = 0;
					itemsCount = 0;
					return default;
				}
			}

			var lengthOfLength = length - 1;

			switch (lengthOfLength)
			{
				case 2:
				{
					itemsCount = ReadValue<ushort>(source, offset: 1, out requestHint);
					break;
				}
				default: // 4
				{
					itemsCount = ReadValue<uint>(source, offset: 1, out requestHint);
					break;
				}
			}

			if (requestHint != 0)
			{
				return CollectionType.None;
			}

			source.Advance(length);
			consumed += length;
			return type;
		}

		public sealed override long DecodeArrayHeader(in SequenceReader<byte> source, out int requestHint)
		{
			var consumed = 0L;
			var type = this.DecodeArrayOrMapHeaderCore(source, ref consumed, out var header, out var itemsCount, out requestHint);
			if (requestHint != 0)
			{
				source.Rewind(consumed);
				return 0;
			}

			if (itemsCount > Int32.MaxValue)
			{
				MessagePackThrow.TooLargeArrayOrMapLength(header, source.Consumed - consumed, itemsCount);
			}

			if (!type.IsArray)
			{
				MessagePackThrow.TypeIsNotArray(header, source.Consumed - consumed);
			}

			return (int)itemsCount;
		}

		public sealed override long DecodeMapHeader(in SequenceReader<byte> source, out int requestHint)
		{
			var consumed = 0L;
			var type = this.DecodeArrayOrMapHeaderCore(source, ref consumed, out var header, out var itemsCount, out requestHint);
			if (requestHint != 0)
			{
				source.Rewind(consumed);
				return 0;
			}

			if (itemsCount > Int32.MaxValue)
			{
				MessagePackThrow.TooLargeArrayOrMapLength(header, source.Consumed - consumed, itemsCount);
			}

			if (!type.IsMap)
			{
				MessagePackThrow.TypeIsNotMap(header, source.Consumed - consumed);
			}

			return (int)itemsCount;
		}
	}
}
