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
	partial class MessagePackObjectTest_Exceptionals
	{
		private void Call<T>( T value )
		{
			// Nop
		}

		[Test]
		public void TestOpExplicitBoolean_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Boolean )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitBoolean_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Boolean )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitSByte_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitSByte_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt16_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt16_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt32_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt32_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt64_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int64 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitInt64_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int64 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitByte_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitByte_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt16_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt16_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt32_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt32_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt64_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitUInt64_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitSingle_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Single )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitSingle_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Single )( new MessagePackObject( "A" ) ) ) );
		}
		
		[Test]
		public void TestOpExplicitDouble_Nil_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Double )( MessagePackObject.Nil ) ) );
		}
		
		[Test]
		public void TestOpExplicitDouble_NotNumerics_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Double )( new MessagePackObject( "A" ) ) ) );
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
		public void TestOpExplicitSByte_SByteMinValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( SByte.MinValue ) );
			Assert.AreEqual( ( System.SByte )( SByte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_SByteMaxValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.SByte )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_Int16MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int16.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_Int16MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int16.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_Int32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_ByteMinValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.SByte )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_ByteMaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( Byte.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_UInt16MinValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.SByte )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_UInt16MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( UInt16.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_UInt32MinValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.SByte )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_UInt32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( UInt32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitSByte_UInt64MinValue_Success()
		{
			var result = ( System.SByte )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.SByte )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitSByte_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.SByte )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitInt16_SByteMinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( SByte.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( SByte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_SByteMaxValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.Int16 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_Int16MinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( Int16.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( Int16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_Int16MaxValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.Int16 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_Int32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( Int32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_ByteMinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_ByteMaxValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.Int16 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_UInt16MinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_UInt16MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( UInt16.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_UInt32MinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_UInt32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( UInt32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt16_UInt64MinValue_Success()
		{
			var result = ( System.Int16 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.Int16 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt16_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int16 )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitInt32_SByteMinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( SByte.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( SByte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_SByteMaxValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.Int32 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_Int16MinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Int16.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( Int16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_Int16MaxValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.Int32 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_Int32MinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Int32.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( Int32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_Int32MaxValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Int32.MaxValue ) );
			Assert.AreEqual( ( System.Int32 )( Int32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt32_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt32_ByteMinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_ByteMaxValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.Int32 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_UInt16MinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_UInt16MaxValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( UInt16.MaxValue ) );
			Assert.AreEqual( ( System.Int32 )( UInt16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_UInt32MinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_UInt32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( new MessagePackObject( UInt32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitInt32_UInt64MinValue_Success()
		{
			var result = ( System.Int32 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.Int32 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt32_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int32 )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitInt64_SByteMinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( SByte.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( SByte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_SByteMaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int16MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int16.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( Int16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int16MaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int32MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int32.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( Int32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int32MaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int32.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( Int32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int64MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int64.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( Int64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_Int64MaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Int64.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( Int64.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_ByteMinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_ByteMaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt16MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt16MaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( UInt16.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( UInt16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt32MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt32MaxValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( UInt32.MaxValue ) );
			Assert.AreEqual( ( System.Int64 )( UInt32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt64MinValue_Success()
		{
			var result = ( System.Int64 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.Int64 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitInt64_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Int64 )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitByte_SByteMinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( SByte.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_SByteMaxValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.Byte )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_Int16MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int16.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_Int16MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int16.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_Int32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_ByteMinValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.Byte )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_ByteMaxValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.Byte )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_UInt16MinValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.Byte )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_UInt16MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( UInt16.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_UInt32MinValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.Byte )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_UInt32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( UInt32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitByte_UInt64MinValue_Success()
		{
			var result = ( System.Byte )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.Byte )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitByte_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.Byte )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitUInt16_SByteMinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( SByte.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_SByteMaxValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.UInt16 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_Int16MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( Int16.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_Int16MaxValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.UInt16 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_Int32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( Int32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_ByteMinValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.UInt16 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_ByteMaxValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.UInt16 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt16MinValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.UInt16 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt16MaxValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( UInt16.MaxValue ) );
			Assert.AreEqual( ( System.UInt16 )( UInt16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt32MinValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.UInt16 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt32MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( UInt32.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt64MinValue_Success()
		{
			var result = ( System.UInt16 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.UInt16 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt16_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt16 )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitUInt32_SByteMinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( SByte.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt32_SByteMaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_Int16MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( Int16.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt32_Int16MaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt32_Int32MaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( Int32.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( Int32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt32_Int64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( Int64.MaxValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt32_ByteMinValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.UInt32 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_ByteMaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt16MinValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.UInt32 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt16MaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( UInt16.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( UInt16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt32MinValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.UInt32 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt32MaxValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( UInt32.MaxValue ) );
			Assert.AreEqual( ( System.UInt32 )( UInt32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt64MinValue_Success()
		{
			var result = ( System.UInt32 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.UInt32 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt32_UInt64MaxValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt32 )( new MessagePackObject( UInt64.MaxValue ) ) ) );
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
		public void TestOpExplicitUInt64_SByteMinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( new MessagePackObject( SByte.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt64_SByteMaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( SByte.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( SByte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_Int16MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( new MessagePackObject( Int16.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt64_Int16MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( Int16.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( Int16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_Int32MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( new MessagePackObject( Int32.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt64_Int32MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( Int32.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( Int32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_Int64MinValue_Fail()
		{
			Assert.Throws<InvalidOperationException>( () => Call( ( System.UInt64 )( new MessagePackObject( Int64.MinValue ) ) ) );
		}

		[Test]
		public void TestOpExplicitUInt64_Int64MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( Int64.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( Int64.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_ByteMinValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( Byte.MinValue ) );
			Assert.AreEqual( ( System.UInt64 )( Byte.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_ByteMaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( Byte.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( Byte.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt16MinValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt16.MinValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt16.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt16MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt16.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt16.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt32MinValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt32.MinValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt32.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt32MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt32.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt32.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt64MinValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt64.MinValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt64.MinValue ), result );
		}

		[Test]
		public void TestOpExplicitUInt64_UInt64MaxValue_Success()
		{
			var result = ( System.UInt64 )( new MessagePackObject( UInt64.MaxValue ) );
			Assert.AreEqual( ( System.UInt64 )( UInt64.MaxValue ), result );
		}

		[Test]
		public void TestOpExplicitSingle_UInt64MaxValue()
		{
			var result = ( System.Single )( new MessagePackObject( UInt64.MaxValue ) );
			Assert.AreEqual( ( System.Single )( UInt64.MaxValue ), result, ( System.Single )0 );
		}
		
		[Test]
		public void TestOpExplicitSingle_Int64MinValue()
		{
			var result = ( System.Single )( new MessagePackObject( Int64.MinValue ) );
			Assert.AreEqual( ( System.Single )( Int64.MinValue ), result, ( System.Single )0 );
		}
		
		[Test]
		public void TestOpExplicitDouble_UInt64MaxValue()
		{
			var result = ( System.Double )( new MessagePackObject( UInt64.MaxValue ) );
			Assert.AreEqual( ( System.Double )( UInt64.MaxValue ), result, ( System.Double )0 );
		}
		
		[Test]
		public void TestOpExplicitDouble_Int64MinValue()
		{
			var result = ( System.Double )( new MessagePackObject( Int64.MinValue ) );
			Assert.AreEqual( ( System.Double )( Int64.MinValue ), result, ( System.Double )0 );
		}
		
	}
}