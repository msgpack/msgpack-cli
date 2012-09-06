
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
using System.Linq;
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
	public 	partial class UnpackingTest_Combinations_Array
	{
		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray0Value_AsFixArray0_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0x90 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray0Value_AsFixArray0_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90 } ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray0Value_AsFixArray0_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0x90 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray0Value_AsFixArray0_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90 } ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray0Value_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDC, 0x00, 0x00 } );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray0Value_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray0Value_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDC, 0x00, 0x00 } );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray0Value_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray0Value_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x00 } );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray0Value_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 0 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray0Value_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x00 } );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray0Value_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray1Value_AsFixArray1_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0x91 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray1Value_AsFixArray1_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray1Value_AsFixArray1_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0x91 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 2, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray1Value_AsFixArray1_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 2, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray1Value_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDC, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray1Value_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray1Value_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDC, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 4, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray1Value_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 4, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArray1Value_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArray1Value_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArray1Value_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() );
			Assert.AreEqual( 6, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArray1Value_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 6, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArrayMaxValue_AsFixArray15_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0x9F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0xF ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArrayMaxValue_AsFixArray15_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x9F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.That( result, Is.EqualTo( 0xF ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArrayMaxValue_AsFixArray15_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0x9F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 16, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArrayMaxValue_AsFixArray15_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x9F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 16, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArrayMaxValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDC, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0xF ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArrayMaxValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( 0xF ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArrayMaxValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDC, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 18, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArrayMaxValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 18, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_FixArrayMaxValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0xF ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_FixArrayMaxValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 0xF ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_FixArrayMaxValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() );
			Assert.AreEqual( 20, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_FixArrayMaxValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x0F }.Concat( Enumerable.Repeat( ( byte )0x57, 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 20, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xF ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Array16MinValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDC, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0x10 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Array16MinValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( 0x10 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_Array16MinValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDC, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() );
			Assert.AreEqual( 19, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_Array16MinValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 19, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Array16MinValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0x10 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Array16MinValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 0x10 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_Array16MinValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() );
			Assert.AreEqual( 21, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_Array16MinValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0x00, 0x10 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 21, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Array16MaxValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDC, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0xFFFF ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Array16MaxValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.That( result, Is.EqualTo( 0xFFFF ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_Array16MaxValue_AsArray16_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDC, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 65538, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xFFFF ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_Array16MaxValue_AsArray16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDC, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 65538, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xFFFF ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Array16MaxValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0xFFFF ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Array16MaxValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 0xFFFF ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_Array16MaxValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 65540, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xFFFF ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_Array16MaxValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0x57, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 65540, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0xFFFF ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Array32MinValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10000 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( 0x10000 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Array32MinValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10000 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArrayLength( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.That( result, Is.EqualTo( 0x10000 ) );
			}
		}
		

		[Test]
		public void TestUnpackArray_ByteArray_Array32MinValue_AsArray32_AsIs()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10000 ) ).ToArray() );
			Assert.AreEqual( 65541, result.ReadCount );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10000 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackArray_Stream_Array32MinValue_AsArray32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0x57, 0x10000 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackArray( buffer );
				Assert.AreEqual( 65541, buffer.Position );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( new MessagePackObject( 0x57 ), 0x10000 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackArrayLength( new byte[ 0 ] ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackArrayLength( default( byte[] ) ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Offset_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackArrayLength( default( byte[] ), 0 ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Offset_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackArrayLength( new byte[]{ 0x1 }, -1 ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Offset_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackArrayLength( new byte[]{ 0x1 }, 1 ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Offset_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackArrayLength( new byte[ 0 ], 0 ) );
		}

		[Test]
		public void TestUnpackArrayLength_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackArrayLength( default( Stream ) ) );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Offset_OffsetIsValid_OffsetIsRespected()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xFF, 0x90, 0xFF }, 1 );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_Null_Nil()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}

		[Test]
		public void TestUnpackArrayLength_ByteArray_NotArray()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackArrayLength( new byte[] { 0x1 } ) );
		}

		[Test]
		public void TestUnpackArray_ByteArray_Null_Nil()
		{
			var result = Unpacking.UnpackArray( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}

		[Test]
		public void TestUnpackArray_ByteArray_NotArray()
		{
			Assert.Throws<MessageTypeException>( () =>  Unpacking.UnpackArray( new byte[] { 0x1 } ) );
		}
	}
}