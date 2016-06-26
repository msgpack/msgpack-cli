#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	// This file was generated from _UnpackHelpers.direct.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit _UnpackHelpers.direct.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class _UnpackHelpers
	{
		private static readonly Dictionary<Type, MethodInfo> _directUnpackMethods = GetDirectUnpackMethods( false );
		private static readonly Dictionary<Type, MethodInfo> _asyncDirectUnpackMethods =
#if FEATURE_TAP
		 GetDirectUnpackMethods( true );
#else
		_directUnpackMethods;
#endif // FEATURE_TAP

		private static Dictionary<Type, MethodInfo> GetDirectUnpackMethods( bool forAsync )
		{
			var suffix = forAsync ? "ValueAsync" : "Value";
			return
				new Dictionary<Type, MethodInfo>( 14 )
				{
			
					{ typeof( SByte ), typeof( UnpackHelpers ).GetMethod( "UnpackSByte" + suffix ) },
					{ typeof( SByte? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableSByte" + suffix ) },
			
					{ typeof( Int16 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt16" + suffix ) },
					{ typeof( Int16? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt16" + suffix ) },
			
					{ typeof( Int32 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt32" + suffix ) },
					{ typeof( Int32? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt32" + suffix ) },
			
					{ typeof( Int64 ), typeof( UnpackHelpers ).GetMethod( "UnpackInt64" + suffix ) },
					{ typeof( Int64? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableInt64" + suffix ) },
			
					{ typeof( Byte ), typeof( UnpackHelpers ).GetMethod( "UnpackByte" + suffix ) },
					{ typeof( Byte? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableByte" + suffix ) },
			
					{ typeof( UInt16 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt16" + suffix ) },
					{ typeof( UInt16? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt16" + suffix ) },
			
					{ typeof( UInt32 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt32" + suffix ) },
					{ typeof( UInt32? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt32" + suffix ) },
			
					{ typeof( UInt64 ), typeof( UnpackHelpers ).GetMethod( "UnpackUInt64" + suffix ) },
					{ typeof( UInt64? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableUInt64" + suffix ) },
			
					{ typeof( Single ), typeof( UnpackHelpers ).GetMethod( "UnpackSingle" + suffix ) },
					{ typeof( Single? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableSingle" + suffix ) },
			
					{ typeof( Double ), typeof( UnpackHelpers ).GetMethod( "UnpackDouble" + suffix ) },
					{ typeof( Double? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableDouble" + suffix ) },
			
					{ typeof( Boolean ), typeof( UnpackHelpers ).GetMethod( "UnpackBoolean" + suffix ) },
					{ typeof( Boolean? ), typeof( UnpackHelpers ).GetMethod( "UnpackNullableBoolean" + suffix ) },
					{ typeof( string ), typeof( UnpackHelpers ).GetMethod( "UnpackString" + suffix ) },
					{ typeof( byte[] ), typeof( UnpackHelpers ).GetMethod( "UnpackBinary" + suffix ) },
				};
		}

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