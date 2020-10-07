// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using MsgPack.Codecs;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	internal static class Iso8601
	{
		private static ReadOnlySpan<string?> FormatStringsWithPeriod =>
			new[]
			{
				"yyyy-MM-ddTHH:mm:ssK",
				"yyyy-MM-ddTHH:mm:ss.fK",
				"yyyy-MM-ddTHH:mm:ss.ffK",
				"yyyy-MM-ddTHH:mm:ss.fffK",
				"yyyy-MM-ddTHH:mm:ss.ffffK",
				"yyyy-MM-ddTHH:mm:ss.fffffK",
				"yyyy-MM-ddTHH:mm:ss.ffffffK",
				null,
				"yyyy-MM-ddTHH:mm:ss.ffffff0K",
				"yyyy-MM-ddTHH:mm:ss.ffffff00K",
			};

		private static ReadOnlySpan<string> FormatStringsWithComma =>
			new[]
			{
				"yyyy-MM-ddTHH:mm:ssK",
				"yyyy-MM-ddTHH:mm:ss,fK",
				"yyyy-MM-ddTHH:mm:ss,ffK",
				"yyyy-MM-ddTHH:mm:ss,fffK",
				"yyyy-MM-ddTHH:mm:ss,ffffK",
				"yyyy-MM-ddTHH:mm:ss,fffffK",
				"yyyy-MM-ddTHH:mm:ss,ffffffK",
				"yyyy-MM-ddTHH:mm:ss,ffffffK",
				"yyyy-MM-ddTHH:mm:ss,ffffff0K",
				"yyyy-MM-ddTHH:mm:ss,ffffff00K",
			};

		public const int MaxLength = 35;
		public static readonly StandardFormat StandardFormat = new StandardFormat('o');

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static string? GetFormatString(char decimalMark, int fractionPrecision)
		{
			Debug.Assert(fractionPrecision >= 0 && fractionPrecision <= 9);

			if (decimalMark == '.')
			{
				return FormatStringsWithPeriod[fractionPrecision];
			}
			else
			{
				Debug.Assert(decimalMark == ',');
				return FormatStringsWithComma[fractionPrecision];
			}
		}
	}

	/// <summary>
	///		Immutable implementation of <see cref="IDateTimeSerializationOptions"/>.
	/// </summary>
	internal sealed class ImmutableDateTimeSerializationOptions : IDateTimeSerializationOptions
	{
		/// <inheritdoc />
		public ISet<Type> KnownDateTimeLikeTypes { get; }

		private readonly DateTimeConversionMethod? _defaultDateTimeConversionMethod;

		/// <inheritdoc />
		public DateTimeConversionMethod GetDefaultDateTimeConversionMethod(CodecFeatures codecFeatures)
		{
			Debug.Assert(codecFeatures != null);
			return this._defaultDateTimeConversionMethod ?? codecFeatures.PreferredDateTimeConversionMethod;
		}

		private readonly int? _iso8601FractionOfSecondsPrecision;

		/// <inheritdoc />
		public int? GetIso8601SubsecondsPrecision(CodecFeatures codecFeatures)
			=> codecFeatures?.Iso8601FractionOfSecondsPrecision ?? this._iso8601FractionOfSecondsPrecision;

		private readonly char? _iso8601DecimalMark;

		/// <inheritdoc />
		public char? GetIso8601DecimalMark(CodecFeatures codecFeatures)
			=> codecFeatures?.Iso8601DecimalSeparator ?? this._iso8601DecimalMark;

		/// <summary>
		///		Initializes a new instance of <see cref="IDateTimeSerializationOptions"/> object.
		/// </summary>
		/// <param name="builder"><see cref="DateTimeSerializationOptionsBuilder"/>.</param>
		public ImmutableDateTimeSerializationOptions(DateTimeSerializationOptionsBuilder builder)
		{
			this.KnownDateTimeLikeTypes = builder.KnownDateTimeLikeTypeSet;
			this._iso8601DecimalMark = builder.Iso8601DecimalMark;
			this._iso8601FractionOfSecondsPrecision = builder.Iso8601FractionOfSecondsPrecision;
			this._defaultDateTimeConversionMethod = builder.DefaultDateTimeConversionMethod;
		}
	}
}
