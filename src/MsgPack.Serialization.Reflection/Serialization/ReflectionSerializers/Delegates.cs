// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal delegate void Serialization(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink);
	internal delegate object? Deserialization(ref DeserializationOperationContext context, ref SequenceReader<byte> source);
	internal delegate ValueTask<object?> AsyncDeserialization(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source);
	internal delegate bool DeserializingFill(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj);
	internal delegate ValueTask<bool> AsyncDeserializingFill(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj);

	internal delegate object? PrimitiveDecoder(FormatDecoder decoder, ref SequenceReader<byte> source);
	internal delegate object? StringDecoder(FormatDecoder decoder, ref SequenceReader<byte> source, Encoding? encoding, CancellationToken cancellationToken);
	internal delegate object? BinaryDecoder(FormatDecoder decoder, ref SequenceReader<byte> source, CancellationToken cancellationToken);
	internal delegate ValueTask<object?> AsyncPrimitiveDecoder(FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken);
	internal delegate ValueTask<object?> AsyncStringDecoder(FormatDecoder decoder, ReadOnlyStreamSequence source, Encoding? encoding, CancellationToken cancellationToken);
	internal delegate ValueTask<object?> AsyncBinaryDecoder(FormatDecoder decoder, ReadOnlyStreamSequence source, CancellationToken cancellationToken);
}
