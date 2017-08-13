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
	///		Defines non generic portion of <see cref="MessagePackPacker{TWriter}"/>.
	/// </summary>
	internal abstract class MessagePackPacker : IDisposable
	{
		private readonly PackerCompatibilityOptions _compatibilityOptions;

		protected bool AllowStr8
		{
			get { return ( this._compatibilityOptions & PackerCompatibilityOptions.PackBinaryAsRaw ) == 0; }
		}

		protected MessagePackPacker( PackerCompatibilityOptions compatibilityOptions )
		{
			this._compatibilityOptions = compatibilityOptions;
		}

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) { }

		protected static class Header
		{
			public const byte Nil = MessagePackCode.NilValue;
			public const byte True = MessagePackCode.TrueValue;
			public const byte False = MessagePackCode.FalseValue;
			public const byte SByte = MessagePackCode.SignedInt8;
			public const byte Int16 = MessagePackCode.SignedInt16;
			public const byte Int32 = MessagePackCode.SignedInt32;
			public const byte Int64 = MessagePackCode.SignedInt64;
			public const byte Byte = MessagePackCode.UnsignedInt8;
			public const byte UInt16 = MessagePackCode.UnsignedInt16;
			public const byte UInt32 = MessagePackCode.UnsignedInt32;
			public const byte UInt64 = MessagePackCode.UnsignedInt64;
			public const byte Single = MessagePackCode.Real32;
			public const byte Double = MessagePackCode.Real64;
			public const byte Array16 = MessagePackCode.Array16;
			public const byte Array32 = MessagePackCode.Array32;
			public const byte Map16 = MessagePackCode.Map16;
			public const byte Map32 = MessagePackCode.Map32;
			public const byte Str8 = MessagePackCode.Str8;
			public const byte Str16 = MessagePackCode.Str16;
			public const byte Str32 = MessagePackCode.Str32;
			public const byte Bin8 = MessagePackCode.Bin8;
			public const byte Bin16 = MessagePackCode.Bin16;
			public const byte Bin32 = MessagePackCode.Bin32;
			public const byte FixExt1 = MessagePackCode.FixExt1;
			public const byte FixExt2 = MessagePackCode.FixExt2;
			public const byte FixExt4 = MessagePackCode.FixExt4;
			public const byte FixExt8 = MessagePackCode.FixExt8;
			public const byte FixExt16 = MessagePackCode.FixExt16;
			public const byte Ext8 = MessagePackCode.Ext8;
			public const byte Ext16 = MessagePackCode.Ext16;
			public const byte Ext32 = MessagePackCode.Ext32;
		}
	}
}
