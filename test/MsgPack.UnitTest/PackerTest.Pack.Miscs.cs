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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MsgPack.Serialization;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.Runtime.Serialization;

namespace MsgPack
{
	[TestFixture]
	public partial class PackerTest_PackMiscs
	{
		private static void TestPackTCore<T>( T value, byte[] expected )
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<T>( value );
				Assert.AreEqual(
					expected,
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_Array_Null_Nil()
		{
			TestPackTCore<MessagePackObject[]>( null, new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackT_IListT_Null_Nil()
		{
			TestPackTCore<IList<MessagePackObject>>( null, new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackT_ObjectArray_Null_Nil()
		{
			TestPackTCore<object[]>( null, new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackT_IList_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObject[] { 1, 2, 3 };
			TestPackTCore<IList>( value, new byte[] { 0x93, 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackT_IList_Null_Nil()
		{
			TestPackTCore<IList>( null, new byte[] { 0xC0 } );
		}


		[Test]
		public void TestPackT_IEnumerable_Null_Nil()
		{
			TestPackTCore<IEnumerable>( null, new byte[] { 0xC0 } );
		}


		[Test]
		public void TestPackT_Dictionary_Null_Nil()
		{
			TestPackTCore<MessagePackObjectDictionary>( null, new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackT_IDictionary_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 } };
			TestPackTCore<IDictionary>( value, new byte[] { 0x82, 0x1, 0x1, 0x2, 0x2 } );
		}

		[Test]
		public void TestPackT_IDictionary_Null_Nil()
		{
			TestPackTCore<IDictionary>( null, new byte[] { 0xC0 } );
		}


		[Test]
		public void TestPackT_ByteArray_Null_Nil()
		{
			TestPackTCore<byte[]>( null, new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackT_String_Null_Nil()
		{
			TestPackTCore<string>( null, new byte[] { 0xC0 } );
		}


		[Test]
		public void TestPackT_IPackable_NotNull_Success()
		{
			var value = new Packable();
			TestPackTCore<Packable>( value, new byte[] { 0xC3 } );
		}

		[Test]
		public void TestPackT_IPackable_Null_AsNil()
		{
			TestPackTCore<Packable>( null, new byte[] { 0xC0 } );
		}


		private static void TestCore<T>( Action<Packer, T> method, T arg, byte[] expected )
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				method( packer, arg );
				Assert.AreEqual(
					expected,
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_Empty_0x90()
		{
			TestCore( ( packer, count ) => packer.PackArrayHeader( count ), 0, new byte[] { 0x90 } );
		}

		[Test]
		public void TestPackArrayHeaderInt32_15_0x9F()
		{
			TestCore( ( packer, count ) => packer.PackArrayHeader( count ), 0xF, new byte[] { 0x9F } );
		}

		[Test]
		public void TestPackArrayHeaderInt32_16_0xDC0100()
		{
			TestCore( ( packer, count ) => packer.PackArrayHeader( count ), 0x100, new byte[] { 0xDC, 0x01, 0x00 } );
		}

		[Test]
		public void TestPackArrayHeaderInt32_0xFFFF_0xDCFFFF()
		{
			TestCore( ( packer, count ) => packer.PackArrayHeader( count ), 0xFFFF, new byte[] { 0xDC, 0xFF, 0xFF } );
		}

		[Test]
		public void TestPackArrayHeaderInt32_0x10000_0xDD00010000()
		{
			TestCore( ( packer, count ) => packer.PackArrayHeader( count ), 0x10000, new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 } );
		}

		[Test]
		public void TestPackArrayHeaderInt32_MinusOne()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => TestCore( ( packer, count ) => packer.PackArrayHeader( count ), -1, new byte[] { 0xC0 } ) );
		}

		[Test]
		public void TestPackArrayHeaderIList_Null_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackArrayHeader( array ), default( IList<MessagePackObject> ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackArrayHeaderIList_NotNull_AsCount()
		{
			TestCore( ( packer, array ) => packer.PackArrayHeader( array ), new MessagePackObject[] { 1, 2, 3 }, new byte[] { 0x93 } );
		}


		[Test]
		public void TestPackMapHeaderInt32_Empty_0x80()
		{
			TestCore( ( packer, count ) => packer.PackMapHeader( count ), 0, new byte[] { 0x80 } );
		}

		[Test]
		public void TestPackMapHeaderInt32_15_0x8F()
		{
			TestCore( ( packer, count ) => packer.PackMapHeader( count ), 0xF, new byte[] { 0x8F } );
		}

		[Test]
		public void TestPackMapHeaderInt32_16_0xDE0100()
		{
			TestCore( ( packer, count ) => packer.PackMapHeader( count ), 0x100, new byte[] { 0xDE, 0x01, 0x00 } );
		}

		[Test]
		public void TestPackMapHeaderInt32_0xFFFF_0xDEFFFF()
		{
			TestCore( ( packer, count ) => packer.PackMapHeader( count ), 0xFFFF, new byte[] { 0xDE, 0xFF, 0xFF } );
		}

		[Test]
		public void TestPackMapHeaderInt32_0x10000_0xDF00010000()
		{
			TestCore( ( packer, count ) => packer.PackMapHeader( count ), 0x10000, new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 } );
		}

		[Test]
		public void TestPackMapHeaderInt32_MinusOne()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => TestCore( ( packer, count ) => packer.PackMapHeader( count ), -1, new byte[] { 0xC0 } ) );
		}

		[Test]
		public void TestPackMapHeaderIDictionary_Null_AsNil()
		{
			TestCore( ( packer, map ) => packer.PackMapHeader( map ), default( IDictionary<MessagePackObject, MessagePackObject> ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackMapHeaderIDictionary_NotNull_AsCount()
		{
			TestCore( ( packer, map ) => packer.PackMapHeader( map ), new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 }, { 3, 3 } }, new byte[] { 0x83 } );
		}


		[Test]
		public void TestPackItems_NotNull__AsArray()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				packer.PackCollection( new int[] { 1, 2, 3 } );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackItems_NotNullEnumerable_AsArray()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Enumerable.Range( 1, 3 ) );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackItems_Null_AsNil()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				packer.PackCollection( default( MessagePackObject[] ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					buffer.ToArray()
				);
			}
		}

#pragma warning disable 0618
		[Test]
		public void TestPackRawHeaderInt32_Empty_0xA0()
		{
			TestCore( ( packer, count ) => packer.PackRawHeader( count ), 0, new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackRawHeaderInt32_31_0xBF()
		{
			TestCore( ( packer, count ) => packer.PackRawHeader( count ), 0x1F, new byte[] { 0xBF } );
		}

		[Test]
		public void TestPackRawHeaderInt32_32_0xDA0200()
		{
			TestCore( ( packer, count ) => packer.PackRawHeader( count ), 0x20, new byte[] { 0xDA, 0x00, 0x20 } );
		}

		[Test]
		public void TestPackRawHeaderInt32_0xFFFF_0xDAFFFF()
		{
			TestCore( ( packer, count ) => packer.PackRawHeader( count ), 0xFFFF, new byte[] { 0xDA, 0xFF, 0xFF } );
		}

		[Test]
		public void TestPackRawHeaderInt32_0x10000_0xDB00010000()
		{
			TestCore( ( packer, count ) => packer.PackRawHeader( count ), 0x10000, new byte[] { 0xDB, 0x00, 0x01, 0x00, 0x00 } );
		}

		[Test]
		public void TestPackRawHeaderInt32_MinusOne()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => TestCore( ( packer, count ) => packer.PackRawHeader( count ), -1, new byte[] { 0xC0 } ) );
		}
#pragma warning restore 0618

		[Test]
		public void TestPackRaw_NotNullByteArray_AsIs()
		{
			TestCore( ( packer, array ) => packer.PackRaw( array ), new byte[] { 1, 2, 3 }, new byte[] { 0xA3, 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRaw_NullByteArray_AsIs()
		{
			TestCore( ( packer, array ) => packer.PackRaw( array ), new byte[ 0 ], new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackRaw_NullByteArray_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackRaw( array ), default( byte[] ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackRaw_NotNullIListOfByte_AsIs()
		{
			TestCore<IList<byte>>( ( packer, array ) => packer.PackRaw( array ), new byte[] { 1, 2, 3 }, new byte[] { 0xA3, 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRaw_EmptyIListOfByte_AsIs()
		{
			TestCore<IList<byte>>( ( packer, array ) => packer.PackRaw( array ), new List<byte>(), new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackRaw_NullIListOfByte_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackRaw( array ), default( IList<byte> ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackRaw_NotNullIEnumerableOfByte_AsRaw32()
		{
			TestCore<IEnumerable<byte>>( ( packer, array ) => packer.PackRaw( array.Select( i => i ) ), new byte[] { 1, 2, 3 }, new byte[] { 0xDB, 0x0, 0x0, 0x0, 0x3, 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRaw_EmptyIEnumerableOfByte_AsZeroLengthRaw32()
		{
			TestCore<IEnumerable<byte>>( ( packer, array ) => packer.PackRaw( array.Select( i => i ) ), Enumerable.Empty<byte>(), new byte[] { 0xDB, 0x0, 0x0, 0x0, 0x0 } );
		}

		[Test]
		public void TestPackRaw_NullIEnumerableOfByte_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackRaw( array ), default( IEnumerable<byte> ), new byte[] { 0xC0 } );
		}


		[Test]
		public void TestPackRawBody_NotNullArray_AsIs()
		{
			TestCore( ( packer, array ) => packer.PackRawBody( array ), new byte[] { 1, 2, 3 }, new byte[] { 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRawBody_EmptyArray_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackRawBody( array ), new byte[ 0 ], new byte[ 0 ] );
		}

		[Test]
		public void TestPackRawBody_NullIArray_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => TestCore( ( packer, array ) => packer.PackRawBody( array ), default( byte[] ), new byte[] { 0xC0 } ) );
		}

		[Test]
		public void TestPackRawBody_ArrayAsNotNullIEnumerableOfByte_AsIs()
		{
			TestCore<IEnumerable<byte>>( ( packer, array ) => packer.PackRawBody( array ), new byte[] { 1, 2, 3 }, new byte[] { 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRawBody_ListAsNotNullIEnumerableOfByte_AsIs()
		{
			TestCore<IEnumerable<byte>>( ( packer, array ) => packer.PackRawBody( array ), new List<byte>() { 1, 2, 3 }, new byte[] { 0x1, 0x2, 0x3 } );
		}

		[Test]
		public void TestPackRawBody_EmptyIEnumerableOfByte_AsNil()
		{
			TestCore( ( packer, array ) => packer.PackRawBody( array ), Enumerable.Empty<byte>(), new byte[ 0 ] );
		}

		[Test]
		public void TestPackRawBody_NullIEnumerableOfByte_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => TestCore( ( packer, array ) => packer.PackRawBody( array ), default( IEnumerable<byte> ), new byte[] { 0xC0 } ) );
		}


		[Test]
		public void TestPackString_String_NotNull_AsUtf8NonBom()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), "ABC", new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } );
		}

		[Test]
		public void TestPackString_String_Empty_AsEmpty()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), String.Empty, new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackString_String_Null_AsNil()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), default( string ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackString_IEnumerableOfChar_NotNull_AsUtf8NonBom()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), new char[] { 'A', 'B', 'C' }, new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } );
		}

		[Test]
		public void TestPackString_IEnumerableOfChar_Empty_AsEmpty()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), new char[ 0 ], new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackString_IEnumerableOfChar_Null_AsNil()
		{
			TestCore( ( packer, str ) => packer.PackString( str ), default( char[] ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackString_String_NotNull_AsSpecifiedEncoding()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), "ABC", new byte[] { 0xA6, ( byte )'A', 0x0, ( byte )'B', 0x0, ( byte )'C', 0x0 } );
		}

		[Test]
		public void TestPackString_StringEncoding_Empty_AsEmpty()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), String.Empty, new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackString_StringEncoding_Null_AsNil()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), default( string ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackString_StringEncoding_EncodingIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => TestCore( ( packer, str ) => packer.PackString( str, null ), default( string ), new byte[] { 0xC0 } ) );
		}

		[Test]
		public void TestPackString_IEnumerableOfCharEncoding_NotNull_AsSpecifiedEncoding()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), new char[] { 'A', 'B', 'C' }, new byte[] { 0xA6, ( byte )'A', 0x0, ( byte )'B', 0x0, ( byte )'C', 0x0 } );
		}

		[Test]
		public void TestPackString_IEnumerableOfCharEncoding_Empty_AsEmpty()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), new char[ 0 ], new byte[] { 0xA0 } );
		}

		[Test]
		public void TestPackString_IEnumerableOfCharEncoding_Null_AsNil()
		{
			TestCore( ( packer, str ) => packer.PackString( str, Encoding.Unicode ), default( char[] ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPackString_IEnumerableOfCharEncoding_EncodingIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => TestCore( ( packer, str ) => packer.PackString( str, null ), default( char[] ), new byte[] { 0xC0 } ) );
		}


		[Test]
		public void TestPackObject_StringNotNull_AsIs()
		{
			TestCore( ( packer, value ) => packer.PackObject( value ), "ABC", new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } );
		}

		[Test]
		public void TestPackObject_ByteArrayNotNull_AsIs()
		{
			TestCore( ( packer, value ) => packer.PackObject( value ), new byte[] { 1, 2, 3 }, new byte[] { 0xA3, 1, 2, 3 } );
		}

		[Test]
		public void TestPackObject_ObjectNull_AsNil()
		{
			TestCore( ( packer, value ) => packer.PackObject( value ), default( object ), new byte[] { 0xC0 } );
		}

		[Test]
		public void TestPack_SByte_0x7F_0x7F()
		{
			TestCore<SByte>( ( packer, value ) => packer.Pack( value ), 0x7F, new byte[] { 0x7F } );
		}

		[Test]
		public void TestPack_Int16_0x7F_0x7F()
		{
			TestCore<Int16>( ( packer, value ) => packer.Pack( value ), 0x7F, new byte[] { 0x7F } );
		}

		[Test]
		public void TestPack_Int32_0x7F_0x7F()
		{
			TestCore<Int32>( ( packer, value ) => packer.Pack( value ), 0x7F, new byte[] { 0x7F } );
		}

		[Test]
		public void TestPack_Int64_0x7F_0x7F()
		{
			TestCore<Int64>( ( packer, value ) => packer.Pack( value ), 0x7F, new byte[] { 0x7F } );
		}


		[Test]
		public void TestPack_Int32_Minus128_AsInt8()
		{
			TestCore<Int32>( ( packer, value ) => packer.Pack( value ), -128, new byte[] { 0xD0, 0x80 } );
		}


		[Test]
		public void TestPack_UInt32_0x80_AsUInt8()
		{
			TestCore<UInt32>( ( packer, value ) => packer.Pack( value ), 0x80, new byte[] { 0xCC, 0x80 } );
		}

		[Test]
		public void TestPack_UInt32_0x100_AsUInt16()
		{
			TestCore<UInt32>( ( packer, value ) => packer.Pack( value ), 0x100, new byte[] { 0xCD, 0x1, 0x00 } );
		}


		[Test]
		public void TestPack_Int64_0x1_0x1()
		{
			TestCore<Int64>( ( packer, value ) => packer.Pack( value ), 1, new byte[] { 0x1 } );
		}

		[Test]
		public void TestPack_Int64_Minus128_AsInt8()
		{
			TestCore<Int64>( ( packer, value ) => packer.Pack( value ), -128, new byte[] { 0xD0, 0x80 } );
		}

		[Test]
		public void TestPack_Int64_0x100_AsInt16()
		{
			TestCore<Int64>( ( packer, value ) => packer.Pack( value ), 0x100, new byte[] { 0xD1, 0x1, 0x00 } );
		}

		[Test]
		public void TestPack_Int64_0x10000_AsInt32()
		{
			TestCore<Int64>( ( packer, value ) => packer.Pack( value ), 0x10000, new byte[] { 0xD2, 0x0, 0x01, 0x00, 0x00 } );
		}


		[Test]
		public void TestPack_UInt64_0x80_AsInt8()
		{
			TestCore<UInt64>( ( packer, value ) => packer.Pack( value ), 0x80, new byte[] { 0xCC, 0x80 } );
		}

		[Test]
		public void TestPack_UInt64_0x100_AsInt16()
		{
			TestCore<UInt64>( ( packer, value ) => packer.Pack( value ), 0x100, new byte[] { 0xCD, 0x1, 0x00 } );
		}

		[Test]
		public void TestPack_UInt64_0x10000_AsInt32()
		{
			TestCore<UInt64>( ( packer, value ) => packer.Pack( value ), 0x10000, new byte[] { 0xCE, 0x0, 0x01, 0x00, 0x00 } );
		}

		private sealed class Packable : IPackable
		{
			public void PackToMessage( Packer packer, PackingOptions options )
			{
				// 0xC3
				packer.Pack( true );
			}
		}

	}
}
