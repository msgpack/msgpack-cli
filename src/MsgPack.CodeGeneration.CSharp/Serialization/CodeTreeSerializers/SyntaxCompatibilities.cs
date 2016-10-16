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
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal static class SyntaxCompatibilities
	{
		public static BinaryExpressionSyntax AndAlsoExpression( ExpressionSyntax left, ExpressionSyntax right ) =>
			BinaryExpression( SyntaxKind.LogicalAndExpression, left, right );

		public static ConstructorInitializerSyntax BaseConstructorInitializer( ArgumentListSyntax argumentList ) =>
			ConstructorInitializer( SyntaxKind.BaseConstructorInitializer, argumentList );

		public static ConstructorDeclarationSyntax ConstructorDeclaration( string identifier, SyntaxTokenList accessibility ) =>
			SyntaxFactory.ConstructorDeclaration( identifier ).WithModifiers( accessibility );

		public static FieldDeclarationSyntax FieldDeclaration( TypeSyntax type, string identifier ) =>
			SyntaxFactory.FieldDeclaration( VariableDeclaration( type ).AddVariables( VariableDeclarator( identifier ) ) );

		public static GenericNameSyntax GenericName( string baseName, params TypeSyntax[] genericArguments ) =>
			SyntaxFactory.GenericName( baseName ).AddTypeArgumentListArguments( genericArguments );

		public static GenericNameSyntax GenericName( string baseName, IEnumerable<TypeSyntax> genericArguments ) =>
			SyntaxFactory.GenericName( baseName ).AddTypeArgumentListArguments( genericArguments.ToArray() );

		public static IfStatementSyntax IfElseStatement( ExpressionSyntax conditionExpression, IEnumerable<StatementSyntax> thenStatements, IEnumerable<StatementSyntax> elseStatements ) =>
			IfStatement(
				conditionExpression,
				Block(
					thenStatements
				),
				ElseClause(
					Block(
						elseStatements
					)
				)
			);

		public static BinaryExpressionSyntax OrExpression( ExpressionSyntax left, ExpressionSyntax right ) =>
			BinaryExpression( SyntaxKind.BitwiseOrExpression, left, right );

		public static StatementSyntax SimpleAssignmentStatement( ExpressionSyntax left, ExpressionSyntax right ) =>
			ExpressionStatement( AssignmentExpression( SyntaxKind.SimpleAssignmentExpression, left, right ) );

		public static MemberAccessExpressionSyntax SimpleMemberAccessExpression( ExpressionSyntax expression, SimpleNameSyntax name ) =>
			MemberAccessExpression( SyntaxKind.SimpleMemberAccessExpression, expression, name );

		public static ArrayCreationExpressionSyntax SZArrayCreationExpression( TypeSyntax elementType, ExpressionSyntax length ) =>
			SyntaxFactory.ArrayCreationExpression(
				ArrayType(
					elementType,
					new SyntaxList<ArrayRankSpecifierSyntax>().Add(
						ArrayRankSpecifier().AddSizes( length )
					)
				)
			);

		public static ArrayCreationExpressionSyntax SZArrayCreationExpression( TypeSyntax elementType, ExpressionSyntax length, IEnumerable<ExpressionSyntax> initializerExpressions ) =>
			SyntaxFactory.ArrayCreationExpression(
				ArrayType(
					elementType,
					new SyntaxList<ArrayRankSpecifierSyntax>().Add(
						ArrayRankSpecifier().AddSizes( length )
					)
				),
				InitializerExpression(
					SyntaxKind.ArrayInitializerExpression,
					new SeparatedSyntaxList<ExpressionSyntax>().AddRange( initializerExpressions )
				)
			);


		public static string GetIdentifierText( this ParameterSyntax source ) => source.Identifier.ValueText;
	}
}