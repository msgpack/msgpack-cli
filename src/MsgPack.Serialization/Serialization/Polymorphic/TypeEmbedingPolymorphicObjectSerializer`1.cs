// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
#if FEATURE_TAP
using System.Threading.Tasks;
using MsgPack.Internal;
#endif // FEATURE_TAP


namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Implements polymorphic serializer which uses open types and non-interoperable ext-type tag and .NET type information.
	/// </summary>
	/// <typeparam name="T">The base type of the polymorhic member.</typeparam>
	internal sealed class TypeEmbedingPolymorphicObjectSerializer<T> : ObjectSerializer<T>
	{
		private readonly PolymorphismSchema _schema;

		public TypeEmbedingPolymorphicObjectSerializer(SerializerProvider ownerProvider, PolymorphismSchema schema)
			: base(ownerProvider, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo)
		{
			if (typeof(T).GetIsValueType())
			{
				Throw.ValueTypeCannotBePolymorphic(typeof(T));
			}

			this._schema = schema.FilterSelf();
		}

		private ObjectSerializer GetActualTypeSerializer(Type actualType)
		{
			var result = this.ObjectSerializerProvider.GetSerializer(actualType, this._schema);
			if (result == null)
			{
				Throw.CannotGetActualTypeSerializer(actualType);
			}

			return result!;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink)
		{
			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			TypeInfoEncoder.EncodeStart(ref context, sink, obj.GetType());
			// T is ref type here, so boxing will not be occurred.
			this.GetActualTypeSerializer(obj.GetType()).SerializeObject(ref context, obj, sink);
			TypeInfoEncoder.EncodeEnd(ref context, sink);
		}

		[return: MaybeNull]
		public sealed override T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> TypeInfoEncoder.Decode(
				ref context,
				ref source,
				(ref DeserializationOperationContext x, ref SequenceReader<byte> s) => TypeInfoEncoder.DecodeRuntimeTypeInfo(ref x, ref s, this._schema.TypeVerifier), // Lamda capture is more efficient.
				(Type t, ref DeserializationOperationContext x, ref SequenceReader<byte> s) => (T)(this.GetActualTypeSerializer(t).DeserializeObject(ref x, ref s)!)
			);

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
			=> this.GetActualTypeSerializer(Ensure.NotNull(obj).GetType()).DeserializeObjectTo(ref context, ref source, obj);

#if FEATURE_TAP

		public sealed override ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
			=> TypeInfoEncoder.DecodeAsync(
					context,
					source,
					(x, s) => TypeInfoEncoder.DecodeRuntimeTypeInfoAsync(x, s, this._schema.TypeVerifier), // Lamda capture is more efficient.
					async (t, x, s) => ((T)await this.GetActualTypeSerializer(t).DeserializeObjectAsync(x, s).ConfigureAwait(false))!
				);

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
			=> this.GetActualTypeSerializer(Ensure.NotNull(obj).GetType()).DeserializeObjectToAsync(context, source, obj);

#endif // FEATURE_TAP
	}
}
