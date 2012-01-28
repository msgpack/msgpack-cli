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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class NilImplicationTest
	{
		private static bool _traceOn = true;

		[SetUp]
		public void SetUp()
		{
			if ( _traceOn )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
			}

			SerializationMethodGeneratorManager.DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.CanDump;
		}

		[TearDown]
		public void TearDown()
		{
			if ( _traceOn )
			{
				try
				{
					DefaultSerializationMethodGeneratorManager.DumpTo();
				}
				finally
				{
					DefaultSerializationMethodGeneratorManager.Refresh();
				}
			}
		}

		private static void PackValuesAsArray(
			Packer packer,
			MessagePackObject memberDefault,
			MessagePackObject nullButValueType,
			MessagePackObject nullAndNullableValueType,
			MessagePackObject nullAndReferenceType,
			MessagePackObject prohibitReferenceType
		)
		{
			packer.PackArrayHeader( 5 );
			packer.Pack( memberDefault );
			packer.Pack( nullButValueType );
			packer.Pack( nullAndNullableValueType );
			packer.Pack( nullAndReferenceType );
			packer.Pack( prohibitReferenceType );
		}

		private static void PackValuesAsMap(
			Packer packer,
			MessagePackObject memberDefault,
			MessagePackObject nullButValueType,
			MessagePackObject nullAndNullableValueType,
			MessagePackObject nullAndReferenceType,
			MessagePackObject prohibitReferenceType
		)
		{
			packer.PackMapHeader( 5 );
			packer.PackString( "MemberDefault" );
			packer.Pack( memberDefault );
			packer.PackString( "NullButValueType" );
			packer.Pack( nullButValueType );
			packer.PackString( "NullAndNullableValueType" );
			packer.Pack( nullAndNullableValueType );
			packer.PackString( "NullAndReferenceType" );
			packer.Pack( nullAndReferenceType );
			packer.PackString( "ProhibitReferenceType" );
			packer.Pack( prohibitReferenceType );
		}

		private static void TestCore(
			Action<NilImplicationTestTarget> adjuster,
			Action<Packer, MessagePackObject, MessagePackObject, MessagePackObject, MessagePackObject, MessagePackObject> packing,
			MessagePackObject? memberDefault = null,
			MessagePackObject? nullButValueType = null,
			MessagePackObject? nullAndNullableValueType = null,
			MessagePackObject? nullAndReferenceType = null,
			MessagePackObject? prohibitReferenceType = null
		)
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var serializer = MessagePackSerializer.Create<NilImplicationTestTarget>( new SerializationContext() );
				var target =
					new NilImplicationTestTarget()
					{
						MemberDefault = 0,
						NullButValueType = 1,
						NullAndNullableValueType = 2,
						NullAndReferenceType = "3",
						ProhibitReferenceType = "4"
					};

				if ( adjuster != null )
				{
					adjuster( target );
				}

				packing(
					packer,
					memberDefault ?? target.MemberDefault,
					nullButValueType ?? target.NullButValueType,
					nullAndNullableValueType ?? ( target.NullAndNullableValueType == null ? MessagePackObject.Nil : target.NullAndNullableValueType.Value ),
					nullAndReferenceType ?? target.NullAndReferenceType,
					prohibitReferenceType ?? target.ProhibitReferenceType
				);

				buffer.Position = 0;

				var result = serializer.Unpack( buffer );
				Assert.That( result.MemberDefault, Is.EqualTo( target.MemberDefault ) );
				Assert.That( result.NullButValueType, Is.EqualTo( target.NullButValueType ) );
				Assert.That( result.NullAndNullableValueType, Is.EqualTo( target.NullAndNullableValueType ) );
				Assert.That( result.NullAndReferenceType, Is.EqualTo( target.NullAndReferenceType ) );
				Assert.That( result.ProhibitReferenceType, Is.EqualTo( target.ProhibitReferenceType ) );

				var expectedBytes = buffer.ToArray();
				using ( var actual = new MemoryStream() )
				{
					serializer.Pack( actual, target );
				}
			}
		}

		[Test]
		public void TestMemberDefault_Array_NilToDefault()
		{
			TestCore(
				target =>
				{
					target.MemberDefault = -1;
				},
				PackValuesAsArray,
				memberDefault: MessagePackObject.Nil
			);
		}

		[Test]
		public void TestMemberDefault_Map_NilToDefault()
		{
			TestCore(
				target =>
				{
					target.MemberDefault = -1;
				},
				PackValuesAsMap,
				memberDefault: MessagePackObject.Nil
			);
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestNullButValueType_Array_Fail()
		{
			TestCore(
				null,
				PackValuesAsArray,
				nullButValueType: MessagePackObject.Nil
			);
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestNullButValueType_Map_Fail()
		{
			TestCore(
				null,
				PackValuesAsMap,
				nullButValueType: MessagePackObject.Nil
			);
		}

		[Test]
		public void TestNullAndNullableValueType_Array_Null()
		{
			TestCore(
				target =>
				{
					target.NullAndNullableValueType = null;
				},
				PackValuesAsArray
			);
		}

		[Test]
		public void TestNullAndNullableValueType_Map_Null()
		{
			TestCore(
				target =>
				{
					target.NullAndNullableValueType = null;
				},
				PackValuesAsMap
			);
		}

		[Test]
		public void TestNullAndReferenceType_Array_Null()
		{
			TestCore(
				target =>
				{
					target.NullAndReferenceType = null;
				},
				PackValuesAsArray
			);
		}

		[Test]
		public void TestNullAndReferenceType_Map_Null()
		{
			TestCore(
				target =>
				{
					target.NullAndReferenceType = null;
				},
				PackValuesAsMap
			);
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestProhibitReferenceType_Array_Pack_Fail()
		{
			TestCore(
				target =>
				{
					target.NullAndReferenceType = null;
				},
				PackValuesAsArray
			);
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestProhibitReferenceType_Map_Pack_Fail()
		{
			TestCore(
				target =>
				{
					target.ProhibitReferenceType = null;
				},
				PackValuesAsMap
			);
		}


		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestProhibitReferenceType_Array_Unpack_Fail()
		{
			TestCore(
				null,
				PackValuesAsArray,
				prohibitReferenceType: MessagePackObject.Nil
			);
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestProhibitReferenceType_Map_Unpack_Fail()
		{
			TestCore(
				null,
				PackValuesAsMap,
				prohibitReferenceType: MessagePackObject.Nil
			);
		}
	}
}
