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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if UNITY
using System.Globalization;
using System.Runtime.Serialization;

using MsgPack.Serialization.Reflection;
using MsgPack.Serialization.CollectionSerializers;
#endif // UNITY

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Provides polymorphism for serializers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class PolymorphicSerializerProvider<T> : MessagePackSerializerProvider
	{
		// This may be null for abstract typed collection which does not have corresponding concrete type.
		private readonly MessagePackSerializer<T> _defaultSerializer;

#if !UNITY
		public PolymorphicSerializerProvider( MessagePackSerializer<T> defaultSerializer )
		{
			this._defaultSerializer = defaultSerializer;
		}
#else
		public PolymorphicSerializerProvider( SerializationContext context, IMessagePackSingleObjectSerializer defaultSerializer )
		{
			this._defaultSerializer =
				defaultSerializer == null
				? null
				: defaultSerializer is ICustomizableEnumSerializer
				? new EnumTypedMessagePackSerializerWrapper( context, defaultSerializer )
				: new TypedMessagePackSerializerWrapper( context, defaultSerializer );
		}
#endif

		public override object Get( SerializationContext context, object providerParameter )
		{
			var schema = providerParameter as PolymorphismSchema;

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
				return new TypeEmbedingPolymorhicMessagePackSerializer<T>( context, schema );
			}
			else
			{
				return new KnownTypePolymorhicMessagePackSerializer<T>( context, schema );
			}
		}

#if UNITY
		private class TypedMessagePackSerializerWrapper : MessagePackSerializer<T>, ICollectionInstanceFactory
		{
			private readonly IMessagePackSingleObjectSerializer _underlyingSerializer;
			private readonly ICollectionInstanceFactory _underlyingFactory;

			public TypedMessagePackSerializerWrapper( SerializationContext context, IMessagePackSingleObjectSerializer underlying )
				: base( context )
			{
				this._underlyingSerializer = underlying;
				this._underlyingFactory = underlying as ICollectionInstanceFactory;
			}

			protected internal override void PackToCore( Packer packer, T objectTree )
			{
				this._underlyingSerializer.PackTo( packer, objectTree );
			}

			protected internal override T UnpackFromCore( Unpacker unpacker )
			{
				return ( T )this._underlyingSerializer.UnpackFrom( unpacker );
			}

			protected internal override void UnpackToCore( Unpacker unpacker, T collection )
			{
				this._underlyingSerializer.UnpackTo( unpacker, collection );
			}

			object ICollectionInstanceFactory.CreateInstance( int initialCapacity )
			{
				if ( this._underlyingFactory == null )
				{
					throw
						new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot serialize type '{0}' because registered or generated serializer '{1}' does not implement '{2}'.",
								typeof( T ).GetFullName(),
								this._underlyingSerializer.GetType().GetFullName(),
								typeof( ICollectionInstanceFactory )
							)
						);

				}

				return this._underlyingFactory.CreateInstance( initialCapacity );
			}
		}

		private sealed class EnumTypedMessagePackSerializerWrapper : TypedMessagePackSerializerWrapper, ICustomizableEnumSerializer
		{
			private readonly ICustomizableEnumSerializer _underlyingEnumSerializer;

			public EnumTypedMessagePackSerializerWrapper( SerializationContext context, IMessagePackSingleObjectSerializer underlying )
				: base( context, underlying )
			{
				this._underlyingEnumSerializer = underlying as ICustomizableEnumSerializer;
			}

			ICustomizableEnumSerializer ICustomizableEnumSerializer.GetCopyAs( EnumSerializationMethod method )
			{
				return this._underlyingEnumSerializer.GetCopyAs( method );
			}
		}
#endif
	}
}