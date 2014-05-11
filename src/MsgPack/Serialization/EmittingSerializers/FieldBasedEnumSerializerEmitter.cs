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
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		<see cref="EnumSerializerEmitter"/> using instance fields to hold serializers for target members.
	/// </summary>
	internal sealed class FieldBasedEnumSerializerEmitter : EnumSerializerEmitter
	{
		private readonly Type[] _constructorParameterTypes;
		private readonly ConstructorBuilder _contextConstructorBuilder;
		private readonly TypeBuilder _typeBuilder;
		private readonly MethodBuilder _packUnderlyingValueToMethodBuilder;
		private readonly MethodBuilder _unpackFromUnderlyingValueMethodBuilder;
		private readonly bool _isDebuggable;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="sequence">The sequence number to name new type.</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public FieldBasedEnumSerializerEmitter( ModuleBuilder host, int? sequence, Type targetType, bool isDebuggable )
		{
			Contract.Requires( host != null );
			Contract.Requires( targetType != null );

			this._constructorParameterTypes =
				new[]
				{
					typeof( SerializationContext ),
					typeof( EnumSerializationMethod )
				};

			string typeName =
#if !NETFX_35
 String.Join(
					Type.Delimiter.ToString( CultureInfo.InvariantCulture ),
					typeof( SerializerEmitter ).Namespace,
					"Generated",
					IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" + sequence
					);
#else
				String.Join(
					Type.Delimiter.ToString(),
					new string[]
					{
						typeof( SerializerEmitter ).Namespace,
						"Generated",
						IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" + sequence
					}
				);
#endif
			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}", typeName );
			this._typeBuilder =
				host.DefineType(
					typeName,
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.UnicodeClass | TypeAttributes.AutoLayout |
					TypeAttributes.BeforeFieldInit,
					typeof( EnumMessagePackSerializer<> ).MakeGenericType( targetType )
				);

			this._contextConstructorBuilder = this._typeBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				this._constructorParameterTypes
			);

			this._packUnderlyingValueToMethodBuilder =
				this._typeBuilder.DefineMethod(
					"PackUnderlyingValueTo",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					typeof( void ),
					new[] { typeof( Packer ), targetType }
				);

			this._unpackFromUnderlyingValueMethodBuilder =
				this._typeBuilder.DefineMethod(
					"UnpackFromUnderlyingValue",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					targetType,
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
				/*
				 *	.ctor( PackerCompatibilityOptions c, EnumSerializerMethod method ) 
				 *	  : base( c, method )
				 *	{
				 *	}
				 */
				var il = new TracingILGenerator( this._contextConstructorBuilder, TextWriter.Null, this._isDebuggable );
				// : base( c, method )
				il.EmitLdarg_0();
				il.EmitLdarg_1();
				il.EmitLdarg_2();

				Contract.Assert( this._typeBuilder.BaseType != null );

				il.EmitCallConstructor(
					this._typeBuilder.BaseType.GetConstructor(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, this._constructorParameterTypes, null
					)
				);

				il.EmitRet();
			}

			var ctor = this._typeBuilder.CreateType().GetConstructor( this._constructorParameterTypes );
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