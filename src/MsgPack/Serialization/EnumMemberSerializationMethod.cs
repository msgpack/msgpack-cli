// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents individual enum typed member serialization method.
	/// </summary>
	public enum EnumMemberSerializationMethod
	{
		/// <summary>
		///		Respects setting in enum type itself or system default.
		/// </summary>
		Default = 0,

		/// <summary>
		///		Enums are serialized with their name. It is more torelant to versioning but less efficient.
		/// </summary>
		ByName,

		/// <summary>
		///		Enums are serialized with their underlying value. It is more efficient but less torelant to versioning.
		/// </summary>
		ByUnderlyingValue
	}
}
