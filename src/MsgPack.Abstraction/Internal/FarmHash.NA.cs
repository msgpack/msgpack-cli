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

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		private static class NA
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong Hash128to64((ulong low, ulong high) x)
			{
				// Murmur-inspired hashing.
				const ulong kMul = 0x9ddfea08eb382d69UL;
				ulong a = (x.low ^ x.high) * kMul;
				a ^= (a >> 47);
				ulong b = (x.high ^ a) * kMul;
				b ^= (b >> 47);
				b *= kMul;
				return b;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe ulong ShiftMix(ulong val)
			{
				return val ^ (val >> 47);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe ulong HashLen16(ulong u, ulong v)
			{
				return Hash128to64((u, v));
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe ulong HashLen16(ulong u, ulong v, ulong mul)
			{
				// Murmur-inspired hashing.
				ulong a = (u ^ v) * mul;
				a ^= (a >> 47);
				ulong b = (v ^ a) * mul;
				b ^= (b >> 47);
				b *= mul;
				return b;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe ulong HashLen0to16(byte* s, uint len)
			{
				if (len >= 8)
				{
					ulong mul = k2 + len * 2;
					ulong a = Fetch64(s) + k2;
					ulong b = Fetch64(s + len - 8);
					ulong c = Rotate64(b, 37) * mul + a;
					ulong d = (Rotate64(a, 25) + b) * mul;
					return HashLen16(c, d, mul);
				}
				if (len >= 4)
				{
					ulong mul = k2 + len * 2;
					ulong a = Fetch32(s);
					return HashLen16(len + (a << 3), Fetch32(s + len - 4), mul);
				}
				if (len > 0)
				{
					byte a = s[0];
					byte b = s[len >> 1];
					byte c = s[len - 1];
					uint y = (uint)a + ((uint)b << 8);
					uint z = len + ((uint)c << 2);
					return ShiftMix(y * k2 ^ z * k0) * k2;
				}
				return k2;
			}

			// This probably works well for 16-byte strings as well, but it may be overkill
			// in that case.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe ulong HashLen17to32(byte* s, uint len)
			{
				ulong mul = k2 + len * 2;
				ulong a = Fetch64(s) * k1;
				ulong b = Fetch64(s + 8);
				ulong c = Fetch64(s + len - 8) * mul;
				ulong d = Fetch64(s + len - 16) * k2;
				return HashLen16(Rotate64(a + b, 43) + Rotate64(c, 30) + d,
								 a + Rotate64(b + k2, 18) + c, mul);
			}

			// Return a 16-byte hash for 48 bytes.  Quick and dirty.
			// Callers do best to use "random-looking" values for a and b.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe (ulong first, ulong second) WeakHashLen32WithSeeds(
				ulong w, ulong x, ulong y, ulong z, ulong a, ulong b)
			{
				a += w;
				b = Rotate64(b + a + z, 21);
				ulong c = a;
				a += x;
				a += y;
				b += Rotate64(a, 44);
				return (a + z, b + c);
			}

			// Return a 16-byte hash for s[0] ... s[31], a, and b.  Quick and dirty.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static unsafe (ulong first, ulong second) WeakHashLen32WithSeeds(byte* s, ulong a, ulong b)
			{
				return WeakHashLen32WithSeeds(Fetch64(s),
											  Fetch64(s + 8),
											  Fetch64(s + 16),
											  Fetch64(s + 24),
											  a,
											  b);
			}

			// Return an 8-byte hash for 33 to 64 bytes.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong HashLen33to64(byte* s, uint len)
			{
				ulong mul = k2 + len * 2;
				ulong a = Fetch64(s) * k2;
				ulong b = Fetch64(s + 8);
				ulong c = Fetch64(s + len - 8) * mul;
				ulong d = Fetch64(s + len - 16) * k2;
				ulong y = Rotate64(a + b, 43) + Rotate64(c, 30) + d;
				ulong z = HashLen16(y, a + Rotate64(b + k2, 18) + c, mul);
				ulong e = Fetch64(s + 16) * mul;
				ulong f = Fetch64(s + 24);
				ulong g = (y + Fetch64(s + len - 32)) * mul;
				ulong h = (z + Fetch64(s + len - 24)) * mul;
				return HashLen16(Rotate64(e + f, 43) + Rotate64(g, 30) + h,
								 e + Rotate64(f + a, 18) + g, mul);
			}

			public static unsafe ulong Hash64(byte* s, uint len)
			{
				const ulong seed = 81;
				if (len <= 32)
				{
					if (len <= 16)
					{
						return HashLen0to16(s, len);
					}
					else
					{
						return HashLen17to32(s, len);
					}
				}
				else if (len <= 64)
				{
					return HashLen33to64(s, len);
				}

				// For strings over 64 bytes we loop.  Internal state consists of
				// 56 bytes: v, w, x, y, and z.
				ulong x = seed;
				ulong y = unchecked(seed * k1) + 113;
				ulong z = ShiftMix(y * k2 + 113) * k2;
				(ulong first, ulong second) v = (0, 0);
				(ulong first, ulong second) w = (0, 0);
				x = x * k2 + Fetch64(s);

				// Set end so that after the loop we have 1 to 64 bytes left to process.
				byte* end = s + ((len - 1) / 64) * 64;
				byte* last64 = end + ((len - 1) & 63) - 63;
				Debug.Assert(s + len - 64 == last64);
				do
				{
					x = Rotate64(x + y + v.first + Fetch64(s + 8), 37) * k1;
					y = Rotate64(y + v.second + Fetch64(s + 48), 42) * k1;
					x ^= w.second;
					y += v.first + Fetch64(s + 40);
					z = Rotate64(z + w.first, 33) * k1;
					v = WeakHashLen32WithSeeds(s, v.second * k1, x + w.first);
					w = WeakHashLen32WithSeeds(s + 32, z + w.second, y + Fetch64(s + 16));
					swap(ref z, ref x);
					s += 64;
				} while (s != end);
				ulong mul = k1 + ((z & 0xff) << 1);
				// Make s point to the last 64 bytes of input.
				s = last64;
				w.first += ((len - 1) & 63);
				v.first += w.first;
				w.first += v.first;
				x = Rotate64(x + y + v.first + Fetch64(s + 8), 37) * mul;
				y = Rotate64(y + v.second + Fetch64(s + 48), 42) * mul;
				x ^= w.second * 9;
				y += v.first * 9 + Fetch64(s + 40);
				z = Rotate64(z + w.first, 33) * mul;
				v = WeakHashLen32WithSeeds(s, v.second * mul, x + w.first);
				w = WeakHashLen32WithSeeds(s + 32, z + w.second, y + Fetch64(s + 16));
				swap(ref z, ref x);
				return HashLen16(HashLen16(v.first, w.first, mul) + ShiftMix(y) * k0 + z,
								 HashLen16(v.second, w.second, mul) + x,
								 mul);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64WithSeed(byte* s, uint len, ulong seed)
				=> Hash64WithSeeds(s, len, k2, seed);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64WithSeeds(byte* s, uint len, ulong seed0, ulong seed1)
				=> HashLen16(Hash64(s, len) - seed0, seed1);
		}
	}
}
