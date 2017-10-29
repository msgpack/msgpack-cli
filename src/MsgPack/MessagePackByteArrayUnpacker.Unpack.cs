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
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

#if !UNITY || MSGPACK_UNITY_FULL
using Int64Stack = System.Collections.Generic.Stack<System.Int64>;
#endif // !UNITY || MSGPACK_UNITY_FULL

namespace MsgPack
{
	// This file was generated from MessagePackByteArrayUnpacker.Unpack.tt and MessagePackUnpackerCommon.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackByteArrayUnpacker.Unpack.tt and MessagePackUnpackerCommon.ttinclude instead.

	partial class MessagePackByteArrayUnpacker
	{
		public sealed override bool ReadByte( out Byte result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Byte );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Byte )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadByteSlow( header, source, ref offset, out result ) )
				{
					result = default( Byte );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadByteSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Byte result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Byte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Byte );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Byte )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Byte )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Byte )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Byte )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Byte )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Byte )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Byte )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Byte )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Byte )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Byte )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Byte ), header );
						// Never
						result = default( Byte );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableByte( out Byte? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Byte? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Byte? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableByteSlow( header, source, ref offset, out result ) )
				{
					result = default( Byte? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableByteSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Byte? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Byte? );
				offset++;
				return true;
			}

			Byte value;
			if( !this.ReadByteSlow( header, source, ref offset, out value ) )
			{
				result = default( Byte? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken )
		{
			Byte result;
			return Task.FromResult( this.ReadByte( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Byte>() );
		}

		public sealed override Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken )
		{
			Byte? result;
			return Task.FromResult( this.ReadNullableByte( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Byte?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadSByte( out SByte result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( SByte );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( SByte )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadSByteSlow( header, source, ref offset, out result ) )
				{
					result = default( SByte );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadSByteSlow( ReadValueResult header, byte[] source, ref Int32 offset, out SByte result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( SByte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( SByte );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( SByte )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( SByte )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( SByte )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( SByte )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( SByte )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( SByte )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( SByte )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( SByte )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( SByte )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( SByte )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( SByte ), header );
						// Never
						result = default( SByte );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableSByte( out SByte? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( SByte? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( SByte? )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableSByteSlow( header, source, ref offset, out result ) )
				{
					result = default( SByte? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableSByteSlow( ReadValueResult header, byte[] source, ref Int32 offset, out SByte? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( SByte? );
				offset++;
				return true;
			}

			SByte value;
			if( !this.ReadSByteSlow( header, source, ref offset, out value ) )
			{
				result = default( SByte? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken )
		{
			SByte result;
			return Task.FromResult( this.ReadSByte( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<SByte>() );
		}

		public sealed override Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken )
		{
			SByte? result;
			return Task.FromResult( this.ReadNullableSByte( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<SByte?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt16( out Int16 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int16 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int16 )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadInt16Slow( header, source, ref offset, out result ) )
				{
					result = default( Int16 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadInt16Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int16 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Int16 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int16 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int16 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int16 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int16 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int16 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int16 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int16 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int16 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int16 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int16 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Int16 ), header );
						// Never
						result = default( Int16 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableInt16( out Int16? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int16? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int16? )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableInt16Slow( header, source, ref offset, out result ) )
				{
					result = default( Int16? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableInt16Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int16? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int16? );
				offset++;
				return true;
			}

			Int16 value;
			if( !this.ReadInt16Slow( header, source, ref offset, out value ) )
			{
				result = default( Int16? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken )
		{
			Int16 result;
			return Task.FromResult( this.ReadInt16( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int16>() );
		}

		public sealed override Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken )
		{
			Int16? result;
			return Task.FromResult( this.ReadNullableInt16( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int16?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt16( out UInt16 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt16 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt16 )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadUInt16Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt16 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadUInt16Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt16 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( UInt16 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt16 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt16 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt16 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt16 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt16 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt16 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt16 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt16 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt16 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt16 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( UInt16 ), header );
						// Never
						result = default( UInt16 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableUInt16( out UInt16? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt16? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt16? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableUInt16Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt16? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableUInt16Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt16? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt16? );
				offset++;
				return true;
			}

			UInt16 value;
			if( !this.ReadUInt16Slow( header, source, ref offset, out value ) )
			{
				result = default( UInt16? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken )
		{
			UInt16 result;
			return Task.FromResult( this.ReadUInt16( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt16>() );
		}

		public sealed override Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken )
		{
			UInt16? result;
			return Task.FromResult( this.ReadNullableUInt16( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt16?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt32( out Int32 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int32 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int32 )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadInt32Slow( header, source, ref offset, out result ) )
				{
					result = default( Int32 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadInt32Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int32 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Int32 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int32 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int32 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int32 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int32 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int32 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int32 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int32 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int32 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int32 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int32 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Int32 ), header );
						// Never
						result = default( Int32 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableInt32( out Int32? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int32? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int32? )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableInt32Slow( header, source, ref offset, out result ) )
				{
					result = default( Int32? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableInt32Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int32? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int32? );
				offset++;
				return true;
			}

			Int32 value;
			if( !this.ReadInt32Slow( header, source, ref offset, out value ) )
			{
				result = default( Int32? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken )
		{
			Int32 result;
			return Task.FromResult( this.ReadInt32( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int32>() );
		}

		public sealed override Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken )
		{
			Int32? result;
			return Task.FromResult( this.ReadNullableInt32( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int32?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt32( out UInt32 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt32 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt32 )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadUInt32Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt32 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadUInt32Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt32 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( UInt32 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt32 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt32 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt32 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt32 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt32 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt32 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt32 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt32 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt32 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt32 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( UInt32 ), header );
						// Never
						result = default( UInt32 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableUInt32( out UInt32? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt32? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt32? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableUInt32Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt32? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableUInt32Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt32? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt32? );
				offset++;
				return true;
			}

			UInt32 value;
			if( !this.ReadUInt32Slow( header, source, ref offset, out value ) )
			{
				result = default( UInt32? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken )
		{
			UInt32 result;
			return Task.FromResult( this.ReadUInt32( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt32>() );
		}

		public sealed override Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken )
		{
			UInt32? result;
			return Task.FromResult( this.ReadNullableUInt32( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt32?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt64( out Int64 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int64 )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadInt64Slow( header, source, ref offset, out result ) )
				{
					result = default( Int64 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadInt64Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int64 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Int64 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int64 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int64 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int64 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int64 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int64 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int64 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int64 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int64 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int64 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int64 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Int64 ), header );
						// Never
						result = default( Int64 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableInt64( out Int64? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int64? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				result = ( Int64? )immediateValue;
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableInt64Slow( header, source, ref offset, out result ) )
				{
					result = default( Int64? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableInt64Slow( ReadValueResult header, byte[] source, ref Int32 offset, out Int64? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int64? );
				offset++;
				return true;
			}

			Int64 value;
			if( !this.ReadInt64Slow( header, source, ref offset, out value ) )
			{
				result = default( Int64? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken )
		{
			Int64 result;
			return Task.FromResult( this.ReadInt64( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int64>() );
		}

		public sealed override Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken )
		{
			Int64? result;
			return Task.FromResult( this.ReadNullableInt64( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int64?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt64( out UInt64 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt64 )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadUInt64Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt64 );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadUInt64Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt64 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( UInt64 );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt64 )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt64 )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt64 )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt64 )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt64 )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt64 )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt64 )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt64 )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt64 )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt64 )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( UInt64 ), header );
						// Never
						result = default( UInt64 );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableUInt64( out UInt64? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( UInt64? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( UInt64? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableUInt64Slow( header, source, ref offset, out result ) )
				{
					result = default( UInt64? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableUInt64Slow( ReadValueResult header, byte[] source, ref Int32 offset, out UInt64? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt64? );
				offset++;
				return true;
			}

			UInt64 value;
			if( !this.ReadUInt64Slow( header, source, ref offset, out value ) )
			{
				result = default( UInt64? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken )
		{
			UInt64 result;
			return Task.FromResult( this.ReadUInt64( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt64>() );
		}

		public sealed override Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken )
		{
			UInt64? result;
			return Task.FromResult( this.ReadNullableUInt64( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<UInt64?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadSingle( out Single result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Single );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Single )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadSingleSlow( header, source, ref offset, out result ) )
				{
					result = default( Single );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadSingleSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Single result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Single ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Single );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Single )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Single )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Single )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Single )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Single )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Single )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Single )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Single )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Single )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Single )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Single ), header );
						// Never
						result = default( Single );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableSingle( out Single? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Single? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Single? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableSingleSlow( header, source, ref offset, out result ) )
				{
					result = default( Single? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableSingleSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Single? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Single? );
				offset++;
				return true;
			}

			Single value;
			if( !this.ReadSingleSlow( header, source, ref offset, out value ) )
			{
				result = default( Single? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken )
		{
			Single result;
			return Task.FromResult( this.ReadSingle( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Single>() );
		}

		public sealed override Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken )
		{
			Single? result;
			return Task.FromResult( this.ReadNullableSingle( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Single?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadDouble( out Double result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Double );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Double )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadDoubleSlow( header, source, ref offset, out result ) )
				{
					result = default( Double );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadDoubleSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Double result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Double ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			if ( source.Length - offset < length )
			{
				result = default( Double );
				return false;
			}

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Double )source[ offset ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Double )BigEndianBinary.ToUInt16( source, offset );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Double )BigEndianBinary.ToUInt32( source, offset );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Double )BigEndianBinary.ToUInt64( source, offset );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Double )( unchecked( ( SByte )source[ offset ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Double )BigEndianBinary.ToInt16( source, offset );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Double )BigEndianBinary.ToInt32( source, offset );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Double )BigEndianBinary.ToInt64( source, offset );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Double )BigEndianBinary.ToSingle( source, offset );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Double )BigEndianBinary.ToDouble( source, offset );
						break;
					}
					default:
					{
						this.ThrowTypeException( typeof( Double ), header );
						// Never
						result = default( Double );
						break;
					}
				} // switch
			} // checked

			offset += length;
			return true;
		}

		public sealed override bool ReadNullableDouble( out Double? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Double? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];

			// Check immediate
			if ( ( header & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				var immediateValue = unchecked( ( sbyte )( byte )( header & ReadValueResult.ValueOrLengthMask ) );
				// Check sign
				result = checked( ( Double? )immediateValue );
				offset++;
				// goto tail
			}
			else
			{
				// Slow path
				if ( !this.ReadNullableDoubleSlow( header, source, ref offset, out result ) )
				{
					result = default( Double? );
					return false;
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadNullableDoubleSlow( ReadValueResult header, byte[] source, ref Int32 offset, out Double? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Double? );
				offset++;
				return true;
			}

			Double value;
			if( !this.ReadDoubleSlow( header, source, ref offset, out value ) )
			{
				result = default( Double? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			Double result;
			return Task.FromResult( this.ReadDouble( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Double>() );
		}

		public sealed override Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken )
		{
			Double? result;
			return Task.FromResult( this.ReadNullableDouble( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Double?>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadBoolean( out Boolean result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Boolean );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			switch ( header )
			{
				case ReadValueResult.True:
				{
					result = true;
					break;
				}
				case ReadValueResult.False:
				{
					result = false;
					break;
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), header );
					// never
					result = false;
					break;
				}
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		} // if ( isAsync && isPseudoAsync )

		public sealed override bool ReadNullableBoolean( out Boolean? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Boolean? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			switch ( header )
			{
				case ReadValueResult.Nil:
				{
					result = default( bool? );
					break;
				}
				case ReadValueResult.True:
				{
					result = true;
					break;
				}
				case ReadValueResult.False:
				{
					result = false;
					break;
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean? ), header );
					// never
					result = false;
					break;
				}
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		} // if ( isAsync && isPseudoAsync )

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken )
		{
			Boolean result;
			return Task.FromResult( this.ReadBoolean( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Boolean>() );
		} // if ( isAsync && isPseudoAsync )

		public sealed override Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken )
		{
			Boolean? result;
			return Task.FromResult( this.ReadNullableBoolean( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Boolean?>() );
		} // if ( isAsync && isPseudoAsync )

#endif // FEATURE_TAP

		public sealed override bool ReadBinary( out Byte[] result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Byte[] );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			// Byte[] can be null.
			if ( header == ReadValueResult.Nil )
			{
				result = null;
				this._offset = offset;
				this._collectionType = CollectionType.None;
				return true;
			}

			// Check type
			if ( ( header & ReadValueResult.RawTypeMask ) != ReadValueResult.RawTypeMask )
			{
				this.ThrowTypeException( "raw", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Raw8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( Byte[] );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Raw16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( Byte[] );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( Byte[] );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			if( !this.ReadBinaryCore( unchecked( ( int )length ), ref offset, out result ) )
			{
				result = default( Byte[] );
				return false;
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Byte[]>> ReadBinaryAsync( CancellationToken cancellationToken )
		{
			Byte[] result;
			return Task.FromResult( this.ReadBinary( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Byte[]>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadString( out String result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( String );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			// String can be null.
			if ( header == ReadValueResult.Nil )
			{
				result = null;
				this._offset = offset;
				this._collectionType = CollectionType.None;
				return true;
			}

			// Check type
			if ( ( header & ReadValueResult.RawTypeMask ) != ReadValueResult.RawTypeMask )
			{
				this.ThrowTypeException( "raw", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Raw8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( String );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Raw16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( String );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( String );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			if( !this.ReadStringCore( unchecked( ( int )length ), ref offset, out result ) )
			{
				result = default( String );
				return false;
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<String>> ReadStringAsync( CancellationToken cancellationToken )
		{
			String result;
			return Task.FromResult( this.ReadString( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<String>() );
		}

#endif // FEATURE_TAP

		private bool ReadObject( bool isDeep, out MessagePackObject result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( !this.ReadObjectCore( isDeep, source, ref offset, out result ) )
			{
				result = default( MessagePackObject );
				return false;
			}
			this._offset = offset;
			return true;
		}

		private bool ReadObjectCore( bool isDeep, byte[] source, ref Int32 offset, out MessagePackObject result )
		{
			if ( source.Length - offset < 1 )
			{
				result = default( MessagePackObject );
				return false;
			}

			var byteHeader = source[ offset ];
			offset++;
			var collectionType = ReadValueResults.CollectionType[ byteHeader ];

			if ( ReadValueResults.HasConstantObject[ byteHeader ] )
			{
				result = ReadValueResults.ContantObject[ byteHeader ];
			}
			else
			{
				var header = ReadValueResults.EncodedTypes[ byteHeader ];

				if ( ( header & ReadValueResult.RawTypeMask ) == ReadValueResult.RawTypeMask && ( header & ReadValueResult.LengthOfLengthMask ) == 0 )
				{
					// fixed raw
					int length = ( int )( header & ReadValueResult.ValueOrLengthMask );

					byte[] binary;
					if ( !this.ReadBinaryCore( length, ref offset, out binary ) )
					{
						result = default( MessagePackObject );
						return false;
					}

					result = binary;
				}
				else
				{
					if ( !this.ReadObjectSlow( header, source, ref offset, out result ) )
					{
						result = default( MessagePackObject );
						return false;
					}
				}
			}

			if ( isDeep && collectionType != CollectionType.None )
			{
				if ( !this.ReadItems( result.AsInt32(), collectionType == CollectionType.Map, source, ref offset, out result ) )
				{
					result = default( MessagePackObject );
					return false;
				}
			}

			this._data = result;
			this._collectionType = collectionType;
			return true;
		}

		private bool ReadObjectSlow( ReadValueResult header, byte[] source, ref Int32 offset, out MessagePackObject result )
		{
			switch ( header & ReadValueResult.TypeCodeMask )
			{
				case ReadValueResult.Array16Type:
				case ReadValueResult.Map16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToUInt16( source, offset );
					offset += 2;
					break;
				}
				case ReadValueResult.Array32Type:
				case ReadValueResult.Map32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToUInt32( source, offset );
					offset += 4;
					break;
				}
				case ReadValueResult.Str8Type:
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = source[ offset ];
					this.CheckLength( length, header );
					offset += 1;
					MessagePackString stringValue;
					if( !this.ReadRawStringCore( unchecked( ( int )length ), ref offset, out stringValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Str16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = BigEndianBinary.ToUInt16( source, offset );
					this.CheckLength( length, header );
					offset += 2;
					MessagePackString stringValue;
					if( !this.ReadRawStringCore( unchecked( ( int )length ), ref offset, out stringValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Str32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = BigEndianBinary.ToUInt32( source, offset );
					this.CheckLength( length, header );
					offset += 4;
					MessagePackString stringValue;
					if( !this.ReadRawStringCore( unchecked( ( int )length ), ref offset, out stringValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Bin8Type:
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = source[ offset ];
					this.CheckLength( length, header );
					offset += 1;
					byte[] binaryValue;
					if( !this.ReadBinaryCore( unchecked( ( int )length ), ref offset, out binaryValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.Bin16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = BigEndianBinary.ToUInt16( source, offset );
					this.CheckLength( length, header );
					offset += 2;
					byte[] binaryValue;
					if( !this.ReadBinaryCore( unchecked( ( int )length ), ref offset, out binaryValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.Bin32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = BigEndianBinary.ToUInt32( source, offset );
					this.CheckLength( length, header );
					offset += 4;
					byte[] binaryValue;
					if( !this.ReadBinaryCore( unchecked( ( int )length ), ref offset, out binaryValue ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.FixExtType:
				{
					var length = ( header & ReadValueResult.ValueOrLengthMask );
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext8Type:
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					var length = source[ offset ];
					offset += 1;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}
					var length = BigEndianBinary.ToUInt16( source, offset );
					offset += 2;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}
					var length = BigEndianBinary.ToUInt32( source, offset );
					offset += 4;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Int8Type:
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = unchecked( ( sbyte )source[ offset ] );
					offset += 1;
					break;
				}
				case ReadValueResult.Int16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToInt16( source, offset );
					offset += 2;
					break;
				}
				case ReadValueResult.Int32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToInt32( source, offset );
					offset += 4;
					break;
				}
				case ReadValueResult.Int64Type:
				{
					if ( source.Length - offset < 8 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToInt64( source, offset );
					offset += 8;
					break;
				}
				case ReadValueResult.UInt8Type:
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = source[ offset ];
					offset += 1;
					break;
				}
				case ReadValueResult.UInt16Type:
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToUInt16( source, offset );
					offset += 2;
					break;
				}
				case ReadValueResult.UInt32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToUInt32( source, offset );
					offset += 4;
					break;
				}
				case ReadValueResult.UInt64Type:
				{
					if ( source.Length - offset < 8 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToUInt64( source, offset );
					offset += 8;
					break;
				}
				case ReadValueResult.Real32Type:
				{
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToSingle( source, offset );
					offset += 4;
					break;
				}
				case ReadValueResult.Real64Type:
				{
					if ( source.Length - offset < 8 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = BigEndianBinary.ToDouble( source, offset );
					offset += 8;
					break;
				}
				default:
				{
#if DEBUG
					Contract.Assert( header == ReadValueResult.InvalidCode, header.ToString( "X" ) + " == ReadValueResult.InvalidCode" );
#endif // DEBUG
					this.ThrowUnassignedMessageTypeException( 0xC1 );
					// never
					result = default( MessagePackObject );
					break;
				}
			}

			return true;
		}

		private bool ReadItems( int count, bool isMap, byte[] source, ref Int32 offset, out MessagePackObject result )
		{
			MessagePackObject container;
			if ( !isMap )
			{
				var array = new MessagePackObject[ count ];
				for ( var i = 0; i < count; i++ )
				{
					MessagePackObject item;
					if ( !this.ReadObjectCore( true, source, ref offset, out item ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					array[ i ] = item;
				}

				container = new MessagePackObject( array, true );
			}
			else
			{
				var map = new MessagePackObjectDictionary( count );

				for ( var i = 0; i < count; i++ )
				{
					MessagePackObject key;
					if ( !this.ReadObjectCore( true, source, ref offset, out key ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					MessagePackObject value;
					if ( !this.ReadObjectCore( true, source, ref offset, out value ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					map.Add( key, value );
				}

				container = new MessagePackObject( map, true );
			}
			result = container;
			return true;
		}

#if FEATURE_TAP

		private Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( bool isDeep, CancellationToken cancellationToken )
		{
			MessagePackObject result;
			return Task.FromResult( this.ReadObject( isDeep, out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<MessagePackObject>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadArrayLength( out Int64 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			// Check type
			if ( ( header & ReadValueResult.ArrayTypeMask ) != ReadValueResult.ArrayTypeMask )
			{
				this.ThrowTypeException( "array", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Array8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Array16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Array32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			result = length;
			this._data = length;
			this._offset = offset;
			this._collectionType = CollectionType.Array;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Int64>> ReadArrayLengthAsync( CancellationToken cancellationToken )
		{
			Int64 result;
			return Task.FromResult( this.ReadArrayLength( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int64>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadMapLength( out Int64 result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			// Check type
			if ( ( header & ReadValueResult.MapTypeMask ) != ReadValueResult.MapTypeMask )
			{
				this.ThrowTypeException( "map", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Map8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Map16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Map32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			result = length;
			this._data = length;
			this._offset = offset;
			this._collectionType = CollectionType.Map;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<Int64>> ReadMapLengthAsync( CancellationToken cancellationToken )
		{
			Int64 result;
			return Task.FromResult( this.ReadMapLength( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<Int64>() );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			offset++;

			// Check type
			if ( ( header & ReadValueResult.ExtTypeMask ) != ReadValueResult.ExtTypeMask )
			{
				this.ThrowTypeException( "ext", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Ext8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackExtendedTypeObject );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Ext16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackExtendedTypeObject );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackExtendedTypeObject );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out result ) )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadMessagePackExtendedTypeObjectCore( int length, byte[] source, ref Int32 offset, out MessagePackExtendedTypeObject result )
		{
			// Read type code
			if ( source.Length - offset < 1 )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			var typeCode = source[ offset ];
			offset++;

			// Read body
			byte[] body;
			if ( !this.ReadBinaryCore( length, ref offset, out body ) )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			result = MessagePackExtendedTypeObject.Unpack( typeCode, body );
			return true;
		}

		public sealed override bool ReadNullableMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject? result )
		{
			var source = this._source;
			var offset = this._offset;
			if ( source.Length - offset < 1 )
			{
				result = default( MessagePackExtendedTypeObject? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ source[ offset ] ];
			if ( header == ReadValueResult.Nil )
			{
				result = default( MessagePackExtendedTypeObject? );
				return true;
			}
			offset++;

			// Check type
			if ( ( header & ReadValueResult.ExtTypeMask ) != ReadValueResult.ExtTypeMask )
			{
				this.ThrowTypeException( "ext", header );
			}

			// Get length
			var lengthOfLength = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			uint length;
			switch ( lengthOfLength )
			{
				case 0:
				{
					length = ( byte )( header & ReadValueResult.ValueOrLengthMask );
					break;
				}
				case 1: // Ext8
				{
					if ( source.Length - offset < 1 )
					{
						result = default( MessagePackExtendedTypeObject? );
						return false;
					}

					length = BigEndianBinary.ToByte( source, offset );

					break;
				}
				case 2: // Ext16
				{
					if ( source.Length - offset < 2 )
					{
						result = default( MessagePackExtendedTypeObject? );
						return false;
					}

					length = BigEndianBinary.ToUInt16( source, offset );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					if ( source.Length - offset < 4 )
					{
						result = default( MessagePackExtendedTypeObject? );
						return false;
					}

					length = BigEndianBinary.ToUInt32( source, offset );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			MessagePackExtendedTypeObject value;
			if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), source, ref offset, out value ) )
			{
				result = default( MessagePackExtendedTypeObject? );
				return false;
			}
			
			result = value;
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			MessagePackExtendedTypeObject result;
			return Task.FromResult( this.ReadMessagePackExtendedTypeObject( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<MessagePackExtendedTypeObject>() );
		}

		public sealed override Task<AsyncReadResult<MessagePackExtendedTypeObject?>> ReadNullableMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			MessagePackExtendedTypeObject? result;
			return Task.FromResult( this.ReadNullableMessagePackExtendedTypeObject( out result ) ? AsyncReadResult.Success( result ) : AsyncReadResult.Fail<MessagePackExtendedTypeObject?>() );
		}

#endif // FEATURE_TAP

		protected sealed override long? SkipCore()
		{
			var startOffset = this._offset;
			MessagePackObject notUsed;
			if ( !this.ReadObject( /* isDeep */true, out notUsed ) )
			{
				return null;
			}

			return this._offset - startOffset;
		}

#if FEATURE_TAP
		protected sealed override async Task<long?> SkipAsyncCore( CancellationToken cancellationToken )
		{
			var startOffset = this._offset;
			var asyncReadResult = await this.ReadObjectAsync( /* isDeep */true, cancellationToken ).ConfigureAwait( false );
			if ( !asyncReadResult.Success )
			{
				return null;
			}

			return this._offset - startOffset;
		}

#endif // FEATURE_TAP
		public sealed override bool ReadObject( out MessagePackObject result )
		{
			return this.ReadObject( /* isDeep*/true, out result );
		}

#if FEATURE_TAP

		public sealed override Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( CancellationToken cancellationToken )
		{
			return this.ReadObjectAsync( /* isDeep*/true,  cancellationToken );
		}

#endif // FEATURE_TAP

		protected sealed override bool ReadCore()
		{
			MessagePackObject value;
			var success = this.ReadObject( /* isDeep */false, out value );
			if ( success )
			{
				this._data = value;
				return true;
			}
			else
			{
				return false;
			}
		}

#if FEATURE_TAP

		protected sealed override async Task<bool> ReadAsyncCore( CancellationToken cancellationToken )
		{
			var result = await this.ReadObjectAsync( /* isDeep */false, cancellationToken ).ConfigureAwait( false );
			if ( result.Success )
			{
				this._data = result.Value;
				return true;
			}
			else
			{
				return false;
			}
		}

#endif // FEATURE_TAP

		private void ThrowUnassignedMessageTypeException( int header )
		{
#if DEBUG
			Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
#endif // DEBUG
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new UnassignedMessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
						? "Unknown header value 0x{0:X} at position {1:#,0}"
						: "Unknown header value 0x{0:X} at offset {1:#,0}",
					header,
					offsetOrPosition
				)
			);
		}

		private void CheckLength( uint length, ReadValueResult type )
		{
			if ( length > Int32.MaxValue )
			{
				this.ThrowTooLongLengthException( length, type );
			}
		}

		private void ThrowTooLongLengthException( uint length, ReadValueResult type )
		{
			string message;
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );

			if ( ( type & ReadValueResult.ArrayTypeMask ) == ReadValueResult.ArrayTypeMask )
			{
				message =
					isRealOffset
					? "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at position {1:#,0}"
					: "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at offset {1:#,0}";
			}
			else if ( ( type & ReadValueResult.MapTypeMask ) == ReadValueResult.MapTypeMask )
			{
				message =
					isRealOffset
					? "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at position {1:#,0}"
					: "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at offset {1:#,0}";
			}
			else if ( ( type & ReadValueResult.ExtTypeMask ) == ReadValueResult.ExtTypeMask )
			{
				message =
					isRealOffset
					? "MessagePack for CLI cannot handle large ext type (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at position {1:#,0}"
					: "MessagePack for CLI cannot handle large ext type (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at offset {1:#,0}";
			}
			else
			{
				message =
					isRealOffset
					? "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at position {1:#,0}"
					: "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at offset {1:#,0}";
			}

			throw new MessageNotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					message,
					length,
					offsetOrPosition
				)
			);
		}

		private void ThrowTypeException( string type, ReadValueResult header )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			var byteHeader = header.ToByte();
			throw new MessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
					? "Cannot convert '{0}' header from type '{2}'(0x{1:X}) in position {3:#,0}."
					: "Cannot convert '{0}' header from type '{2}'(0x{1:X}) in offset {3:#,0}.",
					type,
					byteHeader,
					MessagePackCode.ToString( byteHeader ),
					offsetOrPosition
				)
			);
		}

		private void ThrowTypeException( Type type, ReadValueResult header )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			var byteHeader = header.ToByte();
			throw new MessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
					? "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in position {3:#,0}."
					: "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in offset {3:#,0}.",
					type,
					byteHeader,
					MessagePackCode.ToString( byteHeader ),
					offsetOrPosition
				)
			);
		}
	}
}
