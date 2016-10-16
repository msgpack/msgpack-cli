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
#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;
#endif

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	/// <summary>
	///		Known syntax declarations.
	/// </summary>
	internal static class Syntax
	{
		public static readonly ExpressionSyntax NullLiteralSyntax =
#if CSHARP
			LiteralExpression( SyntaxKind.NullLiteralExpression );
#elif VISUAL_BASIC
			NothingLiteralExpression( Token( SyntaxKind.NothingKeyword ) );
#endif
		public static readonly SyntaxTokenList PublicKeyword =
			TokenList( Token( SyntaxKind.PublicKeyword ) );

		public static readonly SyntaxTokenList PrivateInstanceKeyword =
			TokenList( Token( SyntaxKind.PrivateKeyword ) );

		public static readonly SyntaxTokenList PrivateReadOnlyKeyword =
			TokenList( Token( SyntaxKind.PrivateKeyword ), Token( SyntaxKind.ReadOnlyKeyword ) );

		public static readonly SyntaxTokenList PrivateStaticKeyword =
#if CSHARP
			TokenList( Token( SyntaxKind.PrivateKeyword ), Token( SyntaxKind.StaticKeyword ) );
#elif VISUAL_BASIC
			TokenList( Token( SyntaxKind.PrivateKeyword ), Token( SyntaxKind.SharedKeyword ) );
#endif

		public static readonly SyntaxTokenList ProtectedOverrideKeyword =
#if CSHARP
			TokenList( Token( SyntaxKind.ProtectedKeyword ), Token( SyntaxKind.OverrideKeyword ) );
#elif VISUAL_BASIC
			TokenList( Token( SyntaxKind.ProtectedKeyword ), Token( SyntaxKind.OverridesKeyword ) );
#endif

		public static readonly SyntaxTokenList ProtectedInternalOverrideKeyword =
#if CSHARP
			TokenList( Token( SyntaxKind.ProtectedKeyword ), Token( SyntaxKind.InternalKeyword ), Token( SyntaxKind.OverrideKeyword ) );
#elif VISUAL_BASIC
			TokenList( Token( SyntaxKind.ProtectedKeyword ), Token( SyntaxKind.AssemblyKeyword ), Token( SyntaxKind.OverridesKeyword ) );
#endif

		public static readonly TypeSyntax VoidTypeSyntax =
#if CSHARP
			IdentifierName( Token( SyntaxKind.VoidKeyword ) );
#elif VISUAL_BASIC
			null;
#endif

		public static readonly TypeSyntax Int32TypeSyntax =
#if CSHARP
			IdentifierName( Token( SyntaxKind.IntKeyword ) );
#elif VISUAL_BASIC
			PredefinedType( Token( SyntaxKind.IntegerKeyword ) );
#endif


		public static readonly TypeSyntax MessagePackObjectTypeSyntax = IdentifierName( typeof( MessagePackObject ).Name );

		public static readonly TypeSyntax TaskTypeSyntax = IdentifierName( typeof( Task ).Name );

		public static readonly TypeSyntax CancellationTokenTypeSyntax = IdentifierName( typeof( CancellationToken ).Name );

		public static readonly TypeSyntax MethodInfoTypeSyntax = IdentifierName( typeof( MethodInfo ).Name );

		public static readonly TypeSyntax FieldInfoTypeSyntax = IdentifierName( typeof( FieldInfo ).Name );

		public static readonly TypeSyntax ObjectTypeSyntax = ToTypeSyntax( TypeDefinition.ObjectType );

		public static readonly TypeSyntax SerializationContextTypeSyntax = ToTypeSyntax( TypeDefinition.SerializationContextType );

		public static readonly SyntaxTrivia BlankLine = SyntaxTrivia( SyntaxKind.EndOfLineTrivia, "\r\n" );


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
				return ArrayType( ToTypeSyntax( type.ElementType ) );
			}

			if ( type.HasRuntimeTypeFully() )
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return IdentifierName( type.ResolveRuntimeType().FullName );
				}
				else
				{
					return
						GenericName(
							Identifier( GetGenericTypeBaseName( type ) ),
							TypeArgumentList(
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
					return IdentifierName( type.TypeName );
				}
				else
				{
					return
						GenericName(
							Identifier( type.TypeName ),
							TypeArgumentList(
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