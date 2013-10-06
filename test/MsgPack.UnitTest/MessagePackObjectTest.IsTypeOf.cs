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
	[TestFixture]
	public partial class MessagePackObjectTest_IsTypeOf
	{
		[Test]
		public void TestIsTypeOf_Single_IsTypeOfSingle_True()
		{
			Assert.AreEqual( true, new MessagePackObject( 1.0f ).IsTypeOf( typeof( Single ) ) );
		}

		[Test]
		public void TestIsTypeOf_Single_IsTypeOfDouble_True()
		{
			Assert.AreEqual( true, new MessagePackObject( 1.0f ).IsTypeOf( typeof( Double ) ) );
		}

		[Test]
		public void TestIsTypeOf_Single_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, new MessagePackObject( 1.0f ).IsTypeOf( typeof( Int32 ) ) );
		}

		[Test]
		public void TestIsTypeOf_Nil_IsTypeOfSingle_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( Single ) ) );
		}


		[Test]
		public void TestIsTypeOf_Double_IsTypeOfSingle_False()
		{
			Assert.AreEqual( false, new MessagePackObject( 1.0 ).IsTypeOf( typeof( Single ) ) );
		}

		[Test]
		public void TestIsTypeOf_Double_IsTypeOfDouble_True()
		{
			Assert.AreEqual( true, new MessagePackObject( 1.0 ).IsTypeOf( typeof( Double ) ) );
		}

		[Test]
		public void TestIsTypeOf_Double_IsTypeOfInt32_False()
		{
			Assert.AreEqual( false, new MessagePackObject( 1.0 ).IsTypeOf( typeof( Int32 ) ) );
		}

		[Test]
		public void TestIsTypeOf_Nil_IsTypeOfDouble_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( Double ) ) );
		}


		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfByteArray_True()
		{
			Assert.AreEqual( true, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( byte[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfIEnumerableOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( IEnumerable<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfIListOfByte_True()
		{
			Assert.AreEqual( true, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( IList<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfListOfByte_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( List<byte> ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfString_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( string ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfCharArray_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( char[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfIEnumerableOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( IEnumerable<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfIListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( IList<char> ) ) );
		}

		[Test]
		public void TestIsTypeOf_NonStringBinary_IsTypeOfListOfChar_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( List<char> ) ) );
		}

		[Test]
		public void TestIsRaw_NonStringBinary_True()
		{
			Assert.IsTrue( new MessagePackObject( new byte[] { 0xff } ).IsRaw );
		}


		[Test]
		public void TestIsTypeOf_Binary_IsTypeOfMessagePackExtendedTypeObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new byte[] { 0xff } ).IsTypeOf( typeof( MessagePackExtendedTypeObject ) ) );
		}

		[Test]
		public void TestIsTypeOf_MessagePackExtendedTypeObject_IsTypeOfMessagePackExtendedTypeObject_True()
		{
			Assert.AreEqual( true, new MessagePackObject( new MessagePackExtendedTypeObject( 1, new byte[] { 1 } ) ).IsTypeOf( typeof( MessagePackExtendedTypeObject ) ) );
		}

		[Test]
		public void TestIsTypeOf_Nil_IsTypeOfMessagePackExtendedTypeObject_False()
		{
			Assert.AreEqual( false, MessagePackObject.Nil.IsTypeOf( typeof( MessagePackExtendedTypeObject ) ) );
		}
	}
}
