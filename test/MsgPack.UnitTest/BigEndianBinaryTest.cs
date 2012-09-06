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
using System.Diagnostics;
using System.Globalization;
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
	[TestFixture]
	[Timeout( 1000 )]
	public sealed class BigEndianBinaryTest
	{
		[Test]
		public void TestToSByte()
		{
			AssertPrimitive( "0", BigEndianBinary.ToSByte( new byte[] { 0x0 }, 0 ) );
			AssertPrimitive( "7f", BigEndianBinary.ToSByte( new byte[] { 0x7f }, 0 ) );
			AssertPrimitive( "80", BigEndianBinary.ToSByte( new byte[] { 0x80 }, 0 ) );
			AssertPrimitive( "ff", BigEndianBinary.ToSByte( new byte[] { 0xff }, 0 ) );
			AssertPrimitive( "80", BigEndianBinary.ToSByte( new byte[] { 0x1, 0x80, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToInt16()
		{
			AssertPrimitive( "0", BigEndianBinary.ToInt16( new byte[] { 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fff", BigEndianBinary.ToInt16( new byte[] { 0x7f, 0xff }, 0 ) );
			AssertPrimitive( "8000", BigEndianBinary.ToInt16( new byte[] { 0x80, 0x00 }, 0 ) );
			AssertPrimitive( "ffff", BigEndianBinary.ToInt16( new byte[] { 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000", BigEndianBinary.ToInt16( new byte[] { 0x1, 0x80, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToInt32()
		{
			AssertPrimitive( "0", BigEndianBinary.ToInt32( new byte[] { 0x0, 0x0, 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fffffff", BigEndianBinary.ToInt32( new byte[] { 0x7f, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "80000000", BigEndianBinary.ToInt32( new byte[] { 0x80, 0x00, 0x00, 0x00 }, 0 ) );
			AssertPrimitive( "ffffffff", BigEndianBinary.ToInt32( new byte[] { 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "80000000", BigEndianBinary.ToInt32( new byte[] { 0x1, 0x80, 0x00, 0x00, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToInt64()
		{
			AssertPrimitive( "0", BigEndianBinary.ToInt64( new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fffffffffffffff", BigEndianBinary.ToInt64( new byte[] { 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000000000000000", BigEndianBinary.ToInt64( new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			AssertPrimitive( "ffffffffffffffff", BigEndianBinary.ToInt64( new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000000000000000", BigEndianBinary.ToInt64( new byte[] { 0x1, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToByte()
		{
			AssertPrimitive( "0", BigEndianBinary.ToByte( new byte[] { 0x0 }, 0 ) );
			AssertPrimitive( "7f", BigEndianBinary.ToByte( new byte[] { 0x7f }, 0 ) );
			AssertPrimitive( "80", BigEndianBinary.ToByte( new byte[] { 0x80 }, 0 ) );
			AssertPrimitive( "ff", BigEndianBinary.ToByte( new byte[] { 0xff }, 0 ) );
			AssertPrimitive( "80", BigEndianBinary.ToByte( new byte[] { 0x1, 0x80, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToUInt16()
		{
			AssertPrimitive( "0", BigEndianBinary.ToUInt16( new byte[] { 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fff", BigEndianBinary.ToUInt16( new byte[] { 0x7f, 0xff }, 0 ) );
			AssertPrimitive( "8000", BigEndianBinary.ToUInt16( new byte[] { 0x80, 0x00 }, 0 ) );
			AssertPrimitive( "ffff", BigEndianBinary.ToUInt16( new byte[] { 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000", BigEndianBinary.ToUInt16( new byte[] { 0x1, 0x80, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToUInt32()
		{
			AssertPrimitive( "0", BigEndianBinary.ToUInt32( new byte[] { 0x0, 0x0, 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fffffff", BigEndianBinary.ToUInt32( new byte[] { 0x7f, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "80000000", BigEndianBinary.ToUInt32( new byte[] { 0x80, 0x00, 0x00, 0x00 }, 0 ) );
			AssertPrimitive( "ffffffff", BigEndianBinary.ToUInt32( new byte[] { 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "80000000", BigEndianBinary.ToUInt32( new byte[] { 0x1, 0x80, 0x00, 0x00, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToUInt64()
		{
			AssertPrimitive( "0", BigEndianBinary.ToUInt64( new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }, 0 ) );
			AssertPrimitive( "7fffffffffffffff", BigEndianBinary.ToUInt64( new byte[] { 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000000000000000", BigEndianBinary.ToUInt64( new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			AssertPrimitive( "ffffffffffffffff", BigEndianBinary.ToUInt64( new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			AssertPrimitive( "8000000000000000", BigEndianBinary.ToUInt64( new byte[] { 0x1, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2 }, 1 ) );
		}

		[Test]
		public void TestToSingle()
		{
			Assert.AreEqual( 0.0f, BigEndianBinary.ToSingle( new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -0.0f, BigEndianBinary.ToSingle( new byte[] { 0x80, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Single.Epsilon, BigEndianBinary.ToSingle( new byte[] { 0x00, 0x00, 0x00, 0x01 }, 0 ) );
			Assert.AreEqual( -1.0f * Single.Epsilon, BigEndianBinary.ToSingle( new byte[] { 0x80, 0x00, 0x00, 0x01 }, 0 ) );
			Assert.AreEqual( Single.MinValue, BigEndianBinary.ToSingle( new byte[] { 0xff, 0x7f, 0xff, 0xff }, 0 ) );
			Assert.AreEqual( Single.MaxValue, BigEndianBinary.ToSingle( new byte[] { 0x7f, 0x7f, 0xff, 0xff }, 0 ) );
			Assert.AreEqual( Single.NaN, BigEndianBinary.ToSingle( new byte[] { 0x7f, 0xc0, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Single.NaN, BigEndianBinary.ToSingle( new byte[] { 0xff, 0xc0, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Single.PositiveInfinity, BigEndianBinary.ToSingle( new byte[] { 0x7f, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Single.NegativeInfinity, BigEndianBinary.ToSingle( new byte[] { 0xff, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 1.0f, BigEndianBinary.ToSingle( new byte[] { 0x3f, 0x80, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -1.0f, BigEndianBinary.ToSingle( new byte[] { 0xbf, 0x80, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 2.0f, BigEndianBinary.ToSingle( new byte[] { 0x40, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -2.0f, BigEndianBinary.ToSingle( new byte[] { 0xc0, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 1.0f, BigEndianBinary.ToSingle( new byte[] { 0xff, 0x3f, 0x80, 0x00, 0x00, 0xff }, 1 ) );
		}

		[Test]
		public void TestToDouble()
		{
			Assert.AreEqual( 0.0, BigEndianBinary.ToDouble( new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -0.0, BigEndianBinary.ToDouble( new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Double.Epsilon, BigEndianBinary.ToDouble( new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, 0 ) );
			Assert.AreEqual( -1.0 * Double.Epsilon, BigEndianBinary.ToDouble( new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, 0 ) );
			Assert.AreEqual( Double.MinValue, BigEndianBinary.ToDouble( new byte[] { 0xff, 0xef, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			Assert.AreEqual( Double.MaxValue, BigEndianBinary.ToDouble( new byte[] { 0x7f, 0xef, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, 0 ) );
			Assert.AreEqual( Double.NaN, BigEndianBinary.ToDouble( new byte[] { 0x7f, 0xf8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Double.NaN, BigEndianBinary.ToDouble( new byte[] { 0xff, 0xf8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Double.PositiveInfinity, BigEndianBinary.ToDouble( new byte[] { 0x7f, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( Double.NegativeInfinity, BigEndianBinary.ToDouble( new byte[] { 0xff, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 1.0, BigEndianBinary.ToDouble( new byte[] { 0x3f, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -1.0, BigEndianBinary.ToDouble( new byte[] { 0xbf, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 2.0, BigEndianBinary.ToDouble( new byte[] { 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( -2.0, BigEndianBinary.ToDouble( new byte[] { 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0 ) );
			Assert.AreEqual( 1.0, BigEndianBinary.ToDouble( new byte[] { 0xff, 0x3f, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff }, 1 ) );
		}

#if PERFORMANCE_TEST
		[Test]
		public void PerformanceTest()
		{
			var dummy = new byte[ 8 ];
			// Let's load and JIT
			BigEndianBinary.ToInt16( dummy, 0 );
			BigEndianBinary.ToInt32( dummy, 0 );
			BigEndianBinary.ToInt64( dummy, 0 );
			BigEndianBinary.ToUInt16( dummy, 0 );
			BigEndianBinary.ToUInt32( dummy, 0 );
			BigEndianBinary.ToUInt64( dummy, 0 );
			BigEndianBinary.ToSingle( dummy, 0 );
			BigEndianBinary.ToDouble( dummy, 0 );
			BitConverter.ToInt16( dummy, 0 );
			BitConverter.ToInt32( dummy, 0 );
			BitConverter.ToInt64( dummy, 0 );
			BitConverter.ToUInt16( dummy, 0 );
			BitConverter.ToUInt32( dummy, 0 );
			BitConverter.ToUInt64( dummy, 0 );
			BitConverter.ToSingle( dummy, 0 );
			BitConverter.ToDouble( dummy, 0 );

			// Go
			const int iteration = 1000000;
			PerformanceTestCore( new byte[] { 0, 0x80, 0xff, 0 }, BigEndianBinary.ToInt16, BitConverter.ToInt16, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0xff, 0 }, BigEndianBinary.ToUInt16, BitConverter.ToUInt16, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0x00, 0xff, 0xff, 0 }, BigEndianBinary.ToInt32, BitConverter.ToInt32, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0x00, 0xff, 0xff, 0 }, BigEndianBinary.ToUInt32, BitConverter.ToUInt32, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0 }, BigEndianBinary.ToInt64, BitConverter.ToInt64, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0 }, BigEndianBinary.ToUInt64, BitConverter.ToUInt64, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0xff, 0x00, 0xff, 0 }, BigEndianBinary.ToSingle, BitConverter.ToSingle, iteration );
			PerformanceTestCore( new byte[] { 0, 0x80, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0 }, BigEndianBinary.ToDouble, BitConverter.ToDouble, iteration );
		}

		private static void PerformanceTestCore<T>( byte[] dataWith1ByteDualEdgePadding, Func<byte[], int, T> bigEndianBinaryMethod, Func<byte[], int, T> bitConverterMethod, int iteration )
		{
			GC.Collect();
			var sw = Stopwatch.StartNew();
			for ( int i = 0; i < iteration; i++ )
			{
				bigEndianBinaryMethod( dataWith1ByteDualEdgePadding, 1 );
			}
			sw.Stop();
			long bigEndianBinaryElapsed = sw.ElapsedTicks;
			sw.Reset();
			GC.Collect();
			sw.Start();
			for ( int i = 0; i < iteration; i++ )
			{
				bitConverterMethod( dataWith1ByteDualEdgePadding, 1 );
			}
			sw.Stop();
			long bitConverterElapsed = sw.ElapsedTicks;

			Console.WriteLine(
				"{0} :x{1:0.0}times BigEndianBinary : {2:#,##0.000} msec BitConverter : {3:#,##0.000} msec ({4:#,##0} iteration)",
				typeof( T ),
				( double )bigEndianBinaryElapsed / ( double )bitConverterElapsed,
				bigEndianBinaryElapsed * 1000.0 / Stopwatch.Frequency,
				bitConverterElapsed * 1000.0 / Stopwatch.Frequency,
				iteration
			);
		}
#endif

		private static void AssertPrimitive<T>( string expectedHexString, T actualPrimitive )
			where T : IFormattable
		{
			Assert.AreEqual( expectedHexString, actualPrimitive.ToString( "x", CultureInfo.InvariantCulture ) );
		}
	}
}
