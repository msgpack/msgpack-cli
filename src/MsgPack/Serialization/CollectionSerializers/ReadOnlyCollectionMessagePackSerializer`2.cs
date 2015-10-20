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

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="CollectionMessagePackSerializerBase{TCollection, TItem}"/> 
	///		for collection types which implement <see cref="IReadOnlyCollection{T}"/>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <typeparam name="TItem">The type of the item of collection.</typeparam>
	public abstract class ReadOnlyCollectionMessagePackSerializer<TCollection, TItem> : CollectionMessagePackSerializerBase<TCollection, TItem>
		where TCollection : IReadOnlyCollection<TItem>
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
		protected ReadOnlyCollectionMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
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
	}
}