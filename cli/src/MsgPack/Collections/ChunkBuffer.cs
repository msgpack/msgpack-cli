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
		///		Default size of segments.
		/// </summary>
		public static readonly int DefaultSegmentSize = 32 * 1024;
		
		/// <summary>
		///		Create new default <see cref="ChunkBuffer"/>.
		/// </summary>
		/// <returns>New default <see cref="ChunkBuffer"/> instance.</returns>
		public static ChunkBuffer CreateDefault()
		{
			return CreateDefault( 1 );
		}

		/// <summary>
		///		Create new default <see cref="ChunkBuffer"/>.
		/// </summary>
		/// <returns>New default <see cref="ChunkBuffer"/> instance.</returns>
		public static ChunkBuffer CreateDefault( int initialSegmentCount )
		{
			return CreateDefault( initialSegmentCount, DefaultSegmentSize );
		}

		/// <summary>
		///		Create new default <see cref="ChunkBuffer"/>.
		/// </summary>
		/// <returns>New default <see cref="ChunkBuffer"/> instance.</returns>
		public static ChunkBuffer CreateDefault( int initialSegmentCount, int segmentSize )
		{
			if ( initialSegmentCount < 0 )
			{
				throw new ArgumentOutOfRangeException( "initialSegmentCount" );
			}

			if ( segmentSize <= 0 )
			{
				throw new ArgumentOutOfRangeException( "segmentSize" );
			}

			var initialSegments = new List<ArraySegment<byte>>( initialSegmentCount );
			for ( int i = 0; i < initialSegmentCount; i++ )
			{
				initialSegments.Add( new ArraySegment<byte>( new byte[ segmentSize ] ) );
			}

			return new GCChunkBuffer( new List<ArraySegment<byte>>( 0 ), 0 );
		}

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
		///		Feed new segment.
		/// </summary>
		/// <param name="newSegment">New segment.</param>
		public abstract void Feed( ArraySegment<byte> newSegment );

		/// <summary>
		///		Feed entire specified buffer as new segment.
		/// </summary>
		/// <param name="array">
		///		Array which whole content will be new segment.
		/// </param>
		/// <remarks>
		///		This method is equivalant to call <see cref="Feed(ArraySegment&lt;Byte&gt;)"/> with <see cref="ArraySegment&lt;T&gt;.ArraySegment(T[])"/>.
		/// </remarks>
		public void Feed( byte[] array )
		{
			this.Feed( new ArraySegment<byte>( array ) );
		}

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
		///		Reset internal state.
		/// </summary>
		public abstract void Reset();

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

			if ( array.Length < arrayIndex + this.Count )
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

#warning TODO: impl
		/// <summary>
		///		Clip out specified range from this buffer and exclude it from this buffer.
		/// </summary>
		/// <param name="offset">Offset to start clip out.</param>
		/// <param name="length">Length of clipping range.</param>
		/// <returns>
		///		<see cref="ChunkBuffer"/> which contains clipped range only.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is less than 0, or not less than <see cref="TotalLength"/> of this buffer.
		///		Or <paramref name="length"/> is less than 0, or not less than <see cref="TotalLength"/> of this buffer minus <paramref name="offset"/>.
		/// </exception>
		/// <remarks>
		///		This method does not modify underlying byte array, so this modifies segment definitions only.
		///		You should consider lifecycle of the array to achieve efficient memory management.
		///		The simplest way is allocating good size buffers (e.g. 64K bytes) and feed it to the chunks,
		///		then clipping required range from the chunks and make remains are collected by GC 
		///		(Note that '64K bytes' is platform specific, so you should specify appropriate size 
		///		 which is best to runtime environment. For desktop CLR, 64K is reasonable because 
		///		 it will not be allocated in LOH and is large enough to store medium size BLOBs/CLOBs.)
		/// </remarks>
		public ChunkBuffer Clip( long offset, long length )
		{
			if ( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", "'offset' must not be negative." );
			}

			if ( offset >= this.TotalLength )
			{
				throw new ArgumentOutOfRangeException( "offset", "'offset' must be less than TotalLength." );
			}

			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "length", "'length' must not be negative." );
			}

			if ( length > ( this.TotalLength - offset ) )
			{
				throw new ArgumentOutOfRangeException( "length", "'length' must be less than or equal ( TotalLength - offset )." );
			}

			Contract.EndContractBlock();

			return this.ClipCore( offset, length );
		}

		/// <summary>
		///		Clip out specified range from this buffer and exclude it from this buffer.
		/// </summary>
		/// <param name="offset">Offset to start clip out.</param>
		/// <param name="length">Length of clipping range.</param>
		/// <returns>
		///		<see cref="ChunkBuffer"/> which contains clipped range only.
		/// </returns>
		protected abstract ChunkBuffer ClipCore( long offset, long length );

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
