// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;
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
				Throw.ArgumentNull(paramName);
				Debug.Assert(value != null);
			}

			return value;
		}

		public static int IsNotNegative(int value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			if (value < 0)
			{
				Throw.ArgumentOutOfRange(paramName, "Value cannot be negative number.");
			}

			return value;
		}

		public static T IsNotLessThan<T>(T value, T minInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(minInclusive) < 0)
			{
				Throw.ArgumentOutOfRange(paramName, $"Value cannot be less than {minInclusive}.");
			}

			return value;
		}

		public static T IsNotGreaterThan<T>(T value, T maxInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(maxInclusive) > 0)
			{
				Throw.ArgumentOutOfRange(paramName, $"Value cannot be greater than {maxInclusive}.");
			}

			return value;
		}

		public static T IsBetween<T>(T value, T minInclusive, T maxInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(minInclusive) < 0)
			{
				Throw.ArgumentOutOfRange(paramName, $"Value cannot be less than {minInclusive}.");
			}

			if (value.CompareTo(maxInclusive) > 0)
			{
				Throw.ArgumentOutOfRange(paramName, $"Value cannot be greater than {maxInclusive}.");
			}

			return value;
		}
	}
}
