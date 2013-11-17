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
using System.Reflection;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class InvocationILConsruct : ContextfulILConstruct
	{
		private readonly ILConstruct _target;
		private readonly MethodBase _method;
		private readonly IEnumerable<ILConstruct> _arguments;

		public InvocationILConsruct( MethodInfo method, ILConstruct target, IEnumerable<ILConstruct> arguments )
			: base( method.ReturnType )
		{
			if ( method.IsStatic )
			{
				if ( target != null )
				{
					throw new ArgumentException(
						String.Format( CultureInfo.CurrentCulture, "target must be null for static method '{0}'", method )
						);
				}
			}
			else
			{
				if ( target == null )
				{
					throw new ArgumentException(
						String.Format( CultureInfo.CurrentCulture, "target must not be null for instance method '{0}'", method )
						);
				}
			}

			this._method = method;
			this._target = target;
			this._arguments = arguments;
		}

		public InvocationILConsruct( ConstructorInfo ctor, ILConstruct target, IEnumerable<ILConstruct> arguments )
			: base( ctor.DeclaringType )
		{
			if ( ctor.DeclaringType.GetIsValueType() )
			{
				if ( target == null )
				{
					throw new ArgumentException(
						String.Format( CultureInfo.CurrentCulture, "target must not be null for expression type constructor '{0}'", ctor )
						);
				}
			}

			this._method = ctor;
			this._target = target;
			this._arguments = arguments;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Eval->: {0}", this );
			this.Invoke( il );
			il.TraceWriteLine( "// ->Eval: {0}", this );
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			this.Invoke( il );
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		public override void StoreValue( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			this.Invoke( il );
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		private void Invoke( TracingILGenerator il )
		{
			ConstructorInfo asConsctructor;
			if ( ( asConsctructor = this._method as ConstructorInfo ) != null )
			{
				if ( asConsctructor.DeclaringType.GetIsValueType() )
				{
					this._target.LoadValue( il, true );
					foreach ( var argument in this._arguments )
					{
						argument.LoadValue( il, false );
					}

					il.EmitCallConstructor( asConsctructor );

					// For compatibility to ref type.
					this._target.LoadValue( il, false );
				}
				else
				{
					foreach ( var argument in this._arguments )
					{
						argument.LoadValue( il, false );
					}

					il.EmitNewobj( asConsctructor );
				}
			}
			else
			{
				// method
				if ( !this._method.IsStatic )
				{
					this._target.LoadValue( il, this._target.ContextType.GetIsValueType() );
				}

				foreach ( var argument in this._arguments )
				{
					argument.LoadValue( il, false );
				}

				if ( this._method.IsStatic || this._target.ContextType.GetIsValueType() )
				{
					il.EmitCall( this._method as MethodInfo );
				}
				else
				{
					il.EmitCallvirt( this._method as MethodInfo );
				}
			}
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "Invoke[{0}]: {1}", this.ContextType, this._method );
		}
	}
}