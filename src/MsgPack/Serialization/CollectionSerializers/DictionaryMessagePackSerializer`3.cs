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
#if UNITY
using System.Collections;
#endif // UNITY
using System.Collections.Generic;
#if UNITY
using System.Reflection;
#endif // UNITY

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides basic features for generic <see cref="IDictionary{TKey,TValue}"/> serializers.
	/// </summary>
	/// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
	/// <typeparam name="TKey">The type of the key of dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the value of dictionary.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "By design" )]
	public abstract class DictionaryMessagePackSerializer<TDictionary, TKey, TValue> : DictionaryMessagePackSerializerBase<TDictionary, TKey, TValue>
		where TDictionary : IDictionary<TKey, TValue>
#if UNITY
		, IEnumerable<KeyValuePair<TKey, TValue>> // This is obvious from IDictionary<TKey, TValue>, but Unity compiler cannot recognize this.
#endif // UNITY
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="DictionaryMessagePackSerializer{TDictionary, TKey, TValue}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected DictionaryMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Returns count of the dictionary.
		/// </summary>
		/// <param name="dictionary">A collection. This value will not be <c>null</c>.</param>
		/// <returns>The count of the <paramref name="dictionary"/>.</returns>
		protected override int GetCount( TDictionary dictionary )
		{
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			return dictionary.Count;
#else
			// .constraind call for TDictionary.get_Count/TDictionary.GetEnumerator() causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			return ( dictionary as IDictionary<TKey, TValue> ).Count;
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
		}

		/// <summary>
		///		Adds the deserialized item to the collection on <typeparamref name="TDictionary"/> specific manner
		///		to implement <see cref="DictionaryMessagePackSerializerBase{TDictionary,TKey,TValue}.UnpackToCore(MsgPack.Unpacker,TDictionary)"/>.
		/// </summary>
		/// <param name="dictionary">The dictionary to be added.</param>
		/// <param name="key">The key to be added.</param>
		/// <param name="value">The value to be added.</param>
		/// <exception cref="NotSupportedException">
		///		This implementation always throws it.
		/// </exception>
		protected override void AddItem( TDictionary dictionary, TKey key, TValue value )
		{
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			dictionary.Add( key, value );
#else
			// .constraind call for TDictionary.Add causes AOT error.
			// So use cast and invoke as normal call (it might cause boxing, but most collection should be reference type).
			( dictionary as IDictionary<TKey, TValue> ).Add( key, value );
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
		}
	}

#if UNITY
	internal abstract class UnityDictionaryMessagePackSerializer : NonGenericMessagePackSerializer,
		ICollectionInstanceFactory
	{
		private readonly IMessagePackSingleObjectSerializer _keySerializer;
		private readonly IMessagePackSingleObjectSerializer _valueSerializer;
		private readonly MethodInfo _add;
		private readonly MethodInfo _getCount;
		private readonly MethodInfo _getKey;
		private readonly MethodInfo _getValue;

		protected UnityDictionaryMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			Type keyType,
			Type valueType,
			CollectionTraits traits,
			PolymorphismSchema schema
		)
			: base( ownerContext, targetType )
		{
			var safeSchema = schema ?? PolymorphismSchema.Default;
			this._keySerializer = ownerContext.GetSerializer( keyType, safeSchema.KeySchema );
			this._valueSerializer = ownerContext.GetSerializer( valueType, safeSchema.ItemSchema );
			this._add = traits.AddMethod;
			this._getCount = traits.CountPropertyGetter;
			this._getKey = traits.ElementType.GetProperty( "Key" ).GetGetMethod();
			this._getValue = traits.ElementType.GetProperty( "Value" ).GetGetMethod();
		}

		protected internal override sealed void PackToCore( Packer packer, object objectTree )
		{
			packer.PackMapHeader( ( int )this._getCount.InvokePreservingExceptionType( objectTree ) );
			// ReSharper disable once PossibleNullReferenceException
			foreach ( var item in objectTree as IEnumerable )
			{
				this._keySerializer.PackTo( packer, this._getKey.InvokePreservingExceptionType( item ) );
				this._valueSerializer.PackTo( packer, this._getValue.InvokePreservingExceptionType( item ) );
			}
		}

		protected internal override sealed object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
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

		protected abstract object CreateInstance( int initialCapacity );

		object ICollectionInstanceFactory.CreateInstance( int initialCapacity )
		{
			return this.CreateInstance( initialCapacity );
		}

		protected internal override sealed void UnpackToCore( Unpacker unpacker, object collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, object collection, int itemsCount )
		{
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
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
					throw SerializationExceptions.NewMissingItem( i );
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

				this._add.InvokePreservingExceptionType( collection, key, value );
			}
		}
	}
#endif // UNITY
}