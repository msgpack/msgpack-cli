// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.Reflection
{
	internal static class SpanMethods
	{
		private static MethodInfo GetAsReadOnlySpan<T>()
		{
			var parameterTypes = ArrayPool<Type>.Shared.Rent(1);
			try
			{
				return
					typeof(Span<>).MakeGenericType(typeof(T)).GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Single(m =>
						m.ReturnType == typeof(ReadOnlySpan<T>)
						&& m.Name == "op_Implicit"
						&& m.GetParameterTypes().SequenceEqual(parameterTypes)
					);
			}
			finally
			{
				ArrayPool<Type>.Shared.Return(parameterTypes);
			}
		}

		public static readonly MethodInfo AsReadOnlySpanByte = GetAsReadOnlySpan<byte>();

		public static readonly MethodInfo AsReadOnlySpanChar = GetAsReadOnlySpan<char>();
	}
}
