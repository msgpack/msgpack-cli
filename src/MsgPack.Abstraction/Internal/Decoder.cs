// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MsgPack.Internal
{
	public abstract partial class Decoder
#warning TODO: Use 'Try' prefix for published APIs which can return 'requestHint'.

	{
		public FormatFeatures FormatFeatures { get; }

		public DecoderOptions Options { get; }

		protected Decoder(DecoderOptions options, FormatFeatures formatFeatures)
		{
			this.Options = Ensure.NotNull(options);
			this.FormatFeatures = Ensure.NotNull(formatFeatures);
		}

		public void Skip(in SequenceReader<byte> source, in CollectionContext collectionContext, CancellationToken cancellationToken = default)
		{
			this.Skip(source, collectionContext, out var requestHint, cancellationToken);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForSkip(source.Consumed, requestHint);
			}
		}

		public abstract void Skip(in SequenceReader<byte> source, in CollectionContext collectionContext, out int requestHint, CancellationToken cancellationToken = default);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public bool TryPeek(in SequenceReader<byte> source, out byte value) => source.TryRead(out value);

		public abstract ElementType DecodeItem(in SequenceReader<byte> source, out ReadOnlySequence<byte> valueOrLength, out int requestHint, CancellationToken cancellationToken = default);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void GetRawString(in SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, CancellationToken cancellationToken = default)
		{
			if (!this.GetRawString(source, out rawString, out var requestHint, cancellationToken))
			{
				Throw.InsufficientInputForRawString(source.Consumed, requestHint);
			}
		}

		public abstract bool GetRawString(in SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, CancellationToken cancellationToken = default);

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
		public CollectionType DecodeArrayOrMapHeader(in SequenceReader<byte> source, out long itemsCount)
		{
			var result = this.DecodeArrayOrMapHeader(source, out itemsCount, out var requestHint);
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
		public abstract CollectionType DecodeArrayOrMapHeader(in SequenceReader<byte> source, out long itemsCount, out int requestHint);

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
		public long DecodeArrayHeader(in SequenceReader<byte> source)
		{
			var result = this.DecodeArrayHeader(source, out var requestHint);
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
		public abstract long DecodeArrayHeader(in SequenceReader<byte> source, out int requestHint);

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
		public long DecodeMapHeader(in SequenceReader<byte> source)
		{
			var result = this.DecodeMapHeader(source, out var requestHint);
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
		public abstract long DecodeMapHeader(in SequenceReader<byte> source, out int requestHint);

		public virtual void DecodeExtension(in SequenceReader<byte> source, out byte typeCode, out ReadOnlySequence<byte> body, out int requestHint, CancellationToken cancellationToken = default)
		{
			Throw.ExtensionsIsNotSupported();
			// never
			body = default;
			requestHint = -1;
			typeCode = default;
		}

		public CollectionType DecodeArrayOrMap(in SequenceReader<byte> source, out CollectionItemIterator iterator)
		{
			var result = this.DecodeArrayOrMap(source, out iterator, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayOrMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionType DecodeArrayOrMap(in SequenceReader<byte> source, out CollectionItemIterator iterator, out int requestHint);

		public CollectionItemIterator DecodeArray(in SequenceReader<byte> source)
		{
			var result = this.DecodeArray(source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeArrayHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionItemIterator DecodeArray(in SequenceReader<byte> source, out int requestHint);

		public CollectionItemIterator DecodeMap(in SequenceReader<byte> source)
		{
			var result = this.DecodeMap(source, out var requestHint);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeMapHeader(source.Consumed, requestHint);
			}

			return result;
		}

		public abstract CollectionItemIterator DecodeMap(in SequenceReader<byte> source, out int requestHint);

		public void Drain(in SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, CancellationToken cancellationToken = default)
		{
			this.Drain(source, collectionContext, itemsCount, out var requestHint, cancellationToken);
			if (requestHint != 0)
			{
				Throw.InsufficientInputForDecodeMapHeader(source.Consumed, requestHint);
			}
		}

		public abstract void Drain(in SequenceReader<byte> source, in CollectionContext collectionContext, long itemsCount, out int requestHint, CancellationToken cancellationToken = default);
	}
}
