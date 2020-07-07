// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;

namespace MsgPack.Serialization.Internal
{
	internal struct CollectionTypeAndIterator
	{
		public CollectionType CollectionType { get; }
		public CollectionItemIterator Iterator { get; }

		public CollectionTypeAndIterator(CollectionType collectionType, CollectionItemIterator iterator)
		{
			this.CollectionType = collectionType;
			this.Iterator = iterator;
		}
	}
}
