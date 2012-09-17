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
using System.Linq;

namespace MsgPack
{
	partial class UnpackingStreamingUnpacker : StreamingUnpacker2
	{
		private static readonly MessagePackObject[] _positiveIntegers =
			Enumerable.Range( 0, 0x80 ).Select( i => new MessagePackObject( ( byte )i ) ).ToArray();
		private static readonly MessagePackObject[] _negativeIntegers =
			Enumerable.Range( -32, 32 ).Select( i => new MessagePackObject( ( sbyte )i ) ).ToArray();
		private static readonly MessagePackObject _emptyRaw = new MessagePackObject( new byte[ 0 ] );
		private static readonly MessagePackObject _true = new MessagePackObject( true );
		private static readonly MessagePackObject _false = new MessagePackObject( false );

		#region -- Collection States --

		private CollectionHeaderKind _collectionHeaderKind;

		private enum CollectionHeaderKind
		{
			NotCollection = 0,
			Array,
			Map
		}

		public bool IsInArrayHeader
		{
			get { return this._collectionHeaderKind == CollectionHeaderKind.Array; }
		}

		public bool IsInMapHeader
		{
			get { return this._collectionHeaderKind == CollectionHeaderKind.Map; }
		}

		private uint? _collectionItemsCount;

		public uint UnpackingItemsCount
		{
			get
			{
				return this._collectionItemsCount ?? ( this._currentCollectionState == null ? 0 : this._currentCollectionState.UnpackingItemsCount );
			}
		}

		#endregion -- Collection States --


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
	}
}
