 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.Linq;
using System.Runtime.Serialization;
#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
using MsgPack.Serialization.EmittingSerializers;
#endif // !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
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
	public class CodeDomBasedNilImplicationTest
	{
		private SerializationContext CreateSerializationContext()
		{
			return new SerializationContext { EmitterFlavor = EmitterFlavor.CodeDomBased };
		}

		private MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return MessagePackSerializer.CreateInternal<T>( context, PolymorphismSchema.Default );
		}
#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID

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
			SerializerDebugging.DeletePastTemporaries();
			//SerializerDebugging.TraceEnabled = true;
			//SerializerDebugging.DumpEnabled = true;
			if ( SerializerDebugging.TraceEnabled )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
			}

			SerializerDebugging.OnTheFlyCodeDomEnabled = true;
			SerializerDebugging.AddRuntimeAssembly( this.GetType().Assembly.Location );
			if ( this.GetType().Assembly != typeof( NilImplicationTestTargetForValueTypeMemberDefault ).Assembly )
			{
				SerializerDebugging.AddRuntimeAssembly( typeof( NilImplicationTestTargetForValueTypeMemberDefault ).Assembly.Location );
			}
		}

		[TearDown]
		public void TearDown()
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				try
				{
					SerializerDebugging.Dump();
				}
				finally
				{
					DefaultSerializationMethodGeneratorManager.Refresh();
				}
			}

			SerializerDebugging.Reset();
			SerializerDebugging.OnTheFlyCodeDomEnabled = false;
		}
#endif // !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID

		// ------ Creation ------

		[Test]
		public void TestCreation_ValueType_OnlyNullIsInvalid()
		{
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForValueTypeMemberDefault>( this.CreateSerializationContext() ) );
			Assert.Throws<SerializationException>(
				() => CreateTarget<NilImplicationTestTargetForValueTypeNull>( this.CreateSerializationContext() )
			);
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForValueTypeProhibit>( this.CreateSerializationContext() ) );
		}

		[Test]
		public void TestCreation_ReferenceType_AllOk()
		{
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForReferenceType>( this.CreateSerializationContext() ) );
		}

		[Test]
		public void TestCreation_NullableValueType_AllOk()
		{
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForNullableValueType>( this.CreateSerializationContext() ) );
		}

		[Test]
		public void TestCreation_ReadOnlyCollectionProperty_OnlyNullIsInvalid()
		{
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForReadOnlyCollectionPropertyMemberDefault>( this.CreateSerializationContext() ) );
			Assert.Throws<SerializationException>(
				() => CreateTarget<NilImplicationTestTargetForReadOnlyCollectionPropertyNull>( this.CreateSerializationContext() )
			);
			Assert.NotNull( CreateTarget<NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit>( this.CreateSerializationContext() ) );
		}

		// ------ Packing ------

		private void TestPackFail<T, TException>(
			SerializationMethod method,
			Func<T> inputFactory
		)
			where T : new()
			where TException : Exception
		{
			var context = this.CreateSerializationContext();
			context.SerializationMethod = method;
			var seraizlier = CreateTarget<T>( context );

			using ( var buffer = new MemoryStream() )
			{
				Assert.Throws<TException>( () => seraizlier.Pack( buffer, inputFactory == null ? new T() : inputFactory() ) );
			}
		}

		// Packing null ValueType is not possible.

		[Test]
		public void TestPack_ReadOnlyCollectionProperty_Prohibit_Fail_Array()
		{
			TestPackFail<NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit, SerializationException>(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit { Prohibit = null }
			);
		}

		[Test]
		public void TestPack_ReadOnlyCollectionProperty_Prohibit_Fail_Map()
		{
			TestPackFail<NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit, SerializationException>(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit { Prohibit = null }
			);
		}

		[Test]
		public void TestPack_NullableValueType_ProhibitWillFail_Array()
		{
			TestPackFail<NilImplicationTestTargetForNullableValueType, SerializationException>(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForNullableValueType { Prohibit = null }
			);
		}

		[Test]
		public void TestPack_NullableValueType_ProhibitWillFail_Map()
		{
			TestPackFail<NilImplicationTestTargetForNullableValueType, SerializationException>(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForNullableValueType { Prohibit = null }
			);
		}

		[Test]
		public void TestPack_ReferenceType_ProhibitWillFail_Array()
		{
			TestPackFail<NilImplicationTestTargetForReferenceType, SerializationException>(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForReferenceType { Prohibit = null }
			);
		}

		[Test]
		public void TestPack_ReferenceType_ProhibitWillFail_Map()
		{
			TestPackFail<NilImplicationTestTargetForReferenceType, SerializationException>(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForReferenceType { Prohibit = null }
			);
		}

		// ------ Unpacking ------


		private void TestUnpackSuccess<T>(
			SerializationMethod method,
			Action<T, T> assertion
		)
			where T : new()
		{
			this.TestUnpackSuccess( method, () => new T(), null, assertion );
		}

		private void TestUnpackSuccess<T>(
			SerializationMethod method,
			Func<T> inputFactory,
			Action<T, T> assertion
		)
			where T : new()
		{
			this.TestUnpackSuccess( method, inputFactory, null, assertion );
		}

		private void TestUnpackSuccess<T>(
			SerializationMethod method,
			Func<byte[]> packedStreamFactory,
			Action<T, T> assertion
		)
			where T : new()
		{
			this.TestUnpackSuccess( method, () => new T(), packedStreamFactory, assertion );
		}

		private void TestUnpackSuccess<T>(
			SerializationMethod method,
			Func<T> inputFactory,
			Func<byte[]> packedStreamFactory,
			Action<T, T> assertion
		)
			where T : new()
		{
			var context = this.CreateSerializationContext();
			context.SerializationMethod = method;
			var seraizlier = CreateTarget<T>( context );

			using ( var buffer = new MemoryStream() )
			{
				if ( packedStreamFactory != null )
				{
					var bytes = packedStreamFactory();
					buffer.Write( bytes, 0, bytes.Length );
				}
				else
				{
					seraizlier.Pack( buffer, inputFactory == null ? new T() : inputFactory() );
				}

				buffer.Position = 0;
				assertion( seraizlier.Unpack( buffer ), new T() );
			}
		}

		private void TestUnpackFail<T, TException>(
			SerializationMethod method,
			Func<byte[]> packedStreamFactory
		)
			where T : new()
			where TException : Exception
		{
			this.TestUnpackingFail<T, TException>( method, null, packedStreamFactory );
		}

		private void TestUnpackingFail<T, TException>(
			SerializationMethod method,
			Func<T> inputFactory,
			Func<byte[]> packedStreamFactory
		)
			where T : new()
			where TException : Exception
		{
			var context = this.CreateSerializationContext();
			context.SerializationMethod = method;
			var seraizlier = CreateTarget<T>( context );

			using ( var buffer = new MemoryStream() )
			{
				if ( packedStreamFactory != null )
				{
					var bytes = packedStreamFactory();
					buffer.Write( bytes, 0, bytes.Length );
				}
				else
				{
					seraizlier.Pack( buffer, inputFactory == null ? new T() : inputFactory() );
				}

				buffer.Position = 0;
				Assert.Throws<TException>( () => seraizlier.Unpack( buffer ) );
			}
		}

		[Test]
		public void TestUnpack_ValueType_MemberDefault_Preserved_Array()
		{
			TestUnpackSuccess<NilImplicationTestTargetForValueTypeMemberDefault>(
				SerializationMethod.Array,
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ValueType_MemberDefault_Preserved_Map()
		{
			TestUnpackSuccess<NilImplicationTestTargetForValueTypeMemberDefault>(
				SerializationMethod.Map,
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ValueType_Prohibit_Fail_Array()
		{
			TestUnpackFail<NilImplicationTestTargetForValueTypeProhibit, SerializationException>(
				SerializationMethod.Array,
				() => new byte[] { 0x91, 0xC0 }
			);
		}

		[Test]
		public void TestUnpack_ValueType_Prohibit_Fail_Map()
		{
			TestUnpackFail<NilImplicationTestTargetForValueTypeProhibit, SerializationException>(
				SerializationMethod.Map,
				() => new byte[] { 0x81, ( byte )( 0xA0 + "Prohibit".Length ) }.Concat( "Prohibit".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } ).ToArray()
			);
		}

		[Test]
		public void TestUnpack_ReadOnlyCollectionProperty_MemberDefault_Preserved_Array()
		{
			TestUnpackSuccess<NilImplicationTestTargetForReadOnlyCollectionPropertyMemberDefault>(
				SerializationMethod.Array,
				() => new byte[] { 0x91, 0xC0 },
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ReadOnlyCollectionProperty_MemberDefault_Preserved_Map()
		{
			TestUnpackSuccess<NilImplicationTestTargetForReadOnlyCollectionPropertyMemberDefault>(
				SerializationMethod.Array,
				() => new byte[] { 0x81, ( byte )( 0xA0 + ( "MemberDefault".Length ) ) }.Concat( "MemberDefault".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } ).ToArray(),
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ReadOnlyCollectionProperty_Prohibit_Fail_Array()
		{
			TestUnpackFail<NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit, SerializationException>(
				SerializationMethod.Array,
				() => new byte[] { 0x91, 0xC0 }
			);
		}

		[Test]
		public void TestUnpack_ReadOnlyCollectionProperty_Prohibit_Fail_Map()
		{
			TestUnpackFail<NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit, SerializationException>(
				SerializationMethod.Array,
				() => new byte[] { 0x81, ( byte )( 0xA0 + ( "Prohibit".Length ) ) }.Concat( "Prohibit".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } ).ToArray()
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_MemberDefaultWillBePreserved_Array()
		{
			TestUnpackSuccess(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForNullableValueType { MemberDefault = null },
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_MemberDefaultWillBePreserved_Map()
		{
			TestUnpackSuccess(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForNullableValueType { MemberDefault = null },
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_NullWillBeNull_Array()
		{
			TestUnpackSuccess(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForNullableValueType { Null = null },
				( actual, @default ) => Assert.That( actual.Null, Is.Null.And.Not.EqualTo( @default.Null ) )
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_NullWillBeNull_Map()
		{
			TestUnpackSuccess(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForNullableValueType { Null = null },
				( actual, @default ) => Assert.That( actual.Null, Is.Null.And.Not.EqualTo( @default.Null ) )
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_ProhibitWillFail_Array()
		{
			TestUnpackFail<NilImplicationTestTargetForNullableValueType, SerializationException>(
				SerializationMethod.Array,
				() => new byte[] { 0x93, 0xC0, 0xC0, 0xC0 }
			);
		}

		[Test]
		public void TestUnpack_NullableValueType_ProhibitWillFail_Map()
		{
			TestUnpackFail<NilImplicationTestTargetForNullableValueType, SerializationException>(
				SerializationMethod.Map,
				() =>
					new byte[] { 0x83 }
					.Concat( new[] { ( byte )( 0xA0 + ( "MemberDefault".Length ) ) } )
					.Concat( "MemberDefault".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.Concat( new[] { ( byte )( 0xA0 + ( "Null".Length ) ) } )
					.Concat( "Null".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.Concat( new[] { ( byte )( 0xA0 + ( "Prohibit".Length ) ) } )
					.Concat( "Prohibit".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.ToArray()
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_MemberDefaultWillBePreserved_Array()
		{
			TestUnpackSuccess(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForReferenceType { MemberDefault = null },
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_MemberDefaultWillBePreserved_Map()
		{
			TestUnpackSuccess(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForReferenceType { MemberDefault = null },
				( actual, @default ) => Assert.That( actual.MemberDefault, Is.EqualTo( @default.MemberDefault ) )
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_NullWillBeNull_Array()
		{
			TestUnpackSuccess(
				SerializationMethod.Array,
				() => new NilImplicationTestTargetForReferenceType { Null = null },
				( actual, @default ) => Assert.That( actual.Null, Is.Null.And.Not.EqualTo( @default.Null ) )
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_NullWillBeNull_Map()
		{
			TestUnpackSuccess(
				SerializationMethod.Map,
				() => new NilImplicationTestTargetForReferenceType { Null = null },
				( actual, @default ) => Assert.That( actual.Null, Is.Null.And.Not.EqualTo( @default.Null ) )
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_ProhibitWillFail_Array()
		{
			TestUnpackFail<NilImplicationTestTargetForReferenceType, SerializationException>(
				SerializationMethod.Array,
				() => new byte[] { 0x93, 0xC0, 0xC0, 0xC0 }
			);
		}

		[Test]
		public void TestUnpack_ReferenceType_ProhibitWillFail_Map()
		{
			TestUnpackFail<NilImplicationTestTargetForReferenceType, SerializationException>(
				SerializationMethod.Map,
				() =>
					new byte[] { 0x83 }
					.Concat( new[] { ( byte )( 0xA0 + ( "MemberDefault".Length ) ) } )
					.Concat( "MemberDefault".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.Concat( new[] { ( byte )( 0xA0 + ( "Null".Length ) ) } )
					.Concat( "Null".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.Concat( new[] { ( byte )( 0xA0 + ( "Prohibit".Length ) ) } )
					.Concat( "Prohibit".ToCharArray().Select( c => ( byte )c ) ).Concat( new byte[] { 0xC0 } )
					.ToArray()
			);
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

				var context = this.CreateSerializationContext();
				context.SerializationMethod = SerializationMethod.Map;
				var target = CreateTarget<ComplexTypeWithTwoMember>( context );
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
				const string valueOfValue1 = "123";
				const string valueOfValue2 = "234";
				const string valueOfValue3 = "345";
				using ( var packer = Packer.Create( buffer, false ) )
				{
					packer.PackMapHeader( 3 );
					packer.PackString( "Value1" );
					packer.PackString( valueOfValue1 );
					packer.PackString( "Value2" );
					packer.PackString( valueOfValue2 );
					packer.PackString( "Value3" );
					packer.PackString( valueOfValue3 );
				}

				buffer.Position = 0;

				var context = this.CreateSerializationContext();
				context.SerializationMethod = SerializationMethod.Map;
				var target = CreateTarget<ComplexTypeWithTwoMember>( context );
				var result = target.Unpack( buffer );

				Assert.That( result.Value1, Is.EqualTo( valueOfValue1 ) );
				Assert.That( result.Value2, Is.EqualTo( valueOfValue2 ) );
			}
		}
	}
}	
