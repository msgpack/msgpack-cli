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
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

#if !UNITY || MSGPACK_UNITY_FULL
using Int64Stack = System.Collections.Generic.Stack<System.Int64>;
#endif // !UNITY || MSGPACK_UNITY_FULL

namespace MsgPack
{
	// This file was generated from MessagePackStreamUnpacker.Unpack.tt and MessagePackUnpackerCommon.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackStreamUnpacker.Unpack.tt and MessagePackUnpackerCommon.ttinclude instead.

	partial class MessagePackStreamUnpacker
	{
		public sealed override bool ReadByte( out Byte result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Byte );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadByteSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadByteSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Byte result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Byte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Byte );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Byte )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Byte )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Byte )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Byte )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Byte )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Byte )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Byte )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Byte )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Byte )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Byte )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Byte? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableByteSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableByteSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Byte? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Byte? );
				offset++;
				return true;
			}

			Byte value;
			if( !this.ReadByteSlow( header, buffer, ref offset, out value ) )
			{
				result = default( Byte? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken )
		{
			Byte result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Byte>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Byte>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Byte>>> ReadByteSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Byte result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Byte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Byte>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Byte )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Byte )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Byte )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Byte )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Byte )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Byte )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Byte )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Byte )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Byte )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Byte )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken )
		{
			Byte? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Byte?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Byte?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Byte?>>> ReadNullableByteSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Byte? ), offset + 1 );
			}

			Byte value;
			var asyncReadResult = await this.ReadByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Byte?>>();
			}

			return AsyncReadResult.Success( ( Byte? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadSByte( out SByte result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( SByte );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadSByteSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadSByteSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out SByte result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( SByte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( SByte );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( SByte )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( SByte )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( SByte )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( SByte )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( SByte )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( SByte )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( SByte )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( SByte )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( SByte )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( SByte )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( SByte? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableSByteSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableSByteSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out SByte? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( SByte? );
				offset++;
				return true;
			}

			SByte value;
			if( !this.ReadSByteSlow( header, buffer, ref offset, out value ) )
			{
				result = default( SByte? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken )
		{
			SByte result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<SByte>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadSByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<SByte>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<SByte>>> ReadSByteSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			SByte result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( SByte ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<SByte>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( SByte )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( SByte )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( SByte )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( SByte )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( SByte )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( SByte )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( SByte )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( SByte )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( SByte )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( SByte )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken )
		{
			SByte? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<SByte?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableSByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<SByte?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<SByte?>>> ReadNullableSByteSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( SByte? ), offset + 1 );
			}

			SByte value;
			var asyncReadResult = await this.ReadSByteSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<SByte?>>();
			}

			return AsyncReadResult.Success( ( SByte? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt16( out Int16 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int16 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadInt16Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadInt16Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int16 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Int16 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int16 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int16 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int16 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int16 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int16 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int16 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int16 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int16 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int16 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int16? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableInt16Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableInt16Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int16? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int16? );
				offset++;
				return true;
			}

			Int16 value;
			if( !this.ReadInt16Slow( header, buffer, ref offset, out value ) )
			{
				result = default( Int16? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken )
		{
			Int16 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int16>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int16>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int16>>> ReadInt16SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Int16 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Int16>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int16 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int16 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int16 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int16 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int16 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int16 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int16 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int16 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int16 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken )
		{
			Int16? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int16?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int16?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int16?>>> ReadNullableInt16SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Int16? ), offset + 1 );
			}

			Int16 value;
			var asyncReadResult = await this.ReadInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Int16?>>();
			}

			return AsyncReadResult.Success( ( Int16? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt16( out UInt16 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt16 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadUInt16Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadUInt16Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt16 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( UInt16 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt16 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt16 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt16 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt16 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt16 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt16 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt16 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt16 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt16 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt16? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableUInt16Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableUInt16Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt16? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt16? );
				offset++;
				return true;
			}

			UInt16 value;
			if( !this.ReadUInt16Slow( header, buffer, ref offset, out value ) )
			{
				result = default( UInt16? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken )
		{
			UInt16 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt16>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadUInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt16>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt16>>> ReadUInt16SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			UInt16 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt16 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<UInt16>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt16 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt16 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt16 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt16 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt16 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt16 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt16 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt16 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt16 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken )
		{
			UInt16? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt16?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableUInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt16?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt16?>>> ReadNullableUInt16SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( UInt16? ), offset + 1 );
			}

			UInt16 value;
			var asyncReadResult = await this.ReadUInt16SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<UInt16?>>();
			}

			return AsyncReadResult.Success( ( UInt16? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt32( out Int32 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int32 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadInt32Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadInt32Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int32 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Int32 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int32 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int32 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int32 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int32 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int32 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int32 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int32 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int32 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int32? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableInt32Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableInt32Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int32? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int32? );
				offset++;
				return true;
			}

			Int32 value;
			if( !this.ReadInt32Slow( header, buffer, ref offset, out value ) )
			{
				result = default( Int32? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken )
		{
			Int32 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int32>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int32>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int32>>> ReadInt32SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Int32 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Int32>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int32 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int32 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int32 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int32 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int32 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int32 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int32 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int32 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken )
		{
			Int32? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int32?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int32?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int32?>>> ReadNullableInt32SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Int32? ), offset + 1 );
			}

			Int32 value;
			var asyncReadResult = await this.ReadInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Int32?>>();
			}

			return AsyncReadResult.Success( ( Int32? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt32( out UInt32 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt32 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadUInt32Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadUInt32Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt32 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( UInt32 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt32 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt32 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt32 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt32 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt32 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt32 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt32? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableUInt32Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableUInt32Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt32? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt32? );
				offset++;
				return true;
			}

			UInt32 value;
			if( !this.ReadUInt32Slow( header, buffer, ref offset, out value ) )
			{
				result = default( UInt32? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken )
		{
			UInt32 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt32>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadUInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt32>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt32>>> ReadUInt32SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			UInt32 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt32 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<UInt32>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt32 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt32 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt32 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt32 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt32 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt32 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken )
		{
			UInt32? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt32?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableUInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt32?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt32?>>> ReadNullableUInt32SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( UInt32? ), offset + 1 );
			}

			UInt32 value;
			var asyncReadResult = await this.ReadUInt32SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<UInt32?>>();
			}

			return AsyncReadResult.Success( ( UInt32? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadInt64( out Int64 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadInt64Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadInt64Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int64 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Int64 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int64 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int64 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int64 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int64 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int64 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int64 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int64 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int64? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableInt64Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableInt64Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Int64? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Int64? );
				offset++;
				return true;
			}

			Int64 value;
			if( !this.ReadInt64Slow( header, buffer, ref offset, out value ) )
			{
				result = default( Int64? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken )
		{
			Int64 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int64>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int64>>> ReadInt64SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Int64 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Int64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Int64>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Int64 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Int64 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Int64 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Int64 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Int64 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Int64 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Int64 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken )
		{
			Int64? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int64?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Int64?>>> ReadNullableInt64SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Int64? ), offset + 1 );
			}

			Int64 value;
			var asyncReadResult = await this.ReadInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Int64?>>();
			}

			return AsyncReadResult.Success( ( Int64? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadUInt64( out UInt64 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadUInt64Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadUInt64Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt64 result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( UInt64 );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt64 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt64 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt64 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( UInt64? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableUInt64Slow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableUInt64Slow( ReadValueResult header, byte[] buffer, ref Int64 offset, out UInt64? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( UInt64? );
				offset++;
				return true;
			}

			UInt64 value;
			if( !this.ReadUInt64Slow( header, buffer, ref offset, out value ) )
			{
				result = default( UInt64? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken )
		{
			UInt64 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt64>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadUInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt64>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt64>>> ReadUInt64SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			UInt64 result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( UInt64 ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<UInt64>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( UInt64 )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( UInt64 )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( UInt64 )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken )
		{
			UInt64? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<UInt64?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableUInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<UInt64?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<UInt64?>>> ReadNullableUInt64SlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( UInt64? ), offset + 1 );
			}

			UInt64 value;
			var asyncReadResult = await this.ReadUInt64SlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<UInt64?>>();
			}

			return AsyncReadResult.Success( ( UInt64? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadSingle( out Single result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Single );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadSingleSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadSingleSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Single result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Single ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Single );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Single )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Single )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Single )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Single )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Single )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Single )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Single )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Single )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Single )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Single )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Single? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableSingleSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableSingleSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Single? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Single? );
				offset++;
				return true;
			}

			Single value;
			if( !this.ReadSingleSlow( header, buffer, ref offset, out value ) )
			{
				result = default( Single? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken )
		{
			Single result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Single>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadSingleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Single>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Single>>> ReadSingleSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Single result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Single ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Single>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Single )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Single )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Single )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Single )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Single )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Single )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Single )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Single )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Single )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Single )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken )
		{
			Single? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Single?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableSingleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Single?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Single?>>> ReadNullableSingleSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Single? ), offset + 1 );
			}

			Single value;
			var asyncReadResult = await this.ReadSingleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Single?>>();
			}

			return AsyncReadResult.Success( ( Single? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadDouble( out Double result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Double );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadDoubleSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadDoubleSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Double result )
		{
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Double ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = this._source.Read( buffer, bufferOffset, reading );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							result = default( Double );
							return false;
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Double )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Double )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Double )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Double )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Double )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Double )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Double )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Double )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Double )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Double )BigEndianBinary.ToDouble( buffer, 0 );
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Double? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				if ( !this.ReadNullableDoubleSlow( header, buffer, ref offset, out result ) )
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

		private bool ReadNullableDoubleSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out Double? result )
		{
			if ( header == ReadValueResult.Nil )
			{
				result = default( Double? );
				offset++;
				return true;
			}

			Double value;
			if( !this.ReadDoubleSlow( header, buffer, ref offset, out value ) )
			{
				result = default( Double? );
				return false;
			}

			result = value;
			return true;
		}

#if FEATURE_TAP

		public sealed override async Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			Double result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Double>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadDoubleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Double>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Double>>> ReadDoubleSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			Double result;
			// Check type
			if ( ( header & ReadValueResult.NonScalarBitMask ) != 0 )
			{
				this.ThrowTypeException( typeof( Double ), header );
			}

			offset++;
			var length = ( int )( header & ReadValueResult.LengthOfLengthMask ) >> 8;
			// scope for local
			{
				var bufferOffset = 0;
				var reading = length;
				// Retrying for splitted Stream such as NetworkStream
				while( true )
				{
					var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
					if ( readLength < reading )
					{
						if ( readLength > 0 )
						{
							// retry reading
							bufferOffset += readLength;
							reading -= readLength;
							continue;
						}
						else
						{
							if ( this._useStreamPosition )
							{
								// Rollback
								this._source.Position -= ( bufferOffset + readLength );
							}
							else
							{
								// Throw because rollback is not available
								this.ThrowEofException( reading );
							}
			
							return AsyncReadResult.Fail<Int64OffsetValue<Double>>();
						}
					} // if readLength < reading
			
					break;
				} // while true
			} // scope for local

			checked
			{
				switch( ( int )( header & ReadValueResult.TypeCodeMask ) )
				{
					case 0x100: // Byte
					{
						result = ( Double )buffer[ 0 ];
						break;
					}
					case 0x200: // UInt16
					{
						result = ( Double )BigEndianBinary.ToUInt16( buffer, 0 );
						break;
					}
					case 0x400: // UInt32
					{
						result = ( Double )BigEndianBinary.ToUInt32( buffer, 0 );
						break;
					}
					case 0x800: // UInt64
					{
						result = ( Double )BigEndianBinary.ToUInt64( buffer, 0 );
						break;
					}
					case 0x1100: // SByte
					{
						result = ( Double )( unchecked( ( SByte )buffer[ 0 ] ) );
						break;
					}
					case 0x1200: // Int16
					{
						result = ( Double )BigEndianBinary.ToInt16( buffer, 0 );
						break;
					}
					case 0x1400: // Int32
					{
						result = ( Double )BigEndianBinary.ToInt32( buffer, 0 );
						break;
					}
					case 0x1800: // Int64
					{
						result = ( Double )BigEndianBinary.ToInt64( buffer, 0 );
						break;
					}
					case 0x2400: // Single
					{
						result = ( Double )BigEndianBinary.ToSingle( buffer, 0 );
						break;
					}
					case 0x2800: // Double
					{
						result = ( Double )BigEndianBinary.ToDouble( buffer, 0 );
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
			return AsyncReadResult.Success( result, offset );
		}

		public sealed override async Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken )
		{
			Double? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Double?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];

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
				var slowAsyncResult = await this.ReadNullableDoubleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( slowAsyncResult.Success )
				{
					result = slowAsyncResult.Value.Result;
					offset = slowAsyncResult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Double?>();
				}
				// goto tail
			}

			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<Double?>>> ReadNullableDoubleSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( Double? ), offset + 1 );
			}

			Double value;
			var asyncReadResult = await this.ReadDoubleSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				value = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<Double?>>();
			}

			return AsyncReadResult.Success( ( Double? )value, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadBoolean( out Boolean result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Boolean );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Boolean? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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

		public sealed override async Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken )
		{
			Boolean result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Boolean>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
			return AsyncReadResult.Success( result );
		} // if ( isAsync && isPseudoAsync )

		public sealed override async Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken )
		{
			Boolean? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Boolean?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
			return AsyncReadResult.Success( result );
		} // if ( isAsync && isPseudoAsync )

#endif // FEATURE_TAP

		public sealed override bool ReadBinary( out Byte[] result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Byte[] );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( Byte[] );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Raw16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Byte[] );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Byte[] );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

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

		public sealed override async Task<AsyncReadResult<Byte[]>> ReadBinaryAsync( CancellationToken cancellationToken )
		{
			Byte[] result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Byte[]>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
			offset++;

			// Byte[] can be null.
			if ( header == ReadValueResult.Nil )
			{
				result = null;
				this._offset = offset;
				this._collectionType = CollectionType.None;
				return AsyncReadResult.Success( result );
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Byte[]>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Raw16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Byte[]>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Byte[]>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			var asyncReadResult = await this.ReadBinaryCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				offset = asyncReadResult.Value.Offset;
				result = asyncReadResult.Value.Result;
			}
			else
			{
				return AsyncReadResult.Fail<Byte[]>();
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadString( out String result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( String );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( String );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Raw16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( String );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( String );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

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

		public sealed override async Task<AsyncReadResult<String>> ReadStringAsync( CancellationToken cancellationToken )
		{
			String result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<String>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
			offset++;

			// String can be null.
			if ( header == ReadValueResult.Nil )
			{
				result = null;
				this._offset = offset;
				this._collectionType = CollectionType.None;
				return AsyncReadResult.Success( result );
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<String>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Raw16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<String>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Raw32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<String>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			var asyncReadResult = await this.ReadStringCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				offset = asyncReadResult.Value.Offset;
				result = asyncReadResult.Value.Result;
			}
			else
			{
				return AsyncReadResult.Fail<String>();
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

#endif // FEATURE_TAP

		private bool ReadObject( bool isDeep, out MessagePackObject result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( !this.ReadObjectCore( isDeep, buffer, ref offset, out result ) )
			{
				result = default( MessagePackObject );
				return false;
			}
			this._offset = offset;
			return true;
		}

		private bool ReadObjectCore( bool isDeep, byte[] buffer, ref Int64 offset, out MessagePackObject result )
		{
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( MessagePackObject );
				return false;
			}

			var byteHeader = buffer[ 0 ];
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
					if ( !this.ReadObjectSlow( header, buffer, ref offset, out result ) )
					{
						result = default( MessagePackObject );
						return false;
					}
				}
			}

			if ( isDeep && collectionType != CollectionType.None )
			{
				if ( !this.ReadItems( result.AsInt32(), collectionType == CollectionType.Map, buffer, ref offset, out result ) )
				{
					result = default( MessagePackObject );
					return false;
				}
			}

			this._data = result;
			this._collectionType = collectionType;
			return true;
		}

		private bool ReadObjectSlow( ReadValueResult header, byte[] buffer, ref Int64 offset, out MessagePackObject result )
		{
			switch ( header & ReadValueResult.TypeCodeMask )
			{
				case ReadValueResult.Array16Type:
				case ReadValueResult.Map16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.Array32Type:
				case ReadValueResult.Map32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Str8Type:
				{
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = buffer[ 0 ];
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
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt16( buffer, 0 );
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
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt32( buffer, 0 );
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}

					var length = buffer[ 0 ];
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
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt16( buffer, 0 );
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
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt32( buffer, 0 );
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
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext8Type:
				{
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					var length = buffer[ 0 ];
					offset += 1;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					var length = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					var length = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					MessagePackExtendedTypeObject ext;
					if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out ext ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Int8Type:
				{
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = unchecked( ( sbyte )buffer[ 0 ] );
					offset += 1;
					break;
				}
				case ReadValueResult.Int16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.Int32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Int64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt64( buffer, 0 );
					offset += 8;
					break;
				}
				case ReadValueResult.UInt8Type:
				{
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackObject );
						return false;
					}
					result = buffer[ 0 ];
					offset += 1;
					break;
				}
				case ReadValueResult.UInt16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.UInt32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.UInt64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt64( buffer, 0 );
					offset += 8;
					break;
				}
				case ReadValueResult.Real32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToSingle( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Real64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToDouble( buffer, 0 );
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

		private bool ReadItems( int count, bool isMap, byte[] buffer, ref Int64 offset, out MessagePackObject result )
		{
			MessagePackObject container;
			if ( !isMap )
			{
				var array = new MessagePackObject[ count ];
				for ( var i = 0; i < count; i++ )
				{
					MessagePackObject item;
					if ( !this.ReadObjectCore( true, buffer, ref offset, out item ) )
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
					if ( !this.ReadObjectCore( true, buffer, ref offset, out key ) )
					{
						result = default( MessagePackObject );
						return false;
					}
					MessagePackObject value;
					if ( !this.ReadObjectCore( true, buffer, ref offset, out value ) )
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

		private async Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( bool isDeep, CancellationToken cancellationToken )
		{
			MessagePackObject result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			var asyncReadResult = await this.ReadObjectCoreAsync( isDeep, buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				result = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<MessagePackObject>();
			}
			this._offset = offset;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<MessagePackObject>>> ReadObjectCoreAsync( bool isDeep, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
			}

			var byteHeader = buffer[ 0 ];
			offset++;
			var collectionType = ReadValueResults.CollectionType[ byteHeader ];
			MessagePackObject result;

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
					var asyncReadResult = await this.ReadBinaryCoreAsync( length, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						binary = asyncReadResult.Value.Result;
						offset = asyncReadResult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}

					result = binary;
				}
				else
				{
					var asyncReadReasult = await this.ReadObjectSlowAsync( header, buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadReasult.Success )
					{
						result = asyncReadReasult.Value.Result;
						offset = asyncReadReasult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
				}
			}

			if ( isDeep && collectionType != CollectionType.None )
			{
				var asyncReadReasult = await this.ReadItemsAsync( result.AsInt32(), collectionType == CollectionType.Map, buffer, offset, cancellationToken ).ConfigureAwait( false );
				if ( asyncReadReasult.Success )
				{
					result = asyncReadReasult.Value.Result;
					offset = asyncReadReasult.Value.Offset;
				}
				else
				{
					return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
				}
			}

			this._data = result;
			this._collectionType = collectionType;
			return AsyncReadResult.Success( result, offset );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<MessagePackObject>>> ReadObjectSlowAsync( ReadValueResult header, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			MessagePackObject result;
			switch ( header & ReadValueResult.TypeCodeMask )
			{
				case ReadValueResult.Array16Type:
				case ReadValueResult.Map16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.Array32Type:
				case ReadValueResult.Map32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Str8Type:
				{
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}

					var length = buffer[ 0 ];
					this.CheckLength( length, header );
					offset += 1;
					MessagePackString stringValue;
					var asyncReadResult = await this.ReadRawStringCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						stringValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Str16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt16( buffer, 0 );
					this.CheckLength( length, header );
					offset += 2;
					MessagePackString stringValue;
					var asyncReadResult = await this.ReadRawStringCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						stringValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Str32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt32( buffer, 0 );
					this.CheckLength( length, header );
					offset += 4;
					MessagePackString stringValue;
					var asyncReadResult = await this.ReadRawStringCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						stringValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( stringValue );
					break;
				}
				case ReadValueResult.Bin8Type:
				{
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}

					var length = buffer[ 0 ];
					this.CheckLength( length, header );
					offset += 1;
					byte[] binaryValue;
					var asyncReadResult = await this.ReadBinaryCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						binaryValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.Bin16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt16( buffer, 0 );
					this.CheckLength( length, header );
					offset += 2;
					byte[] binaryValue;
					var asyncReadResult = await this.ReadBinaryCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						binaryValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.Bin32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					var length = BigEndianBinary.ToUInt32( buffer, 0 );
					this.CheckLength( length, header );
					offset += 4;
					byte[] binaryValue;
					var asyncReadResult = await this.ReadBinaryCoreAsync( unchecked( ( int )length ), offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						offset = asyncReadResult.Value.Offset;
						binaryValue = asyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = new MessagePackObject( binaryValue, /* isBinary */true );
					break;
				}
				case ReadValueResult.FixExtType:
				{
					var length = ( header & ReadValueResult.ValueOrLengthMask );
					MessagePackExtendedTypeObject ext;
					var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						ext = asyncReadResult.Value.Result;
						offset = asyncReadResult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext8Type:
				{
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					var length = buffer[ 0 ];
					offset += 1;
					MessagePackExtendedTypeObject ext;
					var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						ext = asyncReadResult.Value.Result;
						offset = asyncReadResult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					var length = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					MessagePackExtendedTypeObject ext;
					var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						ext = asyncReadResult.Value.Result;
						offset = asyncReadResult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Ext32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					var length = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					MessagePackExtendedTypeObject ext;
					var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( asyncReadResult.Success )
					{
						ext = asyncReadResult.Value.Result;
						offset = asyncReadResult.Value.Offset;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					
					result = ext;
					break;
				}
				case ReadValueResult.Int8Type:
				{
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					result = unchecked( ( sbyte )buffer[ 0 ] );
					offset += 1;
					break;
				}
				case ReadValueResult.Int16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.Int32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Int64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToInt64( buffer, 0 );
					offset += 8;
					break;
				}
				case ReadValueResult.UInt8Type:
				{
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					result = buffer[ 0 ];
					offset += 1;
					break;
				}
				case ReadValueResult.UInt16Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					offset += 2;
					break;
				}
				case ReadValueResult.UInt32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt32( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.UInt64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToUInt64( buffer, 0 );
					offset += 8;
					break;
				}
				case ReadValueResult.Real32Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToSingle( buffer, 0 );
					offset += 4;
					break;
				}
				case ReadValueResult.Real64Type:
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 8;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local
					result = BigEndianBinary.ToDouble( buffer, 0 );
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

			return AsyncReadResult.Success( result, offset );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<MessagePackObject>>> ReadItemsAsync( int count, bool isMap, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			MessagePackObject container;
			if ( !isMap )
			{
				var array = new MessagePackObject[ count ];
				for ( var i = 0; i < count; i++ )
				{
					MessagePackObject item;
					var itemAsyncReadResult = await this.ReadObjectCoreAsync( true, buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( itemAsyncReadResult.Success )
					{
						offset = itemAsyncReadResult.Value.Offset;
						item = itemAsyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
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
					var keyAsyncReadResult = await this.ReadObjectCoreAsync( true, buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( keyAsyncReadResult.Success )
					{
						offset = keyAsyncReadResult.Value.Offset;
						key = keyAsyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					MessagePackObject value;
					var valueAsyncReadResult = await this.ReadObjectCoreAsync( true, buffer, offset, cancellationToken ).ConfigureAwait( false );
					if ( valueAsyncReadResult.Success )
					{
						offset = valueAsyncReadResult.Value.Offset;
						value = valueAsyncReadResult.Value.Result;
					}
					else
					{
						return AsyncReadResult.Fail<Int64OffsetValue<MessagePackObject>>();
					}
					map.Add( key, value );
				}

				container = new MessagePackObject( map, true );
			}
			return AsyncReadResult.Success( container, offset );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadArrayLength( out Int64 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Array16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Int64 );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Array32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Int64 );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

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

		public sealed override async Task<AsyncReadResult<Int64>> ReadArrayLengthAsync( CancellationToken cancellationToken )
		{
			Int64 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Array16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Array32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			result = length;
			this._data = length;
			this._offset = offset;
			this._collectionType = CollectionType.Array;
			return AsyncReadResult.Success( result );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadMapLength( out Int64 result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( Int64 );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( Int64 );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Map16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Int64 );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Map32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( Int64 );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

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

		public sealed override async Task<AsyncReadResult<Int64>> ReadMapLengthAsync( CancellationToken cancellationToken )
		{
			Int64 result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<Int64>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Map16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Map32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<Int64>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			result = length;
			this._data = length;
			this._offset = offset;
			this._collectionType = CollectionType.Map;
			return AsyncReadResult.Success( result );
		}

#endif // FEATURE_TAP

		public sealed override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackExtendedTypeObject );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Ext16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackExtendedTypeObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackExtendedTypeObject );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out result ) )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return true;
		}

		private bool ReadMessagePackExtendedTypeObjectCore( int length, byte[] buffer, ref Int64 offset, out MessagePackExtendedTypeObject result )
		{
			// Read type code
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			var typeCode = buffer[ 0 ];
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
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( this._source.Read( buffer, 0, 1 ) < 1 )
			{
				result = default( MessagePackExtendedTypeObject? );
				return false;
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( this._source.Read( buffer, 0, 1 ) < 1 )
					{
						result = default( MessagePackExtendedTypeObject? );
						return false;
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Ext16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackExtendedTypeObject? );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = this._source.Read( buffer, bufferOffset, reading );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									result = default( MessagePackExtendedTypeObject? );
									return false;
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			MessagePackExtendedTypeObject value;
			if ( !this.ReadMessagePackExtendedTypeObjectCore( unchecked( ( int )length ), buffer, ref offset, out value ) )
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

		public sealed override async Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			MessagePackExtendedTypeObject result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Ext16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				result = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
		}

		private async Task<AsyncReadResult<Int64OffsetValue<MessagePackExtendedTypeObject>>> ReadMessagePackExtendedTypeObjectCoreAsync( int length, byte[] buffer, Int64 offset, CancellationToken cancellationToken )
		{
			// Read type code
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<Int64OffsetValue<MessagePackExtendedTypeObject>>();
			}

			var typeCode = buffer[ 0 ];
			offset++;

			// Read body
			byte[] body;
			var asyncReadResult = await this.ReadBinaryCoreAsync( length, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				body = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<Int64OffsetValue<MessagePackExtendedTypeObject>>();
			}
			return AsyncReadResult.Success( MessagePackExtendedTypeObject.Unpack( typeCode, body ), offset );
		}

		public sealed override async Task<AsyncReadResult<MessagePackExtendedTypeObject?>> ReadNullableMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			MessagePackExtendedTypeObject? result;
			var buffer = this._scalarBuffer;
			var offset = this._offset;
			this._lastOffset = this._offset;
			if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
			{
				return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
			}

			var header = ReadValueResults.EncodedTypes[ buffer[ 0 ] ];
			if ( header == ReadValueResult.Nil )
			{
				return AsyncReadResult.Success( default( MessagePackExtendedTypeObject? ) );
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
					if ( await this._source.ReadAsync( buffer, 0, 1, cancellationToken ).ConfigureAwait( false ) < 1 )
					{
						return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
					}

					length = BigEndianBinary.ToByte( buffer, 0 );

					break;
				}
				case 2: // Ext16
				{
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 2;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt16( buffer, 0 );

					break;
				}
				default: // Ext32
				{
#if DEBUG
					Contract.Assert( lengthOfLength == 4, lengthOfLength + " == 4" );
#endif // DEBUG
					// scope for local
					{
						var bufferOffset = 0;
						var reading = 4;
						// Retrying for splitted Stream such as NetworkStream
						while( true )
						{
							var readLength = await this._source.ReadAsync( buffer, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
							if ( readLength < reading )
							{
								if ( readLength > 0 )
								{
									// retry reading
									bufferOffset += readLength;
									reading -= readLength;
									continue;
								}
								else
								{
									if ( this._useStreamPosition )
									{
										// Rollback
										this._source.Position -= ( bufferOffset + readLength );
									}
									else
									{
										// Throw because rollback is not available
										this.ThrowEofException( reading );
									}
					
									return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
								}
							} // if readLength < reading
					
							break;
						} // while true
					} // scope for local

					length = BigEndianBinary.ToUInt32( buffer, 0 );

					break;
				}
			}

			offset += lengthOfLength;
			this.CheckLength( length, header );
			var asyncReadResult = await this.ReadMessagePackExtendedTypeObjectCoreAsync( unchecked( ( int )length ), buffer, offset, cancellationToken ).ConfigureAwait( false );
			if ( asyncReadResult.Success )
			{
				result = asyncReadResult.Value.Result;
				offset = asyncReadResult.Value.Offset;
			}
			else
			{
				return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
			}
			
			this._offset = offset;
			this._collectionType = CollectionType.None;
			return AsyncReadResult.Success( result );
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
