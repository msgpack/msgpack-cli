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
	///		Provides basic features for non-generic dictionary serializers.
	/// </summary>
	/// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
	/// <remarks>
	///		This class provides framework to implement variable collection serializer, and this type seals some virtual members to maximize future backward compatibility.
	///		If you cannot use this class, you can implement your own serializer which inherits <see cref="MessagePackSerializer{T}"/> and implements <see cref="ICollectionInstanceFactory"/>.
	/// </remarks>
	public abstract class NonGenericDictionaryMessagePackSerializer<TDictionary> : MessagePackSerializer<TDictionary>, ICollectionInstanceFactory
		where TDictionary : IDictionary
	{
		private readonly MessagePackSerializer _keySerializer;
		private readonly MessagePackSerializer _valueSerializer;

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericDictionaryMessagePackSerializer{TDictionary}"/> class.
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
		protected NonGenericDictionaryMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext )
		{
			var safeSchema = schema ?? PolymorphismSchema.Default;
			this._keySerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.KeySchema );
			this._valueSerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.ItemSchema );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericDictionaryMessagePackSerializer{TDictionary}"/> class.
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by base .ctor" )]
		protected NonGenericDictionaryMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, capabilities )
		{
			var safeSchema = schema ?? PolymorphismSchema.Default;
			this._keySerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.KeySchema );
			this._valueSerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.ItemSchema );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TDictionary"/> is not serializable etc.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, TDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( DictionaryEntry item in objectTree )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
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
		///		<typeparamref name="TDictionary"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal override async Task PackToAsyncCore( Packer packer, TDictionary objectTree, CancellationToken cancellationToken )
		{
			await packer.PackMapHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( DictionaryEntry item in objectTree )
			{
				await this._keySerializer.PackToAsync( packer, item.Key, cancellationToken ).ConfigureAwait( false );
				await this._valueSerializer.PackToAsync( packer, item.Value, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

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
		///		<typeparamref name="TDictionary"/> is abstract type.
		/// </exception>
		/// <remarks>
		///		This method invokes <see cref="CreateInstance(int)"/>, and then fill deserialized items to resultong collection.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override TDictionary UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromCore( unpacker );
		}

		internal virtual TDictionary InternalUnpackFromCore( Unpacker unpacker )
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
		///		<typeparamref name="TDictionary"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task<TDictionary> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromAsyncCore( unpacker, cancellationToken );
		}

		internal virtual Task<TDictionary> InternalUnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			return this.UnpackToAsyncCore( unpacker, collection, itemsCount, cancellationToken );
		}

#endif // FEATURE_TAP

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
		protected abstract TDictionary CreateInstance( int initialCapacity );

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
		///		<typeparamref name="TDictionary"/> is not collection.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
		protected internal override void UnpackToCore( Unpacker unpacker, TDictionary collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, TDictionary collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingKey( i, unpacker );
				}

				object key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = this._keySerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				object value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = this._valueSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = this._valueSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				collection.Add( key, value );
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
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
		///		<typeparamref name="TDictionary"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, TDictionary collection, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.UnpackToAsyncCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ), cancellationToken );
		}

		private async Task<TDictionary> UnpackToAsyncCore( Unpacker unpacker, TDictionary collection, int itemsCount, CancellationToken cancellationToken )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingKey( i, unpacker );
				}

				object key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = await this._keySerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = await this._keySerializer.UnpackFromAsync( subtreeUnpacker, cancellationToken ).ConfigureAwait( false );
					}
				}

				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				object value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = await this._valueSerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = await this._valueSerializer.UnpackFromAsync( subtreeUnpacker, cancellationToken ).ConfigureAwait( false );
					}
				}

				collection.Add( key, value );
			}

			return collection;
		}


#endif // FEATURE_TAP
	}

#if UNITY
#warning TODO: Remove if possible for maintenancibility.
	internal abstract class UnityNonGenericDictionaryMessagePackSerializer : NonGenericMessagePackSerializer, ICollectionInstanceFactory
	{
		private readonly MessagePackSerializer _keySerializer;
		private readonly MessagePackSerializer _valueSerializer;

		protected UnityNonGenericDictionaryMessagePackSerializer( SerializationContext ownerContext, Type targetType, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, targetType, capabilities )
		{
			var safeSchema = schema ?? PolymorphismSchema.Default;
			this._keySerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.KeySchema );
			this._valueSerializer = ownerContext.GetSerializer( typeof( object ), safeSchema.ItemSchema );
		}

		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			var asDictionary = objectTree as IDictionary;
			// ReSharper disable once PossibleNullReferenceException
			packer.PackMapHeader( asDictionary.Count );
			foreach ( DictionaryEntry item in asDictionary )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
		}

		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return this.InternalUnpackFromCore( unpacker );
		}

		internal virtual object InternalUnpackFromCore( Unpacker unpacker )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			this.UnpackToCore( unpacker, collection as IDictionary, itemsCount );
			return collection;
		}

		protected abstract object CreateInstance( int initialCapacity );

		object ICollectionInstanceFactory.CreateInstance( int initialCapacity )
		{
			return this.CreateInstance( initialCapacity );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, object collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			this.UnpackToCore( unpacker, collection as IDictionary, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, IDictionary collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				object key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = this._keySerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, ( key ?? String.Empty ).ToString(), unpacker );
				}

				object value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = this._valueSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = this._valueSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				collection.Add( key, value );
			}
		}
	}
#endif // UNITY
}