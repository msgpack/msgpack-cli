#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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

using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///	A sample to describe constructor related behavior.
	/// </summary>
	[TestFixture]
	public class ConstructorBasedDeserializationSample
	{
		/// <summary>
		///	Demonstrates constructor based deserialization.
		/// </summary>
		[Test]
		public void DoConstructorBasedDeserialization()
		{
			// As of 0.8, constructor based deserialization is relaxed.
			// 1. If the type have a constructor with MessagePackDeserializationConstructorAttribute, then it will be used for deserialization.
			// 2. Else, if the type have a default public constructor then it will be used for deserialization.
			// 3. Otherwise, most parameterful constructor will be used.

			var serializerForSimpleRecord = MessagePackSerializer.Get<MySimpleRecordClass>();
			using ( var buffer = new MemoryStream() )
			{
				serializerForSimpleRecord.Pack( buffer, new MySimpleRecordClass( "John Doe" ) );
				buffer.Position = 0;
				Assert.That( serializerForSimpleRecord.Unpack( buffer ).Name, Is.EqualTo( "John Doe" ) );
			}

			var serializerForComplexRecord = MessagePackSerializer.Get<MyComplexRecordClass>();
			using ( var buffer = new MemoryStream() )
			{
				serializerForComplexRecord.Pack( buffer, new MyComplexRecordClass( "John Doe" ) );
				buffer.Position = 0;
				Assert.That( serializerForComplexRecord.Unpack( buffer ).Name, Is.EqualTo( "John Doe" ) );
			}
		}
	}

	/// <summary>
	///	"Record" class which has getters and parameterful constructor.
	/// </summary>
	public class MySimpleRecordClass
	{
		public string Name { get; private set; }

		public MySimpleRecordClass( string name )
		{
			this.Name = name;
		}
	}

	/// <summary>
	///	A example class which has getters and parameterless and parameterful constructor,
	///	and the parameterful constructor is qualified with MessagePackDeserializationConstructor.
	/// </summary>
	public class MyComplexRecordClass
	{
		public string Name { get; private set; }

		public bool ExtraProperty { get; private set; }

		public MyComplexRecordClass()
		{
			this.Name = "<WTF>";
		}

		[MessagePackDeserializationConstructor]
		public MyComplexRecordClass( string name )
		{
			this.Name = name;
		}

		public void SetExtraPropertyFromDomainLogic( bool value )
		{
			this.ExtraProperty = value;
		}
	}
}