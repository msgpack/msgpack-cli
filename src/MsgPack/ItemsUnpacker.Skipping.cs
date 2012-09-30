 
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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	// This file was generated from ItemsUnpacker.Skipping.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit ItemsUnpacker.Skipping.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class ItemsUnpacker
	{
		private static readonly byte[] DummyBufferForSkipping = new byte[ 64 * 1024 ];
		private readonly Stack<uint> _remainingCollections = new Stack<uint>( 4 );

		protected sealed override long? SkipCore()
		{
			var source = this._stream;
			var buffer = this._scalarBuffer;
			Contract.Assert( source != null );
			Contract.Assert( buffer != null );

			long remainingItems = -1;
			long skipped = 0;
			Stack<long> remainingCollections = null;
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
								remainingCollections = new Stack<long>( 4 );
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
								remainingCollections = new Stack<long>( 4 );
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
						}
						
						skipped += bytesRead;
						
						#endregion DrainValue
						continue;
					}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
							var reading = remaining > DummyBufferForSkipping.Length ? DummyBufferForSkipping.Length : unchecked( ( int )remaining );
							bytesRead += source.Read( DummyBufferForSkipping, 0, reading );
							if ( bytesRead < reading )
							{
								return null;
							}
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
									remainingCollections = new Stack<long>( 4 );
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
									remainingCollections = new Stack<long>( 4 );
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
									remainingCollections = new Stack<long>( 4 );
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
									remainingCollections = new Stack<long>( 4 );
								}
								
								remainingCollections.Push( remainingItems );
							}
							
							remainingItems = length * 2;
							
							#endregion PushContextCollection
						}

						continue;
					}
					default:
					{
						throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", header ) );
					}
				}
			} while ( remainingItems > 0 );

			return skipped;
		}
	}
}
