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

using System;
using System.Globalization;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="UnpackerReader"/> for <see cref="ArraySegment{T}"/> or array of bytes.
	/// </summary>
	internal sealed partial class ByteArrayUnpackerReader : UnpackerReader
	{
		private readonly ArraySegment<byte> _source;

#if DEBUG

		internal ArraySegment<byte> DebugSource
		{
			get { return this._source; }
		}

#endif // DEBUG

		private int _offset;

		public override long Offset
		{
			get { return this._offset; }
		}

		public override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._offset;
			return false;
		}

		public ByteArrayUnpackerReader( ArraySegment<byte> source )
		{
			if ( source.Array == null )
			{
				throw new ArgumentException( "Source must have non null Array.", "source" );
			}

			this._source = source;
		}

		private void ReadStrictCore( byte[] buffer, int size )
		{
			var remaining = this._source.Count - this.Offset;
			if ( remaining < size )
			{
				this.ThrowEofException( size );
			}

			Buffer.BlockCopy( this._source.Array, this._source.Offset + this._offset, buffer, 0, size );
			this._offset += size;
		}

		// TODO: Use Span<T>
		public override void Read( byte[] buffer, int size )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return;
			}

			this.ReadStrictCore( buffer, size );
		}

#if FEATURE_TAP

		public override Task ReadAsync( byte[] buffer, int size, CancellationToken cancellationToken )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return TaskAugument.CompletedTask;
			}

			this.ReadStrictCore( buffer, size );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP

		public override string ReadString( int length )
		{
			if ( this._source.Count - this._offset < length )
			{
				this.ThrowEofException( length );
			}

			var result = Encoding.UTF8.GetString( this._source.Array, this._source.Offset + this._offset, length );
			this._offset += length;
			return result;
		}

#if FEATURE_TAP

		public override Task<string> ReadStringAsync( int length, CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadString( length ) );
		}

#endif // FEATURE_TAP

		public override bool Drain( uint size )
		{
			if ( size > Int32.MaxValue )
			{
				return false;
			}

			if ( this._source.Count - this._offset >= size )
			{
				this._offset += unchecked( ( int )size );
				return true;
			}

			return false;
		}

#if FEATURE_TAP

		public override Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			return Task.FromResult( this.Drain( size ) );
		}

#endif // FEATURE_TAP

		private void ThrowEofException( long reading )
		{
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Data source unexpectedly ends. Cannot read {0:#,0} bytes from byte array segment at offset {1:#,0}.",
						reading,
						this._offset
					)
				);
		}
	}
}
