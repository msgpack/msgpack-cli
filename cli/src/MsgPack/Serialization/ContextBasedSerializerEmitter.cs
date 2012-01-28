#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="SerializerEmitter"/> using <see cref="SerializationContext"/> to hold serializers for target members.
	/// </summary>
	internal sealed class ContextBasedSerializerEmitter : SerializerEmitter
	{
		private readonly Type _targetType;
		private readonly DynamicMethod _packToMethod;
		private readonly DynamicMethod _unpackFromMethod;
		private DynamicMethod _unpackToMethod;

		public ContextBasedSerializerEmitter( Type targetType )
		{
			this._targetType = targetType;

			this._packToMethod = new DynamicMethod( "PackToCore", null, new[] { typeof( SerializationContext ), typeof( Packer ), targetType } );
			this._unpackFromMethod = new DynamicMethod( "UnpackFromCore", targetType, new[] { typeof( SerializationContext ), typeof( Unpacker ) } );
		}

		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod );
			}

			return new TracingILGenerator( this._packToMethod, this.Trace );
		}

		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod );
			}

			return new TracingILGenerator( this._unpackFromMethod, this.Trace );
		}

		public override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( this._unpackToMethod == null )
			{
				this._unpackToMethod = new DynamicMethod( "UnpackToCore", null, new[] { typeof( SerializationContext ), typeof( Unpacker ), this._targetType } );
			}

			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackToMethod );
			}

			return new TracingILGenerator( this._unpackToMethod, this.Trace );
		}

		public override MessagePackSerializer<T> CreateInstance<T>( SerializationContext context )
		{
			var packTo = this._packToMethod.CreateDelegate( typeof( Action<SerializationContext, Packer, T> ) ) as Action<SerializationContext, Packer, T>;
			var unpackFrom = this._unpackFromMethod.CreateDelegate( typeof( Func<SerializationContext, Unpacker, T> ) ) as Func<SerializationContext, Unpacker, T>;
			var unpackTo = default( Action<SerializationContext, Unpacker, T> );
			if ( this._unpackToMethod != null )
			{
				unpackTo = this._unpackToMethod.CreateDelegate( typeof( Action<SerializationContext, Unpacker, T> ) ) as Action<SerializationContext, Unpacker, T>;
			}

			return new CallbackMessagePackSerializer<T>( context, packTo, unpackFrom, unpackTo );
		}

		public override Action<TracingILGenerator, int> RegisterSerializer( Type targetType )
		{
			return
				( il, contextIndex ) =>
				{
					il.EmitAnyLdarg( contextIndex );
					il.EmitAnyCall( Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType ) );
				};
		}
	}
}
