// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Serialization.Internal
{
	internal struct CollectionTypeAndCount
	{
		public CollectionType CollectionType { get; }
		public int ItemsCount { get; }

		public CollectionTypeAndCount(CollectionType collectionType, int itemsCount)
		{
			this.CollectionType = collectionType;
			this.ItemsCount = itemsCount;
		}
	}
}
