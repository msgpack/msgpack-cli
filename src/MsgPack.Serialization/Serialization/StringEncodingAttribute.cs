// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Specifies <see cref="Encoding"/> for this member's value.
	/// </summary>
	public sealed class StringEncodingAttribute : Attribute
	{
		/// <summary>
		///		Gets name of the encoding.
		/// </summary>
		/// <value>Name of the encoding.</value>
		public string Name { get; }

		internal Encoding Encoding { get; }

		/// <summary>
		///		Initializes new instance.
		/// </summary>
		/// <param name="name">
		///		Name of the encoding. This value must be valid name for <see cref="Encoding.GetEncoding(String)"/>.
		/// </param>
		public StringEncodingAttribute(string name)
		{
			this.Name = Ensure.NotBlank(name);
			this.Encoding = Encoding.GetEncoding(name, EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
		}
	}
}
