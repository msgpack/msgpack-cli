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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization
{
	public abstract class ArrayMarshaler<TCollection>
	{
		public void MarshalTo( Packer packer, TCollection collection, SerializationContext context )
		{
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			if ( collection == null )
			{
				packer.PackNull();
				return;
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this.MarshalCore( packer, collection, context );
		}

		protected abstract void MarshalCore( Packer packer, TCollection collection, SerializationContext context );

		public void UnmarshalTo( Unpacker unpacker, TCollection collection, SerializationContext context )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.Assert( unpacker.IsArrayHeader );

			this.UnmarshalCore( unpacker, collection, context );
		}

		protected abstract void UnmarshalCore( Unpacker unpacker, TCollection collection, SerializationContext context );
	}
}