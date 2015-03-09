

 
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

#pragma warning disable 3003
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !NETFX_35 && !WINDOWS_PHONE
using System.Numerics;
#endif // !NETFX_35 && !WINDOWS_PHONE
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;
#if !NETFX_CORE && !WINDOWS_PHONE && !UNITY_IPHONE && !UNITY_ANDROID && !XAMIOS && !XAMDROID
using MsgPack.Serialization.CodeDomSerializers;
using MsgPack.Serialization.EmittingSerializers;
#endif // !NETFX_CORE && !WINDOWS_PHONE && !UNITY_IPHONE && !UNITY_ANDROID && !XAMIOS && !XAMDROID
#if !NETFX_35 && !UNITY_IPHONE && !UNITY_ANDROID && !XAMIOS && !XAMDROID
using MsgPack.Serialization.ExpressionSerializers;
#endif // !NETFX_35 && !UNITY_IPHONE && !UNITY_ANDROID && !XAMIOS && !XAMDROID
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using CategoryAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCategoryAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 60000 )]
	public class ArrayContextBasedAutoMessagePackSerializerTest
	{
		private SerializationContext GetSerializationContext()
		{
			return new SerializationContext { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.ContextBased };
		}

		private SerializationContext  NewSerializationContext( PackerCompatibilityOptions compatibilityOptions )
		{
			return new SerializationContext( compatibilityOptions ) { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.ContextBased };
		}

		private MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return context.GetSerializer<T>( context );
		}
		
		private bool CanDump
		{
			get { return false; }
		}

#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
		[SetUp]
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
			SerializerDebugging.AddRuntimeAssembly( typeof( AddOnlyCollection<> ).Assembly.Location );
			if( typeof( AddOnlyCollection<> ).Assembly != this.GetType().Assembly )
			{
				SerializerDebugging.AddRuntimeAssembly( this.GetType().Assembly.Location );
			}
		}

		[TearDown]
		public void TearDown()
		{
			if ( SerializerDebugging.DumpEnabled && this.CanDump )
			{
				try
				{
					SerializerDebugging.Dump();
				}
				catch ( NotSupportedException ex )
				{
					Console.Error.WriteLine( ex );
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

		private void DoKnownCollectionTest<T>( SerializationContext context )
			where T : new()
		{
			using ( var buffer = new MemoryStream() )
			{
				CreateTarget<T>( context ).Pack( buffer, new T() );
			}
		}

		[Test]
		public void TestUnpackTo()
		{
			var target = this.CreateTarget<Int32[]>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, new[] { 1, 2 } );
				buffer.Position = 0;
				int[] result = new int[ 2 ];
				using ( var unpacker = Unpacker.Create( buffer, false ) )
				{
					unpacker.Read();
					target.UnpackTo( unpacker, result );
					Assert.That( result, Is.EqualTo( new[] { 1, 2 } ) );
				}
			}
		}

		[Test]
		public void TestInt32()
		{
			TestCore( 1, stream => Unpacking.UnpackInt32( stream ), null );
		}

		[Test]
		public void TestInt64()
		{
			TestCore( Int32.MaxValue + 1L, stream => Unpacking.UnpackInt64( stream ), null );
		}

		[Test]
		public void TestString()
		{
			TestCore( "abc", stream => Unpacking.UnpackString( stream ), null );
		}

		[Test]
		public void TestDateTime()
		{
			TestCore(
				DateTime.UtcNow,
				stream => MessagePackConvert.ToDateTime( Unpacking.UnpackInt64( stream ) ),
				CompareDateTime
			);
		}

		[Test]
		public void TestDateTimeOffset()
		{
			TestCore(
				DateTimeOffset.UtcNow,
				stream => MessagePackConvert.ToDateTimeOffset( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => CompareDateTime( x.DateTime.ToUniversalTime(), y.DateTime.ToUniversalTime() )
			);
		}

		private static bool CompareDateTime( DateTime x, DateTime y )
		{
			return x.Date == y.Date && x.Hour == y.Hour && x.Minute == y.Minute && x.Second == y.Second && x.Millisecond == y.Millisecond;
		}

		[Test]
		public void TestUri()
		{
			TestCore( new Uri( "http://www.example.com" ), stream => new Uri( Unpacking.UnpackString( stream ) ), null );
		}

		[Test]
		public void TestComplexObject_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestComplexObjectCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexObject_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestComplexObjectCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		private void TestComplexObjectCore( SerializationContext context )
		{
			var target = new ComplexType() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.Points.Add( 123 );
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestComplexTypeWithoutAnyAttribute_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestComplexTypeWithoutAnyAttribute( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexTypeWithoutAnyAttribute_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestComplexTypeWithoutAnyAttribute( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		private void TestComplexTypeWithoutAnyAttribute( SerializationContext context )
		{
			var target = new ComplexTypeWithoutAnyAttribute() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestTypeWithMissingMessagePackMemberAttributeMember()
		{
			this.TestTypeWithMissingMessagePackMemberAttributeMemberCore( this.GetSerializationContext() );
		}

		private void TestTypeWithMissingMessagePackMemberAttributeMemberCore( SerializationContext context )
		{
			var target = new TypeWithMissingMessagePackMemberAttributeMember();
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestTypeWithInvalidMessagePackMemberAttributeMember()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<TypeWithInvalidMessagePackMemberAttributeMember>( GetSerializationContext() ) );
		}

		[Test]
		public void TestTypeWithDuplicatedMessagePackMemberAttributeMember()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<TypeWithDuplicatedMessagePackMemberAttributeMember>( GetSerializationContext() ) );
		}

		[Test]
		public void TestComplexObjectTypeWithDataContract_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestComplexObjectTypeWithDataContractCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexObjectTypeWithDataContract_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestComplexObjectTypeWithDataContractCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		private void TestComplexObjectTypeWithDataContractCore( SerializationContext context )
		{
			var target = new ComplexTypeWithDataContract() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
#if !NETFX_CORE && !SILVERLIGHT
			target.NonSerialized = new DefaultTraceListener();
#else
			target.NonSerialized = new Stopwatch();
#endif // !NETFX_CORE && !SILVERLIGHT
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestComplexTypeWithDataContractWithOrder_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestComplexTypeWithDataContractWithOrderCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexTypeWithDataContractWithOrder_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestComplexTypeWithDataContractWithOrderCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		private void TestComplexTypeWithDataContractWithOrderCore( SerializationContext context )
		{
			var target = new ComplexTypeWithDataContractWithOrder() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
#if !NETFX_CORE && !SILVERLIGHT
			target.NonSerialized = new DefaultTraceListener();
#else
			target.NonSerialized = new Stopwatch();
#endif
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestComplexObjectTypeWithNonSerialized_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestComplexObjectTypeWithNonSerializedCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexObjectTypeWithNonSerialized_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestComplexObjectTypeWithNonSerializedCore( this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
	}

		private void TestComplexObjectTypeWithNonSerializedCore( SerializationContext context )
		{
			var target = new ComplexTypeWithNonSerialized() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
#if !NETFX_CORE && !SILVERLIGHT
			target.NonSerialized = new DefaultTraceListener();
#endif
			TestCoreWithVerify( target, context );
		}

		[Test]
		public void TestDataMemberAttributeOrderWithOneBase()
		{
			var context = GetSerializationContext();
			var value = new ComplexTypeWithOneBaseOrder();
			var target = this.CreateTarget<ComplexTypeWithOneBaseOrder>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				Assert.That( unpacked.One, Is.EqualTo( value.One ) );
				Assert.That( unpacked.Two, Is.EqualTo( value.Two ) );
			}
		}

		[Test]
		public void TestDataMemberAttributeOrderWithOneBase_ProtoBufCompatible()
		{
			var context = GetSerializationContext();
			context.CompatibilityOptions.OneBoundDataMemberOrder = true;
			var value = new ComplexTypeWithOneBaseOrder();
			var target = this.CreateTarget<ComplexTypeWithOneBaseOrder>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				Assert.That( unpacked.One, Is.EqualTo( value.One ) );
				Assert.That( unpacked.Two, Is.EqualTo( value.Two ) );
			}
		}

		[Test]
		public void TestDataMemberAttributeOrderWithOneBaseDeserialize()
		{
			var context = GetSerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			var target = this.CreateTarget<ComplexTypeWithOneBaseOrder>( context );
			using ( var buffer = new MemoryStream() )
			{
				buffer.Write( new byte[] { 0x93, 0xff, 10, 20 } );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				Assert.That( unpacked.One, Is.EqualTo( 10 ) );
				Assert.That( unpacked.Two, Is.EqualTo( 20 ) );
			}
		}

		[Test]
		public void TestDataMemberAttributeOrderWithOneBaseDeserialize_ProtoBufCompatible()
		{
			var context = GetSerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			context.CompatibilityOptions.OneBoundDataMemberOrder = true;
			var target = this.CreateTarget<ComplexTypeWithOneBaseOrder>( context );
			using ( var buffer = new MemoryStream() )
			{
				buffer.Write( new byte[] { 0x92, 10, 20 } );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				Assert.That( unpacked.One, Is.EqualTo( 10 ) );
				Assert.That( unpacked.Two, Is.EqualTo( 20 ) );
			}
		}

		[Test]
		public void TestDataMemberAttributeOrderWithZeroBase_ProtoBufCompatible_Fail()
		{
			var context = GetSerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			context.CompatibilityOptions.OneBoundDataMemberOrder = true;
			Assert.Throws<NotSupportedException>(
				() => this.CreateTarget<ComplexTypeWithTwoMember>( context )
			);
		}

		[Test]
		public void TestDataMemberAttributeNamedProperties()
		{
			var context = GetSerializationContext();
			if ( context.SerializationMethod == SerializationMethod.Array )
			{
				// Nothing to test.
				return;
			}

			var value = new DataMemberAttributeNamedPropertyTestTarget() { Member = "A Member" };
			var target = this.CreateTarget<DataMemberAttributeNamedPropertyTestTarget>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				var asDictionary = Unpacking.UnpackDictionary( buffer );

				Assert.That( asDictionary[ "Alias" ] == value.Member );

				buffer.Position = 0;

				var unpacked = target.Unpack( buffer );
				Assert.That( unpacked.Member, Is.EqualTo( value.Member ) );
			}
		}

		[Test]
		public void TestEnum()
		{
			TestCore( DayOfWeek.Sunday, stream => ( DayOfWeek )Enum.Parse( typeof( DayOfWeek ), Unpacking.UnpackString( stream ) ), ( x, y ) => x == y );
		}

#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestNameValueCollection()
		{
			var target = new NameValueCollection();
			target.Add( String.Empty, "Empty-1" );
			target.Add( String.Empty, "Empty-2" );
			target.Add( "1", "1-1" );
			target.Add( "1", "1-2" );
			target.Add( "1", "1-3" );
			target.Add( "null", null ); // This value will not be packed.
			target.Add( "Empty", String.Empty );
			target.Add( "2", "2" );
			var serializer = this.CreateTarget<NameValueCollection>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, target );
				stream.Position = 0;
				NameValueCollection result = serializer.Unpack( stream );
				Assert.That( result.GetValues( String.Empty ), Is.EquivalentTo( new[] { "Empty-1", "Empty-2" } ) );
				Assert.That( result.GetValues( "1" ), Is.EquivalentTo( new[] { "1-1", "1-2", "1-3" } ) );
				Assert.That( result.GetValues( "null" ), Is.Null );
				Assert.That( result.GetValues( "Empty" ), Is.EquivalentTo( new string[] { String.Empty } ) );
				Assert.That( result.GetValues( "2" ), Is.EquivalentTo( new string[] { "2" } ) );
				// null only value is not packed.
				Assert.That( result.Count, Is.EqualTo( target.Count - 1 ) );
			}
		}

		[Test]
		public void TestNameValueCollection_NullKey()
		{
			var target = new NameValueCollection();
			target.Add( null, "null" );
			var serializer = this.CreateTarget<NameValueCollection>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				Assert.Throws<NotSupportedException>( () => serializer.Pack( stream, target ) );
			}
		}
#endif

		[Test]
		public void TestByteArrayContent()
		{
			var serializer = this.CreateTarget<byte[]>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[] { 1, 2, 3, 4 } );
				stream.Position = 0;
				Assert.That( Unpacking.UnpackBinary( stream ).ToArray(), Is.EqualTo( new byte[] { 1, 2, 3, 4 } ) );
			}
		}

		[Test]
		public void TestCharArrayContent()
		{
			var serializer = this.CreateTarget<char[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new char[] { 'a', 'b', 'c', 'd' } );
				stream.Position = 0;
				Assert.That( Unpacking.UnpackString( stream ), Is.EqualTo( "abcd" ) );
			}
		}

#if !NETFX_35
		[Test]
		public void TestTuple1()
		{
			TestTupleCore( new Tuple<int>( 1 ) );
		}

		[Test]
		public void TestTuple7()
		{
			TestTupleCore( new Tuple<int, string, int, string, int, string, int>( 1, "2", 3, "4", 5, "6", 7 ) );
		}

		[Test]
		public void TestTuple8()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<string>>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string>( "8" )
				)
			);
		}

		[Test]
		public void TestTuple14()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<
				string, int, string, int, string, int, string
				>
				>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string, int, string, int, string, int, string>(
						"8", 9, "10", 11, "12", 13, "14"
					)
				)
			);
		}

		[Test]
		public void TestTuple15()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<
				string, int, string, int, string, int, string,
				Tuple<int>
				>
				>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string, int, string, int, string, int, string, Tuple<int>>(
						"8", 9, "10", 11, "12", 13, "14",
						new Tuple<int>( 15 )
					)
				)
			);
		}

		private void TestTupleCore<T>( T expected )
			where T : IStructuralEquatable
		{
			var serializer = this.CreateTarget<T>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, expected );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( expected ) );
			}
		}
#endif // !NETFX_35

		[Test]
		public void TestEmptyBytes()
		{
			var serializer = this.CreateTarget<byte[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ), BitConverter.ToString( stream.ToArray() ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new byte[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyString()
		{
			var serializer = this.CreateTarget<string>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, String.Empty );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( String.Empty ) );
			}
		}

		[Test]
		public void TestEmptyIntArray()
		{
			var serializer = this.CreateTarget<int[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new int[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new int[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyKeyValuePairArray()
		{
			var serializer = this.CreateTarget<KeyValuePair<string, int>[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new KeyValuePair<string, int>[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new KeyValuePair<string, int>[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyMap()
		{
			var serializer = this.CreateTarget<Dictionary<string, int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new Dictionary<string, int>() );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new Dictionary<string, int>() ) );
			}
		}

		[Test]
		public void TestNullable()
		{
			var serializer = this.CreateTarget<int?>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, 1 );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( 1 ) );

				stream.Position = 0;
				serializer.Pack( stream, null );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( null ) );
			}
		}

		[Test]
		public void TestValueType_Success()
		{
			var serializer = this.CreateTarget<TestValueType>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new TestValueType() { StringField = "ABC", Int32ArrayField = new int[] { 1, 2, 3 }, DictionaryField = new Dictionary<int, int>() { { 1, 1 } } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.StringField, Is.EqualTo( value.StringField ) );
				Assert.That( result.Int32ArrayField, Is.EqualTo( value.Int32ArrayField ) );
				Assert.That( result.DictionaryField, Is.EqualTo( value.DictionaryField ) );
			}
		}

		[Test]
		public void TestHasInitOnlyField_Fail()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<HasInitOnlyField>( GetSerializationContext() ) );
		}

		[Test]
		public void TestHasInitOnlyFieldWithConstructor_Success()
		{
			var serializer = this.CreateTarget<HasInitOnlyFieldWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new HasInitOnlyFieldWithConstructor( "123" );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Field, Is.EqualTo( "123" ) );
			}
		}

		[Test]
		public void TestHasInitOnlyFieldWithConstructorMissing_Success()
		{
			var serializer = this.CreateTarget<HasInitOnlyFieldWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 } );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Field, Is.Null );
			}
		}

		[Test]
		public void TestHasInitOnlyFieldWithConstructorWithExtra_Success()
		{
			var serializer = this.CreateTarget<HasInitOnlyFieldWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				using ( var packer = Packer.Create( stream, false ) )
				{
					packer.PackArrayHeader( 2 );
					packer.Pack( "ABC" );
					packer.PackNull();
				}

				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Field, Is.EqualTo( "ABC" ) );
			}
		}

		[Test]
		public void TestHasGetOnlyProperty_Fail()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<HasGetOnlyProperty>( GetSerializationContext() ) );
		}

		[Test]
		public void TestHasGetOnlyPropertyWithConstructor_Success()
		{
			var serializer = this.CreateTarget<HasGetOnlyPropertyWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new HasGetOnlyPropertyWithConstructor( "123" );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Property, Is.EqualTo( "123" ) );
			}
		}

		[Test]
		public void TestHasGetOnlyPropertyWithConstructorMissing_Success()
		{
			var serializer = this.CreateTarget<HasGetOnlyPropertyWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 } );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Property, Is.Null );
			}
		}

		[Test]
		public void TestHasGetOnlyPropertyWithConstructorWithExtra_Success()
		{
			var serializer = this.CreateTarget<HasGetOnlyPropertyWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				using ( var packer = Packer.Create( stream, false ) )
				{
					packer.PackArrayHeader( 2 );
					packer.Pack( "ABC" );
					packer.PackNull();
				}

				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Property, Is.EqualTo( "ABC" ) );
			}
		}

		[Test]
		public void TestHasPrivateSetterPropertyWithConstructor_Success()
		{
			var serializer = this.CreateTarget<HasGetOnlyPropertyWithConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				using ( var packer = Packer.Create( stream, false ) )
				{
					packer.PackArrayHeader( 2 );
					packer.Pack( "ABC" );
					packer.PackNull();
				}

				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Property, Is.EqualTo( "ABC" ) );
			}
		}

		[Test]
		public void TestOnlyCollection_Success()
		{
			var serializer = this.CreateTarget<OnlyCollection>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new OnlyCollection();
				value.Collection.Add( 1 );
				value.Collection.Add( 2 );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Collection.ToArray(), Is.EqualTo( new [] { 1, 2 } ) );
			}
		}

		[Test]
		public void TestConstrutorDeserializationOnlyCollection_Fail()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<OnlyCollectionWithConstructor>( GetSerializationContext() ) );
		}

		[Test]
		public void TestConstrutorDeserializationWithAnotherNameConstrtor_DifferIsSetDefault()
		{
			var serializer = this.CreateTarget<WithAnotherNameConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new WithAnotherNameConstructor( 1, 2 );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ReadOnlySame, Is.EqualTo( 1 ) );
				Assert.That( result.ReadOnlyDiffer, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestConstrutorDeserializationWithAnotherTypeConstrtor_DifferIsSetDefault()
		{
			var serializer = this.CreateTarget<WithAnotherTypeConstructor>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new WithAnotherTypeConstructor( 1, 2 );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ReadOnlySame, Is.EqualTo( 1 ) );
				Assert.That( result.ReadOnlyDiffer, Is.EqualTo( "0" ) );
			}
		}

		[Test]
		public void TestConstrutorDeserializationWithAttribute_Preferred()
		{
			var serializer = this.CreateTarget<WithConstructorAttribute>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new WithConstructorAttribute( 1, false );
				Assert.That( value.IsAttributePreferred, Is.False );
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( 1 ) );
				Assert.That( result.IsAttributePreferred, Is.True );
			}
		}

		[Test]
		public void TestConstrutorDeserializationWithMultipleAttributes_Fail()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<WithMultipleConstructorAttributes>( GetSerializationContext() ) );
		}


		[Test]
		public void TestOptionalConstructorByte_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterByte>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( byte )2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorSByte_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterSByte>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( sbyte )-2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorInt16_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterInt16>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( short )-2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorUInt16_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterUInt16>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( ushort )2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorInt32_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterInt32>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( -2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorUInt32_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterUInt32>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( uint )2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorInt64_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterInt64>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( -2L ) );
			}
		}

		[Test]
		public void TestOptionalConstructorUInt64_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterUInt64>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( ( ulong )2L ) );
			}
		}

		[Test]
		public void TestOptionalConstructorSingle_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterSingle>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( 1.2f ) );
			}
		}

		[Test]
		public void TestOptionalConstructorDouble_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterDouble>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( 1.2 ) );
			}
		}

		[Test]
		public void TestOptionalConstructorDecimal_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterDecimal>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( 1.2m ) );
			}
		}

		[Test]
		public void TestOptionalConstructorBoolean_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterBoolean>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( true ) );
			}
		}

		[Test]
		public void TestOptionalConstructorChar_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterChar>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( 'A' ) );
			}
		}

		[Test]
		public void TestOptionalConstructorString_Success()
		{
			var serializer = this.CreateTarget<WithOptionalConstructorParameterString>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				stream.Write( new byte[]{ 0x90 }, 0, 1 );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Value, Is.EqualTo( "ABC" ) );
			}
		}

		[Test]
		public void TestCollection_Success()
		{
			var serializer = this.CreateTarget<Collection<int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new Collection<int>() { 1, 2, 3 };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EqualTo( new int[] { 1, 2, 3 } ) );
			}
		}

		[Test]
		public void TestIListValueType_Success()
		{
			var serializer = this.CreateTarget<ListValueType<int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new ListValueType<int>( 3 ) { 1, 2, 3 };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EqualTo( new int[] { 1, 2, 3 } ) );
			}
		}

		[Test]
		public void TestIDictionaryValueType_Success()
		{
			var serializer = this.CreateTarget<DictionaryValueType<int, int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new DictionaryValueType<int, int>( 3 ) { { 1, 1 }, { 2, 2 }, { 3, 3 } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EquivalentTo( Enumerable.Range( 1, 3 ).Select( i => new KeyValuePair<int, int>( i, i ) ).ToArray() ) );
			}
		}

		[Test]
		public void TestPackable_PackToMessageUsed()
		{
			var serializer = this.CreateTarget<JustPackable>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new JustPackable();
				value.Int32Field = 1;
				serializer.Pack( stream, value );
				Assert.That( stream.ToArray(), Is.EqualTo( new byte[] { 0x91, 0xA1, ( byte )'A' } ) );
				stream.Position = 0;
				Assert.Throws<SerializationException>( () => serializer.Unpack( stream ), "Round-trip should not be succeeded." );
			}
		}

		[Test]
		public void TestUnpackable_UnpackFromMessageUsed()
		{
			var serializer = this.CreateTarget<JustUnpackable>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new JustUnpackable();
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.Int32Field.ToString(), Is.EqualTo( JustUnpackable.Dummy ) );
			}
		}

		[Test]
		public void TestPackableUnpackable_PackToMessageAndUnpackFromMessageUsed()
		{
			var serializer = this.CreateTarget<PackableUnpackable>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new PackableUnpackable();
				value.Int32Field = 1;
				serializer.Pack( stream, value );
				Assert.That( stream.ToArray(), Is.EqualTo( new byte[] { 0x91, 0xA1, ( byte )'A' } ) );
				stream.Position = 0;
				serializer.Unpack( stream );
			}
		}

		[Test]
		public void TestBinary_DefaultContext()
		{
			var serializer = SerializationContext.Default.GetSerializer<byte[]>();

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[] { 1 } );
				Assert.That( stream.ToArray(), Is.EqualTo( new byte[] { MessagePackCode.MinimumFixedRaw + 1, 1 } ) );
			}
		}

		[Test]
		public void TestBinary_ContextWithPackerCompatilibyOptionsNone()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var serializer = CreateTarget<byte[]>( context );

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[] { 1 } );
				Assert.That( stream.ToArray(), Is.EqualTo( new byte[] { MessagePackCode.Bin8, 1, 1 } ) );
			}
		}
		[Test]
		public void TestExt_DefaultContext()
		{
			var context = NewSerializationContext( SerializationContext.Default.CompatibilityOptions.PackerCompatibilityOptions );
			context.Serializers.Register( new CustomDateTimeSerealizer() );
			var serializer = CreateTarget<DateTime>( context );

			using ( var stream = new MemoryStream() )
			{
				var date = DateTime.UtcNow;
				serializer.Pack( stream, date );
				stream.Position = 0;
				var unpacked = serializer.Unpack( stream );
				Assert.That( unpacked.ToString( "yyyyMMddHHmmssfff" ), Is.EqualTo( date.ToString( "yyyyMMddHHmmssfff" ) ) );
			}
		}

		[Test]
		public void TestExt_ContextWithPackerCompatilibyOptionsNone()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.Serializers.Register( new CustomDateTimeSerealizer() );
			context.CompatibilityOptions.PackerCompatibilityOptions = PackerCompatibilityOptions.None;
			var serializer = CreateTarget<DateTime>( context );

			using ( var stream = new MemoryStream() )
			{
				var date = DateTime.UtcNow;
				serializer.Pack( stream, date );
				stream.Position = 0;
				var unpacked = serializer.Unpack( stream );
				Assert.That( unpacked.ToString( "yyyyMMddHHmmssfff" ), Is.EqualTo( date.ToString( "yyyyMMddHHmmssfff" ) ) );
			}
		}

		[Test]
		public void TestAbstractTypes_KnownCollections_Default_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var serializer = CreateTarget<WithAbstractInt32Collection>( context );

			using ( var stream = new MemoryStream() )
			{
				var value = new WithAbstractInt32Collection() { Collection = new[] { 1, 2 } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var unpacked = serializer.Unpack( stream );
				Assert.That( unpacked.Collection, Is.Not.Null.And.InstanceOf<List<int>>() );
				Assert.That( unpacked.Collection[ 0 ], Is.EqualTo( 1 ) );
				Assert.That( unpacked.Collection[ 1 ], Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestAbstractTypes_KnownCollections_WithoutRegistration_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Unregister( typeof( IList<> ) );
			Assert.Throws<NotSupportedException>( () => DoKnownCollectionTest<WithAbstractInt32Collection>( context ) );
		}

		[Test]
		public void TestAbstractTypes_KnownCollections_ExplicitRegistration_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( Collection<> ) );
			var serializer = CreateTarget<WithAbstractInt32Collection>( context );

			using ( var stream = new MemoryStream() )
			{
				var value = new WithAbstractInt32Collection() { Collection = new[] { 1, 2 } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var unpacked = serializer.Unpack( stream );
				Assert.That( unpacked.Collection, Is.Not.Null.And.InstanceOf<Collection<int>>() );
				Assert.That( unpacked.Collection[ 0 ], Is.EqualTo( 1 ) );
				Assert.That( unpacked.Collection[ 1 ], Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestAbstractTypes_KnownCollections_ExplicitRegistrationForSpecific_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( IList<int> ), typeof( Collection<int> ) );
			var serializer1 = CreateTarget<WithAbstractInt32Collection>( context );

			using ( var stream = new MemoryStream() )
			{
				var value = new WithAbstractInt32Collection() { Collection = new[] { 1, 2 } };
				serializer1.Pack( stream, value );
				stream.Position = 0;
				var unpacked = serializer1.Unpack( stream );
				Assert.That( unpacked.Collection, Is.Not.Null.And.InstanceOf<Collection<int>>() );
				Assert.That( unpacked.Collection[ 0 ], Is.EqualTo( 1 ) );
				Assert.That( unpacked.Collection[ 1 ], Is.EqualTo( 2 ) );
			}

			// check other types are not affected
			var serializer2 = CreateTarget<WithAbstractStringCollection>( context );

			using ( var stream = new MemoryStream() )
			{
				var value = new WithAbstractStringCollection() { Collection = new[] { "1", "2" } };
				serializer2.Pack( stream, value );
				stream.Position = 0;
				var unpacked = serializer2.Unpack( stream );
				Assert.That( unpacked.Collection, Is.Not.Null.And.InstanceOf<List<string>>() );
				Assert.That( unpacked.Collection[ 0 ], Is.EqualTo( "1" ) );
				Assert.That( unpacked.Collection[ 1 ], Is.EqualTo( "2" ) );
			}
		}

		[Test]
		public void TestAbstractTypes_NotACollection_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			Assert.Throws<NotSupportedException>( () => DoKnownCollectionTest<WithAbstractNonCollection>( context ) );
		}

		// FIXME: init-only field, get-only property, Value type which implements IList<T> and has .ctor(int), Enumerator class which explicitly implements IEnumerator

		private void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer )
		{
			var safeComparer = comparer ?? EqualityComparer<T>.Default.Equals;
			var target = this.CreateTarget<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T intermediate = unpacking( buffer );
				Assert.That( safeComparer( intermediate, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, intermediate );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				Assert.That( safeComparer( unpacked, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, unpacked );
			}
		}

		private void TestCoreWithVerify<T>( T value, SerializationContext context )
			where T : IVerifiable
		{
			var target = this.CreateTarget<T>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( buffer );
			}
		}

		[Test]
		public void TestIssue25_Plain()
		{
			var hasEnumerable = new HasEnumerable { Numbers = new[] { 1, 2 } };
			var target = CreateTarget<HasEnumerable>( this.GetSerializationContext() );

			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, hasEnumerable );
				buffer.Position = 0;
				var result = target.Unpack( buffer );
				var resultNumbers = result.Numbers.ToArray();
				Assert.That( resultNumbers.Length, Is.EqualTo( 2 ) );
				Assert.That( resultNumbers[ 0 ], Is.EqualTo( 1 ) );
				Assert.That( resultNumbers[ 1 ], Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestIssue25_SelfComposite()
		{
			SerializationContext serializationContext =
				SerializationContext.Default;
			try
			{

				serializationContext.Serializers.Register( new PersonSerializer() );
				serializationContext.Serializers.Register( new ChildrenSerializer() );

				object[] array = new object[] { new Person { Name = "Joe" }, 3 };

				MessagePackSerializer<object[]> context =
					serializationContext.GetSerializer<object[]>();

				byte[] packed = context.PackSingleObject( array ); 
				object[] unpacked = context.UnpackSingleObject( packed );

				Assert.That( unpacked.Length, Is.EqualTo( 2 ) );
				Assert.That( ( ( MessagePackObject )unpacked[ 0 ] ).AsDictionary()[ "Name" ].AsString(), Is.EqualTo( "Joe" ) );
				Assert.That( ( ( MessagePackObject )unpacked[ 0 ] ).AsDictionary()[ "Children" ].IsNil );
				Assert.That( ( MessagePackObject )unpacked[ 1 ], Is.EqualTo( new MessagePackObject( 3 ) ) );
			}
			finally
			{
				SerializationContext.Default = new SerializationContext();
			}
		}

#region -- ReadOnly / Private Members --

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
#if !NETFX_CORE && !SILVERLIGHT
		private const string NonSerializedPublicField = "NonSerializedPublicField";
		private const string NonSerializedPublicReadOnlyField = "NonSerializedPublicReadOnlyField";
		private const string NonSerializedNonPublicField = "NonSerializedNonPublicField";
		private const string NonSerializedPublicFieldPlain = "NonSerializedPublicFieldPlain";
		private const string NonSerializedPublicReadOnlyFieldPlain = "NonSerializedPublicReadOnlyFieldPlain";
		private const string NonSerializedNonPublicFieldPlain = "NonSerializedNonPublicFieldPlain";
#endif // !NETFX_CORE && !SILVERLIGHT
		// ReSharper restore UnusedMember.Local

		[Test]
		public void TestNonPublicWritableMember_PlainOldCliClass()
		{
			var target = new PlainClass();
			target.CollectionReadOnlyProperty.Add( 10 );
			TestNonPublicWritableMemberCore( target, PublicProperty, PublicField, CollectionReadOnlyProperty );
		}

		[Test]
		public void TestNonPublicWritableMember_MessagePackMember()
		{
			var target = new AnnotatedClass();
			target.CollectionReadOnlyProperty.Add( 10 );
#if !NETFX_CORE && !SILVERLIGHT
			TestNonPublicWritableMemberCore( target, PublicProperty, NonPublicProperty, PublicField, NonPublicField, NonSerializedPublicField, NonSerializedNonPublicField, CollectionReadOnlyProperty );
#else
			TestNonPublicWritableMemberCore( target, PublicProperty, NonPublicProperty, PublicField, NonPublicField, CollectionReadOnlyProperty );
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[Test]
		public void TestNonPublicWritableMember_DataContract()
		{
			// includes issue33
			var target = new DataMamberClass();
			target.CollectionReadOnlyProperty.Add( 10 );
#if !NETFX_CORE && !SILVERLIGHT
			TestNonPublicWritableMemberCore( target, PublicProperty, NonPublicProperty, PublicField, NonPublicField, NonSerializedPublicField, NonSerializedNonPublicField, CollectionReadOnlyProperty );
#else
			TestNonPublicWritableMemberCore( target, PublicProperty, NonPublicProperty, PublicField, NonPublicField, CollectionReadOnlyProperty );
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		private void TestNonPublicWritableMemberCore<T>( T original, params string[] expectedMemberNames )
		{
			var serializer = CreateTarget<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, original );
				buffer.Position = 0;
				var actual = serializer.Unpack( buffer );

				foreach ( var memberName in expectedMemberNames )
				{
					Func<T, Object> getter = null;
#if !NETFX_CORE
					var property = typeof( T ).GetProperty( memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
#else
					var property = typeof( T ).GetRuntimeProperties().SingleOrDefault( p => p.Name == memberName );
#endif
					if ( property != null )
					{
						getter = obj => property.GetValue( obj, null );
					}
					else
					{
#if !NETFX_CORE
						var field =  typeof( T ).GetField( memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
#else
						var field = typeof( T ).GetRuntimeFields().SingleOrDefault( f => f.Name == memberName );
#endif
						if ( field == null )
						{
							Assert.Fail( memberName + " is not found." );
						}

						getter = obj => field.GetValue( obj );
					}

					Assert.That( getter( actual ), Is.EqualTo( getter( original ) ), typeof(T) + "." + memberName );
				}
			}
		}

#endregion -- ReadOnly / Private Members --

#region -- Exclusion --

		private void TestIgnoreCore<T>( Action<T> setter, Action<T, T> assertion )
			where T : new()
		{
			var target = GetSerializationContext().GetSerializer<T>();
			var obj = new T();
			setter( obj );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, obj );
				buffer.Position = 0;
				var actual = target.Unpack( buffer );
				assertion( obj, actual );
			}
		}

		private void TestIgnoreCore<T, TException>()
			where TException : Exception
		{
			Assert.Throws<TException>( () => GetSerializationContext().GetSerializer<T>() );
		}

		[Test]
		public void TestIgnore_Normal()
		{
			TestIgnoreCore<Excluded>( 
				target => { 
					target.IgnoredField = "ABC";
					target.IgnoredProperty = "ABC";
					target.NotIgnored = "ABC";
				},
				( expected, actual ) =>
				{
					Assert.That( actual.IgnoredField, Is.Null );
					Assert.That( actual.IgnoredProperty, Is.Null );
					Assert.That( actual.NotIgnored, Is.EqualTo( expected.NotIgnored ) );
				}
			);
		}

		[Test]
		public void TestIgnore_ExcludedOnly()
		{
			TestIgnoreCore<OnlyExcluded, SerializationException>();
		}

		[Test]
		public void TestIgnore_ExclusionAndInclusionMixed()
		{
			TestIgnoreCore<ExclusionAndInclusionMixed>( 
				target => { 
					target.IgnoredField = "ABC";
					target.IgnoredProperty = "ABC";
					target.NotMarked = "ABC";
					target.Marked = "ABC";
				},
				( expected, actual ) =>
				{
					Assert.That( actual.IgnoredField, Is.Null );
					Assert.That( actual.IgnoredProperty, Is.Null );
					Assert.That( actual.NotMarked, Is.Null );
					Assert.That( actual.Marked, Is.EqualTo( expected.Marked ) );
				}
			);
		}

		[Test]
		public void TestIgnore_ExclusionAndInclusionSimulatously()
		{
			TestIgnoreCore<ExclusionAndInclusionSimulatously, SerializationException>();
		}


		public class OnlyExcluded
		{
			[MessagePackIgnore]
			public string Ignored { get; set; }
		}

		public class Excluded
		{
			[MessagePackIgnore]
			public string IgnoredField;

			[MessagePackIgnore]
			public string IgnoredProperty { get; set; }

			public string NotIgnored { get; set; }
		}

		public class ExclusionAndInclusionMixed
		{
			[MessagePackIgnore]
			public string IgnoredField;

			[MessagePackIgnore]
			public string IgnoredProperty { get; set; }

			public string NotMarked { get; set; }

			[MessagePackMember( 0 )]
			public string Marked { get; set; }
		}

		public class ExclusionAndInclusionSimulatously
		{
			[MessagePackMember( 0 )]
			public string Marked { get; set; }

			[MessagePackIgnore]
			[MessagePackMember( 1 )]
			public string DoubleMarked { get; set; }
		}

#endregion -- Exclusion --
		public class HasInitOnlyField
		{
			public readonly string Field = "ABC";
		}

		public class HasInitOnlyFieldWithConstructor
		{
			public readonly string Field;

			public HasInitOnlyFieldWithConstructor( string field )
			{
				this.Field = field;
			}
		}

		public class HasGetOnlyProperty
		{
			public string Property { get { return "ABC"; } }
		}

		public class HasGetOnlyPropertyWithConstructor
		{
			private readonly string _property;
			public string Property { get { return this._property; } }

			public HasGetOnlyPropertyWithConstructor( string property )
			{
				this._property = property;
			}
		}

		public class HasPrivateSetterPropertyWithConstructor
		{
			public string Property { get; private set; }

			public HasPrivateSetterPropertyWithConstructor( string property )
			{
				this.Property = property;
			}
		}

		public class OnlyCollection
		{
			public readonly List<int> Collection = new List<int>();
		}

		public class OnlyCollectionWithConstructor
		{
			public readonly List<int> Collection;

			public OnlyCollectionWithConstructor( List<int> collection )
			{
				this.Collection = collection;
			}
		}

		public class WithAnotherNameConstructor
		{
			public readonly int ReadOnlySame;
			public readonly int ReadOnlyDiffer;

			public WithAnotherNameConstructor( int readonlysame, int the2 )
			{
				this.ReadOnlySame = readonlysame;
				this.ReadOnlyDiffer = the2;
			}
		}

		public class WithAnotherTypeConstructor
		{
			public readonly int ReadOnlySame;
			public readonly string ReadOnlyDiffer;

			public WithAnotherTypeConstructor( int readonlysame, int the2 )
			{
				this.ReadOnlySame = readonlysame;
				this.ReadOnlyDiffer = the2.ToString();
			}
		}

		public class WithConstructorAttribute
		{
			public readonly int Value;
			public readonly bool IsAttributePreferred;

			public WithConstructorAttribute( int value, bool isAttributePreferred )
			{
				this.Value = value;
				this.IsAttributePreferred = isAttributePreferred;
			}

			[MessagePackDeserializationConstructor]
			public WithConstructorAttribute( int value ) : this( value, true ) {}
		}

		public class WithMultipleConstructorAttributes
		{
			public readonly int Value;

			[MessagePackDeserializationConstructor]
			public WithMultipleConstructorAttributes( int value, string arg ) { }

			[MessagePackDeserializationConstructor]
			public WithMultipleConstructorAttributes( int value, bool arg ) { }
		}

#pragma warning disable 3001
		public class WithOptionalConstructorParameterByte
		{
			public readonly Byte Value;

			public WithOptionalConstructorParameterByte( Byte value = ( byte )2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterSByte
		{
			public readonly SByte Value;

			public WithOptionalConstructorParameterSByte( SByte value = ( sbyte )-2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterInt16
		{
			public readonly Int16 Value;

			public WithOptionalConstructorParameterInt16( Int16 value = ( short )-2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterUInt16
		{
			public readonly UInt16 Value;

			public WithOptionalConstructorParameterUInt16( UInt16 value = ( ushort )2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterInt32
		{
			public readonly Int32 Value;

			public WithOptionalConstructorParameterInt32( Int32 value = -2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterUInt32
		{
			public readonly UInt32 Value;

			public WithOptionalConstructorParameterUInt32( UInt32 value = ( uint )2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterInt64
		{
			public readonly Int64 Value;

			public WithOptionalConstructorParameterInt64( Int64 value = -2L )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterUInt64
		{
			public readonly UInt64 Value;

			public WithOptionalConstructorParameterUInt64( UInt64 value = ( ulong )2L )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterSingle
		{
			public readonly Single Value;

			public WithOptionalConstructorParameterSingle( Single value = 1.2f )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterDouble
		{
			public readonly Double Value;

			public WithOptionalConstructorParameterDouble( Double value = 1.2 )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterDecimal
		{
			public readonly Decimal Value;

			public WithOptionalConstructorParameterDecimal( Decimal value = 1.2m )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterBoolean
		{
			public readonly Boolean Value;

			public WithOptionalConstructorParameterBoolean( Boolean value = true )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterChar
		{
			public readonly Char Value;

			public WithOptionalConstructorParameterChar( Char value = 'A' )
			{
				this.Value = value;
			}
		}
		public class WithOptionalConstructorParameterString
		{
			public readonly String Value;

			public WithOptionalConstructorParameterString( String value = "ABC" )
			{
				this.Value = value;
			}
		}
#pragma warning restore 3001

		public class JustPackable : IPackable
		{
			public const string Dummy = "A";

			public int Int32Field { get; set; }

			public void PackToMessage( Packer packer, PackingOptions options )
			{
				packer.PackArrayHeader( 1 );
				packer.PackString( Dummy );
			}
		}

		public class JustUnpackable : IUnpackable
		{
			public const string Dummy = "1";

			public int Int32Field { get; set; }

			public void UnpackFromMessage( Unpacker unpacker )
			{
				var value = unpacker.UnpackSubtreeData();
				if ( value.IsArray )
				{
					Assert.That( value.AsList()[ 0 ] == 0, "{0} != \"[{1}]\"", value, 0 );
				}
				else if ( value.IsMap )
				{
					Assert.That( value.AsDictionary().First().Value == 0, "{0} != \"[{1}]\"", value, 0 );
				}
				else
				{
					Assert.Fail( "Unknown spec." );
				}

				this.Int32Field = Int32.Parse( Dummy );
			}
		}

		public class PackableUnpackable : IPackable, IUnpackable
		{
			public const string Dummy = "A";

			public int Int32Field { get; set; }

			public void PackToMessage( Packer packer, PackingOptions options )
			{
				packer.PackArrayHeader( 1 );
				packer.PackString( Dummy );
			}

			public void UnpackFromMessage( Unpacker unpacker )
			{
				Assert.That( unpacker.IsArrayHeader );
				var value = unpacker.UnpackSubtreeData();
				Assert.That( value.AsList()[ 0 ] == Dummy, "{0} != \"[{1}]\"", value, Dummy );
			}
		}

		public class CustomDateTimeSerealizer : MessagePackSerializer<DateTime>
		{
			private const byte _typeCodeForDateTimeForUs = 1;

			public CustomDateTimeSerealizer()
				: base( SerializationContext.Default ) {}

			protected internal override void PackToCore( Packer packer, DateTime objectTree )
			{
				byte[] data;
				if ( BitConverter.IsLittleEndian )
				{
					data = BitConverter.GetBytes( objectTree.ToUniversalTime().Ticks ).Reverse().ToArray();
				}
				else
				{
					data = BitConverter.GetBytes( objectTree.ToUniversalTime().Ticks );
				}

				packer.PackExtendedTypeValue( _typeCodeForDateTimeForUs, data );
			}

			protected internal override DateTime UnpackFromCore( Unpacker unpacker )
			{
				var ext = unpacker.LastReadData.AsMessagePackExtendedTypeObject();
				Assert.That( ext.TypeCode, Is.EqualTo( 1 ) );
				return new DateTime( BigEndianBinary.ToInt64( ext.Body, 0 ) ).ToUniversalTime();
			}
		}

		// Issue #25

		public class Person : IEnumerable<Person>
		{
			public string Name { get; set; }

			internal IEnumerable<Person> Children { get; set; }

			public IEnumerator<Person> GetEnumerator()
			{
				return Children.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		public class PersonSerializer : MessagePackSerializer<Person>
		{
			public PersonSerializer()
				: base( SerializationContext.Default ) {}

			protected internal override void PackToCore( Packer packer, Person objectTree )
			{
				packer.PackMapHeader( 2 );
				packer.Pack( "Name" );
				packer.Pack( objectTree.Name );
				packer.Pack( "Children" );
				if ( objectTree.Children == null )
				{
					packer.PackNull();
				}
				else
				{
					this.PackPeople( packer, objectTree.Children );
				}
			}

			internal void PackPeople( Packer packer, IEnumerable<Person> people )
			{
				var children = people.ToArray();

				packer.PackArrayHeader( children.Length );
				foreach ( var child in children )
				{
					this.PackTo( packer, child );
				}
			}

			protected internal override Person UnpackFromCore( Unpacker unpacker )
			{
				Assert.That( unpacker.IsMapHeader );
				Assert.That( unpacker.ItemsCount, Is.EqualTo( 2 ) );
				var person = new Person();
				for ( int i = 0; i < 2; i++ )
				{
					string key;
					Assert.That( unpacker.ReadString( out key ) );
					switch ( key )
					{
						case "Name":
						{

							string name;
							Assert.That( unpacker.ReadString( out name ) );
							person.Name = name;
							break;
						}
						case "Children":
						{
							Assert.That( unpacker.Read() );
							if ( !unpacker.LastReadData.IsNil )
							{
								person.Children = this.UnpackPeople( unpacker );
							}
							break;
						}
					}
				}

				return person;
			}

			internal IEnumerable<Person> UnpackPeople( Unpacker unpacker )
			{
				Assert.That( unpacker.IsArrayHeader );
				var itemsCount = ( int )unpacker.ItemsCount;
				var people = new List<Person>( itemsCount );
				for ( int i = 0; i < itemsCount; i++ )
				{
					people.Add( this.UnpackFrom( unpacker ) );
				}

				return people;
			}
		}

		public class ChildrenSerializer : MessagePackSerializer<IEnumerable<Person>>
		{
			private readonly PersonSerializer _personSerializer = new PersonSerializer();

			public ChildrenSerializer()
				: base( SerializationContext.Default ) {}

			protected internal override void PackToCore( Packer packer, IEnumerable<Person> objectTree )
			{
				if ( objectTree is Person )
				{
					this._personSerializer.PackTo( packer, objectTree as Person );
				}
				else
				{
					this._personSerializer.PackPeople( packer, objectTree );
				}
			}

			protected internal override IEnumerable<Person> UnpackFromCore( Unpacker unpacker )
			{
				return this._personSerializer.UnpackPeople( unpacker );
			}
		}

		// Related to issue #62 -- internal types handling is not consistent at first.

		[Test]
		public void TestNonPublicType_Plain_Failed()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<NonPublic>( GetSerializationContext() ) );
		}

		[Test]
		public void TestNonPublicType_MessagePackMember_Failed()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<NonPublicWithMessagePackMember>( GetSerializationContext() ) );
		}

		[Test]
		public void TestNonPublicType_DataContract_Failed()
		{
			Assert.Throws<SerializationException>( () => this.CreateTarget<NonPublicWithDataContract>( GetSerializationContext() ) );
		}

#pragma warning disable 649
		internal class NonPublic
		{
			public int Value;
		}

		internal class NonPublicWithMessagePackMember
		{
			[MessagePackMember( 0 )]
			public int Value;
		}

		[DataContract]
		internal class NonPublicWithDataContract
		{
			[DataMember]
			public int Value;
		}
#pragma warning restore 649

		// issue #63
		[Test]
		public void TestManyMembers()
		{
			var serializer = this.CreateTarget<WithManyMembers>( GetSerializationContext() );
			var target = new WithManyMembers();
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

#pragma warning disable 659
		public class WithManyMembers
		{
			private readonly int[] _backingField = Enumerable.Range( 0, SByte.MaxValue + 2 ).ToArray();

			public int Member0
			{
				get { return this._backingField[ 0 ]; }
				set { this._backingField[ 0 ] = value; }
			}
			public int Member1
			{
				get { return this._backingField[ 1 ]; }
				set { this._backingField[ 1 ] = value; }
			}
			public int Member2
			{
				get { return this._backingField[ 2 ]; }
				set { this._backingField[ 2 ] = value; }
			}
			public int Member3
			{
				get { return this._backingField[ 3 ]; }
				set { this._backingField[ 3 ] = value; }
			}
			public int Member4
			{
				get { return this._backingField[ 4 ]; }
				set { this._backingField[ 4 ] = value; }
			}
			public int Member5
			{
				get { return this._backingField[ 5 ]; }
				set { this._backingField[ 5 ] = value; }
			}
			public int Member6
			{
				get { return this._backingField[ 6 ]; }
				set { this._backingField[ 6 ] = value; }
			}
			public int Member7
			{
				get { return this._backingField[ 7 ]; }
				set { this._backingField[ 7 ] = value; }
			}
			public int Member8
			{
				get { return this._backingField[ 8 ]; }
				set { this._backingField[ 8 ] = value; }
			}
			public int Member9
			{
				get { return this._backingField[ 9 ]; }
				set { this._backingField[ 9 ] = value; }
			}
			public int Member10
			{
				get { return this._backingField[ 10 ]; }
				set { this._backingField[ 10 ] = value; }
			}
			public int Member11
			{
				get { return this._backingField[ 11 ]; }
				set { this._backingField[ 11 ] = value; }
			}
			public int Member12
			{
				get { return this._backingField[ 12 ]; }
				set { this._backingField[ 12 ] = value; }
			}
			public int Member13
			{
				get { return this._backingField[ 13 ]; }
				set { this._backingField[ 13 ] = value; }
			}
			public int Member14
			{
				get { return this._backingField[ 14 ]; }
				set { this._backingField[ 14 ] = value; }
			}
			public int Member15
			{
				get { return this._backingField[ 15 ]; }
				set { this._backingField[ 15 ] = value; }
			}
			public int Member16
			{
				get { return this._backingField[ 16 ]; }
				set { this._backingField[ 16 ] = value; }
			}
			public int Member17
			{
				get { return this._backingField[ 17 ]; }
				set { this._backingField[ 17 ] = value; }
			}
			public int Member18
			{
				get { return this._backingField[ 18 ]; }
				set { this._backingField[ 18 ] = value; }
			}
			public int Member19
			{
				get { return this._backingField[ 19 ]; }
				set { this._backingField[ 19 ] = value; }
			}
			public int Member20
			{
				get { return this._backingField[ 20 ]; }
				set { this._backingField[ 20 ] = value; }
			}
			public int Member21
			{
				get { return this._backingField[ 21 ]; }
				set { this._backingField[ 21 ] = value; }
			}
			public int Member22
			{
				get { return this._backingField[ 22 ]; }
				set { this._backingField[ 22 ] = value; }
			}
			public int Member23
			{
				get { return this._backingField[ 23 ]; }
				set { this._backingField[ 23 ] = value; }
			}
			public int Member24
			{
				get { return this._backingField[ 24 ]; }
				set { this._backingField[ 24 ] = value; }
			}
			public int Member25
			{
				get { return this._backingField[ 25 ]; }
				set { this._backingField[ 25 ] = value; }
			}
			public int Member26
			{
				get { return this._backingField[ 26 ]; }
				set { this._backingField[ 26 ] = value; }
			}
			public int Member27
			{
				get { return this._backingField[ 27 ]; }
				set { this._backingField[ 27 ] = value; }
			}
			public int Member28
			{
				get { return this._backingField[ 28 ]; }
				set { this._backingField[ 28 ] = value; }
			}
			public int Member29
			{
				get { return this._backingField[ 29 ]; }
				set { this._backingField[ 29 ] = value; }
			}
			public int Member30
			{
				get { return this._backingField[ 30 ]; }
				set { this._backingField[ 30 ] = value; }
			}
			public int Member31
			{
				get { return this._backingField[ 31 ]; }
				set { this._backingField[ 31 ] = value; }
			}
			public int Member32
			{
				get { return this._backingField[ 32 ]; }
				set { this._backingField[ 32 ] = value; }
			}
			public int Member33
			{
				get { return this._backingField[ 33 ]; }
				set { this._backingField[ 33 ] = value; }
			}
			public int Member34
			{
				get { return this._backingField[ 34 ]; }
				set { this._backingField[ 34 ] = value; }
			}
			public int Member35
			{
				get { return this._backingField[ 35 ]; }
				set { this._backingField[ 35 ] = value; }
			}
			public int Member36
			{
				get { return this._backingField[ 36 ]; }
				set { this._backingField[ 36 ] = value; }
			}
			public int Member37
			{
				get { return this._backingField[ 37 ]; }
				set { this._backingField[ 37 ] = value; }
			}
			public int Member38
			{
				get { return this._backingField[ 38 ]; }
				set { this._backingField[ 38 ] = value; }
			}
			public int Member39
			{
				get { return this._backingField[ 39 ]; }
				set { this._backingField[ 39 ] = value; }
			}
			public int Member40
			{
				get { return this._backingField[ 40 ]; }
				set { this._backingField[ 40 ] = value; }
			}
			public int Member41
			{
				get { return this._backingField[ 41 ]; }
				set { this._backingField[ 41 ] = value; }
			}
			public int Member42
			{
				get { return this._backingField[ 42 ]; }
				set { this._backingField[ 42 ] = value; }
			}
			public int Member43
			{
				get { return this._backingField[ 43 ]; }
				set { this._backingField[ 43 ] = value; }
			}
			public int Member44
			{
				get { return this._backingField[ 44 ]; }
				set { this._backingField[ 44 ] = value; }
			}
			public int Member45
			{
				get { return this._backingField[ 45 ]; }
				set { this._backingField[ 45 ] = value; }
			}
			public int Member46
			{
				get { return this._backingField[ 46 ]; }
				set { this._backingField[ 46 ] = value; }
			}
			public int Member47
			{
				get { return this._backingField[ 47 ]; }
				set { this._backingField[ 47 ] = value; }
			}
			public int Member48
			{
				get { return this._backingField[ 48 ]; }
				set { this._backingField[ 48 ] = value; }
			}
			public int Member49
			{
				get { return this._backingField[ 49 ]; }
				set { this._backingField[ 49 ] = value; }
			}
			public int Member50
			{
				get { return this._backingField[ 50 ]; }
				set { this._backingField[ 50 ] = value; }
			}
			public int Member51
			{
				get { return this._backingField[ 51 ]; }
				set { this._backingField[ 51 ] = value; }
			}
			public int Member52
			{
				get { return this._backingField[ 52 ]; }
				set { this._backingField[ 52 ] = value; }
			}
			public int Member53
			{
				get { return this._backingField[ 53 ]; }
				set { this._backingField[ 53 ] = value; }
			}
			public int Member54
			{
				get { return this._backingField[ 54 ]; }
				set { this._backingField[ 54 ] = value; }
			}
			public int Member55
			{
				get { return this._backingField[ 55 ]; }
				set { this._backingField[ 55 ] = value; }
			}
			public int Member56
			{
				get { return this._backingField[ 56 ]; }
				set { this._backingField[ 56 ] = value; }
			}
			public int Member57
			{
				get { return this._backingField[ 57 ]; }
				set { this._backingField[ 57 ] = value; }
			}
			public int Member58
			{
				get { return this._backingField[ 58 ]; }
				set { this._backingField[ 58 ] = value; }
			}
			public int Member59
			{
				get { return this._backingField[ 59 ]; }
				set { this._backingField[ 59 ] = value; }
			}
			public int Member60
			{
				get { return this._backingField[ 60 ]; }
				set { this._backingField[ 60 ] = value; }
			}
			public int Member61
			{
				get { return this._backingField[ 61 ]; }
				set { this._backingField[ 61 ] = value; }
			}
			public int Member62
			{
				get { return this._backingField[ 62 ]; }
				set { this._backingField[ 62 ] = value; }
			}
			public int Member63
			{
				get { return this._backingField[ 63 ]; }
				set { this._backingField[ 63 ] = value; }
			}
			public int Member64
			{
				get { return this._backingField[ 64 ]; }
				set { this._backingField[ 64 ] = value; }
			}
			public int Member65
			{
				get { return this._backingField[ 65 ]; }
				set { this._backingField[ 65 ] = value; }
			}
			public int Member66
			{
				get { return this._backingField[ 66 ]; }
				set { this._backingField[ 66 ] = value; }
			}
			public int Member67
			{
				get { return this._backingField[ 67 ]; }
				set { this._backingField[ 67 ] = value; }
			}
			public int Member68
			{
				get { return this._backingField[ 68 ]; }
				set { this._backingField[ 68 ] = value; }
			}
			public int Member69
			{
				get { return this._backingField[ 69 ]; }
				set { this._backingField[ 69 ] = value; }
			}
			public int Member70
			{
				get { return this._backingField[ 70 ]; }
				set { this._backingField[ 70 ] = value; }
			}
			public int Member71
			{
				get { return this._backingField[ 71 ]; }
				set { this._backingField[ 71 ] = value; }
			}
			public int Member72
			{
				get { return this._backingField[ 72 ]; }
				set { this._backingField[ 72 ] = value; }
			}
			public int Member73
			{
				get { return this._backingField[ 73 ]; }
				set { this._backingField[ 73 ] = value; }
			}
			public int Member74
			{
				get { return this._backingField[ 74 ]; }
				set { this._backingField[ 74 ] = value; }
			}
			public int Member75
			{
				get { return this._backingField[ 75 ]; }
				set { this._backingField[ 75 ] = value; }
			}
			public int Member76
			{
				get { return this._backingField[ 76 ]; }
				set { this._backingField[ 76 ] = value; }
			}
			public int Member77
			{
				get { return this._backingField[ 77 ]; }
				set { this._backingField[ 77 ] = value; }
			}
			public int Member78
			{
				get { return this._backingField[ 78 ]; }
				set { this._backingField[ 78 ] = value; }
			}
			public int Member79
			{
				get { return this._backingField[ 79 ]; }
				set { this._backingField[ 79 ] = value; }
			}
			public int Member80
			{
				get { return this._backingField[ 80 ]; }
				set { this._backingField[ 80 ] = value; }
			}
			public int Member81
			{
				get { return this._backingField[ 81 ]; }
				set { this._backingField[ 81 ] = value; }
			}
			public int Member82
			{
				get { return this._backingField[ 82 ]; }
				set { this._backingField[ 82 ] = value; }
			}
			public int Member83
			{
				get { return this._backingField[ 83 ]; }
				set { this._backingField[ 83 ] = value; }
			}
			public int Member84
			{
				get { return this._backingField[ 84 ]; }
				set { this._backingField[ 84 ] = value; }
			}
			public int Member85
			{
				get { return this._backingField[ 85 ]; }
				set { this._backingField[ 85 ] = value; }
			}
			public int Member86
			{
				get { return this._backingField[ 86 ]; }
				set { this._backingField[ 86 ] = value; }
			}
			public int Member87
			{
				get { return this._backingField[ 87 ]; }
				set { this._backingField[ 87 ] = value; }
			}
			public int Member88
			{
				get { return this._backingField[ 88 ]; }
				set { this._backingField[ 88 ] = value; }
			}
			public int Member89
			{
				get { return this._backingField[ 89 ]; }
				set { this._backingField[ 89 ] = value; }
			}
			public int Member90
			{
				get { return this._backingField[ 90 ]; }
				set { this._backingField[ 90 ] = value; }
			}
			public int Member91
			{
				get { return this._backingField[ 91 ]; }
				set { this._backingField[ 91 ] = value; }
			}
			public int Member92
			{
				get { return this._backingField[ 92 ]; }
				set { this._backingField[ 92 ] = value; }
			}
			public int Member93
			{
				get { return this._backingField[ 93 ]; }
				set { this._backingField[ 93 ] = value; }
			}
			public int Member94
			{
				get { return this._backingField[ 94 ]; }
				set { this._backingField[ 94 ] = value; }
			}
			public int Member95
			{
				get { return this._backingField[ 95 ]; }
				set { this._backingField[ 95 ] = value; }
			}
			public int Member96
			{
				get { return this._backingField[ 96 ]; }
				set { this._backingField[ 96 ] = value; }
			}
			public int Member97
			{
				get { return this._backingField[ 97 ]; }
				set { this._backingField[ 97 ] = value; }
			}
			public int Member98
			{
				get { return this._backingField[ 98 ]; }
				set { this._backingField[ 98 ] = value; }
			}
			public int Member99
			{
				get { return this._backingField[ 99 ]; }
				set { this._backingField[ 99 ] = value; }
			}
			public int Member100
			{
				get { return this._backingField[ 100 ]; }
				set { this._backingField[ 100 ] = value; }
			}
			public int Member101
			{
				get { return this._backingField[ 101 ]; }
				set { this._backingField[ 101 ] = value; }
			}
			public int Member102
			{
				get { return this._backingField[ 102 ]; }
				set { this._backingField[ 102 ] = value; }
			}
			public int Member103
			{
				get { return this._backingField[ 103 ]; }
				set { this._backingField[ 103 ] = value; }
			}
			public int Member104
			{
				get { return this._backingField[ 104 ]; }
				set { this._backingField[ 104 ] = value; }
			}
			public int Member105
			{
				get { return this._backingField[ 105 ]; }
				set { this._backingField[ 105 ] = value; }
			}
			public int Member106
			{
				get { return this._backingField[ 106 ]; }
				set { this._backingField[ 106 ] = value; }
			}
			public int Member107
			{
				get { return this._backingField[ 107 ]; }
				set { this._backingField[ 107 ] = value; }
			}
			public int Member108
			{
				get { return this._backingField[ 108 ]; }
				set { this._backingField[ 108 ] = value; }
			}
			public int Member109
			{
				get { return this._backingField[ 109 ]; }
				set { this._backingField[ 109 ] = value; }
			}
			public int Member110
			{
				get { return this._backingField[ 110 ]; }
				set { this._backingField[ 110 ] = value; }
			}
			public int Member111
			{
				get { return this._backingField[ 111 ]; }
				set { this._backingField[ 111 ] = value; }
			}
			public int Member112
			{
				get { return this._backingField[ 112 ]; }
				set { this._backingField[ 112 ] = value; }
			}
			public int Member113
			{
				get { return this._backingField[ 113 ]; }
				set { this._backingField[ 113 ] = value; }
			}
			public int Member114
			{
				get { return this._backingField[ 114 ]; }
				set { this._backingField[ 114 ] = value; }
			}
			public int Member115
			{
				get { return this._backingField[ 115 ]; }
				set { this._backingField[ 115 ] = value; }
			}
			public int Member116
			{
				get { return this._backingField[ 116 ]; }
				set { this._backingField[ 116 ] = value; }
			}
			public int Member117
			{
				get { return this._backingField[ 117 ]; }
				set { this._backingField[ 117 ] = value; }
			}
			public int Member118
			{
				get { return this._backingField[ 118 ]; }
				set { this._backingField[ 118 ] = value; }
			}
			public int Member119
			{
				get { return this._backingField[ 119 ]; }
				set { this._backingField[ 119 ] = value; }
			}
			public int Member120
			{
				get { return this._backingField[ 120 ]; }
				set { this._backingField[ 120 ] = value; }
			}
			public int Member121
			{
				get { return this._backingField[ 121 ]; }
				set { this._backingField[ 121 ] = value; }
			}
			public int Member122
			{
				get { return this._backingField[ 122 ]; }
				set { this._backingField[ 122 ] = value; }
			}
			public int Member123
			{
				get { return this._backingField[ 123 ]; }
				set { this._backingField[ 123 ] = value; }
			}
			public int Member124
			{
				get { return this._backingField[ 124 ]; }
				set { this._backingField[ 124 ] = value; }
			}
			public int Member125
			{
				get { return this._backingField[ 125 ]; }
				set { this._backingField[ 125 ] = value; }
			}
			public int Member126
			{
				get { return this._backingField[ 126 ]; }
				set { this._backingField[ 126 ] = value; }
			}
			public int Member127
			{
				get { return this._backingField[ 127 ]; }
				set { this._backingField[ 127 ] = value; }
			}
			public int Member128
			{
				get { return this._backingField[ 128 ]; }
				set { this._backingField[ 128 ] = value; }
			}

			public override bool Equals( object obj )
			{
				var other = obj as WithManyMembers;
				if ( other == null )
				{
					return false;
				}

				return this._backingField == other._backingField || this._backingField.SequenceEqual( other._backingField );
			}
		}
#pragma warning restore 659

		#region -- Polymorphism --

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}
			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty() 
			{
				this._Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}
			public PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject() 
			{
				this._Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField
		{
			public  Version Reference;
			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField() 
			{
				this.Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject
		{
			public  object Reference;
			public PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject() 
			{
				this.Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				private  set { this._Reference = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				private  set { this._Reference = value; }
			}

			public PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor
		{
			public readonly Version Reference;

			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor( Version Reference ) 
			{
				this.Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Reference;

			public PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}
			public PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty() 
			{
				this._Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}
			public PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject() 
			{
				this._Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField
		{
			public  DateTime Value;
			public PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField() 
			{
				this.Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject
		{
			public  object Value;
			public PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject() 
			{
				this.Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				private  set { this._Value = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				private  set { this._Value = value; }
			}

			public PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor
		{
			public readonly DateTime Value;

			public PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor( DateTime Value ) 
			{
				this.Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Value;

			public PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject( object Value ) 
			{
				this.Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}
			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty() 
			{
				this._Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}
			public PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject() 
			{
				this._Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField
		{
			public  int Primitive;
			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField() 
			{
				this.Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject
		{
			public  object Primitive;
			public PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject() 
			{
				this.Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				private  set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				private  set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor
		{
			public readonly int Primitive;

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor( int Primitive ) 
			{
				this.Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Primitive;

			public PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}
			public PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty() 
			{
				this._String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}
			public PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject() 
			{
				this._String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadWriteField
		{
			public  string String;
			public PolymorphicMemberTypeKnownType_Normal_StringReadWriteField() 
			{
				this.String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_StringReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_StringReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject
		{
			public  object String;
			public PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject() 
			{
				this.String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				private  set { this._String = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				private  set { this._String = value; }
			}

			public PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor
		{
			public readonly string String;

			public PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor( string String ) 
			{
				this.String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject
		{
			public readonly object String;

			public PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject( object String ) 
			{
				this.String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty() 
			{
				this._Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}
			public PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject() 
			{
				this._Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public  FileSystemEntry Polymorphic;
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField() 
			{
				this.Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public  object Polymorphic;
			public PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject() 
			{
				this.Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				private  set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
				private  set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public readonly FileSystemEntry Polymorphic;

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public readonly object Polymorphic;

			public PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;
			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject
		{
			public  IList<string> ListStaticItem;
			public PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}
			public PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemIsReadOnly
		{
			public readonly IList<string> ListStaticItem;
			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemIsReadOnly() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemIsReadOnlyAsObject
		{
			public readonly IList<string> ListStaticItem;
			public PolymorphicMemberTypeKnownType_ListStaticItemIsReadOnlyAsObject() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyPropertyAndConstructor
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyPropertyAndConstructor( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyPropertyAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyPropertyAndConstructorAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyPropertyAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyPropertyAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyFieldAndConstructor
		{
			public readonly IList<string> ListStaticItem;

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyFieldAndConstructor( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyFieldAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyFieldAndConstructorAsObject
		{
			public readonly IList<string> ListStaticItem;

			public PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyFieldAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListStaticItemReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyFieldAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemIsReadOnly
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemIsReadOnly() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemIsReadOnlyAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeKnownType_ListPolymorphicItemIsReadOnlyAsObject() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public  IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public  IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}
			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfIsReadOnly
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfIsReadOnly() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfIsReadOnlyAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfIsReadOnlyAsObject() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty() 
			{
				this._Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}
			public PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject() 
			{
				this._Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField
		{
			public  Tuple<string> Tuple1Static;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField() 
			{
				this.Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject
		{
			public  Tuple<string> Tuple1Static;
			public PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject() 
			{
				this.Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private  set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private  set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty() 
			{
				this._Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField() 
			{
				this.Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;
			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private  set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private  set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty() 
			{
				this._Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject() 
			{
				this._Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField() 
			{
				this.Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;
			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject() 
			{
				this.Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private  set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private  set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty() 
			{
				this._Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField() 
			{
				this.Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private  set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private  set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty() 
			{
				this._Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField() 
			{
				this.Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private  set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private  set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty() 
			{
				this._Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField() 
			{
				this.Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private  set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private  set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty() 
			{
				this._Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField() 
			{
				this.Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private  set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private  set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty() 
			{
				this._Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject() 
			{
				this._Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField() 
			{
				this.Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;
			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject() 
			{
				this.Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private  set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private  set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty() 
			{
				this._Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField() 
			{
				this.Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private  set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private  set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty() 
			{
				this._Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField() 
			{
				this.Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;
			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private  set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private  set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty() 
			{
				this._Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}
			public PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject() 
			{
				this._Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField
		{
			public  Version Reference;
			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField() 
			{
				this.Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject
		{
			public  object Reference;
			public PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject() 
			{
				this.Reference = new Version( 1, 2, 3, 4 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				private  set { this._Reference = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				private  set { this._Reference = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor
		{
			public readonly Version Reference;

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor( Version Reference ) 
			{
				this.Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Reference;

			public PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject( new Version( 1, 2, 3, 4 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.EqualTo( target.Reference ) );
				Assert.That( result.Reference, Is.InstanceOf( target.Reference.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty() 
			{
				this._Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}
			public PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject() 
			{
				this._Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField
		{
			public  DateTime Value;
			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField() 
			{
				this.Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject
		{
			public  object Value;
			public PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject() 
			{
				this.Value = new DateTime( 1982, 1, 29, 15, 46, 12 );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				private  set { this._Value = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				private  set { this._Value = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor
		{
			public readonly DateTime Value;

			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor( DateTime Value ) 
			{
				this.Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Value;

			public PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject( object Value ) 
			{
				this.Value = Value;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty() 
			{
				this._Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}
			public PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject() 
			{
				this._Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField
		{
			public  int Primitive;
			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField() 
			{
				this.Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject
		{
			public  object Primitive;
			public PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject() 
			{
				this.Primitive = 123;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				private  set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				private  set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor
		{
			public readonly int Primitive;

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor( int Primitive ) 
			{
				this.Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Primitive;

			public PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject( 123 );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.EqualTo( target.Primitive ) );
				Assert.That( result.Primitive, Is.InstanceOf( target.Primitive.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}
			public PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty() 
			{
				this._String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}
			public PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject() 
			{
				this._String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField
		{
			public  string String;
			public PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField() 
			{
				this.String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject
		{
			public  object String;
			public PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject() 
			{
				this.String = "ABC";
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				private  set { this._String = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				private  set { this._String = value; }
			}

			public PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor
		{
			public readonly string String;

			public PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor( string String ) 
			{
				this.String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject
		{
			public readonly object String;

			public PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject( object String ) 
			{
				this.String = String;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject( "ABC" );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.EqualTo( target.String ) );
				Assert.That( result.String, Is.InstanceOf( target.String.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty
		{
			[MessagePackRuntimeType]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty() 
			{
				this._Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeType]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject() 
			{
				this._Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField
		{
			[MessagePackRuntimeType]
			public  FileSystemEntry Polymorphic;
			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField() 
			{
				this.Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeType]
			public  object Polymorphic;
			public PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject() 
			{
				this.Polymorphic = new FileEntry { Name = "file", Size = 1 };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeType]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeType]
			private FileSystemEntry _Polymorphic;

			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				private  set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			private object _Polymorphic;

			public object Polymorphic
			{
				get { return this._Polymorphic; }
				private  set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeType]
			public readonly FileSystemEntry Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			public readonly object Polymorphic;

			public PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject( new FileEntry { Name = "file", Size = 1 } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Polymorphic, Is.EqualTo( target.Polymorphic ) );
				Assert.That( result.Polymorphic, Is.InstanceOf( target.Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;
			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject
		{
			public  IList<string> ListStaticItem;
			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}
			public PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}
			public PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemIsReadOnly
		{
			public readonly IList<string> ListStaticItem;
			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemIsReadOnly() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemIsReadOnlyAsObject
		{
			public readonly IList<string> ListStaticItem;
			public PolymorphicMemberTypeRuntimeType_ListStaticItemIsReadOnlyAsObject() 
			{
				this.ListStaticItem = new List<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyPropertyAndConstructor
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyPropertyAndConstructor( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyPropertyAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyPropertyAndConstructorAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyPropertyAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyPropertyAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private  set { this._ListStaticItem = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyFieldAndConstructor
		{
			public readonly IList<string> ListStaticItem;

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyFieldAndConstructor( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyFieldAndConstructor( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyFieldAndConstructorAsObject
		{
			public readonly IList<string> ListStaticItem;

			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyFieldAndConstructorAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyFieldAndConstructorAsObject( new List<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListStaticItem, Is.EqualTo( target.ListStaticItem ) );
				Assert.That( result.ListStaticItem, Is.InstanceOf( target.ListStaticItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemIsReadOnly
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemIsReadOnly() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemIsReadOnlyAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemIsReadOnlyAsObject() 
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			private IList<FileSystemEntry> _ListPolymorphicItem;

			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private  set { this._ListPolymorphicItem = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItem, Is.EqualTo( target.ListPolymorphicItem ) );
				Assert.That( result.ListPolymorphicItem, Is.InstanceOf( target.ListPolymorphicItem.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackRuntimeType]
			public  IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionPropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject() 
			{
				this._ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfIsReadOnly
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfIsReadOnly() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfIsReadOnlySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfIsReadOnly();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfIsReadOnly>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfIsReadOnlyAsObject
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;
			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfIsReadOnlyAsObject() 
			{
				this.ListPolymorphicItself = new Collection<string>{ "A", "B" };
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfIsReadOnlyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfIsReadOnlyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfIsReadOnlyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			private IList<string> _ListPolymorphicItself;

			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private  set { this._ListPolymorphicItself = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject( new Collection<string>{ "A", "B" } );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListPolymorphicItself, Is.EqualTo( target.ListPolymorphicItself ) );
				Assert.That( result.ListPolymorphicItself, Is.InstanceOf( target.ListPolymorphicItself.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty() 
			{
				this._Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject() 
			{
				this._Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField
		{
			public  Tuple<string> Tuple1Static;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField() 
			{
				this.Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject
		{
			public  Tuple<string> Tuple1Static;
			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject() 
			{
				this.Tuple1Static = Tuple.Create( "1" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private  set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private  set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Static, Is.EqualTo( target.Tuple1Static ) );
				Assert.That( result.Tuple1Static, Is.InstanceOf( target.Tuple1Static.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty() 
			{
				this._Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField() 
			{
				this.Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple1Polymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private  set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private  set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1Polymorphic, Is.EqualTo( target.Tuple1Polymorphic ) );
				Assert.That( result.Tuple1Polymorphic, Is.InstanceOf( target.Tuple1Polymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty() 
			{
				this._Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject() 
			{
				this._Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField() 
			{
				this.Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;
			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject() 
			{
				this.Tuple7AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private  set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private  set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllStatic, Is.EqualTo( target.Tuple7AllStatic ) );
				Assert.That( result.Tuple7AllStatic, Is.InstanceOf( target.Tuple7AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty() 
			{
				this._Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField() 
			{
				this.Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7FirstPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private  set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private  set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.EqualTo( target.Tuple7FirstPolymorphic ) );
				Assert.That( result.Tuple7FirstPolymorphic, Is.InstanceOf( target.Tuple7FirstPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty() 
			{
				this._Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField() 
			{
				this.Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private  set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private  set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.EqualTo( target.Tuple7LastPolymorphic ) );
				Assert.That( result.Tuple7LastPolymorphic, Is.InstanceOf( target.Tuple7LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty() 
			{
				this._Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField() 
			{
				this.Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7IntermediatePolymorphic = Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7");
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private  set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private  set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.EqualTo( target.Tuple7IntermediatePolymorphic ) );
				Assert.That( result.Tuple7IntermediatePolymorphic, Is.InstanceOf( target.Tuple7IntermediatePolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty() 
			{
				this._Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField() 
			{
				this.Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple7AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private  set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private  set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.EqualTo( target.Tuple7AllPolymorphic ) );
				Assert.That( result.Tuple7AllPolymorphic, Is.InstanceOf( target.Tuple7AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty() 
			{
				this._Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject() 
			{
				this._Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField() 
			{
				this.Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;
			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject() 
			{
				this.Tuple8AllStatic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private  set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private  set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllStatic, Is.EqualTo( target.Tuple8AllStatic ) );
				Assert.That( result.Tuple8AllStatic, Is.InstanceOf( target.Tuple8AllStatic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty() 
			{
				this._Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField() 
			{
				this.Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple8LastPolymorphic = Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private  set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private  set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.EqualTo( target.Tuple8LastPolymorphic ) );
				Assert.That( result.Tuple8LastPolymorphic, Is.InstanceOf( target.Tuple8LastPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty() 
			{
				this._Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWritePropertySuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject() 
			{
				this._Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField() 
			{
				this.Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteFieldSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;
			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject() 
			{
				this.Tuple8AllPolymorphic = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private  set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private  set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
// L185

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructorSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObjectSuccess()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.EqualTo( target.Tuple8AllPolymorphic ) );
				Assert.That( result.Tuple8AllPolymorphic, Is.InstanceOf( target.Tuple8AllPolymorphic.GetType() ) );
			}
		}
		public class PolymorphicMemberTypeMixed
		{
			public string NormalVanilla { get; set; }
			[MessagePackRuntimeType]
			public FileSystemEntry NormalRuntime { get; set; }
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackKnownType( 2, typeof( DirectoryEntry ) )]
			public FileSystemEntry NormalKnown { get; set; }
			[MessagePackRuntimeType]
			public Object ObjectRuntime { get; set; }
			public IList<string> ListVanilla { get; set; }
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 2, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListKnownItem { get; set; }
			[MessagePackKnownType( 1, typeof( Collection<FileSystemEntry> ) )]
			[MessagePackKnownType( 2, typeof( List<FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListKnwonContainerRuntimeItem { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectRuntimeItem { get; set; }
			public IDictionary<string, string> DictionaryVanilla { get; set; }
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 2, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryKnownValue { get; set; }
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, FileSystemEntry> ) )]
			[MessagePackKnownType( 2, typeof( Dictionary<string, FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryKnownContainerRuntimeValue { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictionaryObjectRuntimeValue { get; set; }
			[MessagePackKnownTupleItemType( 2, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, 2, typeof( DirectoryEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, FileSystemEntry, FileSystemEntry, object> Tuple { get; set; }
			public PolymorphicMemberTypeMixed() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeMixed_Success()
		{
				var context = NewSerializationContext( PackerCompatibilityOptions.None );
				var target = new PolymorphicMemberTypeMixed();
				var serializer = context.GetSerializer<PolymorphicMemberTypeMixed>();
				
				using ( var buffer = new MemoryStream() )
				{
					serializer.Pack( buffer, target );
					buffer.Position = 0;
					var result = serializer.Unpack( buffer );

					Assert.That( result, Is.Not.Null );
					Assert.That( result, Is.Not.SameAs( target ) );
					Assert.That( result.NormalVanilla, Is.EqualTo( target.NormalVanilla ), "NormalVanilla" );
					Assert.That( result.NormalVanilla, Is.InstanceOf( target.NormalVanilla.GetType() ), "NormalVanilla" );
					Assert.That( result.NormalRuntime, Is.EqualTo( target.NormalRuntime ), "NormalRuntime" );
					Assert.That( result.NormalRuntime, Is.InstanceOf( target.NormalRuntime.GetType() ), "NormalRuntime" );
					Assert.That( result.NormalKnown, Is.EqualTo( target.NormalKnown ), "NormalKnown" );
					Assert.That( result.NormalKnown, Is.InstanceOf( target.NormalKnown.GetType() ), "NormalKnown" );
					Assert.That( result.ObjectRuntime, Is.EqualTo( target.ObjectRuntime ), "ObjectRuntime" );
					Assert.That( result.ObjectRuntime, Is.InstanceOf( target.ObjectRuntime.GetType() ), "ObjectRuntime" );
					Assert.That( result.ListVanilla, Is.EqualTo( target.ListVanilla ), "ListVanilla" );
					Assert.That( result.ListVanilla, Is.InstanceOf( target.ListVanilla.GetType() ), "ListVanilla" );
					Assert.That( result.ListKnownItem, Is.EqualTo( target.ListKnownItem ), "ListKnownItem" );
					Assert.That( result.ListKnownItem, Is.InstanceOf( target.ListKnownItem.GetType() ), "ListKnownItem" );
					Assert.That( result.ListKnwonContainerRuntimeItem, Is.EqualTo( target.ListKnwonContainerRuntimeItem ), "ListKnwonContainerRuntimeItem" );
					Assert.That( result.ListKnwonContainerRuntimeItem, Is.InstanceOf( target.ListKnwonContainerRuntimeItem.GetType() ), "ListKnwonContainerRuntimeItem" );
					Assert.That( result.ListObjectRuntimeItem, Is.EqualTo( target.ListObjectRuntimeItem ), "ListObjectRuntimeItem" );
					Assert.That( result.ListObjectRuntimeItem, Is.InstanceOf( target.ListObjectRuntimeItem.GetType() ), "ListObjectRuntimeItem" );
					Assert.That( result.DictionaryVanilla, Is.EqualTo( target.DictionaryVanilla ), "DictionaryVanilla" );
					Assert.That( result.DictionaryVanilla, Is.InstanceOf( target.DictionaryVanilla.GetType() ), "DictionaryVanilla" );
					Assert.That( result.DictionaryKnownValue, Is.EqualTo( target.DictionaryKnownValue ), "DictionaryKnownValue" );
					Assert.That( result.DictionaryKnownValue, Is.InstanceOf( target.DictionaryKnownValue.GetType() ), "DictionaryKnownValue" );
					Assert.That( result.DictionaryKnownContainerRuntimeValue, Is.EqualTo( target.DictionaryKnownContainerRuntimeValue ), "DictionaryKnownContainerRuntimeValue" );
					Assert.That( result.DictionaryKnownContainerRuntimeValue, Is.InstanceOf( target.DictionaryKnownContainerRuntimeValue.GetType() ), "DictionaryKnownContainerRuntimeValue" );
					Assert.That( result.DictionaryObjectRuntimeValue, Is.EqualTo( target.DictionaryObjectRuntimeValue ), "DictionaryObjectRuntimeValue" );
					Assert.That( result.DictionaryObjectRuntimeValue, Is.InstanceOf( target.DictionaryObjectRuntimeValue.GetType() ), "DictionaryObjectRuntimeValue" );
					Assert.That( result.Tuple, Is.EqualTo( target.Tuple ), "Tuple" );
					Assert.That( result.Tuple, Is.InstanceOf( target.Tuple.GetType() ), "Tuple" );
				}
		}

		public class AbstractClassMemberNoAttribute
		{
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassMemberNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassMemberNoAttribute { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<AbstractClassMemberNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<AbstractClassMemberNoAttribute>() );
		}

		public class AbstractClassMemberKnownType
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassMemberKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassMemberKnownType { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<AbstractClassMemberKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class AbstractClassMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassMemberRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassMemberRuntimeType { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<AbstractClassMemberRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class AbstractClassCollectionItemNoAttribute
		{
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionItemNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionItemNoAttribute { Value = new Collection<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<AbstractClassCollectionItemNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<AbstractClassCollectionItemNoAttribute>() );
		}

		public class AbstractClassCollectionItemKnownType
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionItemKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionItemKnownType { Value = new Collection<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<AbstractClassCollectionItemKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class AbstractClassCollectionItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionItemRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionItemRuntimeType { Value = new Collection<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<AbstractClassCollectionItemRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class AbstractClassDictionaryKeyNoAttribute
		{
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictionaryKeyNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictionaryKeyNoAttribute { Value = new SortedDictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<AbstractClassDictionaryKeyNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<AbstractClassDictionaryKeyNoAttribute>() );
		}

		public class AbstractClassDictionaryKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictionaryKeyKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictionaryKeyKnownType { Value = new SortedDictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<AbstractClassDictionaryKeyKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class AbstractClassDictionaryKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictionaryKeyRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictionaryKeyRuntimeType { Value = new SortedDictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<AbstractClassDictionaryKeyRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceMemberNoAttribute
		{
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceMemberNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceMemberNoAttribute { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<InterfaceMemberNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<InterfaceMemberNoAttribute>() );
		}

		public class InterfaceMemberKnownType
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceMemberKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceMemberKnownType { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<InterfaceMemberKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class InterfaceMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceMemberRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceMemberRuntimeType { Value = new FileEntry { Name = "file", Size = 1 } };
			var serializer = context.GetSerializer<InterfaceMemberRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class InterfaceCollectionItemNoAttribute
		{
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionItemNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionItemNoAttribute { Value = new Collection<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<InterfaceCollectionItemNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<InterfaceCollectionItemNoAttribute>() );
		}

		public class InterfaceCollectionItemKnownType
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionItemKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionItemKnownType { Value = new Collection<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<InterfaceCollectionItemKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceCollectionItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionItemRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionItemRuntimeType { Value = new Collection<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };
			var serializer = context.GetSerializer<InterfaceCollectionItemRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceDictionaryKeyNoAttribute
		{
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictionaryKeyNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictionaryKeyNoAttribute { Value = new SortedDictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<InterfaceDictionaryKeyNoAttribute>();

			Assert.Throws<SerializationException>( ()=> context.GetSerializer<InterfaceDictionaryKeyNoAttribute>() );
		}

		public class InterfaceDictionaryKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictionaryKeyKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictionaryKeyKnownType { Value = new SortedDictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<InterfaceDictionaryKeyKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceDictionaryKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictionaryKeyRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictionaryKeyRuntimeType { Value = new SortedDictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };
			var serializer = context.GetSerializer<InterfaceDictionaryKeyRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

	public class EchoKeyedCollection<T> : KeyedCollection<T, T>
	{
		protected override T GetKeyForItem( T item )
		{
			return item;
		}
	}

		public class AbstractClassCollectionNoAttribute
		{
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionNoAttribute_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionNoAttribute { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<AbstractClassCollectionNoAttribute>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class AbstractClassCollectionKnownType
		{
			[MessagePackKnownType( 1, typeof( EchoKeyedCollection<string> ) )]
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionKnownType { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<AbstractClassCollectionKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class AbstractClassCollectionRuntimeType
		{
			[MessagePackRuntimeType]
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassCollectionRuntimeType { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<AbstractClassCollectionRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceCollectionNoAttribute
		{
			public IList<string> Value { get; set; }

			public InterfaceCollectionNoAttribute() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionNoAttribute_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionNoAttribute { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<InterfaceCollectionNoAttribute>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceCollectionKnownType
		{
			[MessagePackKnownType( 1, typeof( EchoKeyedCollection<string> ) )]
			public IList<string> Value { get; set; }

			public InterfaceCollectionKnownType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionKnownType { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<InterfaceCollectionKnownType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class InterfaceCollectionRuntimeType
		{
			[MessagePackRuntimeType]
			public IList<string> Value { get; set; }

			public InterfaceCollectionRuntimeType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceCollectionRuntimeType { Value = new EchoKeyedCollection<string> { "ABC" } };
			var serializer = context.GetSerializer<InterfaceCollectionRuntimeType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value.Count, Is.EqualTo( target.Value.Count ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
				Assert.That( result.Value, Is.EquivalentTo( target.Value ) );
			}
		}

		public class TupleAbstractType
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, 1, typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<AbstractFileSystemEntry, IFileSystemEntry, AbstractFileSystemEntry, IFileSystemEntry> Value { get; set; }

			public TupleAbstractType() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestTupleAbstractType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new TupleAbstractType { Value = Tuple.Create( new FileEntry { Name = "1", Size = 1 } as AbstractFileSystemEntry, new FileEntry { Name = "2", Size = 2 } as IFileSystemEntry, new FileEntry { Name = "3", Size = 3 } as AbstractFileSystemEntry, new FileEntry { Name = "4", Size = 4 } as IFileSystemEntry ) };
			var serializer = context.GetSerializer<TupleAbstractType>();

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.EqualTo( target.Value ) );
				Assert.That( result.Value, Is.InstanceOf( target.Value.GetType() ) );
			}
		}

		public class DuplicatedKnownMember
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public FileSystemEntry Value  { get; set; }

			public DuplicatedKnownMember() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownMember_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownMember();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownMember>() );
		}

		public class DuplicatedKnownCollectionItem
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownCollectionItem() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownCollectionItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownCollectionItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownCollectionItem>() );
		}

		public class DuplicatedKnownDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public DuplicatedKnownDictionaryKey() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownDictionaryKey_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownDictionaryKey();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownDictionaryKey>() );
		}

		public class DuplicatedKnownTupleItem
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownTupleItem() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownTupleItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownTupleItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownTupleItem>() );
		}

		public class KnownAndRuntimeMember
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeType]
			public FileSystemEntry Value  { get; set; }

			public KnownAndRuntimeMember() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeMember_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeMember();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeMember>() );
		}

		public class KnownAndRuntimeCollectionItem
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeCollectionItem() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeCollectionItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeCollectionItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeCollectionItem>() );
		}

		public class KnownAndRuntimeDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public KnownAndRuntimeDictionaryKey() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeDictionaryKey_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeDictionaryKey();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeDictionaryKey>() );
		}

		public class KnownAndRuntimeTupleItem
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeTupleItem() { }
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeTupleItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeTupleItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeTupleItem>() );
		}
		public interface IFileSystemEntry { }

		public abstract class AbstractFileSystemEntry : IFileSystemEntry { }

		public class FileSystemEntry : AbstractFileSystemEntry, IComparable<FileSystemEntry>
		{
			public string Name { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as FileSystemEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode();
			}

			int IComparable<FileSystemEntry>.CompareTo( FileSystemEntry other )
			{
				return String.Compare( this.Name, other.Name, StringComparison.Ordinal );
			}
		}

		public class FileEntry : FileSystemEntry
		{
			public long Size { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as FileEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name && this.Size == other.Size;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode() ^ this.Size.GetHashCode();
			}

			public override string ToString()
			{
				return "File(Name=" + this.Name + ", Size=" + this.Size + ")";
			}
		}

		public class DirectoryEntry : FileSystemEntry
		{
			public int ChildCount { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as DirectoryEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name && this.ChildCount == other.ChildCount;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode() ^ this.ChildCount.GetHashCode();
			}

			public override string ToString()
			{
				return "Directory(Name=" + this.Name + ", ChildCount=" + this.ChildCount + ")";
			}
		}

		#endregion -- Polymorphism --

		[Test]
		public void TestNullField()
		{
			this.TestCoreWithAutoVerify( default( object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestTrueField()
		{
			this.TestCoreWithAutoVerify( true, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTrueFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseField()
		{
			this.TestCoreWithAutoVerify( false, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteField()
		{
			this.TestCoreWithAutoVerify( 1, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteField()
		{
			this.TestCoreWithAutoVerify( 0x80, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteField()
		{
			this.TestCoreWithAutoVerify( 0xff, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0x100, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0xffff, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32Field()
		{
			this.TestCoreWithAutoVerify( 0x10000, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64Field()
		{
			this.TestCoreWithAutoVerify( 0x100000000, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeField()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriField()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestVersionField()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestFILETIMEField()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFILETIMEFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanField()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidField()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharField()
		{
			this.TestCoreWithAutoVerify( '　', this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalField()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestBigIntegerField()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestBigIntegerFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestComplexField()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestDictionaryEntryField()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestKeyValuePairStringComplexField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestStringField()
		{
			this.TestCoreWithAutoVerify( "StringValue", this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldNull()
		{
			this.TestCoreWithAutoVerify( default( String ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestByteArrayField()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCharArrayField()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestArraySegmentByteField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32Field()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectField()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestImage_Field()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectArrayField()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListField()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableField()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObject_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				var target = new ComplexTypeGeneratedEnclosure();
				target.Initialize();
				this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexTypeGeneratedEnclosure_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				var target = new ComplexTypeGeneratedEnclosure();
				target.Initialize();
				this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}
		
		[Test]
		public void TestComplexTypeGenerated_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				var target = new ComplexTypeGenerated();
				target.Initialize();
				this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		[Test]
		public void TestComplexTypeGenerated_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				var target = new ComplexTypeGenerated();
				target.Initialize();
				this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray_WithoutShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = true;
			try 
			{
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContext() );
			}
			finally
			{
				SerializerDebugging.AvoidsGenericSerializer = false;
			}
		}

		private void TestCoreWithAutoVerify<T>( T value, SerializationContext context )
		{
			var target = this.CreateTarget<T>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				AutoMessagePackSerializerTest.Verify( value, unpacked );
			}
		}
		
		private void TestCoreWithVerifiable<T>( T value, SerializationContext context )
			where T : IVerifiable<T>
		{
			var target = this.CreateTarget<T>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( value );
			}
		}	
		
		private void TestCoreWithVerifiable<T>( T[] value, SerializationContext context )
			where T : IVerifiable<T>
		{
			var target = this.CreateTarget<T[]>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T[] unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.Length, Is.EqualTo( value.Length ) );
				for( int i = 0; i < unpacked.Length; i ++ )
				{
					try
					{
						unpacked[ i ].Verify( value[ i ] );
					}
#if MSTEST
					catch( Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException ae )
					{
						throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException( i.ToString(), ae );
					}
#else
					catch( AssertionException ae )
					{
						throw new AssertionException( i.ToString(), ae );
					}
#endif
				}
			}
		}	
		
		private static FILETIME ToFileTime( DateTime dateTime )
		{
			var fileTime = dateTime.ToFileTimeUtc();
			return new FILETIME(){ dwHighDateTime = unchecked( ( int )( fileTime >> 32 ) ), dwLowDateTime = unchecked( ( int )( fileTime & 0xffffffff ) ) };
		}
	}
}
