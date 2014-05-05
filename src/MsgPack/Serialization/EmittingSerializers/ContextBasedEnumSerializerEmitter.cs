#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal sealed class ContextBasedEnumSerializerEmitter : EnumSerializerEmitter
	{
		private readonly Type _targetType;
		private readonly DynamicMethod _packUnderyingValueToMethod;
		private readonly DynamicMethod _getUnderlyingValueStringMethod;
		private readonly DynamicMethod _unpackFromUnderlyingValueMethod;
		private readonly DynamicMethod _parseMethod;

		public ContextBasedEnumSerializerEmitter( Type targetType )
		{
			this._targetType = targetType;
			this._packUnderyingValueToMethod =
				new DynamicMethod( "PackUnderyingValue", null, new[] { typeof( Packer ), this._targetType } );
			this._getUnderlyingValueStringMethod =
				new DynamicMethod( "GetUnderlyingValueString", typeof( String ), new[] { this._targetType } );
			this._unpackFromUnderlyingValueMethod =
				new DynamicMethod( "UnpackFromUnderlyingValue", this._targetType, new[] { typeof( MessagePackObject ) } );
			this._parseMethod =
				new DynamicMethod( "Parse", this._targetType, new[] { typeof( String ) } );
		}

		public override TracingILGenerator GetPackUnderyingValueToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._packUnderyingValueToMethod );
			}

			return new TracingILGenerator( this._packUnderyingValueToMethod, SerializerDebugging.ILTraceWriter );
		}

		public override TracingILGenerator GetGetUnderlyingValueStringMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._getUnderlyingValueStringMethod );
			}

			return new TracingILGenerator( this._getUnderlyingValueStringMethod, SerializerDebugging.ILTraceWriter );
		}

		public override TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromUnderlyingValueMethod );
			}

			return new TracingILGenerator( this._unpackFromUnderlyingValueMethod, SerializerDebugging.ILTraceWriter );
		}

		public override TracingILGenerator GetParseMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._parseMethod );
			}

			return new TracingILGenerator( this._parseMethod, SerializerDebugging.ILTraceWriter );
		}

		public override Func<SerializationContext, EnumSerializationMethod, IList<string>, IList<T>, EnumMessagePackSerializer<T>> CreateConstructor<T>()
		{
			Contract.Assert( this._targetType == typeof( T ) );

			var packUnderyingValueTo =
				this._packUnderyingValueToMethod.CreateDelegate( typeof( Action<Packer, T> ) ) as
					Action<Packer, T>;
			var getUnderlyingValueString =
				this._getUnderlyingValueStringMethod.CreateDelegate( typeof( Func<T, String> ) ) as
					Func<T, String>;
			var unpackFromUnderlyingValue =
				this._unpackFromUnderlyingValueMethod.CreateDelegate( typeof( Func<MessagePackObject, T> ) ) as
					Func<MessagePackObject, T>;
			var parsed =
				this._parseMethod.CreateDelegate( typeof( Func<String, T> ) ) as
					Func<String, T>;

			return
				(context, method, names, values  )=>
					new CallbackEnumMessagePackSerializer<T>(
						context.CompatibilityOptions.PackerCompatibilityOptions,
						method,
						names,
						values,
						packUnderyingValueTo,
						getUnderlyingValueString,
						unpackFromUnderlyingValue,
						parsed
					);
		}
	}
}