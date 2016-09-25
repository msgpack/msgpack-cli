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

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
			this._parameter = SyntaxFactory.Parameter( this._identifierName.Identifier ).WithType( Syntax.ToTypeSyntax( contextType ) );
			this._identifierName = expression;
		}

		public override ParameterSyntax AsParameter() => this._parameter;

		public override ExpressionSyntax AsExpression() => this._identifierName;
	}
}