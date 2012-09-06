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
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfArrayOfNotItemType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( KeyValuePair<string, bool>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( KeyValuePair<MessagePackObject, MessagePackObject>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfArrayOfItemType_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( KeyValuePair<string, int>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfIEnumerableOfMessagePackObject_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfIListOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( IList<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfListOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( List<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfIDictionaryOfMessagePackObject_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( IDictionary<MessagePackObject, MessagePackObject> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNotNull_IsTypeOfMessagePackObjectDictionary_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsTypeOf( typeof( MessagePackObjectDictionary ) ) );
		}
		
		[Test]
		public void TestIsMap_DictionaryNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  new MessagePackObjectDictionary() { { "1", 1 } } ).IsMap );
		}
		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfArrayOfNotItemType_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new MessagePackObjectDictionary() ).IsTypeOf( typeof( KeyValuePair<string, bool>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfArrayOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject( new MessagePackObjectDictionary() ).IsTypeOf( typeof( KeyValuePair<MessagePackObject, MessagePackObject>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfArrayOfItemType_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( KeyValuePair<string, int>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfIEnumerableOfMessagePackObject_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfIListOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( IList<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfListOfMessagePackObject_False()
		{
			Assert.AreEqual( false, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( List<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfIDictionaryOfMessagePackObject_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( IDictionary<MessagePackObject, MessagePackObject> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryEmptyNotNull_IsTypeOfMessagePackObjectDictionary_True()
		{
			Assert.AreEqual( true, new MessagePackObject(  new MessagePackObjectDictionary() ).IsTypeOf( typeof( MessagePackObjectDictionary ) ) );
		}
		
		[Test]
		public void TestIsMap_DictionaryEmptyNotNull_True()
		{
			Assert.IsTrue( new MessagePackObject(  new MessagePackObjectDictionary() ).IsMap );
		}
		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfArrayOfNotItemType_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( KeyValuePair<string, bool>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfArrayOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject( default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( KeyValuePair<MessagePackObject, MessagePackObject>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfArrayOfItemType_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( KeyValuePair<string, int>[] ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfIEnumerableOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfIListOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( IList<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfListOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( List<KeyValuePair<MessagePackObject, MessagePackObject>> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfIDictionaryOfMessagePackObject_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( IDictionary<MessagePackObject, MessagePackObject> ) ) );
		}

		[Test]
		public void TestIsTypeOf_DictionaryNull_IsTypeOfMessagePackObjectDictionary_Null()
		{
			Assert.AreEqual( null, new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsTypeOf( typeof( MessagePackObjectDictionary ) ) );
		}
		
		[Test]
		public void TestIsMap_DictionaryNull_False()
		{
			Assert.IsFalse( new MessagePackObject(  default( MessagePackObjectDictionary ) ).IsMap );
		}
	}
}