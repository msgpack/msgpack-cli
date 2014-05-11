#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	public enum EnumDefault
	{
		None = 0,
		Foo = 1
	}

	[MessagePackEnum( SerializationMethod = EnumSerializationMethod.ByName )]
	public enum EnumByName
	{
		None = 0,
		Foo = 1
	}

	[MessagePackEnum( SerializationMethod = EnumSerializationMethod.ByUnderlyingValue )]
	public enum EnumByUnderlyingValue
	{
		None = 0,
		Foo = 1
	}

#pragma warning disable 3009

	public enum EnumByte : byte
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumByteFlags : byte
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumSByte : sbyte
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumSByteFlags : sbyte
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumInt16 : short
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumInt16Flags : short
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumUInt16 : ushort
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumUInt16Flags : ushort
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumInt32 : int
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumInt32Flags : int
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumUInt32 : uint
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumUInt32Flags : uint
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumInt64 : long
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumInt64Flags : long
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}

	public enum EnumUInt64 : ulong
	{
		None = 0,
		Foo = 1
	}

	[Flags]
	public enum EnumUInt64Flags : ulong
	{
		None = 0,
		Foo = 0x1,
		Bar = 0x2
	}
#pragma warning restore 3009

	public sealed class EnumMemberObject
	{


		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.Default )]
		public EnumDefault DefaultDefaultProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByName )]
		public EnumDefault DefaultByNameProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByUnderlyingValue )]
		public EnumDefault DefaultByUnderlyingValueProperty { get; set; }


		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.Default )]
		public EnumByName ByNameDefaultProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByName )]
		public EnumByName ByNameByNameProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByUnderlyingValue )]
		public EnumByName ByNameByUnderlyingValueProperty { get; set; }


		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.Default )]
		public EnumByUnderlyingValue ByUnderlyingValueDefaultProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByName )]
		public EnumByUnderlyingValue ByUnderlyingValueByNameProperty { get; set; }

		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByUnderlyingValue )]
		public EnumByUnderlyingValue ByUnderlyingValueByUnderlyingValueProperty { get; set; }
	}

}