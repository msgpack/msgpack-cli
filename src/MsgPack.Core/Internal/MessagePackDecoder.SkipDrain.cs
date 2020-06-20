// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void Skip(ref SequenceReader<byte> source, in CollectionContext collectionContext, out int requestHint, CancellationToken cancellationToken = default)
			=> this.Drain(ref source, collectionContext, itemsCount: 1, out requestHint, cancellationToken);

#warning TODO: Remove ref consumed
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void Drain(ref SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, out int requestHint, CancellationToken cancellationToken = default)
		{
			var consumed = 0L;
			if (!this.SkipItems(ref source, collectionContext, itemsCount, ref consumed, out requestHint, cancellationToken))
			{
				source.Rewind(consumed);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static bool SkipLength(ref SequenceReader<byte> source, long length, ref long consumed, out int requestHint)
		{
			if (source.Remaining < length)
			{
				requestHint = (int)((length - source.Remaining) & Int32.MaxValue);
				return false;
			}

			requestHint = 0;
			source.Advance(length);
			consumed += length;
			return true;
		}

		private bool SkipArray(ref SequenceReader<byte> source, in CollectionContext collectionContext, ref long consumed, out int requestHint, CancellationToken cancellationToken = default)
		{
			collectionContext.IncrementDepth();

			var initialConsumed = consumed;
			var type = this.DecodeArrayOrMapHeaderCore(ref source, out var header, out var arrayLength, out requestHint);
			if (requestHint != 0)
			{
				return false;
			}

			if (!type.IsArray)
			{
				MessagePackThrow.TypeIsNotArray(header, initialConsumed);
			}

			consumed = source.Consumed - initialConsumed;

			if (!this.SkipItems(ref source, collectionContext, arrayLength, ref consumed, out requestHint, cancellationToken))
			{
				return false;
			}

			collectionContext.DecrementDepth();
			return true;
		}

		private bool SkipMap(ref SequenceReader<byte> source, in CollectionContext collectionContext, ref long consumed, out int requestHint, CancellationToken cancellationToken = default)
		{
			collectionContext.IncrementDepth();

			var initialConsumed = consumed;
			var type = this.DecodeArrayOrMapHeaderCore(ref source, out var header, out var mapCount, out requestHint);
			if (requestHint != 0)
			{
				return false;
			}

			if (!type.IsMap)
			{
				MessagePackThrow.TypeIsNotMap(header, initialConsumed);
			}

			consumed = source.Consumed - initialConsumed;

			if (!this.SkipItems(ref source, collectionContext, mapCount * 2, ref consumed, out requestHint, cancellationToken))
			{
				return false;
			}

			collectionContext.DecrementDepth();
			return true;
		}

		private bool SkipItems(ref SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, ref long consumed, out int requestHint, CancellationToken cancellationToken = default)
		{
			while (itemsCount > 0)
			{
				if (!source.TryPeek(out var header))
				{
					requestHint = 1;
					return false;
				}

				consumed++;

				var length = 0L;

				if (header >= MessagePackCode.MinimumFixedArray && header <= MessagePackCode.MaximumFixedArray)
				{
					if (!this.SkipArray(ref source, collectionContext, ref consumed, out requestHint, cancellationToken))
					{
						return false;
					}
				}
				else if (header >= MessagePackCode.MinimumFixedMap && header <= MessagePackCode.MaximumFixedMap)
				{
					if (!this.SkipMap(ref source, collectionContext, ref consumed, out requestHint, cancellationToken))
					{
						return false;
					}
				}
				else if (header >= MessagePackCode.MinimumFixedRaw && header <= MessagePackCode.MaximumFixedRaw)
				{
					length = header - MessagePackCode.MinimumFixedRaw;
				}
				else
				{
					switch (header)
					{
						case MessagePackCode.SignedInt8:
						case MessagePackCode.UnsignedInt8:
						{
							length = 1;
							break;
						}
						case MessagePackCode.SignedInt16:
						case MessagePackCode.UnsignedInt16:
						case MessagePackCode.Real32:
						case MessagePackCode.SignedInt32:
						case MessagePackCode.UnsignedInt32:
						{
							length = 4;
							break;
						}
						case MessagePackCode.Real64:
						case MessagePackCode.SignedInt64:
						case MessagePackCode.UnsignedInt64:
						{
							length = 8;
							break;
						}
						case MessagePackCode.Bin8:
						{
							if (!source.TryRead(out var b))
							{
								requestHint = 1;
								return false;
							}

							length = b;
							consumed++;
							break;
						}
						case MessagePackCode.Bin16:
						case MessagePackCode.Raw16:
						{
							length = ReadValue<ushort>(ref source, offset: 0, out requestHint);
							if (requestHint != 0)
							{
								source.Rewind(consumed);
								return false;
							}

							consumed += sizeof(ushort);
							break;
						}
						case MessagePackCode.Bin32:
						case MessagePackCode.Raw32:
						{
							length = ReadValue<uint>(ref source, offset: 0, out requestHint);
							if (requestHint != 0)
							{
								source.Rewind(consumed);
								return false;
							}

							consumed += sizeof(int);
							break;
						}
						case MessagePackCode.FixExt1:
						{
							length = 2;
							break;
						}
						case MessagePackCode.FixExt2:
						{
							length = 3;
							break;
						}
						case MessagePackCode.FixExt4:
						{
							length = 5;
							break;
						}
						case MessagePackCode.FixExt8:
						{
							length = 9;
							break;
						}
						case MessagePackCode.FixExt16:
						{
							length = 17;
							break;
						}
						case MessagePackCode.Ext8:
						{
							if (!source.TryRead(out var b))
							{
								requestHint = 1;
								return false;
							}

							length = b + 1;
							consumed++;
							break;
						}
						case MessagePackCode.Ext16:
						{
							length = ReadValue<ushort>(ref source, offset: 0, out requestHint);
							if (requestHint != 0)
							{
								source.Rewind(consumed);
								return false;
							}

							consumed += sizeof(ushort) + 1;
							break;
						}
						case MessagePackCode.Ext32:
						{
							length = ReadValue<int>(ref source, offset: 0, out requestHint);
							if (requestHint != 0)
							{
								source.Rewind(consumed);
								return false;
							}

							consumed += sizeof(int) + 1;
							break;
						}
						case MessagePackCode.Array16:
						case MessagePackCode.Array32:
						{
							if (!this.SkipArray(ref source, collectionContext, ref consumed, out requestHint, cancellationToken))
							{
								return false;
							}
							break;
						}
						case MessagePackCode.Map16:
						case MessagePackCode.Map32:
						{
							if (!this.SkipMap(ref source, collectionContext, ref consumed, out requestHint, cancellationToken))
							{
								return false;
							}
							break;
						}
					}
				}

				if (length > 0 && !SkipLength(ref source, length, ref consumed, out requestHint))
				{
					return false;
				}

				itemsCount--;
			} // while (itemsCount > 0)

			requestHint = 0;
			return true;
		}
	}
}
