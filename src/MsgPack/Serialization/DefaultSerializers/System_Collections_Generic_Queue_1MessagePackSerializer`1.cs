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
	internal sealed class System_Collections_Generic_Queue_1MessagePackSerializer<TItem> : MessagePackSerializer<Queue<TItem>>, ICollectionInstanceFactory
	{
		private readonly MessagePackSerializer<TItem> _itemSerializer;

		public System_Collections_Generic_Queue_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, Queue<TItem> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override Queue<TItem> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var queue = new Queue<TItem>( UnpackHelpers.GetItemsCount( unpacker ) );
			this.UnpackToCore( unpacker, queue );

			return queue;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Queue<TItem> collection )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				TItem item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = this._itemSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = this._itemSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				collection.Enqueue( item );
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new Queue<TItem>( initialCapacity );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Queue<TItem> objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in objectTree )
			{
				await this._itemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<Queue<TItem>> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var queue = new Queue<TItem>( UnpackHelpers.GetItemsCount( unpacker ) );
			await this.UnpackToAsyncCore( unpacker, queue, cancellationToken ).ConfigureAwait( false );

			return queue;
		}

		protected internal override async Task UnpackToAsyncCore( Unpacker unpacker, Queue<TItem> collection, CancellationToken cancellationToken )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				TItem item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = await this._itemSerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = await this._itemSerializer.UnpackFromAsync( subtreeUnpacker, cancellationToken ).ConfigureAwait( false );
					}
				}

				collection.Enqueue( item );
			}
		}

#endif // FEATURE_TAP
	}
}