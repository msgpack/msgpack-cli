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
using System.Collections.Specialized;
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Specialized_NameValueCollectionMessagePackSerializer : MessagePackSerializer<NameValueCollection>, ICollectionInstanceFactory
	{
		public System_Collections_Specialized_NameValueCollectionMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, NameValueCollection objectTree )
		{
			if ( objectTree == null )
			{
				packer.PackNull();
				return;
			}

			packer.PackMapHeader( objectTree.Count );
			foreach ( string key in objectTree )
			{
				if ( key == null )
				{
					throw new NotSupportedException( "null key is not supported." );
				}

				packer.PackString( key );
				
				var values = objectTree.GetValues( key );
				if ( values == null )
				{
					packer.PackArrayHeader( 0 );
				}
				else
				{
					packer.PackArrayHeader( values.Length );
					foreach ( var value in values )
					{
						packer.PackString( value );
					}
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override NameValueCollection UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new NameValueCollection( count );
			UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, NameValueCollection collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private static void UnpackToCore( Unpacker unpacker, NameValueCollection collection, int keyCount )
		{
			for ( var k = 0; k < keyCount; k++ )
			{
				string key;
				if ( !unpacker.ReadString( out key ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !unpacker.IsArrayHeader )
				{
					throw new SerializationException( "Invalid NameValueCollection value." );
				}

				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
				using ( var valuesUnpacker = unpacker.ReadSubtree() )
				{
					for ( var v = 0; v < itemsCount; v++ )
					{
						string value;

						if ( !valuesUnpacker.ReadString( out value ) )
						{
							SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
						}

						collection.Add( key, value );
					}
				}
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new NameValueCollection( initialCapacity );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, NameValueCollection objectTree, CancellationToken cancellationToken )
		{
			if ( objectTree == null )
			{
				await packer.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			await packer.PackMapHeaderAsync( objectTree.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( string key in objectTree )
			{
				if ( key == null )
				{
					throw new NotSupportedException( "null key is not supported." );
				}

				await packer.PackStringAsync( key, cancellationToken ).ConfigureAwait( false );

				var values = objectTree.GetValues( key);
				if ( values == null )
				{
					await packer.PackArrayHeaderAsync( 0, cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					await packer.PackArrayHeaderAsync( values.Length, cancellationToken ).ConfigureAwait( false );
					foreach ( var value in values )
					{
						await packer.PackStringAsync( value, cancellationToken ).ConfigureAwait( false );
					}
				}
			}
		}

		protected internal override async Task<NameValueCollection> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new NameValueCollection( count );
			await UnpackToAsyncCore( unpacker, collection, count, cancellationToken ).ConfigureAwait( false );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, NameValueCollection collection, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			return UnpackToAsyncCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ), cancellationToken );
		}

		private static async Task UnpackToAsyncCore( Unpacker unpacker, NameValueCollection collection, int keyCount, CancellationToken cancellationToken )
		{
			for ( var k = 0; k < keyCount; k++ )
			{
				var key = await unpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !key.Success )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !unpacker.IsArrayHeader )
				{
					throw new SerializationException( "Invalid NameValueCollection value." );
				}

				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
				using ( var valuesUnpacker = unpacker.ReadSubtree() )
				{
					for ( var v = 0; v < itemsCount; v++ )
					{
						var value = await valuesUnpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
						if ( !value.Success )
						{
							SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
						}

						collection.Add( key.Value, value.Value );
					}
				}
			}
		}

#endif // FEATURE_TAP
	
	}
}
#endif // !NETSTANDARD1_1