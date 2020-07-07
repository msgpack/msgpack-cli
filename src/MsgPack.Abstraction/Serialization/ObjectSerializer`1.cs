// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	public abstract class ObjectSerializer<T> : ObjectSerializer
	{
		private static readonly bool CanBeNull = !typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null;

		protected ObjectSerializer(ObjectSerializationContext ownerContext, SerializerCapabilities capabilities)
			: base(ownerContext, capabilities) { }

		public abstract void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink);

		public sealed override void SerializeObject(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink)
			=> this.Serialize(ref context, Cast(obj), sink);

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

		public sealed override object? DeserializeObject(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> this.Deserialize(ref context, ref source);

		public abstract ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source);

		public sealed override async ValueTask<object?> DeserializeObjectAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
			=> await this.DeserializeAsync(context, source).ConfigureAwait(false);

		public abstract bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj);

		public sealed override bool DeserializeObjectTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
			=> this.DeserializeTo(ref context, ref source, Cast(obj)!);

		public abstract ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj);

		public sealed override async ValueTask<bool> DeserializeObjectToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
			=> await this.DeserializeObjectToAsync(context, source, Cast(obj)).ConfigureAwait(false);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		[return: MaybeNull]
		[return: NotNullIfNotNull("obj")]
		private static T Cast(object? obj)
		{
			if (!(obj is null))
			{
				return (T)obj;
			}
			else if (CanBeNull)
			{
				return (T)obj;
			}
			else
			{
				Throw.CannotBeNull(typeof(T));
				// never
				return default;
			}
		}
	}
}
