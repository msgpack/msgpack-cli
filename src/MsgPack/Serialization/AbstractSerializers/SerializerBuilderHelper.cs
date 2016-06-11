#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	internal static class SerializerBuilderHelper
	{
		public static readonly KeyValuePair<string, TypeDefinition>[] EmptyParameters = new KeyValuePair<string, TypeDefinition>[ 0 ];

#if FEATURE_TAP
		public static readonly Type[] UnpackFromAsyncParameterTypes = { typeof( Unpacker ), typeof( CancellationToken ) };
#endif // FEATURE_TAP

		public const string UnpackingContextTypeName = "UnpackingContext";

		internal static Type GetResolvedDelegateType( TypeDefinition returnType, TypeDefinition[] parameterTypes )
		{
			var typeDefinition = FindDelegateType( returnType, parameterTypes );
			return
				returnType.TryGetRuntimeType() == typeof( void )
					? parameterTypes.Length == 0
					? typeDefinition
					: typeDefinition.MakeGenericType( parameterTypes.Select( t => t.ResolveRuntimeType() ).ToArray() )
					: typeDefinition.MakeGenericType( parameterTypes.Select( t => t.ResolveRuntimeType() ).Concat( new[] { returnType.ResolveRuntimeType() } ).ToArray() );
		}

		internal static TypeDefinition GetDelegateTypeDefinition( TypeDefinition returnType, TypeDefinition[] parameterTypes )
		{
			var typeDefinition = FindDelegateType( returnType, parameterTypes );
			return
				returnType.TryGetRuntimeType() == typeof( void )
					? parameterTypes.Length == 0
					? typeDefinition
					: TypeDefinition.GenericReferenceType( typeDefinition, parameterTypes )
					: TypeDefinition.GenericReferenceType( typeDefinition, parameterTypes.Concat( new[] { returnType } ).ToArray() );
		}

		public static Type FindDelegateType( TypeDefinition returnType, TypeDefinition[] parameterTypes )
		{
			var typeName =
				"System." +
				( returnType.TryGetRuntimeType() == typeof( void ) ? "Action" : "Func" );

			if ( returnType.TryGetRuntimeType() == typeof( void ) && parameterTypes.Length > 0 )
			{
				typeName += "`" + parameterTypes.Length.ToString( "D", CultureInfo.InvariantCulture );
			}
			else
			{
				typeName += "`" + ( parameterTypes.Length + 1 ).ToString( "D", CultureInfo.InvariantCulture );
			}

			var type = Type.GetType( typeName, false );
			if ( type == null )
			{
				// Try get from System.Core.dll
				type = typeof( Enumerable ).GetAssembly().GetType( typeName );
			}

			return type;
		}
	}
}