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

#if !NETSTANDARD1_1
using System;
using System.Collections;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_QueueMessagePackSerializer : MessagePackSerializer<Queue>, ICollectionInstanceFactory
	{
		public System_Collections_QueueMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, Queue objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				packer.PackObject( item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override Queue UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var queue = new Queue( UnpackHelpers.GetItemsCount( unpacker ) );
			this.UnpackToCore( unpacker, queue );

			return queue;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Queue collection )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				collection.Enqueue( unpacker.LastReadData );
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new Queue( initialCapacity );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Queue objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in objectTree )
			{
				await packer.PackObjectAsync( item, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<Queue> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var queue = new Queue( UnpackHelpers.GetItemsCount( unpacker ) );
			await this.UnpackToAsyncCore( unpacker, queue, cancellationToken ).ConfigureAwait( false );

			return queue;
		}

		protected internal override async Task UnpackToAsyncCore( Unpacker unpacker, Queue collection, CancellationToken cancellationToken )
		{
			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
			for ( int i = 0; i < itemsCount; i++ )
			{
				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				collection.Enqueue( unpacker.LastReadData );
			}
		}

#endif // FEATURE_TAP

	}
}
#endif // !NETSTANDARD1_1