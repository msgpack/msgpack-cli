
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from MessagePackStreamPacker.Pack.tt and MessagePackPackerCommon.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackStreamPacker.Pack.tt  and MessagePackPackerCommon.ttinclude instead.

	partial class MessagePackStreamPacker
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

#if FEATURE_TAP

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

#endif // FEATURE_TAP

		private void WriteBytes( byte header, byte value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( byte ) + 1 );
		}

		private void WriteBytes( byte header, ushort value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( ushort ) + 1 );
		}

		private void WriteBytes( byte header, uint value )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( uint ) + 1 );
		}

		private void WriteBytes( byte header, ulong value )
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

		private void WriteBytes( byte header, float value )
		{
			var bits = Binary.ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits & 0xFF ) );
			this.WriteBytes( this._scalarBuffer, 0, sizeof( float ) + 1 );
		}

		private void WriteBytes( byte header, double value )
		{
			var bits = Binary.ToBits( value );
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

		private void WriteBytes( string value, bool allowStr8 )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			this.WriteStringHeader( encodedLength, allowStr8 );
			if ( encodedLength == 0 )
			{
				return;
			}

			this.WriteStringBody( value );
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
				this.WriteStringBody( chars, copying );
				offset += copying;
			}
		}

		private void WriteStringBody( char[] value, int remainingCharsLength )
		{

#else

		private void WriteStringBody( string value )
		{
			var remainingCharsLength = value.Length;

#endif // !FEATURE_POINTER_CONVERSION

			var buffer = BufferManager.NewByteBuffer( value.Length * 4 );
			var encoder = Encoding.UTF8.GetEncoder();
			var valueOffset = 0;
			
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

		private async Task WriteBytesAsync( byte header, byte value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( byte ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		private async Task WriteBytesAsync( byte header, ushort value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( ushort ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		private async Task WriteBytesAsync( byte header, uint value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( value >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( value >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( value >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( value & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( uint ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		private async Task WriteBytesAsync( byte header, ulong value, CancellationToken cancellationToken )
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

		private async Task WriteBytesAsync( byte header, float value, CancellationToken cancellationToken )
		{
			var bits = Binary.ToBits( value );
			this._scalarBuffer[ 0 ] = header;
			this._scalarBuffer[ 1 ] = unchecked( ( byte )( bits >> 24 & 0xFF ) );
			this._scalarBuffer[ 2 ] = unchecked( ( byte )( bits >> 16 & 0xFF ) );
			this._scalarBuffer[ 3 ] = unchecked( ( byte )( bits >> 8 & 0xFF ) );
			this._scalarBuffer[ 4 ] = unchecked( ( byte )( bits & 0xFF ) );
			await this.WriteBytesAsync( this._scalarBuffer, 0, sizeof( float ) + 1, cancellationToken ).ConfigureAwait( false );
		}

		private async Task WriteBytesAsync( byte header, double value, CancellationToken cancellationToken )
		{
			var bits = Binary.ToBits( value );
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

		private async Task WriteBytesAsync( string value, bool allowStr8, CancellationToken cancellationToken )
		{
			var encodedLength = Encoding.UTF8.GetByteCount( value );
			await this.WriteStringHeaderAsync( encodedLength, allowStr8, cancellationToken ).ConfigureAwait( false );
			if ( encodedLength == 0 )
			{
				return;
			}

			await this.WriteStringBodyAsync( value, cancellationToken ).ConfigureAwait( false );
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
				await this.WriteStringBodyAsync( chars, copying, cancellationToken ).ConfigureAwait( false );
				offset += copying;
			}
		}

		private async Task WriteStringBodyAsync( char[] value, int remainingCharsLength, CancellationToken cancellationToken )
		{

#else

		private async Task WriteStringBodyAsync( string value, CancellationToken cancellationToken )
		{
			var remainingCharsLength = value.Length;

#endif // !FEATURE_POINTER_CONVERSION

			var buffer = BufferManager.NewByteBuffer( value.Length * 4 );
			var encoder = Encoding.UTF8.GetEncoder();
			var valueOffset = 0;
			
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
