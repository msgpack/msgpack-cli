// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents event data of <see cref="SerializerProvider.ResolveObjectSerializer"/> event.
	/// </summary>
	public struct ResolveObjectSerializerEventArgs
	{
		public SerializerProvider Source { get; }
		public Type TargetType { get; }
		public PolymorphismSchema PolymorphismSchema { get; }

		private ObjectSerializer? _serializer;

		internal ObjectSerializer? GetSerializer() => this._serializer;

		public void SetSerializer(ObjectSerializer serializer)
			=> this._serializer = Ensure.NotNull(serializer);

		internal ResolveObjectSerializerEventArgs(SerializerProvider source, Type targetType, PolymorphismSchema polymorphismSchema)
		{
			this.Source = source;
			this.TargetType = targetType;
			this.PolymorphismSchema = polymorphismSchema;
			this._serializer = null;
		}
	}
}
