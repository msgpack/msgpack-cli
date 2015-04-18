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

namespace MsgPack.Serialization.ReflectionSerializers
{
	// Note: ReflectionEnumerableMessagePackSerializer cannot be realized because it cannot implement UnpackFromCore...

	internal sealed class ReflectionCollectionMessagePackSerializer<TCollection, TItem> : CollectionMessagePackSerializer<TCollection, TItem>
		where TCollection : ICollection<TItem>
	{
		private readonly Func<int, TCollection> _factory;

		public ReflectionCollectionMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema itemsSchema )
			: base( ownerContext, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TCollection>( targetType );
		}

		protected override TCollection CreateInstance( int initialCapacity )
		{
			return this._factory( initialCapacity );
		}
	}
}