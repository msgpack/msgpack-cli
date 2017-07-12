
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
	// This file was generated from ByteArrayUnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude T4Template.
	// Do not modify this file. Edit ByteArrayUnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude instead.

	partial class ByteArrayUnpackerReader
	{

		public override byte ReadByte()
		{
			var read = this.TryReadByte();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			return unchecked( ( byte )read );
		}

		public override int TryReadByte()
		{
			if ( this._currentSourceRemains >= sizeof( byte ) )
			{
				// fast path
				var result = BigEndianBinary.ToByte( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( byte );
				this._currentSourceRemains -= sizeof( byte );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( byte ) ) )
			{
				return -1;
			}

			return BigEndianBinary.ToByte( this._scalarBuffer, 0 );
		}

		public override sbyte ReadSByte()
		{
			if ( this._currentSourceRemains >= sizeof( sbyte ) )
			{
				// fast path
				var result = BigEndianBinary.ToSByte( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( sbyte );
				this._currentSourceRemains -= sizeof( sbyte );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( sbyte ) ) )
			{
				this.ThrowEofException( sizeof( sbyte ) );
			}

			return BigEndianBinary.ToSByte( this._scalarBuffer, 0 );
		}

		public override short ReadInt16()
		{
			if ( this._currentSourceRemains >= sizeof( short ) )
			{
				// fast path
				var result = BigEndianBinary.ToInt16( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( short );
				this._currentSourceRemains -= sizeof( short );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( short ) ) )
			{
				this.ThrowEofException( sizeof( short ) );
			}

			return BigEndianBinary.ToInt16( this._scalarBuffer, 0 );
		}

		public override ushort ReadUInt16()
		{
			var read = this.TryReadUInt16();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			return unchecked( ( ushort )read );
		}

		public override int TryReadUInt16()
		{
			if ( this._currentSourceRemains >= sizeof( ushort ) )
			{
				// fast path
				var result = BigEndianBinary.ToUInt16( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( ushort );
				this._currentSourceRemains -= sizeof( ushort );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( ushort ) ) )
			{
				return -1;
			}

			return BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
		}

		public override int ReadInt32()
		{
			if ( this._currentSourceRemains >= sizeof( int ) )
			{
				// fast path
				var result = BigEndianBinary.ToInt32( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( int );
				this._currentSourceRemains -= sizeof( int );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( int ) ) )
			{
				this.ThrowEofException( sizeof( int ) );
			}

			return BigEndianBinary.ToInt32( this._scalarBuffer, 0 );
		}

		public override uint ReadUInt32()
		{
			var read = this.TryReadUInt32();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			return unchecked( ( uint )read );
		}

		public override long TryReadUInt32()
		{
			if ( this._currentSourceRemains >= sizeof( uint ) )
			{
				// fast path
				var result = BigEndianBinary.ToUInt32( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( uint );
				this._currentSourceRemains -= sizeof( uint );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( uint ) ) )
			{
				return -1;
			}

			return BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
		}

		public override long ReadInt64()
		{
			if ( this._currentSourceRemains >= sizeof( long ) )
			{
				// fast path
				var result = BigEndianBinary.ToInt64( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( long );
				this._currentSourceRemains -= sizeof( long );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( long ) ) )
			{
				this.ThrowEofException( sizeof( long ) );
			}

			return BigEndianBinary.ToInt64( this._scalarBuffer, 0 );
		}

		public override ulong ReadUInt64()
		{
			if ( this._currentSourceRemains >= sizeof( ulong ) )
			{
				// fast path
				var result = BigEndianBinary.ToUInt64( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( ulong );
				this._currentSourceRemains -= sizeof( ulong );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( ulong ) ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			return BigEndianBinary.ToUInt64( this._scalarBuffer, 0 );
		}

		public override float ReadSingle()
		{
			if ( this._currentSourceRemains >= sizeof( float ) )
			{
				// fast path
				var result = BigEndianBinary.ToSingle( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( float );
				this._currentSourceRemains -= sizeof( float );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( float ) ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			return BigEndianBinary.ToSingle( this._scalarBuffer, 0 );
		}

		public override double ReadDouble()
		{
			if ( this._currentSourceRemains >= sizeof( double ) )
			{
				// fast path
				var result = BigEndianBinary.ToDouble( this._currentSource, this._currentSourceOffset );
				this._currentSourceOffset += sizeof( double );
				this._currentSourceRemains -= sizeof( double );
				return result;
			}

			// slow path
			if ( !this.TryReadSlow( this._scalarBuffer, sizeof( double ) ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			return BigEndianBinary.ToDouble( this._scalarBuffer, 0 );
		}

#if FEATURE_TAP

		public override Task<byte> ReadByteAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadByte() );
		}

		public override Task<int> TryReadByteAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.TryReadByte() );
		}

		public override Task<sbyte> ReadSByteAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadSByte() );
		}

		public override Task<short> ReadInt16Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadInt16() );
		}

		public override Task<ushort> ReadUInt16Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadUInt16() );
		}

		public override Task<int> TryReadUInt16Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.TryReadUInt16() );
		}

		public override Task<int> ReadInt32Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadInt32() );
		}

		public override Task<uint> ReadUInt32Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadUInt32() );
		}

		public override Task<long> TryReadUInt32Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.TryReadUInt32() );
		}

		public override Task<long> ReadInt64Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadInt64() );
		}

		public override Task<ulong> ReadUInt64Async( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadUInt64() );
		}

		public override Task<float> ReadSingleAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadSingle() );
		}

		public override Task<double> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadDouble() );
		}

#endif // FEATURE_TAP
	}
}
