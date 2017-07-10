
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
using System.IO;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from StreamUnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude T4Template.
	// Do not modify this file. Edit StreamUnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude instead.

	partial class StreamUnpackerReader
	{

		public override Byte ReadByte()
		{
			var read = this.TryReadByte();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			return unchecked( ( byte )( read ) );
		}

		public override int TryReadByte()
		{
			var read = this._source.Read( this._scalarBuffer, 0, sizeof( byte ) );
			this._offset += read;
			if ( read > 0 )
			{
				return this._scalarBuffer[ 0 ];
			}
		
			return -1;
		}

		public override SByte ReadSByte()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( sbyte ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( sbyte ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( sbyte ) )
			{
				return BigEndianBinary.ToSByte( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( sbyte ) );
				// never reaches
				return default( sbyte );
			}
		}

		public override Int16 ReadInt16()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( short ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( short ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( short ) )
			{
				return BigEndianBinary.ToInt16( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( short ) );
				// never reaches
				return default( short );
			}
		}

		public override UInt16 ReadUInt16()
		{
			var read = this.TryReadUInt16();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			return unchecked( ( ushort )( read ) );
		}

		public override int TryReadUInt16()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( ushort ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( ushort ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( ushort ) )
			{
				return BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
		}

		public override Int32 ReadInt32()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( int ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( int ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( int ) )
			{
				return BigEndianBinary.ToInt32( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( int ) );
				// never reaches
				return default( int );
			}
		}

		public override UInt32 ReadUInt32()
		{
			var read = this.TryReadUInt32();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			return unchecked( ( uint )( read ) );
		}

		public override long TryReadUInt32()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( uint ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( uint ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( uint ) )
			{
				return BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
		}

		public override Int64 ReadInt64()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( long ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( long ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( long ) )
			{
				return BigEndianBinary.ToInt64( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( long ) );
				// never reaches
				return default( long );
			}
		}

		public override UInt64 ReadUInt64()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( ulong ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( ulong ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( ulong ) )
			{
				return BigEndianBinary.ToUInt64( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( ulong ) );
				// never reaches
				return default( ulong );
			}
		}

		public override Single ReadSingle()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( float ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( float ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( float ) )
			{
				return BigEndianBinary.ToSingle( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( float ) );
				// never reaches
				return default( float );
			}
		}

		public override Double ReadDouble()
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( double ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( double ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( double ) )
			{
				return BigEndianBinary.ToDouble( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( double ) );
				// never reaches
				return default( double );
			}
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

		public override string ReadString( int length )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( length == 0 )
			{
				return String.Empty;
			}

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
			bool isCompleted;
			do
			{
				var reading = Math.Min( remaining, bytes.Length );
				this._lastOffset = this._offset;
				var bytesRead = this._source.Read( bytes, 0, reading );
				this._offset += bytesRead;
				if ( bytesRead == 0 )
				{
					this.ThrowEofException( reading );
				}

				isCompleted = decoder.DecodeString( bytes, 0, bytesRead, chars, stringBuffer );
				remaining -= bytesRead;
			} while ( remaining > 0 );

			if ( !isCompleted )
			{
				this.ThrowBadUtf8Exception();
			}

			return stringBuffer.ToString();
		}

		public override bool Drain( uint size )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return true;
			}

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
			else
			{
				var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked(( int )Math.Min( size, Int32.MaxValue )) );
				long remaining = size;
				while ( remaining > 0 )
				{
					var reading = unchecked( ( int )Math.Min( remaining, dummyBufferForSkipping.Length ) );
					this._lastOffset = this._offset;
					var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
					this._offset += lastRead;
					remaining -= lastRead;
					if ( lastRead == 0 )
					{
						return false;
					}
				}

				return true;
			}
		}

#if FEATURE_TAP

		public override async Task<Byte> ReadByteAsync( CancellationToken cancellationToken )
		{
			var read = await this.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			return unchecked( ( byte )( read ) );
		}

		public override async Task<int> TryReadByteAsync( CancellationToken cancellationToken )
		{
			var read = await this._source.ReadAsync( this._scalarBuffer, 0, sizeof( byte ), cancellationToken ).ConfigureAwait( false );
			this._offset += read;
			if ( read > 0 )
			{
				return this._scalarBuffer[ 0 ];
			}
		
			return -1;
		}

		public override async Task<SByte> ReadSByteAsync( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( sbyte ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( sbyte ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( sbyte ) )
			{
				return BigEndianBinary.ToSByte( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( sbyte ) );
				// never reaches
				return default( sbyte );
			}
		}

		public override async Task<Int16> ReadInt16Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( short ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( short ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( short ) )
			{
				return BigEndianBinary.ToInt16( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( short ) );
				// never reaches
				return default( short );
			}
		}

		public override async Task<UInt16> ReadUInt16Async( CancellationToken cancellationToken )
		{
			var read = await this.TryReadUInt16Async( cancellationToken ).ConfigureAwait( false );
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			return unchecked( ( ushort )( read ) );
		}

		public override async Task<int> TryReadUInt16Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( ushort ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( ushort ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( ushort ) )
			{
				return BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
		}

		public override async Task<Int32> ReadInt32Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( int ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( int ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( int ) )
			{
				return BigEndianBinary.ToInt32( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( int ) );
				// never reaches
				return default( int );
			}
		}

		public override async Task<UInt32> ReadUInt32Async( CancellationToken cancellationToken )
		{
			var read = await this.TryReadUInt32Async( cancellationToken ).ConfigureAwait( false );
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			return unchecked( ( uint )( read ) );
		}

		public override async Task<long> TryReadUInt32Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( uint ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( uint ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( uint ) )
			{
				return BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
		}

		public override async Task<Int64> ReadInt64Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( long ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( long ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( long ) )
			{
				return BigEndianBinary.ToInt64( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( long ) );
				// never reaches
				return default( long );
			}
		}

		public override async Task<UInt64> ReadUInt64Async( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( ulong ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( ulong ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( ulong ) )
			{
				return BigEndianBinary.ToUInt64( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( ulong ) );
				// never reaches
				return default( ulong );
			}
		}

		public override async Task<Single> ReadSingleAsync( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( float ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( float ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( float ) )
			{
				return BigEndianBinary.ToSingle( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( float ) );
				// never reaches
				return default( float );
			}
		}

		public override async Task<Double> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( double ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( double ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( double ) )
			{
				return BigEndianBinary.ToDouble( this._scalarBuffer, 0 );
			}
			else
			{
				this.ThrowEofException( sizeof( double ) );
				// never reaches
				return default( double );
			}
		}

		// TODO: Use Span<T>
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

		public override async Task<string> ReadStringAsync( int length, CancellationToken cancellationToken )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( length == 0 )
			{
				return String.Empty;
			}

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
			bool isCompleted;
			do
			{
				var reading = Math.Min( remaining, bytes.Length );
				this._lastOffset = this._offset;
				var bytesRead = await this._source.ReadAsync( bytes, 0, reading, cancellationToken ).ConfigureAwait( false );
				this._offset += bytesRead;
				if ( bytesRead == 0 )
				{
					this.ThrowEofException( reading );
				}

				isCompleted = decoder.DecodeString( bytes, 0, bytesRead, chars, stringBuffer );
				remaining -= bytesRead;
			} while ( remaining > 0 );

			if ( !isCompleted )
			{
				this.ThrowBadUtf8Exception();
			}

			return stringBuffer.ToString();
		}

		public override async Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return true;
			}

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
			else
			{
				var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked(( int )Math.Min( size, Int32.MaxValue )) );
				long remaining = size;
				while ( remaining > 0 )
				{
					var reading = unchecked( ( int )Math.Min( remaining, dummyBufferForSkipping.Length ) );
					this._lastOffset = this._offset;
					var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
					this._offset += lastRead;
					remaining -= lastRead;
					if ( lastRead == 0 )
					{
						return false;
					}
				}

				return true;
			}
		}

#endif // FEATURE_TAP
	}
}
