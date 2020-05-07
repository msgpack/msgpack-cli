// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace MsgPack.Serialization.Internal
{
	public interface IObjectSerializer<T>
	{
		void Serialize(in SerializationOperationContext context, T obj, IBufferWriter<byte> sink);
		ValueTask SerializeAsync(SerializationOperationContext context, T obj, Stream streamSink);
		T Deserialize(in DeserializationOperationContext context, in SequenceReader<byte> source);
		ValueTask<T> DeserializeAsync(DeserializationOperationContext context, Stream streamSource);
		void DeserializeTo(in DeserializationOperationContext context, in SequenceReader<byte> source, in T obj);
		ValueTask DeserializeToAsync(DeserializationOperationContext context, Stream streamSource, T obj);
	}
}
