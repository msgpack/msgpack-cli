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
	public partial class PackerTest_Raw
	{

		[Test]
		public void TestUnpackFixStr0_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr80_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr80_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr80_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr80_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr81_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr81_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr81_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr831_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr831_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr831_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr832_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr832_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr832_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr160_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr160_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr160_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr160_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr161_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr161_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr161_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr320_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr320_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr320_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr320_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr321_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr321_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr321_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin80_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin80_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin80_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin80_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin81_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin81_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin81_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin81_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin81_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin81_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin831_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin831_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin831_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin831_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin831_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin831_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin832_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin832_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin832_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin832_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin832_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin832_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin160_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin160_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin160_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin160_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin161_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin161_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin161_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin161_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin161_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin161_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1631_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1631_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin1631_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1631_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1631_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin1631_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1632_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1632_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin1632_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1632_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1632_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin1632_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16256_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16256_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16256_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16256_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16256_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16256_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1665535_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1665535_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin1665535_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1665535_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin1665535_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin1665535_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin320_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin320_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin320_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin320_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin321_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin321_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin321_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin321_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin321_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin321_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3231_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3231_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin3231_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3231_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3231_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin3231_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3232_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3232_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin3232_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3232_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3232_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin3232_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32255_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32255_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32255_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32255_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32255_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32255_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32256_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32256_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32256_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32256_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32256_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32256_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265535_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265535_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin3265535_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265535_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265535_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin3265535_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265536_AsString_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265536_AsString_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin3265536_AsString_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265536_ReadString_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin3265536_ReadString_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsFalse( unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin3265536_ReadString_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr0_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr1_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr1_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr31_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr31_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr80_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr80_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr80_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr80_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr81_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr81_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr81_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr81_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr831_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr831_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr831_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr831_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr832_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr832_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr832_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr832_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8255_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8255_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr160_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr160_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr160_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr160_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr161_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr161_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr161_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr161_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1631_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1631_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1632_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1632_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16255_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16255_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16256_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16256_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr1665535_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr1665535_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr320_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr320_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr320_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr320_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr321_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr321_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr321_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr321_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3231_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3231_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3232_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3232_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32255_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32255_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32256_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32256_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3265535_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3265535_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsBinary_Read_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsBinary_Read_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsFalse( unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr3265536_AsBinary_Read_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
				var result = unpacker.Data;
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadBinary_JustLength_Success()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadBinary_TooShort_Fail()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsFalse( unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr3265536_ReadBinary_HasExtra_NoProblem()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}
	}
}
