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
	// This file was generated from ByteArrayUnpackerTest.Scalar.tt T4Template.
	// Do not modify this file. Edit ByteArrayUnpackerTest.Scalar.tt instead.

	partial class ByteArrayUnpackerTest
	{

		[Test]
		public void TestRead_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadInt64_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				Assert.IsTrue( unpacker.ReadInt64( out result ) );
				Assert.That( result, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadUInt64_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				Assert.IsTrue( unpacker.ReadUInt64( out result ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBoolean_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBoolean_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean result;
				Assert.IsTrue( unpacker.ReadBoolean( out result ) );
				Assert.That( result, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result.Value, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MaxValue.Equals( ( System.Single )result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadSingle_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single result;
				Assert.IsTrue( unpacker.ReadSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( Single.MaxValue.Equals( result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MaxValue.Equals( ( System.Double )result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadDouble_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double result;
				Assert.IsTrue( unpacker.ReadDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( Double.MaxValue.Equals( result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableBoolean_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				Assert.IsTrue( unpacker.ReadNullableBoolean( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableSingle_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single? result;
				Assert.IsTrue( unpacker.ReadNullableSingle( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableDouble_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double? result;
				Assert.IsTrue( unpacker.ReadNullableDouble( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableSByte_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				SByte? result;
				Assert.IsTrue( unpacker.ReadNullableSByte( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt16_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int16? result;
				Assert.IsTrue( unpacker.ReadNullableInt16( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt32_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int32? result;
				Assert.IsTrue( unpacker.ReadNullableInt32( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableInt64_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int64? result;
				Assert.IsTrue( unpacker.ReadNullableInt64( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableByte_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte? result;
				Assert.IsTrue( unpacker.ReadNullableByte( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt16_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt16? result;
				Assert.IsTrue( unpacker.ReadNullableUInt16( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt32_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt32? result;
				Assert.IsTrue( unpacker.ReadNullableUInt32( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadNullableUInt64_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt64? result;
				Assert.IsTrue( unpacker.ReadNullableUInt64( out result ) );
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int64MinValue_Extra()
		{
			var data = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int32MinValue_Extra()
		{
			var data = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -2147483648 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Int16MinValue_Extra()
		{
			var data = new byte[] { 0xD1, 0x80, 0x00 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32768 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_SByteMinValue_Extra()
		{
			var data = new byte[] { 0xD0, 0x80 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -128 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_NegativeFixNumMinValue_Extra()
		{
			var data = new byte[] { 0xE0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -32 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Int64 )result.Value, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadInt64Async_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64 result;
				var ret = await unpacker.ReadInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_MinusOne_Extra()
		{
			var data = new byte[] { 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( -1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_Zero_Extra()
		{
			var data = new byte[] { 0x0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PlusOne_Extra()
		{
			var data = new byte[] { 0x1 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_PositiveFixNumMaxValue_Extra()
		{
			var data = new byte[] { 0x7F };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 127 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_ByteMaxValue_Extra()
		{
			var data = new byte[] { 0xCC, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt16MaxValue_Extra()
		{
			var data = new byte[] { 0xCD, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt32MaxValue_Extra()
		{
			var data = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 4294967295 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.UInt64 )result.Value, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadUInt64Async_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64 result;
				var ret = await unpacker.ReadUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_UInt64MaxValue_Extra()
		{
			var data = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );
				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset -1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBooleanAsync_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanTrue_Extra()
		{
			var data = new byte[] { 0xC3 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( true ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( System.Boolean )result.Value, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBooleanAsync_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean result;
				var ret = await unpacker.ReadBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_BooleanFalse_Extra()
		{
			var data = new byte[] { 0xC2 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.Value, Is.EqualTo( false ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Single.MaxValue.Equals( ( System.Single )result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadSingleAsync_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single result;
				var ret = await unpacker.ReadSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_SingleMaxValue_Extra()
		{
			var data = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Single.MaxValue.Equals( result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( Double.MaxValue.Equals( ( System.Double )result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadDoubleAsync_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double result;
				var ret = await unpacker.ReadDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_DoubleMaxValue_Extra()
		{
			var data = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( Double.MaxValue.Equals( result.Value ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableBooleanAsync_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Boolean? result;
				var ret = await unpacker.ReadNullableBooleanAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableSingleAsync_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Single? result;
				var ret = await unpacker.ReadNullableSingleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableDoubleAsync_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Double? result;
				var ret = await unpacker.ReadNullableDoubleAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableSByteAsync_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				SByte? result;
				var ret = await unpacker.ReadNullableSByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt16Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int16? result;
				var ret = await unpacker.ReadNullableInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt32Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int32? result;
				var ret = await unpacker.ReadNullableInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableInt64Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Int64? result;
				var ret = await unpacker.ReadNullableInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableByteAsync_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte? result;
				var ret = await unpacker.ReadNullableByteAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt16Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt16? result;
				var ret = await unpacker.ReadNullableUInt16Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt32Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt32? result;
				var ret = await unpacker.ReadNullableUInt32Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadNullableUInt64Async_Extra()
		{
			var data = new byte[] { 0xC0 };
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				UInt64? result;
				var ret = await unpacker.ReadNullableUInt64Async();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.Null );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#endif // FEATURE_TAP

	}
}
