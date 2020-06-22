// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
#warning TODO: NonPublic
	[Flags]
	internal enum CollectionTraitOptions
	{
		None = 0,
		WithAddMethod = 0x1,
		WithCountPropertyGetter = 0x2,
		WithGetEnumeratorMethod = 0x4,
		WithConstructor = 0x8,
		AllowNonCollectionEnumerableTypes = 0x800,
		Full = WithAddMethod | WithCountPropertyGetter | WithGetEnumeratorMethod | WithConstructor
	}
}
