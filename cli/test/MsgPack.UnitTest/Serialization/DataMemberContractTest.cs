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
using System.Reflection;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class DataMemberContractTest
	{
		[Test]
		public void TestEmitDefaultValue_AttributeIsNotNull_SameAsAttributeEmitDefaultValue()
		{
			var attribute = new DataMemberAttribute() { EmitDefaultValue = false };
			var member= MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, attribute );
			Assert.That( target.EmitDefaultValue, Is.EqualTo( attribute.EmitDefaultValue ) );
		}

		[Test]
		public void TestEmitDefaultValue_AttributeIsNull_True()
		{
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, null );
			Assert.That( target.EmitDefaultValue, Is.True );
		}

		[Test]
		public void TestIsRequired_AttributeIsNotNull_SameAsAttributeIsRequired()
		{
			var attribute = new DataMemberAttribute() { IsRequired = true };
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, attribute );
			Assert.That( target.IsRequired, Is.EqualTo( attribute.IsRequired ) );
		}

		[Test]
		public void TestIsRequired_AttributeIsNull_False()
		{
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, null );
			Assert.That( target.IsRequired, Is.False );
		}

		[Test]
		public void TestName_AttributeIsNotNull_SameAsAttributeName()
		{
			var attribute = new DataMemberAttribute() { Name = "Abc" };
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, attribute );
			Assert.That( target.Name, Is.EqualTo( attribute.Name ) );
		}

		[Test]
		public void TestName_AttributeIsNull_MemberName()
		{
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, null );
			Assert.That( target.Name, Is.EqualTo( member.Name ) );
		}

		[Test]
		public void TestOrder_AttributeIsNotNull_SameAsAttributeOrder()
		{
			var attribute = new DataMemberAttribute() { Order = 2 };
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, attribute );
			Assert.That( target.Order, Is.EqualTo( attribute.Order ) );
		}

		[Test]
		public void TestOrder_AttributeIsNull_EqualToUnspecifiedOrder()
		{
			var member = MethodBase.GetCurrentMethod();

			var target = new DataMemberContract( member, null );
			Assert.That( target.Order, Is.EqualTo( DataMemberContract.UnspecifiedOrder ) );
		}
	}
}
