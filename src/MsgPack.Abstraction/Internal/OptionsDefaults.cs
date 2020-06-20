// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Internal
{
#warning TODO: tuning default as backward compatible
	internal static class OptionsDefaults
	{
		public static readonly int CancellationSupportThreshold = 128 * 1024 * 1024; // About 0.1 sec in desktop, more for IoT
		public static readonly int MaxNumberLengthInBytes = 32;
		public static readonly int MaxStringLengthInBytes = 256 * 1024 * 1024;
		public static readonly int MaxBinaryLengthInBytes = 256 * 1024 * 1024;
		public static readonly int MaxByteBufferLength = 2 * 1024 * 1024;
		public static readonly int MaxCharBufferLength = 2 * 1024 * 1024;
		public static readonly int MaxArrayLength = 1024 * 1024;
		public static readonly int MaxMapCount = 1024 * 1024;
		public static readonly int MaxPropertyKeyLength = 256;
		public static readonly int MaxDepth = 100;
		public static readonly ArrayPool<byte> ByteBufferPool = ArrayPool<byte>.Shared;
		public static readonly ArrayPool<char> CharBufferPool = ArrayPool<char>.Shared;
		public static readonly bool ClearsBuffer = true;
		public static readonly bool CanTreatRealAsInteger = true;
	}
}
