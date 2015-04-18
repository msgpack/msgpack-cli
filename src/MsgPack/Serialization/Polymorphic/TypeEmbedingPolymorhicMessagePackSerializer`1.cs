#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.Globalization;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Implements polymorphic serializer which uses open types and non-interoperable ext-type tag and .NET type information.
	/// </summary>
	/// <typeparam name="T">The base type of the polymorhic member.</typeparam>
	internal sealed class TypeEmbedingPolymorhicMessagePackSerializer<T> : MessagePackSerializer<T>, IPolymorphicDeserializer
	{
		private readonly PolymorphismSchema _schema;

		public TypeEmbedingPolymorhicMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext )
		{
			if ( typeof( T ).GetIsValueType() )
			{
				throw SerializationExceptions.NewValueTypeCannotBePolymorphic( typeof( T ) );
			}

			this._schema = schema.FilterSelf();
		}

		private IMessagePackSerializer GetActualTypeSerializer( Type actualType )
		{
			var result = this.OwnerContext.GetSerializer( actualType, this._schema );
			if ( result == null )
			{
				throw new SerializationException(
					String.Format( CultureInfo.CurrentCulture, "Cannot get serializer for actual type {0} from context.", actualType ) 
				);
			}

			return result;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackArrayHeader( 3 );
			packer.PackExtendedTypeValue(
				this.OwnerContext.TypeEmbeddingSettings.TypeEmbeddingIdentifier,
				TypeInfoEncodingBytes.RawCompressed
			);
			TypeInfoEncoder.Encode( packer, objectTree.GetType() );

			this.GetActualTypeSerializer( objectTree.GetType() ).PackTo( packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			// It is not reasonable to identify other forms.
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 3 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !subTreeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}
				var header = subTreeUnpacker.LastReadData.AsMessagePackExtendedTypeObject();
				if ( header.TypeCode != this.OwnerContext.TypeEmbeddingSettings.TypeEmbeddingIdentifier )
				{
					throw new SerializationException(
						String.Format( CultureInfo.CurrentCulture, "Unknown extension type {0}.", header.TypeCode )
					);
				}

				if ( header.Body.Length != TypeInfoEncodingBytes.RawCompressed.Length
					|| header.Body[ 0 ] != TypeInfoEncodingBytes.RawCompressed[ 0 ] )
				{
					throw new SerializationException( "Unknown type info encoding type." );
				}

				if ( !subTreeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				var objectType = TypeInfoEncoder.Decode( subTreeUnpacker );

				if ( !subTreeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				return ( T )this.GetActualTypeSerializer( objectType ).UnpackFrom( subTreeUnpacker );
			}
		}

		object IPolymorphicDeserializer.PolymorphicUnpackFrom( Unpacker unpacker )
		{
			return this.UnpackFromCore( unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this.GetActualTypeSerializer( collection.GetType() ).UnpackTo( unpacker, collection );
		}
	}
}