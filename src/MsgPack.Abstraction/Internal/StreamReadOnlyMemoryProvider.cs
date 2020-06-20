// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Internal
{
#warning TODO: PubTernal or CodeGen
	public sealed class StreamReadOnlyMemoryProvider
	{
		private readonly Stream _stream;
		private readonly byte[] _buffer;

		public StreamReadOnlyMemoryProvider(Stream stream, byte[] buffer)
		{
			this._stream = stream;
			this._buffer = buffer;
		}

		public async ValueTask<ReadOnlyMemory<byte>> GetNextAsync(ReadOnlyMemory<byte> previous, int requestHint, CancellationToken cancellationToken)
		{
			var required = (long)previous.Length + requestHint;
			if (required > UInt32.MaxValue)
			{
#warning TODO:
				throw new Exception("Too large");
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
