// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization.Polymorphic
{
	internal sealed class PolymorphicSerializerProvider : IObjectSerializerProvider
	{
		private readonly ObjectSerializer _defaultSerializer;
		private readonly PolymorphismSchema _defaultSchema;

		public PolymorphicSerializerProvider(Type targetType, ObjectSerializer defaultSerializer)
		{
			this._defaultSerializer = defaultSerializer;
			this._defaultSchema = PolymorphismSchema.Create(targetType, null);
		}

		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
		{
			var schema = (providerParameter ?? this._defaultSchema) as PolymorphismSchema;

			if (schema == null || schema.UseDefault || schema.TargetType != targetType)
			{
				// No schema is applied or this provider is used for container but the schema is only applied for keys/items.
				if (this._defaultSerializer == null)
				{
					Throw.NotSupportedBecauseCannotInstanciateAbstractType(targetType);
					return default!; // Never reaches
				}

				// Fallback.
				return this._defaultSerializer;
			}

			if (schema.UseTypeEmbedding)
			{
				return new TypeEmbedingPolymorphicMessagePackSerializer<T>(context, schema);
			}
			else
			{
				return new KnownTypePolymorphicMessagePackSerializer<T>(context, schema);
			}
		}
	}
}
