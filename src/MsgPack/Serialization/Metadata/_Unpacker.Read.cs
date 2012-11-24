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
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace MsgPack.Serialization.Metadata
{
	// This file was generated from _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class _Unpacker
	{
		private static readonly Dictionary<Type,MethodInfo> _directReadMethods;

		public static readonly MethodInfo ReadSByte;
		public static readonly MethodInfo ReadNullableSByte;
		public static readonly MethodInfo ReadInt16;
		public static readonly MethodInfo ReadNullableInt16;
		public static readonly MethodInfo ReadInt32;
		public static readonly MethodInfo ReadNullableInt32;
		public static readonly MethodInfo ReadInt64;
		public static readonly MethodInfo ReadNullableInt64;
		public static readonly MethodInfo ReadByte;
		public static readonly MethodInfo ReadNullableByte;
		public static readonly MethodInfo ReadUInt16;
		public static readonly MethodInfo ReadNullableUInt16;
		public static readonly MethodInfo ReadUInt32;
		public static readonly MethodInfo ReadNullableUInt32;
		public static readonly MethodInfo ReadUInt64;
		public static readonly MethodInfo ReadNullableUInt64;
		public static readonly MethodInfo ReadSingle;
		public static readonly MethodInfo ReadNullableSingle;
		public static readonly MethodInfo ReadDouble;
		public static readonly MethodInfo ReadNullableDouble;
		public static readonly MethodInfo ReadBoolean;
		public static readonly MethodInfo ReadNullableBoolean;
		public static readonly MethodInfo ReadString;
		public static readonly MethodInfo ReadBinary;
		public static readonly MethodInfo ReadObject;

		static _Unpacker()
		{
			var directReadMethods = new Dictionary<Type, MethodInfo>( 25 );
			
			ReadSByte = typeof( Unpacker ).GetMethod( "ReadSByte" );
			ReadNullableSByte = typeof( Unpacker ).GetMethod( "ReadNullableSByte" );
			directReadMethods.Add( typeof( System.SByte ), ReadNullableSByte );
			directReadMethods.Add( typeof( System.SByte? ), ReadNullableSByte );
			
			ReadInt16 = typeof( Unpacker ).GetMethod( "ReadInt16" );
			ReadNullableInt16 = typeof( Unpacker ).GetMethod( "ReadNullableInt16" );
			directReadMethods.Add( typeof( System.Int16 ), ReadNullableInt16 );
			directReadMethods.Add( typeof( System.Int16? ), ReadNullableInt16 );
			
			ReadInt32 = typeof( Unpacker ).GetMethod( "ReadInt32" );
			ReadNullableInt32 = typeof( Unpacker ).GetMethod( "ReadNullableInt32" );
			directReadMethods.Add( typeof( System.Int32 ), ReadNullableInt32 );
			directReadMethods.Add( typeof( System.Int32? ), ReadNullableInt32 );
			
			ReadInt64 = typeof( Unpacker ).GetMethod( "ReadInt64" );
			ReadNullableInt64 = typeof( Unpacker ).GetMethod( "ReadNullableInt64" );
			directReadMethods.Add( typeof( System.Int64 ), ReadNullableInt64 );
			directReadMethods.Add( typeof( System.Int64? ), ReadNullableInt64 );
			
			ReadByte = typeof( Unpacker ).GetMethod( "ReadByte" );
			ReadNullableByte = typeof( Unpacker ).GetMethod( "ReadNullableByte" );
			directReadMethods.Add( typeof( System.Byte ), ReadNullableByte );
			directReadMethods.Add( typeof( System.Byte? ), ReadNullableByte );
			
			ReadUInt16 = typeof( Unpacker ).GetMethod( "ReadUInt16" );
			ReadNullableUInt16 = typeof( Unpacker ).GetMethod( "ReadNullableUInt16" );
			directReadMethods.Add( typeof( System.UInt16 ), ReadNullableUInt16 );
			directReadMethods.Add( typeof( System.UInt16? ), ReadNullableUInt16 );
			
			ReadUInt32 = typeof( Unpacker ).GetMethod( "ReadUInt32" );
			ReadNullableUInt32 = typeof( Unpacker ).GetMethod( "ReadNullableUInt32" );
			directReadMethods.Add( typeof( System.UInt32 ), ReadNullableUInt32 );
			directReadMethods.Add( typeof( System.UInt32? ), ReadNullableUInt32 );
			
			ReadUInt64 = typeof( Unpacker ).GetMethod( "ReadUInt64" );
			ReadNullableUInt64 = typeof( Unpacker ).GetMethod( "ReadNullableUInt64" );
			directReadMethods.Add( typeof( System.UInt64 ), ReadNullableUInt64 );
			directReadMethods.Add( typeof( System.UInt64? ), ReadNullableUInt64 );
			
			ReadSingle = typeof( Unpacker ).GetMethod( "ReadSingle" );
			ReadNullableSingle = typeof( Unpacker ).GetMethod( "ReadNullableSingle" );
			directReadMethods.Add( typeof( System.Single ), ReadNullableSingle );
			directReadMethods.Add( typeof( System.Single? ), ReadNullableSingle );
			
			ReadDouble = typeof( Unpacker ).GetMethod( "ReadDouble" );
			ReadNullableDouble = typeof( Unpacker ).GetMethod( "ReadNullableDouble" );
			directReadMethods.Add( typeof( System.Double ), ReadNullableDouble );
			directReadMethods.Add( typeof( System.Double? ), ReadNullableDouble );
			
			ReadBoolean = typeof( Unpacker ).GetMethod( "ReadBoolean" );
			ReadNullableBoolean = typeof( Unpacker ).GetMethod( "ReadNullableBoolean" );
			directReadMethods.Add( typeof( System.Boolean ), ReadNullableBoolean );
			directReadMethods.Add( typeof( System.Boolean? ), ReadNullableBoolean );
			ReadString = typeof( Unpacker ).GetMethod( "ReadString" );
			directReadMethods.Add( typeof( string ), ReadString );
			ReadBinary = typeof( Unpacker ).GetMethod( "ReadBinary" );
			directReadMethods.Add( typeof( byte[] ), ReadBinary );
			ReadObject = typeof( Unpacker ).GetMethod( "ReadObject" );
			directReadMethods.Add( typeof( MessagePackObject ), ReadObject );

			Interlocked.Exchange( ref _directReadMethods, directReadMethods );
		}

		public static MethodInfo GetDirectReadMethod( Type type )
		{
			MethodInfo result;
			_directReadMethods.TryGetValue( type, out result );
			return result;
		}
	}
}