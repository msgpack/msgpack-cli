// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Buffers.Text;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines common JSON formatting logic.
	/// </summary>
	internal static class JsonFormatter
	{
		public static void WriteNull(IBufferWriter<byte> buffer)
			=> buffer.Write(JsonTokens.Null);

		public static void Format(float value, IBufferWriter<byte> buffer, JsonEncoderOptions options)
		{
			var span = buffer.GetSpan(0);
			while (!Utf8Formatter.TryFormat(value, span, out _))
			{
				span = buffer.GetSpan((span.Length + 1) * 2);
			}
		}

		public static void Format(double value, IBufferWriter<byte> buffer, JsonEncoderOptions options)
		{
			var span = buffer.GetSpan(0);
			while (!Utf8Formatter.TryFormat(value, span, out _))
			{
				span = buffer.GetSpan((span.Length + 1) * 2);
			}
		}
	}
}
