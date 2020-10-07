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
	///		Implements <see cref="IReflectionObjectSerializer"/> for <see cref="IDictionary"/>, <see cref="IDictionary{TKey, TValue}"/>, and <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
	/// </summary>
	internal sealed class DictionaryDelegateSerializer : IReflectionObjectSerializer
	{
		private readonly bool _keyIsCollection;
		private readonly bool _valueIsCollection;
		private readonly Func<object, int> _getCount;
		private readonly Func<object, IEnumerator> _getEnumerator;
		private readonly Func<object, object> _getKey;
		private readonly Func<object, object> _getValue;
		private readonly Serialization _serializeKey;
		private readonly Serialization _serializeValue;
		private readonly Func<int, object>? _constructorWithCapacity;
		private readonly Func<object>? _defaultConstructor;
		private readonly Action<object, object?, object?> _add;
		private readonly Deserialization _deserializeKey;
		private readonly Deserialization _deserializeValue;
#if FEATURE_TAP
		private readonly AsyncDeserialization _deserializeKeyAsync;
		private readonly AsyncDeserialization _deserializeValueAsync;
#endif // FEATURE_TAP

		public DictionaryDelegateSerializer(
			Serialization serializeKey,
			Serialization serializeValue,
			Deserialization deserializeKey,
			Deserialization deserializeValue,
#if FEATURE_TAP
			AsyncDeserialization deserializeKeyAsync,
			AsyncDeserialization deserializeValueAsync,
#endif // FEATURE_TAP
			Type targetType,
			in CollectionTraits traits,
			ISerializerGenerationOptions options
		)
		{
			var countPropertyGetter = traits.CountPropertyGetter!;
			var getEnumeratorMethod = traits.GetEnumeratorMethod!;
			var addMethod = traits.AddMethod;
			var constructorWithCapacity = traits.ConstructorWithCapacity;
			var defaultConstructor = traits.DefaultConstructor;

			var keyPropertyGetter = traits.ElementType!.GetProperty("Key")!.GetGetMethod()!;
			var valuePropertyGetter = traits.ElementType!.GetProperty("Value")!.GetGetMethod()!;

			this._getEnumerator = c => (IEnumerator)getEnumeratorMethod.Invoke(c, BindingFlags.DoNotWrapExceptions, null, null, null)!;
			this._getCount = c => (int)countPropertyGetter.Invoke(c, BindingFlags.DoNotWrapExceptions, null, null, null)!;
			this._getKey = e => (string)keyPropertyGetter.Invoke(e, BindingFlags.DoNotWrapExceptions, null, null, null)!;
			this._getValue = e => valuePropertyGetter.Invoke(e, BindingFlags.DoNotWrapExceptions, null, null, null)!;

			this._add =
				addMethod == null ?
					new Action<object, object?, object?>((c, k, v) => Throw.UndeserializableCollection(targetType)) :
					new Action<object, object?, object?>((c, k, v) => addMethod.Invoke(c, BindingFlags.DoNotWrapExceptions, null, new object?[] { k, v }, null));
			var (keyType, valueType) = traits.GetKeyValueType();

			this._constructorWithCapacity =
				constructorWithCapacity == null ?
					default(Func<int, object>) :
					c => constructorWithCapacity!.Invoke(BindingFlags.DoNotWrapExceptions, new object?[] { c })!;
			this._defaultConstructor = () => defaultConstructor!.Invoke(BindingFlags.DoNotWrapExceptions, null)!;
			if (this._constructorWithCapacity == null && this._defaultConstructor == null)
			{
				Throw.NoConstructorForCollection(targetType);
			}

			this._serializeKey = serializeKey;
			this._serializeValue = serializeValue;

			this._deserializeKey = deserializeKey;
			this._deserializeValue = deserializeValue;
#if FEATURE_TAP
			this._deserializeKeyAsync = deserializeKeyAsync;
			this._deserializeValueAsync = deserializeValueAsync;
#endif // FEATURE_TAP

			this._keyIsCollection =
				keyType!.GetCollectionTraits(
					CollectionTraitOptions.None, 
					allowNonCollectionEnumerableTypes: options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes
				).CollectionType != CollectionKind.NotCollection;
			this._valueIsCollection =
				valueType!.GetCollectionTraits(
					CollectionTraitOptions.None, allowNonCollectionEnumerableTypes: options.CompatibilityOptions.AllowsNonCollectionEnumerableTypes
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

			var count = this._getCount(obj);
			var i = 0;
			var enumerator = this._getEnumerator(obj);
			try
			{
				encoder.EncodeMapStart(count, sink, context.CollectionContext);
				while (enumerator.MoveNext())
				{
					var entry = enumerator.Current;
					var key = this._getKey(entry!);
					var value = this._getValue(entry!);

					encoder.EncodeMapKeyStart(i, sink, context.CollectionContext);

					if (this._keyIsCollection)
					{
						context.IncrementDepth();
					}

					this._serializeKey(ref context, key, sink);

					if (this._keyIsCollection)
					{
						context.DecrementDepth();
					}

					encoder.EncodeMapKeyEnd(i, sink, context.CollectionContext);

					encoder.EncodeMapValueStart(i, sink, context.CollectionContext);

					if (this._valueIsCollection)
					{
						context.IncrementDepth();
					}

					this._serializeValue(ref context, key, sink);

					if (this._valueIsCollection)
					{
						context.DecrementDepth();
					}

					encoder.EncodeMapValueEnd(i, sink, context.CollectionContext);

					i++;
				}
				encoder.EncodeMapEnd(count, sink, context.CollectionContext);
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}

		public object? Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var decoder = context.Decoder;
			if (decoder.TryDecodeNull(ref source))
			{
				return null;
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

			return result;
		}

		private object Deserialize(int itemsCount, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = this._constructorWithCapacity?.Invoke(itemsCount) ?? this._defaultConstructor!();

			for (var i = 0; i < itemsCount; i++)
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = this._deserializeKey(ref context, ref source);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = this._deserializeValue(ref context, ref source);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(result, key, value);
			}

			return result;
		}

		private object Deserialize(FormatDecoder decoder, ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!(0);

			var iterator = decoder.DecodeArray(ref source);

			while (!iterator.CollectionEnds(ref source))
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = this._deserializeKey(ref context, ref source);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = this._deserializeValue(ref context, ref source);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(result, key, value);
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

			if (decoder.Options.Features.CanCountCollectionItems)
			{
				this.DeserializeTo(decoder.DecodeArrayHeader(ref source), ref context, ref source, obj);
			}
			else
			{
				this.DeserializeTo(decoder, ref context, ref source, obj);
			}

			return true;
		}

		private void DeserializeTo(int itemsCount, ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			for (var i = 0; i < itemsCount; i++)
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = this._deserializeKey(ref context, ref source);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = this._deserializeValue(ref context, ref source);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(obj, key, value);
			}
		}

		private void DeserializeTo(FormatDecoder decoder, ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			var iterator = decoder.DecodeArray(ref source);
			while (!iterator.CollectionEnds(ref source))
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = this._deserializeKey(ref context, ref source);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = this._deserializeValue(ref context, ref source);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(obj, key, value);
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

			return result;
		}

		private async ValueTask<object?> DeserializeAsync(int itemsCount, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = this._constructorWithCapacity?.Invoke(itemsCount) ?? this._defaultConstructor!.Invoke();

			for (var i = 0; i < itemsCount; i++)
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = await this._deserializeKeyAsync(context, source).ConfigureAwait(false);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = await this._deserializeValueAsync(context, source).ConfigureAwait(false);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(result, key, value);
			}

			return result;
		}

		private async ValueTask<object?> DeserializeAsync(FormatDecoder decoder, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!.Invoke(0);

			var iterator = await decoder.DecodeArrayAsync(source, context.CancellationToken).ConfigureAwait(false);

			while (!await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = await this._deserializeKeyAsync(context, source).ConfigureAwait(false);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = await this._deserializeValueAsync(context, source).ConfigureAwait(false);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(result, key, value);
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

			return true;
		}

		private async ValueTask DeserializeToAsync(int itemsCount, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			for (var i = 0; i < itemsCount; i++)
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = await this._deserializeKeyAsync(context, source).ConfigureAwait(false);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = await this._deserializeValueAsync(context, source).ConfigureAwait(false);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(obj, key, value);
			}
		}

		private async ValueTask DeserializeToAsync(FormatDecoder decoder, AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			var result = this._defaultConstructor?.Invoke() ?? this._constructorWithCapacity!.Invoke(0);

			var iterator = await decoder.DecodeArrayAsync(source, context.CancellationToken).ConfigureAwait(false);

			while (!await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				if (this._keyIsCollection)
				{
					context.IncrementDepth();
				}

				var key = await this._deserializeKeyAsync(context, source).ConfigureAwait(false);

				if (this._keyIsCollection)
				{
					context.DecrementDepth();
				}

				if (this._valueIsCollection)
				{
					context.IncrementDepth();
				}

				var value = await this._deserializeValueAsync(context, source).ConfigureAwait(false);

				if (this._valueIsCollection)
				{
					context.DecrementDepth();
				}

				this._add(result, key, value);
			}

			await iterator.DrainAsync(source, context.CancellationToken).ConfigureAwait(false);
		}

#endif // FEATURE_TAP
	}
}
