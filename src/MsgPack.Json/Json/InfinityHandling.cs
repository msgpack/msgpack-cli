// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Json
{
	/// <summary>
	///		Defines positive and/or negative infinity handling in JSON serialization to be compliant with RFC 8259.
	/// </summary>
	public enum InfinityHandling
	{
		/// <summary>
		///		Use system default setting. See <see cref="JsonEncoderOptionsBuilder"/> for current default.
		/// </summary>
		Default = 0,

		/// <summary>
		///		Use <c>MinValue</c> for negative infinity and <c>MaxValue</c> for positive infinity.
		/// </summary>
		MinMax = 1,

		/// <summary>
		///		Throws exception conservatively.
		/// </summary>
		Error = 2,

		/// <summary>
		///		Use own custom formatting logic.
		/// </summary>
		Custom = 3
	}
}
