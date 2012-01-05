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
using System.Runtime.Serialization;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class EmittingSerializerBuilderTest
	{
		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestCreateSerializer_NonMemberType()
		{
			var target = new EmittingSerializerBuilder<EmptyType>( new SerializationContext() );
			target.CreateSerializer( SerializationMemberOption.OptOut );
		}

		[Test]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void TestCreateSerializer_InvalidSerializationMemberOption()
		{
			var target = new EmittingSerializerBuilder<string>( new SerializationContext() );
			target.CreateSerializer( ( SerializationMemberOption )( -1 ) );
		}

		private sealed class EmptyType { }
	}
}
