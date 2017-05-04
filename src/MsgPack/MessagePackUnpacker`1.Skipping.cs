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
	// This file was generated from MessagePackUnpacker`1.Skipping.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackUnpacker`1.Skipping.tt and Core.ttinclude instead.

	partial class MessagePackUnpacker<TReader>
	{
		public override long? Skip()
		{
			long remainingItems = -1;
			long startOffset = this.Reader.Offset;
			Int64Stack remainingCollections = null;
			do
			{
				var header = this.Reader.TryReadByte();
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
						var size = unchecked( ( uint )( header & 0x1F ) );
						// <WriteDrainValue sizeVariable="size" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( size ) )
						{
							return null;
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
						
						
						if ( !this.Reader.Drain( 1 ) )
						{
							return null;
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
						
						
						if ( !this.Reader.Drain( 2 ) )
						{
							return null;
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
						
						
						if ( !this.Reader.Drain( 4 ) )
						{
							return null;
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
						
						
						if ( !this.Reader.Drain( 8 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
							length = unchecked( ( byte )read );
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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
						
						var lengthMayFail = this.Reader.TryReadUInt16();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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
						
						var lengthMayFail = this.Reader.TryReadUInt32();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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
						
						var lengthMayFail = this.Reader.TryReadUInt16();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
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
						
						var lengthMayFail = this.Reader.TryReadUInt32();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
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
						
						var lengthMayFail = this.Reader.TryReadUInt16();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
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
						
						var lengthMayFail = this.Reader.TryReadUInt32();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="1" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( 1 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="2" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( 2 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="4" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( 4 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="8" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( 8 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="16" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( 16 ) )
						{
							return null;
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
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
							length = unchecked( ( byte )read );
						}
						// </WriteUnpackByteCore>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="False">
						read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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
						
						var lengthMayFail = this.Reader.TryReadUInt16();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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
						
						var lengthMayFail = this.Reader.TryReadUInt32();
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="False">
						var read = this.Reader.TryReadByte();
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="False">
						#region DrainValue
						
						
						if ( !this.Reader.Drain( length ) )
						{
							return null;
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

			return this.Reader.Offset - startOffset;
		}

#if FEATURE_TAP
		public override async Task<long?> SkipAsync( CancellationToken cancellationToken )
		{
			long remainingItems = -1;
			long startOffset = this.Reader.Offset;
			Int64Stack remainingCollections = null;
			do
			{
				var header = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
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
						var size = unchecked( ( uint )( header & 0x1F ) );
						// <WriteDrainValue sizeVariable="size" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( size, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						
						if ( !await this.Reader.DrainAsync( 1, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						
						if ( !await this.Reader.DrainAsync( 2, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						
						if ( !await this.Reader.DrainAsync( 4, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						
						if ( !await this.Reader.DrainAsync( 8, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
							length = unchecked( ( byte )read );
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						var lengthMayFail = await this.Reader.TryReadUInt16Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						var lengthMayFail = await this.Reader.TryReadUInt32Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						var lengthMayFail = await this.Reader.TryReadUInt16Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
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
						
						var lengthMayFail = await this.Reader.TryReadUInt32Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
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
						
						var lengthMayFail = await this.Reader.TryReadUInt16Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
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
						
						var lengthMayFail = await this.Reader.TryReadUInt32Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="1" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( 1, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="2" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( 2, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="4" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( 4, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="8" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( 8, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="16" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( 16, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
							length = unchecked( ( byte )read );
						}
						// </WriteUnpackByteCore>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="False" isAsync="True">
						read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						var lengthMayFail = await this.Reader.TryReadUInt16Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt16  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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
						
						var lengthMayFail = await this.Reader.TryReadUInt32Async( cancellationToken ).ConfigureAwait( false );
						if ( lengthMayFail < 0 )
						{
							return null;
						}
						
						length = unchecked( ( UInt32  )lengthMayFail );
						// </WriteUnpackLength>
						// <WriteUnpackByteCore lengthVariable="(null)" needsDeclaration="True" isAsync="True">
						var read = await this.Reader.TryReadByteAsync( cancellationToken ).ConfigureAwait( false );
						if ( read < 0 )
						{
							return null;
						}
						else
						{
						}
						// </WriteUnpackByteCore>
						// <WriteDrainValue sizeVariable="length" isAsync="True">
						#region DrainValue
						
						
						if ( !await this.Reader.DrainAsync( length, cancellationToken ).ConfigureAwait( false ) )
						{
							return null;
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

			return this.Reader.Offset - startOffset;
		}

#endif // FEATURE_TAP
	}
}
