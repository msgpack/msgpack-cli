// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		// Common methods for Intel compatible CPU intrinsics and corresponds 64bit methods.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe ulong Fetch64(byte* p)
			=> *(ulong*)p;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe Vector128<uint> Fetch128(byte* s)
			=> Sse2.LoadVector128(s).AsUInt32(); // _mm_loadu_si128

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Rotate64(ulong val, int shift)
			// Avoid shifting by 64: doing so yields an undefined result.
			=> shift == 0 ? val : ((val >> shift) | (val << (64 - shift)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Add(Vector128<uint> x, Vector128<uint> y)
			=> Sse2.Add(x, y); // _mm_add_epi32

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Xor(Vector128<uint> x, Vector128<uint> y)
			=> Sse2.Xor(x, y); // _mm_xor_si128

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Or(Vector128<uint> x, Vector128<uint> y)
			=> Sse2.Or(x, y); // _mm_or_si128

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Mul(Vector128<uint> x, Vector128<uint> y)
			=> Sse41.MultiplyLow(x, y); // _mm_mullo_epi32

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Mul5(Vector128<uint> x)
			=> Add(
				x,
				Sse2.ShiftLeftLogical(x, 2) // _mm_slli_epi32
			);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> RotateLeft(Vector128<uint> x, byte c)
			=> Or(
				Sse2.ShiftLeftLogical(x, c),// _mm_slli_epi32
				Sse2.ShiftRightLogical(x, (byte)(32 - c)) // _mm_srli_epi32
			);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Rol17(Vector128<uint> x)
			=> RotateLeft(x, 17);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Rol19(Vector128<uint> x)
			=> RotateLeft(x, 19);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<uint> Shuffle0321(Vector128<uint> x)
			=> Sse2.Shuffle(x, (0 << 6) + (3 << 4) + (2 << 2) + (1 << 0)); // _mm_shuffle_epi32

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void swap(ref ulong left, ref ulong right)
		{
			var temp = left;
			left = right;
			right = temp;
		}
	}
}
