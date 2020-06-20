// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		<see cref="IBufferWriter{T}"/> wrapping <see cref="StringBuilder"/>.
	/// </summary>
	/// <remarks>
	///		<see cref="Dispose"/> method returns internal char buffer to the pool, so it is important to ensure calling it.
	/// </remarks>
	internal sealed class StringBuilderBufferWriter : IBufferWriter<char>, IDisposable
	{
		private readonly ArrayPool<char> _bufferPool;
		private readonly bool _clearsArray;
		private readonly StringBuilder _stringBuilder;
		private char[] _buffer;
		private int _currentOffset;

		private int CurrentBufferSize => this._buffer.Length - this._currentOffset;

		public StringBuilderBufferWriter(StringBuilder stringBuilder, JsonDecoderOptions options)
		{
			this._stringBuilder = stringBuilder;
			this._bufferPool = options.CharBufferPool;
			this._clearsArray = options.ClearsBuffer;
#warning TODO: OPTION
			this._buffer = this._bufferPool.Rent(64/*options.MaxCharBufferLength*/);
		}

		public void Dispose()
		{
			var array = Interlocked.Exchange(ref this._buffer, null!);
			if (array != null)
			{
				this._bufferPool.Return(array, this._clearsArray);
			}
		}

		public override string ToString()
		{
			this.Flush(this._currentOffset);
			return this._stringBuilder.ToString();
		}

		private unsafe void Flush(int length)
		{
			Debug.Assert(length <= this._buffer.Length, $"length ({length}) <= this._buffer.Length ({this._buffer.Length})");
			Debug.Assert(length <= this._currentOffset, $"length ({length}) <= this._currentOffset ({this._currentOffset})");

			if (length == 0)
			{
				return;
			}

			this._stringBuilder.Append(this._buffer, 0, length);
			if (this._currentOffset > length)
			{
				// Compact
				fixed (char* source = this._buffer)
				fixed (char* destination = this._buffer)
				{
					Buffer.MemoryCopy(source + length, destination, this._buffer.Length, this._currentOffset - length);
				}
			}

			this._currentOffset = 0;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void Advance(int count)
		{
			if (this.CurrentBufferSize > count)
			{
				// Just advance offset.
				this._currentOffset += count;
				return;
			}

			this.AdvanceToNewBuffer(count);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AdvanceToNewBuffer(int count)
		{
			// We must prepare new buffer.
			var remainingCount = count - this._currentOffset;
			this.Flush(this._currentOffset);
			if (remainingCount <= 0)
			{
				return;
			}

			// Fill StringBuilder with zero.
			this._stringBuilder.Append('\0', remainingCount);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public Memory<char> GetMemory(int sizeHint = 0)
		{
			if (sizeHint <= this.CurrentBufferSize)
			{
				return this._buffer.AsMemory(this._currentOffset);
			}

			return this.GetMemorySlow(sizeHint);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Memory<char> GetMemorySlow(int sizeHint)
		{
			this.Flush(this._currentOffset);
			Debug.Assert(this._currentOffset == 0, "this._currentOffset == 0");
			if (sizeHint <= this._buffer.Length)
			{
				return this._buffer.AsMemory();
			}

			// sizeHint is too large. So realloc buffer.
			this._bufferPool.Return(this._buffer, this._clearsArray);
			this._buffer = this._bufferPool.Rent(sizeHint);
			return this._buffer.AsMemory();
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public Span<char> GetSpan(int sizeHint = 0)
			=> this.GetMemory(sizeHint).Span;

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public void AppendUtf16CodePoint(int utf16CodePoint)
			=> this.GetSpan(1)[0] = (char)utf16CodePoint;
	}
}
