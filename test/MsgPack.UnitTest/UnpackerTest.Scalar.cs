#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
	}
}