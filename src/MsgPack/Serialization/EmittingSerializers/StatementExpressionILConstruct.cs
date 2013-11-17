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
using System.Globalization;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class StatementExpressionILConstruct : ContextfulILConstruct
	{
		private bool _isBound;
		private readonly ILConstruct _binding;
		private readonly ILConstruct _expression;

		public StatementExpressionILConstruct( ILConstruct binding, ILConstruct expression )
			: base( expression.ContextType )
		{
			this._binding = binding;
			this._expression = expression;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this.Evaluate( il, false );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this.Evaluate( il, shouldBeAddress );
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		private void Evaluate( TracingILGenerator il, bool shouldBeAddress )
		{
			if ( !this._isBound )
			{
				this._binding.Evaluate( il );
				this._isBound = true;
			}

			this._expression.LoadValue( il, shouldBeAddress );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "Bind[{0}]: {1} context: {2}", this.ContextType, this._binding, this._expression );
		}
	}
}