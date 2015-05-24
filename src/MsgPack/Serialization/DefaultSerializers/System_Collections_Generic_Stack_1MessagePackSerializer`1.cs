#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Generic_Stack_1MessagePackSerializer<TItem> : MessagePackSerializer<Stack<TItem>>, ICollectionInstanceFactory
	{
		private readonly MessagePackSerializer<TItem> _itemSerializer;

		public System_Collections_Generic_Stack_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, Stack<TItem> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override Stack<TItem> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			return new Stack<TItem>( this.UnpackItemsInReverseOrder( unpacker, UnpackHelpers.GetItemsCount( unpacker ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Stack<TItem> collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			foreach ( var item in this.UnpackItemsInReverseOrder( unpacker, UnpackHelpers.GetItemsCount( unpacker ) ) )
			{
				collection.Push( item );
			}
		}

		private IEnumerable<TItem> UnpackItemsInReverseOrder( Unpacker unpacker, int count )
		{
			var buffer = new TItem[ count ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				// Reverse Order
				for ( var i = buffer.Length - 1; i >= 0; i-- )
				{
					if ( !subTreeUnpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					buffer[ i ] = this._itemSerializer.UnpackFrom( subTreeUnpacker );
				}
			}

			return buffer;
		}

		public object CreateInstance( int initialCapacity )
		{
			return new Stack<TItem>( initialCapacity );
		}
	}
}
