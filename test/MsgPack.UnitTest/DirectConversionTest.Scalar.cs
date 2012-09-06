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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
	partial class DirectConversionTest
	{

		[Test]
		public void TestByte()
		{
			TestByte( 0 );
			TestByte( ( Byte )1 );
			TestByte( Byte.MinValue );
			TestByte( Byte.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestByte( rand.NextByte() );
			}
			sw.Stop();
			Console.WriteLine( "Byte: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestByte( Byte value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackByte( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackByte( output.ToArray() ).Value );
		}

		[Test]
		public void TestSByte()
		{
			TestSByte( 0 );
			TestSByte( -1 );
			TestSByte( ( SByte )1 );
			TestSByte( SByte.MinValue );
			TestSByte( SByte.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestSByte( rand.NextSByte() );
			}
			sw.Stop();
			Console.WriteLine( "SByte: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestSByte( SByte value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackSByte( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackSByte( output.ToArray() ).Value );
		}

		[Test]
		public void TestInt16()
		{
			TestInt16( 0 );
			TestInt16( -1 );
			TestInt16( ( Int16 )1 );
			TestInt16( Int16.MinValue );
			TestInt16( Int16.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestInt16( rand.NextInt16() );
			}
			sw.Stop();
			Console.WriteLine( "Int16: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestInt16( Int16 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackInt16( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackInt16( output.ToArray() ).Value );
		}

		[Test]
		public void TestUInt16()
		{
			TestUInt16( 0 );
			TestUInt16( ( UInt16 )1 );
			TestUInt16( UInt16.MinValue );
			TestUInt16( UInt16.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestUInt16( rand.NextUInt16() );
			}
			sw.Stop();
			Console.WriteLine( "UInt16: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestUInt16( UInt16 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackUInt16( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackUInt16( output.ToArray() ).Value );
		}

		[Test]
		public void TestInt32()
		{
			TestInt32( 0 );
			TestInt32( -1 );
			TestInt32( ( Int32 )1 );
			TestInt32( Int32.MinValue );
			TestInt32( Int32.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestInt32( rand.NextInt32() );
			}
			sw.Stop();
			Console.WriteLine( "Int32: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestInt32( Int32 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackInt32( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackInt32( output.ToArray() ).Value );
		}

		[Test]
		public void TestUInt32()
		{
			TestUInt32( 0 );
			TestUInt32( ( UInt32 )1 );
			TestUInt32( UInt32.MinValue );
			TestUInt32( UInt32.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestUInt32( rand.NextUInt32() );
			}
			sw.Stop();
			Console.WriteLine( "UInt32: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestUInt32( UInt32 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackUInt32( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackUInt32( output.ToArray() ).Value );
		}

		[Test]
		public void TestInt64()
		{
			TestInt64( 0 );
			TestInt64( -1 );
			TestInt64( ( Int64 )1 );
			TestInt64( Int64.MinValue );
			TestInt64( Int64.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestInt64( rand.NextInt64() );
			}
			sw.Stop();
			Console.WriteLine( "Int64: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestInt64( Int64 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackInt64( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackInt64( output.ToArray() ).Value );
		}

		[Test]
		public void TestUInt64()
		{
			TestUInt64( 0 );
			TestUInt64( ( UInt64 )1 );
			TestUInt64( UInt64.MinValue );
			TestUInt64( UInt64.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestUInt64( rand.NextUInt64() );
			}
			sw.Stop();
			Console.WriteLine( "UInt64: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestUInt64( UInt64 value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackUInt64( new MemoryStream( output.ToArray() ) ) );
			Assert.AreEqual( value, Unpacking.UnpackUInt64( output.ToArray() ).Value );
		}

		[Test]
		public void TestSingle()
		{
			TestSingle( 0.0f );
			TestSingle( -0.0f );
			TestSingle( 1.0f );
			TestSingle( -1.0f );
			TestSingle( Single.MaxValue );
			TestSingle( Single.MinValue );
			TestSingle( Single.NaN );
			TestSingle( Single.NegativeInfinity );
			TestSingle( Single.PositiveInfinity );
			var sw = Stopwatch.StartNew();
			TestRandom rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestSingle( rand.NextSingle() );
			}
			sw.Stop();
			Console.WriteLine( "Single: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestSingle( Single value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackSingle( new MemoryStream( output.ToArray() ) ), 10e-10 );
			Assert.AreEqual( value, Unpacking.UnpackSingle( output.ToArray() ).Value, 10e-10 );
		}

		[Test]
		public void TestDouble()
		{
			TestDouble( 0.0 );
			TestDouble( -0.0 );
			TestDouble( 1.0 );
			TestDouble( -1.0 );
			TestDouble( Double.MaxValue );
			TestDouble( Double.MinValue );
			TestDouble( Double.NaN );
			TestDouble( Double.NegativeInfinity );
			TestDouble( Double.PositiveInfinity );
			var sw = Stopwatch.StartNew();
			TestRandom rand = new TestRandom();
			for ( int i = 0; i < 1000; i++ )
			{
				TestDouble( rand.NextDouble() );
			}
			sw.Stop();
			Console.WriteLine( "Double: {0:0.###} msec/object", sw.ElapsedMilliseconds / 1000.0 );
		}

		private static void TestDouble( Double value )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( value );
			Assert.AreEqual( value, Unpacking.UnpackDouble( new MemoryStream( output.ToArray() ) ), 10e-10 );
			Assert.AreEqual( value, Unpacking.UnpackDouble( output.ToArray() ).Value, 10e-10 );
		}
	}
}