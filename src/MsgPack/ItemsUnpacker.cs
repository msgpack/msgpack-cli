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
using System.Text;

namespace MsgPack
{
	internal sealed partial class ItemsUnpacker : Unpacker
	{
		private readonly bool _ownsStream;
		private readonly Stream _stream;
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private long _itemsCount;
		private CollectionType _collectionType;
		private MessagePackObject? _data;

		public override MessagePackObject? Data
		{
			get { return this._data; }
			protected set { this._data = value; }
		}

		internal void InternalSetData( MessagePackObject? value )
		{
			this._data = value;
		}

		public override bool IsArrayHeader
		{
			get { return this._collectionType == CollectionType.Array; }
		}

		public override bool IsMapHeader
		{
			get { return this._collectionType == CollectionType.Map; }
		}

		public override long ItemsCount
		{
			get { return this._itemsCount; }
		}

		protected sealed override Stream UnderlyingStream
		{
			get { return this._stream; }
		}

#if DEBUG
		internal override long? UnderlyingStreamPosition
		{
			get { return this._stream.Position; }
		}
#endif

		public ItemsUnpacker( Stream stream, bool ownsStream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			this._stream = stream;
			this._ownsStream = ownsStream;
		}

		protected sealed override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( this._ownsStream )
				{
					this._stream.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		protected override bool ReadCore()
		{
			MessagePackObject value;
			var success = this.ReadSubtreeObject( out value );
			if ( success )
			{
				this._data = value;
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
		protected sealed override Unpacker ReadSubtreeCore()
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

		private enum CollectionType
		{
			None,
			Array,
			Map
		}
	}
}
