#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="NonGenericCollectionMessagePackSerializer{TCollection}"/> 
	///		for collection types which implement <see cref="IList"/>.
	/// </summary>
	/// <typeparam name="TList">The type of the collection.</typeparam>
	public abstract class NonGenericListMessagePackSerializer<TList> : NonGenericCollectionMessagePackSerializer<TList>
		where TList : IList
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericListMessagePackSerializer{TList}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected NonGenericListMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericListMessagePackSerializer{TList}"/> class.
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
		protected NonGenericListMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, schema, capabilities ) { }

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
		///		<typeparamref name="TList"/> is abstract type.
		/// </exception>
		/// <remarks>
		///		This method invokes <see cref="NonGenericEnumerableMessagePackSerializerBase{TList}.CreateInstance(int)"/>, and then fill deserialized items to resultong collection.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override TList UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromCore( unpacker );
		}

		internal virtual TList InternalUnpackFromCore( Unpacker unpacker )
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
		///		<typeparamref name="TList"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task<TList> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromAsyncCore( unpacker, cancellationToken );
		}

		internal virtual Task<TList> InternalUnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			return this.InternalUnpackToAsyncCore( unpacker, collection, itemsCount, cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Adds the deserialized item to the collection on <typeparamref name="TList"/> specific manner
		///		to implement <see cref="MessagePackSerializer{TList}.UnpackToCore(Unpacker,TList)"/>.
		/// </summary>
		/// <param name="collection">The collection to be added.</param>
		/// <param name="item">The item to be added.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected override void AddItem( TList collection, object item )
		{
			collection.Add( item );
		}
	}

#if UNITY
#warning TODO: Remove if possible for maintenancibility.
	internal abstract class UnityNonGenericListMessagePackSerializer : UnityNonGenericCollectionMessagePackSerializer
	{
		protected UnityNonGenericListMessagePackSerializer( SerializationContext ownerContext, Type targetType, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, targetType, schema, capabilities ) { }

		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
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
			( collection as IList ).Add( item );
		}
	}
#endif // UNITY
}