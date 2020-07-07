// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines decoder options for <see cref="JsonDecoder"/>.
	/// </summary>
	public class JsonDecoderOptions : FormatDecoderOptions
	{
		public static JsonDecoderOptions Default { get; } = new JsonDecoderOptionsBuilder().Build();

		public JsonParseOptions ParseOptions { get; }

		public JsonDecoderOptions(JsonDecoderOptionsBuilder builder)
			: base(builder, JsonFormatFeatures.Value)
		{
			this.ParseOptions = builder.ParseOptions;
		}
	}
}
