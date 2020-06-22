// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		public sealed override bool DecodeItem(ref SequenceReader<byte> source, out DecodeItemResult result, CancellationToken cancellationToken = default)
		{
			var startPosition = source.Position;
			var triviaLength = this.ReadTrivia(ref source);
			if (triviaLength != 0)
			{
				result = DecodeItemResult.ScalarOrSequence(ElementType.OtherTrivia, source.Sequence.Slice(startPosition, source.Position));
				return true;
			}

			if (!source.TryPeek(out var token))
			{
				result = DecodeItemResult.InsufficientInput(1);
				return false;
			}

#warning HANDLE :/= and ,
			switch (token)
			{
				case (byte)'t':
				{
					ReadOnlySpan<byte> @true;
					unsafe
					{
						byte* pTrue = stackalloc byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
						@true = new ReadOnlySpan<byte>(pTrue, 4);
					}

					if (source.IsNext(@true, advancePast: true))
					{
						result = DecodeItemResult.True();
						return true;
					}

					break;
				}
				case (byte)'f':
				{
					ReadOnlySpan<byte> @false;
					unsafe
					{
						byte* pFalse = stackalloc byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
						@false = new ReadOnlySpan<byte>(pFalse, 5);
					}

					if (source.IsNext(@false, advancePast: true))
					{
						result = DecodeItemResult.False();
						return true;
					}

					break;
				}
				case (byte)'n':
				{
					ReadOnlySpan<byte> @null;
					unsafe
					{
						byte* pNull = stackalloc byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
						@null = new ReadOnlySpan<byte>(pNull, 4);
					}

					if (source.IsNext(@null, advancePast: true))
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
					var number = this.DecodeNumber(ref source, out var requestHint);
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
					if (this.GetRawStringCore(ref source, out var rawString, out var requestHint))
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
