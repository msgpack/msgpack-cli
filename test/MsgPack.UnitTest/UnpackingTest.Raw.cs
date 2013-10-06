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
	// This file was generated from UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude instead.

	[TestFixture]
	public partial class UnpackingTest_Raw
	{

		[Test]
		public void TestUnpackFixStr_0_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xA0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_0_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xA0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_0_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xA0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_0_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xA0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr8_0_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_0_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr8_0_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_0_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr16_0_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_0_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr16_0_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_0_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackStr32_0_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_0_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr32_0_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_0_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65536 ) ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( Encoding.UTF8.GetString( result, 0, result.Length ), Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsString_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( Encoding.UTF8.GetString( result.Value, 0, result.Value.Length ), Is.EqualTo( new String( 'A', 65536 ) ) );
		}

		[Test]
		public void TestUnpackBin8_0_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC4, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_0_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC4, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin8_0_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC4, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_0_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC4, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC4, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC4, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC4, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC4, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC4, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_1_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC4, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC4, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC4, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC4, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_31_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC4, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC4, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC4, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_32_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC4, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC4, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC4, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC4, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC4, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC4, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin8_255_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC4, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin16_0_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_0_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin16_0_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_0_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_1_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_31_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_32_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_255_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_256_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin16_65535_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC5, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackBin32_0_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_0_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin32_0_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_0_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
		}

		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_1_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
		}

		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_31_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
		}

		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_32_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
		}

		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_255_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
		}

		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_256_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65535_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackString( buffer ) );
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackString( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}
		
		[Test]
		public void TestUnpackBin32_65536_AsString_UnpackString_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xC6, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray();
			var result = Unpacking.UnpackString( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xA0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_0_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xA0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xA0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_0_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xA0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xA1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_1_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xA1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xBF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackFixStr_31_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xBF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_0_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_0_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_1_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_31_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_32_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xD9, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr8_255_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xD9, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_0_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_0_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_1_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_31_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_32_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_255_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_256_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr16_65535_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDA, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_0_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 0 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_0_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 0 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 1 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_1_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 1 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 1 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 31 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_31_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x1F };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 31 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 32 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_32_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0x20 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 32 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 255 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_255_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 255 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 256 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_256_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 1, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 256 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65535 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65535_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 0, 0xFF, 0xFF };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65535 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_JustLength_Success_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_JustLength_Success_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_TooShort_Fail_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_TooShort_Fail_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
				Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( buffer ) );
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_HasExtra_NoProblem_Stream()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			using( var buffer =
				new MemoryStream( 
					header.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.That( buffer.Position, Is.EqualTo( header.Length + 65536 ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}
		
		[Test]
		public void TestUnpackStr32_65536_AsBinary_UnpackBinary_HasExtra_NoProblem_ByteArray()
		{
			var header = new byte[] { 0xDB, 0, 1, 0, 0 };
			var buffer = header.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray();
			var result = Unpacking.UnpackBinary( buffer );
			Assert.That( result.ReadCount, Is.EqualTo( header.Length + 65536 ) );
			Assert.That( result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
		}
	}
}
