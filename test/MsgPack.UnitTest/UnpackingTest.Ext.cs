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
	// This file was generated from UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude instead.

	[TestFixture]
	public partial class UnpackingTest_Ext
	{

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 3 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xD4, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 3 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xD4, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 3 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xD4, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 3 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xD5, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xD5, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xD5, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 6 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xD6, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 6 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 4 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xD6, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 6 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xD6, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 6 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 4 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 10 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xD7, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 10 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 8 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xD7, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 10 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xD7, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 10 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 8 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 18 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xD8, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 18 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 16 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xD8, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 18 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xD8, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 18 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 16 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs0JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 3 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs0JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 3 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs0HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 3 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs0HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 3 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs1HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 20 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 20 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 20 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs17HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 20 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 258 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 258 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 258 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext8_AndBinaryLengthIs255HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC7, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 258 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs0JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs0JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs0HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 4 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs0HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 4 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 5 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 5 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 5 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs1HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 5 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 21 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 21 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 21 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs17HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 21 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 259 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 259 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 259 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs255HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 259 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 260 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 260 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 256 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 260 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs256HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 260 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 256 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65539 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65539 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65535 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65539 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC8, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65539 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65535 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs0JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 6 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs0JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 6 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs0HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 6 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs0HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 6 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 7 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 7 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 7 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs1HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 1, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 7 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 23 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 23 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 23 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs17HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 17, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 23 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 17 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 261 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 261 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 261 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs255HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 261 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 255 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 262 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 262 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 256 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 262 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs256HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 1, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 262 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 256 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65541 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65541 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65535 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65541 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65541 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65535 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536JustLength_Success_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65542 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536JustLength_Success_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 1, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65542 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65536 ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536TooShort_Fail_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536TooShort_Fail_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 1, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackExtendedTypeObject( buffer ) );
		}

		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem_Stream()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackExtendedTypeObject( buffer );
				Assert.That( buffer.Position, Is.EqualTo( 65542 ) );
				Assert.That( result.TypeCode, Is.EqualTo( 1 ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}
		
		[Test]
		public void TestUnpackMessagePackExtendedTypeObject_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem_ByteArray()
		{
			var buffer = new byte[] { 0xC9, 0, 1, 0, 0, 1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray();
			var result = Unpacking.UnpackExtendedTypeObject( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( 65542 ) );
			Assert.That( result.Value.TypeCode, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Body, Is.Not.Null );
			Assert.That( result.Value.Body.Length, Is.EqualTo( 65536 ) );
		}
	}
}
