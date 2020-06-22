// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
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
	public sealed class JsonEncoder : FormatEncoder
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
		public sealed override void EncodeRawString(ReadOnlySpan<byte> rawString, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
			=> Ensure.NotNull(buffer).Write(rawString);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeInt32(int value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(11);
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
		public sealed override void EncodeUInt32(uint value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(10);
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
		public sealed override void EncodeInt64(long value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(20);
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
		public sealed override void EncodeUInt64(ulong value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			var span = buffer.GetSpan(19);
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
		public sealed override void EncodeSingle(float value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			if (Single.IsNaN(value))
			{
				this._singleNanFormatter(value, buffer, this._options);
			}
			else if (Single.IsInfinity(value))
			{
				this._singleInfinityFormatter(value, buffer, this._options);
			}
			else
			{
				JsonFormatter.Format(value, buffer);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeDouble(double value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			if (Double.IsNaN(value))
			{
				this._doubleNanFormatter(value, buffer, this._options);
			}
			else if (Double.IsInfinity(value))
			{
				this._doubleInfinityFormatter(value, buffer, this._options);
			}
			else
			{
				JsonFormatter.Format(value, buffer);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeBoolean(bool value, IBufferWriter<byte> buffer)
		{
			buffer = Ensure.NotNull(buffer);

			if (value)
			{
				var span = buffer.GetSpan(4);
				span[0] = (byte)'t';
				span[1] = (byte)'r';
				span[2] = (byte)'u';
				span[3] = (byte)'e';
				buffer.Advance(4);
			}
			else
			{
				var span = buffer.GetSpan(5);
				span[0] = (byte)'f';
				span[1] = (byte)'a';
				span[2] = (byte)'l';
				span[3] = (byte)'s';
				span[4] = (byte)'e';
				buffer.Advance(5);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeNull(IBufferWriter<byte> buffer)
			=> JsonFormatter.WriteNull(Ensure.NotNull(buffer));

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private void WriteIndent(IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			if (this._isPrettyPrint)
			{
				this.WriteIndentCore(buffer, collectionContext);
			}
		}

		private void WriteIndentCore(IBufferWriter<byte> buffer, CollectionContext collectionContext)
		{
			Debug.Assert(buffer != null);
			buffer.Write(this._newLineChars.Span);
			for (var i = 0; i < collectionContext.CurrentDepth; i++)
			{
				buffer.Write(this._indentChars.Span);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			buffer.GetSpan(1)[0] = (byte)'[';
			buffer.Advance(1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			buffer.GetSpan(1)[0] = (byte)']';
			buffer.Advance(1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayItemStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			if (index > 0)
			{
				buffer.GetSpan(1)[0] = (byte)',';
				buffer.Advance(1);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeArrayItemEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			buffer.GetSpan(1)[0] = (byte)'{';
			buffer.Advance(1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			buffer.GetSpan(1)[0] = (byte)'}';
			buffer.Advance(1);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapKeyStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			this.WriteIndent(buffer, collectionContext);
			if (index > 0)
			{
				buffer.GetSpan(1)[0] = (byte)',';
				buffer.Advance(1);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapKeyEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapValueStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext)
		{
			buffer = Ensure.NotNull(buffer);

			if (!this._isPrettyPrint)
			{
				buffer.GetSpan(1)[0] = (byte)':';
				buffer.Advance(1);
			}
			else
			{
				var span = buffer.GetSpan(2);
				span[0] = (byte)':';
				span[1] = (byte)' ';
				buffer.Advance(2);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeMapValueEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static ReadOnlySpan<byte> GetEscapeSequence(byte c, bool newLineAllowed, bool tabAllowed, bool escapeHtmlChars)
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
					if (escapeHtmlChars)
					{
						return ReadOnlySpan<byte>.Empty;
					}

					return JsonEscapeSequence.Quatation;
				}
				case (byte)'\\':
				{
					return JsonEscapeSequence.ReverseSolidous;
				}
				case (byte)'\t':
				{
					if (newLineAllowed)
					{
						// Does not escape.
						return JsonCharactor.Tab;
					}

					return JsonEscapeSequence.Tab;
				}
			}

			// Use \uXXXX
			return ReadOnlySpan<byte>.Empty;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override void EncodeString(ReadOnlySpan<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = Ensure.NotNull(buffer);

			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);

			var source = encodedValue;
			this.EncodeStringBody(ref source, 0, buffer, out var requestHint);
			if (requestHint > 0)
			{
				JsonThrow.TooShortUtf8();
			}

			// End quot
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);
		}

		private void EncodeStringBody(ref ReadOnlySpan<byte> encodedValue, long position, IBufferWriter<byte> buffer, out int requestHint)
		{
			var utf8 = encodedValue;
			while (utf8.Length > 0)
			{
				if ((utf8[0] & 0b_1000_0000) == 0)
				{
					// 1 byte, 7bits
					byte codePoint = utf8[0];

					if (this._escapeTargetChars1Byte.Span.Contains(codePoint)
						|| codePoint < 0x20 // Control chars must be escaped
						|| codePoint == 0x7F  // Control chars must be escaped (DEL)
						)
					{
						var escapeSequence = GetEscapeSequence(codePoint, newLineAllowed: false, tabAllowed: !this._options.EscapesHorizontalTab, this._options.EscapesHtmlChars);
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
						requestHint = 1;
						return;
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
						requestHint = 3 - utf8.Length;
						return;
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
						requestHint = 4 - utf8.Length;
						return;
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

					int codePoint = ((bits1 << 18) | (bits2 << 12) | (bits3 << 6) | bits4);

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

			requestHint = 0;
		}

		private static void EscapeCodePoint(int codePoint, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonEscapeSequence.Unicode);
			Span<byte> numbers = stackalloc byte[4];
			Utf8Formatter.TryFormat(codePoint, numbers, out _, JsonEscapeSequence.UnicodeFormat);
			buffer.Write(numbers);
		}

		public sealed override void EncodeString(in ReadOnlySequence<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = Ensure.NotNull(buffer);

			// Start quot
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);

			var source = new SequenceReader<byte>(encodedValue);

			while (!source.End)
			{
				var length = source.UnreadSpan.Length;
				var span = source.UnreadSpan;
				this.EncodeStringBody(ref span, source.Consumed, buffer, out var requestHint);
				source.Advance(length - span.Length);

				if (requestHint > 0)
				{
					Span<byte> codePoints = stackalloc byte[span.Length + requestHint];
					if (!source.TryCopyTo(codePoints))
					{
						JsonThrow.TooShortUtf8();
					}

					source.Advance(codePoints.Length);

					ReadOnlySpan<byte> readOnlyCodePoints = codePoints;
					this.EncodeStringBody(ref readOnlyCodePoints, source.Consumed, buffer, out requestHint);
				}
			}

			// End quot
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);
		}

		public sealed override void EncodeString(ReadOnlySpan<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			buffer = Ensure.NotNull(buffer);
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);

			var source = value;
			this.EncodeStringBody(ref source, buffer, out var requestHint);
			if (requestHint > 0)
			{
				JsonThrow.OrphanSurrogate(value.Length - source.Length, source[0]);
			}

			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);
		}

		public sealed override void EncodeString(in ReadOnlySequence<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			if (value.IsSingleSegment)
			{
				this.EncodeString(value.FirstSpan, buffer, encoding);
				return;
			}
			else
			{
				buffer = Ensure.NotNull(buffer);

				// Start quot
				buffer.GetSpan(1)[0] = (byte)'"';
				buffer.Advance(1);

				var source = new SequenceReader<char>(value);

				while (!source.End)
				{
					var length = source.UnreadSpan.Length;
					var span = source.UnreadSpan;
					this.EncodeStringBody(ref span, buffer, out var requestHint);
					source.Advance(length - span.Length);

					if (requestHint > 0)
					{
						Span<char> codePoints = stackalloc char[span.Length + requestHint];
						if (!source.TryCopyTo(codePoints))
						{
							JsonThrow.TooShortUtf8();
						}

						source.Advance(codePoints.Length);

						ReadOnlySpan<char> readOnlyCodePoints = codePoints;
						this.EncodeStringBody(ref readOnlyCodePoints, buffer, out requestHint);
					}
				}

				// End quot
				buffer.GetSpan(1)[0] = (byte)'"';
				buffer.Advance(1);
			}
		}

		private void EncodeStringBody(ref ReadOnlySpan<char> chars, IBufferWriter<byte> buffer, out int requestHint)
		{
			var position = 0;
			for(var i = 0; i < chars.Length; i++)
			{
				var c = chars[i];
				if (c < 0x80)
				{
					var codePoint = (byte)c;

					if (this._escapeTargetChars1Byte.Span.Contains(codePoint)
						|| codePoint < 0x20 // Control chars must be escaped
						|| codePoint == 0x7F  // Control chars must be escaped (DEL)
						)
					{
						Utf8EncodingNonBom.Instance.GetBytes(chars.Slice(0, i), buffer);
						chars = chars.Slice(i + 1);
						position += i + 1;
						i = 0;

						var escapeSequence = GetEscapeSequence(codePoint, newLineAllowed: false, tabAllowed: !this._options.EscapesHorizontalTab, this._options.EscapesHtmlChars);
						if (escapeSequence.IsEmpty)
						{
							EscapeCodePoint(codePoint, buffer);
						}
						else
						{
							buffer.Write(escapeSequence);
						}
					}
				}
				else if (c < 0x800)
				{
					ushort codePoint = c;

					if (codePoint < 0xA0 // Control chars must be escaped
						|| this._escapeTargetChars2Byte.Span.Contains(codePoint))
					{
						Utf8EncodingNonBom.Instance.GetBytes(chars.Slice(0, i), buffer);
						chars = chars.Slice(i);
						position += i + 1;
						i = 0;

						EscapeCodePoint(codePoint, buffer);
					}
				}
				else if (!Char.IsSurrogate(c))
				{
					ushort codePoint = c;

					if (codePoint >= 0xFFFE // U+FFFE and U+FFFF are Reserved
						|| (this._options.EscapesPrivateUseCharactors && codePoint >= 0xE000 && codePoint <= 0xF8FF) // Privte Use
						|| this._escapeTargetChars2Byte.Span.Contains(codePoint))
					{
						Utf8EncodingNonBom.Instance.GetBytes(chars.Slice(0, i), buffer);
						chars = chars.Slice(i);
						position += i + 1;
						i = 0;

						EscapeCodePoint(codePoint, buffer);
					}
				}
				else
				{
					// Surrogate
					if(chars.Length < i + 2)
					{
						Utf8EncodingNonBom.Instance.GetBytes(chars.Slice(0, i), buffer);
						chars = chars.Slice(i);
						position += i;
						i = 0;
						requestHint = 1;
						return;
					}

					if(!Char.IsSurrogatePair(c, chars[1]))
					{
						JsonThrow.OrphanSurrogate(position, c);
					}

					var codePoint = Char.ConvertToUtf32(c, chars[1]);

					if ((this._options.EscapesPrivateUseCharactors && codePoint >= 0xF0000) // Private use
						|| this._escapeTargetChars4Byte.Span.Contains(codePoint))
					{
						EscapeCodePoint(codePoint, buffer);
					}

					position += 2;
					i++;
				}
			}

			requestHint = 0;
			Utf8EncodingNonBom.Instance.GetBytes(chars, buffer);
			chars = ReadOnlySpan<char>.Empty;
		}

		public sealed override void EncodeBinary(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			// Start quot
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);

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
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);
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

		public sealed override void EncodeBinary(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			if (value.IsSingleSegment)
			{
				// Fast-path
				this.EncodeBinary(value.FirstSpan, buffer, cancellationToken);
				return;
			}

			// Start quot
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);

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
			buffer.GetSpan(1)[0] = (byte)'"';
			buffer.Advance(1);
		}
	}
}
