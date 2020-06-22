// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		A builder object for immutable <see cref="JsonDecoderOptions"/>.
	/// </summary>
	public class JsonDecoderOptionsBuilder : FormatDecoderOptionsBuilder
	{
		public JsonParseOptions ParseOptions { get; set; }

		public JsonDecoderOptionsBuilder() { }

		public JsonDecoderOptions Build() => new JsonDecoderOptions(this);
	}
}
