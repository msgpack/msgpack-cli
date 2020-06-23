// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	public abstract class Serializer<T> : Serializer
	{
		private readonly Func<FormatEncoder> _encoderFactory;
		private readonly Func<FormatDecoder> _decoderFactory;
		private readonly ObjectSerializer<T> _underlying;
		private readonly SerializationOptions _serializationOptions;
		private readonly DeserializationOptions _deserializationOptions;

		protected Serializer(
			Func<FormatEncoder> encoderFactory,
			Func<FormatDecoder> decoderFactory,
			ObjectSerializer<T> underlying,
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

		private void InitializeSerializationOperationContext(CancellationToken cancellationToken, out SerializationOperationContext context)
			=> context = new SerializationOperationContext(this._encoderFactory(), this._serializationOptions, cancellationToken);

		private void InitializeAsyncSerializationOperationContext(CancellationToken cancellationToken, out AsyncSerializationOperationContext context)
			=> context = new AsyncSerializationOperationContext(this._encoderFactory(), this._serializationOptions, cancellationToken);

		private void InitializeDeserializationOperationContext(CancellationToken cancellationToken, out DeserializationOperationContext context)
			=> context = new DeserializationOperationContext(this._decoderFactory(), this._deserializationOptions, cancellationToken);

		private void InitializeAsyncDeserializationOperationContext(CancellationToken cancellationToken, out AsyncDeserializationOperationContext context)
			=> context = new AsyncDeserializationOperationContext(this._decoderFactory(), this._deserializationOptions, cancellationToken);

		/// <inheritdoc />
		public sealed override void SerializeObject(object? obj, IBufferWriter<byte> sink, CancellationToken cancellationToken = default)
			=> this.Serialize((T)obj, cancellationToken);

		public void Serialize(T obj, IBufferWriter<byte> sink, CancellationToken cancellationToken = default)
		{
			this.InitializeSerializationOperationContext(cancellationToken, out var context);
			this._underlying.Serialize(ref context, obj, sink);
		}

		/// <inheritdoc />
		public sealed override ReadOnlyMemory<byte> SerializeObject(object? obj, CancellationToken cancellationToken = default)
			=> this.Serialize((T)obj, cancellationToken);

		public ReadOnlyMemory<byte> Serialize([AllowNull]T obj, CancellationToken cancellationToken = default)
		{
			this.InitializeSerializationOperationContext(cancellationToken, out var context);
			var writer = new ArrayBufferWriter<byte>();
			this._underlying.Serialize(ref context, obj, writer);
			return writer.WrittenMemory;
		}

		/// <inheritdoc />
		public sealed override ValueTask SerializeObjectAsync(object? obj, Stream streamSink, CancellationToken cancellationToken = default)
			=> this.SerializeAsync((T)obj, streamSink, cancellationToken);

		public ValueTask SerializeAsync([AllowNull]T obj, Stream streamSink, CancellationToken cancellationToken = default)
		{
			this.InitializeAsyncSerializationOperationContext(cancellationToken, out var context);
			return this._underlying.SerializeAsync(context, obj, streamSink);
		}

		/// <inheritdoc />
		public sealed override object? DeserializeObject(ref SequenceReader<byte> reader, CancellationToken cancellationToken = default)
			=> this.Deserialize(ref reader, cancellationToken);

		[return:MaybeNull]
		public T Deserialize(ref SequenceReader<byte> reader, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.Deserialize(ref context, ref reader);
		}

		public sealed override object? DeserializeObject(ref ReadOnlySequence<byte> source, CancellationToken cancellationToken = default)
			=> this.Deserialize(ref source, cancellationToken);

		[return:MaybeNull]
		public T Deserialize(ref ReadOnlySequence<byte> source, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(source);
			var result = this._underlying.Deserialize(ref context, ref reader);
			source = source.Slice(source.Start, reader.Position);
			return result;
		}

		/// <inheritdoc />
		public sealed override object? DeserializeObject(ref ReadOnlyMemory<byte> memorySource, CancellationToken cancellationToken = default)
			=> this.Deserialize(ref memorySource, cancellationToken);

		[return:MaybeNull]
		public T Deserialize(ref ReadOnlyMemory<byte> memorySource, CancellationToken cancellationToken = default)
		{
			this.InitializeDeserializationOperationContext(cancellationToken, out var context);
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memorySource));
			var result = this._underlying.Deserialize(ref context, ref reader);
			memorySource = memorySource.Slice((int)reader.Consumed);
			return result;
		}

		/// <inheritdoc />
		public sealed override async ValueTask<object?> DeserializeObjectAsync(Stream streamSource, CancellationToken cancellationToken = default)
			=> await this.DeserializeAsync(streamSource, cancellationToken).ConfigureAwait(false);

		public ValueTask<T> DeserializeAsync(Stream streamSource, CancellationToken cancellationToken = default)
		{
			this.InitializeAsyncDeserializationOperationContext(cancellationToken, out var context);
			return this._underlying.DeserializeAsync(context, context.CreateSequence(streamSource));
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
			return this._underlying.DeserializeToAsync(context, context.CreateSequence(streamSource), obj);
		}
	}
}
