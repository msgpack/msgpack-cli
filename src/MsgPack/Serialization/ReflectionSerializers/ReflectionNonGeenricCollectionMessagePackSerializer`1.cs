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
#if !UNITY
using System.Collections;
#endif // !UNITY

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
#if !UNITY
	internal sealed class ReflectionNonGenericCollectionMessagePackSerializer<TCollection> : NonGenericCollectionMessagePackSerializer<TCollection>
		where TCollection : ICollection
#else
	internal sealed class ReflectionNonGenericCollectionMessagePackSerializer : UnityNonGenericCollectionMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly Func<int, TCollection> _factory;
		private readonly Action<TCollection, object> _addItem;
#else
		private readonly Func<int, object> _factory;
		private readonly Action<object, object> _addItem;
#endif // !UNITY

#if !UNITY
		public ReflectionNonGenericCollectionMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			PolymorphismSchema itemsSchema
		)
			: base( ownerContext, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TCollection, object>( targetType );
			this._addItem = ReflectionSerializerHelper.GetAddItem<TCollection, object>( targetType );
		}
#else
		public ReflectionNonGenericCollectionMessagePackSerializer(
			SerializationContext ownerContext,
			Type abstractType,
			Type concreteType,
			PolymorphismSchema itemsSchema 
		)
			: base( ownerContext, abstractType, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory( abstractType, concreteType, typeof( object ) );
			this._addItem = ReflectionSerializerHelper.GetAddItem( concreteType );
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
#if !UNITY
		protected internal override TCollection UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

#if !UNITY
		protected override TCollection CreateInstance( int initialCapacity )
#else
		protected override object CreateInstance( int initialCapacity )
#endif // !UNITY
		{
			return this._factory( initialCapacity );
		}

#if !UNITY
		protected override void AddItem( TCollection collection, object item )
#else
		protected override void AddItem( object collection, object item )
#endif // !UNITY
		{
			this._addItem( collection, item );
		}
	}
}