#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_Collections_DictionaryEntryMessagePackSerializer : MessagePackSerializer<DictionaryEntry>
	{
		public System_Collections_DictionaryEntryMessagePackSerializer( PackerCompatibilityOptions packerCompatibilityOptions )
			: base( packerCompatibilityOptions ) { }

		protected internal sealed override void PackToCore( Packer packer, DictionaryEntry objectTree )
		{
			packer.PackArrayHeader( 2 );
			packer.Pack( EnsureMessagePackObject( objectTree.Key ) );
			packer.Pack( EnsureMessagePackObject( objectTree.Value ) );
		}

		private static MessagePackObject EnsureMessagePackObject( object obj )
		{
			if ( obj == null )
			{
				return MessagePackObject.Nil;
			}

			if ( !( obj is MessagePackObject ) )
			{
				throw new NotSupportedException( "Only MessagePackObject Key/Value is supported." );
			}

			return ( MessagePackObject )obj;
		}

		protected internal sealed override DictionaryEntry UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.IsArrayHeader )
			{
				MessagePackObject key;
				MessagePackObject value;

				if ( !unpacker.ReadObject( out key ) )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( !unpacker.ReadObject( out value ) )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				return new DictionaryEntry( key, value );
			}
			else
			{
				// Previous DictionaryEntry serializer accidentally pack it as map...
				MessagePackObject key = default( MessagePackObject );
				MessagePackObject value = default( MessagePackObject );
				bool isKeyFound = false;
				bool isValueFound = false;
				string propertyName;

				while ( ( !isKeyFound || !isValueFound ) && unpacker.ReadString( out propertyName ) )
				{
					switch ( propertyName )
					{
						case "Key":
						{
							if ( !unpacker.ReadObject(out key) )
							{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
							}

							isKeyFound = true;
							break;
						}
						case "Value":
						{
							if ( !unpacker.ReadObject( out value ) )
							{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
							}

							isValueFound = true;
							break;
						}
					}
				}

				if ( !isKeyFound )
				{
				throw SerializationExceptions.NewMissingProperty( "Key" );
				}

				if ( !isValueFound )
				{
				throw SerializationExceptions.NewMissingProperty( "Value" );
				}

				return new DictionaryEntry( key, value );
			}
		}
	}
}