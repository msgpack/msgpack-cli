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
	internal sealed class AbstractNonGenericEnumerableMessagePackSerializer<TCollection> : NonGenericEnumerableMessagePackSerializerBase<TCollection>
		where TCollection : IEnumerable
	{
		private readonly ICollectionInstanceFactory _concreteCollectionInstanceFactory;
		private readonly IMessagePackSingleObjectSerializer _concreteSerializer;

		public AbstractNonGenericEnumerableMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema schema
		)
			: base( ownerContext, schema )
		{
			AbstractCollectionSerializerHelper.GetConcreteSerializer(
				ownerContext,
				schema,
				typeof( TCollection ),
				targetType,
				typeof( EnumerableMessagePackSerializerBase<,> ),
				out this._concreteCollectionInstanceFactory,
				out this._concreteSerializer
			);
		}

		protected internal override void PackToCore( Packer packer, TCollection objectTree )
		{
			this._concreteSerializer.PackTo( packer, objectTree );
		}

		protected internal override TCollection UnpackFromCore( Unpacker unpacker )
		{
			var polymorPhicSerializer = this._concreteSerializer as IPolymorphicDeserializer;
			if ( polymorPhicSerializer != null )
			{
				// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
				return ( TCollection )polymorPhicSerializer.PolymorphicUnpackFrom( unpacker );
			}
			else
			{
				return ( TCollection )this._concreteSerializer.UnpackFrom( unpacker );
			}
		}

		protected override TCollection CreateInstance( int initialCapacity )
		{
			// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
			return ( TCollection )this._concreteCollectionInstanceFactory.CreateInstance( initialCapacity );
		}
	}
}