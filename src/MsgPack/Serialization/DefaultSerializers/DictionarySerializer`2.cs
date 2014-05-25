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
using System.Collections.Generic;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Dictionary interface serializer.
	/// </summary>
	/// <typeparam name="TKey">The type of the key of dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the value of dictionary.</typeparam>
	internal sealed class DictionarySerializer<TKey, TValue> : MessagePackSerializer<IDictionary<TKey, TValue>>
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;
		private readonly IMessagePackSerializer _collectionDeserializer;

		public DictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
			this._collectionDeserializer = ownerContext.GetSerializer( targetType );
		}

		protected internal override void PackToCore( Packer packer, IDictionary<TKey, TValue> objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
		}

		protected internal override IDictionary<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary<TKey, TValue>;
		}
	}
}