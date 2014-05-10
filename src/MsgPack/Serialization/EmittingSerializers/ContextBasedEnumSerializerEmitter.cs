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
		private readonly DynamicMethod _unpackFromUnderlyingValueMethod;
		public ContextBasedEnumSerializerEmitter( Type targetType )
		{
			this._targetType = targetType;
			// NOTE: first argument is dummy to align with FieldBased(its 0th arg is this reference).
			this._packUnderyingValueToMethod =
				new DynamicMethod( "PackUnderyingValue", null, new[] { typeof( SerializationContext ), typeof( Packer ), this._targetType } );
			this._unpackFromUnderlyingValueMethod =
				new DynamicMethod( "UnpackFromUnderlyingValue", this._targetType, new[] { typeof( SerializationContext ), typeof( MessagePackObject ) } );
		}

		public override TracingILGenerator GetPackUnderyingValueToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._packUnderyingValueToMethod );
			}

			return new TracingILGenerator( this._packUnderyingValueToMethod, SerializerDebugging.ILTraceWriter );
		}

		public override TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromUnderlyingValueMethod );
			}

			return new TracingILGenerator( this._unpackFromUnderlyingValueMethod, SerializerDebugging.ILTraceWriter );
		}

		public override Func<SerializationContext, EnumSerializationMethod, MessagePackSerializer<T>> CreateConstructor<T>()
		{
			Contract.Assert( this._targetType == typeof( T ) );

			var packUnderyingValueTo =
				this._packUnderyingValueToMethod.CreateDelegate( typeof( Action<SerializationContext, Packer, T> ) ) as
					Action<SerializationContext, Packer, T>;
			var unpackFromUnderlyingValue =
				this._unpackFromUnderlyingValueMethod.CreateDelegate( typeof( Func<SerializationContext, MessagePackObject, T> ) ) as
					Func<SerializationContext, MessagePackObject, T>;

			var targetType = typeof( CallbackEnumMessagePackSerializer<> ).MakeGenericType( typeof( T ) );

			return
				( context, method ) =>
					Activator.CreateInstance(
						targetType,
						context,
						method,
						packUnderyingValueTo,
						unpackFromUnderlyingValue
					) as MessagePackSerializer<T>;
		}
	}
}