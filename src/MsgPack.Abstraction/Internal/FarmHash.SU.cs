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

using __m128i = System.Runtime.Intrinsics.Vector128<uint>;

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		internal static class SU
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe __m128i Fetch128(byte* s)
				=> Sse2.LoadVector128(s).AsUInt32(); // _mm_loadu_si128

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Add(__m128i x, __m128i y)
				=> Sse2.Add(x, y); // _mm_add_epi32

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Xor(__m128i x, __m128i y)
				=> Sse2.Xor(x, y); // _mm_xor_si128

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Or(__m128i x, __m128i y)
				=> Sse2.Or(x, y); // _mm_or_si128

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Mul(__m128i x, __m128i y)
				=> Sse41.MultiplyLow(x, y); // _mm_mullo_epi32

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Mul5(__m128i x)
				=> Add(
					x,
					Sse2.ShiftLeftLogical(x, 2) // _mm_slli_epi32
				);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i RotateLeft(__m128i x, byte c)
				=> Or(
					Sse2.ShiftLeftLogical(x, c),// _mm_slli_epi32
					Sse2.ShiftRightLogical(x, (byte)(32 - c)) // _mm_srli_epi32
				);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Rol17(__m128i x)
				=> RotateLeft(x, 17);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Rol19(__m128i x)
				=> RotateLeft(x, 19);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static __m128i Shuffle0321(__m128i x)
				=> Sse2.Shuffle(x, (0 << 6) + (3 << 4) + (2 << 2) + (1 << 0)); // _mm_shuffle_epi32

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void swap(ref __m128i left, ref __m128i right)
			{
				var temp = left;
				left = right;
				right = temp;
			}

			public static unsafe uint Hash32(byte* s, uint len)
			{
				const uint seed = 81;
				if (len <= 24)
				{
					return len <= 12 ?
						(len <= 4 ?
						 MK.Hash32Len0to4(s, len) :
						 MK.Hash32Len5to12(s, len)) :
						MK.Hash32Len13to24(s, len);
				}

				if (len < 40)
				{
					uint a = len, b = unchecked(seed * c2), c = a + b;
					a += Fetch32(s + len - 4);
					b += Fetch32(s + len - 20);
					c += Fetch32(s + len - 16);
					uint d = a;
					a = Rotate32(a, 21);
					a = Mur(a, Mur(b, Sse42.Crc32(c, d))); // _mm_crc32_u32
					a += Fetch32(s + len - 12);
					b += Fetch32(s + len - 8);
					d += a;
					a += d;
					b = Mur(b, d) * c2;
					a = Sse42.Crc32(a, b + c); // _mm_crc32_u32
					return MK.Hash32Len13to24(s, (len + 1) / 2, a) + b;
				}

				// Murk
				// Add(k, Mul5(Rol19(Xor(Mul(Rol17(Mul(★, cc1)), cc2),(h)))))
				// Add(k, Mul5(Rot19(Xor(Mul(Rot17(Mul(d, cc1)), cc2), (h)))))


				__m128i cc1 = Vector128.Create(c1); // _mm_set1_epi32
				__m128i cc2 = Vector128.Create(c2); // _mm_set1_epi32
				__m128i h = Vector128.Create(seed); // _mm_set1_epi32
				__m128i g = Vector128.Create(unchecked(c1 * seed)); // _mm_set1_epi32
				__m128i f = g;
				__m128i k = Vector128.Create(0xe6546b64); // _mm_set1_epi32
				__m128i q;
				if (len < 80)
				{
					__m128i a = Fetch128(s);
					__m128i b = Fetch128(s + 16);
					__m128i c = Fetch128(s + (len - 15) / 2);
					__m128i d = Fetch128(s + len - 32);
					__m128i e = Fetch128(s + len - 16);
					h = Add(h, a);
					g = Add(g, b);
					q = g;
					g = Shuffle0321(g);
					f = Add(f, c);
					__m128i be = Add(b, Mul(e, cc1));
					h = Add(h, f);
					f = Add(f, h);
					h = Add(Add(k, Mul5(Rol19(Xor(Mul(Rol17(Mul(d, cc1)), cc2), (h))))), e);
					k = Xor(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); // _mm_shuffle_epi8
					g = Add(Xor(c, g), a);
					f = Add(Xor(be, f), d);
					k = Add(k, be);
					k = Add(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32()); // _mm_shuffle_epi8
					f = Add(f, g);
					g = Add(g, f);
					g = Add(
						 Vector128.Create(len), // _mm_set1_epi32
						Mul(g, cc1)
					);
				}
				else
				{
					// len >= 80
					// The following is loosely modelled after farmhashmk::Hash32.
					uint iters = (len - 1) / 80;
					len -= iters * 80;
					q = g;
					while (iters-- != 0)
					{
						// <Chunk>                      
						__m128i a = Fetch128(s);
						__m128i b = Fetch128(s + 16);
						__m128i c = Fetch128(s + 32);
						__m128i d = Fetch128(s + 48);
						__m128i e = Fetch128(s + 64);
						h = Add(h, a);
						g = Add(g, b);
						g = Shuffle0321(g);
						f = Add(f, c);
						__m128i be = Add(b, Mul(e, cc1));
						h = Add(h, f);
						f = Add(f, h);
						h = Add(h, d);
						q = Add(q, e);
						h = Rol17(h);
						h = Mul(h, cc1);
						k = Xor(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); //_mm_shuffle_epi8      
						g = Add(Xor(c, g), a);
						f = Add(Xor(be, f), d);
						swap(ref f, ref q);
						q = Aes.InverseMixColumns(q.AsByte()).AsUInt32(); // _mm_aesimc_si128                
						k = Add(k, be);
						k = Add(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32());
						f = Add(f, g);
						g = Add(g, f);
						f = Mul(f, cc1);
						// </Chunk>
						s += 80;
					}

					if (len != 0)
					{
						h = Add(h, Vector128.Create(len)); // _mm_set1_epi32
						s = s + len - 80;
						// <Chunk>                      
						__m128i a = Fetch128(s);
						__m128i b = Fetch128(s + 16);
						__m128i c = Fetch128(s + 32);
						__m128i d = Fetch128(s + 48);
						__m128i e = Fetch128(s + 64);
						h = Add(h, a);
						g = Add(g, b);
						g = Shuffle0321(g);
						f = Add(f, c);
						__m128i be = Add(b, Mul(e, cc1));
						h = Add(h, f);
						f = Add(f, h);
						h = Add(h, d);
						q = Add(q, e);
						h = Rol17(h);
						h = Mul(h, cc1);
						k = Xor(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); //_mm_shuffle_epi8      
						g = Add(Xor(c, g), a);
						f = Add(Xor(be, f), d);
						swap(ref f, ref q);
						q = Aes.InverseMixColumns(q.AsByte()).AsUInt32(); // _mm_aesimc_si128                
						k = Add(k, be);
						k = Add(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32());
						f = Add(f, g);
						g = Add(g, f);
						f = Mul(f, cc1);
						// </Chunk>
					}
				}

				g = Shuffle0321(g);
				k = Xor(k, g);
				k = Xor(k, q);
				h = Xor(h, q);
				f = Mul(f, cc1);
				k = Mul(k, cc2);
				g = Mul(g, cc1);
				h = Mul(h, cc2);
				k = Add(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); // _mm_shuffle_epi8
				h = Add(h, f);
				f = Add(f, h);
				g = Add(g, k);
				k = Add(k, g);
				k = Xor(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32()); // _mm_shuffle_epi8
				var buf = stackalloc __m128i[4];
				buf[0] = f;
				buf[1] = g;
				buf[2] = k;
				buf[3] = h;
				s = (byte*)buf;
				uint x = Fetch32(s);
				uint y = Fetch32(s + 4);
				uint z = Fetch32(s + 8);
				x = Sse42.Crc32(x, Fetch32(s + 12)); // _mm_crc32_u32
				y = Sse42.Crc32(y, Fetch32(s + 16)); // _mm_crc32_u32
				z = Sse42.Crc32(z * c1, Fetch32(s + 20)); // _mm_crc32_u32
				x = Sse42.Crc32(x, Fetch32(s + 24)); // _mm_crc32_u32
				y = Sse42.Crc32(y * c1, Fetch32(s + 28)); // _mm_crc32_u32
				uint o = y;
				z = Sse42.Crc32(z, Fetch32(s + 32)); // _mm_crc32_u32
				x = Sse42.Crc32(x * c1, Fetch32(s + 36)); // _mm_crc32_u32
				y = Sse42.Crc32(y, Fetch32(s + 40)); // _mm_crc32_u32
				z = Sse42.Crc32(z * c1, Fetch32(s + 44)); // _mm_crc32_u32
				x = Sse42.Crc32(x, Fetch32(s + 48)); // _mm_crc32_u32
				y = Sse42.Crc32(y * c1, Fetch32(s + 52)); // _mm_crc32_u32
				z = Sse42.Crc32(z, Fetch32(s + 56)); // _mm_crc32_u32
				x = Sse42.Crc32(x, Fetch32(s + 60)); // _mm_crc32_u32
				return (o - x + y - z) * c1;
			}

			public static unsafe uint Hash32WithSeed(byte* s, uint len, uint seed, uint h)
				=> Sse42.Crc32(Hash32(s + 24, len - 24) + seed, h); // _mm_crc32_u32
		}
	}
}
