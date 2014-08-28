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
	///		Provides basic features for non-dictionary generic collection serializers.
	/// </summary>
	/// <typeparam name="T">The type of the collection.</typeparam>
	/// <typeparam name="TItem">The type of the item of the collection.</typeparam>
	internal abstract class EnumerableSerializerBase<T, TItem> : MessagePackSerializer<T>
		where T : IEnumerable<TItem>
	{
		private readonly MessagePackSerializer<TItem> _itemSerializer;
		private readonly IMessagePackSerializer _collectionDeserializer;
		private readonly MethodInfo _addItem;
		private readonly ConstructorInfo _collectionConstructorWithoutCapacity;
		private readonly ConstructorInfo _collectionConstructorWithCapacity;

		protected EnumerableSerializerBase( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>();
			if ( ownerContext.EmitterFlavor == EmitterFlavor.ReflectionBased )
			{
				// First use abstract type instead of surrogate concrete type.
				var traits = typeof( T ).GetCollectionTraits();
				if ( traits.AddMethod != null )
				{
					this._addItem = traits.AddMethod;
				}
				else
				{
					// Try use concrete type method... it might fail.
					traits = targetType.GetCollectionTraits();
					if ( traits.AddMethod != null )
					{
						this._addItem = traits.AddMethod;
					}
				}

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

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this.PackArrayHeader( packer, objectTree );
			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		protected abstract void PackArrayHeader( Packer packer, T objectTree );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			if ( this._collectionDeserializer != null )
			{
				// Fast path:
				return ( T )this._collectionDeserializer.UnpackFrom( unpacker );
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection =
				( T )( this._collectionConstructorWithoutCapacity != null
					? this._collectionConstructorWithoutCapacity.Invoke( null )
					: this._collectionConstructorWithCapacity.Invoke( new object[] { itemsCount } ) );
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._collectionDeserializer != null )
			{
				// Fast path:
				this._collectionDeserializer.UnpackTo( unpacker, collection );
			}
			else
			{
				if ( this._addItem == null )
				{
					throw SerializationExceptions.NewUnpackToIsNotSupported( typeof( T ), null );
				}

				if ( !unpacker.IsArrayHeader )
				{
					throw SerializationExceptions.NewIsNotArrayHeader();
				}

				this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
			}
		}

		protected void UnpackToCore( Unpacker unpacker, T collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				TItem item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = this._itemSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = this._itemSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				this.AddItem( collection, item );
			}
		}

		protected virtual void AddItem( T collection, TItem item )
		{
			if ( this._addItem == null )
			{
				throw SerializationExceptions.NewUnpackToIsNotSupported( typeof( T ), null );
			}

			try
			{
				this._addItem.Invoke( collection, new object[] { item } );
			}
			catch ( TargetInvocationException )
			{
				throw;
			}
			catch ( Exception ex )
			{
				throw SerializationExceptions.NewUnpackToIsNotSupported( typeof( T ), ex );
			}
		}
	}
}