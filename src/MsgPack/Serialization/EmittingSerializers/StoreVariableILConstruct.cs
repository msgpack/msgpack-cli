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
	internal class StoreVariableILConstruct : ILConstruct
	{
		private readonly ILConstruct _variable;
		private readonly ILConstruct _value;

		public StoreVariableILConstruct( ILConstruct variable, ILConstruct value )
			: base( typeof( void ) )
		{
			this._variable = variable;
			this._value = value;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			if ( this._value != null )
			{
				this._value.LoadValue( il, false );
			}

			this._variable.StoreValue( il );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "StoreVariable: {0} = (context)", this._variable );
		}
	}
}