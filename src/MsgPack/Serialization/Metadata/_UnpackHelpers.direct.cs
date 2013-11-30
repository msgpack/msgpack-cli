#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Reflection;
using System.Threading;

namespace MsgPack.Serialization.Metadata
{
	// This file was generated from _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class _UnpackHelpers
	{
		private static readonly Dictionary<Type,MethodInfo> _directUnpackMethods;

		public static readonly MethodInfo UnpackSByteValue;
		public static readonly MethodInfo UnpackInt16Value;
		public static readonly MethodInfo UnpackInt32Value;
		public static readonly MethodInfo UnpackInt64Value;
		public static readonly MethodInfo UnpackByteValue;
		public static readonly MethodInfo UnpackUInt16Value;
		public static readonly MethodInfo UnpackUInt32Value;
		public static readonly MethodInfo UnpackUInt64Value;
		public static readonly MethodInfo UnpackSingleValue;
		public static readonly MethodInfo UnpackDoubleValue;
		public static readonly MethodInfo UnpackBooleanValue;
		public static readonly MethodInfo UnpackStringValue;
		public static readonly MethodInfo UnpackBinaryValue;
		public static readonly MethodInfo UnpackObjectValue;

		static _UnpackHelpers()
		{
			var directUnpackMethods = new Dictionary<Type, MethodInfo>( 14 );
			
			UnpackSByteValue = typeof( Unpacker ).GetMethod( "UnpackSByteValue" );
			directUnpackMethods.Add( typeof( SByte? ), UnpackSByteValue );
			
			UnpackInt16Value = typeof( Unpacker ).GetMethod( "UnpackInt16Value" );
			directUnpackMethods.Add( typeof( Int16? ), UnpackInt16Value );
			
			UnpackInt32Value = typeof( Unpacker ).GetMethod( "UnpackInt32Value" );
			directUnpackMethods.Add( typeof( Int32? ), UnpackInt32Value );
			
			UnpackInt64Value = typeof( Unpacker ).GetMethod( "UnpackInt64Value" );
			directUnpackMethods.Add( typeof( Int64? ), UnpackInt64Value );
			
			UnpackByteValue = typeof( Unpacker ).GetMethod( "UnpackByteValue" );
			directUnpackMethods.Add( typeof( Byte? ), UnpackByteValue );
			
			UnpackUInt16Value = typeof( Unpacker ).GetMethod( "UnpackUInt16Value" );
			directUnpackMethods.Add( typeof( UInt16? ), UnpackUInt16Value );
			
			UnpackUInt32Value = typeof( Unpacker ).GetMethod( "UnpackUInt32Value" );
			directUnpackMethods.Add( typeof( UInt32? ), UnpackUInt32Value );
			
			UnpackUInt64Value = typeof( Unpacker ).GetMethod( "UnpackUInt64Value" );
			directUnpackMethods.Add( typeof( UInt64? ), UnpackUInt64Value );
			
			UnpackSingleValue = typeof( Unpacker ).GetMethod( "UnpackSingleValue" );
			directUnpackMethods.Add( typeof( Single? ), UnpackSingleValue );
			
			UnpackDoubleValue = typeof( Unpacker ).GetMethod( "UnpackDoubleValue" );
			directUnpackMethods.Add( typeof( Double? ), UnpackDoubleValue );
			
			UnpackBooleanValue = typeof( Unpacker ).GetMethod( "UnpackBooleanValue" );
			directUnpackMethods.Add( typeof( Boolean? ), UnpackBooleanValue );
			UnpackStringValue = typeof( Unpacker ).GetMethod( "UnpackStringValue" );
			directUnpackMethods.Add( typeof( string ), UnpackStringValue );
			UnpackBinaryValue = typeof( Unpacker ).GetMethod( "UnpackBinaryValue" );
			directUnpackMethods.Add( typeof( byte[] ), UnpackBinaryValue );
			UnpackObjectValue = typeof( Unpacker ).GetMethod( "UnpackObjectValue" );
			directUnpackMethods.Add( typeof( MessagePackObject ), UnpackObjectValue );

			Interlocked.Exchange( ref _directUnpackMethods, directUnpackMethods );
		}

		public static MethodInfo GetDirectUnpackMethod( Type type )
		{
			MethodInfo result;
			_directUnpackMethods.TryGetValue( type, out result );
			return result;
		}
	}
}