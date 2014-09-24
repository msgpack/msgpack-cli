#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
	internal static class MessagePackCode
	{
		public const int NilValue = 0xc0;
		public const int TrueValue = 0xc3;
		public const int FalseValue = 0xc2;
		public const int SignedInt8 = 0xd0;
		public const int UnsignedInt8 = 0xcc;
		public const int SignedInt16 = 0xd1;
		public const int UnsignedInt16 = 0xcd;
		public const int SignedInt32 = 0xd2;
		public const int UnsignedInt32 = 0xce;
		public const int SignedInt64 = 0xd3;
		public const int UnsignedInt64 = 0xcf;
		public const int Real32 = 0xca;
		public const int Real64 = 0xcb;
		public const int MinimumFixedArray = 0x90;
		public const int MaximumFixedArray = 0x9f;
		public const int Array16 = 0xdc;
		public const int Array32 = 0xdd;
		public const int MinimumFixedMap = 0x80;
		public const int MaximumFixedMap = 0x8f;
		public const int Map16 = 0xde;
		public const int Map32 = 0xdf;
		public const int MinimumFixedRaw = 0xa0;
		public const int MaximumFixedRaw = 0xbf;
		public const int Str8 = 0xd9;
		public const int Raw16 = 0xda;
		public const int Raw32 = 0xdb;
		public const int Bin8 = 0xc4;
		public const int Bin16 = 0xc5;
		public const int Bin32 = 0xc6;
		public const int Ext8 = 0xc7;
		public const int Ext16 = 0xc8;
		public const int Ext32 = 0xc9;
		public const int FixExt1 = 0xd4;
		public const int FixExt2 = 0xd5;
		public const int FixExt4 = 0xd6;
		public const int FixExt8 = 0xd7;
		public const int FixExt16 = 0xd8;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Switch for many cases" )]
		public static string ToString( int code )
		{
			if( code < 0x80)
			{
				return "PositiveFixNum";
			}
			else if( code >= 0xE0)
			{
				return "NegativeFixNum";
			}

			switch( code )
			{
				case 0xC0:
				{
					return "Nil";
				}
				case 0xC3:
				{
					return "True";
				}
				case 0xD0:
				{
					return "SingnedInt8";
				}
				case 0xCC:
				{
					return "UnsignedInt8";
				}
				case 0xD1:
				{
					return "SignedInt16";
				}
				case 0xCD:
				{
					return "UnsignedInt16";
				}
				case 0xD2:
				{
					return "SignedInt32";
				}
				case 0xCE:
				{
					return "UnsignedInt32";
				}
				case 0xD3:
				{
					return "SignedInt64";
				}
				case 0xCF:
				{
					return "UnsignedInt64";
				}
				case 0xCA:
				{
					return "Real32";
				}
				case 0xCB:
				{
					return "Real64";
				}
				case 0xDC:
				{
					return "Array16";
				}
				case 0xDD:
				{
					return "Array32";
				}
				case 0xDE:
				{
					return "Map16";
				}
				case 0xDF:
				{
					return "Map32";
				}
				case 0xDA:
				{
					return "Raw16";
				}
				case 0xDB:
				{
					return "Raw32";
				}
			}

			switch( ( code & 0xF0))
			{
				case 0x80:
				{
					return "FixedMap";
				}
				case 0x90:
				{
					return "FixedArray";
				}
				case 0xA0:
				case 0xB0:
				{
					return "FixedRaw";
				}
			}

			return "Unknown";
		}
	}
}
