#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2017 FUJIWARA, Yusuke
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
using System.Reflection;
using System.Text.RegularExpressions;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

#if !SILVERLIGHT && !AOT && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
using MsgPack.Serialization.CodeDomSerializers;
#endif // !SILVERLIGHT && !AOT && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !SILVERLIGHT && !AOT && !NETSTANDARD1_1
using MsgPack.Serialization.EmittingSerializers;
#endif // !SILVERLIGHT && !AOT && !NETSTANDARD1_1

#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using SetUpAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TearDownAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCleanupAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 30000 )]
	public class ArrayFieldBasedEnumSerializerTest
	{
		private SerializationContext GetSerializationContext()
		{
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Array };
			context.SerializerOptions.EmitterFlavor = EmitterFlavor.FieldBased;
			return context;
		}
		private bool CanDump
		{
			get { return true; }
		}

#if !SILVERLIGHT && !AOT && !XAMARIN
		[SetUp]
		public void SetUp()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			//SerializerDebugging.TraceEnabled = true;
			//SerializerDebugging.DumpEnabled = true;
			if ( SerializerDebugging.TraceEnabled )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
#if NETSTANDARD2_0
				Tracer.Emit.Listeners.Add( new TextWriterTraceListener( Console.Out ) );
#else // NETSTANDRD2_0
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
#endif // NETSTANDRD2_0
			}

			SerializerDebugging.DependentAssemblyManager = new TempFileDependentAssemblyManager( TestContext.CurrentContext.TestDirectory );
			SerializerDebugging.DeletePastTemporaries();
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = true;

#if NET35
			SerializerDebugging.SetCodeCompiler( CodeDomCodeGeneration.Compile );
#else
			SerializerDebugging.SetCodeCompiler( RoslynCodeGeneration.Compile );
#endif // NET35

			SerializerDebugging.DumpDirectory = TestContext.CurrentContext.TestDirectory;
			SerializerDebugging.AddRuntimeAssembly( typeof( AddOnlyCollection<> ).Assembly.Location );
			if( typeof( AddOnlyCollection<> ).Assembly != this.GetType().Assembly )
			{
				SerializerDebugging.AddRuntimeAssembly( this.GetType().Assembly.Location );
			}
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		[TearDown]
		public void TearDown()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			if ( SerializerDebugging.DumpEnabled && this.CanDump )
			{
#if !NETSTANDARD2_0
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
					SerializationMethodGeneratorManager.Refresh();
				}
#else // !NETSTANDARD2_0
				SerializationMethodGeneratorManager.Refresh();
#endif // !NETSTANDARD2_0
			}

			SerializerDebugging.Reset();
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = false;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}
#endif // !SILVERLIGHT && !AOT && !XAMARIN
		private static void TestEnumForByNameCore<T>( Stream stream, T value, T deserialized, string property )
		{
				if ( property == null )
				{
					Assert.That( deserialized, Is.EqualTo( value ) );
					stream.Position = 0;
					Assert.That( Unpacking.UnpackString( stream ), Is.EqualTo( value.ToString() ) );
				}
				else
				{
					var propertyInfo = typeof( T ).GetProperty( property );
#if !UNITY
					Assert.That( propertyInfo.GetValue( deserialized, null ), Is.EqualTo( propertyInfo.GetValue( value, null ) ) );
#else
					Assert.That( propertyInfo.GetGetMethod().Invoke( deserialized, null ), Is.EqualTo( propertyInfo.GetGetMethod().Invoke( value, null ) ) );
#endif // !UNITY
					stream.Position = 0;
					// Properties are sorted by lexical order
					var index = Array.IndexOf( typeof( T ).GetProperties().OrderBy( p => p.Name ).ToArray(), propertyInfo );
					var result = Unpacking.UnpackArray( stream );
					Assert.That(
#if !UNITY
						result[ index ].Equals( propertyInfo.GetValue( value, null ).ToString() ),
						result[ index ] + " == " + propertyInfo.GetValue( value, null )
#else
						result[ index ].Equals( propertyInfo.GetGetMethod().Invoke( value, null ).ToString() ),
						result[ index ] + " == " + propertyInfo.GetGetMethod().Invoke( value, null )
#endif // !UNITY
					);
				}
		}

		private static void TestEnumForByName<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );
				TestEnumForByNameCore( stream, value, deserialized, property );
			}
		}

#if FEATURE_TAP

		private static async Task TestEnumForByNameAsync<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				await serializer.PackAsync( stream, value, CancellationToken.None ).ConfigureAwait( false );
				stream.Position = 0;
				var deserialized = await serializer.UnpackAsync( stream, CancellationToken.None ).ConfigureAwait( false );
				TestEnumForByNameCore( stream, value, deserialized, property );
			}
		}

#endif // FEATURE_TAP

		private static void TestEnumForByUnderlyingValueCore<T>( Stream stream, T value, T deserialized, string property )
		{

				if ( property == null )
				{
					Assert.That( deserialized, Is.EqualTo( value ) );
					stream.Position = 0;
					var result = Unpacking.UnpackObject( stream );
					Assert.That( 
						result.ToString().Equals( ( ( IFormattable )value ).ToString( "D", null ) ),
						result + " == " + ( ( IFormattable )value ).ToString( "D", null ) 
					);
				}
				else
				{
					var propertyInfo = typeof( T ).GetProperty( property );
#if !UNITY
					Assert.That( propertyInfo.GetValue( deserialized, null ), Is.EqualTo( propertyInfo.GetValue( value, null ) ) );
#else
					Assert.That( propertyInfo.GetGetMethod().Invoke( deserialized, null ), Is.EqualTo( propertyInfo.GetGetMethod().Invoke( value, null ) ) );
#endif // !UNITY
					stream.Position = 0;
					var result = Unpacking.UnpackArray( stream );
					// Properties are sorted by lexical order
					var index = Array.IndexOf( typeof( T ).GetProperties().OrderBy( p => p.Name ).ToArray(), propertyInfo );
					Assert.That(
#if !UNITY
						result[ index ].ToString().Equals( ( ( IFormattable )propertyInfo.GetValue( value, null ) ).ToString( "D", null ) ),
						result[ index ] + " == " + ( ( IFormattable )propertyInfo.GetValue( value , null) ).ToString( "D", null )
#else
						result[ index ].ToString().Equals( ( ( IFormattable )propertyInfo.GetGetMethod().Invoke( value, null ) ).ToString( "D", null ) ),
						result[ index ] + " == " + ( ( IFormattable )propertyInfo.GetGetMethod().Invoke( value , null) ).ToString( "D", null )
#endif // !UNITY
					);
				}
		}

		private static void TestEnumForByUnderlyingValue<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );
				TestEnumForByUnderlyingValueCore( stream, value, deserialized, property );
			}
		}

#if FEATURE_TAP

		private static async Task TestEnumForByUnderlyingValueAsync<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				await serializer.PackAsync( stream, value, CancellationToken.None ).ConfigureAwait( false );
				stream.Position = 0;
				var deserialized = await serializer.UnpackAsync( stream, CancellationToken.None ).ConfigureAwait( false );
				TestEnumForByUnderlyingValueCore( stream, value, deserialized, property );
			}
		}


#endif // FEATURE_TAP


		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, EnumDefault.Foo, null );
		}

#if FEATURE_TAP

		[Test]
		public async Task TestAsyncSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			await TestEnumForByNameAsync( context, EnumDefault.Foo, null );
		}

#endif // FEATURE_TAP


		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, new EnumMemberObject(), "DefaultDefaultProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, new EnumMemberObject(), "DefaultByNameProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultByUnderlyingValueProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, EnumByName.Foo, null );
		}

#if FEATURE_TAP

		[Test]
		public async Task TestAsyncSerializationMethod_ContextIsDefault_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			await TestEnumForByNameAsync( context, EnumByName.Foo, null );
		}

#endif // FEATURE_TAP


		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByName_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, new EnumMemberObject(), "ByNameDefaultProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByName_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, new EnumMemberObject(), "ByNameByNameProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByName_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByNameByUnderlyingValueProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			TestEnumForByUnderlyingValue( context, EnumByUnderlyingValue.Foo, null );
		}

#if FEATURE_TAP

		[Test]
		public async Task TestAsyncSerializationMethod_ContextIsDefault_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			await TestEnumForByUnderlyingValueAsync( context, EnumByUnderlyingValue.Foo, null );
		}

#endif // FEATURE_TAP


		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByUnderlyingValue_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueDefaultProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByUnderlyingValue_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, new EnumMemberObject(), "ByUnderlyingValueByNameProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsByUnderlyingValue_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueByUnderlyingValueProperty" );
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumDefault.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByName.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByNameByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByUnderlyingValue.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByUnderlyingValueByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumDefault.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, EnumByName.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByNameByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByUnderlyingValue.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByUnderlyingValueByNameProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByteFlags.Foo | EnumByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByteFlags.Foo | EnumByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumSByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumSByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumSByteFlags.Foo | EnumSByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumSByteFlags.Foo | EnumSByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt16Flags.Foo | EnumInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt16Flags.Foo | EnumInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt16Flags.Foo | EnumUInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt16Flags.Foo | EnumUInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt32Flags.Foo | EnumInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt32Flags.Foo | EnumInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt32Flags.Foo | EnumUInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt32Flags.Foo | EnumUInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt64Flags.Foo | EnumInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt64Flags.Foo | EnumInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt64Flags.Foo | EnumUInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt64Flags.Foo | EnumUInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		// Issue #184
		[Test]
		public void TestEnumKeyTransformer_Default_AsIs()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			Assert.That( context.EnumSerializationOptions.NameTransformer, Is.Null, "default value" );
			TestEnumKeyCore( context, "ToEven", isAsync: false );
		}

#if FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_Default_AsIs_Async()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			Assert.That( context.EnumSerializationOptions.NameTransformer, Is.Null, "default value" );
			TestEnumKeyCore( context, "ToEven", isAsync: true );
		}

#endif // FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_LowerCamel()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = EnumNameTransformers.LowerCamel;
			TestEnumKeyCore( context, "toEven", isAsync: false );
		}

#if FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_LowerCamel_Async()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = EnumNameTransformers.LowerCamel;
			TestEnumKeyCore( context, "toEven", isAsync: true );
		}

#endif // FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_AllUpper()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = EnumNameTransformers.UpperSnake;
			TestEnumKeyCore( context, "TO_EVEN", isAsync: false );
		}

#if FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_AllUpper_Async()
		{
			var context = GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = EnumNameTransformers.UpperSnake;
			TestEnumKeyCore( context, "TO_EVEN", isAsync: true );
		}

#endif // FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_Custom()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = 
				key => Regex.Replace( key, "[A-Z]", match => match.Index == 0 ? match.Value.ToLower() : "-" + match.Value.ToLower() );
			TestEnumKeyCore( context, "to-even", isAsync: false );
		}

#if FEATURE_TAP

		[Test]
		public void TestEnumKeyTransformer_Custom_Async()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;
			context.EnumSerializationOptions.NameTransformer = 
				key => Regex.Replace( key, "[A-Z]", match => match.Index == 0 ? match.Value.ToLower() : "-" + match.Value.ToLower() );
			TestEnumKeyCore( context, "to-even", isAsync: true );
		}

#endif // FEATURE_TAP

		private static void TestEnumKeyCore( SerializationContext context, string expected, bool isAsync )
		{
			var serializer = context.GetSerializer<MidpointRounding>();
			var obj = MidpointRounding.ToEven;
			using ( var buffer = new MemoryStream() )
			{
#if FEATURE_TAP
				if ( isAsync )
				{
					serializer.PackAsync( buffer, obj, CancellationToken.None ).Wait();
				}
				else
				{
#endif // FEATURE_TAP
					serializer.Pack( buffer, obj );
#if FEATURE_TAP
				}
#endif // FEATURE_TAP

				buffer.Position = 0;
				var stringValue = MessagePackSerializer.UnpackMessagePackObject( buffer ).AsString();

				Assert.That( stringValue, Is.EqualTo( expected ) );

				buffer.Position = 0;

				MidpointRounding deserialized;
#if FEATURE_TAP
				if ( isAsync )
				{
					deserialized = serializer.UnpackAsync( buffer, CancellationToken.None ).Result;
				}
				else
				{
#endif // FEATURE_TAP
					deserialized = serializer.Unpack( buffer );
#if FEATURE_TAP
				}
#endif // FEATURE_TAP

				Assert.That( deserialized, Is.EqualTo( obj ) );
			}
		}
	}
}
