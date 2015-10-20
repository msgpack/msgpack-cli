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
	[TestFixture]
	public partial class PackerTest_Pack
	{
		[Test]
		public void TestPack_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( int? ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPack_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPack_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
	}
}