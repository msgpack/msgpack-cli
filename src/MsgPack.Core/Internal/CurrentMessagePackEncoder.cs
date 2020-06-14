// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	/// <summary>
	///		MessagePack encoder.
	/// </summary>
	internal sealed class CurrentMessagePackEncoder : MessagePackEncoder
	{
		public CurrentMessagePackEncoder(MessagePackEncoderOptions options)
			: base(options) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override int EncodeStringHeader(uint length, Span<byte> buffer)
		{
			if (length < 32)
			{
				buffer[0] = unchecked((byte)(MessagePackCode.MinimumFixedRaw | length));
				return 1;
			}
			else if (length <= Byte.MaxValue)
			{
				buffer[1] = unchecked((byte)length);
				buffer[0] = MessagePackCode.Str8;
				return 2;
			}
			else if (length <= UInt16.MaxValue)
			{
				buffer[0] = MessagePackCode.Str16;
				BinaryPrimitives.WriteUInt16BigEndian(buffer, unchecked((ushort)length));
				return sizeof(ushort) + 1;
			}
			else
			{
				buffer[0] = MessagePackCode.Str32;
				BinaryPrimitives.WriteUInt32BigEndian(buffer, length);
				return sizeof(uint) + 1;
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override int EncodeBinaryHeader(uint length, Span<byte> buffer)
		{
			if (length <= Byte.MaxValue)
			{
				buffer[1] = unchecked((byte)length);
				buffer[0] = MessagePackCode.Bin8;
				return 2;
			}
			else if (length <= UInt16.MaxValue)
			{
				buffer[0] = MessagePackCode.Bin16;
				BinaryPrimitives.WriteUInt16BigEndian(buffer, unchecked((ushort)length));
				return sizeof(ushort) + 1;
			}
			else
			{
				buffer[0] = MessagePackCode.Bin32;
				BinaryPrimitives.WriteUInt32BigEndian(buffer, length);
				return sizeof(uint) + 1;
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeExtension(MessagePackExtensionType typeCode, ReadOnlySpan<byte> serializedValue, IBufferWriter<byte> buffer)
		{
			EncodeExtensionHeader(typeCode, unchecked((uint)serializedValue.Length), buffer);
			buffer.Write(serializedValue);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeExtension(MessagePackExtensionType typeCode, in ReadOnlySequence<byte> serializedValue, IBufferWriter<byte> buffer)
		{
			EncodeExtensionHeader(typeCode, unchecked((uint)serializedValue.Length), buffer);
			this.WriteRaw(serializedValue, buffer);
		}

		private static void EncodeExtensionHeader(MessagePackExtensionType typeCode, uint serializedValueLength, IBufferWriter<byte> buffer)
		{
			switch (serializedValueLength)
			{
				case 1:
				{
					var span = buffer.GetSpan(1);
					span[0] = MessagePackCode.FixExt1;
					buffer.Advance(1);
					break;
				}
				case 2:
				{
					var span = buffer.GetSpan(1);
					span[0] = MessagePackCode.FixExt2;
					buffer.Advance(1);
					break;
				}
				case 4:
				{
					var span = buffer.GetSpan(1);
					span[0] = MessagePackCode.FixExt4;
					buffer.Advance(1);
					break;
				}
				case 8:
				{
					var span = buffer.GetSpan(1);
					span[0] = MessagePackCode.FixExt8;
					buffer.Advance(1);
					break;
				}
				case 16:
				{
					var span = buffer.GetSpan(1);
					span[0] = MessagePackCode.FixExt16;
					buffer.Advance(1);
					break;
				}
				default:
				{
					if (serializedValueLength <= Byte.MaxValue)
					{
						var span = buffer.GetSpan(2);
						span[0] = MessagePackCode.Ext8;
						span[1] = unchecked((byte)serializedValueLength);
						buffer.Advance(2);
					}
					else if (serializedValueLength <= UInt16.MaxValue)
					{
						var span = buffer.GetSpan(sizeof(ushort) + 1);
						span[0] = MessagePackCode.Ext16;
						span = span.Slice(1);
						BinaryPrimitives.WriteUInt16BigEndian(span, unchecked((ushort)serializedValueLength));
						buffer.Advance(sizeof(ushort) + 1);
					}
					else
					{
						var span = buffer.GetSpan(sizeof(uint) + 1);
						span[0] = MessagePackCode.Ext32;
						span = span.Slice(1);
						BinaryPrimitives.WriteUInt32BigEndian(span, serializedValueLength);
						buffer.Advance(sizeof(uint) + 1);
					}

					break;
				}
			} // switch

			// type code
			{
				var span = buffer.GetSpan(1);
				span[0] = unchecked(typeCode.TypeCode);
				buffer.Advance(1);
			}
		}

	}
}
