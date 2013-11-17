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
	internal class UnaryOperatorILConstruct : ContextfulILConstruct
	{
		private readonly string _operator;
		private readonly ILConstruct _input;
		private readonly Action<TracingILGenerator, ILConstruct> _operation;
		private readonly Action<TracingILGenerator, ILConstruct, Label> _branchOperation;

		public UnaryOperatorILConstruct( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation )
			: this( @operator, input, operation, ( il, i, @else ) => BranchWithOperationResult( i, operation, il, @else ) )
		{
		}

		public UnaryOperatorILConstruct( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, Label> branchOperation )
			: base( input.ContextType )
		{
			this._operator = @operator;
			this._input = input;
			this._operation = operation;
			this._branchOperation = branchOperation;
		}

		private static void BranchWithOperationResult( ILConstruct input, Action<TracingILGenerator, ILConstruct> operation, TracingILGenerator il, Label @else )
		{
			operation( il, input );
			il.EmitBrfalse( @else );
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this._operation( il, this._input );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			this._operation( il, this._input );
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		protected override void BranchCore( TracingILGenerator il, Label @else )
		{
			this._branchOperation( il, this._input, @else );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "UnaryOperator[{0}]: ({1} {2})", this.ContextType, this._operator, this._input );
		}
	}
}