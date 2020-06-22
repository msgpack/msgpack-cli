// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public sealed class MessagePackEncoderOptions : FormatEncoderOptions
	{
		public static MessagePackEncoderOptions Default { get; } = new MessagePackEncoderOptionsBuilder().Build();

		public MessagePackEncoderOptions(MessagePackEncoderOptionsBuilder builder) : base(builder) { }
	}
}
