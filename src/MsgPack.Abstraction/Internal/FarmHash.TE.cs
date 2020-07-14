// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

// This file is ported from Google's FarmHash (https://github.com/google/farmhash/)
// Original copyright notice is following:

// Copyright (c) 2014 Google, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// FarmHash, by Geoff Pike

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using __m128i = System.Runtime.Intrinsics.Vector128<ulong>;

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		private static class TE
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe __m128i Fetch128(byte* s)
			=> Sse2.LoadVector128(s).AsUInt64(); // _mm_loadu_si128

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Add(__m128i x, __m128i y)
				=> Sse2.Add(x, y); // _mm_add_epi64

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Xor(__m128i x, __m128i y)
				=> Sse2.Xor(x, y); // _mm_xor_si128
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Mul(__m128i x, __m128i y)
				=> Sse41.MultiplyLow(x.AsUInt32(), y.AsUInt32()).AsUInt64(); // _mm_mullo_epi32
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Shuf(__m128i x, __m128i y)
				=> Ssse3.Shuffle(x.AsByte(), y.AsByte()).AsUInt64(); // _mm_shuffle_epi8

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void swap(ref __m128i left, ref __m128i right)
			{
				var temp = left;
				left = right;
				right = temp;
			}

			// Requires n >= 256.  Requires SSE4.1. Should be slightly faster if the
			// compiler uses AVX instructions (e.g., use the -mavx flag with GCC).
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong Hash64Long(byte* s, uint n, ulong seed0, ulong seed1)
			{
				__m128i kShuf =
					Vector128.Create(4, 11, 10, 5, 8, 15, 6, 9, 12, 2, 14, 13, 0, 7, 3, 1).AsUInt64(); // _mm_set_epi8
				__m128i kMult =
					Vector128.Create(0xbd, 0xd6, 0x33, 0x39, 0x45, 0x54, 0xfa, 0x03,
								 0x34, 0x3e, 0x33, 0xed, 0xcc, 0x9e, 0x2d, 0x51).AsUInt64(); // _mm_set_epi8
				ulong seed2 = (seed0 + 113) * (seed1 + 9);
				ulong seed3 = (Rotate64(seed0, 23) + 27) * (Rotate64(seed1, 30) + 111);
				__m128i d0 = Sse2.X64.ConvertScalarToVector128UInt64(seed0); // _mm_cvtsi64_si128
				__m128i d1 = Sse2.X64.ConvertScalarToVector128UInt64(seed1); // _mm_cvtsi64_si128
				__m128i d2 = Shuf(kShuf, d0);
				__m128i d3 = Shuf(kShuf, d1);
				__m128i d4 = Xor(d0, d1);
				__m128i d5 = Xor(d1, d2);
				__m128i d6 = Xor(d2, d4);
				__m128i d7 = Vector128.Create(seed2 >> 32); // _mm_set1_epi32
				__m128i d8 = Mul(kMult, d2);
				__m128i d9 = Vector128.Create(seed3 >> 32); // _mm_set1_epi32
				__m128i d10 = Vector128.Create(seed3); // _mm_set1_epi32
				__m128i d11 = Add(d2, Vector128.Create(seed2)); // _mm_set1_epi32
				// size_t == ulong here because this code should be run under X64 process.
				byte* end = s + (n & ~(/* size_t */ ulong)(255));
				do
				{
					__m128i z;
					z = Fetch128(s);
					d0 = Add(d0, z);
					d1 = Shuf(kShuf, d1);
					d2 = Xor(d2, d0);
					d4 = Xor(d4, z);
					d4 = Xor(d4, d1);
					swap(ref d0, ref d6);
					z = Fetch128(s + 16);
					d5 = Add(d5, z);
					d6 = Shuf(kShuf, d6);
					d8 = Shuf(kShuf, d8);
					d7 = Xor(d7, d5);
					d0 = Xor(d0, z);
					d0 = Xor(d0, d6);
					swap(ref d5, ref d11);
					z = Fetch128(s + 32);
					d1 = Add(d1, z);
					d2 = Shuf(kShuf, d2);
					d4 = Shuf(kShuf, d4);
					d5 = Xor(d5, z);
					d5 = Xor(d5, d2);
					swap(ref d10, ref d4);
					z = Fetch128(s + 48);
					d6 = Add(d6, z);
					d7 = Shuf(kShuf, d7);
					d0 = Shuf(kShuf, d0);
					d8 = Xor(d8, d6);
					d1 = Xor(d1, z);
					d1 = Add(d1, d7);
					z = Fetch128(s + 64);
					d2 = Add(d2, z);
					d5 = Shuf(kShuf, d5);
					d4 = Add(d4, d2);
					d6 = Xor(d6, z);
					d6 = Xor(d6, d11);
					swap(ref d8, ref d2);
					z = Fetch128(s + 80);
					d7 = Xor(d7, z);
					d8 = Shuf(kShuf, d8);
					d1 = Shuf(kShuf, d1);
					d0 = Add(d0, d7);
					d2 = Add(d2, z);
					d2 = Add(d2, d8);
					swap(ref d1, ref d7);
					z = Fetch128(s + 96);
					d4 = Shuf(kShuf, d4);
					d6 = Shuf(kShuf, d6);
					d8 = Mul(kMult, d8);
					d5 = Xor(d5, d11);
					d7 = Xor(d7, z);
					d7 = Add(d7, d4);
					swap(ref d6, ref d0);
					z = Fetch128(s + 112);
					d8 = Add(d8, z);
					d0 = Shuf(kShuf, d0);
					d2 = Shuf(kShuf, d2);
					d1 = Xor(d1, d8);
					d10 = Xor(d10, z);
					d10 = Xor(d10, d0);
					swap(ref d11, ref d5);
					z = Fetch128(s + 128);
					d4 = Add(d4, z);
					d5 = Shuf(kShuf, d5);
					d7 = Shuf(kShuf, d7);
					d6 = Add(d6, d4);
					d8 = Xor(d8, z);
					d8 = Xor(d8, d5);
					swap(ref d4, ref d10);
					z = Fetch128(s + 144);
					d0 = Add(d0, z);
					d1 = Shuf(kShuf, d1);
					d2 = Add(d2, d0);
					d4 = Xor(d4, z);
					d4 = Xor(d4, d1);
					z = Fetch128(s + 160);
					d5 = Add(d5, z);
					d6 = Shuf(kShuf, d6);
					d8 = Shuf(kShuf, d8);
					d7 = Xor(d7, d5);
					d0 = Xor(d0, z);
					d0 = Xor(d0, d6);
					swap(ref d2, ref d8);
					z = Fetch128(s + 176);
					d1 = Add(d1, z);
					d2 = Shuf(kShuf, d2);
					d4 = Shuf(kShuf, d4);
					d5 = Mul(kMult, d5);
					d5 = Xor(d5, z);
					d5 = Xor(d5, d2);
					swap(ref d7, ref d1);
					z = Fetch128(s + 192);
					d6 = Add(d6, z);
					d7 = Shuf(kShuf, d7);
					d0 = Shuf(kShuf, d0);
					d8 = Add(d8, d6);
					d1 = Xor(d1, z);
					d1 = Xor(d1, d7);
					swap(ref d0, ref d6);
					z = Fetch128(s + 208);
					d2 = Add(d2, z);
					d5 = Shuf(kShuf, d5);
					d4 = Xor(d4, d2);
					d6 = Xor(d6, z);
					d6 = Xor(d6, d9);
					swap(ref d5, ref d11);
					z = Fetch128(s + 224);
					d7 = Add(d7, z);
					d8 = Shuf(kShuf, d8);
					d1 = Shuf(kShuf, d1);
					d0 = Xor(d0, d7);
					d2 = Xor(d2, z);
					d2 = Xor(d2, d8);
					swap(ref d10, ref d4);
					z = Fetch128(s + 240);
					d3 = Add(d3, z);
					d4 = Shuf(kShuf, d4);
					d6 = Shuf(kShuf, d6);
					d7 = Mul(kMult, d7);
					d5 = Add(d5, d3);
					d7 = Xor(d7, z);
					d7 = Xor(d7, d4);
					swap(ref d3, ref d9);
					s += 256;
				} while (s != end);
				d6 = Add(Mul(kMult, d6), Sse2.X64.ConvertScalarToVector128UInt64(n)); // _mm_cvtsi64_si128
				if (n % 256 != 0)
				{
					d7 = Add(Sse2.Shuffle(d8.AsUInt32(), (0 << 6) + (3 << 4) + (2 << 2) + (1 << 0)).AsUInt64(), d7); // _mm_shuffle_epi32
					d8 = Add(Mul(kMult, d8), Sse2.X64.ConvertScalarToVector128UInt64(XO.Hash64(s, n % 256))); // _mm_cvtsi64_si128
				}
				var t = stackalloc __m128i[8];
				d0 = Mul(kMult, Shuf(kShuf, Mul(kMult, d0)));
				d3 = Mul(kMult, Shuf(kShuf, Mul(kMult, d3)));
				d9 = Mul(kMult, Shuf(kShuf, Mul(kMult, d9)));
				d1 = Mul(kMult, Shuf(kShuf, Mul(kMult, d1)));
				d0 = Add(d11, d0);
				d3 = Xor(d7, d3);
				d9 = Add(d8, d9);
				d1 = Add(d10, d1);
				d4 = Add(d3, d4);
				d5 = Add(d9, d5);
				d6 = Xor(d1, d6);
				d2 = Add(d0, d2);
				t[0] = d0;
				t[1] = d3;
				t[2] = d9;
				t[3] = d1;
				t[4] = d4;
				t[5] = d5;
				t[6] = d6;
				t[7] = d2;
				return XO.Hash64((byte*)t, /* sizeof(t) */ 8);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64(byte* s, uint len)
				=> len >= 512 ?
					Hash64Long(s, len, k2, k1) :
					XO.Hash64(s, len);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64WithSeed(byte* s, uint len, uint seed)
				=> len >= 512 ?
					Hash64Long(s, len, k1, seed) :
					XO.Hash64WithSeed(s, len, seed);
		}
	}
}
