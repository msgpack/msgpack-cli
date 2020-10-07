// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Text;

namespace MsgPack.Serialization
{
	public static class KeyNameTransformers
	{
		public static readonly Func<string, string> AsIs = key => key;

		public static readonly Func<string, string> ToLowerCamel = ToLowerCamelCore;

		public static readonly Func<string, string> ToUpperSnake = s => ToSnakeCaseCore(s, toBeUpperCase: true);

		public static readonly Func<string, string> ToLowerSnake = s => ToSnakeCaseCore(s, toBeUpperCase: false);

		private static string ToLowerCamelCore(string mayBeUpperCamel)
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

		private static string ToSnakeCaseCore(string mayBeUpperCamel, bool toBeUpperCase)
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
