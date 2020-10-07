// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Globalization;

namespace MsgPack
{
	internal partial class TimestampStringConverter
	{
		// Currently, custom format and normal date time format except 'o' or 'O' 's' are NOT supported.
		public static TimestampParseResult TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out Timestamp result)
		{
			if (format != "o" && format != "O" && format != "s")
			{
				result = default(Timestamp);
				return TimestampParseResult.UnsupportedFormat;
			}

			var numberFormat = NumberFormatInfo.GetInstance(formatProvider);

			var position = 0;
			if (!ParseWhitespace(input, ref position, (styles & DateTimeStyles.AllowLeadingWhite) != 0, /* isTrailing */false))
			{
				result = default(Timestamp);
				return TimestampParseResult.LeadingWhitespaceNotAllowed;
			}

			long year;
			if (!ParseYear(input, ref position, numberFormat, out year))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidYear;
			}

			if (!ParseDelimiter(input, ref position, DateDelimiter))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidYearMonthDeilimiter;
			}

			var isLeapYear = Timestamp.IsLeapYearInternal(year);

			int month;
			if (!ParseDigitRange(input, 2, ref position, 1, 12, out month))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidMonth;
			}

			if (!ParseDelimiter(input, ref position, DateDelimiter))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidMonthDayDelimiter;
			}

			int day;
			if (!ParseDay(input, ref position, month, isLeapYear, out day))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidDay;
			}

			if (!ParseDelimiter(input, ref position, DateTimeDelimiter))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidDateTimeDelimiter;
			}

			int hour;
			if (!ParseDigitRange(input, 2, ref position, 0, 23, out hour))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidHour;
			}

			if (!ParseDelimiter(input, ref position, TimeDelimiter))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidHourMinuteDelimiter;
			}

			int minute;
			if (!ParseDigitRange(input, 2, ref position, 0, 59, out minute))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidMinute;
			}

			if (!ParseDelimiter(input, ref position, TimeDelimiter))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidMinuteSecondDelimiter;
			}

			int second;
			if (!ParseDigitRange(input, 2, ref position, 0, 59, out second))
			{
				result = default(Timestamp);
				return TimestampParseResult.InvalidSecond;
			}

			var nanosecond = 0;
			if (format != "s")
			{
				// "o" or "O"
				if (!ParseDelimiter(input, ref position, SubsecondDelimiter))
				{
					result = default(Timestamp);
					return TimestampParseResult.InvalidSubsecondDelimiter;
				}

				if (!ParseDigitRange(input, 9, ref position, 0, 999999999, out nanosecond))
				{
					result = default(Timestamp);
					return TimestampParseResult.InvalidNanoSecond;
				}
			}

			if (!ParseDelimiter(input, ref position, UtcSign))
			{
				result = default(Timestamp);
				return TimestampParseResult.MissingUtcSign;
			}

			if (!ParseWhitespace(input, ref position, (styles & DateTimeStyles.AllowTrailingWhite) != 0, /* isTrailing */true))
			{
				result = default(Timestamp);
				return TimestampParseResult.TrailingWhitespaceNotAllowed;
			}

			if (position != input.Length)
			{
				result = default(Timestamp);
				return TimestampParseResult.ExtraCharactors;
			}

			var components = new Timestamp.Value();
			components.Year = year;
			components.Month = month;
			components.Day = day;
			components.Hour = hour;
			components.Minute = minute;
			components.Second = second;
			components.Nanoseconds = unchecked((uint)nanosecond);

			try
			{
				result = Timestamp.FromComponents(ref components, isLeapYear);
			}
			catch (OverflowException)
			{
				result = default(Timestamp);
				return TimestampParseResult.YearOutOfRange;
			}

			return TimestampParseResult.Success;
		}

		private static bool ParseWhitespace(string input, ref int position, bool allowWhitespace, bool isTrailing)
		{
			if (input.Length <= position)
			{
				return isTrailing;
			}

			if (!allowWhitespace)
			{
				return !Char.IsWhiteSpace(input[position]);
			}

			while (position < input.Length && Char.IsWhiteSpace(input[position]))
			{
				position++;
			}

			return true;
		}

		private static bool ParseDelimiter(string input, ref int position, char delimiter)
		{
			if (input.Length <= position)
			{
				return false;
			}

			if (input[position] != delimiter)
			{
				return false;
			}

			position++;
			return true;
		}

		private static bool ParseSign(string input, ref int position, NumberFormatInfo numberFormat, out int sign)
		{
			if (input.Length <= position)
			{
				sign = default(int);
				return false;
			}

			if (IsDigit(input[position]))
			{
				sign = 1;
				return true;
			}

			if (StartsWith(input, position, numberFormat.NegativeSign))
			{
				position += numberFormat.NegativeSign.Length;
				sign = -1;
				return true;
			}

			if (StartsWith(input, position, numberFormat.PositiveSign))
			{
				position += numberFormat.NegativeSign.Length;
				sign = 1;
				return true;
			}

			sign = default(int);
			return false;
		}

		private static bool StartsWith(string input, int startIndex, string comparison)
		{
			for (var i = 0; i < comparison.Length; i++)
			{
				if (i + startIndex >= input.Length)
				{
					return false;
				}

				if (input[i + startIndex] != comparison[i])
				{
					return false;
				}
			}

			return true;
		}

		private static bool ParseDigit(string input, int minLength, ref int position, out long digit)
		{
			var startPosition = position;
			var bits = 0L;
			while (position < input.Length)
			{
				var c = input[position];
				if (!IsDigit(c))
				{
					break;
				}

				bits = bits * 10 + (c - '0');
				position++;
			}

			digit = bits;
			return position >= startPosition + minLength;
		}

		private static bool IsDigit(char c)
		{
			return '0' <= c && c <= '9';
		}

		private static bool ParseDigitRange(string input, int minLength, ref int position, int min, int max, out int result)
		{
			long digit;
			if (!ParseDigit(input, minLength, ref position, out digit))
			{
				result = default(int);
				return false;
			}

			if (digit < min || max < digit)
			{
				result = default(int);
				return false;
			}

			result = unchecked((int)digit);
			return true;
		}

		private static bool ParseYear(string input, ref int position, NumberFormatInfo numberFormat, out long year)
		{
			int sign;
			if (!ParseSign(input, ref position, numberFormat, out sign))
			{
				year = default(long);
				return false;
			}

			long digit;
			if (!ParseDigit(input, 4, ref position, out digit))
			{
				year = default(long);
				return false;
			}

			// as of ISO 8601, 0001-01-01 -1 day is 0000-12-31.
			year = digit * sign;
			return true;
		}

		private static bool ParseDay(string input, ref int position, int month, bool isLeapYear, out int day)
		{
			long digit;
			if (!ParseDigit(input, 2, ref position, out digit))
			{
				day = default(int);
				return false;
			}

			var lastDay = Timestamp.GetLastDay(month, isLeapYear);

			if (digit < 1 || lastDay < digit)
			{
				day = default(int);
				return false;
			}

			day = unchecked((int)digit);
			return true;
		}
	}
}
