// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		A builder object to construct immutable <see cref="EncoderOptions"/> object.
	/// </summary>
	public class JsonEncoderOptionsBuilder : EncoderOptionsBuilder
	{
		private static readonly ReadOnlyMemory<byte> DefaultIndentChars =
			new byte[] { (byte)' ', (byte)' ' };

		private static readonly ReadOnlyMemory<byte> DefaultNewLineChars =
			new byte[] { (byte)'\n' };

		private InfinityHandling _infinityHandling = InfinityHandling.Default;

		public InfinityHandling InfinityHandling
		{
			get => this._infinityHandling;
			set => this._infinityHandling = JsonOptionsValidation.EnsureKnownNonCustom(value);
		}

		private NaNHandling _nanHandling;

		public NaNHandling NaNHandling
		{
			get => this._nanHandling;
			set => this._nanHandling = JsonOptionsValidation.EnsureKnownNonCustom(value);
		}

		private ReadOnlyMemory<byte> _indentChars = DefaultIndentChars;
		public ReadOnlyMemory<byte> IndentChars
		{
			get => this._indentChars;
			set
			{
				for (var i = 0; i < value.Length; i++)
				{
					switch (value.Span[i])
					{
						case (byte)' ':
						case (byte)'\t':
						case (byte)'\r':
						case (byte)'\n':
						{
							// OK 
							break;
						}
						default:
						{
							throw new ArgumentException(
								$"IndentChars can only contains whitespace (U+0020), horizontal tab (U+0009), carriage return (U+000A), and line feed (U+000D) according to RFC8259, but char at {i} is U+00{value.Span[i]:X2}.",
								"value"
							);
						}
					}
				}

				this._indentChars = value;
			}
		}

		private ReadOnlyMemory<byte> _newLineChars = DefaultNewLineChars;
		public ReadOnlyMemory<byte> NewLineChars
		{
			get => this._newLineChars;
			set
			{
				for (var i = 0; i < value.Length; i++)
				{
					switch (value.Span[i])
					{
						case (byte)'\r':
						case (byte)'\n':
						{
							// OK 
							break;
						}
						default:
						{
							throw new ArgumentException(
								$"NewLineChars can only contains carriage return (U+000A), and line feed (U+000D) according to RFC8259, but char at {i} is U+00{value.Span[i]:X2}.",
								"value"
							);
						}
					}
				}

				this._newLineChars = value;
			}
		}

		public bool IsPrettyPrint { get; set; }

		public ReadOnlyMemory<Rune> AdditionalEscapeTargetChars { get; set; }

		public bool EscapesHorizontalTab { get; set; } = true;
		public bool EscapesPrivateUseCharactors { get; set; } = false;
		public bool EscapesHtmlChars { get; set; } = false;

		internal Action<float, IBufferWriter<byte>, JsonEncoderOptions>? SingleInfinityFormatter;
		internal Action<double, IBufferWriter<byte>, JsonEncoderOptions>? DoubleInfinityFormatter;
		internal Action<float, IBufferWriter<byte>, JsonEncoderOptions>? SingleNaNFormatter;
		internal Action<double, IBufferWriter<byte>, JsonEncoderOptions>? DoubleNaNFormatter;

		public JsonEncoderOptionsBuilder() { }

		public JsonEncoderOptionsBuilder SetCustomInfinityFormatter(
			Action<float, IBufferWriter<byte>, JsonEncoderOptions> singleInfinityFormatter,
			Action<double, IBufferWriter<byte>, JsonEncoderOptions> doubleInfinityFormatter
		)
		{
			this.SingleInfinityFormatter = Ensure.NotNull(singleInfinityFormatter);
			this.DoubleInfinityFormatter = Ensure.NotNull(doubleInfinityFormatter);
			this._infinityHandling = InfinityHandling.Custom;
			return this;
		}

		public JsonEncoderOptionsBuilder SetCustomNaNFormatter(
			Action<float, IBufferWriter<byte>, JsonEncoderOptions> singleNaNFormatter,
			Action<double, IBufferWriter<byte>, JsonEncoderOptions> doubleNaNFormatter
		)
		{
			this.SingleNaNFormatter = Ensure.NotNull(singleNaNFormatter);
			this.DoubleNaNFormatter = Ensure.NotNull(doubleNaNFormatter);
			this._nanHandling = NaNHandling.Custom;
			return this;
		}

		public JsonEncoderOptionsBuilder WithHtmlCharactorEscaping()
		{
			this.EscapesHtmlChars = true;
			return this;
		}

		public JsonEncoderOptionsBuilder WithoutHorizontalTabEscaping()
		{
			this.EscapesHorizontalTab = false;
			return this;
		}

		public JsonEncoderOptionsBuilder WithoutPrivateUseCharsEscaping()
		{
			this.EscapesPrivateUseCharactors = false;
			return this;
		}

		public JsonEncoderOptions Build() => new JsonEncoderOptions(this);
	}
}
