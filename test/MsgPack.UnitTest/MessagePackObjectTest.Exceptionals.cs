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
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	public partial class MessagePackObjectTest_Exceptionals
	{
		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestAsEnumreable_String()
		{
			new MessagePackObject( "ABC" ).AsEnumerable();
		}

		[Test]
		public void TestAsEnumreable_Nil_ReturnsNull()
		{
			Assert.IsNull( MessagePackObject.Nil.AsEnumerable() );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestAsList_String()
		{
			new MessagePackObject( "ABC" ).AsList();
		}

		[Test]
		public void TestAsList_Nil_ReturnsNull()
		{
			Assert.IsNull( MessagePackObject.Nil.AsList() );
		}

		[Test]
		public void TestAsBinary_Nil_ReturnsNull()
		{
			Assert.IsNull( MessagePackObject.Nil.AsBinary() );
		}

		[Test]
		public void TestAsString_Nil_ReturnsNull()
		{
			Assert.IsNull( MessagePackObject.Nil.AsString() );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestAsString_NotString()
		{
			Assert.IsNull( new MessagePackObject( 0 ).AsString() );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestAsStringUtf8_NotString()
		{
			Assert.IsNull( new MessagePackObject( 0 ).AsStringUtf8() );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestAsStringUtf16_NotString()
		{
			Assert.IsNull( new MessagePackObject( 0 ).AsStringUtf16() );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIsTypeOf_Null()
		{
			new MessagePackObject( 0 ).IsTypeOf( null );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIsTypeOf_ForNull_Null()
		{
			new MessagePackObject( default( string ) ).IsTypeOf( null );
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestPackToMessage_Null()
		{
			new MessagePackObject( 0 ).PackToMessage( null, new PackingOptions() );
		}


		[Test]
		public void TestOpExplicitSingle_Double_Success()
		{
			var target = new MessagePackObject( Double.MaxValue );
			Assert.AreEqual(
				( float )( Double.MaxValue ),
				( float )target
			);
		}

		[Test]
		public void TestOpExplicitDouble_Single_Success()
		{
			var target = new MessagePackObject( Single.MaxValue );
			Assert.AreEqual(
				( double )( Single.MaxValue ),
				( double )target
			);
		}
	}
}
