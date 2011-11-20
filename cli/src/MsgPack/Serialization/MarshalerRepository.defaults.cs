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
using System.Text;

namespace MsgPack.Serialization
{
	// This file generated from MarshalerRepository.tt T4Template.
	// Do not modify this file. Edit MarshalerRepository.tt instead.

	partial class MarshalerRepository 
	{
		private static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable()
		{
			var dictionary = new Dictionary<RuntimeTypeHandle, object>( 397 + 6 );
			dictionary.Add( typeof( MessagePackObject ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.MsgPack_MessagePackObjectMessageMarshaler() );
			dictionary.Add( typeof( System.DateTime ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_DateTimeMessageMarshaler() );
			dictionary.Add( typeof( System.DateTimeOffset ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_DateTimeOffsetMessageMarshaler() );
			dictionary.Add( typeof( System.Boolean ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_BooleanMessageMarshaler() );
			dictionary.Add( typeof( System.Byte ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_ByteMessageMarshaler() );
			dictionary.Add( typeof( System.Char ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_CharMessageMarshaler() );
			dictionary.Add( typeof( System.Decimal ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_DecimalMessageMarshaler() );
			dictionary.Add( typeof( System.Double ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_DoubleMessageMarshaler() );
			dictionary.Add( typeof( System.Guid ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_GuidMessageMarshaler() );
			dictionary.Add( typeof( System.Int16 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Int16MessageMarshaler() );
			dictionary.Add( typeof( System.Int32 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Int32MessageMarshaler() );
			dictionary.Add( typeof( System.Int64 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Int64MessageMarshaler() );
			dictionary.Add( typeof( System.SByte ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_SByteMessageMarshaler() );
			dictionary.Add( typeof( System.Single ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_SingleMessageMarshaler() );
			dictionary.Add( typeof( System.TimeSpan ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_TimeSpanMessageMarshaler() );
			dictionary.Add( typeof( System.UInt16 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_UInt16MessageMarshaler() );
			dictionary.Add( typeof( System.UInt32 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_UInt32MessageMarshaler() );
			dictionary.Add( typeof( System.UInt64 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_UInt64MessageMarshaler() );
			dictionary.Add( typeof( System.Runtime.InteropServices.ComTypes.FILETIME ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Runtime_InteropServices_ComTypes_FILETIMEMessageMarshaler() );
			dictionary.Add( typeof( System.Collections.Specialized.BitVector32 ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Collections_Specialized_BitVector32MessageMarshaler() );
			dictionary.Add( typeof( System.Numerics.BigInteger ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Numerics_BigIntegerMessageMarshaler() );
			dictionary.Add( typeof( Object ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_ObjectMessageMarshaler() );
			dictionary.Add( typeof( String ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_StringMessageMarshaler() );
			dictionary.Add( typeof( StringBuilder ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_Text_StringBuilderMessageMarshaler() );
			dictionary.Add( typeof( Char[] ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_CharArrayMessageMarshaler() );
			dictionary.Add( typeof( Byte[] ).TypeHandle, new MsgPack.Serialization.DefaultMarshalers.System_ByteArrayMessageMarshaler() );
			return dictionary;
		}
	}
}