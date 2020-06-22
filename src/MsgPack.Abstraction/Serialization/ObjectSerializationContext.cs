// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
#warning TODO: Remove UGLY TExtensionType
	public sealed class ObjectSerializationContext
	{
		public ObjectSerializer<T> GetSerializer<T>(object providerParameter)
		{
			throw new NotImplementedException();
		}
	}
}
