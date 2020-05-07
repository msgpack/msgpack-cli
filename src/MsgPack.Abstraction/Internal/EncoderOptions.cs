// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
	public abstract class EncoderOptions
	{
		public int CancellationSupportThreshold { get; }

		public int MaxByteBufferLength { get; }

		public int MaxCharBufferLength { get; }

		public ArrayPool<byte> ByteBufferPool { get; }

		public ArrayPool<char> CharBufferPool { get; }

		public bool ClearsBuffer { get; }

		protected EncoderOptions(EncoderOptionsBuilder builder)
		{
			builder = Ensure.NotNull(builder);

			this.CancellationSupportThreshold = builder.CancellationSupportThreshold;
			this.MaxByteBufferLength = builder.MaxByteBufferLength;
			this.MaxCharBufferLength = builder.MaxCharBufferLength;
			this.ByteBufferPool = builder.ByteBufferPool;
			this.CharBufferPool = builder.CharBufferPool;
			this.ClearsBuffer = builder.ClearsBuffer;
		}
	}
}
