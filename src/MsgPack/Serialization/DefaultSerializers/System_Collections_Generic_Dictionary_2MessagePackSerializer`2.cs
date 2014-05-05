#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
	/// <summary>
	///		Provides default implementation for <see cref="Dictionary{TKey,TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of keys of the <see cref="Dictionary{TKey,TValue}"/>.</typeparam>
	/// <typeparam name="TValue">The type of values of the <see cref="Dictionary{TKey,TValue}"/>.</typeparam>
	// ReSharper disable once InconsistentNaming
	internal class System_Collections_Generic_Dictionary_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<Dictionary<TKey, TValue>>
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_Dictionary_2MessagePackSerializer( SerializationContext context )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this._keySerializer = context.GetSerializer<TKey>();
			this._valueSerializer = context.GetSerializer<TValue>();
		}

		protected internal override void PackToCore( Packer packer, Dictionary<TKey, TValue> objectTree )
		{
			PackerUnpackerExtensions.PackDictionaryCore( packer, objectTree, this._keySerializer, this._valueSerializer );
		}

		protected internal override Dictionary<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var result = new Dictionary<TKey, TValue>( count );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				TKey key;
				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						key = this._keySerializer.UnpackFromCore( subTreeUnpacker );
					}
				}
				else
				{
					key = this._keySerializer.UnpackFromCore( unpacker );
				}

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						result.Add( key, this._valueSerializer.UnpackFromCore( subTreeUnpacker ) );
					}
				}
				else
				{
					result.Add( key, this._valueSerializer.UnpackFromCore( unpacker ) );
				}
			}

			return result;
		}
	}
}