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
#if ( NET35 || SILVERLIGHT ) && !WINDOWS_UWP
using System.Threading;
#endif // ( NET35 || SILVERLIGHT ) && !WINDOWS_UWP
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
		public void TestParseExact_WithDateTimeStyles_Zero_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"0000-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62167219200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Zero_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"0000-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62167219200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_Zero_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"0000-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62167219200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Zero_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"0000-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62167219200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_Zero_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"0000-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62167219200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Zero_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"0000-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62167219200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_FullDigits_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -30585822590, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_FullDigits_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -30585822590, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_FullDigits_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -30585822590, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_FullDigits_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -30585822590, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_FullDigits_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -30585822590, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_FullDigits_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -30585822590, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-0001-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62198755200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-0001-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62198755200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-0001-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62198755200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-0001-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62198755200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-0001-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62198755200, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-0001-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62198755200, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1000_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-1000-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -93724128000, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1000_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-1000-01-01T00:00:00.000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -93724128000, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1000_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-1000-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -93724128000, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1000_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-1000-01-01T00:00:00.000000000Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -93724128000, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus1000_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-1000-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -93724128000, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus1000_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-1000-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -93724128000, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_Year10000_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"10000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 253426788610, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Year10000_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"10000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 253426788610, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_Year10000_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"10000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 253426788610, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Year10000_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"10000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 253426788610, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_Year10000_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"10000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 253426788610, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_Year10000_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"10000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 253426788610, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus10000_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-10000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -377712251390, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus10000_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-10000-10-10T10:10:10.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -377712251390, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus10000_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-10000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -377712251390, 123456789 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus10000_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-10000-10-10T10:10:10.123456789Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -377712251390, 123456789 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_YearMinus10000_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-10000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -377712251390, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_YearMinus10000_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-10000-10-10T10:10:10Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -377712251390, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMin_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-292277022657-01-27T08:29:52.000000000Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMin_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-292277022657-01-27T08:29:52.000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMin_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-292277022657-01-27T08:29:52.000000000Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMin_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-292277022657-01-27T08:29:52.000000000Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMin_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-292277022657-01-27T08:29:52Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMin_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-292277022657-01-27T08:29:52Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -9223372036854775808, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMax_LowerO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"292277026596-12-04T15:30:07.999999999Z",
					"o",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 9223372036854775807, 999999999 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMax_LowerO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"292277026596-12-04T15:30:07.999999999Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 9223372036854775807, 999999999 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMax_UpperO()
		{
			Assert.That(
				Timestamp.ParseExact(
					"292277026596-12-04T15:30:07.999999999Z",
					"O",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 9223372036854775807, 999999999 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMax_UpperO()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"292277026596-12-04T15:30:07.999999999Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 9223372036854775807, 999999999 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_TimestampMax_LowerS()
		{
			Assert.That(
				Timestamp.ParseExact(
					"292277026596-12-04T15:30:07Z",
					"s",
					null,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 9223372036854775807, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_TimestampMax_LowerS()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"292277026596-12-04T15:30:07Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 9223372036854775807, 0 ) ) );
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerO_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerO_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatUpperO_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatUpperO_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerS_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerS_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_LeadingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_LeadingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_LeadingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_LeadingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerO_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerO_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatUpperO_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatUpperO_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerS_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerS_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_TrailingWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_TrailingWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_TrailingWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_TrailingWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_None_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_None_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhite_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatUpperO_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowTrailingWhite_FormatLowerS_BothWhiteIsNotAllowed()
		{
			Assert.Throws<FormatException>( () =>
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowTrailingWhite
				)
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_BothWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerO_BothWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"o",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_BothWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatUpperO_BothWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01.000000000Z ",
					"O",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestTryParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_BothWhiteIsAllowed()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 1, 0 ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_AllowLeadingWhiteOrAllowTrailingWhite_FormatLowerS_BothWhiteIsAllowed()
		{
			Assert.That(
				Timestamp.ParseExact(
					" 1970-01-01T00:00:01Z ",
					"s",
					null,
					DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
				)
				, Is.EqualTo( new Timestamp( 1, 0 ) )
			);
		}

		[Test]
		public void TestParseExact_WithoutDateTimeStyles_AsNone()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					" 1970-01-01T00:00:00.123456789Z",
					"o",
					null
				)
			);
		}

		[Test]
		public void TestParseExact_IFormatProvider_Zero_o_InvariantCulture()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:00.000000000Z",
					"o",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 0, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_IFormatProvider_Zero_o_InvariantCulture()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.000000000Z",
					"o",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 0, 0 ) ) );
		}

		[Test]
		public void TestParseExact_IFormatProvider_Zero_s_InvariantCulture()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1970-01-01T00:00:00Z",
					"s",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( 0, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_IFormatProvider_Zero_s_InvariantCulture()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00Z",
					"s",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( 0, 0 ) ) );
		}

		[Test]
		public void TestParseExact_IFormatProvider_YearMinus1_o_InvariantCulture()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-0001-03-01T00:00:00.000000000Z",
					"o",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62193657600, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_IFormatProvider_YearMinus1_o_InvariantCulture()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-0001-03-01T00:00:00.000000000Z",
					"o",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
		}

		[Test]
		public void TestParseExact_IFormatProvider_YearMinus1_s_InvariantCulture()
		{
			Assert.That(
				Timestamp.ParseExact(
					"-0001-03-01T00:00:00Z",
					"s",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62193657600, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_IFormatProvider_YearMinus1_s_InvariantCulture()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-0001-03-01T00:00:00Z",
					"s",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
		}

		[Test]
		public void TestParseExact_Distinguishable_o_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1234-05-06T07:08:09.123456789Z",
					"o",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -23215049511, 123456789 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_Distinguishable_o_CustomCulture_UsedForNegativeSign()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1234-05-06T07:08:09.123456789Z",
					"o",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -23215049511, 123456789 ) ) );
		}

#if !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_Distinguishable_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Assert.That(
					Timestamp.ParseExact(
						"1234-05-06T07:08:09.123456789Z",
						"o",
						null,
						DateTimeStyles.None
					),
					Is.EqualTo( new Timestamp( -23215049511, 123456789 ) )
				);
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}
		
		[Test]
		public void TestTryParseExact_Distinguishable_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Timestamp result;
				Assert.That(
					Timestamp.TryParseExact(
						"1234-05-06T07:08:09.123456789Z",
						"o",
						null,
						DateTimeStyles.None,
						out result
					),
					Is.True
				);
				Assert.That( result, Is.EqualTo( new Timestamp( -23215049511, 123456789 ) ) );
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}

#endif // !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_Distinguishable_s_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				Timestamp.ParseExact(
					"1234-05-06T07:08:09Z",
					"s",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -23215049511, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_Distinguishable_s_CustomCulture_UsedForNegativeSign()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1234-05-06T07:08:09Z",
					"s",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -23215049511, 0 ) ) );
		}

#if !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_Distinguishable_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Assert.That(
					Timestamp.ParseExact(
						"1234-05-06T07:08:09Z",
						"s",
						null,
						DateTimeStyles.None
					),
					Is.EqualTo( new Timestamp( -23215049511, 0 ) )
				);
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}
		
		[Test]
		public void TestTryParseExact_Distinguishable_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Timestamp result;
				Assert.That(
					Timestamp.TryParseExact(
						"1234-05-06T07:08:09Z",
						"s",
						null,
						DateTimeStyles.None,
						out result
					),
					Is.True
				);
				Assert.That( result, Is.EqualTo( new Timestamp( -23215049511, 0 ) ) );
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}

#endif // !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_YearMinus1_o_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				Timestamp.ParseExact(
					"\uFF0D0001-03-01T00:00:00.000000000Z",
					"o",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62193657600, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_YearMinus1_o_CustomCulture_UsedForNegativeSign()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"\uFF0D0001-03-01T00:00:00.000000000Z",
					"o",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
		}

#if !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_YearMinus1_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Assert.That(
					Timestamp.ParseExact(
						"\uFF0D0001-03-01T00:00:00.000000000Z",
						"o",
						null,
						DateTimeStyles.None
					),
					Is.EqualTo( new Timestamp( -62193657600, 0 ) )
				);
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}
		
		[Test]
		public void TestTryParseExact_YearMinus1_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Timestamp result;
				Assert.That(
					Timestamp.TryParseExact(
						"\uFF0D0001-03-01T00:00:00.000000000Z",
						"o",
						null,
						DateTimeStyles.None,
						out result
					),
					Is.True
				);
				Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}

#endif // !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_YearMinus1_s_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				Timestamp.ParseExact(
					"\uFF0D0001-03-01T00:00:00Z",
					"s",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None
				),
				Is.EqualTo( new Timestamp( -62193657600, 0 ) )
			);
		}
		
		[Test]
		public void TestTryParseExact_YearMinus1_s_CustomCulture_UsedForNegativeSign()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"\uFF0D0001-03-01T00:00:00Z",
					"s",
					new LegacyJapaneseCultureInfo(),
					DateTimeStyles.None,
					out result
				),
				Is.True
			);
			Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
		}

#if !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_YearMinus1_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Assert.That(
					Timestamp.ParseExact(
						"\uFF0D0001-03-01T00:00:00Z",
						"s",
						null,
						DateTimeStyles.None
					),
					Is.EqualTo( new Timestamp( -62193657600, 0 ) )
				);
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}
		
		[Test]
		public void TestTryParseExact_YearMinus1_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					new LegacyJapaneseCultureInfo();

				if ( !( CultureInfo.CurrentCulture is LegacyJapaneseCultureInfo ) || CultureInfo.CurrentCulture.NumberFormat.NegativeSign != "\uFF0D" )
				{
					Assert.Ignore( "This platform does not support custom culture correctly." );
				}

				Timestamp result;
				Assert.That(
					Timestamp.TryParseExact(
						"\uFF0D0001-03-01T00:00:00Z",
						"s",
						null,
						DateTimeStyles.None,
						out result
					),
					Is.True
				);
				Assert.That( result, Is.EqualTo( new Timestamp( -62193657600, 0 ) ) );
			}
			finally
			{
#if ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				CultureInfo.CurrentCulture = 
#else // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
				Thread.CurrentThread.CurrentCulture =
#endif // ( !NET35 && !SILVERLIGHT ) || WINDOWS_UWP
					originalCurrentCulture;
			}
		}

#endif // !WINDOWS_PHONE && !WINDOWS_PHONE_APP && !NETFX_CORE

		[Test]
		public void TestParseExact_ParseError_EmptyValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_EmptyValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_EmptyValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_EmptyValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidYearValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"AAAA-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidYearValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"AAAA-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidYearValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"AAAA-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidYearValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"AAAA-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortYearDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"001-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortYearDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"001-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortYearDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"001-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortYearDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"001-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallYearValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"-584554047285-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallYearValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-584554047285-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallYearValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"-584554047285-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallYearValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"-584554047285-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeYearValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"584554051224-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeYearValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"584554051224-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeYearValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"584554051224-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeYearValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"584554051224-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidYearMonthDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970/01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidYearMonthDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970/01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidYearMonthDelimiter_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970/01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidYearMonthDelimiter_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970/01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMonthValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-AA-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMonthValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-AA-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMonthValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-AA-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMonthValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-AA-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortMonthDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-1-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortMonthDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-1-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortMonthDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-1-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortMonthDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-1-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallMonthValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"A-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallMonthValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"A-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallMonthValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"A-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallMonthValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"A-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeMonthValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"A-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeMonthValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"A-01-01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeMonthValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"A-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeMonthValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"A-01-01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMonthDayDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01/01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMonthDayDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01/01T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMonthDayDelimiter_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01/01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMonthDayDelimiter_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01/01T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidDayValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-AAT00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidDayValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-AAT00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidDayValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-AAT00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidDayValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-AAT00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortDayDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-1T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortDayDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-1T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortDayDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-1T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortDayDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-1T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeDayValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-32T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeDayValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-32T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeDayValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-32T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeDayValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-32T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallDayValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-00T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallDayValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-00T00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallDayValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-00T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallDayValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-00T00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidDayHourDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01_00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidDayHourDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01_00:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidDayHourDelimiter_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01_00:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidDayHourDelimiter_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01_00:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidHourValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01TAA:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidHourValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01TAA:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidHourValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01TAA:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidHourValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01TAA:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortHourDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T0:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortHourDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T0:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortHourDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T0:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortHourDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T0:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeHourValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T24:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeHourValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T24:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeHourValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T24:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeHourValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T24:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallHourValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T-01:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallHourValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T-01:00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallHourValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T-01:00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallHourValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T-01:00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidHourMinuteDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00-00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidHourMinuteDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00-00:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidHourMinuteDelimiter_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00-00:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidHourMinuteDelimiter_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00-00:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMinuteValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:AA:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMinuteValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:AA:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMinuteValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:AA:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMinuteValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:AA:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortMinuteDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:0:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortMinuteDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:0:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortMinuteDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:0:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortMinuteDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:0:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeMinuteValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:60:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeMinuteValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:60:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeMinuteValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:60:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeMinuteValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:60:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallMinuteValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:-01:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallMinuteValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:-01:00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallMinuteValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:-01:00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallMinuteValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:-01:00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMinuteSecondDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00-00.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMinuteSecondDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00-00.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidMinuteSecondDelimiter_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00-00Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidMinuteSecondDelimiter_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00-00Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidSecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:AA.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidSecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:AA.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidSecondValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:AAZ",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidSecondValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:AAZ",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortSecondDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:0.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortSecondDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:0.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortSecondDigit_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:0Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortSecondDigit_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:0Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeSecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:60.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeSecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:60.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeSecondValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:60Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeSecondValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:60Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallSecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:-01.123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallSecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:-01.123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallSecondValue_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:-01Z",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallSecondValue_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:-01Z",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidNanosecondDelimiter_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00_123456789Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidNanosecondDelimiter_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00_123456789Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_InvalidNanosecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00.AAABBBCCCZ",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_InvalidNanosecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.AAABBBCCCZ",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooShortNanosecondDigit_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00.12345678Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooShortNanosecondDigit_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.12345678Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooLargeNanosecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00.1000000000Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooLargeNanosecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.1000000000Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_ParseError_TooSmallNanosecondValue_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00.-000000001Z",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_TooSmallNanosecondValue_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.-000000001Z",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}


		[Test]
		public void TestParseExact_ParseError_MissingUtcSign_o()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00.123456789",
					"o",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_MissingUtcSign_o()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00.123456789",
					"o",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}


		[Test]
		public void TestParseExact_ParseError_MissingUtcSign_s()
		{
			Assert.Throws<FormatException>(
				() => Timestamp.ParseExact(
					"1970-01-01T00:00:00",
					"s",
					null,
					DateTimeStyles.None
				)
			);
		}

		[Test]
		public void TestTryParseExact_ParseError_MissingUtcSign_s()
		{
			Timestamp result;
			Assert.That(
				Timestamp.TryParseExact(
					"1970-01-01T00:00:00",
					"s",
					null,
					DateTimeStyles.None,
					out result
				),
				Is.False
			);
			Assert.That( result, Is.EqualTo( default( Timestamp ) ) );
		}

		[Test]
		public void TestParseExact_WithDateTimeStyles_NullValue()
		{
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.ParseExact( null, "o", CultureInfo.InvariantCulture, DateTimeStyles.None )
			);
		}

		[Test]
		public void TestParseExact_WithoutDateTimeStyles_NullValue()
		{
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.ParseExact( null, "o", CultureInfo.InvariantCulture )
			);
		}

		[Test]
		public void TestTryParseExact_NullValue()
		{
			Timestamp result;
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.TryParseExact( null, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out result )
			);
		}
		[Test]
		public void TestParseExact_WithDateTimeStyles_NullFormat()
		{
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.ParseExact( null, "o", CultureInfo.InvariantCulture, DateTimeStyles.None )
			);
		}

		[Test]
		public void TestParseExact_WithoutDateTimeStyles_NullFormat()
		{
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.ParseExact( null, "o", CultureInfo.InvariantCulture )
			);
		}

		[Test]
		public void TestTryParseExact_NullFormat()
		{
			Timestamp result;
			Assert.Throws<ArgumentNullException>(
				() => Timestamp.TryParseExact( null, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out result )
			);
		}
		[Test]
		public void TestParseExact_WithDateTimeStyles_UnsupportedFormat()
		{
			Assert.Throws<ArgumentException>(
				() => Timestamp.ParseExact( "1970-01-01T00:00:00", "G", CultureInfo.InvariantCulture, DateTimeStyles.None )
			);
		}

		[Test]
		public void TestParseExact_WithoutDateTimeStyles_UnsupportedFormat()
		{
			Assert.Throws<ArgumentException>(
				() => Timestamp.ParseExact( "1970-01-01T00:00:00", "G", CultureInfo.InvariantCulture )
			);
		}

		[Test]
		public void TestTryParseExact_UnsupportedFormat()
		{
			Timestamp result;
			Assert.Throws<ArgumentException>(
				() => Timestamp.TryParseExact( "1970-01-01T00:00:00", "G", CultureInfo.InvariantCulture, DateTimeStyles.None, out result )
			);
		}
		[Test]
		public void TestParseExact_WithDateTimeStyles_InvalidStyles()
		{
			Assert.Throws<ArgumentException>(
				() => Timestamp.ParseExact( "1970-01-01T00:00:00", "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal )
			);
		}

		[Test]
		public void TestTryParseExact_InvalidStyles()
		{
			Timestamp result;
			Assert.Throws<ArgumentException>(
				() => Timestamp.TryParseExact( "1970-01-01T00:00:00", "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result )
			);
		}

		[Test]
		public void TestParseExact_DatesAD()
		{
			var seconds = -62135596800L;
			foreach ( var year in Enumerable.Range( 1, 802 ) )
			{
				var isLeapYear = year % 400 == 0 || ( year % 4 == 0 && year % 100 != 0 );
				foreach ( var dayOfYear in Enumerable.Range( 1, isLeapYear ? 366 : 365 ) )
				{
					var expected = new DateTimeOffset( year, 1, 1, 0, 0, 0, TimeSpan.Zero ).AddDays( dayOfYear - 1 );
					var target = Timestamp.ParseExact( String.Format( CultureInfo.InvariantCulture, "{0:yyyy-MM-dd'T'HH:mm:ss}.000000000Z", expected ), "o", null );
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
		public void TestParseExact_DatesBC()
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

					var target = Timestamp.ParseExact( String.Format( CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}T00:00:00.000000000Z", year, month, day ), "o", null );
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
