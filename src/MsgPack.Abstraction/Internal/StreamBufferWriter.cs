// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace MsgPack.Internal
{
#warning TODO: Pubternal or CodeGen
	public sealed class StreamBufferWriter : IBufferWriter<byte>, IAsyncDisposable
	{
		private readonly Stream _underlying;
		private readonly bool _ownsStream;

		private readonly ArrayPool<byte> _arrayPool;
		private readonly bool _cleansBuffer;

		private byte[] _buffer = null!;
		private int _offset;

		private ValueTask _flushTask;
		private byte[]? _flushingBuffer;

		public int Remaining => this._buffer.Length - this._offset;

		public StreamBufferWriter(
			Stream underlying,
			bool ownsStream,
			ArrayPool<byte> arrayPool,
			bool cleansBuffer
		)
		{
			this._underlying = underlying;
			this._ownsStream = ownsStream;
			this._arrayPool = arrayPool;
			this._cleansBuffer = cleansBuffer;
		}

		public async ValueTask DisposeAsync()
		{
			await this._flushTask.ConfigureAwait(false);
			this.CleanUpAsyncWork();
			await this.FlushToStreamAsync().ConfigureAwait(false);
			await this._underlying.FlushAsync().ConfigureAwait(false);
			this._arrayPool.Return(this._buffer, this._cleansBuffer);
			this._buffer = null!;
			this._offset = 0;
			if (this._ownsStream)
			{
				await this._underlying.DisposeAsync();
			}
		}

		public void Advance(int count)
			=> this._offset++;

		public Memory<byte> GetMemory(int sizeHint = 0)
		{
			this.EnsureBuffer(sizeHint);
			return this._buffer.AsMemory(this._offset);
		}

		public Span<byte> GetSpan(int sizeHint = 0)
		{
			this.EnsureBuffer(sizeHint);
			return this._buffer.AsSpan(this._offset);
		}

		private void EnsureBuffer(int sizeHint)
		{
			if (this._buffer == null)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			if (this.Remaining > 0 && this.Remaining >= sizeHint)
			{
				return;
			}

			if (!this._flushTask.IsCompletedSuccessfully)
			{
				this._flushTask.ConfigureAwait(false).GetAwaiter().GetResult();
				this.CleanUpAsyncWork();
			}

			this._flushTask = this.FlushToStreamAsync();
			if (!this._flushTask.IsCompletedSuccessfully)
			{
				this._flushingBuffer = this._buffer;
				this._buffer = this._arrayPool.Rent(this._buffer.Length);
			}

			this._offset = 0;
		}

		private ValueTask FlushToStreamAsync()
			=> this._underlying.WriteAsync(this._buffer.AsMemory(0, this._offset));

		private void CleanUpAsyncWork()
		{
			if (this._flushingBuffer != null)
			{
				this._arrayPool.Return(this._flushingBuffer!, this._cleansBuffer);
				this._flushingBuffer = null;
				this._flushTask = default;
			}
		}
	}
}
