// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents options for custom or pre-generated serializer registration.
	/// </summary>
	[Flags]
	public enum SerializerRegistrationOptions
	{
		/// <summary>
		///		None of options are applied.
		/// </summary>
		None = 0,

		/// <summary>
		///		Overrides existing registration with specified serializer.
		/// </summary>
		AllowOverride = 0x1,

#if !UNITY
#warning TODO: Test in newest Unity
		// Unity causes AOT error for auto nullable registration...
		/// <summary>
		///		For non-nullable value type, registering nullable companion simulary.
		/// </summary>
		WithNullable = 0x2
#endif // !UNITY
	}
}
