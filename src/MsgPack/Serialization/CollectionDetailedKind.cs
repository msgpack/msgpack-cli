// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	internal enum CollectionDetailedKind
	{
		NotCollection = 0,
		Array,
		GenericList,
		NonGenericList,
		GenericDictionary,
		NonGenericDictionary,
#if !NET35
		GenericSet,
#endif // !NET35
		GenericCollection,
		NonGenericCollection,
		GenericEnumerable,
		NonGenericEnumerable,
#if !NET35 && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
		GenericReadOnlyList,
		GenericReadOnlyCollection,
		GenericReadOnlyDictionary,
#endif // !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		Unserializable
	}
}
