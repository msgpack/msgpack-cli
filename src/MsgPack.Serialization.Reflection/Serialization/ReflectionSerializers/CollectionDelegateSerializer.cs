// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements <see cref="IReflectionObjectSerializer"/> for <see cref="IEnumerable"/>, <see cref="IEnumerable{T}"/>, 
	///		<see cref="ICollection"/>, <see cref="ICollection{T}"/>, <see cref="IReadOnlyCollection{T}"/>, <see cref="ISet{T}"/>, 
	///		<see cref="IList"/>, <see cref="IList{T}"/>, and <see cref="IReadOnlyList{T}"/> except array.
	/// </summary>
	internal sealed class CollectionDelegateSerializer : IReflectionObjectSerializer
	{
		private readonly bool _itemIsCollection;
		private readonly Func<object, int>? _getCount;
		private readonly Func<object, IEnumerator> _getEnumerator;
		private readonly Serialization _serializeItem;
		private readonly Func<int, object>? _constructorWithCapacity;
		private readonly Func<object>? _defaultConstructor;
		private readonly Action<object, object?> _add;
		private readonly Deserialization _deserializeItem;
#if FEATURE_TAP
		private readonly AsyncDeserialization _deserializeItemAsync;
#endif // FEATURE_TAP

		public CollectionDelegateSerializer(
			Serialization serializeItem,
			Deserialization deserializeItem,
#if FEATURE_TAP
			AsyncDeserialization deserializeItemAsync,
#endif // FEATURE_TAP
			Type targetType,
			in CollectionTraits traits,
			ISerializerGenerationOptions options
		)
		{
			var countPropertyGetter = traits.CountPropertyGetter!;
			var getEnumeratorMethod = traits.GetEnumeratorMethod!;
			var addMethod = traits.AddMethod!;
			var constructorWithCapacity = traits.ConstructorWithCapacity;
			var defaultConstructor = traits.DefaultConstructor;

			this._getEnumerator = c => (IEnumerator)getEnumeratorMethod.Invoke(c, BindingFlags.DoNotWrapExceptions, null, null, null)!;
			this._getCount = c => (int)countPropertyGetter.Invoke(c, BindingFlags.DoNotWrapExceptions, null, null, null)!;
			this._add =
				addMethod == null ?
					new Action<object, object?>((c, i) => Throw.UndeserializableCollection(targetType)) :
					new Action<object, object?>((c, i) => addMethod.Invoke(c, BindingFlags.DoNotWrapExceptions, null, new object?[] { i }, null));
			this._constructorWithCapacity =
				constructorWithCapacity == null ?
					default(Func<int, object>) :
					c => constructorWithCapacity!.Invoke(BindingFlags.DoNotWrapExceptions, new object?[] { c })!;
			this._defaultConstructor = () => defaultConstructor!.Invoke(BindingFlags.DoNotWrapExceptions, null)!;
			if (this._constructorWithCapacity == null && this._defaultConstructor == null)
			{
				Throw.NoConstructorForCollection(targetType);
			}

			this._serializeItem = serializeItem;
			this._deserializeItem = deserializeItem;
#if FEATURE_TAP
			this._deserializeItemAsync = deserializeItemAsync;
#endif // FEATURE_TAP

			this._itemIsCollection =
				traits.ElementType!.GetCollectionTraits(
					CollectionTraitOptions.None,
					allowNonCollectionEnumerableTypes: options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes
				).CollectionType != CollectionKind.NotCollection;
		}

		public void Serialize(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink)
		{
			var encoder = context.Encoder;

			if (obj is null)
			{
				encoder.EncodeNull(sink);
				return;
			}

			if (this._getCount != null)
			{
				this.SerializeCountable(ref context, obj, sink, this._getCount);
			}
			else
			{
				this.SerializeUncountable(ref context, obj, sink);
			}
		}

		private void SerializeCountable(ref SerializationOperationContext context, object obj, IBufferWriter<byte> sink, Func<object, int> getCount)
		{
			var encoder = context.Encoder;

			var count = getCount(obj);
			var i = 0;
			var enumerator = this._getEnumerator(obj);
			try
			{
				encoder.EncodeArrayStart(count, sink, context.CollectionContext);

				if (this._itemIsCollection)
				{
					context.IncrementDepth();
				}

				while (enumerator.MoveNext())
				{
					encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
					this._serializeItem(ref context, enumerator.Current, sink);
					encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
					i++;
				}

				if (this._itemIsCollection)
				{
					context.DecrementDepth();
				}

				encoder.EncodeArrayEnd(count, sink, context.CollectionContext);
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}

		private void SerializeUncountable(ref SerializationOperationContext context, object obj, IBufferWriter<byte> sink)
		{
			var encoder = context.Encoder;

			var asList = new List<object?>();
			var enumerator = this._getEnumerator(obj);
			try
			{
				while (enumerator.MoveNext())
				{
					asList.Add(enumerator.Current);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			var i = 0;
			encoder.EncodeArrayStart(asList.Count, sink, context.CollectionContext);

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			foreach (var item in asList)
			{
				encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
				this._serializeItem(ref context, item, sink);
				encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
				i++;
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			encoder.EncodeArrayEnd(asList.Count, sink, context.CollectionContext);
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

		private object Deserialize(int itemsCount, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = this._constructorWithCapacity?.Invoke(itemsCount) ?? this._defaultConstructor!();

			for (var i = 0; i < itemsCount; i++)
			{
				this._add(result, this._deserializeItem(ref context, ref source));
			}

			return result;
		}

		private object Deserialize(FormatDecoder decoder, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!(0);

			var iterator = decoder.DecodeArray(ref source);
			while (!iterator.CollectionEnds(ref source))
			{
				this._add(result, this._deserializeItem(ref context, ref source));
			}
			iterator.Drain(ref source);

			return result;
		}

		public bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			var decoder = context.Decoder;
			if (decoder.TryDecodeNull(ref source))
			{
				return false;
			}

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			if (decoder.Options.Features.CanCountCollectionItems)
			{
				this.DeserializeTo(decoder.DecodeArrayHeader(ref source), ref context, ref source, obj);
			}
			else
			{
				this.DeserializeTo(decoder, ref context, ref source, obj);
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			return true;
		}

		private void DeserializeTo(int itemsCount, ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			for (var i = 0; i < itemsCount; i++)
			{
				this._add(obj, this._deserializeItem(ref context, ref source));
			}
		}

		private void DeserializeTo(FormatDecoder decoder, ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			var iterator = decoder.DecodeArray(ref source);
			while (!iterator.CollectionEnds(ref source))
			{
				this._add(obj, this._deserializeItem(ref context, ref source));
			}
			iterator.Drain(ref source);
		}

#if FEATURE_TAP

		public async ValueTask<object?> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var decoder = context.Decoder;

			if (await decoder.TryDecodeNullAsync(source, context.CancellationToken).ConfigureAwait(false))
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

		private async ValueTask<object?> DeserializeAsync(int itemsCount, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = this._constructorWithCapacity?.Invoke(itemsCount) ?? this._defaultConstructor!.Invoke();

			for (var i = 0; i < itemsCount; i++)
			{
				this._add(result, await this._deserializeItemAsync(context, source).ConfigureAwait(false));
			}

			return result;
		}

		private async ValueTask<object?> DeserializeAsync(FormatDecoder decoder, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!.Invoke(0);

			var iterator = await decoder.DecodeArrayAsync(source, context.CancellationToken).ConfigureAwait(false);

			while (!await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				this._add(result, await this._deserializeItemAsync(context, source).ConfigureAwait(false));
			}

			await iterator.DrainAsync(source, context.CancellationToken).ConfigureAwait(false);

			return result;
		}

		public async ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			var decoder = context.Decoder;

			if (await decoder.TryDecodeNullAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				return false;
			}

			if (this._itemIsCollection)
			{
				context.IncrementDepth();
			}

			if (decoder.Options.Features.CanCountCollectionItems)
			{
				await this.DeserializeToAsync(
					await decoder.DecodeArrayHeaderAsync(source, context.CancellationToken).ConfigureAwait(false),
					context,
					source,
					obj
				).ConfigureAwait(false);
			}
			else
			{
				await this.DeserializeToAsync(decoder, context, source, obj).ConfigureAwait(false);
			}

			if (this._itemIsCollection)
			{
				context.DecrementDepth();
			}

			return true;
		}

		private async ValueTask DeserializeToAsync(int itemsCount, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			for (var i = 0; i < itemsCount; i++)
			{
				this._add(obj, await this._deserializeItemAsync(context, source).ConfigureAwait(false));
			}
		}

		private async ValueTask DeserializeToAsync(FormatDecoder decoder, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!.Invoke(0);

			var iterator = await decoder.DecodeArrayAsync(source, context.CancellationToken).ConfigureAwait(false);

			while (!await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				this._add(result, await this._deserializeItemAsync(context, source).ConfigureAwait(false));
			}

			await iterator.DrainAsync(source, context.CancellationToken).ConfigureAwait(false);
		}

#endif // FEATURE_TAP
	}
}
