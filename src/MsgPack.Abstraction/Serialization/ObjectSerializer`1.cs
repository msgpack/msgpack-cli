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
	public abstract class ObjectSerializer<T>
	{
		protected ObjectSerializationContext OwnerContext { get; }
		public SerializerCapabilities Capabilities { get; }
		public bool CanSerialize => (this.Capabilities & SerializerCapabilities.Serialize) != 0;
		public bool CanDeserialize => (this.Capabilities & SerializerCapabilities.Deserialize) != 0;
		public bool CanDeserializeTo => (this.Capabilities & SerializerCapabilities.DeserializeTo) != 0;

		protected ObjectSerializer(ObjectSerializationContext ownerContext, SerializerCapabilities capabilities)
		{
			this.OwnerContext = Ensure.NotNull(ownerContext);
			this.Capabilities = capabilities;
		}

		public abstract void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink);
		public async ValueTask SerializeAsync(AsyncSerializationOperationContext context, [AllowNull] T obj, Stream streamSink)
		{
			await using (var writer = new StreamBufferWriter(streamSink, ownsStream: false, ArrayPool<byte>.Shared, cleansBuffer: true))
			{
				var serializationOperationContext = context.AsSerializationOperationContext();
				this.Serialize(ref serializationOperationContext, obj, writer);
			}
		}

		[return: MaybeNull]
		public abstract T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source);
		public abstract ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source);
		public abstract bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, in T obj);
		public abstract ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj);
	}
}
