#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2016 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class MsgPack_MessagePackObjectDictionaryMessagePackSerializer : MessagePackSerializer<MessagePackObjectDictionary>, ICollectionInstanceFactory
	{
		public MsgPack_MessagePackObjectDictionaryMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, MessagePackObjectDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( var entry in objectTree )
			{
				entry.Key.PackToMessage( packer, null );
				entry.Value.PackToMessage( packer, null );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override MessagePackObjectDictionary UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var result = new MessagePackObjectDictionary( count );
			UnpackToCore( unpacker, count, result );
			return result;
		}

		protected internal override void UnpackToCore( Unpacker unpacker, MessagePackObjectDictionary collection )
		{
			UnpackToCore( unpacker, UnpackHelpers.GetItemsCount( unpacker ), collection );
		}

		private static void UnpackToCore( Unpacker unpacker, int count, MessagePackObjectDictionary collection )
		{
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				var key = unpacker.LastReadData;

				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( unpacker.IsCollectionHeader )
				{
					MessagePackObject value;
					if ( !unpacker.UnpackSubtreeDataCore( out value ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					collection.Add( key, value );
				}
				else
				{
					collection.Add( key, unpacker.LastReadData );
				}
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new MessagePackObjectDictionary( initialCapacity );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, MessagePackObjectDictionary objectTree, CancellationToken cancellationToken )
		{
			await packer.PackMapHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var entry in objectTree )
			{
				await entry.Key.PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
				await entry.Value.PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<MessagePackObjectDictionary> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var result = new MessagePackObjectDictionary( count );
			await UnpackToAsyncCore( unpacker, count, result, cancellationToken ).ConfigureAwait( false );
			return result;
		}

		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, MessagePackObjectDictionary collection, CancellationToken cancellationToken )
		{
			return UnpackToAsyncCore( unpacker, UnpackHelpers.GetItemsCount( unpacker ), collection, cancellationToken );
		}

		private static async Task UnpackToAsyncCore( Unpacker unpacker, int count, MessagePackObjectDictionary collection, CancellationToken cancellationToken )
		{
			for ( int i = 0; i < count; i++ )
			{
				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				var key = unpacker.LastReadData;

				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( unpacker.IsCollectionHeader )
				{
					var value = await unpacker.UnpackSubtreeDataAsyncCore( cancellationToken ).ConfigureAwait( false );
					if ( !value.Success )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					collection.Add( key, value.Value );
				}
				else
				{
					collection.Add( key, unpacker.LastReadData );
				}
			}
		}

#endif // FEATURE_TAP

	}
}
