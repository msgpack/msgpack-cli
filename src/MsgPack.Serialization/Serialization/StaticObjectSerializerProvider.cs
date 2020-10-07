// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Simple, static provider of <see cref="ObjectSerializer"/>.
	/// </summary>
	internal sealed class StaticObjectSerializerProvider : IObjectSerializerProvider
	{
		private readonly ObjectSerializer _serializer;

		public StaticObjectSerializerProvider(ObjectSerializer serialzier)
		{
			this._serializer = serialzier;
		}

		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
			=> this._serializer;
	}
}
