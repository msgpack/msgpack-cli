#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides basic features for non-dictionary non-generic collection serializers.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <remarks>
	///		This class provides framework to implement variable collection serializer, and this type seals some virtual members to maximize future backward compatibility.
	///		If you cannot use this class, you can implement your own serializer which inherits <see cref="MessagePackSerializer{T}"/> and implements <see cref="ICollectionInstanceFactory"/>.
	/// </remarks>
	public abstract class NonGenericEnumerableMessagePackSerializerBase<TCollection> : MessagePackSerializer<TCollection>, ICollectionInstanceFactory
		where TCollection : IEnumerable
	{
		private readonly IMessagePackSingleObjectSerializer _itemSerializer;

		internal IMessagePackSingleObjectSerializer ItemSerializer { get { return this._itemSerializer; } }

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericEnumerableMessagePackSerializerBase{TCollection}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by base .ctor" )]
		protected NonGenericEnumerableMessagePackSerializerBase( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer( typeof( object ), ( schema ?? PolymorphismSchema.Default ).ItemSchema );
		}

		/// <summary>
		///		Creates a new collection instance with specified initial capacity.
		/// </summary>
		/// <param name="initialCapacity">
		///		The initial capacy of creating collection.
		///		Note that this parameter may <c>0</c> for non-empty collection.
		/// </param>
		/// <returns>
		/// New collection instance. This value will not be <c>null</c>.
		/// </returns>
		/// <remarks>
		///		An author of <see cref="Unpacker" /> could implement unpacker for non-MessagePack format,
		///		so implementer of this interface should not rely on that <paramref name="initialCapacity" /> reflects actual items count.
		///		For example, JSON unpacker cannot supply collection items count efficiently.
		/// </remarks>
		/// <seealso cref="ICollectionInstanceFactory.CreateInstance"/>
		protected abstract TCollection CreateInstance( int initialCapacity );

		object ICollectionInstanceFactory.CreateInstance( int initialCapacity )
		{
			return this.CreateInstance( initialCapacity );
		}

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="TCollection"/> is not collection.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			MessageId = "0", Justification = "By design" )]
		protected internal sealed override void UnpackToCore( Unpacker unpacker, TCollection collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		internal void UnpackToCore( Unpacker unpacker, TCollection collection, int itemsCount )
		{
			for ( var i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				object item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = this._itemSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = this._itemSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				this.AddItem( collection, item );
			}
		}

		/// <summary>
		///		When implemented by derive class, 
		///		adds the deserialized item to the collection on <typeparamref name="TCollection"/> specific manner
		///		to implement <see cref="UnpackToCore(Unpacker,TCollection)"/>.
		/// </summary>
		/// <param name="collection">The collection to be added.</param>
		/// <param name="item">The item to be added.</param>
		/// <exception cref="NotSupportedException">
		///		This implementation always throws it.
		/// </exception>
		protected virtual void AddItem( TCollection collection, object item )
		{
			throw SerializationExceptions.NewUnpackToIsNotSupported( typeof( TCollection ), null );
		}
	}

#if UNITY
	internal abstract class UnityNonGenericEnumerableMessagePackSerializerBase : NonGenericMessagePackSerializer, ICollectionInstanceFactory
	{
		private readonly IMessagePackSingleObjectSerializer _itemSerializer;

		internal IMessagePackSingleObjectSerializer ItemSerializer { get { return this._itemSerializer; } }

		protected UnityNonGenericEnumerableMessagePackSerializerBase( SerializationContext ownerContext, Type targetType, PolymorphismSchema schema )
			: base( ownerContext, targetType )
		{
			this._itemSerializer = ownerContext.GetSerializer( typeof( object ), ( schema ?? PolymorphismSchema.Default ).ItemSchema );
		}

		protected abstract object CreateInstance( int initialCapacity );

		object ICollectionInstanceFactory.CreateInstance( int initialCapacity )
		{
			return this.CreateInstance( initialCapacity );
		}

		protected internal sealed override void UnpackToCore( Unpacker unpacker, object collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		internal void UnpackToCore( Unpacker unpacker, object collection, int itemsCount )
		{
			for ( var i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				object item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = this._itemSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = this._itemSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				this.AddItem( collection, item );
			}
		}

		protected virtual void AddItem( object collection, object item )
		{
			throw SerializationExceptions.NewUnpackToIsNotSupported( this.TargetType, null );
		}
	}
#endif // UNITY
}