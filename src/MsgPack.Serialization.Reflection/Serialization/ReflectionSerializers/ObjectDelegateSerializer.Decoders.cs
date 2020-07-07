// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal partial class ObjectDelegateSerializer
	{
		private static readonly PrimitiveDecoder DecodeSByteNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeSByte(ref s);
		private static readonly PrimitiveDecoder DecodeSByteNullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableSByte(ref s);
		private static readonly PrimitiveDecoder DecodeInt16NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeInt16(ref s);
		private static readonly PrimitiveDecoder DecodeInt16Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableInt16(ref s);
		private static readonly PrimitiveDecoder DecodeInt32NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeInt32(ref s);
		private static readonly PrimitiveDecoder DecodeInt32Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableInt32(ref s);
		private static readonly PrimitiveDecoder DecodeInt64NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeInt64(ref s);
		private static readonly PrimitiveDecoder DecodeInt64Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableInt64(ref s);
		private static readonly PrimitiveDecoder DecodeByteNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeByte(ref s);
		private static readonly PrimitiveDecoder DecodeByteNullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableByte(ref s);
		private static readonly PrimitiveDecoder DecodeUInt16NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeUInt16(ref s);
		private static readonly PrimitiveDecoder DecodeUInt16Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableUInt16(ref s);
		private static readonly PrimitiveDecoder DecodeUInt32NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeUInt32(ref s);
		private static readonly PrimitiveDecoder DecodeUInt32Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableUInt32(ref s);
		private static readonly PrimitiveDecoder DecodeUInt64NonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeUInt64(ref s);
		private static readonly PrimitiveDecoder DecodeUInt64Nullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableUInt64(ref s);
		private static readonly PrimitiveDecoder DecodeBooleanNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeBoolean(ref s);
		private static readonly PrimitiveDecoder DecodeBooleanNullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableBoolean(ref s);
		private static readonly PrimitiveDecoder DecodeSingleNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeSingle(ref s);
		private static readonly PrimitiveDecoder DecodeSingleNullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableSingle(ref s);
		private static readonly PrimitiveDecoder DecodeDoubleNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeDouble(ref s);
		private static readonly PrimitiveDecoder DecodeDoubleNullable =
			(FormatDecoder d, ref SequenceReader<byte> s) => d.DecodeNullableDouble(ref s);

		// Also for IEnumerable<char>
		private static readonly StringDecoder DecodeStringNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeString(ref s, e, c);

		// Also for IEnumerable<char>?
		private static readonly StringDecoder DecodeStringNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeNullableString(ref s, e, c);

		private static readonly StringDecoder DecodeStringBuilderNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => AsStringBuilder(d.DecodeString(ref s, e, c));

		private static readonly StringDecoder DecodeStringBuilderNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => AsStringBuilder(d.DecodeNullableString(ref s, e, c));

		private static readonly StringDecoder DecodeCharArrayNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeString(ref s, e, c).ToCharArray();

		private static readonly StringDecoder DecodeCharArrayNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeNullableString(ref s, e, c)?.ToCharArray();

		private static readonly StringDecoder DecodeReadOnlyMemoryOfCharNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeString(ref s, e, c).AsMemory();

		private static readonly StringDecoder DecodeReadOnlyMemoryOfCharNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeNullableString(ref s, e, c)?.AsMemory();

		private static readonly StringDecoder DecodeMemoryOfCharNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeString(ref s, e, c).ToCharArray().AsMemory();

		private static readonly StringDecoder DecodeMemoryOfCharNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => d.DecodeNullableString(ref s, e, c)?.ToCharArray()?.AsMemory();

		private static readonly StringDecoder DecodeReadOnlySequenceOfCharNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => new ReadOnlySequence<char>(d.DecodeString(ref s, e, c).AsMemory());

		private static readonly StringDecoder DecodeReadOnlySequenceOfCharNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, Encoding? e, CancellationToken c) => AsSequence(d.DecodeNullableString(ref s, e, c));

		private static readonly BinaryDecoder DecodeByteArrayNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => d.DecodeBinary(ref s, c);

		private static readonly BinaryDecoder DecodeByteArrayNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => d.DecodeNullableBinary(ref s, c);

		private static readonly BinaryDecoder DecodeReadOnlyMemoryOfByteNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => AsReadOnlyMemory(d.DecodeBinary(ref s, c));

		private static readonly BinaryDecoder DecodeReadOnlyMemoryOfByteNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => AsReadOnlyMemory(d.DecodeNullableBinary(ref s, c));

		private static readonly BinaryDecoder DecodeMemoryOfByteNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => d.DecodeBinary(ref s, c).AsMemory();

		private static readonly BinaryDecoder DecodeMemoryOfByteNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => d.DecodeNullableBinary(ref s, c)?.AsMemory();

		private static readonly BinaryDecoder DecodeReadOnlySequenceOfByteNonNull =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => new ReadOnlySequence<byte>(d.DecodeBinary(ref s, c));

		private static readonly BinaryDecoder DecodeReadOnlySequenceOfByteNullable =
			(FormatDecoder d, ref SequenceReader<byte> s, CancellationToken c) => AsSequence(d.DecodeNullableBinary(ref s, c));

		[return: NotNullIfNotNull("value")]
		private static StringBuilder? AsStringBuilder(string? value) => value == null ? null : new StringBuilder(value);

		[return: NotNullIfNotNull("value")]
		private static ReadOnlyMemory<byte>? AsReadOnlyMemory(byte[]? value) => value == null ? default(ReadOnlyMemory<byte>?) : new ReadOnlyMemory<byte>(value);

		[return: NotNullIfNotNull("value")]
		private static ReadOnlySequence<char>? AsSequence(string? value) => value == null ? default(ReadOnlySequence<char>?) : new ReadOnlySequence<char>(value.AsMemory());

		[return: NotNullIfNotNull("value")]
		private static ReadOnlySequence<byte>? AsSequence(byte[]? value) => value == null ? default(ReadOnlySequence<byte>?) : new ReadOnlySequence<byte>(value);

		// Span/ReadOnlySpan is not supported.
	}
}
