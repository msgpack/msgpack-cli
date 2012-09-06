
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
	public 	partial class UnpackingTest_Combinations_Nil
	{
		[Test]
		public void TestUnpackNull_ByteArray_True()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackNull( new byte[] { 0xC3 } ) );
		}
		
		[Test]
		public void TestUnpackNull_Stream_True()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			{
				Assert.Throws<MessageTypeException>( () => Unpacking.UnpackNull( buffer ) );
			}
		}

		[Test]
		public void TestUnpackNull_ByteArray_False()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackNull( new byte[] { 0xC2 } ) );
		}
		
		[Test]
		public void TestUnpackNull_Stream_False()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			{
				Assert.Throws<MessageTypeException>( () => Unpacking.UnpackNull( buffer ) );
			}
		}
		
		[Test]
		public void TestUnpackNull_ByteArray_AsIs()
		{
			var result = Unpacking.UnpackNull( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
		
		[Test]
		public void TestUnpackNull_Stream_Nil_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			{
				var result = Unpacking.UnpackNull( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.IsNull( result );
			}
		}

		[Test]
		public void TestUnpackNull_ByteArray_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackNull( new byte[ 0 ] ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackNull( default( byte[] ) ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Offset_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackNull( default( byte[] ), 0 ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Offset_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackNull( new byte[]{ 0x1 }, -1 ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Offset_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackNull( new byte[]{ 0x1 }, 1 ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Offset_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackNull( new byte[ 0 ], 0 ) );
		}

		[Test]
		public void TestUnpackNull_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackNull( default( Stream ) ) );
		}

		[Test]
		public void TestUnpackNull_ByteArray_Offset_OffsetIsValid_OffsetIsRespected()
		{
			var result = Unpacking.UnpackNull( new byte[] { 0xFF, 0xC0, 0xFF }, 1 );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
	}
}