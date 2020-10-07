// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Codecs
{
	/// <summary>
	///		Defines basic features and interfaces of codec provider which provides encoder and decoder.
	/// </summary>
	public abstract class CodecProvider 
	{
		/// <summary>
		///		Initializes a new instance of <see cref="CodecProvider"/> object.
		/// </summary>
		protected CodecProvider() { }

		/// <summary>
		///		Gets an instance of <see cref="FormatEncoder"/> object.
		/// </summary>
		/// <returns>An instance of <see cref="FormatEncoder"/> object.</returns>
		public abstract FormatEncoder GetEncoder();

		/// <summary>
		///		Gets an instance of <see cref="FormatDecoder"/> object.
		/// </summary>
		/// <returns>An instance of <see cref="FormatDecoder"/> object.</returns>
		public abstract FormatDecoder GetDecoder();
	}
}
