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
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
#endif // DEBUG && !UNITY
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	internal sealed partial class ItemsUnpacker : Unpacker
	{
		private readonly bool _ownsStream;
		private readonly bool _useStreamPosition;
		private readonly Stream _source;

		private readonly byte[] _oneByteBuffer = new byte[ 1 ];
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

		internal override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._lastOffset;
			return this._useStreamPosition;
		}

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance.
		/// </summary>
		private long _offset;

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance before last operation.
		/// </summary>
		private long _lastOffset;

		public ItemsUnpacker( Stream stream, PackerUnpackerStreamOptions streamOptions )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			var options = streamOptions ?? PackerUnpackerStreamOptions.None;
			this._source = options.WrapStream( stream );
			this._ownsStream = options.OwnsStream;
			this._useStreamPosition = stream.CanSeek;
			this._offset = this._useStreamPosition ? stream.Position : 0L;
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
			var success = this.ReadSubtreeObject( /* isDeep */false, out value );
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

#if FEATURE_TAP

		protected override async Task<bool> ReadAsyncCore( CancellationToken cancellationToken )
		{
			var result = await this.ReadSubtreeObjectAsync( /* isDeep */false, cancellationToken ).ConfigureAwait( false );
			if ( result.Success )
			{
				this.InternalData = result.Value;
				return true;
			}
			else
			{
				return false;
			}
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP
		
		internal Task<bool> ReadSubtreeItemAsync( CancellationToken cancellationToken )
		{
			return this.ReadAsyncCore( cancellationToken );
		}

#endif // FEATURE_TAP

		internal long? SkipSubtreeItem()
		{
			return this.SkipCore();
		}

#if FEATURE_TAP

		internal Task<long?> SkipSubtreeItemAsync( CancellationToken cancellationToken )
		{
			return this.SkipAsyncCore( cancellationToken );
		}

#endif // FEATURE_TAP

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

			this._lastOffset = this._offset;
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
				this.ThrowEofException( size );
			}
		}

#if FEATURE_TAP

		private async Task ReadStrictAsync( byte[] buffer, int size, CancellationToken cancellationToken )
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

			this._lastOffset = this._offset;
			var remaining = size;
			var offset = 0;
			int read;

			do
			{
				read = await this._source.ReadAsync( buffer, offset, remaining, cancellationToken ).ConfigureAwait( false );
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
				this.ThrowEofException( size );
			}
		}

#endif // FEATURE_TAP

		private int ReadByteFromSource()
		{
			this._lastOffset = this._offset;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			var read = this._source.Read( this._oneByteBuffer, 0, 1 );
			if ( read > 0 )
			{
				this._offset++;
			}

#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			return read == 0 ? -1 : this._oneByteBuffer[ 0 ];
		}

		private byte ReadByteStrict()
		{
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			this._lastOffset = this._offset;
			var read = this._source.Read( this._oneByteBuffer, 0, 1 );
			if ( read == 0 )
			{
				this.ThrowEofException( 1 );
			}

			this._offset++;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			return this._oneByteBuffer[ 0 ];
		}

#if FEATURE_TAP

		private async Task<int> ReadByteFromSourceAsync( CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			var read = await this._source.ReadAsync( this._oneByteBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
			if ( read > 0 )
			{
				this._offset++;
			}

#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			return read == 0 ? -1 : this._oneByteBuffer[ 0 ];
		}

		private async Task<byte> ReadByteStrictAsync( CancellationToken cancellationToken )
		{
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			this._lastOffset = this._offset;
			var read = await this._source.ReadAsync( this._oneByteBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
			if ( read == 0 )
			{
				this.ThrowEofException( 1 );
			}

			this._offset++;
#if DEBUG && !UNITY
			if ( this._source.CanSeek )
			{
				Contract.Assert( this._source.Position == this._offset, this._source.Position + "==" + this._offset );
			}
#endif // DEBUG && !UNITY
			return this._oneByteBuffer[ 0 ];
		}

#endif // FEATURE_TAP

		internal override void ThrowEofException()
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read object from stream. Current position is {0:#,0}."
							: "Stream unexpectedly ends. Cannot read object from stream. Current offset is {0:#,0}.",
						offsetOrPosition
					)
				);
		}

		private void ThrowEofException( long reading )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at position {1:#,0}."
							: "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at offset {1:#,0}.",
						reading,
						offsetOrPosition
					)
				);
		}

		private void ThrowUnassignedMessageTypeException( int header )
		{
#if DEBUG && !UNITY
			Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
#endif // DEBUG && !UNITY
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new UnassignedMessageTypeException(
				String.Format( 
					CultureInfo.CurrentCulture,
					isRealOffset
						? "Unknown header value 0x{0:X} at position {1:#,0}"
						: "Unknown header value 0x{0:X} at offset {1:#,0}",
					header,
					offsetOrPosition
				)
			);
		}

		private void ThrowUnexpectedExtCodeException( ReadValueResult type )
		{
#if DEBUG && !UNITY
			Contract.Assert( false, "Unexpected ext-code type:" + type );
#endif // DEBUG && !UNITY
			// ReSharper disable HeuristicUnreachableCode
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );

			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
						? "Unexpeded ext-code type {0} at position {1:#,0}"
						: "Unexpeded ext-code type {0} at offset {1:#,0}",
					type,
					offsetOrPosition
				)
			);
			// ReSharper restore HeuristicUnreachableCode
		}

		private void CheckLength( long length, ReadValueResult type )
		{
			if ( length > Int32.MaxValue )
			{
				this.ThrowTooLongLengthException( length, type );
			}
		}

		private void ThrowTooLongLengthException( long length, ReadValueResult type )
		{
			string message;
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );

			switch ( type )
			{
				case ReadValueResult.ArrayLength:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at offset {1:#,0}";
					break;
				}
				case ReadValueResult.MapLength:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at offset {1:#,0}";
					break;
				}
				default:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at offset {1:#,0}";
					break;
				}
			}

			throw new MessageNotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					message,
					length,
					offsetOrPosition
				)
			);
		}

		private void ThrowTypeException( Type type, byte header )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new MessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
					? "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in position {3:#,0}."
					: "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in offset {3:#,0}.",
					type,
					header,
					MessagePackCode.ToString( header ),
					offsetOrPosition
				)
			);
		}

		private enum ReadValueResult
		{
			Eof = 0,
			Nil,
			Boolean,
			SByte,
			Byte,
			Int16,
			UInt16,
			Int32,
			UInt32,
			Int64,
			UInt64,
			Single,
			Double,
			ArrayLength,
			MapLength,
			String,
			Binary,
			FixExt1,
			FixExt2,
			FixExt4,
			FixExt8,
			FixExt16,
			Ext8,
			Ext16,
			Ext32,
		}

#if FEATURE_TAP

		private struct AsyncReadValueResult
		{
			public ReadValueResult type;
			public byte header;
			public long integral;
			public float real32;
			public double real64;
		}

#endif // FEATURE_TAP

		internal enum CollectionType
		{
			// Value must be items count of collection element.
			None = 0,
			Array = 1,
			Map = 2
		}
	}
}
