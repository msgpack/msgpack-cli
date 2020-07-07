// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Specialized implementation of <see cref="IReflectionObjectSerializer"/> for arrays.
	/// </summary>
	internal sealed class ArrayDelegateSerializer : IReflectionObjectSerializer
	{
		private readonly Type _targetType;
		private readonly bool _itemIsCollection;
		private readonly Serialization _serializeItem;
		private readonly Deserialization _deserializeItem;
#if FEATURE_TAP
		private readonly AsyncDeserialization _deserializeItemAsync;
#endif // FEATURE_TAP

		public ArrayDelegateSerializer(
			Serialization serializeItem,
			Deserialization deserializeItem,
#if FEATURE_TAP
			AsyncDeserialization deserializeItemAsync,
#endif // FEATURE_TAP
			Type targetType,
			in CollectionTraits traits
		)
		{
			this._targetType = targetType;
			this._serializeItem = serializeItem;
			this._deserializeItem = deserializeItem;
#if FEATURE_TAP
			this._deserializeItemAsync = deserializeItemAsync;
#endif // FEATURE_TAP
#warning TODO: allowNonCollectionEnumerableTypes
			this._itemIsCollection = traits.ElementType!.GetCollectionTraits(CollectionTraitOptions.None, allowNonCollectionEnumerableTypes: false).CollectionType != CollectionKind.NotCollection;
		}

		public void Serialize(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink)
		{
			var encoder = context.Encoder;

			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			var array = (object?[])obj;

			encoder.EncodeArrayStart(array.Length, sink, context.CollectionContext);

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			for (var i = 0; i < array.Length; i++)
			{
				encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
				this._serializeItem(ref context, array[i], sink);
				encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);

				i++;
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			encoder.EncodeArrayEnd(array.Length, sink, context.CollectionContext);
		}

		public object? Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var decoder = context.Decoder;
			if (decoder.TryDecodeNull(ref source))
			{
				return null;
			}

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			object? result;
			if (decoder.Options.Features.CanCountCollectionItems)
			{
				result = this.Deserialize(decoder.DecodeArrayHeader(ref source), ref context, ref source);
			}
			else
			{
				result = this.Deserialize(decoder, ref context, ref source);
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			return result;
		}

		private object?[] Deserialize(int itemsCount, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = new object?[itemsCount];

			for (var i = 0; i < itemsCount; i++)
			{
				result[i] = this._deserializeItem(ref context, ref source);
			}

			return result;
		}

		private object?[] Deserialize(FormatDecoder decoder, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = new List<object?>();

			var iterator = decoder.DecodeArray(ref source);
			while (!iterator.CollectionEnds(ref source))
			{
				result.Add(this._deserializeItem(ref context, ref source));
			}
			iterator.Drain(ref source);

			return result.ToArray();
		}

		public bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
			=> throw new NotSupportedException($"Array serializer does not support DeserializeTo.");

#if FEATURE_TAP

		public async ValueTask<object?> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var decoder = context.Decoder;

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			object? result;
			if (decoder.Options.Features.CanCountCollectionItems)
			{
				result =
					await this.DeserializeAsync(
						await decoder.DecodeArrayHeaderAsync(source, context.CancellationToken).ConfigureAwait(false),
						context,
						source
					).ConfigureAwait(false);
			}
			else
			{
				result = await this.DeserializeAsync(decoder, context, source).ConfigureAwait(false);
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			return result;
		}

		private async ValueTask<object?[]> DeserializeAsync(int itemsCount, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = new object?[itemsCount];

			for (var i = 0; i < itemsCount; i++)
			{
				result[i] = await this._deserializeItemAsync(context, source).ConfigureAwait(false);
			}

			return result;
		}

		private async ValueTask<object?[]> DeserializeAsync(FormatDecoder decoder, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = new List<object?>();

			var iterator = await decoder.DecodeArrayAsync(source, context.CancellationToken).ConfigureAwait(false);
			while (!await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				result.Add(await this._deserializeItemAsync(context, source).ConfigureAwait(false));
			}
			await iterator.DrainAsync(source, context.CancellationToken).ConfigureAwait(false);

			return result.ToArray();
		}

		public ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(this._targetType);
			// never
			return default;
		}
#endif // FEATURE_TAP
	}
}
