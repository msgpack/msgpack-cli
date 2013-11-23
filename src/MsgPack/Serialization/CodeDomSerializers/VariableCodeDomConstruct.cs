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

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class VariableCodeDomConstruct : CodeDomConstruct
	{
		private readonly CodeTypeReference _type;
		private readonly string _name;

		public CodeVariableDeclarationStatement Declaration
		{
			get { return new CodeVariableDeclarationStatement( this._type, this._name, new CodeDefaultValueExpression( this._type ) ); }
		}

		public CodeVariableReferenceExpression Reference
		{
			get { return new CodeVariableReferenceExpression( this._name ); }
		}

		public override bool IsStatement
		{
			get { return true; }
		}

		public override IEnumerable<CodeStatement> AsStatements()
		{
			yield return this.Declaration;
		}

		public override void AddStatements( CodeStatementCollection collection )
		{
			collection.Add( this.Declaration );
		}

		public override bool IsExpression
		{
			get { return true; }
		}

		public override CodeExpression AsExpression()
		{
			return this.Reference;
		}

		public VariableCodeDomConstruct( Type type, string name )
			: base( type )
		{
			this._type = new CodeTypeReference( type );
			this._name = name;
		}
	}
}