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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MsgPack
{
	// TODO: Code comment for protected specifically <exception>
	// TODO: Refactoring or use T4 template.

	/*
	 *  Convention
	 *  PackXxx (public, provides overloads) -> PackXxxCore(protected virtual, provides validation) 
	 *  -> PrivatePackXxx(private, provides null handling) -> PrivatePackXxxCore(private, provides serialization)
	 * 
	 */

	/// <summary>
	///		Implements serialization feature of MsgPack.
	/// </summary>
	public abstract partial class Packer : IDisposable
	{
		private static volatile int _defaultCompatibilityOptions = ( int )PackerCompatibilityOptions.Classic;

		/// <summary>
		///		Gets or sets the default <see cref="PackerCompatibilityOptions"/> for all instances.
		/// </summary>
		/// <value>
		///		The default <see cref="PackerCompatibilityOptions"/>.
		///		The default value is <see cref="PackerCompatibilityOptions.Classic"/>.
		/// </value>
		/// <remarks>
		///		<para>
		///			Note that modification of this value will affect all new instances from the point.
		///			Existent instances are not afectted by the modification.
		///		</para>
		///		<para>
		///			This property is intended to be set in application initialization code.
		///		</para>
		///		<para>
		///			Note that the default value is <see cref="PackerCompatibilityOptions.Classic"/>, not <see cref="PackerCompatibilityOptions.None"/>.
		///		</para>
		/// </remarks>
		public static PackerCompatibilityOptions DefaultCompatibilityOptions
		{
			get { return ( PackerCompatibilityOptions )_defaultCompatibilityOptions; }
			set { _defaultCompatibilityOptions = ( int )value; }
		}

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

		private readonly PackerCompatibilityOptions _compatibilityOptions;

		/// <summary>
		///		Gets a compatibility options for this instance.
		/// </summary>
		/// <value>
		///		The compatibility options.
		/// </value>
		public PackerCompatibilityOptions CompatibilityOptions
		{
			get { return this._compatibilityOptions; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="Packer"/> class with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		protected Packer() : this( DefaultCompatibilityOptions ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="Packer"/> class with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		protected Packer( PackerCompatibilityOptions compatibilityOptions )
		{
			this._compatibilityOptions = compatibilityOptions;
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instancde wrapping specified <see cref="Stream"/> with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream )
		{
			return Create( stream, true );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instancde wrapping specified <see cref="Stream"/> with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( stream, compatibilityOptions, true );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instancde wrapping specified <see cref="Stream"/> with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <param name="ownsStream">
		///		<c>true</c> to close <paramref name="stream"/> when this instance is disposed;
		///		<c>false</c>, otherwise.
		/// </param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, bool ownsStream )
		{
			return Create( stream, DefaultCompatibilityOptions, ownsStream );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instancde wrapping specified <see cref="Stream"/> with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <param name="ownsStream">
		///		<c>true</c> to close <paramref name="stream"/> when this instance is disposed;
		///		<c>false</c>, otherwise.
		/// </param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, PackerCompatibilityOptions compatibilityOptions, bool ownsStream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			return new StreamPacker( stream, compatibilityOptions, ownsStream );
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
		///		When overridden by derived class, writes specified byte to stream using implementation specific manner.
		/// </summary>
		/// <param name="value">A byte to be written.</param>
		protected abstract void WriteByte( byte value );

		#region -- Bulk writing --

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner.
		/// </summary>
		/// <param name="value">Collection of bytes to be written.</param>
		protected virtual void WriteBytes( ICollection<byte> value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			foreach ( var b in value )
			{
				this.WriteByte( b );
			}
		}

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner.
		/// </summary>
		/// <param name="value">Bytes to be written.</param>
		/// <param name="isImmutable">If the <paramref name="value"/> can be treat as immutable (that is, can be used safely without copying) then <c>true</c>.</param>
		protected virtual void WriteBytes( byte[] value, bool isImmutable )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			foreach ( var b in value )
			{
				this.WriteByte( b );
			}
		}

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
					this.WriteByte( ( byte )( ( bodyLength >> 24 ) & 0xff ) );
					this.WriteByte( ( byte )( ( bodyLength >> 16 ) & 0xff ) );
					this.WriteByte( ( byte )( ( bodyLength >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( bodyLength & 0xff ) );
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
					this.WriteByte( ( byte )( ( bodyLength >> 24 ) & 0xff ) );
					this.WriteByte( ( byte )( ( bodyLength >> 16 ) & 0xff ) );
					this.WriteByte( ( byte )( ( bodyLength >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( bodyLength & 0xff ) );
				}

				writeBody( asCollection, options );
			}
		}

		#endregion -- Bulk writing --

		#region -- Int8 --

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( sbyte value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( sbyte value )
		{
			if ( this.TryPackTinySignedInteger( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackInt8( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="SByte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		protected bool TryPackInt8( long value )
		{
			if ( value > SByte.MaxValue || value < SByte.MinValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.SignedInt8 );
			this.WriteByte( ( byte )value );
			return true;
		}

		#endregion -- Int8 --

		#region -- UInt8 --

		/// <summary>
		///		Packs <see cref="Byte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( byte value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( byte value )
		{
			if ( this.TryPackTinyUnsignedInteger( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackUInt8( value );
#pragma warning restore 168
#if !UNITY && DEBUG
			Contract.Assert( b, "success" );
#endif // !UNITY && DEBUG
		}

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Byte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		private bool TryPackUInt8( ulong value )
		{
			if ( value > Byte.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt8 );
			this.WriteByte( ( byte )value );
			return true;
		}

		#endregion -- UInt8 --

		#region -- Int16 --

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( short value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( short value )
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
		protected bool TryPackInt16( long value )
		{
			if ( value < Int16.MinValue || value > Int16.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.SignedInt16 );
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
		///		Packs <see cref="UInt16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( ushort value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( ushort value )
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
		protected bool TryPackUInt16( ulong value )
		{
			if ( value > UInt16.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt16 );
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
		///		Packs <see cref="Int32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( int value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( int value )
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
		protected bool TryPackInt32( long value )
		{
			if ( value > Int32.MaxValue || value < Int32.MinValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.SignedInt32 );
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
		///		Packs <see cref="UInt32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( uint value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( uint value )
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
		protected bool TryPackUInt32( ulong value )
		{
			if ( value > UInt32.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt32 );
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
		///		Packs <see cref="Int64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer Pack( long value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( long value )
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
		protected bool TryPackInt64( long value )
		{
			this.WriteByte( MessagePackCode.SignedInt64 );
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
		///		Packs <see cref="UInt64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( ulong value )
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( ulong value )
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
		protected bool TryPackUInt64( ulong value )
		{
			this.WriteByte( MessagePackCode.UnsignedInt64 );
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
				this.WriteByte( ( byte )( ( bits >> 56 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 48 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 40 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 32 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 24 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 16 ) & 0xff ) );
				this.WriteByte( ( byte )( ( bits >> 8 ) & 0xff ) );
				this.WriteByte( ( byte )( bits & 0xff ) );
			}
		}

		#endregion -- Double --

		#region -- Boolean --

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( bool value )
		{
			this.VerifyNotDisposed();
			this.PrivatePackCore( value );
			return this;
		}

		private void PrivatePackCore( bool value )
		{
			this.WriteByte( value ? ( byte )MessagePackCode.TrueValue : ( byte )MessagePackCode.FalseValue );
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
				throw new ArgumentOutOfRangeException( "count", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "count" ) );
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
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( count & 0xff ) );
				}
			}
			else
			{
				this.WriteByte( MessagePackCode.Array32 );
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
				throw new ArgumentOutOfRangeException( "count", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "count" ) );
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
					this.WriteByte( ( byte )( ( count >> 8 ) & 0xff ) );
					this.WriteByte( ( byte )( count & 0xff ) );
				}
			}
			else
			{
				this.WriteByte( MessagePackCode.Map32 );
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
		/// <remarks>
		///		This method acts as alias of <see cref="PackStringHeader"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeader(Int32) or Use PackBinaryHeader(Int32) instead." )]
		public Packer PackRawHeader( int length )
		{
			this.PackRawHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes may represent well formed encoded string.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStringHeader( int length )
		{
			this.PackStringHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes do not represent well formed encoded string.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackBinaryHeader( int length )
		{
			this.PackBinaryHeaderCore( length );
			return this;
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		/// <remarks>
		///		This method acts as alias of <see cref="PackStringHeaderCore"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeaderCore(Int32) or Use PackBinaryHeaderCore(Int32) instead." )]
		protected void PackRawHeaderCore( int length )
		{
			this.PackStringHeaderCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes may represent well formed encoded string.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		protected void PackStringHeaderCore( int length )
		{
			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "length", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "length" ) );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY
			this.VerifyNotDisposed();

			this.PrivatePackRawHeaderCore( length, true );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes do not represent well formed encoded string.
		/// </summary>
		/// <param name="length">Byte length.</param>
		/// <returns>This instance.</returns>
		protected void PackBinaryHeaderCore( int length )
		{
			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "length", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "length" ) );
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
						this.WriteByte( ( byte )( length & 0xff ) );
					}

					return;
				}

				if ( length <= UInt16.MaxValue )
				{
					this.WriteByte( MessagePackCode.Raw16 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
						this.WriteByte( ( byte )( length & 0xff ) );
					}
				}
				else
				{
					this.WriteByte( MessagePackCode.Raw32 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 24 ) & 0xff ) );
						this.WriteByte( ( byte )( ( length >> 16 ) & 0xff ) );
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
						this.WriteByte( ( byte )( length & 0xff ) );
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
						this.WriteByte( ( byte )( length & 0xff ) );
					}
				}
				else if ( length <= UInt16.MaxValue )
				{
					this.WriteByte( MessagePackCode.Bin16 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
						this.WriteByte( ( byte )( length & 0xff ) );
					}
				}
				else
				{
					this.WriteByte( MessagePackCode.Bin32 );
					unchecked
					{
						this.WriteByte( ( byte )( ( length >> 24 ) & 0xff ) );
						this.WriteByte( ( byte )( ( length >> 16 ) & 0xff ) );
						this.WriteByte( ( byte )( ( length >> 8 ) & 0xff ) );
						this.WriteByte( ( byte )( length & 0xff ) );
					}
				}
			}
		}

		#endregion -- Collection Header --

		#region -- Raw with Header --

		/// <summary>
		///		Packs specified byte stream to current stream.
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
		///		Packs specified byte stream to current stream.
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
		///		Packs specified byte array to current stream.
		/// </summary>
		/// <param name="value">Source byte array.</param>
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

		private void PrivatePackRaw( byte[] value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			PrivatePackRawCore( value, false );
		}

		private void PrivatePackRawCore( byte[] value, bool isImmutable )
		{
			this.PrivatePackRawHeaderCore( value.Length, /*isString:*/ true );
			this.WriteBytes( value, isImmutable );
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

		private void PrivatePackRaw( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackRawCore( value );
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

		#region -- String --

		/// <summary>
		///		Packs specified char stream to current stream with UTF-8 <see cref="Encoding"/>.
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
		///		Packs specified char stream to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( IEnumerable<char> value, Encoding encoding )
		{
			this.PackStringCore( value, encoding );
			return this;
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value, Encoding encoding )
		{
			this.PackStringCore( value, encoding );
			return this;
		}

		/// <summary>
		///		Packs specified char stream to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		protected virtual void PackStringCore( IEnumerable<char> value, Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
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
		///		Packs specified string to current stream with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		protected virtual void PackStringCore( string value, Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
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

		#endregion -- String --

		#region -- Binary --

		/// <summary>
		///		Packs specified byte stream to current stream.
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
		///		Packs specified byte stream to current stream.
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
		///		Packs specified byte array to current stream.
		/// </summary>
		/// <param name="value">Source byte array.</param>
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

		private void PrivatePackBinary( byte[] value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			PrivatePackBinaryCore( value, false );
		}

		private void PrivatePackBinaryCore( byte[] value, bool isImmutable )
		{
			this.PrivatePackRawHeaderCore( value.Length, /*isString:*/ false );
			this.WriteBytes( value, isImmutable );
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

		private void PrivatePackBinary( IEnumerable<byte> value )
		{
			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			this.PrivatePackBinaryCore( value );
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

		#endregion -- Binary --


		#region -- List --

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

		#endregion -- List --

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

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream as tiny fix num.
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
		///		Try packs <see cref="Byte"/> value to current stream as tiny fix num.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="Byte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
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
		///		Packs a null value to current stream.
		/// </summary>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackNull()
		{
			this.VerifyNotDisposed();
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			this.PrivatePackNullCore();
			return this;
		}

		private void PrivatePackNullCore()
		{
			this.WriteByte( MessagePackCode.NilValue );
		}

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
			} // swith

			this.WriteByte( typeCode );
			this.WriteBytes( body, true );
		}
	}
}
