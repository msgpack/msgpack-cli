using System;
using System.Buffers;
using System.Buffers.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Json
{
	internal static class JsonTokens
	{
		public static readonly ReadOnlyMemory<byte> Null = new[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
		public static readonly ReadOnlyMemory<byte> True = new[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
		public static readonly ReadOnlyMemory<byte> False = new[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
		public static readonly ReadOnlyMemory<byte> ArrayStart = new[] { (byte)'[' };
		public static readonly ReadOnlyMemory<byte> ArrayEnd = new[] { (byte)']' };
		public static readonly ReadOnlyMemory<byte> MapStart = new[] { (byte)'{' };
		public static readonly ReadOnlyMemory<byte> MapEnd = new[] { (byte)'}' };
		public static readonly ReadOnlyMemory<byte> Whitespace = new[] { (byte)' ' };
		public static readonly ReadOnlyMemory<byte> Comma = new[] { (byte)',' };
		public static readonly ReadOnlyMemory<byte> Colon = new[] { (byte)':' };
		public static readonly ReadOnlyMemory<byte> Quatation = new[] { (byte)'"' };
	}

	internal static class JsonCharactor
	{
		public static readonly ReadOnlyMemory<byte> MustBeEscaped = new int[] { '\\', '"' }.Concat(Enumerable.Range(0, 0x1F)).Select(i => unchecked((byte)i)).ToArray();
		public static readonly ReadOnlyMemory<byte> ShouldBeEscaped = new[] { (byte)'/', (byte)'\'', (byte)'<', (byte)'>', (byte)'&' };
		public static readonly ReadOnlyMemory<byte> CarriageReturn = new[] { (byte)'\r' };
		public static readonly ReadOnlyMemory<byte> LineFeed = new[] { (byte)'\n' };
	}

	internal static class JsonEscapeSequence
	{
		public static readonly ReadOnlyMemory<byte> Unicode = new[] { (byte)'\\', (byte)'u' };

		public static readonly ReadOnlyMemory<byte> ReverseSolidous = new[] { (byte)'\\', (byte)'\\' };
		public static readonly ReadOnlyMemory<byte> Quatation = new[] { (byte)'\\', (byte)'"' };
		public static readonly ReadOnlyMemory<byte> Tab = new[] { (byte)'\\', (byte)'t' };
		public static readonly ReadOnlyMemory<byte> CarriageReturn = new[] { (byte)'\\', (byte)'r' };
		public static readonly ReadOnlyMemory<byte> LineFeed = new[] { (byte)'\\', (byte)'n' };
		public static readonly ReadOnlyMemory<byte> BackSpace = new[] { (byte)'\\', (byte)'b' };
		public static readonly ReadOnlyMemory<byte> FormFeed = new[] { (byte)'\\', (byte)'f' };

		public static readonly StandardFormat UnicodeFormat = new StandardFormat('X', 4);
	}

	public enum NaNHandling
	{
		Default = 0,
		Null = 1,
		Error = 2,
		Custom = 3
	}

	public enum InfinityHandling
	{
		Default = 0,
		MinMax = 1,
		Error = 2,
		Custom = 3
	}

	[Flags]
	public enum JsonParseOptions
	{
		None = 0,
		AllowHashSingleLineComment = 0x1,
		AllowDoubleSolidousSingleLineComment = 0x2,
		AllowUnicodeWhitespace = 0x10,
		AllowAllTrivias = 0xFF,
		AllowNaN = 0x100,
		AllowInfinity = 0x200,
		AllowUndefined = 0x1000,
		AllowIrregalValues = 0xFF00,
		AllowEqualSignSeparator = 0x10000,
		AllowSemicolonDelimiter = 0x20000,
		AllowExtraComma = 0x40000,
		AllowSingleQuatationString = 0x100000,
		AllowUnescapedNewLineInString = 0x200000,
		AllowUnescapedTabInString = 0x400000,
		AllowWellknownSyntaxErrors = 0xFF0000,
		AllowAllErrors = unchecked((int)0xFFFFFFFF)
	}

	public class JsonEncoderOptionsBase
	{
#warning TODO: Options
	}

	public class JsonEncoderBase : Internal.Encoder
	{
#warning TODO: Abstract
#warning TODO: tuning
		private static readonly Func<float, Memory<byte>, int> s_singleFormatter =
			(v, m) => Utf8Formatter.TryFormat(v, m.Span, out var used) ? used : -1;
		private static readonly Func<float, Memory<byte>, int> s_defaultSingleNanFormatter =
			(_, m) => WriteNull(m.Span);
		private static readonly Func<float, Memory<byte>, int> s_defaultSingleInfinityFormatter =
			(v, m) => Utf8Formatter.TryFormat(v < 0 ? Single.MinValue : Single.MaxValue, m.Span, out var used) ? used : -1;

		private static readonly Func<double, Memory<byte>, int> s_doubleFormatter =
			(v, m) => Utf8Formatter.TryFormat(v, m.Span, out var used) ? used : -1;
		private static readonly Func<double, Memory<byte>, int> s_defaultDoubleNanFormatter =
			(_, m) => WriteNull(m.Span);
		private static readonly Func<double, Memory<byte>, int> s_defaultDoubleInfinityFormatter =
			(v, m) => Utf8Formatter.TryFormat(v < 0 ? Single.MinValue : Single.MaxValue, m.Span, out var used) ? used : -1;

		private readonly Func<float, Memory<byte>, int> _singleInfinityFormatter;
		private readonly Func<float, Memory<byte>, int> _singleNanFormatter;
		private readonly Func<double, Memory<byte>, int> _doubleInfinityFormatter;
		private readonly Func<double, Memory<byte>, int> _doubleNanFormatter;
		private readonly bool _doIndent;
		private readonly byte[] _indentString;
		private readonly byte[] _newLine;
		private readonly ReadOnlyMemory<byte> _toBeEscaped;

		private int _indentLevel;

#warning TODO: Options
		protected JsonEncoderBase()
		{
			this._singleInfinityFormatter = s_defaultSingleInfinityFormatter;
			this._singleNanFormatter = s_defaultSingleNanFormatter;
			this._doubleInfinityFormatter = s_defaultDoubleInfinityFormatter;
			this._doubleNanFormatter = s_defaultDoubleNanFormatter;
			this._doIndent = false;
			this._indentString = new[] { (byte)' ', (byte)' ' };
			this._newLine = new[] { (byte)'\n' };
			this._toBeEscaped = JsonCharactor.MustBeEscaped;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static int WriteNull(Span<byte> span)
		{
			if (span.Length < 4)
			{
				return -1;
			}

			JsonTokens.Null.Span.CopyTo(span);
			return 4;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void Encode(int value, IBufferWriter<byte> buffer)
		{
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
		public override void Encode(long value, IBufferWriter<byte> buffer)
		{
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
		public override void Encode(float value, IBufferWriter<byte> buffer)
		{
			var formatter =
				Single.IsNaN(value) ?
				this._singleNanFormatter :
				Single.IsInfinity(value) ?
				this._singleInfinityFormatter :
				s_singleFormatter;

			var memory = buffer.GetMemory();
			while (true)
			{
				var used = formatter(value, memory);
				if (used < 0)
				{
					memory = buffer.GetMemory(memory.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void Encode(double value, IBufferWriter<byte> buffer)
		{
			var formatter =
				Double.IsNaN(value) ?
				this._doubleNanFormatter :
				Double.IsInfinity(value) ?
				this._doubleInfinityFormatter :
				s_doubleFormatter;

			var memory = buffer.GetMemory();
			while (true)
			{
				var used = formatter(value, memory);
				if (used < 0)
				{
					memory = buffer.GetMemory(memory.Length * 2);
				}
				else
				{
					buffer.Advance(used);
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void Encode(bool value, IBufferWriter<byte> buffer)
		{
			if (value)
			{
				buffer.Write(JsonTokens.True.Span);
			}
			else
			{
				buffer.Write(JsonTokens.False.Span);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeNull(IBufferWriter<byte> buffer)
			=> buffer.Write(JsonTokens.Null.Span);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		protected void WriteIndent(IBufferWriter<byte> buffer)
		{
			if (this._doIndent)
			{
				buffer.Write(this._newLine);
				for (var i = 0; i < this._indentLevel; i++)
				{
					buffer.Write(this._indentString);
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayStart(uint length, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonTokens.ArrayStart.Span);
			this._indentLevel++;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayEnd(uint length, IBufferWriter<byte> buffer)
		{
			this._indentLevel--;
			this.WriteIndent(buffer);
			buffer.Write(JsonTokens.ArrayEnd.Span);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayItemStart(uint index, IBufferWriter<byte> buffer)
		{
			this.WriteIndent(buffer);
			if (index > 0)
			{
				buffer.Write(JsonTokens.Comma.Span);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeArrayItemEnd(uint index, IBufferWriter<byte> buffer) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapStart(uint length, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonTokens.MapStart.Span);
			this._indentLevel++;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapEnd(uint length, IBufferWriter<byte> buffer)
		{
			this._indentLevel--;
			this.WriteIndent(buffer);
			buffer.Write(JsonTokens.MapEnd.Span);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapKeyStart(uint index, IBufferWriter<byte> buffer)
		{
			this.WriteIndent(buffer);
			if (index > 0)
			{
				buffer.Write(JsonTokens.Comma.Span);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapKeyEnd(uint index, IBufferWriter<byte> buffer) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapValueStart(uint index, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonTokens.Colon.Span);
			if (this._doIndent)
			{
				buffer.Write(JsonTokens.Whitespace.Span);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeMapValueEnd(uint index, IBufferWriter<byte> buffer) { }

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static ReadOnlyMemory<byte> GetEscapeSequence(byte c, bool newLineAllowed)
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
			return ReadOnlyMemory<byte>.Empty;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override void EncodeString(ReadOnlySpan<byte> encodedValue, int charLength, IBufferWriter<byte> buffer)
		{
			buffer.Write(JsonTokens.Quatation.Span);

			this.EncodeStringBody(encodedValue, buffer);

			// End quot
			buffer.Write(JsonTokens.Quatation.Span);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private void EncodeStringBody(ReadOnlySpan<byte> encodedValue, IBufferWriter<byte> buffer)
		{
			var source = encodedValue;
			var sink = buffer.GetSpan(encodedValue.Length);
			while (!source.IsEmpty)
			{
				var moreBytes = this.EscapeStringContent(source, sink, out var sourceUsed, out var sinkUsed);
				buffer.Advance(sinkUsed);
				source = source.Slice(sourceUsed);
				sink = buffer.GetSpan(moreBytes);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private int EscapeStringContent(ReadOnlyMemory<byte> source, Memory<byte> sink, out int sourceUsed, out int sinkUsed)
			=> this.EscapeStringContent(source.Span, sink.Span, out sourceUsed, out sinkUsed);

		private int EscapeStringContent(ReadOnlySpan<byte> source, Span<byte> sink, out int sourceUsed, out int sinkUsed)
		{
			sourceUsed = 0;
			sinkUsed = 0;

			while (!sink.IsEmpty)
			{
				var toBeEscaped = source.IndexOfAny(this._toBeEscaped.Span);
				if (toBeEscaped < 0)
				{
					// Copy entire source to sink, and then advance buffer.
					source.CopyTo(sink);
					sourceUsed += source.Length;
					sinkUsed += source.Length;
					continue;
				}

				if (toBeEscaped > 0)
				{
					// Copy source data which have not to be escaped to sink, and then advance buffer.
					source.Slice(0, toBeEscaped).CopyTo(sink);
					sink = sink.Slice(toBeEscaped);
					sourceUsed += toBeEscaped;
					sinkUsed += toBeEscaped;
					source = source.Slice(toBeEscaped);
				}

#warning TODO: OPTIONS
				var escapeSequence = GetEscapeSequence(source[0], newLineAllowed: false);
				switch (escapeSequence.Length)
				{
					case 1:
					{
						// Does not escape because of the option.
						sink[0] = source[0];
						sink = sink.Slice(1);
						sinkUsed++;
						break;
					}
					case 0:
					{
						// \uXXXX escape
						if (sink.Length < 6)
						{
							// Return to expecte realloc.
							return source.Length + 6;
						}

						JsonEscapeSequence.Unicode.Span.CopyTo(sink);
						sink = sink.Slice(2); // \u
						Utf8Formatter.TryFormat(source[0], sink, out _, JsonEscapeSequence.UnicodeFormat);
						sink = sink.Slice(4); // xxxx
						sinkUsed += 6;
						break;
					}
					default:
					{
						// Use returned escape sequence

						if (sink.Length < escapeSequence.Length)
						{
							// Return to expecte realloc.
							return source.Length + escapeSequence.Length;
						}

						escapeSequence.Span.CopyTo(sink);
						sink = sink.Slice(escapeSequence.Length);
						sinkUsed += escapeSequence.Length;
						break;
					}
				} // switch

				sourceUsed++;
				source = source.Slice(1);
			} // while (!source.IsEmpty)

			return 0;
		}

		public override void EncodeString(in ReadOnlySequence<byte> encodedValue, int charLength, IBufferWriter<byte> buffer)
		{
			// Start quot
			buffer.Write(JsonTokens.Quatation.Span);

			var source = new SequenceReader<byte>(encodedValue);

			while (!source.UnreadSpan.IsEmpty)
			{
				var length = source.UnreadSpan.Length;
				this.EncodeStringBody(source.UnreadSpan, buffer);
				source.Advance(length);
			}

			// End quot
			buffer.Write(JsonTokens.Quatation.Span);
		}

		public override void EncodeString(ReadOnlySpan<char> value, IBufferWriter<byte> buffer, Encoding encoding)
		{
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
				buffer.Write(JsonTokens.Quatation.Span);

				var encoder = encoding.GetEncoder();
				var source = value;

				var encodingBuffer = this.ByteArrayPool.Rent(Math.Min(2 * 1024 * 1024, encoding.GetMaxByteCount(unchecked((int)(value.Length & 0x1FFFFFFF)))));
				try
				{
					Span<byte> encodingSpan = encodingBuffer;
					while (!source.IsEmpty)
					{
						var length = source.Length;
						encoder.Convert(source, encodingSpan, flush: source.Length <= encodingSpan.Length, out var charsUsed, out var bytesUsed, out _);

						this.EncodeStringBody(encodingSpan.Slice(0, bytesUsed), buffer);
						source = source.Slice(charsUsed);
					}
				}
				finally
				{
					this.ByteArrayPool.Return(encodingBuffer, this.ClearsBufferPool);
				}

				// End quot
				buffer.Write(JsonTokens.Quatation.Span);
			}
		}

		public override void EncodeString(in ReadOnlySequence<char> value, IBufferWriter<byte> buffer, Encoding encoding)
		{
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
				buffer.Write(JsonTokens.Quatation.Span);

				var encoder = encoding.GetEncoder();
				var source = new SequenceReader<char>(value);

				var encodingBuffer = this.ByteArrayPool.Rent(Math.Min(2 * 1024 * 1024, encoding.GetMaxByteCount(unchecked((int)(value.Length & 0x1FFFFFFF)))));
				try
				{
					Span<byte> encodingSpan = encodingBuffer;
					while (!source.UnreadSpan.IsEmpty)
					{
						var length = source.UnreadSpan.Length;
						encoder.Convert(source.UnreadSpan, encodingSpan, flush: source.Remaining <= encodingSpan.Length, out var charsUsed, out var bytesUsed, out _);

						this.EncodeStringBody(encodingSpan.Slice(0, bytesUsed), buffer);
						source.Advance(charsUsed);
					}
				}
				finally
				{
					this.ByteArrayPool.Return(encodingBuffer, this.ClearsBufferPool);
				}

				// End quot
				buffer.Write(JsonTokens.Quatation.Span);
			}
		}

		protected override async ValueTask<long> EncodeLargeStringCoreAsync(TextReader source, Stream sink, Encoding encoding, Func<Stream> bufferStreamProvider, CancellationToken cancellationToken)
		{
			// Start quot
			await sink.WriteAsync(JsonTokens.Quatation, cancellationToken).ConfigureAwait(false);
			long written = JsonTokens.Quatation.Length;

			var maxBytesPerChar = encoding.GetMaxByteCount(1);
			var encoder = encoding.GetEncoder();
			var charBufferLength = ByteBufferLength / maxBytesPerChar;

			var encodingBufferArray = this.ByteArrayPool.Rent(ByteBufferLength);
			try
			{
				var sourceBufferArray = this.CharArrayPool.Rent(charBufferLength);
				try
				{
					var escapingBufferArray = this.ByteArrayPool.Rent(ByteBufferLength);
					try
					{
						Memory<char> sourceBuffer = sourceBufferArray;
						Memory<byte> encodingBuffer = encodingBufferArray;
						Memory<byte> escapingBuffer = escapingBufferArray;

						bool completed = false;

						do
						{
							var readLength = await source.ReadAsync(sourceBuffer, cancellationToken).ConfigureAwait(false);
							var shouldFlush = readLength <= sourceBuffer.Length && source.Peek() < 0;
							var chars = sourceBuffer.Slice(0, readLength);
							while (!chars.IsEmpty)
							{
								encoder.Convert(chars, encodingBuffer, shouldFlush, out var charsUsed, out var bytesUsed, out completed);
								encodingBuffer = encodingBuffer.Slice(0, bytesUsed);

								while (!encodingBuffer.IsEmpty)
								{
									var moreEscapingBufferSize = this.EscapeStringContent(encodingBuffer, escapingBuffer, out var encodingBufferUsed, out var escapingBufferUsed);

									await sink.WriteAsync(escapingBuffer.Slice(0, escapingBufferUsed), cancellationToken).ConfigureAwait(false);
									written += escapingBufferUsed;

									encodingBuffer = encodingBuffer.Slice(encodingBufferUsed);
									escapingBuffer = escapingBuffer.Compact(escapingBufferArray, escapingBufferUsed);
								}

								chars = chars.Slice(charsUsed);
							}
						} while (!completed);
					}
					finally
					{
						this.ByteArrayPool.Return(escapingBufferArray, this.ClearsBufferPool);
					}
				}
				finally
				{
					this.CharArrayPool.Return(sourceBufferArray, this.ClearsBufferPool);
				}
			}
			finally
			{
				this.ByteArrayPool.Return(encodingBufferArray, this.ClearsBufferPool);
			}

			// End quot
			await sink.WriteAsync(JsonTokens.Quatation, cancellationToken).ConfigureAwait(false);
			written += JsonTokens.Quatation.Length;

			return written;
		}

		public override void EncodeBinary(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer)
		{
			// Start quot
			buffer.Write(JsonTokens.Quatation.Span);

			// Enough space -- over 138% 
			var span = buffer.GetSpan((int)(value.Length * 1.38));
			Base64.EncodeToUtf8(value, span, out var bytesConsumed, out var bytesWritten, isFinalBlock: true);
			buffer.Advance(bytesWritten);

			// End quot
			buffer.Write(JsonTokens.Quatation.Span);
		}

		public override void EncodeBinary(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer)
		{
			// Start quot
			buffer.Write(JsonTokens.Quatation.Span);

			var array = this.ByteArrayPool.Rent(ByteBufferLength);
			try
			{
				var reader = new SequenceReader<byte>(value);
				Span<byte> sourceBuffer = array;

				while (true)
				{
					if (!reader.TryCopyTo(sourceBuffer))
					{
						while (!reader.End)
						{
							reader.UnreadSpan.CopyTo(sourceBuffer);
							sourceBuffer = sourceBuffer.Slice(0, reader.UnreadSpan.Length);
							reader.Advance(reader.UnreadSpan.Length);
						}
					}

					var span = buffer.GetSpan(sourceBuffer.Length * 2);
					var status = Base64.EncodeToUtf8(sourceBuffer, span, out var bytesConsumed, out var bytesWritten, reader.End);
					buffer.Advance(bytesWritten);
					reader.Advance(bytesConsumed);
					sourceBuffer = array;

					if (status == OperationStatus.Done && reader.End)
					{
						break;
					}
				}

				// End quot
				buffer.Write(JsonTokens.Quatation.Span);
			}
			finally
			{
				this.ByteArrayPool.Return(array, this.ClearsBufferPool);
			}

			// End quot
			buffer.Write(JsonTokens.Quatation.Span);
		}

		protected override async ValueTask<long> EncodeLargeBinaryCoreAsync(Stream source, Stream sink, Func<Stream> bufferStreamProvider, CancellationToken cancellationToken)
		{
			static OperationStatus EncodeBase64ToUtf8(ReadOnlyMemory<byte> sourceMemory, Memory<byte> sinkMemory, out int bytesConsumed, out int bytesWritten, bool isFinalBlock)
				=> Base64.EncodeToUtf8(sourceMemory.Span, sinkMemory.Span, out bytesConsumed, out bytesWritten, isFinalBlock);

			// End quot
			await sink.WriteAsync(JsonTokens.Quatation, cancellationToken).ConfigureAwait(false);
			long written = JsonTokens.Quatation.Length;

			var sourceBufferArray = this.ByteArrayPool.Rent(ByteBufferLength / 2);
			try
			{
				var sinkBufferArray = this.ByteArrayPool.Rent(ByteBufferLength);
				try
				{
					Memory<byte> sourceBuffer = sourceBufferArray;
					Memory<byte> sinkBuffer = sinkBufferArray;

					while (true)
					{
						await source.ReadAsync(sourceBuffer, cancellationToken).ConfigureAwait(false);

						var status = EncodeBase64ToUtf8(sourceBuffer, sinkBuffer, out var sourceConsumed, out var sinkWritten, source.Position == source.Length);

						if (status == OperationStatus.Done && source.Position == source.Length)
						{
							break;
						}
					}
				}
				finally
				{
					this.ByteArrayPool.Return(sinkBufferArray, this.ClearsBufferPool);
				}
			}
			finally
			{
				this.ByteArrayPool.Return(sourceBufferArray, this.ClearsBufferPool);
			}

			// End quot
			await sink.WriteAsync(JsonTokens.Quatation, cancellationToken).ConfigureAwait(false);
			written += JsonTokens.Quatation.Length;
			return written;
		}
	}
}
