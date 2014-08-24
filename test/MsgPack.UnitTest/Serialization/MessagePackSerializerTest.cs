#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using TearDownAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCleanupAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackSerializerTest
	{
		[TearDown]
		public void TearDown()
		{
			// Reset Default
			SerializationContext.Default = new SerializationContext();
		}

#pragma warning disable 618
		[Test]
		public void TestCreate1_WithoutContext_NewInstance()
		{
			var first = MessagePackSerializer.Create<int>();
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>();
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		public void TestCreate1_WithContext_NewInstance()
		{
			var context = new SerializationContext();
			var first = MessagePackSerializer.Create<int>( context );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>( context );
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		public void TestCreate1_WithContext_Null_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Create<int>( null ) );
		}

		[Test]
		public void TestCreate_WithoutContext_NewInstance()
		{
			var first = MessagePackSerializer.Create( typeof( int ) );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create( typeof( int ) );
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		public void TestCreate_WithoutContext_SameTypeAsCreate1()
		{
			var first = MessagePackSerializer.Create( typeof( int ) );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>();
			Assert.That( second, Is.Not.Null );
			Assert.That( first.GetType(), Is.EqualTo( second.GetType() ) );
		}

		[Test]
		public void TestCreate_WithContext_NewInstance()
		{
			var context = new SerializationContext();
			var first = MessagePackSerializer.Create( typeof( int ), context );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create( typeof( int ), context );
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		public void TestCreate_WithContext_SameTypeAsCreate1()
		{
			var context = new SerializationContext();
			var first = MessagePackSerializer.Create( typeof( int ), context );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>( context );
			Assert.That( second, Is.Not.Null );
			Assert.That( first.GetType(), Is.EqualTo( second.GetType() ) );
		}

		[Test]
		public void TestCreate_WithoutContext_TypeIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Create( null ) );
		}

		[Test]
		public void TestCreate_WithContext_TypeIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Create( null, new SerializationContext() ) );
		}

		[Test]
		public void TestCreate_WithContext_ContextIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Create( typeof( int ), null ) );
		}
#pragma warning restore 618

		[Test]
		public void TestGet1_WithoutContext_Ok()
		{
			var instance = MessagePackSerializer.Get<int>();
			Assert.That( instance, Is.Not.Null );
		}

		[Test]
		public void TestGet1_WithContext_Ok()
		{
			var context = new SerializationContext();
			var instance = MessagePackSerializer.Get<int>( context );
			Assert.That( instance, Is.Not.Null );
		}

		[Test]
		public void TestGet1_WithContext_Null_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Get<int>( null ) );
		}

		[Test]
		public void TestGet_WithoutContext_Ok()
		{
			var instance = MessagePackSerializer.Get( typeof( int ) );
			Assert.That( instance, Is.Not.Null );
		}

		[Test]
		public void TestGet_WithoutContext_SameTypeAsGet1()
		{
			var first = MessagePackSerializer.Get( typeof( int ) );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Get<int>();
			Assert.That( second, Is.Not.Null );
			Assert.That( first.GetType(), Is.EqualTo( second.GetType() ) );
		}

		[Test]
		public void TestGet_WithContext_Ok()
		{
			var context = new SerializationContext();
			var instance = MessagePackSerializer.Get( typeof( int ), context );
			Assert.That( instance, Is.Not.Null );
		}

		[Test]
		public void TestGet_WithContext_SameTypeAsGet1()
		{
			var context = new SerializationContext();
			var first = MessagePackSerializer.Get( typeof( int ), context );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Get<int>( context );
			Assert.That( second, Is.Not.Null );
			Assert.That( first.GetType(), Is.EqualTo( second.GetType() ) );
		}

		[Test]
		public void TestGet_WithoutContext_TypeIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Get( null ) );
		}

		[Test]
		public void TestGet_WithContext_TypeIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Get( null, new SerializationContext() ) );
		}

		[Test]
		public void TestGet_WithContext_ContextIsNull_Fail()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackSerializer.Get( typeof( int ), null ) );
		}
	}
}
