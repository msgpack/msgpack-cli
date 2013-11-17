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
using System.Diagnostics.Contracts;
using System.Globalization;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class VariableILConstruct : ContextfulILConstruct
	{
		private readonly bool _isLocal;
		private int _index;
		private readonly string _name;

		public VariableILConstruct( string name, Type valueType )
			: base( valueType )
		{
			Contract.Assert( name != null );
			this._isLocal = true;
			this._name = name;
			this._index = -1;
		}

		public VariableILConstruct( string name, Type valueType, int parameterIndex )
			: base( valueType )
		{
			Contract.Assert( name != null );
			this._isLocal = false;
			this._name = name;
			this._index = parameterIndex;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			if ( this._isLocal && this._index < 0 )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );

				this._index = il.DeclareLocal( this.ContextType, this._name ).LocalIndex;

				il.TraceWriteLine( "// ->Eval: {0}", this );
			}
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			this.Evaluate( il );
			il.TraceWriteLine( "// Load->: {0}", this );
			if ( this.ContextType.GetIsValueType() && shouldBeAddress )
			{
				if ( this._isLocal )
				{
					il.EmitAnyLdloca( this._index );
				}
				else
				{
					il.EmitAnyLdarga( this._index );
				}
			}
			else
			{
				if ( this._isLocal )
				{
					il.EmitAnyLdloc( this._index );
				}
				else
				{
					il.EmitAnyLdarg( this._index );
				}
			}
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		public override void StoreValue( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			if ( !this._isLocal )
			{
				throw new InvalidOperationException( "Cannot overwrite argument." );
			}

			il.EmitAnyStloc( this._index );
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "Variable[{0}]: [{2}{3}]{1}({0})", this.ContextType, this._name, this._isLocal ? "local" : "arg", this._index );
		}
	}
}