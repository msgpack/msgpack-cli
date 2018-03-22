#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2018 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Provides polymorphism for serializers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
#if UNITY && DEBUG
	public
#else
	internal 
#endif
	sealed class PolymorphicSerializerProvider<T> : MessagePackSerializerProvider
	{
		// This may be null for abstract typed collection which does not have corresponding concrete type.
		private readonly MessagePackSerializer<T> _defaultSerializer;
		private readonly PolymorphismSchema _defaultSchema;

#if !UNITY
		public PolymorphicSerializerProvider( MessagePackSerializer<T> defaultSerializer )
		{
			this._defaultSerializer = defaultSerializer;
			this._defaultSchema = PolymorphismSchema.Create( typeof( T ), null );
		}
#else
		public PolymorphicSerializerProvider( SerializationContext context, MessagePackSerializer defaultSerializer )
		{
			this._defaultSerializer = MessagePackSerializer.Wrap<T>( context, defaultSerializer );
			this._defaultSchema = PolymorphismSchema.Create( typeof( T ), null );
		}
#endif

		public override object Get( SerializationContext context, object providerParameter )
		{
			var schema = ( providerParameter ?? this._defaultSchema ) as PolymorphismSchema;

			if ( schema == null || schema.UseDefault || schema.TargetType != typeof( T ) )
			{
				// No schema is applied or this provider is used for container but the schema is only applied for keys/items.
				if ( this._defaultSerializer == null )
				{
					throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( typeof( T ) );
				}

				// Fallback.
				return this._defaultSerializer;
			}

			if ( schema.UseTypeEmbedding )
			{
				return new TypeEmbedingPolymorphicMessagePackSerializer<T>( context, schema );
			}
			else
			{
				return new KnownTypePolymorphicMessagePackSerializer<T>( context, schema );
			}
		}
	}
}
