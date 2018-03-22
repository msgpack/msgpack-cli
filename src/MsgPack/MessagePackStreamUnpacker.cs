#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017-2018 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.IO;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Implements common features for stream based MessagePack unpacker.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	abstract partial class MessagePackStreamUnpacker : Unpacker, IRootUnpacker
	{
		private readonly byte[] _oneByteBuffer = new byte[ 1 ];
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private readonly Stream _source;
		private readonly bool _useStreamPosition;
		private readonly bool _ownsStream;
		private CollectionType _collectionType;
		private MessagePackObject _data;
		private int _subtreeCount;

#pragma warning disable CS0672
		public sealed override MessagePackObject? Data
		{
			get { return this._data; }
			protected set { this._data = value.GetValueOrDefault(); }
		}
#pragma warning restore CS0672

		public sealed override MessagePackObject LastReadData
		{
			get { return this._data; }
			protected set { this._data = value; }
		}

		public sealed override bool IsArrayHeader
		{
			get { return this._collectionType == CollectionType.Array; }
		}

		public sealed override bool IsMapHeader
		{
			get { return this._collectionType == CollectionType.Map; }
		}

		public sealed override long ItemsCount
		{
			get { return this._collectionType == CollectionType.None ? 0 : this._data.AsInt64(); }
		}

		public sealed override bool IsCollectionHeader
		{
			get { return this._collectionType != CollectionType.None; }
		}

		CollectionType IRootUnpacker.CollectionType
		{
			get { return this._collectionType; }
		}

		MessagePackObject? IRootUnpacker.Data
		{
#pragma warning disable CS0618
			get { return this.Data; }
			set { this.Data = value; }
#pragma warning restore CS0618
		}

		MessagePackObject IRootUnpacker.LastReadData
		{
			get { return this._data; }
			set { this._data = value; }
		}

#if DEBUG
#if UNITY && DEBUG
		public
#else
		internal
#endif
		Stream DebugSource
		{
			get { return this._source; }
		}

#if UNITY && DEBUG
		public
#else
		internal
#endif
		bool DebugOwnsStream
		{
			get { return this._ownsStream; }
		}

#if UNITY && DEBUG
		public
#else
		internal
#endif
		long DebugOffset
		{
			get { return this._offset; }
		}

		internal sealed override long? UnderlyingStreamPosition
		{
			get { return this._offset; }
		}

		long? IRootUnpacker.UnderlyingStreamPosition
		{
			get { return this.UnderlyingStreamPosition; }
		}
#endif // DEBUG

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance.
		/// </summary>
		private long _offset;

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance before last operation.
		/// </summary>
		private long _lastOffset;

		internal override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._lastOffset;
			return this._useStreamPosition;
		}

		public MessagePackStreamUnpacker( Stream stream, PackerUnpackerStreamOptions streamOptions )
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
				if ( this._subtreeCount == 0 && this._ownsStream )
				{
					this._source.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		protected void BeginReadSubtree()
		{
			this._subtreeCount++;
		}

		protected internal override void EndReadSubtree()
		{
			base.EndReadSubtree();
			this._subtreeCount--;
		}

		private bool ReadBinaryCore( int length, ref long offset, out byte[] result )
		{
			this._lastOffset = this._offset;

			if ( length == 0 )
			{
				result = Binary.Empty;
				return true;
			}

			result = new byte[ length ];
			var bufferOffset = 0;
			var reading = length;
			// Retrying for splitted Stream such as NetworkStream
			while ( true )
			{
				var readLength = this._source.Read( result, bufferOffset, reading );
				if ( readLength < reading )
				{
					if ( readLength > 0 )
					{
						// retry reading
						bufferOffset += readLength;
						reading -= readLength;
						continue;
					}
					else
					{
						if ( this._useStreamPosition )
						{
							// Rollback
							this._source.Position -= ( bufferOffset + readLength );
						}
						else
						{
							// Throw because rollback is not available
							this.ThrowEofException( reading );
						}
					}

					result = default( byte[] );
					return false;
				}

				break;
			}

			offset += length;
			return true;
		}

#if FEATURE_TAP

		private async Task<AsyncReadResult<Int64OffsetValue<byte[]>>> ReadBinaryCoreAsync( int length, long offset, CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;

			if ( length == 0 )
			{
				return AsyncReadResult.Success( Binary.Empty, offset );
			}

			var result = new byte[ length ];
			var bufferOffset = 0;
			var reading = length;
			// Retrying for splitted Stream such as NetworkStream
			while ( true )
			{
				var readLength = await this._source.ReadAsync( result, bufferOffset, reading, cancellationToken ).ConfigureAwait( false );
				if ( readLength < reading )
				{
					if ( readLength > 0 )
					{
						// retry reading
						bufferOffset += readLength;
						reading -= readLength;
						continue;
					}
					else
					{
						if ( this._useStreamPosition )
						{
							// Rollback
							this._source.Position -= ( bufferOffset + readLength );
						}
						else
						{
							// Throw because rollback is not available
							this.ThrowEofException( reading );
						}
					}

					return AsyncReadResult.Fail<Int64OffsetValue<byte[]>>();
				}

				break;
			}

			offset += length;
			return AsyncReadResult.Success( result, offset );
		}

#endif // FEATURE_TAP

		private bool ReadStringCore( int length, ref long offset, out string result )
		{
			this._lastOffset = this._offset;

			if ( length == 0 )
			{
				result = String.Empty;
				return true;
			}

			// TODO: Span<byte>
			var byteBuffer = BufferManager.NewByteBuffer( length * 4 );
			var charBuffer = BufferManager.NewCharBuffer( length );
			var resultBuffer = new StringBuilder( length );
			var decoder = MessagePackConvert.Utf8NonBomStrict.GetDecoder();
			var remaining = length;
#if DEBUG
			bool isCompleted;
#endif // DEBUG
			// Retrying for splitted Stream such as NetworkStream
			do
			{
				var reading = Math.Min( byteBuffer.Length, remaining );
				var readLength = this._source.Read( byteBuffer, 0, reading );
				if ( readLength == 0 )
				{
					if ( this._useStreamPosition )
					{
						// Rollback
						this._source.Position -= resultBuffer.Length;
					}
					else
					{
						// Throw because rollback is not available
						this.ThrowEofException( reading );
					}

					result = default( string );
					return false;
				}

#if DEBUG
				isCompleted =
#endif // DEBUG
					decoder.DecodeString( byteBuffer, 0, readLength, charBuffer, resultBuffer );

				remaining -= readLength;
			} while ( remaining > 0 );

#if DEBUG
			Contract.Assert( isCompleted, "isCompleted == true" );
#endif // DEBUG
			result = resultBuffer.ToString();
			offset += length;
			return true;
		}

#if FEATURE_TAP

		private async Task<AsyncReadResult<Int64OffsetValue<string>>> ReadStringCoreAsync( int length, long offset, CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;

			if ( length == 0 )
			{
				return AsyncReadResult.Success( String.Empty, offset );
			}

			// TODO: Span<byte>
			var byteBuffer = BufferManager.NewByteBuffer( length * 4 );
			var charBuffer = BufferManager.NewCharBuffer( length );
			var resultBuffer = new StringBuilder( length );
			var decoder = MessagePackConvert.Utf8NonBomStrict.GetDecoder();
			var remaining = length;
#if DEBUG
			bool isCompleted;
#endif // DEBUG
			// Retrying for splitted Stream such as NetworkStream
			do
			{
				var reading = Math.Min( byteBuffer.Length, remaining );
				var readLength = await this._source.ReadAsync( byteBuffer, 0, reading, cancellationToken ).ConfigureAwait( false );
				if ( readLength == 0 )
				{
					if ( this._useStreamPosition )
					{
						// Rollback
						this._source.Position -= resultBuffer.Length;
					}
					else
					{
						// Throw because rollback is not available
						this.ThrowEofException( reading );
					}

					return AsyncReadResult.Fail<Int64OffsetValue<string>>();
				}

#if DEBUG
				isCompleted =
#endif // DEBUG
					decoder.DecodeString( byteBuffer, 0, readLength, charBuffer, resultBuffer );

				remaining -= readLength;
			} while ( remaining > 0 );

#if DEBUG
			Contract.Assert( isCompleted, "isCompleted == true" );
#endif // DEBUG
			offset += length;
			return AsyncReadResult.Success( resultBuffer.ToString(), offset );
		}

#endif // FEATURE_TAP

		private bool ReadRawStringCore( int length, ref long offset, out MessagePackString result )
		{
			byte[] asBinary;
			if ( !this.ReadBinaryCore( length, ref offset, out asBinary ) )
			{
				result = default( MessagePackString );
				return false;
			}

			try
			{
				result = new MessagePackString( MessagePackConvert.Utf8NonBomStrict.GetString( asBinary, 0, asBinary.Length ) );
			}
			catch ( DecoderFallbackException )
			{
				result = new MessagePackString( asBinary, true );
			}

			return true;
		}

#if FEATURE_TAP

		private async Task<AsyncReadResult<Int64OffsetValue<MessagePackString>>> ReadRawStringCoreAsync( int length, long offset, CancellationToken cancellationToken )
		{
			var asyncReadResult = await this.ReadBinaryCoreAsync( length, offset, cancellationToken ).ConfigureAwait( false );
			if ( !asyncReadResult.Success )
			{
				return AsyncReadResult.Fail<Int64OffsetValue<MessagePackString>>();
			}

			var asBinary = asyncReadResult.Value.Result;
			MessagePackString result;
			try
			{
				result = new MessagePackString( MessagePackConvert.Utf8NonBomStrict.GetString( asBinary, 0, asBinary.Length ) );
			}
			catch ( DecoderFallbackException )
			{
				result = new MessagePackString( asBinary, true );
			}

			return AsyncReadResult.Success( result, asyncReadResult.Value.Offset );
		}

#endif // FEATURE_TAP

		private bool Drain( uint size )
		{
			this._lastOffset = this._offset;

			if ( this._useStreamPosition )
			{
				var drained = this._source.Seek( size, SeekOrigin.Current );
				if ( drained < size )
				{
					// Rollback
					this._source.Position -= drained;
					return false;
				}

				this._offset += size;
				return true;
			}
			else
			{
				// Actually, buffer size should be smaller than 2GB.
				var buffer = BufferManager.NewByteBuffer( unchecked(( int )Math.Max( Int32.MaxValue, size )) );
				var totalSkipped = 0L;
				while ( totalSkipped < size )
				{
					var skipping = unchecked(( int )Math.Min( buffer.Length, size - totalSkipped ));
					var skipped = this._source.Read( buffer, 0, skipping );
					totalSkipped += skipped;
					if ( skipped < skipping )
					{
						// Record
						this._offset += totalSkipped;
						return false;
					}
				}

				this._offset += totalSkipped;
				return true;
			}
		}

#if FEATURE_TAP

		private async Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			this._lastOffset = this._offset;

			if ( this._useStreamPosition )
			{
				var drained = this._source.Seek( size, SeekOrigin.Current );
				if ( drained < size )
				{
					// Rollback
					this._source.Position -= drained;
					return false;
				}

				this._offset += size;
				return true;
			}
			else
			{
				// Actually, buffer size should be smaller than 2GB.
				var buffer = BufferManager.NewByteBuffer( unchecked(( int )Math.Max( Int32.MaxValue, size )) );
				var totalSkipped = 0L;
				while ( totalSkipped < size )
				{
					var skipping = unchecked( ( int )Math.Min( buffer.Length, size - totalSkipped ) );
					var skipped = await this._source.ReadAsync( buffer, 0, skipping, cancellationToken ).ConfigureAwait( false );
					totalSkipped += skipped;
					if ( skipped < skipping )
					{
						// Record
						this._offset += totalSkipped;
						return false;
					}
				}

				this._offset += totalSkipped;
				return true;
			}
		}

#endif // FEATURE_TAP

		bool IRootUnpacker.ReadObject( bool isDeep, out MessagePackObject result )
		{
			return this.ReadObject( isDeep, out result );
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
	}
}
