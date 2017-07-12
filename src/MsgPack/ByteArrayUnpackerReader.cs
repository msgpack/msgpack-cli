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
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
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
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		// TODO: Use Span<byte> and keep ArraySegment<byte>[] as _originalByteArraySegments for GetRemainingBytes();

		// TODO: Use Span<byte>
		private byte[] _source;
		private int _offset;

		public override long Offset
		{
			get { return this._offset; }
		}

#if DEBUG
		internal byte[] DebugSource
		{
			get { return this._source; }
		}

#endif // DEBUG

		public override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._offset;
			return false;
		}

		// TODO: Use Span<byte>

		// TODO: Use Span<byte>
		public ByteArrayUnpackerReader( byte[] source, int startOffset )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( source.Length == 0 )
			{
				throw new ArgumentException( "The source is empty.", "source" );
			}

			if ( startOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startOffset" );
			}

			if ( startOffset >= source.Length )
			{
				throw new ArgumentException( "The startOffset is too large or the length of source is too small." );
			}

			this._source = source;
			this._offset = startOffset;
		}

		// TODO: Use Span<byte>
		public bool TryRead( byte[] buffer, int requestedSize )
		{
			if ( requestedSize == 0 )
			{
				return true;
			}

			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < requestedSize )
			{
				return false;
			}
			
			// TODO: Use currentSource.CopyTo( buffer, requestedSize );
			Buffer.BlockCopy( source, offset, buffer, 0, requestedSize );
			this._offset += requestedSize;
			return true;
		}

		// TODO: Use Span<T>
		public override void Read( byte[] buffer, int size )
		{
			if ( !this.TryRead( buffer, size ) )
			{
				this.ThrowEofException( size );
			}
		}

#if FEATURE_TAP

		public override Task ReadAsync( byte[] buffer, int size, CancellationToken cancellationToken )
		{
			this.Read( buffer, size );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP

		public override string ReadString( int length )
		{
			if ( length == 0 )
			{
				return String.Empty;
			}

			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < length )
			{
				this.ThrowEofException( length );
			}

			var result = Encoding.UTF8.GetString( source, offset, length );
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
			if ( this._source.Length - this._offset < size )
			{
				return false;
			}

			if ( this._offset + size > Int32.MaxValue )
			{
				return false;
			}

			this._offset += unchecked( ( int ) size );
			return true;
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
					"Data source unexpectedly ends. Cannot read {0:#,0} bytes at offset {1:#,0}.",
					reading,
					this._offset
				)
			);
		}

		private void ThrowBadUtf8Exception()
		{
			throw new InvalidMessagePackStreamException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data source has invalid UTF-8 sequence. Last code point at offset {1:#,0}.",
					this._offset
				)
			);
		}
	}
}
