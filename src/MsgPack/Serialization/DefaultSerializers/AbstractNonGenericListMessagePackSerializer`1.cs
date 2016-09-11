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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance", Justification = "User may not use this hierarchy." )]
	internal sealed class AbstractNonGenericListMessagePackSerializer<TCollection> : NonGenericListMessagePackSerializer<TCollection>
		where TCollection : IList
#else
#warning TODO: Use generic collection if possible for maintenancibility.
	internal sealed class AbstractNonGenericListMessagePackSerializer : UnityNonGenericListMessagePackSerializer
#endif // !UNITY
	{
		private readonly ICollectionInstanceFactory _concreteCollectionInstanceFactory;
		private readonly IPolymorphicDeserializer _polymorphicDeserializer;

		public AbstractNonGenericListMessagePackSerializer(
			SerializationContext ownerContext,
#if !UNITY
			Type targetType,
#else
			Type abstractType,
			Type concreteType,
#endif // !UNITY
			PolymorphismSchema schema
		)
#if !UNITY
			: base( ownerContext, schema, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo )
#else
			: base( ownerContext, abstractType, schema, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo )
#endif // !UNITY
		{
			MessagePackSerializer serializer;
			AbstractCollectionSerializerHelper.GetConcreteSerializer(
				ownerContext,
				schema,
#if !UNITY
				typeof( TCollection ),
				targetType,
#else
				abstractType,
				concreteType,
#endif // !UNITY
				typeof( EnumerableMessagePackSerializerBase<,> ),
				out this._concreteCollectionInstanceFactory,
				out serializer
			);
			this._polymorphicDeserializer = serializer as IPolymorphicDeserializer;
		}

#if !UNITY
		internal override TCollection InternalUnpackFromCore( Unpacker unpacker )
#else
		internal override object InternalUnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( this._polymorphicDeserializer != null )
			{
				// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
				return 
#if !UNITY
					( TCollection )
#endif // !UNITY
					this._polymorphicDeserializer.PolymorphicUnpackFrom( unpacker );
			}
			else
			{
				return base.InternalUnpackFromCore( unpacker );
			}
		}

#if FEATURE_TAP

		internal override Task<TCollection> InternalUnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( this._polymorphicDeserializer != null )
			{
				return
					this._polymorphicDeserializer.PolymorphicUnpackFromAsync( unpacker, cancellationToken )
						.ContinueWith(
							t => ( TCollection )t.Result,
							cancellationToken,
							TaskContinuationOptions.ExecuteSynchronously,
							TaskScheduler.Current
						);
			}
			else
			{
				return base.InternalUnpackFromAsyncCore( unpacker, cancellationToken );
			}
		}

#endif // FEATURE_TAP

#if !UNITY
		protected override TCollection CreateInstance( int initialCapacity )
#else
		protected override object CreateInstance( int initialCapacity )
#endif // !UNITY
		{
			// This boxing is OK because TCollection should be reference type because TCollection is abstract class or interface.
			return 
#if !UNITY
				( TCollection )
#endif // !UNITY
				this._concreteCollectionInstanceFactory.CreateInstance( initialCapacity );
		}
	}
}