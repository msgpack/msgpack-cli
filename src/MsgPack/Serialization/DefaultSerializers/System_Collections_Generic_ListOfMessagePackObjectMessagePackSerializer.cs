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
	internal class System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer : MessagePackSerializer<List<MessagePackObject>>, ICollectionInstanceFactory
	{
		public System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, List<MessagePackObject> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				item.PackToMessage( packer, null );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override List<MessagePackObject> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new List<MessagePackObject>( count );
			UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, List<MessagePackObject> collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private static void UnpackToCore( Unpacker unpacker, List<MessagePackObject> collection, int count )
		{
			for ( var i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				collection.Add( unpacker.LastReadData );
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new List<MessagePackObject>( initialCapacity );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, List<MessagePackObject> objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in objectTree )
			{
				await item.PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<List<MessagePackObject>> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new List<MessagePackObject>( count );
			await UnpackToAsyncCore( unpacker, collection, count, cancellationToken ).ConfigureAwait( false );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, List<MessagePackObject> collection, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			return UnpackToAsyncCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ), cancellationToken );
		}

		private static async Task UnpackToAsyncCore( Unpacker unpacker, List<MessagePackObject> collection, int count, CancellationToken cancellationToken )
		{
			for ( var i = 0; i < count; i++ )
			{
				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				collection.Add( unpacker.LastReadData );
			}
		}

#endif // FEATURE_TAP

	}
}
