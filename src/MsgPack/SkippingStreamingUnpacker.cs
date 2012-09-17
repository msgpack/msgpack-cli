
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

namespace MsgPack
{
		// This file generated from SkippingStreamingUnpacker.tt and StreamingUnapkcerBase.ttinclude T4Template.
		// Do not modify this file. Edit SkippingStreamingUnpacker..tt and StreamingUnapkcerBase.ttinclude instead.
	
	
		internal sealed partial class SkippingStreamingUnpacker
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
				// Continue 
				if( this._scalarBufferPosition >= 0 )
				{
					
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
					
					
					#region ContinueScalarUnpacking
					var scalarReadBytes = source.Read( this._scalarBuffer, this._scalarBufferPosition, ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition );
					this._readByteLength += scalarReadBytes;
					if( scalarReadBytes < ( ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition ) )
					{
						// Must wait extra bytes, book keep states.
						this._scalarBufferPosition += scalarReadBytes;
						return null;
					}
					else
					{
						// Whole scalar bytes are read.
					
					#region ClearScalarBuffer
					this._scalarBufferPosition = -1;
					#endregion ClearScalarBuffer
					
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
								this._remainingBlob = length;
								long blobReadBytes = 0;
								int recentRead;
								int reading;
								do
								{
									reading  = _dummyBuffer.Length < length ? _dummyBuffer.Length : unchecked( ( int )length );
									recentRead = source.Read( _dummyBuffer, 0, reading );
									blobReadBytes += recentRead;
								} while ( recentRead == reading && blobReadBytes < length);
								
								this._readByteLength += blobReadBytes;
								if( blobReadBytes < this._remainingBlob )
								{
									// Must wait extra bytes, book keep states.
									this._remainingBlob -= blobReadBytes;
								
									return null;
								}
								else
								{
									// Whole blob bytes are read.
									// Reset buffer.
									this._remainingBlob = -1;
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									if( this._currentCollectionState == null )
									{
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								#endregion StartBlobUnpacking
								
								break;
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
								
								#region StartBlobUnpacking
								this._remainingBlob = length;
								long blobReadBytes = 0;
								int recentRead;
								int reading;
								do
								{
									reading  = _dummyBuffer.Length < length ? _dummyBuffer.Length : unchecked( ( int )length );
									recentRead = source.Read( _dummyBuffer, 0, reading );
									blobReadBytes += recentRead;
								} while ( recentRead == reading && blobReadBytes < length);
								
								this._readByteLength += blobReadBytes;
								if( blobReadBytes < this._remainingBlob )
								{
									// Must wait extra bytes, book keep states.
									this._remainingBlob -= blobReadBytes;
								
									return null;
								}
								else
								{
									// Whole blob bytes are read.
									// Reset buffer.
									this._remainingBlob = -1;
									
									#region TryPopContextCollection
									while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									}
									#endregion TryPopContextCollection
									
									if( this._currentCollectionState == null )
									{
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								#endregion StartBlobUnpacking
								
								break;
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
								this._currentCollectionState.SetItemsCount( length );
								if( length == 0 )
								{
									if( this._collectionStates.Count == 1 )
									{
										// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								break;
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
								this._currentCollectionState.SetItemsCount( length );
								if( length == 0 )
								{
									if( this._collectionStates.Count == 1 )
									{
										// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								break;
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
								this._currentCollectionState.SetItemsCount( length );
								if( length == 0 )
								{
									if( this._collectionStates.Count == 1 )
									{
										// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								break;
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
								this._currentCollectionState.SetItemsCount( length );
								if( length == 0 )
								{
									if( this._collectionStates.Count == 1 )
									{
										// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								break;
							}
							case ScalarKind.Float32:
							case ScalarKind.Float64:
							case ScalarKind.Int8:
							case ScalarKind.UInt8:
							case ScalarKind.Int16:
							case ScalarKind.UInt16:
							case ScalarKind.Int32:
							case ScalarKind.UInt32:
							case ScalarKind.Int64:
							case ScalarKind.UInt64:
							{
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
								if( this._currentCollectionState == null )
								{
									#region Returnlength
									var readLength = this._readByteLength;
									this._readByteLength = 0;
									return new MessagePackObject( readLength );
									#endregion ReturnLength
								}
								
								#region TryPopContextCollection
								while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
									if( this._currentCollectionState == null )
									{
										#region Returnlength
										var readLength = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readLength );
										#endregion ReturnLength
									}
								}
								#endregion TryPopContextCollection
								
								break;
							}
						} // switch this._scalarKind 
					}
					#endregion ContinueScalarUnpacking
					
				}
				else if(  this._remainingBlob >= 0 )
				{
					
					#region ContinueBlobUnpacking
					
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
					
					long blobReadBytes = 0;
					int recentRead;
					int reading;
					do
					{
						reading  = _dummyBuffer.Length < this._remainingBlob ? _dummyBuffer.Length : unchecked( ( int )this._remainingBlob );
						recentRead = source.Read( _dummyBuffer, 0, reading );
						blobReadBytes += recentRead;
					} while ( recentRead == reading && blobReadBytes < this._remainingBlob);
					
					this._readByteLength += blobReadBytes;
					if( blobReadBytes < this._remainingBlob )
					{
						// Must wait extra bytes, book keep states.
						this._remainingBlob -= blobReadBytes;
					
						return null;
					}
					else
					{
						// Whole blob bytes are read.
						// Reset buffer.
						this._remainingBlob = -1;
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						if( this._currentCollectionState == null )
						{
							#region Returnlength
							var readLength = this._readByteLength;
							this._readByteLength = 0;
							return new MessagePackObject( readLength );
							#endregion ReturnLength
						}
					}
					#endregion ContinueBlobUnpacking
					
				}
	
				// Header or scalar
				for( var b = source.ReadByte(); b >= 0; b = source.ReadByte() )
				{
					this._readByteLength++;
	
					if ( b < 0x80 )
					{
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						if( this._currentCollectionState == null )
						{
							#region Returnlength
							var readLength = this._readByteLength;
							this._readByteLength = 0;
							return new MessagePackObject( readLength );
							#endregion ReturnLength
						}
						continue;
					}
	
					if ( b >= 0xE0 )
					{
						
						#region TryPopContextCollection
						while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						}
						#endregion TryPopContextCollection
						
						if( this._currentCollectionState == null )
						{
							#region Returnlength
							var readLength = this._readByteLength;
							this._readByteLength = 0;
							return new MessagePackObject( readLength );
							#endregion ReturnLength
						}
						continue;
					}
	
					switch ( b )
					{
						case 0x80:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
						case 0x90:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
						case 0xA0:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
						case MessagePackCode.TrueValue:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
						case MessagePackCode.FalseValue:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
						case MessagePackCode.NilValue:
						{
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
							continue;
						}
					}
	
					if ( b < 0x90 )
					{
						// map
						
						#region PushFixedContextCollection
						var newCollectionState = CollectionUnpackingState.FixedMap( ( b & 0xF ) );
						this._currentCollectionState = newCollectionState;
						this._collectionStates.Push( newCollectionState );
						#endregion PushFixedContextCollection
						
					}
					else if ( b < 0xA0 )
					{
						// array
						
						#region PushFixedContextCollection
						var newCollectionState = CollectionUnpackingState.FixedArray( ( b & 0xF ) );
						this._currentCollectionState = newCollectionState;
						this._collectionStates.Push( newCollectionState );
						#endregion PushFixedContextCollection
						
					}
					else if ( b < 0xC0 )
					{
						// raw
						
						#region StartBlobUnpacking
						this._remainingBlob = b - 0xA0;
						long blobReadBytes = 0;
						int recentRead;
						int reading;
						do
						{
							reading  = _dummyBuffer.Length < b - 0xA0 ? _dummyBuffer.Length : unchecked( ( int )b - 0xA0 );
							recentRead = source.Read( _dummyBuffer, 0, reading );
							blobReadBytes += recentRead;
						} while ( recentRead == reading && blobReadBytes < b - 0xA0);
						
						this._readByteLength += blobReadBytes;
						if( blobReadBytes < this._remainingBlob )
						{
							// Must wait extra bytes, book keep states.
							this._remainingBlob -= blobReadBytes;
						
							return null;
						}
						else
						{
							// Whole blob bytes are read.
							// Reset buffer.
							this._remainingBlob = -1;
							
							#region TryPopContextCollection
							while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							}
							#endregion TryPopContextCollection
							
							if( this._currentCollectionState == null )
							{
								#region Returnlength
								var readLength = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readLength );
								#endregion ReturnLength
							}
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
								this._scalarKind = ScalarKind.ArrayLength16;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Array();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.ArrayLength16 ) & 0xF );
								this._readByteLength += scalarReadBytes;
								if( scalarReadBytes < ( ( ( int )ScalarKind.ArrayLength16 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
									
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
									this._currentCollectionState.SetItemsCount( length );
									if( length == 0 )
									{
										if( this._collectionStates.Count == 1 )
										{
											// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
											this._collectionStates.Pop();
											this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											#region Returnlength
											var readLength = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readLength );
											#endregion ReturnLength
										}
									}
								}
								#endregion StartScalarUnpacking
								
								break;
							}
							case 0xDD: // array32
							{
								
								#region StartScalarUnpacking
								this._scalarKind = ScalarKind.ArrayLength32;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Array();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.ArrayLength32 ) & 0xF );
								this._readByteLength += scalarReadBytes;
								if( scalarReadBytes < ( ( ( int )ScalarKind.ArrayLength32 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
									
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
									this._currentCollectionState.SetItemsCount( length );
									if( length == 0 )
									{
										if( this._collectionStates.Count == 1 )
										{
											// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
											this._collectionStates.Pop();
											this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											#region Returnlength
											var readLength = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readLength );
											#endregion ReturnLength
										}
									}
								}
								#endregion StartScalarUnpacking
								
								break;
							}
							case 0xDE: // map16
							{
								
								#region StartScalarUnpacking
								this._scalarKind = ScalarKind.MapLength16;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Map();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.MapLength16 ) & 0xF );
								this._readByteLength += scalarReadBytes;
								if( scalarReadBytes < ( ( ( int )ScalarKind.MapLength16 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
									
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
									this._currentCollectionState.SetItemsCount( length );
									if( length == 0 )
									{
										if( this._collectionStates.Count == 1 )
										{
											// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
											this._collectionStates.Pop();
											this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											#region Returnlength
											var readLength = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readLength );
											#endregion ReturnLength
										}
									}
								}
								#endregion StartScalarUnpacking
								
								break;
							}
							case 0xDF: // map32
							{
								
								#region StartScalarUnpacking
								this._scalarKind = ScalarKind.MapLength32;
								this._scalarBufferPosition = 0;
								
								#region PushContextCollection
								var newCollectionState = CollectionUnpackingState.Map();
								this._currentCollectionState = newCollectionState;
								this._collectionStates.Push( newCollectionState );
								#endregion PushContextCollection
								
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )ScalarKind.MapLength32 ) & 0xF );
								this._readByteLength += scalarReadBytes;
								if( scalarReadBytes < ( ( ( int )ScalarKind.MapLength32 ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
									
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
									this._currentCollectionState.SetItemsCount( length );
									if( length == 0 )
									{
										if( this._collectionStates.Count == 1 )
										{
											// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
											this._collectionStates.Pop();
											this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											#region Returnlength
											var readLength = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readLength );
											#endregion ReturnLength
										}
									}
								}
								#endregion StartScalarUnpacking
								
								break;
							}
							default:
							{
								
								#region StartScalarUnpacking
								this._scalarKind = _scalarKinds[ b - 0xC0 ];
								this._scalarBufferPosition = 0;
								var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF );
								this._readByteLength += scalarReadBytes;
								if( scalarReadBytes < ( ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF ) )
								{
									// Must wait extra bytes, book keep states.
									this._scalarBufferPosition += scalarReadBytes;
									return null;
								}
								else
								{
									// Whole scalar bytes are read.
								
								#region ClearScalarBuffer
								this._scalarBufferPosition = -1;
								#endregion ClearScalarBuffer
								
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
											this._remainingBlob = length;
											long blobReadBytes = 0;
											int recentRead;
											int reading;
											do
											{
												reading  = _dummyBuffer.Length < length ? _dummyBuffer.Length : unchecked( ( int )length );
												recentRead = source.Read( _dummyBuffer, 0, reading );
												blobReadBytes += recentRead;
											} while ( recentRead == reading && blobReadBytes < length);
											
											this._readByteLength += blobReadBytes;
											if( blobReadBytes < this._remainingBlob )
											{
												// Must wait extra bytes, book keep states.
												this._remainingBlob -= blobReadBytes;
											
												return null;
											}
											else
											{
												// Whole blob bytes are read.
												// Reset buffer.
												this._remainingBlob = -1;
												
												#region TryPopContextCollection
												while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
												}
												#endregion TryPopContextCollection
												
												if( this._currentCollectionState == null )
												{
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											#endregion StartBlobUnpacking
											
											break;
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
											
											#region StartBlobUnpacking
											this._remainingBlob = length;
											long blobReadBytes = 0;
											int recentRead;
											int reading;
											do
											{
												reading  = _dummyBuffer.Length < length ? _dummyBuffer.Length : unchecked( ( int )length );
												recentRead = source.Read( _dummyBuffer, 0, reading );
												blobReadBytes += recentRead;
											} while ( recentRead == reading && blobReadBytes < length);
											
											this._readByteLength += blobReadBytes;
											if( blobReadBytes < this._remainingBlob )
											{
												// Must wait extra bytes, book keep states.
												this._remainingBlob -= blobReadBytes;
											
												return null;
											}
											else
											{
												// Whole blob bytes are read.
												// Reset buffer.
												this._remainingBlob = -1;
												
												#region TryPopContextCollection
												while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
												}
												#endregion TryPopContextCollection
												
												if( this._currentCollectionState == null )
												{
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											#endregion StartBlobUnpacking
											
											break;
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
											this._currentCollectionState.SetItemsCount( length );
											if( length == 0 )
											{
												if( this._collectionStates.Count == 1 )
												{
													// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											break;
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
											this._currentCollectionState.SetItemsCount( length );
											if( length == 0 )
											{
												if( this._collectionStates.Count == 1 )
												{
													// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											break;
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
											this._currentCollectionState.SetItemsCount( length );
											if( length == 0 )
											{
												if( this._collectionStates.Count == 1 )
												{
													// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											break;
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
											this._currentCollectionState.SetItemsCount( length );
											if( length == 0 )
											{
												if( this._collectionStates.Count == 1 )
												{
													// Top of stack must be empty which is pushed just now, so we can return tiny length safely.
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											break;
										}
										case ScalarKind.Float32:
										case ScalarKind.Float64:
										case ScalarKind.Int8:
										case ScalarKind.UInt8:
										case ScalarKind.Int16:
										case ScalarKind.UInt16:
										case ScalarKind.Int32:
										case ScalarKind.UInt32:
										case ScalarKind.Int64:
										case ScalarKind.UInt64:
										{
											
											#region ClearScalarBuffer
											this._scalarBufferPosition = -1;
											#endregion ClearScalarBuffer
											
											if( this._currentCollectionState == null )
											{
												#region Returnlength
												var readLength = this._readByteLength;
												this._readByteLength = 0;
												return new MessagePackObject( readLength );
												#endregion ReturnLength
											}
											
											#region TryPopContextCollection
											while( this._currentCollectionState != null && this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
												if( this._currentCollectionState == null )
												{
													#region Returnlength
													var readLength = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readLength );
													#endregion ReturnLength
												}
											}
											#endregion TryPopContextCollection
											
											break;
										}
									} // switch _scalarKinds[ b - 0xC0 ] 
								}
								#endregion StartScalarUnpacking
								
								break;
							}
						}
					}
					
				} // for 
	
				// stream ends
				return null;
			}
		}
	}
