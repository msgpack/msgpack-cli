
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
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from MessagePackByteArrayPacker.Pack.tt and MessagePackPackerCommon.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackByteArrayPacker.Pack.tt  and MessagePackPackerCommon.ttinclude instead.

	partial class MessagePackByteArrayPacker
	{
		protected override void PackCore( Boolean value )
		{
			this.WriteByte( value ? ( byte )MessagePackCode.TrueValue : ( byte )MessagePackCode.FalseValue );
		}

		protected override void PackCore( Byte value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )value ) );
		}

		protected override void PackCore( SByte value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )value ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )value ) );
		}

		protected override void PackCore( Int16 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )value ) );
		}

		protected override void PackCore( UInt16 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )value ) );
		}

		protected override void PackCore( Int32 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				this.WriteBytes( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )value ) );
		}

		protected override void PackCore( UInt32 value )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.UnsignedInt32, unchecked( ( UInt32 )value ) );
		}

		protected override void PackCore( Int64 value )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					this.WriteBytes( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
					return;
				}

				this.WriteBytes( ( byte )MessagePackCode.SignedInt64, unchecked( ( UInt64 )value ) );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.SignedInt64, unchecked( ( UInt64 )value ) );
		}

		protected override void PackCore( UInt64 value )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				this.WriteByte( unchecked( ( byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ) );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ) );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				this.WriteBytes( ( byte )MessagePackCode.UnsignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.UnsignedInt64, unchecked( ( UInt64 )value ) );
		}

		protected override void PackCore( Single value )
		{
			this.WriteBytes( ( byte )MessagePackCode.Real32, unchecked( ( Single )value ) );
		}

		protected override void PackCore( Double value )
		{
			this.WriteBytes( ( byte )MessagePackCode.Real64, unchecked( ( Double )value ) );
		}

		protected override void PackArrayHeaderCore( int length )
		{
			if ( length < 0x10 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | length ) ) );
				return;
			}

			if ( length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Array16, unchecked( ( ushort )( length & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Array32, unchecked( ( uint )length ) );
			return;
		}

		protected override void PackMapHeaderCore( int length )
		{
			if ( length < 0x10 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | length ) ) );
				return;
			}

			if ( length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Map16, unchecked( ( ushort )( length & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Map32, unchecked( ( uint )length ) );
			return;
		}

		protected override void PackStringHeaderCore( int length )
		{
			if ( length < 0x20 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | length ) ) );
				return;
			}

			if ( length < 0x100 && ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Str8, unchecked( ( byte )( length & 0xFF ) ) );
				return;
			}

			if ( length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Str16, unchecked( ( ushort )( length & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Str32, unchecked( ( uint )length ) );
			return;
		}

		protected override void PackBinaryHeaderCore( int length )
		{
			if ( length < 0x100 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Bin8, unchecked( ( byte )( length & 0xFF ) ) );
				return;
			}

			if ( length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Bin16, unchecked( ( ushort )( length & 0xFFFF ) ) );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Bin32, unchecked( ( uint )length ) );
			return;
		}

		protected override void PackRawCore( string value )
		{
			this.WriteBytes( value, ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 );
		}

		protected override void PackRawCore( byte[] value )
		{
			if ( value.Length < 0x20 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | value.Length ) ) );
				this.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x100 && ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Str8, unchecked( ( byte )( value.Length ) ) );
				this.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Str16, unchecked( ( ushort )( value.Length ) ) );
				this.WriteBytes( value );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Str32, unchecked( ( uint )value.Length ) );
			this.WriteBytes( value );
			return;
		}

		protected override void PackBinaryCore( byte[] value )
		{
			if ( value.Length < 0x100 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Bin8, unchecked( ( byte )( value.Length ) ) );
				this.WriteBytes( value );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				this.WriteBytes( ( byte )MessagePackCode.Bin16, unchecked( ( ushort )( value.Length ) ) );
				this.WriteBytes( value );
				return;
			}

			this.WriteBytes( ( byte )MessagePackCode.Bin32, unchecked( ( uint )value.Length ) );
			this.WriteBytes( value );
			return;
		}

		protected override void PackExtendedTypeValueCore( byte typeCode, byte[] body )
		{
			unchecked
			{
				switch ( body.Length )
				{
					case 1:
					{
						this.WriteByte( ( byte )MessagePackCode.FixExt1 );
						break;
					}
					case 2:
					{
						this.WriteByte( ( byte )MessagePackCode.FixExt2 );
						break;
					}
					case 4:
					{
						this.WriteByte( ( byte )MessagePackCode.FixExt4 );
						break;
					}
					case 8:
					{
						this.WriteByte( ( byte )MessagePackCode.FixExt8 );
						break;
					}
					case 16:
					{
						this.WriteByte( ( byte )MessagePackCode.FixExt16 );
						break;
					}
					default:
					{
						if ( body.Length < 0x100 )
						{
							this.WriteBytes( ( byte )MessagePackCode.Ext8, ( byte )body.Length );
						}
						else if ( body.Length < 0x10000 )
						{
							this.WriteBytes( ( byte )MessagePackCode.Ext16, ( ushort )body.Length );
						}
						else
						{
							this.WriteBytes( ( byte )MessagePackCode.Ext32, ( uint )body.Length );
						}

						break;
					}
				} // switch
			} // unchecked

			this.WriteByte( typeCode );
			this.WriteBytes( body );
		}

#if FEATURE_TAP

		protected override async Task PackAsyncCore( Boolean value, CancellationToken cancellationToken )
		{
			await this.WriteByteAsync( value ? ( byte )MessagePackCode.TrueValue : ( byte )MessagePackCode.FalseValue, cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Byte value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( SByte value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Int16 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( UInt16 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Int32 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & 0xFFFFFFE0 ) == 0 )
			{
				// Negative fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( UInt32 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x0000007F ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt32, unchecked( ( UInt32 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Int64 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( ( ~value & unchecked( ( long )0xFFFFFFFFFFFFFFE0 ) ) == 0 )
			{
				// Negative fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value < 0 )
			{
				if ( value >= SByte.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int16.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				if ( value >= Int32.MinValue )
				{
					await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
					return;
				}

				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= SByte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int16.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Int32.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.SignedInt64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( UInt64 value, CancellationToken cancellationToken )
		{
			if ( ( value & 0x000000000000007FL ) == value )
			{
				// Positive fix num
				await this.WriteByteAsync( unchecked( ( byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= Byte.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt8, unchecked( ( Byte )( value & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt16.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt16, unchecked( ( UInt16 )( value & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value <= UInt32.MaxValue )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt32, unchecked( ( UInt32 )( value & 0xFFFFFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.UnsignedInt64, unchecked( ( UInt64 )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Single value, CancellationToken cancellationToken )
		{
			await this.WriteBytesAsync( ( byte )MessagePackCode.Real32, unchecked( ( Single )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackAsyncCore( Double value, CancellationToken cancellationToken )
		{
			await this.WriteBytesAsync( ( byte )MessagePackCode.Real64, unchecked( ( Double )value ), cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackArrayHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			if ( length < 0x10 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | length ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Array16, unchecked( ( ushort )( length & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Array32, unchecked( ( uint )length ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackMapHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			if ( length < 0x10 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | length ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Map16, unchecked( ( ushort )( length & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Map32, unchecked( ( uint )length ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackStringHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			if ( length < 0x20 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | length ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( length < 0x100 && ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Str8, unchecked( ( byte )( length & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Str16, unchecked( ( ushort )( length & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Str32, unchecked( ( uint )length ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackBinaryHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			if ( length < 0x100 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Bin8, unchecked( ( byte )( length & 0xFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Bin16, unchecked( ( ushort )( length & 0xFFFF ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Bin32, unchecked( ( uint )length ), cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackRawAsyncCore( string value, CancellationToken cancellationToken )
		{
			await this.WriteBytesAsync( value, ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0, cancellationToken ).ConfigureAwait( false );
		}

		protected override async Task PackRawAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			if ( value.Length < 0x20 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x100 && ( this.CompatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Str8, unchecked( ( byte )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Str16, unchecked( ( ushort )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Str32, unchecked( ( uint )value.Length ), cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackBinaryAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			if ( value.Length < 0x100 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Bin8, unchecked( ( byte )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( value.Length < 0x10000 )
			{
				await this.WriteBytesAsync( ( byte )MessagePackCode.Bin16, unchecked( ( ushort )( value.Length ) ), cancellationToken ).ConfigureAwait( false );
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.WriteBytesAsync( ( byte )MessagePackCode.Bin32, unchecked( ( uint )value.Length ), cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
			return;
		}

		protected override async Task PackExtendedTypeValueAsyncCore( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			unchecked
			{
				switch ( body.Length )
				{
					case 1:
					{
						await this.WriteByteAsync( ( byte )MessagePackCode.FixExt1, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 2:
					{
						await this.WriteByteAsync( ( byte )MessagePackCode.FixExt2, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 4:
					{
						await this.WriteByteAsync( ( byte )MessagePackCode.FixExt4, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 8:
					{
						await this.WriteByteAsync( ( byte )MessagePackCode.FixExt8, cancellationToken ).ConfigureAwait( false );
						break;
					}
					case 16:
					{
						await this.WriteByteAsync( ( byte )MessagePackCode.FixExt16, cancellationToken ).ConfigureAwait( false );
						break;
					}
					default:
					{
						if ( body.Length < 0x100 )
						{
							await this.WriteBytesAsync( ( byte )MessagePackCode.Ext8, ( byte )body.Length, cancellationToken ).ConfigureAwait( false );
						}
						else if ( body.Length < 0x10000 )
						{
							await this.WriteBytesAsync( ( byte )MessagePackCode.Ext16, ( ushort )body.Length, cancellationToken ).ConfigureAwait( false );
						}
						else
						{
							await this.WriteBytesAsync( ( byte )MessagePackCode.Ext32, ( uint )body.Length, cancellationToken ).ConfigureAwait( false );
						}

						break;
					}
				} // switch
			} // unchecked

			await this.WriteByteAsync( typeCode, cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( body, cancellationToken ).ConfigureAwait( false );
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

			this.WriteBytes( MessagePackCode.Str32, unchecked( ( uint )bytesLength ) );
		}

		private void WriteBytes( byte header, byte value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( byte ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( byte ) - 1 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( byte );
		}

		private void WriteBytes( byte header, ushort value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( ushort ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ushort ) - 2 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( ushort );
		}

		private void WriteBytes( byte header, uint value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( uint ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( uint ) - 4 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( uint );
		}

		private void WriteBytes( byte header, ulong value )
		{
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( ulong ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 4 ) * 8 ) & 0xFF ) );
			buffer[ offset + 4 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 5 ) * 8 ) & 0xFF ) );
			buffer[ offset + 5 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 6 ) * 8 ) & 0xFF ) );
			buffer[ offset + 6 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 7 ) * 8 ) & 0xFF ) );
			buffer[ offset + 7 ] = unchecked( ( byte )( value >> ( ( sizeof( ulong ) - 8 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( ulong );
		}

		private void WriteBytes( byte header, float value )
		{
			var bits = Binary.ToBits( value );
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( float ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( float ) - 4 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( float );
		}

		private void WriteBytes( byte header, double value )
		{
			var bits = Binary.ToBits( value );
			var buffer = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;
			const int requiredSize = sizeof( double ) + 1;
			if ( remains < requiredSize && !this._allocator.TryAllocate( buffer, requiredSize, out buffer ) )
			{
				this.ThrowEofException( requiredSize );
			}

			buffer[ offset ] = header;
			offset++;

			buffer[ offset ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 1 ) * 8 ) & 0xFF ) );
			buffer[ offset + 1 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 2 ) * 8 ) & 0xFF ) );
			buffer[ offset + 2 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 3 ) * 8 ) & 0xFF ) );
			buffer[ offset + 3 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 4 ) * 8 ) & 0xFF ) );
			buffer[ offset + 4 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 5 ) * 8 ) & 0xFF ) );
			buffer[ offset + 5 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 6 ) * 8 ) & 0xFF ) );
			buffer[ offset + 6 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 7 ) * 8 ) & 0xFF ) );
			buffer[ offset + 7 ] = unchecked( ( byte )( bits >> ( ( sizeof( double ) - 8 ) * 8 ) & 0xFF ) );

			this._buffer = buffer;
			this._offset = offset + sizeof( double );
		}

		private void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}

			var buffer  = this._buffer;
			var offset = this._offset;
			var remains = buffer.Length - offset;

			if ( remains < encodedLength && !this._allocator.TryAllocate( buffer, encodedLength, out buffer ) )
			{
				this.ThrowEofException( encodedLength );
			}

			Encoding.UTF8.GetBytes( value, 0, value.Length, buffer, offset );
			this._buffer = buffer;
			this._offset += encodedLength;
		}

#if FEATURE_TAP

		private Task WriteBytesAsync( byte header, byte value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( byte header, ushort value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( byte header, uint value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( byte header, ulong value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( byte header, float value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( byte header, double value, CancellationToken cancellationToken )
		{
			this.WriteBytes( header, value );
			return TaskAugument.CompletedTask;
		}

		private Task WriteBytesAsync( string value, bool allowStr8, CancellationToken cancellationToken )
		{
			this.WriteBytes( value, allowStr8 );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP
	}
}
