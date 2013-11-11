#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Holds reusable(temporal) local variable info.
	/// </summary>
	internal sealed class LocalVariableHolder
	{
		private readonly TracingILGenerator _il;

		private readonly Dictionary<Type, LocalBuilder> _serializingValues = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetSerializingValue( Type type )
		{
			LocalBuilder result;
			if ( !this._serializingValues.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, "serializingValue" );
				this._serializingValues[ type ] = result;
			}

			return result;
		}

		public LocalVariableHolder( TracingILGenerator il )
		{
			this._il = il;
		}
	}
}