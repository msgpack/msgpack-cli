 
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
	// This file was generated from SubtreeUnpacker.Unpacking.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit SubtreeUnpacker.Unpacking.tt and Core.ttinclude instead.

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
			
			if ( !this._root.ReadBoolean( out result ) )
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
			
			if ( !this._root.ReadNullableBoolean( out result ) )
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
			
			if ( !this._root.ReadByte( out result ) )
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
			
			if ( !this._root.ReadNullableByte( out result ) )
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
			
			if ( !this._root.ReadSByte( out result ) )
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
			
			if ( !this._root.ReadNullableSByte( out result ) )
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
			
			if ( !this._root.ReadInt16( out result ) )
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
			
			if ( !this._root.ReadNullableInt16( out result ) )
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
			
			if ( !this._root.ReadUInt16( out result ) )
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
			
			if ( !this._root.ReadNullableUInt16( out result ) )
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
			
			if ( !this._root.ReadInt32( out result ) )
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
			
			if ( !this._root.ReadNullableInt32( out result ) )
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
			
			if ( !this._root.ReadUInt32( out result ) )
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
			
			if ( !this._root.ReadNullableUInt32( out result ) )
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
			
			if ( !this._root.ReadInt64( out result ) )
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
			
			if ( !this._root.ReadNullableInt64( out result ) )
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
			
			if ( !this._root.ReadUInt64( out result ) )
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
			
			if ( !this._root.ReadNullableUInt64( out result ) )
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
			
			if ( !this._root.ReadSingle( out result ) )
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
			
			if ( !this._root.ReadNullableSingle( out result ) )
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
			
			if ( !this._root.ReadDouble( out result ) )
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
			
			if ( !this._root.ReadNullableDouble( out result ) )
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
			
			if ( !this._root.ReadArrayLength( out result ) )
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
			
			if ( !this._root.ReadMapLength( out result ) )
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
			
			if ( !this._root.ReadBinary( out result ) )
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
			
			if ( !this._root.ReadString( out result ) )
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
			
			if ( !this._root.ReadMessagePackExtendedTypeObject( out result ) )
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
			
			if ( !this._internalRoot.ReadObject( /* isDeep */true, out result ) )
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
			
			return true;
		}
	}
}
