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

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class ExpressionCodeDomConstruct : CodeDomConstruct
	{
		private readonly CodeExpression _dom;

		public override bool IsExpression
		{
			get { return true; }
		}

		public override bool IsStatement
		{
			get { return true; }
		}

		public override CodeExpression AsExpression()
		{
			return this._dom;
		}

		public override System.Collections.Generic.IEnumerable<CodeStatement> AsStatements()
		{
			yield return new CodeExpressionStatement( this._dom );
		}

		public override void AddStatements( CodeStatementCollection collection )
		{
			collection.Add( new CodeExpressionStatement( this._dom ) );
		}

		public ExpressionCodeDomConstruct( Type contextType, CodeExpression dom )
			: base( contextType )
		{
			this._dom = dom;
		}
	}
}