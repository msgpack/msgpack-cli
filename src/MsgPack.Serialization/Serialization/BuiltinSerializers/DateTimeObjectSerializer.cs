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
	///		Implements date time serializer for <see cref="DateTime"/>.
	/// </summary>
	internal sealed class DateTimeObjectSerializer : NonCollectionObjectSerializer<DateTime>
	{
		private readonly DateTimeConversionMethod? _method;

		public DateTimeObjectSerializer(SerializerProvider ownerProvider, DateTimeConversionMethod? method)
			: base(ownerProvider)
		{
			this._method = method;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, DateTime obj, IBufferWriter<byte> sink)
			=> Serialize(ref context, obj, sink, this._method, this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions);

		public static void Serialize(
			ref SerializationOperationContext context,
			DateTime obj,
			IBufferWriter<byte> sink,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options
		)
		{
			switch (specifiedMethod ?? options.GetDefaultDateTimeConversionMethod(context.Encoder.Options.Features))
			{
				case DateTimeConversionMethod.Native:
				{
					context.Encoder.EncodeInt64(obj.ToBinary(), sink);
					break;
				}
				case DateTimeConversionMethod.UnixEpoc:
				{
					context.Encoder.EncodeInt64(MessagePackConvert.FromDateTime(obj), sink);
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
					var bytesUsed = Timestamp.FromDateTime(obj).Encode(buffer);

					context.Encoder.EncodeExtension(ExtensionType.Timestamp, buffer.Slice(0, bytesUsed), sink);
					break;
				}
			}
		}

		public sealed override DateTime Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> Deserialize(ref context, ref source, this._method, this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions);

		public static DateTime Deserialize(
			ref DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options
		)
		{
			var position = source.Consumed;
			try
			{
				context.Decoder.DecodeItem(ref source, out var result, context.CancellationToken);
				position = source.Consumed - result.Value.Length;
				return Deserialize(context, result, specifiedMethod, options, position);
			}
			catch (Exception ex)
			{
				Throw.DecodeFailure(ex, position);
				throw; // Never reaches
			}
		}
		private static DateTime Deserialize(
			in DeserializationOperationContext context,
			in DecodeItemResult result,
			DateTimeConversionMethod? specifiedMethod,
			IDateTimeSerializationOptions options,
			long position
		)
		{
			try
			{
				long bits;
				switch (result.ElementType)
				{
					case ElementType.String:
					{
						return DateTimeOffsetObjectSerializer.DeserializeIso8601(context, result, position).UtcDateTime;
					}
					case ElementType.Extension:
					{
						// Timestamp
						return Timestamp.Decode(new ExtensionTypeObject(result.ExtensionType, result.ExtensionBody)).ToDateTime();
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
						Throw.InvalidDateTimeFormat(result.ElementType, typeof(DateTime), position);
						return default; // Never reaches
					}
				}

				// Native or UnixEpoc
				if ((specifiedMethod ?? options.GetDefaultDateTimeConversionMethod(context.Decoder.Options.Features)) == DateTimeConversionMethod.UnixEpoc)
				{
					return MessagePackConvert.ToDateTime(bits);
				}
				else
				{
					return DateTime.FromBinary(bits);
				}
			}
			catch (DecodeException ex)
			{
				Throw.DecodeFailure(ex, position);
				throw; // Never reaches
			}
		}

#if FEATURE_TAP
		public sealed override async ValueTask<DateTime> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
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
						context.AsDeserializationOperationContext(),
						result,
						this._method,
						this.OwnerProvider.SerializerGenerationOptions.DateTimeOptions,
						position
					);
			}
			catch (Exception ex)
			{
				Throw.DecodeFailure(ex, position);
				throw; // Never reaches
			}
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
