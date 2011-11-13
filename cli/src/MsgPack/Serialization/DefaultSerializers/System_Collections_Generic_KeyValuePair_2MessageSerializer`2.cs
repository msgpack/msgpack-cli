#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
	internal sealed class System_Collections_Generic_KeyValuePair_2MessageSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
	{
		private readonly SerializationContext _context;

		public System_Collections_Generic_KeyValuePair_2MessageSerializer()
			: this( null, null ) { }

		public System_Collections_Generic_KeyValuePair_2MessageSerializer( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			this._context = new SerializationContext( marshalers ?? MarshalerRepository.Default, serializers ?? SerializerRepository.Default );
		}

		protected sealed override void PackToCore( Packer packer, KeyValuePair<TKey, TValue> objectTree )
		{
			packer.PackMapHeader( 2 );
			packer.PackString( "Key" );
			this._context.MarshalTo( packer, objectTree.Key );
			packer.PackString( "Value" );
			this._context.MarshalTo( packer, objectTree.Value );
		}

		protected sealed override KeyValuePair<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			TKey key = default( TKey );
			TValue value = default( TValue );
			bool isKeyFound = false;
			bool isValueFound = false;
			while ( unpacker.MoveToNextEntry() )
			{
				if ( !unpacker.Data.HasValue )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				switch ( unpacker.Data.Value.AsString() )
				{
					case "Key":
					{
						if ( !unpacker.MoveToNextEntry() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						isKeyFound = true;
						key = this._context.UnmarshalFrom<TKey>( unpacker );
						break;
					}
					case "Value":
					{
						if ( !unpacker.MoveToNextEntry() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						isValueFound = true;
						value = this._context.UnmarshalFrom<TValue>( unpacker );
						break;
					}
				}
			}

			unpacker.MoveToEndCollection();

			if ( !isKeyFound )
			{
				throw SerializationExceptions.NewMissingProperty( "Key" );
			}

			if ( !isValueFound )
			{
				throw SerializationExceptions.NewMissingProperty( "Value" );
			}

			return new KeyValuePair<TKey, TValue>( key, value );
		}
	}
}
