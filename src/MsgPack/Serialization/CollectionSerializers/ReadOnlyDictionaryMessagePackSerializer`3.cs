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
	///		Provides basic features for generic <see cref="IReadOnlyDictionary{TKey,TValue}"/> serializers.
	/// </summary>
	/// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
	/// <typeparam name="TKey">The type of the key of dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the value of dictionary.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "By design" )]
	public abstract class ReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue> : DictionaryMessagePackSerializerBase<TDictionary, TKey, TValue>
		where TDictionary : IReadOnlyDictionary<TKey, TValue>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyDictionaryMessagePackSerializer{TDictionary, TKey, TValue}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected ReadOnlyDictionaryMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
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
	}
}