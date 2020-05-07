// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MsgPack.Internal
{
	public sealed partial class MessagePackDecoder : Decoder
	{
		private static readonly FormatFeatures MessagePackFormatFeatures =
			new FormatFeaturesBuilder
			{
				IsContextful = false,
				CanCountCollectionItems = true,
				CanSpecifyStringEncoding = true
			}.Build();

		public MessagePackDecoder(MessagePackDecoderOptions options)
			: base(options, MessagePackFormatFeatures)
		{
			this._detectCollectionEnds = this.DetectCollectionEnds;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override bool DecodeBoolean(in SequenceReader<byte> source, out int requestHint)
		{
			if (!this.TryPeek(source, out var header))
			{
				requestHint = 1;
				return false;
			}

			requestHint = 0;
			switch (header)
			{
				case MessagePackCode.TrueValue:
				{
					source.Advance(1);
					return true;
				}
				case MessagePackCode.FalseValue:
				{
					source.Advance(1);
					return false;
				}
				default:
				{
					MessagePackThrow.IsNotType(header, source.Consumed, typeof(bool));
					// Never reached.
					return false;
				}
			}
		}

		private bool TryReadNull(in SequenceReader<byte> source)
		{
			if (this.TryPeek(source, out var b) && b == MessagePackCode.NilValue)
			{
				source.Advance(1);
				return true;
			}

			return false;
		}

		private static void ParseNumberHeader(byte header, in SequenceReader<byte> source, Type type, out int length, out NumberKind kind)
		{
			// 0xD0-D3 -- SignedIntN
			// 1101-0000 -> 1101-0011
			// 0xCC-CF -- UnsignedIntN
			// 1100-1100 -> 1100-1111
			// 0XCA-CB -- RealN
			// 1100-1010 -> 1100-1011

			if ((header & 0xD3) == header)
			{
				// SignedIntN
				length = (int)Math.Pow(2, (header & 0x3)) + 1;
				kind = NumberKind.Signed;
			}
			else if ((header & 0xCC) == header)
			{
				// UnsignedIntN
				length = (int)Math.Pow(2, (header & 0x3)) + 1;
				kind = NumberKind.Unsigned;
			}
			if (header == MessagePackCode.Real64)
			{
				length = 9;
				kind = NumberKind.Double;
			}
			else if (header == MessagePackCode.Real32)
			{
				length = 5;
				kind = NumberKind.Single;
			}
			else
			{
				MessagePackThrow.IsNotNumber(header, source.Consumed, type);
				length = default;
				kind = NumberKind.Unknown;
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static byte ReadByte(in SequenceReader<byte> source, int offset, out int requestHint)
		{
			if (source.UnreadSpan.Length > offset + 1)
			{
				var result = source.UnreadSpan[offset];
				source.Advance(offset + 1);
				requestHint = 0;
				return result;
			}

			return ReadByteMultiSegment(source, offset, out requestHint);
		}

		private static byte ReadByteMultiSegment(in SequenceReader<byte> source, int offset, out int requestHint)
		{
			if (source.Remaining < offset + 1)
			{
				requestHint = 1;
				return default;
			}

			Span<byte> buffer = stackalloc byte[offset + 1];
			source.TryCopyTo(buffer);
			source.Advance(offset + 1);
			requestHint = 0;
			return buffer[offset];
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static sbyte ReadSByte(in SequenceReader<byte> source, int offset, out int requestHint)
			=> unchecked((sbyte)ReadByte(source, offset, out requestHint));

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static unsafe T ReadValue<T>(in SequenceReader<byte> source, int offset, out int requestHint)
			where T : unmanaged
		{
			if (source.UnreadSpan.Length > offset + sizeof(T))
			{
				var result = Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(source.UnreadSpan.Slice(offset)));
				source.Advance(offset + sizeof(T));
				requestHint = 0;
				return result;
			}

			return ReadMultiSegment<T>(source, offset, out requestHint);
		}

		private static unsafe T ReadMultiSegment<T>(in SequenceReader<byte> source, int offset, out int requestHint)
				where T : unmanaged
		{
			if (source.Remaining < offset + sizeof(T))
			{
				requestHint = offset + sizeof(T) - (int)source.Remaining;
				return default;
			}

			Span<byte> buffer = stackalloc byte[offset + sizeof(T)];
			source.TryCopyTo(buffer);
			source.Advance(offset + sizeof(T));
			requestHint = 0;
			return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(buffer.Slice(offset)));
		}

		private enum NumberKind
		{
			Unknown = 0,
			Unsigned = 0x1,
			Signed = 0x2,
			Half = 0x11,
			Single = 0x12,
			Double = 0x13,
			RealBitMask = 0x10
		}
	}
}
