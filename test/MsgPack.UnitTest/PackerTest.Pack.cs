#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Threading.Tasks;
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
	// This file was generated from PackerTest.Pack.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit PackerTest.Pack.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class PackerTest
	{
		[Test]
		public void TestPack_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Int16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( SByte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( object ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPack_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPack_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		

		[Test]
		public void TestPackArrayHeaderInt32_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( 0 );
				Assert.AreEqual(
					new byte[] { 0x90 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_15()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( 15 );
				Assert.AreEqual(
					new byte[] { 0x9F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( 0x100 );
				Assert.AreEqual(
					new byte[] { 0xDC, 0x01, 0x00 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_0xFFFF()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( 0xFFFF );
				Assert.AreEqual(
					new byte[] { 0xDC, 0xFF, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_0x10000()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( 0x10000 );
				Assert.AreEqual(
					new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderIList_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( default( IList<MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderIList_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackArrayHeader( new MessagePackObject[] { 1, 2, 3 } );
				Assert.AreEqual(
					new byte[] { 0x93 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderInt32_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( () => packer.PackArrayHeader( -1 ) );
			}
		}

		[Test]
		public void TestPackMapHeaderInt32_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( 0 );
				Assert.AreEqual(
					new byte[] { 0x80 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderInt32_15()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( 15 );
				Assert.AreEqual(
					new byte[] { 0x8F },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderInt32_16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( 0x100 );
				Assert.AreEqual(
					new byte[] { 0xDE, 0x01, 0x00 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderInt32_0xFFFF()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( 0xFFFF );
				Assert.AreEqual(
					new byte[] { 0xDE, 0xFF, 0xFF },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderInt32_0x10000()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( 0x10000 );
				Assert.AreEqual(
					new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderIDictionary_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( default( IDictionary<MessagePackObject, MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderIDictionary_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackMapHeader( new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 }, { 3, 3 } } );
				Assert.AreEqual(
					new byte[] { 0x83 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderInt32_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( () => packer.PackMapHeader( -1 ) );
			}
		}

		[Test]
		public void TestPackCollectionArray_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackCollection( default( MessagePackObject[] ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackCollectionArray_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackCollection( new int[] { 1, 2, 3 } );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackCollectionEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackCollection( default( IEnumerable<MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackCollectionEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.PackCollection( Enumerable.Range( 1, 3 ) );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackSByte_0x7F()
		{
			SByte value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt16_0x7F()
		{
			Int16 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt32_0x7F()
		{
			Int32 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt64_0x7F()
		{
			Int64 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt32_Minus128()
		{
			Int32 value = -128;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackUInt32_0x80()
		{
			UInt32 value = 0x80;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackUInt32_0x100_AsUInt16()
		{
			UInt32 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xCD, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt64_0x1()
		{
			Int64 value = 0x1;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x1 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt64_Minus128()
		{
			Int64 value = -128;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt64_0x100_AsInt16()
		{
			Int64 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xD1, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackInt64_0x10000_AsInt32()
		{
			Int64 value = 0x10000;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xD2, 0x0, 0x1, 0x0, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackUInt64_0x80_AsInt8()
		{
			UInt64 value = 0x80;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackUInt64_0x100_AsInt16()
		{
			UInt64 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xCD, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackUInt64_0x10000_AsInt32()
		{
			UInt64 value = 0x10000;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xCE, 0x0, 0x1, 0x0, 0x0 },
					this.GetResult( packer )
				);
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestPackAsync_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Int16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( SByte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( object ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackAsync_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsync_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		

		[Test]
		public async Task TestPackArrayHeaderAsyncInt32_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( 0 );
				Assert.AreEqual(
					new byte[] { 0x90 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncInt32_15()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( 15 );
				Assert.AreEqual(
					new byte[] { 0x9F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncInt32_16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( 0x100 );
				Assert.AreEqual(
					new byte[] { 0xDC, 0x01, 0x00 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncInt32_0xFFFF()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( 0xFFFF );
				Assert.AreEqual(
					new byte[] { 0xDC, 0xFF, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncInt32_0x10000()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( 0x10000 );
				Assert.AreEqual(
					new byte[] { 0xDD, 0x00, 0x01, 0x00, 0x00 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncIList_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( default( IList<MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackArrayHeaderAsyncIList_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackArrayHeaderAsync( new MessagePackObject[] { 1, 2, 3 } );
				Assert.AreEqual(
					new byte[] { 0x93 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackArrayHeaderAsyncInt32_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				AssertEx.ThrowsAsync<ArgumentOutOfRangeException>( async () => await packer.PackArrayHeaderAsync( -1 ) );
			}
		}

		[Test]
		public async Task TestPackMapHeaderAsyncInt32_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( 0 );
				Assert.AreEqual(
					new byte[] { 0x80 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncInt32_15()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( 15 );
				Assert.AreEqual(
					new byte[] { 0x8F },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncInt32_16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( 0x100 );
				Assert.AreEqual(
					new byte[] { 0xDE, 0x01, 0x00 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncInt32_0xFFFF()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( 0xFFFF );
				Assert.AreEqual(
					new byte[] { 0xDE, 0xFF, 0xFF },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncInt32_0x10000()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( 0x10000 );
				Assert.AreEqual(
					new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncIDictionary_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( default( IDictionary<MessagePackObject, MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public async Task TestPackMapHeaderAsyncIDictionary_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackMapHeaderAsync( new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 }, { 3, 3 } } );
				Assert.AreEqual(
					new byte[] { 0x83 },
					this.GetResult( packer )
				);
			}
		}


		[Test]
		public void TestPackMapHeaderAsyncInt32_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				AssertEx.ThrowsAsync<ArgumentOutOfRangeException>( async () => await packer.PackMapHeaderAsync( -1 ) );
			}
		}

		[Test]
		public async Task TestPackCollectionAsyncArray_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackCollectionAsync( default( MessagePackObject[] ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackCollectionAsyncArray_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackCollectionAsync( new int[] { 1, 2, 3 } );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackCollectionAsyncEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackCollectionAsync( default( IEnumerable<MessagePackObject> ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackCollectionAsyncEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackCollectionAsync( Enumerable.Range( 1, 3 ) );
				Assert.AreEqual(
					new byte[] { 0x93, 1, 2, 3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncSByte_0x7F()
		{
			SByte value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt16_0x7F()
		{
			Int16 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt32_0x7F()
		{
			Int32 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt64_0x7F()
		{
			Int64 value = 0x7F;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt32_Minus128()
		{
			Int32 value = -128;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncUInt32_0x80()
		{
			UInt32 value = 0x80;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncUInt32_0x100_AsUInt16()
		{
			UInt32 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xCD, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt64_0x1()
		{
			Int64 value = 0x1;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x1 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt64_Minus128()
		{
			Int64 value = -128;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt64_0x100_AsInt16()
		{
			Int64 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xD1, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncInt64_0x10000_AsInt32()
		{
			Int64 value = 0x10000;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xD2, 0x0, 0x1, 0x0, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncUInt64_0x80_AsInt8()
		{
			UInt64 value = 0x80;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncUInt64_0x100_AsInt16()
		{
			UInt64 value = 0x100;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xCD, 0x1, 0x0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackAsyncUInt64_0x10000_AsInt32()
		{
			UInt64 value = 0x10000;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xCE, 0x0, 0x1, 0x0, 0x0 },
					this.GetResult( packer )
				);
			}
		}

#endif // FEATURE_TAP

	}
}
