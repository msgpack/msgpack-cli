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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		<see cref="SerializerEmitter"/> using instance fields to hold serializers for target members.
	/// </summary>
	internal sealed class FieldBasedSerializerEmitter : SerializerEmitter
	{
		private static readonly Type[] _constructorParameterTypes = { typeof( SerializationContext ) };

		private readonly Dictionary<SerializerFieldKey, FieldBuilder> _serializers;
		private readonly Dictionary<RuntimeFieldHandle, FieldBuilder> _fieldInfos;
		private readonly Dictionary<RuntimeMethodHandle, FieldBuilder> _methodBases;
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
		{
			Contract.Requires( host != null );
			Contract.Requires( targetType != null );

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
					new[] { typeof( Packer ), targetType }
				);

			this._unpackFromMethodBuilder =
				this._typeBuilder.DefineMethod(
					"UnpackFromCore",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig,
					CallingConventions.HasThis,
					targetType,
					UnpackFromCoreParameterTypes
				);

			var baseType = this._typeBuilder.BaseType;
#if DEBUG
			Contract.Assert( baseType != null, "baseType != null" );
#endif
			this._typeBuilder.DefineMethodOverride( this._packMethodBuilder, baseType.GetMethod( this._packMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._typeBuilder.DefineMethodOverride( this._unpackFromMethodBuilder, baseType.GetMethod( this._unpackFromMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._serializers = new Dictionary<SerializerFieldKey, FieldBuilder>();
			this._fieldInfos = new Dictionary<RuntimeFieldHandle, FieldBuilder>();
			this._methodBases = new Dictionary<RuntimeMethodHandle, FieldBuilder>();
			this._isDebuggable = isDebuggable;

#if !SILVERLIGHT && !NETFX_35
			if ( isDebuggable && SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump( host.Assembly as AssemblyBuilder );
			}
#endif
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._packMethodBuilder );
			}

			return new TracingILGenerator( this._packMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackFromMethodBuilder );
			}

			return new TracingILGenerator( this._unpackFromMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
		public override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackToMethodBuilder );
			}

			if ( this._unpackToMethodBuilder == null )
			{
				this._unpackToMethodBuilder =
					this._typeBuilder.DefineMethod(
						"UnpackToCore",
						MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final,
						CallingConventions.HasThis,
						null,
						new[] { typeof( Unpacker ), this._unpackFromMethodBuilder.ReturnType }
					);

#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif
				this._typeBuilder.DefineMethodOverride( this._unpackToMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._unpackToMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			}

			return new TracingILGenerator( this._unpackToMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		public override Func<SerializationContext, MessagePackSerializer<T>> CreateConstructor<T>()
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
					il.Emit( OpCodes.Call, this._contextConstructorBuilder );
					il.Emit( OpCodes.Ret );
				}

				// context
				{
					var il = new TracingILGenerator( this._contextConstructorBuilder, TextWriter.Null, this._isDebuggable );
					// : base()
					il.EmitLdarg_0();
					il.EmitLdarg_1();
#if DEBUG
					Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif
					il.EmitCallConstructor(
						this._typeBuilder.BaseType.GetConstructor(
							BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, _constructorParameterTypes, null
						)
					);

					// this._serializerN = context.GetSerializer<T>();
					foreach ( var entry in this._serializers )
					{
						var targetType = Type.GetTypeFromHandle( entry.Key.TypeHandle );
						MethodInfo getMethod;
						if ( !targetType.GetIsEnum() )
						{
							getMethod = Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType );
						}
						else
						{
							getMethod = Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType );
						}
						il.EmitLdarg_0();
						il.EmitLdarg_1();
						if ( targetType.GetIsEnum() )
						{
							il.EmitLdarg_1();
							il.EmitTypeOf( targetType );
							il.EmitAnyLdc_I4( ( int )entry.Key.EnumSerializationMethod );
							il.EmitCall( Metadata._EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethodMethod );
							il.EmitBox( typeof( EnumSerializationMethod ) );
						}

						il.EmitCallvirt( getMethod );
						il.EmitStfld( entry.Value );

					}

					foreach ( var entry in this._fieldInfos )
					{
						il.EmitLdarg_0();
						il.EmitLdtoken( FieldInfo.GetFieldFromHandle( entry.Key ) );
						il.EmitCall( Metadata._FieldInfo.GetFieldFromHandle );
						il.EmitStfld( entry.Value );
					}

					foreach ( var entry in this._methodBases )
					{
						il.EmitLdarg_0();
						il.EmitLdtoken( MethodBase.GetMethodFromHandle( entry.Key ) );
						il.EmitCall( Metadata._MethodBase.GetMethodFromHandle );
						il.EmitStfld( entry.Value );
					}

					il.EmitRet();
				}
			}

			var ctor = this._typeBuilder.CreateType().GetConstructor( _constructorParameterTypes );
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
#if DEBUG
			Contract.Assert( ctor != null, "ctor != null" );
#endif
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<T>>>(
					Expression.New(
						ctor,
						contextParameter
					),
					contextParameter
				).Compile();
		}

		/// <summary>
		///		Regisgter using <see cref="MessagePackSerializer{T}"/> target type to the current emitting session.
		/// </summary>
		/// <param name="targetType">The type of the member to be serialized/deserialized.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method of the member to be serialized/deserialized.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override Action<TracingILGenerator, int> RegisterSerializer( Type targetType, EnumMemberSerializationMethod enumMemberSerializationMethod )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = new SerializerFieldKey( targetType, enumMemberSerializationMethod );

			FieldBuilder result;
			if ( !this._serializers.TryGetValue( key, out result ) )
			{
				result = this._typeBuilder.DefineField( "_serializer" + this._serializers.Count, typeof( MessagePackSerializer<> ).MakeGenericType( targetType ), FieldAttributes.Private | FieldAttributes.InitOnly );
				this._serializers.Add( key, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result );
				};
		}

		public override Action<TracingILGenerator, int> RegisterField( FieldInfo field )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = field.FieldHandle;

			FieldBuilder result;
			if ( !this._fieldInfos.TryGetValue( key, out result ) )
			{
				result = this._typeBuilder.DefineField( "_field" + field.DeclaringType.Name + "_" + field.Name + this._fieldInfos.Count, typeof( FieldInfo ), FieldAttributes.Private | FieldAttributes.InitOnly );
				this._fieldInfos.Add( key, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result );
				};
		}

		public override Action<TracingILGenerator, int> RegisterMethod( MethodBase method )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = method.MethodHandle;

			FieldBuilder result;
			if ( !this._methodBases.TryGetValue( key, out result ) )
			{
				result = this._typeBuilder.DefineField( "_function" + method.DeclaringType.Name + "_" + method.Name + this._methodBases.Count, typeof( FieldInfo ), FieldAttributes.Private | FieldAttributes.InitOnly );
				this._methodBases.Add( key, result );
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
