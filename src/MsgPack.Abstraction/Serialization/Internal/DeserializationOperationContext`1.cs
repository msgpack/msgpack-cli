// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Serialization.Internal
{
	public struct DeserializationOperationContext<TExtensionType>
	{
		public Decoder<TExtensionType> Decoder { get; }
		public DeserializationOptions Options { get; }
		public int CurrentDepth { get; private set; }
		public CancellationToken CancellationToken { get; }
		public ArrayPool<byte> ArrayPool => this.Options.ArrayPool;
		public Encoding? StringEncoding => this.Options.StringEncoding;

		public DeserializationOperationContext(Decoder<TExtensionType> decoder, DeserializationOptions? options, CancellationToken cancellationToken)
		{
			this.Decoder = Ensure.NotNull(decoder);
			this.Options = options ?? DeserializationOptions.Default;
			this.CurrentDepth = 0;
			this.CancellationToken = cancellationToken;
		}

		public CollectionContext CollectionContext =>
			new CollectionContext(
				this.Options.MaxArrayLength, 
				this.Options.MaxMapCount,
				this.Options.MaxDepth, 
				this.CurrentDepth
			);

		public int IncrementDepth()
		{
			if (this.CurrentDepth == this.Options.MaxDepth)
			{
				Throw.DepthExeeded(this.CurrentDepth, this.Options.MaxDepth);
			}

			return this.CurrentDepth++;
		}

		public int DecrementDepth()
		{
			if (this.CurrentDepth == 0)
			{
				Throw.DepthUnderflow();
			}

			return this.CurrentDepth--;
		}

		public readonly void ValidatePropertyKeyLength(long position, int length)
		{
			if(length > this.Options.MaxPropertyKeyLength)
			{
				Throw.TooLargePropertyKey(position, length, this.Options.MaxPropertyKeyLength);
			}
		}
	}
}
