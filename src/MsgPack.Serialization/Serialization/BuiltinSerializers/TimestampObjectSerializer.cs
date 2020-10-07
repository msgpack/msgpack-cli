// Copyright (c) FUJIWARA, Yusuke and all cotributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
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
	///		Implements date time serializer for <see cref="Timestamp"/>.
	/// </summary>
	internal sealed class TimestampObjectSerializer : NonCollectionObjectSerializer<Timestamp>
	{
		private readonly DateTimeConversionMethod? _method;

		public TimestampObjectSerializer(SerializerProvider ownerProvider, DateTimeConversionMethod? method)
			: base(ownerProvider)
		{
			this._method = method;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, Timestamp obj, IBufferWriter<byte> sink)
		{
			var options = this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions;

			switch (this._method ?? options.GetDefaultDateTimeConversionMethod(context.Encoder.Options.Features))
			{
				case DateTimeConversionMethod.Native:
				{
					context.Encoder.EncodeInt64(obj.ToDateTime().ToBinary(), sink);
					break;
				}
				case DateTimeConversionMethod.UnixEpoc:
				{
					context.Encoder.EncodeInt64(MessagePackConvert.FromDateTime(obj.ToDateTime()), sink);
					break;
				}
				case DateTimeConversionMethod.Iso8601ExtendedFormat:
				{
					//var features = context.Encoder.Options.Features;

#warning TODO: Custom precision support.
					//var format =
					//	Iso8601.GetFormatString(
					//		options.GetIso8601DecimalMark(features) ?? '.',
					//		options.GetIso8601SubsecondsPrecision(features) ?? 9
					//	);
					context.Encoder.EncodeString(obj.ToString(), sink, context.StringEncoding, context.CancellationToken);
					break;
				}
				case DateTimeConversionMethod.Timestamp:
				default:
				{
					Span<byte> buffer = stackalloc byte[12];
					var bytesUsed = obj.Encode(buffer);
					context.Encoder.EncodeExtension(ExtensionType.Timestamp, buffer.Slice(0, bytesUsed), sink);
					break;
				}
			}
		}

		public sealed override Timestamp Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
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
		private static Timestamp Deserialize(
			in DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			in DecodeItemResult result,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options,
			ref long position
		)
		{
			long bits;
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
					bits = BinaryPrimitives.ReadInt32BigEndian(buffer);
					break;
				}
				case ElementType.Int64:
				case ElementType.UInt64:
				{
					Debug.Assert(result.Value.Length == sizeof(long));
					Span<byte> buffer = stackalloc byte[sizeof(long)];
					result.Value.CopyTo(buffer);
					bits = BinaryPrimitives.ReadInt64BigEndian(buffer);
					break;
				}
				default:
				{
					Throw.InvalidDateTimeFormat(result.ElementType, typeof(Timestamp), position);
					return default; // Never reaches
				}
			}

			// Native or UnixEpoc
			if ((specifiedMethod ?? options.GetDefaultDateTimeConversionMethod(context.Decoder.Options.Features)) == DateTimeConversionMethod.UnixEpoc)
			{
				return Timestamp.FromDateTime(MessagePackConvert.ToDateTime(bits));
			}
			else
			{
				return Timestamp.FromDateTime(DateTime.FromBinary(bits));
			}
		}
		private static Timestamp DeserializeIso8601(
			in DeserializationOperationContext context,
			in DecodeItemResult result,
			long position
		)
		{
			var features = context.Decoder.Options.Features;

			// Iso8601
			var encoding = context.StringEncoding ?? features.DefaultStringEncoding;
#warning TODO: Optimize for Utf8Encoding

			var maxLength = encoding.GetMaxByteCount(Iso8601.MaxLength);
			if (result.Value.Length > maxLength)
			{
				Throw.TooLongIso8601FormatString(result.Value.Length, maxLength, encoding, position);
			}

			Span<byte> buffer = stackalloc byte[unchecked((int)result.Value.Length)];
			result.Value.CopyTo(buffer);
			string chars = default!;
			try
			{
				chars = encoding.GetString(buffer);
			}
			catch (DecoderFallbackException ex)
			{
				Throw.InvalidIso8601FormatString(buffer, position, ex);
			}

			if (!Timestamp.TryParseExact(chars, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
			{
				Throw.InvalidIso8601FormatString(chars, position);
			}

			return timestamp;
		}


#if FEATURE_TAP
		public sealed override async ValueTask<Timestamp> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
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
		private static Timestamp Deserialize(
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
