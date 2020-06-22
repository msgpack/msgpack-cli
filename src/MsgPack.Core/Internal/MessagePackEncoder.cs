// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MsgPack.Internal
{
	/// <summary>
	///		Common encoder implementation between legacy and current MessagePack format enconders.
	/// </summary>
	public abstract partial class MessagePackEncoder : FormatEncoder
	{
#warning TODO: Can devirt? If not, change to Create(MessagePackEncoderOptions)
		public static MessagePackEncoder CreateLegacy(MessagePackEncoderOptions options) => new LegacyMessagePackEncoder(options);
		public static MessagePackEncoder CreateCurrent(MessagePackEncoderOptions options) => new CurrentMessagePackEncoder(options);

		public new MessagePackEncoderOptions Options => (base.Options as MessagePackEncoderOptions)!;

		protected MessagePackEncoder(MessagePackEncoderOptions options)
			: base(options) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeRawString(ReadOnlySpan<byte> rawString, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
			=> Ensure.NotNull(buffer).Write(rawString);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeSingle(float value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(sizeof(float) + 1);

			span[0] = MessagePackCode.Real32;
			span = span.Slice(1);
			BinaryPrimitives.WriteInt32BigEndian(span, BitConverter.SingleToInt32Bits(value));
			buffer.Advance(sizeof(float) + 1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeDouble(double value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(sizeof(double) + 1);

			span[0] = MessagePackCode.Real64;
			span = span.Slice(1);
			BinaryPrimitives.WriteInt64BigEndian(span, BitConverter.DoubleToInt64Bits(value));
			buffer.Advance(sizeof(double) + 1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeBoolean(bool value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(1);
			span[0] = unchecked((byte)(value ? MessagePackCode.TrueValue : MessagePackCode.FalseValue));
			buffer.Advance(1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);
			collectionContext.IncrementDepth();

			if (length < 16)
			{
				var span = buffer.GetSpan(1);
				span[0] = unchecked((byte)(MessagePackCode.MinimumFixedArray | length));
				buffer.Advance(1);
			}
			else if (length < 16)
			{
				var span = buffer.GetSpan(sizeof(ushort) + 1);
				span[0] = MessagePackCode.Array16;
				span = span.Slice(1);
				BinaryPrimitives.WriteUInt16BigEndian(span, unchecked((ushort)length));
				buffer.Advance(sizeof(ushort) + 1);
			}
			else
			{
				var span = buffer.GetSpan(sizeof(uint) + 1);
				span[0] = MessagePackCode.Array32;
				span = span.Slice(1);
				BinaryPrimitives.WriteInt32BigEndian(span, length);
				buffer.Advance(sizeof(uint) + 1);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayItemStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayItemEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			if (length < 16)
			{
				var span = buffer.GetSpan(1);
				span[0] = unchecked((byte)(MessagePackCode.MinimumFixedMap | length));
				buffer.Advance(1);
			}
			else if (length < 16)
			{
				var span = buffer.GetSpan(sizeof(ushort) + 1);
				span[0] = MessagePackCode.Map16;
				span = span.Slice(1);
				BinaryPrimitives.WriteUInt16BigEndian(span, unchecked((ushort)length));
				buffer.Advance(sizeof(ushort) + 1);
			}
			else
			{
				var span = buffer.GetSpan(sizeof(uint) + 1);
				span[0] = MessagePackCode.Map32;
				span = span.Slice(1);
				BinaryPrimitives.WriteInt32BigEndian(span, length);
				buffer.Advance(sizeof(uint) + 1);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapKeyStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapKeyEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapValueStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapValueEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeNull(IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(1);
			span[0] = MessagePackCode.NilValue;
			buffer.Advance(1);
		}

		private void EncodeHugeString2Path(ReadOnlySpan<char> value, IBufferWriter<byte> buffer, Encoding encoding)
		{
			// Huge path, we cannot use rollback because it requires multiple span from buffer.
			int actualLength;
			try
			{
				actualLength = encoding.GetByteCount(value);
			}
			catch(OverflowException ex)
			{
				Throw.TooLargeByteLength(ex, encoding.EncodingName);
				return;
			}

			var span = buffer.GetSpan(5);
			var headerLength = this.EncodeStringHeader(unchecked((uint)actualLength), span);
			buffer.Advance(headerLength);
			encoding.GetBytes(value, buffer);
		}

		private void EncodeHugeString2Path(in ReadOnlySequence<char> value, IBufferWriter<byte> buffer, Encoding encoding)
		{
			// Huge path, we cannot use rollback because it requires multiple span from buffer.
			var actualLength = 0L;
			var reader = new SequenceReader<char>(value);
			while (!reader.End)
			{
				actualLength += encoding.GetByteCount(reader.UnreadSpan);
				reader.Advance(reader.UnreadSpan.Length);
			}

			if (actualLength > UInt32.MaxValue)
			{
				Throw.TooLargeByteLength(actualLength, encoding.EncodingName);
			}

			var span = buffer.GetSpan(5);
			var headerLength = this.EncodeStringHeader(unchecked((uint)actualLength), span);
			buffer.Advance(headerLength);
			encoding.GetBytes(value, buffer);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeString(ReadOnlySpan<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = Ensure.NotNull(buffer);
			
			var span = buffer.GetSpan(5);
			var used = this.EncodeStringHeader(unchecked((uint)encodedValue.Length), span);
			buffer.Advance(used);
			this.WriteRaw(encodedValue, buffer, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private void EncodeStringHeader(uint length, Memory<byte> memory)
			=> this.EncodeStringHeader(length, memory.Span);

		protected abstract int EncodeStringHeader(uint length, Span<byte> buffer);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeString(in ReadOnlySequence<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			var span = buffer.GetSpan(5);
			var used = this.EncodeStringHeader(unchecked((uint)encodedValue.Length), span);
			buffer.Advance(used);
			this.WriteRaw(encodedValue, buffer, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeBinary(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			var span = buffer.GetSpan(5);
			var used = this.EncodeBinaryHeader(unchecked((uint)value.Length), span);
			buffer.Advance(used);
			this.WriteRaw(value, buffer, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeBinary(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken)
		{
			var span = buffer.GetSpan(5);
			var used = this.EncodeBinaryHeader(unchecked((uint)value.Length), span);
			buffer.Advance(used);
			this.WriteRaw(value, buffer, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private int EncodeBinaryHeader(uint length, Memory<byte> memory)
			=> this.EncodeBinaryHeader(length, memory.Span);

		protected abstract int EncodeBinaryHeader(uint length, Span<byte> buffer);
	}
}
