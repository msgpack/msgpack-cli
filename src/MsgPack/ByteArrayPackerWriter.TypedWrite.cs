
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
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from ByteArrayPackerWriter.TypedWrite.tt and PackerWriter.TypedWrite.ttinclude T4Template.
	// Do not modify this file. Edit ByteArrayPackerWriter.TypedWrite.tt and PackerWriter.TypedWrite.ttinclude instead.

	partial class ByteArrayPackerWriter
	{
		private const int MaximumUtf8Length = 4;

		public override void WriteBytes( byte header, byte value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( byte ) && !this._allocator.TryAllocate( buffer, sizeof( byte ), out buffer ) )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( byte ) - 1 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( byte );
		}


		public override void WriteBytes( byte header, ushort value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( ushort ) && !this._allocator.TryAllocate( buffer, sizeof( ushort ), out buffer ) )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 2 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( ushort );
		}


		public override void WriteBytes( byte header, uint value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( uint ) && !this._allocator.TryAllocate( buffer, sizeof( uint ), out buffer ) )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 4 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( uint );
		}


		public override void WriteBytes( byte header, ulong value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( ulong ) && !this._allocator.TryAllocate( buffer, sizeof( ulong ), out buffer ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 4 ) * 8 ) & 0xFF ) );
			buffer[ offset + 4 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 5 ) * 8 ) & 0xFF ) );
			buffer[ offset + 5 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 6 ) * 8 ) & 0xFF ) );
			buffer[ offset + 6 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 7 ) * 8 ) & 0xFF ) );
			buffer[ offset + 7 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 8 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( ulong );
		}


		public override void WriteBytes( byte header, float value )
		{
			var bits = ToBits( value );
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( float ) && !this._allocator.TryAllocate( buffer, sizeof( float ), out buffer ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 4 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( float );
		}


		public override void WriteBytes( byte header, double value )
		{
			var bits = ToBits( value );
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			if ( remains < sizeof( double ) && !this._allocator.TryAllocate( buffer, sizeof( double ), out buffer ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			buffer[ offset ] = header;
			offset++;

			// Fast path
			buffer[ offset ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 4 ) * 8 ) & 0xFF ) );
			buffer[ offset + 4 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 5 ) * 8 ) & 0xFF ) );
			buffer[ offset + 5 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 6 ) * 8 ) & 0xFF ) );
			buffer[ offset + 6 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 7 ) * 8 ) & 0xFF ) );
			buffer[ offset + 7 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 8 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( double );
		}


		public override void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}

			var buffer  = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;

			if ( remains < encodedLength && !this._allocator.TryAllocate( buffer, encodedLength, out buffer ) )
			{
				this.ThrowEofException( encodedLength );
			}

			Encoding.UTF8.GetBytes( value, 0, value.Length, buffer, offset );
			this._buffer = buffer;
			this._offset += encodedLength;
		}

#if FEATURE_TAP

		public override Task WriteBytesAsync( byte header, byte value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( byte header, ushort value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( byte header, uint value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( byte header, ulong value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( byte header, float value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( byte header, double value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}


		public override Task WriteBytesAsync( string value, bool allowStr8, CancellationToken cancellationToken )
		{
			this.WriteBytes( value, allowStr8 );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP
		private void WriteStringHeader( int bytesLength, bool allowStr8 )
		{
			if( bytesLength < 0x20 )
			{
				this.WriteByte( ( byte )( bytesLength | MessagePackCode.MinimumFixedRaw ) );
				return;
			}

			if ( bytesLength < 0x100 && allowStr8 )
			{
				this.WriteBytes( MessagePackCode.Str8, ( byte )bytesLength );
				return;
			}

			if ( bytesLength < 0x10000 )
			{
				this.WriteBytes( MessagePackCode.Str16, ( ushort )bytesLength );
				return;
			}

			this.WriteBytes( MessagePackCode.Str32, unchecked(( uint )bytesLength) );
		}
	}
}
