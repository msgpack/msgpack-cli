// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;

namespace MsgPack.Internal
{
	/// <summary>
	///		Defines sealed UTF-8 class which cannot emit BOM.
	/// </summary>
	/// <remarks>
	///		Sealed class typed static constant is important for JIT devirtualization optimization.
	/// </remarks>
	internal sealed class Utf8EncodingNonBom : UTF8Encoding
	{
		public static Utf8EncodingNonBom Instance { get; } = new Utf8EncodingNonBom();

		private Utf8EncodingNonBom() : base(encoderShouldEmitUTF8Identifier: false) { }
	}
}
