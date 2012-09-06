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
	public partial class PackerTest_PackT
	{
		[Test]
		public void TestPackT_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int64>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int16>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.SByte>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double>( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single>( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt64>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt32>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt16>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Byte>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.SByte>( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32>( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Byte>( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Boolean>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Boolean>( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Object>( default( object ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single>( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double>( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single>( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double>( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single>( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double>( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double?>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Double?>( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single?>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Single?>( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int64?>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int64?>( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32?>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int32?>( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int16?>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Int16?>( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.SByte?>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.SByte?>( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt64?>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt64?>( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt32?>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt32?>( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt16?>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.UInt16?>( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Byte?>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Byte?>( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Boolean?>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					buffer.ToArray()
				);
			}
		}

		[Test]
		public void TestPackT_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				packer.Pack<System.Boolean?>( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					buffer.ToArray()
				);
			}
		}
		
	}
}