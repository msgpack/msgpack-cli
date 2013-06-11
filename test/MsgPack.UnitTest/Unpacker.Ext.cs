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
using TimeoutAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	// This file was generated from UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude instead.

	[TestFixture]
	public partial class PackerTest_Ext
	{

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_FixExt_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_FixExt_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext8LengthIs1_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext8LengthIs1_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext16LengthIs2_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext16LengthIs2_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 17 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_Read_Ext32LengthIs4_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				// ver 0.3.2 should always fail with NIE.
				Assert.Throws<NotImplementedException>( ()=> unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}

		[Test]
		public void TestUnpack_ReadExtension_Ext32LengthIs4_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Environment.TickCount % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, typeCode, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
			}
		}
	}
}
