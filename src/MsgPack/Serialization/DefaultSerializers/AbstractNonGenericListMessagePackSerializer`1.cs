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
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class AbstractNonGenericListMessagePackSerializer<TCollection> : NonGenericListMessagePackSerializer<TCollection>
		where TCollection : IList
	{
		private readonly ICollectionInstanceFactory _concreteCollectionInstanceFactory;
		private readonly IPolymorphicDeserializer _polymorPhicSerializer;

		public AbstractNonGenericListMessagePackSerializer(
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
			this._polymorPhicSerializer = serializer as IPolymorphicDeserializer;
		}

		internal override TCollection InternalUnpackFromCore( Unpacker unpacker )
		{
			if ( this._polymorPhicSerializer != null )
			{
				// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
				return ( TCollection )this._polymorPhicSerializer.PolymorphicUnpackFrom( unpacker );
			}
			else
			{
				return base.InternalUnpackFromCore( unpacker );
			}
		}

		protected override TCollection CreateInstance( int initialCapacity )
		{
			// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
			return ( TCollection )this._concreteCollectionInstanceFactory.CreateInstance( initialCapacity );
		}
	}
}