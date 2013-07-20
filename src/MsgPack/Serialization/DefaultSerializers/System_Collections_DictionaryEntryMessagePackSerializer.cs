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
		public System_Collections_DictionaryEntryMessagePackSerializer() { }

		protected internal sealed override void PackToCore( Packer packer, DictionaryEntry objectTree )
		{
			packer.PackMapHeader( 2 );
			packer.PackString( "Key" );
			packer.Pack( EnsureMessagePackObject( objectTree.Key ) );
			packer.PackString( "Value" );
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
			object key = null;
			object value = null;
			bool isKeyFound = false;
			bool isValueFound = false;

			while ( unpacker.Read() )
			{
				if ( !unpacker.Data.HasValue )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				switch ( unpacker.Data.Value.DeserializeAsString() )
				{
					case "Key":
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						isKeyFound = true;
						key = unpacker.Data.Value;
						break;
					}
					case "Value":
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						isValueFound = true;
						value = unpacker.Data.Value;
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