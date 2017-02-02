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

using System;
using System.Collections.Generic;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	[Preserve( AllMembers = true )]
	internal sealed class System_Collections_Generic_Stack_1MessagePackSerializer<TItem> : MessagePackSerializer<Stack<TItem>>, ICollectionInstanceFactory
	{
		private readonly MessagePackSerializer<TItem> _itemSerializer;

		public System_Collections_Generic_Stack_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, Stack<TItem> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override Stack<TItem> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return new Stack<TItem>( this.UnpackItemsInReverseOrder( unpacker, UnpackHelpers.GetItemsCount( unpacker ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Stack<TItem> collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
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
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
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

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Stack<TItem> objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in objectTree )
			{
				await this._itemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<Stack<TItem>> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return new Stack<TItem>( await this.UnpackItemsInReverseOrderAsync( unpacker, UnpackHelpers.GetItemsCount( unpacker ), cancellationToken ).ConfigureAwait( false ) );
		}

		protected internal override async Task UnpackToAsyncCore( Unpacker unpacker, Stack<TItem> collection, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			foreach ( var item in await this.UnpackItemsInReverseOrderAsync( unpacker, UnpackHelpers.GetItemsCount( unpacker ), cancellationToken ).ConfigureAwait( false ) )
			{
				collection.Push( item );
			}
		}

		private async Task<IEnumerable<TItem>> UnpackItemsInReverseOrderAsync( Unpacker unpacker, int count, CancellationToken cancellationToken )
		{
			var buffer = new TItem[ count ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				// Reverse Order
				for ( var i = buffer.Length - 1; i >= 0; i-- )
				{
					if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					buffer[ i ] = await this._itemSerializer.UnpackFromAsync( subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}

			return buffer;
		}

#endif // FEATURE_TAP

	}
}
