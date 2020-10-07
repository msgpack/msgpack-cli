// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;
using MsgPack.Json;

namespace MsgPack.Codecs
{
	public sealed class JsonCodecProvider : CodecProvider
	{
		private readonly JsonEncoder _encoder;
		private readonly JsonDecoder _decoder;

		public JsonCodecProvider(
			JsonEncoderOptions encoderOptions, 
			JsonDecoderOptions decoderOptions
		)
		{
			this._encoder = new JsonEncoder(Ensure.NotNull(encoderOptions));
			Ensure.NotNull(decoderOptions);
			if (decoderOptions.ParseOptions == JsonParseOptions.None)
			{
				this._decoder = new SimpleJsonDecoder(decoderOptions);
			}
			else
			{
				this._decoder = new FlexibleJsonDecoder(decoderOptions);
			}
		}

		public sealed override FormatEncoder GetEncoder() => this._encoder;

		public sealed override FormatDecoder GetDecoder() => this._decoder;
	}
}
