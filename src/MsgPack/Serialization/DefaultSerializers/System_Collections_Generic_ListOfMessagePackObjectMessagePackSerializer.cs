#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal class System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer : MessagePackSerializer<List<MessagePackObject>>
	{
		public System_Collections_Generic_ListOfMessagePackObjectMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) { }

		protected internal override void PackToCore( Packer packer, List<MessagePackObject> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			foreach ( var item in objectTree )
			{
				item.PackToMessage( packer, null );
			}
		}

		protected internal override List<MessagePackObject> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
			var result = new List<MessagePackObject>( count );
			for ( var i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				result.Add( unpacker.LastReadData );
			}

			return result;
		}
	}
}
