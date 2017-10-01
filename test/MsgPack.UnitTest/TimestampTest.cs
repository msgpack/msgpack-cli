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
	public partial class TimestampTest
	{
		private static readonly int[] DaysInMonthsInNonLeapYear =
			new[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
		private static readonly int[] DaysInMonthsInLeapYear =
			new[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

		[Test]
		public void TestZero_AllZero()
		{
			var target = Timestamp.Zero;
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestMaxValue_AllMax()
		{
			var target = Timestamp.MaxValue;
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( Int64.MaxValue ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 999999999 ) );
		}

		[Test]
		public void TestMinValue_AllMin()
		{
			var target = Timestamp.MinValue;
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( Int64.MinValue ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestConstractor_AsIs()
		{
			var unixEpochSeconds = DateTime.UtcNow.Ticks;
			var nanoseconds = DateTime.UtcNow.Millisecond;
			var target = new Timestamp( unixEpochSeconds, nanoseconds );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( unixEpochSeconds ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( nanoseconds ) );
		}

		[Test]
		public void TestConstractor_Max_Ok()
		{
			var target = new Timestamp( Int64.MaxValue, 999999999 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( Timestamp.MaxValue.UnixEpochSecondsPart ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( Timestamp.MaxValue.NanosecondsPart ) );
		}

		[Test]
		public void TestConstractor_Min_Ok()
		{
			var target = new Timestamp( Int64.MinValue, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( Timestamp.MinValue.UnixEpochSecondsPart ) );
			Assert.That( target.NanosecondsPart, Is.EqualTo( Timestamp.MinValue.NanosecondsPart ) );
		}

		[Test]
		public void TestConstractor_TooLargeNanoseconds_Exception()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => new Timestamp( 0, 999999999 + 1 ) );
		}

		[Test]
		public void TestConstractor_TooSmallNanoseconds_Exception()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => new Timestamp( 0, -1 ) );
		}
		
		[Test]
		public void TestGetHashCode_Zero()
		{
			// Just check no exceptions thrown.
			Timestamp.Zero.GetHashCode();
		}

		[Test]
		public void TestGetHashCode_MaxValue()
		{
			// Just check no exceptions thrown.
			Timestamp.MaxValue.GetHashCode();
		}

		[Test]
		public void TestGetHashCode_MinValue()
		{
			// Just check no exceptions thrown.
			Timestamp.MinValue.GetHashCode();
		}

		private static void AssertUtc( DateTime value )
		{
			Assert.That( value.Kind, Is.EqualTo( DateTimeKind.Utc ) );
		}

		private static void AssertUtc( DateTimeOffset value )
		{
			Assert.That( value.Offset, Is.EqualTo( TimeSpan.Zero ) );
		}
	}
}
