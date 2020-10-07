// Copyright (c) FUJIWARA, Yusuke and all cotributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Diagnostics;
using System.Globalization;
using System.Text;
#if FEATURE_TAP
using System.Threading.Tasks;
#endif // FEATURE_TAP
using MsgPack.Internal;

namespace MsgPack.Serialization.BuiltinSerializers
{
	/// <summary>
	///		Implements date time serializer for <see cref="DateTimeOffset"/>.
	/// </summary>
	internal sealed class DateTimeOffsetObjectSerializer : NonCollectionObjectSerializer<DateTimeOffset>
	{
		private readonly DateTimeConversionMethod? _method;

		public DateTimeOffsetObjectSerializer(SerializerProvider ownerProvider, DateTimeConversionMethod? method)
			: base(ownerProvider)
		{
			this._method = method;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, DateTimeOffset obj, IBufferWriter<byte> sink)
		{
			var options = this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions;

			switch (this._method ?? options.GetDefaultDateTimeConversionMethod(context.Encoder.Options.Features))
			{
				case DateTimeConversionMethod.Native:
				{
					context.IncrementDepth();
					context.Encoder.EncodeArrayStart(2, sink, context.CollectionContext);
					context.Encoder.EncodeArrayItemStart(0, sink, context.CollectionContext);
					DateTimeObjectSerializer.Serialize(ref context, obj.DateTime, sink, this._method, options);
					context.Encoder.EncodeArrayItemEnd(0, sink, context.CollectionContext);
					context.Encoder.EncodeArrayItemStart(1, sink, context.CollectionContext);
					Debug.Assert(obj.Offset.TotalMinutes >= Int16.MinValue && obj.Offset.TotalMinutes <= Int16.MaxValue);
					context.Encoder.EncodeInt32((int)obj.Offset.TotalMinutes, sink);
					context.Encoder.EncodeArrayItemEnd(1, sink, context.CollectionContext);
					context.Encoder.EncodeArrayEnd(2, sink, context.CollectionContext);
					context.DecrementDepth();
					break;
				}
				case DateTimeConversionMethod.UnixEpoc:
				{
					context.Encoder.EncodeInt64(MessagePackConvert.FromDateTimeOffset(obj), sink);
					break;
				}
				case DateTimeConversionMethod.Iso8601ExtendedFormat:
				{
					var features = context.Encoder.Options.Features;
					var format =
						Iso8601.GetFormatString(
							options.GetIso8601DecimalMark(features) ?? '.',
							options.GetIso8601SubsecondsPrecision(features) ?? 7
						);
					if (format == null && (context.StringEncoding ?? features.DefaultStringEncoding) is UTF8Encoding)
					{
						// fast path
						Span<byte> buffer = stackalloc byte[Iso8601.MaxLength];
						var shouldBeTrue = Utf8Formatter.TryFormat(obj, buffer, out var written, Iso8601.StandardFormat);
						Debug.Assert(shouldBeTrue);
						context.Encoder.EncodeString(buffer.Slice(0, written), buffer.Length, sink, context.CancellationToken);
					}
					else
					{
						// slow path
						Span<char> buffer = stackalloc char[Iso8601.MaxLength];
						var shouldBeTrue = obj.TryFormat(buffer, out var written, format, CultureInfo.InvariantCulture);
						Debug.Assert(shouldBeTrue);
						context.Encoder.EncodeString(buffer.Slice(0, written), sink, context.StringEncoding, context.CancellationToken);
					}

					break;
				}
				case DateTimeConversionMethod.Timestamp:
				default:
				{
					Span<byte> buffer = stackalloc byte[12];
					var bytesUsed = Timestamp.FromDateTimeOffset(obj).Encode(buffer);

					context.Encoder.EncodeExtension(ExtensionType.Timestamp, buffer.Slice(0, bytesUsed), sink);
					break;
				}
			}
		}

		public sealed override DateTimeOffset Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var position = source.Consumed;
			try
			{
				context.Decoder.DecodeItem(ref source, out var result, context.CancellationToken);
				position = source.Consumed - result.Value.Length;
				return
					Deserialize(
						context,
						ref source,
						result,
						this._method,
						this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions,
						ref position
					);
			}
			catch (DecodeException ex)
			{
				Throw.DecodeFailure(ex, position);
				throw; // Never reaches
			}
		}
		private static DateTimeOffset Deserialize(
			in DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			in DecodeItemResult result,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options,
			ref long position
		)
		{
			switch (result.ElementType)
			{
				case ElementType.String:
				{
					return DeserializeIso8601(context, result, position);
				}
				case ElementType.Extension:
				{
					// Timestamp
					return Timestamp.Decode(new ExtensionTypeObject(result.ExtensionType, result.ExtensionBody)).ToDateTimeOffset();
				}
				case ElementType.Int32:
				{
					Debug.Assert(result.Value.Length == sizeof(int));
					Span<byte> buffer = stackalloc byte[sizeof(int)];
					result.Value.CopyTo(buffer);
					return MessagePackConvert.ToDateTimeOffset(BinaryPrimitives.ReadInt32BigEndian(buffer));
				}
				case ElementType.Int64:
				case ElementType.UInt64:
				{
					Debug.Assert(result.Value.Length == sizeof(long));
					Span<byte> buffer = stackalloc byte[sizeof(long)];
					result.Value.CopyTo(buffer);
					return MessagePackConvert.ToDateTimeOffset(BinaryPrimitives.ReadInt64BigEndian(buffer));
				}
				case ElementType.Array:
				{
					bool canOmitIterator = result.CollectionLength >= 0;
					var arrayStartPosition = source.Consumed;

					if (canOmitIterator && result.CollectionLength != 2)
					{
						Throw.InvalidDateTimeOffsetArray(result.CollectionLength, position);
					}

					var decoder = context.Decoder;

					var originalSourcePosition = source.Consumed;

					var dateTimeBits = decoder.DecodeInt64(ref source);
					var dateTime = DateTime.FromBinary(dateTimeBits);

					position += source.Consumed - originalSourcePosition;

					if (!canOmitIterator)
					{
						if (result.CollectionIterator.CollectionEnds(ref source))
						{
							Throw.InvalidDateTimeOffsetArray(1, arrayStartPosition);
						}
					}

					var offset = TimeSpan.FromMinutes(decoder.DecodeInt16(ref source));

					if (!canOmitIterator)
					{
						if (!result.CollectionIterator.CollectionEnds(ref source))
						{
							Throw.InvalidDateTimeOffsetArray(arrayStartPosition);
						}
					}

					// The position points offset here because it will be used to report offset validation error.
					return new DateTimeOffset(dateTime, offset);
				}
				default:
				{
					Throw.InvalidDateTimeFormat(result.ElementType, typeof(DateTimeOffset), position);
					return default; // Never reaches
				}
			}
		}
		public static DateTimeOffset DeserializeIso8601(
			in DeserializationOperationContext context,
			in DecodeItemResult result,
			long position
		)
		{
			var features = context.Decoder.Options.Features;

			// Iso8601
			var encoding = context.StringEncoding ?? features.DefaultStringEncoding;
			if (encoding is UTF8Encoding)
			{
				if (result.Value.Length > Iso8601.MaxLength)
				{
					Throw.TooLongIso8601FormatString(result.Value.Length, Iso8601.MaxLength, position);
				}

				Span<byte> buffer = stackalloc byte[unchecked((int)result.Value.Length)];
				result.Value.CopyTo(buffer);
				if (!Utf8Parser.TryParse(buffer, out DateTimeOffset dateTimeOffset, out _, 'o'))
				{
					Throw.InvalidIso8601FormatString(buffer, position);
				}

				return dateTimeOffset.UtcDateTime;
			}
			else
			{
				var maxLength = encoding.GetMaxByteCount(Iso8601.MaxLength);
				if (result.Value.Length > maxLength)
				{
					Throw.TooLongIso8601FormatString(result.Value.Length, maxLength, encoding, position);
				}

				Span<byte> buffer = stackalloc byte[unchecked((int)result.Value.Length)];
				Span<char> chars = stackalloc char[Iso8601.MaxLength];
				int charsUsed = 0;
				result.Value.CopyTo(buffer);
				try
				{
					charsUsed = encoding.GetChars(buffer, chars);
				}
				catch (DecoderFallbackException ex)
				{
					Throw.InvalidIso8601FormatString(buffer, position, ex);
				}

				chars = chars.Slice(0, charsUsed);

				if (!DateTimeOffset.TryParse(chars, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeOffset))
				{
					Throw.InvalidIso8601FormatString(chars.ToString(), position);
				}

				return dateTimeOffset;
			}
		}


#if FEATURE_TAP
		public sealed override async ValueTask<DateTimeOffset> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var position = source.Position;
			try
			{

				DecodeItemResult result;
				while (!TryDecodeItem(context, source, out result, out var requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken);
				}

				position = source.Position - result.Value.Length;
				return
					Deserialize(
						context,
						source,
						result,
						this._method,
						this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions,
						ref position
					);
			}
			catch (Exception ex)
			{
				Throw.DecodeFailure(ex, position);
				throw; // Never reaches
			}
		}
		private static DateTimeOffset Deserialize(
			AsyncDeserializationOperationContext context,
			ReadOnlyStreamSequence source,
			in DecodeItemResult result,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options,
			ref long position
		)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			var deserialized =
				Deserialize(
					context.AsDeserializationOperationContext(),
					ref reader,
					result,
					specifiedMethod,
					options,
					ref position
				);
			source.Advance(reader.Consumed);
			return deserialized;
		}

		private static bool TryDecodeItem(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out DecodeItemResult result, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			context.Decoder.DecodeItem(ref reader, out result, out requestHint, context.CancellationToken);
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);
			return true;
		}

#endif // FEATURE_TAP
	}
}
