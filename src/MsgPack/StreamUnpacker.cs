#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.IO;
using System.Linq;

namespace MsgPack
{
	/// <summary>
	///		Stream based unpacker.
	/// </summary>
	internal sealed class StreamUnpacker : Unpacker
	{
		/// <summary>
		///		Default buffer size.
		/// </summary>
		/// <remarks>
		///		This value is subject to change.
		/// </remarks>
		public static readonly int DefaultBufferSize = 1024 * 64;

		/// <summary>
		///		Actual unpackaging strategy.
		/// </summary>
		private readonly StreamingUnpacker _unpacker = new StreamingUnpacker();

		/// <summary>
		///		If current position MAY be in tail of source then true, otherwise false.
		/// </summary>
		/// <remarks>
		///		This value should be refered via <see cref="IsInStreamTail"/>.
		/// </remarks>
		private bool _mayInTail;

		/// <summary>
		///		Queue of successors of data source.
		/// </summary>
		private readonly Queue<DataSource> _successorSources = new Queue<DataSource>();

		/// <summary>
		///		Current data source.
		/// </summary>
		private DataSource _currentSource;

		/// <summary>
		///		Last unpacked data or null.
		/// </summary>
		private MessagePackObject? _data;

		/// <summary>
		///		Get last unpacked data.
		/// </summary>
		/// <value>
		///		Last unpacked data or null.
		/// </value>
		public sealed override MessagePackObject? Data
		{
			get { return this._data; }
		}

		/// <summary>
		///		Gets a value indicating whether this instance is positioned to array header.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is positioned to array header; otherwise, <c>false</c>.
		/// </value>
		public sealed override bool IsArrayHeader
		{
			get { return this._unpacker.IsInArrayHeader; }
		}

		/// <summary>
		///		Gets a value indicating whether this instance is positioned to map header.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is positioned to map header; otherwise, <c>false</c>.
		/// </value>
		public sealed override bool IsMapHeader
		{
			get { return this._unpacker.IsInMapHeader; }
		}

		/// <summary>
		///		Gets the items count for current array or map.
		/// </summary>
		/// <value>
		///		The items count for current array or map.
		/// </value>
		/// <exception cref="InvalidOperationException">
		///		Both of the <see cref="IsArrayHeader"/> and <see cref="IsMapHeader"/> are <c>false</c>.
		/// </exception>
		public sealed override long ItemsCount
		{
			get
			{
				if ( !this.IsArrayHeader && !this.IsMapHeader )
				{
					throw new InvalidOperationException( "This instance is not positioned to Array nor Map header." );
				}

				return this._unpacker.UnpackingItemsCount;
			}
		}
		
		/// <summary>
		///		Gets the underlying stream to handle direct API.
		/// </summary>
		protected sealed override Stream UnderlyingStream
		{
			get { return this._currentSource.Stream; }
		}

#if DEBUG
		internal override long? UnderlyingStreamPosition
		{
			get { return this.UnderlyingStream.Position; }
		}
#endif

		/// <summary>
		///		Initializes a new instance with default sized on memory buffer.
		/// </summary>
		public StreamUnpacker() : this( new MemoryStream( DefaultBufferSize ), true ) { }

		/// <summary>
		///		Initializes a new instance using specified <see cref="Stream"/> as source.
		/// </summary>
		/// <param name="source">Source <see cref="Stream"/>.</param>
		/// <param name="ownsStream">If you want to dispose stream when this instance is disposed, then true.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public StreamUnpacker( Stream source, bool ownsStream )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			this._currentSource = new DataSource( source, ownsStream );
		}

		/// <summary>
		///		Clean up internal resources.
		/// </summary>
		protected sealed override void Dispose( bool disposing )
		{
			var source = this._currentSource;
			if ( source.Stream != null && source.OwnsStream )
			{
				source.Stream.Dispose();
				this._currentSource = default( DataSource );
			}

			foreach ( var successor in this._successorSources.ToArray() )
			{
				if ( successor.Stream != null && successor.OwnsStream )
				{
					successor.Stream.Dispose();
				}
			}

			this._successorSources.Clear();

			base.Dispose( disposing );
		}

		/// <summary>
		///		Starts unpacking of current subtree.
		/// </summary>
		/// <returns>
		///		<see cref="Unpacker"/> to unpack current subtree.
		///		This will not be <c>null</c>.
		/// </returns>
		protected sealed override Unpacker ReadSubtreeCore()
		{
			return new SubtreeUnpacker( this );
		}

		/// <summary>
		///		Reads next Message Pack entry.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if position is sucessfully move to next entry;
		///		<c>false</c>, if position reaches the tail of the Message Pack stream.
		/// </returns>
		protected sealed override bool ReadCore()
		{
			return this.Read( this._unpacker, UnpackingMode.PerEntry );
		}

		/// <summary>
		///		Read subtree item from current stream.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if position is sucessfully move to next entry;
		///		<c>false</c>, if position reaches the tail of the Message Pack stream.
		/// </returns>
		/// <remarks>
		///		This method only be called from <see cref="SubtreeUnpacker"/>.
		/// </remarks>
		internal bool ReadSubtreeItem()
		{
			return this.ReadCore();
		}

		private StreamingUnpacker _skipper;

		protected sealed override long? SkipCore()
		{
			if ( this._skipper == null )
			{
				this._skipper = new StreamingUnpacker();
			}

			if ( this.Read( this._skipper, UnpackingMode.SkipSubtree ) )
			{
				this._skipper = null;
				return this._data.Value.AsInt64();
			}
			else
			{
				return null;
			}
		}

		internal long? SkipSubtreeItem()
		{
			return this.SkipCore();
		}

		private bool Read( StreamingUnpacker unpacker, UnpackingMode unpackingMode )
		{
			while ( !this.IsInStreamTail() )
			{
				var data = unpacker.Unpack( this._currentSource.Stream, unpackingMode );
				if ( data != null )
				{
					this._data = data;
					return true;
				}
				else
				{
					this._mayInTail = true;
				}
			}

			return false;
		}

		// FIXME: Quota & Depth checking?

		/// <summary>
		///		Determins this instance is in tail of all data sources.
		///		This method deque successors when needed.
		/// </summary>
		/// <returns>If this instance is in tail of all data sources then true, otherwise false.</returns>
		private bool IsInStreamTail()
		{
			if ( !this._mayInTail )
			{
				return false;
			}

			if ( this._currentSource.Stream.CanSeek && this._currentSource.Stream.Position < this._currentSource.Stream.Length )
			{
				return false;
			}

			if ( this._successorSources.Count == 0 )
			{
				return true;
			}

			this._currentSource = this._successorSources.Dequeue();
			return false;
		}

		/// <summary>
		///		Feeds new data source.
		/// </summary>
		/// <param name="stream">New data source to feed. This will not be <c>null</c>.</param>
		/// <param name="ownsStream">If <paramref name="stream"/> should be disposed in this instance then true.</param>
		protected sealed override void FeedCore( Stream stream, bool ownsStream )
		{
			this._successorSources.Enqueue( new DataSource( stream, ownsStream ) );
		}

		/// <summary>
		///		Encapselates Stream and ownership information.
		/// </summary>
		private struct DataSource
		{
			/// <summary>
			///		Indicates whether this unpacker should <see cref="IDisposable.Dispose"/> <see cref="Stream"/>.
			/// </summary>
			public readonly bool OwnsStream;

			/// <summary>
			///		Underlying stream of this source. This value could be null.
			/// </summary>
			public readonly Stream Stream;

			public DataSource( Stream stream, bool ownsStream )
			{
				this.Stream = stream;
				this.OwnsStream = ownsStream;
			}
		}
	}
}
