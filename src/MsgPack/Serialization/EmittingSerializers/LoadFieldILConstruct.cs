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
	internal class LoadFieldILConstruct : ContextfulILConstruct
	{
		private readonly ILConstruct _instance;
		private readonly FieldInfo _field;

		public LoadFieldILConstruct( ILConstruct instance, FieldInfo field )
			: base( field.FieldType )
		{
			this._instance = instance;
			this._field = field;
		}

		public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			il.TraceWriteLine( "// Load->: {0}", this );
			if ( this._instance != null )
			{
				this._instance.LoadValue( il, this._instance.ContextType.GetIsValueType() );
			}

			if ( shouldBeAddress )
			{
				il.EmitLdflda( this._field );
			}
			else
			{
				il.EmitLdfld( this._field );
			}
			il.TraceWriteLine( "// ->Load: {0}", this );
		}

		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "LoadField[{0}]: {1}", this.ContextType, this._field );
		}
	}
}