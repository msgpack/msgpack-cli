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
using System.Reflection;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal sealed class ReflectionDictionaryMessagePackSerializer<TDictionary, TKey, TValue> : DictionaryMessagePackSerializer<TDictionary, TKey, TValue>
		where TDictionary : IDictionary<TKey, TValue>
	{
		private readonly Func<int, TDictionary> _factory;

		public ReflectionDictionaryMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema itemsSchema )
			: base( ownerContext, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TDictionary>( targetType );
		}

		protected override TDictionary CreateInstance( int initialCapacity )
		{
			return this._factory( initialCapacity );
		}
	}
}