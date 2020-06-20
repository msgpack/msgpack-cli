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
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override bool GetRawString(ref SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, CancellationToken cancellationToken = default)
		{
			if (!this.GetRawStringCore(ref source, out var rawStringSequence, out requestHint, cancellationToken))
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

		private bool GetRawStringCore(ref SequenceReader<byte> source, out ReadOnlySequence<byte> rawString, out int requestHint, CancellationToken cancellationToken = default)
		{ 
			this.ReadTrivia(ref source);
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(ref source, out requestHint);
			if (requestHint != 0)
			{
				rawString = default;
				return false;
			}

			if (!source.TryReadTo(out ReadOnlySequence<byte> sequence, quotation, (byte)'\\', advancePastDelimiter: false))
			{
				// EoF
				requestHint = 1;
				rawString = default;
				source.Rewind(source.Consumed - startOffset);
				return false;
			}

			rawString = sequence;
			source.Advance(1); // skip end quote
			return true;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override string? DecodeNullableString(ref SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(ref source);
			if (this.TryDecodeNull(ref source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeStringCore(ref source, out requestHint, cancellationToken);
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override string? DecodeString(ref SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(ref source);
			return this.DecodeStringCore(ref source, out requestHint, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private string? DecodeStringCore(ref SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(ref source, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			// fast-path
			if (!source.TryReadTo(out ReadOnlySequence<byte> sequence, quotation, (byte)'\\', advancePastDelimiter: false) || !source.TryRead(out var delimiter))
			{
				// EoF
				requestHint = 1;
				source.Rewind(source.Consumed - startOffset);
				return default;
			}

			if (sequence.Length > this.Options.MaxStringLengthInBytes)
			{
				Throw.StringLengthExceeded(source.Consumed - sequence.Length, sequence.Length, this.Options.MaxBinaryLengthInBytes);
			}

			if (delimiter == quotation)
			{
				return Encoding.UTF8.GetString(sequence);
			}

			return this.DecodeStringCoreSlow(ref source, out requestHint, startOffset, quotation, ref sequence, delimiter, cancellationToken);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string? DecodeStringCoreSlow(ref SequenceReader<byte> source, out int requestHint, long startOffset, byte quotation, ref ReadOnlySequence<byte> sequence, byte delimiter, CancellationToken cancellationToken)
		{
			var length = sequence.Length;
			using (var result = new StringBuilderBufferWriter(new StringBuilder(), this.Options))
			{
				Encoding.UTF8.GetChars(sequence, result);
				length += (int)sequence.Length;

				// We accept unescaped charactors except quotation even if the value is '\0' because we can handle them correctly.
				while (true)
				{
					// Decode escape sequence
					if (source.End)
					{
						// EoF
						requestHint = 2;
						break;
					}

					if (!source.IsNext((byte)'u', advancePast: true))
					{
						DecodeSpetialEscapeSequence(ref source, result);
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

					cancellationToken.ThrowIfCancellationRequested();

					if (!source.TryReadTo(out sequence, quotation, (byte)'\\', advancePastDelimiter: false))
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
					source.TryRead(out delimiter);
					if (delimiter == quotation)
					{
						// End
						requestHint = 0;
						return result.ToString();
					}
				}
			}

			// No closing quotation.
			return default;
		}

		private byte ReadStringStart(ref SequenceReader<byte> source, out int requestHint)
		{
			if (source.End)
			{
				requestHint = 2;
				return default;
			}

			requestHint = 0;

			if (source.IsNext((byte)'"', advancePast: false))
			{
				source.Advance(1);
				return (byte)'"';
			}
			else
			{
				if ((this.Options.ParseOptions & JsonParseOptions.AllowSingleQuotationString) != 0)
				{
					// ['"]
					if (source.IsNext((byte)'\'', advancePast: false))
					{
						source.Advance(1);
						return (byte)'\'';
					}
					else
					{
						JsonThrow.IsNotStringStart(source.Consumed, JsonStringTokens.AnyQuotations);
						// never
						return default;
					}
				}
				else
				{
					JsonThrow.IsNotStringStart(source.Consumed, JsonStringTokens.DoubleQuotation);
					// never
					return default;
				}
			}
		}

		private static void DecodeSpetialEscapeSequence(ref SequenceReader<byte> source, StringBuilderBufferWriter result)
		{
			source.TryPeek(out var escaped);
			char unescaped;
			switch(escaped)
			{
				case (byte)'b':
				{
					unescaped = '\b';
					break;
				}
				case (byte)'t':
				{
					unescaped = '\t';
					break;
				}
				case (byte)'f':
				{
					unescaped = '\f';
					break;
				}
				case (byte)'r':
				{
					unescaped = '\r';
					break;
				}
				case (byte)'n':
				{
					unescaped = '\n';
					break;
				}
				case (byte)'"':
				{
					unescaped = '"';
					break;
				}
				case (byte)'\\':
				{
					unescaped = '\\';
					break;
				}
				case (byte)'/':
				{
					unescaped = '/';
					break;
				}
				default:
				{
					JsonThrow.InvalidEscapeSequence(source.Consumed - 1, escaped);
					// never
					unescaped = default;
					break;
				}
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

				if(!source.TryCopyTo(buffer))
				{
					requestHint = buffer.Length - (int)source.Remaining;
					return;
				}

				source.Advance(4);

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
		public sealed override byte[]? DecodeNullableBinary(ref SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(ref source);
			if (this.TryDecodeNull(ref source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeBinaryCore(ref source, out requestHint, cancellationToken);
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override byte[]? DecodeBinary(ref SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(ref source);
			return this.DecodeBinaryCore(ref source, out requestHint, cancellationToken);
		}

		private byte[]? DecodeBinaryCore(ref SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			var startOffset = source.Consumed;

			var quotation = this.ReadStringStart(ref source, out requestHint);
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
