#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
	// This file was generated from StreamUnpackerTest.Scalar.tt T4Template.
	// Do not modify this file. Edit StreamUnpackerTest.Scalar.tt instead.

	partial class StreamUnpackerTest
	{

		[Test]
		public void TestRead_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public void TestRead_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public void TestRead_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public void TestRead_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public void TestRead_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public void TestRead_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public void TestRead_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public void TestRead_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public void TestRead_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public void TestRead_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadInt64_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestRead_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public void TestRead_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public void TestRead_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestRead_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public void TestRead_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public void TestRead_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadUInt64_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public void TestRead_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadBoolean_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestRead_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadBoolean_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public void TestRead_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadSingle_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestRead_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public void TestReadDouble_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableSingle_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableDouble_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableSByte_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				SByte? result;
				Assert.IsTrue( unpacker.ReadNullableSByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt16_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int16? result;
				Assert.IsTrue( unpacker.ReadNullableInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt32_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int32? result;
				Assert.IsTrue( unpacker.ReadNullableInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableByte_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Byte? result;
				Assert.IsTrue( unpacker.ReadNullableByte( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt16_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt16? result;
				Assert.IsTrue( unpacker.ReadNullableUInt16( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt32_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt32? result;
				Assert.IsTrue( unpacker.ReadNullableUInt32( out result ) );
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public void TestReadNullableUInt64_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result, Is.Null );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int64MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int32MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int32MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int16MinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32769 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int16MinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32768 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_SByteMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -129 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_SByteMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -128 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_NegativeFixNumMinValueMinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -33 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_NegativeFixNumMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadInt64Async_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_MinusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_Zero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PositiveFixNumMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 127 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PositiveFixNumMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 128 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_ByteMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_ByteMaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt16MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt16MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt32MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt32MaxValuePlusOne_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadUInt64Async_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt64MaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadBooleanAsync_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( true ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanTrue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadBooleanAsync_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( false ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanFalse_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( false ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SinglePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0f ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadSingleAsync_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SinglePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MinValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleEpsilon_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.Epsilon.Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoublePositiveZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( 0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNegativeZero_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( ( -0.0 ).Equals( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNPositiveMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNPositiveMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNNegativeMinValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNaNNegativeMaxValue_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNaN( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleNegativeInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsNegativeInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
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
		public async Task TestReadDoubleAsync_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoublePositiveInfinity_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.IsPositiveInfinity( result.Value ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableSByteAsync_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				SByte? result;
				var ret = await unpacker.ReadNullableSByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt16Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int16? result;
				var ret = await unpacker.ReadNullableInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt32Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int32? result;
				var ret = await unpacker.ReadNullableInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableByteAsync_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				Byte? result;
				var ret = await unpacker.ReadNullableByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt16Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt16? result;
				var ret = await unpacker.ReadNullableUInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt32Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt32? result;
				var ret = await unpacker.ReadNullableUInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_Splitted()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = this.CreateUnpacker( splitted ) )
			{
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );
			}
		}

#endif // FEATURE_TAP

	}
}
