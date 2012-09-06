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

using System;
using System.Text;
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
	/// <summary>
	///Tests the Message Pack Convert 
	/// </summary>
	[TestFixture()]
	public class MessagePackConvertTest
	{
		// 1byte, 3bytes, 2bytes chars.
		private const string _testValue = "A\u3000\u00C0";

		private static readonly DateTime _utcEpoc = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		[Test]
		public void TestEncodeString_Normal_EncodedAsUtf8NonBom()
		{
			var encoding = new UTF8Encoding( encoderShouldEmitUTF8Identifier: false );
			Assert.AreEqual( encoding.GetBytes( _testValue ), MessagePackConvert.EncodeString( _testValue ) );
		}

		[Test]
		public void TestEncodeString_Empty_EncodedAsEmpty()
		{
			Assert.That( MessagePackConvert.EncodeString( String.Empty ), Is.Empty );
		}

		[Test]
		public void TestEncodeString_Null()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackConvert.EncodeString( null ) );
		}


		[Test]
		public void TestDecodeStringStrict_Normal_Success()
		{
			var encoding = new UTF8Encoding( encoderShouldEmitUTF8Identifier: false );
			byte[] value = encoding.GetBytes( _testValue );
			Assert.AreEqual( _testValue, MessagePackConvert.DecodeStringStrict( value ) );
		}

		[Test]
		public void TestDecodeStringStrict_WithBom_Success()
		{
			var encoding = new UTF8Encoding( encoderShouldEmitUTF8Identifier: true );
			byte[] value = encoding.GetBytes( _testValue );
			Assert.AreEqual( _testValue, MessagePackConvert.DecodeStringStrict( value ) );
		}

		[Test]
		public void TestDecodeStringStrict_Invalid()
		{
			byte[] value = Encoding.Unicode.GetBytes( _testValue );
			Assert.Throws<DecoderFallbackException>( () => MessagePackConvert.DecodeStringStrict( value ) );
		}

		[Test]
		public void TestDecodeStringStrict_Empty_Empty()
		{
			Assert.That( MessagePackConvert.DecodeStringStrict( new byte[ 0 ] ), Is.Empty );
		}

		[Test]
		public void TestDecodeStringStrict_Null()
		{
			Assert.Throws<ArgumentNullException>( () => MessagePackConvert.DecodeStringStrict( null ) );
		}


		private static void AssertIsUnixEpocDateTimeOffset( DateTimeOffset actual, long millisecondsOffset )
		{
			Assert.That( actual.Offset, Is.EqualTo( TimeSpan.Zero ) );
			var epoc = new DateTimeOffset( 1970, 1, 1, 0, 0, 0, TimeSpan.Zero );
			Assert.That( actual, Is.EqualTo( epoc.AddMilliseconds( millisecondsOffset ) ) );
		}

		[Test]
		public void TestToDateTimeOffset_Zero_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTimeOffset( MessagePackConvert.ToDateTimeOffset( 0 ), 0 );
		}

		[Test]
		public void TestToDateTimeOffset_One_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTimeOffset( MessagePackConvert.ToDateTimeOffset( 1 ), 1 );
		}

		[Test]
		public void TestToDateTimeOffset_MinuOne_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTimeOffset( MessagePackConvert.ToDateTimeOffset( -1 ), -1 );
		}

		[Test]
		public void TestToDateTimeOffset_Maximum_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MaxValue.Subtract( _utcEpoc ).TotalMilliseconds ) - 1L;
			AssertIsUnixEpocDateTimeOffset( MessagePackConvert.ToDateTimeOffset( offset ), offset );
		}

		[Test]
		public void TestToDateTimeOffset_Minimum_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MinValue.Subtract( _utcEpoc ).TotalMilliseconds );
			AssertIsUnixEpocDateTimeOffset( MessagePackConvert.ToDateTimeOffset( offset ), offset );
		}

		[Test]
		public void TestToDateTimeOffset_MaximumPlusOne_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MaxValue.Subtract( _utcEpoc ).TotalMilliseconds + 1L );
			Assert.Throws<ArgumentOutOfRangeException>( () => MessagePackConvert.ToDateTimeOffset( offset ) );
		}

		[Test]
		public void TestToDateTimeOffset_MinimumMinusOne_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MinValue.Subtract( _utcEpoc ).TotalMilliseconds - 1L );
			Assert.Throws<ArgumentOutOfRangeException>( () => MessagePackConvert.ToDateTimeOffset( offset ) );
		}


		private static void AssertIsUnixEpocDateTime( DateTime actual, long millisecondsOffset )
		{
			Assert.That( actual.Kind, Is.EqualTo( DateTimeKind.Utc ) );
			var epoc = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
			Assert.That( actual, Is.EqualTo( epoc.AddMilliseconds( millisecondsOffset ) ) );
		}

		[Test]
		public void TestToDateTime_Zero_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTime( MessagePackConvert.ToDateTime( 0 ), 0 );
		}

		[Test]
		public void TestToDateTime_One_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTime( MessagePackConvert.ToDateTime( 1 ), 1 );
		}

		[Test]
		public void TestToDateTime_MinuOne_IsUtcEpoc()
		{
			AssertIsUnixEpocDateTime( MessagePackConvert.ToDateTime( -1 ), -1 );
		}

		[Test]
		public void TestToDateTime_Maximum_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MaxValue.Subtract( _utcEpoc ).TotalMilliseconds ) - 1L;
			AssertIsUnixEpocDateTime( MessagePackConvert.ToDateTime( offset ), offset );
		}

		[Test]
		public void TestToDateTime_Minimum_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MinValue.Subtract( _utcEpoc ).TotalMilliseconds );
			AssertIsUnixEpocDateTime( MessagePackConvert.ToDateTime( offset ), offset );
		}

		[Test]
		public void TestToDateTime_MaximumPlusOne_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MaxValue.Subtract( _utcEpoc ).TotalMilliseconds + 1L );
			Assert.Throws<ArgumentOutOfRangeException>( () => MessagePackConvert.ToDateTime( offset ) );
		}

		[Test]
		public void TestToDateTime_MinimumMinusOne_IsUtcEpoc()
		{
			var offset = checked( ( long )DateTime.MinValue.Subtract( _utcEpoc ).TotalMilliseconds - 1L );
			Assert.Throws<ArgumentOutOfRangeException>( () => MessagePackConvert.ToDateTime( offset ) );
		}


		[Test]
		public void TestFromDateTimeOffset_UtcNow_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )DateTime.UtcNow.Subtract( _utcEpoc ).TotalMilliseconds ),
				MessagePackConvert.FromDateTimeOffset( DateTimeOffset.UtcNow )
			);
		}

		[Test]
		public void TestFromDateTimeOffset_Now_AsUtcUnixEpoc()
		{
			// LocalTime will be converted to UtcTime
			Assert.AreEqual(
				checked( ( long )DateTime.UtcNow.Subtract( _utcEpoc ).TotalMilliseconds ),
				MessagePackConvert.FromDateTimeOffset( DateTimeOffset.Now )
			);
		}

		[Test]
		public void TestFromDateTimeOffset_UtcEpoc_Zero()
		{
			Assert.AreEqual(
				0L,
				MessagePackConvert.FromDateTimeOffset( new DateTimeOffset( 1970, 1, 1, 0, 0, 0, TimeSpan.Zero ) )
			);
		}

		[Test]
		public void TestFromDateTimeOffset_MinValue_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )( DateTimeOffset.MinValue.ToUniversalTime().Subtract( _utcEpoc ).TotalMilliseconds ) ),
				MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue.ToUniversalTime() )
			);
		}

		[Test]
		public void TestFromDateTimeOffset_MaxValue_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )( DateTimeOffset.MaxValue.ToUniversalTime().Subtract( _utcEpoc ).TotalMilliseconds ) ),
				MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue.ToUniversalTime() )
			);
		}


		[Test]
		public void TestFromDateTime_UtcNow_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )DateTime.UtcNow.Subtract( _utcEpoc ).TotalMilliseconds ),
				MessagePackConvert.FromDateTime( DateTime.UtcNow )
			);
		}

		[Test]
		public void TestFromDateTime_Now_AsUtcUnixEpoc()
		{
			// LocalTime will be converted to UtcTime
			var now = DateTime.Now;
			Assert.AreEqual(
				checked( ( long )now.ToUniversalTime().Subtract( _utcEpoc ).TotalMilliseconds ),
				MessagePackConvert.FromDateTime( now )
			);
		}

		[Test]
		public void TestFromDateTime_UtcEpoc_Zero()
		{
			Assert.AreEqual(
				0L,
				MessagePackConvert.FromDateTime( new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc ) )
			);
		}

		[Test]
		public void TestFromDateTime_MinValue_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )( DateTime.MinValue.ToUniversalTime().Subtract( _utcEpoc ).TotalMilliseconds ) ),
				MessagePackConvert.FromDateTime( DateTime.MinValue.ToUniversalTime() )
			);
		}

		[Test]
		public void TestFromDateTime_MaxValue_AsUnixEpoc()
		{
			Assert.AreEqual(
				checked( ( long )( DateTime.MaxValue.ToUniversalTime().Subtract( _utcEpoc ).TotalMilliseconds ) ),
				MessagePackConvert.FromDateTime( DateTime.MaxValue.ToUniversalTime() )
			);
		}
	}
}
