// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MsgPack
{
#if !FEATURE_UTF8_STRING

	internal sealed class Utf8String
	{
		private readonly byte[] _bytes;

		public Utf8String(ReadOnlySpan<char> value)
		{
			var bytes = new byte[Encoding.UTF8.GetByteCount(value)];

			var used = Encoding.UTF8.GetBytes(value, bytes);
			this._bytes = bytes;
		}

		public Utf8Span AsSpan() => new Utf8Span(this._bytes);
	}

	[StructLayout(LayoutKind.Auto)]
	internal readonly ref struct Utf8Span
	{
		public ReadOnlySpan<byte> Bytes { get; }

		internal Utf8Span(byte[] bytes)
		{
			this.Bytes = bytes;
		}
	}

#endif
}
