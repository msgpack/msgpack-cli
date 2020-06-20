// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		A decoder for JSON format.
	/// </summary>
	public abstract partial class JsonDecoder : Decoder<NullExtensionType>
	{
		private static readonly FormatFeatures JsonFormatFeatures =
			new FormatFeaturesBuilder
			{
				CanCountCollectionItems = false,
				CanSpecifyStringEncoding = false,
				SupportsExtensionTypes = false,
				IsContextful = true
			}.Build();

		public new JsonDecoderOptions Options { get; }

		protected JsonDecoder(JsonDecoderOptions options)
			: base(options, JsonFormatFeatures)
		{
			this.Options = options;
		}

		public static JsonDecoder Create(JsonDecoderOptions options)
		{
			if ((options?.ParseOptions).GetValueOrDefault() == JsonParseOptions.None)
			{
				return new SimpleJsonDecoder(options!);
			}
			else
			{
				return new FlexibleJsonDecoder(options!);
			}
		}

		private protected static Utf8UnitStatus TryGetUtf8Unit(ref SequenceReader<byte> source, out ReadOnlySequence<byte> unit)
		{
			if (source.UnreadSpan.IsEmpty)
			{
				unit = default;
				return Utf8UnitStatus.TooShort;
			}

			var utf80 = source.UnreadSpan[0];
			if ((utf80 & 0b_1000_0000) == 0)
			{
				// 1byte
				unit = source.Sequence.Slice(source.Position, 1);
				return Utf8UnitStatus.Valid;
			}
			else if ((utf80 & 0b_1110_0000) == 0b_1100_0000)
			{
				Span<byte> utf8 = stackalloc byte[2];
				if (!source.TryCopyTo(utf8))
				{
					unit = source.Sequence.Slice(source.Position);
					return Utf8UnitStatus.TooShort;
				}

				unit = source.Sequence.Slice(source.Position, 2);

				// 2 bytes, 5 + 6 bits
				var bits1 = utf8[0] & 0b_0001_1111;
				var bits2 = utf8[1] & 0b_0011_1111;

				if ((bits1 & 0b_1_1110) == 0
					|| (utf8[1] & 0b_1100_0000) != 0b_1000_0000)
				{
					return Utf8UnitStatus.Invalid;
				}

				return Utf8UnitStatus.Valid;
			}
			else if ((utf80 & 0b_1111_0000) == 0b_1110_0000)
			{
				Span<byte> utf8 = stackalloc byte[3];
				if (!source.TryCopyTo(utf8))
				{
					unit = source.Sequence.Slice(source.Position);
					return Utf8UnitStatus.TooShort;
				}

				unit = source.Sequence.Slice(source.Position, 3);

				if ((utf8[1] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[2] & 0b_1100_0000) != 0b_1000_0000)
				{
					return Utf8UnitStatus.Invalid;
				}

				// 3 bytes, 4 + 6 + 6 bits
				var bits1 = utf8[0] & 0b_0000_1111;
				var bits2 = utf8[1] & 0b_0011_1111;
				var bits3 = utf8[2] & 0b_0011_1111;

				ushort codePoint = (ushort)((bits1 << 12) | (bits2 << 6) | bits3);

				if ((codePoint & 0b_1111_100000_000000) == 0 || !Rune.IsValid(codePoint))
				{
					return Utf8UnitStatus.Invalid;
				}

				return Utf8UnitStatus.Valid;
			}
			else if ((utf80 & 0b_1111_1000) == 0b_1111_0000)
			{
				Span<byte> utf8 = stackalloc byte[4];
				if (!source.TryCopyTo(utf8))
				{
					unit = source.Sequence.Slice(source.Position);
					return Utf8UnitStatus.TooShort;
				}

				unit = source.Sequence.Slice(source.Position, 4);

				if ((utf8[1] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[2] & 0b_1100_0000) != 0b_1000_0000
					|| (utf8[3] & 0b_1100_0000) != 0b_1000_0000)
				{
					return Utf8UnitStatus.Invalid;
				}

				// 4 bytes, 3 + 6 + 6 + 6 bits
				var bits1 = (utf8[0] & 0b_0000_0111) << 18;
				var bits2 = (utf8[1] & 0b_0011_1111) << 12;
				var bits3 = (utf8[2] & 0b_0011_1111) << 6;
				var bits4 = (utf8[3] & 0b_0011_1111);

				int codePoint = ((bits1 << 18) | (bits2 << 12) | (bits1 << 6) | bits4);

				if ((codePoint & 0b_111_110000_000000_000000) == 0)
				{
					return Utf8UnitStatus.Invalid;
				}

				return Utf8UnitStatus.Valid;
			}
			else
			{
				unit = source.Sequence.Slice(source.Position, 1);
				return Utf8UnitStatus.Invalid;
			}
		}

		protected abstract long ReadTrivia(ref SequenceReader<byte> source);

		protected enum Utf8UnitStatus
		{
			TooShort = 0,
			Valid = 1,
			Invalid = 2
		}
	}
}
