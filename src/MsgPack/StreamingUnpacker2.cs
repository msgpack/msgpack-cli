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
	// This file generated from StreamingUnpacker2.Nullable.tt T4Template.
	// Do not modify this file. Edit StreamingUnpacker2.Nullable.tt instead.


	internal sealed partial class StreamingUnpacker2
	{
		public MessagePackObject? Unpack( Stream source, UnpackingMode unpackingMode )
		{
			// Continue 
			if( this._scalarBufferPosition >= 0 )
			{
				
				#region ContinueScalarUnpacking
				var scalarReadBytes = source.Read( this._scalarBuffer, this._scalarBufferPosition, ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition );
				if( scalarReadBytes > 0 )
				{
					this._readByteLength += scalarReadBytes;
					
					#region TryPopContextCollection
					if( this._currentCollectionState != null )
					{
						if( this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
					
							if( unpackingMode == UnpackingMode.SkipSubtree )
							{
								var readByteLenth = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readByteLength );
							}
						}
					}
					#endregion TryPopContextCollection
					
				}
				if( scalarReadBytes < ( ( ( ( int )this._scalarKind ) & 0xF ) - this._scalarBufferPosition ) )
				{
					// Must wait extra bytes, book keep states.
					this._scalarBufferPosition += scalarReadBytes;
					return null;
				}
				else
				{
					// Whole blob bytes are read.
					// Reset buffer.
					this._scalarBufferPosition = 0;
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
							var blobLength = length;
							var blob = new byte[ blobLength ];
							
							var blobReadBytes = source.Read( blob, 0, blobLength );
							
							
							if( blobReadBytes < blobLength )
							{
								// Must wait extra bytes, book keep states.
								this._blobBuffer = blob;
								this._blobBufferPosition += blobReadBytes;
							
								return null;
							}
							else
							{
								// Whole blob bytes are read.
								
								#region TryPopContextCollection
								if( this._currentCollectionState != null )
								{
									if( this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								
										if( unpackingMode == UnpackingMode.SkipSubtree )
										{
											var readByteLenth = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readByteLength );
										}
									}
								}
								#endregion TryPopContextCollection
								
								if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
								{
									return new MessagePackObject( blob );
								}
								
								// retry loop.
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
							
							#region Ensure32Bytes
							if( length > Int32.MaxValue )
							{
								throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
							}
							#endregion Ensure32Bytes
							
							
							#region StartBlobUnpacking
							var blobLength = unchecked( ( int )length );
							var blob = new byte[ blobLength ];
							
							var blobReadBytes = source.Read( blob, 0, blobLength );
							
							
							if( blobReadBytes < blobLength )
							{
								// Must wait extra bytes, book keep states.
								this._blobBuffer = blob;
								this._blobBufferPosition += blobReadBytes;
							
								return null;
							}
							else
							{
								// Whole blob bytes are read.
								
								#region TryPopContextCollection
								if( this._currentCollectionState != null )
								{
									if( this._currentCollectionState.IncrementUnpacked() )
									{
										this._collectionStates.Pop();
										this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
								
										if( unpackingMode == UnpackingMode.SkipSubtree )
										{
											var readByteLenth = this._readByteLength;
											this._readByteLength = 0;
											return new MessagePackObject( readByteLength );
										}
									}
								}
								#endregion TryPopContextCollection
								
								if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
								{
									return new MessagePackObject( blob );
								}
								
								// retry loop.
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
							
							ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							this._currentCollectionState.SetItemsCount( length );
							this._collectionHeaderKind = CollectionHeaderKind.Array;
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( this._currentCollectionState.ItemsCount );
							}
							
							// retry loop.
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
							
							ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							this._currentCollectionState.SetItemsCount( length );
							this._collectionHeaderKind = CollectionHeaderKind.Map;
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( this._currentCollectionState.ItemsCount );
							}
							
							// retry loop.
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
							
							uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							
							#region Ensure32Bytes
							if( length > Int32.MaxValue )
							{
								throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
							}
							#endregion Ensure32Bytes
							
							this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
							this._collectionHeaderKind = CollectionHeaderKind.Array;
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( this._currentCollectionState.ItemsCount );
							}
							
							// retry loop.
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
							
							uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							
							#region Ensure32Bytes
							if( length > Int32.MaxValue )
							{
								throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
							}
							#endregion Ensure32Bytes
							
							this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
							this._collectionHeaderKind = CollectionHeaderKind.Map;
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( this._currentCollectionState.ItemsCount );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
						}
						case ScalarKind.Int8:
						{
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
						}
						case ScalarKind.UInt8:
						{
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = this._scalarBuffer[ 0 ];
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
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
							
							
							#region TryPopContextCollection
							if( this._currentCollectionState != null )
							{
								if( this._currentCollectionState.IncrementUnpacked() )
								{
									this._collectionStates.Pop();
									this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
							
									if( unpackingMode == UnpackingMode.SkipSubtree )
									{
										var readByteLenth = this._readByteLength;
										this._readByteLength = 0;
										return new MessagePackObject( readByteLength );
									}
								}
							}
							#endregion TryPopContextCollection
							
							var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
							
							#region ClearScalarBuffer
							this._scalarBufferPosition = 0;
							#endregion ClearScalarBuffer
							
							if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
							{
								return new MessagePackObject( value );
							}
							
							// retry loop.
							break;
						}
					} // switch this._scalarKind 
				}
				#endregion ContinueScalarUnpacking
				
			}
			else if(  this._blobBuffer != null )
			{
				
				#region ContinueBlobUnpacking
				var blobLength = this._blobBuffer.Length - this._blobBufferPosition;
				var blob = this._blobBuffer;
				
				var blobReadBytes = source.Read( blob, this._blobBufferPosition, blobLength );
				
				if( blobReadBytes > 0 )
				{
					this._readByteLength += blobReadBytes;
					
					#region TryPopContextCollection
					if( this._currentCollectionState != null )
					{
						if( this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
					
							if( unpackingMode == UnpackingMode.SkipSubtree )
							{
								var readByteLenth = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readByteLength );
							}
						}
					}
					#endregion TryPopContextCollection
					
				}
				
				if( blobReadBytes < blobLength )
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
					if( this._currentCollectionState != null )
					{
						if( this._currentCollectionState.IncrementUnpacked() )
						{
							this._collectionStates.Pop();
							this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
					
							if( unpackingMode == UnpackingMode.SkipSubtree )
							{
								var readByteLenth = this._readByteLength;
								this._readByteLength = 0;
								return new MessagePackObject( readByteLength );
							}
						}
					}
					#endregion TryPopContextCollection
					
					if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
					{
						return new MessagePackObject( blob );
					}
					
					// retry loop.
				}
				#endregion ContinueBlobUnpacking
				
			}

			// Header or scalar
			for( var b = source.ReadByte(); b >= 0; b = source.ReadByte() )
			{
				
				#region TryPopContextCollection
				if( this._currentCollectionState != null )
				{
					if( this._currentCollectionState.IncrementUnpacked() )
					{
						this._collectionStates.Pop();
						this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
				
						if( unpackingMode == UnpackingMode.SkipSubtree )
						{
							var readByteLenth = this._readByteLength;
							this._readByteLength = 0;
							return new MessagePackObject( readByteLength );
						}
					}
				}
				#endregion TryPopContextCollection
				
				this._readByteLength ++;

				if ( b < 0x80 )
				{
					this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
					if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
					{
						return _positiveIntegers[ b ];
					}
					
					// retry loop.
				}

				if ( b >= 0xE0 )
				{
					this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
					if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
					{
						return _negativeIntegers[ b - 0xE0 ];
					}
					
					// retry loop.
				}

				switch ( b )
				{
					case 0x80:
					{
						this._collectionHeaderKind = CollectionHeaderKind.Map;
						
						#region PushEmptyContextCollectionIfIsNotInRoot
						if( this._currentCollectionState != null )
						{
							var newCollectionState = CollectionUnpackingState.FixedMap( 0 );
							this._collectionHeaderKind = CollectionHeaderKind.Map;
							this._currentCollectionState = newCollectionState;
							this._collectionStates.Push( newCollectionState );
						}
						#endregion PushEmptyContextCollectionIfIsNotInRoot
						
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return _emptyMap;
						}
						
						// retry loop.
						break;
					}
					case 0x90:
					{
						this._collectionHeaderKind = CollectionHeaderKind.Array;
						
						#region PushEmptyContextCollectionIfIsNotInRoot
						if( this._currentCollectionState != null )
						{
							var newCollectionState = CollectionUnpackingState.FixedArray( 0 );
							this._collectionHeaderKind = CollectionHeaderKind.Array;
							this._currentCollectionState = newCollectionState;
							this._collectionStates.Push( newCollectionState );
						}
						#endregion PushEmptyContextCollectionIfIsNotInRoot
						
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return _emptyArray;
						}
						
						// retry loop.
						break;
					}
					case 0xA0:
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return _emptyRaw;
						}
						
						// retry loop.
						break;
					}
					case MessagePackCode.TrueValue:
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return _true;
						}
						
						// retry loop.
						break;
					}
					case MessagePackCode.FalseValue:
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return _false;
						}
						
						// retry loop.
						break;
					}
					case MessagePackCode.NilValue:
					{
						this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return MessagePackObject.Nil;
						}
						
						// retry loop.
						break;
					}
				}

				if ( b < 0x90 )
				{
					// map
					
					#region PushFixedContextCollection
					var newCollectionState = CollectionUnpackingState.FixedMap( ( b & 0xF ) );
					this._collectionHeaderKind = CollectionHeaderKind.Map;
					this._currentCollectionState = newCollectionState;
					this._collectionStates.Push( newCollectionState );
					#endregion PushFixedContextCollection
					
					if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
					{
						return new MessagePackObject( ( b & 0xF ) );
					}
					
					// retry loop.
				}
				else if ( b < 0xA0 )
				{
					// array
					
					#region PushFixedContextCollection
					var newCollectionState = CollectionUnpackingState.FixedArray( ( b & 0xF ) );
					this._collectionHeaderKind = CollectionHeaderKind.Array;
					this._currentCollectionState = newCollectionState;
					this._collectionStates.Push( newCollectionState );
					#endregion PushFixedContextCollection
					
					if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
					{
						return new MessagePackObject( ( b & 0xF ) );
					}
					
					// retry loop.
				}
				else if ( b < 0xC0 )
				{
					// raw
					this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
					
					#region StartBlobUnpacking
					var blobLength = b - 0xA0;
					var blob = new byte[ blobLength ];
					
					var blobReadBytes = source.Read( blob, 0, blobLength );
					
					
					if( blobReadBytes < blobLength )
					{
						// Must wait extra bytes, book keep states.
						this._blobBuffer = blob;
						this._blobBufferPosition += blobReadBytes;
					
						return null;
					}
					else
					{
						// Whole blob bytes are read.
						
						#region TryPopContextCollection
						if( this._currentCollectionState != null )
						{
							if( this._currentCollectionState.IncrementUnpacked() )
							{
								this._collectionStates.Pop();
								this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
						
								if( unpackingMode == UnpackingMode.SkipSubtree )
								{
									var readByteLenth = this._readByteLength;
									this._readByteLength = 0;
									return new MessagePackObject( readByteLength );
								}
							}
						}
						#endregion TryPopContextCollection
						
						if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
						{
							return new MessagePackObject( blob );
						}
						
						// retry loop.
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
							
							#region PushContextCollection
							var newCollectionState = CollectionUnpackingState.Array();
							this._collectionHeaderKind = CollectionHeaderKind.Array;
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
								// Whole blob bytes are read.
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
										var blobLength = length;
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										
										#region StartBlobUnpacking
										var blobLength = unchecked( ( int )length );
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.Int8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.UInt8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = this._scalarBuffer[ 0 ];
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
								} // switch ScalarKind.ArrayLength16 
							}
							#endregion StartScalarUnpacking
							
							break;
						}
						case 0xDD: // array32
						{
							
							#region StartScalarUnpacking
							this._scalarKind = ScalarKind.ArrayLength32;
							
							#region PushContextCollection
							var newCollectionState = CollectionUnpackingState.Array();
							this._collectionHeaderKind = CollectionHeaderKind.Array;
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
								// Whole blob bytes are read.
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
										var blobLength = length;
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										
										#region StartBlobUnpacking
										var blobLength = unchecked( ( int )length );
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.Int8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.UInt8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = this._scalarBuffer[ 0 ];
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
								} // switch ScalarKind.ArrayLength32 
							}
							#endregion StartScalarUnpacking
							
							break;
						}
						case 0xDE: // map16
						{
							
							#region StartScalarUnpacking
							this._scalarKind = ScalarKind.MapLength16;
							
							#region PushContextCollection
							var newCollectionState = CollectionUnpackingState.Map();
							this._collectionHeaderKind = CollectionHeaderKind.Map;
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
								// Whole blob bytes are read.
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
										var blobLength = length;
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										
										#region StartBlobUnpacking
										var blobLength = unchecked( ( int )length );
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.Int8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.UInt8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = this._scalarBuffer[ 0 ];
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
								} // switch ScalarKind.MapLength16 
							}
							#endregion StartScalarUnpacking
							
							break;
						}
						case 0xDF: // map32
						{
							
							#region StartScalarUnpacking
							this._scalarKind = ScalarKind.MapLength32;
							
							#region PushContextCollection
							var newCollectionState = CollectionUnpackingState.Map();
							this._collectionHeaderKind = CollectionHeaderKind.Map;
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
								// Whole blob bytes are read.
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
										var blobLength = length;
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										
										#region StartBlobUnpacking
										var blobLength = unchecked( ( int )length );
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.Int8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.UInt8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = this._scalarBuffer[ 0 ];
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
								} // switch ScalarKind.MapLength32 
							}
							#endregion StartScalarUnpacking
							
							break;
						}
						default:
						{
							this._collectionHeaderKind = CollectionHeaderKind.NotCollection;
							
							#region StartScalarUnpacking
							this._scalarKind = _scalarKinds[ b - 0xC0 ];
							var scalarReadBytes = source.Read( this._scalarBuffer, 0, ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF );
							if( scalarReadBytes < ( ( ( int )_scalarKinds[ b - 0xC0 ] ) & 0xF ) )
							{
								// Must wait extra bytes, book keep states.
								this._scalarBufferPosition += scalarReadBytes;
								return null;
							}
							else
							{
								// Whole blob bytes are read.
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
										var blobLength = length;
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										
										#region StartBlobUnpacking
										var blobLength = unchecked( ( int )length );
										var blob = new byte[ blobLength ];
										
										var blobReadBytes = source.Read( blob, 0, blobLength );
										
										
										if( blobReadBytes < blobLength )
										{
											// Must wait extra bytes, book keep states.
											this._blobBuffer = blob;
											this._blobBufferPosition += blobReadBytes;
										
											return null;
										}
										else
										{
											// Whole blob bytes are read.
											
											#region TryPopContextCollection
											if( this._currentCollectionState != null )
											{
												if( this._currentCollectionState.IncrementUnpacked() )
												{
													this._collectionStates.Pop();
													this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
											
													if( unpackingMode == UnpackingMode.SkipSubtree )
													{
														var readByteLenth = this._readByteLength;
														this._readByteLength = 0;
														return new MessagePackObject( readByteLength );
													}
												}
											}
											#endregion TryPopContextCollection
											
											if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
											{
												return new MessagePackObject( blob );
											}
											
											// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										ushort length = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										this._currentCollectionState.SetItemsCount( length );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Array;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
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
										
										uint length = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										
										#region Ensure32Bytes
										if( length > Int32.MaxValue )
										{
											throw new NotSupportedException( "MessagePack for CLI cannot handle a collection which stores more than Int32.MaxValue items." );
										}
										#endregion Ensure32Bytes
										
										this._currentCollectionState.SetItemsCount( unchecked( ( int )length ) );
										this._collectionHeaderKind = CollectionHeaderKind.Map;
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( this._currentCollectionState.ItemsCount );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToSingle( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToDouble( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.Int8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = unchecked( ( sbyte )this._scalarBuffer[ 0 ] );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
									}
									case ScalarKind.UInt8:
									{
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = this._scalarBuffer[ 0 ];
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt16( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt32( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
										break;
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
										
										
										#region TryPopContextCollection
										if( this._currentCollectionState != null )
										{
											if( this._currentCollectionState.IncrementUnpacked() )
											{
												this._collectionStates.Pop();
												this._currentCollectionState = this._collectionStates.Count == 0 ? null : this._collectionStates.Peek();
										
												if( unpackingMode == UnpackingMode.SkipSubtree )
												{
													var readByteLenth = this._readByteLength;
													this._readByteLength = 0;
													return new MessagePackObject( readByteLength );
												}
											}
										}
										#endregion TryPopContextCollection
										
										var value = BitConverter.ToUInt64( this._scalarBuffer, 0 );
										
										#region ClearScalarBuffer
										this._scalarBufferPosition = 0;
										#endregion ClearScalarBuffer
										
										if( ( this._currentCollectionState == null && this._collectionHeaderKind == CollectionHeaderKind.NotCollection ) || unpackingMode == UnpackingMode.PerEntry )
										{
											return new MessagePackObject( value );
										}
										
										// retry loop.
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
