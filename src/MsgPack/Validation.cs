#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Text.RegularExpressions;

namespace MsgPack
{
	// ArgumentValidatorAttribute cannot be used here because of its license terms.
	// So validation in this class is not captured from code contract tools.

	/// <summary>
	///		Common validtion utility.
	/// </summary>
	// [ArgumentValidator]
	internal static class Validation
	{
		public static void ValidateIsNotNullNorEmpty( string value, string parameterName )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( parameterName );
			}

			if ( value.Length == 0 )
			{
				throw new ArgumentException(
					String.Format( CultureInfo.CurrentCulture, "'{0}' cannot be empty.", parameterName ),
					parameterName
				);
			}
		}

		private const string UnicodeTr15Annex7Idneifier =
			@"[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*";

		private static readonly Regex NamespacePattern =
			new Regex(
				"^(" + UnicodeTr15Annex7Idneifier + @")(\." + UnicodeTr15Annex7Idneifier + ")*$",
#if !SILVERLIGHT && !NETFX_CORE && !UNITY
				RegexOptions.Compiled |
#endif // !SILVERLIGHT && !NETFX_CORE && !UNITY
				RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline
			);

		public static void ValidateNamespace( string @namespace, string parameterName )
		{
			if ( @namespace == null )
			{
				throw new ArgumentNullException( "namespace" );
			}

			if ( @namespace.Length == 0 )
			{
				// Global is OK.
				return;
			}

			var matches = NamespacePattern.Matches( @namespace );
			if ( matches.Count == 1 && matches[ 0 ].Success && matches[ 0 ].Index == 0 && matches[ 0 ].Length == @namespace.Length )
			{
				return;
			}

			// Get invalid value.
			int position = 0;
			int validLength = 0;
			for ( int i = 0; i < matches.Count; i++ )
			{
				if ( matches[ i ].Index == validLength )
				{
					validLength += matches[ i ].Length;
				}
				else
				{
					position = validLength;
					break;
				}
			}

#if !UNITY
			Contract.Assert( position >= 0, "position >= 0" );
#endif // !UNITY

			var category = CharUnicodeInfo.GetUnicodeCategory( @namespace, position );
			if ( IsPrintable( category ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Char at {0}('{1}'\\u{2}[{3}] is not used for namespace.",
						position,
						@namespace[ position ],
						( ushort )@namespace[ position ],
						category
					)
				);
			}
			else
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Char at {0}(\\u{1}[{2}] is not used for namespace.",
						position,
						( ushort )@namespace[ position ],
						category
					)
				);
			}
		}

		/// <summary>
		///		Determine specified category is printiable.
		/// </summary>
		/// <param name="category">Unicode cateory.</param>
		/// <returns>
		///		If all charactors in specified category are printable then true.
		///		Other wise false.
		/// </returns>
		/// <remarks>
		///		This method is conservative, but application cannot print the charactor
		///		because appropriate font is not installed the machine.
		/// </remarks>
		private static bool IsPrintable( UnicodeCategory category )
		{
			switch ( category )
			{
				case UnicodeCategory.ClosePunctuation:
				case UnicodeCategory.ConnectorPunctuation:
				case UnicodeCategory.CurrencySymbol:
				case UnicodeCategory.DashPunctuation:
				case UnicodeCategory.DecimalDigitNumber:
				case UnicodeCategory.EnclosingMark:
				case UnicodeCategory.FinalQuotePunctuation:
				case UnicodeCategory.InitialQuotePunctuation:
				case UnicodeCategory.LetterNumber:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.MathSymbol:
				case UnicodeCategory.NonSpacingMark:
				case UnicodeCategory.OpenPunctuation:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.OtherNumber:
				case UnicodeCategory.OtherPunctuation:
				case UnicodeCategory.OtherSymbol:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.UppercaseLetter:
				{
					return false;
				}
				default:
				{
					return true;
				}
			}
		}
	}
}
