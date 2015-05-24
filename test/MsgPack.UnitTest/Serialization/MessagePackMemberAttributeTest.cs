#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.IO;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackMemberAttributeTest
	{
		private static void TestDataContractAndMessagePackMemberAndNonSerializedAreMixedCore( SerializationMethod method )
		{
			var context = new SerializationContext { SerializationMethod = method };

			using ( var buffer = new MemoryStream() )
			{
				var target = new MessagePackMemberAndDataMemberMixedTarget();
				target.ShouldSerialized1 = 111;
				target.ShouldSerialized2 = 222;
				target.ShouldSerialized3 = 333;
				target.ShouldNotSerialized1 = 444;
				target.ShouldNotSerialized2 = 555;
				var serializer = MessagePackSerializer.CreateInternal<MessagePackMemberAndDataMemberMixedTarget>( context, PolymorphismSchema.Default );
				serializer.Pack( buffer, target );

				buffer.Position = 0;
				var intermediate = Unpacking.UnpackObject( buffer );

				if ( method == SerializationMethod.Array )
				{
					var asArray = intermediate.AsList();
					Assert.That( asArray.Count, Is.EqualTo( 3 ) );
					Assert.That( asArray[ 0 ] == target.ShouldSerialized1 );
					Assert.That( asArray[ 1 ] == target.ShouldSerialized2 );
					Assert.That( asArray[ 2 ] == target.ShouldSerialized3 );
				}
				else
				{
					var asMap = intermediate.AsDictionary();
					Assert.That( asMap.Count, Is.EqualTo( 3 ) );
					Assert.That( asMap[ "ShouldSerialized1" ] == target.ShouldSerialized1 );
					Assert.That( asMap[ "ShouldSerialized2" ] == target.ShouldSerialized2 );
					Assert.That( asMap[ "ShouldSerialized3" ] == target.ShouldSerialized3 );
				}

				buffer.Position = 0;

				var result = serializer.Unpack( buffer );

				Assert.That( result.ShouldSerialized1, Is.EqualTo( target.ShouldSerialized1 ) );
				Assert.That( result.ShouldSerialized2, Is.EqualTo( target.ShouldSerialized2 ) );
				Assert.That( result.ShouldSerialized3, Is.EqualTo( target.ShouldSerialized3 ) );
				Assert.That( result.ShouldNotSerialized1, Is.Not.EqualTo( target.ShouldNotSerialized1 ).And.EqualTo( 0 ) );
				Assert.That( result.ShouldNotSerialized2, Is.Not.EqualTo( target.ShouldNotSerialized2 ).And.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestDataContractAndMessagePackMemberAndNonSerializedAreMixed_Array_PreferMessagsePackMemberAndNonSerializedIsIgnored()
		{
			TestDataContractAndMessagePackMemberAndNonSerializedAreMixedCore( SerializationMethod.Array );
		}

		[Test]
		public void TestDataContractAndMessagePackMemberAndNonSerializedAreMixed_Map_PreferMessagsePackMemberAndNonSerializedIsIgnored()
		{
			TestDataContractAndMessagePackMemberAndNonSerializedAreMixedCore( SerializationMethod.Map );
		}

		private static void TestDataContractAndNonSerializableAreMixedCore( SerializationMethod method )
		{
			var context = new SerializationContext { SerializationMethod = method };

			using ( var buffer = new MemoryStream() )
			{
				var target = new DataContractAndNonSerializedMixedTarget();
				target.ShouldSerialized = 111;
				var serializer = MessagePackSerializer.CreateInternal<DataContractAndNonSerializedMixedTarget>( context, PolymorphismSchema.Default );
				serializer.Pack( buffer, target );

				buffer.Position = 0;
				var intermediate = Unpacking.UnpackObject( buffer );

				if ( method == SerializationMethod.Array )
				{
					var asArray = intermediate.AsList();
					Assert.That( asArray.Count, Is.EqualTo( 1 ) );
					Assert.That( asArray[ 0 ] == target.ShouldSerialized );
				}
				else
				{
					var asMap = intermediate.AsDictionary();
					Assert.That( asMap.Count, Is.EqualTo( 1 ) );
					Assert.That( asMap[ "ShouldSerialized" ] == target.ShouldSerialized );
				}

				buffer.Position = 0;

				var result = serializer.Unpack( buffer );

				Assert.That( result.ShouldSerialized, Is.EqualTo( target.ShouldSerialized ) );
			}
		}

		[Test]
		public void TestDataContractAndNonSerializableAreMixed_Array_NonSerializedIsIgnored()
		{
			TestDataContractAndNonSerializableAreMixedCore( SerializationMethod.Array );
		}

		[Test]
		public void TestDataContractAndNonSerializableAreMixed_Map_NonSerializedIsIgnored()
		{
			TestDataContractAndNonSerializableAreMixedCore( SerializationMethod.Map );
		}
	}
}
