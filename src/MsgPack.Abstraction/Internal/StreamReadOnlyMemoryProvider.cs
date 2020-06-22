// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Internal
{
	public struct StreamReadOnlyMemoryProvider
	{
		private readonly Stream _stream;
		private readonly byte[] _buffer;

		public StreamReadOnlyMemoryProvider(Stream stream, byte[] buffer)
		{
			Ensure.NotNull(stream);
			if (!stream.CanRead)
			{
				Throw.StreamMustBeAbleToRead(nameof(stream));
			}

			Ensure.NotNull(buffer);
			if (buffer.Length == 0)
			{
				Throw.TooSmallBuffer(nameof(buffer), 1);
			}

			this._stream = stream;
			this._buffer = buffer;
		}

		public async ValueTask<ReadOnlyMemory<byte>> GetNextAsync(ReadOnlyMemory<byte> previous, int requestHint, CancellationToken cancellationToken)
		{
			if (this._stream is null)
			{
				Throw.EmptyObject(typeof(StreamReadOnlyMemoryProvider));
				// never
				return default;
			}

			var required = (long)previous.Length + requestHint;
			if (required > OptionsDefaults.MaxSingleByteCollectionLength)
			{
				Throw.TooLargeLength(previous.Length, requestHint);
			}

			Memory<byte> buffer;
			if (required < this._buffer.Length)
			{
				if (previous.Length > 0)
				{
					this._buffer.AsSpan().CopyTo(this._buffer.AsSpan(previous.Length));
				}

				buffer = this._buffer.AsMemory(previous.Length);
				if (requestHint > 0)
				{
					buffer = buffer.Slice(0, requestHint);
				}
			}
			else
			{
				buffer = new byte[required];
				previous.CopyTo(buffer);
				buffer = buffer.Slice(previous.Length);
			}

			await this._stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
			return buffer;
		}
	}
}
