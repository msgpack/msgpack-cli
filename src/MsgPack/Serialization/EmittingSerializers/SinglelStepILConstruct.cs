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
	internal class SinglelStepILConstruct : ILConstruct
	{
		private readonly string _description;
		private readonly Action<TracingILGenerator> _instruction;
		private readonly bool _isTerminating;

		public override bool IsTerminating
		{
			get { return this._isTerminating; }
		}

		public SinglelStepILConstruct( Type contextType, string description, bool isTerminating, Action<TracingILGenerator> instruction )
			: base( contextType )
		{
			this._description = description;
			this._instruction = instruction;
			this._isTerminating = isTerminating;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this._instruction( il );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this._instruction( il );
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		public override void StoreValue( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			this._instruction( il );
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "Instruction[{0}]: {1}", this.ContextType, this._description );
		}
	}
}