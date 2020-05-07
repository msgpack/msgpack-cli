// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	internal static class MemoryExtensions
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static void Convert(this System.Text.Encoder encoder, ReadOnlyMemory<char> source, Memory<byte> sink, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
			=> encoder.Convert(source.Span, sink.Span, flush, out charsUsed, out bytesUsed, out completed);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static unsafe Memory<T> Compact<T>(this Memory<T> memory, T[] array, int used)
			where T : unmanaged
		{
			var span = memory.Span.Slice(used);
			if (span.IsEmpty)
			{
				return array;
			}

			fixed (T* pSrc = span)
			fixed (T* pDest = array)
			{
				Buffer.MemoryCopy(pSrc, pDest, array.Length, span.Length);
				return array.AsMemory(0, span.Length);
			}
		}
	}
}
