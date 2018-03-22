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
using System.Globalization;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Implements common features for byte array based MessagePack unpacker.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	abstract partial class MessagePackByteArrayUnpacker : ByteArrayUnpacker, IRootUnpacker
	{
		private byte[] _source;
		private int _offset;
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private CollectionType _collectionType;
		private MessagePackObject _data;

		public sealed override int Offset
		{
			get { return this._offset; }
		}

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

		public sealed override bool IsCollectionHeader
		{
			get { return this._collectionType != CollectionType.None; }
		}

		public sealed override long ItemsCount
		{
			get { return this._collectionType == CollectionType.None ? 0 : this._data.AsInt64(); }
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
		byte[] DebugSource
		{
			get { return this._source; }
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

		internal override long? UnderlyingStreamPosition
		{
			get { return this._offset; }
		}

		long? IRootUnpacker.UnderlyingStreamPosition
		{
			get { return this.UnderlyingStreamPosition; }
		}

#endif // DEBUG

		internal override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._offset;
			return false;
		}

		public MessagePackByteArrayUnpacker( byte[] source, int startOffset )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( source.Length == 0 )
			{
				throw new ArgumentException( "The source is empty.", "source" );
			}

			if ( startOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startOffset" );
			}

			if ( startOffset >= source.Length )
			{
				throw new ArgumentException( "The startOffset is too large or the length of source is too small." );
			}

			this._source = source;
			this._offset = startOffset;
		}

#if FEATURE_MEMCOPY
		[System.Security.SecuritySafeCritical]
#endif // FEATURE_MEMCOPY
		private bool ReadBinaryCore( int length, ref int offset, out byte[] result )
		{
			if ( length == 0 )
			{
				result = Binary.Empty;
				return true;
			}

			var source = this._source;
			if ( source.Length - offset < length )
			{
				result = default( byte[] );
				return false;
			}

			result = new byte[ length ];
#if FEATURE_MEMCOPY
			unsafe
			{
				fixed ( byte* pSource = source )
				{
					fixed ( byte* pResult = result )
					{
						Buffer.MemoryCopy( pSource + offset, pResult, length, length );
					}
				}
			}
#else
			Buffer.BlockCopy( source, offset, result, 0, length );
#endif // FEATURE_MEMCOPY
			offset += length;
			return true;
		}

#if FEATURE_TAP

		private Task<AsyncReadResult<Int32OffsetValue<byte[]>>> ReadBinaryCoreAsync( int length, int offset, CancellationToken cancellationToken )
		{
			byte[] result;
			if ( !this.ReadBinaryCore( length, ref offset, out result ) )
			{
				return Task.FromResult( AsyncReadResult.Fail<Int32OffsetValue<byte[]>>() );
			}

			return Task.FromResult( AsyncReadResult.Success( result, offset ) );
		}

#endif // FEATURE_TAP

		private bool ReadStringCore( int length, ref int offset, out string result )
		{
			if ( length == 0 )
			{
				result = String.Empty;
				return true;
			}

			var source = this._source;
			if ( source.Length - offset < length )
			{
				result = default( string );
				return false;
			}

			result = MessagePackConvert.Utf8NonBomStrict.GetString( source, offset, length );
			offset += length;
			return true;
		}

#if FEATURE_TAP

		private Task<AsyncReadResult<Int32OffsetValue<string>>> ReadStringCoreAsync( int length, int offset, CancellationToken cancellationToken )
		{
			string result;
			if ( !this.ReadStringCore( length, ref offset, out result ) )
			{
				return Task.FromResult( AsyncReadResult.Fail<Int32OffsetValue<string>>() );
			}

			return Task.FromResult( AsyncReadResult.Success( result, offset ) );
		}

#endif // FEATURE_TAP

		private bool ReadRawStringCore( int length, ref int offset, out MessagePackString result )
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
			catch(DecoderFallbackException)
			{
				result = new MessagePackString( asBinary, true );
			}

			return true;
		}

#if FEATURE_TAP

		private Task<AsyncReadResult<Int32OffsetValue<MessagePackString>>> ReadRawStringCoreAsync( int length, int offset, CancellationToken cancellationToken )
		{
			MessagePackString result;
			if ( !this.ReadRawStringCore( length, ref offset, out result ) )
			{
				return Task.FromResult( AsyncReadResult.Fail<Int32OffsetValue<MessagePackString>>() );
			}

			return Task.FromResult( AsyncReadResult.Success( result, offset ) );
		}

#endif // FEATURE_TAP

		private bool Drain( uint size )
		{
			if ( this._source.Length - this._offset < size )
			{
				return false;
			}

			if ( this._offset + size > Int32.MaxValue )
			{
				return false;
			}

			this._offset += unchecked(( int )size);
			return true;
		}

#if FEATURE_TAP

		private Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			return Task.FromResult( this.Drain( size ) );
		}

#endif // FEATURE_TAP

		bool IRootUnpacker.ReadObject(bool isDeep, out MessagePackObject result)
		{
			return this.ReadObject( isDeep, out result );
		}
	}
}
