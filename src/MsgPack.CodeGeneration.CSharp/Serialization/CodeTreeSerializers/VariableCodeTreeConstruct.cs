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
#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;
using VariableDeclarationSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.LocalDeclarationStatementSyntax;
#endif

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal sealed class VariableCodeTreeConstruct : CodeTreeConstruct
	{
		private readonly VariableDeclarationSyntax _syntax;
		private readonly IdentifierNameSyntax _identifierName;

		public override bool IsExpression => true;

		public override bool IsStatement => true;

		public VariableCodeTreeConstruct( TypeDefinition contextType, VariableDeclarationSyntax syntax )
			: base( contextType )
		{
			this._syntax = syntax;
#if CSHARP
			this._identifierName = IdentifierName( syntax.Variables.Single().Identifier );
#elif VISUAL_BASIC
			this._identifierName = IdentifierName( syntax.Declarators.Single().Names.Single().Identifier );
#endif
		}

		public override ExpressionSyntax AsExpression() => this._identifierName;

		public override IEnumerable<StatementSyntax> AsStatements() =>
#if CSHARP
			new StatementSyntax[] { LocalDeclarationStatement( this._syntax ) };
#elif VISUAL_BASIC
			new StatementSyntax[] { LocalDeclarationStatement( this._syntax.Modifiers, this._syntax.Declarators ) };
#endif

		public override string ToString() => this._identifierName.ToString();
	}
}