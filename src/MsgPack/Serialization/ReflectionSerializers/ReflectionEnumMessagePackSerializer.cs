#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Globalization;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Non-generic portion of <see cref="ReflectionEnumMessagePackSerializer{T}"/>.
	/// </summary>
	internal static class ReflectionEnumMessagePackSerializer
	{
		public static object ToBoxedValue( MessagePackObject mpo, Type underlyingType )
		{
			if ( underlyingType == typeof( Int32 ) )
			{
				return mpo.AsInt32();
			}

			if ( underlyingType == typeof( Int64 ) )
			{
				return mpo.AsInt64();
			}

			if ( underlyingType == typeof( UInt32 ) )
			{
				return mpo.AsUInt32();
			}

			if ( underlyingType == typeof( UInt64 ) )
			{
				return mpo.AsUInt64();
			}

			if ( underlyingType == typeof( Int16 ) )
			{
				return mpo.AsInt16();
			}

			if ( underlyingType == typeof( UInt16 ) )
			{
				return mpo.AsUInt16();
			}

			if ( underlyingType == typeof( Byte ) )
			{
				return mpo.AsByte();
			}

			if ( underlyingType == typeof( SByte ) )
			{
				return mpo.AsSByte();
			}

			throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture,"'{0}' is not supported for enum underlying type.", underlyingType  ));
		}
	}
}