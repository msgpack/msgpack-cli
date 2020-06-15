// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public sealed class MessagePackDecoderOptions : DecoderOptions
	{
		public static MessagePackDecoderOptions Default { get; } = new MessagePackDecoderOptionsBuilder().Build();

		internal MessagePackDecoderOptions(MessagePackDecoderOptionsBuilder builder)
			: base(builder) { }
	}
}
