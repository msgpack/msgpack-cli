// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines behavior of built-in serializers to conversion of <see cref="DateTime"/> value for specific member.
	/// </summary>
	/// <remarks>
	///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
	/// </remarks>
	public enum DateTimeMemberConversionMethod
	{
		/// <summary>
		///		Uses systems default <see cref="DateTimeConversionMethod"/> value.
		/// </summary>
		/// <remarks>
		///		The systems default <see cref="DateTimeConversionMethod.Native"/>.
		/// </remarks>
		Default = 0,

		/// <summary>
		///		Uses <see cref="DateTime.Ticks"/> context, that is, Gregorian 0000-01-01 based, 100 nano seconds resolution. This value also preserves <see cref="DateTimeKind"/>.
		/// </summary>
		/// <remarks>
		///		As of 0.6, this value has been become default. This option prevents accidental data loss.
		/// </remarks>
		Native = 1,

		/// <summary>
		///		Uses Unix epoc context, that is, Gregirian 1970-01-01 based, milliseconds resolution.
		/// </summary>
		/// <remarks>
		///		Many binding such as Java uses this resolution, so this option gives maximom interoperability.
		/// </remarks>
		UnixEpoc = 2,

		/// <summary>
		///		Uses MsgPack timestamp format, that is, Gregirian 1970-01-01 based, nanoseconds resolution, with reserved ext type format.
		/// </summary>
		/// <remarks>
		///		<para>
		///			As of 1.0, this value became default.
		///		</para>
		///		<para>
		///			This is best choice for interoperability and prevents accidental data loss, but old implementation does not recognize this type.
		///			For backward compability purposes, use <see cref="Native"/> or <see cref="UnixEpoc"/> instead.
		///		</para>
		///		<para>
		///			Note that <see cref="DateTime"/> and <see cref="DateTimeOffset"/> cannot hold nanoseconds value.
		///			If you can depend on this assembly, consider <see cref="Timestamp"/> for date-time typed members to maximize interoperability for other languages.
		///		</para>
		/// </remarks>
		Timestamp = 3,

		// 3 is reserved for Iso8601BasicFormat

		/// <summary>
		///		Uses ISO 8601 extended format string, that is, "yyyy-MM-ddTHH:mm:ss.f+Z" format.
		/// </summary>
		/// <remarks>
		///		This value is default in JSON codec and its family.
		/// </remarks>
		Iso8601ExtendedFormat = 4
	}
}
