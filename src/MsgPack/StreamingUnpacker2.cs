
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
using System.Globalization;
using System.IO;
using System.Linq;

namespace MsgPack
{
		// This file generated from UnpackingStreamingUnpacker.tt and StreamingUnapkcerBase.ttinclude T4Template.
		// Do not modify this file. Edit UnpackingStreamingUnpacker..tt and StreamingUnapkcerBase.ttinclude instead.
	
	
		internal sealed partial class UnpackingStreamingUnpacker
		{
			#region -- Collection States --
			
			private CollectionUnpackingState _currentCollectionState;
			private readonly Stack<CollectionUnpackingState> _collectionStates = new Stack<CollectionUnpackingState>( 4 );
	
			private sealed class CollectionUnpackingState
			{
				private uint _itemsCount;
	
				public uint ItemsCount
				{
					get { return this._itemsCount; }
				}
	
				public void SetItemsCount( uint value )
				{
					this._itemsCount = this._isMap ? value * 2 : value;
				}
	
				public uint UnpackingItemsCount
				{
					get { return ( this._isMap ? this._itemsCount / 2 : this._itemsCount ); }
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
					this.SetItemsCount( unchecked( ( uint )itemsCount ) );
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
	
			#endregion -- Collection States --
	
			public override MessagePackObject? Unpack( Stream source)
			{
				this._collectionItemsCount = null;
				// Continue 
				if( this._scalarBufferPosition >= 0 )
				{
					
					#region ContinueScalarUnpacking
					
					#region ClearEmptyCollectionState
					if( this._currentCollectionState != null )
					{
						if( this._currentCollectionState.UnpackingItemsCount == 0 )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
					}
					#endregion ClearEmptyCollectionState
					
					var scalarReadBytes = source.Read( this._scalarBuffer, this._scalarBufferPosition, ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition );
					if( scalarReadBytes < ( ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition ) )
					{
						// Must wait extra bytes, book keep states.
						this._scalarBufferPosition += scalarReadBytes;
						return null;
					}
					else
					{
						// Whole scalar bytes are read.
						switch( this._scalarKind )
						{
							case ScalarKind.RawLength16:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
								
								#region StartBlobUnpacking
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
								var blob = new byte[ length ];
								var blobReadBytes = source.Read( blob, 0, length );
								
								if( blobReadBytes < length )
								{
									// Must wait extra bytes, book keep states.
									this._blobBuffer = blob;
									this._blobBufferPosition += blobReadBytes;
								
									return null;
								}
								else
								{
									// Whole blob bytes are read.
									// Reset buffer.
									this._blobBuffer = null;
									this._blobBufferPosition = 0;
								
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									return new MessagePackObject( blob );
								}
								#endregion StartBlobUnpacking
								
							}
							case ScalarKind.RawLength32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
								
								#region EnsureLengthIsInt32
								if( length > Int32.MaxValue )
								{
									throw new MessageNotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
								}
								#endregion EnsureLengthIsInt32
								
								
								#region StartBlobUnpacking
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
								var blob = new byte[ unchecked( ( int )length ) ];
								var blobReadBytes = source.Read( blob, 0, unchecked( ( int )length ) );
								
								if( blobReadBytes < unchecked( ( int )length ) )
								{
									// Must wait extra bytes, book keep states.
									this._blobBuffer = blob;
									this._blobBufferPosition += blobReadBytes;
								
									return null;
								}
								else
								{
									// Whole blob bytes are read.
									// Reset buffer.
									this._blobBuffer = null;
									this._blobBufferPosition = 0;
								
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									return new MessagePackObject( blob );
								}
								#endregion StartBlobUnpacking
								
							}
							case ScalarKind.ArrayLength16:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								this._currentCollectionState.SetItemsCount( length );
								return new MessagePackObject( length );
							}
							case ScalarKind.MapLength16:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								this._currentCollectionState.SetItemsCount( length );
								return new MessagePackObject( length );
							}
							case ScalarKind.ArrayLength32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								this._currentCollectionState.SetItemsCount( length );
								return new MessagePackObject( length );
							}
							case ScalarKind.MapLength32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								this._currentCollectionState.SetItemsCount( length );
								return new MessagePackObject( length );
							}
							case ScalarKind.Float32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.Float64:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
									this._scalarBuffer[ 7 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
									this._scalarBuffer[ 6 ] = temp;
									temp = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
									this._scalarBuffer[ 5 ] = temp;
									temp = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
									this._scalarBuffer[ 4 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.Int8:
							{
								var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.UInt8:
							{
								var value = this._scalarBuffer[ 0 ];
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.Int16:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.UInt16:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.Int32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.UInt32:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.Int64:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
									this._scalarBuffer[ 7 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
									this._scalarBuffer[ 6 ] = temp;
									temp = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
									this._scalarBuffer[ 5 ] = temp;
									temp = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
									this._scalarBuffer[ 4 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							case ScalarKind.UInt64:
							{
								
								#region SwapScalarBufferIfLittleEndian
								if( BitConverter.IsLittleEndian )
								{
									byte temp;
									temp = this._scalarBuffer[ 0 ];
									this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
									this._scalarBuffer[ 7 ] = temp;
									temp = this._scalarBuffer[ 1 ];
									this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
									this._scalarBuffer[ 6 ] = temp;
									temp = this._scalarBuffer[ 2 ];
									this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
									this._scalarBuffer[ 5 ] = temp;
									temp = this._scalarBuffer[ 3 ];
									this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
									this._scalarBuffer[ 4 ] = temp;
								}
								#endregion SwapScalarBufferIfLittleEndian
								
								var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								}
								#endregion TryPopContextCollection
								
								return new MessagePackObject( value );
							}
							default:
							{
								throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Unknown scalar '{0}'.", this._scalarKind ) );
							}
						} // switch this._scalarKind 
					}
					#endregion ContinueScalarUnpacking
					
				}
				else if(  this._blobBuffer != null )
				{
					
					#region ContinueBlobUnpacking
					var blob = this._blobBuffer;
					
					#region ClearEmptyCollectionState
					if( this._currentCollectionState != null )
					{
						if( this._currentCollectionState.UnpackingItemsCount == 0 )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
					}
					#endregion ClearEmptyCollectionState
					
					var blobReadBytes = source.Read( blob, this._blobBufferPosition, this._blobBuffer.Length - this._blobBufferPosition );
					
					if( blobReadBytes < this._blobBuffer.Length - this._blobBufferPosition )
					{
						// Must wait extra bytes, book keep states.
						this._blobBuffer = blob;
						this._blobBufferPosition += blobReadBytes;
					
						return null;
					}
					else
					{
						// Whole blob bytes are read.
						// Reset buffer.
						this._blobBuffer = null;
						this._blobBufferPosition = 0;
					
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						return new MessagePackObject( blob );
					}
					#endregion ContinueBlobUnpacking
					
				}
	
				// Header or scalar
				var b = source.ReadByte();
				if(  b >= 0 )
				{
	
					if ( b < 0x80 )
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						return _positiveIntegers[ b ];
					}
	
					if ( b >= 0xE0 )
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						return _negativeIntegers[ b - 0xE0 ];
					}
	
					switch ( b )
					{
						case 0x80:
						{
							this._collectionHeaderKind = CollectionHeaderKind.Map;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							this._collectionItemsCount = 0;
							return _positiveIntegers[ 0 ];
						}
						case 0x90:
						{
							this._collectionHeaderKind = CollectionHeaderKind.Array;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							this._collectionItemsCount = 0;
							return _positiveIntegers[ 0 ];
						}
						case 0xA0:
						{
							this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							return _emptyRaw;
						}
						case MessagePackCode.TrueValue:
						{
							this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							return _true;
						}
						case MessagePackCode.FalseValue:
						{
							this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							return _false;
						}
						case MessagePackCode.NilValue:
						{
							this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							return MessagePackObject.Nil;
						}
					}
	
					if ( b < 0x90 )
					{
						// map
						this._collectionHeaderKind = CollectionHeaderKind.Map;
						
						#region PushFixedContextCollection
						var newCollectionState = CollectionUnpackingState.FixedMap( ( b & 0xF ) );
						this._currentCollectionState = newCollectionState;
						this._collectionStates.Push( newCollectionState );
						#endregion PushFixedContextCollection
						
						return _positiveIntegers[ ( b & 0xF ) ];
					}
					else if ( b < 0xA0 )
					{
						// array
						this._collectionHeaderKind = CollectionHeaderKind.Array;
						
						#region PushFixedContextCollection
						var newCollectionState = CollectionUnpackingState.FixedArray( ( b & 0xF ) );
						this._currentCollectionState = newCollectionState;
						this._collectionStates.Push( newCollectionState );
						#endregion PushFixedContextCollection
						
						return _positiveIntegers[ ( b & 0xF ) ];
					}
					else if ( b < 0xC0 )
					{
						// raw
						
						#region StartBlobUnpacking
						
						#region ClearScalarBuffer
						this._scalarBufferPosition = -1;
						#endregion ClearScalarBuffer
						
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						var blob = new byte[ b - 0xA0 ];
						var blobReadBytes = source.Read( blob, 0, b - 0xA0 );
						
						if( blobReadBytes < b - 0xA0 )
						{
							// Must wait extra bytes, book keep states.
							this._blobBuffer = blob;
							this._blobBufferPosition += blobReadBytes;
						
							return null;
						}
						else
						{
							// Whole blob bytes are read.
							// Reset buffer.
							this._blobBuffer = null;
							this._blobBufferPosition = 0;
						
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							return new MessagePackObject( blob );
						}
						#endregion StartBlobUnpacking
						
					}
					else
					{
						// variable scalars & collections
						switch( b )
						{
							case 0xDC: // array16
							{
								
								#region StartScalarUnpacking
								this._collectionHeaderKind = CollectionHeaderKind.Array;
								this._scalarKind = ScalarKind.ArrayLength16;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Array();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.ArrayLength16 ) & 0xF );
								if( scalarReadBytes < ( ( ( int )ScalarKind.ArrayLength16 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
									
									#region SwapScalarBufferIfLittleEndian
									if( BitConverter.IsLittleEndian )
									{
										byte temp;
										temp = this._scalarBuffer[ 0 ];
										this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
										this._scalarBuffer[ 1 ] = temp;
									}
									#endregion SwapScalarBufferIfLittleEndian
									
									var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
									
									#region ClearScalarBuffer
									this._scalarBufferPosition = -1;
									#endregion ClearScalarBuffer
									
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									this._currentCollectionState.SetItemsCount( length );
									return new MessagePackObject( length );
								}
								#endregion StartScalarUnpacking
								
							}
							case 0xDD: // array32
							{
								
								#region StartScalarUnpacking
								this._collectionHeaderKind = CollectionHeaderKind.Array;
								this._scalarKind = ScalarKind.ArrayLength32;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Array();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.ArrayLength32 ) & 0xF );
								if( scalarReadBytes < ( ( ( int )ScalarKind.ArrayLength32 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
									
									#region SwapScalarBufferIfLittleEndian
									if( BitConverter.IsLittleEndian )
									{
										byte temp;
										temp = this._scalarBuffer[ 0 ];
										this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
										this._scalarBuffer[ 3 ] = temp;
										temp = this._scalarBuffer[ 1 ];
										this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
										this._scalarBuffer[ 2 ] = temp;
									}
									#endregion SwapScalarBufferIfLittleEndian
									
									var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
									
									#region ClearScalarBuffer
									this._scalarBufferPosition = -1;
									#endregion ClearScalarBuffer
									
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									this._currentCollectionState.SetItemsCount( length );
									return new MessagePackObject( length );
								}
								#endregion StartScalarUnpacking
								
							}
							case 0xDE: // map16
							{
								
								#region StartScalarUnpacking
								this._collectionHeaderKind = CollectionHeaderKind.Map;
								this._scalarKind = ScalarKind.MapLength16;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Map();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.MapLength16 ) & 0xF );
								if( scalarReadBytes < ( ( ( int )ScalarKind.MapLength16 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
									
									#region SwapScalarBufferIfLittleEndian
									if( BitConverter.IsLittleEndian )
									{
										byte temp;
										temp = this._scalarBuffer[ 0 ];
										this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
										this._scalarBuffer[ 1 ] = temp;
									}
									#endregion SwapScalarBufferIfLittleEndian
									
									var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
									
									#region ClearScalarBuffer
									this._scalarBufferPosition = -1;
									#endregion ClearScalarBuffer
									
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									this._currentCollectionState.SetItemsCount( length );
									return new MessagePackObject( length );
								}
								#endregion StartScalarUnpacking
								
							}
							case 0xDF: // map32
							{
								
								#region StartScalarUnpacking
								this._collectionHeaderKind = CollectionHeaderKind.Map;
								this._scalarKind = ScalarKind.MapLength32;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Map();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.MapLength32 ) & 0xF );
								if( scalarReadBytes < ( ( ( int )ScalarKind.MapLength32 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
									
									#region SwapScalarBufferIfLittleEndian
									if( BitConverter.IsLittleEndian )
									{
										byte temp;
										temp = this._scalarBuffer[ 0 ];
										this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
										this._scalarBuffer[ 3 ] = temp;
										temp = this._scalarBuffer[ 1 ];
										this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
										this._scalarBuffer[ 2 ] = temp;
									}
									#endregion SwapScalarBufferIfLittleEndian
									
									var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
									
									#region ClearScalarBuffer
									this._scalarBufferPosition = -1;
									#endregion ClearScalarBuffer
									
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									this._currentCollectionState.SetItemsCount( length );
									return new MessagePackObject( length );
								}
								#endregion StartScalarUnpacking
								
							}
							default:
							{
								
								#region StartScalarUnpacking
								this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
								this._scalarKind = _scalarKinds[ b - 0xC0 ];
								this._scalarBufferPosition = 0;
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF );
								if( scalarReadBytes < ( ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
									switch( this._scalarKind )
									{
										case ScalarKind.RawLength16:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
											
											#region StartBlobUnpacking
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
											var blob = new byte[ length ];
											var blobReadBytes = source.Read( blob, 0, length );
											
											if( blobReadBytes < length )
											{
												// Must wait extra bytes, book keep states.
												this._blobBuffer = blob;
												this._blobBufferPosition += blobReadBytes;
											
												return null;
											}
											else
											{
												// Whole blob bytes are read.
												// Reset buffer.
												this._blobBuffer = null;
												this._blobBufferPosition = 0;
											
												
												#region TryPopContextCollection
												while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
												}
												#endregion TryPopContextCollection
												
												return new MessagePackObject( blob );
											}
											#endregion StartBlobUnpacking
											
										}
										case ScalarKind.RawLength32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
											
											#region EnsureLengthIsInt32
											if( length > Int32.MaxValue )
											{
												throw new MessageNotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
											}
											#endregion EnsureLengthIsInt32
											
											
											#region StartBlobUnpacking
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
											var blob = new byte[ unchecked( ( int )length ) ];
											var blobReadBytes = source.Read( blob, 0, unchecked( ( int )length ) );
											
											if( blobReadBytes < unchecked( ( int )length ) )
											{
												// Must wait extra bytes, book keep states.
												this._blobBuffer = blob;
												this._blobBufferPosition += blobReadBytes;
											
												return null;
											}
											else
											{
												// Whole blob bytes are read.
												// Reset buffer.
												this._blobBuffer = null;
												this._blobBufferPosition = 0;
											
												
												#region TryPopContextCollection
												while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
												}
												#endregion TryPopContextCollection
												
												return new MessagePackObject( blob );
											}
											#endregion StartBlobUnpacking
											
										}
										case ScalarKind.ArrayLength16:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											this._currentCollectionState.SetItemsCount( length );
											return new MessagePackObject( length );
										}
										case ScalarKind.MapLength16:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											this._currentCollectionState.SetItemsCount( length );
											return new MessagePackObject( length );
										}
										case ScalarKind.ArrayLength32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											this._currentCollectionState.SetItemsCount( length );
											return new MessagePackObject( length );
										}
										case ScalarKind.MapLength32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											this._currentCollectionState.SetItemsCount( length );
											return new MessagePackObject( length );
										}
										case ScalarKind.Float32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.Float64:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
												this._scalarBuffer[ 7 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
												this._scalarBuffer[ 6 ] = temp;
												temp = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
												this._scalarBuffer[ 5 ] = temp;
												temp = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
												this._scalarBuffer[ 4 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.Int8:
										{
											var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.UInt8:
										{
											var value = this._scalarBuffer[ 0 ];
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.Int16:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.UInt16:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.Int32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.UInt32:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.Int64:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
												this._scalarBuffer[ 7 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
												this._scalarBuffer[ 6 ] = temp;
												temp = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
												this._scalarBuffer[ 5 ] = temp;
												temp = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
												this._scalarBuffer[ 4 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										case ScalarKind.UInt64:
										{
											
											#region SwapScalarBufferIfLittleEndian
											if( BitConverter.IsLittleEndian )
											{
												byte temp;
												temp = this._scalarBuffer[ 0 ];
												this._scalarBuffer[ 0 ] = this._scalarBuffer[ 7 ];
												this._scalarBuffer[ 7 ] = temp;
												temp = this._scalarBuffer[ 1 ];
												this._scalarBuffer[ 1 ] = this._scalarBuffer[ 6 ];
												this._scalarBuffer[ 6 ] = temp;
												temp = this._scalarBuffer[ 2 ];
												this._scalarBuffer[ 2 ] = this._scalarBuffer[ 5 ];
												this._scalarBuffer[ 5 ] = temp;
												temp = this._scalarBuffer[ 3 ];
												this._scalarBuffer[ 3 ] = this._scalarBuffer[ 4 ];
												this._scalarBuffer[ 4 ] = temp;
											}
											#endregion SwapScalarBufferIfLittleEndian
											
											var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											}
											#endregion TryPopContextCollection
											
											return new MessagePackObject( value );
										}
										default:
										{
											throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Unknown scalar '{0}'.", this._scalarKind ) );
										}
									} // switch _scalarKinds[ b - 0xC0 ] 
								}
								#endregion StartScalarUnpacking
								
							}
						}
					}
					
				} // for 
	
				// stream ends
				return null;
			}
		}
	}
