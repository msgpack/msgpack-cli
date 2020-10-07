// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
	public abstract class FormatEncoderOptionsBuilder
	{
		private int _cancellationSupportThreshold = OptionsDefaults.CancellationSupportThreshold;

		public int CancellationSupportThreshold
		{
			get => this._cancellationSupportThreshold;
			set => this._cancellationSupportThreshold = Ensure.IsNotLessThan(value, 1);
		}

		private int _maxByteBufferLength = OptionsDefaults.MaxByteBufferLength;

		public int MaxByteBufferLength
		{
			get => this._maxByteBufferLength;
			set => this._maxByteBufferLength = Ensure.IsNotLessThan(value, 4);
		}

		private int _maxCharBufferLength = OptionsDefaults.MaxCharBufferLength;

		public int MaxCharBufferLength
		{
			get => this._maxCharBufferLength;
			set => this._maxCharBufferLength = Ensure.IsNotLessThan(value, 2);
		}

		private ArrayPool<byte> _byteBufferPool = OptionsDefaults.ByteBufferPool;

		public ArrayPool<byte> ByteBufferPool
		{
			get => this._byteBufferPool;
			set => this._byteBufferPool = Ensure.NotNull(value);
		}

		private ArrayPool<char> _charBufferPool = OptionsDefaults.CharBufferPool;

		public ArrayPool<char> CharBufferPool
		{
			get => this._charBufferPool;
			set => this._charBufferPool = Ensure.NotNull(value);
		}

		public bool ClearsBuffer { get; set; } = OptionsDefaults.ClearsBufferOnReturn;

		protected FormatEncoderOptionsBuilder() { }

		public FormatEncoderOptionsBuilder WithoutBufferClear()
		{
			this.ClearsBuffer = false;
			return this;
		}
	}
}
