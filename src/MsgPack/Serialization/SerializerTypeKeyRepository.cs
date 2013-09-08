#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Specialized <see cref="TypeKeyRepository"/> for serializers.
	/// </summary>
	internal sealed class SerializerTypeKeyRepository : TypeKeyRepository
	{
		public SerializerTypeKeyRepository()
			: base()
		{
		}

		public SerializerTypeKeyRepository( SerializerTypeKeyRepository copiedFrom )
			: base( copiedFrom )
		{
		}

		public SerializerTypeKeyRepository( Dictionary<RuntimeTypeHandle, object> table )
			: base( table )
		{
		}

		public TEntry Get<T, TEntry>( SerializationContext context )
			where TEntry : class
		{
			object matched;
			object genericDefinitionMatched;
			if ( !this.Get( typeof( T ), out matched, out genericDefinitionMatched ) )
			{
				return null;
			}

			if ( matched != null )
			{
				return matched as TEntry;
			}
			else
			{
				Contract.Assert( typeof( T ).GetIsGenericType() );
				Contract.Assert( !typeof( T ).GetIsGenericTypeDefinition() );
				var type = genericDefinitionMatched as Type;
				Contract.Assert( type != null );
				Contract.Assert( type.GetIsGenericTypeDefinition() );
				var result = ( TEntry )Activator.CreateInstance( type.MakeGenericType( typeof( T ).GetGenericArguments() ), context );
				Contract.Assert( result != null );
				return result;
			}
		}
	}
}