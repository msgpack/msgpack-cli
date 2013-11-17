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
	internal sealed class ConditionalILConstruct : ILConstruct
	{
		private readonly ILConstruct _condition;
		private readonly ILConstruct _thenExpression;
		private readonly ILConstruct _elseExpression;

		public ConditionalILConstruct( ILConstruct condition, ILConstruct thenExpression, ILConstruct elseExpression )
			: base( thenExpression.ContextType )
		{
			if ( condition.ContextType != typeof( bool ) )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "condition must be boolean: {0}", condition ), "condition" );
			}

			if ( elseExpression != null && elseExpression.ContextType != thenExpression.ContextType )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "elseExpression type must be '{0}' but '{1}':{2}", thenExpression.ContextType, elseExpression.ContextType, elseExpression ), "elseExpression" );
			}

			this._condition = condition;
			this._thenExpression = thenExpression;
			this._elseExpression = elseExpression;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this.DoConditionalInstruction(
				il,
				() => this._thenExpression.Evaluate( il ),
				() => this._elseExpression.Evaluate( il )
				);
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this.DoConditionalInstruction(
				il,
				() => this._thenExpression.LoadValue( il, shouldBeAddress ),
				() => this._elseExpression.LoadValue( il, shouldBeAddress )
				);
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		public override void StoreValue( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			this.DoConditionalInstruction(
				il,
				() => this._thenExpression.StoreValue( il ),
				() => this._elseExpression.StoreValue( il )
				);
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		private void DoConditionalInstruction(
			TracingILGenerator il, Action onThen, Action onElse
			)
		{
			if ( this._elseExpression != null )
			{
				var @else = il.DefineLabel( "ELSE" );
				var endIf = il.DefineLabel( "END_IF" );
				this._condition.Branch( il, @else );
				onThen();
				if ( !this._thenExpression.IsTerminating )
				{
					il.EmitBr( endIf );
				}

				il.MarkLabel( @else );
				onElse();
				il.MarkLabel( endIf );
			}
			else
			{
				var endIf = il.DefineLabel( "END_IF" );
				this._condition.Branch( il, endIf );
				onThen();
				il.MarkLabel( endIf );
			}
		}

		public override string ToString()
		{
			return
				String.Format(
					CultureInfo.InvariantCulture,
					"Condition[{0}]: ({1}) ? ({2}) : ({3})",
					this.ContextType,
					this._condition,
					this._thenExpression,
					this._elseExpression
					);
		}
	}
}