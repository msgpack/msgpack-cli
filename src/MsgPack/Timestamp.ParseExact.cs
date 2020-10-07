// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace MsgPack
{
	public partial struct Timestamp
	{
		/// <summary>
		///		Converts specified <see cref="String"/> representation of a msgpack timestamp to its <see cref="Timestamp"/> equivalant
		///		with specified format and culture-specific format information provider.
		/// </summary>
		/// <param name="input">An input <see cref="String"/> representation of a msgpack timestamp. The format must be match exactly to the <paramref name="format"/>.</param>
		/// <param name="format">An expected format string.</param>
		/// <param name="formatProvider">An <see cref="IFormatProvider"/> to provide culture specific information to parse <paramref name="input"/>.</param>
		/// <returns>The converted <see cref="Timestamp"/>.</returns>
		/// <remarks>
		///		<para>
		///			Currently, supported date-time format is only 'o' and 'O' (round-trip) or 's' (sortable, ISO-8601).
		///			Other any standard date-time formats and custom date-time formats are not supported.
		///		</para>
		///		<para>
		///			The rount-trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff</c> which "fffffffff" is nanoseconds.
		///		</para>
		///		<para>
		///			The sign mark can be culture-specific, and leading/trailing whitespaces can be allowed when specify appropriate <see cref="DateTimeStyles"/>.
		///		</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="input"/> is <c>null</c>.
		///		Or, <paramref name="format"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The specified <paramref name="format"/> is not supported.
		/// </exception>
		/// <exception cref="FormatException">
		///		The specified <paramref name="input"/> is not valid for the specified <paramref name="format"/> and <paramref name="formatProvider"/>.
		/// </exception>
		public static Timestamp ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			return ParseExact(input, format, formatProvider, DateTimeStyles.None);
		}

		/// <summary>
		///		Converts specified <see cref="String"/> representation of a msgpack timestamp to its <see cref="Timestamp"/> equivalant
		///		with specified format, culture-specific format information provider, and <see cref="DateTimeStyles"/>.
		/// </summary>
		/// <param name="input">An input <see cref="String"/> representation of a msgpack timestamp. The format must be match exactly to the <paramref name="format"/>.</param>
		/// <param name="format">An expected format string.</param>
		/// <param name="formatProvider">An <see cref="IFormatProvider"/> to provide culture specific information to parse <paramref name="input"/>.</param>
		/// <param name="styles">
		///		Specify bitwise value combination of <see cref="DateTimeStyles"/> to control detailed parsing behavior.
		///		The typical value is <see cref="DateTimeStyles.None"/>.
		/// </param>
		/// <returns>The converted <see cref="Timestamp"/>.</returns>
		/// <remarks>
		///		<para>
		///			Currently, supported date-time format is only 'o' and 'O' (round-trip) or 's' (sortable, ISO-8601).
		///			Other any standard date-time formats and custom date-time formats are not supported.
		///		</para>
		///		<para>
		///			The rount-trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff</c> which "fffffffff" is nanoseconds.
		///		</para>
		///		<para>
		///			The sign mark can be culture-specific, and leading/trailing whitespaces can be allowed when specify appropriate <see cref="DateTimeStyles"/>.
		///		</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="input"/> is <c>null</c>.
		///		Or, <paramref name="format"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The specified <paramref name="format"/> is not supported.
		///		Or the specified <paramref name="styles"/> has invalid combination.
		/// </exception>
		/// <exception cref="FormatException">
		///		The specified <paramref name="input"/> is not valid for the specified <paramref name="format"/>, <paramref name="formatProvider"/>, and <paramref name="styles"/>.
		/// </exception>
		public static Timestamp ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			Timestamp result;
			HandleParseResult(TryParseExactCore(input, format, formatProvider, styles, out result), "Cannot parse specified input with specified format.");
			return result;
		}

		/// <summary>
		///		Converts specified <see cref="String"/> representation of a msgpack timestamp to its <see cref="Timestamp"/> equivalant
		///		with specified format, culture-specific format information provider, and <see cref="DateTimeStyles"/>.
		/// </summary>
		/// <param name="input">The input <see cref="String"/> representation of a msgpack timestamp. The format must be match exactly to one of the <paramref name="formats"/>.</param>
		/// <param name="formats">The array of expected format strings. Unsupported format will be ignored.</param>
		/// <param name="formatProvider">The culture specific information to control </param>
		/// <param name="styles">
		///		Specify bitwise value combination of <see cref="DateTimeStyles"/> to control detailed parsing behavior.
		///		The typical value is <see cref="DateTimeStyles.None"/>.
		/// </param>
		/// <returns>The converted <see cref="Timestamp"/>.</returns>
		/// <remarks>
		///		<para>
		///			Currently, supported date-time format is only 'o' and 'O' (round-trip) or 's' (sortable, ISO-8601).
		///			Other any standard date-time formats and custom date-time formats are not supported.
		///		</para>
		///		<para>
		///			The rount-trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff</c> which "fffffffff" is nanoseconds.
		///		</para>
		///		<para>
		///			The sign mark can be culture-specific, and leading/trailing whitespaces can be allowed when specify appropriate <see cref="DateTimeStyles"/>.
		///		</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="input"/> is <c>null</c>.
		///		Or, <paramref name="formats"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The specified <paramref name="formats"/> is empty.
		///		Or the specified <paramref name="styles"/> has invalid combination.
		/// </exception>
		/// <exception cref="FormatException">
		///		The specified <paramref name="input"/> is not valid for any specified <paramref name="formats"/>, <paramref name="formatProvider"/>, and <paramref name="styles"/>.
		/// </exception>
		public static Timestamp ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			Timestamp result;
			HandleParseResult(TryParseExactCore(input, formats, formatProvider, styles, out result), "Cannot parse specified input with any specified formats.");
			return result;
		}

		private static void HandleParseResult(TimestampParseResult result, string messageForInvalidInput)
		{
			if (result == TimestampParseResult.Success)
			{
				return;
			}

			string parameterName;
			string? message;

			var parameter = result & TimestampParseResult.ParameterMask;
			switch (parameter)
			{
				case TimestampParseResult.ParameterFormat:
				{
					parameterName = "format";
					break;
				}
				default:
				{
					Debug.Assert(parameter == TimestampParseResult.ParameterInput, parameter + " == TimestampParseResult.Input");
					parameterName = "input";
					break;
				}
			}

			var kind = result & TimestampParseResult.KindMask;

			switch (kind)
			{
				case TimestampParseResult.KindEmpty:
				{
					message = String.Format(CultureInfo.CurrentCulture, "'{0}' must not be empty.", parameterName);
					break;
				}
				case TimestampParseResult.KindExtraCharactors:
				{
					message = "The input contains extra charactors.";
					break;
				}
				case TimestampParseResult.KindInvalidDateTimeDelimiter:
				{
					message = "A date-time delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidDay:
				{
					message = "Format of the day portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidHour:
				{
					message = "Format of the hour portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidHourMinuteDelimiter:
				{
					message = "A hour-miniute delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidMinute:
				{
					message = "Format of the minute portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidMinuteSecondDelimiter:
				{
					message = "A miniute-second delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidMonth:
				{
					message = "Format of the month portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidMonthDayDelimiter:
				{
					message = "A month-day delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidNanoSecond:
				{
					message = "Format of the nanosecond portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidSecond:
				{
					message = "Format of the second portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidSubsecondDelimiter:
				{
					message = "A second-nanosecond delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidYear:
				{
					message = "Format of the year portion of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindInvalidYearMonthDeilimiter:
				{
					message = "A year-month delimiter of the input is not valid.";
					break;
				}
				case TimestampParseResult.KindLeadingWhitespaceNotSupported:
				{
					message = "Leading whitespaces in the input is not allowed.";
					break;
				}
				case TimestampParseResult.KindMissingUtcSign:
				{
					message = "No time offset specifier 'Z' is missing in the input.";
					break;
				}
				case TimestampParseResult.KindNull:
				{
					Debug.Assert(
						(result & TimestampParseResult.ExceptionTypeMask) == TimestampParseResult.ArgumentNullException,
						(result & TimestampParseResult.ExceptionTypeMask) + " == TimestampParseResult.ArgumentNullException"
					);
					message = null;
					break;
				}
				case TimestampParseResult.KindTrailingWhitespaceNotSupported:
				{
					message = "Trailing whitespaces in the input is not allowed.";
					break;
				}
				case TimestampParseResult.KindUnsupported:
				{
					message = "The specified format is not supported.";
					break;
				}
				case TimestampParseResult.KindYearOutOfRange:
				{
					message = "The specified year is too small or too large.";
					break;
				}
				default:
				{
					Debug.Assert(kind == TimestampParseResult.KindNoMatchedFormats, kind + " == TimestampParseResult.KindNoMatchedFormats");
					message = "The input is not valid Timestamp.";
					break;
				}
			}

			switch (result & TimestampParseResult.ExceptionTypeMask)
			{
				case TimestampParseResult.ArgumentNullException:
				{
					throw new ArgumentNullException(parameterName);
				}
				case TimestampParseResult.ArgumentException:
				{
					throw new ArgumentException(message, parameterName);
				}
				default:
				{
					Debug.Assert(
						(result & TimestampParseResult.ExceptionTypeMask) == TimestampParseResult.FormatException,
						(result & TimestampParseResult.ExceptionTypeMask) + " == TimestampParseResult.FormatException"
					);
					throw new FormatException(message);
				}
			}
		}
	}
}
