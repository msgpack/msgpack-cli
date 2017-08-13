#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;

namespace MsgPack
{
	/// <summary>
	///		An encoded value reading result.
	/// </summary>
	internal enum ReadValueResult
	{
		// BitMask:
		// +-------+------------+------+-----+
		// | 31-24 |   23 - 16  | 15-8 | 7-0 |
		// +-------+------------+------+-----+
		// | Flags | (reserved) | TP-C | V-L |
		// +-------+------------+------+-----+
		// Flags:
		//   7: failure -- indicates read failure
		//   6-2: reserved.
		//   1: bool -- indicates this type is boolean
		//   0: nil -- indicates this type is nil
		//   So,
		//     0x8000FFFF : failure (Unexpected)
		//     0x800000C1 : failure (0xC1)
		//     0x80000E0F : failure (EoF)
		//     0x02000001 : true
		//     0x02000000 : false
		//     0x01000000 : nil
		//     0x00000000-0x0000FFFF : None(15-0 bits are valid)
		//     other bits are "reserved"
		// TP-C: TypeCode[below]
		// VL: Value Or Length
		//
		// TypeCode:
		// 0xFF is Faiulre, 0x00 is Immediate
		// +---+-----+-----+
		// | 7 | 6-4 | 3-0 |
		// +---+-----+-----+
		// | F | Typ | Len |
		// +---+-----+-----+
		// F: indicates failure (effectivery "reserved" bit)
		// Typ:
		//   0000 uint
		//   0001 int
		//   0010 real
		//   0100 bin
		//   0101 raw
		//   0110 ext
		//   1000 arr
		//   1001 map
		// Len: Length of sucessor length bytes.
		// 
		UInt8Type		= ( 0x00 | 0x1 ) << 8,		// 0b00000001 << 8
		UInt16Type		= ( 0x00 | 0x2 ) << 8,		// 0b00000010 << 8
		UInt32Type		= ( 0x00 | 0x4 ) << 8,		// 0b00000100 << 8
		UInt64Type		= ( 0x00 | 0x8 ) << 8,		// 0b00001000 << 8
		Int8Type		= ( 0x10 | 0x1 ) << 8,		// 0b00010001 << 8
		Int16Type		= ( 0x10 | 0x2 ) << 8,		// 0b00010010 << 8
		Int32Type		= ( 0x10 | 0x4 ) << 8,		// 0b00010100 << 8
		Int64Type		= ( 0x10 | 0x8 ) << 8,		// 0b00011000 << 8
		Real32Type		= ( 0x20 | 0x4 ) << 8,		// 0b00100100 << 8
		Real64Type		= ( 0x20 | 0x8 ) << 8,		// 0b00100100 << 8
		Bin8Type		= ( 0x40 | 0x1 ) << 8,		// 0b01010001 << 8
		Bin16Type		= ( 0x40 | 0x2 ) << 8,		// 0b01010010 << 8
		Bin32Type		= ( 0x40 | 0x4 ) << 8,		// 0b01010100 << 8
		FixExtType		= ( 0x80 | 0x0 ) << 8,		// 0b10000000 << 8
		Ext8Type		= ( 0x80 | 0x1 ) << 8,		// 0b10000001 << 8
		Ext16Type		= ( 0x80 | 0x2 ) << 8,		// 0b10000010 << 8
		Ext32Type		= ( 0x80 | 0x4 ) << 8,		// 0b10000100 << 8
		FixArrayType	= ( 0xA0 | 0x0 ) << 8,		// 0b10100000 << 8
		Array16Type		= ( 0xA0 | 0x2 ) << 8,		// 0b10100010 << 8
		Array32Type		= ( 0xA0 | 0x4 ) << 8,		// 0b10100100 << 8
		FixMapType		= ( 0xB0 | 0x0 ) << 8,		// 0b10110000 << 8
		Map16Type		= ( 0xB0 | 0x2 ) << 8,		// 0b10110010 << 8
		Map32Type		= ( 0xB0 | 0x4 ) << 8,		// 0b10110100 << 8
		FixStrType		= ( 0xC0 | 0x0 ) << 8,		// 0b11000000 << 8
		Str8Type		= ( 0xC0 | 0x1 ) << 8,		// 0b11000001 << 8
		Str16Type		= ( 0xC0 | 0x2 ) << 8,		// 0b11000010 << 8
		Str32Type		= ( 0xC0 | 0x4 ) << 8,		// 0b11000100 << 8
		Nil				= 0x1 << 24,				// 0x01000000
		False			= 0x2 << 24,				// 0x02000000
		True			= ( 0x2 << 24 ) | 1,		// 0x02000001
		// Error
		InvalidCode		= ( 0x8 << 24 ) | 0xC1,		// 0b1000 << 24 | 0xC1
		EoF				= ( 0x8 << 24 ) | 0xE0F,	// 0b1000 << 24 | 0xE0F // (EoF)
		Unexpected		= ( 0x8 << 24 ) | 0xFFFF,	// 0b1000 << 24 | 0xFFFF
		// Mask
		NonScalarBitMask	= 0x0000C000, 
		ArrayTypeMask		= 0x0000A000,
		MapTypeMask			= 0x0000B000,
		BinTypeMask			= 0x00006000,
		RawTypeMask			= 0x00004000, 
		ExtTypeMask			= 0x00008000,
		LengthOfLengthMask = 0x0F << 8,
		ValueOrLengthMask = 0xFF,
		TypeCodeMask = 0xFF << 8,
		FlagsMask = unchecked( ( int )( 0xFF000000 ) ),
		FlagsAndTypeCodeMask = FlagsMask | TypeCodeMask,
		FlagsAndLengthOfLengthMask = FlagsMask | LengthOfLengthMask
	}
}
