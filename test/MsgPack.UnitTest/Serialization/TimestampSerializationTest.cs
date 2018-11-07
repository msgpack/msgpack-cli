#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
using System.Runtime.InteropServices.ComTypes;
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY

using MsgPack.Serialization.DefaultSerializers;

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
	public class TimestampSerializationTest
	{
		private static void TestSerializationCore( SerializationContext context, DateTimeConversionMethod expected )
		{
			Assert.That( context.DefaultDateTimeConversionMethod, Is.EqualTo( expected ) );

			var now = Timestamp.UtcNow;
			var dateTimeNow = now.ToDateTimeOffset();
			var source =
			new ClassHasTimestamp
			{
				Timestamp = now,
				DateTime = dateTimeNow.AddSeconds( 1 ).UtcDateTime,
				DateTimeOffset = dateTimeNow.AddSeconds( 2 ),
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
				FileTime = now.Add( TimeSpan.FromSeconds( 3 ) ).ToDateTime().ToWin32FileTimeUtc()
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
			};

			using ( var buffer = new MemoryStream() )
			{
				var serializer = context.GetSerializer<ClassHasTimestamp>();
				serializer.Pack(
					buffer,
					source
				);

				// Test representation
				buffer.Position = 0;
				var native = MessagePackSerializer.UnpackMessagePackObject( buffer );
				Assert.That( native.IsArray, Is.True );
				var nativeMembers = native.AsList();
				Assert.That( nativeMembers.Count, Is.EqualTo( ClassHasTimestamp.MemberCount ) );

				for ( var i = 0; i < ClassHasTimestamp.MemberCount; i++ )
				{
					switch ( expected )
					{
						case DateTimeConversionMethod.Native:
						{
							if ( i == 2 )
							{
								// DateTimeOffset -> [long, shourt]
								Assert.That( nativeMembers[ i ].IsArray, Is.True );
								var dtoComponents = nativeMembers[ i ].AsList();
								Assert.That( dtoComponents.Count, Is.EqualTo( 2 ) );
								Assert.That( dtoComponents[ 0 ].IsTypeOf<long>(), Is.True );
								Assert.That( dtoComponents[ 1 ].IsTypeOf<short>(), Is.True );
							}
							else
							{
								Assert.That( nativeMembers[ i ].IsTypeOf<long>(), Is.True );
							}
							break;
						}
						case DateTimeConversionMethod.UnixEpoc:
						{
							Assert.That( nativeMembers[ i ].IsTypeOf<long>(), Is.True );
							break;
						}
						case DateTimeConversionMethod.Timestamp:
						{
							Assert.That( nativeMembers[ i ].IsTypeOf<MessagePackExtendedTypeObject>(), Is.True );
							var asExt = nativeMembers[ i ].AsMessagePackExtendedTypeObject();
							Assert.That( asExt.TypeCode, Is.EqualTo( 0xFF ) );
							// Actual encoding should be tested in Timestamp tests.
							break;
						}
					}
				}

				// Test round trip
				buffer.Position = 0;

				var result = serializer.Unpack( buffer );
				switch ( expected )
				{
					case DateTimeConversionMethod.Native:
					{
						Assert.That( result.Timestamp, Is.EqualTo( new Timestamp( now.UnixEpochSecondsPart, now.NanosecondsPart / 100 * 100 ) ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 ) ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart % 100 ) ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
					case DateTimeConversionMethod.UnixEpoc:
					{
						Assert.That( result.Timestamp, Is.EqualTo( new Timestamp( now.UnixEpochSecondsPart, now.NanosecondsPart / 1000000 * 1000000 ) ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
					case DateTimeConversionMethod.Timestamp:
					{
						Assert.That( result.Timestamp, Is.EqualTo( now ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 ) ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
				}
			}
		}

		[Test]
		public void TestSerialization_Default_Timestamp()
		{
			TestSerializationCore( new SerializationContext(), DateTimeConversionMethod.Timestamp );
		}

		[Test]
		public void TestSerialization_Classic0_5_UnixEpoc()
		{
			TestSerializationCore( SerializationContext.CreateClassicContext( SerializationCompatibilityLevel.Version0_5 ), DateTimeConversionMethod.UnixEpoc );
		}

		[Test]
		public void TestSerialization_Classic0_9_Native()
		{
			TestSerializationCore( SerializationContext.CreateClassicContext( SerializationCompatibilityLevel.Version0_9 ), DateTimeConversionMethod.Native );
		}

		[Test]
		public void TestSerialization_Timestamp()
		{
			TestSerializationCore( new SerializationContext() { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Timestamp }, DateTimeConversionMethod.Timestamp );
		}

		[Test]
		public void TestSerialization_Native()
		{
			TestSerializationCore( new SerializationContext() { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native }, DateTimeConversionMethod.Native );
		}

		[Test]
		public void TestSerialization_UnixEpoc()
		{
			TestSerializationCore( new SerializationContext() { DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc }, DateTimeConversionMethod.UnixEpoc );
		}

		private static void TestDeserializationCore( DateTimeConversionMethod kind )
		{
			var now = Timestamp.UtcNow;
			var dateTimeNow = now.ToDateTimeOffset();
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				packer.PackArrayHeader( ClassHasTimestamp.MemberCount );
				for ( var i = 0; i < ClassHasTimestamp.MemberCount; i++ )
				{
					switch ( kind )
					{
						case DateTimeConversionMethod.Native:
						{
							if ( i == 2 )
							{
								// DateTimeOffset -> [long, shourt]
								packer.PackArrayHeader( 2 );
								packer.Pack( dateTimeNow.AddSeconds( i ).Ticks );
								packer.Pack( ( short )0 );
							}
							else
							{
								packer.Pack( dateTimeNow.AddSeconds( i ).Ticks );
							}

							break;
						}
						case DateTimeConversionMethod.UnixEpoc:
						{
							packer.Pack( MessagePackConvert.FromDateTimeOffset( dateTimeNow.AddSeconds( i ) ) );
							break;
						}
						case DateTimeConversionMethod.Timestamp:
						{
							packer.PackExtendedTypeValue( ( now.Add( TimeSpan.FromSeconds( i ) ).Encode() ) );
							break;
						}
						default:
						{
							Assert.Fail( "Unexpected Kind" );
							break;
						}
					}
				}

				var context = new SerializationContext();
				context.DefaultDateTimeConversionMethod = kind;

				var serializer = context.GetSerializer<ClassHasTimestamp>();
				buffer.Position = 0;

				var result = serializer.Unpack( buffer );
				switch ( kind )
				{
					case DateTimeConversionMethod.Native:
					{
						Assert.That( result.Timestamp, Is.EqualTo( new Timestamp( now.UnixEpochSecondsPart, now.NanosecondsPart / 100 * 100 ) ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 )) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart % 100 ) ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
					case DateTimeConversionMethod.UnixEpoc:
					{
						Assert.That( result.Timestamp, Is.EqualTo( new Timestamp( now.UnixEpochSecondsPart, now.NanosecondsPart / 1000000 * 1000000 ) ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ).Subtract( TimeSpan.FromTicks( now.NanosecondsPart / 100 % 10000 ) ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
					case DateTimeConversionMethod.Timestamp:
					{
						Assert.That( result.Timestamp, Is.EqualTo( now ) );
						Assert.That( result.DateTime, Is.EqualTo( now.ToDateTime().AddSeconds( 1 ) ) );
						Assert.That( result.DateTimeOffset, Is.EqualTo( now.ToDateTimeOffset().AddSeconds( 2 ) ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						Assert.That( result.FileTime, Is.EqualTo( now.Add( TimeSpan.FromSeconds( 3 ) ).ToDateTime().ToWin32FileTimeUtc() ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
						break;
					}
				}
			}
		}

		[Test]
		public void TestDeserialization_FromTimestamp()
		{
			TestDeserializationCore( DateTimeConversionMethod.Timestamp );
		}

		[Test]
		public void TestDeserialization_FromNative()
		{
			TestDeserializationCore( DateTimeConversionMethod.Native );
		}

		[Test]
		public void TestDeserialization_FromUnixEpoc()
		{
			TestDeserializationCore( DateTimeConversionMethod.UnixEpoc );
		}
	}

	public class ClassHasTimestamp
	{
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
		internal const int MemberCount = 4;
#else // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
		internal const int MemberCount = 3;
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY

		[MessagePackMember( 0 )]
		public Timestamp Timestamp { get; set; }
		[MessagePackMember( 1 )]
		public DateTime DateTime { get; set; }
		[MessagePackMember( 2 )]
		public DateTimeOffset DateTimeOffset { get; set; }
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
		[MessagePackMember( 3 )]
		public FILETIME FileTime { get; set; }
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMARIN && !UNITY && !UNITY
	}
}
