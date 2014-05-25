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
	///		Dictionary interface serializer.
	/// </summary>
	internal sealed class NonGenericDictionarySerializer : MessagePackSerializer<IDictionary>
	{
		private readonly IMessagePackSerializer _collectionDeserializer;

		public NonGenericDictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._collectionDeserializer = ownerContext.GetSerializer( targetType );
		}

		protected internal override void PackToCore( Packer packer, IDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( DictionaryEntry item in objectTree )
			{
				if ( !( item.Key is MessagePackObject ) )
				{
					throw new SerializationException("Non generic dictionary may contain only MessagePackObject typed key.");
				}

				( item.Key as IPackable ).PackToMessage( packer, null );

				if ( !( item.Value is MessagePackObject ) )
				{
					throw new SerializationException("Non generic dictionary may contain only MessagePackObject typed value.");
				}

				( item.Value as IPackable ).PackToMessage( packer, null );			}
		}

		protected internal override IDictionary UnpackFromCore( Unpacker unpacker )
		{
			return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary;
		}
	}
}