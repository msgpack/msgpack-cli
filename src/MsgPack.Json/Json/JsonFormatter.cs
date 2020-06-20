// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines common JSON formatting logic.
	/// </summary>
	internal static class JsonFormatter
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static void WriteNull(IBufferWriter<byte> buffer)
		{
			var span = buffer.GetSpan(4);
			span[0] = (byte)'n';
			span[1] = (byte)'u';
			span[2] = (byte)'l';
			span[3] = (byte)'l';
			buffer.Advance(4);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static void Format(float value, IBufferWriter<byte> buffer)
		{
			var span = buffer.GetSpan(0);
			while (!Utf8Formatter.TryFormat(value, span, out _))
			{
				span = buffer.GetSpan((span.Length + 1) * 2);
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static void Format(double value, IBufferWriter<byte> buffer)
		{
			var span = buffer.GetSpan(0);
			while (!Utf8Formatter.TryFormat(value, span, out _))
			{
				span = buffer.GetSpan((span.Length + 1) * 2);
			}
		}
	}
}
