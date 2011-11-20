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

using System;
using System.Diagnostics.Contracts;

namespace MsgPack
{
	// This file generated from Unpacker.Direct.tt T4Template.
	// Do not modify this file. Edit Unpacker.Direct.tt instead.

	partial class Unpacker
	{

		/// <summary>
		///		Unpack <see cref="Byte"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Byte"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Byte UnpackByte()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackByte( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="SByte"/> from current buffer.
		/// </summary>
		/// <returns><see cref="SByte"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="SByte"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		[CLSCompliant( false )]
		public SByte UnpackSByte()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackSByte( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="Int16"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Int16"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Int16"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Int16 UnpackInt16()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackInt16( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="UInt16"/> from current buffer.
		/// </summary>
		/// <returns><see cref="UInt16"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="UInt16"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		[CLSCompliant( false )]
		public UInt16 UnpackUInt16()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackUInt16( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="Int32"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Int32"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Int32"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Int32 UnpackInt32()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackInt32( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="UInt32"/> from current buffer.
		/// </summary>
		/// <returns><see cref="UInt32"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="UInt32"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		[CLSCompliant( false )]
		public UInt32 UnpackUInt32()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackUInt32( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="Int64"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Int64"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Int64"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Int64 UnpackInt64()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackInt64( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="UInt64"/> from current buffer.
		/// </summary>
		/// <returns><see cref="UInt64"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="UInt64"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		[CLSCompliant( false )]
		public UInt64 UnpackUInt64()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackUInt64( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="Single"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Single"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Single"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Single UnpackSingle()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackSingle( this._currentSource.Stream );
		}

		/// <summary>
		///		Unpack <see cref="Double"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Double"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Double"/>.</exception>
		/// <remarks>
		///		This method is direct API, so <see cref="Data"/> will be invalidated.
		/// </remarks>
		public Double UnpackDouble()
		{
			this.VerifyNotDisposed();
			this.VerifyMode( UnpackerMode.Direct );
			Contract.EndContractBlock();

			return Unpacking.UnpackDouble( this._currentSource.Stream );
		}
	}
}