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
using System.Collections.Generic;
#if UNITY
using System.Collections;
using System.Reflection;
#endif // UNITY

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="CollectionMessagePackSerializerBase{TCollection, TItem}"/> 
	///		for collection types which implement <see cref="ICollection{T}"/>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <typeparam name="TItem">The type of the item of collection.</typeparam>
	public abstract class CollectionMessagePackSerializer<TCollection, TItem> : CollectionMessagePackSerializerBase<TCollection, TItem>
		where TCollection : ICollection<TItem>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="CollectionMessagePackSerializer{TCollection, TItem}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected CollectionMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Returns count of the collection.
		/// </summary>
		/// <param name="collection">A collection. This value will not be <c>null</c>.</param>
		/// <returns>The count of the <paramref name="collection"/>.</returns>
		protected override int GetCount( TCollection collection )
		{
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			return collection.Count;
#else
			// .constraind call for TCollection.get_Count/TCollection.GetEnumerator() causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			return ( collection as ICollection<TItem> ).Count;
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
		}

		/// <summary>
		///		Adds the deserialized item to the collection on <typeparamref name="TCollection"/> specific manner
		///		to implement <see cref="MessagePackSerializer{TCollection}.UnpackToCore(Unpacker,TCollection)"/>.
		/// </summary>
		/// <param name="collection">The collection to be added.</param>
		/// <param name="item">The item to be added.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override void AddItem( TCollection collection, TItem item )
		{
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			collection.Add( item );
#else
			// .constraind call for TCollection.Add causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			( collection as ICollection<TItem> ).Add( item );
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
		}
	}

#if UNITY
	internal abstract class UnityCollectionMessagePackSerializer : UnityEnumerableMessagePackSerializerBase
	{
		private readonly MethodInfo _getCount;
		private readonly MethodInfo _add;

		protected UnityCollectionMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			CollectionTraits traits,
			PolymorphismSchema schema 
		)
			: base( ownerContext, targetType, traits.ElementType, schema )
		{
			this._getCount = traits.CountPropertyGetter;
			this._add = traits.AddMethod;
		}

		protected internal sealed override void PackToCore( Packer packer, object objectTree )
		{
			packer.PackArrayHeader( ( int )this._getCount.InvokePreservingExceptionType( objectTree ) );
			var itemSerializer = this.ItemSerializer;

			// ReSharper disable once PossibleNullReferenceException
			foreach ( var item in objectTree as IEnumerable )
			{
				itemSerializer.PackTo( packer, item );
			}
		}

		protected internal sealed override object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			return this.InternalUnpackFromCore( unpacker );
		}

		internal virtual object InternalUnpackFromCore( Unpacker unpacker )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

		protected override void AddItem( object collection, object item )
		{
			this._add.InvokePreservingExceptionType( collection, item );
		}
	}
#endif // UNITY
}