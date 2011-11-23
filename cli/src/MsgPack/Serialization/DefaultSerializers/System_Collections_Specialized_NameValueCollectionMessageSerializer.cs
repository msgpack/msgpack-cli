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
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_Collections_Specialized_NameValueCollectionMessageSerializer : MessagePackSerializer<NameValueCollection>
	{
		protected sealed override void PackToCore( Packer packer, NameValueCollection objectTree )
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

				var values = objectTree.GetValues( key );
				if ( values == null )
				{
					// Ignore
					continue;
				}

				packer.PackString( key );
				packer.PackArrayHeader( values.Length );
				foreach ( var value in values )
				{
					packer.PackString( value );
				}
			}
		}

		protected sealed override NameValueCollection UnpackFromCore( Unpacker unpacker )
		{
			var result = new NameValueCollection( checked( ( int )unpacker.ItemsCount ) );

			while ( unpacker.Read() )
			{
				var key = unpacker.Data.Value.AsString();
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( !unpacker.IsArrayHeader )
				{
					throw new SerializationException( "Invalid NameValueCollection value." );
				}

				using ( var valuesUnpacker = unpacker.ReadSubtree() )
				{
					while ( valuesUnpacker.Read() )
					{
						result.Add( key, unpacker.Data.Value.AsString() );
					}
				}
			}

			return result;
		}
	}
}
