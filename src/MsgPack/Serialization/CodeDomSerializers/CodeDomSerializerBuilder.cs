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
using System.CodeDom;
using System.Linq;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	/// <summary>
	///		Defines non-generic portion of <see cref="CodeDomSerializerBuilder{T}"/>.
	/// </summary>
	internal static class CodeDomSerializerBuilder
	{
		public static readonly CodeTypeReference[] EmptyGenericArguments = new CodeTypeReference[ 0 ];

		public static CodeTypeReference ToCodeTypeReference( TypeDefinition type )
		{
			if ( type == null )
			{
				return null;
			}

			if ( type.IsArray )
			{
				return new CodeTypeReference( ToCodeTypeReference( type.ElementType ), 1 );
			}

			if ( type.HasRuntimeTypeFully() )
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return new CodeTypeReference( type.ResolveRuntimeType() );
				}
				else
				{
					return
						new CodeTypeReference(
							GetGenericTypeBaseName( type ),
							type.GenericArguments.Select( ToCodeTypeReference ).ToArray()
						);
				}
			}
			else
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return new CodeTypeReference( type.TypeName );
				}
				else
				{
					return new CodeTypeReference( type.TypeName, type.GenericArguments.Select( ToCodeTypeReference ).ToArray() );
				}
			}
		}

		private static string GetGenericTypeBaseName( TypeDefinition genericType )
		{
			return
				genericType.HasRuntimeTypeFully()
				? genericType.ResolveRuntimeType().FullName.Remove( genericType.ResolveRuntimeType().FullName.IndexOf( '`' ) )
				: genericType.TypeName;
		}

		internal static CodeTypeReferenceExpression ToCodeTypeReferenceExpression( TypeDefinition type )
		{
			return new CodeTypeReferenceExpression( ToCodeTypeReference( type ) );
		}
	}
}