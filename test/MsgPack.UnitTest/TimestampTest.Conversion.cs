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
	partial class TimestampTest
	{
		[Test]
		public void TestFromDateTime_UtcNow_OK()
		{
			var source = DateTime.UtcNow;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTime_UtcNow_OK()
		{
			var source = DateTime.UtcNow;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_MinValue_OK()
		{
			var source = DateTime.MinValue;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTime_MinValue_OK()
		{
			var source = DateTime.MinValue;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_MaxValue_OK()
		{
			var source = DateTime.MaxValue;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTime_MaxValue_OK()
		{
			var source = DateTime.MaxValue;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTime_UtcNow_OK()
		{
			var source = Timestamp.UtcNow;
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestExplicitDateTime_UtcNow_OK()
		{
			var source = Timestamp.UtcNow;
			var target = ( DateTime )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestFromDateTime_Now_AsUtc()
		{
			var source = DateTime.Now;
			var target = Timestamp.FromDateTime( source );
			var expected = source.ToUniversalTime();
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTime_Now_AsUtc()
		{
			var source = DateTime.Now;
			var target = ( Timestamp )source;
			var expected = source.ToUniversalTime();
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTime_OverflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1L, checked( ( int )( DateTimeOffset.MaxValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTime() );
		}

		[Test]
		public void TestToDateTime_OverflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1, 0 );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTime() );
		}

		[Test]
		public void TestExplicitDateTime_OverflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1L, checked( ( int )( DateTimeOffset.MaxValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTime )source; } );
		}

		[Test]
		public void TestExplicitDateTime_OverflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1, 0 );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTime )source; } );
		}

		[Test]
		public void TestToDateTime_UnderflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1L, checked( ( int )( DateTimeOffset.MinValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTime() );
		}

		[Test]
		public void TestToDateTime_UnderflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) -1L, 999999999 );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTime() );
		}

		[Test]
		public void TestExplicitDateTime_UnderflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1L, checked( ( int )( DateTimeOffset.MinValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTime )source; } );
		}

		[Test]
		public void TestExplicitDateTime_UnderflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1, 999999999 );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTime )source; } );
		}

		private static void AssertSubseconds( DateTime target, Timestamp expected )
		{
			var ticks = target.Ticks % 10000;
			Assert.That( ticks / 10, Is.EqualTo( expected.Microsecond ), "Microsecond" );
			Assert.That( ticks % 10, Is.EqualTo( expected.Nanosecond / 100 ), "Nanosecond" );
		}

		private static void AssertSubseconds( Timestamp target, DateTime expected )
		{
			var ticks = expected.Ticks % 10000;
			Assert.That( target.Microsecond, Is.EqualTo( ticks / 10 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( ( ticks % 10 ) * 100 ), "Nanosecond" );
		}

		[Test]
		public void TestFromDateTimeOffset_UtcNow_OK()
		{
			var source = DateTimeOffset.UtcNow;
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTimeOffset_UtcNow_OK()
		{
			var source = DateTimeOffset.UtcNow;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_MinValue_OK()
		{
			var source = DateTimeOffset.MinValue;
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTimeOffset_MinValue_OK()
		{
			var source = DateTimeOffset.MinValue;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_MaxValue_OK()
		{
			var source = DateTimeOffset.MaxValue;
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTimeOffset_MaxValue_OK()
		{
			var source = DateTimeOffset.MaxValue;
			var target = ( Timestamp )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTimeOffset_UtcNow_OK()
		{
			var source = Timestamp.UtcNow;
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestExplicitDateTimeOffset_UtcNow_OK()
		{
			var source = Timestamp.UtcNow;
			var target = ( DateTimeOffset )source;
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestFromDateTimeOffset_Now_AsUtc()
		{
			var source = DateTimeOffset.Now;
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source.ToUniversalTime();
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestImplicitDateTimeOffset_Now_AsUtc()
		{
			var source = DateTimeOffset.Now;
			var target = ( Timestamp )source;
			var expected = source.ToUniversalTime();
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTimeOffset_OverflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1L, checked( ( int )( DateTimeOffset.MaxValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTimeOffset() );
		}

		[Test]
		public void TestToDateTimeOffset_OverflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1, 0 );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTimeOffset() );
		}

		[Test]
		public void TestExplicitDateTimeOffset_OverflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1L, checked( ( int )( DateTimeOffset.MaxValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTimeOffset )source; } );
		}

		[Test]
		public void TestExplicitDateTimeOffset_OverflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MaxValue ) + 1, 0 );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTimeOffset )source; } );
		}

		[Test]
		public void TestToDateTimeOffset_UnderflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1L, checked( ( int )( DateTimeOffset.MinValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTimeOffset() );
		}

		[Test]
		public void TestToDateTimeOffset_UnderflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) -1L, 999999999 );
			Assert.Throws<InvalidOperationException>( () => source.ToDateTimeOffset() );
		}

		[Test]
		public void TestExplicitDateTimeOffset_UnderflowSeconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1L, checked( ( int )( DateTimeOffset.MinValue.Ticks % 10000000 * 100 ) ) );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTimeOffset )source; } );
		}

		[Test]
		public void TestExplicitDateTimeOffset_UnderflowNanoseconds()
		{
			var source = new Timestamp( MessagePackConvert.FromDateTimeOffset( DateTimeOffset.MinValue ) - 1, 999999999 );
			Assert.Throws<InvalidOperationException>( () => { var x = ( DateTimeOffset )source; } );
		}

		private static void AssertSubseconds( DateTimeOffset target, Timestamp expected )
		{
			var ticks = target.Ticks % 10000;
			Assert.That( ticks / 10, Is.EqualTo( expected.Microsecond ), "Microsecond" );
			Assert.That( ticks % 10, Is.EqualTo( expected.Nanosecond / 100 ), "Nanosecond" );
		}

		private static void AssertSubseconds( Timestamp target, DateTimeOffset expected )
		{
			var ticks = expected.Ticks % 10000;
			Assert.That( target.Microsecond, Is.EqualTo( ticks / 10 ), "Microsecond" );
			Assert.That( target.Nanosecond, Is.EqualTo( ( ticks % 10 ) * 100 ), "Nanosecond" );
		}

#if !NET35 && !SILVERLIGHT && !NETSTANDARD1_1

		[Test]
		public void TestFromDateTime_UnixEpoc_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( 0 ).DateTime;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_UnixEpocMinus1Millisecond_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( -1 ).DateTime;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_UnixEpocPlus1Millisecond_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( 1 ).DateTime;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_UnixEpocMinusSecond_OK()
		{
			var source = DateTimeOffset.FromUnixTimeSeconds( -1 ).DateTime;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTime_UnixEpocPlus1Second_OK()
		{
			var source = DateTimeOffset.FromUnixTimeSeconds( 1 ).DateTime;
			var target = Timestamp.FromDateTime( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTime_UnixEpoc_OK()
		{
			var source = default( Timestamp );
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTime_UnixEpocMinus1Millisecond_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromMilliseconds( -1 ) );
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTime_UnixEpocPlus1Millisecond_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromMilliseconds( 1 ) );
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTime_UnixEpocMinus1Second_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromSeconds( -1 ) );
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTime_UnixEpocPlus1Second_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromSeconds( 1 ) );
			var target = source.ToDateTime();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestFromDateTimeOffset_UnixEpoc_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( 0 );
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_UnixEpocMinus1Millisecond_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( -1 );
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_UnixEpocPlus1Millisecond_OK()
		{
			var source = DateTimeOffset.FromUnixTimeMilliseconds( 1 );
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_UnixEpocMinus1Second_OK()
		{
			var source = DateTimeOffset.FromUnixTimeSeconds( -1 );
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestFromDateTimeOffset_UnixEpocPlus1Second_OK()
		{
			var source = DateTimeOffset.FromUnixTimeSeconds( 1 );
			var target = Timestamp.FromDateTimeOffset( source );
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
		}

		[Test]
		public void TestToDateTimeOffset_UnixEpoc_OK()
		{
			var source = default( Timestamp );
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTimeOffset_UnixEpocMinus1Millisecond_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromMilliseconds( -1 ) );
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTimeOffset_UnixEpocPlus1Millisecond_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromMilliseconds( 1 ) );
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTimeOffset_UnixEpocMinus1Second_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromSeconds( -1 ) );
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

		[Test]
		public void TestToDateTimeOffset_UnixEpocPlus1Second_OK()
		{
			var source = default( Timestamp ).Add( TimeSpan.FromSeconds( 1 ) );
			var target = source.ToDateTimeOffset();
			var expected = source;
			Assert.That( target.Year, Is.EqualTo( expected.Year ), "Year" );
			Assert.That( target.Month, Is.EqualTo( expected.Month ), "Month" );
			Assert.That( target.Day, Is.EqualTo( expected.Day ), "Day" );
			Assert.That( target.DayOfYear, Is.EqualTo( expected.DayOfYear ), "DayOfYear" );
			Assert.That( target.DayOfWeek, Is.EqualTo( expected.DayOfWeek ), "DayOfWeek" );
			Assert.That( target.Hour, Is.EqualTo( expected.Hour ), "Hour" );
			Assert.That( target.Minute, Is.EqualTo( expected.Minute ), "Minute" );
			Assert.That( target.Second, Is.EqualTo( expected.Second ), "Second" );
			Assert.That( target.Millisecond, Is.EqualTo( expected.Millisecond ), "Millisecond" );
			AssertSubseconds( target, expected );
			AssertUtc( target );
		}

#endif // NET35 && !SILVERLIGHT && !NETSTANDARD1_1
	}
}
