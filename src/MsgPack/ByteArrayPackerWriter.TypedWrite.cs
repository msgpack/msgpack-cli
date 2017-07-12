
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
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( byte ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( byte ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( value >> ( ( sizeof( byte ) - 1 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( byte );
				currentBufferRemains -= sizeof( byte );
			}
			else
			{
				this.WriteBytesSlow( value, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( byte value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( byte ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( byte ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( byte ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( byte ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( byte ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( byte ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( byte ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( byte header, ushort value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( ushort ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( ushort ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 1 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 2 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( ushort );
				currentBufferRemains -= sizeof( ushort );
			}
			else
			{
				this.WriteBytesSlow( value, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( ushort value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( ushort ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ushort ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( ushort ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( ushort ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( ushort ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( ushort ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( byte header, uint value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( uint ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( uint ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 1 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 2 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 3 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 4 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( uint );
				currentBufferRemains -= sizeof( uint );
			}
			else
			{
				this.WriteBytesSlow( value, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( uint value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( uint ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( uint ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( uint ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( uint ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( uint ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( uint ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( byte header, ulong value )
		{
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( ulong ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( ulong ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 1 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 2 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 3 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 4 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 4 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 5 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 5 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 6 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 6 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 7 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 7 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 8 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( ulong );
				currentBufferRemains -= sizeof( ulong );
			}
			else
			{
				this.WriteBytesSlow( value, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( ulong value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( ulong ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( ulong ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( ulong ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( ulong ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( ulong ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( ulong ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( byte header, float value )
		{
			var bits = ToBits( value );
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( float ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( float ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 1 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 2 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 3 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 4 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( float );
				currentBufferRemains -= sizeof( float );
			}
			else
			{
				this.WriteBytesSlow( bits, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( int value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( float ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( float ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( float ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( float ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( float ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( float ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( float ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( byte header, double value )
		{
			var bits = ToBits( value );
			var currentBuffer = this._currentBuffer;
			var currentBufferOffset = this._currentBufferOffset;
			var currentBufferRemains = this._currentBufferRemains;
			var currentBufferIndex = this._currentBufferIndex;
			if ( !this.ShiftBufferIfNeeded( sizeof( double ) + 1, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			currentBuffer[ currentBufferOffset ] = header;
			currentBufferOffset += 1;
			currentBufferRemains -= 1;

			if ( currentBufferRemains >= sizeof( double ) )
			{
				// Fast path
				currentBuffer[ currentBufferOffset ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 1 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 2 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 3 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 4 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 4 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 5 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 5 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 6 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 6 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 7 ) * 8 ) & 0xFF ) );
				currentBuffer[ currentBufferOffset + 7 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 8 ) * 8 ) & 0xFF ) );
				currentBufferOffset += sizeof( double );
				currentBufferRemains -= sizeof( double );
			}
			else
			{
				this.WriteBytesSlow( bits, ref currentBufferIndex, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains );
			}

			this._currentBufferIndex = currentBufferIndex;
			this._currentBuffer = currentBuffer;
			this._currentBufferOffset = currentBufferOffset;
			this._currentBufferRemains = currentBufferRemains;
		}

		private void WriteBytesSlow( long value, ref int currentBufferIndex, ref byte[] currentBuffer, ref int currentBufferOffset, ref int currentBufferRemains )
		{			
			if ( !this.ShiftBufferIfNeeded( sizeof( double ), 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
			{
				this.ThrowEofException( sizeof( double ) );
			}

			var bufferRemaining = currentBufferRemains;

			for ( var totalWritten = 0; totalWritten < sizeof( double ); )
			{
				var currentWritten = 0;
				for ( ; currentWritten < bufferRemaining && totalWritten < sizeof( double ); currentWritten++, totalWritten++ )
				{
					currentBuffer[ currentBufferOffset + currentWritten ] = unchecked( ( byte )( value >> ( ( sizeof( double ) - totalWritten - 1 ) * 8 ) & 0xFF ) );
				}

				currentBufferOffset += currentWritten;
				currentBufferRemains -= currentWritten;
			
				if ( !this.ShiftBufferIfNeeded( sizeof( double ) - totalWritten, 1, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
				{
					this.ThrowEofException( sizeof( double ) );
				}

				bufferRemaining = currentBufferRemains;
			}
		}

		public override void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}

			if ( encodedLength <= this._currentBufferRemains )
			{
				// Fast path
				Encoding.UTF8.GetBytes( value, 0, value.Length, this._currentBuffer, this._currentBufferOffset );
				this._currentBufferOffset += encodedLength;
				this._currentBufferRemains -= encodedLength;
			}
			else
			{
				this.WriteStringBody( value, encodedLength );
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
		private void WriteStringBody( string value, int encodedLength )
		{
			var chars = BufferManager.NewCharBuffer( value.Length );
			int offset = 0;

			while ( offset < value.Length )
			{
				int copying = Math.Min( value.Length - offset, chars.Length );
				value.CopyTo( offset, chars, 0, copying );
				this.WriteStringBody( chars, copying, encodedLength );
				offset += copying;
			}
		}

		private void WriteStringBody( char[] value, int remainingCharsLength, int encodedLength )
		{
			var charsOffset = 0;
#else
		private unsafe void WriteStringBody( string value, int encodedLength )
		{
			fixed ( char* pValue = value )
#endif // !FEATURE_POINTER_CONVERSION
			{
				var currentBuffer = this._currentBuffer;
				var currentBufferOffset = this._currentBufferOffset;
				var currentBufferRemains = this._currentBufferRemains;
				var currentBufferIndex = this._currentBufferIndex;
				var encoder = Encoding.UTF8.GetEncoder();
#if FEATURE_POINTER_CONVERSION
				var pChars = pValue;
				var remainingCharsLength = value.Length;
#endif // FEATURE_POINTER_CONVERSION
				var remainingBytesLength = encodedLength; 
				var isCompleted = false;
				do
				{
					if ( !this.ShiftBufferIfNeeded( remainingBytesLength, MaximumUtf8Length, ref currentBuffer, ref currentBufferOffset, ref currentBufferRemains, ref currentBufferIndex ) )
					{
						this.ThrowEofExceptionForString( remainingBytesLength );
					}

					int charsUsed, bytesUsed;
#if FEATURE_POINTER_CONVERSION
					fixed ( byte* pBuffer = currentBuffer )
#endif // FEATURE_POINTER_CONVERSION
					{
#if FEATURE_POINTER_CONVERSION
						isCompleted = encoder.EncodeString( pChars, remainingCharsLength, pBuffer + currentBufferOffset, currentBufferRemains, out charsUsed, out bytesUsed );
#else
						isCompleted = encoder.EncodeString( value, charsOffset, remainingCharsLength, currentBuffer, currentBufferOffset, currentBufferRemains, out charsUsed, out bytesUsed );
#endif // FEATURE_POINTER_CONVERSION
					}

#if FEATURE_POINTER_CONVERSION
					pChars += charsUsed;
#else
					charsOffset += charsUsed;
#endif // FEATURE_POINTER_CONVERSION
					remainingCharsLength -= charsUsed;
					remainingBytesLength -= bytesUsed;
					currentBufferOffset += bytesUsed;
					currentBufferRemains -= bytesUsed;
				} while ( remainingCharsLength > 0 );
#if DEBUG
				Contract.Assert( isCompleted, "Encoding is not completed!" );
#endif // DEBUG

				this._currentBufferIndex = currentBufferIndex;
				this._currentBuffer = currentBuffer;
				this._currentBufferOffset = currentBufferOffset;
				this._currentBufferRemains = currentBufferRemains;
			}
		}
	}
}
