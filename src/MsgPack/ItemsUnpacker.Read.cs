 
 
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace MsgPack
{
	// This file was generated from ItemsUnpacker.Read.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit ItemsUnpacker.Read.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class ItemsUnpacker
	{
		public override bool ReadBoolean( out Boolean result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeBoolean( out result );
		}
		
		internal bool ReadSubtreeBoolean( out Boolean result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Boolean );
					return false;
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral != 0;
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), header );
					result = default( Boolean ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableBoolean( out Boolean? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableBoolean( out result );
		}
		
		internal bool ReadSubtreeNullableBoolean( out Boolean? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Boolean );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral != 0;
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), header );
					result = default( Boolean ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadByte( out Byte result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeByte( out result );
		}
		
		internal bool ReadSubtreeByte( out Byte result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Byte );
					return false;
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Byte )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte ), header );
					result = default( Byte ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableByte( out Byte? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableByte( out result );
		}
		
		internal bool ReadSubtreeNullableByte( out Byte? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Byte );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Byte )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Byte )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte ), header );
					result = default( Byte ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadSByte( out SByte result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeSByte( out result );
		}
		
		internal bool ReadSubtreeSByte( out SByte result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( SByte );
					return false;
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( SByte )integral );
					return true;
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte ), header );
					result = default( SByte ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableSByte( out SByte? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableSByte( out result );
		}
		
		internal bool ReadSubtreeNullableSByte( out SByte? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( SByte );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( SByte )integral );
					return true;
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( SByte )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte ), header );
					result = default( SByte ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadInt16( out Int16 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeInt16( out result );
		}
		
		internal bool ReadSubtreeInt16( out Int16 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int16 );
					return false;
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Int16 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), header );
					result = default( Int16 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableInt16( out Int16? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableInt16( out result );
		}
		
		internal bool ReadSubtreeNullableInt16( out Int16? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int16 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Int16 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int16 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), header );
					result = default( Int16 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadUInt16( out UInt16 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeUInt16( out result );
		}
		
		internal bool ReadSubtreeUInt16( out UInt16 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt16 );
					return false;
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt16 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), header );
					result = default( UInt16 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableUInt16( out UInt16? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableUInt16( out result );
		}
		
		internal bool ReadSubtreeNullableUInt16( out UInt16? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt16 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt16 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt16 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), header );
					result = default( UInt16 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadInt32( out Int32 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeInt32( out result );
		}
		
		internal bool ReadSubtreeInt32( out Int32 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int32 );
					return false;
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Int32 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), header );
					result = default( Int32 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableInt32( out Int32? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableInt32( out result );
		}
		
		internal bool ReadSubtreeNullableInt32( out Int32? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int32 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( Int32 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int32 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), header );
					result = default( Int32 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadUInt32( out UInt32 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeUInt32( out result );
		}
		
		internal bool ReadSubtreeUInt32( out UInt32 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt32 );
					return false;
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt32 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), header );
					result = default( UInt32 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableUInt32( out UInt32? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableUInt32( out result );
		}
		
		internal bool ReadSubtreeNullableUInt32( out UInt32? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt32 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt32 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )integral );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt32 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), header );
					result = default( UInt32 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadInt64( out Int64 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeInt64( out result );
		}
		
		internal bool ReadSubtreeInt64( out Int64 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int64 );
					return false;
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = integral;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					result = default( Int64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableInt64( out Int64? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableInt64( out result );
		}
		
		internal bool ReadSubtreeNullableInt64( out Int64? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int64 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = integral;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Int64 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					result = default( Int64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadUInt64( out UInt64 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeUInt64( out result );
		}
		
		internal bool ReadSubtreeUInt64( out UInt64 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt64 );
					return false;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), header );
					result = default( UInt64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableUInt64( out UInt64? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableUInt64( out result );
		}
		
		internal bool ReadSubtreeNullableUInt64( out UInt64? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( UInt64 );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )real32 );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( UInt64 )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), header );
					result = default( UInt64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadSingle( out Single result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeSingle( out result );
		}
		
		internal bool ReadSubtreeSingle( out Single result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Single );
					return false;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = real32;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Single )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Single ), header );
					result = default( Single ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableSingle( out Single? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableSingle( out result );
		}
		
		internal bool ReadSubtreeNullableSingle( out Single? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Single );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = real32;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = checked( ( Single )real64 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Single ), header );
					result = default( Single ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadDouble( out Double result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeDouble( out result );
		}
		
		internal bool ReadSubtreeDouble( out Double result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Double );
					return false;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = real64;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real32;
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Double ), header );
					result = default( Double ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadNullableDouble( out Double? result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeNullableDouble( out result );
		}
		
		internal bool ReadSubtreeNullableDouble( out Double? result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Double );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result  = real64;
					return true;
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real32;
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Double ), header );
					result = default( Double ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadBinary( out Byte[] result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeBinary( out result );
		}
		
		internal bool ReadSubtreeBinary( out Byte[] result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Byte[] );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					result = this.ReadBinaryCore( integral );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte[] ), header );
					result = default( Byte[] ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadString( out String result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeString( out result );
		}
		
		internal bool ReadSubtreeString( out String result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( String );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = null;
					return true;
				}
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					result = this.ReadStringCore( integral );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( String ), header );
					result = default( String ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadObject( out MessagePackObject result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeObject( /* isDeep */true, out result );
		}
		
		internal bool ReadSubtreeObject( bool isDeep, out MessagePackObject result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( MessagePackObject );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = MessagePackObject.Nil;
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral != 0;
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( SByte )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Int16 )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Int32 )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Byte )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt16 )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt32 )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real32;
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real64;
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.ArrayLength:
				{
					var length = unchecked( ( UInt32 )this.ReadArrayLengthCore( integral ) );
					if ( !isDeep )
					{
						result = length;
						return true;
					}
				
					this.CheckLength( length, ReadValueResult.ArrayLength );
					var collection = new List<MessagePackObject>( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject item;
						if( !this.ReadSubtreeObject( /* isDeep */true, out item ) )
						{
							result = default( MessagePackObject );
							return false;
						}
				
						collection.Add( item );
					}
					result = new MessagePackObject( collection, /* isImmutable */true );
					return true;
				}
				case ReadValueResult.MapLength:
				{
					var length = unchecked( ( UInt32 )this.ReadMapLengthCore( integral ) );
					if ( !isDeep )
					{
						result = length;
						return true;
					}
				
					this.CheckLength( length, ReadValueResult.MapLength );
					var collection = new MessagePackObjectDictionary( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject key;
						if( !this.ReadSubtreeObject( /* isDeep */true, out key ) )
						{
							result = default( MessagePackObject );
							return false;
						}
				
						MessagePackObject value;
						if( !this.ReadSubtreeObject( /* isDeep */true, out value ) )
						{
							result = default( MessagePackObject );
							return false;
						}
				
						collection.Add( key, value );
					}
					result = new MessagePackObject( collection, /* isImmutable */true );
					return true;
				}
				
				case ReadValueResult.String:
				{
					result = new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), false ) );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.Binary:
				{
					result = new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), true ) );
					this.InternalData = result;
					return true;
				}
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					result = this.ReadMessagePackExtendedTypeObjectCore( type );
					this.InternalData = result;
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( MessagePackObject ), header );
					result = default( MessagePackObject ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadArrayLength( out Int64 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeArrayLength( out result );
		}
		
		internal bool ReadSubtreeArrayLength( out Int64 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int64 );
					return false;
				}
				case ReadValueResult.ArrayLength:
				{
					result = this.ReadArrayLengthCore( integral );
					this.CheckLength( result, ReadValueResult.ArrayLength );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					result = default( Int64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadMapLength( out Int64 result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeMapLength( out result );
		}
		
		internal bool ReadSubtreeMapLength( out Int64 result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( Int64 );
					return false;
				}
				case ReadValueResult.MapLength:
				{
					result = this.ReadMapLengthCore( integral );
					this.CheckLength( result, ReadValueResult.MapLength );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					result = default( Int64 ); // Never reach
					return false; // Never reach
				}
			}
		}
		
		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
		#if !UNITY
			this.EnsureNotInSubtreeMode();
		#endif // !UNITY
		
			return this.ReadSubtreeMessagePackExtendedTypeObject( out result );
		}
		
		internal bool ReadSubtreeMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					result = default( MessagePackExtendedTypeObject );
					return false;
				}
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					result = this.ReadMessagePackExtendedTypeObjectCore( type );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject ), header );
					result = default( MessagePackExtendedTypeObject ); // Never reach
					return false; // Never reach
				}
			}
		}
		
	}
}

