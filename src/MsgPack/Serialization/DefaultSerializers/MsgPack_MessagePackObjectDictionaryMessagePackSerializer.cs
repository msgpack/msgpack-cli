#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class MsgPack_MessagePackObjectDictionaryMessagePackSerializer : MessagePackSerializer<MessagePackObjectDictionary>, ICollectionInstanceFactory
	{
		public MsgPack_MessagePackObjectDictionaryMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, MessagePackObjectDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( var entry in objectTree )
			{
				entry.Key.PackToMessage( packer, null );
				entry.Value.PackToMessage( packer, null );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override MessagePackObjectDictionary UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
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
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				var key = unpacker.LastReadData;

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.IsCollectionHeader )
				{
					MessagePackObject value;
					if ( !unpacker.UnpackSubtreeDataCore( out value ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
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
	}
}
