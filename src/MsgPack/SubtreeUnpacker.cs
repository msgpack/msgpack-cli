#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

#if !UNITY || MSGPACK_UNITY_FULL
using BooleanStack = System.Collections.Generic.Stack<System.Boolean>;
using Int64Stack = System.Collections.Generic.Stack<System.Int64>;
#endif // !UNITY || MSGPACK_UNITY_FULL


namespace MsgPack
{
	// TODO: Expose base subtree unpacker as API
	/// <summary>
	///		Defines subtree unpacking unpacker.
	/// </summary>
	internal sealed partial class SubtreeUnpacker : Unpacker
	{
		private readonly ItemsUnpacker _root;
		private readonly SubtreeUnpacker _parent;
		private readonly BooleanStack _isMap;
		private readonly Int64Stack _unpacked;
		private readonly Int64Stack _itemsCount;
		private State _state;

		public override long ItemsCount
		{
			get { return this._itemsCount.Count == 0 ? 0 : this._itemsCount.Peek() / ( this._isMap.Peek() ? 2 : 1 ); }
		}

		public override bool IsArrayHeader
		{
			get { return this._root.InternalCollectionType == ItemsUnpacker.CollectionType.Array; }
		}

		public override bool IsMapHeader
		{
			get { return this._root.InternalCollectionType == ItemsUnpacker.CollectionType.Map; }
		}

		public override bool IsCollectionHeader
		{
			get { return this._root.InternalCollectionType != ItemsUnpacker.CollectionType.None; }
		}

		[Obsolete( "Consumer should not use this property. Query LastReadData instead." )]
		public override MessagePackObject? Data
		{
			get { return this._root.InternalData; }
			protected set { this._root.InternalData = value.GetValueOrDefault(); }
		}

		public override MessagePackObject LastReadData
		{
			get { return this._root.InternalData; }
			protected set { this._root.InternalData = value; }
		}

#if DEBUG
		internal override long? UnderlyingStreamPosition
		{
			get { return this._root.UnderlyingStreamPosition; }
		}
#endif

		public SubtreeUnpacker( ItemsUnpacker parent ) : this( parent, null ) { }

		private SubtreeUnpacker( ItemsUnpacker root, SubtreeUnpacker parent )
		{
#if DEBUG && !UNITY
			Contract.Assert( root != null, "root != null" );
			Contract.Assert( root.IsArrayHeader || root.IsMapHeader, "root.IsArrayHeader || root.IsMapHeader" );
#endif // DEBUG && !UNITY
			this._root = root;
			this._parent = parent;
			this._unpacked = new Int64Stack( 2 );

			this._itemsCount = new Int64Stack( 2 );
			this._isMap = new BooleanStack( 2 );

			if ( root.ItemsCount > 0 )
			{
				this._itemsCount.Push( root.InternalItemsCount * ( ( int )root.InternalCollectionType ) );
				this._unpacked.Push( 0 );
				this._isMap.Push( root.InternalCollectionType == ItemsUnpacker.CollectionType.Map );
			}

			this._state = State.InHead;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( this._state != State.Disposed )
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

					this._state = State.Disposed;
				}
			}

			base.Dispose( disposing );
		}

		protected internal override void EndReadSubtree()
		{
			base.EndReadSubtree();

			// Ends current collection.
			this._unpacked.Pop();
			this._unpacked.Push( this._itemsCount.Peek() );
			this.DiscardCompletedStacks();
		}

		protected override Unpacker ReadSubtreeCore()
		{
			if ( this._state == State.InHead )
			{
				// Duplicate call -- just return me.
				return this;
			}

			if ( this._unpacked.Count == 0  )
			{
				ThrowInTailException();
			}

			if ( this._root.InternalCollectionType == ItemsUnpacker.CollectionType.None )
			{
				ThrowNotInHeadOfCollectionException();
			}

			return new SubtreeUnpacker( this._root, this );
		}

		private static void ThrowInTailException()
		{
			throw new InvalidOperationException( "This unpacker is located in the tail." );
		}

		private static void ThrowNotInHeadOfCollectionException()
		{
			throw new InvalidOperationException( "This unpacker is not located in the head of collection." );
		}

		protected override bool ReadCore()
		{
			this.DiscardCompletedStacks();

			if ( this._itemsCount.Count == 0 || !this._root.ReadSubtreeItem() )
			{
				return false;
			}

			switch ( this._root.InternalCollectionType )
			{
				case ItemsUnpacker.CollectionType.Array:
				{
					this._itemsCount.Push( this._root.InternalItemsCount );
					this._unpacked.Push( 0 );
					this._isMap.Push( false );
					break;
				}
				case ItemsUnpacker.CollectionType.Map:
				{
					this._itemsCount.Push( this._root.InternalItemsCount * 2 );
					this._unpacked.Push( 0 );
					this._isMap.Push( true );
					break;
				}
				default:
				{
					this._unpacked.Push( this._unpacked.Pop() + 1 );
					break;
				}
			}

			this._state = State.InProgress;
			return true;
		}

		protected override long? SkipCore()
		{
			this.DiscardCompletedStacks();

			if ( this._itemsCount.Count == 0 )
			{
				return 0;
			}

			var result = this._root.SkipSubtreeItem();
			if ( result != null )
			{
				this._unpacked.Push( this._unpacked.Pop() + 1 );
			}

			return result;
		}

		private void DiscardCompletedStacks()
		{
			if ( this._itemsCount.Count == 0 )
			{
#if DEBUG && !UNITY
				Contract.Assert( this._unpacked.Count == 0, "this._unpacked.Count == 0" );
#endif // DEBUG && !UNITY
				return;
			}

			while ( this._unpacked.Peek() == this._itemsCount.Peek() )
			{
				this._itemsCount.Pop();
				this._unpacked.Pop();
				this._isMap.Pop();

				if ( this._itemsCount.Count == 0 )
				{
#if DEBUG && !UNITY
					Contract.Assert( this._unpacked.Count == 0, "this._unpacked.Count == 0 " );
#endif // DEBUG && !UNITY
					break;
				}

				this._unpacked.Push( this._unpacked.Pop() + 1 );
			}
		}

		private enum State
		{
			InHead = 0,
			InProgress,
			Disposed
		}
	}
}
