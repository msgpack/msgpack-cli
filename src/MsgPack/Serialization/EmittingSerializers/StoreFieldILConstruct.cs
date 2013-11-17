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
using System.Reflection;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class StoreFieldILConstruct : ContextfulILConstruct
	{
		private readonly ILConstruct _instance;
		private readonly ILConstruct _value;
		private readonly FieldInfo _field;

		public StoreFieldILConstruct( ILConstruct instance, FieldInfo field, ILConstruct value )
			: base( typeof( void ) )
		{
			this._instance = instance;
			this._field = field;
			this._value = value;
		}

		public override void Evaluate( TracingILGenerator il )
		{
			this.StoreValue( il );
		}

		public override void StoreValue( TracingILGenerator il )
		{
			il.TraceWriteLine( "// Stor->: {0}", this );
			if ( this._instance != null )
			{
				this._instance.LoadValue( il, this._instance.ContextType.GetIsValueType() );
			}

			this._value.LoadValue( il, false );

			il.EmitStfld( this._field );
			il.TraceWriteLine( "// ->Stor: {0}", this );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "StoreField[void]: {0}", this._field );
		}
	}
}