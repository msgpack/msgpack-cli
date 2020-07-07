// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MsgPack.Internal;

using PrimitiveEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>>;
using StringEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>, System.Text.Encoding?, System.Threading.CancellationToken>;
using BinaryEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>, System.Threading.CancellationToken>;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal partial class ObjectDelegateSerializer
	{
		private static readonly PrimitiveEncoder EncodeInt32NonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeInt32((int)v!, s);
		private static readonly PrimitiveEncoder EncodeInt32Nullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeInt32((int?)v, s);
		private static readonly PrimitiveEncoder EncodeInt64NonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeInt64((long)v!, s);
		private static readonly PrimitiveEncoder EncodeInt64Nullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeInt64((long?)v, s);
		private static readonly PrimitiveEncoder EncodeUInt32NonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeUInt32((uint)v!, s);
		private static readonly PrimitiveEncoder EncodeUInt32Nullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeUInt32((uint?)v, s);
		private static readonly PrimitiveEncoder EncodeUInt64NonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeUInt64((ulong)v!, s);
		private static readonly PrimitiveEncoder EncodeUInt64Nullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeUInt64((ulong?)v, s);
		private static readonly PrimitiveEncoder EncodeBooleanNonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeBoolean((bool)v!, s);
		private static readonly PrimitiveEncoder EncodeBooleanNullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeBoolean((bool?)v, s);
		private static readonly PrimitiveEncoder EncodeSingleNonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeSingle((float)v!, s);
		private static readonly PrimitiveEncoder EncodeSingleNullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeSingle((float?)v, s);
		private static readonly PrimitiveEncoder EncodeDoubleNonNull =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeDouble((double)v!, s);
		private static readonly PrimitiveEncoder EncodeDoubleNullable =
			(FormatEncoder e, object? v, IBufferWriter<byte> s) => e.EncodeDouble((double?)v, s);
		private static readonly StringEncoder EncodeString =
			(FormatEncoder e, object? v, IBufferWriter<byte> s, Encoding? n, CancellationToken c) =>
			{
				if (v is null)
				{
					e.EncodeNull(s);
					return;
				}

				if (v is string str)
				{
					e.EncodeString(str, s, n, c);
				}
				else if (v is StringBuilder builder)
				{
					e.EncodeString(builder, s, n, c);
				}
				else if (v is char[] array)
				{
					e.EncodeString(array, s, n, c);
				}
				else if (v is ReadOnlyMemory<char> rmemory)
				{
					e.EncodeString(rmemory.Span, s, n, c);
				}
				else if (v is Memory<char> memory)
				{
					e.EncodeString(memory.Span, s, n, c);
				}
				else if (v is ReadOnlySequence<char> sequence)
				{
					e.EncodeString(sequence, s, n, c);
				}
				else if (v is IEnumerable<char> collection)
				{
					e.EncodeString(collection.ToArray(), s, n, c);
				}
				else
				{
					Throw.UnexpectedStringType(v);
				}
			};

		private static readonly BinaryEncoder EncodeBinary =
			(FormatEncoder e, object? v, IBufferWriter<byte> s, CancellationToken c) =>
			{
				if (v is null)
				{
					e.EncodeNull(s);
					return;
				}

				if (v is byte[] array)
				{
					e.EncodeBinary(array, s, c);
				}
				else if (v is ReadOnlyMemory<byte> rmemory)
				{
					e.EncodeBinary(rmemory.Span, s, c);
				}
				else if (v is Memory<byte> memory)
				{
					e.EncodeBinary(memory.Span, s, c);
				}
				else if (v is ReadOnlySequence<byte> sequence)
				{
					e.EncodeBinary(sequence, s, c);
				}
				else if (v is IEnumerable<byte> collection)
				{
					e.EncodeBinary(collection.ToArray(), s, c);
				}
				else
				{
					Throw.UnexpectedBinaryType(v);
					sequence = default;
				}
			};
	}
}
