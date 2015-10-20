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
using System.Diagnostics.Contracts;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines common features for serializer builder.
	/// </summary>
	/// <typeparam name="TContext">The type of the context which holds global information for generating serializer.</typeparam>
	/// <typeparam name="TConstruct">The type of the construct which abstracts code constructs.</typeparam>
	/// <typeparam name="TObject">The type of the object which will be target of the generating serializer.</typeparam>
	[ContractClass( typeof( SerializerBuilderContract<,,> ) )]
	internal abstract partial class SerializerBuilder<TContext, TConstruct, TObject> :
#if !NETFX_CORE && !SILVERLIGHT
		ISerializerCodeGenerator,
#endif
		ISerializerBuilder<TObject>
		where TContext : SerializerGenerationContext<TConstruct>
		where TConstruct : class, ICodeConstruct
	{
		private readonly SerializerBuilderNilImplicationHandler _nilImplicationHandler =
			new SerializerBuilderNilImplicationHandler();

		// ReSharper disable once StaticMemberInGenericType
		/// <summary>
		///		The <see cref="CollectionTraits"/> cache of <typeparamref name="TObject"/>.
		/// </summary>
		protected static readonly CollectionTraits CollectionTraitsOfThis;

		// ReSharper disable once StaticMemberInGenericType
		/// <summary>
		///		A base class of the generating serializer.
		/// </summary>
		protected static readonly Type BaseClass;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "BaseClass and CollectionTraitsOfThis should be initialized at once" )]
		static SerializerBuilder()
		{
			var traits = typeof( TObject ).GetCollectionTraits();
			CollectionTraitsOfThis = traits;
#if DEBUG && !UNITY
			Contract.Assert(
				traits.DetailedCollectionType != CollectionDetailedKind.Unserializable,
				typeof( TObject ) + "(" + traits.DetailedCollectionType + ") != CollectionDetailedKind.Unserializable" 
			);
#endif // DEBUG
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.GenericEnumerable:
				{
					BaseClass = typeof( EnumerableMessagePackSerializer<,> ).MakeGenericType( typeof( TObject ), traits.ElementType );
					break;
				}
				case CollectionDetailedKind.GenericCollection:
#if !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericList:
				{
					BaseClass = typeof( CollectionMessagePackSerializer<,> ).MakeGenericType( typeof( TObject ), traits.ElementType );
					break;
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
				{
					BaseClass = typeof( ReadOnlyCollectionMessagePackSerializer<,> ).MakeGenericType( typeof( TObject ), traits.ElementType );
					break;
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					BaseClass =
						typeof( DictionaryMessagePackSerializer<,,> ).MakeGenericType(
							typeof( TObject ),
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
					break;
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					BaseClass =
						typeof( ReadOnlyDictionaryMessagePackSerializer<,,> ).MakeGenericType(
							typeof( TObject ),
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
					break;
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					BaseClass = typeof( NonGenericEnumerableMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					break;
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					BaseClass = typeof( NonGenericCollectionMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					break;
				}
				case CollectionDetailedKind.NonGenericList:
				{
					BaseClass = typeof( NonGenericListMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					break;
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					BaseClass = typeof( NonGenericDictionaryMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					break;
				}
				case CollectionDetailedKind.Array:
				{
					BaseClass =
						typeof( TObject ).GetIsEnum()
							? typeof( EnumMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) )
							: typeof( MessagePackSerializer<TObject> );
					break;
				}
				default:
				{
#if DEBUG && !UNITY
					Contract.Assert(
						traits.DetailedCollectionType == CollectionDetailedKind.NotCollection,
						"Unknown type:" + traits.DetailedCollectionType 
					);
#endif // DEBUG && !UNITY
					BaseClass =
						typeof( TObject ).GetIsEnum()
							? typeof( EnumMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) )
							: typeof( MessagePackSerializer<TObject> );
					break;
				}
			}
		} 

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerBuilder{TContext, TConstruct, TObject}"/> class.
		/// </summary>
		protected SerializerBuilder() { }

		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information.</param>
		/// <param name="concreteType">The substitution type if <typeparamref name="TObject"/> is abstract type. <c>null</c> when <typeparamref name="TObject"/> is not abstract type.</param>
		/// <param name="schema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value may be <c>null</c>.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public MessagePackSerializer<TObject> BuildSerializerInstance( SerializationContext context, Type concreteType, PolymorphismSchema schema )
		{
#if DEBUG && !UNITY
			Contract.Assert(
				CollectionTraitsOfThis.DetailedCollectionType != CollectionDetailedKind.Array,
				typeof( TObject ) + "(" + CollectionTraitsOfThis.DetailedCollectionType + ") != CollectionDetailedKind.Array"
			);
#endif // DEBUG
			Func<SerializationContext, MessagePackSerializer<TObject>> constructor;
			var codeGenerationContext = this.CreateCodeGenerationContextForSerializerCreation( context );
			if ( typeof( TObject ).GetIsEnum() )
			{
				this.BuildEnumSerializer( codeGenerationContext );
				constructor = this.CreateEnumSerializerConstructor( codeGenerationContext );
			}
			else
			{
				this.BuildSerializer( codeGenerationContext, concreteType, schema );
				constructor = this.CreateSerializerConstructor( codeGenerationContext, schema );
			}

			if ( constructor != null )
			{
				var serializer = constructor( context );
#if DEBUG && !UNITY
				Contract.Assert( serializer != null );
#endif // DEBUG && !UNITY
				return serializer;
			}

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( TObject ) );
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
		/// <param name="concreteType">The substitution type if <typeparamref name="TObject"/> is abstract type. <c>null</c> when <typeparamref name="TObject"/> is not abstract type.</param>
		/// <param name="schema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value may be <c>null</c>.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected void BuildSerializer( TContext context, Type concreteType, PolymorphismSchema schema )
		{
#if DEBUG
			Contract.Assert( !typeof( TObject ).IsArray );
			Contract.Assert( !typeof( TObject ).GetIsEnum() );
#endif

			switch ( CollectionTraitsOfThis.CollectionType )
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					this.BuildCollectionSerializer( context, concreteType, schema );
					break;
				}
				case CollectionKind.NotCollection:
				{
					var nullableUnderlyingType = Nullable.GetUnderlyingType( typeof( TObject ) );
					if ( nullableUnderlyingType != null )
					{
						this.BuildNullableSerializer( context, nullableUnderlyingType );
					}
#if !NETFX_35
					else if ( TupleItems.IsTuple( typeof( TObject ) ) )
					{
						this.BuildTupleSerializer( context, ( schema ?? PolymorphismSchema.Default ).ChildSchemaList );
					}
#endif
					else
					{
#if DEBUG && !UNITY
						Contract.Assert( schema == null || schema.UseDefault );
#endif // DEBUG
						this.BuildObjectSerializer( context );
					}
					break;
				}
			}
		}

		/// <summary>
		///		Creates the serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <param name="schema">The polymorphism schema of this.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor(
			TContext codeGenerationContext,
			PolymorphismSchema schema
		);

		/// <summary>
		///		Creates the enum serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor(
			TContext codeGenerationContext
		);


#if !NETFX_CORE && !SILVERLIGHT
		/// <summary>
		///		Builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		/// </param>
		/// <param name="concreteType">The substitution type if <typeparamref name="TObject"/> is abstract type. <c>null</c> when <typeparamref name="TObject"/> is not abstract type.</param>
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
		/// <param name="concreteType">The substitution type if <typeparamref name="TObject"/> is abstract type. <c>null</c> when <typeparamref name="TObject"/> is not abstract type.</param>
		/// <param name="itemSchema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value must not be <c>null</c>.</param>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		protected virtual void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			throw new NotSupportedException();
		}
#endif
		internal class SerializerBuilderNilImplicationHandler :
			NilImplicationHandler<TConstruct, TConstruct, SerializerBuilderOnPackingParameter, SerializerBuilderOnUnpacedParameter>
		{
			protected override TConstruct OnPackingMessagePackObject(
				SerializerBuilderOnPackingParameter parameter )
			{
				return parameter.Builder.EmitGetPropertyExpression(
					parameter.Context,
					parameter.Item,
					Metadata._MessagePackObject.IsNil );
			}

			protected override TConstruct OnPackingReferenceTypeObject(
				SerializerBuilderOnPackingParameter parameter )
			{
				return
					parameter.Builder.EmitEqualsExpression(
						parameter.Context,
						parameter.Item,
						parameter.Builder.MakeNullLiteral( parameter.Context, parameter.ItemType )
						);
			}

			protected override TConstruct OnPackingNullableValueTypeObject(
				SerializerBuilderOnPackingParameter parameter )
			{
				return
					parameter.Builder.EmitNotExpression(
						parameter.Context,
						parameter.Builder.EmitGetPropertyExpression(
							parameter.Context,
							parameter.Item,
							parameter.ItemType.GetProperty( "HasValue" ) )
						);
			}

			protected override TConstruct OnPackingCore(
				SerializerBuilderOnPackingParameter parameter,
				TConstruct condition )
			{
				return
					parameter.Builder.EmitConditionalExpression(
						parameter.Context,
						condition,
						parameter.Builder.EmitThrowExpression(
							parameter.Context,
							parameter.ItemType,
							SerializationExceptions.NewNullIsProhibitedMethod,
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
					parameter.Builder.EmitThrowExpression(
						parameter.Context,
						parameter.Store.ContextType,
						SerializationExceptions.NewNullIsProhibitedMethod,
						parameter.MemberName
					);
			}

			protected override TConstruct OnThrowValueTypeCannotBeNull3OnUnpacked( SerializerBuilderOnUnpacedParameter parameter )
			{
				return
					parameter.Builder.EmitThrowExpression(
						parameter.Context,
						parameter.Store.ContextType,
						SerializationExceptions.NewValueTypeCannotBeNull3Method,
						parameter.MemberName,
						parameter.Builder.EmitTypeOfExpression( parameter.Context, parameter.ItemType ),
						parameter.Builder.EmitTypeOfExpression( parameter.Context, typeof( TObject ) )
					);
			}
		}

		internal struct SerializerBuilderOnPackingParameter : INilImplicationHandlerParameter
		{
			public readonly SerializerBuilder<TContext, TConstruct, TObject> Builder;

			public readonly TContext Context;

			public readonly TConstruct Item;

			private readonly Type _itemType;

			public Type ItemType
			{
				get { return this._itemType; }
			}

			public readonly string MemberName;

			public SerializerBuilderOnPackingParameter(
				SerializerBuilder<TContext, TConstruct, TObject> builder,
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
			public readonly SerializerBuilder<TContext, TConstruct, TObject> Builder;

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
				SerializerBuilder<TContext, TConstruct, TObject> builder,
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