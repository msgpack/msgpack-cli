// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		 Implements non optimized, reflection based serializer.
	/// </summary>
	/// <typeparam name="T">Type of serialization target.</typeparam>
	internal sealed partial class ReflectionSerializer<T> : ObjectSerializer<T>
	{
		// T is PrimitiveEncoder | Stringncoder | BinaryEncoder | ExtensionEncoder | ArrayEncoderToken | DictionaryEncoderToken
		private readonly Serialization _serialize;
		private readonly Deserialization _deserialize;
		private readonly DeserializingFill _deserializeTo;
#if FEATURE_TAP
		private readonly AsyncDeserialization _deserializeAsync;
		private readonly AsyncDeserializingFill _deserializeToAsync;
#endif // FEATURE_TAP

		public ReflectionSerializer(ObjectSerializationContext ownerContext, SerializationTarget target)
			: base(ownerContext, target.GetCapabilities())
		{
#warning TODO: PolymorphismSchema

			IReflectionObjectSerializer serializer;
			if (!target.IsCollection)
			{
				serializer = new ObjectDelegateSerializer(ownerContext, target);
			}
			else if (target.CollectionTraits.CollectionType == CollectionKind.Array)
			{
#warning TODO: PolymorphismSchema
				var itemSerializer = ownerContext.GetSerializer(target.CollectionTraits.ElementType!, null);
				serializer =
					target.CollectionTraits.DetailedCollectionType switch
					{
						CollectionDetailedKind.Array
							=> new ArrayDelegateSerializer(
								itemSerializer.SerializeObject,
								itemSerializer.DeserializeObject,
#if FEATURE_TAP
								itemSerializer.DeserializeObjectAsync,
#endif // FEATURE_TAP
								target.Type,
								target.CollectionTraits
							),
						_ => new CollectionDelegateSerializer(
							itemSerializer.SerializeObject,
							itemSerializer.DeserializeObject,
#if FEATURE_TAP
							itemSerializer.DeserializeObjectAsync,
#endif // FEATURE_TAP
							target.Type,
							target.CollectionTraits
						)
					};
			}
			else
			{
				// Map
				var (keyType, valueType) = target.CollectionTraits.GetKeyValueType();
#warning TODO: PolymorphismSchema
				var keySerializer = ownerContext.GetSerializer(keyType!, null);
#warning TODO: PolymorphismSchema
				var valueSerializer = ownerContext.GetSerializer(valueType!, null);
				serializer =
					new DictionaryDelegateSerializer(
						keySerializer.SerializeObject,
						valueSerializer.SerializeObject,
						keySerializer.DeserializeObject,
						valueSerializer.DeserializeObject,
#if FEATURE_TAP
						keySerializer.DeserializeObjectAsync,
						valueSerializer.DeserializeObjectAsync,
#endif // FEATURE_TAP
						target.Type,
						target.CollectionTraits
					);
			}

			this._serialize = serializer.Serialize;
			this._deserialize = serializer.Deserialize;
			this._deserializeTo = serializer.DeserializeTo;
#if FEATURE_TAP
			this._deserializeAsync = serializer.DeserializeAsync;
			this._deserializeToAsync = serializer.DeserializeToAsync;
#endif // FEATURE_TAP
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink)
			=> this._serialize(ref context, obj, sink);

		[return: MaybeNull]
		public sealed override T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> (T)this._deserialize(ref context, ref source);

		public sealed override async ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
			=> (T)(await this._deserializeAsync(context, source).ConfigureAwait(false))!;

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
			=> this._deserializeTo(ref context, ref source, Ensure.NotNull(obj));

		public sealed override async ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
			=> await this._deserializeToAsync(context, source, Ensure.NotNull(obj)).ConfigureAwait(false);
	}
}
