#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
using System.Collections.Generic;

namespace MsgPack
{
	/// <summary>
	///		An implementation of <see cref="ByteBufferAllocator"/> which reallocte a new single array and copy old contents to it.
	/// </summary>
	internal sealed class SingleArrayBufferAllocator : ByteBufferAllocator
	{
		public static readonly SingleArrayBufferAllocator Default = new SingleArrayBufferAllocator( Allocate );

		private readonly Func<byte[], int, byte[]> _allocator;

		public SingleArrayBufferAllocator( Func<byte[], int, byte[]> allocator )
		{
			this._allocator = allocator;
		}

		private static byte[] Allocate( byte[] old, int requestSize )
		{
			if ( old.Length < 256 )
			{
				return new byte[ 256 ];
			}

			// Use golden ratio to improve linear memory range reusability (of LOH)
			var newSize = Math.Max( ( long )( old.Length * 1.1618 ), requestSize + ( long )old.Length );
			if ( newSize > Int32.MaxValue )
			{
				return null;
			}

			return new byte[ newSize ];
		}

		public override bool TryAllocate( byte[] oldBuffer, int requestSize, out byte[] newBuffer )
		{
			newBuffer = this._allocator( oldBuffer, requestSize );
			if ( newBuffer == null || newBuffer.Length < ( oldBuffer.Length + requestSize ) )
			{
				newBuffer = null;
				return false;
			}

			Buffer.BlockCopy( oldBuffer, 0, newBuffer, 0, oldBuffer.Length );
			return true;
		}
	}
}
