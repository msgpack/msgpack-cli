#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

#if !NETFX_CORE && !WINDOWS_PHONE
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	[TestFixture]
	public class MessagePackObjectTest_Serialization
	{
		[Test]
		public void TestRuntimeSerialization_Zero_Success()
		{
			TestRuntimeSerializationCore( 0 );
		}

		[Test]
		public void TestRuntimeSerialization_Nil_Success()
		{
			TestRuntimeSerializationCore( MessagePackObject.Nil );
		}

		[Test]
		public void TestRuntimeSerialization_True_Success()
		{
			TestRuntimeSerializationCore( true );
		}

		[Test]
		public void TestRuntimeSerialization_False_Success()
		{
			TestRuntimeSerializationCore( false );
		}

		[Test]
		public void TestRuntimeSerialization_TinyPositiveInteger_Success()
		{
			TestRuntimeSerializationCore( 1 );
		}

		[Test]
		public void TestRuntimeSerialization_TinyNegativeIngeter_Success()
		{
			TestRuntimeSerializationCore( -1 );
		}

		[Test]
		public void TestRuntimeSerialization_Int8_Success()
		{
			TestRuntimeSerializationCore( SByte.MinValue );
		}

		[Test]
		public void TestRuntimeSerialization_Int16_Success()
		{
			TestRuntimeSerializationCore( Int16.MinValue );
		}

		[Test]
		public void TestRuntimeSerialization_Int32_Success()
		{
			TestRuntimeSerializationCore( Int32.MinValue );
		}

		[Test]
		public void TestRuntimeSerialization_Int64_Success()
		{
			TestRuntimeSerializationCore( Int64.MinValue );
		}

		[Test]
		public void TestRuntimeSerialization_UInt8_Success()
		{
			TestRuntimeSerializationCore( Byte.MaxValue );
		}

		[Test]
		public void TestRuntimeSerialization_UInt16_Success()
		{
			TestRuntimeSerializationCore( UInt16.MaxValue );
		}

		[Test]
		public void TestRuntimeSerialization_UInt32_Success()
		{
			TestRuntimeSerializationCore( UInt32.MaxValue );
		}

		[Test]
		public void TestRuntimeSerialization_UInt64_Success()
		{
			TestRuntimeSerializationCore( UInt64.MinValue );
		}

		[Test]
		public void TestRuntimeSerialization_Single_Success()
		{
			TestRuntimeSerializationCore( Single.Epsilon );
		}

		[Test]
		public void TestRuntimeSerialization_Double_Success()
		{
			TestRuntimeSerializationCore( Double.Epsilon );
		}

		[Test]
		public void TestRuntimeSerialization_String_Success()
		{
			TestRuntimeSerializationCore( "A" );
		}

		[Test]
		public void TestRuntimeSerialization_Bytes_Success()
		{
			TestRuntimeSerializationCore( new byte[] { 1, 2 } );
		}

		[Test]
		public void TestRuntimeSerialization_Array_Success()
		{
			TestRuntimeSerializationCore( new MessagePackObject[] { 1, 2 } );
		}

		[Test]
		public void TestRuntimeSerialization_Dictionary_Success()
		{
			TestRuntimeSerializationCore( new MessagePackObject( new MessagePackObjectDictionary() { { "1", 1 }, { "2", 2 } } ) );
		}

		private static void TestRuntimeSerializationCore( MessagePackObject target )
		{
			var formatter = new BinaryFormatter();
			using ( var stream = new MemoryStream() )
			{
				formatter.Serialize( stream, target );
				stream.Position = 0;
				var actual = ( MessagePackObject )formatter.Deserialize( stream );

				Assert.AreEqual( target, actual );
			}
		}
	}
}
#endif // #if !NETFX_CORE && !WINDOWS_PHONE