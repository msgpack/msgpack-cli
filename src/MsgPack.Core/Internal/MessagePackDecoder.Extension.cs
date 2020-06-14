// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		private uint ReadExtensionHeader(in SequenceReader<byte> source, out int consumed, out int requestHint)
		{
			if(!this.TryPeek(source, out var header))
			{
				consumed = 0;
				requestHint = 1;
				return default;
			}

			consumed = 1;

			int lengthOflength;
			switch (header)
			{
				case MessagePackCode.FixExt1:
				{
					requestHint = 0;
					return 1;
				}
				case MessagePackCode.FixExt2:
				{
					requestHint = 0;
					return 2;
				}
				case MessagePackCode.FixExt4:
				{
					requestHint = 0;
					return 4;
				}
				case MessagePackCode.FixExt8:
				{
					requestHint = 0;
					return 8;
				}
				case MessagePackCode.FixExt16:
				{
					requestHint = 0;
					return 16;
				}
				case MessagePackCode.Ext8:
				{
					lengthOflength = 1;
					break;
				}
				case MessagePackCode.Ext16:
				{
					lengthOflength = 2;
					break;
				}
				case MessagePackCode.Ext32:
				{
					lengthOflength = 4;
					break;
				}
				default:
				{
					MessagePackThrow.IsNotExtension(header, source.Consumed);
					// never
					requestHint = default;
					return default;
				}
			} // switch

			uint length;
			switch (lengthOflength)
			{
				case 1:
				{
					length = ReadByte(source, offset: 1, out requestHint);
					break;
				}
				case 2:
				{
					length = ReadValue<ushort>(source, offset: 1, out requestHint);
					break;
				}
				default:
				{
					Debug.Assert(lengthOflength == 4, $"length({lengthOflength}) == 4");
					length = ReadValue<uint>(source, offset: 1, out requestHint);
					break;
				}
			}

			if (requestHint != 0)
			{
				return default;
			}

			consumed += lengthOflength;
			return length;
		}

		public override void DecodeExtension(in SequenceReader<byte> source, out MessagePackExtensionType typeCode, out ReadOnlySequence<byte> body, out int requestHint, CancellationToken cancellationToken = default)
		{
			var bodyLength = this.ReadExtensionHeader(source, out var consumed, out requestHint);
			if (requestHint != 0)
			{
				typeCode = default;
				body = default;
				return;
			}

			if (!this.TryPeek(source, out byte typeCodeByte))
			{
				requestHint = 1;
				typeCode = default;
				body = default;
				return;
			}

			consumed++;
			typeCode = new MessagePackExtensionType(typeCodeByte);

			if (source.Remaining < consumed + bodyLength)
			{
				requestHint = (int)((consumed + bodyLength - (int)source.Remaining) & Int32.MaxValue);
				body = default;
				return;
			}

			requestHint = 0;
			body = source.Sequence.Slice(consumed, bodyLength);
			source.Advance(consumed + bodyLength);
		}
	}
}
