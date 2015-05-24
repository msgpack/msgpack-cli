#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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

using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Implements non-generic or common portion of abstract collection serializers.
	/// </summary>
	internal static class AbstractCollectionSerializerHelper
	{
		public static void GetConcreteSerializer(
			SerializationContext context,
			PolymorphismSchema schema,
			Type abstractType,
			Type targetType,
			Type exampleType,
			out ICollectionInstanceFactory factory,
			out IMessagePackSingleObjectSerializer serializer
		)
		{
			if ( abstractType == targetType )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( abstractType );
			}

			serializer = context.GetSerializer( targetType, schema );
			factory = serializer as ICollectionInstanceFactory;
			if ( factory == null && (serializer as IPolymorphicDeserializer) == null )
			{
				throw SerializationExceptions.NewIncompatibleCollectionSerializer( abstractType, serializer.GetType(), exampleType );
			}
		}
	}
}