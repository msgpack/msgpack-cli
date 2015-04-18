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

#if !SILVERLIGHT
using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Specialized_NameValueCollectionMessagePackSerializer : MessagePackSerializer<NameValueCollection>, ICollectionInstanceFactory
	{
		public System_Collections_Specialized_NameValueCollectionMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override NameValueCollection UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var collection = new NameValueCollection( count );
			UnpackToCore( unpacker, collection, count );
			return collection;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void UnpackToCore( Unpacker unpacker, NameValueCollection collection )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
		}

		private static void UnpackToCore( Unpacker unpacker, NameValueCollection collection, int keyCount )
		{
			for ( var k = 0; k < keyCount; k++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				var key = unpacker.LastReadData.DeserializeAsString();

				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
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
						if ( !valuesUnpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						collection.Add( key, valuesUnpacker.LastReadData.DeserializeAsString() );
					}
				}
			}
		}

		public object CreateInstance( int initialCapacity )
		{
			return new NameValueCollection( initialCapacity );
		}
	}
}
#endif
