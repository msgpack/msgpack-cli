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
using System.Diagnostics;

namespace MsgPack
{
	/// <summary>
	///		Manages internal thread-local buffers for packer/unpacker.
	/// </summary>
	internal static class BufferManager
	{
#if DEBUG
		[ThreadStatic]
		private static bool _isByteBufferUsed;
		[ThreadStatic]
		private static bool _isCharBufferUsed;
#endif // DEBUG

		[ThreadStatic]
		private static byte[] _byteBuffer;

		public static byte[] GetByteBuffer()
		{
#if DEBUG
			if ( _isByteBufferUsed )
			{
				throw new InvalidOperationException("ByteBuffer is already used.");
			}

			_isByteBufferUsed = true;
#endif // DEBUG

			if ( _byteBuffer == null )
			{
				_byteBuffer = new byte[ 32 * 1024 ];
			}

			return _byteBuffer;
		}

		[Conditional( "DEBUG" )]
		public static void ReleaseByteBuffer()
		{
#if DEBUG
			_isByteBufferUsed = false;
#endif // DEBUG
		}

		[ThreadStatic]
		private static char[] _charBuffer;

		public static char[] GetCharBuffer()
		{
#if DEBUG
			if ( _isCharBufferUsed )
			{
				throw new InvalidOperationException( "ByteBuffer is already used." );
			}

			_isCharBufferUsed = true;
#endif // DEBUG

			if ( _charBuffer == null )
			{
				_charBuffer = new char[ 32 * 1024 ];
			}

			return _charBuffer;
		}

		[Conditional( "DEBUG" )]
		public static void ReleaseCharBuffer()
		{
#if DEBUG
			_isCharBufferUsed = false;
#endif // DEBUG
		}
	}
}