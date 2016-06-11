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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file generated from Packer.Nullable.tt T4Template.
	// Do not modify this file. Edit Packer.Nullable.tt instead.

	partial class Packer
	{
		/// <summary>
		///		Pack nullable <see cref="SByte"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		[CLSCompliant( false )]
		public Packer Pack( SByte? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="SByte"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( SByte? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="SByte"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( SByte? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Byte"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Byte? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Byte"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Byte? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Byte"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Byte? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int16"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Int16? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int16"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int16? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Int16"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int16? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt16"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		[CLSCompliant( false )]
		public Packer Pack( UInt16? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt16"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt16? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="UInt16"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt16? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int32"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Int32? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int32"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int32? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Int32"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int32? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt32"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		[CLSCompliant( false )]
		public Packer Pack( UInt32? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt32"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt32? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="UInt32"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt32? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int64"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Int64? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Int64"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int64? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Int64"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Int64? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt64"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		[CLSCompliant( false )]
		public Packer Pack( UInt64? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="UInt64"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt64? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="UInt64"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		[CLSCompliant( false )]
		public Task PackAsync( UInt64? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Single"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Single? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Single"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Single? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Single"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Single? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Double"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Double? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Double"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Double? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Double"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Double? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This instance.</returns>
		public Packer Pack( Boolean? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}

#if FEATURE_TAP

		/// <summary>
		///		Pack nullable <see cref="Boolean"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Boolean? value )
		{
			return this.PackAsync( value, CancellationToken.None );
		}

		/// <summary>
		///		Pack nullable <see cref="Boolean"/> value asynchronously.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public Task PackAsync( Boolean? value, CancellationToken cancellationToken )
		{
			return value.HasValue ? this.PackAsync( value.Value, cancellationToken ) : this.PackNullAsync( cancellationToken );
		}

#endif // FEATURE_TAP

	}
}
