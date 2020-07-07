// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	public sealed class ObjectSerializationContext
	{
		public SerializationOptions Options { get; }

		public ObjectSerializer<T> GetSerializer<T>(object? providerParameter)
		{
			throw new NotImplementedException();
		}

		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
		{
			throw new NotImplementedException();
		}
	}
}
