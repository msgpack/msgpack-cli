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
	internal sealed class TypeEmbedingPolymorphicMessagePackSerializer<T> : MessagePackSerializer<T>, IPolymorphicDeserializer
	{
		private readonly PolymorphismSchema _schema;

		public TypeEmbedingPolymorphicMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			TypeInfoEncoder.Encode( packer, objectTree.GetType() );
			this.GetActualTypeSerializer( objectTree.GetType() ).PackTo( packer, objectTree );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return
				TypeInfoEncoder.Decode(
					unpacker,
					// ReSharper disable once ConvertClosureToMethodGroup
					u => TypeInfoEncoder.DecodeRuntimeTypeInfo( u ), // Lamda capture is more efficient.
					( t, u ) => ( T )this.GetActualTypeSerializer( t ).UnpackFrom( u )
				);
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