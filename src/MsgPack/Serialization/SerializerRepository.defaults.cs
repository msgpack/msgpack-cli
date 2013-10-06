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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
#if !WINDOWS_PHONE && !NETFX_35
using System.Numerics;
#endif
using System.Reflection;
using System.Text;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	// This file generated from SerializerRepository.tt T4Template.
	// Do not modify this file. Edit SerializerRepository.tt instead.

	partial class SerializerRepository 
	{
		internal static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable( PackerCompatibilityOptions packerCompatibilityOptions )
		{
			var dictionary = new Dictionary<RuntimeTypeHandle, object>( 428 );
			dictionary.Add( typeof( MessagePackObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackObjectMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( MessagePackExtendedTypeObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackExtendedTypeObjectMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( Object ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ObjectMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( String ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_StringMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( StringBuilder ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Text_StringBuilderMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( Char[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_CharArrayMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( Byte[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ByteArrayMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.DateTime ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DateTimeMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.DateTimeOffset ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DateTimeOffsetMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Boolean ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_BooleanMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Byte ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ByteMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Char ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_CharMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Decimal ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DecimalMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Double ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DoubleMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Guid ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_GuidMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Int16 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int16MessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Int32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int32MessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Int64 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int64MessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.SByte ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_SByteMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Single ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_SingleMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.TimeSpan ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_TimeSpanMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.UInt16 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt16MessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.UInt32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt32MessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.UInt64 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt64MessagePackSerializer( packerCompatibilityOptions ) );
#if !SILVERLIGHT && !NETFX_CORE
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Runtime.InteropServices.ComTypes.FILETIME ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer( packerCompatibilityOptions ) );
#endif // !SILVERLIGHT && !NETFX_CORE
#endif // !SILVERLIGHT
#if !SILVERLIGHT && !NETFX_CORE
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Collections.Specialized.BitVector32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_BitVector32MessagePackSerializer( packerCompatibilityOptions ) );
#endif // !SILVERLIGHT && !NETFX_CORE
#endif // !SILVERLIGHT
#if !WINDOWS_PHONE
#if !NETFX_35
			dictionary.Add( typeof( System.Numerics.BigInteger ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_BigIntegerMessagePackSerializer( packerCompatibilityOptions ) );
#endif // !WINDOWS_PHONE
#endif // !NETFX_35
			dictionary.Add( typeof( System.ArraySegment<> ).TypeHandle, typeof( System_ArraySegment_1MessagePackSerializer<> ) );
			dictionary.Add( typeof( System.Collections.DictionaryEntry ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_DictionaryEntryMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Collections.Generic.KeyValuePair<,> ).TypeHandle, typeof( System_Collections_Generic_KeyValuePair_2MessagePackSerializer<, > ) );
#if !WINDOWS_PHONE
#if !NETFX_35
			dictionary.Add( typeof( System.Numerics.Complex ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_ComplexMessagePackSerializer( packerCompatibilityOptions ) );
#endif // !WINDOWS_PHONE
#endif // !NETFX_35
			dictionary.Add( typeof( System.Uri ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UriMessagePackSerializer( packerCompatibilityOptions ) );
			dictionary.Add( typeof( System.Version ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_VersionMessagePackSerializer( packerCompatibilityOptions ) );
#if !SILVERLIGHT && !NETFX_CORE
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Collections.Specialized.NameValueCollection ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_NameValueCollectionMessagePackSerializer( packerCompatibilityOptions ) );
#endif // !SILVERLIGHT && !NETFX_CORE
#endif // !SILVERLIGHT
			return dictionary;
		}
	}
}
