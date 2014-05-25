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
#if !UNITY_ANDROID && !UNITY_IPHONE
using System.Diagnostics.Contracts;
#endif // !UNITY_ANDROID && !UNITY_IPHONE

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

		public object Get( SerializationContext context, Type keyType )
		{
			object matched;
			object genericDefinitionMatched;
			if ( !this.Get( keyType, out matched, out genericDefinitionMatched ) )
			{
				return null;
			}

			if ( matched != null )
			{
				return matched;
			}
			else
			{
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Assert( keyType.GetIsGenericType() );
				Contract.Assert( !keyType.GetIsGenericTypeDefinition() );
#endif // !UNITY_ANDROID && !UNITY_IPHONE
				var type = genericDefinitionMatched as Type;
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Assert( type != null );
				Contract.Assert( type.GetIsGenericTypeDefinition() );
#endif // !UNITY_ANDROID && !UNITY_IPHONE
				var result = Activator.CreateInstance( type.MakeGenericType( keyType.GetGenericArguments() ), context );
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Assert( result != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE
				return result;
			}
		}
	}
}