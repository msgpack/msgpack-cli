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
using System.Collections;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
	// Note: ReflectionNonGeenricEnumerableMessagePackSerializer and ReflectionNonGeenricCollectionMessagePackSerializer cannot be realized
	// because they cannot implement UnpackFromCore...

	internal sealed class ReflectionNonGenericListMessagePackSerializer<TList> : NonGenericListMessagePackSerializer<TList>
		where TList : IList
	{
		private readonly Func<int, TList> _factory;

		public ReflectionNonGenericListMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema itemsSchema )
			: base( ownerContext, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TList>( targetType );
		}

		protected override TList CreateInstance( int initialCapacity )
		{
			return this._factory( initialCapacity );
		}
	}
}