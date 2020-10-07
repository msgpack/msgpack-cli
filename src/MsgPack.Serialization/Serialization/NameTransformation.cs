// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements name tranformation logic.
	/// </summary>
	internal static class NameTransformation
	{
		/// <summary>
		///		Converts UpperCamel (Pascal) or lowerCamel cased name to lowerCamel casing.
		/// </summary>
		/// <param name="mayBeUpperCamel">Name which is UpperCamel (Pascal) or lowerCamel cased.</param>
		/// <returns>Name with lowerCamel casing.</returns>
		public static string ToLowerCamelCase(string mayBeUpperCamel)
		{
			if (String.IsNullOrEmpty(mayBeUpperCamel))
			{
				return mayBeUpperCamel;
			}

			if (!Char.IsUpper(mayBeUpperCamel[0]))
			{
				return mayBeUpperCamel;
			}

			var buffer = new StringBuilder(mayBeUpperCamel.Length);
			buffer.Append(Char.ToLowerInvariant(mayBeUpperCamel[0]));
			if (mayBeUpperCamel.Length > 1)
			{
				buffer.Append(mayBeUpperCamel, 1, mayBeUpperCamel.Length - 1);
			}

			return buffer.ToString();
		}

		/// <summary>
		///		Converts UpperCamel (Pascal) or lowerCamel cased name to UPPER_SNAKE or lower_snake casing.
		/// </summary>
		/// <param name="mayBeUpperCamel">Name which is UpperCamel (Pascal) or lowerCamel cased.</param>
		/// <param name="toBeUpperCase"><c>true</c>, if result should be upper casing; <c>false</c>, should be lower casing.</param>
		/// <returns>Name with UPPER_SNAKE or lower_snake casing.</returns>
		public static string ToSnakeCase(string mayBeUpperCamel, bool toBeUpperCase)
		{
			if (String.IsNullOrEmpty(mayBeUpperCamel))
			{
				return mayBeUpperCamel;
			}

			var buffer = new StringBuilder(mayBeUpperCamel.Length * 2);
			char previous = '\0';
			int index = 0;
	
			// Process continuous upper cases in head.
			for (; index < mayBeUpperCamel.Length; index++)
			{
				var c = mayBeUpperCamel[index];
				if (Char.IsUpper(c))
				{
					buffer.Append(toBeUpperCase ? c : Char.ToLowerInvariant(c));
					previous = c;
				}
				else
				{
					buffer.Append(toBeUpperCase ? Char.ToUpperInvariant(c) : c);
					previous = c;
					index++;
					break;
				}
			}

			// Process remaining
			for (; index < mayBeUpperCamel.Length; index++)
			{
				var c = mayBeUpperCamel[index];
				if (Char.IsUpper(c))
				{
					if (previous != '_')
					{
						buffer.Append('_');
					}

					buffer.Append(toBeUpperCase ? c : Char.ToLowerInvariant(c));
					previous = c;
				}
				else
				{
					buffer.Append(toBeUpperCase ? Char.ToUpperInvariant(c) : c);
					previous = c;
				}
			}

			return buffer.ToString();
		}
	}
}
