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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Globalization;
using System.Runtime.Serialization;

using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Wraps non-generic <see cref="IMessagePackSingleObjectSerializer"/> to avoid AOT issue.
	/// </summary>
	/// <typeparam name="T">The type to be serialized.</typeparam>
	internal class TypedMessagePackSerializerWrapper<T> : MessagePackSerializer<T>, ICollectionInstanceFactory
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

		protected internal override T UnpackNil()
		{
			var asTyped = this._underlyingSerializer as MessagePackSerializer<T>;
			if ( asTyped != null )
			{
				return asTyped.UnpackNil();
			}
			else
			{
				return base.UnpackNil();
			}
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
}