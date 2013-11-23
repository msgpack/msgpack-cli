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
using System.Linq;

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class StatementCodeDomConstruct : CodeDomConstruct
	{
		private readonly CodeStatement[] _statements;

		public override bool IsStatement
		{
			get { return true; }
		}

		public override IEnumerable<CodeStatement> AsStatements()
		{
			return this._statements;
		}

		public override void AddStatements( CodeStatementCollection collection )
		{
			collection.AddRange( this._statements );
		}

		public StatementCodeDomConstruct( IEnumerable<CodeStatement> statements )
			: base( typeof( void ) )
		{
			this._statements = statements.ToArray();
		}
	}
}