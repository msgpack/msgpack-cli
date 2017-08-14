#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Defines common features for emitters.
	/// </summary>
	internal sealed partial class SerializerEmitter
	{
		private readonly SerializerSpecification _specification;
		private readonly bool _isDebuggable;
		private readonly ModuleBuilder _host;
		private readonly TypeBuilder _typeBuilder;
		private readonly Dictionary<string, MethodBuilder> _methodTable;
		private readonly Dictionary<string, FieldBuilder> _fieldTable;

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerEmitter"/> class.
		/// </summary>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="specification">The specification of the serializer.</param>
		/// <param name="baseClass">Type of the base class of the serializer.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public SerializerEmitter( ModuleBuilder host, SerializerSpecification specification, Type baseClass, bool isDebuggable )
		{
			Contract.Requires( host != null );
			Contract.Requires( specification != null );
			Contract.Requires( baseClass != null );

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}", specification.SerializerTypeFullName );

			this._methodTable = new Dictionary<string, MethodBuilder>();
			this._fieldTable = new Dictionary<string, FieldBuilder>();
			this._specification = specification;
			this._host = host;
			this._typeBuilder =
				host.DefineType(
					specification.SerializerTypeFullName,
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.UnicodeClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit,
					baseClass
				);

#if DEBUG
			Contract.Assert( this._typeBuilder.BaseType != null, "baseType != null" );
#endif // DEBUG
			this._isDebuggable = isDebuggable;

#if DEBUG && !NET35 && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0
			if ( isDebuggable && SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump( host.Assembly as AssemblyBuilder );
			}
#endif // DEBUG && !NET35 && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0
		}

		#region -- Field --

		///  <summary>
		/// 	Regisgters specified field to the current emitting session.
		///  </summary>
		/// <param name="name">The name of the field.</param>
		/// <param name="type">The type of the field.</param>
		/// <returns><see cref="FieldBuilder"/>.</returns>
		public FieldBuilder RegisterField( string name, Type type )
		{
			FieldBuilder field;
			if ( !this._fieldTable.TryGetValue( name, out field ) )
			{
				field = this.DefineInitonlyField( name, type );
				this._fieldTable.Add( name, field );
			}

			return field;
		}

		private FieldBuilder DefineInitonlyField( string name, Type type )
		{
			return
				this._typeBuilder.DefineField(
					name,
					type,
					FieldAttributes.Private | FieldAttributes.InitOnly
				);
		}

#endregion -- Field --


#region -- Method --

		/// <summary>
		///		Gets the IL generator to implement specified method override.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <returns>
		///		The IL generator to implement specified method override.
		///		This value will not be <c>null</c>.
		/// </returns>
		public ILMethodConctext DefineOverrideMethod( string methodName )
		{
			return this.DefineMethod( methodName, true, false, null, ReflectionAbstractions.EmptyTypes );
		}

		/// <summary>
		///		Gets the IL generator to implement specified private instance method.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="isStatic"><c>true</c> for static method.</param>
		/// <param name="returnType">The type of the method return value.</param>
		/// <param name="parameterTypes">The types of the method parameters.</param>
		/// <returns>
		///		The IL generator to implement specified method override.
		///		This value will not be <c>null</c>.
		/// </returns>
		public ILMethodConctext DefinePrivateMethod( string methodName, bool isStatic, Type returnType, params Type[] parameterTypes )
		{
			return this.DefineMethod( methodName, false, isStatic, returnType, parameterTypes );
		}

		private ILMethodConctext DefineMethod( string methodName, bool isOverride, bool isStatic, Type returnType, Type[] parameterTypes )
		{
			if ( this._methodTable.ContainsKey( methodName ) )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Method {0} is already defined.", methodName ) );
			}

#if DEBUG
			Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
			MethodBuilder builder;
			if ( isOverride )
			{
				var baseMethod =
					this._typeBuilder.BaseType.GetRuntimeMethod(
						methodName
					);
				builder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameterTypes()
					);
				this._typeBuilder.DefineMethodOverride(
					builder,
					baseMethod
				);
			}
			else
			{
				builder =
					this._typeBuilder.DefineMethod(
						methodName,
						MethodAttributes.Private | MethodAttributes.HideBySig | ( isStatic ? MethodAttributes.Static : 0 ),
						isStatic ? CallingConventions.Standard : CallingConventions.HasThis,
						returnType,
						parameterTypes
					);
			}

			this._methodTable[ methodName ] = builder;

			return
				new ILMethodConctext(
					this.GetILGenerator( builder, parameterTypes ),
					builder,
					parameterTypes
				);
		}

		#endregion -- Method --

		#region -- IL Generation --

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameterTypes", Justification = "For DEBUG build." )]
		private TracingILGenerator GetILGenerator( ConstructorBuilder builder, Type[] parameterTypes )
		{
#if DEBUG
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.ILTraceWriter.WriteLine();
				SerializerDebugging.ILTraceWriter.WriteLine(
					"{2} {3} {0}::.ctor({1}) {4}",
					this._typeBuilder.Name,
					String.Join( ", ", parameterTypes.Select( t => t.GetFullName() ).ToArray() ),
					builder.Attributes.ToILString(),
					builder.CallingConvention.ToILString(),
#if !NET35 && !NET40
					builder.MethodImplementationFlags.ToILString()
#else
					String.Empty
#endif // !NET35 && !NET40
				);
			}
#endif // DEBUG

			return new TracingILGenerator( builder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameterTypes", Justification = "For DEBUG build." )]
		private TracingILGenerator GetILGenerator( MethodBuilder builder, Type[] parameterTypes )
		{
#if DEBUG
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.ILTraceWriter.WriteLine();
				SerializerDebugging.ILTraceWriter.WriteLine(
					"{4} {5} {3} {0}::{1}({2}) {6}",
					this._typeBuilder.Name,
					builder.Name,
					String.Join( ", ", parameterTypes.Select( t => t.GetFullName() ).ToArray() ),
					builder.ReturnType.GetFullName(),
					builder.Attributes.ToILString(),
					builder.CallingConvention.ToILString(),
#if !NET35 && !NET40
					builder.MethodImplementationFlags.ToILString()
#else
					String.Empty
#endif // !NET35 && !NET40
				);
			}
#endif // DEBUG

			return new TracingILGenerator( builder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

#endregion -- IL Generation --

#region -- Constructor --

		private ConstructorBuilder DefineConstructor( MethodAttributes attributes, params Type[] parameterTypes )
		{
			return this._typeBuilder.DefineConstructor( attributes, CallingConventions.Standard, parameterTypes );
		}

		private ConstructorBuilder CreateConstructor( MethodAttributes attributes, Type[] parameterTypes, Action<Type, TracingILGenerator> emitter )
		{
			var builder = this.DefineConstructor( attributes, parameterTypes );
			emitter( this._typeBuilder.BaseType, this.GetILGenerator( builder, parameterTypes ) );

#if DEBUG
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.FlushTraceData();
			}
#endif // DEBUG

			return builder;
		}

#endregion -- Constructor --
	}
}
