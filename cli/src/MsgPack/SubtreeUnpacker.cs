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
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Diagnostics;

namespace MsgPack
{
	internal sealed class SubtreeUnpacker : Unpacker
	{
		private readonly StreamUnpacker _root;
		private readonly SubtreeUnpacker _parent;
		private readonly bool _isMap;
		private long _unpacked;
		private readonly long _itemsCount;

		public sealed override long ItemsCount
		{
			get { return this._itemsCount; }
		}

		public sealed override bool IsArrayHeader
		{
			get { return this._root.IsArrayHeader; }
		}

		public sealed override bool IsMapHeader
		{
			get { return this._root.IsMapHeader; }
		}

		public sealed override bool IsInStart
		{
			get { return false; }
		}

		public sealed override MessagePackObject? Data
		{
			get { return this._root.Data; }
		}

		public SubtreeUnpacker( StreamUnpacker parent ) : this( parent, null ) { }

		private SubtreeUnpacker( StreamUnpacker root, SubtreeUnpacker parent )
		{
			Contract.Assert( root != null );
			Contract.Assert( root.IsArrayHeader || root.IsMapHeader );
			this._root = root;
			this._parent = parent;
			this._itemsCount = root.ItemsCount;
			this._isMap = root.IsMapHeader;
		}

		protected sealed override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				// Drain...
				while ( this.ReadCore() )
				{
					// nop
				}
				if ( this._parent != null )
				{
					this._parent.EndReadSubtree();
				}
				else
				{
					this._root.EndReadSubtree();
				}
			}
		}

		protected sealed override Unpacker ReadSubtreeCore()
		{
			if ( this._unpacked == 0 )
			{
				return this;
			}

			return new SubtreeUnpacker( this._root, this );
		}

		protected sealed override bool ReadCore()
		{
			if ( this._unpacked == this._itemsCount * ( this._isMap ? 2 : 1 ) )
			{
				return false;
			}

			if ( !this._root.ReadSubtreeItem() )
			{
				return false;
			}

			this._unpacked++;
			return true;
		}

		protected sealed override void FeedCore( Stream stream, bool ownsStream )
		{
			throw new NotSupportedException();
		}
	}
}
