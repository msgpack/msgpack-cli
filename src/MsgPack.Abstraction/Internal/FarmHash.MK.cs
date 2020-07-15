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

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		internal static class MK
		{
			internal static unsafe uint Hash32Len13to24(byte* s, uint len, uint seed = 0)
			{
				uint a = Fetch32(s - 4 + (len >> 1));
				uint b = Fetch32(s + 4);
				uint c = Fetch32(s + len - 8);
				uint d = Fetch32(s + (len >> 1));
				uint e = Fetch32(s);
				uint f = Fetch32(s + len - 4);
				uint h = d * c1 + len + seed;
				a = Rotate32(a, 12) + f;
				h = Mur(c, h) + a;
				a = Rotate32(a, 3) + c;
				h = Mur(e, h) + a;
				a = Rotate32(a + f, 12) + d;
				h = Mur(b ^ seed, h) + a;
				return Fmix(h);
			}

			internal static unsafe uint Hash32Len0to4(byte* s, uint len, uint seed = 0)
			{
				uint b = seed;
				uint c = 9;
				for (var i = 0; i < len; i++)
				{
					var v = s[i];
					b = b * c1 + v;
					c ^= b;
				}
				return Fmix(Mur(b, Mur(len, c)));
			}

			internal static unsafe uint Hash32Len5to12(byte* s, uint len, uint seed = 0)
			{
				uint a = len, b = len * 5, c = 9, d = b + seed;
				a += Fetch32(s);
				b += Fetch32(s + len - 4);
				c += Fetch32(s + ((len >> 1) & 4));
				return Fmix(seed ^ Mur(c, Mur(b, Mur(a, d))));
			}

			public static unsafe uint Hash32(byte* s, uint len)
			{
				if (len <= 24)
				{
					return len <= 12 ?
						(len <= 4 ? Hash32Len0to4(s, len) : Hash32Len5to12(s, len)) :
						Hash32Len13to24(s, len);
				}

				// len > 24
				uint h = len, g = c1 * len, f = g;
				uint a0 = Rotate32(Fetch32(s + len - 4) * c1, 17) * c2;
				uint a1 = Rotate32(Fetch32(s + len - 8) * c1, 17) * c2;
				uint a2 = Rotate32(Fetch32(s + len - 16) * c1, 17) * c2;
				uint a3 = Rotate32(Fetch32(s + len - 12) * c1, 17) * c2;
				uint a4 = Rotate32(Fetch32(s + len - 20) * c1, 17) * c2;
				h ^= a0;
				h = Rotate32(h, 19);
				h = h * 5 + 0xe6546b64;
				h ^= a2;
				h = Rotate32(h, 19);
				h = h * 5 + 0xe6546b64;
				g ^= a1;
				g = Rotate32(g, 19);
				g = g * 5 + 0xe6546b64;
				g ^= a3;
				g = Rotate32(g, 19);
				g = g * 5 + 0xe6546b64;
				f += a4;
				f = Rotate32(f, 19) + 113;
				uint iters = (len - 1) / 20;
				do
				{
					uint a = Fetch32(s);
					uint b = Fetch32(s + 4);
					uint c = Fetch32(s + 8);
					uint d = Fetch32(s + 12);
					uint e = Fetch32(s + 16);
					h += a;
					g += b;
					f += c;
					h = Mur(d, h) + e;
					g = Mur(c, g) + a;
					f = Mur(b + e * c1, f) + d;
					f += g;
					g += f;
					s += 20;
				} while (--iters != 0);
				g = Rotate32(g, 11) * c1;
				g = Rotate32(g, 17) * c1;
				f = Rotate32(f, 11) * c1;
				f = Rotate32(f, 17) * c1;
				h = Rotate32(h + g, 19);
				h = h * 5 + 0xe6546b64;
				h = Rotate32(h, 17) * c1;
				h = Rotate32(h + f, 19);
				h = h * 5 + 0xe6546b64;
				h = Rotate32(h, 17) * c1;
				return h;
			}

			public static unsafe uint Hash32WithSeed(byte* s, uint len, uint seed)
			{
				if (len <= 24)
				{
					if (len >= 13)
					{
						return Hash32Len13to24(s, len, seed * c1);
					}
					else if (len >= 5)
					{
						return Hash32Len5to12(s, len, seed);
					}
					else
					{
						return Hash32Len0to4(s, len, seed);
					}
				}

				uint h = Hash32Len13to24(s, 24, seed ^ len);
				return Mur(Hash32(s + 24, len - 24) + seed, h);
			}
		}
	}
}
