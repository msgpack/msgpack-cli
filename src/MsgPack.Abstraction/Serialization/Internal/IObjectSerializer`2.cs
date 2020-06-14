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
		void Serialize(in SerializationOperationContext<TExtentionType> context, T obj, IBufferWriter<byte> sink);
		ValueTask SerializeAsync(SerializationOperationContext<TExtentionType> context, T obj, Stream streamSink);
		T Deserialize(in DeserializationOperationContext<TExtentionType> context, in SequenceReader<byte> source);
		ValueTask<T> DeserializeAsync(DeserializationOperationContext<TExtentionType> context, Stream streamSource);
		void DeserializeTo(in DeserializationOperationContext<TExtentionType> context, in SequenceReader<byte> source, in T obj);
		ValueTask DeserializeToAsync(DeserializationOperationContext<TExtentionType> context, Stream streamSource, T obj);
	}
}
