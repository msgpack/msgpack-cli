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
using System.IO;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
using IgnoreAttribute =  Microsoft.VisualStudio.TestPlatform.UnitTestFramework.IgnoreAttribute;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public partial class VersioningTest
	{
		private static MessagePackSerializer<T> CreateSerializer<T>( EmitterFlavor flavor )
		{
#if NETFX_35 || NETFX_CORE || SILVERLIGHT
			var context = new SerializationContext();
#else
			var context = PreGeneratedSerializerActivator.CreateContext( SerializationMethod.Array, SerializationContext.Default.CompatibilityOptions.PackerCompatibilityOptions );
#endif
#if !XAMIOS && !UNITY_IPHONE
			context.EmitterFlavor = flavor;
			return MessagePackSerializer.CreateInternal<T>( context, PolymorphismSchema.Default );
#else
			return context.GetSerializer<T>();
#endif // !XAMIOS && !UNITY_IPHONE
		}

		private static void TestExtraFieldCore( SerializationMethod method, EmitterFlavor flavor, PackerCompatibilityOptions compat )
		{
			var serializer = CreateSerializer<VersioningTestTarget>(flavor);

			using ( var stream = new MemoryStream() )
			{
				if ( method == SerializationMethod.Array )
				{
					stream.Write( new byte[] { 0x94, 0x1, 0xFF, 0xA1, ( byte )'a', 0xC0 } );
				}
				else
				{
					const string String10 = "1234567890";	// packed as MinimumFixedRaw or Bin8
					const string String40 = "1234567890123456789012345678901234567890"; // packed as Bin8 or Str8
					var packer = Packer.Create( stream, compat, false );
					packer.PackMapHeader( 7 );
					packer.PackString( "Field1" );
					packer.Pack( 1 );
					packer.PackString( "Extra1" );
					packer.PackString( String40 );
					packer.PackString( "Extra2" );
					packer.PackBinary(System.Text.Encoding.UTF8.GetBytes(String40));
					packer.PackString( "Field2" );
					packer.Pack( -1 );
					packer.PackString( "Extra3" );
					packer.PackString( String10 );
					packer.PackString( "Field3" );
					packer.PackString( "a" );
					packer.PackString( "Extra4" );
					packer.PackNull();
				}

				stream.Position = 0;

				var result = serializer.Unpack( stream );

				Assert.That(result.Field1, Is.EqualTo(1));
				Assert.That(result.Field2, Is.EqualTo(-1));
				Assert.That(result.Field3, Is.EqualTo("a"));
			}
		}

		private static void TestMissingFieldCore( SerializationMethod method, EmitterFlavor flavor )
		{
			var serializer = CreateSerializer<VersioningTestTarget>( flavor );

			using ( var stream = new MemoryStream() )
			{
				if ( method == SerializationMethod.Array )
				{
					stream.Write( new byte[] { 0x91, 0x1 } );
				}
				else
				{
					var packer = Packer.Create( stream, false );
					packer.PackMapHeader( 1 );
					packer.PackString( "Field1" );
					packer.Pack( 1 );
				}

				stream.Position = 0;

				var result = serializer.Unpack( stream );

				Assert.That( result.Field1, Is.EqualTo( 1 ) );
				Assert.That( result.Field2, Is.EqualTo( 0 ) );
				Assert.That( result.Field3, Is.Null );
			}
		}

		private static void TestFieldInvalidTypeCore( SerializationMethod method, EmitterFlavor flavor )
		{
			var serializer = CreateSerializer<VersioningTestTarget>( flavor );

			using ( var stream = new MemoryStream() )
			{
				if ( method == SerializationMethod.Array )
				{
					stream.Write( new byte[] { 0x93, 0x1, 0xFF, 0x1 } );
				}
				else
				{
					var packer = Packer.Create( stream, false );
					packer.PackMapHeader( 3 );
					packer.PackString( "Field1" );
					packer.Pack( 1 );
					packer.PackString( "Field2" );
					packer.Pack( -1 );
					packer.PackString( "Field3" );
					packer.Pack( 1 );
				}

				stream.Position = 0;

				var result = serializer.Unpack( stream );

				Assert.That( result.Field1, Is.EqualTo( 1 ) );
				Assert.That( result.Field2, Is.EqualTo( -1 ) );
				Assert.That( result.Field3, Is.EqualTo( "a" ) );
			}
		}

		private static void TestFieldSwappedCore( EmitterFlavor flavor )
		{
			var serializer = CreateSerializer<VersioningTestTarget>( flavor );

			using ( var stream = new MemoryStream() )
			{

				var packer = Packer.Create( stream, false );
				packer.PackMapHeader( 3 );
				packer.PackString( "Field1" );
				packer.Pack( 1 );
				packer.PackString( "Field2" );
				packer.Pack( -1 );
				packer.PackString( "Extra" );
				packer.Pack( 2 ); // Issue6

				stream.Position = 0;

				var result = serializer.Unpack( stream );

				Assert.That( result.Field1, Is.EqualTo( 1 ) );
				Assert.That( result.Field2, Is.EqualTo( -1 ) );
				Assert.That( result.Field3, Is.Null );
			}
		}
	}


	public class VersioningTestTarget {
		[MessagePackMember(0)]
		public Int32 Field1 { get; set; }
		[MessagePackMember(1)]
		public Int32 Field2 { get; set; }
		[MessagePackMember(2)]
		public String Field3 { get; set; }
	}
}
