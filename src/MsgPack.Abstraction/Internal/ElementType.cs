using System;

namespace MsgPack.Internal
{
	public enum ElementType
	{
		None = 0,
		Int32 = 1,
		Int64 = 2,
		UInt64 = 3,
		Single = 4,
		Double = 5,
		True = 6,
		False = 7,
		Null = 8,
		Array = 0x11,
		Map = 0x12,
		String = 0x31,
		Binary = 0x32,
		Extension = 0x41,
		Whitespace = 0x51,
		Comment = 0x52,
		OtherTrivia = 0x5F
	}
}
