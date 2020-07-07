// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
	public abstract class FormatDecoderOptions
	{
		public bool CanTreatRealAsInteger { get; }

		public int CancellationSupportThreshold { get; }

		public ArrayPool<byte> ByteBufferPool { get; }

		public ArrayPool<char> CharBufferPool { get; }

		public int MaxByteBufferLength { get; }

		public int MaxCharBufferLength { get; }

		public int MaxNumberLengthInBytes { get; }

		public int MaxStringLengthInBytes { get; }

		public int MaxBinaryLengthInBytes { get; }

		public bool ClearsBuffer { get; }

		public FormatFeatures Features { get; }

		protected FormatDecoderOptions(FormatDecoderOptionsBuilder builder, FormatFeatures features)
		{
			builder = Ensure.NotNull(builder);

			this.CanTreatRealAsInteger = builder.CanTreatRealAsInteger;
			this.CancellationSupportThreshold = builder.CancellationSupportThreshold;
			this.ClearsBuffer = builder.ClearsBuffer;
			this.ByteBufferPool = builder.ByteBufferPool;
			this.CharBufferPool = builder.CharBufferPool;
			this.MaxByteBufferLength = builder.MaxByteBufferLength;
			this.MaxCharBufferLength = builder.MaxCharBufferLength;
			this.MaxNumberLengthInBytes = builder.MaxNumberLengthInBytes;
			this.MaxStringLengthInBytes = builder.MaxStringLengthInBytes;
			this.MaxBinaryLengthInBytes = builder.MaxBinaryLengthInBytes;
			this.Features = Ensure.NotNull(features);
		}
	}
}
