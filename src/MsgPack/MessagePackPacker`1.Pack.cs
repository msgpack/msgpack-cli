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

using System;
using System.Collections.Generic;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from MessagePackPacker`1.Pack.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackPacker`1.Pack.tt  and Core.ttinclude instead.

	partial class MessagePackPacker<TWriter>
	{
		public void Pack( Boolean value )
		{
			this.Writer.WriteByte( value ? Header.True : Header.False );
		}

		public void Pack( Boolean? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			this.Writer.WriteByte( value ? Header.True : Header.False );
		}

		public void Pack( Byte value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )value ) );
		}

		public void Pack( Byte? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )value ) );
		}

		public void Pack( SByte value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )value ) );
				return;
			}

			this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )value ) );
		}

		public void Pack( SByte? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )value ) );
				return;
			}

			this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )value ) );
		}

		public void Pack( Int16 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )value ) );
		}

		public void Pack( Int16? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )value ) );
		}

		public void Pack( UInt16 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )value ) );
		}

		public void Pack( UInt16? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )value ) );
		}

		public void Pack( Int32 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )value ) );
		}

		public void Pack( Int32? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )value ) );
		}

		public void Pack( UInt32 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt32, unchecked( ( UInt32 )value ) );
		}

		public void Pack( UInt32? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt32, unchecked( ( UInt32 )value ) );
		}

		public void Pack( Int64 value )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int64, unchecked( ( UInt64 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int64, unchecked( ( UInt64 )value ) );
		}

		public void Pack( Int64? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
					return;
				}

				this.Writer.WriteBytes( Header.Int64, unchecked( ( UInt64 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.Writer.WriteBytes( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				this.Writer.WriteBytes( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Int64, unchecked( ( UInt64 )value ) );
		}

		public void Pack( UInt64 value )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt64, unchecked( ( UInt64 )value ) );
		}

		public void Pack( UInt64? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.Writer.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.Writer.WriteBytes( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				this.Writer.WriteBytes( Header.UInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.UInt64, unchecked( ( UInt64 )value ) );
		}

		public void Pack( Single value )
		{
			this.Writer.WriteBytes( Header.Single, unchecked( ( Single )value ) );
		}

		public void Pack( Single? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			this.Writer.WriteBytes( Header.Single, unchecked( ( Single )value ) );
		}

		public void Pack( Double value )
		{
			this.Writer.WriteBytes( Header.Double, unchecked( ( Double )value ) );
		}

		public void Pack( Double? nullable )
		{
			if ( nullable == null )
			{
				this.Writer.WriteByte( Header.Nil );
				return;
			}

			var value = nullable.GetValueOrDefault();

			this.Writer.WriteBytes( Header.Double, unchecked( ( Double )value ) );
		}

		public void PackArrayHeader( uint count )
		{
			if ( count < 0x10 )
			{
				this.Writer.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | count ) ) );
				return;
			}

			if ( count < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Array16, unchecked( ( ushort )( count & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Array32, unchecked( ( uint )count ) );
			return;
		}

		public void PackMapHeader( uint count )
		{
			if ( count < 0x10 )
			{
				this.Writer.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | count ) ) );
				return;
			}

			if ( count < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Map16, unchecked( ( ushort )( count & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Map32, unchecked( ( uint )count ) );
			return;
		}

		public void PackStringHeader( uint count )
		{
			if ( count < 0x20 )
			{
				this.Writer.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | count ) ) );
				return;
			}

			if ( count < 0x100 && this.AllowStr8 )
			{
				this.Writer.WriteBytes( Header.Str8, unchecked( ( byte )( count & 0xFF ) ) );
				return;
			}

			if ( count < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Str16, unchecked( ( ushort )( count & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Str32, unchecked( ( uint )count ) );
			return;
		}

		public void PackBinaryHeader( uint count )
		{
			if ( count < 0x100 )
			{
				this.Writer.WriteBytes( Header.Bin8, unchecked( ( byte )( count & 0xFF ) ) );
				return;
			}

			if ( count < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Bin16, unchecked( ( ushort )( count & 0xFFFF ) ) );
				return;
			}

			this.Writer.WriteBytes( Header.Bin32, unchecked( ( uint )count ) );
			return;
		}

		public void PackRaw( string value )
		{
			this.Writer.WriteBytes( value, this.AllowStr8 );
		}

		public void PackRaw( byte[] value )
		{
			if ( value.Length < 0x20 )
			{
				this.Writer.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | value.Length ) ) );
				this.Writer.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x100 && this.AllowStr8 )
			{
				this.Writer.WriteBytes( Header.Str8, unchecked( ( byte )( value.Length ) ) );
				this.Writer.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Str16, unchecked( ( ushort )( value.Length ) ) );
				this.Writer.WriteBytes( value );
				return;
			}

			this.Writer.WriteBytes( Header.Str32, unchecked( ( uint )value.Length ) );
			this.Writer.WriteBytes( value );
			return;
		}

		public void PackBinary( byte[] value )
		{
			if ( value.Length < 0x100 )
			{
				this.Writer.WriteBytes( Header.Bin8, unchecked( ( byte )( value.Length ) ) );
				this.Writer.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				this.Writer.WriteBytes( Header.Bin16, unchecked( ( ushort )( value.Length ) ) );
				this.Writer.WriteBytes( value );
				return;
			}

			this.Writer.WriteBytes( Header.Bin32, unchecked( ( uint )value.Length ) );
			this.Writer.WriteBytes( value );
			return;
		}

		public void PackExtendedTypeValue( byte typeCode, byte[] body )
		{
			unchecked
			{
				switch ( body.Length )
				{
					case 1:
					{
						this.Writer.WriteByte( Header.FixExt1 );
						break;
					}
					case 2:
					{
						this.Writer.WriteByte( Header.FixExt2 );
						break;
					}
					case 4:
					{
						this.Writer.WriteByte( Header.FixExt4 );
						break;
					}
					case 8:
					{
						this.Writer.WriteByte( Header.FixExt8 );
						break;
					}
					case 16:
					{
						this.Writer.WriteByte( Header.FixExt16 );
						break;
					}
					default:
					{
						if ( body.Length < 0x100 )
						{
							this.Writer.WriteBytes( Header.Ext8, ( byte )body.Length );
						}
						else if ( body.Length < 0x10000 )
						{
							this.Writer.WriteBytes( Header.Ext16, ( ushort )body.Length );
						}
						else
						{
							this.Writer.WriteBytes( Header.Ext32, ( uint )body.Length );
						}

						break;
					}
				} // switch
			} // unchecked

			this.Writer.WriteByte( typeCode );
			this.Writer.WriteBytes( body );
		}

#if FEATURE_TAP

		public async Task PackAsync( Boolean value, CancellationToken cancellationToken )
		{
			await this.Writer.WriteByteAsync( value ? Header.True : Header.False, cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Boolean? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			await this.Writer.WriteByteAsync( value ? Header.True : Header.False, cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Byte value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Byte? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( SByte value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( SByte? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int16 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int16? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt16 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt16? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int32 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int32? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt32 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt32? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int64 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Int64? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.Writer.WriteBytesAsync( Header.Int64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.SByte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Int32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Int64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt64 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( UInt64? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.Writer.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.Byte, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				await this.Writer.WriteBytesAsync( Header.UInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.UInt64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Single value, CancellationToken cancellationToken )
		{
			await this.Writer.WriteBytesAsync( Header.Single, unchecked( ( Single )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Single? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			await this.Writer.WriteBytesAsync( Header.Single, unchecked( ( Single )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Double value, CancellationToken cancellationToken )
		{
			await this.Writer.WriteBytesAsync( Header.Double, unchecked( ( Double )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackAsync( Double? nullable, CancellationToken cancellationToken )
		{
			if ( nullable == null )
			{
				await this.Writer.WriteByteAsync( Header.Nil, cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = nullable.GetValueOrDefault();

			await this.Writer.WriteBytesAsync( Header.Double, unchecked( ( Double )value ), cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackArrayHeaderAsync( uint count, CancellationToken cancellationToken )
		{
			if ( count < 0x10 )
			{
				await this.Writer.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | count ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( count < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Array16, unchecked( ( ushort )( count & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Array32, unchecked( ( uint )count ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackMapHeaderAsync( uint count, CancellationToken cancellationToken )
		{
			if ( count < 0x10 )
			{
				await this.Writer.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | count ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( count < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Map16, unchecked( ( ushort )( count & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Map32, unchecked( ( uint )count ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackStringHeaderAsync( uint count, CancellationToken cancellationToken )
		{
			if ( count < 0x20 )
			{
				await this.Writer.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | count ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( count < 0x100 && this.AllowStr8 )
			{
				await this.Writer.WriteBytesAsync( Header.Str8, unchecked( ( byte )( count & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( count < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Str16, unchecked( ( ushort )( count & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Str32, unchecked( ( uint )count ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackBinaryHeaderAsync( uint count, CancellationToken cancellationToken )
		{
			if ( count < 0x100 )
			{
				await this.Writer.WriteBytesAsync( Header.Bin8, unchecked( ( byte )( count & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( count < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Bin16, unchecked( ( ushort )( count & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Bin32, unchecked( ( uint )count ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackRawAsync( string value, CancellationToken cancellationToken )
		{
			await this.Writer.WriteBytesAsync( value, this.AllowStr8, cancellationToken ).ConfigureAwait( false );
		}

		public async Task PackRawAsync( byte[] value, CancellationToken cancellationToken )
		{
			if ( value.Length < 0x20 )
			{
				await this.Writer.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x100 && this.AllowStr8 )
			{
				await this.Writer.WriteBytesAsync( Header.Str8, unchecked( ( byte )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Str16, unchecked( ( ushort )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Str32, unchecked( ( uint )value.Length ), cancellationToken ).ConfigureAwait( false );
			await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackBinaryAsync( byte[] value, CancellationToken cancellationToken )
		{
			if ( value.Length < 0x100 )
			{
				await this.Writer.WriteBytesAsync( Header.Bin8, unchecked( ( byte )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				await this.Writer.WriteBytesAsync( Header.Bin16, unchecked( ( ushort )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.Writer.WriteBytesAsync( Header.Bin32, unchecked( ( uint )value.Length ), cancellationToken ).ConfigureAwait( false );
			await this.Writer.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
			return;
		}

		public async Task PackExtendedTypeValueAsync( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			unchecked
			{
				switch ( body.Length )
				{
					case 1:
					{
						await this.Writer.WriteByteAsync( Header.FixExt1, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 2:
					{
						await this.Writer.WriteByteAsync( Header.FixExt2, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 4:
					{
						await this.Writer.WriteByteAsync( Header.FixExt4, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 8:
					{
						await this.Writer.WriteByteAsync( Header.FixExt8, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 16:
					{
						await this.Writer.WriteByteAsync( Header.FixExt16, cancellationToken ).ConfigureAwait( false );
						break;
					}
					default:
					{
						if ( body.Length < 0x100 )
						{
							await this.Writer.WriteBytesAsync( Header.Ext8, ( byte )body.Length, cancellationToken ).ConfigureAwait( false );
						}
						else if ( body.Length < 0x10000 )
						{
							await this.Writer.WriteBytesAsync( Header.Ext16, ( ushort )body.Length, cancellationToken ).ConfigureAwait( false );
						}
						else
						{
							await this.Writer.WriteBytesAsync( Header.Ext32, ( uint )body.Length, cancellationToken ).ConfigureAwait( false );
						}

						break;
					}
				} // switch
			} // unchecked

			await this.Writer.WriteByteAsync( typeCode, cancellationToken ).ConfigureAwait( false );
			await this.Writer.WriteBytesAsync( body, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP
	}
}
