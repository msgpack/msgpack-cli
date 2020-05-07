// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;

namespace MsgPack.Serialization
{
	public sealed class DeserializationOptions
	{
		public static DeserializationOptions Default { get; } = new DeserializationOptionsBuilder().Create();

		public int MaxArrayLength { get; }
		public int MaxMapCount { get; }
		public int MaxPropertyKeyLength { get; }
		public int MaxDepth { get; }
		public Encoding? StringEncoding { get; }
		public ArrayPool<byte> ArrayPool { get; }

		internal DeserializationOptions(
			int maxArrayLength,
			int maxMapCount,
			int maxPropertyKeyLength,
			int maxDepth,
			Encoding? stringEncoding,
			ArrayPool<byte> arrayPool
		)
		{
			this.MaxArrayLength =maxArrayLength;
			this.MaxMapCount = maxMapCount;
			this.MaxPropertyKeyLength = maxPropertyKeyLength;
			this.MaxDepth = maxDepth;
			this.StringEncoding = stringEncoding;
			this.ArrayPool = arrayPool;
		}
	}
}
