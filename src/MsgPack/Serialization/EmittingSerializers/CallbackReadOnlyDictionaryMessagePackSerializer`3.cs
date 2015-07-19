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

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		A helper <see cref="DictionaryMessagePackSerializer{TDictionary, TKey, TValue}"/> for <see cref="DynamicMethodSerializerBuilder{TObject}"/>.
	/// </summary>
	/// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
	/// <typeparam name="TKey">The type of the key of collection.</typeparam>
	/// <typeparam name="TValue">The type of the value of collection.</typeparam>
	internal class CallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue> :
		ReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>
		where TDictionary : IReadOnlyDictionary<TKey, TValue>
	{
		private readonly Func<SerializationContext, int, TDictionary> _createInstance;

		private readonly Action<SerializationContext, TDictionary, TKey, TValue> _addItem;

		/// <summary>
		///		Initializes a new instance of the <see cref="CallbackDictionaryMessagePackSerializer{TDictionary, TKey, TValue}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <param name="createInstance">The delegate to <c>CreateInstance</c> method body. This value must not be <c>null</c>.</param>
		/// <param name="addItem">The delegate to <c>AddItem</c> method body. This value can be <c>null</c>.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		public CallbackReadOnlyDictionaryMessagePackSerializer(
			SerializationContext ownerContext,
			PolymorphismSchema schema,
			Func<SerializationContext, int, TDictionary> createInstance,
			Action<SerializationContext, TDictionary, TKey, TValue> addItem
		)
			: base( ownerContext, schema )
		{
			this._createInstance = createInstance;
			this._addItem = addItem;
		}

		protected override TDictionary CreateInstance( int initialCapacity )
		{
			return this._createInstance( this.OwnerContext, initialCapacity );
		}

		protected override void AddItem( TDictionary dictionary, TKey key, TValue value )
		{
			if ( this._addItem != null )
			{
				this._addItem( this.OwnerContext, dictionary, key, value );
			}
			else
			{
				base.AddItem( dictionary, key, value );
			}
		}
	}
}