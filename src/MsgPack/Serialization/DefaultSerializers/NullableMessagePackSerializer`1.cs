#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if DEBUG
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#endif // DEBUG
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	[Preserve( AllMembers = true )]
#if !UNITY
	internal class NullableMessagePackSerializer<T> : MessagePackSerializer<T?>
		where T : struct
	{
		private readonly MessagePackSerializer<T> _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext )
			: this( ownerContext, ownerContext.GetSerializer<T>() ) { }

		public NullableMessagePackSerializer( SerializationContext ownerContext, MessagePackSerializer<T> valueSerializer )
			: base( ownerContext, valueSerializer.Capabilities )
		{
			this._valueSerializer = valueSerializer;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T? objectTree )
		{
#if DEBUG
			Contract.Assert( objectTree != null, "objectTree != null" );
#endif // DEBUG
			// null was handled in PackTo() method.
			this._valueSerializer.PackToCore( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T? UnpackFromCore( Unpacker unpacker )
		{
#if DEBUG
			Contract.Assert( !unpacker.LastReadData.IsNil, "!unpacker.LastReadData.IsNil" );
#endif // DEBUG
			// nil was handled in UnpackFrom() method.
			return this._valueSerializer.UnpackFromCore( unpacker );
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, T? objectTree, CancellationToken cancellationToken )
		{
#if DEBUG
			Contract.Assert( objectTree != null, "objectTree != null" );
#endif // DEBUG
			// null was handled in PackTo() method.
			return this._valueSerializer.PackToAsyncCore( packer, objectTree.Value, cancellationToken );
		}

		protected internal override async Task<T?> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
#if DEBUG
			Contract.Assert( !unpacker.LastReadData.IsNil, "!unpacker.LastReadData.IsNil" );
#endif // DEBUG
			// nil was handled in UnpackFrom() method.
			return await this._valueSerializer.UnpackFromAsyncCore( unpacker, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

	}
#else
#warning TODO: Use generic collection if possible for maintenancibility.
	internal class NullableMessagePackSerializer : NonGenericMessagePackSerializer
	{
		private readonly MessagePackSerializer _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext, Type nullableType, Type underlyingType )
			: base( ownerContext, nullableType, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._valueSerializer = ownerContext.GetSerializer( underlyingType );
		}

		public NullableMessagePackSerializer( SerializationContext ownerContext, Type nullableType, MessagePackSerializer valueSerializer )
			: base( ownerContext, nullableType, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._valueSerializer = valueSerializer;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			// null was handled in PackTo() method.
			this._valueSerializer.PackTo( packer, objectTree );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			// nil was handled in UnpackFrom() method.
			return this._valueSerializer.UnpackFrom( unpacker );
		}
	}
#endif // !UNITY
}
