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

#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;
#endif

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal sealed class ParameterCodeTreeConstruct : CodeTreeConstruct
	{
		private readonly IdentifierNameSyntax _identifierName;
		private readonly ParameterSyntax _parameter;

		public override bool IsExpression => true;

		public ParameterCodeTreeConstruct( TypeDefinition contextType, IdentifierNameSyntax expression )
			: base( contextType )
		{
			this._identifierName = expression;
			this._parameter =
#if CSHARP
				SyntaxFactory.Parameter( this._identifierName.Identifier )
#elif VISUAL_BASIC
				SyntaxFactory.Parameter( ModifiedIdentifier( this._identifierName.Identifier ) )
#endif
				.WithType( Syntax.ToTypeSyntax( contextType ) );
		}

		public override ParameterSyntax AsParameter() => this._parameter;

		public override ExpressionSyntax AsExpression() => this._identifierName;

		public override string ToString() => this._parameter.ToString();
	}
}