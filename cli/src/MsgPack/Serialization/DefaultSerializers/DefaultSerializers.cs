#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace MsgPack.Serialization.DefaultSerializers
{
	// This file generated from DefaultMarshalers.tt T4Template.
	// Do not modify this file. Edit DefaultMarshalers.tt instead.

	internal sealed class System_DateTimeMessagePackSerializer : MessagePackSerializer< System.DateTime >
	{
		protected sealed override void PackToCore( Packer packer, System.DateTime value )
		{
			packer.Pack( MessagePackConvert.FromDateTime( value ) );
		}

		protected sealed override  System.DateTime UnpackFromCore( Unpacker unpacker )
		{
			return MessagePackConvert.ToDateTime( unpacker.Data.Value.AsInt64() ); 
		}
	}

	internal sealed class System_DateTimeOffsetMessagePackSerializer : MessagePackSerializer< System.DateTimeOffset >
	{
		protected sealed override void PackToCore( Packer packer, System.DateTimeOffset value )
		{
			packer.Pack( MessagePackConvert.FromDateTimeOffset( value ) );
		}

		protected sealed override  System.DateTimeOffset UnpackFromCore( Unpacker unpacker )
		{
			return MessagePackConvert.ToDateTimeOffset( unpacker.Data.Value.AsInt64() ); 
		}
	}

	internal sealed class System_BooleanMessagePackSerializer : MessagePackSerializer< System.Boolean >
	{
		protected sealed override void PackToCore( Packer packer, System.Boolean value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Boolean UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsBoolean();
		}
	}

	internal sealed class System_ByteMessagePackSerializer : MessagePackSerializer< System.Byte >
	{
		protected sealed override void PackToCore( Packer packer, System.Byte value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Byte UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsByte();
		}
	}

	internal sealed class System_CharMessagePackSerializer : MessagePackSerializer< System.Char >
	{
		protected sealed override void PackToCore( Packer packer, System.Char value )
		{
			packer.Pack( ( System.UInt16 )value );
		}

		protected sealed override  System.Char UnpackFromCore( Unpacker unpacker )
		{
			return ( System.Char ) unpacker.Data.Value.AsUInt16(); 
		}
	}

	internal sealed class System_DecimalMessagePackSerializer : MessagePackSerializer< System.Decimal >
	{
		protected sealed override void PackToCore( Packer packer, System.Decimal value )
		{
			packer.Pack( value.ToString( "G", CultureInfo.InvariantCulture ) );
		}

		protected sealed override  System.Decimal UnpackFromCore( Unpacker unpacker )
		{
			return System.Decimal.Parse( unpacker.Data.Value.AsString(), CultureInfo.InvariantCulture ); 
		}
	}

	internal sealed class System_DoubleMessagePackSerializer : MessagePackSerializer< System.Double >
	{
		protected sealed override void PackToCore( Packer packer, System.Double value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Double UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsDouble();
		}
	}

	internal sealed class System_GuidMessagePackSerializer : MessagePackSerializer< System.Guid >
	{
		protected sealed override void PackToCore( Packer packer, System.Guid value )
		{
			packer.Pack( value.ToByteArray() );
		}

		protected sealed override  System.Guid UnpackFromCore( Unpacker unpacker )
		{
			return new System.Guid( unpacker.Data.Value.AsBinary() ); 
		}
	}

	internal sealed class System_Int16MessagePackSerializer : MessagePackSerializer< System.Int16 >
	{
		protected sealed override void PackToCore( Packer packer, System.Int16 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Int16 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsInt16();
		}
	}

	internal sealed class System_Int32MessagePackSerializer : MessagePackSerializer< System.Int32 >
	{
		protected sealed override void PackToCore( Packer packer, System.Int32 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Int32 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsInt32();
		}
	}

	internal sealed class System_Int64MessagePackSerializer : MessagePackSerializer< System.Int64 >
	{
		protected sealed override void PackToCore( Packer packer, System.Int64 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Int64 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsInt64();
		}
	}

	internal sealed class System_SByteMessagePackSerializer : MessagePackSerializer< System.SByte >
	{
		protected sealed override void PackToCore( Packer packer, System.SByte value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.SByte UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsSByte();
		}
	}

	internal sealed class System_SingleMessagePackSerializer : MessagePackSerializer< System.Single >
	{
		protected sealed override void PackToCore( Packer packer, System.Single value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.Single UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsSingle();
		}
	}

	internal sealed class System_TimeSpanMessagePackSerializer : MessagePackSerializer< System.TimeSpan >
	{
		protected sealed override void PackToCore( Packer packer, System.TimeSpan value )
		{
			packer.Pack( value.Ticks );
		}

		protected sealed override  System.TimeSpan UnpackFromCore( Unpacker unpacker )
		{
			return new System.TimeSpan( unpacker.Data.Value.AsInt64() );
		}
	}

	internal sealed class System_UInt16MessagePackSerializer : MessagePackSerializer< System.UInt16 >
	{
		protected sealed override void PackToCore( Packer packer, System.UInt16 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.UInt16 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsUInt16();
		}
	}

	internal sealed class System_UInt32MessagePackSerializer : MessagePackSerializer< System.UInt32 >
	{
		protected sealed override void PackToCore( Packer packer, System.UInt32 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.UInt32 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsUInt32();
		}
	}

	internal sealed class System_UInt64MessagePackSerializer : MessagePackSerializer< System.UInt64 >
	{
		protected sealed override void PackToCore( Packer packer, System.UInt64 value )
		{
			packer.Pack( value );
		}

		protected sealed override  System.UInt64 UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.Data.Value.AsUInt64();
		}
	}

	internal sealed class System_Collections_Specialized_BitVector32MessagePackSerializer : MessagePackSerializer< System.Collections.Specialized.BitVector32 >
	{
		protected sealed override void PackToCore( Packer packer, System.Collections.Specialized.BitVector32 value )
		{
			packer.Pack( value.Data );
		}

		protected sealed override  System.Collections.Specialized.BitVector32 UnpackFromCore( Unpacker unpacker )
		{
			return new System.Collections.Specialized.BitVector32( unpacker.Data.Value.AsInt32() );
		}
	}

	internal sealed class System_Numerics_BigIntegerMessagePackSerializer : MessagePackSerializer< System.Numerics.BigInteger >
	{
		protected sealed override void PackToCore( Packer packer, System.Numerics.BigInteger value )
		{
			packer.Pack( value.ToByteArray() );
		}

		protected sealed override  System.Numerics.BigInteger UnpackFromCore( Unpacker unpacker )
		{
			return new System.Numerics.BigInteger( unpacker.Data.Value.AsBinary() ); 
		}
	}
}
