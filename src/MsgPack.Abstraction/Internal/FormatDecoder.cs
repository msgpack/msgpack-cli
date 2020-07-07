// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MsgPack.Internal
{
#warning TODO: Use 'Try' prefix for published APIs which can return 'requestHint'.

	/// <summary>
	///		Defines an interface and basic functionarity of stateless <see cref="FormatDecoder"/>.
	/// </summary>
	/// <remarks>
	///		The <see cref="FormatDecoder"/> is stateless, so caller (serializer, writer, etc.) can cache the instance for performance.
	/// </remarks>
	public abstract partial class FormatDecoder
	{
		public FormatDecoderOptions Options { get; }

		protected FormatDecoder(FormatDecoderOptions options)
		{
			this.Options = Ensure.NotNull(options);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void Skip(ref SequenceReader<byte> source, in CollectionContext collectionContext, CancellationToken cancellationToken = default)
		{
			this.Skip(ref source, collectionContext, out var requestHint, cancellationToken);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForSkip(source.Consumed, requestHint);
			}
		}

		public abstract void Skip(ref SequenceReader<byte> source, in CollectionContext collectionContext, out int requestHint, CancellationToken cancellationToken = default);

		public abstract bool DecodeItem(ref SequenceReader<byte> source, out DecodeItemResult result, CancellationToken cancellationToken = default);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void GetRawString(ref SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, CancellationToken cancellationToken = default)
		{
			if (!this.GetRawString(ref source, out rawString, out var requestHint, cancellationToken))
			{
				Throw.InsufficientInputForRawString(source.Consumed, requestHint);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool TryDecodeNull(ref SequenceReader<byte> source)
		{
			if (this.TryDecodeNull(ref source, out var requestHint))
			{
				return true;
			}

			if (requestHint != 0)
			{
				Throw.InsufficientInputForNull(source.Consumed, requestHint);
			}

			return false;
		}

		public abstract bool TryDecodeNull(ref SequenceReader<byte> source, out int requestHint);


		public abstract bool GetRawString(ref SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, CancellationToken cancellationToken = default);

		/// <summary>
		///		Decodes current data as array or map header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <param name="itemsCount">Items count if known; <c>-1</c> if underlying format does not contain any count information; <c>0</c> if underlying format is not an array nor a map.</param>
		/// <returns>
		///		<see cref="ElementType.Array"/> for array, <see cref="ElementType.Map"/> for map (dictionary).
		///		This method does not return anything else, but may throw an exception.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not an array nor a map.</exception>
		///	<exception cref="InsufficientInputException"><paramref name="source"/> does not contain enough bytes to decode.</exception>
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public CollectionType DecodeArrayOrMapHeader(ref SequenceReader<byte> source, out int itemsCount)
		{
			var result = this.DecodeArrayOrMapHeader(ref source, out itemsCount, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayOrMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		/// <summary>
		///		Decodes current data as array or map header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <param name="itemsCount">Items count if known; <c>-1</c> if underlying format does not contain any count information; <c>0</c> if underlying format is not an array nor a map.</param>
		/// <param name="requestHint">
		///		<c>0</c> if this method succeeds to decode value; Positive integer when <paramref name="source" /> does not contain enough bytes to decode, and required memory bytes hint is stored.
		///		Note that <c>-1</c> represents unknown size. If so, caller must supply new buffer with most efficient size.
		///	</param>
		/// <returns>
		///		<see cref="ElementType.Array"/> for array, <see cref="ElementType.Map"/> for map (dictionary), or <see cref="ElementType.None"/> if there were not enough bytes to decode.
		///		This method does not return anything else, but may throw an exception.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not an array nor a map.</exception>
		public abstract CollectionType DecodeArrayOrMapHeader(ref SequenceReader<byte> source, out int itemsCount, out int requestHint);

		/// <summary>
		///		Decodes current data as array header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <returns>
		///		Items count if known; <c>-1</c> if underlying format does not contain any count information.
		///		Note that <c>0</c> is valid value when the array is empty.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not an array.</exception>
		///	<exception cref="InsufficientInputException"><paramref name="source"/> does not contain enough bytes to decode.</exception>
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public int DecodeArrayHeader(ref SequenceReader<byte> source)
		{
			var result = this.DecodeArrayHeader(ref source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayHeader(source.Consumed, requestHint);
			}

			return result;
		}

		/// <summary>
		///		Decodes current data as array header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <param name="requestHint">
		///		<c>0</c> if this method succeeds to decode value; Positive integer when <paramref name="source" /> does not contain enough bytes to decode, and required memory bytes hint is stored.
		///		Note that <c>-1</c> represents unknown size. If so, caller must supply new buffer with most efficient size.
		///	</param>
		/// <returns>
		///		Items count if known; <c>-1</c> if underlying format does not contain any count information.
		///		Note that <c>0</c> is valid value when the array is empty.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not an array.</exception>
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public abstract int DecodeArrayHeader(ref SequenceReader<byte> source, out int requestHint);

		/// <summary>
		///		Decodes current data as map header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <returns>
		///		Items count if known; <c>-1</c> if underlying format does not contain any count information.
		///		Note that <c>0</c> is valid value when the map is empty.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not a map.</exception>
		///	<exception cref="InsufficientInputException"><paramref name="source"/> does not contain enough bytes to decode.</exception>
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public int DecodeMapHeader(ref SequenceReader<byte> source)
		{
			var result = this.DecodeMapHeader(ref source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		/// <summary>
		///		Decodes current data as map header, and returns the items count if known.
		/// </summary>
		/// <param name="source">Reader of source byte sequence. If and only if this method succeeds, the reader will be advanced.</param>
		/// <param name="requestHint">
		///		<c>0</c> if this method succeeds to decode value; Positive integer when <paramref name="source" /> does not contain enough bytes to decode, and required memory bytes hint is stored.
		///		Note that <c>-1</c> represents unknown size. If so, caller must supply new buffer with most efficient size.
		///	</param>
		/// <returns>
		///		Items count if known; <c>-1</c> if underlying format does not contain any count information.
		///		Note that <c>0</c> is valid value when the map is empty.
		///	</returns>
		///	<exception cref="MessageTypeException">The decoded value is not a map.</exception>
		public abstract int DecodeMapHeader(ref SequenceReader<byte> source, out int requestHint);

		public virtual void DecodeExtension(ref SequenceReader<byte> source, out ExtensionTypeObject result, out int requestHint, CancellationToken cancellationToken = default)
		{
			Throw.ExtensionsIsNotSupported();
			// never
			result = default;
			requestHint = -1;
		}

		public CollectionType DecodeArrayOrMap(ref SequenceReader<byte> source, out CollectionItemIterator iterator)
		{
			var result = this.DecodeArrayOrMap(ref source, out iterator, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayOrMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionType DecodeArrayOrMap(ref SequenceReader<byte> source, out CollectionItemIterator iterator, out int requestHint);

		public CollectionItemIterator DecodeArray(ref SequenceReader<byte> source)
		{
			var result = this.DecodeArray(ref source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionItemIterator DecodeArray(ref SequenceReader<byte> source, out int requestHint);

		public CollectionItemIterator DecodeMap(ref SequenceReader<byte> source)
		{
			var result = this.DecodeMap(ref source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionItemIterator DecodeMap(ref SequenceReader<byte> source, out int requestHint);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void Drain(ref SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, CancellationToken cancellationToken = default)
		{
			if (itemsCount <= 0)
			{
				return;
			}

			this.Drain(ref source, collectionContext, itemsCount, out var requestHint, cancellationToken);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeMapHeader(source.Consumed, requestHint);
			}
		}

		public abstract void Drain(ref SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, out int requestHint, CancellationToken cancellationToken = default);
	}
}
