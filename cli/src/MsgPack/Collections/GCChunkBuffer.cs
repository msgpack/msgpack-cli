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
using System.Globalization;

namespace MsgPack.Collections
{
	/// <summary>
	///		Simple <see cref="ChunkBuffer"/> implementation which relys on runtime GC, so it does not handle any memory management.
	/// </summary>
	internal sealed class GCChunkBuffer : ChunkBuffer
	{
		private readonly List<ArraySegment<byte>> _chunks;

		public sealed override int Count
		{
			get { return this._chunks.Count; }
		}

		private long _totalLength;

		public sealed override long TotalLength
		{
			get { return this._totalLength; }
		}

		internal GCChunkBuffer( List<ArraySegment<byte>> chunks, long initialTotalLength )
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

		protected sealed override ChunkBuffer ClipCore( long offset, long length )
		{
			// Short-cut pass
			if ( offset == 0L && length == this.TotalLength )
			{
				// Whole slice is required, so just return clone of this.
				var clone = new GCChunkBuffer( new List<ArraySegment<byte>>( this._chunks ), this._totalLength );
				this._chunks.Clear();
				return clone;
			}

			int startSegmentIndex = -1;
			int startOffsetInHeadSegment = -1;
			int endSegmentIndex = -1;
			int endOffsetInTailSegment = -1;

			this.FindNewSegment( offset, length, ref startSegmentIndex, ref startOffsetInHeadSegment, ref endSegmentIndex, ref endOffsetInTailSegment );
			Contract.Assert( startSegmentIndex >= 0, "startSegmentIndex >= 0 : " + startSegmentIndex );
			Contract.Assert( startOffsetInHeadSegment >= 0, "startOffsetInHeadSegment >= 0 ; " + startOffsetInHeadSegment );
			Contract.Assert( endSegmentIndex >= 0, "endSegmentIndex >= 0 : " + endSegmentIndex );
			Contract.Assert( endOffsetInTailSegment >= 0, "endOffsetInTailSegment >= 0 : " + endOffsetInTailSegment );

			var newChunks = new List<ArraySegment<byte>>( endSegmentIndex - startSegmentIndex + 1 );
			long newLength = 0;

			if ( ( endSegmentIndex - startSegmentIndex + 1 ) == 1 )
			{
				var newItem =
					new ArraySegment<byte>( this._chunks[ startSegmentIndex ].Array, startOffsetInHeadSegment, endOffsetInTailSegment - startOffsetInHeadSegment );
				newChunks.Add( newItem );
				newLength = newItem.Count;
			}
			else
			{
				int index = 0;
				foreach ( var newItem in this._chunks.Skip( startSegmentIndex ).Take( endSegmentIndex - startSegmentIndex + 1 ) )
				{
					if ( index == 0 )
					{
						// head
						newChunks.Add( new ArraySegment<byte>( newItem.Array, newItem.Offset + startOffsetInHeadSegment, newItem.Count - startOffsetInHeadSegment ) );
						newLength += newItem.Count - startOffsetInHeadSegment;
					}
					else if ( index == endSegmentIndex - startSegmentIndex )
					{
						// tail
						if ( endOffsetInTailSegment > 0 ) // Avoid adding empty segment
						{
							newChunks.Add( new ArraySegment<byte>( newItem.Array, newItem.Offset, endOffsetInTailSegment ) );
						}

						newLength += endOffsetInTailSegment;
					}
					else
					{
						newChunks.Add( newItem );
						newLength += newItem.Count;
					}

					index++;
				}
			}

			// Remove clipped range
			int startRemovalIndex = startSegmentIndex;
			int removalSegmentCount = 0;

			if ( startSegmentIndex < endSegmentIndex )
			{
				removalSegmentCount = endSegmentIndex - startSegmentIndex + 1;
				if ( startOffsetInHeadSegment > 0 )
				{
					// first clipped segment cannot be remove.
					startRemovalIndex += 1;
					removalSegmentCount -= 1;
				}

				if ( endSegmentIndex == this._chunks.Count
					|| endOffsetInTailSegment < this._chunks[ endSegmentIndex ].Count )
				{
					removalSegmentCount -= 1;
				}
			}

			if ( removalSegmentCount > 0 )
			{
				this._chunks.RemoveRange( startRemovalIndex, removalSegmentCount );
			}

			// Arrange start segment and tail segment.
			if ( startSegmentIndex == endSegmentIndex )
			{
				// The segment must be devided.
				var originalSegment = this._chunks[ startSegmentIndex ];
				this._chunks[ startSegmentIndex ] = new ArraySegment<byte>( originalSegment.Array, originalSegment.Offset, startOffsetInHeadSegment );
				var remaining = new ArraySegment<byte>( originalSegment.Array, originalSegment.Offset + endOffsetInTailSegment, originalSegment.Count - endOffsetInTailSegment );
				// It is OK to append tail of list instead of insert next.
				this._chunks.Add( remaining );
			}
			else
			{
				if ( startOffsetInHeadSegment > 0 )
				{
					// New head is not current head
					this._chunks[ startSegmentIndex ] = new ArraySegment<byte>( this._chunks[ startSegmentIndex ].Array, this._chunks[ startSegmentIndex ].Offset, startOffsetInHeadSegment );
				}

				int currentEndSegmentIndex = endSegmentIndex - removalSegmentCount;
				if ( endSegmentIndex < this._chunks.Count
					&& endOffsetInTailSegment < this._chunks[ currentEndSegmentIndex ].Count )
				{
					// New tail is not current tail
					this._chunks[ currentEndSegmentIndex ] = new ArraySegment<byte>( this._chunks[ currentEndSegmentIndex ].Array, this._chunks[ currentEndSegmentIndex ].Offset + endOffsetInTailSegment, this._chunks[ currentEndSegmentIndex ].Count - endOffsetInTailSegment );
				}
			}

			return new GCChunkBuffer( newChunks, newLength );
		}

		private void FindNewSegment( long offset, long count, ref int startSegmentIndex, ref int startOffsetInHeadSegment, ref int endSegmentIndex, ref int endOffsetInTailSegment )
		{
			long positionOfWhole = 0;
			for ( int segmentIndex = 0; segmentIndex < this._chunks.Count; segmentIndex++ )
			{
				var segment = this._chunks[ segmentIndex ];
				for ( int offsetInSegment = 0; offsetInSegment < segment.Count; offsetInSegment++ )
				{
					if ( positionOfWhole == offset )
					{
						Contract.Assert( startSegmentIndex == -1 );
						Contract.Assert( startOffsetInHeadSegment == -1 );
						startSegmentIndex = segmentIndex;
						startOffsetInHeadSegment = offsetInSegment;
					}

					if ( positionOfWhole == offset + count )
					{
						Contract.Assert( endSegmentIndex == -1 );
						Contract.Assert( endOffsetInTailSegment == -1 );
						endSegmentIndex = segmentIndex;
						endOffsetInTailSegment = offsetInSegment;
						return;
					}

					positionOfWhole++;
				}
			}

			Contract.Assert( startSegmentIndex > -1 );
			Contract.Assert( startOffsetInHeadSegment > -1 );
			// set dummy
			endSegmentIndex = this._chunks.Count;
			endOffsetInTailSegment = 0;
		}
	}
}
