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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeBooleanAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					// Never reach
					result = default( Boolean );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Boolean>> ReadSubtreeBooleanAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Boolean>();
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( asyncResult.integral != 0 );
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Boolean>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableBoolean( out Boolean? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableBoolean( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableBooleanAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Boolean? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Boolean? );
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
					this.ThrowTypeException( typeof( Boolean? ), header );
					// Never reach
					result = default( Boolean? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Boolean?>> ReadSubtreeNullableBooleanAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Boolean?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Boolean? ) );
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Boolean?>( asyncResult.integral != 0 );
				}
				default:
				{
					this.ThrowTypeException( typeof( Boolean? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Boolean?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadByte( out Byte result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeByte( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeByteAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( Byte )integral );
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
					result = checked( ( Byte )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte ), header );
					// Never reach
					result = default( Byte );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Byte>> ReadSubtreeByteAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Byte>();
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( Byte )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Byte )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Byte>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableByte( out Byte? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableByte( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableByteAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Byte? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Byte? );
					return true;
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Byte )integral );
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
					result = checked( ( Byte )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte? ), header );
					// Never reach
					result = default( Byte? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Byte?>> ReadSubtreeNullableByteAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Byte?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Byte? ) );
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Byte?>( unchecked( ( Byte )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Byte?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadSByte( out SByte result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeSByte( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeSByteAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( SByte )integral );
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
					result = checked( ( SByte )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte ), header );
					// Never reach
					result = default( SByte );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<SByte>> ReadSubtreeSByteAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<SByte>();
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( SByte )asyncResult.integral ) );
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( SByte )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<SByte>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableSByte( out SByte? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableSByte( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableSByteAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( SByte? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( SByte? );
					return true;
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( SByte )integral );
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
					result = checked( ( SByte )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte? ), header );
					// Never reach
					result = default( SByte? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<SByte?>> ReadSubtreeNullableSByteAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<SByte?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( SByte? ) );
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<SByte?>( unchecked( ( SByte )asyncResult.integral ) );
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( SByte? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<SByte?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadInt16( out Int16 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt16( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt16Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( Int16 )integral );
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
					result = checked( ( Int16 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), header );
					// Never reach
					result = default( Int16 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int16>> ReadSubtreeInt16Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int16>();
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( Int16 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int16 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int16>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableInt16( out Int16? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt16( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt16Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Int16? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Int16? );
					return true;
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Int16 )integral );
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
					result = checked( ( Int16 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16? ), header );
					// Never reach
					result = default( Int16? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int16?>> ReadSubtreeNullableInt16Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int16?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Int16? ) );
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int16?>( unchecked( ( Int16 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int16? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int16?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadUInt16( out UInt16 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt16( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt16Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( UInt16 )integral );
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
					result = checked( ( UInt16 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), header );
					// Never reach
					result = default( UInt16 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt16>> ReadSubtreeUInt16Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt16>();
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( UInt16 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt16 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt16>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableUInt16( out UInt16? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt16( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt16Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( UInt16? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( UInt16? );
					return true;
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt16 )integral );
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
					result = checked( ( UInt16 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16? ), header );
					// Never reach
					result = default( UInt16? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt16?>> ReadSubtreeNullableUInt16Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt16?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( UInt16? ) );
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt16?>( unchecked( ( UInt16 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt16? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt16?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadInt32( out Int32 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt32( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt32Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( Int32 )integral );
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
					result = checked( ( Int32 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), header );
					// Never reach
					result = default( Int32 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int32>> ReadSubtreeInt32Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int32>();
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( Int32 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int32 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int32>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableInt32( out Int32? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt32( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt32Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Int32? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Int32? );
					return true;
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( Int32 )integral );
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
					result = checked( ( Int32 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32? ), header );
					// Never reach
					result = default( Int32? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int32?>> ReadSubtreeNullableInt32Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int32?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Int32? ) );
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int32?>( unchecked( ( Int32 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int32? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int32?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadUInt32( out UInt32 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt32( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt32Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( UInt32 )integral );
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
					result = checked( ( UInt32 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), header );
					// Never reach
					result = default( UInt32 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt32>> ReadSubtreeUInt32Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt32>();
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( UInt32 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt32 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt32>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableUInt32( out UInt32? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt32( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt32Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( UInt32? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( UInt32? );
					return true;
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt32 )integral );
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
					result = checked( ( UInt32 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32? ), header );
					// Never reach
					result = default( UInt32? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt32?>> ReadSubtreeNullableUInt32Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt32?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( UInt32? ) );
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt32?>( unchecked( ( UInt32 )asyncResult.integral ) );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt32? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt32?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadInt64( out Int64 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt64( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeInt64Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = integral;
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
					result = checked( ( Int64 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					result = default( Int64 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int64>> ReadSubtreeInt64Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int64>();
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( asyncResult.integral );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( asyncResult.integral );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int64 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int64 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Int64 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int64>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableInt64( out Int64? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt64( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableInt64Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Int64? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Int64? );
					return true;
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = integral;
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
					result = checked( ( Int64 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64? ), header );
					// Never reach
					result = default( Int64? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int64?>> ReadSubtreeNullableInt64Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int64?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Int64? ) );
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int64?>( asyncResult.integral );
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int64?>( asyncResult.integral );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int64?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadUInt64( out UInt64 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt64( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeUInt64Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = unchecked( ( UInt64 )integral );
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
					result = checked( ( UInt64 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), header );
					// Never reach
					result = default( UInt64 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt64>> ReadSubtreeUInt64Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt64>();
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( unchecked( ( UInt64 )asyncResult.integral ) );
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
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt64>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableUInt64( out UInt64? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt64( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableUInt64Async( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( UInt64? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( UInt64? );
					return true;
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					result = unchecked( ( UInt64 )integral );
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
					result = checked( ( UInt64 )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64? ), header );
					// Never reach
					result = default( UInt64? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<UInt64?>> ReadSubtreeNullableUInt64Async( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<UInt64?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( UInt64? ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt64?>( unchecked( ( UInt64 )asyncResult.integral ) );
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
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.real32 ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( UInt64? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<UInt64?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadSingle( out Single result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeSingle( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeSingleAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = real32;
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
					result = checked( ( Single )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Single ), header );
					// Never reach
					result = default( Single );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Single>> ReadSubtreeSingleAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Single>();
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( asyncResult.real32 );
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
					return AsyncReadResult.Success( checked( ( Single )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Single )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Single )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Single ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Single>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableSingle( out Single? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableSingle( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableSingleAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Single? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Single? );
					return true;
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real32;
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
					result = checked( ( Single )real32 );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Single? ), header );
					// Never reach
					result = default( Single? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Single?>> ReadSubtreeNullableSingleAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Single?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Single? ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Single?>( asyncResult.real32 );
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
					return AsyncReadResult.Success<Single?>( checked( ( Single )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Single?>( checked( ( Single )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Single?>( checked( ( Single )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Single? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Single?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadDouble( out Double result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeDouble( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeDoubleAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = real64;
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
					// Never reach
					result = default( Double );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Double>> ReadSubtreeDoubleAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Double>();
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( asyncResult.real64 );
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
					return AsyncReadResult.Success( checked( ( Double )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Double )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( checked( ( Double )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Double ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Double>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadNullableDouble( out Double? result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableDouble( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeNullableDoubleAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Double? );
					return false;
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					result = default( Double? );
					return true;
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					result = real64;
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
					this.ThrowTypeException( typeof( Double? ), header );
					// Never reach
					result = default( Double? );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Double?>> ReadSubtreeNullableDoubleAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Double?>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Double? ) );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Double?>( asyncResult.real64 );
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
					return AsyncReadResult.Success<Double?>( checked( ( Double )asyncResult.integral ) );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Double?>( checked( ( Double )( UInt64 )asyncResult.integral ) );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success<Double?>( checked( ( Double )asyncResult.real32 ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Double? ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Double?>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadBinary( out Byte[] result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeBinary( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Byte[]>> ReadBinaryAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeBinaryAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( Byte[] );
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
					// Never reach
					result = default( Byte[] );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Byte[]>> ReadSubtreeBinaryAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Byte[]>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( Byte[] ) );
				}
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					return AsyncReadResult.Success( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( Byte[] ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Byte[]>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadString( out String result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeString( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<String>> ReadStringAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeStringAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = default( String );
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
					// Never reach
					result = default( String );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<String>> ReadSubtreeStringAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<String>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					return AsyncReadResult.Success( default( String ) );
				}
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					return AsyncReadResult.Success( await this.ReadStringAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( String ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<String>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadObject( out MessagePackObject result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeObject( /* isDeep */true, out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeObjectAsync( /* isDeep */true, cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
						this.InternalData = result;
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
				
					{
						result = new MessagePackObject( collection, /* isImmutable */true );
						this.InternalData = result;
						return true;
					}
				}
				case ReadValueResult.MapLength:
				{
					var length = unchecked( ( UInt32 )this.ReadMapLengthCore( integral ) );
					if ( !isDeep )
					{
						result = length;
						this.InternalData = result;
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
				
					{
						result = new MessagePackObject( collection, /* isImmutable */true );
						this.InternalData = result;
						return true;
					}
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
					// Never reach
					result = default( MessagePackObject );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<MessagePackObject>> ReadSubtreeObjectAsync( bool isDeep, CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<MessagePackObject>();
				}
				case ReadValueResult.Nil:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = MessagePackObject.Nil;
					this.InternalData = result;
					return AsyncReadResult.Success( result );
				}
				case ReadValueResult.Boolean:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = asyncResult.integral != 0;
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.SByte:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( SByte )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Int16:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( Int16 )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Int32:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( Int32 )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Int64:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = asyncResult.integral;
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Byte:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( Byte )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.UInt16:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( UInt16 )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.UInt32:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( UInt32 )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.UInt64:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = unchecked( ( UInt64 )asyncResult.integral );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Single:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = asyncResult.real32;
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.Double:
				{
					this.InternalCollectionType = CollectionType.None;
					var result = asyncResult.real64;
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				case ReadValueResult.ArrayLength:
				{
					// ReadArrayLengthCore does not perform I/O, so no ReadArrayLengthAsyncCore exists.
					var length = unchecked( ( UInt32 )this.ReadArrayLengthCore( asyncResult.integral ) );
					if ( !isDeep )
					{
						var result = length;
						this.InternalData = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
					}
				
					this.CheckLength( length, ReadValueResult.ArrayLength );
					var collection = new List<MessagePackObject>( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject item;
						if( !this.ReadSubtreeObject( /* isDeep */true, out item ) )
						{
							return AsyncReadResult.Fail<MessagePackObject>();
						}
				
						collection.Add( item );
					}
				
					{
						var result = new MessagePackObject( collection, /* isImmutable */true );
						this.InternalData = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
					}
				}
				case ReadValueResult.MapLength:
				{
					// ReadMapLengthCore does not perform I/O, so no ReadMapLengthAsyncCore exists.
					var length = unchecked( ( UInt32 )this.ReadMapLengthCore( asyncResult.integral ) );
					if ( !isDeep )
					{
						var result = length;
						this.InternalData = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
					}
				
					this.CheckLength( length, ReadValueResult.MapLength );
					var collection = new MessagePackObjectDictionary( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject key;
						if( !this.ReadSubtreeObject( /* isDeep */true, out key ) )
						{
							return AsyncReadResult.Fail<MessagePackObject>();
						}
				
						MessagePackObject value;
						if( !this.ReadSubtreeObject( /* isDeep */true, out value ) )
						{
							return AsyncReadResult.Fail<MessagePackObject>();
						}
				
						collection.Add( key, value );
					}
				
					{
						var result = new MessagePackObject( collection, /* isImmutable */true );
						this.InternalData = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
					}
				}
				
				case ReadValueResult.String:
				{
					var result = new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), false ) );
					this.InternalData = result;
					return AsyncReadResult.Success( result );
				}
				case ReadValueResult.Binary:
				{
					var result = new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), true ) );
					this.InternalData = result;
					return AsyncReadResult.Success( result );
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
					var result = await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false );
					this.InternalData = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
				}
				default:
				{
					this.ThrowTypeException( typeof( MessagePackObject ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<MessagePackObject>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadArrayLength( out Int64 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeArrayLength( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int64>> ReadArrayLengthAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeArrayLengthAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = this.ReadArrayLengthCore( integral);
					this.CheckLength( result, ReadValueResult.ArrayLength );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					result = default( Int64 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int64>> ReadSubtreeArrayLengthAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int64>();
				}
				case ReadValueResult.ArrayLength:
				{
					// ReadArrayLengthCore does not perform I/O, so no ReadArrayLengthAsyncCore exists.
					var result = this.ReadArrayLengthCore( asyncResult.integral);
					this.CheckLength( result, ReadValueResult.ArrayLength );
					return AsyncReadResult.Success( result );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int64>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadMapLength( out Int64 result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeMapLength( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<Int64>> ReadMapLengthAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeMapLengthAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					result = this.ReadMapLengthCore( integral);
					this.CheckLength( result, ReadValueResult.MapLength );
					return true;
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					result = default( Int64 );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<Int64>> ReadSubtreeMapLengthAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<Int64>();
				}
				case ReadValueResult.MapLength:
				{
					// ReadMapLengthCore does not perform I/O, so no ReadMapLengthAsyncCore exists.
					var result = this.ReadMapLengthCore( asyncResult.integral);
					this.CheckLength( result, ReadValueResult.MapLength );
					return AsyncReadResult.Success( result );
				}
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<Int64>();
				}
			}
		} 
		
#endif // FEATURE_TAP

		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeMessagePackExtendedTypeObject( out result );
		} 
		
#if FEATURE_TAP

		public override Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
#if !UNITY
			this.EnsureNotInSubtreeMode();
#endif // !UNITY

			return this.ReadSubtreeMessagePackExtendedTypeObjectAsync( cancellationToken );
		} 
		
#endif // FEATURE_TAP

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
					// Never reach
					result = default( MessagePackExtendedTypeObject );
					return false;
				}
			}
		} 
		
#if FEATURE_TAP

		internal async Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadSubtreeMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
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
					return AsyncReadResult.Success( await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false ) );
				}
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject ), asyncResult.header );
					// Never reach
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
				}
			}
		} 
		
#endif // FEATURE_TAP

	}
}

