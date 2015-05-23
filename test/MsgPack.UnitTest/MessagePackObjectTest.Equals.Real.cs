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
	partial class MessagePackObjectTest_Equals
	{
		[Test]
		public void TestEquals_DoubleMinValue_DoubleMinValue_True()
		{
			AssertEquals( ( Double.MinValue ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_SingleMinValue_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_MinusSingleEpsilon_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_MinusDoubleEpsilon_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_DoubleZero_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_DoubleEpsilon_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_SingleEpsilon_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_SingleMaxValue_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_DoubleMaxValue_False()
		{
			AssertNotEquals( ( Double.MinValue ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMinValue_Nil_False()
		{
			AssertNotEquals( ( Double.MinValue ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_SingleMinValue_DoubleMinValue_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_SingleMinValue_True()
		{
			AssertEquals( ( Single.MinValue ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_MinusSingleEpsilon_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_MinusDoubleEpsilon_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_DoubleZero_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_DoubleEpsilon_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_SingleEpsilon_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_SingleMaxValue_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_DoubleMaxValue_False()
		{
			AssertNotEquals( ( Single.MinValue ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMinValue_Nil_False()
		{
			AssertNotEquals( ( Single.MinValue ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_MinusSingleEpsilon_DoubleMinValue_False()
		{
			AssertNotEquals( ( -Single.Epsilon ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_SingleMinValue_False()
		{
			AssertNotEquals( ( -Single.Epsilon ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_MinusSingleEpsilon_True()
		{
			AssertEquals( ( -Single.Epsilon ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_MinusDoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Single.Epsilon ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_DoubleZero_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Single.Epsilon ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_DoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Single.Epsilon ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_SingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Single.Epsilon ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_SingleMaxValue_False()
		{
			AssertNotEquals( ( -Single.Epsilon ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_DoubleMaxValue_False()
		{
			AssertNotEquals( ( -Single.Epsilon ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_MinusSingleEpsilon_Nil_False()
		{
			AssertNotEquals( ( -Single.Epsilon ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_MinusDoubleEpsilon_DoubleMinValue_False()
		{
			AssertNotEquals( ( -Double.Epsilon ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_SingleMinValue_False()
		{
			AssertNotEquals( ( -Double.Epsilon ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_MinusSingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Double.Epsilon ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_MinusDoubleEpsilon_True()
		{
			AssertEquals( ( -Double.Epsilon ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_DoubleZero_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Double.Epsilon ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_DoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Double.Epsilon ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_SingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( -Double.Epsilon ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_SingleMaxValue_False()
		{
			AssertNotEquals( ( -Double.Epsilon ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_DoubleMaxValue_False()
		{
			AssertNotEquals( ( -Double.Epsilon ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_MinusDoubleEpsilon_Nil_False()
		{
			AssertNotEquals( ( -Double.Epsilon ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_DoubleZero_DoubleMinValue_False()
		{
			AssertNotEquals( ( 0.0 ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_SingleMinValue_False()
		{
			AssertNotEquals( ( 0.0 ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_MinusSingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( 0.0 ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_MinusDoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( 0.0 ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_DoubleZero_True()
		{
			AssertEquals( ( 0.0 ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_DoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( 0.0 ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_SingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( 0.0 ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_SingleMaxValue_False()
		{
			AssertNotEquals( ( 0.0 ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_DoubleMaxValue_False()
		{
			AssertNotEquals( ( 0.0 ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleZero_Nil_False()
		{
			AssertNotEquals( ( 0.0 ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_DoubleEpsilon_DoubleMinValue_False()
		{
			AssertNotEquals( ( Double.Epsilon ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_SingleMinValue_False()
		{
			AssertNotEquals( ( Double.Epsilon ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_MinusSingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Double.Epsilon ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_MinusDoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Double.Epsilon ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_DoubleZero_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Double.Epsilon ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_DoubleEpsilon_True()
		{
			AssertEquals( ( Double.Epsilon ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_SingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Double.Epsilon ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_SingleMaxValue_False()
		{
			AssertNotEquals( ( Double.Epsilon ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_DoubleMaxValue_False()
		{
			AssertNotEquals( ( Double.Epsilon ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleEpsilon_Nil_False()
		{
			AssertNotEquals( ( Double.Epsilon ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_SingleEpsilon_DoubleMinValue_False()
		{
			AssertNotEquals( ( Single.Epsilon ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_SingleMinValue_False()
		{
			AssertNotEquals( ( Single.Epsilon ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_MinusSingleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Single.Epsilon ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_MinusDoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Single.Epsilon ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_DoubleZero_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Single.Epsilon ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_DoubleEpsilon_False()
		{
			CheckEpsilonComparison();
			AssertNotEquals( ( Single.Epsilon ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_SingleEpsilon_True()
		{
			AssertEquals( ( Single.Epsilon ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_SingleMaxValue_False()
		{
			AssertNotEquals( ( Single.Epsilon ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_DoubleMaxValue_False()
		{
			AssertNotEquals( ( Single.Epsilon ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleEpsilon_Nil_False()
		{
			AssertNotEquals( ( Single.Epsilon ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_SingleMaxValue_DoubleMinValue_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_SingleMinValue_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_MinusSingleEpsilon_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_MinusDoubleEpsilon_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_DoubleZero_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_DoubleEpsilon_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_SingleEpsilon_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_SingleMaxValue_True()
		{
			AssertEquals( ( Single.MaxValue ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_DoubleMaxValue_False()
		{
			AssertNotEquals( ( Single.MaxValue ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_SingleMaxValue_Nil_False()
		{
			AssertNotEquals( ( Single.MaxValue ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_DoubleMaxValue_DoubleMinValue_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( Double.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_SingleMinValue_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( Single.MinValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_MinusSingleEpsilon_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( -Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_MinusDoubleEpsilon_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( -Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_DoubleZero_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( 0.0 ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_DoubleEpsilon_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( Double.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_SingleEpsilon_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( Single.Epsilon ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_SingleMaxValue_False()
		{
			AssertNotEquals( ( Double.MaxValue ), ( Single.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_DoubleMaxValue_True()
		{
			AssertEquals( ( Double.MaxValue ), ( Double.MaxValue ) );
		}
		
		[Test]
		public void TestEquals_DoubleMaxValue_Nil_False()
		{
			AssertNotEquals( ( Double.MaxValue ), MessagePackObject.Nil );
		}
		[Test]
		public void TestEquals_DoubleZero_SingleZero_True()
		{
			AssertEquals( 0.0, 0.0f );
		}
		[Test]
		public void TestEquals_DoubleZero_Byte_True()
		{
			AssertEquals( 0.0, ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_Byte_True()
		{
			AssertEquals( 0.0f, ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_Byte_True()
		{
			AssertEquals( 1.0, ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_Byte_True()
		{
			AssertEquals( 1.0f, ( System.Byte )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleMinusOne_SByte_True()
		{
			AssertEquals( -1.0, ( System.SByte )( -1 ) );
		}

		[Test]
		public void TestEquals_SingleMinusOne_SByte_True()
		{
			AssertEquals( -1.0f, ( System.SByte )( -1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_SByte_True()
		{
			AssertEquals( 0.0, ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_SByte_True()
		{
			AssertEquals( 0.0f, ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_SByte_True()
		{
			AssertEquals( 1.0, ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_SByte_True()
		{
			AssertEquals( 1.0f, ( System.SByte )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleMinusOne_Int16_True()
		{
			AssertEquals( -1.0, ( System.Int16 )( -1 ) );
		}

		[Test]
		public void TestEquals_SingleMinusOne_Int16_True()
		{
			AssertEquals( -1.0f, ( System.Int16 )( -1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_Int16_True()
		{
			AssertEquals( 0.0, ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_Int16_True()
		{
			AssertEquals( 0.0f, ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_Int16_True()
		{
			AssertEquals( 1.0, ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_Int16_True()
		{
			AssertEquals( 1.0f, ( System.Int16 )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_UInt16_True()
		{
			AssertEquals( 0.0, ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_UInt16_True()
		{
			AssertEquals( 0.0f, ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_UInt16_True()
		{
			AssertEquals( 1.0, ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_UInt16_True()
		{
			AssertEquals( 1.0f, ( System.UInt16 )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleMinusOne_Int32_True()
		{
			AssertEquals( -1.0, ( System.Int32 )( -1 ) );
		}

		[Test]
		public void TestEquals_SingleMinusOne_Int32_True()
		{
			AssertEquals( -1.0f, ( System.Int32 )( -1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_Int32_True()
		{
			AssertEquals( 0.0, ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_Int32_True()
		{
			AssertEquals( 0.0f, ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_Int32_True()
		{
			AssertEquals( 1.0, ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_Int32_True()
		{
			AssertEquals( 1.0f, ( System.Int32 )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_UInt32_True()
		{
			AssertEquals( 0.0, ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_UInt32_True()
		{
			AssertEquals( 0.0f, ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_UInt32_True()
		{
			AssertEquals( 1.0, ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_UInt32_True()
		{
			AssertEquals( 1.0f, ( System.UInt32 )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleMinusOne_Int64_True()
		{
			AssertEquals( -1.0, ( System.Int64 )( -1 ) );
		}

		[Test]
		public void TestEquals_SingleMinusOne_Int64_True()
		{
			AssertEquals( -1.0f, ( System.Int64 )( -1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_Int64_True()
		{
			AssertEquals( 0.0, ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_Int64_True()
		{
			AssertEquals( 0.0f, ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_Int64_True()
		{
			AssertEquals( 1.0, ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_Int64_True()
		{
			AssertEquals( 1.0f, ( System.Int64 )( 1 ) );
		}
		[Test]
		public void TestEquals_DoubleZero_UInt64_True()
		{
			AssertEquals( 0.0, ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleZero_UInt64_True()
		{
			AssertEquals( 0.0f, ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SingleOne_UInt64_True()
		{
			AssertEquals( 1.0, ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_DoubleOne_UInt64_True()
		{
			AssertEquals( 1.0f, ( System.UInt64 )( 1 ) );
		}

		private static void CheckEpsilonComparison()
		{
			// Use Equals instead of == to avoid compiler optmization for constant expression
			if ( Double.Epsilon.Equals( 0.0 ) )
			{
				// Xamarin iOS comes here.
				Assert.Inconclusive( "Comparison of Double.Epsilon cannot be checked on this platform." );
			}

			// Use Equals instead of == to avoid compiler optmization for constant expression
			if ( Single.Epsilon.Equals( 0.0f ) )
			{
				// Xamarin iOS comes here.
				Assert.Inconclusive( "Comparison of Single.Epsilon cannot be checked on this platform." );
			}
		}
	}
}