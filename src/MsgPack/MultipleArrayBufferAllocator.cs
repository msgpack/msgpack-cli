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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1

namespace MsgPack
{
	/// <summary>
	///		An implementation of <see cref="ByteBufferAllocator"/> which allocate a new single array and append to the list.
	/// </summary>
	internal sealed class MultipleArrayBufferAllocator : ByteBufferAllocator
	{
		public static readonly ByteBufferAllocator Default = new MultipleArrayBufferAllocator( 64 * 1024 );

		private readonly Func<int, ArraySegment<byte>> _allocator;

		public MultipleArrayBufferAllocator( int allocationUnitSize )
		{
			if ( allocationUnitSize <= 0 )
			{
				throw new ArgumentOutOfRangeException( "allocationUnitSize", "The value must be positive." );
			}

			this._allocator = _ => new ArraySegment<byte>( new byte[ allocationUnitSize ] );
		}

		public MultipleArrayBufferAllocator( Func<int, ArraySegment<byte>> allocator )
		{
#if DEBUG
			Contract.Assert( allocator != null, "allocator != null" );
#endif // DEBUG
			this._allocator = allocator;
		}

		public override bool TryAllocate( IList<ArraySegment<byte>> buffers, int sizeHint, ref int newCurrentBufferIndex, out ArraySegment<byte> newCurrentBuffer )
		{
			if ( buffers.IsReadOnly || buffers.Count == Int32.MaxValue )
			{
				newCurrentBuffer = default( ArraySegment<byte> );
				return false;
			}
			
			var newSegment = this._allocator( sizeHint );
			if ( newSegment.Count == 0 || newSegment.Array == null )
			{
				newCurrentBuffer = default( ArraySegment<byte> );
				return false;
			}

			buffers.Add( newSegment );

			newCurrentBuffer = newSegment;
			newCurrentBufferIndex = buffers.Count - 1;
			return true;
		}
	}
}
