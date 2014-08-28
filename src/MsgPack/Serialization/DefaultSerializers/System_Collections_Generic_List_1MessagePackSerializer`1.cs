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
	// ReSharper disable once InconsistentNaming
	internal class System_Collections_Generic_List_1MessagePackSerializer<T> : MessagePackSerializer<List<T>>
	{
		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_Collections_Generic_List_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<T>();
		}

		protected internal override void PackToCore( Packer packer, List<T> objectTree )
		{
			PackerUnpackerExtensions.PackCollectionCore( packer, objectTree, this._itemSerializer );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override List<T> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new List<T>( count );
			this.UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, List<T> collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private void UnpackToCore( Unpacker unpacker, List<T> collection, int count )
		{
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
						collection.Add( this._itemSerializer.UnpackFromCore( subTreeUnpacker ) );
					}
				}
				else
				{
					collection.Add( this._itemSerializer.UnpackFromCore( unpacker ) );
				}
			}
		}
	}
}