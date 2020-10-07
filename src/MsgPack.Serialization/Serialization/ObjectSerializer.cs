// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines basic interface for serializers which handle individual object rather than entire object tree.
	/// </summary>
	/// <remarks>
	///		This object is intended to be used by serializer implementation.
	///		Applications should use <see cref="Serializer"/> instead.
	/// </remarks>
	public abstract class ObjectSerializer
	{
		protected SerializerProvider OwnerProvider { get; }
		protected IObjectSerializerProvider ObjectSerializerProvider => this.OwnerProvider;
		public SerializerCapabilities Capabilities { get; }
		public bool CanSerialize => (this.Capabilities & SerializerCapabilities.Serialize) != 0;
		public bool CanDeserialize => (this.Capabilities & SerializerCapabilities.Deserialize) != 0;
		public bool CanDeserializeTo => (this.Capabilities & SerializerCapabilities.DeserializeTo) != 0;

		protected ObjectSerializer(SerializerProvider ownerProvider, SerializerCapabilities capabilities)
		{
			this.OwnerProvider = Ensure.NotNull(ownerProvider);
			this.Capabilities = capabilities;
		}

		public abstract void SerializeObject(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink);
		public async ValueTask SerializeObjectAsync(AsyncSerializationOperationContext context, object? obj, Stream streamSink)
		{
			await using (var writer = new StreamBufferWriter(streamSink, ownsStream: false, ArrayPool<byte>.Shared, cleansBuffer: true))
			{
				var serializationOperationContext = context.AsSerializationOperationContext();
				this.SerializeObject(ref serializationOperationContext, obj, writer);
			}
		}

		[return: MaybeNull]
		public abstract object? DeserializeObject(ref DeserializationOperationContext context, ref SequenceReader<byte> source);
		public abstract ValueTask<object?> DeserializeObjectAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source);
		public abstract bool DeserializeObjectTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj);
		public abstract ValueTask<bool> DeserializeObjectToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj);
	}
}
