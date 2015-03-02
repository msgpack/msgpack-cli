// #region -- License Terms --
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
// #endregion -- License Terms --

using System;

namespace MsgPack.Serialization.Polymorphic
{
	internal sealed class PolymorphicSerializerProvider : MessagePackSerializerProvider
	{
		private readonly IMessagePackSerializer _valueSerializer;
		private readonly Type _targetType;

		public PolymorphicSerializerProvider( Type  targetType, IMessagePackSerializer valueSerializer )
		{
			this._targetType = targetType;
			this._valueSerializer = valueSerializer;
		}

		public override object Get( SerializationContext context, object providerParameter )
		{
			var schema = providerParameter as PolymorphismSchema;
			if ( schema == null || schema.UseDefault )
			{
				return this._valueSerializer;
			}

			if ( schema.UseTypeEmbedding )
			{
				return
					CreateSerializer(
						typeof( TypeEmbedingPolymorhicMessagePackSerializer<> ),
						this._targetType,
						context,
						schema
					);
			}
			else
			{
				return
					CreateSerializer(
						typeof( KnownTypePolymorhicMessagePackSerializer<> ),
						this._targetType,
						context,
						schema.CodeTypeMapping
					);
			}
		}

		private static object CreateSerializer( Type serializerType, Type targetType, params object[] arguments )
		{
			return Activator.CreateInstance( serializerType.MakeGenericType( targetType ), arguments );
		}
	}
}