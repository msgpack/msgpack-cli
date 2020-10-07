// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Providers generic serializer instance factory with reflection.
	/// </summary>
	internal static partial class SerializerFactory
	{
		private static readonly Type[] StandardObjectSerializerConstructorParameterTypes = { typeof(SerializerProvider) };

		public static Serializer CreateSerializer(
			Type targetType,
			CodecProvider codecProvider,
			ObjectSerializer underlying,
			SerializationOptions serializationOptions,
			DeserializationOptions deserializationOptions
		)
			// We cannot use ArrayPool<T> here because it does not guarntee returning array length, but Reflection APIs relies it.
			=> typeof(DefaultSerializer<>)
				.MakeGenericType(targetType)
				.GetConstructor(
					new[]
					{
						typeof(CodecProvider),
						typeof(ObjectSerializer<>).MakeGenericType(targetType),
						typeof(SerializationOptions),
						typeof(DeserializationOptions)
					}
				)!.InvokePreservingExceptionType<Serializer>(codecProvider, underlying, serializationOptions, deserializationOptions);

		public static ObjectSerializer CreateObjectSerializer(Type genericSerializerType, Type targetType, SerializerProvider provider)
			// We cannot use ArrayPool<T> here because it does not guarntee returning array length, but Reflection APIs relies it.
			=> genericSerializerType
				.MakeGenericType(targetType)
#warning TODO: ArrayCache(1,2,3)
				.GetRequiredConstructor(StandardObjectSerializerConstructorParameterTypes)
				.InvokePreservingExceptionType<ObjectSerializer>(provider);

		public static ObjectSerializer CreateObjectSerializer<TArg>(Type genericSerializerType, Type targetType, SerializerProvider provider, TArg arg)
			// We cannot use ArrayPool<T> here because it does not guarntee returning array length, but Reflection APIs relies it.
			=> genericSerializerType
				.MakeGenericType(targetType)
				.GetRequiredConstructor(typeof(SerializerProvider), typeof(TArg))
				.InvokePreservingExceptionType<ObjectSerializer>(provider, arg);

		public static ObjectSerializer CreateObjectSerializer<TArg1, TArg2>(Type genericSerializerType, Type targetType, SerializerProvider provider, TArg1 arg1, TArg2 arg2)
			// We cannot use ArrayPool<T> here because it does not guarntee returning array length, but Reflection APIs relies it.
			=> genericSerializerType
				.MakeGenericType(targetType)
				.GetRequiredConstructor(typeof(SerializerProvider), typeof(TArg1), typeof(TArg2))
				.InvokePreservingExceptionType<ObjectSerializer>(provider, arg1, arg2);

		public static ObjectSerializer CreateNullableObjectSerializer(Type targetType, Type underlyingType, SerializerProvider provider, ObjectSerializer underlyingSerializer)
			=> typeof(NullableObjectSerializer<>)
				.MakeGenericType(targetType)
				.GetRequiredConstructor(typeof(SerializerProvider), typeof(ObjectSerializer<>).MakeGenericType(underlyingType))
				.InvokePreservingExceptionType<ObjectSerializer>(provider, underlyingSerializer);

		public static ObjectSerializer CreateEnumSerializer(
			Type targetType, 
			SerializerProvider provider, 
			EnumSerializationMethod? method, 
			NameMapper nameMapper,
			object serializationNameMapping,
			object deserializationNameMapping
		)
		{
			Debug.Assert(targetType.GetIsEnum());

			return
				GetEnumSerializerGenericTypeDefinition(Enum.GetUnderlyingType(targetType))
				.MakeGenericType(targetType)
				.GetRequiredConstructor(
					typeof(SerializerProvider),
					typeof(EnumSerializationMethod?),
					typeof(NameMapper),
					typeof(Dictionary<,>).MakeGenericType(targetType, typeof(string)),
					typeof(Dictionary<,>).MakeGenericType(typeof(string), targetType)
				).InvokePreservingExceptionType<ObjectSerializer>(
					provider,
					method,
					nameMapper,
					serializationNameMapping,
					deserializationNameMapping
				);
		}
	}
}
