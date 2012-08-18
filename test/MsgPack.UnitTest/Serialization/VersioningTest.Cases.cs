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
	partial class VersioningTest
	{

		[Test]
		public void TestExtraField_NotExtensible_Array_FieldBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Array_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestMissingField_Array_FieldBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_FieldBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.FieldBased ) );
		}

		[Test]
		public void TestExtraField_NotExtensible_Array_ContextBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Array_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestMissingField_Array_ContextBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_ContextBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.ContextBased ) );
		}

		[Test]
		public void TestExtraField_NotExtensible_Array_ExpressionBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Array_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestMissingField_Array_ExpressionBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Array, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestFieldInvalidType_Array_ExpressionBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Array, EmitterFlavor.ExpressionBased ) );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_FieldBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Map_FieldBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestMissingField_Map_FieldBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_FieldBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.FieldBased ) );
		}

		[Test]
		public void TestFieldModified_Map_FieldBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.FieldBased );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_ContextBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Map_ContextBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestMissingField_Map_ContextBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_ContextBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.ContextBased ) );
		}

		[Test]
		public void TestFieldModified_Map_ContextBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.ContextBased );
		}

		[Test]
		public void TestExtraField_NotExtensible_Map_ExpressionBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestExtraFieldCore<VersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased ) );
		}


		[Test]
		public void TestExtraField_MessagePackMemberExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_MessagePackMemberExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<MessagePackMemberExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}


		[Test]
		public void TestExtraField_DataContractExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_DataContractExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<DataContractExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}


		[Test]
		public void TestExtraField_PlainExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestExtraFieldRoundTrip_PlainExtensible_Map_ExpressionBased_ExtraIsStoredAsExtensionData()
		{
			TestExtraFieldRoundTripCore<PlainExtensibleVersioningTestTarget>( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestMissingField_Map_ExpressionBased_MissingIsTreatedAsNil()
		{
			TestMissingFieldCore( SerializationMethod.Map, EmitterFlavor.ExpressionBased );
		}

		[Test]
		public void TestFieldInvalidType_Map_ExpressionBased_Fail()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => TestFieldInvalidTypeCore( SerializationMethod.Map, EmitterFlavor.ExpressionBased ) );
		}

		[Test]
		public void TestFieldModified_Map_ExpressionBased_ExtraIsStoredAsExtensionData_MissingIsTreatedAsNil()
		{
			TestFieldSwappedCore( EmitterFlavor.ExpressionBased );
		}
	}
}