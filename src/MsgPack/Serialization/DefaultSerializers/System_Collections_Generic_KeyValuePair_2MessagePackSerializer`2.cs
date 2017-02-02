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
using System.Collections.Generic;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	[Preserve( AllMembers = true )]
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, KeyValuePair<TKey, TValue> objectTree )
		{
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, objectTree.Key );
			this._valueSerializer.PackTo( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override KeyValuePair<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			var key = unpacker.LastReadData.IsNil ? default( TKey ) : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			var value = unpacker.LastReadData.IsNil ? default( TValue ) : this._valueSerializer.UnpackFrom( unpacker );

			return new KeyValuePair<TKey, TValue>( key, value );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, KeyValuePair<TKey, TValue> objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await this._keySerializer.PackToAsync( packer, objectTree.Key, cancellationToken ).ConfigureAwait( false );
			await this._valueSerializer.PackToAsync( packer, objectTree.Value, cancellationToken ).ConfigureAwait( false );
		}

		protected internal override async Task<KeyValuePair<TKey, TValue>> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			var key = unpacker.LastReadData.IsNil ? default( TKey ) : await this._keySerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );

			if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			var value = unpacker.LastReadData.IsNil ? default( TValue ) : await this._valueSerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );

			return new KeyValuePair<TKey, TValue>( key, value );
		}

#endif // FEATURE_TAP

	}
}
