#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from ItemsUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit ItemsUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class ItemsUnpacker
	{
		private ReadValueResult ReadValue( out byte header, out long integral, out float real32, out double real64 )
		{
			var readHeader = this.ReadByteFromSource();
			// This is BAD practice for out, but it reduces IL size very well for this method.
			integral = default( long );
			real32 = default( float );
			real64 = default( double );

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
					this.ThrowUnassignedMessageTypeException( readHeader );
					// Never reach
					return ReadValueResult.Eof;
				}
			}
		}

#if FEATURE_TAP

		private async Task<AsyncReadValueResult> ReadValueAsync( CancellationToken cancellationToken )
		{
			var readHeader = await this.ReadByteFromSourceAsync( cancellationToken ).ConfigureAwait( false );
			var result = default( AsyncReadValueResult );

			if ( readHeader < 0 )
			{
				return result;
			}

			var header = unchecked( ( byte )readHeader );
			result.header = header;

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
					result.integral = header;
					result.type = ReadValueResult.Byte;
					return result;
				}
				case 0x8:
				{
					// FixMap
					result.integral = header & 0xF;
					result.type = ReadValueResult.MapLength;
					return result;
				}
				case 0x9:
				{
					// FixArray
					result.integral = header & 0xF;
					result.type = ReadValueResult.ArrayLength;
					return result;
				}
				case 0xA:
				case 0xB:
				{
					// FixRaw
					result.integral = header & 0x1F;
					result.type = ReadValueResult.String;
					return result;
				}
				case 0xE:
				case 0xF:
				{
					// NegativeFixNum
					this.InternalCollectionType = CollectionType.None;
					result.integral = header | unchecked( ( long )0xFFFFFFFFFFFFFF00 );
					result.type = ReadValueResult.SByte;
					return result;
				}
			}

			switch ( header )
			{
				case MessagePackCode.NilValue:
				{
					result.type = ReadValueResult.Nil;
					return result;
				}
				case MessagePackCode.TrueValue:
				{
					result.integral = 1;
					result.type = ReadValueResult.Boolean;
					return result;
				}
				case MessagePackCode.FalseValue:
				{
					result.integral = 0;
					result.type = ReadValueResult.Boolean;
					return result;
				}
				case MessagePackCode.SignedInt8:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( sbyte ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToSByte( this._scalarBuffer, 0 );
					result.type = ReadValueResult.SByte;
					return result;
				}
				case MessagePackCode.SignedInt16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( short ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Int16;
					return result;
				}
				case MessagePackCode.SignedInt32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( int ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Int32;
					return result;
				}
				case MessagePackCode.SignedInt64:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( long ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToInt64( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Int64;
					return result;
				}
				case MessagePackCode.UnsignedInt8:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( byte ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Byte;
					return result;
				}
				case MessagePackCode.UnsignedInt16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.UInt16;
					return result;
				}
				case MessagePackCode.UnsignedInt32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.UInt32;
					return result;
				}
				case MessagePackCode.UnsignedInt64:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ulong ), cancellationToken ).ConfigureAwait( false );
					result.integral = unchecked( ( long )BigEndianBinary.ToUInt64( this._scalarBuffer, 0 ) );
					result.type = ReadValueResult.UInt64;
					return result;
				}
				case MessagePackCode.Real32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( float ), cancellationToken ).ConfigureAwait( false );
					result.real32 = BigEndianBinary.ToSingle( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Single;
					return result;
				}
				case MessagePackCode.Real64:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( double ), cancellationToken ).ConfigureAwait( false );
					result.real64 = BigEndianBinary.ToDouble( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Double;
					return result;
				}
				case MessagePackCode.Bin8:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( byte ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Str8:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( byte ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Bin16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Raw16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Bin32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Raw32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Array16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.ArrayLength;
					return result;
				}
				case MessagePackCode.Array32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.ArrayLength;
					return result;
				}
				case MessagePackCode.Map16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					result.type = ReadValueResult.MapLength;
					return result;
				}
				case MessagePackCode.Map32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					result.integral = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					result.type = ReadValueResult.MapLength;
					return result;
				}
				case MessagePackCode.FixExt1:
				{
					result.type = ReadValueResult.FixExt1;
					return result;
				}
				case MessagePackCode.FixExt2:
				{
					result.type = ReadValueResult.FixExt2;
					return result;
				}
				case MessagePackCode.FixExt4:
				{
					result.type = ReadValueResult.FixExt4;
					return result;
				}
				case MessagePackCode.FixExt8:
				{
					result.type = ReadValueResult.FixExt8;
					return result;
				}
				case MessagePackCode.FixExt16:
				{
					result.type = ReadValueResult.FixExt16;
					return result;
				}
				case MessagePackCode.Ext8:
				{
					result.type = ReadValueResult.Ext8;
					return result;
				}
				case MessagePackCode.Ext16:
				{
					result.type = ReadValueResult.Ext16;
					return result;
				}
				case MessagePackCode.Ext32:
				{
					result.type = ReadValueResult.Ext32;
					return result;
				}
				default:
				{
					this.ThrowUnassignedMessageTypeException( readHeader );
					// Never reach
					result.type = ReadValueResult.Eof;
					return result;
				}
			}
		}

#endif // FEATURE_TAP

		private long ReadArrayLengthCore( long length )
		{
			this.InternalCollectionType = CollectionType.Array;
			this.InternalItemsCount = length;
			this.InternalData = unchecked( ( uint )length );
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

#if FEATURE_TAP

		private async Task<byte[]> ReadBinaryAsyncCore( long length, CancellationToken cancellationToken )
		{
			if ( length == 0 )
			{
				this.InternalCollectionType = CollectionType.None;
				return Binary.Empty;
			}

			this.CheckLength( length, ReadValueResult.Binary );
			var buffer = new byte[ length ];
			await this.ReadStrictAsync( buffer, buffer.Length, cancellationToken ).ConfigureAwait( false );
			this.InternalCollectionType = CollectionType.None;
			return buffer;
		}

#endif // FEATURE_TAP

		private string ReadStringCore( long length )
		{
			if ( length == 0 )
			{
				this.InternalCollectionType = CollectionType.None;
				return String.Empty;
			}

			this.CheckLength( length, ReadValueResult.String );

			var length32 = unchecked( ( int )length );
			var bytes = BufferManager.NewByteBuffer( length32 );

			if ( length32 <= bytes.Length )
			{
				this.ReadStrict( bytes, length32 );
				var result = Encoding.UTF8.GetString( bytes, 0, length32 );
				this.InternalCollectionType = CollectionType.None;
				return result;
			}

			var decoder = Encoding.UTF8.GetDecoder();
			var chars = BufferManager.NewCharBuffer( bytes.Length );
			var stringBuffer = new StringBuilder( length32 );
			var remaining = length32;
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
		}

#if FEATURE_TAP

		private async Task<string> ReadStringAsyncCore( long length, CancellationToken cancellationToken )
		{
			if ( length == 0 )
			{
				this.InternalCollectionType = CollectionType.None;
				return String.Empty;
			}

			this.CheckLength( length, ReadValueResult.String );

			var length32 = unchecked( ( int )length );
			var bytes = BufferManager.NewByteBuffer( length32 );

			if ( length32 <= bytes.Length )
			{
				await this.ReadStrictAsync( bytes, length32, cancellationToken ).ConfigureAwait( false );
				var result = Encoding.UTF8.GetString( bytes, 0, length32 );
				this.InternalCollectionType = CollectionType.None;
				return result;
			}

			var decoder = Encoding.UTF8.GetDecoder();
			var chars = BufferManager.NewCharBuffer( bytes.Length );
			var stringBuffer = new StringBuilder( length32 );
			var remaining = length32;
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
		}

#endif // FEATURE_TAP

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
					this.ThrowUnexpectedExtCodeException( type );
					return default( MessagePackExtendedTypeObject ); // Never reach
				}
			}

			var data = new byte[ length ];
			this.ReadStrict( data, data.Length );
			this.InternalCollectionType = CollectionType.None;
			return new MessagePackExtendedTypeObject( typeCode, data );
		}

#if FEATURE_TAP

		private async Task<MessagePackExtendedTypeObject> ReadMessagePackExtendedTypeObjectAsyncCore( ReadValueResult type, CancellationToken cancellationToken )
		{
			byte typeCode;
			uint length;
			switch ( type )
			{
				case ReadValueResult.FixExt1:
				{
					typeCode = await this.ReadByteStrictAsync( cancellationToken ).ConfigureAwait( false );
					length = 1;
					break;
				}
				case ReadValueResult.FixExt2:
				{
					typeCode = await this.ReadByteStrictAsync( cancellationToken ).ConfigureAwait( false );
					length = 2;
					break;
				}
				case ReadValueResult.FixExt4:
				{
					typeCode = await this.ReadByteStrictAsync( cancellationToken ).ConfigureAwait( false );
					length = 4;
					break;
				}
				case ReadValueResult.FixExt8:
				{
					typeCode = await this.ReadByteStrictAsync( cancellationToken ).ConfigureAwait( false );
					length = 8;
					break;
				}
				case ReadValueResult.FixExt16:
				{
					typeCode = await this.ReadByteStrictAsync( cancellationToken ).ConfigureAwait( false );
					length = 16;
					break;
				}
				case ReadValueResult.Ext8:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( byte ), cancellationToken ).ConfigureAwait( false );
					length = BigEndianBinary.ToByte( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				case ReadValueResult.Ext16:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( ushort ), cancellationToken ).ConfigureAwait( false );
					length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				case ReadValueResult.Ext32:
				{
					await this.ReadStrictAsync( this._scalarBuffer, sizeof( uint ), cancellationToken ).ConfigureAwait( false );
					length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
					typeCode = this.ReadByteStrict();
					break;
				}
				default:
				{
					this.ThrowUnexpectedExtCodeException( type );
					return default( MessagePackExtendedTypeObject ); // Never reach
				}
			}

			var data = new byte[ length ];
			await this.ReadStrictAsync( data, data.Length, cancellationToken ).ConfigureAwait( false );
			this.InternalCollectionType = CollectionType.None;
			return new MessagePackExtendedTypeObject( typeCode, data );
		}

#endif // FEATURE_TAP

	}
}
