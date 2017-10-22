#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
	partial class VersioningTest
	{
#if !AOT && !SILVERLIGHT

		[Test]
		public void TestExtraField_NotExtensible_Array_FieldBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.FieldBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Array_FieldBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.FieldBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Array_FieldBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_FieldBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.FieldBased ) );
		}
#endif // !AOT && !SILVERLIGHT

		[Test]
		public void TestExtraField_NotExtensible_Array_ReflectionBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.ReflectionBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Array_ReflectionBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.ReflectionBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Array_ReflectionBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.ReflectionBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_ReflectionBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.ReflectionBased ) );
		}
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !AOT && !SILVERLIGHT && !XAMARIN

		[Test]
		public void TestExtraField_NotExtensible_Array_CodeDomBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.CodeDomBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Array_CodeDomBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Array, EmitterFlavor.CodeDomBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Array_CodeDomBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.CodeDomBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_CodeDomBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.CodeDomBased ) );
		}
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !AOT && !SILVERLIGHT && !XAMARIN
#if !AOT && !SILVERLIGHT

		[Test]
		public void TestExtraField_NotExtensible_Map_FieldBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.FieldBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_FieldBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.FieldBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Map_FieldBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_FieldBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.FieldBased ) );
		}

		[Test]
		public void TestFieldModified_Map_FieldBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.FieldBased );
		}
#endif // !AOT && !SILVERLIGHT

		[Test]
		public void TestExtraField_NotExtensible_Map_ReflectionBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.ReflectionBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_ReflectionBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.ReflectionBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Map_ReflectionBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.ReflectionBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_ReflectionBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.ReflectionBased ) );
		}

		[Test]
		public void TestFieldModified_Map_ReflectionBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.ReflectionBased );
		}
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !AOT && !SILVERLIGHT && !XAMARIN

		[Test]
		public void TestExtraField_NotExtensible_Map_CodeDomBased_Classic_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.CodeDomBased, PackerCompatibilityOptions.Classic );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_CodeDomBased_None_Fail()
		{
			TestExtraFieldCore( SerializationMethod.Map, EmitterFlavor.CodeDomBased, PackerCompatibilityOptions.None );
		}

		[Test]
		public void TestMissingField_Map_CodeDomBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.CodeDomBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_CodeDomBased_Fail()
		{
			Assert.Throws<SerializationException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.CodeDomBased ) );
		}

		[Test]
		public void TestFieldModified_Map_CodeDomBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.CodeDomBased );
		}
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !AOT && !SILVERLIGHT && !XAMARIN
	}

}
