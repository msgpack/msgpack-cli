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
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultMarshalers
{
	internal sealed class EnumMarshaler<T> : MessageMarshaler<T>
	{
		public EnumMarshaler()
		{
			if ( !typeof( T ).IsEnum )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not enum.", typeof( T ) ) );
			}
		}
		
		protected sealed override void MarshalToCore( Packer packer, T value )
		{
			packer.PackString( value.ToString() );
		}

		protected sealed override T UnmarshalFromCore( Unpacker unpacker )
		{
			// TODO: Avoid boxing...
			try
			{
				return ( T )Enum.Parse( typeof( T ), unpacker.Data.Value.AsString() );
			}
			catch( ArgumentException ae)
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not valid for enum type '{1}'.", unpacker.Data.Value.AsString(), typeof( T ) ) ,ae);
			}
		}
	}
}
