 
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
using System.Text;

namespace MsgPack
{
	// This file was generated from Packer.Packing.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit Packer.Packing.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class Packer
	{
		#region -- Int16 --

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( Int16 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( Int16 value )
		{
			if ( this.TryPackTinySignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackInt8( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackInt16( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="Int16"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int16"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt16( Int64 value )
		{
			if ( value < Int16.MinValue || value > Int16.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.SignedInt16 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- Int16 --

		#region -- UInt16 --

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt16 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( UInt16 value )
		{
			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackUInt16( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="UInt16"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt16"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		protected bool TryPackUInt16( UInt64 value )
		{
			if ( value > UInt16.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt16 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- UInt16 --

		#region -- Int32 --

		/// <summary>
		///		Packs <see cref="Int32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( Int32 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( Int32 value )
		{
			if ( this.TryPackTinySignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackInt8( value ) )
			{
				return;
			}

			if ( this.TryPackInt16( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackInt32( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="Int32"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int32"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt32( Int64 value )
		{
			if ( value < Int32.MinValue || value > Int32.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.SignedInt32 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- Int32 --

		#region -- UInt32 --

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt32 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( UInt32 value )
		{
			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return;
			}

			if ( this.TryPackUInt16( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackUInt32( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="UInt32"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt32"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		protected bool TryPackUInt32( UInt64 value )
		{
			if ( value > UInt32.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt32 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- UInt32 --

		#region -- Int64 --

		/// <summary>
		///		Packs <see cref="Int64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( Int64 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( Int64 value )
		{
			if ( this.TryPackTinySignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackInt8( value ) )
			{
				return;
			}

			if ( this.TryPackInt16( value ) )
			{
				return;
			}

			if ( this.TryPackInt32( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackInt64( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="Int64"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int64"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt64( Int64 value )
		{

			this.WriteByte( MessagePackCode.SignedInt64 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 56 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 48 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 40 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 32 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- Int64 --

		#region -- UInt64 --

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt64 value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( UInt64 value )
		{
			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return;
			}

			if ( this.TryPackUInt16( value ) )
			{
				return;
			}

			if ( this.TryPackUInt32( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackUInt64( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="UInt64"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt64"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		protected bool TryPackUInt64( UInt64 value )
		{

			this.WriteByte( MessagePackCode.UnsignedInt64 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 56 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 48 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 40 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 32 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( value & 0xFF ) );
			}
			return true;
		}

		#endregion -- UInt64 --

		#region -- Single --

		/// <summary>
		///		Packs <see cref="Single"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( float value )
		{
			this.VerifyNotDisposed();
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( float value )
		{
			this.WriteByte( MessagePackCode.Real32 );

			var bits = new Float32Bits( value );

			if ( BitConverter.IsLittleEndian )
			{
				this.WriteByte( bits.Byte3 );
				this.WriteByte( bits.Byte2 );
				this.WriteByte( bits.Byte1 );
				this.WriteByte( bits.Byte0 );
			}
			else
			{
				this.WriteByte( bits.Byte0 );
				this.WriteByte( bits.Byte1 );
				this.WriteByte( bits.Byte2 );
				this.WriteByte( bits.Byte3 );
			}
		}

		#endregion -- Single --

		#region -- Double --

		/// <summary>
		///		Packs <see cref="Double"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( double value )
		{
			this.VerifyNotDisposed();
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( double value )
		{
			this.WriteByte( MessagePackCode.Real64 );
			unchecked
			{
				long bits = BitConverter.DoubleToInt64Bits( value );
				this.WriteByte( ( byte )( ( bits >> 56 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 48 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 40 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 32 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 24 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 16 ) & 0xFF ) );
				this.WriteByte( ( byte )( ( bits >> 8 ) & 0xFF ) );
				this.WriteByte( ( byte )( bits & 0xFF ) );
			}
		}

		#endregion -- Double --

		#region -- Collection Header --

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackArrayHeader( int count )
		{
			this.PackArrayHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <returns>This instance.</returns>
		protected void PackArrayHeaderCore( int count )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackArrayHeaderCore( count );
		}

		private void PrivatePackArrayHeaderCore( int count )
		{
#if !UNITY
			Contract.Assert( 0 <= count, "0 <= count" );
#endif // !UNITY
			if ( count < 16 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | count ) ) );
			}
			else if ( count <= UInt16.MaxValue )
			{
				this.WriteByte( MessagePackCode.Array16 );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( count & 0xFF ) );
				}
			}
			else
			{
				this.WriteByte( MessagePackCode.Array32 );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 24 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( count >> 16 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( count & 0xFF ) );
				}
			}
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackMapHeader( int count )
		{
			this.PackMapHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <returns>This instance.</returns>
		protected void PackMapHeaderCore( int count )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackMapHeaderCore( count );
		}

		private void PrivatePackMapHeaderCore( int count )
		{
#if !UNITY
			Contract.Assert( 0 <= count, "0 <= count" );
#endif // !UNITY
			if ( count < 16 )
			{
				this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | count ) ) );
			}
			else if ( count <= UInt16.MaxValue )
			{
				this.WriteByte( MessagePackCode.Map16 );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( count & 0xFF ) );
				}
			}
			else
			{
				this.WriteByte( MessagePackCode.Map32 );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 24 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( count >> 16 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( count & 0xFF ) );
				}
			}
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method effectively acts as alias of <see cref="PackStringHeader"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeader(Int32) or Use PackBinaryHeader(Int32) instead." )]
		public Packer PackRawHeader( int length )
		{
			this.PackRawHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStringHeader( int length )
		{
			this.PackStringHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackBinaryHeader( int length )
		{
			this.PackBinaryHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <remarks>
		///		This method acts as alias of <see cref="PackStringHeaderCore"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeaderCore(Int32) or Use PackBinaryHeaderCore(Int32) instead." )]
		protected void PackRawHeaderCore( int length )
		{
			this.PackStringHeaderCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		protected void PackStringHeaderCore( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackRawHeaderCore( length, true );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		protected void PackBinaryHeaderCore( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackRawHeaderCore( length, false );
		}

		private void PrivatePackRawHeaderCore( int length, bool isString )
		{
#if !UNITY
			Contract.Assert( 0 <= length, "0 <= length" );
#endif // !UNITY

			if ( isString || ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) != 0 )
			{
				if ( length < 32 )
				{
					this.WriteByte( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | length ) ) );
					return;
				}

				if ( length <= Byte.MaxValue && ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					this.WriteByte( MessagePackCode.Str8 );
					unchecked
					{
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
				else if ( length <= UInt16.MaxValue )
				{
					this.WriteByte( MessagePackCode.Str16 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xFF ) );
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
				else
				{
					this.WriteByte( MessagePackCode.Str32 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 24 ) & 0xFF ) );
						this.WriteByte( ( byte )( ( length >> 16 ) & 0xFF ) );
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xFF ) );
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
			}
			else
			{
				// !isString && compat options is not set.

				if ( length <= Byte.MaxValue )
				{
					this.WriteByte( MessagePackCode.Bin8 );
					unchecked
					{
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
				else if ( length <= UInt16.MaxValue )
				{
					this.WriteByte( MessagePackCode.Bin16 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xFF ) );
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
				else
				{
					this.WriteByte( MessagePackCode.Bin32 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 24 ) & 0xFF ) );
						this.WriteByte( ( byte )( ( length >> 16 ) & 0xFF ) );
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xFF ) );
						this.WriteByte( ( byte )( length & 0xFF ) );
					}
				}
			}
		}

		#endregion -- Collection Header --

		#region -- Raw with Header --

		/// <summary>
		///		Packs specified byte sequence(it may or may not be string to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Packer PackRaw( IEnumerable<byte> value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			var asCollection = value as ICollection<byte>;
			if ( asCollection == null )
			{
				this.PrivatePackRaw( value );
			}
			else
			{
				this.PrivatePackRaw( asCollection );
			}

			return this;
		}

		/// <summary>
		///		Packs specified byte collection(it may or may not be string to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Packer PackRaw( IList<byte> value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			var asByteArray = value as byte[];
			if ( asByteArray == null )
			{
				this.PrivatePackRaw( value );
			}
			else
			{
				this.PrivatePackRaw( asByteArray );
			}

			return this;
		}

		/// <summary>
		///		Packs specified byte array(it may or may not be string to current stream.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Packer PackRaw( byte[] value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.PrivatePackRaw( value );
			return this;
		}

		private void PrivatePackRaw( ICollection<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackRawHeaderCore( value.Count, /*isString:*/ true );
			this.WriteBytes( value );
		}

		private void PrivatePackRaw( byte[] value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackRawCore( value, false );
		}

		private void PrivatePackRaw( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackRawCore( value );
		}

		private void PrivatePackRawCore( byte[] value, bool isImmutable )
		{
			this.PrivatePackRawHeaderCore( value.Length, /*isString:*/ true );
			this.WriteBytes( value, isImmutable );
		}

		private void PrivatePackRawCore( IEnumerable<byte> value )
		{
			if ( !this.CanSeek )
			{
				// buffered
				this.PrivatePackRawCore( value.ToArray(), true );
			}
			else
			{
				// Header
				this.WriteByte( MessagePackCode.Raw32 );
				this.StreamWrite( value, ( items, _ ) => this.PrivatePackRawBodyCore( items ), null );
			}
		}

		#endregion -- Raw with Header --

		#region -- Binary with Header --

		/// <summary>
		///		Packs specified byte sequence(it should not be string to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Packer PackBinary( IEnumerable<byte> value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			var asCollection = value as ICollection<byte>;
			if ( asCollection == null )
			{
				this.PrivatePackBinary( value );
			}
			else
			{
				this.PrivatePackBinary( asCollection );
			}

			return this;
		}

		/// <summary>
		///		Packs specified byte collection(it should not be string to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Packer PackBinary( IList<byte> value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			var asByteArray = value as byte[];
			if ( asByteArray == null )
			{
				this.PrivatePackBinary( value );
			}
			else
			{
				this.PrivatePackBinary( asByteArray );
			}

			return this;
		}

		/// <summary>
		///		Packs specified byte array(it should not be string to current stream.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Packer PackBinary( byte[] value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.PrivatePackBinary( value );
			return this;
		}

		private void PrivatePackBinary( ICollection<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackRawHeaderCore( value.Count, /*isString:*/ false );
			this.WriteBytes( value );
		}

		private void PrivatePackBinary( byte[] value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackBinaryCore( value, false );
		}

		private void PrivatePackBinary( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackBinaryCore( value );
		}

		private void PrivatePackBinaryCore( byte[] value, bool isImmutable )
		{
			this.PrivatePackRawHeaderCore( value.Length, /*isString:*/ false );
			this.WriteBytes( value, isImmutable );
		}

		private void PrivatePackBinaryCore( IEnumerable<byte> value )
		{
			if ( !this.CanSeek )
			{
				// buffered
				this.PrivatePackBinaryCore( value.ToArray(), true );
			}
			else
			{
				// Header
				// Use biggest data size because actual binary length is not known.
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) != 0 )
				{
					this.WriteByte( MessagePackCode.Raw32 );
				}
				else
				{
					this.WriteByte( MessagePackCode.Bin32 );
				}

				this.StreamWrite( value, ( items, _ ) => this.PrivatePackRawBodyCore( items ), null );
			}
		}

		#endregion -- Binary with Header --

		#region -- String with Header  --

		/// <summary>
		///		Packs specified charactor sequence to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( IEnumerable<char> value )
		{
			this.PackStringCore( value, Encoding.UTF8 );
			return this;
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( IEnumerable<char> value, Encoding encoding )
		{
			this.PackStringCore( value, encoding );
			return this;
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected virtual void PackStringCore( IEnumerable<char> value, Encoding encoding )
		{
			if ( encoding == null )
			{
				ThrowArgumentNullException( "encoding ");
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackString( value, encoding );
		}

		private void PrivatePackString( IEnumerable<char> value, Encoding encoding )
		{
#if !UNITY
			Contract.Assert( encoding != null, "encoding != null" );
#endif // !UNITY

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackStringCore( value, encoding );
		}

		private void PrivatePackStringCore( IEnumerable<char> value, Encoding encoding )
		{
#if !UNITY
			Contract.Assert( value != null, "value != null" );
			Contract.Assert( encoding != null, "encoding != null" );
#endif // !UNITY

			// TODO: streaming encoding
			var encoded = encoding.GetBytes( value.ToArray() );
			this.PrivatePackRawHeaderCore( encoded.Length, /*isString:*/ true );
			this.WriteBytes( encoded, true );
		}

		/// <summary>
		///		Packs specified string to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value )
		{
			this.PackStringCore( value, Encoding.UTF8 );
			return this;
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value, Encoding encoding )
		{
			this.PackStringCore( value, encoding );
			return this;
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected virtual void PackStringCore( string value, Encoding encoding )
		{
			if ( encoding == null )
			{
				ThrowArgumentNullException( "encoding ");
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackString( value, encoding );
		}

		private void PrivatePackString( string value, Encoding encoding )
		{
#if !UNITY
			Contract.Assert( encoding != null, "encoding != null" );
#endif // !UNITY

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackStringCore( value, encoding );
		}

		private void PrivatePackStringCore( string value, Encoding encoding )
		{
#if !UNITY
			Contract.Assert( value != null, "value != null" );
			Contract.Assert( encoding != null, "encoding != null" );
#endif // !UNITY

			// TODO: streaming encoding
			var encoded = encoding.GetBytes( value );
			this.PrivatePackRawHeaderCore( encoded.Length, /*isString:*/ true );
			this.WriteBytes( encoded, true );
		}

		#endregion -- String with Header  --

		#region -- Raw Body --

		/// <summary>
		///		Packs specified byte array to current stream without any header.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Packer PackRawBody( byte[] value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.WriteBytes( value, false );
			return this;
		}

		/// <summary>
		///		Packs specified byte sequence to current stream without any header.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Packer PackRawBody( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.PrivatePackRawBodyCore( value );
			return this;
		}

		private int PrivatePackRawBodyCore( IEnumerable<byte> value )
		{
#if !UNITY
			Contract.Assert( value != null, "value != null" );
#endif // !UNITY

			var asCollection = value as ICollection<byte>;
			if ( asCollection != null )
			{
				return this.PrivatePackRawBodyCore( asCollection, asCollection.IsReadOnly );
			}

			int bodyLength = 0;

			foreach ( var b in value )
			{
				this.WriteByte( b );
				bodyLength++;
			}

			return bodyLength;
		}

		private int PrivatePackRawBodyCore( ICollection<byte> value, bool isImmutable )
		{
#if !UNITY
			Contract.Assert( value != null, "value != null" );
#endif // !UNITY

			var asArray = value as byte[];
			if ( asArray != null )
			{
				this.WriteBytes( asArray, isImmutable );
			}
			else
			{
				this.WriteBytes( value );
			}

			return value.Count;
		}

		#endregion -- Raw Body --
	
		#region -- IList --

		/// <summary>
		///		Bookkeep collection count to be packed on current stream.
		/// </summary>
		/// <param name="array">Collection count to be written.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackArrayHeader<TItem>( IList<TItem> array )
		{
			return array == null ? this.PackNull() : this.PackArrayHeader( array.Count );
		}

		#endregion -- IList --

		#region -- IDictionary --

		/// <summary>
		///		Bookkeep dictionary count to be packed on current stream.
		/// </summary>
		/// <param name="map">Dictionary count to be written.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackMapHeader<TKey, TValue>( IDictionary<TKey, TValue> map )
		{
			return map == null ? this.PackNull() : this.PackMapHeader( map.Count );
		}

		#endregion -- IDictionary --

		#region -- Ext --

		/// <summary>
		/// Packs an extended type value.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		/// <returns>This instance. </returns>
		/// <exception cref="ArgumentNullException"><paramref name="body"/> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackExtendedTypeValue( byte typeCode, byte[] body )
		{
			if ( body == null )
			{
				throw new ArgumentNullException( "body" );
			}

			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.PrivatePackExtendedTypeValueCore( typeCode, body );
			return this;
		}

		/// <summary>
		///		Packs an extended type value.
		/// </summary>
		/// <param name="mpeto">A <see cref="MessagePackExtendedTypeObject"/> to be packed.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ArgumentException"><see cref="MessagePackExtendedTypeObject.IsValid"/> of <paramref name="mpeto"/> is <c>false</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackExtendedTypeValue( MessagePackExtendedTypeObject mpeto )
		{
			if ( !mpeto.IsValid )
			{
				throw new ArgumentException( "MessagePackExtendedTypeObject must have body.", "mpeto" );
			}

			this.PrivatePackExtendedTypeValueCore( mpeto.TypeCode, mpeto.Body );
			return this;
		}

		private void PrivatePackExtendedTypeValueCore( byte typeCode, byte[] body )
		{
			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) != 0 )
			{
				throw new InvalidOperationException( "ExtendedTypeObject is prohibited in this packer." );
			}

			switch ( body.Length )
			{
				case 1:
				{
					this.WriteByte( MessagePackCode.FixExt1 );
					break;
				}
				case 2:
				{
					this.WriteByte( MessagePackCode.FixExt2 );
					break;
				}
				case 4:
				{
					this.WriteByte( MessagePackCode.FixExt4 );
					break;
				}
				case 8:
				{
					this.WriteByte( MessagePackCode.FixExt8 );
					break;
				}
				case 16:
				{
					this.WriteByte( MessagePackCode.FixExt16 );
					break;
				}
				default:
				{
					unchecked
					{
						if ( body.Length < 0x100 )
						{
							this.WriteByte( MessagePackCode.Ext8 );
							this.WriteByte( ( byte )( body.Length & 0xFF ) );
						}
						else if ( body.Length < 0x10000 )
						{
							this.WriteByte( MessagePackCode.Ext16 );
							this.WriteByte( ( byte )( ( body.Length >> 8 ) & 0xFF ) );
							this.WriteByte( ( byte )( body.Length & 0xFF ) );
						}
						else
						{
							this.WriteByte( MessagePackCode.Ext32 );
							this.WriteByte( ( byte )( ( body.Length >> 24 ) & 0xFF ) );
							this.WriteByte( ( byte )( ( body.Length >> 16 ) & 0xFF ) );
							this.WriteByte( ( byte )( ( body.Length >> 8 ) & 0xFF ) );
							this.WriteByte( ( byte )( body.Length & 0xFF ) );
						}
					}

					break;
				}
			} // switch

			this.WriteByte( typeCode );
			this.WriteBytes( body, true );
		}

		#endregion -- Ext --

		private void StreamWrite<TItem>( IEnumerable<TItem> value, Action<IEnumerable<TItem>, PackingOptions> writeBody, PackingOptions options )
		{
			if ( this.CanSeek )
			{
				// Reserve length
				this.SeekTo( 4L );
				var headerPosition = this.Position;
				// Write body
				writeBody( value, options );
				var bodyLength = this.Position - headerPosition;
				// Back to reserved length
				this.SeekTo( -bodyLength );
				this.SeekTo( -4L );
				unchecked
				{
					this.WriteByte( ( byte )( ( bodyLength >> 24 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( bodyLength >> 16 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( bodyLength >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( bodyLength & 0xFF ) );
				}
				// Forward to body tail
				this.SeekTo( bodyLength );
			}
			else
			{
				// Copying is better than forcing stream is seekable...
				var asCollection = value as ICollection<TItem> ?? value.ToArray();

				var bodyLength = asCollection.Count;
				unchecked
				{
					this.WriteByte( ( byte )( ( bodyLength >> 24 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( bodyLength >> 16 ) & 0xFF ) );
					this.WriteByte( ( byte )( ( bodyLength >> 8 ) & 0xFF ) );
					this.WriteByte( ( byte )( bodyLength & 0xFF ) );
				}

				writeBody( asCollection, options );
			}
		}
	}
}
