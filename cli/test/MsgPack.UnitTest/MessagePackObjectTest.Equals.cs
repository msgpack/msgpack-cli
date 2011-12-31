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
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	public partial class MessagePackObjectTest_Equals
	{
		// FIXME: Reals, Arrays, Maps, Strings/Bytes

		private static void TestEqualsCore( MessagePackObject left, MessagePackObject right, bool expected )
		{
			Assert.AreEqual( expected, left.Equals( right ), "{0}=={1}", left, right );
		}

		private static void AssertEquals( MessagePackObject left, MessagePackObject right )
		{
			TestEqualsCore( left, right, true );
		}

		private static void AssertNotEquals( MessagePackObject left, MessagePackObject right )
		{
			TestEqualsCore( left, right, false );
		}

		[Test]
		public void TestEquals_Nil_Nil_True()
		{
			AssertEquals( MessagePackObject.Nil, MessagePackObject.Nil );
		}

		[Test]
		public void TestEquals_Nil_NotNil_False()
		{
			AssertNotEquals( MessagePackObject.Nil, 0 );
		}

		[Test]
		public void TestEquals_Nil_NullString_True()
		{
			AssertEquals( MessagePackObject.Nil, default( string ) );
		}

		[Test]
		public void TestEquals_Nil_NullByteArray_True()
		{
			AssertEquals( MessagePackObject.Nil, default( byte[] ) );
		}

		[Test]
		public void TestEquals_Nil_NullMessagePackString_True()
		{
			AssertEquals( MessagePackObject.Nil, new MessagePackObject( default( MessagePackString ) ) );
		}

		[Test]
		public void TestEquals_Nil_NullArray_True()
		{
			AssertEquals( MessagePackObject.Nil, default( MessagePackObject[] ) );
		}

		[Test]
		public void TestEquals_Nil_NullList_True()
		{
			AssertEquals( MessagePackObject.Nil, new MessagePackObject( default( IList<MessagePackObject> ) ) );
		}

		[Test]
		public void TestEquals_Nil_NullDictionary_True()
		{
			Assert.IsTrue( MessagePackObject.Nil.Equals( default( MessagePackObjectDictionary ) ) );
		}


		[Test]
		public void TestEquals_True_True_True()
		{
			AssertEquals( true, true );
		}

		[Test]
		public void TestEquals_True_False_False()
		{
			AssertNotEquals( true, false );
		}

		[Test]
		public void TestEquals_True_NonBoolean_False()
		{
			AssertNotEquals( true, 1 );
		}

		[Test]
		public void TestEquals_True_Nil_False()
		{
			AssertNotEquals( true, MessagePackObject.Nil );
		}


		[Test]
		public void TestEquals_False_True_False()
		{
			AssertNotEquals( false, true );
		}

		[Test]
		public void TestEquals_False_False_True()
		{
			AssertEquals( false, false );
		}

		[Test]
		public void TestEquals_False_NonBoolean_False()
		{
			AssertNotEquals( false, 0 );
		}

		[Test]
		public void TestEquals_False_Nil_False()
		{
			AssertNotEquals( false, MessagePackObject.Nil );
		}
	}
}
