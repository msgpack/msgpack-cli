// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Text;
using System.Threading;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal partial class ObjectDelegateSerializer
	{
		private static readonly AsyncPrimitiveDecoder DecodeSByteNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeSByteAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeSByteNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableSByteAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt16NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeInt16Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt16NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableInt16Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt32NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeInt32Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt32NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableInt32Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt64NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeInt64Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeInt64NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableInt64Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeByteNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeByteAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeByteNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableByteAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt16NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeUInt16Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt16NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableUInt16Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt32NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeUInt32Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt32NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableUInt32Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt64NonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeUInt64Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeUInt64NullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableUInt64Async(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeBooleanNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeBooleanAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeBooleanNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableBooleanAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeSingleNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeSingleAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeSingleNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableSingleAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeDoubleNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeDoubleAsync(s, c).ConfigureAwait(false);
		private static readonly AsyncPrimitiveDecoder DecodeDoubleNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableDoubleAsync(s, c).ConfigureAwait(false);

		// Also for IEnumerable<char>
		private static readonly AsyncStringDecoder DecodeStringNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => await d.DecodeStringAsync(s, e, c).ConfigureAwait(false);

		// Also for IEnumerable<char>?
		private static readonly AsyncStringDecoder DecodeStringNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false);

		private static readonly AsyncStringDecoder DecodeStringBuilderNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => AsStringBuilder(await d.DecodeStringAsync(s, e, c).ConfigureAwait(false));

		private static readonly AsyncStringDecoder DecodeStringBuilderNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => AsStringBuilder(await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false));

		private static readonly AsyncStringDecoder DecodeCharArrayNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeStringAsync(s, e, c).ConfigureAwait(false)).ToCharArray();

		private static readonly AsyncStringDecoder DecodeCharArrayNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false))?.ToCharArray();

		private static readonly AsyncStringDecoder DecodeReadOnlyMemoryOfCharNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeStringAsync(s, e, c).ConfigureAwait(false)).AsMemory();

		private static readonly AsyncStringDecoder DecodeReadOnlyMemoryOfCharNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false))?.AsMemory();

		private static readonly AsyncStringDecoder DecodeMemoryOfCharNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeStringAsync(s, e, c).ConfigureAwait(false)).ToCharArray().AsMemory();

		private static readonly AsyncStringDecoder DecodeMemoryOfCharNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => (await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false))?.ToCharArray()?.AsMemory();

		private static readonly AsyncStringDecoder DecodeReadOnlySequenceOfCharNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => new ReadOnlySequence<char>((await d.DecodeStringAsync(s, e, c).ConfigureAwait(false)).AsMemory());

		private static readonly AsyncStringDecoder DecodeReadOnlySequenceOfCharNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, Encoding? e, CancellationToken c) => AsSequence(await d.DecodeNullableStringAsync(s, e, c).ConfigureAwait(false));

		private static readonly AsyncBinaryDecoder DecodeByteArrayNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeBinaryAsync(s, c).ConfigureAwait(false);

		private static readonly AsyncBinaryDecoder DecodeByteArrayNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => await d.DecodeNullableBinaryAsync(s, c).ConfigureAwait(false);

		private static readonly AsyncBinaryDecoder DecodeReadOnlyMemoryOfByteNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => AsReadOnlyMemory(await d.DecodeBinaryAsync(s, c).ConfigureAwait(false));

		private static readonly AsyncBinaryDecoder DecodeReadOnlyMemoryOfByteNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => AsReadOnlyMemory(await d.DecodeNullableBinaryAsync(s, c).ConfigureAwait(false));

		private static readonly AsyncBinaryDecoder DecodeMemoryOfByteNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => (await d.DecodeBinaryAsync(s, c).ConfigureAwait(false)).AsMemory();

		private static readonly AsyncBinaryDecoder DecodeMemoryOfByteNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => (await d.DecodeNullableBinaryAsync(s, c).ConfigureAwait(false))?.AsMemory();

		private static readonly AsyncBinaryDecoder DecodeReadOnlySequenceOfByteNonNullAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => new ReadOnlySequence<byte>(await d.DecodeBinaryAsync(s, c).ConfigureAwait(false));

		private static readonly AsyncBinaryDecoder DecodeReadOnlySequenceOfByteNullableAsync =
			async (FormatDecoder d, ReadOnlyStreamSequence s, CancellationToken c) => AsSequence(await d.DecodeNullableBinaryAsync(s, c).ConfigureAwait(false));

		// Span/ReadOnlySpan is not supported.
	}
}
