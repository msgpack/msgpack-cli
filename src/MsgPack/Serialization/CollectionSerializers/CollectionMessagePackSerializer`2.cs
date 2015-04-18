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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="EnumerableMessagePackSerializerBase{TCollection, TItem}"/> 
	///		for collection types which implement <see cref="ICollection{T}"/>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <typeparam name="TItem">The type of the item of collection.</typeparam>
	public abstract class CollectionMessagePackSerializer<TCollection, TItem> : EnumerableMessagePackSerializerBase<TCollection, TItem>
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
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TCollection"/> is not serializable etc.
		/// </exception>
		protected internal sealed override void PackToCore( Packer packer, TCollection objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );

			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree )
			{
				itemSerializer.PackTo( packer, item );
			}
		}



		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="TCollection"/> is abstract type.
		/// </exception>
		/// <remarks>
		///		This method invokes <see cref="EnumerableMessagePackSerializerBase{TCollection,TItem}.CreateInstance(int)"/>, and then fill deserialized items to resultong collection.
		/// </remarks>
		protected internal sealed override TCollection UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			return this.InternalUnpackFromCore( unpacker );
		}

		internal virtual TCollection InternalUnpackFromCore( Unpacker unpacker )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
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
			collection.Add( item );
		}
	}
}