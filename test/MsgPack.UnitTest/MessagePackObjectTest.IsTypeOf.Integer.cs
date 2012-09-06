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
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	partial class MessagePackObjectTest_IsTypeOf
	{
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteZero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_BytePlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Byte )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_Byte_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.Byte ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteSByteMinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -128 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteMinusOne_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.SByte )( -1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SByteZero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_SBytePlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.SByte )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_SByte_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.SByte ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Int16MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -32768 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16SByteMinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -128 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16MinusOne_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( -1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int16ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int16 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_Int16_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.Int16 ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt16UInt16MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt16 )( 65535 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_UInt16_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.UInt16 ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int32MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Int16MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -32768 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32SByteMinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -128 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32MinusOne_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( -1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int32UInt16MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int32 )( 65535 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_Int32_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.Int32 ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt16MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 65535 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt32UInt32MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt32 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_UInt32_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.UInt32 ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int64MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -9223372036854775808 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int32MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -2147483648 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Int16MinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -32768 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64SByteMinValue_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -128 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64MinusOne_IsTypeOfUInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( -1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt16MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 65535 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Int64UInt32MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.Int64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_Int64_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.Int64 ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64Zero_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 0 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfSByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64PlusOne_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 1 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfByte_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64ByteMaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 255 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfUInt16_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt16MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 65535 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfUInt32_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt32MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 4294967295 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.Byte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfSByte_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.SByte ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.Int16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfUInt16_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.UInt16 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.Int32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfUInt32_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.UInt32 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfInt64_False()
		{
			Assert.AreEqual( false, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.Int64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_UInt64UInt64MaxValue_IsTypeOfUInt64_True()
		{
			Assert.AreEqual( true, ( new MessagePackObject( ( System.UInt64 )( 18446744073709551615 ) ).IsTypeOf( typeof( System.UInt64 ) ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_Nil_UInt64_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( System.UInt64 ) ) );
		}
		
	}
}