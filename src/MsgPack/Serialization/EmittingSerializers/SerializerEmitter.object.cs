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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	partial class SerializerEmitter
	{
		private static readonly Type[] ConstructorParameterTypesWithoutCapabilities = { typeof( SerializationContext ) };
		private static readonly Type[] ConstructorParameterTypesWithCapabilities = { typeof( SerializationContext ), typeof( SerializerCapabilities ) };
		private static readonly Type[] CollectionConstructorParameterTypes = { typeof( SerializationContext ), typeof( PolymorphismSchema ), typeof( SerializerCapabilities ) };

		#region -- Dependent Serializer Management --

		private readonly Dictionary<SerializerFieldKey, SerializerFieldInfo> _serializers = new Dictionary<SerializerFieldKey, SerializerFieldInfo>();

		/// <summary>
		///		Regisgters <see cref="MessagePackSerializer{T}"/> of target type usage to the current emitting session.
		/// </summary>
		/// <param name="targetType">The type of the member to be serialized/deserialized.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method of the member to be serialized/deserialized.</param>
		/// <param name="dateTimeConversionMethod">The date time conversion method of the member to be serialized/deserialized.</param>
		/// <param name="polymorphismSchema">The schema for polymorphism support.</param>
		/// <param name="schemaRegenerationCodeProvider">The delegate to provide constructs to emit schema regeneration codes.</param>
		/// <returns><see cref="FieldBuilder"/>.</returns>
		public FieldBuilder RegisterSerializer(
			Type targetType,
			EnumMemberSerializationMethod enumMemberSerializationMethod,
			DateTimeMemberConversionMethod dateTimeConversionMethod,
			PolymorphismSchema polymorphismSchema,
			Func<IEnumerable<ILConstruct>> schemaRegenerationCodeProvider
		)
		{
			var key = new SerializerFieldKey( targetType, enumMemberSerializationMethod, dateTimeConversionMethod, polymorphismSchema );

			SerializerFieldInfo result;
			if ( !this._serializers.TryGetValue( key, out result ) )
			{
				result =
					new SerializerFieldInfo(
						this.DefineInitonlyField(
							"_serializer" + this._serializers.Count,
							typeof( MessagePackSerializer<> ).MakeGenericType( targetType )
						),
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

			return result.Field;
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

		#endregion -- Dependent Serializer Management --

		#region -- FieldInfo Cache Management --

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
		private readonly Dictionary<RuntimeFieldHandle, CachedFieldInfo> _cachedFieldInfos = new Dictionary<RuntimeFieldHandle, CachedFieldInfo>();
#else
		private readonly Dictionary<FieldInfo, CachedFieldInfo> _cachedFieldInfos = new Dictionary<FieldInfo, CachedFieldInfo>();
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

		/// <summary>
		///		Regisgters <see cref="FieldInfo"/> usage to the current emitting session.
		/// </summary>
		/// <param name="field">The <see cref="FieldInfo"/> to be registered.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public Action<TracingILGenerator, int> RegisterFieldCache( FieldInfo field )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			var key = field.FieldHandle;
#else
			var key = field;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

			CachedFieldInfo result;
			if ( !this._cachedFieldInfos.TryGetValue( key, out result ) )
			{
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );
				result =
					new CachedFieldInfo(
						field,
						this.DefineInitonlyField(
							"_field" + field.DeclaringType.Name + "_" + field.Name + this._cachedFieldInfos.Count,
							typeof( FieldInfo )
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

		#endregion -- FieldInfo Cache Management --

		#region -- MethodInfo Cache Management --

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
		private readonly Dictionary<RuntimeMethodHandle, CachedMethodBase> _cachedMethodBases = new Dictionary<RuntimeMethodHandle, CachedMethodBase>();
#else
		private readonly Dictionary<MethodBase, CachedMethodBase> _cachedMethodBases = new Dictionary<MethodBase, CachedMethodBase>();
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

		/// <summary>
		///		Regisgters <see cref="MethodBase"/> usage to the current emitting session.
		/// </summary>
		/// <param name="method">The <see cref="MethodBase"/> to be registered.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public Action<TracingILGenerator, int> RegisterMethodCache( MethodBase method )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			var key = method.MethodHandle;
#else
			var key = method;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

			CachedMethodBase result;
			if ( !this._cachedMethodBases.TryGetValue( key, out result ) )
			{
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );
				result =
					new CachedMethodBase(
						method,
						this.DefineInitonlyField(
							"_function" + method.DeclaringType.Name + "_" + method.Name + this._cachedMethodBases.Count,
							typeof( MethodBase )
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

#endregion -- MethodInfo Cache Management --

		#region -- UnpackingContext Management --

		private TypeBuilder _unpackingContextType;

		public void DefineUnpackingContext( string name, IList<KeyValuePair<string, Type>> fields, out Type type, out ConstructorInfo constructor )
		{
			this._unpackingContextType =
				 this._host.DefineType(
					this._typeBuilder.FullName + "_" + name,
					TypeAttributes.Class | TypeAttributes.UnicodeClass | TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit
				);
			var parameterTypes = fields.Select( kv => kv.Value ).ToArray();
			var ctor =
				this._unpackingContextType.DefineConstructor(
					MethodAttributes.Public,
					CallingConventions.Standard,
					parameterTypes
				);
			var il = this.GetILGenerator( ctor, parameterTypes );
			try
			{
				// call object.ctor
				il.EmitLdargThis();
				il.EmitCallConstructor( Metadata._Object.Ctor );

				for ( var i = 0; i < fields.Count; i++ )
				{
					var field = this._unpackingContextType.DefineField( fields[ i ].Key, fields[ i ].Value, FieldAttributes.Public );
					il.EmitLdargThis();
					il.EmitAnyLdarg( i + 1 );
					il.EmitStfld( field );
				}

				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
#if DEBUG
				SerializerDebugging.FlushTraceData();
#endif // DEBUG
			}

#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0
			type = this._unpackingContextType.CreateType();
#else
			type = this._unpackingContextType.CreateTypeInfo().AsType();
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0
			constructor = type.GetConstructors().Single();
		}

#endregion -- UnpackingContext Management --

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <param name="builder">The builder which implements actions initialization emit.</param>
		/// <param name="targetInfo">The information of the target.</param>
		/// <param name="schema">The <see cref="PolymorphismSchema"/> for this instance.</param>
		/// <param name="capabilities">The capabilities of the generating serializer.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public MessagePackSerializer CreateObjectInstance(
			AssemblyBuilderEmittingContext context,
			AssemblyBuilderSerializerBuilder builder,
			SerializationTarget targetInfo,
			PolymorphismSchema schema,
			SerializerCapabilities? capabilities
		)
		{
			return this.CreateObjectConstructor( context, builder, targetInfo, capabilities )( context.SerializationContext, schema );
		}

		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="builder">The builder which implements actions initialization emit.</param>
		/// <param name="targetInfo">The information of the targe.t</param>
		/// <param name="capabilities">The <see cref="SerializerCapabilities"/> for object serializer. <c>null</c> for other types.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reflection objects" )]
		public Func<SerializationContext, PolymorphismSchema, MessagePackSerializer> CreateObjectConstructor(
			AssemblyBuilderEmittingContext context,
			AssemblyBuilderSerializerBuilder builder,
			SerializationTarget targetInfo,
			SerializerCapabilities? capabilities
		)
		{
			var hasPackActions = targetInfo != null && !typeof( IPackable ).IsAssignableFrom( builder.TargetType );
			var hasUnpackActions = targetInfo != null && !typeof( IUnpackable ).IsAssignableFrom( builder.TargetType );
			var hasUnpackActionTables = hasUnpackActions && targetInfo.Members.Any( m => m.Member != null ); // Except tuples
#if FEATURE_TAP
			var hasPackAsyncActions = targetInfo != null && !typeof( IAsyncPackable ).IsAssignableFrom( builder.TargetType );
			var hasUnpackAsyncActions = targetInfo != null && !typeof( IAsyncUnpackable ).IsAssignableFrom( builder.TargetType );
			var hasUnpackAsyncActionTables = hasUnpackAsyncActions && targetInfo.Members.Any( m => m.Member != null ); // Except tuples
#endif // FEATURE_TAP
			// ReSharper disable RedundantDelegateCreation
			Func<bool, Func<ILConstruct>> packActionsInitialization =
				isAsync =>
					new Func<ILConstruct>( () => builder.EmitPackOperationListInitialization( context, targetInfo, isAsync ) );
			Func<bool, Func<ILConstruct>> packActionTableInitialization =
				isAsync =>
					new Func<ILConstruct>( () => builder.EmitPackOperationTableInitialization( context, targetInfo, isAsync ) );
			Func<ILConstruct> nullCheckerTableInitializtion = () => builder.EmitPackNullCheckerTableInitialization( context, targetInfo );
			Func<bool, Func<ILConstruct>> unpackActionsInitialization =
				isAsync => new Func<ILConstruct>( () => builder.EmitUnpackOperationListInitialization( context, targetInfo, isAsync ) );
			Func<bool, Func<ILConstruct>> unpackActionTableInitialization =
				isAsync => new Func<ILConstruct>( () => builder.EmitUnpackOperationTableInitialization( context, targetInfo, isAsync ) );
			// ReSharper restore RedundantDelegateCreation

			var contextfulConstructor =
				this.CreateConstructor(
					MethodAttributes.Public,
					ConstructorParameterTypesWithoutCapabilities,
					( type, il ) =>
						this.CreateContextfulObjectConstructor(
							context,
							type,
							capabilities,
							il,
							hasPackActions
								? packActionsInitialization( false )
								: default( Func<ILConstruct> ),
							hasPackActions
								? packActionTableInitialization( false )
								: default( Func<ILConstruct> ),
#if DEBUG
							!SerializerDebugging.UseLegacyNullMapEntryHandling &&
#endif // DEBUG
							hasPackActions
#if FEATURE_TAP
							|| hasPackAsyncActions
#endif // FEATURE_TAP
								? nullCheckerTableInitializtion
								: default( Func<ILConstruct> ),
							hasUnpackActions
								? unpackActionsInitialization( false )
								: default( Func<ILConstruct> ),
							hasUnpackActionTables
								? unpackActionTableInitialization( false )
								: default( Func<ILConstruct> ),
#if FEATURE_TAP
							hasPackAsyncActions && context.SerializationContext.SerializerOptions.WithAsync
								? packActionsInitialization( true )
								: default( Func<ILConstruct> ),
							hasPackAsyncActions && context.SerializationContext.SerializerOptions.WithAsync
								? packActionTableInitialization( true )
								: default( Func<ILConstruct> ),
							hasUnpackAsyncActions && context.SerializationContext.SerializerOptions.WithAsync
								? unpackActionsInitialization( true )
								: default( Func<ILConstruct> ),
							hasUnpackAsyncActionTables && context.SerializationContext.SerializerOptions.WithAsync
								? unpackActionTableInitialization( true )
								: default( Func<ILConstruct> ),
#endif // FEATURE_TAP
							( 
								hasUnpackActions
#if FEATURE_TAP
								|| hasUnpackAsyncActions
#endif // FEATURE_TAP
							) ? () => builder.EmitMemberListInitialization( context, targetInfo )
								: default( Func<ILConstruct> ),
							context.IsUnpackToUsed
								? () => builder.EmitUnpackToInitialization( context )
								: default( Func<ILConstruct> )
						)
				);
			this.CreateConstructor(
				MethodAttributes.Public,
				ReflectionAbstractions.EmptyTypes,
				( _, il ) => CreateDefaultObjectConstructor( contextfulConstructor, il )
			);

#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0
			var ctor = this._typeBuilder.CreateType().GetConstructor( ConstructorParameterTypesWithoutCapabilities );
#else
			var ctor = this._typeBuilder.CreateTypeInfo().GetConstructor( ConstructorParameterTypesWithoutCapabilities );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD2_0

#if DEBUG
			Contract.Assert( ctor != null, "ctor != null" );
#endif
			var actualFunc = ctor.CreateConstructorDelegate<Func<SerializationContext, MessagePackSerializer>>();
			return ( c, _ ) => actualFunc( c );
		}

		private static void CreateDefaultObjectConstructor( ConstructorBuilder contextfulConstructorBuilder, TracingILGenerator il )
		{
			/*
			 *	.ctor() : this(null)
			 *	{}
			 */
			// : this(null)
			il.EmitAnyLdarg( 0 );
			il.EmitLdnull();
			il.EmitCallConstructor( contextfulConstructorBuilder );
			il.EmitRet();
		}

		private void CreateContextfulObjectConstructor(
			AssemblyBuilderEmittingContext context,
			Type baseType,
			SerializerCapabilities? capabilities,
			TracingILGenerator il,
			Func<ILConstruct> packActionListInitializerProvider,
			Func<ILConstruct> packActionTableInitializerProvider,
			Func<ILConstruct> nullCheckerTableInitializerProvider,
			Func<ILConstruct> unpackActionListInitializerProvider,
			Func<ILConstruct> unpackActionTableInitializerProvider,
#if FEATURE_TAP
			Func<ILConstruct> packAsyncActionListInitializerProvider,
			Func<ILConstruct> packAsyncActionTableInitializerProvider,
			Func<ILConstruct> unpackAsyncActionListInitializerProvider,
			Func<ILConstruct> unpackAsyncActionTableInitializerProvider,
#endif // FEATURE_TAP
			Func<ILConstruct> memberNamesInitializerProvider,
			Func<ILConstruct> unpackToInitializerProvider
		)
		{
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
			// : base()
			il.EmitLdarg_0();
			il.EmitLdarg_1();
			if ( this._specification.TargetCollectionTraits.CollectionType == CollectionKind.NotCollection )
			{
				if ( capabilities.HasValue )
				{
					il.EmitAnyLdc_I4( ( int )capabilities.Value );

					il.EmitCallConstructor(
						baseType.GetRuntimeConstructor( ConstructorParameterTypesWithCapabilities )
					);
				}
				else
				{
					il.EmitCallConstructor(
						baseType.GetRuntimeConstructor( ConstructorParameterTypesWithoutCapabilities )
					);
				}
			}
			else
			{
				Contract.Assert( capabilities.HasValue );

				il.EmitCall( this._methodTable[ MethodName.RestoreSchema ] );
				il.EmitAnyLdc_I4( ( int )capabilities.Value );
				il.EmitCallConstructor(
					baseType.GetRuntimeConstructor( CollectionConstructorParameterTypes )
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

			if ( packActionListInitializerProvider != null )
			{
				packActionListInitializerProvider().Evaluate( il );
			}

#if FEATURE_TAP
			if ( packAsyncActionListInitializerProvider != null )
			{
				packAsyncActionListInitializerProvider().Evaluate( il );
			}
#endif // FEATURE_TAP

			if ( nullCheckerTableInitializerProvider != null )
			{
				nullCheckerTableInitializerProvider().Evaluate( il );
			}

			if ( packActionTableInitializerProvider != null )
			{
				packActionTableInitializerProvider().Evaluate( il );
			}

#if FEATURE_TAP
			if ( packAsyncActionTableInitializerProvider != null )
			{
				packAsyncActionTableInitializerProvider().Evaluate( il );
			}
#endif // FEATURE_TAP

			if ( unpackActionListInitializerProvider != null )
			{
				unpackActionListInitializerProvider().Evaluate( il );
			}

#if FEATURE_TAP
			if ( unpackAsyncActionListInitializerProvider != null )
			{
				unpackAsyncActionListInitializerProvider().Evaluate( il );
			}
#endif // FEATURE_TAP

			if ( unpackActionTableInitializerProvider != null )
			{
				unpackActionTableInitializerProvider().Evaluate( il );
			}

#if FEATURE_TAP
			if ( unpackAsyncActionTableInitializerProvider != null )
			{
				unpackAsyncActionTableInitializerProvider().Evaluate( il );
			}
#endif // FEATURE_TAP

			if ( memberNamesInitializerProvider != null )
			{
				memberNamesInitializerProvider().Evaluate( il );
			}

			if ( unpackToInitializerProvider != null )
			{
				unpackToInitializerProvider().Evaluate( il );
			}

			foreach ( var cachedDelegateInfo in context.GetCachedDelegateInfos() )
			{
				// this for stfld
				il.EmitLdargThis();

				var delegateType = cachedDelegateInfo.BackingField.FieldType.ResolveRuntimeType();

				// Declare backing field now.
				var field = context.GetDeclaredField( cachedDelegateInfo.BackingField.FieldName ).ResolveRuntimeField();

				if ( cachedDelegateInfo.IsThisInstance )
				{
					il.EmitLdargThis();
				}
				else
				{
					il.EmitLdnull();
				}

				// OK this should not be ldvirtftn because target is private or static.
				il.EmitLdftn( cachedDelegateInfo.TargetMethod.ResolveRuntimeMethod() );
				// call extern .ctor(Object, void*)
				il.EmitNewobj( delegateType.GetConstructors().Single() );

				il.EmitStfld( field );
			}

			il.EmitRet();
		}
	}
}
