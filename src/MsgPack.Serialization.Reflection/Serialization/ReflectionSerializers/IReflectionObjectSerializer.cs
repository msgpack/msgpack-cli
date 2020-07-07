// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal interface IReflectionObjectSerializer
	{
		void Serialize(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink);
		object? Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source);
		bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj);
		ValueTask<object?> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source);
		ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj);
	}
}
