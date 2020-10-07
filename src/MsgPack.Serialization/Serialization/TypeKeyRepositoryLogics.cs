// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines common logics for <see cref="TypeKeyRepository"/> compatible implementation.
	/// </summary>
	internal static class TypeKeyRepositoryLogics
	{
		/// <summary>
		///		Tries to get matched object for the key or for generic definition of the key from specified dictionary.
		/// </summary>
		/// <param name="table">A ditionary which may contain target item.</param>
		/// <param name="type">Key for extraction.</param>
		/// <param name="matched">Matched value for <paramref name="type"/> itself. <c>null</c> when not matched.</param>
		/// <param name="genericDefinitionMatched">Matched value for generic definition of <paramref name="type"/>. <c>null</c> when not matched.</param>
		/// <returns>
		///		<c>true</c> when found, that is either <paramref name="matched"/> or <paramref name="genericDefinitionMatched"/> will be non-null value;
		///		<c>false</c> otherwise, that is both of <paramref name="matched"/> and <paramref name="genericDefinitionMatched"/> are <c>null</c>.
		///	</returns>
		public static bool TryGet(IReadOnlyDictionary<RuntimeTypeHandle, object> table, Type type, out object? matched, out object? genericDefinitionMatched)
		{
			if (table.TryGetValue(type.TypeHandle, out var result))
			{
				matched = result;
				genericDefinitionMatched = null;
				return true;
			}

			if (type.GetIsGenericType())
			{
				if (table.TryGetValue(type.GetGenericTypeDefinition().TypeHandle, out result))
				{
					matched = null;
					genericDefinitionMatched = result;
					return true;
				}
			}

			matched = null;
			genericDefinitionMatched = null;
			return false;
		}
	}
}
