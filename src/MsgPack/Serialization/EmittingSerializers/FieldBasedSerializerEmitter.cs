#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
		private static readonly Type[] ConstructorParameterTypes = { typeof( SerializationContext ) };
		private static readonly Type[] CollectionConstructorParameterTypes = { typeof( SerializationContext ), typeof( PolymorphismSchema ) };

		private readonly Dictionary<SerializerFieldKey, SerializerFieldInfo> _serializers;
		private readonly Dictionary<RuntimeFieldHandle, CachedFieldInfo> _cachedFieldInfos;
		private readonly Dictionary<RuntimeMethodHandle, CachedMethodBase> _cachedMethodBases;
		private readonly ConstructorBuilder _defaultConstructorBuilder;
		private readonly ConstructorBuilder _contextConstructorBuilder;
		private readonly TypeBuilder _typeBuilder;
		private MethodBuilder _packMethodBuilder;
		private MethodBuilder _unpackFromMethodBuilder;
		private MethodBuilder _unpackToMethodBuilder;
		private MethodBuilder _addItemMethodBuilder;
		private MethodBuilder _createInstanceMethodBuilder;
		private MethodBuilder _restoreSchemaMethodBuilder;
		private readonly CollectionTraits _traits;
		private readonly bool _isDebuggable;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="specification">The specification of the serializer.</param>
		/// <param name="baseClass">Type of the base class of the serializer.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public FieldBasedSerializerEmitter( ModuleBuilder host, SerializerSpecification specification, Type baseClass, bool isDebuggable )
		{
			Contract.Requires( host != null );
			Contract.Requires( specification != null );
			Contract.Requires( baseClass != null );

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}", specification.SerializerTypeFullName );
			this._typeBuilder =
				host.DefineType(
					specification.SerializerTypeFullName,
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.UnicodeClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit,
					baseClass
				);

			this._defaultConstructorBuilder = this._typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes );
			this._contextConstructorBuilder = this._typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, ConstructorParameterTypes );

			this._traits = specification.TargetCollectionTraits;
			var baseType = this._typeBuilder.BaseType;
#if DEBUG
			Contract.Assert( baseType != null, "baseType != null" );
#endif
			this._serializers = new Dictionary<SerializerFieldKey, SerializerFieldInfo>();
			this._cachedFieldInfos = new Dictionary<RuntimeFieldHandle, CachedFieldInfo>();
			this._cachedMethodBases = new Dictionary<RuntimeMethodHandle, CachedMethodBase>();
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
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, "PackToCore" );
			}

			if ( this._packMethodBuilder == null )
			{
#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
				var baseMethod =
					this._typeBuilder.BaseType.GetMethod(
						"PackToCore",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
					);
				this._packMethodBuilder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameters().Select( p => p.ParameterType ).ToArray()
					);
				this._typeBuilder.DefineMethodOverride(
					this._packMethodBuilder,
					baseMethod
				);
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
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, "UnpackFromCore" );
			}

			if ( this._unpackFromMethodBuilder == null )
			{
#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
				var baseMethod =
					this._typeBuilder.BaseType.GetMethod(
						"UnpackFromCore",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
					);
				this._unpackFromMethodBuilder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameters().Select( p => p.ParameterType ).ToArray()
					);
				this._typeBuilder.DefineMethodOverride(
					this._unpackFromMethodBuilder,
					baseMethod
				);
			}

			return new TracingILGenerator( this._unpackFromMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement CreateInstance(int) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement CreateInstance(int) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetCreateInstanceMethodILGenerator( MethodInfo declaration )
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, "CreateInstance" );
			}

			if ( this._createInstanceMethodBuilder == null )
			{
#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
				var baseMethod =
					this._typeBuilder.BaseType.GetMethod(
						"CreateInstance",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
					);
				this._createInstanceMethodBuilder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameters().Select( p => p.ParameterType ).ToArray()
					);
				this._typeBuilder.DefineMethodOverride(
					this._createInstanceMethodBuilder,
					baseMethod
				);
			}

			return new TracingILGenerator( this._createInstanceMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetAddItemMethodILGenerator( MethodInfo declaration )
		{
			if ( this._addItemMethodBuilder == null )
			{
#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
				var baseMethod =
					this._typeBuilder.BaseType.GetMethod(
						"AddItem",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
					);
				this._addItemMethodBuilder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameters().Select( p => p.ParameterType ).ToArray()
					);
				this._typeBuilder.DefineMethodOverride(
					this._addItemMethodBuilder,
					baseMethod
				);
			}

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._addItemMethodBuilder );
			}

			return new TracingILGenerator( this._addItemMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
		}

		/// <summary>
		///		Gets the IL generator to implement private static RestoreSchema() method.
		/// </summary>
		/// <returns>
		///		The IL generator to implement RestoreSchema() static method.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetRestoreSchemaMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, "RestoreSchema" );
			}

			if ( this._restoreSchemaMethodBuilder == null )
			{
				this._restoreSchemaMethodBuilder =
					this._typeBuilder.DefineMethod(
						"RestoreSchema",
						MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
						CallingConventions.Standard,
						typeof( PolymorphismSchema ),
						ReflectionAbstractions.EmptyTypes
					);
			}

			return new TracingILGenerator( this._restoreSchemaMethodBuilder, SerializerDebugging.ILTraceWriter, this._isDebuggable );
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
				SerializerDebugging.TraceEvent( "{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, "UnpackToCore" );
			}

			if ( this._unpackToMethodBuilder == null )
			{
#if DEBUG
				Contract.Assert( this._typeBuilder.BaseType != null, "this._typeBuilder.BaseType != null" );
#endif // DEBUG
				var baseMethod =
					this._typeBuilder.BaseType.GetMethod(
						"UnpackToCore",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
					);
				this._unpackToMethodBuilder =
					this._typeBuilder.DefineMethod(
						baseMethod.Name,
						( baseMethod.Attributes | MethodAttributes.Final ) & ( ~MethodAttributes.Abstract ),
						baseMethod.CallingConvention,
						baseMethod.ReturnType,
						baseMethod.GetParameters().Select( p => p.ParameterType ).ToArray()
					);
				this._typeBuilder.DefineMethodOverride(
					this._unpackToMethodBuilder,
					baseMethod
				);
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reflection objects" )]
		public override Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>> CreateConstructor<T>()
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
					if ( this._traits.CollectionType == CollectionKind.NotCollection )
					{
						il.EmitCallConstructor(
							this._typeBuilder.BaseType.GetConstructor(
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ConstructorParameterTypes, null
							)
						);
					}
					else
					{
						il.EmitCall( this._restoreSchemaMethodBuilder );
						il.EmitCallConstructor(
							this._typeBuilder.BaseType.GetConstructor(
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CollectionConstructorParameterTypes, null
							)
						);
					}

					// this._serializerN = context.GetSerializer<T>();
					foreach ( var entry in this._serializers )
					{
						var targetType = Type.GetTypeFromHandle( entry.Key.TypeHandle );
						MethodInfo getMethod = Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType );

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
						else if ( DateTimeMessagePackSerializerHelpers.IsDateTime( targetType ) )
						{
							il.EmitLdarg_1();
							il.EmitAnyLdc_I4( ( int )entry.Key.DateTimeConversionMethod );
							il.EmitCall( Metadata._DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethodMethod );
							il.EmitBox( typeof( DateTimeConversionMethod ) );
						}
						else
						{
							if ( entry.Key.PolymorphismSchema == null )
							{
								il.EmitLdnull();
							}
							else
							{
								entry.Value.SchemaProvider( il );
							}
						}

						il.EmitCallvirt( getMethod );
						il.EmitStfld( entry.Value.Field );
					}

					foreach ( var entry in this._cachedFieldInfos )
					{
						il.EmitLdarg_0();
						il.EmitLdtoken( entry.Value.Target );
						il.EmitLdtoken( entry.Value.Target.DeclaringType );
						il.EmitCall( Metadata._FieldInfo.GetFieldFromHandle );
						il.EmitStfld( entry.Value.StorageFieldBuilder );
					}

					foreach ( var entry in this._cachedMethodBases )
					{
						il.EmitLdarg_0();
						il.EmitLdtoken( entry.Value.Target );
						il.EmitLdtoken( entry.Value.Target.DeclaringType );
						il.EmitCall( Metadata._MethodBase.GetMethodFromHandle );
						il.EmitStfld( entry.Value.StorageFieldBuilder );
					}

					il.EmitRet();
				}
			}

			var ctor = this._typeBuilder.CreateType().GetConstructor( ConstructorParameterTypes );
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			var schemaParameter = Expression.Parameter( typeof( PolymorphismSchema ), "schema" );
#if DEBUG
			Contract.Assert( ctor != null, "ctor != null" );
#endif
			return
				Expression.Lambda<Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>>>(
					Expression.New(
						ctor,
						contextParameter
					),
					contextParameter,
					schemaParameter
				).Compile();
		}

		/// <summary>
		///		Regisgters <see cref="MessagePackSerializer{T}"/> of target type usage to the current emitting session.
		/// </summary>
		/// <param name="targetType">The type of the member to be serialized/deserialized.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method of the member to be serialized/deserialized.</param>
		/// <param name="dateTimeConversionMethod">The date time conversion method of the member to be serialized/deserialized.</param>
		/// <param name="polymorphismSchema">The schema for polymorphism support.</param>
		/// <param name="schemaRegenerationCodeProvider">The delegate to provide constructs to emit schema regeneration codes.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public override Action<TracingILGenerator, int> RegisterSerializer(
			Type targetType,
			EnumMemberSerializationMethod enumMemberSerializationMethod,
			DateTimeMemberConversionMethod dateTimeConversionMethod,
			PolymorphismSchema polymorphismSchema,
			Func<IEnumerable<ILConstruct>> schemaRegenerationCodeProvider
		)
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = new SerializerFieldKey( targetType, enumMemberSerializationMethod, dateTimeConversionMethod, polymorphismSchema );

			SerializerFieldInfo result;
			if ( !this._serializers.TryGetValue( key, out result ) )
			{
				result =
					new SerializerFieldInfo(
						this._typeBuilder.DefineField(
							"_serializer" + this._serializers.Count,
							typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
							FieldAttributes.Private | FieldAttributes.InitOnly ),
						il =>
						{
							foreach ( var construct in schemaRegenerationCodeProvider() )
							{
								construct.Evaluate( il );
							}
						}
					);

				this._serializers.Add( key, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result.Field );
				};
		}

		public override Action<TracingILGenerator, int> RegisterField( FieldInfo field )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = field.FieldHandle;

			CachedFieldInfo result;
			if ( !this._cachedFieldInfos.TryGetValue( key, out result ) )
			{
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );
				result =
					new CachedFieldInfo(
						field,
						this._typeBuilder.DefineField(
							"_field" + field.DeclaringType.Name + "_" + field.Name + this._cachedFieldInfos.Count,
							typeof( FieldInfo ),
							FieldAttributes.Private | FieldAttributes.InitOnly
						)
					);
				this._cachedFieldInfos.Add( key, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result.StorageFieldBuilder );
				};
		}

		public override Action<TracingILGenerator, int> RegisterMethod( MethodBase method )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			var key = method.MethodHandle;

			CachedMethodBase result;
			if ( !this._cachedMethodBases.TryGetValue( key, out result ) )
			{
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );
				result =
					new CachedMethodBase(
						method,
						this._typeBuilder.DefineField(
							"_function" + method.DeclaringType.Name + "_" + method.Name + this._cachedMethodBases.Count,
							typeof( FieldInfo ),
							FieldAttributes.Private | FieldAttributes.InitOnly 
						)
					);
				this._cachedMethodBases.Add( key, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result.StorageFieldBuilder );
				};
		}

		private struct SerializerFieldInfo
		{
			public readonly FieldBuilder Field;
			public readonly Action<TracingILGenerator> SchemaProvider;

			public SerializerFieldInfo( FieldBuilder field, Action<TracingILGenerator> schemaProvider )
			{
				this.Field = field;
				this.SchemaProvider = schemaProvider;
			}
		}

		private struct CachedFieldInfo
		{
			public readonly FieldBuilder StorageFieldBuilder;
			public readonly FieldInfo Target;

			public CachedFieldInfo( FieldInfo target, FieldBuilder storageFieldBuilder )
			{
				this.Target = target;
				this.StorageFieldBuilder = storageFieldBuilder;
			}
		}

		private struct CachedMethodBase
		{
			public readonly FieldBuilder StorageFieldBuilder;
			public readonly MethodBase Target;

			public CachedMethodBase( MethodBase target, FieldBuilder storageFieldBuilder )
			{
				this.Target = target;
				this.StorageFieldBuilder = storageFieldBuilder;
			}
		}
	}
}
