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
using System.Diagnostics.Contracts;

namespace MsgPack.Collections
{
	// TODO: MPO should accept BufferChunks.
	/// <summary>
	///		Represents buffer.
	/// </summary>
	public abstract class ChunkBuffer : IList<ArraySegment<byte>>, IDisposable
	{
		/// <summary>
		///		Get count of segments.
		/// </summary>
		/// <value>Count of segments.</value>
		public abstract int Count
		{
			get;
		}

		/// <summary>
		///		Get total length of segments.
		/// </summary>
		/// <value>
		///		Total length of segments.
		/// </value>
		public abstract long TotalLength
		{
			get;
		}

		/// <summary>
		///		Get segment at specified index.
		/// </summary>
		/// <param name="index">Index of segment.</param>
		/// <returns><see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; at specified index.</returns>
		public ArraySegment<byte> this[ int index ]
		{
			get
			{
				return this.GetAt( index );
			}
		}

		bool ICollection<ArraySegment<byte>>.IsReadOnly
		{
			get { return true; }
		}

		ArraySegment<byte> IList<ArraySegment<byte>>.this[ int index ]
		{
			get { return this[ index ]; }
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		///		Release internal resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///		Release unamanaged resources, optionally managed resources.
		/// </summary>
		/// <param name="disposing">If managed resources to be released then true.</param>
		protected virtual void Dispose( bool disposing ) { }

		/// <summary>
		///		Get segment at specified index.
		/// </summary>
		/// <param name="index">Index of segument to be gotten.</param>
		/// <returns><see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; at specified index.</returns>
		protected abstract ArraySegment<byte> GetAt( int index );

		/// <summary>
		///		Determine specified <see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; is contained in this buffer.
		/// </summary>
		/// <param name="item"><see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; to check.</param>
		/// <returns>If specified <see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; is contained in this buffer then true.</returns>
		public abstract bool Contains( ArraySegment<byte> item );

		/// <summary>
		///		Get index of specified <see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; in this buffer.
		/// </summary>
		/// <param name="item"><see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; to check.</param>
		/// <returns>If specified <see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; is contained in this buffer, its index, otherwise -1.</returns>
		public abstract int IndexOf( ArraySegment<byte> item );

		/// <summary>
		///		Get enumerator of this buffer.
		/// </summary>
		/// <returns><see cref="IEnumerator&lt;T&gt;"/> to enumerate segments.</returns>
		public abstract IEnumerator<ArraySegment<byte>> GetEnumerator();

		/// <summary>
		///		Copy buffer contents to specified array.
		/// </summary>
		/// <param name="array">Array to be copies.</param>
		/// <param name="arrayIndex">Index to start copying in <paramref name="array"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="array"/> is too small to fulfill contents from specified <paramref name="arrayIndex"/>.</exception>
		public void CopyTo( ArraySegment<byte>[] array, int arrayIndex )
		{
			if ( array == null )
			{
				throw new ArgumentNullException( "array" );
			}

			if ( arrayIndex < 0 )
			{
				throw new ArgumentOutOfRangeException( "arrayIndex" );
			}

			if ( array.Length <= arrayIndex + this.Count )
			{
				throw new ArgumentException( "Array too small.", "array" );
			}

			Contract.EndContractBlock();

			this.CopyToCore( array, arrayIndex );
		}

		/// <summary>
		///		Copy buffer contents to specified array.
		/// </summary>
		/// <param name="array">Array to be copies.</param>
		/// <param name="arrayIndex">Index to start copying in <paramref name="array"/>.</param>
		protected abstract void CopyToCore( ArraySegment<byte>[] array, int arrayIndex );

		/// <summary>
		///		Allocate internal chunks to satisfy specified additinoal length.
		/// </summary>
		/// <param name="requiredAdditionalLength">Required length to be satisfied.</param>
		/// <returns>New or existing buffer which satisfiues sepcified length, and contains this buffer's contents.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="requiredAdditionalLength"/> is less than 0 or equal to 0.</exception>
		/// <exception cref="InsufficientMemoryException">Required length is too large.</exception>
		public ChunkBuffer Reallocate( long requiredAdditionalLength )
		{
			if ( requiredAdditionalLength <= 0 )
			{
				throw new ArgumentOutOfRangeException( "requiredAdditionalLength", "'requiredAdditionalLength' is must be positive." );
			}

			return this.ReallocateCore( requiredAdditionalLength );
		}

		/// <summary>
		///		Allocate internal chunks to satisfy specified additinoal length.
		/// </summary>
		/// <param name="requiredAdditionalLength">Required length to be satisfied. This value is positive.</param>
		/// <returns>New or existing buffer which satisfiues sepcified length, and contains this buffer's contents.</returns>
		/// <exception cref="InsufficientMemoryException">Required length is too large.</exception>
		/// <remarks>
		///		Derived class can:
		///		<list type="bullet">
		///			<item>Reuse this instance with increasing internal buffer.</item>
		///			<item>Create new instance which has enough buffer size.</item>
		///			<item>Returned instance has more capacity than required.</item>
		///		</list>
		///		Derived class must:
		///		<list type="bullet">
		///			<item>Return buffer which has enough capacity. When it is not satisifiable, throw InsufficientBufferException.</item>
		///			<item>Ensure that returned buffer has identical contents to this instance.</item>
		///			<item>Preserve Count and Offset of each segments are identical to previous state. Caller must be able to continue its process from next to last segment.</item>
		///		</list>
		/// </remarks>
		protected abstract ChunkBuffer ReallocateCore( long requiredAdditionalLength );

		/// <summary>
		///		Get sub chunks which has specified offset and totalLength of this buffer.
		/// </summary>
		/// <param name="newOffset">New offset of this buffer which returning buffer will have.</param>
		/// <param name="newTotalLength">New total length of returning buffer.</param>
		/// <returns>
		///		New <see cref="ChunkBuffer"/> which starts with <paramref name="newOffset"/> and its length is <paramref name="newTotalLength"/>.
		///	</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="newOffset"/> is less than 0, or not less than <see cref="TotalLength"/> of this buffer.
		///		Or <paramref name="newTotalLength"/> is less than 0, or not less than <see cref="TotalLength"/> of this buffer minus <paramref name="newOffset"/>.
		/// </exception>
		public ChunkBuffer SubChunks( long newOffset, long newTotalLength )
		{
			if ( newOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", "'offset' must not be negative." );
			}

			if ( newOffset >= this.TotalLength )
			{
				throw new ArgumentOutOfRangeException( "offset", "'offset' must be less than TotalLength." );
			}

			if ( newTotalLength < 0 )
			{
				throw new ArgumentOutOfRangeException( "count", "'count' must not be negative." );
			}

			if ( newTotalLength > ( this.TotalLength - newOffset ) )
			{
				throw new ArgumentOutOfRangeException( "count", "'count' must be less than or equal ( TotalLength - offset )." );
			}

			Contract.EndContractBlock();

			return this.SubChunksCore( newOffset, newTotalLength );

		}

		/// <summary>
		///		Get sub chunks which has specified offset and totalLength of this buffer.
		/// </summary>
		/// <param name="newOffset">New offset of this buffer which returning buffer will have.</param>
		/// <param name="newTotalLength">New total length of returning buffer.</param>
		/// <returns>
		///		New <see cref="ChunkBuffer"/> which starts with <paramref name="newOffset"/> and its length is <paramref name="newTotalLength"/>.
		///	</returns>
		protected abstract ChunkBuffer SubChunksCore( long newOffset, long newTotalLength );
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void ICollection<ArraySegment<byte>>.Add( ArraySegment<byte> item )
		{
			throw new NotSupportedException();
		}

		void ICollection<ArraySegment<byte>>.Clear()
		{
			throw new NotSupportedException();
		}

		void IList<ArraySegment<byte>>.Insert( int index, ArraySegment<byte> item )
		{
			throw new NotSupportedException();
		}

		bool ICollection<ArraySegment<byte>>.Remove( ArraySegment<byte> item )
		{
			throw new NotSupportedException();
		}

		void IList<ArraySegment<byte>>.RemoveAt( int index )
		{
			throw new NotSupportedException();
		}
	}
}
