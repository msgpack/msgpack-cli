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
	// This file was generated from MessagePackUnpacker`1.Unpacking.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackUnpacker`1.Unpacking.tt and Core.ttinclude instead.

	partial class MessagePackUnpacker<TReader>
	{
		private ReadValueResult ReadValue( out byte header, out long integral, out float real32, out double real64 )
		{
			var readHeader = this.Reader.TryReadByte();
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
					this.CollectionType = CollectionType.None;
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
					this.CollectionType = CollectionType.None;
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
					integral = this.Reader.ReadSByte();
					return ReadValueResult.SByte;
				}
				case MessagePackCode.SignedInt16:
				{
					integral = this.Reader.ReadInt16();
					return ReadValueResult.Int16;
				}
				case MessagePackCode.SignedInt32:
				{
					integral = this.Reader.ReadInt32();
					return ReadValueResult.Int32;
				}
				case MessagePackCode.SignedInt64:
				{
					integral = this.Reader.ReadInt64();
					return ReadValueResult.Int64;
				}
				case MessagePackCode.UnsignedInt8:
				{
					integral = this.Reader.ReadByte();
					return ReadValueResult.Byte;
				}
				case MessagePackCode.UnsignedInt16:
				{
					integral = this.Reader.ReadUInt16();
					return ReadValueResult.UInt16;
				}
				case MessagePackCode.UnsignedInt32:
				{
					integral = this.Reader.ReadUInt32();
					return ReadValueResult.UInt32;
				}
				case MessagePackCode.UnsignedInt64:
				{
					integral = unchecked( ( long )this.Reader.ReadUInt64() );
					return ReadValueResult.UInt64;
				}
				case MessagePackCode.Real32:
				{
					real32 = this.Reader.ReadSingle();
					return ReadValueResult.Single;
				}
				case MessagePackCode.Real64:
				{
					real64 = this.Reader.ReadDouble();
					return ReadValueResult.Double;
				}
				case MessagePackCode.Bin8:
				{
					integral = this.Reader.ReadByte();
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Str8:
				{
					integral = this.Reader.ReadByte();
					return ReadValueResult.String;
				}
				case MessagePackCode.Bin16:
				{
					integral = this.Reader.ReadUInt16();
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Raw16:
				{
					integral = this.Reader.ReadUInt16();
					return ReadValueResult.String;
				}
				case MessagePackCode.Bin32:
				{
					integral = this.Reader.ReadUInt32();
					return ReadValueResult.Binary;
				}
				case MessagePackCode.Raw32:
				{
					integral = this.Reader.ReadUInt32();
					return ReadValueResult.String;
				}
				case MessagePackCode.Array16:
				{
					integral = this.Reader.ReadUInt16();
					return ReadValueResult.ArrayLength;
				}
				case MessagePackCode.Array32:
				{
					integral = this.Reader.ReadUInt32();
					return ReadValueResult.ArrayLength;
				}
				case MessagePackCode.Map16:
				{
					integral = this.Reader.ReadUInt16();
					return ReadValueResult.MapLength;
				}
				case MessagePackCode.Map32:
				{
					integral = this.Reader.ReadUInt32();
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
			var readHeader = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
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
					this.CollectionType = CollectionType.None;
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
					this.CollectionType = CollectionType.None;
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
					result.integral = await this.Reader.ReadSByteAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.SByte;
					return result;
				}
				case MessagePackCode.SignedInt16:
				{
					result.integral = await this.Reader.ReadInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Int16;
					return result;
				}
				case MessagePackCode.SignedInt32:
				{
					result.integral = await this.Reader.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Int32;
					return result;
				}
				case MessagePackCode.SignedInt64:
				{
					result.integral = await this.Reader.ReadInt64Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Int64;
					return result;
				}
				case MessagePackCode.UnsignedInt8:
				{
					result.integral = await this.Reader.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Byte;
					return result;
				}
				case MessagePackCode.UnsignedInt16:
				{
					result.integral = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.UInt16;
					return result;
				}
				case MessagePackCode.UnsignedInt32:
				{
					result.integral = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.UInt32;
					return result;
				}
				case MessagePackCode.UnsignedInt64:
				{
					result.integral = unchecked( ( long )await this.Reader.ReadUInt64Async( cancellationToken ).ConfigureAwait( false ) );
					result.type = ReadValueResult.UInt64;
					return result;
				}
				case MessagePackCode.Real32:
				{
					result.real32 = await this.Reader.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Single;
					return result;
				}
				case MessagePackCode.Real64:
				{
					result.real64 = await this.Reader.ReadDoubleAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Double;
					return result;
				}
				case MessagePackCode.Bin8:
				{
					result.integral = await this.Reader.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Str8:
				{
					result.integral = await this.Reader.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Bin16:
				{
					result.integral = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Raw16:
				{
					result.integral = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Bin32:
				{
					result.integral = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.Binary;
					return result;
				}
				case MessagePackCode.Raw32:
				{
					result.integral = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.String;
					return result;
				}
				case MessagePackCode.Array16:
				{
					result.integral = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.ArrayLength;
					return result;
				}
				case MessagePackCode.Array32:
				{
					result.integral = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.ArrayLength;
					return result;
				}
				case MessagePackCode.Map16:
				{
					result.integral = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					result.type = ReadValueResult.MapLength;
					return result;
				}
				case MessagePackCode.Map32:
				{
					result.integral = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
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
			this.CollectionType = CollectionType.Array;
			this.ItemsCount = unchecked( ( uint )length );
			this.Data = unchecked( ( uint )length );
			return length;
		}

		private long ReadMapLengthCore( long length )
		{
			this.CollectionType = CollectionType.Map;
			this.ItemsCount = unchecked( ( uint )length );
			this.Data = unchecked( ( uint )length );
			return length;
		}

		private byte[] ReadBinaryCore( long length )
		{
			if ( length == 0 )
			{
				this.CollectionType = CollectionType.None;
				return Binary.Empty;
			}

			this.CheckLength( length, ReadValueResult.Binary );
			var buffer = new byte[ length ];
			this.Reader.Read( buffer, buffer.Length );
			this.CollectionType = CollectionType.None;
			return buffer;
		}

#if FEATURE_TAP

		private async Task<byte[]> ReadBinaryAsyncCore( long length, CancellationToken cancellationToken )
		{
			if ( length == 0 )
			{
				this.CollectionType = CollectionType.None;
				return Binary.Empty;
			}

			this.CheckLength( length, ReadValueResult.Binary );
			var buffer = new byte[ length ];
			await this.Reader.ReadAsync( buffer, buffer.Length, cancellationToken ).ConfigureAwait( false );
			this.CollectionType = CollectionType.None;
			return buffer;
		}

#endif // FEATURE_TAP

		private string ReadStringCore( long length )
		{
			if ( length == 0 )
			{
				this.CollectionType = CollectionType.None;
				return String.Empty;
			}

			this.CheckLength( length, ReadValueResult.String );

			var result = this.Reader.ReadString( unchecked( ( int )length ) );
			this.CollectionType = CollectionType.None;
			return result;
		}

#if FEATURE_TAP

		private async Task<string> ReadStringAsyncCore( long length, CancellationToken cancellationToken )
		{
			if ( length == 0 )
			{
				this.CollectionType = CollectionType.None;
				return String.Empty;
			}

			this.CheckLength( length, ReadValueResult.String );

			var result = await this.Reader.ReadStringAsync( unchecked( ( int )length ), cancellationToken ).ConfigureAwait( false );
			this.CollectionType = CollectionType.None;
			return result;
		}

#endif // FEATURE_TAP

		private MessagePackExtendedTypeObject ReadMessagePackExtendedTypeObjectCore( ReadValueResult type )
		{
			uint length;
			switch ( ( uint )( type & ReadValueResult.VariableLengthMask ) )
			{
				case 0x20: // 001-00000
				{
					length = this.Reader.ReadByte();
					break;
				}
				case 0x40: // 010-00000
				{
					length = this.Reader.ReadUInt16();
					break;
				}
				case 0x80: // 100-00000
				{
					length = this.Reader.ReadUInt32();
					break;
				}
				default: // 000-xxxxx
				{
#if DEBUG
					Contract.Assert( ( uint )( type & ReadValueResult.VariableLengthMask ) == 0, ( uint )( type & ReadValueResult.VariableLengthMask ) + " == 0" );
#endif // DEBUG
					length = ( uint )( type & ReadValueResult.FixedLengthMask );
					break;
				}
			}

			var	typeCode = this.Reader.ReadByte();
			var data = new byte[ length ];
			this.Reader.Read( data, data.Length );
			this.CollectionType = CollectionType.None;
			return new MessagePackExtendedTypeObject( typeCode, data );
		}

#if FEATURE_TAP

		private async Task<MessagePackExtendedTypeObject> ReadMessagePackExtendedTypeObjectAsyncCore( ReadValueResult type, CancellationToken cancellationToken )
		{
			uint length;
			switch ( ( uint )( type & ReadValueResult.VariableLengthMask ) )
			{
				case 0x20: // 001-00000
				{
					length = await this.Reader.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 0x40: // 010-00000
				{
					length = await this.Reader.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 0x80: // 100-00000
				{
					length = await this.Reader.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
					break;
				}
				default: // 000-xxxxx
				{
#if DEBUG
					Contract.Assert( ( uint )( type & ReadValueResult.VariableLengthMask ) == 0, ( uint )( type & ReadValueResult.VariableLengthMask ) + " == 0" );
#endif // DEBUG
					length = ( uint )( type & ReadValueResult.FixedLengthMask );
					break;
				}
			}

			var	typeCode = await this.Reader.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
			var data = new byte[ length ];
			await this.Reader.ReadAsync( data, data.Length, cancellationToken ).ConfigureAwait( false );
			this.CollectionType = CollectionType.None;
			return new MessagePackExtendedTypeObject( typeCode, data );
		}

#endif // FEATURE_TAP

	}
}
