#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017-2018 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

namespace MsgPack
{
	/// <summary>
	///		Represents internal <see cref="TimestampStringConverter.TryParseExact"/> result.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	enum TimestampParseResult
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
