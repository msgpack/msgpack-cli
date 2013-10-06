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
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		<see cref="SerializerEmitter"/> using instance fields to hold serializers for target members.
	/// </summary>
	internal sealed class FieldBasedSerializerEmitter : SerializerEmitter
	{
		private static readonly Type[] _constructorParameterTypes = new[] { typeof( SerializationContext ) };
		private static readonly Type[] _serializerConstructorParameterTypes = new[] { typeof( PackerCompatibilityOptions ) };

		private readonly Dictionary<RuntimeTypeHandle, FieldBuilder> _serializers;
		private readonly ConstructorBuilder _defaultConstructorBuilder;
		private readonly ConstructorBuilder _contextConstructorBuilder;
		private readonly TypeBuilder _typeBuilder;
		private readonly MethodBuilder _packMethodBuilder;
		private readonly MethodBuilder _unpackFromMethodBuilder;
		private MethodBuilder _unpackToMethodBuilder;
		private readonly bool _isDebuggable;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="sequence">The sequence number to name new type.</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public FieldBasedSerializerEmitter( ModuleBuilder host, int? sequence, Type targetType, bool isDebuggable )
			: base()
		{
			Contract.Requires( host != null );
			Contract.Requires( targetType != null );

			string typeName =
#if !NETFX_35
 String.Join(
					Type.Delimiter.ToString(),
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
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.UnicodeClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit,
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType )
				);

			this._defaultConstructorBuilder = this._typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes );
			this._contextConstructorBuilder = this._typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, _constructorParameterTypes );

			this._packMethodBuilder =
				this._typeBuilder.DefineMethod(
					"PackToCore",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					typeof( void ),
					new Type[] { typeof( Packer ), targetType }
				);

			this._unpackFromMethodBuilder =
				this._typeBuilder.DefineMethod(
					"UnpackFromCore",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					targetType,
					UnpackFromCoreParameterTypes
				);

			this._typeBuilder.DefineMethodOverride( this._packMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._packMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._typeBuilder.DefineMethodOverride( this._unpackFromMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._unpackFromMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._serializers = new Dictionary<RuntimeTypeHandle, FieldBuilder>();
			this._isDebuggable = isDebuggable;
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public sealed override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._packMethodBuilder );
			}

			return new TracingILGenerator( this._packMethodBuilder, this.Trace, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public sealed override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackFromMethodBuilder );
			}

			return new TracingILGenerator( this._unpackFromMethodBuilder, this.Trace, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
		public sealed override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine();
				this.Trace.WriteLine( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackToMethodBuilder );
			}

			if ( this._unpackToMethodBuilder == null )
			{
				this._unpackToMethodBuilder =
					this._typeBuilder.DefineMethod(
						"UnpackToCore",
						MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final,
						CallingConventions.HasThis,
						null,
						new Type[] { typeof( Unpacker ), this._unpackFromMethodBuilder.ReturnType }
					);

				this._typeBuilder.DefineMethodOverride( this._unpackToMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._unpackToMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			}

			return new TracingILGenerator( this._unpackToMethodBuilder, this.Trace, this._isDebuggable );
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
		public sealed override MessagePackSerializer<T> CreateInstance<T>( SerializationContext context )
		{
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<T>>>(
					Expression.New(
						this.Create(),
						contextParameter
					),
					contextParameter
				).Compile()( context );
		}


		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		private ConstructorInfo Create()
		{
			if ( !this._typeBuilder.IsCreated() )
			{
				/*
				 *	.ctor() : this(null)
				 *	{}
				 */
				/*
				 *	.ctor( SerializationContext context ) 
				 *	  : base( ( context ?? SerializationContext.Default ).CompabilityOptions.PackerCompatibilityOptions )
				 *	{
				 *		this._serializer0 = context.GetSerializer<T0>();
				 *		this._serializer1 = context.GetSerializer<T1>();
				 *		this._serializer2 = context.GetSerializer<T2>();
				 *			:
				 *	}
				 */
				// default
				{
					var il = this._defaultConstructorBuilder.GetILGenerator();
					// : this(null)
					il.Emit( OpCodes.Ldarg_0 );
					il.Emit( OpCodes.Ldnull );
					il.Emit( OpCodes.Call, this._defaultConstructorBuilder );
					il.Emit( OpCodes.Ret );
				}

				// context
				{
					var il = this._contextConstructorBuilder.GetILGenerator();
					// : base()
					il.Emit( OpCodes.Ldarg_0 );
					// ( context ?? SerializationContext.Default )
					var nullValue = il.DefineLabel();
					var endExpression = il.DefineLabel();
					il.Emit( OpCodes.Ldarg_1 );
					il.Emit( OpCodes.Brfalse_S, nullValue );
					il.Emit( OpCodes.Ldarg_1 );
					il.Emit( OpCodes.Br_S, endExpression );
					il.MarkLabel( nullValue );
					il.Emit( OpCodes.Call, Metadata._SerializationContext.DefaultProperty.GetGetMethod() );
					il.MarkLabel( endExpression );
					il.Emit( OpCodes.Call, Metadata._SerializationContext.CompatibilityOptionsProperty.GetGetMethod() );
					il.Emit( OpCodes.Call, Metadata._SerializationCompatibilityOptions.PackerCompatibilityOptionsProperty.GetGetMethod() );
					il.Emit(
						OpCodes.Call,
						this._typeBuilder.BaseType.GetConstructor(
							BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, _serializerConstructorParameterTypes, null 
						) 
					);

					// this._serializerN = context.GetSerializer<T>();
					foreach ( var entry in this._serializers )
					{
						var targetType = Type.GetTypeFromHandle( entry.Key );
						var getMethod = Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType );
						il.Emit( OpCodes.Ldarg_0 );
						il.Emit( OpCodes.Ldarg_1 );
						il.Emit( OpCodes.Callvirt, getMethod );
						il.Emit( OpCodes.Stfld, entry.Value );
					}

					il.Emit( OpCodes.Ret );
				}
			}

			return this._typeBuilder.CreateType().GetConstructor( _constructorParameterTypes );
		}

		/// <summary>
		///		Regisgter using <see cref="MessagePackSerializer{T}"/> target type to the current emitting session.
		/// </summary>
		/// <param name="targetType">Type to be serialized/deserialized.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder.
		///		This value will not be <c>null</c>.
		/// </returns>
		public sealed override Action<TracingILGenerator, int> RegisterSerializer( Type targetType )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			FieldBuilder result;
			if ( !this._serializers.TryGetValue( targetType.TypeHandle, out result ) )
			{
				result = this._typeBuilder.DefineField( "_serializer" + this._serializers.Count, typeof( MessagePackSerializer<> ).MakeGenericType( targetType ), FieldAttributes.Private | FieldAttributes.InitOnly );
				this._serializers.Add( targetType.TypeHandle, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result );
				};
		}
	}
}
