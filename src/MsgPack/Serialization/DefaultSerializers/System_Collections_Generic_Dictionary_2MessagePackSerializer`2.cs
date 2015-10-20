#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
#if UNITY
using System.Collections;
#endif // UNITY
using System.Collections.Generic;
#if UNITY
using System.Reflection;
#endif // UNITY

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	/// <summary>
	///		Provides default implementation for <see cref="Dictionary{TKey,TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of keys of the <see cref="Dictionary{TKey,TValue}"/>.</typeparam>
	/// <typeparam name="TValue">The type of values of the <see cref="Dictionary{TKey,TValue}"/>.</typeparam>
	// ReSharper disable once InconsistentNaming
	internal class System_Collections_Generic_Dictionary_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<Dictionary<TKey, TValue>>, ICollectionInstanceFactory
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_Dictionary_2MessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>( keysSchema );
			this._valueSerializer = ownerContext.GetSerializer<TValue>( valuesSchema );
		}

		protected internal override void PackToCore( Packer packer, Dictionary<TKey, TValue> objectTree )
		{
			PackerUnpackerExtensions.PackDictionaryCore( packer, objectTree, this._keySerializer, this._valueSerializer );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override Dictionary<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new Dictionary<TKey, TValue>( count );
			this.UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Dictionary<TKey, TValue> collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, Dictionary<TKey, TValue> collection, int count )
		{
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				TKey key;
				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFromCore( subTreeUnpacker );
					}
				}
				else
				{
					key = this._keySerializer.UnpackFromCore( unpacker );
				}

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						collection.Add( key, this._valueSerializer.UnpackFromCore( subTreeUnpacker ) );
					}
				}
				else
				{
					collection.Add( key, this._valueSerializer.UnpackFromCore( unpacker ) );
				}
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new Dictionary<TKey, TValue>( initialCapacity );
		}
	}
#else
	// ReSharper disable once InconsistentNaming
	internal class System_Collections_Generic_Dictionary_2MessagePackSerializer : NonGenericMessagePackSerializer, ICollectionInstanceFactory
	{
		private readonly IMessagePackSingleObjectSerializer  _keySerializer;
		private readonly IMessagePackSingleObjectSerializer _valueSerializer;
		private readonly Type _keyType;
		private readonly ConstructorInfo _constructor;
		private readonly MethodInfo _add;

		public System_Collections_Generic_Dictionary_2MessagePackSerializer( SerializationContext ownerContext, Type targetType, CollectionTraits traits, Type keyType, Type valueType, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
			: base( ownerContext, targetType )
		{
			this._keySerializer = ownerContext.GetSerializer( keyType, keysSchema );
			this._valueSerializer = ownerContext.GetSerializer( valueType, valuesSchema );
			this._keyType = keyType;
			this._constructor =
				targetType.GetConstructor( new[] { typeof( int ), typeof( IEqualityComparer<> ).MakeGenericType( keyType ) } );
			this._add = traits.AddMethod;
		}

		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			var asDictionary = objectTree as IDictionary;
			if ( asDictionary == null )
			{
				packer.PackNull();
				return;
			}

			packer.PackMapHeader( asDictionary.Count );
			foreach ( DictionaryEntry entry in asDictionary )
			{
				this._keySerializer.PackTo( packer, entry.Key );
				this._valueSerializer.PackTo( packer, entry.Value );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( count );
			this.UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, object collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, object collection, int count )
		{
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				object key;
				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFrom( subTreeUnpacker );
					}
				}
				else
				{
					key = this._keySerializer.UnpackFrom( unpacker );
				}

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						this._add.InvokePreservingExceptionType( collection, key, this._valueSerializer.UnpackFrom( subTreeUnpacker ) );
					}
				}
				else
				{
					this._add.InvokePreservingExceptionType( collection, key, this._valueSerializer.UnpackFrom( unpacker ) );
				}
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return AotHelper.CreateSystemCollectionsGenericDictionary( this._constructor, this._keyType, initialCapacity );
		}
	}
#endif // !UNITY
}