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
		private uint ReadExtensionHeader(ref SequenceReader<byte> source, out int consumed, out int requestHint)
		{
			if(!source.TryPeek(out var header))
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
					length = ReadByte(ref source, offset: 1, out requestHint);
					break;
				}
				case 2:
				{
					length = ReadValue<ushort>(ref source, offset: 1, out requestHint);
					break;
				}
				default:
				{
					Debug.Assert(lengthOflength == 4, $"length({lengthOflength}) == 4");
					length = ReadValue<uint>(ref source, offset: 1, out requestHint);
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

		public override void DecodeExtension(ref SequenceReader<byte> source, out ExtensionTypeObject result, out int requestHint, CancellationToken cancellationToken = default)
		{
			var bodyLength = this.ReadExtensionHeader(ref source, out var consumed, out requestHint);
			if (requestHint != 0)
			{
				result = default;
				return;
			}

			if (!source.TryPeek(out byte typeCode))
			{
				requestHint = 1;
				result = default;
				return;
			}

			consumed++;

			if (source.Remaining < consumed + bodyLength)
			{
				requestHint = (int)((consumed + bodyLength - (int)source.Remaining) & Int32.MaxValue);
				result = default;
				return;
			}

			requestHint = 0;
			result = new ExtensionTypeObject(new ExtensionType(typeCode), source.Sequence.Slice(consumed, bodyLength));
			source.Advance(consumed + bodyLength);
		}
	}
}
