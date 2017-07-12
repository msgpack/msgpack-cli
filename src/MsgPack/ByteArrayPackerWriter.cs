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
		private readonly int _initialOffset;

		private readonly ByteBufferAllocator _allocator;

		// TODO: Use Span<byte>
		private byte[] _buffer;
		private int _offset;

		public byte[] Buffer
		{
			get { return this._buffer; }
		}

		public int BytesUsed
		{
			get { return this._offset - this._initialOffset; }
		}

		public int InitialOffset
		{
			get { return this._initialOffset; }
		}

		// TODO: Use Span<byte>
		public ByteArrayPackerWriter( byte[] buffer, int startOffset, ByteBufferAllocator allocator )
		{
			this._buffer = buffer ?? Binary.Empty;
			if ( startOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "startOffset" );
			}

			if ( startOffset > this._buffer.Length )
			{
				throw new ArgumentException( "The startOffset is too large or the length of buffer is too small." );
			}

			this._initialOffset = startOffset;
			this._offset = startOffset;
			this._allocator = allocator;
		}

		public override void WriteByte( byte value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < 1 && !this._allocator.TryAllocate( buffer, 1, out buffer ) )
			{
				this.ThrowEofException( 1 );
			}

			buffer[ offset ] = value;
			this._buffer = buffer;
			this._offset = offset + 1;
		}

		private void WriteBytes( byte[] value, int startIndex, int count )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < count && !this._allocator.TryAllocate( buffer, count, out buffer ) )
			{
				this.ThrowEofException( count );
			}

			System.Buffer.BlockCopy( value, startIndex, buffer, offset, count );

			this._buffer = buffer;
			this._offset += count;
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
					"Data buffer unexpectedly ends. Cannot write {0:#,0} bytes at offset {1:#,0}.",
					requiredSize,
					this._offset
				)
			);
		}

		private void ThrowEofExceptionForString( int requiredCharCount )
		{
			throw new InvalidOperationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data buffer unexpectedly ends. Cannot write {0:#,0} UTF-16 chars in UTF-8 encoding at offset {1:#,0}.",
					requiredCharCount,
					this._offset
				)
			);
		}
	}
}
