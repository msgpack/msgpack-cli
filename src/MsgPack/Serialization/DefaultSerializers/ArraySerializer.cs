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
	internal static partial class ArraySerializer
	{
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).IsArray );
#endif
			return
				( GetPrimitiveArraySerializer<T>( context )
				?? Activator.CreateInstance(
					typeof( ArraySerializer<> ).MakeGenericType( typeof( T ).GetElementType() ),
					context
				) )
				as MessagePackSerializer<T>;
		}

		private static object GetPrimitiveArraySerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).IsArray );
#endif

			Func<SerializationContext, object> serializerFactory;
			if ( !_arraySerializerFactories.TryGetValue( typeof( T ), out serializerFactory ) )
			{
				return null;
			}

			var serializer = serializerFactory( context ) as MessagePackSerializer<T>;
#if DEBUG
			Contract.Assert( serializer != null, serializerFactory( context ) + " is " + typeof( MessagePackSerializer<T> ) );
#endif
			return serializer;
		}
	}
}
