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
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization
{
	// This file generated from MarshalerRepository.tt T4Template.
	// Do not modify this file. Edit MarshalerRepository.tt instead.

	partial class MarshalerRepository 
	{
		private static readonly Dictionary<Type, MethodInfo> _factMarshalers =
			new Dictionary<Type, MethodInfo>( 14 )
			{
				{ typeof( MessagePackObject ), FromExpression.ToMethod( ( Packer packer, MessagePackObject value ) => packer.Pack( value ) ) },
				{ typeof( System.Boolean ), FromExpression.ToMethod( ( Packer packer, System.Boolean value ) => packer.Pack( value ) ) },
				{ typeof( System.Byte ), FromExpression.ToMethod( ( Packer packer, System.Byte value ) => packer.Pack( value ) ) },
				{ typeof( System.SByte ), FromExpression.ToMethod( ( Packer packer, System.SByte value ) => packer.Pack( value ) ) },
				{ typeof( System.Int16 ), FromExpression.ToMethod( ( Packer packer, System.Int16 value ) => packer.Pack( value ) ) },
				{ typeof( System.UInt16 ), FromExpression.ToMethod( ( Packer packer, System.UInt16 value ) => packer.Pack( value ) ) },
				{ typeof( System.Int32 ), FromExpression.ToMethod( ( Packer packer, System.Int32 value ) => packer.Pack( value ) ) },
				{ typeof( System.UInt32 ), FromExpression.ToMethod( ( Packer packer, System.UInt32 value ) => packer.Pack( value ) ) },
				{ typeof( System.Int64 ), FromExpression.ToMethod( ( Packer packer, System.Int64 value ) => packer.Pack( value ) ) },
				{ typeof( System.UInt64 ), FromExpression.ToMethod( ( Packer packer, System.UInt64 value ) => packer.Pack( value ) ) },
				{ typeof( System.Single ), FromExpression.ToMethod( ( Packer packer, System.Single value ) => packer.Pack( value ) ) },
				{ typeof( System.Double ), FromExpression.ToMethod( ( Packer packer, System.Double value ) => packer.Pack( value ) ) },
				{ typeof( System.Byte[] ), FromExpression.ToMethod( ( Packer packer, System.Byte[] value ) => packer.PackRaw( value ) ) },
				{ typeof( System.Char[] ), FromExpression.ToMethod( ( Packer packer, System.Char[] value ) => packer.PackString( value ) ) },
				{ typeof( System.String ), FromExpression.ToMethod( ( Packer packer, System.String value ) => packer.PackString( value ) ) },
			};
			
		private static readonly Dictionary<Type, MethodInfo> _factUnmarshalers =
			new Dictionary<Type, MethodInfo>( 14 )
			{
				{ typeof( MessagePackObject ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackObject() ) },
				{ typeof( System.Boolean ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackBoolean() ) },
				{ typeof( System.Byte ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackByte() ) },
				{ typeof( System.SByte ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackSByte() ) },
				{ typeof( System.Int16 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackInt16() ) },
				{ typeof( System.UInt16 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackUInt16() ) },
				{ typeof( System.Int32 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackInt32() ) },
				{ typeof( System.UInt32 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackUInt32() ) },
				{ typeof( System.Int64 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackInt64() ) },
				{ typeof( System.UInt64 ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackUInt64() ) },
				{ typeof( System.Single ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackSingle() ) },
				{ typeof( System.Double ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackDouble() ) },
				{ typeof( System.Byte[] ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackByteArray() ) },
				{ typeof( System.Char[] ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackCharArray() ) },
				{ typeof( System.String ), FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.UnpackString() ) },
			};
	}
}