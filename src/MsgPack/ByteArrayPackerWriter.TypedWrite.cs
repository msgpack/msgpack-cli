
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
		public override void WriteBytes( byte header, byte value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( byte ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( byte ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( byte ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( byte ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( byte ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( byte ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( byte ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( byte header, ushort value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( ushort ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( ushort ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( ushort ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( ushort ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( ushort ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( ushort ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( byte header, uint value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( uint ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( uint ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( uint ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( uint ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( uint ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( uint ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( byte header, ulong value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( ulong ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( ulong ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( ulong ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( ulong ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( ulong ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( ulong ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( byte header, float value )
		{
			var bits = ToBits( value );
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( float ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( float ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( float ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( float ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( float ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( float ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( byte header, double value )
		{
			var bits = ToBits( value );
			var currentBuffer = this._currentBuffer;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( double ) + 1, ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			currentBuffer.Array[ currentBuffer.Offset ] = header;
			currentBuffer = currentBuffer.Slice( 1 );
			
			if ( !this.ShiftBufferIfNeeded( sizeof( double ), ref currentBuffer, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			var buffer = currentBuffer.Array;
			var offset = currentBuffer.Offset;
			var bufferRemaining = currentBuffer.Count;

			for ( var totalWritten = 0; totalWritten < sizeof( double ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( double ); currentWritten++, totalWritten++ )
				{
					buffer[ offset + currentWritten ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBuffer = currentBuffer.Slice( currentWritten );
			
				if ( !this.ShiftBufferIfNeeded( sizeof( double ) - totalWritten, ref currentBuffer, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( double ) );
				}

				buffer = currentBuffer.Array;
				offset = currentBuffer.Offset;
				bufferRemaining = currentBuffer.Count;
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
		}

		public override void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}

			if ( encodedLength <= this._currentBuffer.Count )
			{
				Encoding.UTF8.GetBytes( value, 0, value.Length, this._currentBuffer.Array, this._currentBuffer.Offset );
				this._currentBuffer = this._currentBuffer.Slice( encodedLength );
			}
			else
			{
				this.WriteStringBody( value );
			}
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

#if !FEATURE_POINTER_CONVERSION
		private void WriteStringBody( string value )
		{
			var chars = BufferManager.NewCharBuffer( value.Length );
			int offset = 0;

			while ( offset < value.Length )
			{
				int copying = Math.Min( value.Length - offset, chars.Length );
				value.CopyTo( offset, chars, 0, copying );
				this.WriteStringBody( chars, copying );
				offset += copying;
			}
		}

		private void WriteStringBody( char[] value, int remainingCharsLength )
		{
			var charsOffset = 0;
#else
		private unsafe void WriteStringBody( string value )
		{
			fixed ( char* pValue = value )
#endif // !FEATURE_POINTER_CONVERSION
			{
				var currentBuffer = this._currentBuffer;
				var currentBufferIndex = this._currentBufferIndex;
				var encoder = Encoding.UTF8.GetEncoder();
#if FEATURE_POINTER_CONVERSION
				var pChars = pValue;
				var remainingCharsLength = value.Length;
#endif // FEATURE_POINTER_CONVERSION
				var isCompleted = false;
				do
				{
					if ( !this.ShiftBufferIfNeeded( remainingCharsLength * sizeof( char ), ref currentBuffer, ref currentBufferIndex ) )
					{
						this.ThrowEofExceptionForString( ( value.Length - remainingCharsLength ) * sizeof( char ) );
					}

					int charsUsed, bytesUsed;
#if FEATURE_POINTER_CONVERSION
					fixed ( byte* pBuffer = currentBuffer.Array )
#endif // FEATURE_POINTER_CONVERSION
					{
#if FEATURE_POINTER_CONVERSION
						isCompleted = encoder.EncodeString( pChars, remainingCharsLength, pBuffer + currentBuffer.Offset, currentBuffer.Count, out charsUsed, out bytesUsed );
#else
						isCompleted = encoder.EncodeString( value, charsOffset, remainingCharsLength, currentBuffer.Array, currentBuffer.Offset, currentBuffer.Count, out charsUsed, out bytesUsed );
#endif // FEATURE_POINTER_CONVERSION
					}

#if FEATURE_POINTER_CONVERSION
					pChars += charsUsed;
#else
					charsOffset += charsUsed;
#endif // FEATURE_POINTER_CONVERSION
					remainingCharsLength -= charsUsed;
					currentBuffer = currentBuffer.Slice( bytesUsed );
				} while ( remainingCharsLength > 0 );
#if DEBUG
				Contract.Assert( isCompleted, "Encoding is not completed!" );
#endif // DEBUG

				this._currentBufferIndex = currentBufferIndex;
				this._currentBuffer = currentBuffer;
			}
		}
	}
}