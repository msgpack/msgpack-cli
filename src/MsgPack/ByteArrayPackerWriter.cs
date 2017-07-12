#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="PackerWriter"/> for byte array.
	/// </summary>
	internal sealed partial class ByteArrayPackerWriter : PackerWriter
	{
		// TODO: Use Span<byte>;
		private readonly IList<ArraySegment<byte>> _buffers;

		private readonly ByteBufferAllocator _allocator;

		private readonly int _initialBufferIndex;
		private int _currentBufferIndex;

		// TODO: Use Span<byte>
		private byte[] _currentBuffer;
		private int _currentBufferOffset;
		private int _currentBufferRemains;

#if DEBUG

		internal ArraySegment<byte> DebugBuffer
		{
			get { return this._buffers[ this._currentBufferIndex ]; }
		}

		internal IList<ArraySegment<byte>> DebugBuffers
		{
			get { return this.GetBufferAsByteArray(); }
		}

#endif // DEBUG

		public long BytesUsed
		{
			get
			{
				return
					this._buffers.Skip( this._initialBufferIndex ).Take( this._currentBufferIndex - this._initialBufferIndex ).Sum( x => x.Count )
					+ this._currentBufferOffset - this._buffers[ this._currentBufferIndex ].Offset;
			}
		}

		public int InitialBufferIndex
		{
			get { return this._initialBufferIndex; }
		}

		public int CurrentBufferIndex
		{
			get { return this._currentBufferIndex; }
		}

		public int CurrentBufferOffset
		{
			get { return this._currentBufferOffset; }
		}

		// TODO: Use Span<byte>
		public ByteArrayPackerWriter( ArraySegment<byte> buffer, ByteBufferAllocator allocator )
		{
			if ( buffer.Array == null || buffer.Count == 0 )
			{
				throw new ArgumentException( "Buffer must have non null, non-empty Array.", "buffer" );
			}

			this._buffers = new[] { buffer };
			this._currentBufferIndex = 0;
			this._currentBuffer = buffer.Array;
			this._currentBufferOffset = buffer.Offset;
			this._currentBufferRemains = buffer.Count;
			this._allocator = allocator;
		}

		// TODO: Use Span<byte>
		public ByteArrayPackerWriter( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, ByteBufferAllocator allocator )
		{
			if ( buffers == null )
			{
				throw new ArgumentNullException( "buffers" );
			}

			if ( startIndex < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startIndex" );
			}

			if ( startOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startOffset" );
			}

			if ( buffers.Count == 0 )
			{
				throw new ArgumentException( "Buffers cannot be empty.", "sources" );
			}

			if ( buffers.Any( x => x.Array == null || x.Count == 0 ) )
			{
				throw new ArgumentException( "Buffers contains null or empty Array.", "sources" );
			}

			if ( buffers.Count <= startIndex )
			{
				throw new ArgumentException( "Buffers is too small." );
			}

			this._buffers = buffers;
			this._initialBufferIndex = startIndex;
			this._currentBufferIndex = startIndex;
			var startBuffer = this._buffers[ startIndex ];
			var skip = startOffset - startBuffer.Offset;
			if ( skip < 0 )
			{
				throw new ArgumentException( "The value cannot be smaller than the array segment Offset.", "startOffset" );
			}

			if ( skip > startBuffer.Count )
			{
				throw new ArgumentException( "The offset cannot exceed the array segment Count.", "startOffset" );
			}

			this._currentBuffer = startBuffer.Array;
			this._currentBufferOffset = startBuffer.Offset + skip;
			this._currentBufferRemains = startBuffer.Count - skip;
			this._allocator = allocator;
		}

		public IList<ArraySegment<byte>> GetBufferAsByteArray()
		{
			return this._buffers;
		}

		private bool ShiftBufferIfNeeded( int sizeHint, int minimumSize, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains, ref int currentBufferIndex )
		{
			// Check current buffer is empty and whether more buffer is required.
			if ( currentBufferRemains < minimumSize && sizeHint > 0 )
			{
				// try shift to next buffer
				currentBufferIndex++;
				ArraySegment<byte> newBuffer;
				if ( this._buffers.Count == currentBufferIndex )
				{
					if ( !this._allocator.TryAllocate( this._buffers, sizeHint, minimumSize, ref currentBufferIndex, out newBuffer ) )
					{
						return false;
					}
				}
				else
				{
					newBuffer = this._buffers[ currentBufferIndex ];
				}

#warning TODO: IList<ArraySegment<byte>> の使い方を再考する必要あり。今だと抜かせない。
				currentBuffer = newBuffer.Array;
				currentBufferOffset = newBuffer.Offset;
				currentBufferRemains = newBuffer.Count;
			}

			return true;
		}

		public override void WriteByte( byte value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( byte ), sizeof( byte ), ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( 1 );
			}

			currentBuffer[ currentBufferOffset ] = value;
			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset + 1;
			this._currentBufferRemains = currentBufferRemains - 1;
		}

		private void WriteBytes( byte[] value, int startIndex, int count )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( count, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( count );
			}

			var written = 0;
			do
			{
				var remains = count - written;
				var writes = Math.Min( currentBufferRemains, remains );
				Buffer.BlockCopy( value, startIndex + written, currentBuffer, currentBufferOffset, writes );

				written += writes;
				currentBufferOffset += writes;
				currentBufferRemains -= writes;

				if ( !this.ShiftBufferIfNeeded( remains, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( count );
				}
			} while ( written < count );

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		public override void WriteBytes( byte[] value )
		{
			this.WriteBytes( value, 0, value.Length );
		}

#if FEATURE_TAP

		public override Task WriteByteAsync( byte value, CancellationToken cancellationToken )
		{
			this.WriteByte( value );
			return TaskAugument.CompletedTask;
		}

		public override Task WriteBytesAsync( byte[] value, CancellationToken cancellationToken )
		{
			this.WriteBytes( value );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP

		private void ThrowEofException( int requiredSize )
		{
			throw new InvalidOperationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data buffer unexpectedly ends. Cannot write {0:#,0} bytes at offset {1:#,0}, buffer index {2}.",
					requiredSize,
					this._currentBufferOffset,
					this._currentBufferIndex
				)
			);
		}

		private void ThrowEofExceptionForString( int requiredCharCount )
		{
			throw new InvalidOperationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data buffer unexpectedly ends. Cannot write {0:#,0} UTF-16 chars in UTF-8 encoding at offset {1:#,0}, buffer index {2}.",
					requiredCharCount,
					this._currentBufferOffset,
					this._currentBufferIndex
				)
			);
		}
	}
}
