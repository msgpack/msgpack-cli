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
#if NET35
using System.Threading;
#endif // NET35
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
		public void TestToStringCore_Zero_LowerO()
		{
			var value =
				new Timestamp.Value(
					0,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "0000-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_Zero_UpperO()
		{
			var value =
				new Timestamp.Value(
					0,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "0000-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_Zero_LowerS()
		{
			var value =
				new Timestamp.Value(
					0,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "0000-01-01T00:00:00Z" )
			);
		}

		[Test]
		public void TestToStringCore_FullDigits_LowerO()
		{
			var value =
				new Timestamp.Value(
					1000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "1000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_FullDigits_UpperO()
		{
			var value =
				new Timestamp.Value(
					1000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "1000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_FullDigits_LowerS()
		{
			var value =
				new Timestamp.Value(
					1000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "1000-10-10T10:10:10Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1_LowerO()
		{
			var value =
				new Timestamp.Value(
					-1,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-0001-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1_UpperO()
		{
			var value =
				new Timestamp.Value(
					-1,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-0001-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1_LowerS()
		{
			var value =
				new Timestamp.Value(
					-1,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-0001-01-01T00:00:00Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1000_LowerO()
		{
			var value =
				new Timestamp.Value(
					-1000,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-1000-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1000_UpperO()
		{
			var value =
				new Timestamp.Value(
					-1000,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-1000-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus1000_LowerS()
		{
			var value =
				new Timestamp.Value(
					-1000,
					1,
					1,
					0,
					0,
					0,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-1000-01-01T00:00:00Z" )
			);
		}

		[Test]
		public void TestToStringCore_Year10000_LowerO()
		{
			var value =
				new Timestamp.Value(
					10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "10000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_Year10000_UpperO()
		{
			var value =
				new Timestamp.Value(
					10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "10000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_Year10000_LowerS()
		{
			var value =
				new Timestamp.Value(
					10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "10000-10-10T10:10:10Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus10000_LowerO()
		{
			var value =
				new Timestamp.Value(
					-10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-10000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus10000_UpperO()
		{
			var value =
				new Timestamp.Value(
					-10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-10000-10-10T10:10:10.123456789Z" )
			);
		}

		[Test]
		public void TestToStringCore_YearMinus10000_LowerS()
		{
			var value =
				new Timestamp.Value(
					-10000,
					10,
					10,
					10,
					10,
					10,
					123456789
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-10000-10-10T10:10:10Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMin_LowerO()
		{
			var value =
				new Timestamp.Value(
					-584554047284,
					2,
					23,
					16,
					59,
					44,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-584554047284-02-23T16:59:44.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMin_UpperO()
		{
			var value =
				new Timestamp.Value(
					-584554047284,
					2,
					23,
					16,
					59,
					44,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-584554047284-02-23T16:59:44.000000000Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMin_LowerS()
		{
			var value =
				new Timestamp.Value(
					-584554047284,
					2,
					23,
					16,
					59,
					44,
					0
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "-584554047284-02-23T16:59:44Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMax_LowerO()
		{
			var value =
				new Timestamp.Value(
					584554051223,
					11,
					9,
					7,
					0,
					16,
					999999999
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"o",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "584554051223-11-09T07:00:16.999999999Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMax_UpperO()
		{
			var value =
				new Timestamp.Value(
					584554051223,
					11,
					9,
					7,
					0,
					16,
					999999999
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"O",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "584554051223-11-09T07:00:16.999999999Z" )
			);
		}

		[Test]
		public void TestToStringCore_TimestampMax_LowerS()
		{
			var value =
				new Timestamp.Value(
					584554051223,
					11,
					9,
					7,
					0,
					16,
					999999999
				);
			Assert.That(
				TimestampStringConverter.ToString(
					"s",
					CultureInfo.InvariantCulture,
					ref value
				),
				Is.EqualTo( "584554051223-11-09T07:00:16Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_o_InvariantCulture()
		{
			Assert.That(
				new Timestamp(
					-23215049511,
					123456789
				).ToString( "o", CultureInfo.InvariantCulture ),
				Is.EqualTo( "1234-05-06T07:08:09.123456789Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_s_InvariantCulture()
		{
			Assert.That(
				new Timestamp(
					-23215049511,
					123456789
				).ToString( "s", CultureInfo.InvariantCulture ),
				Is.EqualTo( "1234-05-06T07:08:09Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_null_InvariantCulture_FormatIsO()
		{
			Assert.That(
				new Timestamp(
					-23215049511,
					123456789
				).ToString( null, CultureInfo.InvariantCulture ),
				Is.EqualTo( "1234-05-06T07:08:09.123456789Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_o_InvariantCulture()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( "o", CultureInfo.InvariantCulture ),
				Is.EqualTo( "-0001-03-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_s_InvariantCulture()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( "s", CultureInfo.InvariantCulture ),
				Is.EqualTo( "-0001-03-01T00:00:00Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_null_InvariantCulture_FormatIsO()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( null, CultureInfo.InvariantCulture ),
				Is.EqualTo( "-0001-03-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_o_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				new Timestamp(
					-23215049511,
					123456789
				).ToString( "o", new LegacyJapaneseCultureInfo() ),
				Is.EqualTo( "1234-05-06T07:08:09.123456789Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-23215049511,
						123456789
					).ToString( "o", null ),
					Is.EqualTo( "1234-05-06T07:08:09.123456789Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_s_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				new Timestamp(
					-23215049511,
					123456789
				).ToString( "s", new LegacyJapaneseCultureInfo() ),
				Is.EqualTo( "1234-05-06T07:08:09Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_Distinguishable_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-23215049511,
						123456789
					).ToString( "s", null ),
					Is.EqualTo( "1234-05-06T07:08:09Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_o_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( "o", new LegacyJapaneseCultureInfo() ),
				Is.EqualTo( "Å|0001-03-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_o_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-62193657600,
						0
					).ToString( "o", null ),
					Is.EqualTo( "Å|0001-03-01T00:00:00.000000000Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_s_CustomCulture_UsedForNegativeSign()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( "s", new LegacyJapaneseCultureInfo() ),
				Is.EqualTo( "Å|0001-03-01T00:00:00Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_YearMinus1_s_null_CurrentCultureIsUsed()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-62193657600,
						0
					).ToString( "s", null ),
					Is.EqualTo( "Å|0001-03-01T00:00:00Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}

		[Test]
		public void TestToString_String_IFormatProvider_DefaultIsOk()
		{
			Assert.That(
				default( Timestamp ).ToString( null, null ),
				Is.EqualTo( "1970-01-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_EmptyFormat()
		{
			Assert.Throws<ArgumentException>(
				() => default( Timestamp ).ToString( String.Empty, null )
			);
		}

		[Test]
		public void TestToString_String_IFormatProvider_UnsupportedFormat()
		{
			Assert.Throws<ArgumentException>(
				() => default( Timestamp ).ToString( "G", null )
			);
		}

		[Test]
		public void TestToString_AsOFormatAndNullIFormatProvider()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-62193657600,
						0
					).ToString(),
					Is.EqualTo( "Å|0001-03-01T00:00:00.000000000Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}

		[Test]
		public void TestToString_IFormatProvider_AsOFormat()
		{
			Assert.That(
				new Timestamp(
					-62193657600,
					0
				).ToString( new LegacyJapaneseCultureInfo() ),
				Is.EqualTo( "Å|0001-03-01T00:00:00.000000000Z" )
			);
		}

		[Test]
		public void TestToString_String_AsNullIFormatProvider()
		{
			var originalCurrentCulture = CultureInfo.CurrentCulture;
			try
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					new LegacyJapaneseCultureInfo();
				Assert.That(
					new Timestamp(
						-62193657600,
						0
					).ToString( "s" ),
					Is.EqualTo( "Å|0001-03-01T00:00:00Z" )
				);
			}
			finally
			{
#if !NET35
				CultureInfo.CurrentCulture = 
#else // !NET35
				Thread.CurrentThread.CurrentCulture =
#endif // !NET35
					originalCurrentCulture;
			}
		}
	}
}
