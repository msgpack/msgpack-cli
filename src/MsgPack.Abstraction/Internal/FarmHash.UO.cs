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
		private static class UO
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong H(ulong x, ulong y, ulong mul, int r)
			{
				ulong a = (x ^ y) * mul;
				a ^= (a >> 47);
				ulong b = (y ^ a) * mul;
				return Rotate64(b, r) * mul;
			}

			private static unsafe ulong Hash64WithSeeds(byte* s, uint len,
							 ulong seed0, ulong seed1)
			{
				if (len <= 64)
				{
					return NA.Hash64WithSeeds(s, len, seed0, seed1);
				}

				// For strings over 64 bytes we loop.  Internal state consists of
				// 64 bytes: u, v, w, x, y, and z.
				ulong x = seed0;
				ulong y = seed1 * k2 + 113;
				ulong z = NA.ShiftMix(y * k2) * k2;
				(ulong first, ulong second) v = (seed0, seed1);
				(ulong first, ulong second) w = (0, 0);
				ulong u = x - z;
				x *= k2;
				ulong mul = k2 + (u & 0x82);

				// Set end so that after the loop we have 1 to 64 bytes left to process.
				byte* end = s + ((len - 1) / 64) * 64;
				byte* last64 = end + ((len - 1) & 63) - 63;
				Debug.Assert(s + len - 64 == last64);
				do
				{
					ulong a0 = Fetch64(s);
					ulong a1 = Fetch64(s + 8);
					ulong a2 = Fetch64(s + 16);
					ulong a3 = Fetch64(s + 24);
					ulong a4 = Fetch64(s + 32);
					ulong a5 = Fetch64(s + 40);
					ulong a6 = Fetch64(s + 48);
					ulong a7 = Fetch64(s + 56);
					x += a0 + a1;
					y += a2;
					z += a3;
					v.first += a4;
					v.second += a5 + a1;
					w.first += a6;
					w.second += a7;

					x = Rotate64(x, 26);
					x *= 9;
					y = Rotate64(y, 29);
					z *= mul;
					v.first = Rotate64(v.first, 33);
					v.second = Rotate64(v.second, 30);
					w.first ^= x;
					w.first *= 9;
					z = Rotate64(z, 32);
					z += w.second;
					w.second += z;
					z *= 9;
					swap(ref u, ref y);

					z += a0 + a6;
					v.first += a2;
					v.second += a3;
					w.first += a4;
					w.second += a5 + a6;
					x += a1;
					y += a7;

					y += v.first;
					v.first += x - y;
					v.second += w.first;
					w.first += v.second;
					w.second += x - y;
					x += w.second;
					w.second = Rotate64(w.second, 34);
					swap(ref u, ref z);
					s += 64;
				} while (s != end);
				// Make s point to the last 64 bytes of input.
				s = last64;
				u *= 9;
				v.second = Rotate64(v.second, 28);
				v.first = Rotate64(v.first, 20);
				w.first += ((len - 1) & 63);
				u += y;
				y += u;
				x = Rotate64(y - x + v.first + Fetch64(s + 8), 37) * mul;
				y = Rotate64(y ^ v.second ^ Fetch64(s + 48), 42) * mul;
				x ^= w.second * 9;
				y += v.first + Fetch64(s + 40);
				z = Rotate64(z + w.first, 33) * mul;
				v = NA.WeakHashLen32WithSeeds(s, v.second * mul, x + w.first);
				w = NA.WeakHashLen32WithSeeds(s + 32, z + w.second, y + Fetch64(s + 16));
				return H(NA.HashLen16(v.first + x, w.first ^ y, mul) + z - u,
						 H(v.second + y, w.second + z, k2, 30) ^ x,
						 k2,
						 31);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64WithSeed(byte* s, uint len, ulong seed) 
				=> len <= 64 ?
					NA.Hash64WithSeed(s, len, seed) :
					Hash64WithSeeds(s, len, 0, seed);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64(byte* s, uint len)
				=> len <= 64 ?
					NA.Hash64(s, len) :
					Hash64WithSeeds(s, len, 81, 0);
		}
	}
}
