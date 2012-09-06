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
	public partial class MessagePackObjectTest_Exceptionals
	{
		[Test]
		public void TestAsEnumreable_String()
		{
			Assert.Throws<InvalidOperationException>( () => new MessagePackObject( "ABC" ).AsEnumerable() );
		}

		[Test]
		public void TestAsEnumreable_Nil_ReturnsNull()
		{
			Assert.IsNull( MessagePackObject.Nil.AsEnumerable() );
		}

		[Test]
		public void TestAsList_String()
		{
			Assert.Throws<InvalidOperationException>( () => new MessagePackObject( "ABC" ).AsList() );
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
		public void TestAsString_NotString()
		{
			Assert.Throws<InvalidOperationException>( () => new MessagePackObject( 0 ).AsString() );
		}

		[Test]
		public void TestAsStringUtf8_NotString()
		{
			Assert.Throws<InvalidOperationException>( () => new MessagePackObject( 0 ).AsStringUtf8() );
		}

		[Test]
		public void TestAsStringUtf16_NotString()
		{
			Assert.Throws<InvalidOperationException>( () => new MessagePackObject( 0 ).AsStringUtf16() );
		}

		[Test]
		public void TestIsTypeOf_Null()
		{
			Assert.Throws<ArgumentNullException>( () => new MessagePackObject( 0 ).IsTypeOf( null ) );
		}

		[Test]
		public void TestIsTypeOf_ForNull_Null()
		{
			Assert.Throws<ArgumentNullException>( () => new MessagePackObject( default( string ) ).IsTypeOf( null ) );
		}


		[Test]
		public void TestPackToMessage_Null()
		{
			Assert.Throws<ArgumentNullException>( () => new MessagePackObject( 0 ).PackToMessage( null, new PackingOptions() ) );
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
