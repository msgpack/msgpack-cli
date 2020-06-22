using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization;
using MsgPack.Serialization.Internal;

namespace Msgpack.Serialization.ReflectionSerializers
{
	internal abstract class SerializerBuilder
	{
		public abstract IObjectSerializer<T, TExtensionType> Build<T, TExtensionType>(SerializationTarget target);
	}

	internal sealed class ReflectionSerializerBuilder : SerializerBuilder
	{
		public sealed override IObjectSerializer<T, TExtensionType> Build<T, TExtensionType>(SerializationTarget target, SerializerGenerationOptions options)
			=> new ReflectionSerializer<T, TExtensionType>(target, options);
	}

	/// <summary>
	///		 Implements non optimized, reflection based serializer.
	/// </summary>
	/// <typeparam name="T">Type of serialization target.</typeparam>
	/// <typeparam name="TExtensionType">Type of extention type specific to codec.</typeparam>
	internal sealed class ReflectionSerializer<T, TExtensionType> : IObjectSerializer<T, TExtensionType>
	{
		private readonly SerializerGenerationOptions _options;
#warning TODO: Use _options
		private bool UsesArray => true;
		// T is PrimitiveEncoder | Stringncoder | BinaryEncoder | ExtensionEncoder | ArrayEncoderToken | DictionaryEncoderToken
		private readonly IReadOnlyList<object> _memberValueEncoders;
		private readonly IReadOnlyList<byte[]> _memberUtf8Names;
		private readonly IReadOnlyList<object> _memberValueDecoders;

		private delegate void PrimitiveEncoder(Encoder<TExtensionType> encoder, object value, IBufferWriter<byte> writer);
		private delegate void Stringncoder(Encoder<TExtensionType> encoder, object value, IBufferWriter<byte> writer, Encoding? encoding, CancellationToken cancellationToken);
		private delegate void BinaryEncoder(Encoder<TExtensionType> encoder, object ovaluebj, IBufferWriter<byte> writer, CancellationToken cancellationToken);
		private delegate void ExtensionEncoder(Encoder<TExtensionType> encoder, TExtensionType extensionType, in ReadOnlySequence<byte> body, IBufferWriter<byte> writer, CancellationToken cancellationToken);

		public void Serialize(ref SerializationOperationContext<TExtensionType> context, [AllowNull] T obj, IBufferWriter<byte> sink)
		{
			var encoder = context.Encoder;
			if (obj is null)
			{
				encoder.EncodeNull(sink);
				return;
			}

			if (this.UsesArray)
			{
				encoder.EncodeArrayStart(this._memberValueEncoders.Count, sink, context.CollectionContext);

				for(var i = 0; i < this._memberValueEncoders.Count;i++)
				{
					encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
					if (this._memberValueEncoders[i] is Action<object, IBufferWriter<byte>> primitiveEncoder)
					{
						encoder.EncodeBinary()
					}
					else if (this._memberValueEncoders[i] is Action<object, IBufferWriter<byte>, Encoding?, CancellationToken> stringEncoder)
					{

					}
					else if (this._memberValueEncoders[i] is Action<object, IBufferWriter<byte>, CancellationToken> binaryEncoder)
					{

					}
					else if (this._memberValueEncoders[i] is Action<object, ReadOnlySequence<byte>, IBufferWriter<byte>> extensionEncoder)
					{
						encoder.EncodeExtension()
					}
					else
					{
						Debug.Fail();
					}
					encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
				}

				encoder.EncodeArrayEnd(this._memberValueEncoders.Count, sink, context.CollectionContext);

			}
			


			throw new NotImplementedException();
		}

		public async ValueTask SerializeAsync(AsyncSerializationOperationContext<TExtensionType> context, [AllowNull] T obj, Stream streamSink)
		{
			await using (var writer = new StreamBufferWriter(streamSink, ownsStream: false, ArrayPool<byte>.Shared, cleansBuffer: true))
			{
				var serializationOperationContext = context.AsSerializationOperationContext();
				this.Serialize(ref serializationOperationContext, obj, writer);
			}
		}

		[return: MaybeNull]
		public T Deserialize(ref DeserializationOperationContext<TExtensionType> context, ref SequenceReader<byte> source)
		{
			throw new NotImplementedException();
		}

		public ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext<TExtensionType> context, Stream streamSource)
		{
			throw new NotImplementedException();
		}

		public bool DeserializeTo(ref DeserializationOperationContext<TExtensionType> context, ref SequenceReader<byte> source, in T obj)
		{
			throw new NotImplementedException();
		}

		public ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext<TExtensionType> context, Stream streamSource, T obj)
		{
			throw new NotImplementedException();
		}
	}
}
