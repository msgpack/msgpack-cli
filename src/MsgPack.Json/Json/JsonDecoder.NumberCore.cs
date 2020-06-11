// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		public override bool DecodeBoolean(in SequenceReader<byte> source, out int requestHint)
		{
			this.ReadTrivia(source, out _);
			return this.DecodeBooleanCore(source, out requestHint);
		}

		private bool DecodeBooleanCore(in SequenceReader<byte> source, out int requestHint)
		{
			requestHint = 0;
			if (source.IsNext(JsonTokens.True, advancePast: true))
			{
				return true;
			}
			else if (source.IsNext(JsonTokens.False, advancePast: true))
			{
				return false;
			}
			else
			{
				requestHint = GetRequestHintForBoolean(source);
				return default;
			}
		}

		private static int GetRequestHintForBoolean(in SequenceReader<byte> source)
		{
			if (!source.TryPeek(out var b))
			{
				return 4;
			}

			switch (b)
			{
				case (byte)'t':
				{
					return GetRequestHintForBoolean(source, offset: 1, JsonTokens.True);
				}
				case (byte)'f':
				{
					return GetRequestHintForBoolean(source, offset: 1, JsonTokens.False);
				}
			}

			ThrowNonBooleanUtfSequence(source, source.Consumed);
			// never
			return default;
		}

		private static int GetRequestHintForBoolean(in SequenceReader<byte> source, int offset, ReadOnlySpan<byte> expected)
		{
			var subReeader = new SequenceReader<byte>(source.Sequence.Slice(source.Consumed + 1));

			for (int i = 0; i < expected.Length; i++)
			{
				if (!subReeader.TryRead(out var b))
				{
					return expected.Length - i;
				}

				if (b != expected[i])
				{
					ThrowNonBooleanUtfSequence(subReeader, subReeader.Consumed + source.Consumed);
				}
			}

			Debug.Fail("should be true/false...");
			return default;
		}

		private static void ThrowNonBooleanUtfSequence(in SequenceReader<byte> source, long position)
		{
			if (TryGetUtf8Unit(source, out var unit) == Utf8UnitStatus.Invalid)
			{
				JsonThrow.MalformedUtf8(unit, position);
			}
			else
			{
				JsonThrow.IsNotType(typeof(bool), unit, position);
			}
		}

		private double DecodeNumber(in SequenceReader<byte> source, out int requestHint)
		{
			var start = source.Position;
			var startOffset = source.Consumed;
			var length = 0L;
			// -? (0 | [1-9] \d*) (\. \d+ ([eE] [+-] \d+)? )?
			// sign
			if (source.IsNext(JsonNumberTokens.PlusMinusSigns, advancePast: true))
			{
				length++;
			}

			// 0
			if (source.IsNext((byte)'0', advancePast: true))
			{
				length++;
			}
			// [1-9]
			else if (source.IsNext(JsonNumberTokens.NonZeroDigits, advancePast: true))
			{
				length++;
				// \d*
				length += source.AdvancePastAny(JsonNumberTokens.Digits);
			}
			else
			{
				ThrowInvalidNumber(source);
			}

			// Check early for integer portion.
			if (length > this.Options.MaxNumberLengthInBytes)
			{
				JsonThrow.TooLongNumber(length, this.Options.MaxNumberLengthInBytes, startOffset);
			}

			// (\. ...)?
			if (source.IsNext((byte)'.', advancePast: true))
			{
				length++;

				if (source.End)
				{
					requestHint = 1;
					source.Rewind(length);
					return default;
				}

				// \d+
				length += ReadAtLeastOneDigits(source, out requestHint);
				if (requestHint != 0)
				{
					source.Rewind(length);
					return default;
				}

				// ([eE] ..)?
				if (source.IsNext(JsonNumberTokens.ExponentialIndicators, advancePast: true))
				{
					// [+-]
					if (source.IsNext(JsonNumberTokens.PlusMinusSigns, advancePast: true))
					{
						// \d+
						length += ReadAtLeastOneDigits(source, out requestHint);
						if (requestHint != 0)
						{
							source.Rewind(length);
							return default;
						}
					}
					else
					{
						ThrowInvalidNumber(source);
					}
				}
			}

			if (length > this.Options.MaxNumberLengthInBytes)
			{
				JsonThrow.TooLongNumber(length, this.Options.MaxNumberLengthInBytes, startOffset);
			}

			var readDirect = source.UnreadSpan.Length <= length;

			var length32 = (int)length;
			byte[]? arrayBuffer = default;
			if (!readDirect && length > 32)
			{
				// length should be Int32 because MaxNumberLength is Int32 type.
				arrayBuffer = this.Options.ByteBufferPool.Rent(length32);
			}

			try
			{
				Span<byte> buffer =
					readDirect ?
						default :
						arrayBuffer ?? (stackalloc byte[length32]);
				var number = GetSpan(source.Sequence.Slice(start, length32), buffer);

				var shouldBeTrue = Utf8Parser.TryParse(number, out double result, out var shouldBeLength);
				Debug.Assert(shouldBeTrue, "Utf8Parser.TryParse returns false!");
				Debug.Assert(shouldBeLength == length, $"Utf8Parser.TryParse outputs ({shouldBeLength}:#,0) is not {length:#,0}!");
				requestHint = 0;
				source.Advance(length);
				return result;
			}
			finally
			{
				if (arrayBuffer != null)
				{
					this.Options.ByteBufferPool.Return(arrayBuffer, this.Options.ClearsBuffer);
				}
			}
		}

		private static ReadOnlySpan<byte> GetSpan(in ReadOnlySequence<byte> source, Span<byte> buffer)
		{
			if (source.IsSingleSegment)
			{
				Debug.Assert(source.FirstSpan.Length == source.Length, "source.FirstSpan.Length == source.Length");
				return source.FirstSpan;
			}

			Debug.Assert(source.Length == buffer.Length, "source.Length == buffer.Length");
			source.CopyTo(buffer);
			return buffer;
		}

		private static long ReadAtLeastOneDigits(in SequenceReader<byte> source, out int requestHint)
		{
			if (source.End)
			{
				requestHint = 1;
				return default;
			}

			var decimalLength = source.AdvancePastAny(JsonNumberTokens.Digits);
			if (decimalLength == 0)
			{
				ThrowInvalidNumber(source);
			}

			requestHint = 0;
			return decimalLength;
		}

		private static void ThrowInvalidNumber(in SequenceReader<byte> source)
		{
			var position = source.Consumed;
			if (TryGetUtf8Unit(source, out var unit) == Utf8UnitStatus.Valid)
			{
				JsonThrow.IsNotType(typeof(double), unit, position);
			}
			else
			{
				JsonThrow.MalformedUtf8(unit, position);
			}
		}
	}
}
