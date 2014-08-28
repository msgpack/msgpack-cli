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

namespace MsgPack.Serialization.DefaultSerializers
{
	internal class NullableMessagePackSerializer<T> : MessagePackSerializer<T?>
		where T : struct
	{
		private readonly MessagePackSerializer<T> _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._valueSerializer = ownerContext.GetSerializer<T>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T? objectTree )
		{
			if ( !objectTree.HasValue )
			{
				packer.PackNull();
			}
			else
			{
				this._valueSerializer.PackToCore( packer, objectTree.Value );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T? UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.LastReadData.IsNil ? default( T? ) : this._valueSerializer.UnpackFromCore( unpacker );
		}
	}
}
