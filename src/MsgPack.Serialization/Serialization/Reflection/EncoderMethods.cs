// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Reflection;
using System.Text;
using System.Threading;
using MsgPack.Internal;

namespace MsgPack.Serialization.Reflection
{
	internal static class EncoderMethods
	{
		public static readonly MethodInfo EncodeInt32NonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeInt32), new[] { typeof(int), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeInt32Nullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeInt32), new[] { typeof(int?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeInt64NonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeInt64), new[] { typeof(long), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeInt64Nullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeInt64), new[] { typeof(long?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeUInt32NonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeUInt32), new[] { typeof(uint), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeUInt32Nullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeUInt32), new[] { typeof(uint?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeUInt64NonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeUInt64), new[] { typeof(ulong), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeUInt64Nullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeUInt64), new[] { typeof(ulong?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeBooleanNonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeBoolean), new[] { typeof(bool), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeBooleanNullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeBoolean), new[] { typeof(bool?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeSingleNonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeSingle), new[] { typeof(float), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeSingleNullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeSingle), new[] { typeof(float?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeDoubleNonNull =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeDouble), new[] { typeof(double), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeDoubleNullable =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeDouble), new[] { typeof(double?), typeof(IBufferWriter<byte>) })!;

		public static readonly MethodInfo EncodeStringString =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeString), new[] { typeof(string), typeof(IBufferWriter<byte>), typeof(Encoding), typeof(CancellationToken) })!;

		public static readonly MethodInfo EncodeStringStringBuilder =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeString), new[] { typeof(StringBuilder), typeof(IBufferWriter<byte>), typeof(Encoding), typeof(CancellationToken) })!;

		public static readonly MethodInfo EncodeStringSpan =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeString), new[] { typeof(ReadOnlySpan<char>), typeof(IBufferWriter<byte>), typeof(Encoding), typeof(CancellationToken) })!;

		public static readonly MethodInfo EncodeStringSequence =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeString), new[] { typeof(ReadOnlySequence<char>), typeof(IBufferWriter<byte>), typeof(Encoding), typeof(CancellationToken) })!;

		public static readonly MethodInfo EncodeBinarySpan =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeBinary), new[] { typeof(ReadOnlySpan<byte>), typeof(IBufferWriter<byte>), typeof(CancellationToken) })!;

		public static readonly MethodInfo EncodeBinarySequence =
			typeof(FormatEncoder).GetMethod(nameof(FormatEncoder.EncodeBinary), new[] { typeof(ReadOnlySequence<byte>), typeof(IBufferWriter<byte>), typeof(CancellationToken) })!;
	}
}
