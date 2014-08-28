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
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Dictionary interface serializer.
	/// </summary>
	internal sealed class NonGenericDictionarySerializer : MessagePackSerializer<IDictionary>
	{
		private readonly IMessagePackSerializer _collectionDeserializer;
		private readonly ConstructorInfo _collectionConstructorWithoutCapacity;
		private readonly ConstructorInfo _collectionConstructorWithCapacity;

		public NonGenericDictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			if ( ownerContext.EmitterFlavor == EmitterFlavor.ReflectionBased )
			{
				this._collectionConstructorWithCapacity =
					targetType.GetConstructor( UnpackHelpers.CollectionConstructorWithCapacityParameterTypes );
				if ( this._collectionConstructorWithCapacity == null )
				{
					this._collectionConstructorWithoutCapacity = targetType.GetConstructor( ReflectionAbstractions.EmptyTypes );
					if ( this._collectionConstructorWithoutCapacity == null )
					{
						throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( targetType );
					}
				}
			}
			else
			{
				this._collectionDeserializer = ownerContext.GetSerializer( targetType );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override IDictionary UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			if ( this._collectionDeserializer != null )
			{
				// Fast path:
				return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary;
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection =
				( this._collectionConstructorWithoutCapacity != null
					? this._collectionConstructorWithoutCapacity.Invoke( null )
					: this._collectionConstructorWithCapacity.Invoke( new object[] { itemsCount } ) ) as IDictionary;
			UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void UnpackToCore( Unpacker unpacker, IDictionary collection )
		{
			if ( this._collectionDeserializer != null )
			{
				// Fast path:
				this._collectionDeserializer.UnpackTo( unpacker, collection );
			}
			else
			{
				if ( !unpacker.IsMapHeader )
				{
					throw SerializationExceptions.NewIsNotArrayHeader();
				}

				UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
			}
		}

		private static void UnpackToCore( Unpacker unpacker, IDictionary collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				MessagePackObject key;
				try
				{
					key = unpacker.ReadItemData();
				}
				catch ( InvalidMessagePackStreamException )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				MessagePackObject value;
				try
				{
					value = unpacker.ReadItemData();
				}
				catch ( InvalidMessagePackStreamException )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}
				collection.Add( key, value );
			}
		}
	}
}