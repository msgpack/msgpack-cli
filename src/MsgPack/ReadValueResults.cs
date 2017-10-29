#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;

namespace MsgPack
{
	/// <summary>
	///		Static utilities of <see cref="ReadValueResult"/>.
	/// </summary>
	internal static class ReadValueResults
	{
		// Index = read_header
		public static readonly ReadValueResult[] EncodedTypes =
			Enumerable.Range( 0, 0x80 ).Select( i => ( ReadValueResult )i )
			.Concat(
				Enumerable.Range( 0x80, 0x10 ).Select( i => ( ReadValueResult )( ( i - 0x80 ) | ( uint )ReadValueResult.FixMapType ) )
			).Concat(
				Enumerable.Range( 0x90, 0x10 ).Select( i => ( ReadValueResult )( ( i - 0x90 ) | ( uint )ReadValueResult.FixArrayType ) )
			).Concat(
				Enumerable.Range( 0xA0, 0x20 ).Select( i => ( ReadValueResult )( ( i - 0xA0 ) | ( uint )ReadValueResult.FixStrType ) )
			).Concat(
				new[]
				{
					ReadValueResult.Nil,
					ReadValueResult.InvalidCode, // reserved
					ReadValueResult.False,
					ReadValueResult.True
				}
			).Concat(
				new[]
				{
					ReadValueResult.Bin8Type,
					ReadValueResult.Bin16Type,
					ReadValueResult.Bin32Type,
					ReadValueResult.Ext8Type,
					ReadValueResult.Ext16Type,
					ReadValueResult.Ext32Type,
					ReadValueResult.Real32Type,
					ReadValueResult.Real64Type,
					ReadValueResult.UInt8Type,
					ReadValueResult.UInt16Type,
					ReadValueResult.UInt32Type,
					ReadValueResult.UInt64Type,
					ReadValueResult.Int8Type,
					ReadValueResult.Int16Type,
					ReadValueResult.Int32Type,
					ReadValueResult.Int64Type,
					( ReadValueResult )( ( int )ReadValueResult.FixExtType | 0x1 ),
					( ReadValueResult )( ( int )ReadValueResult.FixExtType | 0x2 ),
					( ReadValueResult )( ( int )ReadValueResult.FixExtType | 0x4 ),
					( ReadValueResult )( ( int )ReadValueResult.FixExtType | 0x8 ),
					( ReadValueResult )( ( int )ReadValueResult.FixExtType | 0x10 ),
					ReadValueResult.Str8Type,
					ReadValueResult.Str16Type,
					ReadValueResult.Str32Type,
					ReadValueResult.Array16Type,
					ReadValueResult.Array32Type,
					ReadValueResult.Map16Type,
					ReadValueResult.Map32Type
				}
			).Concat(
				Enumerable.Range( 0xE0, 0x20 ).Select( b => ( ReadValueResult )b )
			).ToArray();

		public static readonly bool[] HasConstantObject =
			Enumerable.Repeat( true, 0x80 ) // Positive fix num
			.Concat(
				Enumerable.Repeat( true, 0x10 ) // FixMap
			).Concat(
				Enumerable.Repeat( true, 0x10 ) // FixArray
			).Concat(
				Enumerable.Repeat( false, 0x20 ) // FixRaw
			).Concat(
				new[]
				{
					true, // Nil,
					false, // reserved
					true, // False,
					true, // True
				}
			).Concat(
				Enumerable.Repeat( false, 28 ) // all false -- variable lengthes
			).Concat(
				Enumerable.Repeat( true, 0x20 ) // Negative fix num
			).ToArray();

		public static readonly MessagePackObject[] ContantObject =
			Enumerable.Range( 0, 0x80 ).Select( x => new MessagePackObject( x ) ) // Positive fix num
			.Concat(
				Enumerable.Range( 0, 0x10 ).Select( x => new MessagePackObject( x ) ) // FixMap
			).Concat(
				Enumerable.Range( 0, 0x10 ).Select( x => new MessagePackObject( x ) ) // FixArray
			).Concat(
				Enumerable.Repeat( default( MessagePackObject ), 0x20 ) // FixRaw
			).Concat(
				new[]
				{
					MessagePackObject.Nil, // Nil,
					default( MessagePackObject ), // reserved
					new MessagePackObject( false ), // False,
					new MessagePackObject( true ), // True
				}
			).Concat(
				Enumerable.Repeat( default( MessagePackObject ), 28 ) // all false -- variable lengthes
			).Concat(
				Enumerable.Range( -32, 0x20 ).Select( x => new MessagePackObject( x ) ) // Negative fix num
			).ToArray();

		public static readonly CollectionType[] CollectionType =
			Enumerable.Repeat( MsgPack.CollectionType.None, 0x80 ) // Positive fix num
			.Concat(
				Enumerable.Repeat( MsgPack.CollectionType.Map, 0x10 ) // FixMap
			).Concat(
				Enumerable.Repeat( MsgPack.CollectionType.Array, 0x10 ) // FixArray
			).Concat(
				Enumerable.Repeat( MsgPack.CollectionType.None, 0x20 ) // FixRaw
			).Concat(
				new[]
				{
					MsgPack.CollectionType.None, // Nil,
					MsgPack.CollectionType.None, // reserved
					MsgPack.CollectionType.None, // False,
					MsgPack.CollectionType.None, // True
				}
			).Concat(
				new[]
				{
					MsgPack.CollectionType.None, // Bin8
					MsgPack.CollectionType.None, // Bin16
					MsgPack.CollectionType.None, // Bin32
					MsgPack.CollectionType.None, // Ext8
					MsgPack.CollectionType.None, // Ext16
					MsgPack.CollectionType.None, // Ext32
					MsgPack.CollectionType.None, // Real32
					MsgPack.CollectionType.None, // Real64
					MsgPack.CollectionType.None, // UInt8
					MsgPack.CollectionType.None, // UInt16
					MsgPack.CollectionType.None, // UInt32
					MsgPack.CollectionType.None, // UInt64
					MsgPack.CollectionType.None, // Int8
					MsgPack.CollectionType.None, // Int16
					MsgPack.CollectionType.None, // Int32
					MsgPack.CollectionType.None, // Int64
					MsgPack.CollectionType.None, // FixExt1
					MsgPack.CollectionType.None, // FixExt2
					MsgPack.CollectionType.None, // FixExt4
					MsgPack.CollectionType.None, // FixExt8
					MsgPack.CollectionType.None, // FixExt16
					MsgPack.CollectionType.None, // Str8
					MsgPack.CollectionType.None, // Str16
					MsgPack.CollectionType.None, // Str32
					MsgPack.CollectionType.Array, // Array16
					MsgPack.CollectionType.Array, // Array32
					MsgPack.CollectionType.Map, // Map16
					MsgPack.CollectionType.Map, // Map32
				}
			).Concat(
				Enumerable.Repeat( MsgPack.CollectionType.None, 0x20 ) // Negative fix num
			).ToArray();

		public static byte ToByte( this ReadValueResult source )
		{
			switch ( source )
			{
				case ReadValueResult.Nil:
				{
					return ( byte )MessagePackCode.NilValue;
				}
				case ReadValueResult.True:
				{
					return ( byte )MessagePackCode.TrueValue;
				}
				case ReadValueResult.False:
				{
					return ( byte )MessagePackCode.FalseValue;
				}
				case ReadValueResult.InvalidCode:
				{
					return 0xC1;
				}
			}

			if ( ( source & ReadValueResult.FlagsAndTypeCodeMask ) == 0 )
			{
				return ( byte )( ( int )( source & ReadValueResult.ValueOrLengthMask ) );
			}

			if ( ( source & ReadValueResult.ArrayTypeMask ) == ReadValueResult.ArrayTypeMask )
			{
				var length = ( int )( source & ReadValueResult.LengthOfLengthMask ) >> 8;
				switch ( length )
				{
					case 0:
					{
						return ( byte )( MessagePackCode.MinimumFixedArray | ( int )( source & ReadValueResult.ValueOrLengthMask ) );
					}
					case 2:
					{
						return ( byte )MessagePackCode.Array16;
					}
					default:
					{
#if DEBUG
						Contract.Assert( length == 4, length + " == 4" );
#endif // DEBUG
						return ( byte )MessagePackCode.Array32;
					}
				}
			}

			if ( ( source & ReadValueResult.MapTypeMask ) == ReadValueResult.MapTypeMask )
			{
				var length = ( int )( source & ReadValueResult.LengthOfLengthMask ) >> 8;
				switch ( length )
				{
					case 0:
					{
						return ( byte )( MessagePackCode.MinimumFixedMap | ( int )( source & ReadValueResult.ValueOrLengthMask ) );
					}
					case 2:
					{
						return ( byte )MessagePackCode.Map16;
					}
					default:
					{
#if DEBUG
						Contract.Assert( length == 4, length + " == 4" );
#endif // DEBUG
						return ( byte )MessagePackCode.Map32;
					}
				}
			}

			if ( ( source & ReadValueResult.RawTypeMask ) == ReadValueResult.RawTypeMask )
			{
				var isBin = ( source & ReadValueResult.BinTypeMask ) == ReadValueResult.BinTypeMask;
				var length = ( int )( source & ReadValueResult.LengthOfLengthMask ) >> 8;
				switch ( length )
				{
					case 0:
					{
						return ( byte )( MessagePackCode.MinimumFixedRaw | ( int )( source & ReadValueResult.ValueOrLengthMask ) );
					}
					case 1:
					{
						return ( byte )( isBin ? MessagePackCode.Bin8 : MessagePackCode.Str8 );
					}
					case 2:
					{
						return ( byte )( isBin ? MessagePackCode.Bin16 : MessagePackCode.Str16 );
					}
					default:
					{
#if DEBUG
						Contract.Assert( length == 4, length + " == 4" );
#endif // DEBUG
						return ( byte )( isBin ? MessagePackCode.Bin32 : MessagePackCode.Str32 );
					}
				}
			}

			if ( ( source & ReadValueResult.ExtTypeMask ) == ReadValueResult.ExtTypeMask )
			{
				var length = ( int )( source & ReadValueResult.LengthOfLengthMask ) >> 8;
				switch ( length )
				{
					case 0:
					{
						switch ( ( int )( source & ReadValueResult.ValueOrLengthMask ) )
						{
							case 1:
							{
								return ( byte )MessagePackCode.FixExt1;
							}
							case 2:
							{
								return ( byte )MessagePackCode.FixExt2;
							}
							case 4:
							{
								return ( byte )MessagePackCode.FixExt4;
							}
							case 8:
							{
								return ( byte )MessagePackCode.FixExt8;
							}
							default:
							{
#if DEBUG
								Contract.Assert( ( int )( source & ReadValueResult.ValueOrLengthMask ) == 16, ( int )( source & ReadValueResult.ValueOrLengthMask ) + " == 16" );
#endif // DEBUG
								return ( byte )MessagePackCode.FixExt16;
							}
						}
					}
					case 1:
					{
						return ( byte )MessagePackCode.Ext8;
					}
					case 2:
					{
						return ( byte )MessagePackCode.Ext16;
					}
					default:
					{
#if DEBUG
						Contract.Assert( length == 4, length + " == 4" );
#endif // DEBUG
						return ( byte )MessagePackCode.Ext32;
					}
				}
			}

			switch ( source & ReadValueResult.TypeCodeMask )
			{
				case ReadValueResult.Int8Type:
				{
					return ( byte )MessagePackCode.SignedInt8;
				}
				case ReadValueResult.Int16Type:
				{
					return ( byte )MessagePackCode.SignedInt16;
				}
				case ReadValueResult.Int32Type:
				{
					return ( byte )MessagePackCode.SignedInt32;
				}
				case ReadValueResult.Int64Type:
				{
					return ( byte )MessagePackCode.SignedInt64;
				}
				case ReadValueResult.UInt8Type:
				{
					return ( byte )MessagePackCode.UnsignedInt8;
				}
				case ReadValueResult.UInt16Type:
				{
					return ( byte )MessagePackCode.UnsignedInt16;
				}
				case ReadValueResult.UInt32Type:
				{
					return ( byte )MessagePackCode.UnsignedInt32;
				}
				case ReadValueResult.UInt64Type:
				{
					return ( byte )MessagePackCode.UnsignedInt64;
				}
				case ReadValueResult.Real32Type:
				{
					return ( byte )MessagePackCode.Real32;
				}
				default:
				{
#if DEBUG
					Contract.Assert( ( source & ReadValueResult.TypeCodeMask ) == ReadValueResult.Real64Type, ( source & ReadValueResult.TypeCodeMask ) + " == ReadValueResult.Real64Type" );
#endif // DEBUG
					return ( byte )MessagePackCode.Real64;
				}
			}
		}
	}
}
