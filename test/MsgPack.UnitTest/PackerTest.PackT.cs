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
using System.Collections;
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
	// This file was generated from PackerTest.PackT.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit PackerTest.PackT.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class PackerTest
	{
		[Test]
		public void TestPackT_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int64>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int16>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.SByte>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double>( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single>( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt64>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt32>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt16>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Byte>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.SByte>( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32>( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Byte>( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Boolean>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Boolean>( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Object>( default( object ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single>( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double>( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single>( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double>( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single>( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double>( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double?>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Double?>( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single?>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Single?>( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int64?>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int64?>( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32?>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int32?>( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int16?>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Int16?>( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.SByte?>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.SByte?>( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt64?>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt64?>( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt32?>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt32?>( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt16?>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.UInt16?>( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Byte?>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Byte?>( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Boolean?>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				packer.Pack<System.Boolean?>( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public void TestPackT_Array_Null_Nil()
		{
			MessagePackObject[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_IListT_Null_Nil()
		{
			IList<MessagePackObject> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_ObjectArray_Null_Nil()
		{
			object[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_IList_Null_Nil()
		{
			IList value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_IEnumerable_Null_Nil()
		{
			IEnumerable value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_Dictionary_Null_Nil()
		{
			MessagePackObjectDictionary value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_IDictionary_Null_Nil()
		{
			IDictionary value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_ByteArray_Null_Nil()
		{
			byte[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_String_Null_Nil()
		{
			string value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public void TestPackT_IPackable_Null_Nil()
		{
			Packable value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_IList_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObject[] { 1, 2, 3 };
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x93, 0x1, 0x2, 0x3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_IDictionary_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 } };
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					new byte[] { 0x82, 0x1, 0x1, 0x2, 0x2 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public void TestPackT_IPackable_NotNull_Success()
		{
			var value = new Packable();
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				packer.Pack( value );
				Assert.AreEqual(
					 new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestPackTAsync_DoubleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SingleMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Int64MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int64>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Int32MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Int16MinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int16>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SByteMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.SByte>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_DoubleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double>( Double.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SingleMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single>( Single.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_UInt64MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt64>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_UInt32MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt32>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_UInt16MaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt16>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_ByteMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Byte>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NegativeFixNumMinValueMinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.SByte>( ( ( sbyte )-33 ) );
				Assert.AreEqual(
					new byte[] { 0xD0, unchecked( ( byte )( sbyte )-33 ) },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NegativeFixNumMinValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( ( -32 ) );
				Assert.AreEqual(
					new byte[] { 0xE0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_MinusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( ( -1 ) );
				Assert.AreEqual(
					new byte[] { 0xFF },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Zero_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( 0 );
				Assert.AreEqual(
					new byte[] { 0x00 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_PlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( ( 1 ) );
				Assert.AreEqual(
					new byte[] { 0x01 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_PositiveFixNumMaxValue_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32>( ( 127 ) );
				Assert.AreEqual(
					new byte[] { 0x7F },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_PositiveFixNumMaxValuePlusOne_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Byte>( ( ( byte )128 ) );
				Assert.AreEqual(
					new byte[] { 0xCC, 0x80 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_True_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Boolean>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_False_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Boolean>( false );
				Assert.AreEqual(
					new byte[] { 0xC2 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Nil_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Object>( default( object ) );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SingleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single>( Single.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_DoubleEpsilon_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double>( Double.Epsilon );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.Epsilon ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SinglePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single>( Single.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_DoublePositiveInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double>( Double.PositiveInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.PositiveInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_SingleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single>( Single.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_DoubleNegativeInfinity_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double>( Double.NegativeInfinity );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.NegativeInfinity ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableDouble_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double?>( Double.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCB }.Concat( BitConverter.GetBytes( Double.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableDouble_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Double?>( default( System.Double?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableSingle_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single?>( Single.MinValue );
				Assert.AreEqual(
					new byte[] { 0xCA }.Concat( BitConverter.GetBytes( Single.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableSingle_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Single?>( default( System.Single?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int64?>( Int64.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD3 }.Concat( BitConverter.GetBytes( Int64.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int64?>( default( System.Int64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32?>( Int32.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD2 }.Concat( BitConverter.GetBytes( Int32.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int32?>( default( System.Int32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int16?>( Int16.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD1 }.Concat( BitConverter.GetBytes( Int16.MinValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Int16?>( default( System.Int16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableSByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.SByte?>( SByte.MinValue );
				Assert.AreEqual(
					new byte[] { 0xD0, 0x80 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableSByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.SByte?>( default( System.SByte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableUInt64_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt64?>( UInt64.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCF }.Concat( BitConverter.GetBytes( UInt64.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableUInt64_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt64?>( default( System.UInt64?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableUInt32_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt32?>( UInt32.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCE }.Concat( BitConverter.GetBytes( UInt32.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableUInt32_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt32?>( default( System.UInt32?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableUInt16_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt16?>( UInt16.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCD }.Concat( BitConverter.GetBytes( UInt16.MaxValue ).Reverse() ).ToArray(),
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableUInt16_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.UInt16?>( default( System.UInt16?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableByte_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Byte?>( Byte.MaxValue );
				Assert.AreEqual(
					new byte[] { 0xCC, 0xFF },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableByte_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Byte?>( default( System.Byte?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_NullableBoolean_NotNull_AsValue()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Boolean?>( true );
				Assert.AreEqual(
					new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_NullableBoolean_Null_AsNil()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync<System.Boolean?>( default( System.Boolean?  ) );
				Assert.AreEqual(
					new byte[]{ 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		
		[Test]
		public async Task TestPackTAsync_Array_Null_Nil()
		{
			MessagePackObject[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_IListT_Null_Nil()
		{
			IList<MessagePackObject> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_ObjectArray_Null_Nil()
		{
			object[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_IList_Null_Nil()
		{
			IList value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_IEnumerable_Null_Nil()
		{
			IEnumerable value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_Dictionary_Null_Nil()
		{
			MessagePackObjectDictionary value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_IDictionary_Null_Nil()
		{
			IDictionary value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_ByteArray_Null_Nil()
		{
			byte[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_String_Null_Nil()
		{
			string value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}
		[Test]
		public async Task TestPackTAsync_IPackable_Null_Nil()
		{
			Packable value = null;
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0xC0 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_IList_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObject[] { 1, 2, 3 };
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x93, 0x1, 0x2, 0x3 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_IDictionary_ItemIsMessagePackObject_Success()
		{
			var value = new MessagePackObjectDictionary() { { 1, 1 }, { 2, 2 } };
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					new byte[] { 0x82, 0x1, 0x1, 0x2, 0x2 },
					this.GetResult( packer )
				);
			}
		}

		[Test]
		public async Task TestPackTAsync_IPackable_NotNull_Success()
		{
			var value = new Packable();
			using ( var buffer = new MemoryStream() )
			using ( var packer = CreatePacker( buffer ) )
			{
				await packer.PackAsync( value );
				Assert.AreEqual(
					 new byte[] { 0xC3 },
					this.GetResult( packer )
				);
			}
		}

#endif // FEATURE_TAP

	}
}
