// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		An encoder for JSON format.
	/// </summary>
	public sealed class JsonEncoder : Encoder<NullExtensionType>
	{
		private readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> _singleInfinityFormatter;
		private readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> _singleNanFormatter;
		private readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> _doubleInfinityFormatter;
		private readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> _doubleNanFormatter;
		private readonly ReadOnlyMemory<byte> _indentChars;
		private readonly ReadOnlyMemory<byte> _newLineChars;
		private readonly ReadOnlyMemory<byte> _escapeTargetChars1Byte;
		private readonly ReadOnlyMemory<ushort> _escapeTargetChars2Byte;
		private readonly ReadOnlyMemory<int> _escapeTargetChars4Byte;
		private readonly bool _isPrettyPrint;
		private readonly JsonEncoderOptions _options;

		public JsonEncoder(JsonEncoderOptions options)
			: base(options)
		{
			this._options = options;
			this._singleInfinityFormatter = options.SingleInfinityFormatter;
			this._singleNanFormatter = options.SingleNaNFormatter;
			this._doubleInfinityFormatter = options.DoubleInfinityFormatter;
			this._doubleNanFormatter = options.DoubleNaNFormatter;
			this._indentChars = options.IndentChars;
			this._newLineChars = options.NewLineChars;
			this._isPrettyPrint = options.IsPrettyPrint;
			this._escapeTargetChars1Byte = options.EscapeTargetChars1Byte;
			this._escapeTargetChars2Byte = options.EscapeTargetChars2Byte;
			this._escapeTargetChars4Byte = options.EscapeTargetChars4Byte;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static int WriteNull(Span<byte> span)
		{
			if (span.Length < 4)
			{
				return -1;
			}

			JsonTokens.Null.CopyTo(span);
			return 4;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeInt32(int value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var span = buffer.GetSpan();
			while (true)
			{
				if (!Utf8Formatter.TryFormat(value, span, out var used))
				{
					span = buffer.GetSpan(span.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeUInt32(uint value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var span = buffer.GetSpan();
			while (true)
			{
				if (!Utf8Formatter.TryFormat(value, span, out var used))
				{
					span = buffer.GetSpan(span.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeInt64(long value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var span = buffer.GetSpan();
			while (true)
			{
				if (!Utf8Formatter.TryFormat(value, span, out var used))
				{
					span = buffer.GetSpan(span.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeUInt64(ulong value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var span = buffer.GetSpan();
			while (true)
			{
				if (!Utf8Formatter.TryFormat(value, span, out var used))
				{
					span = buffer.GetSpan(span.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeSingle(float value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var formatter =
				Single.IsNaN(value) ?
				this._singleNanFormatter :
				Single.IsInfinity(value) ?
				this._singleInfinityFormatter :
				JsonFormatter.Format;
			formatter(value, buffer, this._options);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeDouble(double value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			var formatter =
				Double.IsNaN(value) ?
				this._doubleNanFormatter :
				Double.IsInfinity(value) ?
				this._doubleInfinityFormatter :
				JsonFormatter.Format;
			formatter(value, buffer, this._options);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeBoolean(bool value, IBufferWriter<byte> buffer)
		{
			buffer = EnsureNotNull(buffer);

			if (value)
			{
				buffer.Write(JsonTokens.True);
			}
			else
			{
				buffer.Write(JsonTokens.False);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeNull(IBufferWriter<byte> buffer)
			=> buffer.Write(JsonTokens.Null);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private void WriteIndent(IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			if (this._isPrettyPrint)
			{
				buffer.Write(this._newLineChars.Span);
				for (var i = 0; i < collectionContext.CurrentDepth; i++)
				{
					buffer.Write(this._indentChars.Span);
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			buffer.Write(JsonTokens.ArrayStart);
			collectionContext.IncrementDepth();
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			collectionContext.DecrementDepth();
			this.WriteIndent(buffer, collectionContext);
			buffer.Write(JsonTokens.ArrayEnd);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayItemStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			if (index > 0)
			{
				buffer.Write(JsonTokens.Comma);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayItemEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			buffer.Write(JsonTokens.MapStart);
			collectionContext.IncrementDepth();
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			collectionContext.DecrementDepth();
			this.WriteIndent(buffer, collectionContext);
			buffer.Write(JsonTokens.MapEnd);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapKeyStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			if (index > 0)
			{
				buffer.Write(JsonTokens.Comma);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapKeyEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapValueStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = EnsureNotNull(buffer);

			buffer.Write(JsonTokens.Colon);
			if (this._isPrettyPrint)
			{
				buffer.Write(JsonTokens.Whitespace);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapValueEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static ReadOnlySpan<byte> GetEscapeSequence(byte c, bool newLineAllowed)
		{
			switch (c)
			{
				case (byte)'\b':
				{
					return JsonEscapeSequence.BackSpace;
				}
				case (byte)'\r':
				{
					if (newLineAllowed)
					{
						// Does not escape.
						return JsonCharactor.CarriageReturn;
					}

					return JsonEscapeSequence.CarriageReturn;
				}
				case (byte)'\f':
				{
					return JsonEscapeSequence.FormFeed;
				}
				case (byte)'\n':
				{
					if (newLineAllowed)
					{
						// Does not escape.
						return JsonCharactor.LineFeed;
					}

					return JsonEscapeSequence.LineFeed;
				}
				case (byte)'"':
				{
					return JsonEscapeSequence.Quatation;
				}
				case (byte)'\\':
				{
					return JsonEscapeSequence.ReverseSolidous;
				}
				case (byte)'\t':
				{
					return JsonEscapeSequence.Tab;
				}
			}

			// Use \uXXXX
			return ReadOnlySpan<byte>.Empty;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeString(ReadOnlySpan<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = EnsureNotNull(buffer);

			buffer.Write(JsonTokens.Quatation);

			this.EncodeStringBody(encodedValue, 0, buffer);

			// End quot
			buffer.Write(JsonTokens.Quatation);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private void EncodeStringBody(ReadOnlySpan<byte> encodedValue, long position, IBufferWriter<byte> buffer)
		{
			var source = encodedValue;
			while (!source.IsEmpty)
			{
				this.EscapeUtf8(ref source, ref position, buffer);
			}
		}

		private void EscapeUtf8(ref ReadOnlySpan<byte> utf8, ref long position, IBufferWriter<byte> buffer)
		{
			if (utf8.IsEmpty)
			{
				return;
			}

			if ((utf8[0] & 0b_1000_0000) == 0)
			{
				// 1 byte, 7bits
				byte codePoint = (byte)(utf8[0] & 0b_0111_1111);

				if (this._escapeTargetChars1Byte.Span.Contains(codePoint)
					|| codePoint < 0x09 // Control chars must be escaped except horizontal tab
					|| (this._options.EscapesHorizontalTab && codePoint == 0x09)
					|| (codePoint > 0x09 && codePoint < 0x20) // Control chars must be escaped except horizontal tab
					|| codePoint >= 0x7F  // Control chars must be escaped (DEL)
					)
				{
					var escapeSequence = GetEscapeSequence(codePoint, newLineAllowed: false);
					if (escapeSequence.IsEmpty)
					{
						EscapeCodePoint(codePoint, buffer);
					}
					else
					{
						buffer.Write(escapeSequence);
					}
				}
				else
				{
					buffer.Write(utf8.Slice(0, 1));
				}

				utf8 = utf8.Slice(1);
				position += 1;
			}
			else if ((utf8[0] & 0b_1110_0000) == 0b_1100_0000)
			{
				if (utf8.Length < 2)
				{
					JsonThrow.TooShortUtf8();
				}

				// 2 bytes, 5 + 6 bits
				var bits1 = utf8[0] & 0b_0001_1111;
				var bits2 = utf8[1] & 0b_0011_1111;

				if ((bits1 & 0b_1_1110) == 0
					|| (utf8[1] & 0b_1100_0000) != 0b_1000_0000)
				{
					JsonThrow.MalformedUtf8(utf8, position);
				}

				ushort codePoint = (ushort)((bits1 << 6) | (bits2 << 6));

				if (codePoint < 0xA0 // Control chars must be escaped
					|| this._escapeTargetChars2Byte.Span.Contains(codePoint))
				{
					EscapeCodePoint(codePoint, buffer);
				}
				else
				{
					buffer.Write(utf8.Slice(0, 1));
				}

				utf8 = utf8.Slice(2);
				position += 2;
			}
			else if ((utf8[0] & 0b_1111_0000) == 0b_1110_0000)
			{
				if (utf8.Length < 3)
				{
					JsonThrow.TooShortUtf8();
				}

				if ((utf8[1] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[2] & 0b_1100_0000) != 0b_1000_0000)
				{
					JsonThrow.MalformedUtf8(utf8, position);
				}

				// 3 bytes, 4 + 6 + 6 bits
				var bits1 = utf8[0] & 0b_0000_1111;
				var bits2 = utf8[1] & 0b_0011_1111;
				var bits3 = utf8[2] & 0b_0011_1111;

				ushort codePoint = (ushort)((bits1 << 12) | (bits2 << 6) | bits3);

				if ((codePoint & 0b_1111_100000_000000) == 0)
				{
					JsonThrow.MalformedUtf8(utf8, position);
				}

				if (!Rune.IsValid(codePoint))
				{
					JsonThrow.SurrogateCharInUtf8(position, codePoint);
				}

				if (codePoint >= 0xFFFE // U+FFFE and U+FFFF are Reserved
					|| (this._options.EscapesPrivateUseCharactors && codePoint >= 0xE000 && codePoint <= 0xF8FF) // Privte Use
					|| this._escapeTargetChars2Byte.Span.Contains(codePoint))
				{
					EscapeCodePoint(codePoint, buffer);
				}
				else
				{
					buffer.Write(utf8.Slice(0, 1));
				}

				utf8 = utf8.Slice(3);
				position += 3;
			}
			else if ((utf8[0] & 0b_1111_1000) == 0b_1111_0000)
			{
				if (utf8.Length < 4)
				{
					JsonThrow.TooShortUtf8();
				}

				if ((utf8[1] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[2] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[3] & 0b_1100_0000) != 0b_1000_0000)
				{
					JsonThrow.MalformedUtf8(utf8, position);
				}

				// 4 bytes, 3 + 6 + 6 + 6 bits
				var bits1 = (utf8[0] & 0b_0000_0111) << 18;
				var bits2 = (utf8[1] & 0b_0011_1111) << 12;
				var bits3 = (utf8[2] & 0b_0011_1111) << 6;
				var bits4 = (utf8[3] & 0b_0011_1111);

				int codePoint = ((bits1 << 18) | (bits2 << 12) | (bits1 << 6) | bits4);

				if ((codePoint & 0b_111_110000_000000_000000) == 0)
				{
					JsonThrow.MalformedUtf8(utf8, position);
				}

				if ((this._options.EscapesPrivateUseCharactors && codePoint >= 0xF0000) // Private use
					|| this._escapeTargetChars4Byte.Span.Contains(codePoint))
				{
					EscapeCodePoint(codePoint, buffer);
				}
				else
				{
					buffer.Write(utf8.Slice(0, 1));
				}

				utf8 = utf8.Slice(4);
				position += 4;
			}
			else
			{
				JsonThrow.MalformedUtf8(utf8, position);
			}
		}

		private static void EscapeCodePoint(int codePoint, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonEscapeSequence.Unicode);
			Span<byte> numbers = stackalloc byte[4];
			Utf8Formatter.TryFormat(codePoint, numbers, out _, JsonEscapeSequence.UnicodeFormat);
			buffer.Write(numbers);
		}

		public override void EncodeString(in ReadOnlySequence<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = EnsureNotNull(buffer);

			// Start quot
			buffer.Write(JsonTokens.Quatation);

			var source = new SequenceReader<byte>(encodedValue);

			while (!source.UnreadSpan.IsEmpty)
			{
				var length = source.UnreadSpan.Length;
				this.EncodeStringBody(source.UnreadSpan, source.Consumed, buffer);
				source.Advance(length);
			}

			// End quot
			buffer.Write(JsonTokens.Quatation);
		}

		public override void EncodeString(ReadOnlySpan<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			buffer = EnsureNotNull(buffer);
			encoding = encoding ?? Utf8EncodingNonBom.Instance;

			if (value.Length < 256)
			{
				// Fast-path with stackalloc
				var charLength = unchecked((int)value.Length);
				Span<byte> encodingBuffer = stackalloc byte[encoding.GetMaxByteCount(charLength)];
				var actualLength = encoding.GetBytes(value, encodingBuffer);
				this.EncodeString(encodingBuffer.Slice(0, actualLength), charLength, buffer);
			}
			else
			{
				// Slow-path

				// Start quot
				buffer.Write(JsonTokens.Quatation);

				var encoder = encoding.GetEncoder();
				var source = value;

				var encodingBuffer = base.Options.ByteBufferPool.Rent(Math.Min(2 * 1024 * 1024, encoding.GetMaxByteCount(unchecked((int)(value.Length & 0x1FFFFFFF)))));
				try
				{
					Span<byte> encodingSpan = encodingBuffer;
					while (!source.IsEmpty)
					{
						var length = source.Length;
						encoder.Convert(source, encodingSpan, flush: source.Length <= encodingSpan.Length, out var charsUsed, out var bytesUsed, out _);

						this.EncodeStringBody(encodingSpan.Slice(0, bytesUsed), position: 0 /* should not be used because ALWAYS valid UTF-8 */, buffer);
						source = source.Slice(charsUsed);
					}
				}
				finally
				{
					base.Options.ByteBufferPool.Return(encodingBuffer, base.Options.ClearsBuffer);
				}

				// End quot
				buffer.Write(JsonTokens.Quatation);
			}
		}

		public override void EncodeString(in ReadOnlySequence<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			buffer = EnsureNotNull(buffer);
			encoding = encoding ?? Utf8EncodingNonBom.Instance;

			if (value.IsSingleSegment)
			{
				this.EncodeString(value.FirstSpan, buffer, encoding);
				return;
			}

			if (value.Length < 256)
			{
				// Fast-path with stackalloc
				var charLength = unchecked((int)value.Length);
				Span<byte> encodingBuffer = stackalloc byte[encoding.GetMaxByteCount(charLength)];
				var actualLength = encoding.GetBytes(value, encodingBuffer);
				this.EncodeString(encodingBuffer.Slice(0, actualLength), charLength, buffer);
			}
			else
			{
				// Slow-path

				// Start quot
				buffer.Write(JsonTokens.Quatation);

				var encoder = encoding.GetEncoder();
				var source = new SequenceReader<char>(value);

				var encodingBuffer = base.Options.ByteBufferPool.Rent(Math.Min(2 * 1024 * 1024, encoding.GetMaxByteCount(unchecked((int)(value.Length & 0x1FFFFFFF)))));
				try
				{
					Span<byte> encodingSpan = encodingBuffer;
					while (!source.UnreadSpan.IsEmpty)
					{
						cancellationToken.ThrowIfCancellationRequested();

						var length = source.UnreadSpan.Length;
						encoder.Convert(source.UnreadSpan, encodingSpan, flush: source.Remaining <= encodingSpan.Length, out var charsUsed, out var bytesUsed, out _);

						this.EncodeStringBody(encodingSpan.Slice(0, bytesUsed), position: 0 /* should not be used because ALWAYS valid UTF-8 */, buffer);
						source.Advance(charsUsed);
					}
				}
				finally
				{
					base.Options.ByteBufferPool.Return(encodingBuffer, base.Options.ClearsBuffer);
				}

				// End quot
				buffer.Write(JsonTokens.Quatation);
			}
		}

		public override void EncodeBinary(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			// Start quot
			buffer.Write(JsonTokens.Quatation);

			if (value.Length < this.Options.CancellationSupportThreshold)
			{
				// Enough space -- over 138% 
				var span = buffer.GetSpan((int)(value.Length * 1.38));
				Base64.EncodeToUtf8(value, span, out var bytesConsumed, out var bytesWritten, isFinalBlock: true);
				buffer.Advance(bytesWritten);
			}
			else
			{
				EncodeBinarySlow(value, buffer, this.Options.CancellationSupportThreshold, cancellationToken);
			}

			// End quot
			buffer.Write(JsonTokens.Quatation);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void EncodeBinarySlow(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, int bufferSize, CancellationToken cancellationToken)
		{
			while (true)
			{
				var span = buffer.GetSpan(bufferSize);
				if (Base64.EncodeToUtf8(value, span, out var bytesConsumed, out var bytesWritten, isFinalBlock: value.IsEmpty) == OperationStatus.Done)
				{
					break;
				}

				buffer.Advance(bytesWritten);
				value = value.Slice(bytesConsumed);
				cancellationToken.ThrowIfCancellationRequested();
			}
		}

		public override void EncodeBinary(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			if (value.IsSingleSegment)
			{
				// Fast-path
				this.EncodeBinary(value.FirstSpan, buffer, cancellationToken);
				return;
			}

			// Start quot
			buffer.Write(JsonTokens.Quatation);

			var reader = new SequenceReader<byte>(value);
			while (true)
			{
				var span = buffer.GetSpan(this.Options.CancellationSupportThreshold);
				if (Base64.EncodeToUtf8(reader.UnreadSpan, span, out var bytesConsumed, out var bytesWritten, isFinalBlock: reader.End) == OperationStatus.Done)
				{
					break;
				}

				buffer.Advance(bytesWritten);
				reader.Advance(bytesConsumed);
				cancellationToken.ThrowIfCancellationRequested();
			}

			// End quot
			buffer.Write(JsonTokens.Quatation);
		}
	}
}
