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
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal static class SyntaxCompatibilities
	{
		public static SimpleArgumentSyntax Argument( ExpressionSyntax expression ) => SimpleArgument( expression );

		public static SimpleArgumentSyntax AttributeArgument( ExpressionSyntax expression ) => SimpleArgument( expression );

		public static ExpressionStatementSyntax BaseConstructorInitializer( ArgumentListSyntax argumentList ) =>
			ExpressionStatement( InvocationExpression( SimpleMemberAccessExpression( MyBaseExpression(), IdentifierName( "New" ) ), argumentList ) );

		public static SyntaxList<StatementSyntax> Block( IEnumerable<StatementSyntax> statements ) => new SyntaxList<StatementSyntax>().AddRange( statements );

		public static DirectCastExpressionSyntax CastExpression( TypeSyntax type, ExpressionSyntax expression ) => DirectCastExpression( expression, type );

		public static ClassStatementSyntax ClassDeclaration( string identifier ) => ClassStatement( identifier );

		public static InvocationExpressionSyntax ConditionalExpression( ExpressionSyntax conditionExpression, ExpressionSyntax thenExpression, ExpressionSyntax elseExpression ) =>
			InvocationExpression(
				MultiLineFunctionLambdaExpression(
					FunctionLambdaHeader(),
					EndFunctionStatement()
				).AddStatements(
					MultiLineIfBlock(
							SyntaxFactory.IfStatement( conditionExpression )
					).WithStatements( new SyntaxList<StatementSyntax>().Add( ReturnStatement( thenExpression ) ) )
					.WithElseBlock( ElseBlock( new SyntaxList<StatementSyntax>().Add( ReturnStatement( elseExpression ) ) ) )
				)
			);

		public static InvocationExpressionSyntax ElementAccessExpression( ExpressionSyntax typeOrInstance ) => InvocationExpression( typeOrInstance );

		public static FinallyBlockSyntax FinallyClause( SyntaxList<StatementSyntax> statements ) => FinallyBlock( statements );

		public static ForEachBlockSyntax ForEachStatement( TypeSyntax variableType, string variableName, ExpressionSyntax collectionExpression, SyntaxList<StatementSyntax> statements ) =>
			ForEachBlock(
				SyntaxFactory.ForEachStatement( VariableDeclarator( variableName ).WithAsClause( SimpleAsClause( variableType ) ), collectionExpression )
			).WithStatements( statements );

		public static MultiLineIfBlockSyntax IfStatement( ExpressionSyntax conditionExpression, IEnumerable<StatementSyntax> thenStatements ) =>
			MultiLineIfBlock(
				SyntaxFactory.IfStatement( conditionExpression )
			).WithStatements( Block( thenStatements ) );

		public static MultiLineIfBlockSyntax IfElseStatement( ExpressionSyntax conditionExpression, IEnumerable<StatementSyntax> thenStatements, IEnumerable<StatementSyntax> elseStatements ) =>
			MultiLineIfBlock(
				SyntaxFactory.IfStatement( conditionExpression )
			).WithStatements( Block( thenStatements ) )
			.WithElseBlock( ElseBlock( Block( elseStatements ) ) );

		public static ExpressionSyntax MakeRefExpression( ExpressionSyntax expression ) => expression;

		public static ArrayCreationExpressionSyntax SZArrayCreationExpression( TypeSyntax elementType, ExpressionSyntax length ) =>
			ArrayCreationExpression(
				elementType,
				CollectionInitializer()
			).AddArrayBoundsArguments( Argument( length ) );

		public static ArrayCreationExpressionSyntax SZArrayCreationExpression( TypeSyntax elementType, ExpressionSyntax length, IEnumerable<ExpressionSyntax> initializerExpressions ) =>
			ArrayCreationExpression(
				elementType,
				CollectionInitializer( new SeparatedSyntaxList<ExpressionSyntax>().AddRange( initializerExpressions ) )
			).AddArrayBoundsArguments( Argument( length ) );

		public static MeExpressionSyntax ThisExpression() => MeExpression();

		public static GetTypeExpressionSyntax TypeOfExpression( TypeSyntax type ) => GetTypeExpression( type );

		public static ImportsStatementSyntax UsingDirective( IdentifierNameSyntax identifier ) =>
			ImportsStatement( new SeparatedSyntaxList<ImportsClauseSyntax>().Add( SimpleImportsClause( identifier ) ) );

		public static LocalDeclarationStatementSyntax VariableDeclaration( TypeSyntax type, SeparatedSyntaxList<VariableDeclaratorSyntax> variables ) =>
			LocalDeclarationStatement( TokenList( Token( SyntaxKind.DimKeyword ) ), variables );

		public static VariableDeclaratorSyntax VariableDeclarator( string identifier ) =>
			SyntaxFactory.VariableDeclarator( ModifiedIdentifier( identifier ) );

		public static SubNewStatementSyntax ConstructorDeclaration( string identifier, SyntaxTokenList accessibility ) =>
			SubNewStatement().WithModifiers( accessibility );

		public static FieldDeclarationSyntax FieldDeclaration( TypeSyntax type, string identifier ) =>
			SyntaxFactory.FieldDeclaration( VariableDeclarator( identifier ).WithAsClause( SimpleAsClause( type ) ) );

		public static GenericNameSyntax GenericName( string baseName, params TypeSyntax[] genericArguments ) =>
			SyntaxFactory.GenericName( baseName, TypeArgumentList( genericArguments ) );

		public static GenericNameSyntax GenericName( string baseName, IEnumerable<TypeSyntax> genericArguments ) =>
			GenericName( baseName, genericArguments.ToArray() );

		public static MethodStatementSyntax MethodDeclaration( TypeSyntax returnType, string identifier ) =>
			returnType == null
				? SubStatement( identifier )
				: FunctionStatement( identifier ).WithAsClause( SimpleAsClause( returnType ) );

		public static NamespaceBlockSyntax NamespaceDeclaration( NameSyntax name ) =>
			NamespaceBlock( NamespaceStatement( name ) );

		public static TryBlockSyntax TryStatement( SyntaxList<StatementSyntax> statements, SyntaxList<CatchBlockSyntax> catchBlocks, FinallyBlockSyntax finallyBlock ) =>
			TryBlock( statements, catchBlocks, finallyBlock );


		public static ConstructorBlockSyntax AddBodyStatements( this ConstructorBlockSyntax source, params StatementSyntax[] statements ) =>
			source.AddStatements( statements );

		public static string GetIdentifierText( this ParameterSyntax source ) => source.Identifier.Identifier.ValueText;

		public static MethodBlockSyntax WithBody( this MethodStatementSyntax source, SyntaxList<StatementSyntax> statements ) =>
			( source.SubOrFunctionKeyword.ValueText == "Sub" ? SubBlock( source ) : FunctionBlock( source ) )
				.WithStatements( statements );

		public static ConstructorBlockSyntax WithBody( this SubNewStatementSyntax source, SyntaxList<StatementSyntax> statements ) =>
			ConstructorBlock( source, statements );

		public static ConstructorBlockSyntax WithInitializer( this SubNewStatementSyntax source, ExpressionStatementSyntax baseInitializer ) =>
			ConstructorBlock( source, new SyntaxList<StatementSyntax>().Add( baseInitializer ) );

		public static ParameterSyntax WithType( this ParameterSyntax source, TypeSyntax type ) =>
			source.WithAsClause( SimpleAsClause( type ) );

		public static CompilationUnitSyntax WithUsings( this CompilationUnitSyntax source, SyntaxList<ImportsStatementSyntax> imports ) =>
			source.WithImports( imports );
	}
}