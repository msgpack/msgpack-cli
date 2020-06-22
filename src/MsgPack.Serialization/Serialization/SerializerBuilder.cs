// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal abstract class SerializerBuilder
	{
		public abstract ObjectSerializer<T> Build<T>(ObjectSerializationContext context, SerializationTarget target);
	}
}
