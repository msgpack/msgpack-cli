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
using System.Collections;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_StackMessagePackSerializer : MessagePackSerializer<Stack>, ICollectionInstanceFactory
	{
		public System_Collections_StackMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, Stack objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				packer.PackObject( item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override Stack UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			return new Stack( UnpackItemsInReverseOrder( unpacker, UnpackHelpers.GetItemsCount( unpacker ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, Stack collection )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			foreach ( var item in UnpackItemsInReverseOrder( unpacker, UnpackHelpers.GetItemsCount( unpacker ) ) )
			{
				collection.Push( item );
			}
		}

		private static ICollection UnpackItemsInReverseOrder( Unpacker unpacker, int count )
		{
			var buffer = new object[ count ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				// Reverse Order
				for ( var i = buffer.Length - 1; i >= 0; i-- )
				{
					if ( !subTreeUnpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					buffer[ i ] = subTreeUnpacker.LastReadData;
				}
			}

			return buffer;
		}

		public object CreateInstance( int initialCapacity )
		{
			return new Stack( initialCapacity );
		}
	}
}