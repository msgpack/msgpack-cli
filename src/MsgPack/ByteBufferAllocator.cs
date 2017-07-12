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
	///		Defines common interface for byte buffer allocators.
	/// </summary>
	internal abstract class ByteBufferAllocator
	{
		// This type should be public when the design is fixed for Span<byte>

		public abstract bool TryAllocate( byte[] oldBuffer, int requestSize, out /* Span<byte> */byte[] newBuffer );
	}

#warning TODO: SpanBufferAllocator
    //	internal sealed class SpanBufferAllocator : ByteBufferAllocator
    //	{
    //		private readonly Func<int, Span<byte>> _allocator;

    //		public SpanBufferAllocator( Func<int, Span<byte>> allocator)
    //		{
    //			this._allocator = allocator;
    //		}

    //		public override bool TryAllocate( IList<ArraySegment<byte>> arrayBuffers, IList<Span<byte>> spanBuffers, int sizeHint, ref int newCurrentBufferIndex, out Span<byte> newCurrentBuffer )
    //		{
    //#if DEBUG
    //			Contract.Assert( spanBuffers != null, "spanBuffers != null" );
    //			Contract.Assert( !spanBuffers.IsReadOnly, "!spanBuffers.IsReadOnly" );
    //#endif // DEBUG
    //			if ( spanBuffers.Count == Int32.MaxValue )
    //			{
    //				newCurrentBuffer = default( Span<byte> );
    //				return false;
    //			}

    //			var newBuffer = this._allocator( sizeHint );
    //			spanBuffers.Add( newBuffer );
    //			newCurrentBuffer = newBuffer;
    //			newCurrentBufferIndex = spanBuffers.Count - 1;
    //			return true;
    //		}
    //	}
}
