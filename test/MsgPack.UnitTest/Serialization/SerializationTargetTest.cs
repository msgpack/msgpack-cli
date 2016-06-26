#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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

using System.Runtime.Serialization;
using System;
using System.Linq;

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
		private const string NonPublicCollectionProperty = "NonPublicCollectionProperty";
		private const string NonPublicCollectionField = "NonPublicCollectionField";
		private const string NonPublicCollectionReadOnlyProperty = "NonPublicCollectionReadOnlyProperty";
		private const string NonPublicCollectionReadOnlyField = "NonPublicCollectionReadOnlyField";
		private const string NonPublicDictionaryProperty = "NonPublicDictionaryProperty";
		private const string NonPublicDictionaryField = "NonPublicDictionaryField";
		private const string NonPublicDictionaryReadOnlyProperty = "NonPublicDictionaryReadOnlyProperty";
		private const string NonPublicDictionaryReadOnlyField = "NonPublicDictionaryReadOnlyField";
		private const string NonPublicIDictionaryProperty = "NonPublicIDictionaryProperty";
		private const string NonPublicIDictionaryField = "NonPublicIDictionaryField";
		private const string NonPublicIDictionaryReadOnlyProperty = "NonPublicIDictionaryReadOnlyProperty";
		private const string NonPublicIDictionaryReadOnlyField = "NonPublicIDictionaryReadOnlyField";
		private const string PublicField = "PublicField";
		private const string PublicReadOnlyField = "PublicReadOnlyField";
		private const string NonPublicField = "NonPublicField";
		private const string PublicFieldPlain = "PublicFieldPlain";
		private const string PublicReadOnlyFieldPlain = "PublicReadOnlyFieldPlain";
		private const string NonPublicFieldPlain = "NonPublicFieldPlain";
		private const string NonSerializedPublicField = "NonSerializedPublicField";
		private const string NonSerializedPublicReadOnlyField = "NonSerializedPublicReadOnlyField";
		private const string NonSerializedNonPublicField = "NonSerializedNonPublicField";
		private const string NonSerializedPublicFieldPlain = "NonSerializedPublicFieldPlain";
		private const string NonSerializedPublicReadOnlyFieldPlain = "NonSerializedPublicReadOnlyFieldPlain";
		private const string NonSerializedNonPublicFieldPlain = "NonSerializedNonPublicFieldPlain";
		// ReSharper restore UnusedMember.Local

		[Test]
		public void TestPlain()
		{
			// includes issue28
#if XAMARIN
#warning TODO: Xamain Workaround
			TestCore<PlainClass>( PublicProperty, NonSerializedPublicField, PublicField, CollectionReadOnlyProperty );
#else
			TestCore<PlainClass>( PublicProperty, PublicField, CollectionReadOnlyProperty );
#endif // XAMARIN
		}

		[Test]
		public void TestAnnotated()
		{
			TestCore<AnnotatedClass>(
				PublicProperty, NonPublicProperty, PublicField, NonPublicField,
				NonSerializedPublicField, NonSerializedNonPublicField,
				CollectionReadOnlyProperty, 
				NonPublicCollectionProperty, NonPublicCollectionField, NonPublicCollectionReadOnlyProperty, NonPublicCollectionReadOnlyField,
				NonPublicDictionaryProperty, NonPublicDictionaryField, NonPublicDictionaryReadOnlyProperty, NonPublicDictionaryReadOnlyField,
				NonPublicIDictionaryProperty, NonPublicIDictionaryField, NonPublicIDictionaryReadOnlyProperty, NonPublicIDictionaryReadOnlyField
			);
		}

		[Test]
		public void TestDataMember()
		{
			// includes issue33
			TestCore<DataMamberClass>(
				PublicProperty, NonPublicProperty, PublicField, NonPublicField,
				NonSerializedPublicField, NonSerializedNonPublicField,
				CollectionReadOnlyProperty,
				NonPublicCollectionProperty, NonPublicCollectionField, NonPublicCollectionReadOnlyProperty, NonPublicCollectionReadOnlyField,
				NonPublicDictionaryProperty, NonPublicDictionaryField, NonPublicDictionaryReadOnlyProperty, NonPublicDictionaryReadOnlyField,
				NonPublicIDictionaryProperty, NonPublicIDictionaryField, NonPublicIDictionaryReadOnlyProperty, NonPublicIDictionaryReadOnlyField
			);
		}

		[Test]
		public void TestAliasInMessagePackMember()
		{
			var target = SerializationTarget.Prepare( new SerializationContext(), typeof( AnnotatedClass ) ).Members;
			Assert.That( target.Any( m => m.Contract.Name == "Alias" && m.Contract.Name != m.Member.Name ) );
		}

		[Test]
		public void TestAliasInDataMember()
		{
			var target = SerializationTarget.Prepare( new SerializationContext(), typeof( DataMamberClass ) ).Members;
			Assert.That( target.Any( m => m.Contract.Name == "Alias" && m.Contract.Name != m.Member.Name ) );
		}

		[Test]
		public void TestIndexerOverload()
		{
			Assert.Throws<SerializationException>( () => SerializationTarget.Prepare( new SerializationContext(), typeof( WithIndexerOverload ) ) );
		}

		[Test]
		public void TestDuplicatedKey()
		{
			Assert.Throws<InvalidOperationException>( () => SerializationTarget.Prepare( new SerializationContext(), typeof( WithKeyDuplicate ) ) );
		}

		private static void TestCore<T>( params string[] expectedMemberNames )
		{
			var expected = expectedMemberNames.OrderBy( n => n ).ToArray();
			var actual = SerializationTarget.Prepare( new SerializationContext(), typeof( T ) ).Members.Where( m => m.Member != null ).OrderBy( m => m.Member.Name ).Select( m => m.Member.Name ).ToArray();
			Assert.That( actual, Is.EqualTo( expected ), String.Join( ", ", actual ) );
		}

		[Test]
		public void TestIgnoreKinds()
		{
			var result = SerializationTarget.Prepare( new SerializationContext(), typeof( IgnoreAttributesTester ) );
#if XAMARIN
#warning TODO: Xamain Workaround
			Assert.That( result.Members.Count, Is.EqualTo( 2 ), String.Join( ",", result.Members.Select( m => m.Contract.Name ).ToArray() ) ); ;
			Assert.That( result.Members[ 0 ].Contract.Name, Is.EqualTo( "NonSerialized" ), String.Join( ",", result.Members.Select( m => m.Contract.Name ).ToArray() ) );
			Assert.That( result.Members[ 1 ].Contract.Name, Is.EqualTo( "Vanilla" ), String.Join( ",", result.Members.Select( m => m.Contract.Name ).ToArray() ) );
#else
			Assert.That( result.Members.Count, Is.EqualTo( 1 ), String.Join( ",", result.Members.Select( m => m.Contract.Name ).ToArray() ) ); ;
			Assert.That( result.Members[ 0 ].Contract.Name, Is.EqualTo( "Vanilla" ), String.Join( ",", result.Members.Select( m => m.Contract.Name ).ToArray() ) );
#endif // XAMARIN
		}
	}
}
