#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke and contributors
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

namespace MsgPack
{
	// TODO: Code comment for protected specifically <exception>

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
				this.ThrowObjectDisposedException();
			}
		}

		private void ThrowObjectDisposedException()
		{
			throw new ObjectDisposedException( this.ToString() );
		}

		/// <summary>
		///		Flushes internal buffer (including underlying stream).
		/// </summary>
		public virtual void Flush()
		{
			// nop
		}

#if FEATURE_TAP

		/// <summary>
		///		Flushes internal buffer (including underlying stream) asynchronously.
		/// </summary>
		/// <returns>A <see cref="Task"/> to represent pending asynchronous operation.</returns>
		public Task FlushAsync()
		{
			return this.FlushAsync( CancellationToken.None );
		}

		/// <summary>
		///		Flushes internal buffer (including underlying stream) asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> to represent pending asynchronous operation.</returns>
		public virtual Task FlushAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( default( object ) );
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		When overridden by derived class, writes specified byte to stream using implementation specific manner asynchronously.
		/// </summary>
		/// <param name="value">A byte to be written.</param>
		/// <returns>A <see cref="Task"/> to represent pending asynchronous operation.</returns>
		protected Task WriteByteAsync( byte value )
		{
			return this.WriteByteAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		When overridden by derived class, writes specified byte to stream using implementation specific manner asynchronously.
		/// </summary>
		/// <param name="value">A byte to be written.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> to represent pending asynchronous operation.</returns>
		protected virtual Task WriteByteAsync( byte value, CancellationToken cancellationToken )
		{
			return Task.Run( () => this.WriteByte( value ), cancellationToken );
		}

#endif // FEATURE_TAP

		#region -- Bulk writing --

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner.
		/// </summary>
		/// <param name="value">Collection of bytes to be written.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		protected virtual void WriteBytes( ICollection<byte> value )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			Contract.EndContractBlock();

			// ReSharper disable once PossibleNullReferenceException
			foreach ( var b in value )
			{
				this.WriteByte( b );
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner asynchronously.
		/// </summary>
		/// <param name="value">Collection of bytes to be written.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task WriteBytesAsync( ICollection<byte> value )
		{
			return this.WriteBytesAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner asynchronously.
		/// </summary>
		/// <param name="value">Collection of bytes to be written.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task WriteBytesAsync( ICollection<byte> value, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			Contract.EndContractBlock();

			// ReSharper disable once PossibleNullReferenceException
			foreach ( var b in value )
			{
				await this.WriteByteAsync( b, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner.
		/// </summary>
		/// <param name="value">Bytes to be written.</param>
		/// <param name="isImmutable">If the <paramref name="value"/> can be treat as immutable (that is, can be used safely without copying) then <c>true</c>.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		protected virtual void WriteBytes( byte[] value, bool isImmutable )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			Contract.EndContractBlock();

			// ReSharper disable once PossibleNullReferenceException
			foreach ( var b in value )
			{
				this.WriteByte( b );
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner asynchronously.
		/// </summary>
		/// <param name="value">Bytes to be written.</param>
		/// <param name="isImmutable">If the <paramref name="value"/> can be treat as immutable (that is, can be used safely without copying) then <c>true</c>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task WriteBytesAsync( byte[] value, bool isImmutable )
		{
			return this.WriteBytesAsync( value, isImmutable, CancellationToken.None );
		}

		/// <summary>
		///		Writes specified bytes to stream using implementation specific most efficient manner asynchronously.
		/// </summary>
		/// <param name="value">Bytes to be written.</param>
		/// <param name="isImmutable">If the <paramref name="value"/> can be treat as immutable (that is, can be used safely without copying) then <c>true</c>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task WriteBytesAsync( byte[] value, bool isImmutable, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			Contract.EndContractBlock();

			// ReSharper disable once PossibleNullReferenceException
			foreach ( var b in value )
			{
				await this.WriteByteAsync( b, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP
		#endregion -- Bulk writing --

		#region -- Int8 --

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( sbyte value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackCore( value );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( sbyte value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( sbyte value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			return this.PackAsyncCore( value, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		[CLSCompliant( false )]
		protected virtual void PackCore( sbyte value )
		{
			if ( this.TryPackTinySignedInteger( value ) )
			{
				return;
			}

#pragma warning disable 168
			var b = this.TryPackInt8( value );
#pragma warning restore 168
#if DEBUG
			Contract.Assert( b, "success" );
#endif // DEBUG
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="SByte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		protected virtual async Task PackAsyncCore( sbyte value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinySignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackInt8Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
#if DEBUG
			Contract.Assert( b, "success" );
#endif // DEBUG
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="SByte"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		protected Task<bool> TryPackInt8Async( long value )
		{
			return this.TryPackInt8Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="SByte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		protected async Task<bool> TryPackInt8Async( long value, CancellationToken cancellationToken )
		{
			if ( value > SByte.MaxValue || value < SByte.MinValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.SignedInt8, cancellationToken ).ConfigureAwait( false );
			await this.WriteByteAsync( ( byte )value, cancellationToken ).ConfigureAwait( false );
			return true;
		}

#endif // FEATURE_TAP

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
			Contract.EndContractBlock();

			this.PackCore( value );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Byte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( byte value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Byte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( byte value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			return this.PackAsyncCore( value, cancellationToken );
		}

#endif // FEATURE_TAP



		/// <summary>
		///		Packs <see cref="Byte"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		protected virtual void PackCore( byte value )
		{
			this.WriteByte( value );
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Byte"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual Task PackAsyncCore( byte value, CancellationToken cancellationToken )
		{
			return this.WriteByteAsync( value, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="Byte"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
		protected bool TryPackUInt8( ulong value )
		{
			if ( value > Byte.MaxValue )
			{
				return false;
			}

			this.WriteByte( MessagePackCode.UnsignedInt8 );
			this.WriteByte( ( byte )value );
			return true;
		}

#if FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Byte"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		[CLSCompliant( false )]
		protected Task<bool> TryPackUInt8Async( ulong value )
		{
			return this.TryPackUInt8Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Byte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		[CLSCompliant( false )]
		protected async Task<bool> TryPackUInt8Async( ulong value, CancellationToken cancellationToken )
		{
			if ( value > Byte.MaxValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.UnsignedInt8, cancellationToken ).ConfigureAwait( false );
			await this.WriteByteAsync( ( byte )value, cancellationToken ).ConfigureAwait( false );
			return true;
		}

#endif // FEATURE_TAP

		#endregion -- UInt8 --

		#region -- Boolean --

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( bool value )
		{
			this.VerifyNotDisposed();
			this.PackCore( value );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( bool value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( bool value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			return this.PackAsyncCore( value, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		protected virtual void PackCore( bool value )
		{
			this.WriteByte( value ? ( byte )MessagePackCode.TrueValue : ( byte )MessagePackCode.FalseValue );
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Boolean"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( bool value, CancellationToken cancellationToken )
		{
			await this.WriteByteAsync( value ? ( byte )MessagePackCode.TrueValue : ( byte )MessagePackCode.FalseValue, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

		#endregion -- Boolean --

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

#if FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream as tiny fix num asynchronously.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="SByte"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		protected Task<bool> TryPackTinySignedIntegerAsync( long value )
		{
			return this.TryPackTinySignedIntegerAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="SByte"/> value to current stream as tiny fix num asynchronously.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="SByte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		protected async Task<bool> TryPackTinySignedIntegerAsync( long value, CancellationToken cancellationToken )
		{
			// positive fixnum
			if ( value >= 0 && value < 128L )
			{
				await this.WriteByteAsync( unchecked( ( byte )value ), cancellationToken ).ConfigureAwait( false );
				return true;
			}

			// negative fixnum
			if ( value >= -32L && value <= -1L )
			{
				await this.WriteByteAsync( unchecked( ( byte )value ), cancellationToken ).ConfigureAwait( false );
				return true;
			}

			return false;
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream as tiny fix num.
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

#if FEATURE_TAP

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream as tiny fix num asynchronously.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="Byte"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		[CLSCompliant( false )]
		protected Task<bool> TryPackTinyUnsignedIntegerAsync( ulong value )
		{
			return this.TryPackTinyUnsignedIntegerAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="Byte"/> value to current stream as tiny fix num asynchronously.
		/// </summary>
		/// <param name="value">Maybe tiny <see cref="Byte"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		/// </returns>
		[CLSCompliant( false )]
		protected async Task<bool> TryPackTinyUnsignedIntegerAsync( ulong value, CancellationToken cancellationToken )
		{
			// positive fixnum
			if ( value < 128L )
			{
				await this.WriteByteAsync( unchecked( ( byte )value ), cancellationToken ).ConfigureAwait( false );
				return true;
			}

			return false;
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Packs a null value to current stream.
		/// </summary>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackNull()
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PrivatePackNullCore();
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs a null value to current stream asynchronously.
		/// </summary>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackNullAsync()
		{
			return this.PackNullAsync( CancellationToken.None );
		}

		/// <summary>
		///		Packs a null value to current stream asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public async Task PackNullAsync( CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			await this.PrivatePackNullAsyncCore( cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

		private void PrivatePackNullCore()
		{
			this.WriteByte( MessagePackCode.NilValue );
		}

#if FEATURE_TAP

		private async Task PrivatePackNullAsyncCore( CancellationToken cancellationToken )
		{
			await this.WriteByteAsync( MessagePackCode.NilValue, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

		private static void ThrowArgumentNullException( string parameterName )
		{
			throw new ArgumentNullException( parameterName );
		}

		private static void ThrowCannotBeNegativeException( string parameterName )
		{
			throw new ArgumentOutOfRangeException( parameterName, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", parameterName ) );
		}

		private static void ThrowMissingBodyOfExtTypeValueException( string parameterName )
		{
			throw new ArgumentException( "MessagePackExtendedTypeObject must have body.", parameterName );
		}

		private static void ThrowExtTypeIsProhibitedException()
		{
			throw new InvalidOperationException( "ExtendedTypeObject is prohibited in this packer." );
		}
	}
}
