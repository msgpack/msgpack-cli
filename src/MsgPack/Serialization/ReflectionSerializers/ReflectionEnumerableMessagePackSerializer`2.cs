#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
using System.Collections.Generic;
#endif // !UNITY
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
#if !UNITY
	internal sealed class ReflectionEnumerableMessagePackSerializer<TCollection, TItem> : EnumerableMessagePackSerializer<TCollection, TItem>
		where TCollection : IEnumerable<TItem>
#else
	internal sealed class ReflectionEnumerableMessagePackSerializer : UnityEnumerableMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly Func<int, TCollection> _factory;
		private readonly Action<TCollection, TItem> _addItem;
#else
		private readonly Func<int, object> _factory;
		private readonly Action<object, object> _addItem;
#endif // !UNITY

		private readonly bool _isPackable;
		private readonly bool _isUnpackable;
#if FEATURE_TAP
		private readonly bool _isAsyncPackable;
		private readonly bool _isAsyncUnpackable;
#endif // FEATURE_TAP

#if !UNITY
		public ReflectionEnumerableMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			CollectionTraits collectionTraits,
			PolymorphismSchema itemsSchema 
		)
			: base( ownerContext, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TCollection, TItem>( targetType );
			this._addItem = ReflectionSerializerHelper.GetAddItem<TCollection, TItem>( targetType, collectionTraits );
			this._isPackable = typeof( IPackable ).IsAssignableFrom( targetType ?? typeof( TCollection ) );
			this._isUnpackable = typeof( IUnpackable ).IsAssignableFrom( targetType ?? typeof( TCollection ) );
#if FEATURE_TAP
			this._isAsyncPackable = typeof( IAsyncPackable ).IsAssignableFrom( targetType ?? typeof( TCollection ) );
			this._isAsyncUnpackable = typeof( IAsyncUnpackable ).IsAssignableFrom( targetType ?? typeof( TCollection ) );
#endif // FEATURE_TAP
		}
#else
		public ReflectionEnumerableMessagePackSerializer(
			SerializationContext ownerContext,
			Type abstractType,
			Type concreteType,
			CollectionTraits traits,
			PolymorphismSchema itemsSchema
		)
			: base( ownerContext, abstractType, traits, itemsSchema )
		{
			this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory( abstractType, concreteType, traits.ElementType );
			this._addItem = ReflectionSerializerHelper.GetAddItem( concreteType, traits );
			this._isPackable = typeof( IPackable ).IsAssignableFrom( concreteType ?? abstractType );
			this._isUnpackable = typeof( IUnpackable ).IsAssignableFrom( concreteType ?? abstractType );
		}
#endif // !UNITY

#if !UNITY
		protected internal override void PackToCore( Packer packer, TCollection objectTree )
#else
		protected internal override void PackToCore( Packer packer, object objectTree )
#endif
		{
			if ( this._isPackable )
			{
				( ( IPackable )objectTree ).PackToMessage( packer, null );
				return;
			}

			base.PackToCore( packer, objectTree );
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, TCollection objectTree, CancellationToken cancellationToken )
		{
			if ( this._isAsyncPackable )
			{
				return ( ( IAsyncPackable )objectTree ).PackToMessageAsync( packer, null, cancellationToken );
			}

			return base.PackToAsyncCore( packer, objectTree, cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
#if !UNITY
		protected internal override TCollection UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( this._isUnpackable )
			{
				var result = this.CreateInstance( 0 );
				( ( IUnpackable )result ).UnpackFromMessage( unpacker );
				return result;
			}

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			this.UnpackToCore( unpacker, collection, itemsCount );
			return collection;
		}

#if FEATURE_TAP

		protected internal override async Task<TCollection> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( this._isAsyncUnpackable )
			{
				var result = this.CreateInstance( 0 );
				await ( ( IAsyncUnpackable )result ).UnpackFromMessageAsync( unpacker, cancellationToken ).ConfigureAwait( false );
				return result;
			} 
			
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			var collection = this.CreateInstance( itemsCount );
			await this.UnpackToAsyncCore( unpacker, collection, itemsCount, cancellationToken ).ConfigureAwait( false );
			return collection;
		}

#endif // FEATURE_TAP

#if !UNITY
		protected override TCollection CreateInstance( int initialCapacity )
#else
		protected override object CreateInstance( int initialCapacity )
#endif // !UNITY
		{
			return this._factory( initialCapacity );
		}

#if !UNITY
		protected override void AddItem( TCollection collection, TItem item )
#else
		protected override void AddItem( object collection, object item )
#endif // !UNITY
		{
			this._addItem( collection, item );
		}
	}
}