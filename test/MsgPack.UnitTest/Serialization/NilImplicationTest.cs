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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
#if !NETFX_CORE
using MsgPack.Serialization.EmittingSerializers;
#endif
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
	public abstract class NilImplicationTest
	{
#if !NETFX_CORE
		private static bool _traceOn = false;

#if MSTEST
		[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitialize]
		public void Initialize( Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestContext c )
		{
			this.SetUp();
		}
#else
		[SetUp]
#endif
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
#endif

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


		private static void PackValuesAsArray(
			Packer packer,
			List<int> memberDefault,
			List<int> @null,
			List<int> prohibit
		)
		{
			packer.PackArrayHeader( 3 );
			packer.Pack( memberDefault == null ? default( int[] ) : memberDefault.ToArray() );
			packer.Pack( @null == null ? default( int[] ) : @null.ToArray() );
			packer.Pack( prohibit == null ? default( int[] ) : prohibit.ToArray() );
		}

		private static void PackValuesAsMap(
			Packer packer,
			List<int> memberDefault,
			List<int> @null,
			List<int> prohibit
		)
		{
			packer.PackMapHeader( 3 );
			packer.PackString( "MemberDefault" );
			packer.Pack( memberDefault == null ? default( int[] ) : memberDefault.ToArray() );
			packer.PackString( "Null" );
			packer.Pack( @null == null ? default( int[] ) : @null.ToArray() );
			packer.PackString( "Prohibit" );
			packer.Pack( prohibit == null ? default( int[] ) : prohibit.ToArray() );
		}

		private static void TestNonCollectionCore(
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
				var target = new NilImplicationTestTarget();

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


		private void TestCollectionCore(
			Action<NilImplicationCollectionTestTarget> adjuster,
			Action<Packer, List<int>, List<int>, List<int>> packing,
			Func<List<int>> memberDefault = null,
			Func<List<int>> @null = null,
			Func<List<int>> prohibit = null
		)
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var serializer = MessagePackSerializer.Create<NilImplicationCollectionTestTarget>( this.CreateSerializationContext() );
				var target = new NilImplicationCollectionTestTarget();

				if ( adjuster != null )
				{
					adjuster( target );
				}

				packing(
					packer,
					memberDefault != null ? memberDefault() : target.MemberDefault,
					@null != null ? @null() : target.Null,
					prohibit != null ? prohibit() : target.Prohibit
				);

				buffer.Position = 0;

				var result = serializer.Unpack( buffer );
				Assert.That( result.MemberDefault, Is.EqualTo( target.MemberDefault ) );
				Assert.That( result.Null, Is.EqualTo( target.Null ) );
				Assert.That( result.Prohibit, Is.EqualTo( target.Prohibit ) );

				var expectedBytes = buffer.ToArray();
				using ( var actual = new MemoryStream() )
				{
					serializer.Pack( actual, target );
				}
			}
		}

		protected abstract SerializationContext CreateSerializationContext();
				
		[Test]
		public void TestMemberDefault_Array_NilToDefault()
		{
			TestNonCollectionCore(
				null,
				PackValuesAsArray,
				memberDefault: MessagePackObject.Nil
			);
		}

		[Test]
		public void TestMemberDefault_Map_NilToDefault()
		{
			TestNonCollectionCore(
				null,
				PackValuesAsMap,
				memberDefault: MessagePackObject.Nil
			);
		}

		[Test]
		public void TestNullButValueType_Array_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					null,
					PackValuesAsArray,
					nullButValueType: MessagePackObject.Nil
				)
			);
		}

		[Test]
		public void TestNullButValueType_Map_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					null,
					PackValuesAsMap,
					nullButValueType: MessagePackObject.Nil
				)
			);
		}

		[Test]
		public void TestNullAndNullableValueType_Array_Null()
		{
			TestNonCollectionCore(
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
			TestNonCollectionCore(
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
			TestNonCollectionCore(
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
			TestNonCollectionCore(
				target =>
				{
					target.NullAndReferenceType = null;
				},
				PackValuesAsMap
			);
		}

		[Test]
		public void TestProhibitReferenceType_Array_Pack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					target =>
					{
						target.ProhibitReferenceType = null;
					},
					PackValuesAsArray
				)
			);
		}

		[Test]
		public void TestProhibitReferenceType_Map_Pack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					target =>
					{
						target.ProhibitReferenceType = null;
					},
					PackValuesAsMap
				)
			);
		}


		[Test]
		public void TestProhibitReferenceType_Array_Unpack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					null,
					PackValuesAsArray,
					prohibitReferenceType: MessagePackObject.Nil
				)
			);
		}

		[Test]
		public void TestProhibitReferenceType_Map_Unpack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestNonCollectionCore(
					null,
					PackValuesAsMap,
					prohibitReferenceType: MessagePackObject.Nil
				)
			);
		}


		[Test]
		public void TestMemberDefaultForCollection_Array_NilToDefault()
		{
			TestCollectionCore(
				null,
				PackValuesAsArray,
				memberDefault: () => null,
				@null: () => new List<int>(), // Prevent the value to be doubled because packer pack non-null value and unpacker appends unpacked values.
				prohibit: () => new List<int>() // Prevent the value to be doubled because packer pack non-null value and unpacker appends unpacked values.
			);
		}

		[Test]
		public void TestMemberDefaultForCollection_Map_NilToDefault()
		{
			TestCollectionCore(
				null,
				PackValuesAsMap,
				memberDefault: () => null,
				@null: () => new List<int>(), // Prevent the value to be doubled because packer pack non-null value and unpacker appends unpacked values.
				prohibit: () => new List<int>() // Prevent the value to be doubled because packer pack non-null value and unpacker appends unpacked values.
			);
		}

		[Test]
		public void TestNullForCollection_Array_Null()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					target =>
					{
						target.Null = null;
					},
					PackValuesAsArray
				)
			);
		}

		[Test]
		public void TestNullForCollection_Map_Null()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					target =>
					{
						target.Null = null;
					},
					PackValuesAsMap
				)
			);
		}

		[Test]
		public void TestProhibitForCollection_Array_Pack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					target =>
					{
						target.Prohibit = null;
					},
					PackValuesAsArray
				)
			);
		}

		[Test]
		public void TestProhibitForCollection_Map_Pack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					target =>
					{
						target.Prohibit = null;
					},
					PackValuesAsMap
				)
			);
		}


		[Test]
		public void TestProhibitForCollection_Array_Unpack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					null,
					PackValuesAsArray,
					prohibit: () => null
				)
			);
		}

		[Test]
		public void TestProhibitForCollection_Map_Unpack_Fail()
		{
			Assert.Throws<SerializationException>(
				() =>
				TestCollectionCore(
					null,
					PackValuesAsMap,
					prohibit: () => null
				)
			);
		}

		// FIXME: Array and element count less/more expected. Map and missing
		[Test]
		public void TestElelementMissingInTheFirstPlace_Array_Fail()
		{
			using ( var buffer = new MemoryStream() )
			{
				using ( var packer = Packer.Create( buffer, false ) )
				{
					packer.PackArrayHeader( 1 );
					packer.PackString( "123" );
				}

				buffer.Position = 0;

				var target = MessagePackSerializer.Create<ComplexTypeWithTwoMember>( new SerializationContext() { SerializationMethod = SerializationMethod.Array } );
				Assert.Throws<SerializationException>( () => target.Unpack( buffer ) );
			}
		}

		[Test]
		public void TestElelementTooManyInTheFirstPlace_Array_Fail()
		{
			using ( var buffer = new MemoryStream() )
			{
				using ( var packer = Packer.Create( buffer, false ) )
				{
					packer.PackArrayHeader( 3 );
					packer.PackString( "123" );
					packer.PackString( "234" );
					packer.PackString( "345" );
				}

				buffer.Position = 0;

				var target = MessagePackSerializer.Create<ComplexTypeWithTwoMember>( new SerializationContext() { SerializationMethod = SerializationMethod.Array } );
				Assert.Throws<SerializationException>( () =>target.Unpack( buffer ) );
			}
		}

		[Test]
		public void TestElelementMissingInTheFirstPlace_Map_MissingMembersAreSkipped()
		{
			using ( var buffer = new MemoryStream() )
			{
				var valueOfValue1 = "123";
				using ( var packer = Packer.Create( buffer, false ) )
				{
					packer.PackMapHeader( 1 );
					packer.PackString( "Value1" );
					packer.PackString( valueOfValue1 );
				}

				buffer.Position = 0;

				var target = MessagePackSerializer.Create<ComplexTypeWithTwoMember>( new SerializationContext() { SerializationMethod = SerializationMethod.Map } );
				var result = target.Unpack( buffer );

				Assert.That( result.Value1, Is.EqualTo( valueOfValue1 ) );
				Assert.That( result.Value2, Is.EqualTo( new ComplexTypeWithTwoMember().Value2 ) );
			}
		}

		[Test]
		public void TestElelementTooManyInTheFirstPlace_Map_ExtrasAreIgnored()
		{
			using ( var buffer = new MemoryStream() )
			{
				var valueOfValue1 = "123";
				var valueOfValue2 = "234";
				var valueOfValue3 = "345";
				using ( var packer = Packer.Create( buffer, false ) )
				{
					packer.PackMapHeader( 1 );
					packer.PackString( "Value1" );
					packer.PackString( valueOfValue1 );
					packer.PackString( "Value2" );
					packer.PackString( valueOfValue2 );
					packer.PackString( "Value3" );
					packer.PackString( valueOfValue3 );
				}

				buffer.Position = 0;

				var target = MessagePackSerializer.Create<ComplexTypeWithTwoMember>( new SerializationContext() { SerializationMethod = SerializationMethod.Map } );
				var result = target.Unpack( buffer );

				Assert.That( result.Value1, Is.EqualTo( valueOfValue1 ) );
				Assert.That( result.Value2, Is.EqualTo( valueOfValue2 ) );
			}
		}
	}

	[TestFixture]
	public class NilImplicationOnFieldBasedEmittionFlavorTest : NilImplicationTest
	{
		protected override SerializationContext CreateSerializationContext()
		{
			return new SerializationContext() { EmitterFlavor = EmitterFlavor.FieldBased };
		}
	}

	[TestFixture]
	public class NilImplicationOnContextBasedEmittionFlavorTest : NilImplicationTest
	{
		protected override SerializationContext CreateSerializationContext()
		{
			return new SerializationContext() { EmitterFlavor = EmitterFlavor.ContextBased };
		}
	}

	[TestFixture]
	public class NilImplicationOnExpressionFlavorTest : NilImplicationTest
	{
		protected override SerializationContext CreateSerializationContext()
		{
			return new SerializationContext() { EmitterFlavor = EmitterFlavor.ExpressionBased };
		}
	}
}