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
	[TestFixture]
	public partial class MessagePackObjectTest
	{
		private TextWriter Console
		{
			get
			{
#if !NETFX_CORE && !SILVERLIGHT
				return System.Console.Out;
#else
				return TextWriter.Null;
#endif
			}
		}

		[Test]
		public void TestAsByte()
		{
			TestAsByte( 0 );
			TestAsByte( 127 );
			TestAsByte( 128 );
			TestAsByte( ( Byte )1 );
			TestAsByte( Byte.MinValue );
			TestAsByte( Byte.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsByte( rand.NextByte() );
			}
			sw.Stop();
			Console.WriteLine( "Byte: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsByte( Byte value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Byte )target );
		}
		
		[Test]
		public void TestAsByteOverflow()
		{
			var target = new MessagePackObject( Byte.MaxValue + 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Byte )target;
					Console.WriteLine( "TestAsByteOverflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsByteUnderflow()
		{
			var target = new MessagePackObject( Byte.MinValue - 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Byte )target;
					Console.WriteLine( "TestAsByteOverflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsSByte()
		{
			TestAsSByte( 0 );
			TestAsSByte( 127 );
			TestAsSByte( -1 );
			TestAsSByte( -31 );
			TestAsSByte( -32 );
			TestAsSByte( ( SByte )1 );
			TestAsSByte( SByte.MinValue );
			TestAsSByte( SByte.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsSByte( rand.NextSByte() );
			}
			sw.Stop();
			Console.WriteLine( "SByte: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsSByte( SByte value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( SByte )target );
		}
		
		[Test]
		public void TestAsSByteOverflow()
		{
			var target = new MessagePackObject( SByte.MaxValue + 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( SByte )target;
					Console.WriteLine( "TestAsSByteOverflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsSByteUnderflow()
		{
			var target = new MessagePackObject( SByte.MinValue - 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( SByte )target;
					Console.WriteLine( "TestAsSByteOverflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsInt16()
		{
			TestAsInt16( 0 );
			TestAsInt16( 127 );
			TestAsInt16( 128 );
			TestAsInt16( -1 );
			TestAsInt16( -31 );
			TestAsInt16( -32 );
			TestAsInt16( ( Int16 )1 );
			TestAsInt16( Int16.MinValue );
			TestAsInt16( Int16.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsInt16( rand.NextInt16() );
			}
			sw.Stop();
			Console.WriteLine( "Int16: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsInt16( Int16 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Int16 )target );
		}
		
		[Test]
		public void TestAsInt16Overflow()
		{
			var target = new MessagePackObject( Int16.MaxValue + 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Int16 )target;
					Console.WriteLine( "TestAsInt16Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsInt16Underflow()
		{
			var target = new MessagePackObject( Int16.MinValue - 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Int16 )target;
					Console.WriteLine( "TestAsInt16Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsUInt16()
		{
			TestAsUInt16( 0 );
			TestAsUInt16( 127 );
			TestAsUInt16( 128 );
			TestAsUInt16( ( UInt16 )1 );
			TestAsUInt16( UInt16.MinValue );
			TestAsUInt16( UInt16.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsUInt16( rand.NextUInt16() );
			}
			sw.Stop();
			Console.WriteLine( "UInt16: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsUInt16( UInt16 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( UInt16 )target );
		}
		
		[Test]
		public void TestAsUInt16Overflow()
		{
			var target = new MessagePackObject( UInt16.MaxValue + 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( UInt16 )target;
					Console.WriteLine( "TestAsUInt16Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsUInt16Underflow()
		{
			var target = new MessagePackObject( UInt16.MinValue - 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( UInt16 )target;
					Console.WriteLine( "TestAsUInt16Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsInt32()
		{
			TestAsInt32( 0 );
			TestAsInt32( 127 );
			TestAsInt32( 128 );
			TestAsInt32( -1 );
			TestAsInt32( -31 );
			TestAsInt32( -32 );
			TestAsInt32( ( Int32 )1 );
			TestAsInt32( Int32.MinValue );
			TestAsInt32( Int32.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsInt32( rand.NextInt32() );
			}
			sw.Stop();
			Console.WriteLine( "Int32: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsInt32( Int32 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Int32 )target );
		}
		
		[Test]
		public void TestAsInt32Overflow()
		{
			var target = new MessagePackObject( Int32.MaxValue + 1L );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Int32 )target;
					Console.WriteLine( "TestAsInt32Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsInt32Underflow()
		{
			var target = new MessagePackObject( Int32.MinValue - 1L );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Int32 )target;
					Console.WriteLine( "TestAsInt32Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsUInt32()
		{
			TestAsUInt32( 0 );
			TestAsUInt32( 127 );
			TestAsUInt32( 128 );
			TestAsUInt32( ( UInt32 )1 );
			TestAsUInt32( UInt32.MinValue );
			TestAsUInt32( UInt32.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsUInt32( rand.NextUInt32() );
			}
			sw.Stop();
			Console.WriteLine( "UInt32: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsUInt32( UInt32 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( UInt32 )target );
		}
		
		[Test]
		public void TestAsUInt32Overflow()
		{
			var target = new MessagePackObject( UInt32.MaxValue + 1L );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( UInt32 )target;
					Console.WriteLine( "TestAsUInt32Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}
		
		[Test]
		public void TestAsUInt32Underflow()
		{
			var target = new MessagePackObject( UInt32.MinValue - 1L );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( UInt32 )target;
					Console.WriteLine( "TestAsUInt32Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsInt64()
		{
			TestAsInt64( 0 );
			TestAsInt64( 127 );
			TestAsInt64( 128 );
			TestAsInt64( -1 );
			TestAsInt64( -31 );
			TestAsInt64( -32 );
			TestAsInt64( ( Int64 )1 );
			TestAsInt64( Int64.MinValue );
			TestAsInt64( Int64.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsInt64( rand.NextInt64() );
			}
			sw.Stop();
			Console.WriteLine( "Int64: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsInt64( Int64 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Int64 )target );
		}
		
		[Test]
		public void TestAsInt64Overflow()
		{
			var target = new MessagePackObject( ( UInt64 )Int64.MaxValue + 1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( Int64 )target;
					Console.WriteLine( "TestAsInt64Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}


		[Test]
		public void TestAsUInt64()
		{
			TestAsUInt64( 0 );
			TestAsUInt64( 127 );
			TestAsUInt64( 128 );
			TestAsUInt64( ( UInt64 )1 );
			TestAsUInt64( UInt64.MinValue );
			TestAsUInt64( UInt64.MaxValue );
			var sw = Stopwatch.StartNew();
			var rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsUInt64( rand.NextUInt64() );
			}
			sw.Stop();
			Console.WriteLine( "UInt64: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsUInt64( UInt64 value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( UInt64 )target );
		}
		
		[Test]
		public void TestAsUInt64Underflow()
		{
			var target = new MessagePackObject( -1 );
			Assert.Throws<InvalidOperationException>(
				() =>
				{
					var result = ( UInt64 )target;
					Console.WriteLine( "TestAsUInt64Overflow:0x{0:x}({0:#,0})[{1}]", result, result.GetType() );
				}
			);
		}

		[Test]
		public void TestAsSingle()
		{
			TestAsSingle( 0.0f );
			TestAsSingle( -0.0f );
			TestAsSingle( 1.0f );
			TestAsSingle( -1.0f );
			TestAsSingle( Single.MaxValue );
			TestAsSingle( Single.MinValue );
			TestAsSingle( Single.NaN );
			TestAsSingle( Single.NegativeInfinity );
			TestAsSingle( Single.PositiveInfinity );
			var sw = Stopwatch.StartNew();
			TestRandom rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsSingle( rand.NextSingle() );
			}
			sw.Stop();
			Console.WriteLine( "Single: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsSingle( Single value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Single )target, 10e-10 );
		}

		[Test]
		public void TestAsDouble()
		{
			TestAsDouble( 0.0 );
			TestAsDouble( -0.0 );
			TestAsDouble( 1.0 );
			TestAsDouble( -1.0 );
			TestAsDouble( Double.MaxValue );
			TestAsDouble( Double.MinValue );
			TestAsDouble( Double.NaN );
			TestAsDouble( Double.NegativeInfinity );
			TestAsDouble( Double.PositiveInfinity );
			var sw = Stopwatch.StartNew();
			TestRandom rand = new TestRandom();
			for ( int i = 0; i < 100000; i++ )
			{
				TestAsDouble( rand.NextDouble() );
			}
			sw.Stop();
			Console.WriteLine( "Double: {0:#,0.###} usec/object", sw.Elapsed.Ticks / 1000000.0 );
		}

		private static void TestAsDouble( Double value )
		{
			var target = new MessagePackObject( value );
			Assert.AreEqual( value, ( Double )target, 10e-10 );
		}
	}
}