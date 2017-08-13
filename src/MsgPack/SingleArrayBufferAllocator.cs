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

		private readonly Func<ArraySegment<byte>, int, ArraySegment<byte>> _allocator;

		public SingleArrayBufferAllocator( Func<ArraySegment<byte>, int, ArraySegment<byte>> allocator )
		{
			this._allocator = allocator;
		}

		private static ArraySegment<byte> Allocate( ArraySegment<byte> old, int sizeHint )
		{
			// Use golden ratio to improve linear memory range reusability (of LOH)
			var newSize = Math.Max( ( long )( old.Count * 1.1618 ), sizeHint + ( long )old.Count );
			if ( newSize > Int32.MaxValue )
			{
				return default( ArraySegment<byte> );
			}

			return new ArraySegment<byte>( new byte[ newSize ] );
		}

		public override bool TryAllocate( IList<ArraySegment<byte>> buffers, int sizeHint, ref int newCurrentBufferIndex, out ArraySegment<byte> newCurrentBuffer )
		{
			if ( buffers.Count != 1 )
			{
				throw new ArgumentException( "buffers must be single array.", nameof( buffers ) );
			}

			var current = buffers[ 0 ];
			var newSegment = this._allocator( current, sizeHint );
			if ( newSegment.Count < ( current.Count + sizeHint ) || newSegment.Array == null )
			{
				newCurrentBuffer = default( ArraySegment<byte> );
				return false;
			}
			Buffer.BlockCopy( current.Array, current.Offset, newSegment.Array, newSegment.Offset, current.Count );
			buffers[ 0 ] = newSegment;
			newCurrentBufferIndex = 0;
			newCurrentBuffer = new ArraySegment<byte>( newSegment.Array, newSegment.Offset + current.Count, newSegment.Count - current.Count );

			return true;
		}
	}
}
