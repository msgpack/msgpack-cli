// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal sealed class ReflectionSerializerBuilder : SerializerBuilder
	{
		public sealed override ObjectSerializer<T> Build<T>(ObjectSerializationContext context, in SerializationTarget target, ISerializerGenerationOptions options)
			=> new ReflectionSerializer<T>(context, target, options);
	}
}
