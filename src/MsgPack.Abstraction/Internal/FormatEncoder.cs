// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MsgPack.Internal
{
	/// <summary>
	///		Defines an interface and basic functionarity of stateless <see cref="FormatEncoder"/>.
	/// </summary>
	/// <remarks>
	///		The <see cref="FormatEncoder"/> is stateless, so caller (serializer, reader, etc.) can cache the instance for performance.
	/// </remarks>
	public abstract partial class FormatEncoder
	{
		public FormatEncoderOptions Options { get; }

		protected FormatEncoder(FormatEncoderOptions options)
		{
			this.Options = Ensure.NotNull(options);
		}

		public abstract void EncodeNull(IBufferWriter<byte> buffer);

#if FEATURE_UTF8STRING
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void EncodeString(Utf8String? value, IBufferWriter<byte> buffer)
			=> this.EncodeString(value.AsBytes(), buffer);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void EncodeString(Utf8Span value, IBufferWriter<byte> buffer)
			=> this.EncodeString(value.AsBytes(), buffer);
#endif // FEATURE_UTF8STRING

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void EncodeString(string? value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			if (value == null)
			{
				this.EncodeNull(buffer);
				return;
			}

			this.EncodeString(value.AsSpan(), buffer, encoding, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void EncodeString(StringBuilder? value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			if (value == null)
			{
				this.EncodeNull(buffer);
				return;
			}

			this.EncodeString(value.ToSequence(), buffer, encoding, cancellationToken);
		}

		public abstract void EncodeString(ReadOnlySpan<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default);

		public abstract void EncodeString(in ReadOnlySequence<char> value, IBufferWriter<byte> buffer, Encoding? encoding = null, CancellationToken cancellationToken = default);

		public abstract void EncodeString(ReadOnlySpan<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default);

		public abstract void EncodeString(in ReadOnlySequence<byte> encodedValue, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default);

		public abstract void EncodeRawString(ReadOnlySpan<byte> rawString, int charLength, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default);

		public abstract void EncodeBinary(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default);

		public abstract void EncodeBinary(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default);

		public abstract void EncodeArrayStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeArrayEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeArrayItemStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeArrayItemEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapStart(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapEnd(int length, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapKeyStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapKeyEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapValueStart(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public abstract void EncodeMapValueEnd(int index, IBufferWriter<byte> buffer, in CollectionContext collectionContext);

		public virtual void EncodeExtension(ExtensionType typeCode, ReadOnlySpan<byte> serializedValue, IBufferWriter<byte> buffer)
			=> Throw.ExtensionsIsNotSupported();

		public virtual void EncodeExtension(ExtensionType typeCode, in ReadOnlySequence<byte> serializedValue, IBufferWriter<byte> buffer)
			=> Throw.ExtensionsIsNotSupported();

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void WriteRaw(ReadOnlySpan<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			if (value.Length <= this.Options.CancellationSupportThreshold)
			{
				buffer.Write(value);
			}
			else
			{
				WriteRawSlow(buffer, value, buffer.GetSpan(), cancellationToken);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void WriteRawSlow(IBufferWriter<byte> buffer, in ReadOnlySpan<byte> source, Span<byte> destination, CancellationToken cancellationToken)
		{
			// https://github.com/dotnet/runtime/blob/af1db3eccbc238745e1d163458c92c1bfa650fbd/src/libraries/System.Memory/src/System/Buffers/BuffersExtensions.cs#L133
			ReadOnlySpan<byte> input = source;
			while (true)
			{
				if (destination.IsEmpty)
				{
					ThrowBufferWriterReturnsEmptyBuffer(nameof(buffer));
				}

				var writeSize = Math.Min(destination.Length, input.Length);
				input.Slice(0, writeSize).CopyTo(destination);
				buffer.Advance(writeSize);
				input = input.Slice(writeSize);
				if (input.Length > 0)
				{
					destination = buffer.GetSpan();

					cancellationToken.ThrowIfCancellationRequested();
					continue;
				}

				return;
			}
		}

		private static void ThrowBufferWriterReturnsEmptyBuffer(string paramName)
			=> throw new ArgumentOutOfRangeException(paramName);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void WriteRaw(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			buffer = Ensure.NotNull(buffer);

			if (value.Length <= this.Options.MaxByteBufferLength)
			{
				var length32 = unchecked((int)value.Length);
				var span = buffer.GetSpan(length32);
				value.CopyTo(span);
				buffer.Advance(length32);
				return;
			}

			WriteRawSlow(value, buffer, cancellationToken);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void WriteRawSlow(in ReadOnlySequence<byte> value, IBufferWriter<byte> buffer, CancellationToken cancellationToken = default)
		{
			var reader = new SequenceReader<byte>(value);
			while (!reader.End)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var sink = buffer.GetSpan();
				if (sink.IsEmpty)
				{
					ThrowBufferWriterReturnsEmptyBuffer(nameof(buffer));
				}

				var source = reader.UnreadSpan.Slice(0, Math.Min(sink.Length, reader.UnreadSpan.Length));
				source.CopyTo(sink);
				buffer.Advance(source.Length);
				reader.Advance(source.Length);
			}
		}
	}
}
