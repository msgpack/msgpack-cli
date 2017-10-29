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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

#if !UNITY || MSGPACK_UNITY_FULL
using BooleanStack = System.Collections.Generic.Stack<System.Boolean>;
using Int64Stack = System.Collections.Generic.Stack<System.Int64>;
#endif // !UNITY || MSGPACK_UNITY_FULL


namespace MsgPack
{
	/// <summary>
	///		Defines subtree unpacking unpacker.
	/// </summary>
	internal sealed partial class SubtreeUnpacker : Unpacker
	{
		private readonly Unpacker _root;
		private readonly IRootUnpacker _internalRoot;
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
			get { return this._internalRoot.CollectionType == CollectionType.Array; }
		}

		public override bool IsMapHeader
		{
			get { return this._internalRoot.CollectionType == CollectionType.Map; }
		}

		public override bool IsCollectionHeader
		{
			get { return this._internalRoot.CollectionType != CollectionType.None; }
		}

		[Obsolete( "Consumer should not use this property. Query LastReadData instead." )]
		public override MessagePackObject? Data
		{
			get { return this._internalRoot.Data; }
			protected set { this._internalRoot.Data = value; }
		}

		public override MessagePackObject LastReadData
		{
			get { return this._internalRoot.LastReadData; }
			protected set { this._internalRoot.LastReadData = value; }
		}

#if DEBUG
		internal override long? UnderlyingStreamPosition
		{
			get { return this._internalRoot.UnderlyingStreamPosition; }
		}
#endif

		public SubtreeUnpacker( Unpacker parent ) : this( parent, null ) { }

		private SubtreeUnpacker( Unpacker root, SubtreeUnpacker parent )
		{
			var internalRoot = root as IRootUnpacker;
#if DEBUG
			Contract.Assert( root != null, "root != null" );
			Contract.Assert( internalRoot != null, "root is IRootUnpacker" );
			Contract.Assert( internalRoot.CollectionType == CollectionType.Array || internalRoot.CollectionType == CollectionType.Map, "root.IsArrayHeader || root.IsMapHeader" );
#endif // DEBUG
			this._root = root;
			this._internalRoot = internalRoot;
			this._parent = parent;
			this._unpacked = new Int64Stack( 2 );

			this._itemsCount = new Int64Stack( 2 );
			this._isMap = new BooleanStack( 2 );

			if ( root.ItemsCount > 0 )
			{
				this._itemsCount.Push( root.ItemsCount * ( ( int )internalRoot.CollectionType ) );
				this._unpacked.Push( 0 );
				this._isMap.Push( internalRoot.CollectionType == CollectionType.Map );
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
					this.Drain();
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

		public override void Drain()
		{
			if ( this._state >= State.Drained )
			{
				return;
			}

			while ( this.Read() )
			{
				// nop
			}

			this._state  = State.Drained;
		}

#if FEATURE_TAP

		public override async Task DrainAsync( CancellationToken cancellationToken )
		{
			if ( this._state >= State.Drained )
			{
				return;
			}

			while ( await this.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
			{
				// nop
			}

			this._state = State.Drained;
		}

#endif // FEATURE_TAP

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

			if ( this._unpacked.Count == 0 )
			{
				ThrowInTailException();
			}

			if ( this._internalRoot.CollectionType == CollectionType.None )
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

			if ( this._itemsCount.Count == 0 || !this._root.ReadInternal() )
			{
				return false;
			}

			switch ( this._internalRoot.CollectionType )
			{
				case CollectionType.Array:
				{
					this._itemsCount.Push( this._root.ItemsCount );
					this._unpacked.Push( 0 );
					this._isMap.Push( false );
					break;
				}
				case CollectionType.Map:
				{
					this._itemsCount.Push( this._root.ItemsCount * 2 );
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

#if FEATURE_TAP

		protected override async Task<bool> ReadAsyncCore( CancellationToken cancellationToken )
		{
			this.DiscardCompletedStacks();

			if ( this._itemsCount.Count == 0 || !( await this._root.ReadInternalAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				return false;
			}

			switch ( this._internalRoot.CollectionType )
			{
				case CollectionType.Array:
				{
					this._itemsCount.Push( this._root.ItemsCount );
					this._unpacked.Push( 0 );
					this._isMap.Push( false );
					break;
				}
				case CollectionType.Map:
				{
					this._itemsCount.Push( this._root.ItemsCount * 2 );
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

#endif // FEATURE_TAP

		protected override long? SkipCore()
		{
			this.DiscardCompletedStacks();

			if ( this._itemsCount.Count == 0 )
			{
				return 0;
			}

			var result = this._root.Skip();
			if ( result != null )
			{
				this._unpacked.Push( this._unpacked.Pop() + 1 );
			}

			return result;
		}

#if FEATURE_TAP

		protected override async Task<long?> SkipAsyncCore( CancellationToken cancellationToken )
		{
			this.DiscardCompletedStacks();

			if ( this._itemsCount.Count == 0 )
			{
				return 0;
			}

			var result = await this._root.SkipAsync( cancellationToken ).ConfigureAwait( false );
			if ( result != null )
			{
				this._unpacked.Push( this._unpacked.Pop() + 1 );
			}

			return result;
		}

#endif // FEATURE_TAP

		private void DiscardCompletedStacks()
		{
			if ( this._itemsCount.Count == 0 )
			{
#if DEBUG
				Contract.Assert( this._unpacked.Count == 0, "this._unpacked.Count == 0" );
#endif // DEBUG
				return;
			}

			while ( this._unpacked.Peek() == this._itemsCount.Peek() )
			{
				this._itemsCount.Pop();
				this._unpacked.Pop();
				this._isMap.Pop();

				if ( this._itemsCount.Count == 0 )
				{
#if DEBUG
					Contract.Assert( this._unpacked.Count == 0, "this._unpacked.Count == 0 " );
#endif // DEBUG
					break;
				}

				this._unpacked.Push( this._unpacked.Pop() + 1 );
			}
		}

		private enum State
		{
			InHead = 0,
			InProgress = 1,
			Drained = 2,
			Disposed = 3,
		}
	}
}
