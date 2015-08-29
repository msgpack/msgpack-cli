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
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_ObjectMessagePackSerializer : MessagePackSerializer<object>
	{
		public System_ObjectMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, object value )
		{
			if ( value.GetType() == typeof( object ) )
			{
				// Prevents stack overflow -- System.Object cannot be serialized anyway because it does not have any properties/fields to serialize.
				throw new SerializationException( "System.Object cannot be serialized." );
			}

			packer.PackObject( value, this.OwnerContext );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.IsArrayHeader )
			{
				var result = new MessagePackObject[ UnpackHelpers.GetItemsCount( unpacker ) ];
				for ( int i = 0; i < result.Length; i++ )
				{
					if ( !unpacker.ReadObject( out result[ i ] ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}
				}

				return new MessagePackObject( result );
			}
			else if ( unpacker.IsMapHeader )
			{
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );
				var result = new MessagePackObjectDictionary( itemsCount );
				for ( int i = 0; i < itemsCount; i++ )
				{
					MessagePackObject key;
					if ( !unpacker.ReadObject( out key ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					MessagePackObject value;
					if ( !unpacker.ReadObject( out value ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					result.Add( key, value );
				}

				return new MessagePackObject( result );
			}
			else
			{
				var result = unpacker.LastReadData;
				return result.IsNil ? MessagePackObject.Nil : result;
			}
		}
	}
}
