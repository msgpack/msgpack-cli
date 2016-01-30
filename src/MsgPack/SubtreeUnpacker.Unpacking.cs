 
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

using System;

namespace MsgPack
{
	// This file was generated from SubtreeUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit SubtreeUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class SubtreeUnpacker
	{
		public override bool ReadBoolean( out Boolean result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Boolean );
				return false;
			}
			
			if ( !this._root.ReadSubtreeBoolean( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableBoolean( out Boolean? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Boolean? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableBoolean( out result ) )
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
			
			return true;
		}

		public override bool ReadByte( out Byte result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Byte );
				return false;
			}
			
			if ( !this._root.ReadSubtreeByte( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableByte( out Byte? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Byte? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableByte( out result ) )
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
			
			return true;
		}

		public override bool ReadSByte( out SByte result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( SByte );
				return false;
			}
			
			if ( !this._root.ReadSubtreeSByte( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableSByte( out SByte? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( SByte? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableSByte( out result ) )
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
			
			return true;
		}

		public override bool ReadInt16( out Int16 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int16 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeInt16( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableInt16( out Int16? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int16? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableInt16( out result ) )
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
			
			return true;
		}

		public override bool ReadUInt16( out UInt16 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt16 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeUInt16( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableUInt16( out UInt16? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt16? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableUInt16( out result ) )
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
			
			return true;
		}

		public override bool ReadInt32( out Int32 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int32 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeInt32( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableInt32( out Int32? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int32? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableInt32( out result ) )
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
			
			return true;
		}

		public override bool ReadUInt32( out UInt32 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt32 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeUInt32( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableUInt32( out UInt32? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt32? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableUInt32( out result ) )
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
			
			return true;
		}

		public override bool ReadInt64( out Int64 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeInt64( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableInt64( out Int64? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int64? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableInt64( out result ) )
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
			
			return true;
		}

		public override bool ReadUInt64( out UInt64 result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt64 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeUInt64( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableUInt64( out UInt64? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( UInt64? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableUInt64( out result ) )
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
			
			return true;
		}

		public override bool ReadSingle( out Single result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Single );
				return false;
			}
			
			if ( !this._root.ReadSubtreeSingle( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableSingle( out Single? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Single? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableSingle( out result ) )
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
			
			return true;
		}

		public override bool ReadDouble( out Double result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Double );
				return false;
			}
			
			if ( !this._root.ReadSubtreeDouble( out result ) )
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
			
			return true;
		}

		public override bool ReadNullableDouble( out Double? result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Double? );
				return false;
			}
			
			if ( !this._root.ReadSubtreeNullableDouble( out result ) )
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
			
			return true;
		}


		public override bool ReadArrayLength( out long result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeArrayLength( out result ) )
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
			
			return true;
		}

		public override bool ReadMapLength( out long result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Int64 );
				return false;
			}
			
			if ( !this._root.ReadSubtreeMapLength( out result ) )
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
			
			return true;
		}

		public override bool ReadBinary( out byte[] result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( Byte[] );
				return false;
			}
			
			if ( !this._root.ReadSubtreeBinary( out result ) )
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
			
			return true;
		}

		public override bool ReadString( out string result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( String );
				return false;
			}
			
			if ( !this._root.ReadSubtreeString( out result ) )
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
			
			return true;
		}

		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}
			
			if ( !this._root.ReadSubtreeMessagePackExtendedTypeObject( out result ) )
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
			
			return true;
		}

		public override bool ReadObject( out MessagePackObject result )
		{
			this.DiscardCompletedStacks();
			
			if ( this._itemsCount.Count == 0 )
			{
				result = default( MessagePackObject );
				return false;
			}
			
			if ( !this._root.ReadSubtreeObject( /* isDeep */true, out result ) )
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
			
			return true;
		}
	}
}
