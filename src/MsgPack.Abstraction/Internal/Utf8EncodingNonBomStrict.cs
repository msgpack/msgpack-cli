// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;

namespace MsgPack.Internal
{
	/// <summary>
	///		Defines sealed UTF-8 class which cannot emit BOM and throws Exception.
	/// </summary>
	/// <remarks>
	///		Sealed class typed static constant is important for JIT devirtualization optimization.
	/// </remarks>
	internal sealed class Utf8EncodingNonBomStrict : UTF8Encoding
	{
		public static Utf8EncodingNonBomStrict Instance { get; } = new Utf8EncodingNonBomStrict();

		private Utf8EncodingNonBomStrict() : base(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)
		{
			this.DecoderFallback = DecoderFallback.ExceptionFallback;
		}
	}
}
