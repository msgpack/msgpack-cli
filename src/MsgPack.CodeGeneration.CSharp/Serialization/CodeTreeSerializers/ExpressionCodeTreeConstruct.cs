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

#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
#endif

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal sealed class ExpressionCodeTreeConstruct : CodeTreeConstruct
	{
		private readonly ExpressionSyntax _expression;

		public override bool IsExpression => true;

		public ExpressionCodeTreeConstruct( TypeDefinition contextType, ExpressionSyntax expression )
			: base( contextType )
		{
			this._expression = expression;
		}

		public override ExpressionSyntax AsExpression() => this._expression;

		public override IEnumerable<StatementSyntax> AsStatements() => new StatementSyntax[] { SyntaxFactory.ExpressionStatement( this._expression ) };

		public override string ToString() => this._expression.ToString();
	}
}