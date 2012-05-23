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

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackSerializerTest
	{
		[Test]
		public void TestCreate_WithoutContext_NewInstance()
		{
			var first = MessagePackSerializer.Create<int>();
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>();
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		public void TestCreate_WithContext_NewInstance()
		{
			var context = new SerializationContext();
			var first = MessagePackSerializer.Create<int>( context );
			Assert.That( first, Is.Not.Null );
			var second = MessagePackSerializer.Create<int>( context );
			Assert.That( second, Is.Not.Null );
			Assert.That( first, Is.Not.SameAs( second ) );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestCreate_WithContext_Null_Fail()
		{
			MessagePackSerializer.Create<int>( null );
		}
	}
}
