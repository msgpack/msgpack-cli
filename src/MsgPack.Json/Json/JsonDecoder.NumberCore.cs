// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override bool DecodeBoolean(ref SequenceReader<byte> source, out int requestHint)
		{
			this.ReadTrivia(ref source);
			return this.DecodeBooleanCore(ref source, out requestHint);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private bool DecodeBooleanCore(ref SequenceReader<byte> source, out int requestHint)
		{
			ReadOnlySpan<byte> @true;
			ReadOnlySpan<byte> @false;
			unsafe
			{
				byte* pTrue = stackalloc byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e', };
				@true = new ReadOnlySpan<byte>(pTrue, 4);
				byte* pFalse = stackalloc byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e', };
				@false = new ReadOnlySpan<byte>(pFalse, 5);
			}

			requestHint = 0;
			if (source.IsNext(@true, advancePast: true))
			{
				return true;
			}
			else if (source.IsNext(@false, advancePast: true))
			{
				return false;
			}
			else
			{
				requestHint = GetRequestHintForBoolean(ref source);
				return default;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int GetRequestHintForBoolean(ref SequenceReader<byte> source)
		{
			if (!source.TryPeek(out var b))
			{
				return 4;
			}

			switch (b)
			{
				case (byte)'t':
				{
					ReadOnlySpan<byte> @true;
					unsafe
					{
						byte* pTrue = stackalloc byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
						@true = new ReadOnlySpan<byte>(pTrue, 4);
					}

					return GetRequestHintForBoolean(ref source, offset: 1, @true);
				}
				case (byte)'f':
				{
					ReadOnlySpan<byte> @false;
					unsafe
					{
						byte* pFalse = stackalloc byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
						@false = new ReadOnlySpan<byte>(pFalse, 5);
					}

					return GetRequestHintForBoolean(ref source, offset: 1, @false);
				}
			}

			ThrowNonBooleanUtfSequence(ref source, source.Consumed);
			// never
			return default;
		}

		private static int GetRequestHintForBoolean(ref SequenceReader<byte> source, int offset, ReadOnlySpan<byte> expected)
		{
			Span<byte> buffer = stackalloc byte[(int)Math.Min(expected.Length, source.Remaining)];
			source.TryCopyTo(buffer);

			for (var i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] != expected[i])
				{
					ThrowNonBooleanUtfSequence(ref source, source.Consumed + i);
				}
			}

			return expected.Length - buffer.Length;
		}

		private static void ThrowNonBooleanUtfSequence(ref SequenceReader<byte> source, long position)
		{
			if (TryGetUtf8Unit(ref source, out var unit) == Utf8UnitStatus.Invalid)
			{
				JsonThrow.MalformedUtf8(unit, position);
			}
			else
			{
				JsonThrow.IsNotType(typeof(bool), unit, position);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private double DecodeNumber(ref SequenceReader<byte> source, out int requestHint)
		{
			var lengthReader = source;
			ReadOnlySpan<byte> plusMinusSigns;
			ReadOnlySpan<byte> nonZeroDigits;
			ReadOnlySpan<byte> digits;
			unsafe
			{
				byte* pPlusMinusSigns = stackalloc[] { (byte)'+', (byte)'-' };
				plusMinusSigns = new ReadOnlySpan<byte>(pPlusMinusSigns, 2);
				byte* pDigits = stackalloc[] { (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7', (byte)'8', (byte)'9' };
				digits = new ReadOnlySpan<byte>(pDigits, 9);
				nonZeroDigits = digits.Slice(1);
			}

			var length = 0L;
			// -? (0 | [1-9] \d*) (\. \d+ ([eE] [+-] \d+)? )?
			// sign
			length += lengthReader.AdvancePastAny((byte)'+', (byte)'-');

			// 0
			if (lengthReader.IsNext((byte)'0', advancePast: true))
			{
				length++;
			}
			// [1-9]
			else
			{
				var nonZeroLength = lengthReader.AdvancePastAny(nonZeroDigits);
				if (nonZeroLength > 0)
				{
					length += nonZeroLength;
					// \d*
					length += lengthReader.AdvancePastAny(digits);
				}
				else
				{
					ThrowInvalidNumber(ref lengthReader);
				}
			}

			// Check early for integer portion.
			if (length > this.Options.MaxNumberLengthInBytes)
			{
				JsonThrow.TooLongNumber(length, this.Options.MaxNumberLengthInBytes, source.Consumed);
			}

			// (\. ...)?
			if (lengthReader.IsNext((byte)'.', advancePast: true))
			{
				length++;

				if (lengthReader.End)
				{
					requestHint = 1;
					return default;
				}

				// \d+
				length += ReadAtLeastOneDigits(ref lengthReader, digits, out requestHint);
				if (requestHint != 0)
				{
					return default;
				}

				ReadOnlySpan<byte> exponentialIndicators;
				unsafe
				{
					byte* pExponentialIndicators = stackalloc byte[] { (byte)'E', (byte)'e' };
					exponentialIndicators = new ReadOnlySpan<byte>(pExponentialIndicators, 2);
				}

				// ([eE] ..)?
				if (lengthReader.IsNext(exponentialIndicators, advancePast: true))
				{
					// [+-]
					if (lengthReader.IsNext(plusMinusSigns, advancePast: true))
					{
						// \d+
						length += ReadAtLeastOneDigits(ref lengthReader, digits, out requestHint);
						if (requestHint != 0)
						{
							return default;
						}
					}
					else
					{
						ThrowInvalidNumber(ref lengthReader);
					}
				}
			}

			if (length > this.Options.MaxNumberLengthInBytes)
			{
				JsonThrow.TooLongNumber(length, this.Options.MaxNumberLengthInBytes, source.Consumed);
			}

			if (source.UnreadSpan.Length >= length)
			{
				var shouldBeTrue = Utf8Parser.TryParse(source.UnreadSpan, out double result, out var consumed);
				Debug.Assert(shouldBeTrue, "Utf8Parser.TryParse returns false!");
				Debug.Assert(consumed == length, $"Utf8Parser.TryParse outputs ({consumed}:#,0) is not {length:#,0}!");
				source.Advance(consumed);
				requestHint = 0;
				return result;
			}

			var length32 = (int)length;
			byte[]? arrayBuffer = default;
			if (length > 32)
			{
				// length should be Int32 because MaxNumberLength is Int32 type.
				arrayBuffer = this.Options.ByteBufferPool.Rent(length32);
			}

			try
			{
				Span<byte> buffer = arrayBuffer ?? (stackalloc byte[length32]);
				source.TryCopyTo(buffer);

				var shouldBeTrue = Utf8Parser.TryParse(buffer, out double result, out var consumed);
				Debug.Assert(shouldBeTrue, "Utf8Parser.TryParse returns false!");
				Debug.Assert(consumed == length, $"Utf8Parser.TryParse outputs ({consumed}:#,0) is not {length:#,0}!");
				source.Advance(consumed);
				requestHint = 0;
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

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static long ReadAtLeastOneDigits(ref SequenceReader<byte> source, ReadOnlySpan<byte> digits, out int requestHint)
		{
			if (source.End)
			{
				requestHint = 1;
				return default;
			}

			var decimalLength = source.AdvancePastAny(digits);
			if (decimalLength == 0)
			{
				ThrowInvalidNumber(ref source);
			}

			requestHint = 0;
			return decimalLength;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowInvalidNumber(ref SequenceReader<byte> source)
		{
			var position = source.Consumed;
			if (TryGetUtf8Unit(ref source, out var unit) == Utf8UnitStatus.Valid)
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
