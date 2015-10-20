 
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

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
			var source = this._source;
			var buffer = this._scalarBuffer;
#if !UNITY
			Contract.Assert( source != null, "source != null" );
			Contract.Assert( buffer != null, "buffer != null" );
#endif // !UNITY

			long remainingItems = -1;
			long skipped = 0;
			Int64Stack remainingCollections = null;
			do
			{
				var header = source.ReadByte();
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
						skipped += 1;
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
					skipped += 1;
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
					skipped += 1;
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
						skipped += 1;
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
						skipped += 1;
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
						skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
						#endregion DrainValue
						continue;
					}
					case MessagePackCode.SignedInt16:
					case MessagePackCode.UnsignedInt16:
					{
						skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
						#endregion DrainValue
						continue;
					}
					case MessagePackCode.SignedInt32:
					case MessagePackCode.UnsignedInt32:
					case MessagePackCode.Real32:
					{
						skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
						#endregion DrainValue
						continue;
					}
					case MessagePackCode.SignedInt64:
					case MessagePackCode.UnsignedInt64:
					case MessagePackCode.Real64:
					{
						skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
						#endregion DrainValue
						continue;
					}
					case MessagePackCode.Str8:
					case MessagePackCode.Bin8:
					{
						skipped += 1;
						byte length;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							length = buffer[0];
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						ushort length;
						var read = source.Read( buffer, 0, 2 );
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( buffer, 0 );
							skipped += 2;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						uint length;
						var read = source.Read( buffer, 0, 4 );
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( buffer, 0 );
							skipped += 4;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						ushort length;
						var read = source.Read( buffer, 0, 2 );
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( buffer, 0 );
							skipped += 2;
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
						skipped += 1;
						uint length;
						var read = source.Read( buffer, 0, 4 );
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( buffer, 0 );
							skipped += 4;
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
						skipped += 1;
						ushort length;
						var read = source.Read( buffer, 0, 2 );
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( buffer, 0 );
							skipped += 2;
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
						skipped += 1;
						uint length;
						var read = source.Read( buffer, 0, 4 );
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( buffer, 0 );
							skipped += 4;
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
						skipped += 1;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						byte length;
						var read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							length = buffer[0];
							skipped += 1;
						}
						else
						{
							return null;
						}
						 read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						ushort length;
						var read = source.Read( buffer, 0, 2 );
						if ( read == 2 )
						{
							length = BigEndianBinary.ToUInt16( buffer, 0 );
							skipped += 2;
						}
						else
						{
							return null;
						}
						 read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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
						skipped += 1;
						uint length;
						var read = source.Read( buffer, 0, 4 );
						if ( read == 4 )
						{
							length = BigEndianBinary.ToUInt32( buffer, 0 );
							skipped += 4;
						}
						else
						{
							return null;
						}
						 read = source.Read( buffer, 0, 1 );
						if ( read == 1 )
						{
							skipped += 1;
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
							bytesRead += source.Read( dummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
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
						
						skipped += bytesRead;
						
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

			return skipped;
		}
	}
}
