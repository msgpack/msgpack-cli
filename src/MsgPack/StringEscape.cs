#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015-2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion -- License Terms --

using System;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	internal static class StringEscape
	{
		public static string ForDisplay( string value )
		{
			if ( value == null )
			{
				return String.Empty;
			}

			var buffer = new StringBuilder( value.Length );
			foreach ( char c in value )
			{
				switch ( c )
				{
					case '\0':
					{
						buffer.Append( "\\0" );
						continue;
					}
					case '\t':
					{
						buffer.Append( "\\t" );
						continue;
					}
					case '\r':
					{
						buffer.Append( "\\r" );
						continue;
					}
					case '\n':
					{
						buffer.Append( "\\n" );
						continue;
					}
					case '\a':
					{
						buffer.Append( "\\a" );
						continue;
					}
					case '\b':
					{
						buffer.Append( "\\b" );
						continue;
					}
					case '\f':
					{
						buffer.Append( "\\f" );
						continue;
					}
					case '\v':
					{
						buffer.Append( "\\v" );
						continue;
					}
				}

				switch ( CharUnicodeInfo.GetUnicodeCategory( c ) )
				{
					case UnicodeCategory.Control:
					case UnicodeCategory.Format:
					case UnicodeCategory.OtherNotAssigned:
					case UnicodeCategory.PrivateUse:
					{
						buffer.Append( "\\u" ).Append( ( ( int ) c ).ToString( "X4", CultureInfo.InvariantCulture ) );
						continue;
					}
					default:
					{
						buffer.Append( c );
						continue;
					}
				}
			}

			return buffer.ToString();
		}
	}
}
