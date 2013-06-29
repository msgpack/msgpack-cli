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
	public partial class MessagePackObjectTest_Equals
	{
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

		[Test]
		public void TestEquals_OtherIsNotMessagePackObject_False()
		{
			Assert.IsFalse( new MessagePackObject( 0 ).Equals( DateTimeKind.Unspecified ) );
		}


		[Test]
		public void TestEquals_Array_EqualArray_True()
		{
			var target = new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } );
			var other = new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } );
			Assert.IsTrue( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Array_SameLengthButDifferArray_False()
		{
			var target = new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } );
			var other = new MessagePackObject( new MessagePackObject[] { 1, 3, 2 } );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Array_SubArray_False()
		{
			var target = new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } );
			var other = new MessagePackObject( new MessagePackObject[] { 1, 2 } );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Array_Null_False()
		{
			var target = new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } );
			Assert.IsFalse( target.Equals( default( object ) ) );
		}

		[Test]
		public void TestEquals_EmptyArray_Empty_True()
		{
			var target = new MessagePackObject( new MessagePackObject[ 0 ] );
			Assert.IsTrue( target.Equals( new MessagePackObject[ 0 ] ) );
		}

		[Test]
		public void TestEquals_EmptyArray_Null_False()
		{
			var target = new MessagePackObject( new MessagePackObject[ 0 ] );
			Assert.IsFalse( target.Equals( default( object ) ) );
		}

		[Test]
		public void TestEquals_Array_Raw_False()
		{
			var target = new MessagePackObject( new MessagePackObject[] { ( byte )'A', ( byte )'B', ( byte )'C' } );
			var other = new MessagePackObject( "ABC" );
			Assert.IsFalse( target.Equals( other ) );
		}


		[Test]
		public void TestEquals_Map_EqualMap_True()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			var other = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			Assert.IsTrue( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Map_SameLengthButDifferKeyMap_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			var other = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "D", 2 }, { "C", 3 } } );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Map_SameLengthButDifferValueMap_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			var other = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 4 }, { "C", 3 } } );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Map_EquivalantMap_True()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			var other = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "C", 3 }, { "B", 2 } } );
			Assert.IsTrue( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Map_SubMap_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			var other = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 } } );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquals_Map_Null_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 }, { "C", 3 } } );
			Assert.IsFalse( target.Equals( default( object ) ) );
		}

		[Test]
		public void TestEquals_EmptyMap_Empty_True()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary( 0 ) );
			Assert.IsTrue( target.Equals( new MessagePackObjectDictionary( 0 ) ) );
		}

		[Test]
		public void TestEquals_EmptyMap_Null_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary( 0 ) );
			Assert.IsFalse( target.Equals( default( object ) ) );
		}

		[Test]
		public void TestEquals_Map_Raw_False()
		{
			var target = new MessagePackObject( new MessagePackObjectDictionary { { "A", "A" }, { "B", "B" }, { "C", "C" } } );
			var other = new MessagePackObject( "ABC" );
			Assert.IsFalse( target.Equals( other ) );
		}

		[Test]
		public void TestEquality_ValueEqual()
		{
			foreach ( var testCase in
				new[]
				{
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {1}, true ),
					Tuple.Create( 0, 1, new byte[] {1}, new byte[] {1}, false ),
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {1, 2}, false ),
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {2}, false ),
				} )
			{
				checked
				{
					MessagePackObject left = new MessagePackExtendedTypeObject( ( byte )testCase.Item1, testCase.Item3 );
					MessagePackObject right = new MessagePackExtendedTypeObject( ( byte )testCase.Item2, testCase.Item4 );

					Assert.That( left.Equals( right ), Is.EqualTo( testCase.Item5 ), "IEquatable.Equals" );
					Assert.That( left.Equals( ( object )right ), Is.EqualTo( testCase.Item5 ), "Equals" );
					Assert.That( left == right, Is.EqualTo( testCase.Item5 ), "==" );
					Assert.That( left != right, Is.EqualTo( !testCase.Item5 ), "!=" );
				}
			}
		}
	}
}
