// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MsgPack.Internal
{
	public partial class MessagePackDecoder
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public override bool GetRawString(in SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, CancellationToken cancellationToken = default)
		{
			var length = this.PeekRawStringLength(source, out requestHint, cancellationToken);
			if (length < 0)
			{
				rawString = default;
				return false;
			}

			if (source.UnreadSpan.Length < length)
			{
				rawString = source.UnreadSpan.Slice(0, length);
				source.Advance(length);
				requestHint = 0;
				return true;
			}

			return this.GetRawStringMultiSegment(source, out rawString, out requestHint, length, cancellationToken);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool GetRawStringMultiSegment(in SequenceReader<byte> source, out ReadOnlySpan<byte> rawString, out int requestHint, int length, CancellationToken cancellationToken)
		{
			if (source.Remaining < length)
			{
				requestHint = length - (int)source.Remaining;
				rawString = default;
				return false;
			}

			var result = new byte[length];
			var bufferSpan = result.AsSpan();

			var copyLength = Math.Min(this.Options.CancellationSupportThreshold, length);
			while (bufferSpan.Length > 0)
			{
				var destination = bufferSpan.Slice(0, copyLength);
				var shouldTrue = source.TryCopyTo(destination);
				Debug.Assert(shouldTrue, "SequenceReader<byte>.Remaining lied.");

				bufferSpan = bufferSpan.Slice(copyLength);
				source.Advance(copyLength);

				cancellationToken.ThrowIfCancellationRequested();
			}

			rawString = result;
			requestHint = 0;
			return true;
		}

		private int PeekRawStringLength(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken)
		{
			var length = this.DecodeStringHeader(source, out var header, out requestHint, out _);
			if (requestHint != 0)
			{
				return default;
			}

			if (length > Int32.MaxValue)
			{
				MessagePackThrow.TooLargeByteLength(header, source.Consumed, length);
			}

			return (int)length;
		}

		private long DecodeStringHeader(in SequenceReader<byte> source, out byte header, out int requestHint, out int consumed)
		{
			if (!this.TryPeek(source, out header))
			{
				requestHint = 1;
				consumed = 0;
				return 0;
			}

			if (header >= MessagePackCode.MinimumFixedRaw && header <= MessagePackCode.MaximumFixedRaw)
			{
				requestHint = 0;
				consumed = 1;
				return header - MessagePackCode.MinimumFixedRaw;
			}

			long length;
			switch (header)
			{
				case MessagePackCode.Str8:
				{
					length = ReadByte(source, offset: 1, out requestHint);
					consumed = 2;
					break;
				}
				case MessagePackCode.Str16:
				{
					length = ReadValue<ushort>(source, offset: 1, out requestHint);
					consumed = 3;
					break;
				}
				case MessagePackCode.Str32:
				{
					length = ReadValue<uint>(source, offset: 1, out requestHint);
					consumed = 5;
					break;
				}
				default:
				{
					MessagePackThrow.IsNotUtf8String(header, source.Consumed);
					// never
					requestHint = default;
					consumed = default;
					return 0;
				}
			}

			return requestHint == 0 ? length : 0;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override string? DecodeNullableString(in SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			if (this.TryReadNull(source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeString(source, out requestHint, encoding, cancellationToken);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override string? DecodeString(in SequenceReader<byte> source, out int requestHint, Encoding? encoding = null, CancellationToken cancellationToken = default)
		{
			var length = this.DecodeStringHeader(source, out _, out requestHint, out var consumed);
			if (requestHint != 0)
			{
				return default;
			}

			if (source.Remaining < length + consumed)
			{
				requestHint = (int)((length + consumed - source.Remaining) & Int32.MaxValue);
				return default;
			}

			if (length > this.Options.MaxBinaryLengthInBytes)
			{
				Throw.StringLengthExceeded(source.Consumed - consumed, length, this.Options.MaxBinaryLengthInBytes);
			}

			if (encoding == null && length <= this.Options.CancellationSupportThreshold)
			{
				// fast-path
				var value = Utf8EncodingNonBom.Instance.GetString(source.UnreadSpan.Slice(consumed, (int)length));
				source.Advance(length + consumed);
				requestHint = 0;
				return value;
			}

			var result = (encoding ?? Utf8EncodingNonBom.Instance).GetStringMultiSegment(source.Sequence.Slice(consumed), this.Options.CharBufferPool, cancellationToken);
			source.Advance(length + consumed);
			requestHint = 0;
			return result;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override byte[]? DecodeNullableBinary(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			if (this.TryReadNull(source))
			{
				requestHint = 0;
				return null;
			}

			return this.DecodeBinary(source, out requestHint, cancellationToken);
		}

		private int DecodeBinaryHeader(in SequenceReader<byte> source, out int requestHint, out int consumed)
		{
			if (!this.TryPeek(source, out var header))
			{
				requestHint = 1;
				consumed = 0;
				return 0;
			}

			if (header >= MessagePackCode.MinimumFixedRaw && header <= MessagePackCode.MaximumFixedRaw)
			{
				requestHint = 0;
				consumed = 1;
				return header - MessagePackCode.MinimumFixedRaw;
			}

			int length;
			switch (header)
			{
				case MessagePackCode.Str8:
				case MessagePackCode.Bin8:
				{
					length = ReadByte(source, offset: 1, out requestHint);
					consumed = 2;
					break;
				}
				case MessagePackCode.Str16:
				case MessagePackCode.Bin16:
				{
					length = ReadValue<ushort>(source, offset: 1, out requestHint);
					consumed = 3;
					break;
				}
				case MessagePackCode.Str32:
				case MessagePackCode.Bin32:
				{
					length = ReadValue<int>(source, offset: 1, out requestHint);
					if (length < 0)
					{
						MessagePackThrow.TooLargeByteLength(header, source.Consumed - 5, unchecked((uint)length));
					}

					consumed = 5;
					break;
				}
				default:
				{
					MessagePackThrow.IsNotUtf8String(header, source.Consumed);
					// never
					requestHint = default;
					consumed = default;
					return 0;
				}
			}

			return requestHint == 0 ? length : 0;
		}


		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public sealed override byte[]? DecodeBinary(in SequenceReader<byte> source, out int requestHint, CancellationToken cancellationToken = default)
		{
			var length = this.DecodeBinaryHeader(source, out requestHint, out var consumed);
			if(requestHint != 0)
			{
				return default;
			}

			if (length > this.Options.MaxBinaryLengthInBytes)
			{
				Throw.BinaryLengthExceeded(source.Consumed - consumed, length, this.Options.MaxBinaryLengthInBytes);
			}

			if(source.Remaining < length + consumed)
			{
				requestHint = length + consumed - (int)source.Remaining;
				return default;
			}

			// This line may throw OutOfMemoryException, but we cannot determine the OOM is caused by heap exhausion or excess of the implementation specific max length of arrays.
			// So, we just throws OOM for such conditions.
			var result = new byte[length];
			var shouldBeTrue = source.TryCopyTo(result);
			Debug.Assert(shouldBeTrue, "SequenceReader<byte>.Remaining lied.");
			source.Advance(length);
			return result;
		}
	}
}
