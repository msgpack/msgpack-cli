
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
	// This file was generated from StreamPackerWriter.TypedWrite.tt and PackerWriter.TypedWrite.ttinclude T4Template.
	// Do not modify this file. Edit StreamPackerWriter.TypedWrite.tt and PackerWriter.TypedWrite.ttinclude instead.

	partial class StreamPackerWriter
	{
		public override void WriteBytes( byte header, byte value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( byte ) + 1 );
		}

		public override void WriteBytes( byte header, ushort value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( ushort ) + 1 );
		}

		public override void WriteBytes( byte header, uint value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( uint ) + 1 );
		}

		public override void WriteBytes( byte header, ulong value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 56 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 48 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 40 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value >> 32 & 0xFF ) );
			this._scalarBuffer[ 5 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 6 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 7 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 8 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( ulong ) + 1 );
		}

		public override void WriteBytes( byte header, float value )
		{
			var bits = ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( float ) + 1 );
		}

		public override void WriteBytes( byte header, double value )
		{
			var bits = ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 56 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 48 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 40 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits >> 32 & 0xFF ) );
			this._scalarBuffer[ 5 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 6 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 7 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 8 ] = unchecked( ( byte )( bits & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( double ) + 1 );
		}



		public override void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}
			this.WriteStringBody( value );
		}
		
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
				this.WriteStringBody( chars );
				offset += copying;
			}
		}

		private void WriteStringBody( char[] value )
		{
#else
		private void WriteStringBody( string value )
		{
#endif // !FEATURE_POINTER_CONVERSION
			var buffer = BufferManager.NewByteBuffer( value.Length * 4 );
			var encoder = Encoding.UTF8.GetEncoder();
			var valueOffset = 0;
			var remainingCharsLength = value.Length;
			
			bool isCompleted = false;
			do
			{
				int charsUsed, bytesUsed;
				isCompleted = EncodeString( encoder, value, valueOffset, remainingCharsLength, buffer, out charsUsed, out bytesUsed );

				valueOffset += charsUsed;
				remainingCharsLength -= charsUsed;
				this._destination.Write( buffer, 0, bytesUsed );
			} while ( remainingCharsLength > 0 );

#if DEBUG
			Contract.Assert( isCompleted, "Encoding is not completed!" );
#endif // DEBUG
		}
#if FEATURE_TAP

		public override async Task WriteBytesAsync( byte header, byte value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( byte ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		public override async Task WriteBytesAsync( byte header, ushort value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( ushort ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		public override async Task WriteBytesAsync( byte header, uint value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( uint ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		public override async Task WriteBytesAsync( byte header, ulong value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 56 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 48 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 40 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value >> 32 & 0xFF ) );
			this._scalarBuffer[ 5 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 6 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 7 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 8 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( ulong ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		public override async Task WriteBytesAsync( byte header, float value, CancellationToken cancellationToken )
		{
			var bits = ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( float ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		public override async Task WriteBytesAsync( byte header, double value, CancellationToken cancellationToken )
		{
			var bits = ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 56 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 48 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 40 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits >> 32 & 0xFF ) );
			this._scalarBuffer[ 5 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 6 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 7 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 8 ] = unchecked( ( byte )( bits & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( double ) + 1, cancellationToken ).ConfigureAwait( false );
		}



		public override async Task WriteBytesAsync( string value, bool allowStr8, CancellationToken cancellationToken )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			await this.WriteStringHeaderAsync( encodedLength, allowStr8, cancellationToken ).ConfigureAwait( false );
			if ( encodedLength == 0 )
			{
				return;
			}
			await this.WriteStringBodyAsync( value, cancellationToken ).ConfigureAwait( false );
		}
		
		private async Task WriteStringHeaderAsync( int bytesLength, bool allowStr8, CancellationToken cancellationToken )
		{
			if( bytesLength < 0x20 )
			{
				await this.WriteByteAsync( ( byte )( bytesLength | MessagePackCode.MinimumFixedRaw ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( bytesLength < 0x100 && allowStr8 )
			{
				await this.WriteBytesAsync( MessagePackCode.Str8, ( byte )bytesLength, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( bytesLength < 0x10000 )
			{
				await this.WriteBytesAsync( MessagePackCode.Str16, ( ushort )bytesLength, cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( MessagePackCode.Str32, unchecked(( uint )bytesLength), cancellationToken ).ConfigureAwait( false );
		}

#if !FEATURE_POINTER_CONVERSION
		private async Task WriteStringBodyAsync( string value, CancellationToken cancellationToken )
		{
			var chars = BufferManager.NewCharBuffer( value.Length );
			int offset = 0;

			while ( offset < value.Length )
			{
				int copying = Math.Min( value.Length - offset, chars.Length );
				value.CopyTo( offset, chars, 0, copying );
				await this.WriteStringBodyAsync( chars, cancellationToken ).ConfigureAwait( false );
				offset += copying;
			}
		}

		private async Task WriteStringBodyAsync( char[] value, CancellationToken cancellationToken )
		{
#else
		private async Task WriteStringBodyAsync( string value, CancellationToken cancellationToken )
		{
#endif // !FEATURE_POINTER_CONVERSION
			var buffer = BufferManager.NewByteBuffer( value.Length * 4 );
			var encoder = Encoding.UTF8.GetEncoder();
			var valueOffset = 0;
			var remainingCharsLength = value.Length;
			
			bool isCompleted = false;
			do
			{
				int charsUsed, bytesUsed;
				isCompleted = EncodeString( encoder, value, valueOffset, remainingCharsLength, buffer, out charsUsed, out bytesUsed );

				valueOffset += charsUsed;
				remainingCharsLength -= charsUsed;
				await this._destination.WriteAsync( buffer, 0, bytesUsed, cancellationToken ).ConfigureAwait( false );
			} while ( remainingCharsLength > 0 );

#if DEBUG
			Contract.Assert( isCompleted, "Encoding is not completed!" );
#endif // DEBUG
		}
#endif // FEATURE_TAP

#if FEATURE_POINTER_CONVERSION
		private static unsafe bool EncodeString( Encoder encoder, string value, int startOffset, int count, byte[] buffer, out int charsUsed, out int bytesUsed )
		{
			fixed ( char* pValue = value )
			{
				var pChars = pValue + startOffset;

				fixed ( byte* pBuffer = buffer )
				{
					return encoder.EncodeString( pChars, count, pBuffer, buffer.Length, out charsUsed, out bytesUsed );
				}
			}
		}
#else
		private static bool EncodeString( Encoder encoder, char[] value, int startOffset, int count, byte[] buffer, out int charsUsed, out int bytesUsed )
		{
			return encoder.EncodeString( value, startOffset, count, buffer, 0, buffer.Length, out charsUsed, out bytesUsed );
		}
#endif // FEATURE_POINTER_CONVERSION
	}
}
