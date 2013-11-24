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
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal sealed class AndConditionILConstruct : ILConstruct
	{
		private readonly IList<ILConstruct> _expressions;

		public AndConditionILConstruct( IList<ILConstruct> expressions )
			: base( typeof( bool ) )
		{
			if ( expressions.Count == 0 )
			{
				throw new ArgumentException( "Empty expressions.", "expressions" );
			}

			if ( expressions.Any( c => c.ContextType != typeof( bool ) ) )
			{
				throw new ArgumentException( "An argument expressions cannot contains non boolean expression.", "expressions" );
			}

			this._expressions = expressions;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this.EvaluateCore( il );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this.EvaluateCore( il );
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		private void EvaluateCore( TracingILGenerator il )
		{
			for ( int i = 0; i < this._expressions.Count; i++ )
			{
				this._expressions[ i ].LoadValue( il, false );

				if ( i > 0 )
				{
					il.EmitAnd();
				}
			}
		}

		public override void Branch( TracingILGenerator il, Label @else )
		{
			il.TraceWriteLine( "// Brnc->: {0}", this );
			foreach ( var expression in this._expressions )
			{
				expression.LoadValue( il, false );
				il.EmitBrfalse( @else );
			}

			il.TraceWriteLine( "// ->Brnc: {0}", this );
		}

		public override string ToString()
		{
			return
				String.Format(
				// ReSharper disable CoVariantArrayConversion
					CultureInfo.InvariantCulture, "And[{0}]: ({1})", this.ContextType, String.Join( ", ", this._expressions.Select( e => e.ToString() ).ToArray() )
				// ReSharper restore CoVariantArrayConversion
					);
		}
	}
}