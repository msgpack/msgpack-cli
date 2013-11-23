#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Globalization;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class CodeDomConstruct : ICodeConstruct
	{
		private readonly Type _contextType;

		public Type ContextType
		{
			get { return this._contextType; }
		}

		public virtual bool IsArgument { get { return false; } }

		public virtual bool IsExpression { get { return false; } }

		public virtual bool IsStatement { get { return false; } }

		public virtual CodeParameterDeclarationExpression AsParameter()
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as parameter declaration.", this ) 
			);
		}

		public virtual CodeArgumentReferenceExpression AsArgument()
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as argument reference expression.", this )
			);
		}

		public virtual CodeExpression AsExpression()
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as expression.", this )
			);
		}

		public virtual IEnumerable<CodeStatement> AsStatements()
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as statements.", this )
			);
		}

		public virtual void AddStatements( CodeStatementCollection collection )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "Cannot get '{0}' as statements.", this )
			);
		}

		protected CodeDomConstruct( Type contextType )
		{
			this._contextType = contextType;
		}

		public static ParameterCodeDomConstruct Parameter( Type type, string name )
		{
			return new ParameterCodeDomConstruct( type, name );
		}

		public static StatementCodeDomConstruct Statement( params CodeStatement[] statement )
		{
			return new StatementCodeDomConstruct( statement );
		}

		public static StatementCodeDomConstruct Statement( IEnumerable<CodeStatement> statements )
		{
			return new StatementCodeDomConstruct( statements );
		}

		public static ExpressionCodeDomConstruct Expression( Type contextType, CodeExpression expression )
		{
			return new ExpressionCodeDomConstruct( contextType, expression );
		}

		public static CodeDomConstruct Variable( Type type, string name )
		{
			return new VariableCodeDomConstruct( type, name );
		}
	}
}