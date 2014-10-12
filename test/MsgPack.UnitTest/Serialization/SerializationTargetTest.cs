#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;

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
	public class SerializationTargetTest
	{
		// ReSharper disable UnusedMember.Local
		// member names
		private const string PublicProperty = "PublicProperty";
		private const string PublicReadOnlyProperty = "PublicReadOnlyProperty";
		private const string NonPublicProperty = "NonPublicProperty";
		private const string PublicPropertyPlain = "PublicPropertyPlain";
		private const string PublicReadOnlyPropertyPlain = "PublicReadOnlyPropertyPlain";
		private const string NonPublicPropertyPlain = "NonPublicPropertyPlain";
		private const string CollectionReadOnlyProperty = "CollectionReadOnlyProperty";
		private const string PublicField = "PublicField";
		private const string PublicReadOnlyField = "PublicReadOnlyField";
		private const string NonPublicField = "NonPublicField";
		private const string PublicFieldPlain = "PublicFieldPlain";
		private const string PublicReadOnlyFieldPlain = "PublicReadOnlyFieldPlain";
		private const string NonPublicFieldPlain = "NonPublicFieldPlain";
#if !NETFX_CORE && !WINDOWS_PHONE
		private const string NonSerializedPublicField = "NonSerializedPublicField";
		private const string NonSerializedPublicReadOnlyField = "NonSerializedPublicReadOnlyField";
		private const string NonSerializedNonPublicField = "NonSerializedNonPublicField";
		private const string NonSerializedPublicFieldPlain = "NonSerializedPublicFieldPlain";
		private const string NonSerializedPublicReadOnlyFieldPlain = "NonSerializedPublicReadOnlyFieldPlain";
		private const string NonSerializedNonPublicFieldPlain = "NonSerializedNonPublicFieldPlain";
#endif
		// ReSharper restore UnusedMember.Local

		[Test]
		public void TestPlain()
		{
			// includes issue28
			TestCore<PlainClass>( PublicProperty, PublicField, CollectionReadOnlyProperty );
		}

		[Test]
		public void TestAnnotated()
		{
			TestCore<AnnotatedClass>(
				PublicProperty, NonPublicProperty, PublicField, NonPublicField,
#if !NETFX_CORE && !WINDOWS_PHONE
				NonSerializedPublicField, NonSerializedNonPublicField,
#endif
				CollectionReadOnlyProperty );
		}

		[Test]
		public void TestDataMember()
		{
			// includes issue33
			TestCore<DataMamberClass>(
				PublicProperty, NonPublicProperty, PublicField, NonPublicField,
#if !NETFX_CORE && !WINDOWS_PHONE
				NonSerializedPublicField, NonSerializedNonPublicField,
#endif
				CollectionReadOnlyProperty );
		}

		[Test]
		public void TestAliasInMessagePackMember()
		{
			var target = SerializationTarget.GetTargetMembers( typeof( AnnotatedClass ) );
			Assert.That( target.Any( m => m.Contract.Name == "Alias" && m.Contract.Name != m.Member.Name ) );
		}

		[Test]
		public void TestAliasInDataMember()
		{
			var target = SerializationTarget.GetTargetMembers( typeof( DataMamberClass ) );
			Assert.That( target.Any( m => m.Contract.Name == "Alias" && m.Contract.Name != m.Member.Name ) );
		}

		private static void TestCore<T>( params string[] expectedMemberNames )
		{
			var expected = expectedMemberNames.OrderBy( n => n ).ToArray();
			var actual = SerializationTarget.GetTargetMembers( typeof( T ) ).OrderBy( m => m.Member.Name ).Select( m => m.Member.Name ).ToArray();
			Assert.That( actual, Is.EqualTo( expected ), String.Join( ", ", actual ) );
		}

		//private static void PrintResultInDataContractSerializer<T>( T target )
		//{
		//	var serializer = new DataContractSerializer( typeof( T ) );
		//	using ( var buffer = new System.IO.MemoryStream() )
		//	{
		//		serializer.WriteObject( buffer, target );
		//		Console.WriteLine( System.Text.Encoding.UTF8.GetString( buffer.ToArray() ) );
		//	}
		//}
	}
}
