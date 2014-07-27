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

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Wrapper to simulate type argument variance.
	/// </summary>
	/// <typeparam name="T">Actual collection type.</typeparam>
	internal class ReflectionCollectionSerializer<T> : MessagePackSerializer<T>
	{
		private readonly IMessagePackSerializer _collectionSerializer;

		public ReflectionCollectionSerializer(
			SerializationContext ownerContext,
			IMessagePackSerializer collectionSerializer )
			: base( ownerContext )
		{
			this._collectionSerializer = collectionSerializer;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._collectionSerializer.PackTo( packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return ( T )this._collectionSerializer.UnpackFrom( unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._collectionSerializer.UnpackTo( unpacker, collection );
		}
	}
}