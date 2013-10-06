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
	public partial class UnpackingTest_Scalar
	{

		[Test]
		public void TestUnpackInt64MinValue_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -9223372036854775808 ) );
			}
		}
		
		[Test]
		public void TestUnpackInt64MinValue_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD3, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -9223372036854775808 ) );
		}

		[Test]
		public void TestUnpackInt32MinValueMinusOne_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -2147483649 ) );
			}
		}
		
		[Test]
		public void TestUnpackInt32MinValueMinusOne_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD3, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -2147483649 ) );
		}

		[Test]
		public void TestUnpackInt32MinValue_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -2147483648 ) );
			}
		}
		
		[Test]
		public void TestUnpackInt32MinValue_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD2, 0x80, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -2147483648 ) );
		}

		[Test]
		public void TestUnpackInt16MinValueMinusOne_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -32769 ) );
			}
		}
		
		[Test]
		public void TestUnpackInt16MinValueMinusOne_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD2, 0xFF, 0xFF, 0x7F, 0xFF };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -32769 ) );
		}

		[Test]
		public void TestUnpackInt16MinValue_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0x80, 0x00 } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -32768 ) );
			}
		}
		
		[Test]
		public void TestUnpackInt16MinValue_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD1, 0x80, 0x00 };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -32768 ) );
		}

		[Test]
		public void TestUnpackSByteMinValueMinusOne_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD1, 0xFF, 0x7F } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -129 ) );
			}
		}
		
		[Test]
		public void TestUnpackSByteMinValueMinusOne_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD1, 0xFF, 0x7F };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -129 ) );
		}

		[Test]
		public void TestUnpackSByteMinValue_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0x80 } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -128 ) );
			}
		}
		
		[Test]
		public void TestUnpackSByteMinValue_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD0, 0x80 };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -128 ) );
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xD0, 0xDF } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -33 ) );
			}
		}
		
		[Test]
		public void TestUnpackNegativeFixNumMinValueMinusOne_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xD0, 0xDF };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -33 ) );
		}

		[Test]
		public void TestUnpackNegativeFixNumMinValue_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xE0 } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -32 ) );
			}
		}
		
		[Test]
		public void TestUnpackNegativeFixNumMinValue_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xE0 };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -32 ) );
		}

		[Test]
		public void TestUnpackMinusOne_UnpackInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xFF } ) )
			{
				var result = Unpacking.UnpackInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( -1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMinusOne_UnpackInt64_ByteArray()
		{
			var buffer = new byte[] { 0xFF };
			var result = Unpacking.UnpackInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( -1 ) );
		}

		[Test]
		public void TestUnpackZero_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x0 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackZero_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0x0 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackPlusOne_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackPlusOne_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0x1 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValue_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0x7F } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 127 ) );
			}
		}
		
		[Test]
		public void TestUnpackPositiveFixNumMaxValue_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0x7F };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 127 ) );
		}

		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0x80 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 128 ) );
			}
		}
		
		[Test]
		public void TestUnpackPositiveFixNumMaxValuePlusOne_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCC, 0x80 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 128 ) );
		}

		[Test]
		public void TestUnpackByteMaxValue_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCC, 0xFF } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackByteMaxValue_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCC, 0xFF };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackByteMaxValuePlusOne_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0x1, 0x00 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 256 ) );
			}
		}
		
		[Test]
		public void TestUnpackByteMaxValuePlusOne_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCD, 0x1, 0x00 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 256 ) );
		}

		[Test]
		public void TestUnpackUInt16MaxValue_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCD, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 65535 ) );
			}
		}
		
		[Test]
		public void TestUnpackUInt16MaxValue_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCD, 0xFF, 0xFF };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 65535 ) );
		}

		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 65536 ) );
			}
		}
		
		[Test]
		public void TestUnpackUInt16MaxValuePlusOne_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCE, 0x0, 0x1, 0x00, 0x00 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 65536 ) );
		}

		[Test]
		public void TestUnpackUInt32MaxValue_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 4294967295 ) );
			}
		}
		
		[Test]
		public void TestUnpackUInt32MaxValue_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCE, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 4294967295 ) );
		}

		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 4294967296 ) );
			}
		}
		
		[Test]
		public void TestUnpackUInt32MaxValuePlusOne_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCF, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x0 };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 4294967296 ) );
		}

		[Test]
		public void TestUnpackUInt64MaxValue_UnpackUInt64_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackUInt64( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( result, Is.EqualTo( 18446744073709551615 ) );
			}
		}
		
		[Test]
		public void TestUnpackUInt64MaxValue_UnpackUInt64_ByteArray()
		{
			var buffer = new byte[] { 0xCF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackUInt64( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( result.Value, Is.EqualTo( 18446744073709551615 ) );
		}

		[Test]
		public void TestUnpackSingleMinValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.MinValue.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleMinValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0xFF, 0x7F, 0xFF, 0xFF };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.MinValue.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleMaxValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.MaxValue.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleMaxValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x7F, 0x7F, 0xFF, 0xFF };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.MaxValue.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleEpsilon_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.Epsilon.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleEpsilon_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x01 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.Epsilon.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackSinglePositiveZero_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( ( 0.0f ).Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSinglePositiveZero_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( ( 0.0f ).Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNegativeZero_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( ( -0.0f ).Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNegativeZero_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x80, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( ( -0.0f ).Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNPositiveMinValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x01 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNPositiveMaxValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x7F, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNNegativeMinValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x01 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNaNNegativeMaxValue_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackSingleNegativeInfinity_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsNegativeInfinity( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSingleNegativeInfinity_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0xFF, 0x80, 0x00, 0x00 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsNegativeInfinity( result.Value ) );
		}

		[Test]
		public void TestUnpackSinglePositiveInfinity_UnpackSingle_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackSingle( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Single.IsPositiveInfinity( result ) );
			}
		}
		
		[Test]
		public void TestUnpackSinglePositiveInfinity_UnpackSingle_ByteArray()
		{
			var buffer = new byte[] { 0xCA, 0x7F, 0x80, 0x00, 0x00 };
			var result = Unpacking.UnpackSingle( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Single.IsPositiveInfinity( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleMinValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.MinValue.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleMinValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.MinValue.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleMaxValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.MaxValue.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleMaxValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.MaxValue.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleEpsilon_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.Epsilon.Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleEpsilon_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.Epsilon.Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackDoublePositiveZero_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( ( 0.0 ).Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoublePositiveZero_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( ( 0.0 ).Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNegativeZero_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( ( -0.0 ).Equals( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNegativeZero_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( ( -0.0 ).Equals( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNPositiveMinValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNPositiveMaxValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNNegativeMinValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsNaN( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNaNNegativeMaxValue_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsNaN( result.Value ) );
		}

		[Test]
		public void TestUnpackDoubleNegativeInfinity_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsNegativeInfinity( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoubleNegativeInfinity_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsNegativeInfinity( result.Value ) );
		}

		[Test]
		public void TestUnpackDoublePositiveInfinity_UnpackDouble_Stream()
		{
			using( var buffer = new MemoryStream( new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDouble( buffer );
				Assert.That( buffer.Position, Is.EqualTo( buffer.Length ) );
				Assert.That( Double.IsPositiveInfinity( result ) );
			}
		}
		
		[Test]
		public void TestUnpackDoublePositiveInfinity_UnpackDouble_ByteArray()
		{
			var buffer = new byte[] { 0xCB, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var result = Unpacking.UnpackDouble( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( buffer.Length ) );
			Assert.That( Double.IsPositiveInfinity( result.Value ) );
		}
	}
}