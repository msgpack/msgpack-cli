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
using System.Globalization;
using System.Linq;

using MsgPack.Serialization;

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
	partial class TimestampTest
	{
		[Test]
		public void TestEncode_Min32()
		{
			Assert.That(
				new Timestamp( 0L, 0 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[]{ 0, 0, 0, 0 } ) )
			);
		}

		[Test]
		public void TestEncode_Max32()
		{
			Assert.That(
				new Timestamp( 4294967295L, 0 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF } ) )
			);
		}

		[Test]
		public void TestEncode_Min64()
		{
			Assert.That(
				new Timestamp( 0L, 1 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[]{ 0, 0, 0, 0x4, 0, 0, 0, 0 } ) )
			);
		}

		[Test]
		public void TestEncode_Max64()
		{
			Assert.That(
				new Timestamp( 17179869183L, 999999999 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[]{ 0xEE, 0x6B, 0x27, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			);
		}

		[Test]
		public void TestEncode_Min96()
		{
			Assert.That(
				new Timestamp( -9223372036854775808L, 0 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[] { 0, 0, 0, 0, 0x80, 0, 0, 0, 0, 0, 0, 0 } ) )
			);
		}

		[Test]
		public void TestEncode_Max96()
		{
			Assert.That(
				new Timestamp( 9223372036854775807L, 999999999 ).Encode(),
				Is.EqualTo( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte[] { 0x3B, 0x9A, 0xC9, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ) )
			);
		}

		[Test]
		public void TestDecode_Min32()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD6, 0xFF }.Concat( new byte[]{ 0, 0, 0, 0 } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.ToString(), Is.EqualTo( "1970-01-01T00:00:00.000000000Z" ) );
		}

		[Test]
		public void TestDecode_Max32()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD6, 0xFF }.Concat( new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 4294967295L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.ToString(), Is.EqualTo( "2106-02-07T06:28:15.000000000Z" ) );
		}

		[Test]
		public void TestDecode_Min64()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD7, 0xFF }.Concat( new byte[]{ 0, 0, 0, 0x4, 0, 0, 0, 0 } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.ToString(), Is.EqualTo( "1970-01-01T00:00:00.000000001Z" ) );
		}

		[Test]
		public void TestDecode_Max64()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD7, 0xFF }.Concat( new byte[]{ 0xEE, 0x6B, 0x27, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 17179869183L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
			Assert.That( result.ToString(), Is.EqualTo( "2514-05-30T01:53:03.999999999Z" ) );
		}

		[Test]
		public void TestDecode_Min96()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xC7, 12, 0xFF }.Concat( new byte[] { 0, 0, 0, 0, 0x80, 0, 0, 0, 0, 0, 0, 0 } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( -9223372036854775808L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.ToString(), Is.EqualTo( "-292277022657-01-27T08:29:52.000000000Z" ) );
		}

		[Test]
		public void TestDecode_Max96()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xC7, 12, 0xFF }.Concat( new byte[] { 0x3B, 0x9A, 0xC9, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } ).ToArray() ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 9223372036854775807L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
			Assert.That( result.ToString(), Is.EqualTo( "292277026596-12-04T15:30:07.999999999Z" ) );
		}

		[Test]
		public void TestDecode_Min64_AllZero()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD7, 0xFF, 0, 0, 0, 0, 0, 0, 0, 0 } ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.ToString(), Is.EqualTo( "1970-01-01T00:00:00.000000000Z" ) );
		}

		[Test]
		public void TestDecode_Min64_MinSeconds()
		{
			var result = Timestamp.Decode( MessagePackSerializer.UnpackMessagePackObject( new byte[] { 0xD7, 0xFF, 0, 0, 0, 0x1, 0, 0, 0, 0 } ).AsMessagePackExtendedTypeObject() );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0x100000000L ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.ToString(), Is.EqualTo( "2106-02-07T06:28:16.000000000Z" ) );
		}

		[Test]
		public void TestDecode_InvalidLength_0()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 0 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_3()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 3 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_5()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 5 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_7()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 7 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_9()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 9 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_11()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 11 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidLength_13()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( MessagePackExtendedTypeObject.Unpack( 0xFF, new byte [ 13 ] ) ) );
		}

		[Test]
		public void TestDecode_InvalidTypeCode()
		{
			Assert.Throws<ArgumentException>( () => Timestamp.Decode( default( MessagePackExtendedTypeObject ) ) );
		}
	}
}
