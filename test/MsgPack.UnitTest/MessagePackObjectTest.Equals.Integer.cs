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
		public void TestEquals_Int640x8000000000000000_Int640x8000000000000000_True()
		{
			AssertEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x80000000_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x80000000_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x8000_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x8000_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160x8000_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x80_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x80_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160x80_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_SByte0x80_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640xf_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320xf_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160xf_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_SByte0xf_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_SByte0x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Byte0x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt160x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt320x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt640x0_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_SByte0x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Byte0x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt160x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt320x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt640x1_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int640x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int320x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Int160x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int640x8000000000000000_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Int64 )( -9223372036854775808 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x80000000_True()
		{
			AssertEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x80000000_True()
		{
			AssertEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x8000_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x8000_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160x8000_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x80_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x80_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160x80_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_SByte0x80_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640xf_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320xf_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160xf_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_SByte0xf_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_SByte0x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Byte0x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt160x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt320x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt640x0_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_SByte0x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Byte0x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt160x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt320x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt640x1_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int640x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int320x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Int160x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int320x80000000_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Int32 )( -2147483648 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x80000000_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x80000000_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x8000_True()
		{
			AssertEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x8000_True()
		{
			AssertEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160x8000_True()
		{
			AssertEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x80_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x80_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160x80_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_SByte0x80_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640xf_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320xf_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160xf_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_SByte0xf_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_SByte0x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Byte0x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt160x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt320x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt640x0_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_SByte0x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Byte0x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt160x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt320x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt640x1_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int640x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int320x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Int160x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Int160x8000_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Int16 )( -32768 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x80000000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x80000000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x80_True()
		{
			AssertEquals( ( System.SByte )( -128 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x80_True()
		{
			AssertEquals( ( System.SByte )( -128 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160x80_True()
		{
			AssertEquals( ( System.SByte )( -128 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_SByte0x80_True()
		{
			AssertEquals( ( System.SByte )( -128 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640xf_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320xf_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160xf_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_SByte0xf_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_SByte0x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Byte0x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt160x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt320x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt640x0_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_SByte0x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Byte0x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt160x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt320x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt640x1_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int640x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int320x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Int160x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0x80_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.SByte )( -128 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x80000000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x80000000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160x8000_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x80_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x80_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160x80_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_SByte0x80_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640xf_True()
		{
			AssertEquals( ( System.SByte )( -1 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320xf_True()
		{
			AssertEquals( ( System.SByte )( -1 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160xf_True()
		{
			AssertEquals( ( System.SByte )( -1 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_SByte0xf_True()
		{
			AssertEquals( ( System.SByte )( -1 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_SByte0x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Byte0x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt160x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt320x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt640x0_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_SByte0x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Byte0x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt160x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt320x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt640x1_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int640x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int320x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Int160x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_SByte0xf_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.SByte )( -1 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x80_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x80_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160x80_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_SByte0x80_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640xf_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320xf_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160xf_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_SByte0xf_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_SByte0x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Byte0x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt160x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt320x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt640x0_True()
		{
			AssertEquals( ( System.Byte )( 0 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_SByte0x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Byte0x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt160x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt320x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt640x1_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int640x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int320x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Int160x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Byte )( 0 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x80_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x80_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160x80_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_SByte0x80_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640xf_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320xf_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160xf_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_SByte0xf_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_SByte0x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Byte0x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt160x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt320x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt640x0_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_SByte0x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Byte0x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt160x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt320x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt640x1_True()
		{
			AssertEquals( ( System.Byte )( 1 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int640x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int320x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Int160x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x1_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x80000000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160x8000_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x80_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x80_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160x80_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_SByte0x80_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640xf_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320xf_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160xf_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_SByte0xf_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_SByte0x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Byte0x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt160x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt320x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt640x0_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_SByte0x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Byte0x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt160x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt320x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt640x1_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int640x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int320x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Int160x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_Byte0x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt160x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt320x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt640x0ff_True()
		{
			AssertEquals( ( System.Byte )( 255 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_Byte0x0ff_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.Byte )( 255 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x80000000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x80000000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x8000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x8000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160x8000_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x80_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x80_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160x80_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_SByte0x80_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640xf_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320xf_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160xf_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_SByte0xf_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_SByte0x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Byte0x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt160x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt320x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt640x0_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_SByte0x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Byte0x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt160x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt320x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt640x1_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int640x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int320x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Int160x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt160x0_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.UInt16 )( 0 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x80000000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x80000000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x8000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x8000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160x8000_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x80_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x80_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160x80_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_SByte0x80_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640xf_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320xf_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160xf_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_SByte0xf_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_SByte0x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Byte0x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt160x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt320x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt640x0_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_SByte0x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Byte0x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt160x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt320x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt640x1_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int640x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int320x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Int160x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt320x0_UInt640x0ffffffffffffffff_False()
		{
			AssertNotEquals( ( System.UInt32 )( 0 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x8000000000000000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( -9223372036854775808 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x80000000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x80000000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( -2147483648 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x8000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x8000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160x8000_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( -32768 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x80_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x80_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160x80_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_SByte0x80_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.SByte )( -128 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640xf_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320xf_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160xf_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_SByte0xf_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.SByte )( -1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_SByte0x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.SByte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Byte0x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Byte )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt160x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt16 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt320x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt32 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt640x0_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt64 )( 0 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_SByte0x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.SByte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Byte0x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Byte )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt160x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt16 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt320x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt32 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt640x1_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt64 )( 1 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int640x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int320x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Int160x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Int16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_Byte0x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.Byte )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt160x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt16 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt320x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt32 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt640x0ff_False()
		{
			AssertNotEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt64 )( 255 ) );
		}
		
		[Test]
		public void TestEquals_UInt640x0ffffffffffffffff_UInt640x0ffffffffffffffff_True()
		{
			AssertEquals( ( System.UInt64 )( 18446744073709551615 ), ( System.UInt64 )( 18446744073709551615 ) );
		}
		
		[Test]
		public void TestEqualsInt64_Nil_False()
		{
			AssertNotEquals( ( System.Int64 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsInt64MinusOne_Double_True()
		{
			AssertEquals( ( System.Int64 )( -1 ), -1.0 );
		}

		[Test]
		public void TestEqualsInt64MinusOne_Single_True()
		{
			AssertEquals( ( System.Int64 )( -1 ), -1.0f );
		}
		[Test]
		public void TestEqualsInt64Zero_Double_True()
		{
			AssertEquals( ( System.Int64 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsInt64Zero_Single_True()
		{
			AssertEquals( ( System.Int64 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsInt64PlusOne_Double_True()
		{
			AssertEquals( ( System.Int64 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsInt64PlusOne_Single_True()
		{
			AssertEquals( ( System.Int64 )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsInt32_Nil_False()
		{
			AssertNotEquals( ( System.Int32 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsInt32MinusOne_Double_True()
		{
			AssertEquals( ( System.Int32 )( -1 ), -1.0 );
		}

		[Test]
		public void TestEqualsInt32MinusOne_Single_True()
		{
			AssertEquals( ( System.Int32 )( -1 ), -1.0f );
		}
		[Test]
		public void TestEqualsInt32Zero_Double_True()
		{
			AssertEquals( ( System.Int32 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsInt32Zero_Single_True()
		{
			AssertEquals( ( System.Int32 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsInt32PlusOne_Double_True()
		{
			AssertEquals( ( System.Int32 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsInt32PlusOne_Single_True()
		{
			AssertEquals( ( System.Int32 )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsInt16_Nil_False()
		{
			AssertNotEquals( ( System.Int16 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsInt16MinusOne_Double_True()
		{
			AssertEquals( ( System.Int16 )( -1 ), -1.0 );
		}

		[Test]
		public void TestEqualsInt16MinusOne_Single_True()
		{
			AssertEquals( ( System.Int16 )( -1 ), -1.0f );
		}
		[Test]
		public void TestEqualsInt16Zero_Double_True()
		{
			AssertEquals( ( System.Int16 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsInt16Zero_Single_True()
		{
			AssertEquals( ( System.Int16 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsInt16PlusOne_Double_True()
		{
			AssertEquals( ( System.Int16 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsInt16PlusOne_Single_True()
		{
			AssertEquals( ( System.Int16 )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsSByte_Nil_False()
		{
			AssertNotEquals( ( System.SByte )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsSByteMinusOne_Double_True()
		{
			AssertEquals( ( System.SByte )( -1 ), -1.0 );
		}

		[Test]
		public void TestEqualsSByteMinusOne_Single_True()
		{
			AssertEquals( ( System.SByte )( -1 ), -1.0f );
		}
		[Test]
		public void TestEqualsSByteZero_Double_True()
		{
			AssertEquals( ( System.SByte )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsSByteZero_Single_True()
		{
			AssertEquals( ( System.SByte )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsSBytePlusOne_Double_True()
		{
			AssertEquals( ( System.SByte )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsSBytePlusOne_Single_True()
		{
			AssertEquals( ( System.SByte )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsByte_Nil_False()
		{
			AssertNotEquals( ( System.Byte )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsByteZero_Double_True()
		{
			AssertEquals( ( System.Byte )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsByteZero_Single_True()
		{
			AssertEquals( ( System.Byte )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsBytePlusOne_Double_True()
		{
			AssertEquals( ( System.Byte )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsBytePlusOne_Single_True()
		{
			AssertEquals( ( System.Byte )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsUInt16_Nil_False()
		{
			AssertNotEquals( ( System.UInt16 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsUInt16Zero_Double_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsUInt16Zero_Single_True()
		{
			AssertEquals( ( System.UInt16 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsUInt16PlusOne_Double_True()
		{
			AssertEquals( ( System.UInt16 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsUInt16PlusOne_Single_True()
		{
			AssertEquals( ( System.UInt16 )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsUInt32_Nil_False()
		{
			AssertNotEquals( ( System.UInt32 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsUInt32Zero_Double_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsUInt32Zero_Single_True()
		{
			AssertEquals( ( System.UInt32 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsUInt32PlusOne_Double_True()
		{
			AssertEquals( ( System.UInt32 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsUInt32PlusOne_Single_True()
		{
			AssertEquals( ( System.UInt32 )( 1 ), 1.0f );
		}
		
		[Test]
		public void TestEqualsUInt64_Nil_False()
		{
			AssertNotEquals( ( System.UInt64 )( 1 ), MessagePackObject.Nil );
		}

		[Test]
		public void TestEqualsUInt64Zero_Double_True()
		{
			AssertEquals( ( System.UInt64 )( 0 ), -0.0 );
		}

		[Test]
		public void TestEqualsUInt64Zero_Single_True()
		{
			AssertEquals( ( System.UInt64 )( 0 ), -0.0f );
		}
		
		[Test]
		public void TestEqualsUInt64PlusOne_Double_True()
		{
			AssertEquals( ( System.UInt64 )( 1 ), 1.0 );
		}

		[Test]
		public void TestEqualsUInt64PlusOne_Single_True()
		{
			AssertEquals( ( System.UInt64 )( 1 ), 1.0f );
		}
		
	}
}
