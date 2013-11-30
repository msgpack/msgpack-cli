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

namespace MsgPack.Serialization.Metadata
{
	// This file was generated from _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit _Unpacker.Read.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class _UnpackHelpers
	{
		private static readonly Dictionary<Type, MethodInfo> _directUnpackMethods = GetDirectUnpackMethods();

		private static Dictionary<Type, MethodInfo> GetDirectUnpackMethods()
		{
			return
				new Dictionary<Type, MethodInfo>( 14 )
				{
			
					{ typeof( SByte ), typeof( UnpackHelpers ).GetMethod( "UnpackSByteValue" ) },
					{ typeof( SByte? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableSByteValue" ) },
			
					{ typeof( Int16 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt16Value" ) },
					{ typeof( Int16? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt16Value" ) },
			
					{ typeof( Int32 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt32Value" ) },
					{ typeof( Int32? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt32Value" ) },
			
					{ typeof( Int64 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt64Value" ) },
					{ typeof( Int64? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt64Value" ) },
			
					{ typeof( Byte ), typeof( UnpackHelpers ).GetMethod( "UnpackByteValue" ) },
					{ typeof( Byte? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableByteValue" ) },
			
					{ typeof( UInt16 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt16Value" ) },
					{ typeof( UInt16? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt16Value" ) },
			
					{ typeof( UInt32 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt32Value" ) },
					{ typeof( UInt32? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt32Value" ) },
			
					{ typeof( UInt64 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt64Value" ) },
					{ typeof( UInt64? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt64Value" ) },
			
					{ typeof( Single ), typeof( UnpackHelpers ).GetMethod( "UnpackSingleValue" ) },
					{ typeof( Single? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableSingleValue" ) },
			
					{ typeof( Double ), typeof( UnpackHelpers ).GetMethod( "UnpackDoubleValue" ) },
					{ typeof( Double? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableDoubleValue" ) },
			
					{ typeof( Boolean ), typeof( UnpackHelpers ).GetMethod( "UnpackBooleanValue" ) },
					{ typeof( Boolean? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableBooleanValue" ) },
					{ typeof( string ), typeof( UnpackHelpers ).GetMethod( "UnpackStringValue" ) },
					{ typeof( byte[] ), typeof( UnpackHelpers ).GetMethod( "UnpackBinaryValue" ) },
				};
		}

		public static MethodInfo GetDirectUnpackMethod( Type type )
		{
			MethodInfo result;
			if ( _directUnpackMethods.TryGetValue( type, out result ) )
			{
#if DEBUG
				System.Diagnostics.Contracts.Contract.Assert( result != null, "Failed to initialize value for " + type );
#endif
			}
			return result;
		}
	}
}