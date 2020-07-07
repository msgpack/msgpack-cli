// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	[Flags]
	public enum AvailableSerializationMethods
	{
		None = 0,
		Array = 0x1,
		Map = 0x2
	}
}
