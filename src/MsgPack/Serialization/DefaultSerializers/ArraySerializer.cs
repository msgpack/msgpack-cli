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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
	internal static partial class ArraySerializer
	{
		public static MessagePackSerializer<T> Create<T>( SerializationContext context, PolymorphismSchema itemsSchema )
		{
			return Create( context, typeof( T ), itemsSchema ) as MessagePackSerializer<T>;
		}

		public static IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema itemsSchema ) 
		{
#if DEBUG && !UNITY
			Contract.Assert( targetType.IsArray, "targetType.IsArray" );
#endif // DEBUG && !UNITY

			// Check the T is SZArray -- Type.GetArrayRank() returns 1 for single dimension, non-zero based arrays, so use (SZArrayType).IsAssinableFrom() instead.
			if ( targetType.GetElementType().MakeArrayType().IsAssignableFrom( targetType ) )
			{
				return
					( GetPrimitiveArraySerializer( context, targetType )
#if !UNITY
					?? ReflectionExtensions.CreateInstancePreservingExceptionType(
							typeof( ArraySerializer<> ).MakeGenericType( targetType.GetElementType() ),
							context,
							itemsSchema
						)
#else
					?? new UnityArraySerializer( context, targetType.GetElementType(), itemsSchema )
#endif
					) as IMessagePackSingleObjectSerializer;
			}
			else
			{
				return
#if !UNITY
					ReflectionExtensions.CreateInstancePreservingExceptionType(
						typeof( MultidimensionalArraySerializer<,> ).MakeGenericType( targetType, targetType.GetElementType() ),
						context,
						itemsSchema
					) as IMessagePackSingleObjectSerializer;
#else
					new UnityMultidimensionalArraySerializer( context, targetType.GetElementType(), itemsSchema );
#endif
			}
		}

		private static object GetPrimitiveArraySerializer( SerializationContext context, Type targetType )
		{
#if DEBUG && !UNITY
			Contract.Assert( targetType.IsArray, "targetType.IsArray" );
#endif // DEBUG && !UNITY

			Func<SerializationContext, object> serializerFactory;
			if ( !_arraySerializerFactories.TryGetValue( targetType, out serializerFactory ) )
			{
				return null;
			}

			return serializerFactory( context );
		}
	}
}
