
 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	// This file was generated from ItemsUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit ItemsUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class ItemsUnpacker
	{
		public override bool ReadBoolean( out Boolean result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Boolean );
			return false;
			}
			
					switch( header )
					{
						case 0xC3:
						{
							this._collectionType = CollectionType.None;
							this._itemsCount = 0;
							result = true;
							return true;
						}
						case 0xC2:
						{
							this._collectionType = CollectionType.None;
							this._itemsCount = 0;
							result = false;
							return true;
						}
						default:
						{
							throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( bool ), header, MessagePackCode.ToString( header ) ) );
						}
					}
		}

		public override bool ReadNullableBoolean( out Boolean? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Boolean );
			return false;
			}
			
					switch( header )
					{
						case 0xC3:
						{
							this._collectionType = CollectionType.None;
							this._itemsCount = 0;
							result = true;
							return true;
						}
						case 0xC2:
						{
							this._collectionType = CollectionType.None;
							this._itemsCount = 0;
							result = false;
							return true;
						}
						default:
						{
							throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( bool ), header, MessagePackCode.ToString( header ) ) );
						}
					}
		}

		public override bool ReadByte( out Byte result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Byte );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.UnsignedInt8:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 1 );
			if( read == 1 )
			{
				var resultValue = BigEndianBinary.ToByte( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableByte( out Byte? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Byte );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.UnsignedInt8:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 1 );
			if( read == 1 )
			{
				var resultValue = BigEndianBinary.ToByte( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadSByte( out SByte result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( SByte );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.UnsignedInt8:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 1 );
			if( read == 1 )
			{
				var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableSByte( out SByte? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( SByte );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.UnsignedInt8:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 1 );
			if( read == 1 )
			{
				var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadInt16( out Int16 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int16 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 2 );
			if( read == 2 )
			{
				var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableInt16( out Int16? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int16 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 2 );
			if( read == 2 )
			{
				var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadUInt16( out UInt16 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt16 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 2 );
			if( read == 2 )
			{
				var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableUInt16( out UInt16? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt16 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 2 );
			if( read == 2 )
			{
				var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadInt32( out Int32 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int32 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.Real32:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableInt32( out Int32? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int32 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.Real32:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadUInt32( out UInt32 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt32 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.Real32:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableUInt32( out UInt32? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt32 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.Real32:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadInt64( out Int64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int64 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableInt64( out Int64? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Int64 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadUInt64( out UInt64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt64 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableUInt64( out UInt64? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( UInt64 );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadSingle( out Single result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Single );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableSingle( out Single? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Single );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 4 );
			if( read == 4 )
			{
				var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadDouble( out Double result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Double );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadNullableDouble( out Double? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
			result = default( Double );
			return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				case MessagePackCode.SignedInt16:
				case MessagePackCode.SignedInt32:
				case MessagePackCode.SignedInt64:
				case MessagePackCode.UnsignedInt8:
				case MessagePackCode.UnsignedInt16:
				case MessagePackCode.UnsignedInt32:
				case MessagePackCode.UnsignedInt64:
				case MessagePackCode.Real32:
				case MessagePackCode.Real64:
				{
					break;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
			
			var read = source.Read( buffer, 0, 8 );
			if( read == 8 )
			{
				var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				result = resultValue;
				return true;
			}
			else
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
		}

		public override bool ReadBinary( out byte[] result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsingedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsingedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsingedSize );
					break;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedRaw <= header && header <= MessagePackCode.MaximumFixedRaw )
					{
						size = ( header & 0x1F );
						break;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "System.Byte[]", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			int contentBufferOffset = 0;
			var resultValue = new byte[ size ];
			#region UnpackRawContent
			
			var bytesRead = source.Read( resultValue, contentBufferOffset, size );
			if( bytesRead < size )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			#endregion UnpackRawContent
			this._collectionType = CollectionType.None;
			this._itemsCount = 0;
			result = resultValue;
			return true;
		}

		public override bool ReadString( out string result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			var encoding = Encoding.UTF8;
			Contract.Assert( encoding != null );
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsingedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsingedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsingedSize );
					break;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedRaw <= header && header <= MessagePackCode.MaximumFixedRaw )
					{
						size = ( header & 0x1F );
						break;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "System.String", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			var decoder = encoding.GetDecoder();
			var bytes = new byte[ 64 * 1024 ];
			var chars = new char[ 16 * 1024 ];
			var stringBuffer = new StringBuilder();
			int bytesRead;
			do
			{
			    bytesRead = source.Read( bytes, 0, 64 * 1024 );
			
			    bool isCompleted = false;
			    int bytesOffset = 0;
			
			    while( !isCompleted )
			    {
					int bytesUsed;
					int charsUsed;
			        bool flush = ( bytesRead == 0 );
			        decoder.Convert(
						bytes, 
						bytesOffset,
						bytesRead - bytesOffset,
						chars,
						0,
						16 * 1024,
						( bytesRead == 0 ), // flush when last read.
			            out bytesUsed,
						out charsUsed,
						out isCompleted
					);
				
					stringBuffer.Append( chars, 0, charsUsed );
			        bytesOffset += bytesUsed;
			    }
			} while( bytesRead > 0 );
			
			var resultValue = stringBuffer.ToString();
			this._collectionType = CollectionType.None;
			this._itemsCount = 0;
			result = resultValue;
			return true;
		}

		public override bool ReadObject( out MessagePackObject result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( MessagePackObject );
				return false;
			}
			
			switch( header )
			{
				case MessagePackCode.NilValue:
				{
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = MessagePackObject.Nil;
					result = MessagePackObject.Nil;
					return true;
				}
				case MessagePackCode.TrueValue:
				{
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = Unpacking.TrueValue;
					result = Unpacking.TrueValue;
					return true;
				}
				case MessagePackCode.FalseValue:
				{
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = Unpacking.FalseValue;
					result = Unpacking.FalseValue;
					return true;
				}
			}
			
			if( header < 0x80 )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				this.Data = Unpacking.PositiveIntegers[ header ];
				result = Unpacking.PositiveIntegers[ header ];
				return true;
			}
			else if( header >= 0xE0 )
			{
				this._collectionType = CollectionType.None;
				this._itemsCount = 0;
				this.Data = Unpacking.NegativeIntegers[ header - 0xE0 ];
				result = Unpacking.NegativeIntegers[ header - 0xE0 ];
				return true;
			}
			
			switch( header & 0xF0 )
			{
				case 0x80:
				{
					var size = header & 0xF;
					this._collectionType = CollectionType.Map;
					this._itemsCount = size;
					this.Data = size;
					result = Unpacking.PositiveIntegers[ size ];
					return true;
				}
				case 0x90:
				{
					var size = header & 0xF;
					this._collectionType = CollectionType.Array;
					this._itemsCount = size;
					this.Data = size;
					result = Unpacking.PositiveIntegers[ size ];
					return true;
				}
				case 0xA0:
				case 0xB0:
				{
					var size = header & 0x1F;
					var resultValue = new byte[ size ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( resultValue, 0, size );
					if( bytesRead < size )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = new MessagePackObject( new MessagePackString( resultValue ) );
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultMpoValue;
					result = resultMpoValue;
					return true;
				}
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					SByte resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						resultValue = BigEndianBinary.ToSByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.SignedInt16:
				{
					Int16 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						resultValue = BigEndianBinary.ToInt16( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.SignedInt32:
				{
					Int32 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						resultValue = BigEndianBinary.ToInt32( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.SignedInt64:
				{
					Int64 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						resultValue = BigEndianBinary.ToInt64( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.UnsignedInt8:
				{
					Byte resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						resultValue = BigEndianBinary.ToByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.UnsignedInt16:
				{
					UInt16 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.UnsignedInt32:
				{
					UInt32 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.UnsignedInt64:
				{
					UInt64 resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.Real32:
				{
					Single resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						resultValue = BigEndianBinary.ToSingle( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.Real64:
				{
					Double resultValue;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						resultValue = BigEndianBinary.ToDouble( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultValue;
					result = resultValue;
					return true;
				}
				case MessagePackCode.Raw16:
				{
					ushort length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						length = BigEndianBinary.ToUInt16( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					var resultValue = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( resultValue, 0, length );
					if( bytesRead < length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = new MessagePackObject( new MessagePackString( resultValue ) );
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultMpoValue;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Raw32:
				{
					uint length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						length = BigEndianBinary.ToUInt32( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					if( length > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					var size = unchecked( ( int )length );
					var resultValue = new byte[ size ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( resultValue, 0, size );
					if( bytesRead < size )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = new MessagePackObject( new MessagePackString( resultValue ) );
					this._collectionType = CollectionType.None;
					this._itemsCount = 0;
					this.Data = resultMpoValue;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Array16:
				{
					ushort length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						length = BigEndianBinary.ToUInt16( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.Array;
					this._itemsCount = length;
					this.Data = length;
					result = length;
					return true;
				}
				case MessagePackCode.Array32:
				{
					uint length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						length = BigEndianBinary.ToUInt32( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.Array;
					this._itemsCount = length;
					this.Data = length;
					result = ( long )length;
					return true;
				}
				case MessagePackCode.Map16:
				{
					ushort length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						length = BigEndianBinary.ToUInt16( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.Map;
					this._itemsCount = length;
					this.Data = length;
					result = length;
					return true;
				}
				case MessagePackCode.Map32:
				{
					uint length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						length = BigEndianBinary.ToUInt32( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					this._collectionType = CollectionType.Map;
					this._itemsCount = length;
					this.Data = length;
					result = ( long )length;
					return true;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", header ) );
				}
			}
		}
	}
}
