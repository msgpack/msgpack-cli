// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace MsgPack.Serialization.Internal
{
	public interface IObjectSerializer<T, TExtentionType>
	{
		void Serialize(ref SerializationOperationContext<TExtentionType> context, T obj, IBufferWriter<byte> sink);
		ValueTask SerializeAsync(AsyncSerializationOperationContext<TExtentionType> context, T obj, Stream streamSink);
		T Deserialize(ref DeserializationOperationContext<TExtentionType> context, ref SequenceReader<byte> source);
		ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext<TExtentionType> context, Stream streamSource);
		bool DeserializeTo(ref DeserializationOperationContext<TExtentionType> context, ref SequenceReader<byte> source, in T obj);
		ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext<TExtentionType> context, Stream streamSource, T obj);
	}
}
