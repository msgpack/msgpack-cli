// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Serialization
{
	public abstract class Serializer
	{
		public abstract SerializerCapabilities Capabilities { get; }

		public abstract void SerializeObject(object? obj, IBufferWriter<byte> sink, CancellationToken cancellationToken = default);

		public abstract ReadOnlyMemory<byte> SerializeObject(object? obj, CancellationToken cancellationToken = default);

		public abstract ValueTask SerializeObjectAsync(object? obj, Stream streamSink, CancellationToken cancellationToken = default);

		public abstract object? DeserializeObject(ref SequenceReader<byte> reader, CancellationToken cancellationToken = default);

		public abstract object? DeserializeObject(ref ReadOnlySequence<byte> source, CancellationToken cancellationToken = default);

		public abstract object? DeserializeObject(ref ReadOnlyMemory<byte> memorySource, CancellationToken cancellationToken = default);

		public abstract ValueTask<object?> DeserializeObjectAsync(Stream streamSource, CancellationToken cancellationToken = default);

		public abstract bool DeserializeToObject(ref SequenceReader<byte> reader, object destination, CancellationToken cancellationToken = default);

		public abstract bool DeserializeToObject(ref ReadOnlySequence<byte> source, object destination, CancellationToken cancellationToken = default);

		public abstract bool DeserializeToObject(ref ReadOnlyMemory<byte> memorySource, object destination, CancellationToken cancellationToken = default);

		public abstract ValueTask<bool> DeserializeToObjectAsync(Stream streamSource, object destination, CancellationToken cancellationToken = default);
	}
}
