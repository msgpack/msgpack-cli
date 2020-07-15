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

using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

using RandomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator;

namespace MsgPack.Internal
{
	internal static partial class FarmHash
	{
		private const uint c1 = 0xcc9e2d51;
		private const uint c2 = 0x1b873593;
		private const ulong k0 = 0xc3a5c85c97cb3127UL;
		private const ulong k1 = 0xb492b66fbe98f273UL;
		private const ulong k2 = 0x9ae16a3b2f90404fUL;

		/// <summary>
		///		Hash function for a byte array. 
		///		May change from time to time, may differ on different platforms.
		/// </summary>
		/// <param name="bytes">Target byte span.</param>
		/// <returns>Non-cryptgraphic, temporal hash value suitable as hash code.</returns>
		public static unsafe int Hash32WithSeed(ReadOnlySpan<byte> bytes, uint seed)
		{
			var len = (uint)bytes.Length;
			unchecked
			{
				fixed (byte* s = bytes)
				{
					if (Sse41.IsSupported && RuntimeInformation.ProcessArchitecture == Architecture.X64)
					{
						return (int)NT.Hash32WithSeed(s, len, seed);
					}

					if (Sse42.IsSupported)
					{
						// farmhashsa::Hash32WithSeed and farmshahsu::Hash32WithSeed is same for less than or equal to 24bytes,
						// so inline here to reduce code size.
						if (len <= 24)
						{
							if (len >= 13)
							{
								return (int)MK.Hash32Len13to24(s, len, seed * c1);
							}
							else if (len >= 5)
							{
								return (int)MK.Hash32Len5to12(s, len, seed);
							}
							else
							{
								return (int)MK.Hash32Len0to4(s, len, seed);
							}
						}

						// Calculation of h is also common.
						uint h = MK.Hash32Len13to24(s, 24, seed ^ len);

						if (Aes.IsSupported)
						{
							return (int)SU.Hash32WithSeed(s, (uint)bytes.Length, seed, h);
						}
						else
						{
							return (int)SA.Hash32WithSeed(s, (uint)bytes.Length, seed, h);
						}
					}

					return (int)MK.Hash32WithSeed(s, (uint)bytes.Length, seed);
				}
			}
		}

		public static readonly uint DefaultSeed = InitializeRandomSeed();

		private static uint InitializeRandomSeed()
		{
			using var rng = RandomNumberGenerator.Create();
			Span<byte> bytes = stackalloc byte[sizeof(uint)];
			rng.GetBytes(bytes);
			return BinaryPrimitives.ReadUInt32LittleEndian(bytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe uint Fetch32(byte* p)
			=> *(uint*)p;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Rotate32(uint val, int shift)
			// Avoid shifting by 32: doing so yields an undefined result.
			=> shift == 0 ? val : ((val >> shift) | (val << (32 - shift)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Fmix(uint h)
		{
			h ^= h >> 16;
			h *= 0x85ebca6b;
			h ^= h >> 13;
			h *= 0xc2b2ae35;
			h ^= h >> 16;
			return h;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Mur(uint a, uint h)
		{
			// Helper from Murmur3 for combining two 32-bit values.
			a *= c1;
			a = Rotate32(a, 17);
			a *= c2;
			h ^= a;
			h = Rotate32(h, 19);
			return h * 5 + 0xe6546b64;
		}
	}
}

