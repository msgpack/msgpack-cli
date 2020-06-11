// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines JSON parser options in deserialization.
	/// </summary>
	[Flags]
	public enum JsonParseOptions
	{
		/// <summary>
		///		No options specified. This should be fastest.
		/// </summary>
		None = 0,

		/// <summary>
		///		Allows single line comment which starts with <c>#</c> outside of string value.
		/// </summary>
		AllowHashSingleLineComment = 0x1,

		/// <summary>
		///		Allows single line comment which starts with <c>//</c> outside of string value.
		/// </summary>
		AllowDoubleSolidousSingleLineComment = 0x2,

		/// <summary>
		///		Allows multi line comment which starts with <c>/*</c> and ends with <c>*/</c> outside of string value.
		/// </summary>
		AllowMultilineComment = 0x4,

		/// <summary>
		///		Allows Unicode whitespace chars such as 1/4 spacing and full width space, and treats them as ASCII whitespace.
		/// </summary>
		AllowUnicodeWhitespace = 0x10,

		/// <summary>
		///		Allows all trivial tokens.
		/// </summary>
		AllowAllTrivias = 0xFF,

		/// <summary>
		///		Allows <c>+NaN</c> and <c>-NaN</c> as valid number.
		/// </summary>
		AllowNaN = 0x100,

		/// <summary>
		///		Allows <c>+Infinity</c> and <c>-Infinity</c> as valid number.
		/// </summary>
		AllowInfinity = 0x200,

		/// <summary>
		///		Allows <c>undefined</c> as valid value, and treats it as <c>null</c>.
		/// </summary>
		AllowUndefined = 0x1000,

		/// <summary>
		///		Allows all irregal values which is not valid in RFC 8259 but is valid as ECMA Script literal.
		/// </summary>
		AllowIrregalValues = 0xFF00,

		/// <summary>
		///		Allows <c>=</c> as object key-valid separator as well as <c>:</c>.
		/// </summary>
		AllowEqualSignSeparator = 0x10000,

		/// <summary>
		///		Allows <c>;</c> as collection item delimiter as well as <c>,</c>.
		/// </summary>
		AllowSemicolonDelimiter = 0x20000,

		/// <summary>
		///		Allows extra trailing comma after last collection item.
		/// </summary>
		AllowExtraComma = 0x40000,

		/// <summary>
		///		Allows <c>'</c> for string value quotation instead of <c>"</c>.
		/// </summary>
		AllowSingleQuotationString = 0x100000,

		/// <summary>
		///		Allows all known syntax errors.
		/// </summary>
		AllowWellknownSyntaxErrors = 0xFF0000,

		/// <summary>
		///		Allows all grammer errors.
		/// </summary>
		AllowAllErrors = unchecked((int)0xFFFFFFFF)
	}
}
