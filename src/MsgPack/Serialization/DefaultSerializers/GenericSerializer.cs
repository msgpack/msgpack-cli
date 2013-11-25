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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Defines serializer factory for well known structured types.
	/// </summary>
	internal static class GenericSerializer
	{
		public static MessagePackSerializer<T> CreateArraySerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).IsArray );
#endif
			return
				Activator.CreateInstance(
					typeof( ArraySerializer<> ).MakeGenericType( typeof( T ).GetElementType() ), 
					context
				) as MessagePackSerializer<T>;

		}

		public static MessagePackSerializer<T> CreateNullableSerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( Nullable.GetUnderlyingType( typeof( T ) ) != null );
#endif
			return new NullableMessagePackSerializer<T>( context );
		}

		public static MessagePackSerializer<T> CreateEnumSerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).GetIsEnum() );
#endif
			return
				new EnumMessagePackSerializer<T>(
					( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions 
				);
		}
	}
}
