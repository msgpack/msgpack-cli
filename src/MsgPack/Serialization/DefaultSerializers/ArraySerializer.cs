#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
			return Create( context, typeof( T ) ) as MessagePackSerializer<T>;
		}

		public static IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType ) {
#if DEBUG
			Contract.Assert( targetType.IsArray );
#endif
			return
				( GetPrimitiveArraySerializer( context, targetType )
				?? Activator.CreateInstance(
					typeof( ArraySerializer<> ).MakeGenericType( targetType.GetElementType() ),
					context
				) )
				as IMessagePackSingleObjectSerializer;
		}

		private static object GetPrimitiveArraySerializer( SerializationContext context, Type targetType )
		{
#if DEBUG
			Contract.Assert( targetType.IsArray );
#endif

			Func<SerializationContext, object> serializerFactory;
			if ( !_arraySerializerFactories.TryGetValue( targetType, out serializerFactory ) )
			{
				return null;
			}

			return serializerFactory( context );
		}
	}
}
