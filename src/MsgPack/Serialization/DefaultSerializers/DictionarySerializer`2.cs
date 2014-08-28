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
using System.Reflection;

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
		private readonly ConstructorInfo _collectionConstructorWithoutCapacity;
		private readonly ConstructorInfo _collectionConstructorWithCapacity;

		public DictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
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
		protected internal override void PackToCore( Packer packer, IDictionary<TKey, TValue> objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override IDictionary<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			if ( this._collectionDeserializer != null )
			{
				// Fast path:
				return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary<TKey, TValue>;
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection =
				( this._collectionConstructorWithoutCapacity != null
					? this._collectionConstructorWithoutCapacity.Invoke( null )
					: this._collectionConstructorWithCapacity.Invoke( new object[] { itemsCount } ) ) as IDictionary<TKey, TValue>;
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
		protected internal override void UnpackToCore( Unpacker unpacker, IDictionary<TKey, TValue> collection )
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

				this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
			}
		}

		private void UnpackToCore( Unpacker unpacker, IDictionary<TKey, TValue> collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}
	
				TKey key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = this._keySerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}
	

				TValue value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = this._valueSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = this._valueSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				collection.Add( key, value );
			}
		}
	}
}