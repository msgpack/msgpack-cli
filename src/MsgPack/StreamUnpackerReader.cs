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
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.IO;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="UnpackerReader"/> for <see cref="Stream"/>.
	/// </summary>
	internal sealed partial class StreamUnpackerReader : UnpackerReader
	{
		private readonly byte[] _oneByteBuffer = new byte[ 1 ];
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private readonly Stream _source;
		private readonly bool _useStreamPosition;
		private readonly bool _ownsStream;

#if DEBUG
		internal Stream DebugSource
		{
			get { return this._source; }
		}

		internal bool DebugOwnsStream
		{
			get { return this._ownsStream; }
		}
#endif // DEBUG

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance.
		/// </summary>
		private long _offset;

		public override long Offset
		{
			get { return this._offset; }
		}


		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance before last operation.
		/// </summary>
		private long _lastOffset;

		public override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._lastOffset;
			return this._useStreamPosition;
		}

		public StreamUnpackerReader( Stream stream, PackerUnpackerStreamOptions streamOptions )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			var options = streamOptions ?? PackerUnpackerStreamOptions.None;
			this._source = options.WrapStream( stream );
			this._ownsStream = options.OwnsStream;
			this._useStreamPosition = stream.CanSeek;
			this._offset = this._useStreamPosition ? stream.Position : 0L;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( this._ownsStream )
				{
					this._source.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		// TODO: Use Span<T>
		public override void Read( byte[] buffer, int size )
		{
#if DEBUG
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG

			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return;
			}

			this._lastOffset = this._offset;
			var remaining = size;
			var offset = 0;
			int read;

			do
			{
				read = this._source.Read( buffer, offset, remaining );
				remaining -= read;
				offset += read;
			} while ( read > 0 && remaining > 0 );

			this._offset += offset;
#if DEBUG
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG

			if ( offset < size )
			{
				this.ThrowEofException( size );
			}
		}

#if FEATURE_TAP

		public override async Task ReadAsync( byte[] buffer, int size, CancellationToken cancellationToken )
		{
#if DEBUG
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return;
			}

			this._lastOffset = this._offset;
			var remaining = size;
			var offset = 0;
			int read;

			do
			{
				read = await this._source.ReadAsync( buffer, offset, remaining, cancellationToken ).ConfigureAwait( false );
				remaining -= read;
				offset += read;
			} while ( read > 0 && remaining > 0 );

			this._offset += offset;
#if DEBUG
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG

			if ( offset < size )
			{
				this.ThrowEofException( size );
			}
		}

#endif // FEATURE_TAP

		private int DecodeString( byte[] bytes, Decoder decoder, char[] chars, StringBuilder stringBuffer, int remaining, int reading, int bytesRead )
		{
			this._offset += bytesRead;
			if ( bytesRead == 0 )
			{
				this.ThrowEofException( reading );
			}

			remaining -= bytesRead;

			var isCompleted = false;
			var bytesOffset = 0;

			while ( !isCompleted )
			{
				int bytesUsed;
				int charsUsed;
				decoder.Convert(
					bytes,
					bytesOffset,
					bytesRead - bytesOffset,
					chars,
					0,
					chars.Length,
					( bytesRead == 0 ),
					// flush when last read.
					out bytesUsed,
					out charsUsed,
					out isCompleted
				);

				stringBuffer.Append( chars, 0, charsUsed );
				bytesOffset += bytesUsed;
			}

			return remaining;
		}

		public override string ReadString( int length )
		{
			var bytes = BufferManager.NewByteBuffer( length );

			if ( length <= bytes.Length )
			{
				this.Read( bytes, length );
				return Encoding.UTF8.GetString( bytes, 0, length );
			}

			var decoder = Encoding.UTF8.GetDecoder();
			var chars = BufferManager.NewCharBuffer( bytes.Length );
			var stringBuffer = new StringBuilder( length );
			var remaining = length;
			do
			{
				var reading = Math.Min( remaining, bytes.Length );
				this._lastOffset = this._offset;
				var bytesRead = this._source.Read( bytes, 0, reading );
				remaining = this.DecodeString( bytes, decoder, chars, stringBuffer, remaining, reading, bytesRead );
			} while ( remaining > 0 );

			return stringBuffer.ToString();
		}

#if FEATURE_TAP

		public override async Task<string> ReadStringAsync( int length, CancellationToken cancellationToken )
		{
			var bytes = BufferManager.NewByteBuffer( length );

			if ( length <= bytes.Length )
			{
				await this.ReadAsync( bytes, length, cancellationToken ).ConfigureAwait( false );
				return Encoding.UTF8.GetString( bytes, 0, length );
			}

			var decoder = Encoding.UTF8.GetDecoder();
			var chars = BufferManager.NewCharBuffer( bytes.Length );
			var stringBuffer = new StringBuilder( length );
			var remaining = length;
			do
			{
				var reading = Math.Min( remaining, bytes.Length );
				this._lastOffset = this._offset;
				var bytesRead = await this._source.ReadAsync( bytes, 0, reading, cancellationToken ).ConfigureAwait( false );
				remaining = this.DecodeString( bytes, decoder, chars, stringBuffer, remaining, reading, bytesRead );
			} while ( remaining > 0 );

			return stringBuffer.ToString();
		}

#endif // FEATURE_TAP

		public override bool Drain( uint size )
		{
			if ( this._useStreamPosition )
			{
				var remaining = this._source.Length - this._source.Position;
				if ( remaining >= size )
				{
					this._source.Position += size;
					this._offset += size;
					return true;
				}
				else
				{
					return false;
				}
			}

			long bytesRead = 0;
			var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked(( int )Math.Min( size, Int32.MaxValue )) );
			while ( size > bytesRead )
			{
				var remaining = ( 4 - bytesRead );
				var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked(( int )remaining);
				this._lastOffset = this._offset;
				var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
				this._offset += lastRead;
				bytesRead += lastRead;
				if ( lastRead == 0 )
				{
					return false;
				}
			}

			return true;
		}

#if FEATURE_TAP

		public override async Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			if ( this._useStreamPosition )
			{
				var remaining = this._source.Length - this._source.Position;
				if ( remaining >= size )
				{
					this._source.Position += size;
					this._offset += size;
					return true;
				}
				else
				{
					return false;
				}
			}

			long bytesRead = 0;
			var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked(( int )Math.Min( size, Int32.MaxValue )) );
			while ( size > bytesRead )
			{
				var remaining = ( 4 - bytesRead );
				var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked(( int )remaining);
				this._lastOffset = this._offset;
				var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
				this._offset += lastRead;
				bytesRead += lastRead;
				if ( lastRead == 0 )
				{
					return false;
				}
			}

			return true;
		}

#endif // FEATURE_TAP

		private void ThrowEofException( long reading )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at position {1:#,0}."
							: "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at offset {1:#,0}.",
						reading,
						offsetOrPosition
					)
				);
		}
	}
}
