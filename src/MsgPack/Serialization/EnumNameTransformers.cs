#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines built-in, out-of-box handlers for <see cref="EnumSerializationOptions.NameTransformer"/>.
	/// </summary>
	public static class EnumNameTransformers
	{
		private static readonly Func<string, string> _lowerCamel = KeyNameTransformers.ToLowerCamel;

		/// <summary>
		///		Gets the handler which transforms upper camel casing (PascalCasing) name to lower camel casing (camelCasing) name.
		/// </summary>
		/// <value>
		///		The handler which transforms upper camel casing (PascalCasing) name to lower camel casing (camelCasing) name.
		/// </value>
		/// <remarks>
		///		This method uses <see cref="Char"/> based invariant culture to tranform casing, so non ASCII charactors may not be transformed correctly espetially surrogate pairs.
		/// </remarks>
		public static Func<string, string> LowerCamel
		{
			get { return _lowerCamel; }
		}

		private static readonly Func<string, string> _upperSnake = KeyNameTransformers.ToUpperSnake;

		/// <summary>
		///		Gets the handler which transforms upper camel casing (PascalCasing) name to upper snake casing (UPPER_SNAKE_CASING) name.
		/// </summary>
		/// <value>
		///		The handler which transforms upper camel casing (PascalCasing) name to upper snake casing (UPPER_SNAKE_CASING) name.
		/// </value>
		/// <remarks>
		///		This method uses <see cref="Char"/> based invariant culture to tranform casing, so non ASCII charactors may not be transformed correctly espetially surrogate pairs.
		/// </remarks>
		public static Func<string, string> UpperSnake
		{
			get { return _upperSnake; }
		}
	}
}