// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Serialization
{
	public sealed class SerializationOptionsBuilder
	{
		private int _maxDepth = OptionsDefaults.MaxDepth;

		public int MaxDepth
		{
			get => this._maxDepth;
			set => this._maxDepth = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		public System.Text.Encoding? StringEncoding { get; set; }

		public SerializationOptions Create()
			=> new SerializationOptions(this.MaxDepth, this.StringEncoding);
	}
}