// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Internal immutable implementation for <see cref="IDefaultConcreteTypeRepository"/>.
	/// </summary>
	internal sealed class ImmutableDefaultConcreteTypeRepository : IDefaultConcreteTypeRepository
	{
		internal static ImmutableDefaultConcreteTypeRepository Default { get; } = new DefaultConcreteTypeRepositoryBuilder().Build();

		private readonly Dictionary<RuntimeTypeHandle, object> _defaultCollectionTypes;

		internal ImmutableDefaultConcreteTypeRepository(Dictionary<RuntimeTypeHandle, object> defaultCollectionTypes)
		{
			this._defaultCollectionTypes = defaultCollectionTypes;
		}

		public Type? Get(Type abstractCollectionType)
		{
			TypeKeyRepositoryLogics.TryGet(this._defaultCollectionTypes, Ensure.NotNull(abstractCollectionType), out var concrete, out var genericDefinition);
			return concrete as Type ?? genericDefinition as Type;
		}

		internal Type? GetConcreteType(Type abstractCollectionType)
		{
			var typeOrDefinition = this.Get(abstractCollectionType);
			if (typeOrDefinition == null || !typeOrDefinition.GetIsGenericTypeDefinition() || !abstractCollectionType.GetIsGenericType())
			{
				return typeOrDefinition;
			}

			// Assume type repository has only concrete generic type definition which has same arity for abstract type.
			return typeOrDefinition.MakeGenericType(abstractCollectionType.GetGenericArguments());
		}

		public IEnumerable<KeyValuePair<RuntimeTypeHandle, object>> AsEnumerable()
			=> this._defaultCollectionTypes;
	}
}
