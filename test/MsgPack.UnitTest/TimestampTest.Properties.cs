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
using System.Linq;
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
		public void TestProperties_Zero()
		{
			// 1970-01-01T00:00:00.000000000Z
			var target = new Timestamp( 0, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 0 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1970 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Minus1Sec()
		{
			// 1969-12-31T23:59:59.000000000Z
			var target = new Timestamp( -1, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -1 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1969 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 365 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Plus1Sec()
		{
			// 1970-01-01T00:00:01.000000000Z
			var target = new Timestamp( 1, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 1 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1970 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 1 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Minus1Nsec()
		{
			// 1969-12-31T23:59:59.999999999Z
			var target = new Timestamp( -1, 999999999 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -1 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 999999999 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1969 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 999 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 999 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 999 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 365 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Plus1Nsec()
		{
			// 1970-01-01T00:00:00.000000001Z
			var target = new Timestamp( 0, 1 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 0 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 1 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1970 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 1 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMin()
		{
			// 0001-01-01T00:00:00.000000000Z
			var target = new Timestamp( -62135596800, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62135596800 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMax()
		{
			// 9999-12-31T23:59:59.000000000Z
			var target = new Timestamp( 253402300799, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253402300799 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 9999 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 365 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Friday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMinMinus1Day()
		{
			// 0000-12-31T00:00:00.000000000Z
			var target = new Timestamp( -62135683200, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62135683200 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 0 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 366 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Sunday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMaxPlus1Day()
		{
			// 10000-01-01T23:59:59.000000000Z
			var target = new Timestamp( 253402387199, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253402387199 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10000 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Saturday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMinMinus1Sec()
		{
			// 0000-12-31T23:59:59.000000000Z
			var target = new Timestamp( -62135596801, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62135596801 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 0 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 366 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Sunday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMaxPlus1Sec()
		{
			// 10000-01-01T00:00:00.000000000Z
			var target = new Timestamp( 253402300800, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253402300800 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10000 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 1 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Saturday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMinMinus1Nsec()
		{
			// 0000-12-31T23:59:59.999999999Z
			var target = new Timestamp( -62135596801, 999999999 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62135596801 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 999999999 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 0 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 999 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 999 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 999 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 366 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Sunday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_DateTimeMaxPlus1Nsec()
		{
			// 9999-12-31T23:59:59.000000001Z
			var target = new Timestamp( 253402300799, 1 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253402300799 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 1 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 9999 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 31 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 23 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 59 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 59 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 1 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 365 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Friday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_TimestampMin()
		{
			// -292277022657-01-27T08:29:52.000000000Z
			var target = new Timestamp( -9223372036854775808, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -9223372036854775808 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -292277022657 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 1 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 27 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 8 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 29 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 52 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 27 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Saturday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_TimestampMax()
		{
			// 292277026596-12-04T15:30:07.999999999Z
			var target = new Timestamp( 9223372036854775807, 999999999 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 9223372036854775807 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 999999999 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 292277026596 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 12 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 4 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 15 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 30 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 7 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 999 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 999 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 999 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 339 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Distinguishable()
		{
			// 1234-05-06T07:08:09.123456789Z
			var target = new Timestamp( -23215049511, 123456789 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -23215049511 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 123456789 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1234 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 5 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 6 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 7 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 8 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 9 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 123 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 456 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 789 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 126 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Saturday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year0()
		{
			// 0000-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62162035200, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62162035200 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 0 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year1()
		{
			// 0001-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62130499200, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62130499200 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 1 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year4()
		{
			// 0004-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62035804800, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62035804800 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 4 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year100()
		{
			// 0100-03-01T00:00:00.000000000Z
			var target = new Timestamp( -59006361600, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -59006361600 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 100 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year400()
		{
			// 0400-03-01T00:00:00.000000000Z
			var target = new Timestamp( -49539254400, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -49539254400 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 400 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year10000()
		{
			// 10000-03-01T00:00:00.000000000Z
			var target = new Timestamp( 253407484800, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253407484800 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10000 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year10001()
		{
			// 10001-03-01T00:00:00.000000000Z
			var target = new Timestamp( 253439020800, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 253439020800 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10001 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year10100()
		{
			// 10100-03-01T00:00:00.000000000Z
			var target = new Timestamp( 256563158400, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 256563158400 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10100 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_Year10400()
		{
			// 10400-03-01T00:00:00.000000000Z
			var target = new Timestamp( 266030265600, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( 266030265600 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( 10400 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus1()
		{
			// -0001-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62193657600, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62193657600 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -1 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Monday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus3()
		{
			// -0003-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62256729600, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62256729600 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -3 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Saturday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus4()
		{
			// -0004-03-01T00:00:00.000000000Z
			var target = new Timestamp( -62288265600, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -62288265600 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -4 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Friday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus99()
		{
			// -0099-03-01T00:00:00.000000000Z
			var target = new Timestamp( -65286259200, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -65286259200 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -99 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Friday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus100()
		{
			// -0100-03-01T00:00:00.000000000Z
			var target = new Timestamp( -65317795200, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -65317795200 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -100 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus399()
		{
			// -0399-03-01T00:00:00.000000000Z
			var target = new Timestamp( -74753280000, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -74753280000 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -399 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 60 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Thursday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( false ), "IsLeapYear" );
		}

		[Test]
		public void TestProperties_YearMinus400()
		{
			// -0400-03-01T00:00:00.000000000Z
			var target = new Timestamp( -74784816000, 0 );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( -74784816000 ), "UnixEpochSecondsPart" );
			Assert.That( target.NanosecondsPart, Is.EqualTo( 0 ), "NanosecondsPart" );

			Assert.That( target.Year, Is.EqualTo( -400 ), "Year" );
			Assert.That( target.Month, Is.EqualTo( 3 ), "Month" );
			Assert.That( target.Day, Is.EqualTo( 1 ), "Day" );
			Assert.That( target.Hour, Is.EqualTo( 0 ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( 0 ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( 0 ), "Second" );

			Assert.That( target.Millisecond, Is.EqualTo( 0 ), "Millisecond" );
			Assert.That( target.Microsecond, Is.EqualTo( 0 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( 0 ), "Nanosecond" );

			Assert.That( target.DayOfYear, Is.EqualTo( 61 ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( DayOfWeek.Wednesday ), "DayOfWeek" );
			Assert.That( target.IsLeapYear, Is.EqualTo( true ), "IsLeapYear" );
		}

		[Test]
		public void TestUtcNow()
		{
			var before = DateTimeOffset.UtcNow;
			var target = Timestamp.UtcNow;
			var after = DateTimeOffset.UtcNow;
			// Assert before <= now <= after
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( MessagePackConvert.FromDateTimeOffset( before ) / 1000 ).Or.GreaterThan( MessagePackConvert.FromDateTimeOffset( before ) ) );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( MessagePackConvert.FromDateTimeOffset( after ) / 1000 ).Or.LessThan( MessagePackConvert.FromDateTimeOffset( after ) ) );
		}

		[Test]
		public void TestToday()
		{
			var before = new DateTimeOffset( DateTimeOffset.UtcNow.Date, TimeSpan.Zero );
			var target = Timestamp.Today;
			var after = new DateTimeOffset( DateTimeOffset.UtcNow.Date, TimeSpan.Zero );
			// Assert before <= today <= after
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( MessagePackConvert.FromDateTimeOffset( before ) / 1000 ).Or.GreaterThan( MessagePackConvert.FromDateTimeOffset( before ) ) );
			Assert.That( target.UnixEpochSecondsPart, Is.EqualTo( MessagePackConvert.FromDateTimeOffset( after ) / 1000 ).Or.LessThan( MessagePackConvert.FromDateTimeOffset( after ) ) );
		}

		[Test]
		public void TestProperties_DatesAD()
		{
			var seconds = -62135596800L;
			foreach ( var year in Enumerable.Range( 1, 802 ) )
			{
				var isLeapYear = year % 400 == 0 || ( year % 4 == 0 && year % 100 != 0 );
				foreach ( var dayOfYear in Enumerable.Range( 1, isLeapYear ? 366 : 365 ) )
				{
					var target = new Timestamp( seconds, 0 );
					var expected = new DateTimeOffset( year, 1, 1, 0, 0, 0, TimeSpan.Zero ).AddDays( dayOfYear - 1 );
					Assert.That( expected.DayOfYear, Is.EqualTo( dayOfYear ), "{0:yyyy-MM-dd}", expected );
					Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "{0:yyyy-MM-dd}", expected );
					Assert.That( target.Year, Is.EqualTo( expected.Year ), "{0:yyyy-MM-dd}", expected );
					Assert.That( target.Month, Is.EqualTo( expected.Month ), "{0:yyyy-MM-dd}", expected );
					Assert.That( target.Day, Is.EqualTo( expected.Day ), "{0:yyyy-MM-dd}", expected );
					Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "{0:yyyy-MM-dd}", expected );

					seconds += 24 * 60 * 60;
				}
			}
		}

		[Test]
		public void TestProperties_DatesBC()
		{
			var seconds = -62135596800L;
			var dayOfWeek = ( long )DateTimeOffset.MinValue.DayOfWeek;

			foreach ( var year in Enumerable.Range( 1, 802 ).Select( x => 1 - x ) )
			{
				var isLeapYear = year % 400 == 0 || ( year % 4 == 0 && year % 100 != 0 );
				var month = 12;
				var day = 31;
				var daysInMonths = isLeapYear ? DaysInMonthsInLeapYear : DaysInMonthsInNonLeapYear;

				foreach ( var dayOfYear in Enumerable.Range( 1, isLeapYear ? 366 : 365 ).Reverse() )
				{
					seconds -= 24 * 60 * 60;
					dayOfWeek--;
					if ( dayOfWeek < 0 )
					{
						dayOfWeek = 6;
					}

					var target = new Timestamp( seconds, 0 );
					Assert.That( target.DayOfYear, Is.EqualTo( dayOfYear ), "{0:0000}-{1:00}-{2:00}", year, month, day );
					Assert.That( target.Year, Is.EqualTo( year ), "{0:0000}-{1:00}-{2:00}", year, month, day );
					Assert.That( target.Month, Is.EqualTo( month ), "{0:0000}-{1:00}-{2:00}", year, month, day );
					Assert.That( target.Day, Is.EqualTo( day ), "{0:0000}-{1:00}-{2:00}", year, month, day );
					Assert.That( target.DayOfWeek, Is.EqualTo( ( DayOfWeek )dayOfWeek ), "{0:0000}-{1:00}-{2:00}", year, month, day );

					if ( day == 1 )
					{
						month--;
						day = daysInMonths[ month ];
					}
					else
					{
						day--;
					}
				}
			}
		}
	}
}
