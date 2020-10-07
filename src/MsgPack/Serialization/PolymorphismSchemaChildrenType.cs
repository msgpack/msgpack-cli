// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents children items type of <see cref="PolymorphismSchema"/>
	/// </summary>
	internal enum PolymorphismSchemaChildrenType
	{
		/// <summary>
		///		Leaf, that is no children schema.
		/// </summary>
		None = 0,

		/// <summary>
		///		Collection items, so children count is 1.
		/// </summary>
		CollectionItems,

		/// <summary>
		///		Dictionary keys and values, so children count is 2, index 0 is for keys, 1 is for values.
		/// </summary>
		DictionaryKeyValues,

#if !NET35 && !UNITY
		/// <summary>
		///		Tuple items, so chidren count is equal to tuple's arity.
		/// </summary>
		TupleItems
#endif // !NET35 && !UNITY
	}
}
