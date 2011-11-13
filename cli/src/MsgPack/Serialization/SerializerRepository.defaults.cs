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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Reflection;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	// This file generated from SerializerRepository.tt T4Template.
	// Do not modify this file. Edit SerializerRepository.tt instead.

	partial class SerializerRepository 
	{
		private static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable()
		{
			var dictionary = new Dictionary<RuntimeTypeHandle, object>( 7 );
			dictionary.Add( typeof( System.ArraySegment<> ).TypeHandle, typeof( System_ArraySegment_1MessageSerializer<> ) );
			dictionary.Add( typeof( System.Collections.DictionaryEntry ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Collections_DictionaryEntryMessageSerializer() );
			dictionary.Add( typeof( System.Collections.Generic.KeyValuePair<,> ).TypeHandle, typeof( System_Collections_Generic_KeyValuePair_2MessageSerializer<, > ) );
			dictionary.Add( typeof( System.Numerics.Complex ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Numerics_ComplexMessageSerializer() );
			dictionary.Add( typeof( System.Text.StringBuilder ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_Text_StringBuilderMessageSerializer() );
			dictionary.Add( typeof( System.Uri ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_UriMessageSerializer() );
			dictionary.Add( typeof( System.Version ).TypeHandle, new MsgPack.Serialization.DefaultSerializers.System_VersionMessageSerializer() );
			return dictionary;
		}
	}
}
