#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines common features for serializer builder.
	/// </summary>
	/// <typeparam name="TContext">The type of the context which holds global information for generating serializer.</typeparam>
	/// <typeparam name="TConstruct">The type of the construct which abstracts code constructs.</typeparam>
	internal abstract partial class SerializerBuilder<TContext, TConstruct> :
#if FEATURE_CODEGEN
		ISerializerCodeGenerator,
#endif // FEATURE_CODEGEN
		ISerializerBuilder
		where TContext : SerializerGenerationContext<TConstruct>
		where TConstruct : class, ICodeConstruct
	{
		private readonly SerializerBuilderNilImplicationHandler _nilImplicationHandler;

		/// <summary>
		///		Gets the type of the serialization target.
		/// </summary>
		/// <value>
		///		The type of the serialization target.
		/// </value>
		protected internal Type TargetType { get; private set; }

		/// <summary>
		///		Gets the <see cref="T:CollectionTraits"/> cache of <see cref="TargetType"/>.
		/// </summary>
		/// <value>
		///		The <see cref="T:CollectionTraits"/> cache of <see cref="TargetType"/>.
		/// </value>
		protected CollectionTraits CollectionTraits { get; private set; }

		/// <summary>
		///		Gets the base class of the generating serializer.
		/// </summary>
		/// <value>
		///		The base class of the generating serializer.
		/// </value>
		protected Type BaseClass { get; private set; }

		private static Type DetermineBaseClass( Type targetType, CollectionTraits traits )
		{

#if DEBUG
			Contract.Assert(
				traits.DetailedCollectionType != CollectionDetailedKind.Unserializable,
				targetType + "(" + traits.DetailedCollectionType + ") != CollectionDetailedKind.Unserializable" 
			);
#endif // DEBUG
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.GenericEnumerable:
				{
					return typeof( EnumerableMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
				case CollectionDetailedKind.GenericCollection:
#if !NET35
				case CollectionDetailedKind.GenericSet:
#endif // !NET35
				case CollectionDetailedKind.GenericList:
				{
					return typeof( CollectionMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
#if !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
				{
					return typeof( ReadOnlyCollectionMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
#endif // !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					return
						typeof( DictionaryMessagePackSerializer<,,> ).MakeGenericType(
							targetType,
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
				}
#if !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					return
						typeof( ReadOnlyDictionaryMessagePackSerializer<,,> ).MakeGenericType(
							targetType,
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
				}
#endif // !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					return typeof( NonGenericEnumerableMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					return typeof( NonGenericCollectionMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericList:
				{
					return typeof( NonGenericListMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					return typeof( NonGenericDictionaryMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.Array:
				{
					// Should be handled by GenericSerializer
					throw new NotSupportedException( "Array is not supported." );
				}
				default:
				{
#if DEBUG
					Contract.Assert(
						traits.DetailedCollectionType == CollectionDetailedKind.NotCollection,
						"Unknown type:" + traits.DetailedCollectionType 
					);
#endif // DEBUG
					return
						targetType.GetIsEnum()
							? typeof( EnumMessagePackSerializer<> ).MakeGenericType( targetType )
							: typeof( MessagePackSerializer<> ).MakeGenericType( targetType );
				}
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerBuilder{TContext, TConstruct}"/> class.
		/// </summary>
		/// <param name="targetType">The type of serialization target.</param>
		/// <param name="collectionTraits">The collection traits of the serialization target.</param>
		protected SerializerBuilder( Type targetType, CollectionTraits collectionTraits )
		{
#if DEBUG
			Contract.Assert( targetType != null, "targetType != null" );
#endif // DEBUG

			this.TargetType = targetType;
			this.CollectionTraits = collectionTraits;
			this.BaseClass = DetermineBaseClass( targetType, collectionTraits );
			this._nilImplicationHandler = new SerializerBuilderNilImplicationHandler( targetType );
		}

		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information.</param>
		/// <param name="concreteType">The substitution type if <see cref="TargetType"/> is abstract type. <c>null</c> when <see cref="TargetType"/> is not abstract type.</param>
		/// <param name="schema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value may be <c>null</c>.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		public MessagePackSerializer BuildSerializerInstance( SerializationContext context, Type concreteType, PolymorphismSchema schema )
		{
#if DEBUG
			Contract.Assert(
				this.CollectionTraits.DetailedCollectionType != CollectionDetailedKind.Array,
				this.TargetType + "(" + this.CollectionTraits.DetailedCollectionType + ") != CollectionDetailedKind.Array"
			);
#endif // DEBUG

			Func<SerializationContext, MessagePackSerializer> constructor;
			var codeGenerationContext = this.CreateCodeGenerationContextForSerializerCreation( context );
			if ( this.TargetType.GetIsEnum() )
			{
				this.BuildEnumSerializer( codeGenerationContext );
				constructor = this.CreateEnumSerializerConstructor( codeGenerationContext );
			}
			else
			{
				SerializationTarget targetInfo;
				this.BuildSerializer( codeGenerationContext, concreteType, schema, out targetInfo );
				constructor =
					this.CreateSerializerConstructor(
						codeGenerationContext,
						targetInfo,
						schema,
						targetInfo == null ? default( SerializerCapabilities? ) : targetInfo.GetCapabilitiesForObject()
					);
			}

			if ( constructor != null )
			{
				var serializer = constructor( context );
#if DEBUG
				Contract.Assert( serializer != null );
#endif // DEBUG
				return serializer;
			}

			throw SerializationExceptions.NewTypeCannotSerialize( this.TargetType );
		}

		/// <summary>
		///		Creates the code generation context for serializer instance creation.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <returns>
		///		The code generation context for serializer instance creation.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract TContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context );

		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information. This value will not be <c>null</c>.</param>
		/// <param name="concreteType">The substitution type if <see cref="TargetType"/> is abstract type. <c>null</c> when <see cref="TargetType"/> is not abstract type.</param>
		/// <param name="schema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value may be <c>null</c>.</param>
		/// <param name="targetInfo">The parsed serialization target information.</param>
		protected void BuildSerializer( TContext context, Type concreteType, PolymorphismSchema schema, out SerializationTarget targetInfo )
		{
#if DEBUG
			Contract.Assert( !this.TargetType.IsArray );
			Contract.Assert( !this.TargetType.GetIsEnum() );
#endif

			switch ( this.CollectionTraits.CollectionType )
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					this.BuildCollectionSerializer( context, concreteType, schema, out targetInfo );
					break;
				}
				case CollectionKind.NotCollection:
				{
					var nullableUnderlyingType = Nullable.GetUnderlyingType( this.TargetType );
					if ( nullableUnderlyingType != null )
					{
						targetInfo = null;
						this.BuildNullableSerializer( context, nullableUnderlyingType );
					}
#if !NET35
					else if ( TupleItems.IsTuple( this.TargetType ) )
					{
						this.BuildTupleSerializer( context, ( schema ?? PolymorphismSchema.Default ).ChildSchemaList, out targetInfo );
					}
#endif
					else
					{
#if DEBUG
						Contract.Assert( schema == null || schema.UseDefault );
#endif // DEBUG
						targetInfo = this.BuildObjectSerializer( context );
					}
					break;
				}
				default:
				{
					throw new NotSupportedException( "Unknown traits :" + this.CollectionTraits );
				}
			}
		}

		/// <summary>
		///		Creates the serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <param name="targetInfo">The parsed serialization target information.</param>
		/// <param name="schema">The polymorphism schema of this.</param>
		/// <param name="capabilities">The capabilities of the generating serializer.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructor(
			TContext codeGenerationContext,
			SerializationTarget targetInfo,
			PolymorphismSchema schema,
			SerializerCapabilities? capabilities
		);

		/// <summary>
		///		Creates the enum serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer> CreateEnumSerializerConstructor(
			TContext codeGenerationContext
		);


#if FEATURE_CODEGEN
		/// <summary>
		///		Builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		/// </param>
		/// <param name="concreteType">The substitution type if <see cref="TargetType"/> is abstract type. <c>null</c> when <see cref="TargetType"/> is not abstract type.</param>
		/// <param name="itemSchema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value must not be <c>null</c>.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		public void BuildSerializerCode( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this.BuildSerializerCodeCore( context, concreteType, itemSchema );
		}

		/// <summary>
		///		In derived class, builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		///		This value will not be <c>null</c>.
		/// </param>
		/// <param name="concreteType">The substitution type if <see cref="TargetType"/> is abstract type. <c>null</c> when <see cref="TargetType"/> is not abstract type.</param>
		/// <param name="itemSchema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value must not be <c>null</c>.</param>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		protected virtual void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			throw new NotSupportedException();
		}

#endif // FEATURE_CODEGEN

		internal class SerializerBuilderNilImplicationHandler :
			NilImplicationHandler<TConstruct, TConstruct, SerializerBuilderOnPackingParameter, SerializerBuilderOnUnpacedParameter>
		{
			private readonly Type _targetType;

			public SerializerBuilderNilImplicationHandler( Type targetType )
			{
				this._targetType = targetType;
			}

			protected override TConstruct OnPackingMessagePackObject(
				SerializerBuilderOnPackingParameter parameter 
			)
			{
				return
					parameter.Builder.EmitGetPropertyExpression(
						parameter.Context,
						parameter.Item,
						Metadata._MessagePackObject.IsNil 
					);
			}

			protected override TConstruct OnPackingReferenceTypeObject(
				SerializerBuilderOnPackingParameter parameter
			)
			{
				return
					parameter.Builder.EmitEqualsExpression(
						parameter.Context,
						parameter.Item,
						parameter.Builder.MakeNullLiteral( parameter.Context, parameter.ItemType )
					);
			}

			protected override TConstruct OnPackingNullableValueTypeObject(
				SerializerBuilderOnPackingParameter parameter
			)
			{
				return
					parameter.Builder.EmitNotExpression(
						parameter.Context,
						parameter.Builder.EmitGetPropertyExpression(
							parameter.Context,
							parameter.Item,
							parameter.ItemType.GetProperty( "HasValue" ) 
						)
					);
			}

			protected override TConstruct OnPackingCore(
				SerializerBuilderOnPackingParameter parameter,
				TConstruct condition 
			)
			{
				return
					parameter.Builder.EmitConditionalExpression(
						parameter.Context,
						condition,
						parameter.Builder.EmitInvokeVoidMethod(
							parameter.Context,
							null,
							SerializationExceptions.ThrowNullIsProhibitedMethod,
							parameter.Builder.MakeStringLiteral( parameter.Context, parameter.MemberName )
						),
						null
					);
			}

			protected override TConstruct OnNopOnUnpacked( SerializerBuilderOnUnpacedParameter parameter )
			{
				return null;
			}

			protected override TConstruct OnThrowNullIsProhibitedExceptionOnUnpacked( SerializerBuilderOnUnpacedParameter parameter )
			{
				return
					parameter.Builder.EmitInvokeVoidMethod(
						parameter.Context,
						null,
						SerializationExceptions.ThrowNullIsProhibitedMethod,
						parameter.MemberName
					);
			}

			protected override TConstruct OnThrowValueTypeCannotBeNull3OnUnpacked( SerializerBuilderOnUnpacedParameter parameter )
			{
				return
					parameter.Builder.EmitInvokeVoidMethod(
						parameter.Context,
						null,
						SerializationExceptions.ThrowValueTypeCannotBeNull3Method,
						parameter.MemberName,
						parameter.Builder.EmitTypeOfExpression( parameter.Context, parameter.ItemType ),
						parameter.Builder.EmitTypeOfExpression( parameter.Context, this._targetType )
					);
			}
		}

		internal struct SerializerBuilderOnPackingParameter : INilImplicationHandlerParameter
		{
			public readonly SerializerBuilder<TContext, TConstruct> Builder;

			public readonly TContext Context;

			public readonly TConstruct Item;

			private readonly Type _itemType;

			public Type ItemType
			{
				get { return this._itemType; }
			}

			public readonly string MemberName;

			public SerializerBuilderOnPackingParameter(
				SerializerBuilder<TContext, TConstruct> builder,
				TContext context,
				TConstruct item,
				Type itemType,
				string memberName
				)
			{
				this.Builder = builder;
				this.Context = context;
				this.Item = item;
				this._itemType = itemType;
				this.MemberName = memberName;
			}
		}

		internal struct SerializerBuilderOnUnpacedParameter : INilImplicationHandlerOnUnpackedParameter<TConstruct>
		{
			public readonly SerializerBuilder<TContext, TConstruct> Builder;

			public readonly TContext Context;

			private readonly Type _itemType;

			public Type ItemType
			{
				get { return this._itemType; }
			}

			public readonly TConstruct MemberName;

			private readonly TConstruct _store;
			public TConstruct Store
			{
				get { return this._store; }
			}


			public SerializerBuilderOnUnpacedParameter(
				SerializerBuilder<TContext, TConstruct> builder,
				TContext context,
				Type itemType,
				TConstruct memberName,
				TConstruct store
				)
			{
				this.Builder = builder;
				this.Context = context;
				this._itemType = itemType;
				this.MemberName = memberName;
				this._store = store;
			}
		}
	}
}
