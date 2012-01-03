
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
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	public 	partial class UnpackingTest_Combinations_Raw
	{
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw0Value_AsFixRaw0_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xA0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw0Value_AsFixRaw0_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xA0 } ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( 0, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw0Value_AsRaw16_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDA, 0x00, 0x00 } );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw0Value_AsRaw16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDA, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw0Value_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x00 } );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw0Value_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw1Value_AsFixRaw1_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xA1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() );
			Assert.AreEqual( 2, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw1Value_AsFixRaw1_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xA1 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 2, buffer.Position );
				Assert.AreEqual( 0x1, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw1Value_AsRaw16_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDA, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() );
			Assert.AreEqual( 4, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw1Value_AsRaw16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDA, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 4, buffer.Position );
				Assert.AreEqual( 0x1, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRaw1Value_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() );
			Assert.AreEqual( 6, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRaw1Value_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x01 }.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 6, buffer.Position );
				Assert.AreEqual( 0x1, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRawMaxValue_AsFixRaw31_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xBF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() );
			Assert.AreEqual( 32, result.ReadCount );
			Assert.AreEqual( 0x1F, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRawMaxValue_AsFixRaw31_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xBF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 32, buffer.Position );
				Assert.AreEqual( 0x1F, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRawMaxValue_AsRaw16_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDA, 0x00, 0x1F }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() );
			Assert.AreEqual( 34, result.ReadCount );
			Assert.AreEqual( 0x1F, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRawMaxValue_AsRaw16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDA, 0x00, 0x1F }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 34, buffer.Position );
				Assert.AreEqual( 0x1F, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_FixRawMaxValue_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x1F }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() );
			Assert.AreEqual( 36, result.ReadCount );
			Assert.AreEqual( 0x1F, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_FixRawMaxValue_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x1F }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x1F ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 36, buffer.Position );
				Assert.AreEqual( 0x1F, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_Raw16MinValue_AsRaw16_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDA, 0x00, 0x20 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x20 ) ).ToArray() );
			Assert.AreEqual( 35, result.ReadCount );
			Assert.AreEqual( 0x20, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_Raw16MinValue_AsRaw16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDA, 0x00, 0x20 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x20 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 35, buffer.Position );
				Assert.AreEqual( 0x20, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_Raw16MinValue_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x20 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x20 ) ).ToArray() );
			Assert.AreEqual( 37, result.ReadCount );
			Assert.AreEqual( 0x20, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_Raw16MinValue_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x00, 0x00, 0x20 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x20 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 37, buffer.Position );
				Assert.AreEqual( 0x20, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_Raw16MaxValue_AsRaw16_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDA, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 65538, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_Raw16MaxValue_AsRaw16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDA, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 65538, buffer.Position );
				Assert.AreEqual( 0xFFFF, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_Raw16MaxValue_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 65540, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_Raw16MaxValue_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x00, 0xFF, 0xFF }.Concat( Enumerable.Repeat( ( byte )0xFF, 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 65540, buffer.Position );
				Assert.AreEqual( 0xFFFF, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		public void TestUnpackBinary_ByteArray_Raw32MinValue_AsRaw32_AsIs()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xDB, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x10000 ) ).ToArray() );
			Assert.AreEqual( 65541, result.ReadCount );
			Assert.AreEqual( 0x10000, result.Value.Length );
			Assert.That( result.Value, Is.All.EqualTo( 0xFF ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_Raw32MinValue_AsRaw32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDB, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x10000 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackBinary( buffer );
				Assert.AreEqual( 65541, buffer.Position );
				Assert.AreEqual( 0x10000, result.Length );
				Assert.That( result, Is.All.EqualTo( 0xFF ) );
			}
		}
		
		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBinary_ByteArray_Empty()
		{
			Unpacking.UnpackBinary( new byte[ 0 ] );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBinary_ByteArray_Null()
		{
			Unpacking.UnpackBinary( default( byte[] ) );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBinary_ByteArray_Offset_Null()
		{
			Unpacking.UnpackBinary( default( byte[] ), 0 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void TestUnpackBinary_ByteArray_Offset_OffsetIsNegative()
		{
			Unpacking.UnpackBinary( new byte[]{ 0x1 }, -1 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBinary_ByteArray_Offset_OffsetIsTooBig()
		{
			Unpacking.UnpackBinary( new byte[]{ 0x1 }, 1 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBinary_ByteArray_Offset_Empty()
		{
			Unpacking.UnpackBinary( new byte[ 0 ], 0 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBinary_Stream_Null()
		{
			Unpacking.UnpackBinary( default( Stream ) );
		}

		[Test]
		public void TestUnpackBinary_ByteArray_Offset_OffsetIsValid_OffsetIsRespected()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xFF, 0xA0, 0xFF }, 1 );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Length );
		}

		[Test]
		public void TestUnpackBinary_ByteArray_Null_Nil()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
	
		[Test]
		[ExpectedException( typeof( MessageTypeException ) )]
		public void TestUnpackBinary_ByteArray_NotBinary()
		{
			var result = Unpacking.UnpackBinary( new byte[] { 0x1 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
	}
}