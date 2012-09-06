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
	partial class MessagePackObjectTest_IsTypeOf
	{
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfArrayOfNotByteType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { ( byte )'A' } ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { ( byte )'A' } ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfByteArray_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfString_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfIEnumerableOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfIListOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfListOfByte_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfCharArray_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfIEnumerableOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfIListOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNotNull_IsTypeOfListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[] { ( byte )'A' } ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_ByteArrayNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  new byte[] { ( byte )'A' } ).IsRaw );
		}
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfArrayOfNotByteType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[ 0 ] ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[ 0 ] ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfByteArray_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfString_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfIEnumerableOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfIListOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfListOfByte_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfCharArray_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfIEnumerableOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfIListOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayEmptyNotNull_IsTypeOfListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new byte[ 0 ] ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_ByteArrayEmptyNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  new byte[ 0 ] ).IsRaw );
		}
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfArrayOfNotByteType_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( byte[] ) ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfArrayOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( byte[] ) ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfByteArray_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfString_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfIEnumerableOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfIListOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfListOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfCharArray_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfIEnumerableOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfIListOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_ByteArrayNull_IsTypeOfListOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( byte[] ) ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_ByteArrayNull_False()
		{
			Assert.IsFalse( new MessagePackObject(  default( byte[] ) ).IsRaw );
		}
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfArrayOfNotByteType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( "A" ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( "A" ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfByteArray_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfString_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfIEnumerableOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfIListOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfListOfByte_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  "A" ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfCharArray_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  "A" ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfIEnumerableOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfIListOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  "A" ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNotNull_IsTypeOfListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  "A" ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_StringNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  "A" ).IsRaw );
		}
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfArrayOfNotByteType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( String.Empty ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( String.Empty ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfByteArray_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfString_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfIEnumerableOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfIListOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfListOfByte_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfCharArray_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfIEnumerableOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfIListOfChar_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringEmptyNotNull_IsTypeOfListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  String.Empty ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_StringEmptyNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  String.Empty ).IsRaw );
		}
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfArrayOfNotByteType_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( string ) ).IsTypeOf( typeof( bool[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfArrayOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( string ) ).IsTypeOf( typeof( MessagePackObject[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfByteArray_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfString_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( string ) ) );
		}
	
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfIEnumerableOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfIListOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( IList<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfListOfByte_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( List<byte> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfCharArray_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( char[] ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfIEnumerableOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfIListOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( IList<char> ) ) );
		}
		
		[Test]
		public void TestIsTypeOf_StringNull_IsTypeOfListOfChar_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( string ) ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_StringNull_False()
		{
			Assert.IsFalse( new MessagePackObject(  default( string ) ).IsRaw );
		}
	}
}