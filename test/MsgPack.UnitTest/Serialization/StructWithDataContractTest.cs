#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//   Samuel Cragg
//
#endregion -- License Terms --

using System.Runtime.Serialization;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif


namespace MsgPack.Serialization
{
#if !SILVERLIGHT || SILVERLIGHT_PRIVILEGED

	[TestFixture]
	public class StructWithDataContractTest
	{
		[Test]
		public void ShouldSerializeStructsWithDataContracts()
		{
			MessagePackSerializer<TestStruct> serializer = MessagePackSerializer.Get<TestStruct>();

			byte[] result = serializer.PackSingleObject( new TestStruct() );

			Assert.That( result, Is.Not.Null );
		}

		[Test]
		public void ShouldDeserializeStructsWithDataContracts()
		{
			MessagePackSerializer<TestStruct> serializer = MessagePackSerializer.Get<TestStruct>();
			byte[] bytes = serializer.PackSingleObject( new TestStruct( 123 ) );

			TestStruct result = serializer.UnpackSingleObject( bytes );

			Assert.That( result.Field, Is.EqualTo( 123 ) );
		}

		[DataContract]
		public struct TestStruct
		{
			[DataMember]
			private int _field;

			public TestStruct( int field )
			{
				_field = field;
			}

			public int Field
			{
				get { return _field; }
			}
		}
	}

#endif // !SILVERLIGHT || SILVERLIGHT_PRIVILEGED
}
