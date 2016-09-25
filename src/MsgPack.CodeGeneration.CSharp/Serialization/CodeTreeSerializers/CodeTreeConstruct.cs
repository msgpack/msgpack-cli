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
using System.Globalization;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal abstract class CodeTreeConstruct : ICodeConstruct
	{
		public TypeDefinition ContextType { get; }

		public virtual bool IsExpression => false;

		public virtual bool IsStatement => false;

		protected CodeTreeConstruct( TypeDefinition contextType )
		{
			this.ContextType = contextType;
		}

		public virtual ParameterSyntax AsParameter()
		{
			throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as parameter declaration.", this )
				);
		}

		public virtual ExpressionSyntax AsExpression()
		{
			throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as expression.", this )
				);
		}

		public virtual IEnumerable<StatementSyntax> AsStatements()
		{
			throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as statements.", this )
				);
		}

		public static CodeTreeConstruct Expression( TypeDefinition contextType, ExpressionSyntax expression )
			=> new ExpressionCodeTreeConstruct( contextType, expression );

		public static CodeTreeConstruct Statement( StatementSyntax statement )
			=> new StatementCodeTreeConstruct( new[] { statement } );

		public static CodeTreeConstruct Statement( IEnumerable<StatementSyntax> statements )
			=> new StatementCodeTreeConstruct( statements );

		public static CodeTreeConstruct Variable( TypeDefinition contextType, VariableDeclarationSyntax syntax )
			=> new VariableCodeTreeConstruct( contextType, syntax );

		public static CodeTreeConstruct Parameter( TypeDefinition contextType, IdentifierNameSyntax syntax )
			=> new ParameterCodeTreeConstruct( contextType, syntax );
	}
}