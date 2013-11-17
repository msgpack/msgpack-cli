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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class SequenceILConstruct : ILConstruct
	{
		private readonly ILConstruct[] _statements;

		public SequenceILConstruct( Type contextType, IEnumerable<ILConstruct> statements )
			: base( contextType )
		{
			this._statements = statements.ToArray();
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );

			foreach ( var statement in this._statements )
			{
				statement.Evaluate( il );
			}

			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "Sequence[{0}]", this._statements.Length );
		}
	}
}