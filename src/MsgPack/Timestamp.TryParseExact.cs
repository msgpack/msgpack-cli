// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Globalization;

namespace MsgPack
{
	partial struct Timestamp
	{
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
		/// <param name="result">If the conversion succeeded, the conversion result will be stored; otherwise, the default value will be stored.</param>
		/// <returns><c>true</c>, if the conversion succeeded; otherwise, <c>false</c>.</returns>
		/// <remarks>
		///		<para>
		///			Currently, supported date-time format is only 'o' and 'O' (round-trip) or 's' (sortable, ISO-8601).
		///			Other any standard date-time formats and custom date-time formats are not supported.
		///		</para>
		///		<para>
		///			The rount-trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which "fffffffff" is nanoseconds.
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
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out Timestamp result)
		{
			return TryParseExactCore(input, format, formatProvider, styles, out result) == TimestampParseResult.Success;
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
		/// <param name="result">If the conversion succeeded, the conversion result will be stored; otherwise, the default value will be stored.</param>
		/// <returns><c>true</c>, if the conversion succeeded; otherwise, <c>false</c>.</returns>
		/// <remarks>
		///		<para>
		///			Currently, supported date-time format is only 'o' and 'O' (round-trip) or 's' (sortable, ISO-8601).
		///			Other any standard date-time formats and custom date-time formats are not supported.
		///		</para>
		///		<para>
		///			The rount-trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which "fffffffff" is nanoseconds.
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
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out Timestamp result)
		{
			return TryParseExactCore(input, formats, formatProvider, styles, out result) == TimestampParseResult.Success;
		}

		private static TimestampParseResult TryParseExactCore(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out Timestamp result)
		{
			ValidateParseInput(input);

			if (input.Length == 0)
			{
				result = default(Timestamp);
				return TimestampParseResult.EmptyInput;
			}

			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			if (format.Length == 0)
			{
				throw new ArgumentException("The 'format' must not be empty.", "format");
			}

			ValidateParseStyles(styles);

			var error = TimestampStringConverter.TryParseExact(input, format, formatProvider, styles, out result);
			if (error == TimestampParseResult.UnsupportedFormat)
			{
				// UnsupportedFormat should throw Exception instead of returning false.
				HandleParseResult(error, "Cannot parse specified input with specified format.");
			}

			return error;
		}

		private static TimestampParseResult TryParseExactCore(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out Timestamp result)
		{
			ValidateParseInput(input);

			if (formats == null)
			{
				throw new ArgumentNullException("formats");
			}

			if (formats.Length == 0)
			{
				throw new ArgumentException("The 'formats' must not be empty.", "formats");
			}

			ValidateParseStyles(styles);

			if (input.Length == 0)
			{
				result = default(Timestamp);
				return TimestampParseResult.EmptyInput;
			}

			foreach (var format in formats)
			{
				if (TimestampStringConverter.TryParseExact(input, format, formatProvider, styles, out result) == TimestampParseResult.Success)
				{
					return TimestampParseResult.Success;
				}
			}

			result = default(Timestamp);
			return TimestampParseResult.NoMatchedFormats;
		}

		private static void ValidateParseInput(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
		}

		private static void ValidateParseStyles(DateTimeStyles styles)
		{
			if (styles != DateTimeStyles.None && (styles & ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite)) != 0)
			{
				throw new ArgumentException("Timestamp currently only support DateTimeStyles.None, DateTimeStyles.AllowLeadingWhite, and DateTimeStyles.AllowTrailingWhite.", "styles");
			}
		}
	}
}
