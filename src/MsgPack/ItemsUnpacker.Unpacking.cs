#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Text;

namespace MsgPack
{
	partial class ItemsUnpacker
	{
		private ReadValueResult ReadValue( out byte header, out long integral, out float real32, out double real64 )
		{
			// This is BAD practice for out, but it reduces IL size very well for this method.
			integral = default( long );
			real32 = default( float );
			real64 = default( double );

			var readHeader = this._source.ReadByte();
			if ( this._source.CanSeek )
			{
				// Re-sync offset here.
				this._offset = this._source.Position;
			}
			else
			{
				this._offset++;
			}

			if ( readHeader < 0 )
			{
				header = 0;
				return ReadValueResult.Eof;
			}

			header = unchecked( ( byte )readHeader );

			switch ( header >> 4 )
			{
				case 0x0:
				case 0x1:
				case 0x2:
				case 0x3:
				case 0x4:
				case 0x5:
				case 0x6:
				case 0x7:
				{
					// PositiveFixNum
					this.InternalCollectionType = CollectionType.None;
					integral = header;
					return ReadValueResult.Byte;
				}
				case 0x8:
				{
					// FixMap
					integral = header & 0xF;
					return ReadValueResult.MapLength;
				}
				case 0x9:
				{
					// FixArray
					integral = header & 0xF;
					return ReadValueResult.ArrayLength;
				}
				case 0xA:
				case 0xB:
				{
					// FixRaw
					integral = header & 0x1F;
					return ReadValueResult.String;
				}
				case 0xE:
				case 0xF:
				{
					// NegativeFixNum
					this.InternalCollectionType = CollectionType.None;
					integral = header | unchecked( ( long )0xFFFFFFFFFFFFFF00 );
					return ReadValueResult.SByte;
				}
			}

			switch ( header )
			{
				case MessagePackCode.NilValue:
				{
					return ReadValueResult.Nil;
				}
				case MessagePackCode.TrueValue:
				{
					integral = 1;
					return ReadValueResult.Boolean;
				}
				case MessagePackCode.FalseValue:
				{
					integral = 0;
					return ReadValueResult.Boolean;
				}
				case MessagePackCode.SignedInt8:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( sbyte ) );
					integral = BigEndianBinary.ToSByte( this._scalarBuffer, 0 );
					return ReadValueResult.SByte;
				}
				case MessagePackCode.SignedInt16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( short ) );
					integral = BigEndianBinary.ToInt16( this._scalarBuffer, 0 );
					return ReadValueResult.Int16;
				}
				case MessagePackCode.SignedInt32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( int ) );
					integral = BigEndianBinary.ToInt32( this._scalarBuffer, 0 );
					return ReadValueResult.Int32;
				}
				case MessagePackCode.SignedInt64:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( long ) );
					integral = BigEndianBinary.ToInt64( this._scalarBuffer, 0 );
					return ReadValueResult.Int64;
				}
				case MessagePackCode.UnsignedInt8:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( byte ) );
					integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					return ReadValueResult.Byte;
				}
				case MessagePackCode.UnsignedInt16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					return ReadValueResult.UInt16;
				}
				case MessagePackCode.UnsignedInt32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					return ReadValueResult.UInt32;
				}
				case MessagePackCode.UnsignedInt64:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ulong ) );
					integral = unchecked( ( long )BigEndianBinary.ToUInt64( this._scalarBuffer, 0 ) );
					return ReadValueResult.UInt64;
				}
				case MessagePackCode.Real32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( float ) );
					real32 = BigEndianBinary.ToSingle( this._scalarBuffer, 0 );
					return ReadValueResult.Single;
				}
				case MessagePackCode.Real64:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( double ) );
					real64 = BigEndianBinary.ToDouble( this._scalarBuffer, 0 );
					return ReadValueResult.Double;
				}
				case MessagePackCode.Bin8:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( byte ) );
					integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Str8:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( byte ) );
					integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					return ReadValueResult.String;
				}
				case MessagePackCode.Bin16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Raw16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					return ReadValueResult.String;
				}
				case MessagePackCode.Bin32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Raw32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					return ReadValueResult.String;
				}
				case MessagePackCode.Array16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					return ReadValueResult.ArrayLength;
				}
				case MessagePackCode.Array32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					return ReadValueResult.ArrayLength;
				}
				case MessagePackCode.Map16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					return ReadValueResult.MapLength;
				}
				case MessagePackCode.Map32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					return ReadValueResult.MapLength;
				}
				case MessagePackCode.FixExt1:
				{
					return ReadValueResult.FixExt1;
				}
				case MessagePackCode.FixExt2:
				{
					return ReadValueResult.FixExt2;
				}
				case MessagePackCode.FixExt4:
				{
					return ReadValueResult.FixExt4;
				}
				case MessagePackCode.FixExt8:
				{
					return ReadValueResult.FixExt8;
				}
				case MessagePackCode.FixExt16:
				{
					return ReadValueResult.FixExt16;
				}
				case MessagePackCode.Ext8:
				{
					return ReadValueResult.Ext8;
				}
				case MessagePackCode.Ext16:
				{
					return ReadValueResult.Ext16;
				}
				case MessagePackCode.Ext32:
				{
					return ReadValueResult.Ext32;
				}
				default:
				{
					ThrowUnassingedMessageTypeException( header );
					return ReadValueResult.Eof; // Never reach
				}
			}
		}

		private static void ThrowUnassingedMessageTypeException( int header )
		{
#if DEBUG && !UNITY
			Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
#endif // DEBUG && !UNITY
			throw new UnassignedMessageTypeException(
				String.Format( CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", header ) 
			);
		}

		private long ReadArrayLengthCore( long length )
		{
			this.InternalCollectionType = CollectionType.Array;
			this.InternalItemsCount = length;
			this.InternalData = unchecked( ( uint ) length );
			return length;
		}

		private long ReadMapLengthCore( long length )
		{
			this.InternalCollectionType = CollectionType.Map;
			this.InternalItemsCount = length;
			this.InternalData = unchecked( ( uint )length );
			return length;
		}

		private byte[] ReadBinaryCore( long length )
		{
			if ( length == 0 )
			{
				this.InternalCollectionType = CollectionType.None;
				return Binary.Empty;
			}

			this.CheckLength( length, ReadValueResult.Binary );
			var buffer = new byte[ length ];
			this.ReadStrict( buffer, buffer.Length );
			this.InternalCollectionType = CollectionType.None;
			return buffer;
		}

		private string ReadStringCore( long length )
		{
			if ( length == 0 )
			{
				this.InternalCollectionType = CollectionType.None;
				return String.Empty;
			}

			this.CheckLength( length, ReadValueResult.String );

			var length32 = unchecked( ( int )length );
			var bytes = BufferManager.GetByteBuffer();
#if DEBUG
			try
			{ 
#endif // DEBUG

			if ( length32 <= bytes.Length )
			{
				this.ReadStrict( bytes, length32 );
				var result = Encoding.UTF8.GetString( bytes, 0, length32 );
				this.InternalCollectionType = CollectionType.None;
				return result;
			}

			var decoder = Encoding.UTF8.GetDecoder();
			var chars = BufferManager.GetCharBuffer();
#if DEBUG
			try
			{ 
#endif // DEBUG
			var stringBuffer = new StringBuilder( Math.Min( length32, Int32.MaxValue ) );
			var remaining = length32;
			do
			{
				var reading = Math.Min( remaining, bytes.Length );
				var bytesRead = this._source.Read( bytes, 0, reading );
				this._offset += bytesRead;
				if ( bytesRead == 0 )
				{
					this.ThrowEofException( 0, reading );
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
			} while ( remaining > 0 );

			this.InternalCollectionType = CollectionType.None;
			return stringBuffer.ToString();
#if DEBUG
			}
			finally
			{
				BufferManager.ReleaseCharBuffer();
			}
			}
			finally
			{
				BufferManager.ReleaseByteBuffer();
			}
#endif // DEBUG
		}

		private MessagePackExtendedTypeObject ReadMessagePackExtendedTypeObjectCore( ReadValueResult type )
		{
			byte typeCode;
			uint length;
			switch ( type )
			{
				case ReadValueResult.FixExt1:
				{
					typeCode = this.ReadByteStrict();
					length = 1;
					break;
				}
				case ReadValueResult.FixExt2:
				{
					typeCode = this.ReadByteStrict();
					length = 2;
					break;
				}
				case ReadValueResult.FixExt4:
				{
					typeCode = this.ReadByteStrict();
					length = 4;
					break;
				}
				case ReadValueResult.FixExt8:
				{
					typeCode = this.ReadByteStrict();
					length = 8;
					break;
				}
				case ReadValueResult.FixExt16:
				{
					typeCode = this.ReadByteStrict();
					length = 16;
					break;
				}
				case ReadValueResult.Ext8:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( byte ) );
					length = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				case ReadValueResult.Ext16:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( ushort ) );
					length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				case ReadValueResult.Ext32:
				{
					this.ReadStrict( this._scalarBuffer, sizeof( uint ) );
					length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				default:
				{
					ThrowUnexpectedExtCodeException( type );
					return default( MessagePackExtendedTypeObject ); // Never reach
				}
			}

			var data = new byte[ length ];
			this.ReadStrict( data, data.Length );
			this.InternalCollectionType = CollectionType.None;
			return new MessagePackExtendedTypeObject( typeCode, data );
		}

		private static void ThrowUnexpectedExtCodeException( ReadValueResult type )
		{
#if DEBUG && !UNITY
			Contract.Assert( false, "Unexpected ext-code type:" + type );
#endif // DEBUG && !UNITY
			// ReSharper disable once HeuristicUnreachableCode
			throw new NotSupportedException( "Unexpeded ext-code type. " + type );
		}

		private void CheckLength( long length, ReadValueResult type )
		{
			if ( length > Int32.MaxValue )
			{
				this.ThrowTooLongLengthException( length, type );
			}
		}

		private void ThrowTooLongLengthException( long length, ReadValueResult type )
		{
			string message;
			switch ( type )
			{
				case ReadValueResult.ArrayLength:
				{
					message =
						this._source.CanSeek
						? "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at offset {1:#,0}";
					break;
				}
				case ReadValueResult.MapLength:
				{
					message =
						this._source.CanSeek
						? "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at offset {1:#,0}";
					break;
				}
				default:
				{
					message =
						this._source.CanSeek
						? "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at offset {1:#,0}";
					break;
				}
			}

			throw new MessageNotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					message,
					length,
					this._offset
				)
			);
		}

		private void ThrowTypeException( Type type, byte header )
		{
			throw new MessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					this._source.CanSeek
					? "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in position {3:#,0}."
					: "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in offset {3:#,0}.",
					type,
					header,
					MessagePackCode.ToString( header ),
					this._offset
				)
			);
		}

		private enum ReadValueResult
		{
			Eof,
			Nil,
			Boolean,
			SByte,
			Byte,
			Int16,
			UInt16,
			Int32,
			UInt32,
			Int64,
			UInt64,
			Single,
			Double,
			ArrayLength,
			MapLength,
			String,
			Binary,
			FixExt1,
			FixExt2,
			FixExt4,
			FixExt8,
			FixExt16,
			Ext8,
			Ext16,
			Ext32,
		}
	}
}
