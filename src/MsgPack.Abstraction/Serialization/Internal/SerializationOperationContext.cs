// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Serialization.Internal
{
	public struct SerializationOperationContext
	{
		public Encoder Encoder { get; }
		public SerializationOptions Options { get; }
		private int _currentDepth;
		public int CurrentDepth => this._currentDepth;
		public CancellationToken CancellationToken { get; }
		public System.Text.Encoding? StringEncoding => this.Options.StringEncoding;

		public SerializationOperationContext(Encoder encoder, SerializationOptions? options, CancellationToken cancellationToken)
		{
			this.Encoder = Ensure.NotNull(encoder);
			this.Options = options ?? SerializationOptions.Default;
			this._currentDepth = 0;
			this.CancellationToken = cancellationToken;
		}

		public CollectionContext CollectionContext => new CollectionContext(Int32.MaxValue, Int32.MaxValue, Int32.MaxValue, this._currentDepth);

		public int IncrementDepth()
		{
			if (this._currentDepth == this.Options.MaxDepth)
			{
				Throw.DepthExeeded(this._currentDepth, this.Options.MaxDepth);
			}

			return this._currentDepth++;
		}

		public int DecrementDepth()
		{
			if (this._currentDepth == 0)
			{
				Throw.DepthUnderflow();
			}

			return this._currentDepth--;
		}
	}
}
