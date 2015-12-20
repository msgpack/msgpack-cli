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
#if FEATURE_TAP
using System.Collections.Concurrent;
#endif // FEATURE_TAP
using System.Diagnostics;
#if FEATURE_TAP
using System.Linq;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Manages internal thread-local buffers for packer/unpacker.
	/// </summary>
	internal static class BufferManager
	{
		private const int ByteBufferSize = 64 * 1024;
		private const int CharBufferSize = 32 * 1024;

#if DEBUG
		[ThreadStatic]
		private static bool _isByteBufferUsed;

		[ThreadStatic]
		private static bool _isCharBufferUsed;
#endif // DEBUG

		[ThreadStatic]
		private static byte[] _byteBuffer;

		[ThreadStatic]
		private static char[] _charBuffer;

		public static byte[] GetByteBuffer()
		{
#if DEBUG
			if ( _isByteBufferUsed )
			{
				throw new InvalidOperationException( "ByteBuffer is already used." );
			}

			_isByteBufferUsed = true;
#endif // DEBUG

			if ( _byteBuffer == null )
			{
				_byteBuffer = new byte[ ByteBufferSize ];
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
				_charBuffer = new char[ CharBufferSize ];
			}

			return _charBuffer;
		}

		[Conditional("DEBUG")]
		public static void ReleaseCharBuffer()
		{
#if DEBUG
			_isCharBufferUsed = false;
#endif // DEBUG
		}


#if FEATURE_TAP

		private static readonly int InitialAsyncBufferPoolSize = Environment.ProcessorCount;

		private static readonly int MaxAsyncBufferPoolSize = InitialAsyncBufferPoolSize * 16;

		private static readonly ConcurrentQueue<byte[]> _globalByteBufferPool =
			new ConcurrentQueue<byte[]>( Enumerable.Repeat( new byte[ BufferPool.ByteBufferSize ], InitialAsyncBufferPoolSize ) );

		private static readonly ConcurrentQueue<char[]> _globalCharBufferPool =
			new ConcurrentQueue<char[]>( Enumerable.Repeat( new char[ BufferPool.CharBufferSize ], InitialAsyncBufferPoolSize ) );

		public static byte[] GetAsyncByteBuffer()
		{
			byte[] buffer;
			if ( !_globalByteBufferPool.TryDequeue( out buffer ) )
			{
				buffer = new byte[ ByteBufferSize ];
			}

			return buffer;
		}

		public static void ReturnAsyncByteBuffer( byte[] buffer )
		{
			_globalByteBufferPool.Enqueue( buffer );

			if ( _globalByteBufferPool.Count > MaxAsyncBufferPoolSize )
			{
				byte[] dummy;
				_globalByteBufferPool.TryDequeue( out dummy );
			}
		}

		public static char[] GetAsyncCharBuffer()
		{
			char[] buffer;
			if ( !_globalCharBufferPool.TryDequeue( out buffer ) )
			{
				buffer = new char[ CharBufferSize ];
			}

			return buffer;
		}

		public static void ReturnAsyncCharBuffer( char[] buffer )
		{
			_globalCharBufferPool.Enqueue( buffer );

			if ( _globalCharBufferPool.Count > MaxAsyncBufferPoolSize )
			{
				char[] dummy;
				_globalCharBufferPool.TryDequeue( out dummy );
			}
		}

#endif // FEATURE_TAP
	}
}