#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Threading.Tasks;
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

	partial class UnpackerTest
	{

		[Test]
		public void TestRead_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestRead_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestRead_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestRead_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestRead_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestRead_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestRead_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestRead_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestRead_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestRead_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadInt64_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestRead_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestRead_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestRead_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestRead_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestRead_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestRead_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadUInt64_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestRead_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadBoolean_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestRead_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadBoolean_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestRead_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadSingle_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
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
		public void TestReadDouble_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableSingle()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableDouble()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableSByte()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				SByte? result;
				Assert.IsTrue( unpacker.ReadNullableSByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt16()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int16? result;
				Assert.IsTrue( unpacker.ReadNullableInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt32()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int32? result;
				Assert.IsTrue( unpacker.ReadNullableInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableByte()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte? result;
				Assert.IsTrue( unpacker.ReadNullableByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt16()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt16? result;
				Assert.IsTrue( unpacker.ReadNullableUInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt32()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt32? result;
				Assert.IsTrue( unpacker.ReadNullableUInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt64()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_SByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_SByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_NegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_NegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_MinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_Zero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_ByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_ByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public async Task TestReadBooleanAsync_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanTrue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public async Task TestReadBooleanAsync_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanFalse()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MinValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MaxValue.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.Epsilon.Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsNegativeInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.IsPositiveInfinity( ( System.Single )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MinValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MaxValue.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.Epsilon.Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( 0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( -0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsNegativeInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.IsPositiveInfinity( ( System.Double )result.Value ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableSByteAsync()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				SByte? result;
				var ret = await unpacker.ReadNullableSByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt16Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int16? result;
				var ret = await unpacker.ReadNullableInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt32Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int32? result;
				var ret = await unpacker.ReadNullableInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableByteAsync()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte? result;
				var ret = await unpacker.ReadNullableByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt16Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt16? result;
				var ret = await unpacker.ReadNullableUInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt32Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt32? result;
				var ret = await unpacker.ReadNullableUInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

#endif // FEATURE_TAP


		[Test]
		public void TestReadSingleFromPackedPositiveFixNum_Minimum_Normal()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				buffer.Position = 0;
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
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
				using( var unpacker = this.CreateUnpacker( buffer ) )
				{
					UInt64 result;
					Assert.That( unpacker.ReadUInt64( out result ) );
					Assert.That( result, Is.EqualTo( ( UInt64 )( UInt64.MaxValue ) ) );
				}
			}
		}
	}
}
