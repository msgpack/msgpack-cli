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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
#if !NET35 && !UNITY
using System.Security;
#endif // !NET35 && !UNITY

namespace MsgPack.Serialization
{
	/// <summary>
	///		Specialized <see cref="TypeKeyRepository"/> for serializers.
	/// </summary>
#if !NET35 && !UNITY
	[SecuritySafeCritical]
#endif // !NET35
	internal sealed class SerializerTypeKeyRepository : TypeKeyRepository
	{
#if UNITY
		private static readonly Type[] NonGenericSerializerConstructorParameterTypes =
			{
				typeof( SerializationContext ),
				typeof( Type )
			};
#endif // UNITY

		public SerializerTypeKeyRepository()
			// ReSharper disable once RedundantBaseConstructorCall
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
#if DEBUG
				Contract.Assert( keyType.GetIsGenericType(), "keyType.GetIsGenericType()" );
				Contract.Assert( !keyType.GetIsGenericTypeDefinition(), "!keyType.GetIsGenericTypeDefinition()" );
#endif // DEBUG
				var type = genericDefinitionMatched as Type;
#if DEBUG
				Contract.Assert( type != null, "type != null" );
				Contract.Assert( type.GetIsGenericTypeDefinition(), "type.GetIsGenericTypeDefinition()" );
#endif // DEBUG
#if !UNITY
				var result =
					ReflectionExtensions.CreateInstancePreservingExceptionType( 
						type.MakeGenericType( keyType.GetGenericArguments() ), 
						context
					);
#else
				var resultType = type.IsGenericTypeDefinition ? type.MakeGenericType( keyType.GetGenericArguments() ) : type;
				var constructor2 = resultType.GetConstructor( NonGenericSerializerConstructorParameterTypes );
				object result;
				try
				{
					result =
						constructor2 == null
						? ReflectionExtensions.CreateInstancePreservingExceptionType( resultType, context )
						: ReflectionExtensions.CreateInstancePreservingExceptionType( resultType, context, keyType );
				}
				catch ( Exception ex )
				{
					AotHelper.HandleAotError( keyType, ex );
					throw;
				}
#endif // !UNITY
				Contract.Assert( result != null, "result != null" );

				return result;
			}
		}
	}
}