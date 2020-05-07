// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public sealed class MessagePackEncoderOptionsBuilder : EncoderOptionsBuilder
	{
		public MessagePackEncoderOptionsBuilder() { }

		public MessagePackEncoderOptions Build() => new MessagePackEncoderOptions(this);
	}
}
