#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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

namespace MsgPack
{
	internal sealed class BufferPool
	{
		public const int ByteBufferSize = 64 * 1024;
		public const int CharBufferSize = 32 * 1024;

#if DEBUG
		private bool _isByteBufferUsed;
		private bool _isCharBufferUsed;
#endif // DEBUG

		private byte[] _byteBuffer;

		public byte[] GetByteBuffer()
		{
#if DEBUG
			if ( this._isByteBufferUsed )
			{
				throw new InvalidOperationException( "ByteBuffer is already used." );
			}

			this._isByteBufferUsed = true;
#endif // DEBUG

			if ( this._byteBuffer == null )
			{
				this._byteBuffer = new byte[ ByteBufferSize ];
			}

			return this._byteBuffer;
		}

		public void ReleaseByteBuffer()
		{
#if DEBUG
			this._isByteBufferUsed = false;
#endif // DEBUG
		}

		private char[] _charBuffer;

		public char[] GetCharBuffer()
		{
#if DEBUG
			if ( this._isCharBufferUsed )
			{
				throw new InvalidOperationException( "ByteBuffer is already used." );
			}

			this._isCharBufferUsed = true;
#endif // DEBUG

			if ( this._charBuffer == null )
			{
				this._charBuffer = new char[ CharBufferSize ];
			}

			return this._charBuffer;
		}

		public void ReleaseCharBuffer()
		{
#if DEBUG
			this._isCharBufferUsed = false;
#endif // DEBUG
		}
	}
}