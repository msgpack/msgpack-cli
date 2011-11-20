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

namespace MsgPack.Serialization.DefaultMarshalers
{
	// This file generated from DefaultMarshalers.tt T4Template.
	// Do not modify this file. Edit DefaultMarshalers.tt instead.

	internal sealed class System_DateTimeMessageMarshaler : MessageMarshaler< System.DateTime >
	{
		protected sealed override void MarshalToCore( Packer packer, System.DateTime value )
		{
			packer.Pack( MessagePackConvert.FromDateTime( value ) );
		}

		protected sealed override  System.DateTime UnmarshalFromCore( Unpacker unpacker )
		{
			return MessagePackConvert.ToDateTime( unpacker.UnpackInt64() ); 
		}
	}

	internal sealed class System_DateTimeOffsetMessageMarshaler : MessageMarshaler< System.DateTimeOffset >
	{
		protected sealed override void MarshalToCore( Packer packer, System.DateTimeOffset value )
		{
			packer.Pack( MessagePackConvert.FromDateTimeOffset( value ) );
		}

		protected sealed override  System.DateTimeOffset UnmarshalFromCore( Unpacker unpacker )
		{
			return MessagePackConvert.ToDateTimeOffset( unpacker.UnpackInt64() ); 
		}
	}

	internal sealed class System_CharMessageMarshaler : MessageMarshaler< System.Char >
	{
		protected sealed override void MarshalToCore( Packer packer, System.Char value )
		{
			packer.Pack( ( System.UInt16 )value );
		}

		protected sealed override  System.Char UnmarshalFromCore( Unpacker unpacker )
		{
			return ( System.Char ) unpacker.UnpackUInt16(); 
		}
	}

	internal sealed class System_DecimalMessageMarshaler : MessageMarshaler< System.Decimal >
	{
		protected sealed override void MarshalToCore( Packer packer, System.Decimal value )
		{
			packer.Pack( value.ToString( "G", CultureInfo.InvariantCulture ) );
		}

		protected sealed override  System.Decimal UnmarshalFromCore( Unpacker unpacker )
		{
			return System.Decimal.Parse( unpacker.UnpackString(), CultureInfo.InvariantCulture ); 
		}
	}

	internal sealed class System_GuidMessageMarshaler : MessageMarshaler< System.Guid >
	{
		protected sealed override void MarshalToCore( Packer packer, System.Guid value )
		{
			packer.Pack( value.ToByteArray() );
		}

		protected sealed override  System.Guid UnmarshalFromCore( Unpacker unpacker )
		{
			return new System.Guid( unpacker.UnpackByteArray() ); 
		}
	}

	internal sealed class System_TimeSpanMessageMarshaler : MessageMarshaler< System.TimeSpan >
	{
		protected sealed override void MarshalToCore( Packer packer, System.TimeSpan value )
		{
			packer.Pack( value.Ticks );
		}

		protected sealed override  System.TimeSpan UnmarshalFromCore( Unpacker unpacker )
		{
			return new System.TimeSpan( unpacker.Data.Value.AsInt64() );
		}
	}

	internal sealed class System_Collections_Specialized_BitVector32MessageMarshaler : MessageMarshaler< System.Collections.Specialized.BitVector32 >
	{
		protected sealed override void MarshalToCore( Packer packer, System.Collections.Specialized.BitVector32 value )
		{
			packer.Pack( value.Data );
		}

		protected sealed override  System.Collections.Specialized.BitVector32 UnmarshalFromCore( Unpacker unpacker )
		{
			return new System.Collections.Specialized.BitVector32( unpacker.Data.Value.AsInt32() );
		}
	}

	internal sealed class System_Numerics_BigIntegerMessageMarshaler : MessageMarshaler< System.Numerics.BigInteger >
	{
		protected sealed override void MarshalToCore( Packer packer, System.Numerics.BigInteger value )
		{
			packer.Pack( value.ToByteArray() );
		}

		protected sealed override  System.Numerics.BigInteger UnmarshalFromCore( Unpacker unpacker )
		{
			return new System.Numerics.BigInteger( unpacker.UnpackByteArray() ); 
		}
	}
}
