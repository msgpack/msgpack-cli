// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		public sealed override CollectionType DecodeArrayOrMapHeader(ref SequenceReader<byte> source, out int itemsCount, out int requestHint)
		{
			var startOffset = source.Consumed;
			var result = this.PrivateDecodeArrayOrMapHeader(ref source, out var header, out itemsCount, out requestHint);

			if (itemsCount > Int32.MaxValue)
			{
				MessagePackThrow.TooLargeArrayOrMapLength(header, startOffset, itemsCount);
			}

			return result;
		}

		private CollectionType PrivateDecodeArrayOrMapHeader(ref SequenceReader<byte> source, out byte header, out int itemsCount, out int requestHint)
		{
			var startOffset = source.Consumed;
			var result = this.DecodeArrayOrMapHeaderCore(ref source, out header, out itemsCount, out requestHint);
			if (result == CollectionType.None)
			{
				MessagePackThrow.TypeIsNotArrayNorMap(header, startOffset);
			}

			return result;
		}

		private CollectionType DecodeArrayOrMapHeaderCore(ref SequenceReader<byte> source, out byte header, out int itemsCount, out int requestHint)
		{
			if (!source.TryRead(out header))
			{
				requestHint = 1;
				itemsCount = 0;
				return default;
			}

			requestHint = 0;

			if (header == MessagePackCode.NilValue)
			{
				itemsCount = 0;
				return CollectionType.Null;
			}

			if ((header & MessagePackCode.MinimumFixedArray) == MessagePackCode.MinimumFixedArray)
			{
				itemsCount = (int)header - MessagePackCode.MinimumFixedArray;
				return CollectionType.Array;
			}

			if ((header & MessagePackCode.MinimumFixedMap) == MessagePackCode.MinimumFixedMap)
			{
				itemsCount = (int)header - MessagePackCode.MinimumFixedMap;
				return CollectionType.Map;
			}

			int lengthOfLength;
			CollectionType type;
			switch (header)
			{
				case MessagePackCode.Array16:
				{
					lengthOfLength = 2;
					type = CollectionType.Array;
					break;
				}
				case MessagePackCode.Array32:
				{
					lengthOfLength = 4;
					type = CollectionType.Array;
					break;
				}
				case MessagePackCode.Map16:
				{
					lengthOfLength = 2;
					type = CollectionType.Map;
					break;
				}
				case MessagePackCode.Map32:
				{
					lengthOfLength = 4;
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

			switch (lengthOfLength)
			{
				case 2:
				{
					itemsCount = ReadValue<ushort>(ref source, offset: 1, out requestHint);
					break;
				}
				default: // 4
				{
					var count = ReadValue<uint>(ref source, offset: 1, out requestHint);
					if (count > OptionsDefaults.MaxMultiByteCollectionLength)
					{
						MessagePackThrow.TooLargeArrayOrMapLength(header, source.Consumed - 1, count);
					}

					itemsCount = unchecked((int)count);
					break;
				}
			}

			if (requestHint != 0)
			{
				source.Rewind(1);
				return CollectionType.None;
			}

			return type;
		}

		public sealed override int DecodeArrayHeader(ref SequenceReader<byte> source, out int requestHint)
		{
			var startOffset = source.Consumed;
			var type = this.DecodeArrayOrMapHeaderCore(ref source, out var header, out var itemsCount, out requestHint);
			if (requestHint != 0)
			{
				return 0;
			}

			if (type.IsMap)
			{
				MessagePackThrow.TypeIsNotArray(header, startOffset);
			}

			return (int)itemsCount;
		}

		public sealed override int DecodeMapHeader(ref SequenceReader<byte> source, out int requestHint)
		{
			var startOffset = source.Consumed;
			var type = this.DecodeArrayOrMapHeaderCore(ref source, out var header, out var itemsCount, out requestHint);
			if (requestHint != 0)
			{
				return 0;
			}

			if (type.IsArray)
			{
				MessagePackThrow.TypeIsNotMap(header, startOffset);
			}

			return (int)itemsCount;
		}
	}
}
