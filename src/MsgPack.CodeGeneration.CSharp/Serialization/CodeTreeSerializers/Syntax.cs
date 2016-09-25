#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	/// <summary>
	///		Known syntax declarations.
	/// </summary>
	internal static class Syntax
	{
		public static readonly ExpressionSyntax NullLiteralSyntax = SyntaxFactory.LiteralExpression( SyntaxKind.NullLiteralExpression );

		public static readonly SyntaxTokenList PublicKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.PublicKeyword ) );

		public static readonly SyntaxTokenList PrivateInstanceKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.PrivateKeyword ) );

		public static readonly SyntaxTokenList PrivateReadOnlyKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.PrivateKeyword ), SyntaxFactory.Token( SyntaxKind.ReadOnlyKeyword ) );

		public static readonly SyntaxTokenList PrivateStaticKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.PrivateKeyword ), SyntaxFactory.Token( SyntaxKind.StaticKeyword ) );

		public static readonly SyntaxTokenList ProtectedOverrideKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.ProtectedKeyword ), SyntaxFactory.Token( SyntaxKind.InternalKeyword ), SyntaxFactory.Token( SyntaxKind.OverrideKeyword ) );

		public static readonly SyntaxTokenList ProtectedInternalOverrideKeyword =
			SyntaxFactory.TokenList( SyntaxFactory.Token( SyntaxKind.ProtectedKeyword ), SyntaxFactory.Token( SyntaxKind.OverrideKeyword ) );

		public static readonly TypeSyntax VoidTypeSyntax = SyntaxFactory.IdentifierName( SyntaxFactory.Token( SyntaxKind.VoidKeyword ) );

		public static readonly TypeSyntax Int32TypeSyntax = SyntaxFactory.IdentifierName( SyntaxFactory.Token( SyntaxKind.IntKeyword ) );

		public static readonly TypeSyntax MessagePackObjectTypeSyntax = SyntaxFactory.IdentifierName( typeof( MessagePackObject ).Name );

		public static readonly TypeSyntax TaskTypeSyntax = SyntaxFactory.IdentifierName( typeof( Task ).Name );

		public static readonly TypeSyntax CancellationTokenTypeSyntax = SyntaxFactory.IdentifierName( typeof( CancellationToken ).Name );

		public static readonly TypeSyntax MethodInfoTypeSyntax = SyntaxFactory.IdentifierName( typeof( MethodInfo ).Name );

		public static readonly TypeSyntax FieldInfoTypeSyntax = SyntaxFactory.IdentifierName( typeof( FieldInfo ).Name );

		public static readonly TypeSyntax ObjectTypeSyntax = ToTypeSyntax( TypeDefinition.ObjectType );

		public static readonly TypeSyntax SerializationContextTypeSyntax = ToTypeSyntax( TypeDefinition.SerializationContextType );

		public static readonly SyntaxTrivia BlankLine = SyntaxFactory.SyntaxTrivia( SyntaxKind.EndOfLineTrivia, "\r\n" );


		private static string GetGenericTypeBaseName( TypeDefinition genericType )
		{
			return
				genericType.HasRuntimeTypeFully()
					? genericType.ResolveRuntimeType().FullName.Remove( genericType.ResolveRuntimeType().FullName.IndexOf( '`' ) )
					: genericType.TypeName;
		}

		public static TypeSyntax ToTypeSyntax( TypeDefinition type )
		{
			if ( type.IsArray )
			{
				return SyntaxFactory.ArrayType( ToTypeSyntax( type.ElementType ) );
			}

			if ( type.HasRuntimeTypeFully() )
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return SyntaxFactory.IdentifierName( type.ResolveRuntimeType().FullName );
				}
				else
				{
					return
						SyntaxFactory.GenericName(
							SyntaxFactory.Identifier( GetGenericTypeBaseName( type ) ),
							SyntaxFactory.TypeArgumentList(
								new SeparatedSyntaxList<TypeSyntax>().AddRange(
									type.GenericArguments.Select( ToTypeSyntax )
								)
							)
						);
				}
			}
			else
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return SyntaxFactory.IdentifierName( type.TypeName );
				}
				else
				{
					return
						SyntaxFactory.GenericName(
							SyntaxFactory.Identifier( type.TypeName ),
							SyntaxFactory.TypeArgumentList(
								new SeparatedSyntaxList<TypeSyntax>().AddRange(
									type.GenericArguments.Select( ToTypeSyntax )
								)
							)
						);
				}
			}
		}

	}
}