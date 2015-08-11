#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		<see cref="EnumSerializerEmitter"/> using instance fields to hold serializers for target members.
	/// </summary>
	internal sealed class FieldBasedEnumSerializerEmitter : EnumSerializerEmitter
	{
		private static readonly Type[] ContextConstructorParameterTypes =
				{
					typeof( SerializationContext )
				};
		private static readonly Type[] ContextAndEnumSerializationMethodConstructorParameterTypes =
				{
					typeof( SerializationContext ),
					typeof( EnumSerializationMethod )
				};
		private readonly ConstructorBuilder _contextConstructorBuilder;
		private readonly ConstructorBuilder _contextAndEnumSerializationMethodConstructorBuilder;
		private readonly EnumSerializationMethod _defaultEnumSerializationMethod;
		private readonly TypeBuilder _typeBuilder;
		private readonly MethodBuilder _packUnderlyingValueToMethodBuilder;
		private readonly MethodBuilder _unpackFromUnderlyingValueMethodBuilder;
		private readonly bool _isDebuggable;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="context">A <see cref="SerializationContext"/>.</param>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="specification">The specification of the serializer.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public FieldBasedEnumSerializerEmitter( SerializationContext context, ModuleBuilder host, SerializerSpecification specification, bool isDebuggable )
		{
			Contract.Requires( host != null );
			Contract.Requires( specification != null );

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}", specification.SerializerTypeFullName );
			this._typeBuilder =
				host.DefineType(
					specification.SerializerTypeFullName,
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.UnicodeClass | TypeAttributes.AutoLayout |
					TypeAttributes.BeforeFieldInit,
					typeof( EnumMessagePackSerializer<> ).MakeGenericType( specification.TargetType )
				);

			this._contextConstructorBuilder = 
				this._typeBuilder.DefineConstructor(
					MethodAttributes.Public,
					CallingConventions.Standard,
					ContextConstructorParameterTypes
				);
			this._defaultEnumSerializationMethod = context.EnumSerializationMethod;
			this._contextAndEnumSerializationMethodConstructorBuilder =
				this._typeBuilder.DefineConstructor(
					MethodAttributes.Public,
					CallingConventions.Standard,
					ContextAndEnumSerializationMethodConstructorParameterTypes
				);

			this._packUnderlyingValueToMethodBuilder =
				this._typeBuilder.DefineMethod(
					"PackUnderlyingValueTo",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					typeof( void ),
					new[] { typeof( Packer ), specification.TargetType }
				);

			this._unpackFromUnderlyingValueMethodBuilder =
				this._typeBuilder.DefineMethod(
					"UnpackFromUnderlyingValue",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					specification.TargetType,
					UnpackFromUnderlyingValueParameterTypes
				);

			var baseType = this._typeBuilder.BaseType;
#if DEBUG
			Contract.Assert( baseType != null, "baseType != null" );
#endif
			this._typeBuilder.DefineMethodOverride(
				this._packUnderlyingValueToMethodBuilder,
				baseType.GetMethod(
					this._packUnderlyingValueToMethodBuilder.Name,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
				)
			);
			this._typeBuilder.DefineMethodOverride(
				this._unpackFromUnderlyingValueMethodBuilder,
				baseType.GetMethod(
					this._unpackFromUnderlyingValueMethodBuilder.Name,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
				)
			);
			this._isDebuggable = isDebuggable;

#if !SILVERLIGHT && !NETFX_35
			if ( isDebuggable && SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump( host.Assembly as AssemblyBuilder );
			}
#endif
		}

		public override TracingILGenerator GetPackUnderyingValueToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._packUnderlyingValueToMethodBuilder );
			}

			return new TracingILGenerator( this._packUnderlyingValueToMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		public override TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackFromUnderlyingValueMethodBuilder );
			}

			return new TracingILGenerator( this._unpackFromUnderlyingValueMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}
		
		public override Func<SerializationContext, EnumSerializationMethod, MessagePackSerializer<T>> CreateConstructor<T>()
		{
			if ( !this._typeBuilder.IsCreated() )
			{

				Contract.Assert( this._typeBuilder.BaseType != null );
				
				/*
				 *	.ctor( SerializationContext c ) 
				 *	  : this( c, DEFAULT_METHOD )
				 *	{
				 *	}
				 */
				var il1 = new TracingILGenerator( this._contextConstructorBuilder, TextWriter.Null, this._isDebuggable );
				// : this( c, DEFAULT_METHOD )
				il1.EmitLdarg_0();
				il1.EmitLdarg_1();
				il1.EmitAnyLdc_I4( ( int ) this._defaultEnumSerializationMethod );

				il1.EmitCallConstructor( this._contextAndEnumSerializationMethodConstructorBuilder );

				il1.EmitRet();

				/*
				 *	.ctor( SerializationContext c, EnumSerializerMethod method ) 
				 *	  : base( c, method )
				 *	{
				 *	}
				 */
				var il2 = new TracingILGenerator( this._contextAndEnumSerializationMethodConstructorBuilder, TextWriter.Null, this._isDebuggable );
				// : base( c, method )
				il2.EmitLdarg_0();
				il2.EmitLdarg_1();
				il2.EmitLdarg_2();

				il2.EmitCallConstructor(
					this._typeBuilder.BaseType.GetConstructor(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ContextAndEnumSerializationMethodConstructorParameterTypes, null
					)
				);

				il2.EmitRet();
			}

			var ctor = this._typeBuilder.CreateType().GetConstructor( ContextAndEnumSerializationMethodConstructorParameterTypes );
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			var methodParameter = Expression.Parameter( typeof( EnumSerializationMethod ), "method" );
#if DEBUG
			Contract.Assert( ctor != null, "ctor != null" );
#endif
			return
				Expression.Lambda<Func<SerializationContext, EnumSerializationMethod, MessagePackSerializer<T>>>(
					Expression.New(
						ctor,
						contextParameter,
						methodParameter
					),
					contextParameter,
					methodParameter
				).Compile();
		}
	}
}