#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
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
	public class RegressionTests
	{
#if !NETFX_CORE
		[Test]
		public void TestIssue70()
		{
			var serializer = MessagePackSerializer.Get<DBNull>( new SerializationContext() );
#if !UNITY
			// Should not be created dynamically.
			Assert.That( serializer, Is.TypeOf( typeof( DefaultSerializers.System_DBNullMessagePackSerializer ) ) );
#endif // !UNITY

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, DBNull.Value );

				buffer.Position = 0;
				var packed = Unpacking.UnpackObject( buffer );
				Assert.That( packed.IsNil, packed.ToString() );

				buffer.Position = 0;
				var unpacked = serializer.Unpack( buffer );
				Assert.That( Object.ReferenceEquals( unpacked, null ), Is.False );
				Assert.That( unpacked, Is.SameAs( DBNull.Value ) );
			}
		}
#endif // !NETFX_CORE

		[Test]
		public void TestIssue73()
		{
			var original = SerializationContext.ConfigureClassic();
			try
			{
				var value =
					new Dictionary<string, object> { { "1", new object() }, { "2", new object() } };
				var serializer = MessagePackSerializer.Get<Dictionary<string, object>>( new SerializationContext() );
				using ( var buffer = new MemoryStream() )
				{
					Assert.Throws<SerializationException>( () => serializer.Pack( buffer, value ) );
				}
			}
			finally
			{
				SerializationContext.Default = original;
			}
		}

		[Test]
		public void TestIssue92_EmptyAsMpo()
		{
			var bytes = new byte[] { 0x82, 0xA1, 0x74, 0x81, 0xA1, 0x74, 0x04, 0xA4, 0x64, 0x61, 0x74, 0x61, 0x80 };
			using ( var buffer = new MemoryStream( bytes ) )
			{
				var serializer = MessagePackSerializer.Get<Dictionary<string, MessagePackObject>>( new SerializationContext() );
				var d = serializer.Unpack( buffer );
			}
		}

		[Test]
		public void TestIssue92_EmptyAsCollection()
		{
			var value = new int[][] { new[] { 1, 2 }, new int[ 0 ] };
			var serializer = MessagePackSerializer.Get<int[][]>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				Console.WriteLine( Binary.ToHexString( buffer.ToArray() ) );
				buffer.Position = 0;
				var a = serializer.Unpack( buffer );
				Assert.That( a.Length, Is.EqualTo( 2 ) );
				Assert.That( a[ 0 ].Length, Is.EqualTo( 2 ) );
				Assert.That( a[ 0 ][ 0 ], Is.EqualTo( 1 ) );
				Assert.That( a[ 0 ][ 1 ], Is.EqualTo( 2 ) );
				Assert.That( a[ 1 ].Length, Is.EqualTo( 0 ) );
			}
		}
	}
}
