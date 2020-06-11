// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using MsgPack.Internal;

using DecodeItemResult = MsgPack.Internal.DecodeItemResult<MsgPack.Internal.NullExtensionType>;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		public override bool DecodeItem(in SequenceReader<byte> source, out DecodeItemResult result, CancellationToken cancellationToken = default)
		{
			this.ReadTrivia(source, out var trivia);
			if (!trivia.IsEmpty)
			{
				result = DecodeItemResult.ScalarOrSequence(ElementType.OtherTrivia, trivia);
				return true;
			}

			if (!this.TryPeek(source, out var token))
			{
				result = DecodeItemResult.InsufficientInput(1);
				return false;
			}

			switch (token)
			{
				case (byte)'t':
				{
					if (source.IsNext(JsonTokens.True, advancePast: true))
					{
						result = DecodeItemResult.True();
						return true;
					}

					break;
				}
				case (byte)'f':
				{
					if (source.IsNext(JsonTokens.False, advancePast: true))
					{
						result = DecodeItemResult.False();
						return true;
					}

					break;
				}
				case (byte)'n':
				{
					if (source.IsNext(JsonTokens.Null, advancePast: true))
					{
						result = DecodeItemResult.Null();
						return true;
					}

					break;
				}
				case (byte)'0':
				case (byte)'1':
				case (byte)'2':
				case (byte)'3':
				case (byte)'4':
				case (byte)'5':
				case (byte)'6':
				case (byte)'7':
				case (byte)'8':
				case (byte)'9':
				case (byte)'-':
				case (byte)'+':
				{
					var number = this.DecodeNumber(source, out var requestHint);
					if(requestHint != 0)
					{
						result = DecodeItemResult.InsufficientInput(requestHint);
						return false;
					}

					var value = new byte[sizeof(double)];
					Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(value.AsSpan()), number);
					result = DecodeItemResult.ScalarOrSequence(ElementType.Double, value);
					return true;
				}
				case (byte)'[':
				{
					result = DecodeItemResult.CollectionHeader(ElementType.Array, this.CreateArrayIterator());
					return true;
				}
				case (byte)'{':
				{
					result = DecodeItemResult.CollectionHeader(ElementType.OtherTrivia, this.CreateObjectPropertyIterator());
					return true;
				}
				case (byte)'\'':
				{
					if ((this.Options.ParseOptions & JsonParseOptions.AllowSingleQuotationString) != 0)
					{
						goto case (byte)'"';
					}

					break;
				}
				case (byte)'"':
				{
					if (this.GetRawStringCore(source, out var rawString, out var requestHint))
					{
						result = DecodeItemResult.InsufficientInput(requestHint);
						return false;
					}

					result = DecodeItemResult.ScalarOrSequence(ElementType.String, rawString);
					return true;
				}
			}

			JsonThrow.UnexpectedToken(source.Consumed, token);
			// never
			result = default;
			return default;
		}
	}
}
