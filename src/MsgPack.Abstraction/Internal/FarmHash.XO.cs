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

namespace MsgPack.Internal
{
	internal partial class FarmHash
	{
		private static class XO
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong H32(byte* s, uint len, ulong mul,
						   ulong seed0 = 0, ulong seed1 = 0)
			{
				ulong a = Fetch64(s) * k1;
				ulong b = Fetch64(s + 8);
				ulong c = Fetch64(s + len - 8) * mul;
				ulong d = Fetch64(s + len - 16) * k2;
				ulong u = Rotate64(a + b, 43) + Rotate64(c, 30) + d + seed0;
				ulong v = a + Rotate64(b + k2, 18) + c + seed1;
				a = NA.ShiftMix((u ^ v) * mul);
				b = NA.ShiftMix((v ^ a) * mul);
				return b;
			}

			// Return an 8-byte hash for 33 to 64 bytes.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong HashLen33to64(byte* s, uint len)
			{
				ulong mul0 = k2 - 30;
				ulong mul1 = k2 - 30 + 2 * len;
				ulong h0 = H32(s, 32, mul0);
				ulong h1 = H32(s + len - 32, 32, mul1);
				return ((h1 * mul1) + h0) * mul1;
			}

			// Return an 8-byte hash for 65 to 96 bytes.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static unsafe ulong HashLen65to96(byte* s, uint len)
			{
				ulong mul0 = k2 - 114;
				ulong mul1 = k2 - 114 + 2 * len;
				ulong h0 = H32(s, 32, mul0);
				ulong h1 = H32(s + 32, 32, mul1);
				ulong h2 = H32(s + len - 32, 32, mul1, h0, h1);
				return (h2 * 9 + (h0 >> 17) + (h1 >> 21)) * mul1;
			}

			internal static unsafe ulong Hash64(byte* s, uint len)
			{
				if (len <= 32)
				{
					if (len <= 16)
					{
						return NA.HashLen0to16(s, len);
					}
					else
					{
						return NA.HashLen17to32(s, len);
					}
				}
				else if (len <= 64)
				{
					return HashLen33to64(s, len);
				}
				else if (len <= 96)
				{
					return HashLen65to96(s, len);
				}
				else if (len <= 256)
				{
					return NA.Hash64(s, len);
				}
				else
				{
					return UO.Hash64(s, len);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe ulong Hash64WithSeed(byte* s, uint len, ulong seed)
				=> UO.Hash64WithSeed(s, len, seed);
		}
	}
}
