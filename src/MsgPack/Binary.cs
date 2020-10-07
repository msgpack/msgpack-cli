// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Text;
using MsgPack.Internal;

namespace MsgPack
{
	/// <summary>
	///		Defines binary related utilities.
	/// </summary>
	internal static class Binary
	{
		/// <summary>
		///		Singleton empty <see cref="Byte"/>[].
		/// </summary>
		public static readonly byte[] Empty = new byte[0];

		public static string ToHexString(ReadOnlySpan<byte> blob)
		{
			return ToHexString(blob, true);
		}

		public static string ToHexString(ReadOnlySpan<byte> blob, bool withPrefix)
		{
			if (blob == null || blob.Length == 0)
			{
				return String.Empty;
			}

			var buffer = new StringBuilder(blob.Length * 2 + (withPrefix ? 2 : 0));
			ToHexStringCore(blob, buffer, withPrefix);
			return buffer.ToString();
		}

		public static void ToHexString(ReadOnlySpan<byte> blob, StringBuilder buffer)
		{
			if (blob == null || blob.Length == 0)
			{
				return;
			}

			ToHexStringCore(blob, buffer, true);
		}

		private static void ToHexStringCore(ReadOnlySpan<byte> blob, StringBuilder buffer, bool withPrefix)
		{
			if (withPrefix)
			{
				buffer.Append("0x");
			}

			foreach (var b in blob)
			{
				buffer.Append(ToHexChar(b >> 4));
				buffer.Append(ToHexChar(b & 0xF));
			}
		}

		private static char ToHexChar(int b)
		{
			if (b < 10)
			{
				return unchecked((char)('0' + b));
			}
			else
			{
				return unchecked((char)('A' + (b - 10)));
			}
		}

#if FEATURE_ADVANCED_BIT_CONVERTER
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static int ToBits(float value)
			=> BitConverter.SingleToInt32Bits(value);
#else
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static unsafe int ToBits(float value)
			=> *((int*)&value);
#endif

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static long ToBits(double value)
			=> BitConverter.DoubleToInt64Bits(value);

#if FEATURE_ADVANCED_BIT_CONVERTER
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static float FromBits(int value)
			=> BitConverter.Int32BitsToSingle(value);
#else
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static unsafe float FromBits(int value)
			=> *((float*)&value);
#endif

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static double FromBits(long value)
			=> BitConverter.Int64BitsToDouble(value);
	}
}
