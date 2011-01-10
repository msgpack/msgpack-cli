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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace MsgPack.Collections
{
	internal sealed class GCChunkBuffer : ChunkBuffer
	{
		private readonly IList<ArraySegment<byte>> _chunks;

		public sealed override int Count
		{
			get { return this._chunks.Count; }
		}

		private long _totalLength;

		public sealed override long TotalLength
		{
			get { return this._totalLength; }
		}

		public GCChunkBuffer( IList<ArraySegment<byte>> chunks, long initialTotalLength )
		{
			Contract.Assume( chunks != null );
			Contract.Assume( initialTotalLength >= 0 );

			this._chunks = chunks;
			this._totalLength = initialTotalLength;
		}

		protected sealed override ArraySegment<byte> GetAt( int index )
		{
			if ( index < 0 || index >= this._chunks.Count )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			return this._chunks[ index ];
		}

		public sealed override void Feed( ArraySegment<byte> newSegment )
		{
			this._chunks.Add( newSegment );
			this._totalLength += newSegment.Count;
		}

		public sealed override bool Contains( ArraySegment<byte> item )
		{
			return this._chunks.Contains( item );
		}

		public sealed override int IndexOf( ArraySegment<byte> item )
		{
			return this._chunks.IndexOf( item );
		}

		public sealed override IEnumerator<ArraySegment<byte>> GetEnumerator()
		{
			return this._chunks.GetEnumerator();
		}

		protected sealed override void CopyToCore( ArraySegment<byte>[] array, int arrayIndex )
		{
			this._chunks.CopyTo( array, arrayIndex );
		}

		protected sealed override ChunkBuffer SubChunksCore( long newOffset, long newTotalLength )
		{
			int startSegmentIndex = -1;
			int startOffsetInHeadSegment = -1;
			int endSegmentIndex = -1;
			int endOffsetInTailSegment = -1;

			this.FindNewSegment( newOffset, newTotalLength, ref startSegmentIndex, ref startOffsetInHeadSegment, ref endSegmentIndex, ref endOffsetInTailSegment );
			Contract.Assert( startSegmentIndex >= 0 );
			Contract.Assert( startOffsetInHeadSegment >= 0 );
			Contract.Assert( endSegmentIndex >= 0 );
			Contract.Assert( endOffsetInTailSegment >= 0 );

			var newChunks = new List<ArraySegment<byte>>( endSegmentIndex - startSegmentIndex + 1 );

			if ( newChunks.Count == 1 )
			{
				var newItem =
					new ArraySegment<byte>( this._chunks[ startSegmentIndex ].Array, startOffsetInHeadSegment, endOffsetInTailSegment - startOffsetInHeadSegment );
				newChunks.Add( newItem );
				return new GCChunkBuffer( newChunks, newItem.Count );
			}

			long newLength = 0;
			int index = 0;
			foreach ( var newItem in this._chunks.Skip( startSegmentIndex ).Take( endSegmentIndex - startSegmentIndex + 1 ) )
			{
				if ( index == 0 )
				{
					// head
					newChunks.Add( new ArraySegment<byte>( newItem.Array, startOffsetInHeadSegment, newItem.Count - startOffsetInHeadSegment ) );
					newLength += newItem.Count - startOffsetInHeadSegment;
				}
				else if ( index == endSegmentIndex - startSegmentIndex )
				{
					// tail
					newChunks.Add( new ArraySegment<byte>( newItem.Array, newItem.Offset, endOffsetInTailSegment ) );
					newLength += endOffsetInTailSegment;
				}
				else
				{
					newChunks.Add( newItem );
					newLength += newItem.Count;
				}
			}

			return new GCChunkBuffer( newChunks, newLength );
		}

		private void FindNewSegment( long offset, long count, ref int startSegmentIndex, ref int startOffsetInHeadSegment, ref int endSegmentIndex, ref int endOffsetInTailSegment )
		{
			long position = 0;
			for ( int segmentIndex = 0; segmentIndex < this._chunks.Count; segmentIndex++ )
			{
				var segment = this._chunks[ segmentIndex ];
				for ( int offsetInSegment = 0; offsetInSegment < segment.Count; offsetInSegment++ )
				{
					if ( position == offset )
					{
						Contract.Assert( startSegmentIndex == -1 );
						Contract.Assert( startOffsetInHeadSegment == -1 );
						startSegmentIndex = segmentIndex;
						startOffsetInHeadSegment = offsetInSegment;
					}

					if ( position == offset + count )
					{
						Contract.Assert( endSegmentIndex == -1 );
						Contract.Assert( endOffsetInTailSegment == -1 );
						endSegmentIndex = segmentIndex;
						endOffsetInTailSegment = offsetInSegment;
						return;
					}

					position++;
				}
			}
		}
	}
}
