// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	internal enum CollectionKind
	{
		NotCollection = 0,
		Array,
		Map,
		Unserializable
	}
}
