#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	[Preserve( AllMembers = true )]
	internal sealed class System_Collections_DictionaryEntryMessagePackSerializer : MessagePackSerializer<DictionaryEntry>
	{
		public System_Collections_DictionaryEntryMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, DictionaryEntry objectTree )
		{
			packer.PackArrayHeader( 2 );
			EnsureMessagePackObject( objectTree.Key ).PackToMessage( packer, null );
			EnsureMessagePackObject( objectTree.Value ).PackToMessage( packer, null );
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override DictionaryEntry UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.IsArrayHeader )
			{
				MessagePackObject key;
				MessagePackObject value;

				if ( !unpacker.ReadObject( out key ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !unpacker.ReadObject( out value ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
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
							if ( !unpacker.ReadObject( out key ) )
							{
								SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
							}

							isKeyFound = true;
							break;
						}
						case "Value":
						{
							if ( !unpacker.ReadObject( out value ) )
							{
								SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
							}

							isValueFound = true;
							break;
						}
					}
				}

				if ( !isKeyFound )
				{
					SerializationExceptions.ThrowMissingProperty( "Key" );
				}

				if ( !isValueFound )
				{
					SerializationExceptions.ThrowMissingProperty( "Value" );
				}

				return new DictionaryEntry( key, value );
			}
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, DictionaryEntry objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await EnsureMessagePackObject( objectTree.Key ).PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
			await EnsureMessagePackObject( objectTree.Value ).PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
		}

		protected internal override async Task<DictionaryEntry> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( unpacker.IsArrayHeader )
			{
				var key = await unpacker.ReadObjectAsync( cancellationToken ).ConfigureAwait( false );
				if ( !key.Success )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				var value = await unpacker.ReadObjectAsync( cancellationToken ).ConfigureAwait( false );
				if ( !value.Success )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				return new DictionaryEntry( key.Value, value.Value );
			}
			else
			{
				// Previous DictionaryEntry serializer accidentally pack it as map...
				AsyncReadResult<MessagePackObject> key = default( AsyncReadResult<MessagePackObject> );
				AsyncReadResult<MessagePackObject> value = default( AsyncReadResult<MessagePackObject> );

				for ( var propertyName = await unpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
					( !key.Success || !value.Success ) && propertyName.Success;
					propertyName = await unpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					switch ( propertyName.Value )
					{
						case "Key":
						{
							key = await unpacker.ReadObjectAsync( cancellationToken ).ConfigureAwait( false );
							if ( !key.Success )
							{
								SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
							}

							break;
						}
						case "Value":
						{
							value = await unpacker.ReadObjectAsync( cancellationToken ).ConfigureAwait( false );
							if ( !value.Success )
							{
								SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
							}

							break;
						}
					}
				}

				if ( !key.Success )
				{
					SerializationExceptions.ThrowMissingProperty( "Key" );
				}

				if ( !value.Success )
				{
					SerializationExceptions.ThrowMissingProperty( "Value" );
				}

				return new DictionaryEntry( key.Value, value.Value );
			}
		}

#endif // FEATURE_TAP

	}
}