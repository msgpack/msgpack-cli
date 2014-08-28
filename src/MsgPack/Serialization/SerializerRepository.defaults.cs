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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
#if !WINDOWS_PHONE && !NETFX_35 && !UNITY
using System.Numerics;
#endif // !WINDOWS_PHONE && !NETFX_35 && !UNITY
using System.Reflection;
using System.Text;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	// This file generated from SerializerRepository.tt T4Template.
	// Do not modify this file. Edit SerializerRepository.tt instead.

	partial class SerializerRepository 
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This API is naturally coupled with many types" )]
		internal static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable( SerializationContext ownerContext )
		{
			var dictionary = new Dictionary<RuntimeTypeHandle, object>( 436 );
			dictionary.Add( typeof( MessagePackObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( MessagePackObjectDictionary ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackObjectDictionaryMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( MessagePackExtendedTypeObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackExtendedTypeObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( List<MessagePackObject> ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( Object ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( String ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_StringMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( StringBuilder ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Text_StringBuilderMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( Char[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_CharArrayMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( Byte[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ByteArrayMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.DateTime ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DateTimeMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.DateTimeOffset ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DateTimeOffsetMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Boolean ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_BooleanMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Byte ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ByteMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Char ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_CharMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Decimal ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DecimalMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Double ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DoubleMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Guid ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_GuidMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Int16 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int16MessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Int32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int32MessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Int64 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Int64MessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.SByte ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_SByteMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Single ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_SingleMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.TimeSpan ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_TimeSpanMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.UInt16 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt16MessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.UInt32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt32MessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.UInt64 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UInt64MessagePackSerializer( ownerContext ) );
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Runtime.InteropServices.ComTypes.FILETIME ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT
#if !SILVERLIGHT && !NETFX_CORE
			dictionary.Add( typeof( System.Collections.Specialized.BitVector32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_BitVector32MessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT && !NETFX_CORE
#if !WINDOWS_PHONE
#if !NETFX_35 && !UNITY
			dictionary.Add( typeof( System.Numerics.BigInteger ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_BigIntegerMessagePackSerializer( ownerContext ) );
#endif // !NETFX_35 && !UNITY
#endif // !WINDOWS_PHONE
			dictionary.Add( typeof( System.ArraySegment<> ).TypeHandle, typeof( System_ArraySegment_1MessagePackSerializer<> ) );
			dictionary.Add( typeof( System.Collections.DictionaryEntry ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_DictionaryEntryMessagePackSerializer( ownerContext ) );
#if !SILVERLIGHT && !NETFX_CORE
			dictionary.Add( typeof( System.Collections.Stack ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_StackMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT && !NETFX_CORE
#if !SILVERLIGHT && !NETFX_CORE
			dictionary.Add( typeof( System.Collections.Queue ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_QueueMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT && !NETFX_CORE
			dictionary.Add( typeof( System.Collections.Generic.KeyValuePair<,> ).TypeHandle, typeof( System_Collections_Generic_KeyValuePair_2MessagePackSerializer<, > ) );
			dictionary.Add( typeof( System.Collections.Generic.Stack<> ).TypeHandle, typeof( System_Collections_Generic_Stack_1MessagePackSerializer<> ) );
			dictionary.Add( typeof( System.Collections.Generic.Queue<> ).TypeHandle, typeof( System_Collections_Generic_Queue_1MessagePackSerializer<> ) );
#if !WINDOWS_PHONE
#if !NETFX_35 && !UNITY
			dictionary.Add( typeof( System.Numerics.Complex ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_ComplexMessagePackSerializer( ownerContext ) );
#endif // !NETFX_35 && !UNITY
#endif // !WINDOWS_PHONE
			dictionary.Add( typeof( System.Uri ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UriMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Version ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_VersionMessagePackSerializer( ownerContext ) );
#if !SILVERLIGHT && !NETFX_CORE
			dictionary.Add( typeof( System.Collections.Specialized.NameValueCollection ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_NameValueCollectionMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT && !NETFX_CORE
			return dictionary;
		}
	}
}
