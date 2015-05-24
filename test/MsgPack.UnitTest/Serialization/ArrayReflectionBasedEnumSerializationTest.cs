#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
	[Timeout( 30000 )]
	public class ArrayReflectionBasedEnumSerializerTest
	{
		private SerializationContext GetSerializationContext()
		{
			return new SerializationContext { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.ReflectionBased };
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
		private void TestEnumForByName<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );

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
		}

		private void TestEnumForByName( SerializationContext context, Type builtType, params string[] builtMembers )
		{
			var serializer = context.GetSerializer( builtType );
			var value = Enum.Parse( builtType, String.Join( ",", builtMembers ) );

			using ( var stream = new MemoryStream() )
			{
				serializer.PackTo( Packer.Create( stream, false ), value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );

				Assert.That( deserialized, Is.EqualTo( value ) );
				stream.Position = 0;
				Assert.That( Unpacking.UnpackString( stream ), Is.EqualTo( value.ToString() ) );
			}
		}

		private void TestEnumForByUnderlyingValue<T>( SerializationContext context, T value, string property )
		{
			var serializer = context.GetSerializer<T>();

			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );

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
		}

		private void TestEnumForByUnderlyingValue( SerializationContext context, Type builtType, params string[] builtMembers )
		{
			var serializer = context.GetSerializer( builtType );
			var value = ( IFormattable )Enum.Parse( builtType, String.Join( ",", builtMembers ) );

			using ( var stream = new MemoryStream() )
			{
				serializer.PackTo( Packer.Create( stream, false ), value );
				stream.Position = 0;
				var deserialized = serializer.Unpack( stream );

				Assert.That( deserialized, Is.EqualTo( value ) );
				stream.Position = 0;
				var result = Unpacking.UnpackObject( stream );
				Assert.That( 
					result.ToString().Equals( value.ToString( "D", null ) ),
					result + " == " + value.ToString( "D", null ) 
				);
			}
		}


		[Test]
		public void TestSerializationMethod_ContextIsDefault_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			TestEnumForByName( context, EnumDefault.Foo, null );
		}

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
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumDefault.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsNone_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByName.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByName_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByNameByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByUnderlyingValue.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByUnderlyingValueByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByName_TypeIsByUnderlyingValue_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumDefault.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "DefaultByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsNone_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "DefaultByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, EnumByName.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByNameByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByName_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByNameByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsNone()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByUnderlyingValue.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsDefault()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueDefaultProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByName( context, new EnumMemberObject(), "ByUnderlyingValueByNameProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestSerializationMethod_ContextIsByUnderlyingValue_TypeIsByUnderlyingValue_MemberIsByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, new EnumMemberObject(), "ByUnderlyingValueByUnderlyingValueProperty" );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumByteFlags.Foo | EnumByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumByte_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumByteFlags.Foo | EnumByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumSByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumSByte.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumSByteFlags.Foo | EnumSByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumSByte_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumSByteFlags.Foo | EnumSByteFlags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt16Flags.Foo | EnumInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt16_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt16Flags.Foo | EnumInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt16.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt16Flags.Foo | EnumUInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt16_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt16Flags.Foo | EnumUInt16Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt32Flags.Foo | EnumInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt32_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt32Flags.Foo | EnumInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt32.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt32Flags.Foo | EnumUInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt32_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt32Flags.Foo | EnumUInt32Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumInt64Flags.Foo | EnumInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumInt64_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumInt64Flags.Foo | EnumInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithoutFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithoutFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt64.Foo, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithFlags_ByName()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			try
			{
				TestEnumForByName( context, EnumUInt64Flags.Foo | EnumUInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}

		[Test]
		public void TestEnumUInt64_WithFlags_ByUnderlyingValue()
		{
			var context = this.GetSerializationContext();
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			try
			{
				TestEnumForByUnderlyingValue( context, EnumUInt64Flags.Foo | EnumUInt64Flags.Bar, null );
			}
			finally
			{
				context.EnumSerializationMethod = EnumSerializationMethod.ByName;
			}
		}
	}
}
