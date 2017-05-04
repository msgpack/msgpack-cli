#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

#if !UNITY || MSGPACK_UNITY_FULL
using Int64Stack = System.Collections.Generic.Stack<System.Int64>;
#endif // !UNITY || MSGPACK_UNITY_FULL

namespace MsgPack
{
	// This file was generated from ItemsUnpacker.Skipping.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit ItemsUnpacker.Skipping.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class ItemsUnpacker
	{
		protected override long? SkipCore()
		{
			long remainingItems = -1;
			long startOffset = this._offset;
			Int64Stack remainingCollections = null;
			do
			{
				var header = this.ReadByteFromSource();
				if ( header < 0 )
				{
					return null;
				}

				switch ( header )
				{
					case MessagePackCode.NilValue:
					case MessagePackCode.TrueValue:
					case MessagePackCode.FalseValue:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
				}

				if ( header < 0x80 )
				{
					#region TryPopContextCollection
					
					remainingItems--;
					
					if( remainingCollections != null )
					{
						while ( remainingItems == 0 && remainingCollections.Count > 0 )
						{
							if( remainingCollections.Count == 0 )
							{
								break;
							}
					
							remainingItems = remainingCollections.Pop();
							remainingItems--;
						}
					}
					
					#endregion TryPopContextCollection
					continue;
				}
				else if ( header >= 0xE0 )
				{
					#region TryPopContextCollection
					
					remainingItems--;
					
					if( remainingCollections != null )
					{
						while ( remainingItems == 0 && remainingCollections.Count > 0 )
						{
							if( remainingCollections.Count == 0 )
							{
								break;
							}
					
							remainingItems = remainingCollections.Pop();
							remainingItems--;
						}
					}
					
					#endregion TryPopContextCollection
					continue;
				}

				switch ( header & 0xF0 )
				{
					case 0x80:
					{
						var size = header & 0xF;
						if( size == 0 )
						{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						}
						else
						{
						// <WritePushCollection sizeVariable="size * 2">
						#region PushContextCollection
						
						if( remainingItems >= 0 )
						{
							if( remainingCollections == null )
							{
								remainingCollections = new Int64Stack( 4 );
							}
							
							remainingCollections.Push( remainingItems );
						}
						
						remainingItems = size * 2;
						
						#endregion PushContextCollection
						// </WritePushCollection>
						}

						continue;
					}
					case 0x90:
					{
						var size = header & 0xF;
						if( size == 0 )
						{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						}
						else
						{
						// <WritePushCollection sizeVariable="size">
						#region PushContextCollection
						
						if( remainingItems >= 0 )
						{
							if( remainingCollections == null )
							{
								remainingCollections = new Int64Stack( 4 );
							}
							
							remainingCollections.Push( remainingItems );
						}
						
						remainingItems = size;
						
						#endregion PushContextCollection
						// </WritePushCollection>
						}

						continue;
					}
					case 0xA0:
					case 0xB0:
					{
						var size = header & 0x1F;
						// <WriteDrainValue sizeVariable="size" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( size, Int32.MaxValue ) ) );
						while( size > bytesRead )
						{
							var remaining = ( size - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
				}

				switch ( header )
				{
					case MessagePackCode.SignedInt8:
					case MessagePackCode.UnsignedInt8:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="1" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 1, Int32.MaxValue ) ) );
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt16:
					case MessagePackCode.UnsignedInt16:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="2" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 2, Int32.MaxValue ) ) );
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt32:
					case MessagePackCode.UnsignedInt32:
					case MessagePackCode.Real32:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="4" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 4, Int32.MaxValue ) ) );
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt64:
					case MessagePackCode.UnsignedInt64:
					case MessagePackCode.Real64:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="8" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 8, Int32.MaxValue ) ) );
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.Str8:
					case MessagePackCode.Bin8:
					{
						byte length;
						// <WriteUnpackByteCore lengthVariable="length" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
							length = this._scalarBuffer[0];
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Bin16:
					case MessagePackCode.Raw16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 2 );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Bin32:
					case MessagePackCode.Raw32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 4 );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Array16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 2 );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Array32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 4 );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Map16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 2 );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length * 2">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length * 2;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Map32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 4 );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length * 2">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length * 2;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.FixExt1:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="1" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 1, Int32.MaxValue ) ) );
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt2:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="2" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 2, Int32.MaxValue ) ) );
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt4:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="4" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 4, Int32.MaxValue ) ) );
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt8:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="8" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 8, Int32.MaxValue ) ) );
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt16:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="16" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 16, Int32.MaxValue ) ) );
						while( 16 > bytesRead )
						{
							var remaining = ( 16 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext8:
					{
						byte length;
						// <WriteUnpackByteCore lengthVariable="length" needsDeclaration="True" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
							length = this._scalarBuffer[0];
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="False">
						this._lastOffset = this._offset;
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 2 );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="False">
						this._lastOffset = this._offset;
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="False">
						this._lastOffset = this._offset;
						var read = this._source.Read( this._scalarBuffer, 0, 4 );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="False">
						this._lastOffset = this._offset;
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					default:
					{
						this.ThrowUnassignedMessageTypeException( header );
						return null; // Never reach
					}
				}
			} while ( remainingItems > 0 );

			return this._offset - startOffset;
		}

#if FEATURE_TAP
		protected override async Task<long?> SkipAsyncCore( CancellationToken cancellationToken )
		{
			long remainingItems = -1;
			long startOffset = this._offset;
			Int64Stack remainingCollections = null;
			do
			{
				var header = await this.ReadByteFromSourceAsync( cancellationToken ).ConfigureAwait( false );
				if ( header < 0 )
				{
					return null;
				}

				switch ( header )
				{
					case MessagePackCode.NilValue:
					case MessagePackCode.TrueValue:
					case MessagePackCode.FalseValue:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
				}

				if ( header < 0x80 )
				{
					#region TryPopContextCollection
					
					remainingItems--;
					
					if( remainingCollections != null )
					{
						while ( remainingItems == 0 && remainingCollections.Count > 0 )
						{
							if( remainingCollections.Count == 0 )
							{
								break;
							}
					
							remainingItems = remainingCollections.Pop();
							remainingItems--;
						}
					}
					
					#endregion TryPopContextCollection
					continue;
				}
				else if ( header >= 0xE0 )
				{
					#region TryPopContextCollection
					
					remainingItems--;
					
					if( remainingCollections != null )
					{
						while ( remainingItems == 0 && remainingCollections.Count > 0 )
						{
							if( remainingCollections.Count == 0 )
							{
								break;
							}
					
							remainingItems = remainingCollections.Pop();
							remainingItems--;
						}
					}
					
					#endregion TryPopContextCollection
					continue;
				}

				switch ( header & 0xF0 )
				{
					case 0x80:
					{
						var size = header & 0xF;
						if( size == 0 )
						{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						}
						else
						{
						// <WritePushCollection sizeVariable="size * 2">
						#region PushContextCollection
						
						if( remainingItems >= 0 )
						{
							if( remainingCollections == null )
							{
								remainingCollections = new Int64Stack( 4 );
							}
							
							remainingCollections.Push( remainingItems );
						}
						
						remainingItems = size * 2;
						
						#endregion PushContextCollection
						// </WritePushCollection>
						}

						continue;
					}
					case 0x90:
					{
						var size = header & 0xF;
						if( size == 0 )
						{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						}
						else
						{
						// <WritePushCollection sizeVariable="size">
						#region PushContextCollection
						
						if( remainingItems >= 0 )
						{
							if( remainingCollections == null )
							{
								remainingCollections = new Int64Stack( 4 );
							}
							
							remainingCollections.Push( remainingItems );
						}
						
						remainingItems = size;
						
						#endregion PushContextCollection
						// </WritePushCollection>
						}

						continue;
					}
					case 0xA0:
					case 0xB0:
					{
						var size = header & 0x1F;
						// <WriteDrainValue sizeVariable="size" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( size, Int32.MaxValue ) ) );
						while( size > bytesRead )
						{
							var remaining = ( size - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
				}

				switch ( header )
				{
					case MessagePackCode.SignedInt8:
					case MessagePackCode.UnsignedInt8:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="1" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 1, Int32.MaxValue ) ) );
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt16:
					case MessagePackCode.UnsignedInt16:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="2" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 2, Int32.MaxValue ) ) );
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt32:
					case MessagePackCode.UnsignedInt32:
					case MessagePackCode.Real32:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="4" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 4, Int32.MaxValue ) ) );
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.SignedInt64:
					case MessagePackCode.UnsignedInt64:
					case MessagePackCode.Real64:
					{
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						// <WriteDrainValue sizeVariable="8" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 8, Int32.MaxValue ) ) );
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						continue;
					}
					case MessagePackCode.Str8:
					case MessagePackCode.Bin8:
					{
						byte length;
						// <WriteUnpackByteCore lengthVariable="length" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
							length = this._scalarBuffer[0];
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Bin16:
					case MessagePackCode.Raw16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 2, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Bin32:
					case MessagePackCode.Raw32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 4, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Array16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 2, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Array32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 4, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Map16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 2, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length * 2">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length * 2;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.Map32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 4, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						if( length == 0 )
						{
							#region TryPopContextCollection
							
							remainingItems--;
							
							if( remainingCollections != null )
							{
								while ( remainingItems == 0 && remainingCollections.Count > 0 )
								{
									if( remainingCollections.Count == 0 )
									{
										break;
									}
							
									remainingItems = remainingCollections.Pop();
									remainingItems--;
								}
							}
							
							#endregion TryPopContextCollection
						}
						else
						{
							// <WritePushCollection sizeVariable="length * 2">
							#region PushContextCollection
							
							if( remainingItems >= 0 )
							{
								if( remainingCollections == null )
								{
									remainingCollections = new Int64Stack( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length * 2;
							
							#endregion PushContextCollection
							// </WritePushCollection>
						}

						continue;
					}
					case MessagePackCode.FixExt1:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="1" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 1, Int32.MaxValue ) ) );
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt2:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="2" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 2, Int32.MaxValue ) ) );
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt4:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="4" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 4, Int32.MaxValue ) ) );
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt8:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="8" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 8, Int32.MaxValue ) ) );
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.FixExt16:
					{
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="16" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( 16, Int32.MaxValue ) ) );
						while( 16 > bytesRead )
						{
							var remaining = ( 16 - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext8:
					{
						byte length;
						// <WriteUnpackByteCore lengthVariable="length" needsDeclaration="True" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
							length = this._scalarBuffer[0];
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="True">
						this._lastOffset = this._offset;
						 read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext16:
					{
						ushort length;
						// <WriteUnpackLength size="2" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 2, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="True">
						this._lastOffset = this._offset;
						 read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					case MessagePackCode.Ext32:
					{
						uint length;
						// <WriteUnpackLength size="4" lengthVariable="length" isAsync="True">
						this._lastOffset = this._offset;
						var read = await this._source.ReadAsync( this._scalarBuffer, 0, 4, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( this._scalarBuffer, 0 );
						}
						else
						{
							return null;
						}
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="True">
						this._lastOffset = this._offset;
						 read = await this._source.ReadAsync( this._scalarBuffer, 0, 1, cancellationToken ).ConfigureAwait( false );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						long bytesRead = 0;
						var dummyBufferForSkipping = BufferManager.NewByteBuffer( unchecked( ( int )Math.Min( length, Int32.MaxValue ) ) );
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							this._lastOffset = this._offset;
							var lastRead = await this._source.ReadAsync( dummyBufferForSkipping, 0, reading, cancellationToken ).ConfigureAwait( false );
							this._offset += lastRead;
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								return null;
							}
						}
						
						#endregion DrainValue
						// </WriteDrainValue>
						#region TryPopContextCollection
						
						remainingItems--;
						
						if( remainingCollections != null )
						{
							while ( remainingItems == 0 && remainingCollections.Count > 0 )
							{
								if( remainingCollections.Count == 0 )
								{
									break;
								}
						
								remainingItems = remainingCollections.Pop();
								remainingItems--;
							}
						}
						
						#endregion TryPopContextCollection
						continue;
					}
					default:
					{
						this.ThrowUnassignedMessageTypeException( header );
						return null; // Never reach
					}
				}
			} while ( remainingItems > 0 );

			return this._offset - startOffset;
		}

#endif // FEATURE_TAP
	}
}
