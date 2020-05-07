// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;

namespace MsgPack.Serialization
{
	public sealed class SerializationOptions
	{
		public static SerializationOptions Default { get; } = new SerializationOptionsBuilder().Create();

		public int MaxDepth { get; }

		public Encoding? StringEncoding { get; }

		internal SerializationOptions(
			int maxDepth,
			Encoding? stringEncoding
		)
		{
			this.MaxDepth = maxDepth;
			this.StringEncoding = stringEncoding;
		}
	}
}
