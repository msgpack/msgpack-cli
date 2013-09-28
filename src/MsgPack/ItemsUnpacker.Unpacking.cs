
 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = true;
					return true;
				}
				case 0xC2:
				{
					this.InternalCollectionType = CollectionType.None;
					result = false;
					return true;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( bool ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeBoolean( out Boolean result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = true;
					return true;
				}
				case 0xC2:
				{
					this.InternalCollectionType = CollectionType.None;
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
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = true;
					return true;
				}
				case 0xC2:
				{
					this.InternalCollectionType = CollectionType.None;
					result = false;
					return true;
				}
				case 0xC0:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( bool ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableBoolean( out Boolean? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = true;
					return true;
				}
				case 0xC2:
				{
					this.InternalCollectionType = CollectionType.None;
					result = false;
					return true;
				}
				case 0xC0:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
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
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Byte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Byte )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeByte( out Byte result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Byte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Byte )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableByte( out Byte? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Byte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Byte )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableByte( out Byte? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Byte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Byte )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Byte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadSByte( out SByte result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( SByte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( SByte )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeSByte( out SByte result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( SByte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( SByte )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableSByte( out SByte? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( SByte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( SByte )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableSByte( out SByte? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( SByte );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = BigEndianBinary.ToSByte( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( SByte )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( SByte ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadInt16( out Int16 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeInt16( out Int16 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableInt16( out Int16? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableInt16( out Int16? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int16 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadUInt16( out UInt16 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeUInt16( out UInt16 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableUInt16( out UInt16? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableUInt16( out UInt16? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt16 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt16 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = BigEndianBinary.ToUInt16( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt16 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadInt32( out Int32 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeInt32( out Int32 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableInt32( out Int32? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableInt32( out Int32? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadUInt32( out UInt32 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeUInt32( out UInt32 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableUInt32( out UInt32? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableUInt32( out UInt32? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt32 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToUInt32( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt32 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt32 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadInt64( out Int64 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeInt64( out Int64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableInt64( out Int64? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableInt64( out Int64? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Int64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Int64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadUInt64( out UInt64 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeUInt64( out UInt64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableUInt64( out UInt64? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableUInt64( out UInt64? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( UInt64 );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToUInt64( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( UInt64 )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( UInt64 ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadSingle( out Single result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Single );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeSingle( out Single result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Single );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableSingle( out Single? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Single );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableSingle( out Single? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Single );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = BigEndianBinary.ToSingle( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Single )BigEndianBinary.ToDouble( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Single ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadDouble( out Double result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Double );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeDouble( out Double result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Double );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadNullableDouble( out Double? result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Double );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		internal bool ReadSubtreeNullableDouble( out Double? result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = default( Double );
				return false;
			}
			
			if( header < 0x80 )
			{
				var resultValue = unchecked( ( byte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header >= 0xE0 )
			{
				var resultValue = unchecked( ( sbyte )header );
				this.InternalCollectionType = CollectionType.None;
				result = resultValue;
				return true;
			}
			if( header == MessagePackCode.NilValue )
			{
				this.InternalCollectionType = CollectionType.None;
				result = null;
				return true;
			}
			
			switch( header )
			{
				case MessagePackCode.SignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.SignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt8:
				{
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToByte( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt16:
				{
					var read = source.Read( buffer, 0, 2 );
					if( read == 2 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt16( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt32( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.UnsignedInt64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToUInt64( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real32:
				{
					var read = source.Read( buffer, 0, 4 );
					if( read == 4 )
					{
						var resultValue = checked( ( Double )BigEndianBinary.ToSingle( buffer, 0 ) );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				case MessagePackCode.Real64:
				{
					var read = source.Read( buffer, 0, 8 );
					if( read == 8 )
					{
						var resultValue = BigEndianBinary.ToDouble( buffer, 0 );
						this.InternalCollectionType = CollectionType.None;
						result = resultValue;
						return true;
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", typeof( Double ), header, MessagePackCode.ToString( header ) ) );
				}
			}
		}
		
		public override bool ReadBinary( out Byte[] result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackRawLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = null;
				return false;
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.NilValue:
				{
					result = null;
					this.InternalCollectionType = CollectionType.None;
					return true;
				}
				case MessagePackCode.Bin8: 
				case MessagePackCode.Str8: 
				{
					size = source.ReadByte();
					if( size < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					break;
				}
				case MessagePackCode.Bin16: 
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Bin32:
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsignedSize );
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
			#endregion UnpackRawLength
			#region UnpackByteArray
			
			if ( size == 0 )
			{
				result = Binary.Empty;
				this.InternalCollectionType = CollectionType.None;
				return true;
			}
			
			int contentBufferOffset = 0;
			var resultValue = new byte[ size ];
			#region UnpackRawContent
			
			var bytesRead = source.Read( resultValue, contentBufferOffset, size );
			if( bytesRead < size )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			#endregion UnpackRawContent
			#endregion UnpackByteArray
			this.InternalCollectionType = CollectionType.None;
			result = resultValue;
			return true;
		}
		
		internal bool ReadSubtreeBinary( out Byte[] result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackRawLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = null;
				return false;
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.NilValue:
				{
					result = null;
					this.InternalCollectionType = CollectionType.None;
					return true;
				}
				case MessagePackCode.Bin8: 
				case MessagePackCode.Str8: 
				{
					size = source.ReadByte();
					if( size < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					break;
				}
				case MessagePackCode.Bin16: 
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Bin32:
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsignedSize );
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
			#endregion UnpackRawLength
			#region UnpackByteArray
			
			if ( size == 0 )
			{
				result = Binary.Empty;
				this.InternalCollectionType = CollectionType.None;
				return true;
			}
			
			int contentBufferOffset = 0;
			var resultValue = new byte[ size ];
			#region UnpackRawContent
			
			var bytesRead = source.Read( resultValue, contentBufferOffset, size );
			if( bytesRead < size )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			#endregion UnpackRawContent
			#endregion UnpackByteArray
			this.InternalCollectionType = CollectionType.None;
			result = resultValue;
			return true;
		}
		
		public override bool ReadString( out String result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			var encoding = Encoding.UTF8;
			#if DEBUG
			Contract.Assert( encoding != null );
			#endif
			#region UnpackRawLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = null;
				return false;
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.NilValue:
				{
					result = null;
					this.InternalCollectionType = CollectionType.None;
					return true;
				}
				case MessagePackCode.Bin8: 
				case MessagePackCode.Str8: 
				{
					size = source.ReadByte();
					if( size < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					break;
				}
				case MessagePackCode.Bin16: 
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Bin32:
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsignedSize );
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
			#endregion UnpackRawLength
			#region UnpackString
			
			if ( size == 0 )
			{
				result = String.Empty;
				this.InternalCollectionType = CollectionType.None;
				return true;
			}
			
			var decoder = encoding.GetDecoder();
			int chunkSize = size > 16 * 1024 ? 16 * 1024 : size;
			var bytes = new byte[ chunkSize ];
			var chars = new char[ chunkSize ];
			var stringBuffer = new StringBuilder( size );
			var remaining = size;
			do
			{
				var reading = ( remaining > bytes.Length ) ? bytes.Length : remaining;
			    var bytesRead = source.Read( bytes, 0, reading );
				if ( bytesRead == 0 )
				{
					throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
				}
			
				remaining -= bytesRead;
			
			    var isCompleted = false;
			    var bytesOffset = 0;
			
			    while( !isCompleted )
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
						( bytesRead == 0 ), // flush when last read.
			            out bytesUsed,
						out charsUsed,
						out isCompleted
					);
				
					stringBuffer.Append( chars, 0, charsUsed );
			        bytesOffset += bytesUsed;
			    }
			} while( remaining > 0 );
			
			var resultValue = stringBuffer.ToString();
			#endregion UnpackString
			this.InternalCollectionType = CollectionType.None;
			result = resultValue;
			return true;
		}
		
		internal bool ReadSubtreeString( out String result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			var encoding = Encoding.UTF8;
			#if DEBUG
			Contract.Assert( encoding != null );
			#endif
			#region UnpackRawLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				result = null;
				return false;
			}
			
			int size;
			switch( header )
			{
				case MessagePackCode.NilValue:
				{
					result = null;
					this.InternalCollectionType = CollectionType.None;
					return true;
				}
				case MessagePackCode.Bin8: 
				case MessagePackCode.Str8: 
				{
					size = source.ReadByte();
					if( size < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					break;
				}
				case MessagePackCode.Bin16: 
				case MessagePackCode.Raw16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					size = BigEndianBinary.ToUInt16( buffer, 0 );
					break;
				}
				case MessagePackCode.Bin32:
				case MessagePackCode.Raw32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					size = unchecked( ( int )unsignedSize );
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
			#endregion UnpackRawLength
			#region UnpackString
			
			if ( size == 0 )
			{
				result = String.Empty;
				this.InternalCollectionType = CollectionType.None;
				return true;
			}
			
			var decoder = encoding.GetDecoder();
			int chunkSize = size > 16 * 1024 ? 16 * 1024 : size;
			var bytes = new byte[ chunkSize ];
			var chars = new char[ chunkSize ];
			var stringBuffer = new StringBuilder( size );
			var remaining = size;
			do
			{
				var reading = ( remaining > bytes.Length ) ? bytes.Length : remaining;
			    var bytesRead = source.Read( bytes, 0, reading );
				if ( bytesRead == 0 )
				{
					throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
				}
			
				remaining -= bytesRead;
			
			    var isCompleted = false;
			    var bytesOffset = 0;
			
			    while( !isCompleted )
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
						( bytesRead == 0 ), // flush when last read.
			            out bytesUsed,
						out charsUsed,
						out isCompleted
					);
				
					stringBuffer.Append( chars, 0, charsUsed );
			        bytesOffset += bytesUsed;
			    }
			} while( remaining > 0 );
			
			var resultValue = stringBuffer.ToString();
			#endregion UnpackString
			this.InternalCollectionType = CollectionType.None;
			result = resultValue;
			return true;
		}
		
		public override bool ReadObject( out MessagePackObject result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = MessagePackObject.Nil;
					return true;
				}
				case MessagePackCode.TrueValue:
				{
					this.InternalCollectionType = CollectionType.None;
					result = Unpacking.TrueValue;
					return true;
				}
				case MessagePackCode.FalseValue:
				{
					this.InternalCollectionType = CollectionType.None;
					result = Unpacking.FalseValue;
					return true;
				}
			}
			
			if( header < 0x80 )
			{
				this.InternalCollectionType = CollectionType.None;
				result = Unpacking.PositiveIntegers[ header ];
				return true;
			}
			else if( header >= 0xE0 )
			{
				this.InternalCollectionType = CollectionType.None;
				result = Unpacking.NegativeIntegers[ header - 0xE0 ];
				return true;
			}
			
			switch( header & 0xF0 )
			{
				case 0x80:
				{
					var size = header & 0xF;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = size;
					this.InternalData = size;
					result = Unpacking.PositiveIntegers[ size ];
					return true;
				}
				case 0x90:
				{
					var size = header & 0xF;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = size;
					this.InternalData = size;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
					result = resultValue;
					return true;
				}
				case MessagePackCode.Bin8:
				case MessagePackCode.Str8:
				{
					byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
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
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Bin16:
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
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Bin32:
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = length;
					this.InternalData = length;
					result = ( long )length;
					return true;
				}
				case MessagePackCode.FixExt1:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 1 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt2:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 2 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt4:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 4 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt8:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 8 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt16:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 16 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext8:
				{
					#region UnpackExt
			
					Byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext16:
				{
					#region UnpackExt
			
					UInt16 length;
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
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext32:
				{
					#region UnpackExt
			
					UInt32 length;
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
			
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				default:
				{
			#if DEBUG
					Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
			#endif
					throw new UnassignedMessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", header ) );
				}
			}
		}
		
		internal bool ReadSubtreeObject( out MessagePackObject result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
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
					this.InternalCollectionType = CollectionType.None;
					result = MessagePackObject.Nil;
					return true;
				}
				case MessagePackCode.TrueValue:
				{
					this.InternalCollectionType = CollectionType.None;
					result = Unpacking.TrueValue;
					return true;
				}
				case MessagePackCode.FalseValue:
				{
					this.InternalCollectionType = CollectionType.None;
					result = Unpacking.FalseValue;
					return true;
				}
			}
			
			if( header < 0x80 )
			{
				this.InternalCollectionType = CollectionType.None;
				result = Unpacking.PositiveIntegers[ header ];
				return true;
			}
			else if( header >= 0xE0 )
			{
				this.InternalCollectionType = CollectionType.None;
				result = Unpacking.NegativeIntegers[ header - 0xE0 ];
				return true;
			}
			
			switch( header & 0xF0 )
			{
				case 0x80:
				{
					var size = header & 0xF;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = size;
					this.InternalData = size;
					result = Unpacking.PositiveIntegers[ size ];
					return true;
				}
				case 0x90:
				{
					var size = header & 0xF;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = size;
					this.InternalData = size;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.None;
					result = resultValue;
					return true;
				}
				case MessagePackCode.Bin8:
				case MessagePackCode.Str8:
				{
					byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
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
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Bin16:
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
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
				}
				case MessagePackCode.Bin32:
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
					this.InternalCollectionType = CollectionType.None;
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
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = length;
					this.InternalData = length;
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
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = length;
					this.InternalData = length;
					result = ( long )length;
					return true;
				}
				case MessagePackCode.FixExt1:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 1 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt2:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 2 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt4:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 4 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt8:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 8 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt16:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 16 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext8:
				{
					#region UnpackExt
			
					Byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext16:
				{
					#region UnpackExt
			
					UInt16 length;
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
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext32:
				{
					#region UnpackExt
			
					UInt32 length;
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
			
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				default:
				{
			#if DEBUG
					Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
			#endif
					throw new UnassignedMessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", header ) );
				}
			}
		}
		
		public override bool ReadArrayLength( out Int64 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackArrayLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.Array16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				case MessagePackCode.Array32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					result = unsignedSize;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedArray <= header && header <= MessagePackCode.MaximumFixedArray )
					{
						result = header & 0xF;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "array header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackArrayLength
		}
		
		internal bool ReadSubtreeArrayLength( out Int64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackArrayLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.Array16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				case MessagePackCode.Array32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					result = unsignedSize;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedArray <= header && header <= MessagePackCode.MaximumFixedArray )
					{
						result = header & 0xF;
					this.InternalCollectionType = CollectionType.Array;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "array header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackArrayLength
		}
		
		public override bool ReadMapLength( out Int64 result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackMapLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.Map16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				case MessagePackCode.Map32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					result = unsignedSize;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedMap <= header && header <= MessagePackCode.MaximumFixedMap )
					{
						result = header & 0xF;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "map header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackMapLength
		}
		
		internal bool ReadSubtreeMapLength( out Int64 result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackMapLength
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.Map16: 
				{
					if( source.Read( buffer, 0, 2 ) < 2 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					result = BigEndianBinary.ToUInt16( buffer, 0 );
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				case MessagePackCode.Map32:
				{
					if( source.Read( buffer, 0, 4 ) < 4 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var unsignedSize = BigEndianBinary.ToUInt32( buffer, 0 );
					if( unsignedSize > Int32.MaxValue )
					{
						throw new MessageNotSupportedException( "MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes." );
					}
			
					result = unsignedSize;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
				}
				default:
				{
					if( MessagePackCode.MinimumFixedMap <= header && header <= MessagePackCode.MaximumFixedMap )
					{
						result = header & 0xF;
					this.InternalCollectionType = CollectionType.Map;
					this.InternalItemsCount = result;
					this.InternalData = result;
					return true;
					}
			
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "map header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackMapLength
		}
		
		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			this.EnsureNotInSubtreeMode();
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackExt
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.FixExt1:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 1 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt2:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 2 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt4:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 4 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt8:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 8 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt16:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 16 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext8:
				{
					#region UnpackExt
			
					Byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext16:
				{
					#region UnpackExt
			
					UInt16 length;
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
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext32:
				{
					#region UnpackExt
			
					UInt32 length;
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
			
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "map header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackExt
		}
		
		internal bool ReadSubtreeMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			#region UnpackExt
			#if DEBUG
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );
			#endif
			
			var header = source.ReadByte();
			if( header < 0 )
			{
				throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
			}
			
			switch( header )
			{
				case MessagePackCode.FixExt1:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 1 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt2:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 2 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt4:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 4 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt8:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 8 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.FixExt16:
				{
					#region UnpackExt
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ 16 ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext8:
				{
					#region UnpackExt
			
					Byte length;
					#region UnpackScalar
					
					var read = source.Read( buffer, 0, 1 );
					if( read == 1 )
					{
						length = BigEndianBinary.ToByte( buffer, 0 );
					}
					else
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackScalar
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext16:
				{
					#region UnpackExt
			
					UInt16 length;
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
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				case MessagePackCode.Ext32:
				{
					#region UnpackExt
			
					UInt32 length;
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
			
					var typeCode = source.ReadByte();
					if( typeCode < 0 )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
			
					var data = new byte[ length ];
					#region UnpackRawContent
					
					var bytesRead = source.Read( data, 0, data.Length );
					if( bytesRead < data.Length )
					{
						throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
					}
					
					#endregion UnpackRawContent
					var resultMpoValue = MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )typeCode ), data );
					this.InternalCollectionType = CollectionType.None;
					result = resultMpoValue;
					return true;
					#endregion UnpackExt
				}
				default:
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", "map header", header, MessagePackCode.ToString( header ) ) );
				}
			}// switch
			#endregion UnpackExt
		}
		
	}
}
