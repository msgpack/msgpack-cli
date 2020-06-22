// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	public sealed class DeserializationOptionsBuilder
	{
		private int _maxArrayLength = OptionsDefaults.MaxArrayLength;

		public int MaxArrayLength
		{
			get => this._maxArrayLength;
			set => this._maxArrayLength = Ensure.IsBetween(value, 0, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		private int _maxMapCount = OptionsDefaults.MaxMapCount;

		public int MaxMapCount
		{
			get => this._maxMapCount;
			set => this._maxMapCount = Ensure.IsBetween(value, 0, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		private int _maxPropertyKeyLength = OptionsDefaults.MaxPropertyKeyLength;

		public int MaxPropertyKeyLength
		{
			get => this._maxPropertyKeyLength;
			set => this._maxPropertyKeyLength = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		private int _maxDepth = OptionsDefaults.MaxDepth;

		public int MaxDepth
		{
			get => this._maxDepth;
			set => this._maxDepth = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		public Encoding? StringEncoding { get; set; }

		public ArrayPool<byte>? ArrayPool { get; set; }

		public DeserializationOptions Create()
			=> new DeserializationOptions(this.MaxArrayLength, this.MaxMapCount, this.MaxPropertyKeyLength, this.MaxDepth, this.StringEncoding, this.ArrayPool ?? ArrayPool<byte>.Shared);
	}
}
