#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from Packer.Packing.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit Packer.Packing.tt and StreamingUnapkcerBase.ttinclude instead.

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "AsyncResult<T>" )]
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
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		protected virtual void PackCore( Int16 value )
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
			Contract.Assert( b, "success" );
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int16 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int16 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="Int16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( Int16 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinySignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackInt16Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="Int16"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int16"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected Task<bool> TryPackInt16Async( Int64 value )
		{
			return this.TryPackInt16Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="Int16"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected async Task<bool> TryPackInt16Async( Int64 value, CancellationToken cancellationToken )
		{
			if ( value < Int16.MinValue || value > Int16.MaxValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.SignedInt16 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

		#endregion -- Int16 --

		#region -- UInt16 --

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( UInt16 value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		[CLSCompliant( false )]
		protected virtual void PackCore( UInt16 value )
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
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt16"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt16"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt16 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt16 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="UInt16"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		protected virtual async Task PackAsyncCore( UInt16 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinyUnsignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackUInt16Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt16"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt16"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected Task<bool> TryPackUInt16Async( UInt64 value )
		{
			return this.TryPackUInt16Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="UInt16"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt16"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected async Task<bool> TryPackUInt16Async( UInt64 value, CancellationToken cancellationToken )
		{
			if ( value > UInt16.MaxValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.UnsignedInt16 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

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
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="Int32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		protected virtual void PackCore( Int32 value )
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
			Contract.Assert( b, "success" );
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Int32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int32 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Int32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int32 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="Int32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( Int32 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinySignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt16Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackInt32Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="Int32"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int32"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected Task<bool> TryPackInt32Async( Int64 value )
		{
			return this.TryPackInt32Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="Int32"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected async Task<bool> TryPackInt32Async( Int64 value, CancellationToken cancellationToken )
		{
			if ( value < Int32.MinValue || value > Int32.MaxValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.SignedInt32 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

		#endregion -- Int32 --

		#region -- UInt32 --

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( UInt32 value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		[CLSCompliant( false )]
		protected virtual void PackCore( UInt32 value )
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
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt32"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt32"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt32 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt32 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="UInt32"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		protected virtual async Task PackAsyncCore( UInt32 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinyUnsignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt16Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackUInt32Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt32"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt32"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected Task<bool> TryPackUInt32Async( UInt64 value )
		{
			return this.TryPackUInt32Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="UInt32"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt32"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected async Task<bool> TryPackUInt32Async( UInt64 value, CancellationToken cancellationToken )
		{
			if ( value > UInt32.MaxValue )
			{
				return false;
			}

			await this.WriteByteAsync( MessagePackCode.UnsignedInt32 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

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
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="Int64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		protected virtual void PackCore( Int64 value )
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
			Contract.Assert( b, "success" );
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Int64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int64 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Int64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackAsync( Int64 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="Int64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( Int64 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinySignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt16Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackInt32Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackInt64Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="Int64"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int64"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected Task<bool> TryPackInt64Async( Int64 value )
		{
			return this.TryPackInt64Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="Int64"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="Int64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		protected async Task<bool> TryPackInt64Async( Int64 value, CancellationToken cancellationToken )
		{

			await this.WriteByteAsync( MessagePackCode.SignedInt64 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 56 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 48 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 40 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 32 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

		#endregion -- Int64 --

		#region -- UInt64 --

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Packer Pack( UInt64 value )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			this.PackCore( value );
			return this;
		}

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		[CLSCompliant( false )]
		protected virtual void PackCore( UInt64 value )
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
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt64"/> value to current stream strictly.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt64"/> value.</param>
		/// <returns>If <paramref name="value"/> has be packed successfully then true, otherwise false (normally, larger type required).</returns>
		[CLSCompliant( false )]
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt64 value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		[CLSCompliant( false )]
		public Task PackAsync( UInt64 value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();
			return this.PackAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs <see cref="UInt64"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		protected virtual async Task PackAsyncCore( UInt64 value, CancellationToken cancellationToken )
		{
			if ( await this.TryPackTinyUnsignedIntegerAsync( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt8Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt16Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

			if ( await this.TryPackUInt32Async( value, cancellationToken ).ConfigureAwait( false ) )
			{
				return;
			}

#pragma warning disable 168
			var b = await this.TryPackUInt64Async( value, cancellationToken ).ConfigureAwait( false );
#pragma warning restore 168
			Contract.Assert( b, "success" );
		}

		/// <summary>
		///		Try packs <see cref="UInt64"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt64"/> value.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected Task<bool> TryPackUInt64Async( UInt64 value )
		{
			return this.TryPackUInt64Async( value, CancellationToken.None );
		}

		/// <summary>
		///		Try packs <see cref="UInt64"/> value to current stream strictly asynchronously.
		/// </summary>
		/// <param name="value">Maybe <see cref="UInt64"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the value
		///		whether <paramref name="value"/> has be packed successfully or not(normally, larger type required).
		///	</returns>
		[CLSCompliant( false )]
		protected async Task<bool> TryPackUInt64Async( UInt64 value, CancellationToken cancellationToken )
		{

			await this.WriteByteAsync( MessagePackCode.UnsignedInt64 ).ConfigureAwait( false );
			unchecked
			{
				await this.WriteByteAsync( ( byte )( ( value >> 56 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 48 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 40 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 32 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( value >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( value & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
			return true;
		}

#endif // FEATURE_TAP

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
			this.PackCore( value );
			return this;
		}


		/// <summary>
		///		Packs <see cref="Single"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		protected virtual void PackCore( float value )
		{
			this.WriteByte( MessagePackCode.Real32 );

			var bits = new Float32Bits( value );

			// Float32Bits usage is effectively pointer dereference operation rather than shifting operators, so we must consider endianness here.
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Single"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( float value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Single"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( float value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			return this.PackAsyncCore( value, cancellationToken );
		}


		/// <summary>
		///		Packs <see cref="Single"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( float value, CancellationToken cancellationToken )
		{
			await this.WriteByteAsync( MessagePackCode.Real32, cancellationToken ).ConfigureAwait( false );

			var bits = new Float32Bits( value );

			// Float32Bits usage is effectively pointer dereference operation rather than shifting operators, so we must consider endianness here.
			if ( BitConverter.IsLittleEndian )
			{
				await this.WriteByteAsync( bits.Byte3, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte2, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte1, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte0, cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				await this.WriteByteAsync( bits.Byte0, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte1, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte2, cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( bits.Byte3, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

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
			this.PackCore( value );
			return this;
		}


		/// <summary>
		///		Packs <see cref="Double"/> value to current stream.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		protected virtual void PackCore( double value )
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

#if FEATURE_TAP

		/// <summary>
		///		Packs <see cref="Double"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( double value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs <see cref="Double"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( double value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			return this.PackAsyncCore( value, cancellationToken );
		}


		/// <summary>
		///		Packs <see cref="Double"/> value to current stream asynchronously.
		/// </summary>
		/// <param name="value"><see cref="Double"/> value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackAsyncCore( double value, CancellationToken cancellationToken )
		{
			await this.WriteByteAsync( MessagePackCode.Real64, cancellationToken ).ConfigureAwait( false );
			unchecked
			{
				long bits = BitConverter.DoubleToInt64Bits( value );
				await this.WriteByteAsync( ( byte )( ( bits >> 56 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 48 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 40 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 32 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( ( bits >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
				await this.WriteByteAsync( ( byte )( bits & 0xFF ), cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

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
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			this.PackArrayHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		protected virtual void PackArrayHeaderCore( int count )
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


#if FEATURE_TAP

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackArrayHeaderAsync( int count )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			return this.PackArrayHeaderAsyncCore( count );
		}

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task PackArrayHeaderAsyncCore( int count )
		{
			return this.PackArrayHeaderAsyncCore( count, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackArrayHeaderAsync( int count, CancellationToken cancellationToken )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			return this.PackArrayHeaderAsyncCore( count, cancellationToken );
		}

		/// <summary>
		///		Bookkeep array length or list items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Array length or list items count.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackArrayHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
#if !UNITY
			Contract.Assert( 0 <= count, "0 <= count" );
#endif // !UNITY
			if ( count < 16 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedArray | count ) ), cancellationToken ).ConfigureAwait( false );
			}
			else if ( count <= UInt16.MaxValue )
			{
				await this.WriteByteAsync( MessagePackCode.Array16, cancellationToken ).ConfigureAwait( false );
				unchecked
				{
					await this.WriteByteAsync( ( byte )( ( count >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( count & 0xFF ), cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
				await this.WriteByteAsync( MessagePackCode.Array32, cancellationToken ).ConfigureAwait( false );
				unchecked
				{
					await this.WriteByteAsync( ( byte )( ( count >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( count >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( count >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( count & 0xFF ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackMapHeader( int count )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			this.PackMapHeaderCore( count );
			return this;
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		protected virtual void PackMapHeaderCore( int count )
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


#if FEATURE_TAP

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackMapHeaderAsync( int count )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			return this.PackMapHeaderAsyncCore( count );
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task PackMapHeaderAsyncCore( int count )
		{
			return this.PackMapHeaderAsyncCore( count, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackMapHeaderAsync( int count, CancellationToken cancellationToken )
		{
			if ( count < 0 )
			{
				ThrowCannotBeNegativeException( "count" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			return this.PackMapHeaderAsyncCore( count, cancellationToken );
		}

		/// <summary>
		///		Bookkeep dictionary (map) items count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="count">Dictionary (map) items count.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackMapHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
#if !UNITY
			Contract.Assert( 0 <= count, "0 <= count" );
#endif // !UNITY
			if ( count < 16 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedMap | count ) ), cancellationToken ).ConfigureAwait( false );
			}
			else if ( count <= UInt16.MaxValue )
			{
				await this.WriteByteAsync( MessagePackCode.Map16, cancellationToken ).ConfigureAwait( false );
				unchecked
				{
					await this.WriteByteAsync( ( byte )( ( count >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( count & 0xFF ), cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
				await this.WriteByteAsync( MessagePackCode.Map32, cancellationToken ).ConfigureAwait( false );
				unchecked
				{
					await this.WriteByteAsync( ( byte )( ( count >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( count >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( count >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( count & 0xFF ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

#endif // FEATURE_TAP

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
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			this.PackStringHeaderCore( length );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method effectively acts as alias of <see cref="PackStringHeader"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeader(Int32) or Use PackBinaryHeader(Int32) instead." )]
		public Task PackRawHeaderAsync( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			return this.PackStringHeaderAsyncCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method effectively acts as alias of <see cref="PackStringHeader"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeader(Int32) or Use PackBinaryHeader(Int32) instead." )]
		public Task PackRawHeaderAsync( int length, CancellationToken cancellationToken )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			return this.PackStringHeaderAsyncCore( length, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackStringHeader( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			this.PackStringHeaderCore( length );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringHeaderAsync( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			return this.PackStringHeaderAsyncCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringHeaderAsync( int length, CancellationToken cancellationToken )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			return this.PackStringHeaderAsyncCore( length, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackBinaryHeader( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) != 0 )
			{
				// In compat mode, use raw(str) header.
				this.PackStringHeaderCore( length );
				return this;
			}
			this.PackBinaryHeaderCore( length );
			return this;
		}

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackBinaryHeaderAsync( int length )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) != 0 )
			{
				// In compat mode, use raw(str) header.
				return this.PackStringHeaderAsyncCore( length );
			}
			return this.PackBinaryHeaderAsyncCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackBinaryHeaderAsync( int length, CancellationToken cancellationToken )
		{
			if ( length < 0 )
			{
				ThrowCannotBeNegativeException( "length" );
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();
			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) != 0 )
			{
				// In compat mode, use raw(str) header.
				return this.PackStringHeaderAsyncCore( length, cancellationToken );
			}
			return this.PackBinaryHeaderAsyncCore( length, cancellationToken );
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <remarks>
		///		This method acts as alias of <see cref="PackStringHeaderCore"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeaderCore(Int32) or Use PackBinaryHeaderCore(Int32) instead." )]
		protected Task PackRawHeaderAsyncCore( int length )
		{
			return this.PackStringHeaderAsyncCore( length );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes might represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <remarks>
		///		This method acts as alias of <see cref="PackStringHeaderCore"/> for compatibility.
		/// </remarks>
		[Obsolete( "Use PackStringHeaderCore(Int32) or Use PackBinaryHeaderCore(Int32) instead." )]
		protected Task PackRawHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			return this.PackStringHeaderAsyncCore( length, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		protected virtual void PackStringHeaderCore( int length )
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

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task PackStringHeaderAsyncCore( int length )
		{
			return this.PackStringHeaderAsyncCore( length, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of encoded byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackStringHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
			if ( length < 32 )
			{
				await this.WriteByteAsync( unchecked( ( byte )( MessagePackCode.MinimumFixedRaw | length ) ), cancellationToken ).ConfigureAwait( false );
				return;
			}

				if ( length <= Byte.MaxValue && ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					await this.WriteByteAsync( MessagePackCode.Str8, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
				else if ( length <= UInt16.MaxValue )
				{
					await this.WriteByteAsync( MessagePackCode.Str16, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( ( length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
				else
				{
					await this.WriteByteAsync( MessagePackCode.Str32, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( ( length >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( ( length >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( ( length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		protected virtual void PackBinaryHeaderCore( int length )
		{
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

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected Task PackBinaryHeaderAsyncCore( int length )
		{
			return this.PackBinaryHeaderAsyncCore( length, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep byte length to be packed on current stream as the bytes should not represent well formed encoded string asynchronously.
		/// </summary>
		/// <param name="length">A length of byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackBinaryHeaderAsyncCore( int length, CancellationToken cancellationToken )
		{
				if ( length <= Byte.MaxValue )
				{
					await this.WriteByteAsync( MessagePackCode.Bin8, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
				else if ( length <= UInt16.MaxValue )
				{
					await this.WriteByteAsync( MessagePackCode.Bin16, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( ( length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
				else
				{
					await this.WriteByteAsync( MessagePackCode.Bin32, cancellationToken ).ConfigureAwait( false );
					unchecked
					{
						await this.WriteByteAsync( ( byte )( ( length >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( ( length >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( ( length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
						await this.WriteByteAsync( ( byte )( length & 0xFF ), cancellationToken ).ConfigureAwait( false );
					}
				}
		}

#endif // FEATURE_TAP

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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				this.PackRawCore( asArray );
			}
			else
			{
				this.PackRawCore( value.ToArray() );
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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				this.PackRawCore( asArray );
			}
			else
			{
				this.PackRawCore( value.ToArray() );
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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			this.PackRawCore( value );
			return this;
		}


		/// <summary>
		///		Packs specified byte array(it may or may not be string to current stream.
		/// </summary>
		/// <param name="value">A byte array.</param>
		protected virtual void PackRawCore( byte[] value )
		{
			this.PackStringHeaderCore( value.Length );
			this.WriteBytes( value, false );
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs specified byte sequence(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( IEnumerable<byte> value )
		{
			return this.PackRawAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte sequence(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( IEnumerable<byte> value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				return this.PackRawAsyncCore( asArray, cancellationToken );
			}
			else
			{
				return this.PackRawAsyncCore( value.ToArray(), cancellationToken );
			}
		}

		/// <summary>
		///		Packs specified byte collection(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( IList<byte> value )
		{
			return this.PackRawAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte collection(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( IList<byte> value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				return this.PackRawAsyncCore( asArray, cancellationToken );
			}
			else
			{
				return this.PackRawAsyncCore( value.ToArray(), cancellationToken );
			}
		}

		/// <summary>
		///		Packs specified byte array(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( byte[] value )
		{
			return this.PackRawAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte array(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use str types (previously known as raw types) for compability.
		/// </remarks>
		public Task PackRawAsync( byte[] value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			return this.PackRawAsyncCore( value, cancellationToken );
		}


		/// <summary>
		///		Packs specified byte array(it may or may not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackRawAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			await this.PackStringHeaderAsyncCore( value.Length, cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( value, false, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					this.PackBinaryCore( asArray );
				}
				else
				{
					this.PackRawCore( asArray );
				}
	
			}
			else
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					this.PackBinaryCore( value.ToArray() );
				}
				else
				{
					this.PackRawCore( value.ToArray() );
				}
	
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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					this.PackBinaryCore( asArray );
				}
				else
				{
					this.PackRawCore( asArray );
				}
	
			}
			else
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					this.PackBinaryCore( value.ToArray() );
				}
				else
				{
					this.PackRawCore( value.ToArray() );
				}
	
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
			Contract.EndContractBlock();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				this.PackBinaryCore( value );
			}
			else
			{
				this.PackRawCore( value );
			}

			return this;
		}


		/// <summary>
		///		Packs specified byte array(it should not be string to current stream.
		/// </summary>
		/// <param name="value">A byte array.</param>
		protected virtual void PackBinaryCore( byte[] value )
		{
			this.PackBinaryHeaderCore( value.Length );
			this.WriteBytes( value, false );
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs specified byte sequence(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( IEnumerable<byte> value )
		{
			return this.PackBinaryAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte sequence(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is not known.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( IEnumerable<byte> value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					return this.PackBinaryAsyncCore( asArray, cancellationToken );
				}
				else
				{
					return this.PackRawAsyncCore( asArray, cancellationToken );
				}
	
			}
			else
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					return this.PackBinaryAsyncCore( value.ToArray(), cancellationToken );
				}
				else
				{
					return this.PackRawAsyncCore( value.ToArray(), cancellationToken );
				}
	
			}
		}

		/// <summary>
		///		Packs specified byte collection(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( IList<byte> value )
		{
			return this.PackBinaryAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte collection(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">Source bytes its size is known.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( IList<byte> value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			var asArray = value as byte[];
			if ( asArray != null )
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					return this.PackBinaryAsyncCore( asArray, cancellationToken );
				}
				else
				{
					return this.PackRawAsyncCore( asArray, cancellationToken );
				}
	
			}
			else
			{
				if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
				{
					return this.PackBinaryAsyncCore( value.ToArray(), cancellationToken );
				}
				else
				{
					return this.PackRawAsyncCore( value.ToArray(), cancellationToken );
				}
	
			}
		}

		/// <summary>
		///		Packs specified byte array(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( byte[] value )
		{
			return this.PackBinaryAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte array(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		This method use bin types unless <see cref="CompatibilityOptions"/> contains <see cref="PackerCompatibilityOptions.PackBinaryAsRaw"/>.
		/// </remarks>
		public Task PackBinaryAsync( byte[] value, CancellationToken cancellationToken )
		{
			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0 )
			{
				return this.PackBinaryAsyncCore( value, cancellationToken );
			}
			else
			{
				return this.PackRawAsyncCore( value, cancellationToken );
			}

		}


		/// <summary>
		///		Packs specified byte array(it should not be string to current stream asynchronously.
		/// </summary>
		/// <param name="value">A byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual async Task PackBinaryAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			await this.PackBinaryHeaderAsyncCore( value.Length, cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( value, false, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

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

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			this.PackRawCore( value );
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

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			var encoded = encoding.GetBytes( value.ToArray() );
			this.PackRawCore( encoded );
		}


#if FEATURE_TAP

		/// <summary>
		///		Packs specified charactor sequence to current stream with UTF-8 <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( IEnumerable<char> value )
		{
			return this.PackStringAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( IEnumerable<char> value, Encoding encoding )
		{
			return this.PackStringAsync( value, encoding, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected Task PackStringAsyncCore( IEnumerable<char> value, Encoding encoding )
		{
			return this.PackStringAsyncCore( value, encoding, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with UTF-8 <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( IEnumerable<char> value, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			return this.PackRawAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( IEnumerable<char> value, Encoding encoding, CancellationToken cancellationToken )
		{
			return this.PackStringAsyncCore( value, encoding, cancellationToken );
		}

		/// <summary>
		///		Packs specified charactor sequence to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source chars its size is not known.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected virtual async Task PackStringAsyncCore( IEnumerable<char> value, Encoding encoding, CancellationToken cancellationToken )
		{
			if ( encoding == null )
			{
				ThrowArgumentNullException( "encoding ");
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			if ( value == null )
			{
				await this.PrivatePackNullAsyncCore( cancellationToken ).ConfigureAwait( false );
				return;
			}

			var encoded = encoding.GetBytes( value.ToArray() );
			await this.PackRawAsyncCore( encoded, cancellationToken ).ConfigureAwait( false );
		}


#endif // FEATURE_TAP

		/// <summary>
		///		Packs specified string to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackString( string value )
		{

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return this;
			}

			this.PackRawCore( value );
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

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			if ( value == null )
			{
				this.PrivatePackNullCore();
				return;
			}

			var encoded = encoding.GetBytes( value );
			this.PackRawCore( encoded );
		}


#if FEATURE_TAP

		/// <summary>
		///		Packs specified string to current stream with UTF-8 <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( string value )
		{
			return this.PackStringAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( string value, Encoding encoding )
		{
			return this.PackStringAsync( value, encoding, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected Task PackStringAsyncCore( string value, Encoding encoding )
		{
			return this.PackStringAsyncCore( value, encoding, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified string to current stream with UTF-8 <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( string value, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				return this.PrivatePackNullAsyncCore( cancellationToken );
			}

			return this.PackRawAsyncCore( value, cancellationToken );
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackStringAsync( string value, Encoding encoding, CancellationToken cancellationToken )
		{
			return this.PackStringAsyncCore( value, encoding, cancellationToken );
		}

		/// <summary>
		///		Packs specified string to current stream with specified <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">Source string.</param>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <c>null</c>.</exception>
		protected virtual async Task PackStringAsyncCore( string value, Encoding encoding, CancellationToken cancellationToken )
		{
			if ( encoding == null )
			{
				ThrowArgumentNullException( "encoding ");
			}

			Contract.EndContractBlock();
			this.VerifyNotDisposed();

			if ( value == null )
			{
				await this.PrivatePackNullAsyncCore( cancellationToken ).ConfigureAwait( false );
				return;
			}

			var encoded = encoding.GetBytes( value );
			await this.PackRawAsyncCore( encoded, cancellationToken ).ConfigureAwait( false );
		}


#endif // FEATURE_TAP

		/// <summary>
		///		Packs specified <see cref="String" /> to current stream with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="value">A string.</param>
		protected virtual void PackRawCore( string value )
		{
			this.PackStringCore( value, Encoding.UTF8 );
		}

		private void PackRawCore( IEnumerable<char> value )
		{
#if !NETSTANDARD1_1
			var asString = value as string;
			if ( asString == null )
			{
				asString = new String( value.ToArray() );
			}
#else
			var asString = new String( value.ToArray() );
#endif // !NETSTANDARD1_1

			this.PackStringCore( asString, Encoding.UTF8 );
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs specified <see cref="String" /> to current stream with UTF-8 <see cref="Encoding"/> asynchronously.
		/// </summary>
		/// <param name="value">A string.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		protected virtual Task PackRawAsyncCore( string value, CancellationToken cancellationToken )
		{
			return this.PackStringAsyncCore( value, Encoding.UTF8, cancellationToken );
		}

		private Task PackRawAsyncCore( IEnumerable<char> value, CancellationToken cancellationToken )
		{
#if !NETSTANDARD1_1
			var asString = value as string;
			if ( asString == null )
			{
				asString = new String( value.ToArray() );
			}
#else
			var asString = new String( value.ToArray() );
#endif // !NETSTANDARD1_1

			return this.PackStringAsyncCore( asString, Encoding.UTF8, cancellationToken );
		}

#endif // FEATURE_TAP

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
				ThrowArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

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
				ThrowArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PrivatePackRawBodyCore( value );
			return this;
		}

		private int PrivatePackRawBodyCore( IEnumerable<byte> value )
		{
			Contract.Assert( value != null, "value != null" );

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
			Contract.Assert( value != null, "value != null" );

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

#if FEATURE_TAP

		/// <summary>
		///		Packs specified byte array to current stream without any header asynchronously.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Task PackRawBodyAsync( byte[] value )
		{
			return this.PackRawBodyAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte sequence to current stream without any header asynchronously.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Task PackRawBodyAsync( IEnumerable<byte> value )
		{
			return this.PackRawBodyAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified byte array to current stream without any header asynchronously.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Task PackRawBodyAsync( byte[] value, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			return this.WriteBytesAsync( value, false, cancellationToken );
		}

		/// <summary>
		///		Packs specified byte sequence to current stream without any header asynchronously.
		/// </summary>
		/// <param name="value">Source byte array.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <remarks>
		///		If you forget to write header first, then resulting stream will be corrupsed.
		/// </remarks>
		public Task PackRawBodyAsync( IEnumerable<byte> value, CancellationToken cancellationToken )
		{
			if ( value == null )
			{
				ThrowArgumentNullException( "value" );
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			return this.PrivatePackRawBodyAsyncCore( value, cancellationToken );
		}

		private async Task<int> PrivatePackRawBodyAsyncCore( IEnumerable<byte> value, CancellationToken cancellationToken )
		{
			Contract.Assert( value != null, "value != null" );

			var asCollection = value as ICollection<byte>;
			if ( asCollection != null )
			{
				return await this.PrivatePackRawBodyAsyncCore( asCollection, asCollection.IsReadOnly, cancellationToken ).ConfigureAwait( false );
			}

			int bodyLength = 0;

			foreach ( var b in value )
			{
				await this.WriteByteAsync( b, cancellationToken ).ConfigureAwait( false );
				bodyLength++;
			}

			return bodyLength;
		}

		private async Task<int> PrivatePackRawBodyAsyncCore( ICollection<byte> value, bool isImmutable, CancellationToken cancellationToken )
		{
			Contract.Assert( value != null, "value != null" );

			var asArray = value as byte[];
			if ( asArray != null )
			{
				await this.WriteBytesAsync( asArray, isImmutable, cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				await this.WriteBytesAsync( value, cancellationToken ).ConfigureAwait( false );
			}

			return value.Count;
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep collection count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="array">Collection count to be written.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackArrayHeaderAsync<TItem>( IList<TItem> array )
		{
			return this.PackArrayHeaderAsync( array, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep collection count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="array">Collection count to be written.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackArrayHeaderAsync<TItem>( IList<TItem> array, CancellationToken cancellationToken )
		{
			return array == null ? this.PackNullAsync( cancellationToken ) : this.PackArrayHeaderAsync( array.Count, cancellationToken );
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		Bookkeep dictionary count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="map">Dictionary count to be written.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackMapHeaderAsync<TKey, TValue>( IDictionary<TKey, TValue> map )
		{
			return this.PackMapHeaderAsync( map, CancellationToken.None );
		}

		/// <summary>
		///		Bookkeep dictionary count to be packed on current stream asynchronously.
		/// </summary>
		/// <param name="map">Dictionary count to be written.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackMapHeaderAsync<TKey, TValue>( IDictionary<TKey, TValue> map, CancellationToken cancellationToken )
		{
			return map == null ? this.PackNullAsync( cancellationToken ) : this.PackMapHeaderAsync( map.Count, cancellationToken );
		}

#endif // FEATURE_TAP

		#endregion -- IDictionary --

		#region -- Ext --

		/// <summary>
		///		Packs an extended type value.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="body"/> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Packer PackExtendedTypeValue( byte typeCode, byte[] body )
		{
			if ( body == null )
			{
				ThrowArgumentNullException( "body" );
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) != 0 )
			{
				ThrowExtTypeIsProhibitedException();
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			this.PackExtendedTypeValueCore( typeCode, body );
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
				ThrowMissingBodyOfExtTypeValueException( "mpeto" );
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) != 0 )
			{
				ThrowExtTypeIsProhibitedException();
			}

			this.PackExtendedTypeValueCore( mpeto.TypeCode, mpeto.Body );
			return this;
		}

		/// <summary>
		///		Packs an extended type value.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		protected virtual void PackExtendedTypeValueCore( byte typeCode, byte[] body )
		{
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

#if FEATURE_TAP
		/// <summary>
		///		Packs an extended type value asynchronously.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="body"/> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackExtendedTypeValueAsync( byte typeCode, byte[] body )
		{
			return this.PackExtendedTypeValueAsync( typeCode, body, CancellationToken.None );
		}

		/// <summary>
		///		Packs an extended type value asynchronously.
		/// </summary>
		/// <param name="mpeto">A <see cref="MessagePackExtendedTypeObject"/> to be packed.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentException"><see cref="MessagePackExtendedTypeObject.IsValid"/> of <paramref name="mpeto"/> is <c>false</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackExtendedTypeValueAsync( MessagePackExtendedTypeObject mpeto )
		{
			return this.PackExtendedTypeValueAsync( mpeto, CancellationToken.None );
		}

		/// <summary>
		///		Packs an extended type value asynchronously.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="body"/> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackExtendedTypeValueAsync( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			if ( body == null )
			{
				ThrowArgumentNullException( "body" );
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) != 0 )
			{
				ThrowExtTypeIsProhibitedException();
			}

			this.VerifyNotDisposed();
			Contract.EndContractBlock();

			return this.PackExtendedTypeValueAsyncCore( typeCode, body, cancellationToken );
		}

		/// <summary>
		///		Packs an extended type value asynchronously.
		/// </summary>
		/// <param name="mpeto">A <see cref="MessagePackExtendedTypeObject"/> to be packed.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentException"><see cref="MessagePackExtendedTypeObject.IsValid"/> of <paramref name="mpeto"/> is <c>false</c>.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompatibilityOptions"/> property contains <see cref="PackerCompatibilityOptions.ProhibitExtendedTypeObjects"/>.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public Task PackExtendedTypeValueAsync( MessagePackExtendedTypeObject mpeto, CancellationToken cancellationToken )
		{
			if ( !mpeto.IsValid )
			{
				ThrowMissingBodyOfExtTypeValueException( "mpeto" );
			}

			if ( ( this._compatibilityOptions & PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) != 0 )
			{
				ThrowExtTypeIsProhibitedException();
			}

			return this.PackExtendedTypeValueAsyncCore( mpeto.TypeCode, mpeto.Body, cancellationToken );
		}

		/// <summary>
		///		Packs an extended type value asynchronously.
		/// </summary>
		/// <param name="typeCode">A type code of the extended type value.</param>
		/// <param name="body">A binary value portion of the extended type value.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		protected virtual async Task PackExtendedTypeValueAsyncCore( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			switch ( body.Length )
			{
				case 1:
				{
					await this.WriteByteAsync( MessagePackCode.FixExt1, cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 2:
				{
					await this.WriteByteAsync( MessagePackCode.FixExt2, cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 4:
				{
					await this.WriteByteAsync( MessagePackCode.FixExt4, cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 8:
				{
					await this.WriteByteAsync( MessagePackCode.FixExt8, cancellationToken ).ConfigureAwait( false );
					break;
				}
				case 16:
				{
					await this.WriteByteAsync( MessagePackCode.FixExt16, cancellationToken ).ConfigureAwait( false );
					break;
				}
				default:
				{
					unchecked
					{
						if ( body.Length < 0x100 )
						{
							await this.WriteByteAsync( MessagePackCode.Ext8, cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( body.Length & 0xFF ), cancellationToken ).ConfigureAwait( false );
						}
						else if ( body.Length < 0x10000 )
						{
							await this.WriteByteAsync( MessagePackCode.Ext16, cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( ( body.Length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( body.Length & 0xFF ), cancellationToken ).ConfigureAwait( false );
						}
						else
						{
							await this.WriteByteAsync( MessagePackCode.Ext32, cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( ( body.Length >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( ( body.Length >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( ( body.Length >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
							await this.WriteByteAsync( ( byte )( body.Length & 0xFF ), cancellationToken ).ConfigureAwait( false );
						}
					}

					break;
				}
			} // switch

			await this.WriteByteAsync( typeCode, cancellationToken ).ConfigureAwait( false );
			await this.WriteBytesAsync( body, true, cancellationToken ).ConfigureAwait( false );
		}
#endif // FEATURE_TAP

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

#if FEATURE_TAP
		private async Task StreamWriteAsync<TItem>( IEnumerable<TItem> value, Func<IEnumerable<TItem>, PackingOptions, CancellationToken, Task> writeBody, PackingOptions options, CancellationToken cancellationToken )
		{
			if ( this.CanSeek )
			{
				// Reserve length
				this.SeekTo( 4L );
				var headerPosition = this.Position;
				// Write body
				await writeBody( value, options, cancellationToken ).ConfigureAwait( false );
				var bodyLength = this.Position - headerPosition;
				// Back to reserved length
				this.SeekTo( -bodyLength );
				this.SeekTo( -4L );
				unchecked
				{
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( bodyLength & 0xFF ), cancellationToken ).ConfigureAwait( false );
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
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 24 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 16 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( ( bodyLength >> 8 ) & 0xFF ), cancellationToken ).ConfigureAwait( false );
					await this.WriteByteAsync( ( byte )( bodyLength & 0xFF ), cancellationToken ).ConfigureAwait( false );
				}

				await writeBody( asCollection, options, cancellationToken ).ConfigureAwait( false );
			}
		}
#endif // FEATURE_TAP

	}
}
