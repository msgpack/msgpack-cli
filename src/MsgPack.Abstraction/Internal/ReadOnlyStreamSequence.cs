// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Internal
{
	/// <summary>
	///		Wraps <see cref="Stream"/> and provides <see cref="ReadOnlySequence{T}"/> via internal buffer.
	/// </summary>
	/// <remarks>
	///		User of this object must be call <see cref="Dispose()"/> when this sequence used.
	///		Note that the <see cref="Stream"/> will NOT be closed when <see cref="Dispose()"/> is called.
	/// </remarks>
	public sealed class ReadOnlyStreamSequence : IDisposable
	{
		private readonly ArrayPool<byte> _arrayPool;
		private readonly bool _clearsBuffer;
		private readonly Stream _stream; // DO NOT Dispose this in this class.
		private byte[] _array;

		/// <summary>
		///		Gets internal buffer as <see cref="ReadOnlyMemory{Byte}"/> which holds last fetched data.
		/// </summary>
		public ReadOnlyMemory<byte> Memory { get; private set; }

		/// <summary>
		///		Gets internal buffer as <see cref="ReadOnlySequence{Byte}"/> which holds last fetched data.
		/// </summary>
		public ReadOnlySequence<byte> Sequence => new ReadOnlySequence<byte>(this.Memory);

		internal ReadOnlyStreamSequence(Stream stream, ArrayPool<byte> arrayPool, int bufferSize, bool clearsBuffer)
		{
			this._stream = stream;
			this._arrayPool = arrayPool;
			this._array = arrayPool.Rent(bufferSize);
			this.Memory = this._array;
			this._clearsBuffer = clearsBuffer;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			var array = Interlocked.Exchange(ref this._array, null!);
			if (array != null)
			{
				this._arrayPool.Return(this._array, this._clearsBuffer);
			}
		}

		/// <summary>
		///		Fetches first data from underlying <see cref="Stream"/> and fills internal buffer.
		///		Remaining data in the current buffer will be included new buffer.
		/// </summary>
		/// <param name="cancellationToken"><see cref="CancellationToken"/> to cancel read operation.</param>
		/// <returns>Async operation state.</returns>
		/// <exception cref="ObjectDisposedException">This object is already disposed.</exception>
		public ValueTask FetchAsync(CancellationToken cancellationToken)
			=> this.FetchAsync(0, cancellationToken);

		/// <summary>
		///		Fetches next data from underlying <see cref="Stream"/> and fills internal buffer.
		///		Remaining data in the current buffer will be included new buffer.
		/// </summary>
		/// <param name="requestHint">
		///		Hint size to fetch from <see cref="Stream"/>.
		///		<c>0</c> is valid value which means that the implementation fetches bytes from <see cref="Stream"/> to fill out current buffer.
		///		Note that it is not guaranteed that <see cref="Memory"/> contains this length because underlying <see cref="Stream"/> may not have enough data.
		///	</param>
		/// <param name="cancellationToken"><see cref="CancellationToken"/> to cancel read operation.</param>
		/// <returns>Async operation state.</returns>
		/// <exception cref="ObjectDisposedException">This object is already disposed.</exception>
		public async ValueTask FetchAsync(int requestHint, CancellationToken cancellationToken)
		{
			if (this._array == null)
			{
				Throw.ObjectDisposed(this.ToString());
				// never
				return;
			}

			var existingLength = this.Memory.Length;
			if (existingLength + requestHint > this._array.Length)
			{
				// realloc
				var newArray = this._arrayPool.Rent(Math.Max(this._array.Length * 2, existingLength + requestHint));
				this.Memory.CopyTo(newArray);
				this._arrayPool.Return(this._array, this._clearsBuffer);
				this._array = newArray;
			}
			else
			{
				// comact
				this.Memory.CopyTo(this._array);
			}

			var readLength = await this._stream.ReadAsync(this._array.AsMemory(existingLength), cancellationToken).ConfigureAwait(false);
			this.Memory = this._array.AsMemory(existingLength + readLength);
		}

		/// <summary>
		///		Advances internal buffer position.
		/// </summary>
		/// <param name="length">Length to be advanced.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is greater than length of <see cref="Memory"/>.</exception>
		/// <exception cref="ObjectDisposedException">This object is already disposed.</exception>
		public void Advance(long length)
		{
			if (this._array == null)
			{
				Throw.ObjectDisposed(this.ToString());
			}

			this.Memory = this.Memory.Slice((int)Ensure.IsNotGreaterThan(length, (long)this.Memory.Length));
		}
	}
}
