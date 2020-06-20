// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack;

namespace System.Text
{
	internal static class Throw
	{
		public static void TooLargeByteLengthForString(string encodingName)
			=> throw new MessageTypeException($"Input is too large for encoding '{encodingName}' to decode.");
	}
}
