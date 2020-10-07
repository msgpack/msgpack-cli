// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Codecs
{
	public sealed class MessagePackCodecProvider : CodecProvider
	{
		public static MessagePackCodecProvider Default { get; } = new MessagePackCodecProvider(MessagePackEncoderOptions.Default, MessagePackDecoderOptions.Default);

		private readonly MessagePackEncoder _encoder;
		private readonly MessagePackDecoder _decoder;

		public MessagePackCodecProvider(
			MessagePackEncoderOptions encoderOptions, 
			MessagePackDecoderOptions decoderOptions,
			MessagePackCompatibilityLevel compatibilityLevel = MessagePackCompatibilityLevel.Latest
		)
		{
			if (compatibilityLevel == MessagePackCompatibilityLevel.Version2008)
			{
				this._encoder = new LegacyMessagePackEncoder(encoderOptions);
			}
			else
			{
				this._encoder = new CurrentMessagePackEncoder(encoderOptions);
			}

			this._decoder = new MessagePackDecoder(decoderOptions);
		}

		public sealed override FormatEncoder GetEncoder() => this._encoder;

		public sealed override FormatDecoder GetDecoder() => this._decoder;

	}
}
