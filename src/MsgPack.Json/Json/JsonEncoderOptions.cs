// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Linq;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines encoder options for <see cref="JsonEncoder"/>.
	/// </summary>
	public sealed class JsonEncoderOptions : FormatEncoderOptions
	{
		public static JsonEncoderOptions Default { get; } = new JsonEncoderOptionsBuilder().Build();

		private static readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> ErrorSingleInfinityFormatter =
			(value, buffer, options) => throw new ArgumentException($"Cannot serialize infinity ({value}) to JSON.", "value");

		private static readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> MinMaxSingleInfinityFormatter =
			(value, buffer, _) => JsonFormatter.Format(Single.IsPositiveInfinity(value) ? Single.MaxValue : Single.MinValue, buffer);

		private static readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> ErrorDoubleInfinityFormatter =
			(value, buffer, options) => throw new ArgumentException($"Cannot serialize infinity ({value}) to JSON.", "value");

		private static readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> MinMaxDoubleInfinityFormatter =
			(value, buffer, _) => JsonFormatter.Format(Double.IsPositiveInfinity(value) ? Double.MaxValue : Double.MinValue, buffer);

		private static readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> ErrorSingleNaNFormatter =
			(value, buffer, options) => throw new ArgumentException($"Cannot serialize NaN to JSON.", "value");
		private static readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions> NullSingleNaNFormatter =
			(value, buffer, options) => JsonFormatter.WriteNull(buffer);

		private static readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> ErrorDoubleNaNFormatter =
			(value, buffer, options) => throw new ArgumentException($"Cannot serialize NaN to JSON.", "value");

		private static readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions> NullDoubleNaNFormatter =
			(value, buffer, options) => JsonFormatter.WriteNull(buffer);

		private readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions>? _singleInfinityFormatter;
		private readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions>? _doubleInfinityFormatter;
		private readonly Action<float, IBufferWriter<byte>, JsonEncoderOptions>? _singleNaNFormatter;
		private readonly Action<double, IBufferWriter<byte>, JsonEncoderOptions>? _doubleNaNFormatter;

		public ReadOnlyMemory<byte> IndentChars { get; }
		public ReadOnlyMemory<byte> NewLineChars { get; }
		public bool IsPrettyPrint { get; }
		public bool EscapesHorizontalTab { get; }
		public bool EscapesHtmlChars { get; }
		public bool EscapesPrivateUseCharactors { get; }
		public ReadOnlyMemory<Rune> EscapeTargetChars { get; }
		internal ReadOnlyMemory<byte> EscapeTargetChars1Byte { get; }
		internal ReadOnlyMemory<ushort> EscapeTargetChars2Byte { get; }
		internal ReadOnlyMemory<int> EscapeTargetChars4Byte { get; }

		public JsonEncoderOptions(JsonEncoderOptionsBuilder builder)
			: base(builder, JsonFormatFeatures.Value)
		{
			this._singleInfinityFormatter =
				builder.InfinityHandling switch
				{
					InfinityHandling.Custom => builder.SingleInfinityFormatter,
					InfinityHandling.Error => ErrorSingleInfinityFormatter,
					InfinityHandling.MinMax => MinMaxSingleInfinityFormatter,
					_ => default,
				};
			this._doubleInfinityFormatter =
				builder.InfinityHandling switch
				{
					InfinityHandling.Custom => builder.DoubleInfinityFormatter,
					InfinityHandling.Error => ErrorDoubleInfinityFormatter,
					InfinityHandling.MinMax => MinMaxDoubleInfinityFormatter,
					_ => default,
				};
			this._singleNaNFormatter =
				builder.NaNHandling switch
				{
					NaNHandling.Custom => builder.SingleNaNFormatter,
					NaNHandling.Error => ErrorSingleNaNFormatter,
					NaNHandling.Null => NullSingleNaNFormatter,
					_ => default,
				};
			this._doubleNaNFormatter =
				builder.NaNHandling switch
				{
					NaNHandling.Custom => builder.DoubleNaNFormatter,
					NaNHandling.Error => ErrorDoubleNaNFormatter,
					NaNHandling.Null => NullDoubleNaNFormatter,
					_ => default,
				};

			this.IndentChars = builder.IndentChars;
			this.NewLineChars = builder.NewLineChars;
			this.IsPrettyPrint = builder.IsPrettyPrint;
			var escapeTargetChars = builder.AdditionalEscapeTargetChars.ToArray().Concat(JsonCharactor.MustBeEscaped1Byte.Select(b => new Rune(b))).Concat(builder.EscapesHtmlChars ? JsonCharactor.ShouldBeEscaped.ToArray() : Array.Empty<Rune>()).Distinct().OrderBy(r => r).ToArray();
			this.EscapeTargetChars = escapeTargetChars;
			this.EscapeTargetChars1Byte = escapeTargetChars.Where(r => r.Value <= Byte.MaxValue).Select(r => (byte)r.Value).ToArray();
			this.EscapeTargetChars2Byte = escapeTargetChars.Where(r => r.Value > Byte.MaxValue && r.Value <= UInt16.MaxValue).Select(r => (ushort)r.Value).ToArray();
			this.EscapeTargetChars4Byte = escapeTargetChars.Where(r => r.Value > UInt16.MaxValue).Select(r => r.Value).ToArray();
			this.EscapesHtmlChars = builder.EscapesHtmlChars;
			this.EscapesHorizontalTab = builder.EscapesHorizontalTab;
			this.EscapesPrivateUseCharactors = builder.EscapesPrivateUseCharactors;
		}

		internal Action<float, IBufferWriter<byte>, JsonEncoderOptions> SingleInfinityFormatter =>
			this._singleInfinityFormatter ?? MinMaxSingleInfinityFormatter;

		internal Action<float, IBufferWriter<byte>, JsonEncoderOptions> SingleNaNFormatter =>
			this._singleNaNFormatter ?? NullSingleNaNFormatter;

		internal Action<double, IBufferWriter<byte>, JsonEncoderOptions> DoubleInfinityFormatter =>
			this._doubleInfinityFormatter ?? MinMaxDoubleInfinityFormatter;

		internal Action<double, IBufferWriter<byte>, JsonEncoderOptions> DoubleNaNFormatter =>
			this._doubleNaNFormatter ?? NullDoubleNaNFormatter;
	}
}
