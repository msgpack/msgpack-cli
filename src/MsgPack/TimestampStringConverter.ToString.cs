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
using System.Text;

namespace MsgPack
{
	partial class TimestampStringConverter
	{
		private const string DefaultFormat = "o";

		public static string ToString( string format, IFormatProvider formatProvider, ref Timestamp.Value value )
		{
			switch ( format ?? DefaultFormat )
			{
				case "o":
				case "O":
				{
					// round-trip
					return ToIso8601String( formatProvider, /* containsNanoseconds */true, ref value );
				}
				case "s":
				{
					// sortable(ISO-8601)
					return ToIso8601String( formatProvider, /* containsNanoseconds */false, ref value );
				}
				default:
				{
					throw new ArgumentException( "The specified format is not supported.", "format" );
				}
			}
		}

		private static string ToIso8601String( IFormatProvider formatProvider, bool containsNanosecons, ref Timestamp.Value value )
		{
			var numberFormat = NumberFormatInfo.GetInstance( formatProvider );

			// Most cases are yyyy-MM-ddTHH:mm:ss[.fffffffff]Z -- 50 or 60 chars.
			var buffer = new StringBuilder( 49 + ( containsNanosecons ? 11 : 1 ) );
			buffer.Append( value.Year.ToString( "0000", formatProvider ) );
			buffer.Append( DateDelimiter );
			buffer.Append( value.Month.ToString( "00", formatProvider ) );
			buffer.Append( DateDelimiter );
			buffer.Append( value.Day.ToString( "00", formatProvider ) );
			buffer.Append( DateTimeDelimiter );
			buffer.Append( value.Hour.ToString( "00", formatProvider ) );
			buffer.Append( TimeDelimiter );
			buffer.Append( value.Minute.ToString( "00", formatProvider ) );
			buffer.Append( TimeDelimiter );
			buffer.Append( value.Second.ToString( "00", formatProvider ) );

			if ( containsNanosecons )
			{
				buffer.Append( SubsecondDelimiter );
				buffer.Append( value.Nanoseconds.ToString( "000000000", formatProvider ) );
			}

			buffer.Append( UtcSign );

			return buffer.ToString();
		}
	}
}
