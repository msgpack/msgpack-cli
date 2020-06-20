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
	///		MessagePack encoder for legacy format which uses raw type instead of str type and does not support ext type.
	/// </summary>
	internal sealed class LegacyMessagePackEncoder : MessagePackEncoder
	{
		private static readonly LegacyMessagePackEncoder DefaultInstance = new LegacyMessagePackEncoder(MessagePackEncoderOptions.Default);

		public LegacyMessagePackEncoder(MessagePackEncoderOptions options)
			: base(options) { }

		internal static byte[] InternalEncodeString(string value)
		{
			var arrayBuffer = new ArrayBufferWriter<byte>();
			DefaultInstance.EncodeString(value, arrayBuffer);
			return arrayBuffer.WrittenMemory.ToArray();
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override int EncodeStringHeader(uint length, Span<byte> buffer)
		{
			if (length < 32)
			{
				buffer[0] = unchecked((byte)(MessagePackCode.MinimumFixedRaw | length));
				return 1;
			}
			else if (length <= UInt16.MaxValue)
			{
				buffer[0] = MessagePackCode.Raw16;
				BinaryPrimitives.WriteUInt16BigEndian(buffer, unchecked((ushort)length));
				return sizeof(ushort) + 1;
			}
			else
			{
				buffer[0] = MessagePackCode.Raw32;
				BinaryPrimitives.WriteUInt32BigEndian(buffer, length);
				return sizeof(uint) + 1;
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected sealed override int EncodeBinaryHeader(uint length, Span<byte> buffer)
			=> this.EncodeStringHeader(length, buffer);
	}
}
