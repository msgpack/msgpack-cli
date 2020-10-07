// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack
{
	/// <summary>
	///		Represents internal <see cref="TimestampStringConverter.TryParseExact"/> result.
	/// </summary>
	internal enum TimestampParseResult
	{
		Success = 0,

		KindMask							= 0xFFFF,
		KindNull							= 1,
		KindEmpty							= 2,
		KindUnsupported						= 3,
		KindLeadingWhitespaceNotSupported	= 11,
		KindTrailingWhitespaceNotSupported	= 12,
		KindMissingUtcSign					= 13,
		KindExtraCharactors					= 14,
		KindInvalidYear						= 21,
		KindInvalidMonth					= 22,
		KindInvalidDay						= 23,
		KindInvalidHour						= 24,
		KindInvalidMinute					= 25,
		KindInvalidSecond					= 26,
		KindInvalidNanoSecond				= 27,
		KindYearOutOfRange					= 31,
		KindInvalidYearMonthDeilimiter		= 101,
		KindInvalidMonthDayDelimiter		= 102,
		KindInvalidDateTimeDelimiter		= 103,
		KindInvalidHourMinuteDelimiter		= 104,
		KindInvalidMinuteSecondDelimiter	= 105,
		KindInvalidSubsecondDelimiter		= 106,
		KindNoMatchedFormats				= 1001,

		ParameterMask	= 0xFF << 16,
		ParameterInput	= 1 << 16,
		ParameterFormat	= 2 << 16,

		ExceptionTypeMask		= 0xFF << 24,
		ArgumentNullException	= 1 << 24,
		ArgumentException		= 2 << 24,
		FormatException			= 3 << 24,

		NullInput						= ArgumentNullException | ParameterInput | KindNull,
		NullFormat						= ArgumentNullException | ParameterFormat | KindNull,
		EmptyInput						= FormatException | ParameterInput | KindEmpty,
		EmptyFormat						= ArgumentException | ParameterFormat | KindEmpty,
		UnsupportedFormat				= ArgumentException | ParameterFormat | KindUnsupported,
		LeadingWhitespaceNotAllowed		= FormatException | ParameterInput | KindLeadingWhitespaceNotSupported,
		TrailingWhitespaceNotAllowed	= FormatException | ParameterInput | KindTrailingWhitespaceNotSupported,
		MissingUtcSign					= FormatException | ParameterInput | KindMissingUtcSign,
		ExtraCharactors					= FormatException | ParameterInput | KindExtraCharactors,
		InvalidYear						= FormatException | ParameterInput | KindInvalidYear,
		InvalidMonth					= FormatException | ParameterInput | KindInvalidMonth,
		InvalidDay						= FormatException | ParameterInput | KindInvalidDay,
		InvalidHour						= FormatException | ParameterInput | KindInvalidHour,
		InvalidMinute					= FormatException | ParameterInput | KindInvalidMinute,
		InvalidSecond					= FormatException | ParameterInput | KindInvalidSecond,
		InvalidNanoSecond				= FormatException | ParameterInput | KindInvalidNanoSecond,
		YearOutOfRange					= FormatException | ParameterInput | KindYearOutOfRange,
		InvalidYearMonthDeilimiter		= FormatException | ParameterInput | KindInvalidYearMonthDeilimiter,
		InvalidMonthDayDelimiter		= FormatException | ParameterInput | KindInvalidMonthDayDelimiter,
		InvalidDateTimeDelimiter		= FormatException | ParameterInput | KindInvalidDateTimeDelimiter,
		InvalidHourMinuteDelimiter		= FormatException | ParameterInput | KindInvalidHourMinuteDelimiter,
		InvalidMinuteSecondDelimiter	= FormatException | ParameterInput | KindInvalidMinuteSecondDelimiter,
		InvalidSubsecondDelimiter		= FormatException | ParameterInput | KindInvalidSubsecondDelimiter,
		NoMatchedFormats				= FormatException | ParameterInput | KindNoMatchedFormats
	}
}
