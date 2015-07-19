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
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class AbstractReadOnlyCollectionMessagePackSerializer<TCollection, TItem> : ReadOnlyCollectionMessagePackSerializer<TCollection, TItem>
		where TCollection : IReadOnlyCollection<TItem>
	{
		private readonly ICollectionInstanceFactory _concreteCollectionInstanceFactory;
		private readonly IPolymorphicDeserializer _polymorphicDeserializer;
		private readonly IMessagePackSingleObjectSerializer _concreteDeserializer;

		public AbstractReadOnlyCollectionMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema schema
		)
			: base( ownerContext, schema )
		{
			IMessagePackSingleObjectSerializer serializer;
			AbstractCollectionSerializerHelper.GetConcreteSerializer(
				ownerContext,
				schema,
				typeof( TCollection ),
				targetType,
				typeof( EnumerableMessagePackSerializerBase<,> ),
				out this._concreteCollectionInstanceFactory,
				out serializer
			);
			this._polymorphicDeserializer = serializer as IPolymorphicDeserializer;
			this._concreteDeserializer = serializer;
		}

		internal override TCollection InternalUnpackFromCore( Unpacker unpacker )
		{
			if ( this._polymorphicDeserializer != null )
			{
				// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
				return
					( TCollection )this._polymorphicDeserializer.PolymorphicUnpackFrom( unpacker );
			}
			else if ( this._concreteDeserializer != null )
			{
				return ( TCollection ) this._concreteDeserializer.UnpackFrom( unpacker );
			}
			else
			{
				return base.InternalUnpackFromCore( unpacker );
			}
		}


		protected override TCollection CreateInstance( int initialCapacity )
		{
			// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
			return
				( TCollection )this._concreteCollectionInstanceFactory.CreateInstance( initialCapacity );
		}
	}
}