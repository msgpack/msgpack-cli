// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	// This is a custom struct instead of raw MessagePackMemberAttribute to reduce GC.
	/// <summary>
	///		Represents a data of <see cref="MessagePackIgnoreAttribute"/>.
	/// </summary>
	public readonly struct MessagePackIgnoreAttributeData
	{
	}
}
