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
using System.IO;
using System.Linq;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.Collections.Generic;

namespace MsgPack
{
	[TestFixture]
	public class MessagePackObjectTest_IPackable
	{
		[Test]
		public void TestPackToMessage_Zero_Success()
		{
			TestPackToMessageCore( 0, 0 );
		}

		[Test]
		public void TestPackToMessage_Null_Success()
		{
			TestPackToMessageCore( MessagePackObject.Nil, MessagePackCode.NilValue );
		}

		[Test]
		public void TestPackToMessage_True_Success()
		{
			TestPackToMessageCore( true, MessagePackCode.TrueValue );
		}

		[Test]
		public void TestPackToMessage_False_Success()
		{
			TestPackToMessageCore( false, MessagePackCode.FalseValue );
		}

		[Test]
		public void TestPackToMessage_TinyPositiveInteger_Success()
		{
			TestPackToMessageCore( 1, 1 );
		}

		[Test]
		public void TestPackToMessage_TinyNegativeIngeter_Success()
		{
			TestPackToMessageCore( -1, 0xFF );
		}

		[Test]
		public void TestPackToMessage_Int8_Success()
		{
			TestPackToMessageCore( SByte.MinValue, MessagePackCode.SignedInt8, 0x80 );
		}

		[Test]
		public void TestPackToMessage_Int16_Success()
		{
			TestPackToMessageCore( Int16.MinValue, MessagePackCode.SignedInt16, 0x80, 0x00 );
		}

		[Test]
		public void TestPackToMessage_Int32_Success()
		{
			TestPackToMessageCore( Int32.MinValue, MessagePackCode.SignedInt32, 0x80, 0x00, 0x00, 0x00 );
		}

		[Test]
		public void TestPackToMessage_Int64_Success()
		{
			TestPackToMessageCore( Int64.MinValue, MessagePackCode.SignedInt64, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 );
		}

		[Test]
		public void TestPackToMessage_UInt8_Success()
		{
			TestPackToMessageCore( Byte.MaxValue, MessagePackCode.UnsignedInt8, 0xFF );
		}

		[Test]
		public void TestPackToMessage_UInt16_Success()
		{
			TestPackToMessageCore( UInt16.MaxValue, MessagePackCode.UnsignedInt16, 0xFF, 0xFF );
		}

		[Test]
		public void TestPackToMessage_UInt32_Success()
		{
			TestPackToMessageCore( UInt32.MaxValue, MessagePackCode.UnsignedInt32, 0xFF, 0xFF, 0xFF, 0xFF );
		}

		[Test]
		public void TestPackToMessage_UInt64_Success()
		{
			TestPackToMessageCore( UInt64.MaxValue, MessagePackCode.UnsignedInt64, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF );
		}

		[Test]
		public void TestPackToMessage_Single_Success()
		{
			TestPackToMessageCore( Single.Epsilon, new byte[] { MessagePackCode.Real32 }.Concat( SingleToBytes( Single.Epsilon ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_Double_Success()
		{
			TestPackToMessageCore( Double.Epsilon, new byte[] { MessagePackCode.Real64 }.Concat( SingleToBytes( Double.Epsilon ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_StringEmpty_Success()
		{
			TestPackToMessageCore( String.Empty, MessagePackCode.MinimumFixedRaw );
		}

		[Test]
		public void TestPackToMessage_BytesEmpty_Success()
		{
			TestPackToMessageCore( new byte[ 0 ], MessagePackCode.MinimumFixedRaw );
		}

		[Test]
		public void TestPackToMessage_String1Length_Success()
		{
			TestPackToMessageCore( "A", MessagePackCode.MinimumFixedRaw | 0x1, ( byte )'A' );
		}

		[Test]
		public void TestPackToMessage_Bytes1Length_Success()
		{
			var value = new byte[] { 1, 2 };
			TestPackToMessageCore( value, new byte[] { MessagePackCode.MinimumFixedRaw | 0x2 }.Concat( value ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_String31Length_Success()
		{
			var value = new String( 'A', 31 );
			TestPackToMessageCore( value, new byte[] { MessagePackCode.MinimumFixedRaw | 31 }.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_Bytes31Length_Success()
		{
			var value = Enumerable.Repeat( ( byte )123, 31 ).ToArray();
			TestPackToMessageCore( value, new byte[] { MessagePackCode.MinimumFixedRaw | 31 }.Concat( value ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_String32Length_Success()
		{
			var value = new String( 'A', 32 );
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw16, 0x0, 32 }.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_Bytes32Length_Success()
		{
			var value = Enumerable.Repeat( ( byte )123, 32 ).ToArray();
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw16, 0x0, 32 }.Concat( value ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_String0xFFFFLength_Success()
		{
			var value = new String( 'A', 0xFFFF );
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw16, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )'A', 0xFFFF ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_Bytes0xFFFFLength_Success()
		{
			var value = Enumerable.Repeat( ( byte )123, 0xFFFF ).ToArray();
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw16, 0xFF, 0xFF }.Concat( value ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_String0x10000Length_Success()
		{
			var value = new String( 'A', 0x10000 );
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw32, 0x00, 0x1, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )'A', 0x10000 ) ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_Bytes0x10000Length_Success()
		{
			var value = Enumerable.Repeat( ( byte )123, 0x10000 ).ToArray();
			TestPackToMessageCore( value, new byte[] { MessagePackCode.Raw32, 0x00, 0x1, 0x00, 0x00 }.Concat( value ).ToArray() );
		}

		[Test]
		public void TestPackToMessage_ArrayEmpty_Success()
		{
			TestPackToMessageCore( new MessagePackObject[ 0 ], new byte[] { MessagePackCode.MinimumFixedArray } );
		}

		[Test]
		public void TestPackToMessage_Array1Length_Success()
		{
			TestPackToMessageCore( new MessagePackObject[] { 1 }, new byte[] { MessagePackCode.MinimumFixedArray | 1, 0x1 } );
		}

		[Test]
		public void TestPackToMessage_Array15Length_Success()
		{
			TestPackToMessageCore(
				Enumerable.Repeat( ( byte )1, 15 ).Select( b => new MessagePackObject( b ) ).ToArray(),
				new byte[] { MessagePackCode.MinimumFixedArray | 15 }.Concat( Enumerable.Repeat( ( byte )1, 15 ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Array16Length_Success()
		{
			TestPackToMessageCore(
				Enumerable.Repeat( ( byte )1, 16 ).Select( b => new MessagePackObject( b ) ).ToArray(),
				new byte[] { MessagePackCode.Array16, 0, 16 }.Concat( Enumerable.Repeat( ( byte )1, 16 ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Array0xFFFFLength_Success()
		{
			TestPackToMessageCore(
				Enumerable.Repeat( ( byte )1, 0xFFFF ).Select( b => new MessagePackObject( b ) ).ToArray(),
				new byte[] { MessagePackCode.Array16, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )1, 0xFFFF ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Array0x10000Length_Success()
		{
			TestPackToMessageCore(
				Enumerable.Repeat( ( byte )1, 0x10000 ).Select( b => new MessagePackObject( b ) ).ToArray(),
				new byte[] { MessagePackCode.Array32, 0x0, 0x1, 0x0, 0x0 }.Concat( Enumerable.Repeat( ( byte )1, 0x10000 ) ).ToArray()
			);
		}


		[Test]
		public void TestPackToMessage_DictionaryEmpty_Success()
		{
			TestPackToMessageCore( new MessagePackObject( new MessagePackObjectDictionary() ), new byte[] { MessagePackCode.MinimumFixedMap } );
		}

		[Test]
		public void TestPackToMessage_Dictionary1Length_Success()
		{
			TestPackToMessageCore(
				new MessagePackObject( new MessagePackObjectDictionary() { { 1, 1 } } ),
				new byte[] { MessagePackCode.MinimumFixedMap | 1, 0x1, 0x1 }
			);
		}

		[Test]
		public void TestPackToMessage_Dictionary15Length_Success()
		{
			TestPackToMessageCore(
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 15 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new byte[] { MessagePackCode.MinimumFixedMap | 15 }.Concat( GetMessagePackMapBytes( 1, 15 ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Dictionary16Length_Success()
		{
			TestPackToMessageCore(
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 16 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new byte[] { MessagePackCode.Map16, 0x0, 16 }.Concat( GetMessagePackMapBytes( 1, 16 ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Dictionary0xFFFFLength_Success()
		{
			TestPackToMessageCore(
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0xFFFF ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new byte[] { MessagePackCode.Map16, 0xFF, 0xFF }.Concat( GetMessagePackMapBytes( 1, 0xFFFF ) ).ToArray()
			);
		}

		[Test]
		public void TestPackToMessage_Dictionary0x10000Length_Success()
		{
			TestPackToMessageCore(
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0x10000 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new byte[] { MessagePackCode.Map32, 0x0, 0x1, 0x0, 0x0 }.Concat( GetMessagePackMapBytes( 1, 0x10000 ) ).ToArray()
			);
		}

		// Ext types

		private static IEnumerable<byte> BytesRange( int start, int count )
		{
			return Enumerable.Range( start, count ).Select( i => ( byte ) ( i & 0xff ) );
		}

		[Test]
		public void TestPackToMessage_Ext0Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, new byte[0]) ),
				new byte[] { MessagePackCode.Ext8, 0x0, 0x1 },
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x1Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 1 ).ToArray() ) ),
				new byte[] { MessagePackCode.FixExt1, 0x1 }.Concat( BytesRange( 1, 1 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x1Length_ProhibitExtType_InvalidOperationException()
		{
			Assert.Throws<InvalidOperationException>(
				() => TestPackToMessageCoreWithOption(
					new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 1 ).ToArray() ) ),
					new byte[] { MessagePackCode.FixExt1, 0x1 }.Concat( BytesRange( 1, 1 ) ).ToArray(),
					PackerCompatibilityOptions.ProhibitExtendedTypeObjects
			) );
		}

		[Test]
		public void TestPackToMessage_Ext0x2Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 2 ).ToArray() ) ),
				new byte[] { MessagePackCode.FixExt2, 0x1 }.Concat( BytesRange( 1, 2 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x3Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 3 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext8, 0x3, 0x1 }.Concat( BytesRange( 1, 3 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x4Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 4 ).ToArray() ) ),
				new byte[] { MessagePackCode.FixExt4, 0x1 }.Concat( BytesRange( 1, 4 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x5Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 5 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext8, 0x5, 0x1 }.Concat( BytesRange( 1, 5 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x8Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 8 ).ToArray() ) ),
				new byte[] { MessagePackCode.FixExt8, 0x1 }.Concat( BytesRange( 1, 8 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x9Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 9 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext8, 0x9, 0x1 }.Concat( BytesRange( 1, 9 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x16Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 16 ).ToArray() ) ),
				new byte[] { MessagePackCode.FixExt16, 0x1 }.Concat( BytesRange( 1, 16 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x17Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 17 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext8, 0x11, 0x1 }.Concat( BytesRange( 1, 17 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0xFFLength_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 0xFF ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext8, 0xFF, 0x1 }.Concat( BytesRange( 1, 0xFF ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0x100Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 0x100 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext16, 0x1, 0, 0x1 }.Concat( BytesRange( 1, 0x100 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		[Test]
		public void TestPackToMessage_Ext0xFFFFLength_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 0xFFFF ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext16, 0xFF, 0xFF, 0x1 }.Concat( BytesRange( 1, 0xFFFF ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}
		
		[Test]
		public void TestPackToMessage_Ext0x10000Length_Success()
		{
			TestPackToMessageCoreWithOption(
				new MessagePackObject( new MessagePackExtendedTypeObject( 1, BytesRange( 1, 0x10000 ).ToArray() ) ),
				new byte[] { MessagePackCode.Ext32, 0, 0x1, 0, 0, 0x1 }.Concat( BytesRange( 1, 0x10000 ) ).ToArray(),
				PackerCompatibilityOptions.None
			);
		}

		private static void TestPackToMessageCore( MessagePackObject target, params byte[] expected )
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackToMessage( packer, new PackingOptions() );
				var actual = buffer.ToArray();
				Assert.AreEqual( expected, actual );
			}
		}

		private static void TestPackToMessageCoreWithOption( MessagePackObject target, byte[] expected, PackerCompatibilityOptions compatibilityOptions )
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer, compatibilityOptions ) )
			{
				target.PackToMessage( packer, new PackingOptions() );
				var actual = buffer.ToArray();
				Assert.AreEqual( expected, actual );
			}
		}

		private static byte[] GetMessagePackMapBytes( int start, int count )
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				for ( int i = 0; i < count; i++ )
				{
					packer.Pack( i + start );
					packer.Pack( i + start );
				}

				return buffer.ToArray();
			}
		}

		private static IEnumerable<byte> SingleToBytes( float value )
		{
			return ( BitConverter.IsLittleEndian ? BitConverter.GetBytes( value ).Reverse() : BitConverter.GetBytes( value ) );
		}

		private static IEnumerable<byte> SingleToBytes( double value )
		{
			return ( BitConverter.IsLittleEndian ? BitConverter.GetBytes( value ).Reverse() : BitConverter.GetBytes( value ) );
		}
	}
}
