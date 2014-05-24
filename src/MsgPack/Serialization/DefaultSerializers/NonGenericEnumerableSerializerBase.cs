#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Collections;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Provides basic features for non dictionary, non-generic collection interface serializers.
	/// </summary>
	/// <typeparam name="T">The type of the item of collection.</typeparam>
	internal abstract class NonGenericEnumerableSerializerBase<T> : MessagePackSerializer<T>
		where T : IEnumerable
	{
		private readonly IMessagePackSerializer _collectionDeserializer;

		protected NonGenericEnumerableSerializerBase( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._collectionDeserializer = ownerContext.GetSerializer( targetType );
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this.PackArrayHeader( packer, objectTree );
			foreach ( var item in objectTree )
			{
				if ( !( item is MessagePackObject ) )
				{
					throw new SerializationException("Non generic collection may contain only MessagePackObject type.");
				}

				( item as IPackable ).PackToMessage( packer, null );
			}
		}

		protected abstract void PackArrayHeader( Packer packer, T objectTree );

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return ( T ) this._collectionDeserializer.UnpackFrom( unpacker );
		}
	}
}