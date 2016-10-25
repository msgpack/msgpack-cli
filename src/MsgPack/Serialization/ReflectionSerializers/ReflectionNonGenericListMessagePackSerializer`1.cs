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
using System.Collections;
#endif // !UNITY
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
	[Preserve( AllMembers = true )]
#if !UNITY
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance", Justification = "By design" )]
	internal sealed class ReflectionNonGenericListMessagePackSerializer<TList> : NonGenericListMessagePackSerializer<TList>
		where TList : IList
#else
	internal sealed class ReflectionNonGenericListMessagePackSerializer : UnityNonGenericListMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly Func<int, TList> _factory;
#else
		private readonly Func<int, object> _factory;
#endif // !UNITY

		private readonly bool _isPackable;
		private readonly bool _isUnpackable;
#if FEATURE_TAP
		private readonly bool _isAsyncPackable;
		private readonly bool _isAsyncUnpackable;
#endif // FEATURE_TAP

#if !UNITY
		public ReflectionNonGenericListMessagePackSerializer(
			SerializationContext ownerContext,
			Type targetType,
			CollectionTraits collectionTraits,
			PolymorphismSchema itemsSchema,
			SerializationTarget targetInfo
		)
			: base( ownerContext, itemsSchema, targetInfo.GetCapabilitiesForCollection( collectionTraits ) )
		{
			if ( targetInfo.CanDeserialize )
			{
				this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory<TList, object>( targetInfo.DeserializationConstructor );
			}
			else
			{
				this._factory = _ => { throw SerializationExceptions.NewCreateInstanceIsNotSupported( targetType ); };
			}

			this._isPackable = typeof( IPackable ).IsAssignableFrom( targetType ?? typeof( TList ) );
			this._isUnpackable = typeof( IUnpackable ).IsAssignableFrom( targetType ?? typeof( TList ) );
#if FEATURE_TAP
			this._isAsyncPackable = typeof( IAsyncPackable ).IsAssignableFrom( targetType ?? typeof( TList ) );
			this._isAsyncUnpackable = typeof( IAsyncUnpackable ).IsAssignableFrom( targetType ?? typeof( TList ) );
#endif // FEATURE_TAP
		}
#else
		public ReflectionNonGenericListMessagePackSerializer(
			SerializationContext ownerContext,
			Type abstractType,
			Type concreteType,
			CollectionTraits concreteTypeCollectionTraits,
			PolymorphismSchema itemsSchema,
			SerializationTarget targetInfo
		)
			: base( ownerContext, abstractType, itemsSchema, targetInfo.GetCapabilitiesForCollection( concreteTypeCollectionTraits ) )
		{
			if ( targetInfo.CanDeserialize )
			{
				this._factory = ReflectionSerializerHelper.CreateCollectionInstanceFactory( abstractType, concreteType, typeof( object ), targetInfo.DeserializationConstructor );
			}
			else
			{
				this._factory = _ => { throw SerializationExceptions.NewCreateInstanceIsNotSupported( concreteType ); };
			}

			this._isPackable = typeof( IPackable ).IsAssignableFrom( concreteType ?? abstractType );
			this._isUnpackable = typeof( IUnpackable ).IsAssignableFrom( concreteType ?? abstractType );
		}
#endif // !UNITY

#if !UNITY
		protected internal override void PackToCore( Packer packer, TList objectTree )
#else
		protected internal override void PackToCore( Packer packer, object objectTree )
#endif // !UNITY
		{
			if ( this._isPackable )
			{
				( ( IPackable )objectTree ).PackToMessage( packer, null );
				return;
			}

			base.PackToCore( packer, objectTree );
		}

#if !UNITY
		protected internal override TList UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif
		{
			if ( this._isUnpackable )
			{
				var result = this.CreateInstance( 0 );
				( ( IUnpackable )result ).UnpackFromMessage( unpacker );
				return result;
			}

			return base.UnpackFromCore( unpacker );
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, TList objectTree, CancellationToken cancellationToken )
		{
			if ( this._isAsyncPackable )
			{
				return ( ( IAsyncPackable )objectTree ).PackToMessageAsync( packer, null, cancellationToken );
			}

			return base.PackToAsyncCore( packer, objectTree, cancellationToken );
		}

		protected internal override Task<TList> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( this._isAsyncUnpackable )
			{
				return this.UnpackFromMessageAsync( unpacker, cancellationToken );
			}

			return base.UnpackFromAsyncCore( unpacker, cancellationToken );
		}

		private async Task<TList> UnpackFromMessageAsync( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var result = this.CreateInstance( 0 );
			await ( ( IAsyncUnpackable )result ).UnpackFromMessageAsync( unpacker, cancellationToken ).ConfigureAwait( false );
			return result;
		}

#endif // FEATURE_TAP

#if !UNITY
		protected override TList CreateInstance( int initialCapacity )
		{
			return this._factory( initialCapacity );
		}
#else
		protected override object CreateInstance( int initialCapacity )
		{
			return this._factory( initialCapacity );
		}
#endif // !UNITY
	}
}