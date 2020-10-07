// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines basic features for non collection objects serializers.
	/// </summary>
	/// <typeparam name="T">The type of the target object.</typeparam>
	/// <remarks>
	///		This type reduces IL size by implementing <see cref="DeserializeTo"/> and its async counter part.
	/// </remarks>
	internal abstract class NonCollectionObjectSerializer<T> : ObjectSerializer<T>
	{
		protected NonCollectionObjectSerializer(SerializerProvider ownerProvider)
			: base(ownerProvider, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) { }

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(typeof(T));
			return default;
		}

#if FEATURE_TAP

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(typeof(T));
			return default;
		}

#endif // FEATURE_TAP
	}
}
