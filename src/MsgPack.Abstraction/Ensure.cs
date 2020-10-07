// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MsgPack
{
	internal static class Ensure
	{
		[return: NotNull]
		public static T NotNull<T>([NotNull]T value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			if (value == null)
			{
				ThrowArgumentNull(paramName);
			}

			return value;
		}

		public static string NotNullNorEmpty([NotNull]string? value, [CallerArgumentExpression("value")] string paramName = null!)
		{
			NotNull(value);

			if (value.Length ==0)
			{
				ThrowArgumentEmptyString(paramName);
			}

			return value;
		}

		public static string NotBlank([NotNull] string? value, [CallerArgumentExpression("value")] string paramName = null!)
		{
			NotNull(value);

			if (String.IsNullOrWhiteSpace(value))
			{
				ThrowArgumentBlankString(paramName);
			}

			return value;
		}

		public static string NotBlankAndTrimmed([NotNull] string? value, [CallerArgumentExpression("value")] string paramName = null!)
		{
			NotNull(value);

			if (String.IsNullOrWhiteSpace(value))
			{
				ThrowArgumentBlankString(paramName);
			}

			return value.Trim();
		}

		public static int IsNotNegative(int value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			if (value < 0)
			{
				ThrowArgumentOutOfRange(paramName, "Value cannot be negative number.");
			}

			return value;
		}

		public static T IsNotLessThan<T>(T value, T minInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(minInclusive) < 0)
			{
				ThrowArgumentOutOfRange(paramName, $"Value cannot be less than {minInclusive}.");
			}

			return value;
		}

		public static T IsNotGreaterThan<T>(T value, T maxInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(maxInclusive) > 0)
			{
				ThrowArgumentOutOfRange(paramName, $"Value cannot be greater than {maxInclusive}.");
			}

			return value;
		}

		public static T IsBetween<T>(T value, T minInclusive, T maxInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(minInclusive) < 0)
			{
				ThrowArgumentOutOfRange(paramName, $"Value cannot be less than {minInclusive}.");
			}

			if (value.CompareTo(maxInclusive) > 0)
			{
				ThrowArgumentOutOfRange(paramName, $"Value cannot be greater than {maxInclusive}.");
			}

			return value;
		}

		[DoesNotReturn]
		private static void ThrowArgumentNull(string paramName)
			=> throw new ArgumentNullException(paramName);

		[DoesNotReturn]
		private static void ThrowArgumentOutOfRange(string paramName, string message)
			=> throw new ArgumentOutOfRangeException(paramName, message);

		[DoesNotReturn]
		private static void ThrowArgumentEmptyString(string paramName)
			=> throw new ArgumentException("Value cannot be empty string.", paramName);

		[DoesNotReturn]
		private static void ThrowArgumentBlankString(string paramName)
			=> throw new ArgumentException("Value cannot be empty or blank string.", paramName);
	}
}
