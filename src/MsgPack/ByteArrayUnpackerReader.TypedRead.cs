
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
			var read = this.TryReadByte ();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			return unchecked( ( byte )read );
		}

		public override int TryReadByte()
		{
			if ( this._source.Count - this._offset < sizeof( byte ) )
			{
				return -1;
			}

			var result = BigEndianBinary.ToByte( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( byte );
			return result;
		}

		public override sbyte ReadSByte()
		{
			if ( this._source.Count - this._offset < sizeof( sbyte ) )
			{
				this.ThrowEofException( sizeof( sbyte ) );
			}

			var result = BigEndianBinary.ToSByte( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( sbyte );
			return result;
		}

		public override short ReadInt16()
		{
			if ( this._source.Count - this._offset < sizeof( short ) )
			{
				this.ThrowEofException( sizeof( short ) );
			}

			var result = BigEndianBinary.ToInt16( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( short );
			return result;
		}

		public override ushort ReadUInt16()
		{
			var read = this.TryReadUInt16 ();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			return unchecked( ( ushort )read );
		}

		public override int TryReadUInt16()
		{
			if ( this._source.Count - this._offset < sizeof( ushort ) )
			{
				return -1;
			}

			var result = BigEndianBinary.ToUInt16( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( ushort );
			return result;
		}

		public override int ReadInt32()
		{
			if ( this._source.Count - this._offset < sizeof( int ) )
			{
				this.ThrowEofException( sizeof( int ) );
			}

			var result = BigEndianBinary.ToInt32( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( int );
			return result;
		}

		public override uint ReadUInt32()
		{
			var read = this.TryReadUInt32 ();
			if ( read < 0 )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			return unchecked( ( uint )read );
		}

		public override long TryReadUInt32()
		{
			if ( this._source.Count - this._offset < sizeof( uint ) )
			{
				return -1;
			}

			var result = BigEndianBinary.ToUInt32( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( uint );
			return result;
		}

		public override long ReadInt64()
		{
			if ( this._source.Count - this._offset < sizeof( long ) )
			{
				this.ThrowEofException( sizeof( long ) );
			}

			var result = BigEndianBinary.ToInt64( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( long );
			return result;
		}

		public override ulong ReadUInt64()
		{
			if ( this._source.Count - this._offset < sizeof( ulong ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			var result = BigEndianBinary.ToUInt64( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( ulong );
			return result;
		}

		public override float ReadSingle()
		{
			if ( this._source.Count - this._offset < sizeof( float ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			var result = BigEndianBinary.ToSingle( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( float );
			return result;
		}

		public override double ReadDouble()
		{
			if ( this._source.Count - this._offset < sizeof( double ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			var result = BigEndianBinary.ToDouble( this._source.Array, this._source.Offset + this._offset );
			this._offset += sizeof( double );
			return result;
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
