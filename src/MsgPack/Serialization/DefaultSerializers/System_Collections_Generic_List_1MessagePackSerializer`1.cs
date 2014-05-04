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
	///		Provides default implementation for <see cref="List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of items of the <see cref="List{T}"/>.</typeparam>
	internal class System_Collections_Generic_List_1MessagePackSerializer<T> : MessagePackSerializer<List<T>>
	{
		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_Collections_Generic_List_1MessagePackSerializer( SerializationContext context )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this._itemSerializer = context.GetSerializer<T>();
		}

		protected internal override void PackToCore( Packer packer, List<T> objectTree )
		{
			PackerUnpackerExtensions.PackCollectionCore( packer, objectTree, this._itemSerializer );
		}

		protected internal override List<T> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var result = new List<T>( count );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}
				if ( unpacker.IsCollectionHeader )
				{
					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						result.Add( this._itemSerializer.UnpackFromCore( subTreeUnpacker ) );
					}
				}
				else
				{
					result.Add( this._itemSerializer.UnpackFromCore( unpacker ) );
				}
			}

			return result;
		}
	}
}