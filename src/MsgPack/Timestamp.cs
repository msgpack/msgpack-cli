// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack
{
	/// <summary>
	///		Represents high resolution timestamp for MessagePack eco-system.
	/// </summary>
	/// <remarks>
	///     The <c>timestamp</c> consists of 64bit Unix epoc seconds and 32bit unsigned nanoseconds offset from the calculated datetime with the epoc.
	///     So this type supports wider range than <see cref="System.DateTime" /> and <see cref="System.DateTimeOffset" /> and supports 1 or 10 nano seconds precision.
	///     However, this type does not support local date time and time zone information, so this type always represents UTC time.
	/// </remarks>
#if FEATURE_BINARY_SERIALIZATION
    [Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public partial struct Timestamp
	{
		/// <summary>
		///		MessagePack ext type code for msgpack timestamp type.
		/// </summary>
		public const byte TypeCode = 0xFF;

		/// <summary>
		///		An instance represents zero. This is 1970-01-01T00:00:00.000000000.
		/// </summary>
		public static readonly Timestamp Zero = new Timestamp(0, 0);

		/// <summary>
		///		An instance represents minimum value of this instance. This is <c>[<see cref="Int64.MinValue"/>, 0]</c> in encoded format.
		/// </summary>
		public static readonly Timestamp MinValue = new Timestamp(Int64.MinValue, 0);

		/// <summary>
		///		An instance represents maximum value of this instance. This is <c>[<see cref="Int64.MaxValue"/>, 999999999]</c> in encoded format.
		/// </summary>
		public static readonly Timestamp MaxValue = new Timestamp(Int64.MaxValue, MaxNanoSeconds);

		private static readonly int[] LastDays =
			new[]
			{
				0, // There are no month=0
				31,
				0, // 28 or 29
				31,
				30,
				31,
				30,
				31,
				31,
				30,
				31,
				30,
				31
			};

		private const long MinUnixEpochSecondsForTicks = -62135596800L;
		private const long MaxUnixEpochSecondsForTicks = 253402300799;

		private const int MaxNanoSeconds = 999999999;

		private const long UnixEpochTicks = 621355968000000000;
		private const long UnixEpochInSeconds = 62135596800;
		private const int SecondsToTicks = 10 * 1000 * 1000;
		private const int NanoToTicks = 100;
		private const int SecondsToNanos = 1000 * 1000 * 1000;

		private readonly long unixEpochSeconds;
		private readonly uint nanoseconds; // 0 - 999,999,999

		/// <summary>
		///		Initializes a new instance of <see cref="Timestamp"/> structure.
		/// </summary>
		/// <param name="unixEpochSeconds">A unit epoc seconds part of the msgpack timestamp.</param>
		/// <param name="nanoseconds">A unit nanoseconds part of the msgpack timestamp.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="nanoseconds"/> is negative or is greater than <c>999,999,999</c> exclusive.
		/// </exception>
		public Timestamp(long unixEpochSeconds, int nanoseconds)
		{
			if (nanoseconds > MaxNanoSeconds || nanoseconds < 0)
			{
				throw new ArgumentOutOfRangeException("nanoseconds", "nanoseconds must be non negative value and lessor than 999,999,999.");
			}

			this.unixEpochSeconds = unixEpochSeconds;
			this.nanoseconds = unchecked((uint)nanoseconds);
		}

		internal static Timestamp FromComponents(ref Value value, bool isLeapYear)
		{
			long epoc;
			checked
			{
				var days = YearsToDaysOfNewYear(value.Year) + ToDaysOffsetFromNewYear(value.Month, value.Day, isLeapYear) - Timestamp.UnixEpochInSeconds / Timestamp.SecondsPerDay;
				// First set time offset to avoid overflow.
				epoc = value.Hour * 60 * 60;
				epoc += value.Minute * 60;
				epoc += value.Second;
				if (days < 0)
				{
					// Avoid right side overflow.
					epoc += (days + 1) * Timestamp.SecondsPerDay;
					epoc -= Timestamp.SecondsPerDay;
				}
				else
				{
					epoc += days * Timestamp.SecondsPerDay;
				}
			}

			return new Timestamp(epoc, unchecked((int)value.Nanoseconds));
		}

		private static long YearsToDaysOfNewYear(long years)
		{
			long remainOf400Years, remainOf100Years, remainOf4Years;

			// For AD, uses offset from 0001, so decrement 1 at first.
			var numberOf400Years = DivRem(years > 0 ? (years - 1) : years, 400, out remainOf400Years);
			var numberOf100Years = DivRem(remainOf400Years, 100, out remainOf100Years);
			var numberOf4Years = DivRem(remainOf100Years, 4, out remainOf4Years);
			var days =
				DaysPer400Years * numberOf400Years +
				DaysPer100Years * numberOf100Years +
				DaysPer4Years * numberOf4Years +
				DaysPerYear * remainOf4Years;
			if (years <= 0)
			{
				// For BC, subtract year 0000 offset.
				days -= (DaysPerYear + 1);
			}

			return days;
		}

		private static int ToDaysOffsetFromNewYear(int month, int day, bool isLeapYear)
		{
			var result = -1; // 01-01 should be 0, so starts with -1.
			for (var i = 1; i < month; i++)
			{
				result += LastDays[i];
				if (i == 2)
				{
					result += isLeapYear ? 29 : 28;
				}
			}

			result += day;
			return result;
		}

#if NETSTANDARD1_1 || NETSTANDARD1_3 || SILVERLIGHT

		// Slow alternative
		internal static long DivRem( long dividend, long divisor, out long remainder )
		{
			remainder = dividend % divisor;
			return dividend / divisor;
		}

#else // NETSTANDARD1_1 || NETSTANDARD1_3 || SILVERLIGHT

		internal static long DivRem(long dividend, long divisor, out long remainder)
		{
			return Math.DivRem(dividend, divisor, out remainder);
		}

#endif // NETSTANDARD1_1 || NETSTANDARD1_3 || SILVERLIGHT

#if UNITY && DEBUG
		public
#else
		internal
#endif
		struct Value
		{
			public long Year;
			public int Month;
			public int Day;
			public int Hour;
			public int Minute;
			public int Second;
			public uint Nanoseconds;

			public Value(Timestamp encoded)
			{
				int dayOfYear;
				encoded.GetDatePart(out this.Year, out this.Month, out this.Day, out dayOfYear);
				this.Hour = encoded.Hour;
				this.Minute = encoded.Minute;
				this.Second = encoded.Second;
				this.Nanoseconds = encoded.nanoseconds;
			}

#if DEBUG
			public Value(long year, int month, int day, int hour, int minute, int second, uint nanoseconds)
			{
				this.Year = year;
				this.Month = month;
				this.Day = day;
				this.Hour = hour;
				this.Minute = minute;
				this.Second = second;
				this.Nanoseconds = nanoseconds;
			}
#endif // DEBUG
		}
	}
}
