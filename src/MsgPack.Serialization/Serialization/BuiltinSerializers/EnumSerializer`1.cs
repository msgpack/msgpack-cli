// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.BuiltinSerializers
{
	/// <summary>
	///		Implements basic features of enum serializer.
	/// </summary>
	/// <typeparam name="T">The type of the target enum type.</typeparam>
	internal abstract class EnumSerializer<T> : ObjectSerializer<T>
	{
		private readonly EnumSerializationMethod? _method;
		private readonly NameMapper _nameMapper;

#pragma warning disable CS8714
		private readonly Dictionary<T, string> _serializationNameMapping;
#pragma warning restore CS8714
		private readonly Dictionary<string, T> _deserializationNameMapping;

		protected EnumSerializer(
			SerializerProvider ownerProvider,
			EnumSerializationMethod? method,
			NameMapper nameMapper,
#pragma warning disable CS8714
			Dictionary<T, string> serializationNameMapping,
#pragma warning restore CS8714
			Dictionary<string, T> deserializationNameMapping
		) : base(ownerProvider, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize)
		{
			Debug.Assert(typeof(T).GetIsEnum());
			this._method = method;
			this._nameMapper = nameMapper;
			this._serializationNameMapping = serializationNameMapping;
			this._deserializationNameMapping = deserializationNameMapping;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink)
		{
			var method = this._method ?? this.OwnerProvider.SerializerGenerationOptions.EnumOptions.GetSerializationMethod(context.Encoder.Options.Features);
			if (method == EnumSerializationMethod.ByName)
			{
				if (!this._serializationNameMapping.TryGetValue(obj!, out var asString))
				{
					// May be undefined value which should be numeric.
					asString = obj!.ToString() ?? String.Empty;
				}

				context.Encoder.EncodeString(this._nameMapper.SerializeName(asString), sink, context.Options.DefaultStringEncoding, context.CancellationToken);
			}
			else
			{
				this.SerializeUnderlyingValue(ref context, obj!, sink);
			}
		}

		protected abstract void SerializeUnderlyingValue(ref SerializationOperationContext context, [DisallowNull]T obj, IBufferWriter<byte> sink);

		[return: MaybeNull]
		public sealed override T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var position = source.Consumed;
			context.Decoder.DecodeItem(ref source, out var decoded, context.CancellationToken);
			return this.DeserializeFromItem(context.Options.DefaultStringEncoding, decoded, position);
		}

		private T DeserializeFromItem(Encoding? defaultStringEncoding, in DecodeItemResult decoded, long position)
		{
			switch (decoded.ElementType)
			{
				case ElementType.String:
				{
					var asString = this._nameMapper.DeserializeName((defaultStringEncoding ?? Encoding.UTF8).GetString(decoded.Value));

					if (!this._deserializationNameMapping.TryGetValue(asString, out var result))
					{
						if (asString.Length == 1)
						{
							return this.FromChar(asString[0]);
						}

						Throw.UndefinedEnumName(asString, typeof(T), position);
						return default!; // never reaches
					}

					return result;
				}
				case ElementType.True:
				{
					return this.FromBoolean(true);
				}
				case ElementType.False:
				{
					return this.FromBoolean(false);
				}
				case ElementType.Int32:
				{
					Debug.Assert(decoded.Value.Length == sizeof(int));
					Span<byte> buffer = stackalloc byte[sizeof(int)];
					decoded.Value.CopyTo(buffer);

					return this.FromInt32(BinaryPrimitives.ReadInt32BigEndian(buffer));
				}
				case ElementType.Int64:
				{
					Debug.Assert(decoded.Value.Length == sizeof(long));
					Span<byte> buffer = stackalloc byte[sizeof(long)];
					decoded.Value.CopyTo(buffer);

					return this.FromInt64(BinaryPrimitives.ReadInt64BigEndian(buffer));
				}
				case ElementType.UInt64:
				{
					Debug.Assert(decoded.Value.Length == sizeof(ulong));
					Span<byte> buffer = stackalloc byte[sizeof(ulong)];
					decoded.Value.CopyTo(buffer);

					return this.FromUInt64(BinaryPrimitives.ReadUInt64BigEndian(buffer));
				}
				case ElementType.Single:
				{
					Debug.Assert(decoded.Value.Length == sizeof(float));
					Span<byte> buffer = stackalloc byte[sizeof(float)];
					decoded.Value.CopyTo(buffer);

					return this.FromSingle(BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32BigEndian(buffer)));
				}
				case ElementType.Double:
				{
					Debug.Assert(decoded.Value.Length == sizeof(double));
					Span<byte> buffer = stackalloc byte[sizeof(double)];
					decoded.Value.CopyTo(buffer);

					return this.FromDouble(BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64BigEndian(buffer)));
				}
				default:
				{
					Throw.DecodedTypeIsNotEnum(decoded.ElementType, position);
					return default!; // Never reaches
				}
			}
		}

		protected abstract T FromBoolean(bool value);

		protected abstract T FromChar(char value);
		
		protected abstract T FromInt32(int value);
		
		protected abstract T FromInt64(long value);
		
		protected abstract T FromUInt64(ulong value);
		
		protected abstract T FromSingle(float value);
		
		protected abstract T FromDouble(double value);

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(typeof(T));
			return default;
		}

#if FEATURE_TAP

		public override async ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			DecodeItemResult decoded;
			var initialPosition = source.Position;

			while (TryDecodeItem(context, source, out decoded, out var requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			return this.DeserializeFromItem(context.Options.DefaultStringEncoding, decoded, source.Position);
		}

		private static bool TryDecodeItem(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out DecodeItemResult decoded, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			context.Decoder.DecodeItem(ref reader, out decoded, out requestHint, context.CancellationToken);
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);
			return true;
		}

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(typeof(T));
			return default;
		}

#endif // FEATURE_TAP

	}
}
