 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
	// This file was generated from SubtreeUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit SubtreeUnpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class SubtreeUnpacker
	{
		public override bool ReadBoolean( out Boolean result )
		{
			return this._root.ReadBoolean( out result );
		}

		public override bool ReadNullableBoolean( out Boolean? result )
		{
			return this._root.ReadNullableBoolean( out result );
		}

		public override bool ReadByte( out Byte result )
		{
			return this._root.ReadByte( out result );
		}

		public override bool ReadNullableByte( out Byte? result )
		{
			return this._root.ReadNullableByte( out result );
		}

		public override bool ReadSByte( out SByte result )
		{
			return this._root.ReadSByte( out result );
		}

		public override bool ReadNullableSByte( out SByte? result )
		{
			return this._root.ReadNullableSByte( out result );
		}

		public override bool ReadInt16( out Int16 result )
		{
			return this._root.ReadInt16( out result );
		}

		public override bool ReadNullableInt16( out Int16? result )
		{
			return this._root.ReadNullableInt16( out result );
		}

		public override bool ReadUInt16( out UInt16 result )
		{
			return this._root.ReadUInt16( out result );
		}

		public override bool ReadNullableUInt16( out UInt16? result )
		{
			return this._root.ReadNullableUInt16( out result );
		}

		public override bool ReadInt32( out Int32 result )
		{
			return this._root.ReadInt32( out result );
		}

		public override bool ReadNullableInt32( out Int32? result )
		{
			return this._root.ReadNullableInt32( out result );
		}

		public override bool ReadUInt32( out UInt32 result )
		{
			return this._root.ReadUInt32( out result );
		}

		public override bool ReadNullableUInt32( out UInt32? result )
		{
			return this._root.ReadNullableUInt32( out result );
		}

		public override bool ReadInt64( out Int64 result )
		{
			return this._root.ReadInt64( out result );
		}

		public override bool ReadNullableInt64( out Int64? result )
		{
			return this._root.ReadNullableInt64( out result );
		}

		public override bool ReadUInt64( out UInt64 result )
		{
			return this._root.ReadUInt64( out result );
		}

		public override bool ReadNullableUInt64( out UInt64? result )
		{
			return this._root.ReadNullableUInt64( out result );
		}

		public override bool ReadSingle( out Single result )
		{
			return this._root.ReadSingle( out result );
		}

		public override bool ReadNullableSingle( out Single? result )
		{
			return this._root.ReadNullableSingle( out result );
		}

		public override bool ReadDouble( out Double result )
		{
			return this._root.ReadDouble( out result );
		}

		public override bool ReadNullableDouble( out Double? result )
		{
			return this._root.ReadNullableDouble( out result );
		}


		public override bool ReadArrayLength( out long result )
		{
			return this._root.ReadArrayLength( out result );
		}

		public override bool ReadMapLength( out long result )
		{
			return this._root.ReadMapLength( out result );
		}

		public override bool ReadBinary( out byte[] result )
		{
			return this._root.ReadBinary( out result );
		}

		public override bool ReadString( out string result )
		{
			return this._root.ReadString( out result );
		}

		public override bool ReadObject( out MessagePackObject result )
		{
			return this._root.ReadObject( out result );
		}
	}
}