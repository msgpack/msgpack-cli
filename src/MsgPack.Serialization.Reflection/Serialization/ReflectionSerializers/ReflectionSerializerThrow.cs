// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal static class ReflectionSerializerThrow
	{
		public static Delegate UnexpectedMemberType(MemberInfo member)
			=> throw new Exception($"Assertion failure: unexpected MemberInfo type '{member}'({member.GetType()})");

		public static void UnexpectedEncoderDelegate(Delegate @delegate)
			=> throw new Exception($"Assertion failure: unexpected encoder delegate '{@delegate}'.");

		public static void UnexpectedDecoderDelegate(Delegate @delegate)
			=> throw new Exception($"Assertion failure: unexpected encoder delegate '{@delegate}'.");

		public static Delegate ByRefLikeIsNotSupported(MemberInfo member)
			// This should be able to be handled in future runtime...
			=> throw new PlatformNotSupportedException($"Cannot deserialize {member} because its type ({member.GetMemberValueType()}) is \"by-ref\" type. Reflection based serializer cannot handle it in current runtime.");
	}
}
