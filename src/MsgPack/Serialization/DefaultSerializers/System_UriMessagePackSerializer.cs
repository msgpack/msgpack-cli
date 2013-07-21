#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_UriMessagePackSerializer : MessagePackSerializer<Uri>
	{
		public System_UriMessagePackSerializer( PackerCompatibilityOptions packerCompatibilityOptions )
			: base( packerCompatibilityOptions ) { }

		protected internal sealed override void PackToCore( Packer packer, Uri objectTree )
		{
			packer.PackString( objectTree.ToString() );
		}

		protected internal sealed override Uri UnpackFromCore( Unpacker unpacker )
		{
			return new Uri( unpacker.LastReadData.DeserializeAsString() );
		}
	}
}
