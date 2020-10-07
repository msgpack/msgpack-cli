// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Boxed representation for <see cref="DateTimeConversionMethod"/> to reduce allocation.
	/// </summary>
	internal sealed class BoxedDateTimeConversionMethod
	{
		/// <summary>
		///		Uses <see cref="DateTime.Ticks"/> context, that is, Gregorian 0000-01-01 based, 100 nano seconds resolution. This value also preserves <see cref="DateTimeKind"/>.
		/// </summary>
		/// <remarks>
		///		As of 0.6 to 0.9, this value became default. This option prevents accidental data loss.
		/// </remarks>
		public static BoxedDateTimeConversionMethod Native { get; } = new BoxedDateTimeConversionMethod(DateTimeConversionMethod.Native);

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
		public static BoxedDateTimeConversionMethod Timestamp { get; } = new BoxedDateTimeConversionMethod(DateTimeConversionMethod.Timestamp);
	
		/// <summary>
		///		Uses Unix epoc context, that is, Gregirian 1970-01-01 based, milliseconds resolution.
		/// </summary>
		/// <remarks>
		///		Many binding such as Java uses this resolution, so this option gives maximum interoperability.
		/// </remarks>
		public static BoxedDateTimeConversionMethod UnixEpoc { get; } = new BoxedDateTimeConversionMethod(DateTimeConversionMethod.UnixEpoc);

		/// <summary>
		///		Uses ISO 8601 extended format string, that is, "yyyy-MM-ddTHH:mm:ss.f+Z" format.
		/// </summary>
		/// <remarks>
		///		This value is default in JSON codec and its family.
		/// </remarks>
		public static BoxedDateTimeConversionMethod Iso8601ExtendedFormat { get; } = new BoxedDateTimeConversionMethod(DateTimeConversionMethod.Iso8601ExtendedFormat);

		/// <summary>
		///		Gets an equivalent <see cref="DateTimeConversionMethod"/> value.
		/// </summary>
		/// <value>An equivalent <see cref="DateTimeConversionMethod"/> value.</value>
		public DateTimeConversionMethod Value { get; }

		private BoxedDateTimeConversionMethod(DateTimeConversionMethod value)
		{
			this.Value = value;
		}

		/// <summary>
		///		Gets an equivalent <see cref="BoxedDateTimeConversionMethod"/> instance for specified <see cref="DateTimeConversionMethod"/> value.
		/// </summary>
		/// <param name="enumValue"><see cref="DateTimeConversionMethod"/> value or <c>null</c>.</param>
		/// <returns>
		///		An equivalent <see cref="BoxedDateTimeConversionMethod"/> instance for specified <see cref="DateTimeConversionMethod"/> value.
		///		<c>null</c> when <paramref name="enumValue"/> is <c>null</c>.
		/// </returns>
		public static BoxedDateTimeConversionMethod? Get(DateTimeConversionMethod? enumValue)
			=> enumValue switch
			{
				DateTimeConversionMethod.Native => Native,
				DateTimeConversionMethod.Timestamp => Timestamp,
				DateTimeConversionMethod.UnixEpoc => UnixEpoc,
				DateTimeConversionMethod.Iso8601ExtendedFormat => Iso8601ExtendedFormat,
				_ => null
			};
	}
}
