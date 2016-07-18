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

namespace MsgPack.Serialization.DefaultSerializers
{
	[Preserve( AllMembers = true )]
	internal sealed class ImmutableStackSerializer<T, TItem> : ImmutableCollectionSerializer<T, TItem>
		where T : IEnumerable<TItem>
	{
		public ImmutableStackSerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext, itemsSchema ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var buffer = new TItem[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				// Reverse Order
				for ( int i = buffer.Length - 1; i >= 0; i-- )
				{
					if ( !subTreeUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					buffer[ i ] = this.ItemSerializer.UnpackFrom( subTreeUnpacker );
				}
			}

			return this.Factory( buffer );
		}

#if FEATURE_TAP

		protected internal override async Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var buffer = new TItem[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				// Reverse Order
				for ( int i = buffer.Length - 1; i >= 0; i-- )
				{
					if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					buffer[ i ] = await this.ItemSerializer.UnpackFromAsync( subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}

			return this.Factory( buffer );
		}

#endif // FEATURE_TAP

	}
}