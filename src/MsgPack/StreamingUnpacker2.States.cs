using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MsgPack
{
	partial class StreamingUnpacker2
	{
		private static readonly MessagePackObject[] _positiveIntegers =
			Enumerable.Range( 0, 0x80 ).Select( i => new MessagePackObject( ( byte )i ) ).ToArray();
		private static readonly MessagePackObject[] _negativeIntegers =
			Enumerable.Range( -32, 32 ).Select( i => new MessagePackObject( ( sbyte )i ) ).ToArray();
		private static readonly MessagePackObject _emptyRaw = new MessagePackObject( new byte[ 0 ] );
		private static readonly MessagePackObject _emptyArray = new MessagePackObject( 0 );
		private static readonly MessagePackObject _emptyMap = new MessagePackObject( 0 );
		private static readonly MessagePackObject _true = new MessagePackObject( true );
		private static readonly MessagePackObject _false = new MessagePackObject( false );

		public bool IsInArrayHeader
		{
			get { return this._collectionHeaderKind == CollectionHeaderKind.Array; }
		}

		public bool IsInMapHeader
		{
			get { return this._collectionHeaderKind == CollectionHeaderKind.Map; }
		}

		public int UnpackingItemsCount
		{
			get { return this._currentCollectionState == null ? 0 : this._currentCollectionState.UnpackingItemsCount; }
		}

		#region -- Length --

		private long _readByteLength;

		#endregion

		#region -- Scalar Buffer --

		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private int _scalarBufferPosition = -1;
		private ScalarKind _scalarKind;

		private static readonly ScalarKind[] _scalarKinds =
			new[]
			{
				ScalarKind.Nil,
				ScalarKind.Reserved,
				ScalarKind.True,
				ScalarKind.False,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Float32,
				ScalarKind.Float64,
				ScalarKind.UInt8,
				ScalarKind.UInt16,
				ScalarKind.UInt32,
				ScalarKind.UInt64,
				ScalarKind.Int8,
				ScalarKind.Int16,
				ScalarKind.Int32,
				ScalarKind.Int64,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.Reserved,
				ScalarKind.RawLength16,
				ScalarKind.RawLength32,
				ScalarKind.ArrayLength16,
				ScalarKind.ArrayLength32,
				ScalarKind.MapLength16,
				ScalarKind.MapLength32
			};

		private enum ScalarKind
		{
			Reserved = 0,
			Nil = 0x1000,
			True = 0x2000,
			False = 0x3000,
			RawLength16 = 0x012,
			RawLength32 = 0x014,
			ArrayLength16 = 0x112,
			ArrayLength32 = 0x114,
			MapLength16 = 0x212,
			MapLength32 = 0x214,
			Int8 = 0x301,
			Int16 = 0x302,
			Int32 = 0x304,
			Int64 = 0x308,
			UInt8 = 0x311,
			UInt16 = 0x312,
			UInt32 = 0x314,
			UInt64 = 0x318,
			Float32 = 0x404,
			Float64 = 0x408
		}

		#endregion

		#region -- Blob Buffer --

		private byte[] _blobBuffer;
		private int _blobBufferPosition;

		#endregion

		#region -- Collection State --

		private CollectionHeaderKind _collectionHeaderKind;

		private enum CollectionHeaderKind
		{
			NotCollection = 0,
			Array,
			Map
		}

		private CollectionUnpackingState _currentCollectionState;
		private readonly Stack<CollectionUnpackingState> _collectionStates = new Stack<CollectionUnpackingState>( 4 );

		private sealed class CollectionUnpackingState
		{
			private uint _itemsCount;

			public uint ItemsCount
			{
				get { return this._itemsCount; }
			}

			public void SetItemsCount( int value )
			{
				unchecked
				{
					this._itemsCount = this._isMap ? ( uint )value * 2 : ( uint )value;
				}
			}

			public int UnpackingItemsCount
			{
				get { return unchecked( ( int )( this._isMap ? this._itemsCount / 2 : this._itemsCount ) ); }
			}

			private uint _unpacked;

			public bool IncrementUnpacked()
			{
				return ( ++this._unpacked ) == this.ItemsCount;
			}

			private readonly bool _isMap;

			private CollectionUnpackingState( int itemsCount, bool isMap )
			{
				this._isMap = isMap;
				this.SetItemsCount( itemsCount );
			}

			public static CollectionUnpackingState Array()
			{
				return new CollectionUnpackingState( -1, false );
			}

			public static CollectionUnpackingState FixedArray( int count )
			{
				return new CollectionUnpackingState( count, false );
			}

			public static CollectionUnpackingState Map()
			{
				return new CollectionUnpackingState( -1, true );
			}

			public static CollectionUnpackingState FixedMap( int count )
			{
				return new CollectionUnpackingState( count, true );
			}
		}

		#endregion
	}

	internal class SkippingUnpacker
	{
		private long _skipped;
		private uint _remaining;
		private int _depth;

		public long? Skip( System.IO.Stream source )
		{
			for ( ; this._remaining > 0; this._remaining-- )
			{
				var read = source.ReadByte();
				if ( read < 0 )
				{
					return null;
				}
			}

			for( var b = source.ReadByte(); b >= 0; b = source.ReadByte())
			{
				
			}
		}
	}
}
