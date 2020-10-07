// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
#if FEATURE_COM_TYPES
using System.Runtime.InteropServices.ComTypes;
#endif // FEATURE_COM_TYPES
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Builder object to configure <see cref="DateTime"/> and similar types serialization behavior.
	/// </summary>
	public sealed class DateTimeSerializationOptionsBuilder
	{
		internal static DateTimeSerializationOptionsBuilder Default { get; } = new DateTimeSerializationOptionsBuilder();

		private readonly HashSet<Type> _knownDateTimeLikeTypes;

		internal ISet<Type> KnownDateTimeLikeTypeSet => this._knownDateTimeLikeTypes;

		/// <summary>
		///		Gets the collection which contains types which will be considered as date-time like type.
		/// </summary>
		/// <value>
		///		The collection which contains types which will be considered as date-time like type.
		/// </value>
		public IEnumerable<Type> KnownDateTimeLikeTypes => this._knownDateTimeLikeTypes;

		/// <summary>
		///		Gets the default value of date time conversion method.
		/// </summary>
		/// <value>
		///		The default value of date time conversion method.
		///		Default is <c>null</c>, which means usage of the codec default format (<see cref="CodecFeatures.PreferredDateTimeConversionMethod"/>).
		/// </value>
		public DateTimeConversionMethod? DefaultDateTimeConversionMethod { get; private set; }

		/// <summary>
		///		Gets the precision of fraction portion on ISO 8601 date time string.
		/// </summary>
		/// <value>
		///		The precision of fraction portion on ISO 8601 date time string as specified by this codec.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public int? Iso8601FractionOfSecondsPrecision { get; private set; }

		/// <summary>
		///		Gets the separator char for fraction portion.
		/// </summary>
		/// <value>
		///		The separator char for fraction portion.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		public char? Iso8601DecimalMark { get; private set; }


		/// <summary>
		///		Initialized a new instance of <see cref="DateTimeSerializationOptionsBuilder"/> object.
		/// </summary>
		public DateTimeSerializationOptionsBuilder()
		{
			this._knownDateTimeLikeTypes =
				new HashSet<Type>
				{
					typeof(DateTime),
					typeof(DateTimeOffset),
					typeof(Timestamp),
#if FEATURE_COM_TYPES
					typeof(FILETIME),
#endif // FEATURE_COM_TYPES
				};
		}

		/// <summary>
		///		Adds the specified type as date-time like type.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to be treated as date-time like.</param>
		/// <returns>This instance.</returns>
		/// <remarks>
		///		"Date-time like" types mean that a type which can be controlled its serialization behavior via <see cref="DateTimeConversionMethod"/> provider parameter.
		///		To use the type as date-time like type, you should do following actions:
		///		<list type="number">
		///			<item>Register the type via this method.</item>
		///			<item>Implements dedicated <see cref="IObjectSerializerProvider"/> for the type. You can use <see cref="DateTimeSerializerProvider"/> to implement provier easily.</item>
		///			<item>Register dedicated <see cref="IObjectSerializerProvider"/> for the type via <see cref="SerializerProvider.RegisterDateTimeSerializerProvider"/>.</item>
		///		</list>
		///		Note that <see cref="SerializerProvider" /> treats following types and their nullable wrappers as date time like tyeps by default:
		///		<list type="bullet">
		///			<item><see cref="DateTime"/></item>
		///			<item><see cref="DateTimeOffset"/></item>
		///			<item><see cref="Timestamp"/></item>
		///		</list>
		///		You can override default behavior with <see cref="SerializerProvider.RegisterDateTimeSerializerProvider"/>.
		/// </remarks>
		public DateTimeSerializationOptionsBuilder AddDateTimeLikeType(Type type)
		{
			this._knownDateTimeLikeTypes.Add(Ensure.NotNull(type));
			return this;
		}

		/// <summary>
		///		Indicates that the default value of date time conversion method to be codec default (<see cref="CodecFeatures.PreferredDateTimeConversionMethod"/>).
		/// </summary>
		/// <returns>This <see cref="DateTimeSerializationOptionsBuilder" /> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="DefaultDateTimeConversionMethod"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DateTimeSerializationOptionsBuilder UseCodecDefaultDateTimeConversionMethod()
		{
			this.DefaultDateTimeConversionMethod = null;
			return this;
		}

		/// <summary>
		///		Sets the default value of date time conversion method.
		/// </summary>
		/// <param name="value">
		///		The default value of date time conversion method.
		/// </param>
		/// <returns>This <see cref="DateTimeSerializationOptionsBuilder" /> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="DefaultDateTimeConversionMethod"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DateTimeSerializationOptionsBuilder UseCustomDefaultDateTimeConversionMethod(DateTimeConversionMethod value)
		{
			this.DefaultDateTimeConversionMethod = Ensure.Defined(value);
			return this;
		}

		/// <summary>
		///		Sets the precision of ISO 8601 fraction of seconds portion.
		/// </summary>
		/// <param name="value">The precision.</param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		<paramref name="value"/> is negative value or greater than 9.
		/// </exception>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public DateTimeSerializationOptionsBuilder SetIso8601FractionOfSecondsPrecision(int value)
		{
			this.Iso8601FractionOfSecondsPrecision = Ensure.IsBetween(value, 0, 9);
			return this;
		}

		/// <summary>
		///		Resets the precision of ISO 8601 fraction of seconds portion.
		/// </summary>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		public DateTimeSerializationOptionsBuilder ResetIso8601FractionOfSecondsPrecision()
		{
			this.Iso8601FractionOfSecondsPrecision = null;
			return this;
		}

		/// <summary>
		///		Sets the decimal separator of ISO 8601.
		/// </summary>
		/// <param name="value">The precision.</param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		<paramref name="value"/> is not <c>,</c> nor <c>.</c>.
		/// </exception>
		public DateTimeSerializationOptionsBuilder SetIso8601DecimalMark(char value)
		{
			switch (value)
			{
				case ',':
				case '.':
				{
					this.Iso8601DecimalMark = value;
					break;
				}
				default:
				{
					Throw.InvalidIso8601DecimalMark(value);
					break; // Never reaches
				}
			}

			return this;
		}

		/// <summary>
		///		Sets the decimal separator of ISO 8601.
		/// </summary>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		public DateTimeSerializationOptionsBuilder ResetIso8601DecimalMark()
		{
			this.Iso8601DecimalMark = null;
			return this;
		}
	}
}
