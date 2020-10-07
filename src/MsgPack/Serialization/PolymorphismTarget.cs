// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Specifies <see cref="MessagePackKnownTypeAttribute"/> or <see cref="MessagePackRuntimeTypeAttribute"/> target.
	/// </summary>
	internal enum PolymorphismTarget
	{
		/// <summary>
		///		Applies to member type itself.
		///		This option disables <see cref="SerializerProvider.DefaultCollectionTypes"/> settings.
		/// </summary>
		Member = 0,

		/// <summary>
		///		Applies to items of collection member type (values for dictionary).
		///		This options causes entire attribute will be ignored for non-collection types.
		/// </summary>
		CollectionItem = 1,

		/// <summary>
		///		Applies to keys of dictionary member type.
		///		This options causes entire attribute will be ignored for non-dictionary types.
		/// </summary>
		DictionaryKey = 2,

		/// <summary>
		///		Applies to keys of dictionary member type.
		///		This options causes entire attribute will be ignored for non-dictionary types.
		/// </summary>
		TupleItem = 3,
	}
}
