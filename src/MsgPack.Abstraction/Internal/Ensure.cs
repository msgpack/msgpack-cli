// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	internal static class Ensure
	{
		[return: NotNull]
		public static T NotNull<T>([NotNull]T value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}

			return value;
		}

		public static int IsNotNegative(int value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative number.");
			}

			return value;
		}

		public static T IsNotLessThan<T>(T value, T minInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(minInclusive) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, $"Value cannot be less than {minInclusive}.");
			}

			return value;
		}

		public static T IsNotGreaterThan<T>(T value, T maxInclusive, [CallerArgumentExpression("value")] string paramName = null!)
			where T : struct, IComparable<T>
		{
			if (value.CompareTo(maxInclusive) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, $"Value cannot be greater than {maxInclusive}.");
			}

			return value;
		}
	}
}
