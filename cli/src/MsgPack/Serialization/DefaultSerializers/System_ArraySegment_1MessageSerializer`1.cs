#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Linq;
using System.Text;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal class System_ArraySegment_1MessageSerializer<T> : MessagePackSerializer<ArraySegment<T>>
	{
		private readonly SerializationContext _context;

		public System_ArraySegment_1MessageSerializer() 
			: this( null, null ){}

		public System_ArraySegment_1MessageSerializer( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			this._context = new SerializationContext( marshalers ?? MarshalerRepository.Default, serializers ?? SerializerRepository.Default );
		}

		protected sealed override void PackToCore( Packer packer, ArraySegment<T> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
			for ( int i = 0; i < objectTree.Count; i++ )
			{
				this._context.MarshalTo( packer, objectTree.Array[ i + objectTree.Offset ] );
			}
		}

		protected sealed override ArraySegment<T> UnpackFromCore( Unpacker unpacker )
		{
			T[] array = new T[ unpacker.ItemsCount ];
			for ( int i = 0; i < array.Length; i++ )
			{
				array[ i ] = this._context.UnmarshalFrom<T>( unpacker );
			}

			return new ArraySegment<T>( array );
		}
	}
}
