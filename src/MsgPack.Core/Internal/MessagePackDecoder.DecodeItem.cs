// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		public sealed override bool DecodeItem(in SequenceReader<byte> source, out DecodeItemResult<MessagePackExtensionType> result, CancellationToken cancellationToken = default)
		{
			var elementType = this.ReadHeader(source, out var consumed, out var valueOrLength, out var requestHint);
			if (requestHint != 0)
			{
				result = DecodeItemResult<MessagePackExtensionType>.InsufficientInput(requestHint);
				return false;
			}

			requestHint = 0;

			switch (elementType)
			{
				case ElementType.True:
				{
					result = DecodeItemResult<MessagePackExtensionType>.True();
					break;
				}
				case ElementType.False:
				{
					result = DecodeItemResult<MessagePackExtensionType>.False();
					break;
				}
				case ElementType.Null:
				{
					result = DecodeItemResult<MessagePackExtensionType>.Null();
					break;
				}
				case ElementType.Int32:
				case ElementType.Single:
				{
					var buffer = new byte[sizeof(int)];
					var span = MemoryMarshal.Cast<byte, int>(buffer);
					span[0] = (int)valueOrLength;
					result = DecodeItemResult<MessagePackExtensionType>.ScalarOrSequence(elementType, buffer);
					break;
				}
				case ElementType.Array:
				case ElementType.Map:
				{
					this.DecodeArrayOrMap(source, out var iterator);
					result = DecodeItemResult<MessagePackExtensionType>.CollectionHeader(elementType, this.CreateIterator((uint)valueOrLength), valueOrLength);
					break;
				}
				case ElementType.Int64:
				case ElementType.UInt64:
				case ElementType.Double:
				{
					var buffer = new byte[sizeof(long)];
					var span = MemoryMarshal.Cast<byte, long>(buffer);
					span[0] = valueOrLength;
					result = DecodeItemResult<MessagePackExtensionType>.ScalarOrSequence(elementType, buffer);
					break;
				}
				case ElementType.String:
				case ElementType.Binary:
				{
					if (source.Remaining < valueOrLength + consumed)
					{
						result =
							DecodeItemResult < MessagePackExtensionType >.InsufficientInput(
								(int)((valueOrLength + consumed - source.Remaining) & Int32.MaxValue)
							);
						return false;
					}

					result = DecodeItemResult<MessagePackExtensionType>.ScalarOrSequence(elementType, source.Sequence.Slice(source.Consumed + consumed, valueOrLength));
					consumed += valueOrLength;
					break;
				}
				default:
				{
					Debug.Assert(elementType == ElementType.Extension, $"elementType({elementType}, 0x{elementType:X8}) == ElementType.Extension");

					if(source.Remaining < valueOrLength + consumed)
					{
						result =
							DecodeItemResult<MessagePackExtensionType>.InsufficientInput(
								(int)((valueOrLength + consumed - source.Remaining) & Int32.MaxValue)
							);
						return false;
					}

					var extensionSlice = source.Sequence.Slice(source.Consumed + consumed - 1);
					var typeCode = new MessagePackExtensionType(extensionSlice.FirstSpan[0]);
					var body = extensionSlice.Slice(1, valueOrLength);

					result = DecodeItemResult<MessagePackExtensionType>.ExtensionTypeObject(typeCode, body);
					consumed += valueOrLength;
					break;
				}
			}

			source.Advance(consumed);
			return true;
		}

		private ElementType ReadHeader(in SequenceReader<byte> source, out long consumed, out long valueOrLength, out int requestHint)
		{
			if (!this.TryPeek(source, out var header))
			{
				requestHint = 1;
				valueOrLength = default;
				consumed = 0;
				return default;
			}

			consumed = 1;
			ElementType result;
			requestHint = 0;

			if (header < 128)
			{
				valueOrLength = header;
				result = ElementType.Int32;
			}
			else if (header >= 0xE0)
			{
				valueOrLength = unchecked((sbyte)header);
				result = ElementType.Int32;
			}
			else if (header >= MessagePackCode.MinimumFixedRaw && header <= MessagePackCode.MaximumFixedRaw)
			{
				valueOrLength = header - MessagePackCode.MinimumFixedRaw;
				result = ElementType.String;
			}
			else if (header >= MessagePackCode.MinimumFixedArray && header <= MessagePackCode.MaximumFixedArray)
			{
				valueOrLength = header - MessagePackCode.MinimumFixedArray;
				result = ElementType.Array;
			}
			else if (header >= MessagePackCode.MinimumFixedMap && header <= MessagePackCode.MaximumFixedMap)
			{
				valueOrLength = header - MessagePackCode.MinimumFixedMap;
				result = ElementType.Map;
			}
			else
			{
				switch (header)
				{
					case MessagePackCode.SignedInt8:
					{
						valueOrLength = ReadSByte(source, offset: 1, out requestHint);
						consumed += sizeof(sbyte);
						result = ElementType.Int32;
						break;
					}
					case MessagePackCode.SignedInt16:
					{
						valueOrLength = ReadValue<short>(source, offset: 1, out requestHint);
						consumed += sizeof(short);
						result = ElementType.Int32;
						break;
					}
					case MessagePackCode.SignedInt32:
					{
						valueOrLength = ReadValue<int>(source, offset: 1, out requestHint);
						consumed += sizeof(int);
						result = ElementType.Int32;
						break;
					}
					case MessagePackCode.SignedInt64:
					{
						valueOrLength = ReadValue<long>(source, offset: 1, out requestHint);
						consumed += sizeof(long);
						result = ElementType.Int64;
						break;
					}
					case MessagePackCode.UnsignedInt8:
					{
						valueOrLength = ReadByte(source, offset: 1, out requestHint);
						consumed += sizeof(byte);
						result = ElementType.Int32;
						break;
					}
					case MessagePackCode.UnsignedInt16:
					{
						valueOrLength = ReadValue<ushort>(source, offset: 1, out requestHint);
						consumed += sizeof(ushort);
						result = ElementType.Int32;
						break;
					}
					case MessagePackCode.UnsignedInt32:
					{
						valueOrLength = ReadValue<uint>(source, offset: 1, out requestHint);
						consumed += sizeof(uint);
						result = ElementType.Int64;
						break;
					}
					case MessagePackCode.UnsignedInt64:
					{
						valueOrLength = unchecked((long)ReadValue<ulong>(source, offset: 1, out requestHint));
						consumed += sizeof(ulong);
						result = ElementType.UInt64;
						break;
					}
					case MessagePackCode.Real32:
					{
						valueOrLength = ReadValue<int>(source, offset: 1, out requestHint);
						consumed += sizeof(int);
						result = ElementType.Single;
						break;
					}
					case MessagePackCode.Real64:
					{
						valueOrLength = ReadValue<long>(source, offset: 1, out requestHint);
						consumed += sizeof(long);
						result = ElementType.Double;
						break;
					}
					case MessagePackCode.NilValue:
					{
						valueOrLength = 0;
						result = ElementType.Null;
						break;
					}
					case MessagePackCode.TrueValue:
					{
						valueOrLength = 1;
						result = ElementType.True;
						break;
					}
					case MessagePackCode.FalseValue:
					{
						valueOrLength = 0;
						result = ElementType.False;
						break;
					}
					case MessagePackCode.Array16:
					{
						valueOrLength = ReadValue<ushort>(source, offset: 1, out requestHint);
						consumed += sizeof(ushort);
						result = ElementType.Array;
						break;
					}
					case MessagePackCode.Array32:
					{
						valueOrLength = ReadValue<uint>(source, offset: 1, out requestHint);
						consumed += sizeof(uint);
						result = ElementType.Array;
						break;
					}
					case MessagePackCode.Map16:
					{
						valueOrLength = ReadValue<ushort>(source, offset: 1, out requestHint);
						consumed += sizeof(ushort);
						result = ElementType.Map;
						break;
					}
					case MessagePackCode.Map32:
					{
						valueOrLength = ReadValue<uint>(source, offset: 1, out requestHint);
						consumed += sizeof(uint);
						result = ElementType.Map;
						break;
					}
					case MessagePackCode.FixExt1:
					case MessagePackCode.FixExt2:
					case MessagePackCode.FixExt4:
					case MessagePackCode.FixExt8:
					case MessagePackCode.FixExt16:
					case MessagePackCode.Ext8:
					case MessagePackCode.Ext16:
					case MessagePackCode.Ext32:
					{
						result = ReadExtentionItem(header, source, offset: 1, ref consumed, out valueOrLength, out requestHint);
						break;
					}
					default:
					{
						Debug.Assert(header == 0xC1, $"header(0x{header:X2}) == 0xC1");
						valueOrLength = 0;
						result = ElementType.None;
						break;
					}
				} // switch
			} // else

			return result;
		}

		private static ElementType ReadExtentionItem(byte header, SequenceReader<byte> source, int offset, ref long consumed, out long valueOrLength, out int requestHint)
		{
			consumed++;
			requestHint = 0;

			switch (header)
			{
				case MessagePackCode.FixExt1:
				{
					valueOrLength = 1;
					break;
				}
				case MessagePackCode.FixExt2:
				{
					valueOrLength = 2;
					break;
				}
				case MessagePackCode.FixExt4:
				{
					valueOrLength = 4;
					break;
				}
				case MessagePackCode.FixExt8:
				{
					valueOrLength = 8;
					break;
				}
				case MessagePackCode.FixExt16:
				{
					valueOrLength = 16;
					break;
				}
				default:
				{
					_ = ReadByte(source, offset, out requestHint);
					if (requestHint != 0)
					{
						valueOrLength = default;
						return default;
					}

					consumed++;
					offset++;

					switch (header)
					{
						case MessagePackCode.Ext8:
						{
							valueOrLength = ReadByte(source, offset, out requestHint);
							consumed++;
							break;
						}
						case MessagePackCode.Ext16:
						{
							valueOrLength = ReadValue<ushort>(source, offset, out requestHint);
							consumed += sizeof(ushort);
							break;
						}
						default:
						{
							Debug.Assert(header == MessagePackCode.Ext32, $"header(0x{header:X2}) == MessagePackCode.Ext32(0x{MessagePackCode.Ext32:X2})");
							valueOrLength = ReadValue<uint>(source, offset + 1, out requestHint);
							consumed += sizeof(uint);
							break;
						}
					}

					break;
				}
			}

			if (requestHint != 0)
			{
				return default;
			}

			return ElementType.Extension;
		}
	}
}
