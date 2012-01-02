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
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	public partial class PackerTest_Scalar
	{
		[Test]
		public void TestUnpackInt64MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -9223372036854775808 );
			}
		}
		
		[Test]
		public void TestUnpackInt32MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -2147483649 );
			}
		}
		
		[Test]
		public void TestUnpackInt32MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -2147483648 );
			}
		}
		
		[Test]
		public void TestUnpackInt16MinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -32769 );
			}
		}
		
		[Test]
		public void TestUnpackInt16MinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -32768 );
			}
		}
		
		[Test]
		public void TestUnpackSByteMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -129 );
			}
		}
		
		[Test]
		public void TestUnpackSByteMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -128 );
			}
		}
		
		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -33 );
			}
		}
		
		[Test]
		public void TestUnpackNegativeFixNumMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -32 );
			}
		}
		
		[Test]
		public void TestUnpackMinusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.Int64 )result.Value, -1 );
			}
		}
		
		[Test]
		public void TestUnpackZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 0 );
			}
		}
		
		[Test]
		public void TestUnpackPlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 1 );
			}
		}
		
		[Test]
		public void TestUnpackPositiveFixNumMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 127 );
			}
		}
		
		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 128 );
			}
		}
		
		[Test]
		public void TestUnpackByteMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 255 );
			}
		}
		
		[Test]
		public void TestUnpackByteMaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 256 );
			}
		}
		
		[Test]
		public void TestUnpackUInt16MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 65535 );
			}
		}
		
		[Test]
		public void TestUnpackUInt16MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 65536 );
			}
		}
		
		[Test]
		public void TestUnpackUInt32MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 4294967295 );
			}
		}
		
		[Test]
		public void TestUnpackUInt32MaxValuePlusOne()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 4294967296 );
			}
		}
		
		[Test]
		public void TestUnpackUInt64MaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.AreEqual( ( System.UInt64 )result.Value, 18446744073709551615 );
			}
		}
		
		[Test]
		public void TestUnpackSingleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.MinValue.Equals( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.MaxValue.Equals( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.Epsilon.Equals( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSinglePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( ( 0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( ( -0.0f ).Equals( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsNaN( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsNegativeInfinity( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackSinglePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Single.IsPositiveInfinity( ( System.Single )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.MinValue.Equals( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.MaxValue.Equals( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleEpsilon()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.Epsilon.Equals( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoublePositiveZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( ( 0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNegativeZero()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( ( -0.0 ).Equals( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsNaN( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNegativeInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsNegativeInfinity( ( System.Double )result.Value ) );
			}
		}
		
		[Test]
		public void TestUnpackDoublePositiveInfinity()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.IsTrue( Double.IsPositiveInfinity( ( System.Double )result.Value ) );
			}
		}
		
	}
}