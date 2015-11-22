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
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	partial class SerializerEmitter
	{
		private static readonly Type[] ConstructorParameterTypes = { typeof( SerializationContext ) };
		private static readonly Type[] CollectionConstructorParameterTypes = { typeof( SerializationContext ), typeof( PolymorphismSchema ) };

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

#if !CORE_CLR
		private readonly Dictionary<RuntimeFieldHandle, CachedFieldInfo> _cachedFieldInfos = new Dictionary<RuntimeFieldHandle, CachedFieldInfo>();
#else
		private readonly Dictionary<FieldInfo, CachedFieldInfo> _cachedFieldInfos = new Dictionary<FieldInfo, CachedFieldInfo>();
#endif // !CORE_CLR

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
#if !CORE_CLR
			var key = field.FieldHandle;
#else
			var key = field;
#endif // !CORE_CLR

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

#if !CORE_CLR
		private readonly Dictionary<RuntimeMethodHandle, CachedMethodBase> _cachedMethodBases = new Dictionary<RuntimeMethodHandle, CachedMethodBase>();
#else
		private readonly Dictionary<MethodBase, CachedMethodBase> _cachedMethodBases = new Dictionary<MethodBase, CachedMethodBase>();
#endif // !CORE_CLR

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
#if !CORE_CLR
			var key = method.MethodHandle;
#else
			var key = method;
#endif // CORE_CLR

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

		private readonly Dictionary<string, FieldBuilder> _unpackingContextFields = new Dictionary<string, FieldBuilder>();

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
					this._unpackingContextFields.Add( fields[ i ].Key, field );
					il.EmitLdargThis();
					il.EmitAnyLdarg( i + 1 );
					il.EmitStfld( field );
				}

				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
				SerializerDebugging.FlushTraceData();
			}

#if !CORE_CLR
			type = this._unpackingContextType.CreateType();
#else
			type = this._unpackingContextType.CreateTypeInfo().AsType();
#endif // !CORE_CLR
			constructor = type.GetConstructors().Single();
		}

#endregion -- UnpackingContext Management --

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <param name="builder">The builder which implements actions initialization emit.</param>
		/// <param name="targetInfo">The information of the target.</param>
		/// <param name="schema">The <see cref="PolymorphismSchema"/> for this instance.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public MessagePackSerializer<T> CreateObjectInstance<T>( AssemblyBuilderEmittingContext context, AssemblyBuilderSerializerBuilder<T> builder, SerializationTarget targetInfo, PolymorphismSchema schema )
		{
			return this.CreateObjectConstructor( context, builder, targetInfo )( context.SerializationContext, schema );
		}

		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="builder">The builder which implements actions initialization emit.</param>
		/// <param name="targetInfo">The information of the target</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reflection objects" )]
		public Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>> CreateObjectConstructor<T>( AssemblyBuilderEmittingContext context, AssemblyBuilderSerializerBuilder<T> builder, SerializationTarget targetInfo )
		{
			var hasPackActions = targetInfo != null && !typeof( IPackable ).IsAssignableFrom( typeof( T ) );
			var hasUnackActions = targetInfo != null && !typeof( IUnpackable ).IsAssignableFrom( typeof( T ) );
			var hasUnpackActionTables = hasUnackActions && targetInfo.Members.Any( m => m.Member != null ); // Except tuples

			var contextfulConstructor =
				this.CreateConstructor(
					MethodAttributes.Public,
					ConstructorParameterTypes,
					( type, il ) =>
						this.CreateContextfulObjectConstructor(
							type,
							il,
							hasPackActions
								? () => 
									context.SerializationContext.SerializationMethod == SerializationMethod.Array
									|| ( targetInfo != null && targetInfo.Members.All( m => m.Member == null ) ) // tuple
									? builder.EmitPackOperationListInitialization( context, targetInfo )
									: builder.EmitPackOperationTableInitialization( context, targetInfo )
								: default( Func<ILConstruct> ),
							hasUnackActions
								? () => builder.EmitUnpackOperationListInitialization( context, targetInfo )
								: default( Func<ILConstruct> ),
							hasUnpackActionTables
								? () => builder.EmitUnpackOperationTableInitialization( context, targetInfo )
								: default( Func<ILConstruct> ),
							hasUnackActions
								? () => builder.EmitMemberListInitialization( context, targetInfo )
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

#if !CORE_CLR
			var ctor = this._typeBuilder.CreateType().GetConstructor( ConstructorParameterTypes );
#else
			var ctor = this._typeBuilder.CreateTypeInfo().GetConstructor( ConstructorParameterTypes );
#endif // !CORE_CLR
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
			Type baseType,
			TracingILGenerator il,
			Func<ILConstruct> packActionListInitializerProvider,
			Func<ILConstruct> unpackActionListInitializerProvider,
			Func<ILConstruct> unpackActionTableInitializerProvider,
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
				il.EmitCallConstructor(
#if !CORE_CLR
					baseType.GetConstructor(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ConstructorParameterTypes, null
					)
#else
					baseType.GetConstructor( ConstructorParameterTypes )
#endif // !CORE_CLR
				);
			}
			else
			{
				il.EmitCall( this._methodTable[ MethodName.RestoreSchema ] );
				il.EmitCallConstructor(
#if !CORE_CLR
					baseType.GetConstructor(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CollectionConstructorParameterTypes, null
					)
#else
					baseType.GetConstructor( CollectionConstructorParameterTypes )
#endif // !CORE_CLR
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

			if ( unpackActionListInitializerProvider != null )
			{
				unpackActionListInitializerProvider().Evaluate( il );
			}

			if ( unpackActionTableInitializerProvider != null )
			{
				unpackActionTableInitializerProvider().Evaluate( il );
			}

			if ( memberNamesInitializerProvider != null )
			{
				memberNamesInitializerProvider().Evaluate( il );
			}

			if ( unpackToInitializerProvider != null )
			{
				unpackToInitializerProvider().Evaluate( il );
			}

			il.EmitRet();
		}
	}
}
