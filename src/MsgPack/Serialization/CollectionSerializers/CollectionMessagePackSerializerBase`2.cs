#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2018 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="EnumerableMessagePackSerializerBase{TCollection, TItem}"/> 
	///		for collection types which implement <see cref="ICollection{T}"/> or <c>IReadOnlyCollection{T}</c>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <typeparam name="TItem">The type of the item of collection.</typeparam>
	public abstract class CollectionMessagePackSerializerBase<TCollection, TItem> : EnumerableMessagePackSerializerBase<TCollection, TItem>
		where TCollection : IEnumerable<TItem>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="CollectionMessagePackSerializerBase{TCollection, TItem}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected CollectionMessagePackSerializerBase( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="CollectionMessagePackSerializerBase{TCollection, TItem}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <param name="capabilities">A serializer calability flags represents capabilities of this instance.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected CollectionMessagePackSerializerBase( SerializationContext ownerContext, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, schema, capabilities ) { }

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TCollection"/> is not serializable etc.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, TCollection objectTree )
		{
			packer.PackArrayHeader( this.GetCount( objectTree ) );
#if ( !UNITY && !AOT ) || AOT_CHECK
			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree )
			{
				itemSerializer.PackTo( packer, item );
			}
#else
			// .constraind call for TCollection.get_Count/TCollection.GetEnumerator() causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree as IEnumerable<TItem> )
			{
				itemSerializer.PackTo( packer, item );
			}
#endif // ( !UNITY ) || AOT_CHECK
		}

#if FEATURE_TAP

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/> asynchronously.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="TCollection"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal override async Task PackToAsyncCore( Packer packer, TCollection objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( this.GetCount( objectTree ), cancellationToken ).ConfigureAwait( false );
#if ( !UNITY && !AOT ) || AOT_CHECK
			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree )
			{
				await itemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
#else
			// .constraind call for TCollection.get_Count/TCollection.GetEnumerator() causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree as IEnumerable<TItem> )
			{
				await itemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
#endif // ( !UNITY ) || AOT_CHECK
		}

#endif // FEATURE_TAP

		/// <summary>
		///		When overridden in derived class, returns count of the collection.
		/// </summary>
		/// <param name="collection">A collection. This value will not be <c>null</c>.</param>
		/// <returns>The count of the <paramref name="collection"/>.</returns>
		protected abstract int GetCount( TCollection collection );

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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override TCollection UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
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

#if FEATURE_TAP

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="TCollection"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task<TCollection> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromAsyncCore( unpacker, cancellationToken );
		}

		internal virtual Task<TCollection> InternalUnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			return this.InternalUnpackToAsyncCore( unpacker, collection, itemsCount, cancellationToken );
		}

#endif // FEATURE_TAP
	}
}
