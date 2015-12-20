


 
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

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
	public class MapCodeDomBasedAutoMessagePackSerializerTest
	{
		private static SerializationContext GetSerializationContext()
		{
#if !UNITY
			return new SerializationContext { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.CodeDomBased };
#else
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.CodeDomBased };
			AotWorkarounds.SetWorkaround( context );
			return context;

#endif // !UNITY
		}

		private static SerializationContext  NewSerializationContext( PackerCompatibilityOptions compatibilityOptions )
		{
			return new SerializationContext( compatibilityOptions ) { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.CodeDomBased };
		}

		private MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return context.GetSerializer<T>( context );
		}
		
		private bool CanDump
		{
			get { return true; }
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
				DateTime.Now,
				stream => DateTime.FromBinary( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => x.Equals( y ),
				context =>
				{
					Assert.That( context.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.Native ) );
				}
			);
		}

		[Test]
		public void TestDateTimeOffset()
		{
			TestCore(
				DateTimeOffset.Now,
				stream => 
					{
						var array = Unpacking.UnpackArray( stream );
						return new DateTimeOffset( DateTime.FromBinary( array[ 0 ].AsInt64() ), TimeSpan.FromMinutes( array[ 1 ].AsInt16() ) );
					},
				( x, y ) => x.Equals( y ),
				context =>
				{
					Assert.That( context.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.Native ) );
				}
			);
		}

		[Test]
		public void TestDateTimeClassic()
		{
			TestCore(
				DateTime.UtcNow,
				stream => MessagePackConvert.ToDateTime( Unpacking.UnpackInt64( stream ) ),
				CompareDateTime,
				context =>
				{
					context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
				}
			);
		}

		[Test]
		public void TestDateTimeOffsetClassic()
		{
			TestCore(
				DateTimeOffset.UtcNow,
				stream => MessagePackConvert.ToDateTimeOffset( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => CompareDateTime( x.DateTime.ToUniversalTime(), y.DateTime.ToUniversalTime() ),
				context =>
				{
					context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
				}
			);
		}
		
		[Test]
		public void TestDateTimeNullableChangeOnDemand()
		{
			TestCore(
				( DateTime? )DateTime.UtcNow,
				stream => MessagePackConvert.ToDateTime( Unpacking.UnpackInt64( stream ) ),
				CompareDateTime,
				context =>
				{
					context.GetSerializer<DateTime?>();
					context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
				}
			);
		}

		[Test]
		public void TestDateTimeOffsetNullableChangeOnDemand()
		{
			TestCore(
				( DateTimeOffset? )DateTimeOffset.UtcNow,
				stream => MessagePackConvert.ToDateTimeOffset( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => CompareDateTime( x.Value.DateTime.ToUniversalTime(), y.Value.DateTime.ToUniversalTime() ),
				context =>
				{
					context.GetSerializer<DateTimeOffset?>();
					context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
				}
			);
		}

		private static bool CompareDateTime( DateTime x, DateTime y )
		{
			return x.Date == y.Date && x.Hour == y.Hour && x.Minute == y.Minute && x.Second == y.Second && x.Millisecond == y.Millisecond;
		}

		private static bool CompareDateTime( DateTime? x, DateTime? y )
		{
			return CompareDateTime( x.Value, y.Value );
		}

		[Test]
		public void TestDateTimeMemberAttributes_NativeContext_Local()
		{
			var context = GetSerializationContext();
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			var target = this.CreateTarget<AnnotatedDateTimes>( context );
			using ( var buffer = new MemoryStream() )
			{
				var input = new AnnotatedDateTimes( DateTimeOffset.Now );
				target.Pack( buffer, input );
				buffer.Position = 0;
				var result = target.Unpack( buffer );

				// Kind is preserved.
				Assert.That( result.VanillaDateTimeField, Is.EqualTo( input.VanillaDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeField, input.VanillaDateTimeField );
				Assert.That( result.DefaultDateTimeField, Is.EqualTo( input.DefaultDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeField, input.DefaultDateTimeField );
				Assert.That( result.NativeDateTimeField, Is.EqualTo( input.NativeDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeField, input.NativeDateTimeField );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeField, input.UnixEpocDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeField, input.UnixEpocDateTimeField );


				// Offset is preserved. 
				Assert.That( result.VanillaDateTimeOffsetField.DateTime, Is.EqualTo( input.VanillaDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime );
				Assert.That( result.DefaultDateTimeOffsetField.DateTime, Is.EqualTo( input.DefaultDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime );
				Assert.That( result.NativeDateTimeOffsetField.DateTime, Is.EqualTo( input.NativeDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetField.DateTime, input.NativeDateTimeOffsetField.DateTime );
				// UTC is forced.
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime );

				// Kind is preserved.
				Assert.That( result.VanillaDateTimeProperty, Is.EqualTo( input.VanillaDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeProperty, input.VanillaDateTimeProperty );
				Assert.That( result.DefaultDateTimeProperty, Is.EqualTo( input.DefaultDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeProperty, input.DefaultDateTimeProperty );
				Assert.That( result.NativeDateTimeProperty, Is.EqualTo( input.NativeDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeProperty, input.NativeDateTimeProperty );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty );


				// Offset is preserved. 
				Assert.That( result.VanillaDateTimeOffsetProperty.DateTime, Is.EqualTo( input.VanillaDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime );
				Assert.That( result.DefaultDateTimeOffsetProperty.DateTime, Is.EqualTo( input.DefaultDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime );
				Assert.That( result.NativeDateTimeOffsetProperty.DateTime, Is.EqualTo( input.NativeDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetProperty.DateTime, input.NativeDateTimeOffsetProperty.DateTime );
				// UTC is forced.
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime );
			}
		}

		[Test]
		public void TestDateTimeMemberAttributes_NativeContext_Utc()
		{
			var context = GetSerializationContext();
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			var target = this.CreateTarget<AnnotatedDateTimes>( context );
			using ( var buffer = new MemoryStream() )
			{
				var input = new AnnotatedDateTimes( DateTimeOffset.UtcNow );
				target.Pack( buffer, input );
				buffer.Position = 0;
				var result = target.Unpack( buffer );

				// Kind is preserved.
				Assert.That( result.VanillaDateTimeField, Is.EqualTo( input.VanillaDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeField, input.VanillaDateTimeField );
				Assert.That( result.DefaultDateTimeField, Is.EqualTo( input.DefaultDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeField, input.DefaultDateTimeField );
				Assert.That( result.NativeDateTimeField, Is.EqualTo( input.NativeDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeField, input.NativeDateTimeField );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeField, input.UnixEpocDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeField, input.UnixEpocDateTimeField );


				// Offset is preserved. 
				Assert.That( result.VanillaDateTimeOffsetField.DateTime, Is.EqualTo( input.VanillaDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime );
				Assert.That( result.DefaultDateTimeOffsetField.DateTime, Is.EqualTo( input.DefaultDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime );
				Assert.That( result.NativeDateTimeOffsetField.DateTime, Is.EqualTo( input.NativeDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetField.DateTime, input.NativeDateTimeOffsetField.DateTime );
				// UTC == UTC
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime );

				// Kind is preserved.
				Assert.That( result.VanillaDateTimeProperty, Is.EqualTo( input.VanillaDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeProperty, input.VanillaDateTimeProperty );
				Assert.That( result.DefaultDateTimeProperty, Is.EqualTo( input.DefaultDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeProperty, input.DefaultDateTimeProperty );
				Assert.That( result.NativeDateTimeProperty, Is.EqualTo( input.NativeDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeProperty, input.NativeDateTimeProperty );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty );


				// Offset is preserved. 
				Assert.That( result.VanillaDateTimeOffsetProperty.DateTime, Is.EqualTo( input.VanillaDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime );
				Assert.That( result.DefaultDateTimeOffsetProperty.DateTime, Is.EqualTo( input.DefaultDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime );
				Assert.That( result.NativeDateTimeOffsetProperty.DateTime, Is.EqualTo( input.NativeDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetProperty.DateTime, input.NativeDateTimeOffsetProperty.DateTime );
				// UTC == UTC
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime );
			}
		}

		[Test]
		public void TestDateTimeMemberAttributes_UnixEpocContext_Local()
		{
			var context = GetSerializationContext();
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
			var target = this.CreateTarget<AnnotatedDateTimes>( context );
			using ( var buffer = new MemoryStream() )
			{
				var input = new AnnotatedDateTimes( DateTimeOffset.Now );
				target.Pack( buffer, input );
				buffer.Position = 0;
				var result = target.Unpack( buffer );

				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeField, input.VanillaDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeField, input.VanillaDateTimeField );
				Assert.That( CompareDateTime( result.DefaultDateTimeField, input.DefaultDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeField, input.DefaultDateTimeField );
				Assert.That( result.NativeDateTimeField, Is.EqualTo( input.NativeDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeField, input.NativeDateTimeField );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeField, input.UnixEpocDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeField, input.UnixEpocDateTimeField );


				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime );
				Assert.That( CompareDateTime( result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime );
				Assert.That( result.NativeDateTimeOffsetField.DateTime, Is.EqualTo( input.NativeDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetField.DateTime, input.NativeDateTimeOffsetField.DateTime );
				// UTC is forced.
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime );

				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeProperty, input.VanillaDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeProperty, input.VanillaDateTimeProperty );
				Assert.That( CompareDateTime( result.DefaultDateTimeProperty, input.DefaultDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeProperty, input.DefaultDateTimeProperty );
				Assert.That( result.NativeDateTimeProperty, Is.EqualTo( input.NativeDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeProperty, input.NativeDateTimeProperty );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty );


				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime );
				Assert.That( CompareDateTime( result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime );
				Assert.That( result.NativeDateTimeOffsetProperty.DateTime, Is.EqualTo( input.NativeDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetProperty.DateTime, input.NativeDateTimeOffsetProperty.DateTime );
				// UTC is forced.
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime );
			}
		}

		[Test]
		public void TestDateTimeMemberAttributes_UnixEpocContext_Utc()
		{
			var context = GetSerializationContext();
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc;
			var target = this.CreateTarget<AnnotatedDateTimes>( context );
			using ( var buffer = new MemoryStream() )
			{
				var input = new AnnotatedDateTimes( DateTimeOffset.UtcNow );
				target.Pack( buffer, input );
				buffer.Position = 0;
				var result = target.Unpack( buffer );

				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeField, input.VanillaDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeField, input.VanillaDateTimeField );
				Assert.That( CompareDateTime( result.DefaultDateTimeField, input.DefaultDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeField, input.DefaultDateTimeField );
				Assert.That( result.NativeDateTimeField, Is.EqualTo( input.NativeDateTimeField ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeField, input.NativeDateTimeField );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeField, input.UnixEpocDateTimeField.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeField, input.UnixEpocDateTimeField );


				// UTC == UTC
				Assert.That( CompareDateTime( result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetField.DateTime, input.VanillaDateTimeOffsetField.DateTime );
				Assert.That( CompareDateTime( result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetField.DateTime, input.DefaultDateTimeOffsetField.DateTime );
				Assert.That( result.NativeDateTimeOffsetField.DateTime, Is.EqualTo( input.NativeDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetField.DateTime, input.NativeDateTimeOffsetField.DateTime );
				// UTC == UTC
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetField.DateTime, input.UnixEpocDateTimeOffsetField.DateTime );

				// UTC is forced.
				Assert.That( CompareDateTime( result.VanillaDateTimeProperty, input.VanillaDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeProperty, input.VanillaDateTimeProperty );
				Assert.That( CompareDateTime( result.DefaultDateTimeProperty, input.DefaultDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeProperty, input.DefaultDateTimeProperty );
				Assert.That( result.NativeDateTimeProperty, Is.EqualTo( input.NativeDateTimeProperty ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeProperty, input.NativeDateTimeProperty );
				Assert.That( CompareDateTime( result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty.ToUniversalTime() ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeProperty, input.UnixEpocDateTimeProperty );


				// UTC == UTC
				Assert.That( CompareDateTime( result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.VanillaDateTimeOffsetProperty.DateTime, input.VanillaDateTimeOffsetProperty.DateTime );
				Assert.That( CompareDateTime( result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.DefaultDateTimeOffsetProperty.DateTime, input.DefaultDateTimeOffsetProperty.DateTime );
				Assert.That( result.NativeDateTimeOffsetProperty.DateTime, Is.EqualTo( input.NativeDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.NativeDateTimeOffsetProperty.DateTime, input.NativeDateTimeOffsetProperty.DateTime );
				// UTC == UTC
				Assert.That( CompareDateTime( result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime ), "{0:O}({0:%K}) == {1:O}({1:%K})", result.UnixEpocDateTimeOffsetProperty.DateTime, input.UnixEpocDateTimeOffsetProperty.DateTime );
			}
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
				this.TestComplexObjectCore( GetSerializationContext() );
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
				this.TestComplexObjectCore( GetSerializationContext() );
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
				this.TestComplexTypeWithoutAnyAttribute( GetSerializationContext() );
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
				this.TestComplexTypeWithoutAnyAttribute( GetSerializationContext() );
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
			this.TestTypeWithMissingMessagePackMemberAttributeMemberCore( GetSerializationContext() );
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
				this.TestComplexObjectTypeWithDataContractCore( GetSerializationContext() );
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
				this.TestComplexObjectTypeWithDataContractCore( GetSerializationContext() );
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
				this.TestComplexTypeWithDataContractWithOrderCore( GetSerializationContext() );
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
				this.TestComplexTypeWithDataContractWithOrderCore( GetSerializationContext() );
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
				this.TestComplexObjectTypeWithNonSerializedCore( GetSerializationContext() );
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
				this.TestComplexObjectTypeWithNonSerializedCore( GetSerializationContext() );
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
			var serializer = this.CreateTarget<NameValueCollection>( GetSerializationContext() );
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
			var serializer = this.CreateTarget<NameValueCollection>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				Assert.Throws<NotSupportedException>( () => serializer.Pack( stream, target ) );
			}
		}
#endif

		[Test]
		public void TestByteArrayContent()
		{
			var serializer = this.CreateTarget<byte[]>( GetSerializationContext() );
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
				Assert.That( stream.Length, Is.EqualTo( 2 ), BitConverter.ToString( stream.ToArray() ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new byte[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyBytes_Classic()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.Classic );
			var serializer = this.CreateTarget<byte[]>( context );
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
				var value = 
					new TestValueType()
					{ 
						StringField = "ABC", 
						Int32ArrayField = new int[] { 1, 2, 3 }, 
						DictionaryField = 
#if !UNITY
							new Dictionary<int, int>() 
#else
							new Dictionary<int, int>( AotHelper.GetEqualityComparer<int>() ) 
#endif // !UNITY
							{ { 1, 1 } } 
					};
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.StringField, Is.EqualTo( value.StringField ) );
				Assert.That( result.Int32ArrayField, Is.EqualTo( value.Int32ArrayField ) );
				Assert.That( result.DictionaryField, Is.EqualTo( value.DictionaryField ) );
			}
		}

		// Issue81
		[Test]
		public void TestMultiDimensionalArray()
		{
			var array = new int [ 2, 2 ];
			array[ 0, 0 ] = 0;
			array[ 0, 1 ] = 1;
			array[ 1, 0 ] = 10;
			array[ 1, 1 ] = 11;


			var serializer = this.CreateTarget<int[,]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, array );
				stream.Position = 0;

				var result = serializer.Unpack( stream );
				Assert.That( result, Is.TypeOf<int[,]>() );
				Assert.That( result.Rank, Is.EqualTo( 2 ) );
				Assert.That( result.Length, Is.EqualTo( 4 ) );
				Assert.That( result.GetLength( 0 ), Is.EqualTo( 2 ) );
				Assert.That( result.GetLength( 1 ), Is.EqualTo( 2 ) );
				Assert.That( result[ 0, 0 ], Is.EqualTo( 0 ) );
				Assert.That( result[ 0, 1 ], Is.EqualTo( 1 ) );
				Assert.That( result[ 1, 0 ], Is.EqualTo( 10 ) );
				Assert.That( result[ 1, 1 ], Is.EqualTo( 11 ) );
			}
		}

		[Test]
		public void TestMultiDimensionalArrayComprex()
		{
			var array = new int [ 2, 3, 4 ];
			array[ 0, 0, 0 ] = 0;
			array[ 0, 0, 1 ] = 1;
			array[ 0, 0, 2 ] = 2;
			array[ 0, 0, 3 ] = 3;
			array[ 0, 1, 0 ] = 10;
			array[ 0, 1, 1 ] = 11;
			array[ 0, 1, 2 ] = 12;
			array[ 0, 1, 3 ] = 13;
			array[ 0, 2, 0 ] = 20;
			array[ 0, 2, 1 ] = 21;
			array[ 0, 2, 2 ] = 22;
			array[ 0, 2, 3 ] = 23;
			array[ 1, 0, 0 ] = 100;
			array[ 1, 0, 1 ] = 101;
			array[ 1, 0, 2 ] = 102;
			array[ 1, 0, 3 ] = 103;
			array[ 1, 1, 0 ] = 110;
			array[ 1, 1, 1 ] = 111;
			array[ 1, 1, 2 ] = 112;
			array[ 1, 1, 3 ] = 113;
			array[ 1, 2, 0 ] = 120;
			array[ 1, 2, 1 ] = 121;
			array[ 1, 2, 2 ] = 122;
			array[ 1, 2, 3 ] = 123;

			var serializer = this.CreateTarget<int[,,]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, array );
				stream.Position = 0;

				var result = serializer.Unpack( stream );
				Assert.That( result, Is.TypeOf<int[,,]>() );
				Assert.That( result.Rank, Is.EqualTo( 3 ) );
				Assert.That( result.Length, Is.EqualTo( 24 ) );
				Assert.That( result.GetLength( 0 ), Is.EqualTo( 2 ) );
				Assert.That( result.GetLength( 1 ), Is.EqualTo( 3 ) );
				Assert.That( result.GetLength( 2 ), Is.EqualTo( 4 ) );
				Assert.That( result[ 0, 0, 0 ], Is.EqualTo( 0 ) );
				Assert.That( result[ 0, 0, 1 ], Is.EqualTo( 1 ) );
				Assert.That( result[ 0, 0, 2 ], Is.EqualTo( 2 ) );
				Assert.That( result[ 0, 0, 3 ], Is.EqualTo( 3 ) );
				Assert.That( result[ 0, 1, 0 ], Is.EqualTo( 10 ) );
				Assert.That( result[ 0, 1, 1 ], Is.EqualTo( 11 ) );
				Assert.That( result[ 0, 1, 2 ], Is.EqualTo( 12 ) );
				Assert.That( result[ 0, 1, 3 ], Is.EqualTo( 13 ) );
				Assert.That( result[ 0, 2, 0 ], Is.EqualTo( 20 ) );
				Assert.That( result[ 0, 2, 1 ], Is.EqualTo( 21 ) );
				Assert.That( result[ 0, 2, 2 ], Is.EqualTo( 22 ) );
				Assert.That( result[ 0, 2, 3 ], Is.EqualTo( 23 ) );
				Assert.That( result[ 1, 0, 0 ], Is.EqualTo( 100 ) );
				Assert.That( result[ 1, 0, 1 ], Is.EqualTo( 101 ) );
				Assert.That( result[ 1, 0, 2 ], Is.EqualTo( 102 ) );
				Assert.That( result[ 1, 0, 3 ], Is.EqualTo( 103 ) );
				Assert.That( result[ 1, 1, 0 ], Is.EqualTo( 110 ) );
				Assert.That( result[ 1, 1, 1 ], Is.EqualTo( 111 ) );
				Assert.That( result[ 1, 1, 2 ], Is.EqualTo( 112 ) );
				Assert.That( result[ 1, 1, 3 ], Is.EqualTo( 113 ) );
				Assert.That( result[ 1, 2, 0 ], Is.EqualTo( 120 ) );
				Assert.That( result[ 1, 2, 1 ], Is.EqualTo( 121 ) );
				Assert.That( result[ 1, 2, 2 ], Is.EqualTo( 122 ) );
				Assert.That( result[ 1, 2, 3 ], Is.EqualTo( 123 ) );
			}
		}

		[Test]
		public void TestNonZeroBoundArray()
		{
			var array = Array.CreateInstance( typeof( int ), new [] { 2 }, new [] { 1 } );
			array.SetValue( 1, 1 );
			array.SetValue( 2, 2 );

			var serializer = GetSerializationContext().GetSerializer( array.GetType() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, array );
				stream.Position = 0;

				var result = ( Array )serializer.Unpack( stream );
#if !SILVERLIGHT 
				Assert.That( result, Is.TypeOf( array.GetType() ) );
#else
				// Silverlight does not support lower bound settings, so sz array will return.
				Assert.That( result, Is.TypeOf<int[]>() );
#endif // !SILVERLIGHT
				Assert.That( result.Rank, Is.EqualTo( 1 ) );
#if !SILVERLIGHT 
				Assert.That( result.Length, Is.EqualTo( 2 ) );
				Assert.That( result.GetLowerBound( 0 ), Is.EqualTo( 1 ) );
#else
				// Silverlight does not support lower bound settings, so lowerBound + length will  be set.
				Assert.That( result.Length, Is.EqualTo( 3 ) );
				Assert.That( result.GetLowerBound( 0 ), Is.EqualTo( 0 ) );
#endif // !SILVERLIGHT
				Assert.That( result.GetValue( 1 ), Is.EqualTo( 1 ) );
				Assert.That( result.GetValue( 2 ), Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestNonZeroBoundMultidimensionalArray()
		{
			var array = Array.CreateInstance( typeof( int ), new [] { 2, 2 }, new [] { 1, 1 } );
			array.SetValue( 11, new [] { 1, 1 } );
			array.SetValue( 12, new [] { 1, 2 } );
			array.SetValue( 21, new [] { 2, 1 } );
			array.SetValue( 22, new [] { 2, 2 } );

			var serializer = GetSerializationContext().GetSerializer( array.GetType() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, array );
				stream.Position = 0;

				var result = ( Array )serializer.Unpack( stream );
				Assert.That( result, Is.TypeOf( array.GetType() ) );
				Assert.That( result.Rank, Is.EqualTo( 2 ) );
#if !SILVERLIGHT 
				Assert.That( result.Length, Is.EqualTo( 4 ) );
				Assert.That( result.GetLowerBound( 0 ), Is.EqualTo( 1 ) );
				Assert.That( result.GetLowerBound( 1 ), Is.EqualTo( 1 ) );
#else
				// Silverlight does not support lower bound settings, so lowerBound + length will  be set.
				Assert.That( result.Length, Is.EqualTo( 9 ) );
				Assert.That( result.GetLowerBound( 0 ), Is.EqualTo( 0 ) );
				Assert.That( result.GetLowerBound( 1 ), Is.EqualTo( 0 ) );
#endif // !SILVERLIGHT
				Assert.That( result.GetValue( 1, 1 ), Is.EqualTo( 11 ) );
				Assert.That( result.GetValue( 1, 2 ), Is.EqualTo( 12 ) );
				Assert.That( result.GetValue( 2, 1 ), Is.EqualTo( 21 ) );
				Assert.That( result.GetValue( 2, 2 ), Is.EqualTo( 22 ) );
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
				stream.Write( new byte[]{ 0x80 } );
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
					packer.PackMapHeader( 2 );
					packer.PackString( "Field" );
					packer.PackString( "ABC" );
					packer.PackString( "Extra" );
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
				stream.Write( new byte[]{ 0x80 } );
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
					packer.PackMapHeader( 2 );
					packer.PackString( "Property" );
					packer.PackString( "ABC" );
					packer.PackString( "Extra" );
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
					packer.PackMapHeader( 2 );
					packer.PackString( "Property" );
					packer.PackString( "ABC" );
					packer.PackString( "Extra" );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
				stream.Write( new byte[]{ 0x80 }, 0, 1 );
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
		public void TestBinary_ClassicContext()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.Classic );
			var serializer = context.GetSerializer<byte[]>();

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
		public void TestExt_ClassicContext()
		{
			var context = NewSerializationContext( SerializationContext.CreateClassicContext().CompatibilityOptions.PackerCompatibilityOptions );
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

#if !NETFX_35 && !UNITY
		[Test]
		public void TestReadOnlyCollectionInterfaceDefault()
		{
			TestCollectionInterfaceCore<IReadOnlyCollection<int>>(
				new byte[] { 0x91, 0x1 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<List<int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.First(), Is.EqualTo( 1 ) );
				},
				null
			);
			TestCollectionInterfaceCore<IReadOnlyList<int>>(
				new byte[] { 0x91, 0x1 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<List<int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.First(), Is.EqualTo( 1 ) );
				},
				null
			);
			TestCollectionInterfaceCore<IReadOnlyDictionary<int, int>>(
				new byte[] { 0x81, 0x1, 0x2 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<Dictionary<int, int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.ContainsKey( 1 ) );
					Assert.That( result[ 1 ], Is.EqualTo( 2 ) );
				},
				null
			);
		}

		[Test]
		public void TestReadOnlyCollectionInterfaceExplicit()
		{
			TestCollectionInterfaceCore<IReadOnlyCollection<int>>(
				new byte[] { 0x91, 0x1 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<List<int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.First(), Is.EqualTo( 1 ) );
				},
				context => context.DefaultCollectionTypes.Register( typeof( IReadOnlyCollection<int> ), typeof( AppendableReadOnlyCollection<int> ) )
			);
			TestCollectionInterfaceCore<IReadOnlyList<int>>(
				new byte[] { 0x91, 0x1 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<List<int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.First(), Is.EqualTo( 1 ) );
				},
				context => context.DefaultCollectionTypes.Register( typeof( IReadOnlyList<int> ), typeof( AppendableReadOnlyList<int> ) )
			);
			TestCollectionInterfaceCore<IReadOnlyDictionary<int, int>>(
				new byte[] { 0x81, 0x1, 0x2 },
				result =>
				{
					Assert.That( result, Is.InstanceOf<Dictionary<int, int>>() );
					Assert.That( result.Count, Is.EqualTo( 1 ) );
					Assert.That( result.ContainsKey( 1 ) );
					Assert.That( result[ 1 ], Is.EqualTo( 2 ) );
				},
				context => context.DefaultCollectionTypes.Register( typeof( IReadOnlyDictionary<int, int> ), typeof( AppendableReadOnlyDictionary<int, int> ) )
			);
		}

		private static void TestCollectionInterfaceCore<T>( byte[] data, Action<T> assertion, Action<SerializationContext> registration )
		{
			using ( var buffer = new MemoryStream(data) )
			{
				var serializer = MessagePackSerializer.Get<T>( NewSerializationContext( PackerCompatibilityOptions.None ) );
				var result = serializer.Unpack( buffer );
				assertion( result );
			}
		}
#endif // !NETFX_35 && !UNITY

		private void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer )
		{
			TestCore( value, unpacking, comparer, null );
		}

		private void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer, Action<SerializationContext> contextAdjuster )
		{
			var safeComparer = comparer ?? EqualityComparer<T>.Default.Equals;
			var context = GetSerializationContext();
			if ( contextAdjuster != null )
			{
				contextAdjuster( context );
			}

			var target = this.CreateTarget<T>( context );
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
			var target = CreateTarget<HasEnumerable>( GetSerializationContext() );

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
#if !UNITY
						getter = obj => property.GetValue( obj, null );
#else
						getter = obj => property.GetGetMethod( true ).InvokePreservingExceptionType( obj );
#endif // !UNITY
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

#if !UNITY
		// Mono 2.7.3 AOT fails when these classes are used...
		// Issue 119
#region -- Generic --

		[Test]
		public void TestGenericDerived_Value_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target =
				new GenericValueClass
				{
					GenericField = 1,
					GenericProperty = 2
				};
			var serializer = context.GetSerializer<GenericValueClass>();

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.Not.Null );
				Assert.That( result.GenericField, Is.EqualTo( target.GenericField ) );
				Assert.That( result.GenericProperty, Is.EqualTo( target.GenericProperty ) );
			}
		}

		[Test]
		public void TestGenericDerived_Reference_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target =
				new GenericReferenceClass
				{
					GenericField = "1",
					GenericProperty = "2"
				};
			var serializer = context.GetSerializer<GenericReferenceClass>();

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.Not.Null );
				Assert.That( result.GenericField, Is.EqualTo( target.GenericField ) );
				Assert.That( result.GenericProperty, Is.EqualTo( target.GenericProperty ) );
			}
		}

		[Test]
		public void TestGenericRecordDerived_Value_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target =
				new GenericRecordValueClass( 1, 2 );
			var serializer = context.GetSerializer<GenericRecordValueClass>();

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.Not.Null );
				Assert.That( result.GenericField, Is.EqualTo( target.GenericField ) );
				Assert.That( result.GenericProperty, Is.EqualTo( target.GenericProperty ) );
			}
		}

		[Test]
		public void TestGenericRecordDerived_Reference_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target =
				new GenericRecordReferenceClass( "1", "2" );
			var serializer = context.GetSerializer<GenericRecordReferenceClass>();

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.Not.Null );
				Assert.That( result.GenericField, Is.EqualTo( target.GenericField ) );
				Assert.That( result.GenericProperty, Is.EqualTo( target.GenericProperty ) );
			}
		}

#endregion -- Generic --

#region -- Nullable --
		// Issue #121

		[Test]
		public void TestNullable_Primitive_NonNull_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = DateTime.UtcNow.Millisecond;
			var serializer = MessagePackSerializer.CreateInternal<int?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

		[Test]
		public void TestNullable_Primitive_Null_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = default( int? );
			var serializer = MessagePackSerializer.CreateInternal<int?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

		[Test]
		public void TestNullable_Complex_NonNull_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = TimeSpan.FromSeconds( DateTime.UtcNow.Millisecond );
			var serializer = MessagePackSerializer.CreateInternal<TimeSpan?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

		[Test]
		public void TestNullable_Complex_Null_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = default( TimeSpan? );
			var serializer = MessagePackSerializer.CreateInternal<TimeSpan?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

		[Test]
		public void TestNullable_Enum_NonNull_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = DayOfWeek.Monday;
			var serializer = MessagePackSerializer.CreateInternal<DayOfWeek?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}

		[Test]
		public void TestNullable_Enum_Null_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = default( DayOfWeek? );
			var serializer = MessagePackSerializer.CreateInternal<DayOfWeek?>( context, null );

			using( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( target ) );
			}
		}
#endregion  -- Nullable --

#endif // !UNITY

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
				packer.PackString( "Name" );
				packer.PackString( objectTree.Name );
				packer.PackString( "Children" );
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
		#region ---- KnownType ----

		#region ------ KnownType.NormalTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_StringReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject_Success()
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
		#endregion ------ KnownType.NormalTypes ------

		#region ------ KnownType.CollectionTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField>() );
		}
		#endregion ------ KnownType.CollectionTypes ------

		#region ------ KnownType.DictionaryTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField>() );
		}
		#endregion ------ KnownType.DictionaryTypes ------

#if !NETFX_35 && !UNITY
		#region ------ KnownType.TupleTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor_Success()
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
		#endregion ------ KnownType.TupleTypes ------
#endif // #if !NETFX_35 && !UNITY

		#endregion ---- KnownType ----
		#region ---- RuntimeType ----

		#region ------ RuntimeType.NormalTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Reference, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Value, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.Primitive, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject_AsMpo()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject_AsMpo()
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
				Assert.That( result.String, Is.InstanceOf( typeof( MessagePackObject ) ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject_Success()
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
		#endregion ------ RuntimeType.NormalTypes ------

		#region ------ RuntimeType.CollectionTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItem, Is.EqualTo( target.ListObjectItem ) );
				Assert.That( result.ListObjectItem, Is.InstanceOf( target.ListObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField>();
				
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.ListObjectItself, Is.EqualTo( target.ListObjectItself ) );
				Assert.That( result.ListObjectItself, Is.InstanceOf( target.ListObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField>() );
		}
		#endregion ------ RuntimeType.CollectionTypes ------

		#region ------ RuntimeType.DictionaryTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.EqualTo( target.DictStaticKeyAndStaticItem ) );
				Assert.That( result.DictStaticKeyAndStaticItem, Is.InstanceOf( target.DictStaticKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.EqualTo( target.DictPolymorphicKeyAndStaticItem ) );
				Assert.That( result.DictPolymorphicKeyAndStaticItem, Is.InstanceOf( target.DictPolymorphicKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.EqualTo( target.DictObjectKeyAndStaticItem ) );
				Assert.That( result.DictObjectKeyAndStaticItem, Is.InstanceOf( target.DictObjectKeyAndStaticItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.EqualTo( target.DictStaticKeyAndPolymorphicItem ) );
				Assert.That( result.DictStaticKeyAndPolymorphicItem, Is.InstanceOf( target.DictStaticKeyAndPolymorphicItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.EqualTo( target.DictStaticKeyAndObjectItem ) );
				Assert.That( result.DictStaticKeyAndObjectItem, Is.InstanceOf( target.DictStaticKeyAndObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.EqualTo( target.DictPolymorphicKeyAndItem ) );
				Assert.That( result.DictPolymorphicKeyAndItem, Is.InstanceOf( target.DictPolymorphicKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectKeyAndItem, Is.EqualTo( target.DictObjectKeyAndItem ) );
				Assert.That( result.DictObjectKeyAndItem, Is.InstanceOf( target.DictObjectKeyAndItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictPolymorphicItself, Is.EqualTo( target.DictPolymorphicItself ) );
				Assert.That( result.DictPolymorphicItself, Is.InstanceOf( target.DictPolymorphicItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.DictObjectItself, Is.EqualTo( target.DictObjectItself ) );
				Assert.That( result.DictObjectItself, Is.InstanceOf( target.DictObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField.Initialize();
			Assert.Throws<SerializationException>( () => context.GetSerializer<PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField>() );
		}
		#endregion ------ RuntimeType.DictionaryTypes ------

#if !NETFX_35 && !UNITY
		#region ------ RuntimeType.TupleTypes ------

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItem, Is.EqualTo( target.Tuple1ObjectItem ) );
				Assert.That( result.Tuple1ObjectItem, Is.InstanceOf( target.Tuple1ObjectItem.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple1ObjectItself, Is.EqualTo( target.Tuple1ObjectItself ) );
				Assert.That( result.Tuple1ObjectItself, Is.InstanceOf( target.Tuple1ObjectItself.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField.Initialize();
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			var serializer = context.GetSerializer<PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.EqualTo( target.Tuple7MidPolymorphic ) );
				Assert.That( result.Tuple7MidPolymorphic, Is.InstanceOf( target.Tuple7MidPolymorphic.GetType() ) );
			}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField.Initialize();
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor_Success()
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor_Success()
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
		#endregion ------ RuntimeType.TupleTypes ------
#endif // #if !NETFX_35 && !UNITY

		#endregion ---- RuntimeType ----

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeMixed_Success()
		{
				var context = NewSerializationContext( PackerCompatibilityOptions.None );
				var target = new PolymorphicMemberTypeMixed();
				target.NormalVanilla = "ABC";
				target.NormalRuntime = new FileEntry { Name = "File", Size = 1 };
				target.NormalKnown = new FileEntry { Name = "File", Size = 2 };
				target.ObjectRuntime = new FileEntry { Name = "File", Size = 3 };
				target.ObjectRuntimeOmittedType = new MsgPack.UnitTest.TestTypes.OmittedType { Value = "ABC" };
				target.ListVanilla = new List<string> { "ABC" };
				target.ListKnownItem = new List<FileSystemEntry> { new FileEntry { Name = "File", Size = 1 } };
				target.ListKnwonContainerRuntimeItem = new List<FileSystemEntry> { new FileEntry { Name = "File", Size = 2 } };
				target.ListObjectRuntimeItem = new List<object> { new FileEntry { Name = "File", Size = 3 } };
				target.DictionaryVanilla = new Dictionary<string, string> { { "Key", "ABC" } };
				target.DictionaryKnownValue = new Dictionary<string, FileSystemEntry> { { "Key", new FileEntry { Name = "File", Size = 1 } } };
				target.DictionaryKnownContainerRuntimeValue = new Dictionary<string, FileSystemEntry> { { "Key", new FileEntry { Name = "File", Size = 2 } } };
				target.DictionaryObjectRuntimeValue = new Dictionary<string, object> { { "Key", new FileEntry { Name = "File", Size = 3 } } };
#if !NETFX_35 && !UNITY
				target.Tuple = Tuple.Create<string, FileSystemEntry, FileSystemEntry, object>( "ABC", new FileEntry { Name = "File", Size = 1 }, new FileEntry { Name = "File", Size = 3 }, new FileEntry { Name = "File", Size = 3 } );
#endif // !NETFX_35 && !UNITY
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
					Assert.That( result.ObjectRuntimeOmittedType, Is.EqualTo( target.ObjectRuntimeOmittedType ), "ObjectRuntimeOmittedType" );
					Assert.That( result.ObjectRuntimeOmittedType, Is.InstanceOf( target.ObjectRuntimeOmittedType.GetType() ), "ObjectRuntimeOmittedType" );
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
#if !NETFX_35 && !UNITY
					Assert.That( result.Tuple, Is.EqualTo( target.Tuple ), "Tuple" );
					Assert.That( result.Tuple, Is.InstanceOf( target.Tuple.GetType() ), "Tuple" );
#endif // !NETFX_35 && !UNITY
				}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestPolymorphicMemberTypeMixed_Null_Success()
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
					Assert.That( result.NormalVanilla, Is.Null );
					Assert.That( result.NormalRuntime, Is.Null );
					Assert.That( result.NormalKnown, Is.Null );
					Assert.That( result.ObjectRuntime, Is.Null );
					Assert.That( result.ObjectRuntimeOmittedType, Is.Null );
					Assert.That( result.ListVanilla, Is.Null );
					Assert.That( result.ListKnownItem, Is.Null );
					Assert.That( result.ListKnwonContainerRuntimeItem, Is.Null );
					Assert.That( result.ListObjectRuntimeItem, Is.Null );
					Assert.That( result.DictionaryVanilla, Is.Null );
					Assert.That( result.DictionaryKnownValue, Is.Null );
					Assert.That( result.DictionaryKnownContainerRuntimeValue, Is.Null );
					Assert.That( result.DictionaryObjectRuntimeValue, Is.Null );
#if !NETFX_35 && !UNITY
					Assert.That( result.Tuple, Is.Null );
#endif // !NETFX_35 && !UNITY
				}
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassMemberNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassMemberNoAttribute { Value = new FileEntry { Name = "file", Size = 1 } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<AbstractClassMemberNoAttribute>() );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassListItemNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassListItemNoAttribute { Value = new List<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<AbstractClassListItemNoAttribute>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassListItemKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassListItemKnownType { Value = new List<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			var serializer = context.GetSerializer<AbstractClassListItemKnownType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassListItemRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassListItemRuntimeType { Value = new List<AbstractFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			var serializer = context.GetSerializer<AbstractClassListItemRuntimeType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictKeyNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictKeyNoAttribute { Value = new Dictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<AbstractClassDictKeyNoAttribute>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictKeyKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictKeyKnownType { Value = new Dictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			var serializer = context.GetSerializer<AbstractClassDictKeyKnownType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassDictKeyRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new AbstractClassDictKeyRuntimeType { Value = new Dictionary<AbstractFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			var serializer = context.GetSerializer<AbstractClassDictKeyRuntimeType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceMemberNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceMemberNoAttribute { Value = new FileEntry { Name = "file", Size = 1 } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<InterfaceMemberNoAttribute>() );
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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceListItemNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceListItemNoAttribute { Value = new List<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<InterfaceListItemNoAttribute>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceListItemKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceListItemKnownType { Value = new List<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			var serializer = context.GetSerializer<InterfaceListItemKnownType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceListItemRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceListItemRuntimeType { Value = new List<IFileSystemEntry>{ new FileEntry { Name = "file", Size = 1 } } };

			var serializer = context.GetSerializer<InterfaceListItemRuntimeType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictKeyNoAttribute_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictKeyNoAttribute { Value = new Dictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			Assert.Throws<NotSupportedException>( ()=> context.GetSerializer<InterfaceDictKeyNoAttribute>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictKeyKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictKeyKnownType { Value = new Dictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			var serializer = context.GetSerializer<InterfaceDictKeyKnownType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceDictKeyRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new InterfaceDictKeyRuntimeType { Value = new Dictionary<IFileSystemEntry, string> { { new FileEntry { Name = "file", Size = 1 }, "ABC" } } };

			var serializer = context.GetSerializer<InterfaceDictKeyRuntimeType>();

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionNoAttribute_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( KeyedCollection<string, string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new AbstractClassCollectionNoAttribute { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( KeyedCollection<string, string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new AbstractClassCollectionKnownType { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAbstractClassCollectionRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( KeyedCollection<string, string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new AbstractClassCollectionRuntimeType { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionNoAttribute_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new InterfaceCollectionNoAttribute { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionKnownType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new InterfaceCollectionKnownType { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestInterfaceCollectionRuntimeType_Success()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( EchoKeyedCollection<string, string> ) );
			var target = new InterfaceCollectionRuntimeType { Value = new EchoKeyedCollection<string, string> { "ABC" } };

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
#if !NETFX_35 && !UNITY
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
#endif // !NETFX_35 && !UNITY

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownMember_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownMember();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownMember>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownCollectionItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownCollectionItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownCollectionItem>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownDictionaryKey_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownDictionaryKey();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownDictionaryKey>() );
		}
#if !NETFX_35 && !UNITY

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_DuplicatedKnownTupleItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new DuplicatedKnownTupleItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<DuplicatedKnownTupleItem>() );
		}
#endif // !NETFX_35 && !UNITY

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeMember_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeMember();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeMember>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeCollectionItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeCollectionItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeCollectionItem>() );
		}

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeDictionaryKey_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeDictionaryKey();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeDictionaryKey>() );
		}
#if !NETFX_35 && !UNITY

		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestAttribute_KnownAndRuntimeTupleItem_Fail()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new KnownAndRuntimeTupleItem();
			Assert.Throws<SerializationException>( ()=> context.GetSerializer<KnownAndRuntimeTupleItem>() );
		}
#endif // !NETFX_35 && !UNITY
		// Issue 137
		[Test]
		[Category( "PolymorphicSerialization" )]
		public void TestGlobalNamespace()
		{
			var context = NewSerializationContext( PackerCompatibilityOptions.None );
			var target = new HasGlobalNamespaceType { GlobalType = new TypeInGlobalNamespace { Value = "ABC" } };
			var serializer = context.GetSerializer<HasGlobalNamespaceType>();
				
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, target );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result, Is.Not.Null );
				Assert.That( result, Is.Not.SameAs( target ) );
				Assert.That( result.GlobalType, Is.Not.Null );
				Assert.That( result.GlobalType, Is.Not.SameAs( target.GlobalType ) );
				Assert.That( result.GlobalType.Value, Is.EqualTo( target.GlobalType.Value ) );
			}
		}

		#endregion -- Polymorphism --
		[Test]
		public void TestNullField()
		{
			this.TestCoreWithAutoVerify( default( object ), GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestTrueField()
		{
			this.TestCoreWithAutoVerify( true, GetSerializationContext() );
		}
		
		[Test]
		public void TestTrueFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseField()
		{
			this.TestCoreWithAutoVerify( false, GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteField()
		{
			this.TestCoreWithAutoVerify( 1, GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestByteField()
		{
			this.TestCoreWithAutoVerify( 0x80, GetSerializationContext() );
		}
		
		[Test]
		public void TestByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteField()
		{
			this.TestCoreWithAutoVerify( 0xff, GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0x100, GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0xffff, GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32Field()
		{
			this.TestCoreWithAutoVerify( 0x10000, GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64Field()
		{
			this.TestCoreWithAutoVerify( 0x100000000, GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeField()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestUriField()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestVersionField()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestFILETIMEField()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), GetSerializationContext() );
		}
		
		[Test]
		public void TestFILETIMEFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanField()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidField()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCharField()
		{
			this.TestCoreWithAutoVerify( '　', GetSerializationContext() );
		}
		
		[Test]
		public void TestCharFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalField()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), GetSerializationContext() );
		}
		
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestBigIntegerField()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, GetSerializationContext() );
		}
		
		[Test]
		public void TestBigIntegerFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestComplexField()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestDictionaryEntryField()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), 2 ).ToArray(), GetSerializationContext() );
		}
		
#if !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestKeyValuePairStringComplexField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), GetSerializationContext() );
		}
		
#endif // !NETFX_35 && !WINDOWS_PHONE
		[Test]
		public void TestStringField()
		{
			this.TestCoreWithAutoVerify( "StringValue", GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldNull()
		{
			this.TestCoreWithAutoVerify( default( String ), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestByteArrayField()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestCharArrayField()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestArraySegmentByteField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32Field()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), GetSerializationContext() );
		}
		
#if !NETFX_35
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectField()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestImage_Field()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectArrayField()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), GetSerializationContext() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListField()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableField()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObject_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), GetSerializationContext() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure_WithShortcut()
		{
			SerializerDebugging.AvoidsGenericSerializer = false;
			try 
			{
				var target = new ComplexTypeGeneratedEnclosure();
				target.Initialize();
				this.TestCoreWithVerifiable( target, GetSerializationContext() );
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
				this.TestCoreWithVerifiable( target, GetSerializationContext() );
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
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), GetSerializationContext() );
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
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), GetSerializationContext() );
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
				this.TestCoreWithVerifiable( target, GetSerializationContext() );
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
				this.TestCoreWithVerifiable( target, GetSerializationContext() );
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
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), GetSerializationContext() );
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
				this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), GetSerializationContext() );
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
						throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException( String.Format( "[{0}]:{1}", i, ae.Message ), ae );
					}
#else
					catch( AssertionException ae )
					{
						throw new AssertionException( String.Format( "[{0}]:{1}", i, ae.Message ), ae );
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
