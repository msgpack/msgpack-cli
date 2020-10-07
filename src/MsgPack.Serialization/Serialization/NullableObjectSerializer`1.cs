// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements nullable value type handling.
	/// </summary>
	/// <typeparam name="T">The type of the target nullable value type.</typeparam>
	internal sealed class NullableObjectSerializer<T> : NonCollectionObjectSerializer<T?>
		where T : struct
	{
		private readonly ObjectSerializer<T> _underlying;

		/// <summary>
		///		Initialized a new instance of the <see cref="NullableObjectSerializer{T}"/> object.
		/// </summary>
		/// <param name="ownerProvider">The provider which will own this object.</param>
		/// <param name="underlying">The serializer which will handle non-null case.</param>
		public NullableObjectSerializer(
			SerializerProvider ownerProvider,
			ObjectSerializer<T> underlying
		) : base(ownerProvider)
		{
			this._underlying = underlying;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T? obj, IBufferWriter<byte> sink)
		{
			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			this._underlying.Serialize(ref context, obj.GetValueOrDefault(), sink);
		}

		[return: MaybeNull]
		public sealed override T? Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			if (context.Decoder.TryDecodeNull(ref source))
			{
				return null;
			}

			return this._underlying.Deserialize(ref context, ref source);
		}

#if FEATURE_TAP

		public sealed override async ValueTask<T?> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			bool wasNull;
			while(!TryDecodeNull(context, source, out wasNull, out var requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken);
			}

			if (wasNull)
			{
				return null;
			}

			return await this._underlying.DeserializeAsync(context, source);
		}

		private static bool TryDecodeNull(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out bool result, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);
			result = context.Decoder.TryDecodeNull(ref reader, out requestHint);
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);
			return true;
		}

#endif // FEATURE_TAP
	}
}
