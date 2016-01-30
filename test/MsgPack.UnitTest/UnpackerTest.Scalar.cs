#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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

namespace MsgPack
{
	// This file was generated from UnpackerTest.Scalar.tt T4Template.
	// Do not modify this file. Edit UnpackerTest.Scalar.tt instead.

	[TestFixture]
	public partial class UnpackerTest_Scalar
	{
		// FIXME: Direct reading and direct/MPO mixing cases.

		[Test]
		public void TestUnpackInt64MinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt64MinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt64MinValue_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt64MinValue_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt64MinValue_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt64MinValue_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt32MinValue_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackInt16MinValue_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackSByteMinValue_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_ReadInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_ReadInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackMinusOne_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackZero_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackZero_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackZero_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackZero_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackZero_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackZero_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPlusOne_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValue_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValue_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValue_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_ReadUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_ReadUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackUInt64MaxValue_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_ReadBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_ReadBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_ReadNullableBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanTrue_ReadNullableBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_ReadBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_ReadBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_ReadNullableBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackBooleanFalse_ReadNullableBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MinValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MinValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMinValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MaxValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MaxValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleMaxValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.Epsilon.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.Epsilon.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleEpsilon_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveZero_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeZero_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNegativeInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNegativeInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsPositiveInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsPositiveInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_ReadSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_ReadSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MinValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MinValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMinValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MaxValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MaxValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleMaxValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.Epsilon.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.Epsilon.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleEpsilon_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveZero_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNegativeInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNegativeInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_Read()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsPositiveInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_Read_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsPositiveInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_ReadDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_ReadDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableSByte()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				SByte? result;
				Assert.IsTrue( unpacker.ReadNullableSByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableSByte_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				SByte? result;
				Assert.IsTrue( unpacker.ReadNullableSByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt16()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int16? result;
				Assert.IsTrue( unpacker.ReadNullableInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt16_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int16? result;
				Assert.IsTrue( unpacker.ReadNullableInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt32()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int32? result;
				Assert.IsTrue( unpacker.ReadNullableInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt32_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int32? result;
				Assert.IsTrue( unpacker.ReadNullableInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableByte()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte? result;
				Assert.IsTrue( unpacker.ReadNullableByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableByte_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte? result;
				Assert.IsTrue( unpacker.ReadNullableByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt16()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt16? result;
				Assert.IsTrue( unpacker.ReadNullableUInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt16_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt16? result;
				Assert.IsTrue( unpacker.ReadNullableUInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt32()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt32? result;
				Assert.IsTrue( unpacker.ReadNullableUInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt32_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt32? result;
				Assert.IsTrue( unpacker.ReadNullableUInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestUnpackNil_ReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadSingleFromPackedPositiveFixNum_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedPositiveFixNum_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( 0x7F ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedNegativeFixNum_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedNegativeFixNum_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( -1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt8_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt8_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( -32 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt8_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt8_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( Byte.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt16_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( Int16.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt16_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( SByte.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt16_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt16_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( UInt16.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt32_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( Int32.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt32_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( ( int )Int16.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt32_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt32_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( UInt32.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt64_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( Int64.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedInt64_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( ( long )Int32.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt64_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( ( ulong )UInt32.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedUInt64_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )( UInt64.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSingleFromPackedZero_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Single result;
					Assert.That( unpacker.ReadSingle( out result ) );
					Assert.That( result, Is.EqualTo( ( Single )0 ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedPositiveFixNum_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedPositiveFixNum_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( 0x7F ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedNegativeFixNum_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedNegativeFixNum_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( -1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt8_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt8_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( -32 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt8_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt8_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( Byte.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt16_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( Int16.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt16_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( SByte.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt16_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt16_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( UInt16.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt32_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( Int32.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt32_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( ( int )Int16.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt32_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt32_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( UInt32.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt64_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( Int64.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedInt64_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( ( long )Int32.MinValue - 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt64_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( ( ulong )UInt32.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedUInt64_Maximum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )( UInt64.MaxValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadDoubleFromPackedZero_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Double result;
					Assert.That( unpacker.ReadDouble( out result ) );
					Assert.That( result, Is.EqualTo( ( Double )0 ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedSingle_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( -1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedDouble_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( -1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( SByte )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedNegativeFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedNegativeFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( -1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					SByte result;
					Assert.That( unpacker.ReadSByte( out result ) );
					Assert.That( result, Is.EqualTo( ( SByte )( -32 ) ) );
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedUInt8_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedUInt8_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedUInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedUInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedUInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedUInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadSByteFromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadSByteFromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							SByte result;
							unpacker.ReadSByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedSingle_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( -1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedDouble_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( -1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int16 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedNegativeFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedNegativeFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( -1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( -32 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( Int16.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( SByte.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int16 result;
					Assert.That( unpacker.ReadInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( Int16 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedUInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedUInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedUInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt16FromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt16FromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int16 result;
							unpacker.ReadInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedSingle_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( -1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedDouble_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( -1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int32 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedNegativeFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedNegativeFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( -1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( -32 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( Int16.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( SByte.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedUInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( UInt16.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( Int32.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedInt32_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( ( int )Int16.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedUInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int32 result;
					Assert.That( unpacker.ReadInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( Int32 )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedUInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int32 result;
							unpacker.ReadInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int32 result;
							unpacker.ReadInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int32 result;
							unpacker.ReadInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt32FromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int32 result;
							unpacker.ReadInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadInt32FromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int32 result;
							unpacker.ReadInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedSingle_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( -1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedDouble_Negative_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( -1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Int64 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedNegativeFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( -31 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedNegativeFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( -1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( SByte.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( -32 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( Int16.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( SByte.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedUInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( UInt16.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( Int32.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedInt32_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( ( int )Int16.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedUInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedUInt32_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( UInt32.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedInt64_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( Int64.MinValue ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedInt64_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( ( long )Int32.MinValue - 1 ) ) );
				}
			}
		}
		[Test]
		public void TestReadInt64FromPackedUInt64_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Int64 result;
					Assert.That( unpacker.ReadInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( Int64 )( ( ulong )UInt32.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadInt64FromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Int64 result;
							unpacker.ReadInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedSingle_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedDouble_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( Byte )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedNegativeFixNum_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedNegativeFixNum_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedInt8_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedInt8_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Byte result;
					Assert.That( unpacker.ReadByte( out result ) );
					Assert.That( result, Is.EqualTo( ( Byte )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedUInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedUInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedUInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedUInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadByteFromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadByteFromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							Byte result;
							unpacker.ReadByte( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedSingle_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedDouble_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt16 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedNegativeFixNum_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedNegativeFixNum_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedInt8_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedInt8_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedUInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt16 result;
					Assert.That( unpacker.ReadUInt16( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt16 )( UInt16.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedUInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedUInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt16FromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt16FromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt16 result;
							unpacker.ReadUInt16( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedSingle_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedDouble_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt32 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedNegativeFixNum_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedNegativeFixNum_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedInt8_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedInt8_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedUInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( UInt16.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedUInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedUInt32_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt32 result;
					Assert.That( unpacker.ReadUInt32( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt32 )( UInt32.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt32FromPackedUInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt32FromPackedUInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt32 result;
							unpacker.ReadUInt32( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedSingle_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedSingle_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( 1.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedSingle_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( -0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedSingle_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( 0.1f ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedSingle_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0f );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )0.0f ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedDouble_Negative_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedDouble_Positive_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 1.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( 1.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedDouble_LittleNegative_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( -0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedDouble_LittlePositive_AsZero()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( unchecked( ( UInt64 )( 0.1 ) ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedDouble_Zero_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0.0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )0.0 ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedPositiveFixNum_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( 0 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedPositiveFixNum_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x7F );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( 0x7F ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedNegativeFixNum_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -31 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedNegativeFixNum_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedInt8_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedInt8_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( -32 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedUInt8_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0x80 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( 0x80 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedUInt8_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( Byte.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedInt16_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedInt16_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedUInt16_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ushort )Byte.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( ( ushort )Byte.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedUInt16_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( UInt16.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedInt32_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedInt32_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( int )Int16.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedUInt32_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( uint )UInt16.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( ( uint )UInt16.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedUInt32_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( UInt32.MaxValue ) ) );
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedInt64_MinValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedInt64_MaxValue_Overflow()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( long )Int32.MinValue - 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					Assert.Throws<OverflowException>( () =>
						{
							UInt64 result;
							unpacker.ReadUInt64( out result );
						}
					);
				}
			}
		}
		[Test]
		public void TestReadUInt64FromPackedUInt64_MinValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ulong )UInt32.MaxValue + 1 );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( ( ulong )UInt32.MaxValue + 1 ) ) );
				}
			}
		}

		[Test]
		public void TestReadUInt64FromPackedUInt64_MaxValue_AsInteger()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				buffer.Position = 0;
				using( var unpacker = Unpacker.Create( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( UInt64.MaxValue ) ) );
				}
			}
		}
	}
}