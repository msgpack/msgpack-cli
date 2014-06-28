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

using MsgPack.Serialization.AbstractSerializers;
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
			TestCore<PlainClass>( PublicProperty, PublicField, CollectionReadOnlyProperty );
		}

		[Test]
		public void TestAnnotated()
		{
			TestCore<AnnotatedClass>( PublicProperty, NonPublicProperty, PublicField, NonPublicField, NonSerializedPublicField, NonSerializedNonPublicField, CollectionReadOnlyProperty );
		}

		[Test]
		public void TestDataMember()
		{
			// includes issue33
			TestCore<DataMamberClass>( PublicProperty, NonPublicProperty, PublicField, NonPublicField, NonSerializedPublicField, NonSerializedNonPublicField, CollectionReadOnlyProperty );
		}

		private static void TestCore<T>( params string[] expectedMemberNames )
		{
			var expected = expectedMemberNames.OrderBy( n => n ).ToArray();
			var actual = SerializationTarget.GetTargetMembers( typeof( T ) ).OrderBy( m => m.Contract.Name ).Select( m => m.Contract.Name ).ToArray();
			Assert.That( actual, Is.EqualTo( expected ), String.Join( ", ", actual ) );
		}

		// ReSharper disable UnusedMember.Local
		// ReSharper disable NotAccessedField.Local
		// ReSharper disable MemberHidesStaticFromOuterClass
		internal class PlainClass
		{
			public static int StaticProperty { get; set; }

			public int PublicProperty { get; set; }
			public int PublicField;

			private readonly int _publicReadOnlyProperty;
			public int PublicReadOnlyProperty { get { return this._publicReadOnlyProperty; } }
			public readonly int PublicReadOnlyField;
			internal int NonPublicProperty { get; set; }
			internal int NonPublicField;

#if !NETFX_CORE
			[NonSerialized]
			public int NonSerializedPublicField;

			[NonSerialized]
			public readonly int NonSerializedPublicReadOnlyField;

			[NonSerialized]
			// ReSharper disable once InconsistentNaming
			private int NonSerializedNonPublicField;
#endif
			private readonly List<int> _collectionReadOnlyProperty;
			public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

			public PlainClass()
			{
				StaticProperty = -1;
				this.PublicProperty = 1;
				this.PublicField = 2;
				this._publicReadOnlyProperty = 3;
				this.PublicReadOnlyField = 4;
				this.NonPublicProperty = 5;
				this.NonPublicField = 6;
#if !NETFX_CORE
				this.NonSerializedPublicField = 7;
				this.NonSerializedPublicReadOnlyField = 8;
				this.NonSerializedNonPublicField = 9;
#endif
				this._collectionReadOnlyProperty = new List<int> { 10 };
			}
		}

		internal class AnnotatedClass
		{
			public static int StaticProperty { get; set; }

			[MessagePackMember( 0 )]
			public int PublicProperty { get; set; }
			[MessagePackMember( 1 )]
			public int PublicField;

			private int _PublicReadOnlyProperty;
			[MessagePackMember( 2 )]
			public int PublicReadOnlyProperty { get { return this._PublicReadOnlyProperty; } }
			[MessagePackMember( 3 )]
			public readonly int PublicReadOnlyField;
			[MessagePackMember( 4 )]
			internal int NonPublicProperty { get; set; }
			[MessagePackMember( 5 )]
			internal int NonPublicField;

#if !NETFX_CORE
			[NonSerialized]
			[MessagePackMember( 6 )]
			public int NonSerializedPublicField;

			[NonSerialized]
			[MessagePackMember( 7 )]
			public readonly int NonSerializedPublicReadOnlyField;

			[NonSerialized]
			[MessagePackMember( 8 )]
			// ReSharper disable once InconsistentNaming
			private int NonSerializedNonPublicField;
#endif
			private readonly List<int> _collectionReadOnlyProperty;
			[MessagePackMember( 9 )]
			public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

			public int PublicPropertyPlain { get; set; }
			public int PublicFieldPlain;
			private readonly int _publicReadOnlyPropertyPlain;
			public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
			public int PublicReadOnlyFieldPlain;
			internal int NonPublicPropertyPlain { get; set; }
			internal int NonPublicFieldPlain;

#if !NETFX_CORE
			[NonSerialized]
			public int NonSerializedPublicFieldPlain;

			[NonSerialized]
			public readonly int NonSerializedPublicReadOnlyFieldPlain;

			[NonSerialized]
			// ReSharper disable once InconsistentNaming
			private int NonSerializedNonPublicFieldPlain;
#endif

			public AnnotatedClass()
			{
				StaticProperty = -1;
				this.PublicProperty = 1;
				this.PublicField = 2;
				this._PublicReadOnlyProperty = 3;
				this.PublicReadOnlyField = 4;
				this.NonPublicProperty = 5;
				this.NonPublicField = 6;
#if !NETFX_CORE
				this.NonSerializedPublicField = 7;
				this.NonSerializedPublicReadOnlyField = 8;
				this.NonSerializedNonPublicField = 9;
#endif
				this._collectionReadOnlyProperty = new List<int> { 10 };
				this.PublicPropertyPlain = 11;
				this.PublicFieldPlain = 12;
				this._publicReadOnlyPropertyPlain = 13;
				this.PublicReadOnlyFieldPlain = 14;
				this.NonPublicPropertyPlain = 15;
				this.NonPublicFieldPlain = 16;
#if !NETFX_CORE
				this.NonSerializedPublicFieldPlain = 17;
				this.NonSerializedPublicReadOnlyFieldPlain = 18;
				this.NonSerializedNonPublicFieldPlain = 19;
#endif
			}
		}

		[DataContract]
		internal class DataMamberClass
		{
			public static int StaticProperty { get; set; }

			[DataMember( Order = 0 )]
			public int PublicProperty { get; set; }
			[DataMember( Order = 1 )]
			public int PublicField;

			private int _PublicReadOnlyProperty;
			[DataMember( Order = 2 )]
			public int PublicReadOnlyProperty { get { return this._PublicReadOnlyProperty; } }
			[DataMember( Order = 3 )]
			public readonly int PublicReadOnlyField;
			[DataMember( Order = 4 )]
			internal int NonPublicProperty { get; set; }
			[DataMember( Order = 5 )]
			internal int NonPublicField;

#if !NETFX_CORE
			[NonSerialized]
			[DataMember( Order = 6 )]
			public int NonSerializedPublicField;

			[NonSerialized]
			[DataMember( Order = 7 )]
			public readonly int NonSerializedPublicReadOnlyField;

			[NonSerialized]
			[DataMember( Order = 8 )]
			// ReSharper disable once InconsistentNaming
			private int NonSerializedNonPublicField;
#endif
			private readonly List<int> _collectionReadOnlyProperty;
			[DataMember( Order = 9 )]
			public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

			public int PublicPropertyPlain { get; set; }
			public int PublicFieldPlain;
			private readonly int _publicReadOnlyPropertyPlain;
			public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
			public int PublicReadOnlyFieldPlain;
			internal int NonPublicPropertyPlain { get; set; }
			internal int NonPublicFieldPlain;

#if !NETFX_CORE
			[NonSerialized]
			public int NonSerializedPublicFieldPlain;

			[NonSerialized]
			public readonly int NonSerializedPublicReadOnlyFieldPlain;

			[NonSerialized]
			// ReSharper disable once InconsistentNaming
			private int NonSerializedNonPublicFieldPlain;
#endif

			public DataMamberClass()
			{
				StaticProperty = -1;
				this.PublicProperty = 1;
				this.PublicField = 2;
				this._PublicReadOnlyProperty = 3;
				this.PublicReadOnlyField = 4;
				this.NonPublicProperty = 5;
				this.NonPublicField = 6;
#if !NETFX_CORE
				this.NonSerializedPublicField = 7;
				this.NonSerializedPublicReadOnlyField = 8;
				this.NonSerializedNonPublicField = 9;
#endif
				this._collectionReadOnlyProperty = new List<int> { 10 };
				this.PublicPropertyPlain = 11;
				this.PublicFieldPlain = 12;
				this._publicReadOnlyPropertyPlain = 13;
				this.PublicReadOnlyFieldPlain = 14;
				this.NonPublicPropertyPlain = 15;
				this.NonPublicFieldPlain = 16;
#if !NETFX_CORE
				this.NonSerializedPublicFieldPlain = 17;
				this.NonSerializedPublicReadOnlyFieldPlain = 18;
				this.NonSerializedNonPublicFieldPlain = 19;
#endif
			}
		}
		// ReSharper restore MemberHidesStaticFromOuterClass
		// ReSharper restore NotAccessedField.Local
		// ReSharper restore UnusedMember.Local

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
