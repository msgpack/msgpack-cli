
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
	public partial class UnpackingTest_Combinations_Boolean
	{
		[Test]
		public void TestUnpackBoolean_ByteArray_True_AsIs()
		{
			var result = Unpacking.UnpackBoolean( new byte[] { 0xC3 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( true, result.Value );
		}
		
		[Test]
		public void TestUnpackBoolean_Stream_True_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC3 } ) )
			{
				var result = Unpacking.UnpackBoolean( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( true, result );
			}
		}

		[Test]
		public void TestUnpackBoolean_ByteArray_False_AsIs()
		{
			var result = Unpacking.UnpackBoolean( new byte[] { 0xC2 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( false, result.Value );
		}
		
		[Test]
		public void TestUnpackBoolean_Stream_False_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			{
				var result = Unpacking.UnpackBoolean( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( false, result );
			}
		}
		
		[Test]
		[ExpectedException( typeof( MessageTypeException ) )]
		public void TestUnpackBoolean_ByteArray_Nil()
		{
			var result = Unpacking.UnpackBoolean( new byte[] { 0xC0 } );
		}
		
		[Test]
		[ExpectedException( typeof( MessageTypeException ) )]
		public void TestUnpackBoolean_Stream_Nil()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			{
				var result = Unpacking.UnpackBoolean( buffer );
			}
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBoolean_ByteArray_Empty()
		{
			Unpacking.UnpackBoolean( new byte[ 0 ] );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBoolean_ByteArray_Null()
		{
			Unpacking.UnpackBoolean( default( byte[] ) );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBoolean_ByteArray_Offset_Null()
		{
			Unpacking.UnpackBoolean( default( byte[] ), 0 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void TestUnpackBoolean_ByteArray_Offset_OffsetIsNegative()
		{
			Unpacking.UnpackBoolean( new byte[]{ 0x1 }, -1 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBoolean_ByteArray_Offset_OffsetIsTooBig()
		{
			Unpacking.UnpackBoolean( new byte[]{ 0x1 }, 1 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestUnpackBoolean_ByteArray_Offset_Empty()
		{
			Unpacking.UnpackBoolean( new byte[ 0 ], 0 );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackBoolean_Stream_Null()
		{
			Unpacking.UnpackBoolean( default( Stream ) );
		}

		[Test]
		public void TestUnpackBoolean_ByteArray_Offset_OffsetIsValid_OffsetIsRespected()
		{
			// Offset 1 is true
			var result = Unpacking.UnpackBoolean( new byte[] { 0xFF, 0xC3, 0xFF }, 1 );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( true, result.Value );
		}
	}
}