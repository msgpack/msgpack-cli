#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	// This file was generated from _UnpackHelpers.direct.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit _UnpackHelpers.direct.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class _UnpackHelpers
	{
		private static readonly Dictionary<Type, MethodInfo> _directUnpackMethods = GetDirectUnpackMethods();
		private static readonly Dictionary<Type, MethodInfo> _asyncDirectUnpackMethods =
#if FEATURE_TAP
		 GetAsyncDirectUnpackMethods();
#else
		_directUnpackMethods;
#endif // FEATURE_TAP


		private static Dictionary<Type, MethodInfo> GetDirectUnpackMethods()
		{
			return
				new Dictionary<Type, MethodInfo>( 14 )
				{
					{ typeof( SByte ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackSByteValue ) ) },
					{ typeof( SByte? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableSByteValue ) ) },
					{ typeof( Int16 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt16Value ) ) },
					{ typeof( Int16? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt16Value ) ) },
					{ typeof( Int32 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt32Value ) ) },
					{ typeof( Int32? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt32Value ) ) },
					{ typeof( Int64 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt64Value ) ) },
					{ typeof( Int64? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt64Value ) ) },
					{ typeof( Byte ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackByteValue ) ) },
					{ typeof( Byte? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableByteValue ) ) },
					{ typeof( UInt16 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt16Value ) ) },
					{ typeof( UInt16? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt16Value ) ) },
					{ typeof( UInt32 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt32Value ) ) },
					{ typeof( UInt32? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt32Value ) ) },
					{ typeof( UInt64 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt64Value ) ) },
					{ typeof( UInt64? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt64Value ) ) },
					{ typeof( Single ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackSingleValue ) ) },
					{ typeof( Single? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableSingleValue ) ) },
					{ typeof( Double ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackDoubleValue ) ) },
					{ typeof( Double? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableDoubleValue ) ) },
					{ typeof( Boolean ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackBooleanValue ) ) },
					{ typeof( Boolean? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableBooleanValue ) ) },
					{ typeof( string ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackStringValue ) ) },
					{ typeof( byte[] ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackBinaryValue ) ) },
				};
		}

#if FEATURE_TAP

		private static Dictionary<Type, MethodInfo> GetAsyncDirectUnpackMethods()
		{
			return
				new Dictionary<Type, MethodInfo>( 14 )
				{
					{ typeof( SByte ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackSByteValueAsync ) ) },
					{ typeof( SByte? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableSByteValueAsync ) ) },
					{ typeof( Int16 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt16ValueAsync ) ) },
					{ typeof( Int16? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt16ValueAsync ) ) },
					{ typeof( Int32 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt32ValueAsync ) ) },
					{ typeof( Int32? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt32ValueAsync ) ) },
					{ typeof( Int64 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackInt64ValueAsync ) ) },
					{ typeof( Int64? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableInt64ValueAsync ) ) },
					{ typeof( Byte ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackByteValueAsync ) ) },
					{ typeof( Byte? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableByteValueAsync ) ) },
					{ typeof( UInt16 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt16ValueAsync ) ) },
					{ typeof( UInt16? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt16ValueAsync ) ) },
					{ typeof( UInt32 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt32ValueAsync ) ) },
					{ typeof( UInt32? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt32ValueAsync ) ) },
					{ typeof( UInt64 ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackUInt64ValueAsync ) ) },
					{ typeof( UInt64? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableUInt64ValueAsync ) ) },
					{ typeof( Single ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackSingleValueAsync ) ) },
					{ typeof( Single? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableSingleValueAsync ) ) },
					{ typeof( Double ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackDoubleValueAsync ) ) },
					{ typeof( Double? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableDoubleValueAsync ) ) },
					{ typeof( Boolean ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackBooleanValueAsync ) ) },
					{ typeof( Boolean? ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackNullableBooleanValueAsync ) ) },
					{ typeof( string ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackStringValueAsync ) ) },
					{ typeof( byte[] ), typeof( UnpackHelpers ).GetMethod( nameof( UnpackHelpers.UnpackBinaryValueAsync ) ) },
				};
		}
#endif // FEATURE_TAP

		public static MethodInfo GetDirectUnpackMethod( Type type, bool forAsync )
		{
			MethodInfo result;
			if ( ( forAsync ? _asyncDirectUnpackMethods : _directUnpackMethods ).TryGetValue( type, out result ) )
			{
#if DEBUG
				Contract.Assert( result != null, "Failed to initialize value for " + type );
#endif
			}
			return result;
		}
	}
}
