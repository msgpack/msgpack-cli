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
						}

						continue;
					}
					case 0xA0:
					case 0xB0:
					{
						var size = header & 0x1F;
						#region DrainValue
						
						long bytesRead = 0;
						while( size > bytesRead )
						{
							var remaining = ( size - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
						continue;
					}
					case MessagePackCode.Str8:
					case MessagePackCode.Bin8:
					{
						byte length;
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
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						}

						continue;
					}
					case MessagePackCode.Array32:
					{
						uint length;
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
						}

						continue;
					}
					case MessagePackCode.Map16:
					{
						ushort length;
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
						}

						continue;
					}
					case MessagePackCode.Map32:
					{
						uint length;
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
						}

						continue;
					}
					case MessagePackCode.FixExt1:
					{
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( 1 > bytesRead )
						{
							var remaining = ( 1 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( 2 > bytesRead )
						{
							var remaining = ( 2 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( 4 > bytesRead )
						{
							var remaining = ( 4 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( 8 > bytesRead )
						{
							var remaining = ( 8 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						var read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( 16 > bytesRead )
						{
							var remaining = ( 16 - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						 read = this._source.Read( this._scalarBuffer, 0, 1 );
						this._offset += read;
						if ( read == 1 )
						{
						}
						else
						{
							return null;
						}
						#region DrainValue
						
						long bytesRead = 0;
						while( length > bytesRead )
						{
							var remaining = ( length - bytesRead );
							var dummyBufferForSkipping = BufferManager.GetByteBuffer();
						#if DEBUG
							try
							{
						#endif // DEBUG
							var reading = remaining > dummyBufferForSkipping.Length ? dummyBufferForSkipping.Length : unchecked( ( int )remaining );
							var lastRead = this._source.Read( dummyBufferForSkipping, 0, reading );
							bytesRead += lastRead;
							if ( lastRead == 0 )
							{
								this._offset += bytesRead;
								return null;
							}
						#if DEBUG
							}
							finally
							{
								BufferManager.ReleaseByteBuffer();
							}
						#endif // DEBUG
						}
						
						this._offset += bytesRead;
						
						#endregion DrainValue
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
						ThrowUnassingedMessageTypeException( header );
						return null; // Never reach
					}
				}
			} while ( remainingItems > 0 );

			return this._offset - startOffset;
		}
	}
}
