// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
#warning Remove unused buffer max pr rename to initial buffer length if appropriate.
	public abstract class DecoderOptionsBuilder
	{
		public bool CanTreatRealAsInteger { get; set; } = OptionsDefaults.CanTreatRealAsInteger;

		private int _cancellationSupportThreshold = OptionsDefaults.CancellationSupportThreshold;

		public int CancellationSupportThreshold
		{
			get => this._cancellationSupportThreshold;
			set => this._cancellationSupportThreshold = Ensure.IsNotLessThan(value, 1);
		}

		private int _maxNumberLengthInBytes = OptionsDefaults.MaxNumberLengthInBytes;

		public int MaxNumberLengthInBytes
		{
			get => this._maxNumberLengthInBytes;
			set => this._maxNumberLengthInBytes = Ensure.IsBetween(value, 1, OptionsDefaults.MaxSingleByteCollectionLength);
		}

		private int _maxStringLengthInBytes = OptionsDefaults.MaxStringLengthInBytes;

		public int MaxStringLengthInBytes
	{
			get => this._maxStringLengthInBytes;
			set => this._maxStringLengthInBytes = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength);
		}

		private int _naxBinaryLengthInBytes = OptionsDefaults.MaxBinaryLengthInBytes;

		public int MaxBinaryLengthInBytes
		{
			get => this._naxBinaryLengthInBytes;
			set => this._naxBinaryLengthInBytes = Ensure.IsBetween(value, 1, OptionsDefaults.MaxSingleByteCollectionLength);
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

		public bool ClearsBuffer { get; set; } = OptionsDefaults.ClearsBuffer;

		protected DecoderOptionsBuilder() { }

		public DecoderOptionsBuilder WithoutBufferClear()
		{
			this.ClearsBuffer = false;
			return this;
		}

		public DecoderOptionsBuilder ProhibitTreatRealAsInteger()
		{
			this.CanTreatRealAsInteger = false;
			return this;
		}
	}
}
