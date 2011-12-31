#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using NUnit.Framework;

namespace MsgPack
{
	partial class MessagePackObjectTest_Exceptionals
	{
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitBoolean_Nil_Fail()
		{
			var result = ( System.Boolean )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitBoolean_NotNumerics_Fail()
		{
			var result = ( System.Boolean )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitSByte_Nil_Fail()
		{
			var result = ( System.SByte )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitSByte_NotNumerics_Fail()
		{
			var result = ( System.SByte )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt16_Nil_Fail()
		{
			var result = ( System.Int16 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt16_NotNumerics_Fail()
		{
			var result = ( System.Int16 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt32_Nil_Fail()
		{
			var result = ( System.Int32 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt32_NotNumerics_Fail()
		{
			var result = ( System.Int32 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt64_Nil_Fail()
		{
			var result = ( System.Int64 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitInt64_NotNumerics_Fail()
		{
			var result = ( System.Int64 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitByte_Nil_Fail()
		{
			var result = ( System.Byte )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitByte_NotNumerics_Fail()
		{
			var result = ( System.Byte )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt16_Nil_Fail()
		{
			var result = ( System.UInt16 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt16_NotNumerics_Fail()
		{
			var result = ( System.UInt16 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt32_Nil_Fail()
		{
			var result = ( System.UInt32 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt32_NotNumerics_Fail()
		{
			var result = ( System.UInt32 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt64_Nil_Fail()
		{
			var result = ( System.UInt64 )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitUInt64_NotNumerics_Fail()
		{
			var result = ( System.UInt64 )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitSingle_Nil_Fail()
		{
			var result = ( System.Single )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitSingle_NotNumerics_Fail()
		{
			var result = ( System.Single )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitDouble_Nil_Fail()
		{
			var result = ( System.Double )( MessagePackObject.Nil );
		}
		
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestOpExplicitDouble_NotNumerics_Fail()
		{
			var result = ( System.Double )( new MessagePackObject( "A" ) );
		}
		
		[Test]
		public void TestOpExplicitSByte_SinglePlusOne()
		{
			var result = ( System.SByte )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.SByte )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSByte_SingleMinusOne()
		{
			var result = ( System.SByte )( new MessagePackObject( ( System.Single )( -1 ) ) );
			Assert.AreEqual( ( System.SByte )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSByte_DoublePlusOne()
		{
			var result = ( System.SByte )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.SByte )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSByte_DoubleMinusOne()
		{
			var result = ( System.SByte )( new MessagePackObject( ( System.Double )( -1 ) ) );
			Assert.AreEqual( ( System.SByte )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt16_SinglePlusOne()
		{
			var result = ( System.Int16 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.Int16 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt16_SingleMinusOne()
		{
			var result = ( System.Int16 )( new MessagePackObject( ( System.Single )( -1 ) ) );
			Assert.AreEqual( ( System.Int16 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt16_DoublePlusOne()
		{
			var result = ( System.Int16 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.Int16 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt16_DoubleMinusOne()
		{
			var result = ( System.Int16 )( new MessagePackObject( ( System.Double )( -1 ) ) );
			Assert.AreEqual( ( System.Int16 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt32_SinglePlusOne()
		{
			var result = ( System.Int32 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.Int32 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt32_SingleMinusOne()
		{
			var result = ( System.Int32 )( new MessagePackObject( ( System.Single )( -1 ) ) );
			Assert.AreEqual( ( System.Int32 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt32_DoublePlusOne()
		{
			var result = ( System.Int32 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.Int32 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt32_DoubleMinusOne()
		{
			var result = ( System.Int32 )( new MessagePackObject( ( System.Double )( -1 ) ) );
			Assert.AreEqual( ( System.Int32 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt64_SinglePlusOne()
		{
			var result = ( System.Int64 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.Int64 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt64_SingleMinusOne()
		{
			var result = ( System.Int64 )( new MessagePackObject( ( System.Single )( -1 ) ) );
			Assert.AreEqual( ( System.Int64 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt64_DoublePlusOne()
		{
			var result = ( System.Int64 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.Int64 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitInt64_DoubleMinusOne()
		{
			var result = ( System.Int64 )( new MessagePackObject( ( System.Double )( -1 ) ) );
			Assert.AreEqual( ( System.Int64 )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitByte_SinglePlusOne()
		{
			var result = ( System.Byte )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.Byte )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitByte_DoublePlusOne()
		{
			var result = ( System.Byte )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.Byte )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt16_SinglePlusOne()
		{
			var result = ( System.UInt16 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.UInt16 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt16_DoublePlusOne()
		{
			var result = ( System.UInt16 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.UInt16 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt32_SinglePlusOne()
		{
			var result = ( System.UInt32 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.UInt32 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt32_DoublePlusOne()
		{
			var result = ( System.UInt32 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.UInt32 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt64_SinglePlusOne()
		{
			var result = ( System.UInt64 )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.UInt64 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitUInt64_DoublePlusOne()
		{
			var result = ( System.UInt64 )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.UInt64 )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSingle_SinglePlusOne()
		{
			var result = ( System.Single )( new MessagePackObject( ( System.Single )1 ) );
			Assert.AreEqual( ( System.Single )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSingle_SingleMinusOne()
		{
			var result = ( System.Single )( new MessagePackObject( ( System.Single )( -1 ) ) );
			Assert.AreEqual( ( System.Single )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSingle_DoublePlusOne()
		{
			var result = ( System.Single )( new MessagePackObject( ( System.Double )1 ) );
			Assert.AreEqual( ( System.Single )( 1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSingle_DoubleMinusOne()
		{
			var result = ( System.Single )( new MessagePackObject( ( System.Double )( -1 ) ) );
			Assert.AreEqual( ( System.Single )( -1 ), result );
		}
		
		[Test]
		public void TestOpExplicitSingle_PlusOne()
		{
			var result = ( System.Single )( new MessagePackObject( 1 ) );
			Assert.AreEqual( ( System.Single )( 1 ), result, ( System.Single )0 );
		}
		
		[Test]
		public void TestOpExplicitSingle_MinusOne()
		{
			var result = ( System.Single )( new MessagePackObject( -1 ) );
			Assert.AreEqual( ( System.Single )( -1 ), result, ( System.Single )0 );
		}
		
		[Test]
		public void TestOpExplicitDouble_PlusOne()
		{
			var result = ( System.Double )( new MessagePackObject( 1 ) );
			Assert.AreEqual( ( System.Double )( 1 ), result, ( System.Double )0 );
		}
		
		[Test]
		public void TestOpExplicitDouble_MinusOne()
		{
			var result = ( System.Double )( new MessagePackObject( -1 ) );
			Assert.AreEqual( ( System.Double )( -1 ), result, ( System.Double )0 );
		}
		
	}
}