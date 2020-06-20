// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

namespace MsgPack.Serialization
{
	public abstract class Serializer<T, TExtentionType>
	{
		private readonly Func<Encoder<TExtentionType>> _encoderFactory;
		private readonly Func<Decoder<TExtentionType>> _decoderFactory;
		private readonly IObjectSerializer<T, TExtentionType> _underlying;
		private readonly SerializationOptions _serializationOptions;
		private readonly DeserializationOptions _deserializationOptions;

		protected Serializer(
			Func<Encoder<TExtentionType>> encoderFactory,
			Func<Decoder<TExtentionType>> decoderFactory,
			IObjectSerializer<T, TExtentionType> underlying,
			SerializationOptions serializationOptions,
			DeserializationOptions deserializationOptions
		)
		{
			this._encoderFactory = Ensure.NotNull(encoderFactory);
			this._decoderFactory = Ensure.NotNull(decoderFactory);
			this._underlying = Ensure.NotNull(underlying);
			this._serializationOptions = Ensure.NotNull(serializationOptions);
			this._deserializationOptions = Ensure.NotNull(deserializationOptions);
		}

		private void InitializeSerializationOperationContext(CancellationToken cancellationToken, out SerializationOperationContext<TExtentionType> context)
			=> context = new SerializationOperationContext<TExtentionType>(this._encoderFactory(), this._serializationOptions, cancellationToken);

		private void InitializeAsyncSerializationOperationContext(CancellationToken cancellationToken, out AsyncSerializationOperationContext<TExtentionType> context)
			=> context = new AsyncSerializationOperationContext<TExtentionType>(this._encoderFactory(), this._serializationOptions, cancellationToken);

		private void InitializeDeserializationOperationContext(CancellationToken cancellationToken, out DeserializationOperationContext<TExtentionType> context)
			=> context = new DeserializationOperationContext<TExtentionType>(this._decoderFactory(), this._deserializationOptions, cancellationToken);

		private void InitializeAsyncDeserializationOperationContext(CancellationToken cancellationToken, out AsyncDeserializationOperationContext<TExtentionType> context)
			=> context = new AsyncDeserializationOperationContext<TExtentionType>(this._decoderFactory(), this._deserializationOptions, cancellationToken);

		public void Serialize(T obj, IBufferWriter<byte> sink, CancellationToken cancellationToken = default)
		{
			this.InitializeSerializationOperationContext(cancellationToken, out var context);
			this._underlying.Serialize(ref context, obj, sink);
		}

		public ReadOnlyMemory<byte> Serialize(T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeSerializationOperationContext(cancellationToken, out var context);
			var writer = new ArrayBufferWriter<byte>();
			this._underlying.Serialize(ref context, obj, writer);
			return writer.WrittenMemory;
		}

		public ValueTask SerializeAsync(T obj, Stream streamSink, CancellationToken cancellationToken = default)
		{
			this.InitializeAsyncSerializationOperationContext(cancellationToken, out var context);
			return this._underlying.SerializeAsync(context, obj, streamSink);
		}

		public T Deserialize(ref SequenceReader<byte> reader, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.Deserialize(ref context, ref reader);
		}

		public T Deserialize(ref ReadOnlySequence<byte> source, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(source);
			var result = this._underlying.Deserialize(ref context, ref reader);
			source = source.Slice(source.Start, reader.Position);
			return result;
		}

		public T Deserialize(ref ReadOnlyMemory<byte> memorySource, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memorySource));
			var result = this._underlying.Deserialize(ref context, ref reader);
			memorySource = memorySource.Slice((int)reader.Consumed);
			return result;
		}

		public ValueTask<T> DeserializeAsync(Stream streamSource, CancellationToken cancellationToken = default)
		{
			this.InitializeAsyncDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.DeserializeAsync(context, streamSource);
		}

		public bool DeserializeTo(ref SequenceReader<byte> reader, T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.DeserializeTo(ref context, ref reader, obj);
		}

		public bool DeserializeTo(ref ReadOnlySequence<byte> source, T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(source);
			var result = this._underlying.DeserializeTo(ref context, ref reader, obj);
			source = source.Slice(source.Start, reader.Position);
			return result;
		}

		public bool DeserializeTo(ref ReadOnlyMemory<byte> memorySource, T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memorySource));
			var result = this._underlying.DeserializeTo(ref context, ref reader, obj);
			memorySource = memorySource.Slice((int)reader.Consumed);
			return result;
		}

		public ValueTask<bool> DeserializeToAsync(Stream streamSource, T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeAsyncDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.DeserializeToAsync(context, streamSource, obj);
		}
	}
}
