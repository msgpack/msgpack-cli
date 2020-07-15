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

using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using __m128i = System.Runtime.Intrinsics.Vector128<uint>;

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		internal static class SA
		{
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
					a = Mur(a, Mur(b, Mur(c, d)));
					a += Fetch32(s + len - 12);
					b += Fetch32(s + len - 8);
					d += a;
					a += d;
					b = Mur(b, d) * c2;
					a = Sse42.Crc32(a, b + c); // _mm_crc32_u32
					return MK.Hash32Len13to24(s, (len + 1) / 2, a) + b;
				}

				__m128i cc1 = Vector128.Create(c1); // _mm_set1_epi32
				__m128i cc2 = Vector128.Create(c2); // _mm_set1_epi32
				__m128i h = Vector128.Create(seed); // _mm_set1_epi32
				__m128i g = Vector128.Create(unchecked(c1 * seed)); // _mm_set1_epi32
				__m128i f = g;
				__m128i k = Vector128.Create(0xe6546b64); // _mm_set1_epi32
				if (len < 80)
				{
					__m128i a = Fetch128(s);
					__m128i b = Fetch128(s + 16);
					__m128i c = Fetch128(s + (len - 15) / 2);
					__m128i d = Fetch128(s + len - 32);
					__m128i e = Fetch128(s + len - 16);
					h = Add(h, a);
					g = Add(g, b);
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
						h = Add(Add(k, Mul5(Rol19(Xor(Mul(Rol17(Mul(d, cc1)), cc2), (h))))), e);
						k = Xor(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); // _mm_shuffle_epi8
						g = Add(Xor(c, g), a);
						f = Add(Xor(be, f), d);
						k = Add(k, be);
						k = Add(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32()); // _mm_shuffle_epi8
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
						h = Add(Add(k, Mul5(Rol19(Xor(Mul(Rol17(Mul(d, cc1)), cc2), (h))))), e);
						k = Xor(k, Ssse3.Shuffle(g.AsByte(), f.AsByte()).AsUInt32()); // _mm_shuffle_epi8
						g = Add(Xor(c, g), a);
						f = Add(Xor(be, f), d);
						k = Add(k, be);
						k = Add(k, Ssse3.Shuffle(f.AsByte(), h.AsByte()).AsUInt32()); // _mm_shuffle_epi8
						f = Add(f, g);
						g = Add(g, f);
						f = Mul(f, cc1);
						// </Chunk>
					}
				}

				g = Shuffle0321(g);
				k = Xor(k, g);
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
				s = (byte*)(buf);
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
