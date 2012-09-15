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
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
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


		/// <summary>
		///		Initializes a new instance of the <see cref="ContextBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		public ContextBasedSerializerEmitter( Type targetType )
		{
			Contract.Requires( targetType != null );

			this._targetType = targetType;

			this._packToMethod = new DynamicMethod( "PackToCore", null, new[] { typeof( SerializationContext ), typeof( Packer ), targetType } );
			this._unpackFromMethod = new DynamicMethod( "UnpackFromCore", targetType, new[] { typeof( SerializationContext ), typeof( Unpacker ) } );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._packToMethod );
			}

			return new TracingILGenerator( this._packToMethod, this.Trace );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod );
			}

			return new TracingILGenerator( this._unpackFromMethod, this.Trace );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
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

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		/// </returns>
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

		/// <summary>
		///		Regisgter using <see cref="MessagePackSerializer{T}"/> target type to the current emitting session.
		/// </summary>
		/// <param name="targetType">Type to be serialized/deserialized.</param>
		/// <returns>
		///   <see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder.
		///		This value will not be <c>null</c>.
		/// </returns>
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
