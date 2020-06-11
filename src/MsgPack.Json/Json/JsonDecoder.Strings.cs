// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		/// <inheritdoc />
		public override bool GetRawString(in SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, CancellationToken cancellationToken = default)
		{
			if (!this.GetRawStringCore(source, out var rawStringSequence, out requestHint, cancellationToken))
			{
				rawString = default;
				return false;
			}

			if (rawStringSequence.IsSingleSegment)
			{
				rawString = rawStringSequence.FirstSpan;
			}
			else
			{
				// length should be Int32 because max length option is Int32 type.
				Span<byte> result = new byte[(int)rawStringSequence.Length];
				rawStringSequence.CopyTo(result);
				rawString = result;
			}

			return true;
		}

		private bool GetRawStringCore(in SequenceReader<byte> source, out ReadOnlySequence<byte> rawString, out int requestHint, CancellationToken cancellationToken = default)
		{ 
			this.ReadTrivia(source, out _);
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(source, out var delimiters, out requestHint);
			if (requestHint != 0)
			{
				rawString = default;
				return false;
			}

			var length = 0L;
			var rawStringStart = source.Position;
			// We accept unescaped charactors except quotation even if the value is '\0' because we can handle them correctly.
			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (!source.TryReadToAny(out ReadOnlySequence<byte> sequence, delimiters, advancePastDelimiter: false))
				{
					// EoF
					requestHint = 1;
					break;
				}

				if (sequence.Length + length > this.Options.MaxStringLengthInBytes)
				{
					Throw.StringLengthExceeded(source.Consumed - sequence.Length, sequence.Length + length, this.Options.MaxBinaryLengthInBytes);
				}

				length += (int)sequence.Length;

				// Handle delimiter
				source.TryRead(out var delimiter);
				if (delimiter == quotation)
				{
					// End
					requestHint = 0;
					rawString = source.Sequence.Slice(rawStringStart, length);
					return true;
				}

				// Decode escape sequence
				if (source.End)
				{
					// EoF
					requestHint = 2;
					break;
				}
			}

			// No closing quotation.
			rawString = default;
			source.Rewind(source.Consumed - startOffset);
			return false;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override string? DecodeNullableString(in SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(source, out _);
			if (this.TryReadNull(source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeStringCore(source, out requestHint, cancellationToken);
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override string? DecodeString(in SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(source, out _);
			return this.DecodeStringCore(source, out requestHint, cancellationToken);
		}

		private string? DecodeStringCore(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(source, out var delimiters, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			var length = 0L;
			using (var result = new StringBuilderBufferWriter(new StringBuilder(), this.Options))
			{
				// We accept unescaped charactors except quotation even if the value is '\0' because we can handle them correctly.
				while (true)
				{
					cancellationToken.ThrowIfCancellationRequested();

					if (!source.TryReadToAny(out ReadOnlySequence<byte> sequence, delimiters, advancePastDelimiter: false))
					{
						// EoF
						requestHint = 1;
						source.Rewind(source.Consumed - startOffset);
						break;
					}

					if (sequence.Length + length > this.Options.MaxStringLengthInBytes)
					{
						Throw.StringLengthExceeded(source.Consumed - sequence.Length, sequence.Length + length, this.Options.MaxBinaryLengthInBytes);
					}

					// Copy & UTF8 decoding existing.
					Encoding.UTF8.GetChars(sequence, result);
					length += (int)sequence.Length;

					// Handle delimiter
					source.TryRead(out var delimiter);
					if (delimiter == quotation)
					{
						// End
						requestHint = 0;
						return result.ToString();
					}

					// Decode escape sequence
					if (source.End)
					{
						// EoF
						requestHint = 2;
						break;
					}

					if (!source.IsNext((byte)'u', advancePast: true))
					{
						DecodeSpetialEscapeSequence(source, result);
						length += 2;
					}
					else
					{
						DecodeUnicodeEscapceSequence(source, result, out requestHint);
						if (requestHint != 0)
						{
							source.Rewind(source.Consumed - startOffset);
							return default;
						}

						length += 6;
					}
				}
			}

			// No closing quotation.
			return default;
		}

		private byte ReadStringStart(in SequenceReader<byte> source, out ReadOnlySpan<byte> delimiters, out int requestHint)
		{
			if (source.End)
			{
				requestHint = 2;
				delimiters = default;
				return default;
			}

			if (source.IsNext((byte)'"', advancePast: false))
			{
				delimiters = JsonStringTokens.DoubleQuotationDelimiters;
			}
			else
			{
				if ((this.Options.ParseOptions & JsonParseOptions.AllowSingleQuotationString) != 0)
				{
					// ['"]
					if (source.IsNext((byte)'\'', advancePast: false))
					{
						delimiters = JsonStringTokens.SingleQuotationDelimiters;
					}
					else
					{
						JsonThrow.IsNotStringStart(source.Consumed, JsonStringTokens.AnyQuotations);
						// never
						delimiters = default;
					}
				}
				else
				{
					JsonThrow.IsNotStringStart(source.Consumed, JsonStringTokens.DoubleQuotation);
					// never
					delimiters = default;
				}
			}

			source.TryRead(out var quotation);
			requestHint = 0;
			return quotation;
		}

		private static void DecodeSpetialEscapeSequence(in SequenceReader<byte> source, StringBuilderBufferWriter result)
		{
			source.TryPeek(out var escaped);
			// Handle known escape sequence
			var index = escaped - JsonStringTokens.UnescapedCharactorsOffset;
			var unescaped = Byte.MaxValue;
			if (index >= 0 && index < JsonStringTokens.UnescapedCharactors.Length)
			{
				unescaped = JsonStringTokens.UnescapedCharactors[index];
			}

			if (unescaped >= 0x80)
			{
				JsonThrow.InvalidEscapeSequence(source.Consumed, escaped);
			}

			result.AppendUtf16CodePoint(unescaped);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void DecodeUnicodeEscapceSequence(SequenceReader<byte> source, StringBuilderBufferWriter result, out int requestHint)
		{
			Span<byte> buffer = stackalloc byte[4];
			if (!source.TryCopyTo(buffer))
			{
				requestHint = buffer.Length - (int)source.Remaining;
				return;
			}

			if (!Utf8Parser.TryParse(buffer, out int codePointOrHighSurrogate, out var consumed, standardFormat: 'X'))
			{
				JsonThrow.InvalidUnicodeEscapeSequence(source.Consumed - 2, buffer);
			}

			Debug.Assert(consumed == 4, $"consumed ({consumed}) == 4 for '{BitConverter.ToString(buffer.ToArray())}'");

			var positionOf1stSurrogate = source.Consumed - 2;
			if (Unicode.IsLowSurrogate(codePointOrHighSurrogate))
			{
				// 1st surrogte must be high surrogate.
				JsonThrow.OrphanSurrogate(positionOf1stSurrogate, codePointOrHighSurrogate);
			}

			source.Advance(consumed);
			result.AppendUtf16CodePoint(codePointOrHighSurrogate);

			if (Unicode.IsHighSurrogate(codePointOrHighSurrogate))
			{
				if (source.Remaining < 7)
				{
					requestHint = 7 - (int)source.Remaining;
					return;
				}

				// Search lower surrogate
				if (!source.IsNext((byte)'\\', advancePast: true))
				{
					// No paired low surrogate.
					JsonThrow.OrphanSurrogate(positionOf1stSurrogate, codePointOrHighSurrogate);
				}

				if (!source.IsNext((byte)'u', advancePast: true))
				{
					// No paired low surrogate.
					JsonThrow.OrphanSurrogate(positionOf1stSurrogate, codePointOrHighSurrogate);
				}

				source.Sequence.Slice(source.Consumed, 4).CopyTo(buffer);
				if (!Utf8Parser.TryParse(buffer, out int shouldBeLowSurrogate, out consumed, standardFormat: 'X'))
				{
					JsonThrow.InvalidUnicodeEscapeSequence(source.Consumed, buffer);
				}

				if (!Unicode.IsLowSurrogate(shouldBeLowSurrogate))
				{
					// No paired low surrogate.
					JsonThrow.OrphanSurrogate(positionOf1stSurrogate, codePointOrHighSurrogate);
				}

				Debug.Assert(consumed == 4, $"consumed ({consumed}) == 4 for '{BitConverter.ToString(buffer.ToArray())}'");

				source.Advance(consumed);
				result.AppendUtf16CodePoint(shouldBeLowSurrogate);
			}

			requestHint = 0;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override byte[]? DecodeNullableBinary(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(source, out _);
			if (this.TryReadNull(source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeBinaryCore(source, out requestHint, cancellationToken);
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override byte[]? DecodeBinary(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(source, out _);
			return this.DecodeBinaryCore(source, out requestHint, cancellationToken);
		}

		private byte[]? DecodeBinaryCore(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(source, out _, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			if (!source.TryReadTo(out ReadOnlySequence<byte> sequence, quotation, advancePastDelimiter: true))
			{
				// EoF
				requestHint = 1;
				source.Rewind(source.Consumed - startOffset);
				return default;
			}

			if (sequence.Length > this.Options.MaxBinaryLengthInBytes)
			{
				Throw.BinaryLengthExceeded(startOffset + 1, sequence.Length, this.Options.MaxBinaryLengthInBytes);
			}

			var resultLength = 0;
			var inputBuffer = sequence.IsSingleSegment ? null : this.Options.ByteBufferPool.Rent(this.Options.MaxByteBufferLength);
			try
			{
				var outputBuffer = this.Options.ByteBufferPool.Rent(this.Options.MaxByteBufferLength);
				try
				{
					ReadOnlySpan<byte> inputSpan = sequence.FirstSpan;
					Span<byte> outputSpan = outputBuffer;
					var status = OperationStatus.DestinationTooSmall;
					while (true)
					{
						status = Base64.DecodeFromUtf8(inputSpan, outputSpan, out var bytesConsumed, out var bytesWritten, isFinalBlock: sequence.IsSingleSegment);
						inputSpan = inputSpan.Slice(bytesConsumed);
						outputSpan = outputSpan.Slice(bytesWritten);

						resultLength += bytesWritten;
						switch (status)
						{
							case OperationStatus.Done:
							{
								// OK
								var result = new byte[resultLength];
								unsafe
								{
									fixed (byte* pBuffer = outputBuffer)
									fixed (byte* pResult = result)
									{
										Buffer.MemoryCopy(pBuffer, pResult, result.Length, resultLength);
									}
								}

								return result;
							}
							case OperationStatus.DestinationTooSmall:
							{
								// Realloc buffer and set span head.

								var newBuffer = this.Options.ByteBufferPool.Rent(outputBuffer.Length * 2);
								unsafe
								{
									fixed (byte* pBuffer = outputBuffer)
									fixed (byte* pNewBuffer = newBuffer)
									{
										Buffer.MemoryCopy(pBuffer, pNewBuffer, outputBuffer.Length, outputBuffer.Length);
									}
								}

								outputSpan = newBuffer.AsSpan(outputBuffer.Length - outputSpan.Length);
								this.Options.ByteBufferPool.Return(outputBuffer, this.Options.ClearsBuffer);
								outputBuffer = newBuffer;
								break;
							}
							case OperationStatus.NeedMoreData:
							{
								// In this case, ReadOnlySequence<byte> is multi segment.
								Debug.Assert(!sequence.IsSingleSegment, "!sequence.IsSingleSegment");
								Debug.Assert(inputBuffer != null, "inputBuffer != null");

								// Realloc input span.

								var consumed = inputBuffer.Length - inputSpan.Length;

								sequence = sequence.Slice(consumed);
								var newInputSpanLength = (int)Math.Min(inputBuffer.Length, sequence.Length);
								sequence.Slice(0, newInputSpanLength).CopyTo(inputBuffer);
								inputSpan = inputBuffer.AsSpan(0, newInputSpanLength);
								break;
							}
							default:
							{
								Debug.Assert(status == OperationStatus.InvalidData, $"status ({status}) == OperationStatus.InvalidData");
								JsonThrow.InvalidBase64(startOffset);
								// never
								return default;
							}
						}
					}
				}
				finally
				{
					this.Options.ByteBufferPool.Return(outputBuffer, this.Options.ClearsBuffer);
				}
			}
			finally
			{
				if (!(inputBuffer is null))
				{
					this.Options.ByteBufferPool.Return(inputBuffer, this.Options.ClearsBuffer);
				}
			}
		}
	}
}
