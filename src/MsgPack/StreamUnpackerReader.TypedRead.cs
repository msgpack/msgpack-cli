
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
using System.IO;
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
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = this._source.Read( this._scalarBuffer, totalRead, sizeof( byte ) - totalRead );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( byte ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( byte ) )
			{
				return BigEndianBinary.ToByte( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
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
			this._lastOffset = this._offset;
			var totalRead = 0;
			var read = 0;
			// Retry for splitted stream like NetworkStream
			do
			{
				read = await this._source.ReadAsync( this._scalarBuffer, totalRead, sizeof( byte ) - totalRead, cancellationToken ).ConfigureAwait( false );
				totalRead += read;
			} while ( read > 0 && totalRead < sizeof( byte ) );

			this._offset += totalRead;
			
			if ( totalRead == sizeof( byte ) )
			{
				return BigEndianBinary.ToByte( this._scalarBuffer, 0 );
			}
			else
			{
				return -1;
			}
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

#endif // FEATURE_TAP
	}
}
