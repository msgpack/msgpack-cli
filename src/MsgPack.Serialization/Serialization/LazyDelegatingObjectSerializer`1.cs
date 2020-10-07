// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Lazily initialized serializer which delegates actual work for another serializer implementation.
	/// </summary>
	/// <typeparam name="T">
	///		The type of target type.
	/// </typeparam>
	/// <remarks>
	///		This serializer is intended to support self-composit structure like directories or XML nodes.
	/// </remarks>
	internal sealed class LazyDelegatingObjectSerializer<T> : ObjectSerializer<T>
	{
		private readonly object _providerParameter;
		private ObjectSerializer<T>? _delegated;

		public ObjectSerializer? Delegated => this._delegated;

		/// <summary>
		///		Initializes a new instance of the <see cref="LazyDelegatingObjectSerializer{T}"/> object.
		/// </summary>
		/// <param name="ownerContext">
		///		The serialization context to support lazy retrieval.
		///	</param>
		/// <param name="providerParameter">A provider parameter to be passed in future.</param>
		public LazyDelegatingObjectSerializer(SerializerProvider ownerProvider, object providerParameter, SerializerCapabilities capabilities)
			: base(ownerProvider, capabilities)
		{
			this._providerParameter = providerParameter;
		}

		private ObjectSerializer<T> GetDelegatedSerializer()
		{
			var result = this._delegated;
			if (result == null)
			{
				var untypedResult = this.ObjectSerializerProvider.GetSerializer(typeof(T), this._providerParameter);

				if (untypedResult is LazyDelegatingObjectSerializer<T>)
				{
					throw new InvalidOperationException(
						$"ObjectSerializer for the type '{typeof(T)}' is not constructed yet. It may indicate race condition or may be caused by cyclic reference."
					);
				}

				// Duplicated assignment is accepttable.
				this._delegated = result = (ObjectSerializer<T>)untypedResult;
			}

			return result;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink)
			=> this.GetDelegatedSerializer().Serialize(ref context, obj, sink);

		[return: MaybeNull]
		public sealed override T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> this.GetDelegatedSerializer().Deserialize(ref context, ref source);

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
			=> this.GetDelegatedSerializer().DeserializeTo(ref context, ref source, obj);

#if FEATURE_TAP

		public sealed override ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
			=> this.GetDelegatedSerializer().DeserializeAsync(context, source);

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
			=> this.GetDelegatedSerializer().DeserializeToAsync(context, source, obj);

#endif // FEATURE_TAP

		public sealed override string? ToString()
			=> this.GetDelegatedSerializer().ToString();
	}
}
