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
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class BinaryOperatorILConstruct : ContextfulILConstruct
	{
		private readonly string _operator;
		private readonly ILConstruct _left;
		private readonly ILConstruct _right;
		private readonly Action<TracingILGenerator, ILConstruct, ILConstruct> _operation;
		private readonly Action<TracingILGenerator, ILConstruct, ILConstruct, Label> _branchOperation;

		public BinaryOperatorILConstruct( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, ILConstruct, Label> branchOperation )
			: base( resultType )
		{
			ValidateContextTypeMatch( left, right );
			this._operator = @operator;
			this._left = left;
			this._right = right;
			this._operation = operation;
			this._branchOperation = branchOperation;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this._operation( il, this._left, this._right );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this._operation( il, this._left, this._right );
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		protected override void BranchCore( TracingILGenerator il, Label @else )
		{
			this._branchOperation( il, this._left, this._right, @else );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "BinaryOperator[{0}]: ({2} {1} {3})", this.ContextType, this._operator, this._left, this._right );
		}
	}
}