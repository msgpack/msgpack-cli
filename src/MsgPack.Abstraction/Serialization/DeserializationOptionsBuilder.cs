// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
#warning TODO: tuning default as backward compatible
	public sealed class DeserializationOptionsBuilder
	{
		internal const int DefaultMaxArrayLength = 1024 * 1024;
		internal const int DefaultMaxMapCount = 1024 * 1024;
		internal const int DefaultMaxPropertyKeyLength = 256;
		internal const int DefaultMaxDepth = 100;

		private int _maxArrayLength = DefaultMaxArrayLength;

		public int MaxArrayLength
		{
			get => this._maxArrayLength;
			set => this._maxArrayLength = Ensure.IsNotLessThan(value, 0);
		}

		private int _maxMapCount = DefaultMaxMapCount;

		public int MaxMapCount
		{
			get => this._maxMapCount;
			set => this._maxMapCount = Ensure.IsNotLessThan(value, 0);
		}

		private int _maxPropertyKeyLength = DefaultMaxPropertyKeyLength;

		public int MaxPropertyKeyLength
		{
			get => this._maxPropertyKeyLength;
			set => this._maxPropertyKeyLength = Ensure.IsNotLessThan(value, 1);
		}

		private int _maxDepth = DefaultMaxDepth;

		public int MaxDepth
		{
			get => this._maxDepth;
			set => this._maxDepth = Ensure.IsNotLessThan(value, 1);
		}

		public Encoding? StringEncoding { get; set; }

		public ArrayPool<byte>? ArrayPool { get; set; }

		public DeserializationOptions Create()
			=> new DeserializationOptions(this.MaxArrayLength, this.MaxMapCount, this.MaxPropertyKeyLength, this.MaxDepth, this.StringEncoding, this.ArrayPool ?? ArrayPool<byte>.Shared);
	}
}
