#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
using System.Diagnostics.Contracts;
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
		public static void ValidateBuffer<T>( T[] byteArray, int offset, long length, string nameOfByteArray, string nameOfLength, bool validateBufferSize )
		{
#if !NETFX_35 && !UNITY
			Contract.Assume( !String.IsNullOrWhiteSpace( nameOfByteArray ) );
			Contract.Assume( !String.IsNullOrWhiteSpace( nameOfLength ) );
#endif // !NETFX_35 && !UNITY

			if ( byteArray == null )
			{
				throw new ArgumentNullException( nameOfByteArray );
			}

			if ( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" ) );
			}

			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "nameOfLength", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", nameOfLength ) );
			}

			if ( validateBufferSize && byteArray.Length < offset + length )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"'{0}' is too small for specified '{1}' and '{2}'. Length of '{0}' is {3}, '{1}' is {4}, '{2}' is {5}.",
						nameOfByteArray,
						"offset",
						nameOfLength,
						byteArray.Length,
						offset,
						length
					)
				);
			}
		}

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

		private static readonly Regex UnicodeTr15Annex7IdentifierPattern =
			new Regex(
				UnicodeTr15Annex7Idneifier,
#if !SILVERLIGHT && !NETFX_CORE && !UNITY
				RegexOptions.Compiled |
#endif // !SILVERLIGHT && !NETFX_CORE && !UNITY
				RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline
			);

		public static void ValidateMethodName( string methodName, string parameterName )
		{
			ValidateIsNotNullNorEmpty( methodName, parameterName );

			var matches = UnicodeTr15Annex7IdentifierPattern.Matches( methodName );
			if ( matches.Count == 1 && matches[ 0 ].Success && matches[ 0 ].Index == 0 && matches[ 0 ].Length == methodName.Length )
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
			Contract.Assert( position >= 0 );
#endif // !UNITY

			var category = CharUnicodeInfo.GetUnicodeCategory( methodName, position );
			// ReSharper disable RedundantIfElseBlock
			if ( IsPrintable( category ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Char at {0}('{1}'\\u{2}[{3}] is not used for method name.",
						position,
						methodName[ position ],
						( ushort )methodName[ position ],
						category
					)
				);
			}
			else
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Char at {0}(\\u{1}[{2}] is not used for method name.",
						position,
						( ushort )methodName[ position ],
						category
					)
				);
			}
			// ReSharper restore RedundantIfElseBlock
		}

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
			ValidateIsNotNullNorEmpty( @namespace, parameterName );

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
			Contract.Assert( position >= 0 );
#endif // !UNITY

			var category = CharUnicodeInfo.GetUnicodeCategory( @namespace, position );
			// ReSharper disable RedundantIfElseBlock
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
			// ReSharper restore RedundantIfElseBlock
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
