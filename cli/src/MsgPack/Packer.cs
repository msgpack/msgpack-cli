#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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

// If you build this source for partial trust environment (such as Silverlight),
// define following symbol in compiler option or enabled following line.
// #define NOT_UNSAFE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MsgPack
{
	// TODO: Refactoring or use T4 template.

	/// <summary>
	///		Implements serialization feature of MsgPack.
	/// </summary>
	public abstract partial class Packer : IDisposable
	{
		private bool _isDisposed;

		/// <summary>
		///		Get whether this class supports seek operation and quering <see cref="Position"/> property.
		/// </summary>
		/// <value>If this class supports seek operation and quering <see cref="Position"/> property then true.</value>
		public virtual bool CanSeek
		{
			get { return false; }
		}

		/// <summary>
		///		Get current position of underlying stream.
		/// </summary>
		/// <value>Opaque position value of underlying stream.</value>
		/// <exception cref="NotSupportedException">
		///		A class of this instance does not support seek.
		/// </exception>
		public virtual long Position
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		///		Initialize new instance.
		/// </summary>
		protected Packer() { }

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instancde wrapping specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like <see cref="FileStream"/>, <see cref="MemoryStream"/>,
		///		 <see cref="System.Net.Sockets.NetworkStream"/>, <see cref="UnmanagedMemoryStream"/>, or so.
		/// </remarks>
		public static Packer Create( Stream stream )
		{
			return new StreamPacker( stream );
		}

		/// <summary>
		///		Clean up internal resources.
		/// </summary>
		public void Dispose()
		{
			if ( this._isDisposed )
			{
				return;
			}

			this.Dispose( true );
			GC.SuppressFinalize( this );
			this._isDisposed = true;
		}

		/// <summary>
		///		When overridden by derived class, release all unmanaged resources, optionally release managed resources.
		/// </summary>
		/// <param name="disposing">If true, release managed resources too.</param>
		protected virtual void Dispose( bool disposing ) { }

		private void VerifyNotDisposed()
		{
			if ( this._isDisposed )
			{
				throw new ObjectDisposedException( this.ToString() );
			}
		}

		/// <summary>
		///		When overridden by derived class, change current position to specified offset.
		/// </summary>
		/// <param name="offset">Offset. You shoud not specify the value which causes underflow or overflow.</param>
		/// <exception cref="NotSupportedException">
		///		A class of this instance does not support seek.
		/// </exception>
		protected virtual void SeekTo( long offset )
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///		When overridden by derived class, write specified byte to stream using implementation specific manner.
		/// </summary>
		/// <param name="value">A byte to be written.</param>
		protected abstract void WriteByte( byte value );

		private void StreamWrite<TItem>( IEnumerable<TItem> value, Action<IEnumerable<TItem>> writeBody )
		{
			Contract.Assert( this.CanSeek );

			var headerPosition = this.Position;
			// Reserve length
			this.SeekTo( 4L );
			// Write body
			writeBody( value );
			var bodyLength = this.Position - headerPosition;
			// Back to reserved length
			this.SeekTo( -bodyLength );
			unchecked
			{
				this.WriteByte( ( byte )( ( bodyLength >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bodyLength >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bodyLength >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( bodyLength & 0xff ) );
			}
			// Forward to body tail
			this.SeekTo( bodyLength );
		}

		#region -- Int8 --

		/// <summary>
		///		Pack <see cref="SByte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( sbyte value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinySignedInteger( value ) )
			{
				return this;
			}

			if ( value >= 0 && this.TryPackTinyUnsignedInteger( ( ulong )value ) )
			{
				return this;
			}

			var b = this.TryPackInt8( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="SByte"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for int8 value.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer PackStrict( sbyte value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackInt8( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="SByte"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="SByte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt8( long value )
		{
			if ( value > SByte.MaxValue || value < SByte.MinValue )
			{
				return false;
			}

			this.WriteByte( 0xd0 );
			this.WriteByte( ( byte )value );
			return true;
		}

		#endregion -- Int8 --

		#region -- UInt8 --

		/// <summary>
		///		Pack <see cref="Byte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( byte value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return this;
			}

			var b = this.TryPackUInt8( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="Byte"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for uint8 value.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer PackStrict( byte value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackUInt8( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="Byte"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Byte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		private bool TryPackUInt8( ulong value )
		{
			if ( value > Byte.MaxValue )
			{
				return false;
			}

			this.WriteByte( 0xcc );
			this.WriteByte( ( byte )value );
			return true;
		}

		#endregion -- UInt8 --

		#region -- Int16 --

		/// <summary>
		///		Pack <see cref="Int16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( short value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinySignedInteger( value ) )
			{
				return this;
			}

			if ( value >= 0 && this.TryPackTinyUnsignedInteger( ( ulong )value ) )
			{
				return this;
			}

			if ( this.TryPackInt8( value ) )
			{
				return this;
			}

			var b = this.TryPackInt16( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="Int16"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for int8 value.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStrict( short value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackInt16( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="Int16"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int16"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt16( long value )
		{
			if ( value < Int16.MinValue || value > Int16.MaxValue )
			{
				return false;
			}

			this.WriteByte( 0xd1 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- Int16 --

		#region -- UInt16 --

		/// <summary>
		///		Pack <see cref="UInt16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( ushort value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return this;
			}

			var b = this.TryPackUInt16( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="UInt16"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for uint16 value.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer PackStrict( ushort value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackUInt16( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="UInt16"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt16"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
		protected bool TryPackUInt16( ulong value )
		{
			if ( value > UInt16.MaxValue )
			{
				return false;
			}

			this.WriteByte( 0xcd );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- UInt16 --

		#region -- Int32 --

		/// <summary>
		///		Pack <see cref="Int32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( int value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinySignedInteger( value ) )
			{
				return this;
			}

			if ( value >= 0 && this.TryPackTinyUnsignedInteger( ( ulong )value ) )
			{
				return this;
			}

			if ( this.TryPackInt8( value ) )
			{
				return this;
			}

			if ( this.TryPackInt16( value ) )
			{
				return this;
			}

			var b = this.TryPackInt32( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="Int32"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for int32 value.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStrict( int value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackInt32( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="Int32"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int32"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt32( long value )
		{
			if ( value > Int32.MaxValue || value < Int32.MinValue )
			{
				return false;
			}

			this.WriteByte( 0xd2 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- Int32 --

		#region -- UInt32 --

		/// <summary>
		///		Pack <see cref="UInt32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( uint value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt16( value ) )
			{
				return this;
			}

			var b = this.TryPackUInt32( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="UInt32"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for uint32 value.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer PackStrict( uint value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackUInt32( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="UInt32"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt32"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
		protected bool TryPackUInt32( ulong value )
		{
			if ( value > UInt32.MaxValue )
			{
				return false;
			}

			this.WriteByte( 0xce );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- UInt32 --

		#region -- Int64 --

		/// <summary>
		///		Pack <see cref="Int64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( long value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinySignedInteger( value ) )
			{
				return this;
			}

			if ( value >= 0 && this.TryPackTinyUnsignedInteger( ( ulong )value ) )
			{
				return this;
			}

			if ( this.TryPackInt8( value ) )
			{
				return this;
			}

			if ( this.TryPackInt16( value ) )
			{
				return this;
			}

			if ( this.TryPackInt32( value ) )
			{
				return this;
			}

			var b = this.TryPackInt64( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="Int64"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for int64 value.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStrict( long value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackInt64( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="Int64"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int64"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt64( long value )
		{
			this.WriteByte( 0xd3 );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 56 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 48 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 40 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 32 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- Int64 --

		#region -- UInt64 --

		/// <summary>
		///		Pack <see cref="UInt64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( ulong value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt8( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt16( value ) )
			{
				return this;
			}

			if ( this.TryPackUInt32( value ) )
			{
				return this;
			}

			var b = this.TryPackUInt64( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Pack <see cref="UInt64"/> value to current stream strictly.
		///		This method will not compact packed binary and use redundant representation for uint64 value.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer PackStrict( ulong value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var b = this.TryPackUInt64( value );
			Debug.Assert( b );
			return this;
		}

		/// <summary>
		///		Try pack <see cref="UInt64"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt64"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
		protected bool TryPackUInt64( ulong value )
		{
			this.WriteByte( 0xcf );
			unchecked
			{
				this.WriteByte( ( byte )( ( value >> 56 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 48 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 40 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 32 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( value >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( value & 0xff ) );
			}
			return true;
		}

		#endregion -- UInt64 --

		#region -- Single --

		/// <summary>
		///		Pack <see cref="Single"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( float value )
		{
			this.VerifyNotDisposed();

			this.WriteByte( 0xca );

#if NOT_UNSAFE

			var littleEndeanBytes = BitConverter.GetBytes( value );
			if ( BitConverter.IsLittleEndian )
			{
				this.WriteByte( littleEndeanBytes[ 3 ] );
				this.WriteByte( littleEndeanBytes[ 2 ] );
				this.WriteByte( littleEndeanBytes[ 1 ] );
				this.WriteByte( littleEndeanBytes[ 0 ] );
			}
			else
			{
				this.WriteByte( littleEndeanBytes[ 0 ] );
				this.WriteByte( littleEndeanBytes[ 1 ] );
				this.WriteByte( littleEndeanBytes[ 2 ] );
				this.WriteByte( littleEndeanBytes[ 3 ] );
			}
#else
			unsafe
			{
				byte* pValue = ( byte* )&value;
				if ( BitConverter.IsLittleEndian )
				{
					this.WriteByte( pValue[ 3 ] );
					this.WriteByte( pValue[ 2 ] );
					this.WriteByte( pValue[ 1 ] );
					this.WriteByte( pValue[ 0 ] );
				}
				else
				{
					this.WriteByte( pValue[ 0 ] );
					this.WriteByte( pValue[ 1 ] );
					this.WriteByte( pValue[ 2 ] );
					this.WriteByte( pValue[ 3 ] );
				}
			}
#endif
			return this;
		}

		#endregion -- Single --

		#region -- Double --

		/// <summary>
		///		Pack <see cref="Double"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( double value )
		{
			this.VerifyNotDisposed();

			this.WriteByte( 0xcb );
			unchecked
			{
				long bits = BitConverter.DoubleToInt64Bits( value );
				this.WriteByte( ( byte )( ( bits >> 56 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 48 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 40 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 32 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( bits & 0xff ) );
			}

			return this;
		}

		#endregion -- Double --

		#region -- Boolean --

		/// <summary>
		///		Pack <see cref="Boolean"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( bool value )
		{
			this.VerifyNotDisposed();

			this.WriteByte( value ? ( byte )0xc3 : ( byte )0xc2 );
			return this;
		}

		#endregion -- Boolean --

		#region -- Collection Header --

		/// <summary>
		///		Bookkeep array length to be packed on current stream.
		/// </summary>
		/// <param name="count">Array length.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackArrayHeader( int count )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackArrayHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep array length to be packed on current stream.
		/// </summary>
		/// <param name="count">Array length.</param>
		/// <returns>This instance.</returns>
		protected void PackArrayHeaderCore( int count )
		{
			if ( count < 0 )
			{
				throw new ArgumentOutOfRangeException( "count", count, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "count" ) );
			}
			else if ( count < 16 )
			{
				this.WriteByte( unchecked( ( byte )( 0x90 | count ) ) );
			}
			else if ( count <= UInt16.MaxValue )
			{
				this.WriteByte( 0xdc );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( count & 0xff ) );
				}
			}
			else
			{
				this.WriteByte( 0xdd );
				this.WriteByte( ( byte )( ( count >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( count >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( count & 0xff ) );
			}
		}

		/// <summary>
		///		Bookkeep dictionary items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary items count.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackMapHeader( int count )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackMapHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep dictionary items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary items count.</param>
		/// <returns>This instance.</returns>
		protected void PackMapHeaderCore( int count )
		{
			if ( count < 0 )
			{
				throw new ArgumentOutOfRangeException( "count", count, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "count" ) );
			}
			else if ( count < 16 )
			{
				this.WriteByte( unchecked( ( byte )( 0x80 | count ) ) );
			}
			else if ( count <= UInt16.MaxValue )
			{
				this.WriteByte( 0xde );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( count & 0xff ) );
				}
			}
			else
			{
				this.WriteByte( 0xdf );
				unchecked
				{
					this.WriteByte( ( byte )( ( count >> 24 ) & 0xff ) );
					this.WriteByte( ( byte )( ( count >> 16 ) & 0xff ) );
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( count & 0xff ) );
				}
			}
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackRawHeader( int length )
		{
			this.VerifyNotDisposed();
			this.PackRawHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		protected void PackRawHeaderCore( int length )
		{
			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "length", length, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "length" ) );
			}
			else if ( length < 32 )
			{
				this.WriteByte( unchecked( ( byte )( 0xa0 | length ) ) );
			}
			else if ( length <= UInt16.MaxValue )
			{
				this.WriteByte( 0xda );
				unchecked
				{
					this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( length & 0xff ) );
				}
			}
			else
			{
				this.WriteByte( 0xdb );
				unchecked
				{
					this.WriteByte( ( byte )( ( length >> 24 ) & 0xff ) );
					this.WriteByte( ( byte )( ( length >> 16 ) & 0xff ) );
					this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( length & 0xff ) );
				}
			}
		}

		#endregion -- Collection Header --

		#region -- Raw --

		/// <summary>
		///		Pack specified byte stream to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackRaw( IEnumerable<byte> value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PrivatePackRaw( value );
			return this;
		}

		/// <summary>
		///		Pack specified byte stream to current stream.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackRaw( IList<byte> value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			var asByteArray = value as byte[];
			if ( asByteArray == null )
			{
				this.PackRawCore( value );
			}
			else
			{
				this.PackRawCore( asByteArray );
			}
			return this;
		}

		/// <summary>
		///		Pack specified byte array to current stream.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackRaw( byte[] value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackRawCore( value );
			return this;
		}

		private void PackRawCore( byte[] value )
		{
			if ( value == null )
			{
				this.PackNull();
				return;
			}

			this.PackRawHeaderCore( value.Length );
			this.PackRawBodyCore( value );
		}

		private void PackRawCore( IList<byte> value )
		{
			if ( value == null )
			{
				this.PackNull();
				return;
			}

			this.PackRawHeaderCore( value.Count );
			this.PackRawBodyCore( value );
		}

		private void PrivatePackRaw( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				this.PackNull();
				return;
			}

			this.PackRawCore( value );
		}

		private void PackRawCore( IEnumerable<byte> value )
		{
			if ( !this.CanSeek )
			{
				// buffered
				this.PackRawCore( value.ToArray() );
			}
			else
			{
				// Header
				this.WriteByte( 0xdb );
				this.StreamWrite( value, items => this.PackRawBodyCore( items ) );
			}
		}

		/// <summary>
		///		Pack specified byte array to current stream without any header.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Packer PackRawBody( byte[] value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackRawBodyCore( value );
			return this;
		}

		private void PrivatePackRawBody( byte[] value )
		{
			if ( value == null )
			{
				return;
			}

			this.PackRawBodyCore( value );
		}

		private void PackRawBodyCore( byte[] value )
		{
			this.PackRawBodyCore( value as IEnumerable<byte> );
		}

		private int PackRawBodyCore( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				return 0;
			}

			int bodyLength = 0;

			foreach ( var b in value )
			{
				this.WriteByte( b );
				bodyLength++;
			}

			return bodyLength;
		}

		#endregion -- Raw --

		#region -- String --

		/// <summary>
		///		Pack specified char stream to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( IEnumerable<char> value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PrivatePackString( value, Encoding.UTF8 );
			return this;
		}

		/// <summary>
		///		Pack specified string to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PrivatePackString( value, Encoding.UTF8 );
			return this;
		}

		/// <summary>
		///		Pack specified char stream to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( IEnumerable<char> value, Encoding encoding )
		{
			this.VerifyNotDisposed();
			this.PrivatePackString( value, encoding );
			return this;
		}

		/// <summary>
		///		Pack specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value, Encoding encoding )
		{
			this.VerifyNotDisposed();
			this.PrivatePackString( value, encoding );
			return this;
		}

		private void PrivatePackString( IEnumerable<char> value, Encoding encoding )
		{
			if ( value == null )
			{
				this.PackNull();
				return;
			}

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			this.PackStringCore( value, encoding );
		}

		private void PrivatePackString( string value, Encoding encoding )
		{
			if ( value == null )
			{
				this.PackNull();
				return;
			}

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			this.PackStringCore( value, encoding );
		}

		/// <summary>
		///		Pack specified char stream to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		protected virtual void PackStringCore( IEnumerable<char> value, Encoding encoding )
		{
			this.PackRawCore( encoding.GetBytes( value.ToArray() ) );
		}

		/// <summary>
		///		Pack specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		protected virtual void PackStringCore( string value, Encoding encoding )
		{
			this.PackRawCore( encoding.GetBytes( value ) );
		}

		#endregion -- String --

		#region -- Enumerable --

		/// <summary>
		///		Pack specified collection to current stream with appropriate serialization.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackItems<TItem>( IEnumerable<TItem> value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackArrayCore( value.Select( item => ( Object )item ), GetCount<TItem>( value ), this.PackObjectsCore );
			return this;
		}

		/// <summary>
		///		Pack specified collection to current stream.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackItems( IEnumerable<MessagePackObject> value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackArrayCore( value, GetCount( value ), this.PackObjectsCore );

			return this;
		}

		private static int? GetCount( System.Collections.IEnumerable value )
		{
			var asCollection = value as System.Collections.ICollection;
			if ( asCollection != null )
			{
				return asCollection.Count;
			}
			else
			{
				return null;
			}
		}

		private static int? GetCount<TItem>( IEnumerable<TItem> value )
		{
			int? count = GetCount( value as System.Collections.IEnumerable );

			if ( count != null )
			{
				return count;
			}

			var asCollectionT = value as ICollection<TItem>;
			if ( asCollectionT != null )
			{
				count = asCollectionT.Count;
			}

			return count;
		}

		private void PackArrayCore<TItem>( IEnumerable<TItem> value, int? count, Action<IEnumerable<TItem>> packObjects )
		{
			if ( count != null )
			{
				this.PackArrayHeader( count.Value );
				packObjects( value );
			}
			else
			{
				// array32 indicator
				this.WriteByte( 0xdd );
				this.StreamWrite( value, packObjects );
			}
		}

		private void PackObjectsCore( IEnumerable<MessagePackObject> value )
		{
			foreach ( var item in value )
			{
				// Dispacthed to Pack(IPackable);
				this.Pack( item );
			}
		}

		private void PackObjectsCore<TItem>( IEnumerable<TItem> value )
		{
			foreach ( var item in value )
			{
				// Dispacthed to Pack(Object);
				this.Pack( ( Object )item );
			}
		}

		#endregion -- Enumerable --

		#region -- List --

		/// <summary>
		///		Pack specified collection to current stream with appropriate serialization.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackList<TItem>( IList<TItem> value )
		{
			this.VerifyNotDisposed();
			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackArrayCore( value, value.Count, this.PackObjectsCore );
			return this;
		}

		/// <summary>
		///		Pack specified collection to current stream.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackList( IList<MessagePackObject> value )
		{
			this.VerifyNotDisposed();

			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackArrayHeader( value );
			foreach ( var item in value )
			{
				this.Pack( item );
			}

			return this;
		}

		/// <summary>
		///		Bookkeep collection count to be packed on current stream.
		/// </summary>
		/// <param name="array">Collection count to be written.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackArrayHeader<TItem>( IList<TItem> array )
		{
			if ( array == null )
			{
				return this.PackNull();
			}
			else
			{
				return this.PackArrayHeader( array.Count );
			}
		}

		#endregion -- List --

		#region -- IDictionary --

		/// <summary>
		///		Pack specified dictionary to current stream with appropriate serialization.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackDictionary<TKey, TValue>( IDictionary<TKey, TValue> value )
		{
			this.VerifyNotDisposed();

			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackMapCore( value, value.Count, this.PackObjectsCore );
			return this;
		}

		/// <summary>
		///		Pack specified dictionary to current stream.
		/// </summary>
		/// <param name="value">Source collection.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackDictionary( IDictionary<MessagePackObject, MessagePackObject> value )
		{
			this.VerifyNotDisposed();

			if ( value == null )
			{
				return this.PackNull();
			}

			this.PackMapCore( value, value.Count, this.PackObjectsCore );
			return this;
		}

		/// <summary>
		///		Bookkeep dictionary count to be packed on current stream.
		/// </summary>
		/// <param name="map">Dictionary count to be written.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackMapHeader<TKey, TValue>( IDictionary<TKey, TValue> map )
		{
			if ( map == null )
			{
				return this.PackNull();
			}
			else
			{
				return this.PackMapHeader( map.Count );
			}
		}

		private void PackMapCore<TKey, TValue>( IEnumerable<KeyValuePair<TKey, TValue>> value, int? count, Action<IEnumerable<KeyValuePair<TKey, TValue>>> packObjects )
		{
			if ( count != null )
			{
				this.PackMapHeader( count.Value );
				packObjects( value );
			}
			else
			{
				// array32 indicator
				this.WriteByte( 0xdd );
				this.StreamWrite( value, packObjects );
			}
		}

		private void PackObjectsCore( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> value )
		{
			foreach ( var item in value )
			{
				// Dispacthed to Pack(IPackable);
				this.Pack( item.Key );
				this.Pack( item.Value );
			}
		}

		private void PackObjectsCore<TKey, TValue>( IEnumerable<KeyValuePair<TKey, TValue>> value )
		{
			foreach ( var item in value )
			{
				// Dispacthed to Pack(Object);
				this.Pack( ( Object )item.Key );
				this.Pack( ( Object )item.Value );
			}
		}

		#endregion -- IDictionary --

		// To avoid boxing, use "TPackable : IPackable" generic parameter instead of "IPackable".

		/// <summary>
		///		Pack specified <see cref="IPackable"/> object with special manner of it.
		/// </summary>
		/// <typeparam name="TPackable"><see cref="IPackable"/> type.</typeparam>
		/// <param name="packable"><see cref="IPackable"/> instance.></param>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <returns>This instance.</returns>
		/// <remarks>
		///		This method give you 2 benefits:
		///		<list type="number">
		///			<item>You don't have to need actual type of packing object.</item>
		///			<item>Avoid auto boxing since this method is generic method.</item>
		///		</list>
		/// </remarks>
		public Packer Pack<TPackable>( TPackable packable )
			where TPackable : IPackable
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( packable == null )
			{
				return this.PackNull();
			}

			packable.PackToMessage( this );
			return this;
		}

		/// <summary>
		///		Pack specified <see cref="Object"/> as apporipriate value.
		/// </summary>
		/// <param name="boxedValue">Boxed value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="MessageTypeException">There is no approptiate MessagePack type to represent specified object.</exception>
		public Packer Pack( object boxedValue )
		{
			return this.Pack( boxedValue, ObjectPackingOptions.Recursive );
		}

		/// <summary>
		///		Pack specified <see cref="Object"/> as apporipriate value.
		/// </summary>
		/// <param name="boxedValue">Boxed value.</param>
		/// <param name="options">Serialization options.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="MessageTypeException">There is no approptiate MessagePack type to represent specified object.</exception>
		public Packer Pack( object boxedValue, ObjectPackingOptions options )
		{
			if ( boxedValue == null )
			{
				return this.PackNull();
			}
			else if ( boxedValue is sbyte )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( sbyte )boxedValue )
					: this.Pack( ( sbyte )boxedValue );
			}
			else if ( boxedValue is sbyte? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( sbyte? )boxedValue )
					: this.Pack( ( sbyte? )boxedValue );
			}
			else if ( boxedValue is byte )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( byte )boxedValue )
					: this.Pack( ( byte )boxedValue );
			}
			else if ( boxedValue is byte? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( byte? )boxedValue )
					: this.Pack( ( byte? )boxedValue );
			}
			else if ( boxedValue is short )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( short )boxedValue )
					: this.Pack( ( short )boxedValue );
			}
			else if ( boxedValue is short? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( short? )boxedValue )
					: this.Pack( ( short? )boxedValue );
			}
			else if ( boxedValue is ushort )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( ushort )boxedValue )
					: this.Pack( ( ushort )boxedValue );
			}
			else if ( boxedValue is ushort? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( ushort? )boxedValue )
					: this.Pack( ( ushort? )boxedValue );
			}
			else if ( boxedValue is int )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( int )boxedValue )
					: this.Pack( ( int )boxedValue );
			}
			else if ( boxedValue is int? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( int? )boxedValue )
					: this.Pack( ( int? )boxedValue );
			}
			else if ( boxedValue is uint )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( uint )boxedValue )
					: this.Pack( ( uint )boxedValue );
			}
			else if ( boxedValue is uint? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( uint? )boxedValue )
					: this.Pack( ( uint? )boxedValue );
			}
			else if ( boxedValue is long )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( long )boxedValue )
					: this.Pack( ( long )boxedValue );
			}
			else if ( boxedValue is long? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( long? )boxedValue )
					: this.Pack( ( long? )boxedValue );
			}
			else if ( boxedValue is ulong )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( ulong )boxedValue )
					: this.Pack( ( ulong )boxedValue );
			}
			else if ( boxedValue is ulong? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( ulong? )boxedValue )
					: this.Pack( ( ulong? )boxedValue );
			}
			else if ( boxedValue is float )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( float )boxedValue )
					: this.Pack( ( float )boxedValue );
			}
			else if ( boxedValue is float? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( float? )boxedValue )
					: this.Pack( ( float? )boxedValue );
			}
			else if ( boxedValue is double )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( double )boxedValue )
					: this.Pack( ( double )boxedValue );
			}
			else if ( boxedValue is double? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( double? )boxedValue )
					: this.Pack( ( double? )boxedValue );
			}
			else if ( boxedValue is bool )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( bool )boxedValue )
					: this.Pack( ( bool )boxedValue );
			}
			else if ( boxedValue is bool? )
			{
				return
					options.Has( ObjectPackingOptions.Strict )
					? this.PackStrict( ( bool? )boxedValue )
					: this.Pack( ( bool? )boxedValue );
			}
			else if ( boxedValue is byte[] )
			{
				return this.PackRaw( ( byte[] )boxedValue );
			}
			else if ( boxedValue is string )
			{
				return this.PackString( ( string )boxedValue );
			}
			else if ( boxedValue is IEnumerable<byte> )
			{
				return this.PackRaw( ( IEnumerable<byte> )boxedValue );
			}
			else if ( boxedValue is IEnumerable<char> )
			{
				return this.PackString( ( IEnumerable<char> )boxedValue );
			}
			else if ( boxedValue is IEnumerable<MessagePackObject> )
			{
				return this.PackItems( ( IEnumerable<MessagePackObject> )boxedValue );
			}
			else if ( boxedValue is IDictionary<MessagePackObject, MessagePackObject> )
			{
				return this.PackDictionary( ( IDictionary<MessagePackObject, MessagePackObject> )boxedValue );
			}

			var asPackable = boxedValue as IPackable;
			if ( asPackable != null )
			{
				return this.Pack( asPackable );
			}

			if ( options.Has( ObjectPackingOptions.Recursive ) )
			{
				var types = boxedValue.GetType().GetInterfaces();
				var dictionaryType =
					types
					.Where( type => type.IsGenericType )
					.Select( type => new KeyValuePair<Type, Type>( type, type.GetGenericTypeDefinition() ) )
					.FirstOrDefault( tuple => tuple.Value == typeof( IDictionary<,> ) )
					.Key;
				if ( dictionaryType != null )
				{
					dynamic asDynamic = boxedValue;
					// To dispatch Pack<TKey,TValue>(IDictionary<TKey,TValue>) effeciently, use dynamic infrastructure.
					return this.Pack( asDynamic );
				}

				var listType =
					types
					.Where( type => type.IsGenericType )
					.Select( type => new KeyValuePair<Type, Type>( type, type.GetGenericTypeDefinition() ) )
					.FirstOrDefault( tuple => tuple.Value == typeof( IEnumerable<> ) )
					.Key;

				if ( listType != null )
				{
					dynamic asDynamic = boxedValue;
					// To dispatch Pack<T>(IEnumerable<T>) effeciently, use dynamic infrastructure.
					return this.Pack( asDynamic );
				}
			}

			throw new MessageTypeException( "unknown object " + boxedValue.GetType() );
		}

		/// <summary>
		///		Try pack <see cref="SByte"/> value to current stream as tiny fix num.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="SByte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackTinySignedInteger( long value )
		{
			// positive fixnum
			if ( value >= 0 && value < 128L )
			{
				this.WriteByte( unchecked( ( byte )value ) );
				return true;
			}

			// negative fixnum
			if ( value >= -32L && value <= -1L )
			{
				this.WriteByte( unchecked( ( byte )value ) );
				return true;
			}

			return false;
		}

		/// <summary>
		///		Try pack <see cref="Byte"/> value to current stream as tiny fix num.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="Byte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
		protected bool TryPackTinyUnsignedInteger( ulong value )
		{
			// positive fixnum
			if ( value < 128L )
			{
				this.WriteByte( unchecked( ( byte )value ) );
				return true;
			}

			return false;
		}

		/// <summary>
		///		Pack a null value to current stream.
		/// </summary>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackNull()
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.WriteByte( 0xc0 );
			return this;
		}
	}
}
