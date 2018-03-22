#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.Collections.Specialized;
#endif // !UNITY || MSGPACK_UNITY_FULL
using System.Globalization;
#if !WINDOWS_PHONE && !NET35 && !UNITY
using System.Numerics;
#endif // !WINDOWS_PHONE && !NET35 && !UNITY
using System.Reflection;
using System.Text;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	// This file generated from SerializerRepository.tt T4Template.
	// Do not modify this file. Edit SerializerRepository.tt instead.

	// ReSharper disable RedundantNameQualifier
	partial class SerializerRepository 
	{
#if UNITY && DEBUG
		public
#else
		internal
#endif
		const int DefaultTableCapacity = 58;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This API is naturally coupled with many types" )]
#if UNITY && DEBUG
		public
#else
		internal
#endif
		static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable( SerializationContext ownerContext )
		{
			var dictionary = new Dictionary<RuntimeTypeHandle, object>( DefaultTableCapacity );
			dictionary.Add( typeof( MessagePackObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( MessagePackObjectDictionary ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackObjectDictionaryMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( MessagePackExtendedTypeObject ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.MsgPack_MessagePackExtendedTypeObjectMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( List<MessagePackObject> ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer( ownerContext ) );
#if !UNITY
			dictionary.Add( typeof( Object ).TypeHandle, new MsgPack.Serialization.Polymorphic.PolymorphicSerializerProvider<object>( new MsgPack.Serialization.DefaultSerializers.System_ObjectMessagePackSerializer( ownerContext ) ) );
#else
			dictionary.Add( typeof( Object ).TypeHandle, new MsgPack.Serialization.Polymorphic.PolymorphicSerializerProvider<object>( ownerContext, new MsgPack.Serialization.DefaultSerializers.System_ObjectMessagePackSerializer( ownerContext ) ) );
#endif // !UNITY
			dictionary.Add( typeof( String ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_StringMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( StringBuilder ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Text_StringBuilderMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( Char[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_CharArrayMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( Byte[] ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_ByteArrayMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( DateTime ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.DateTimeMessagePackSerializerProvider( ownerContext, false ) );
			dictionary.Add( typeof( DateTimeOffset ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.DateTimeOffsetMessagePackSerializerProvider( ownerContext, false ) );
			dictionary.Add( typeof( Timestamp ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.TimestampMessagePackSerializerProvider( ownerContext, false ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !UNITY
			dictionary.Add( typeof( System.Runtime.InteropServices.ComTypes.FILETIME ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.FileTimeMessagePackSerializerProvider( ownerContext, false ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !UNITY
			// DateTime, DateTimeOffset, and FILETIME must have nullable providers.
			dictionary.Add( typeof( DateTime? ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.DateTimeMessagePackSerializerProvider( ownerContext, true ) );
			dictionary.Add( typeof( DateTimeOffset? ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.DateTimeOffsetMessagePackSerializerProvider( ownerContext, true ) );
			dictionary.Add( typeof( Timestamp? ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.TimestampMessagePackSerializerProvider( ownerContext, true ) );
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !UNITY
			dictionary.Add( typeof( System.Runtime.InteropServices.ComTypes.FILETIME? ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.FileTimeMessagePackSerializerProvider( ownerContext, true ) );
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !UNITY
#if !NETFX_CORE && !NETSTANDARD1_1
			dictionary.Add( typeof( DBNull ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_DBNullMessagePackSerializer( ownerContext ) );
#endif // !NETFX_CORE && !NETSTANDARD1_1
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
#if !NETSTANDARD1_1
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
			dictionary.Add( typeof( System.Security.Cryptography.HashAlgorithmName ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Security_Cryptography_HashAlgorithmNameMessagePackSerializer( ownerContext ) );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#endif // !NETSTANDARD1_1
#if !NETSTANDARD1_1
#if !SILVERLIGHT
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Collections.Specialized.BitVector32 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_BitVector32MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !SILVERLIGHT
#endif // !NETSTANDARD1_1
#if !WINDOWS_PHONE
#if !NET35 && !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.BigInteger ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_BigIntegerMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY
#endif // !WINDOWS_PHONE
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Matrix3x2 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_Matrix3x2MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Matrix4x4 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_Matrix4x4MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Plane ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_PlaneMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Quaternion ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_QuaternionMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Vector2 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_Vector2MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Vector3 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_Vector3MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Vector4 ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_Vector4MessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#endif // !NET35 && !UNITY && !NET40 && !NET45 && !SILVERLIGHT
			dictionary.Add( typeof( System.ArraySegment<> ).TypeHandle, typeof( System_ArraySegment_1MessagePackSerializer<> ) );
			dictionary.Add( typeof( System.Globalization.CultureInfo ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Globalization_CultureInfoMessagePackSerializer( ownerContext ) );
			dictionary.Add( typeof( System.Collections.DictionaryEntry ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_DictionaryEntryMessagePackSerializer( ownerContext ) );
#if !NETSTANDARD1_1
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Collections.Stack ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_StackMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT
#endif // !NETSTANDARD1_1
#if !NETSTANDARD1_1
#if !SILVERLIGHT
			dictionary.Add( typeof( System.Collections.Queue ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_QueueMessagePackSerializer( ownerContext ) );
#endif // !SILVERLIGHT
#endif // !NETSTANDARD1_1
			dictionary.Add( typeof( System.Collections.Generic.KeyValuePair<,> ).TypeHandle, typeof( System_Collections_Generic_KeyValuePair_2MessagePackSerializer<, > ) );
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Collections.Generic.Stack<> ).TypeHandle, typeof( System_Collections_Generic_Stack_1MessagePackSerializer<> ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Collections.Generic.Queue<> ).TypeHandle, typeof( System_Collections_Generic_Queue_1MessagePackSerializer<> ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#if !WINDOWS_PHONE
#if !NET35 && !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Numerics.Complex ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_ComplexMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY
#endif // !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Uri ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UriMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Version ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_VersionMessagePackSerializer( ownerContext ) );
#if !NETSTANDARD1_1
#if !SILVERLIGHT
#if !UNITY || MSGPACK_UNITY_FULL
			dictionary.Add( typeof( System.Collections.Specialized.NameValueCollection ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_Specialized_NameValueCollectionMessagePackSerializer( ownerContext ) );
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !SILVERLIGHT
#endif // !NETSTANDARD1_1
			return dictionary;
		}
	}
}
