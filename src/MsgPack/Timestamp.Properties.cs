// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;

namespace MsgPack
{
	public partial struct Timestamp
	{
		private const int SecondsPerMinutes = 60;
		private const int SecondsPerHours = SecondsPerMinutes * 60;
		private const int SecondsPerDay = SecondsPerHours * 24;

		private const int DaysPerYear = 365;
		private const int DaysPer4Years = DaysPerYear * 4 + 1;
		private const int DaysPer100Years = DaysPer4Years * 25 - 1;
		private const int DaysPer400Years = DaysPer100Years * 4 + 1;

		private const int DayOfWeekOfEpoc = 4; // day of week of 1970-01-01

		private static readonly uint[] DaysToMonth365 = new uint[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
		private static readonly uint[] DaysToMonth366 = new uint[] { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
		private static readonly uint[] ReversedDaysToMonth365 = new uint[] { 0, 31, 61, 92, 122, 153, 184, 214, 245, 275, 306, 334, 365 };
		private static readonly uint[] ReversedDaysToMonth366 = new uint[] { 0, 31, 61, 92, 122, 153, 184, 214, 245, 275, 306, 335, 366 };

		/// <summary>
		///		Gets a unix epoch seconds part of msgpack timestamp spec.
		/// </summary>
		/// <value>A value of unix epoch seconds part of msgpack timestamp spec. This value may be negative, BC dates, and dates after 9999-12-31.</value>
		/// <remarks>
		///		If you want to get "nanosecond" portion of this instance, use <see cref="Nanosecond"/> property instead.
		/// </remarks>
		public long UnixEpochSecondsPart
		{
			get { return this.unixEpochSeconds; }
		}

		/// <summary>
		///		Gets a nanoseconds part of msgpack timestamp spec.
		/// </summary>
		/// <value>A value of nanoseconds part of msgpack timestamp spec. This value will be between 0 to 999,999,999.</value>
		/// <remarks>
		///		If you want to get "nanosecond" portion of this instance, use <see cref="Nanosecond"/> property instead.
		/// </remarks>
		public int NanosecondsPart
		{
			get { return unchecked((int)this.nanoseconds); }
		}

		/// <summary>
		///		Gets an year portion of this instance.
		/// </summary>
		/// <value>An year portion of this instance. The value may be zero or negative, and may exceed 9,999.</value>
		public long Year
		{
			get
			{
				long year;
				int month, day, dayOfYear;
				this.GetDatePart(out year, out month, out day, out dayOfYear);
				return year;
			}
		}

		/// <summary>
		///		Gets a month portion of this instance.
		/// </summary>
		/// <value>A month portion of this instance. The value will be between 1 and 12.</value>
		public int Month
		{
			get
			{
				long year;
				int month, day, dayOfYear;
				this.GetDatePart(out year, out month, out day, out dayOfYear);
				return month;
			}
		}

		/// <summary>
		///		Gets a day portion of this instance.
		/// </summary>
		/// <value>A day portion of this instance. The value will be valid day of <see cref="Month"/>.</value>
		public int Day
		{
			get
			{
				long year;
				int month, day, dayOfYear;
				this.GetDatePart(out year, out month, out day, out dayOfYear);
				return day;
			}
		}

		/// <summary>
		///		Gets an hour portion of this instance.
		/// </summary>
		/// <value>An hour portion of this instance. The value will be between 0 and 59.</value>
		public int Hour
		{
			get
			{
				long remainder;
				var hour = DivRem(this.unixEpochSeconds, SecondsPerHours, out remainder) % 24;
				unchecked
				{
					return
						(int)(this.unixEpochSeconds < 0
							? hour + (remainder != 0 ? 23 : (hour < 0 ? 24 : 0))
							: hour
						);
				}
			}
		}

		/// <summary>
		///		Gets a minute portion of this instance.
		/// </summary>
		/// <value>A minute portion of this instance. The value will be between 0 and 59.</value>
		public int Minute
		{
			get
			{
				long remainder;
				var minute = DivRem(this.unixEpochSeconds, SecondsPerMinutes, out remainder) % 60;
				unchecked
				{
					return
						(int)(this.unixEpochSeconds < 0
							? minute + (remainder != 0 ? 59 : (minute < 0 ? 60 : 0))
							: minute
						);
				}
			}
		}

		/// <summary>
		///		Gets a second portion of this instance.
		/// </summary>
		/// <value>A second portion of this instance. The value will be between 0 and 59.</value>
		public int Second
		{
			get
			{
				var second = unchecked((int)(this.unixEpochSeconds % 60));
				return second < 0 ? second + 60 : second;
			}
		}

		/// <summary>
		///		Gets a millisecond portion of this instance.
		/// </summary>
		/// <value>A millisecond portion of this instance. The value will be between 0 and 999.</value>
		public int Millisecond
		{
			get { return this.NanosecondsPart / (1000 * 1000); }
		}

		/// <summary>
		///		Gets a microsecond portion of this instance.
		/// </summary>
		/// <value>A microsecond portion of this instance. The value will be between 0 and 999.</value>
		public int Microsecond
		{
			get { return (this.NanosecondsPart / 1000) % 1000; }
		}

		/// <summary>
		///		Gets a nanosecond portion of this instance.
		/// </summary>
		/// <value>A nanosecond portion of this instance. The value will be between 0 and 999.</value>
		/// <remarks>
		///		If you want to get "nanoseconds" part of msgpack timestamp spec, use <see cref="NanosecondsPart"/> property instead.
		/// </remarks>
		public int Nanosecond
		{
			get { return (this.NanosecondsPart) % 1000; }
		}

		/// <summary>
		///		Gets a <see cref="Timestamp"/> which only contains date portion of this instance.
		/// </summary>
		/// <value>A <see cref="Timestamp"/> which only contains date portion of this instance.</value>
		public Timestamp Date
		{
			get
			{
				return new Timestamp(this.unixEpochSeconds - this.TimeOfDay.unixEpochSeconds, 0);
			}
		}

		/// <summary>
		///		Gets a <see cref="Timestamp"/> which only contains time portion of this instance.
		/// </summary>
		/// <value>A <see cref="Timestamp"/> which only contains time portion of this instance.</value>
		public Timestamp TimeOfDay
		{
			get
			{
				return
					new Timestamp(
						this.unixEpochSeconds < 0 ? (this.unixEpochSeconds % SecondsPerDay + SecondsPerDay) : (this.unixEpochSeconds % SecondsPerDay),
						this.NanosecondsPart
					);
			}
		}

		/// <summary>
		///		Gets a <see cref="DayOfWeek"/> of this day.
		/// </summary>
		/// <value>A <see cref="DayOfWeek"/> of this day.</value>
		public DayOfWeek DayOfWeek
		{
			get
			{
				long remainder;
				var divided = unchecked((int)DivRem(this.unixEpochSeconds, SecondsPerDay, out remainder));
				return
					(DayOfWeek)(
						(
							(this.unixEpochSeconds < 0
								? ((divided + ((int)remainder < 0 ? -1 : 0)) % 7 + 7)
								: divided
							) + DayOfWeekOfEpoc
						) % 7
					);
			}
		}

		/// <summary>
		///		Gets a number of days of this year.
		/// </summary>
		/// <value>A number of days of this year.</value>
		public int DayOfYear
		{
			get
			{
				long year;
				int month, day, dayOfYear;
				this.GetDatePart(out year, out month, out day, out dayOfYear);
				return dayOfYear;
			}
		}

		/// <summary>
		///		Gets a value which indicates <see cref="Year"/> is leap year or not.
		/// </summary>
		/// <value>
		///		<c>true</c>, when <see cref="Year"/> is leap year; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///		A <see cref="Year"/> of B.C.1 is <c>0</c>, so if the <see cref="Year"/> is <c>0</c> then it is leap year.
		///		In addition, B.C.3 (the <see cref="Year"/> is <c>-3</c>) is leap year, B.C.99 (the <see cref="Year"/> is <c>-100</c>) is not,
		///		and B.C.399 (the <see cref="Year"/> is <c>-400</c>) is leap year.
		/// </remarks>
		public bool IsLeapYear
		{
			get { return IsLeapYearInternal(this.Year); }
		}

		internal static bool IsLeapYearInternal(long year)
		{
			// Note: This algorithm assumes that BC uses leap year same as AD and B.C.1 is 0.
			// This algorithm avoids remainder operation as possible.
			return !(year % 4 != 0 || (year % 100 == 0 && year % 400 != 0));
		}

		internal static int GetLastDay(int month, bool isLeapYear)
		{
			var lastDay = LastDays[month];
			if (month == 2)
			{
				lastDay = isLeapYear ? 29 : 28;
			}

			return lastDay;
		}

		private void GetDatePart(out long year, out int month, out int day, out int dayOfYear)
		{
			if (this.unixEpochSeconds < -UnixEpochInSeconds)
			{
				this.GetDatePartBC(out year, out month, out day, out dayOfYear);
			}
			else
			{
				this.GetDatePartAD(out year, out month, out day, out dayOfYear);
			}
		}

		private void GetDatePartAD(out long year, out int month, out int day, out int dayOfYear)
		{
			Debug.Assert(this.unixEpochSeconds >= -UnixEpochInSeconds, this.unixEpochSeconds + " > " + (-UnixEpochInSeconds));

			// From coreclr System.DateTime.cs
			// https://github.com/dotnet/coreclr/blob/0825741447c14a6a70c60b7c429e16f95214e74e/src/mscorlib/shared/System/DateTime.cs#L863

			// First, use 0001-01-01 as epoch to simplify leap year calculation
			var seconds = unchecked((ulong)(this.unixEpochSeconds + UnixEpochInSeconds));

			// number of days since 0001-01-01
			var daysOffset = seconds / SecondsPerDay;

			// number of whole 400-year periods since 0001-01-01
			var numberOf400Years = daysOffset / DaysPer400Years;
			// day number within 400-year period
			var daysIn400Years = unchecked((uint)(daysOffset - numberOf400Years * DaysPer400Years));

			// number of whole 100-year periods within 400-year period
			var numberOf100Years = daysIn400Years / DaysPer100Years;
			// Last 100-year period has an extra day, so decrement result if 4
			if (numberOf100Years == 4)
			{
				numberOf100Years = 3;
			}

			// day number within 100-year period
			var daysIn100Years = daysIn400Years - numberOf100Years * DaysPer100Years;

			// number of whole 4-year periods within 100-year period
			var numberOf4years = daysIn100Years / DaysPer4Years;
			// day number within 4-year period
			var daysIn4Years = daysIn100Years - numberOf4years * DaysPer4Years;

			// number of whole years within 4-year period
			var numberOf1Year = daysIn4Years / DaysPerYear;
			// Last year has an extra day, so decrement result if 4
			if (numberOf1Year == 4)
			{
				numberOf1Year = 3;
			}

			// compute year
			year = unchecked((long)(numberOf400Years * 400 + numberOf100Years * 100 + numberOf4years * 4 + numberOf1Year + 1));
			// day number within year
			var daysInYear = daysIn4Years - numberOf1Year * DaysPerYear;
			dayOfYear = unchecked((int)(daysInYear + 1));
			// Leap year calculation
			var isLeapYear = numberOf1Year == 3 && (numberOf4years != 24 || numberOf100Years == 3);
			var days = isLeapYear ? DaysToMonth366 : DaysToMonth365;
			// All months have less than 32 days, so n >> 5 is a good conservative
			// estimate for the month
			var numberOfMonth = (daysInYear >> 5) + 1;

			Debug.Assert(numberOfMonth <= 12, numberOfMonth + "<= 12, daysInYear = " + daysInYear);

			// m = 1-based month number
			while (daysInYear >= days[numberOfMonth])
			{
				numberOfMonth++;

				Debug.Assert(numberOfMonth <= 12, numberOfMonth + "<= 12, daysInYear = " + daysInYear);
			}
			// compute month and day
			month = unchecked((int)numberOfMonth);
			day = unchecked((int)(daysInYear - days[numberOfMonth - 1] + 1));
		}

		private void GetDatePartBC(out long year, out int month, out int day, out int dayOfYear)
		{
			Debug.Assert(this.unixEpochSeconds < -UnixEpochInSeconds, this.unixEpochSeconds + " > " + (-UnixEpochInSeconds));

			// From coreclr System.DateTime.cs
			// https://github.com/dotnet/coreclr/blob/0825741447c14a6a70c60b7c429e16f95214e74e/src/mscorlib/shared/System/DateTime.cs#L863

			// First, use 0001-01-01 as epoch to simplify leap year calculation.
			// This method calculate negative offset from 0001-01-01.
			var seconds = unchecked((ulong)((this.unixEpochSeconds + UnixEpochInSeconds) * -1));

			// number of days since 0001-01-01
			var daysOffset = seconds / SecondsPerDay;
			daysOffset += (seconds % SecondsPerDay) > 0 ? 1u : 0u;

			// number of whole 400-year periods since 0001-01-01
			var numberOf400Years = (daysOffset - 1) / DaysPer400Years; // decrement offset 1 to adjust 1 to 12-31
																	   // day number within 400-year period
			var daysIn400Years = unchecked((uint)(daysOffset - numberOf400Years * DaysPer400Years));

			// number of whole 100-year periods within 400-year period
			var numberOf100Years =
				daysIn400Years <= (DaysPer100Years + 1) // 1st year is leap year (power of 400)
					? 0
					: ((daysIn400Years - 2) / DaysPer100Years); // decrement 1st leap year day and offset 1 to adjust 1 to 12-31

			// day number within 100-year period
			var daysIn100Years = daysIn400Years - numberOf100Years * DaysPer100Years;

			// number of whole 4-year periods within 100-year period
			var numberOf4years =
				daysIn100Years == 0
					? 0
					: ((daysIn100Years - 1) / DaysPer4Years); // decrement offset 1 to adjust 1 to 12-31

			// day number within 4-year period
			var daysIn4Years = daysIn100Years - numberOf4years * DaysPer4Years;

			// number of whole years within 4-year period
			var numberOf1Year =
				daysIn4Years <= (DaysPerYear + (numberOf4years != 0 ? 1 : 0)) // is leap year in 4 years range?
					? 0
					: ((daysIn4Years - 2) / DaysPerYear); // decrement 1st leap year day and offset 1 to adjust 1 to 12-31

			// compute year, note that 0001 -1 is 0000 (=B.C.1)
			year = -unchecked((long)(numberOf400Years * 400 + numberOf100Years * 100 + numberOf4years * 4 + numberOf1Year));
			var isLeapYear = numberOf1Year == 0 && (numberOf4years != 0 || numberOf100Years == 0);
			// day number within year
			var daysInYear =
				isLeapYear
				? (366 - daysIn4Years)
				: (365 - (daysIn4Years - 1 - numberOf1Year * DaysPerYear));

			dayOfYear = unchecked((int)(daysInYear + 1));
			// Leap year calculation
			var days = isLeapYear ? DaysToMonth366 : DaysToMonth365;
			// All months have more than 32 days, so n >> 5 is a good conservative
			// estimate for the month
			var numberOfMonth = (daysInYear >> 5) + 1;

			Debug.Assert(numberOfMonth <= 12, numberOfMonth + "<= 12, daysInYear = " + daysInYear);

			// m = 1-based month number
			while (daysInYear >= days[numberOfMonth])
			{
				numberOfMonth++;

				Debug.Assert(numberOfMonth <= 12, numberOfMonth + "<= 12, daysInYear = " + daysInYear);
			}
			// compute month and day
			month = unchecked((int)numberOfMonth);
			day = unchecked((int)(daysInYear - days[numberOfMonth - 1] + 1));
		}

		/// <summary>
		///		Gets a <see cref="Timestamp"/> instance which represents today on UTC. The result only contains date part.
		/// </summary>
		/// <value>A <see cref="Timestamp"/> instance which represents today on UTC. The result only contains date part.</value>
		/// <remarks>
		///		For underlying system API restriction, this method cannot work after 9999-12-31 in current implementation.
		/// </remarks>
		public static Timestamp Today
		{
			get { return UtcNow.Date; }
		}

		/// <summary>
		///		Gets a <see cref="Timestamp"/> instance of now on UTC.
		/// </summary>
		/// <value>A <see cref="Timestamp"/> instance of now on UTC.</value>
		/// <remarks>
		///		<para>
		///			For underlying system API restriction, this method cannot work after 9999-12-31T23:59:59.999999900 in current implementation.
		///		</para>
		///		<para>
		///			In addition, the precision of the returned <see cref="Timestamp"/> will be restricted by underlying platform.
		///			In current implementation, the precision is 100 nano-seconds at most, and about 1/60 milliseconds on normal Windows platform.
		///		</para>
		/// </remarks>
		public static Timestamp UtcNow
		{
			get
			{
				var now = DateTimeOffset.UtcNow;
				return new Timestamp(
#if !NET35 && !NET45 && !NETSTANDARD1_1 && !UNITY && !SILVERLIGHT
					now.ToUnixTimeSeconds(),
#else // !NET35 && !NET45 && !NETSTANDARD1_1 && !UNITY && !SILVERLIGHT
					( now.Ticks / TimeSpan.TicksPerSecond ) - UnixEpochInSeconds,
#endif // !NET35 && !NET45 && !NETSTANDARD1_1 && !UNITY && !SILVERLIGHT
					unchecked((int)(now.Ticks % 10000000 * 100))
				);
			}
		}
	}
}
