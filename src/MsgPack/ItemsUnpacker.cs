#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if DEBUG && !UNITY
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY
using System.Globalization;
using System.IO;

namespace MsgPack
{
	internal sealed partial class ItemsUnpacker : Unpacker
	{
		private readonly bool _ownsStream;
		private readonly Stream _source;

		private readonly byte[] _scalarBuffer = new byte[ 8 ];

		internal long InternalItemsCount;
		internal CollectionType InternalCollectionType;
		internal MessagePackObject InternalData;

		[Obsolete( "Consumer should not use this property. Query LastReadData instead." )]
		public override MessagePackObject? Data
		{
			get { return this.InternalData; }
			protected set { this.InternalData = value.GetValueOrDefault(); }
		}

		public override MessagePackObject LastReadData
		{
			get { return this.InternalData; }
			protected set { this.InternalData = value; }
		}

		public override bool IsArrayHeader
		{
			get { return this.InternalCollectionType == CollectionType.Array; }
		}

		public override bool IsMapHeader
		{
			get { return this.InternalCollectionType == CollectionType.Map; }
		}

		public override bool IsCollectionHeader
		{
			get { return this.InternalCollectionType != CollectionType.None; }
		}

		public override long ItemsCount
		{
			get { return this.InternalCollectionType != CollectionType.None ? this.InternalItemsCount : 0L; }
		}

		protected override Stream UnderlyingStream
		{
			get { return this._source; }
		}

#if DEBUG
		internal override long? UnderlyingStreamPosition
		{
			get { return this.UnderlyingStream.Position; }
		}
#endif

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance.
		/// </summary>
		private long _offset;

		public ItemsUnpacker( Stream stream, bool ownsStream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			this._source = stream;
			this._ownsStream = ownsStream;
			this._offset = stream.CanSeek ? stream.Position : 0L;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( this._ownsStream )
				{
					this._source.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		protected override bool ReadCore()
		{
			MessagePackObject value;
			var success = this.ReadSubtreeObject( false, out value );
			if ( success )
			{
				this.InternalData = value;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		///		Starts unpacking of current subtree.
		/// </summary>
		/// <returns>
		///		<see cref="Unpacker"/> to unpack current subtree.
		///		This will not be <c>null</c>.
		/// </returns>
		protected override Unpacker ReadSubtreeCore()
		{
			return new SubtreeUnpacker( this );
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

		internal long? SkipSubtreeItem()
		{
			return this.SkipCore();
		}

		private void ReadStrict( byte[] buffer, int size )
		{
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			// Reading 0 byte from stream causes exception in some implementation (issue #60, reported from @odyth).
			if ( size == 0 )
			{
				return;
			}

			var originalOffset = this._offset;
			var remaining = size;
			var offset = 0;
			int read;

			do
			{
				read = this._source.Read( buffer, offset, remaining );
				remaining -= read;
				offset += read;
			} while ( read > 0 && remaining > 0 );

			this._offset += offset;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY

			if ( offset < size )
			{
				this.ThrowEofException( size, originalOffset );
			}
		}

		private byte ReadByteStrict()
		{
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			var originalOffset = this._offset;
			var result = this._source.ReadByte();
			if ( result < 0 )
			{
				this.ThrowEofException( originalOffset, 1 );
			}

			this._offset++;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			return unchecked( ( byte )result );
		}

		internal override void ThrowEofException()
		{
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						this._source.CanSeek
						? "Stream unexpectedly ends. Cannot read object from stream. Current position is {0:#,0}."
						: "Stream unexpectedly ends. Cannot read object from stream. Current offset is {0:#,0}.",
						this._offset
					)
				);
		}

		private void ThrowEofException( long lastOffset, long reading )
		{
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						this._source.CanSeek
						? "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at position {1:#,0}."
						: "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at offset {1:#,0}.",
						reading,
						lastOffset
					)
				);
		}

		internal enum CollectionType
		{
			// Value must be items count of collection element.
			None = 0,
			Array = 1,
			Map = 2
		}
	}
}
