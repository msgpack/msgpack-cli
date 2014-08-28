#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, KeyValuePair<TKey, TValue> objectTree )
		{
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, objectTree.Key );
			this._valueSerializer.PackTo( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override KeyValuePair<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var key = unpacker.LastReadData.IsNil ? default( TKey ) : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var value = unpacker.LastReadData.IsNil ? default( TValue ) : this._valueSerializer.UnpackFrom( unpacker );

			return new KeyValuePair<TKey, TValue>( key, value );
		}
	}
}
