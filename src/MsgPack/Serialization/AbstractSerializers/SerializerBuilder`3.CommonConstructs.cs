#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		protected static readonly TConstruct Unit = null;
		// TODO: Arrays.Empty
		private static readonly Type[] CollectionConstructorWithCapacityParameterTypes = new[] { typeof( int ) };
		private static readonly TConstruct[] NoConstructs = new TConstruct[ 0 ];

		protected class ForLoopContext
		{
			public TConstruct Counter { get; private set; }

			public ForLoopContext( TConstruct counter )
			{
				this.Counter = counter;
			}
		}

		private void BeginPackToMethod( TContext context )
		{
			this.EmitMethodPrologue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "PackToCore", BindingFlags.Instance | BindingFlags.NonPublic )
			);
		}

		private void EndPackToMethod( TContext context, TConstruct construct )
		{
			this.EmitMethodEpilogue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "PackToCore", BindingFlags.Instance | BindingFlags.NonPublic ),
				construct
			);
		}

		private void BeginUnpackFromMethod( TContext context )
		{
			this.EmitMethodPrologue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "UnpackFromCore", BindingFlags.Instance | BindingFlags.NonPublic )
			);
		}

		private void EndUnpackFromMethod( TContext context, TConstruct construct )
		{
			this.EmitMethodEpilogue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "UnpackFromCore", BindingFlags.Instance | BindingFlags.NonPublic ),
				construct
			);
		}

		private void BeginUnpackToMethod( TContext context )
		{
			this.EmitMethodPrologue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "UnpackToCore", BindingFlags.Instance | BindingFlags.NonPublic )
			);
		}

		private void EndUnpackToMethod( TContext context, TConstruct construct )
		{
			this.EmitMethodEpilogue(
				context,
				typeof( MessagePackSerializer<TObject> ).GetMethod( "UnpackToCore", BindingFlags.Instance | BindingFlags.NonPublic ),
				construct
			);
		}

		/// <summary>
		///		Emits the method prologue.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="metadata">The metadata of the method.</param>
		protected abstract void EmitMethodPrologue( TContext context, MethodInfo metadata );

		private void EmitMethodEpilogue( TContext context, MethodInfo metadata, params TConstruct[] constructs )
		{
			this.EmitMethodEpilogue( context, metadata, constructs as IList<TConstruct> );
		}

		///  <summary>
		/// 		Emits the method epiloigue.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="metadata">The metadata of the method.</param>
		/// <param name="constructs">The constructs which represent method statements in order. Null entry should be ignored.</param>
		protected abstract void EmitMethodEpilogue( TContext context, MethodInfo metadata, IList<TConstruct> constructs );

		protected abstract TConstruct EmitStatementExpression( TContext context, TConstruct statement, TConstruct contextExpression );

		protected abstract TConstruct MakeNullLiteral( TContext context );

		/// <summary>
		///		Emits the constant int value reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The constant reference.</returns>
		protected abstract TConstruct MakeInt32Literal( TContext context, int constant );

		protected abstract TConstruct MakeInt64Literal( TContext context, long constant );

		/// <summary>
		///		Emits the constant string value reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The constant reference.</returns>
		protected abstract TConstruct MakeStringLiteral( TContext context, string constant );

		protected abstract TConstruct Nop( TContext context, Type contextType );

		protected abstract TConstruct EmitThisReferenceExpression( TContext context );

		protected abstract TConstruct EmitBoxExpression( TContext context, Type valueType, TConstruct value );

		protected abstract TConstruct EmitNotExpression( TContext context, TConstruct booleanExpression );

		protected abstract TConstruct EmitEqualsExpression( TContext context, TConstruct left, TConstruct right );

		protected virtual TConstruct EmitNotEqualsExpression( TContext context, TConstruct left, TConstruct right )
		{
			return this.EmitNotExpression( context, this.EmitEqualsExpression( context, left, right ) );
		}

		protected abstract TConstruct EmitGraterThanExpression( TContext context, TConstruct left, TConstruct right );

		protected abstract TConstruct EmitLesserThanExpression( TContext context, TConstruct left, TConstruct right );

		protected abstract TConstruct EmitDefaultValueExpression( TContext context, Type type );

		protected abstract TConstruct EmitIncrementExpression( TContext context, TConstruct int32Value );

		protected abstract TConstruct EmitTypeOfExpression( TContext context, Type type );

		protected abstract TConstruct EmitUncheckedConvertExpression( TContext context, Type targetType, TConstruct value );

		protected TConstruct EmitSequentialStatements( TContext context, params TConstruct[] statements )
		{
			return this.EmitSequentialStatements( context, statements as IEnumerable<TConstruct> );
		}

		protected TConstruct EmitSequentialStatements( TContext context, IEnumerable<TConstruct> statements )
		{
			return this.EmitSequentialStatements( context, typeof( void ), statements );
		}

		protected TConstruct EmitSequentialStatements( TContext context, Type contextType, params TConstruct[] statements )
		{
			return this.EmitSequentialStatements( context, contextType, statements as IEnumerable<TConstruct> );
		}

		protected abstract TConstruct EmitSequentialStatements( TContext context, Type contextType, IEnumerable<TConstruct> statements );

		protected abstract TConstruct DeclareLocal( TContext context, Type type, string name, TConstruct initExpression );

		/// <summary>
		///		Emits the local variable declaration.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the local.</param>
		/// <param name="name">The name of the local for debugging.</param>
		/// <param name="initExpression">The init expression. <c>null</c> indicates default value of the type.</param>
		/// <param name="expression">The subsequent expression which uses declared local as monad.</param> // FIXME: TERM
		/// <returns></returns>
		[Obsolete]
		protected abstract TConstruct DeclareLocal( TContext context, Type type, string name, TConstruct initExpression, ExpressionWithMonad expression );

		/// <summary>
		///		Represents monal with declared local.
		/// </summary>
		/// <param name="monadic">The declared local as contextual.</param> // FIXME: Monad?
		/// <returns>THe expression.</returns>
		protected delegate TConstruct ExpressionWithMonad( TConstruct monadic );

		/// <summary>
		///		Emits the invoke void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns></returns>
		/// <remarks>
		///		The derived class must emits codes which discard return non-void value.
		/// </remarks>
		protected abstract TConstruct EmitInvokeVoidMethod(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		);

		protected abstract TConstruct EmitCreateNewObjectExpression(
			TContext context, ConstructorInfo constructor, params TConstruct[] arguments
		);

		protected TConstruct EmitInvokeMethodExpression(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		)
		{
			return
				this.EmitInvokeMethodExpression(
					context, instance, method, ( arguments ?? new TConstruct[ 0 ] ) as IEnumerable<TConstruct>
				);
		}

		protected abstract TConstruct EmitInvokeMethodExpression( TContext context, TConstruct instance, MethodInfo method, IEnumerable<TConstruct> arguments );

		protected TConstruct EmitGetMemberValueExpression( TContext context, TConstruct instance, MemberInfo member )
		{
			// ReSharper disable RedundantIfElseBlock
			FieldInfo asField;
			if ( ( asField = member as FieldInfo ) != null )
			{
				return this.EmitGetFieldExpression( context, instance, asField );
			}
			else
			{
				var asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				return this.EmitGetPropretyExpression( context, instance, asProperty );
			}
			// ReSharper restore RedundantIfElseBlock
		}

		protected abstract TConstruct EmitGetPropretyExpression( TContext context, TConstruct instance, PropertyInfo property );

		protected abstract TConstruct EmitGetFieldExpression( TContext context, TConstruct instance, FieldInfo field );

		protected TConstruct EmitSetMemberValue( TContext context, TConstruct instance, MemberInfo member, TConstruct value )
		{
			TConstruct getCollection;
			CollectionTraits traits;
			FieldInfo asField;
			if ( ( asField = member as FieldInfo ) != null )
			{
				if ( !asField.IsInitOnly )
				{
					return this.EmitSetField( context, instance, asField, value );
				}

				getCollection = this.EmitGetFieldExpression( context, instance, asField );
				traits = asField.FieldType.GetCollectionTraits();
			}
			// ReSharper disable RedundantIfElseBlock
			else
			// ReSharper restore RedundantIfElseBlock
			{
				var asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				if ( asProperty.GetSetMethod( true ) != null )
				{
					return this.EmitSetProprety( context, instance, asProperty, value );
				}

				getCollection = this.EmitGetPropretyExpression( context, instance, asProperty );
				traits = asProperty.PropertyType.GetCollectionTraits();
			}

			// use Add(T) for appendable collection type read only member.

			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					/*
					 *	foreach ( var item in unpacked )
					 *	{
					 *		target.Prop.Add(item);
					 *	}
					 */
					return
						this.EmitForEachLoop(
							context,
							traits,
							value,
							this.Nop( context, typeof( void ) ),
							( current, _ ) =>
							this.EmitAppendCollectionItem(
								context,
								traits,
								member.GetMemberValueType(),
								getCollection,
								current
							)
						);
				}
				case CollectionKind.Map:
				{
					/*
					 *	foreach ( var item in unpacked )
					 *	{
					 *		target.Prop.Add(item.Key, item.Value);
					 *	}
					 */
					Type keyType, valueType;
					GetDictionaryKeyValueType( traits.ElementType, out keyType, out valueType );
					return
						this.EmitForEachLoop(
							context,
							traits,
							value,
							this.Nop( context, typeof( void ) ),
							( current, _ ) =>
							this.EmitAppendDictionaryItem( 
								context,
								traits,
								getCollection,
								keyType,
								this.EmitGetPropretyExpression( 
									context,
									current,
									traits.ElementType == typeof(DictionaryEntry)
									? Metadata._DictionaryEntry.Key
									: traits.ElementType.GetProperty( "Key" )
								),
								valueType,
								this.EmitGetPropretyExpression(
									context,
									current,
									traits.ElementType == typeof( DictionaryEntry )
									? Metadata._DictionaryEntry.Value
									: traits.ElementType.GetProperty( "Value" )
								),
								false
							)
						);
				}
				default:
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Member '{0}' is read only and its type ('{1}') is not an appendable collection",
							member.Name,
							member.GetMemberValueType()
						)
					);
				}
			}
		}

		protected abstract TConstruct EmitSetProprety( TContext context, TConstruct instance, PropertyInfo property, TConstruct value );

		protected abstract TConstruct EmitSetField( TContext context, TConstruct instance, FieldInfo field, TConstruct value );

		// Stores context value to specified variable.
		protected TConstruct EmitSetVariable( TContext context, TConstruct variable )
		{
			return this.EmitSetVariable( context, variable, null );
		}

		protected abstract TConstruct EmitSetVariable( TContext context, TConstruct variable, TConstruct value );

		protected TConstruct EmitThrow( TContext context, Type contextType, MethodInfo factoryMethod, params TConstruct[] arguments )
		{
			return this.EmitThrow( context, this.EmitInvokeMethodExpression( context, null, factoryMethod, arguments ), contextType );
		}

		protected abstract TConstruct EmitThrow( TContext context, TConstruct exceptionExpression, Type contextType );

		protected abstract TConstruct EmitTryFinallyExpression(
			TContext context, TConstruct tryExpression, TConstruct finallyStatement
		);

		protected abstract TConstruct EmitTryCatchExpression<TException>(
			TContext context, TConstruct tryExpression, CatchFunc catchExpression
		);

		protected delegate TConstruct CatchFunc( TConstruct ex );

		protected TConstruct EmitInvariantStringFormat( TContext context, string format )
		{
			return this.MakeStringLiteral( context, format );
		}

		protected TConstruct EmitInvariantStringFormat( TContext context, string format, params TConstruct[] arguments )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._String.Format_P,
					new[]
					{
						this.EmitGetPropretyExpression( context, null, Metadata._CultureInfo.InvariantCulture ),
						this.MakeStringLiteral( context, format ),
						this.EmitCreateNewArrayExpression(
							context,
							typeof( object ),
							arguments.Length,
							arguments.Select( a => a.ContextType.GetIsValueType() ? this.EmitBoxExpression( context, a.ContextType, a ) : a )
						)
					}
				);
		}

		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, Type type, int length, IEnumerable<TConstruct> initialElements
		);

		/// <summary>
		///		Emits the code which gets collection count.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="collection">The collection reference.</param>
		/// <param name="traits">The collection traits.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitGetCollectionCountExpression( TContext context, TConstruct collection, CollectionTraits traits )
		{
			return this.EmitGetPropretyExpression( context, collection, traits.CountProperty );
		}

		/// <summary>
		///		Emits the get serializer expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="targetType">Type of the target of the serializer.</param>
		/// <returns></returns>
		/// <remarks>
		///		The serializer reference methodology is implication specific.
		/// </remarks>
		protected abstract TConstruct EmitGetSerializerExpression( TContext context, Type targetType );

		private TConstruct EmitPackItemExpression( TContext context, TConstruct packer, Type itemType, NilImplication nilImplication, string memberName, TConstruct item )
		{
			var currentItem = item;

			if ( nilImplication == NilImplication.Prohibit )
			{
				TConstruct condition = null;
				if ( itemType == typeof( MessagePackObject ) )
				{
					condition =
						this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, item, itemType.GetProperty( "HasValue" ) )
						);
				}
				else if ( !itemType.GetIsValueType() )
				{
					condition =
						this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, item, itemType.GetProperty( "HasValue" ) )
						);
				}
				else if ( Nullable.GetUnderlyingType( itemType ) != null )
				{
					condition =
						this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, item, itemType.GetProperty( "HasValue" ) )
						);
				}

				if ( condition != null )
				{
					currentItem =
						this.EmitConditionalExpression(
							context,
							condition,
							this.EmitThrow(
								context,
								itemType,
								SerializationExceptions.NewNullIsProhibitedMethod,
								this.MakeStringLiteral( context, memberName ),
								this.EmitTypeOfExpression( context, itemType ),
								this.EmitTypeOfExpression( context, typeof( TObject ) )
							),
							item
						);
				}
			}

			/*
			 * this._serializerN.PackTo(packer, item);
			 */
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitGetSerializerExpression( context, itemType ),
					typeof( MessagePackSerializer<> )
						.MakeGenericType( itemType )
						.GetMethod( "PackTo", BindingFlags.Instance | BindingFlags.Public ),
					packer,
					currentItem
				);
		}

		private TConstruct EmitGetItemsCountExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetItemsCount,
					unpacker
				);
			//var rawItemsCount =
			//	this.EmitTryCatchExpression<InvalidOperationException>(
			//		context,
			//		this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.ItemsCount ),
			//		ex => this.EmitThrow(
			//			context,
			//			typeof( long ),
			//			SerializationExceptions.NewIsIncorrectStreamMethod,
			//			ex
			//		)
			//	);
			//return
			//	this.EmitUncheckedConvertExpression(
			//		context,
			//		typeof( int ),
			//		this.EmitConditionalExpression(
			//			context,
			//			this.EmitGraterThanExpression(
			//				context,
			//				rawItemsCount,
			//				this.MakeInt32Literal( context, Int32.MaxValue )
			//			),
			//			this.EmitThrow(
			//				context,
			//				typeof( long ),
			//				SerializationExceptions.NewIsTooLargeCollectionMethod
			//			),
			//			rawItemsCount
			//		)
			//	);
		}

		private TConstruct EmitUnpackCollectionWithUnpackToExpression(
			TContext context, ConstructorInfo ctor, TConstruct collectionCapacity, TConstruct unpacker
		)
		{
			/*
			 *	TCollection collection = new TCollection( capacity );
			 *	this.UnpackToCore( unpacker, collection );
			 *	return collection;
			 */
			var collection =
				this.DeclareLocal(
					context,
					typeof( TObject ),
					"collection",
					this.EmitCreateNewObjectExpression(
						context,
						ctor,
						ctor.GetParameters().Length == 0
						? NoConstructs
						: new[] { collectionCapacity }
					)
				);

			return
				this.EmitStatementExpression(
					context,
					this.EmitSequentialStatements(
						context,
						collection,
						this.EmitInvokeMethodExpression(
							context,
							this.EmitThisReferenceExpression( context ),
							MessagePackSerializer<TObject>.UnpackToCoreMethod,
							unpacker,
							collection
						)
					),
					collection
				);
		}

		/// <summary>
		/// Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <returns></returns>
		[Obsolete]
		protected TConstruct EmitUnpackItemValueExpression(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct itemIndex,
			TConstruct memberName
		)
		{
			return
				this.EmitSequentialStatements(
					context,
					this.EmitUnpackItemValueExpressionCore(
						context,
						itemType,
						nilImplication,
						unpacker,
						null,
						itemIndex,
						memberName,
						null,
						null,
						( ni, @null, value ) => value
					)
				);
		}

		/// <summary>
		/// Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="unpacking">The reference to the unpacking collection.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="itemsCount">The reference to items count init-only local variable.</param>
		/// <param name="unpacked">The reference to unpacked items count local variable.</param>
		/// <param name="setValueExpression">The expression which sets deserialized value to the target object.</param>
		/// <returns></returns>
		protected TConstruct EmitUnpackItemValueExpression(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct unpacking,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			SetValueFunc setValueExpression
		)
		{
			return
				this.EmitSequentialStatements(
					context,
					this.EmitUnpackItemValueExpressionCore(
						context, itemType, nilImplication, unpacker, unpacking, itemIndex, memberName, null, null, setValueExpression
					)
				);
		}

		private IEnumerable<TConstruct> EmitUnpackItemValueExpressionCore(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct unpacking,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			SetValueFunc setValueExpression
		)
		{
			/*
				 *	if ( !unpacker.Read() )
				 *	{
				 *		throw SerializationExceptiuons.MissingItem(...);
				 *	}
				 *	
				 *	T? nullable;
				 *	if ( unpacked < itemsCount )
				 *	{
				 *		if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				 * 		{
				 * 			nullable = serializer.UnpackFrom( unpacker );
				 * 		}
				 * 		else
				 * 		{
				 * 			using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
				 * 			{
				 * 				nullable = serializer.UnpackFrom( subtreeUnpacker );
				 * 			}
				 * 		}
				 * 	}
				 *	else
				 *	{
				 *		nullable = null;
				 *	}
				 * 
				 *  if ( nullable == null )
				 *  {
				 *  #if MEMBER_DEFAULT
				 *      // nop
				 *  #elif PROHIBIT
				 *		throw SerializationExceptiuons.NullIsProhibited(...);
				 *  #elif VALUE_TYPE
				 *		throw SerializationExceptiuons.ValueTypeCannotbeNull(...);
				 *  #else
				 *		SET_VALUE(item);
				 *  #endif
				 *  }
				 *  else
				 *  {
				 *		SET_VALUE(item);
				 *  }
				 *  
				 *	#if MEMBER_UNPACKING
				 *	unpacked++;
				 *	#endif
				 *  
				 *  context unpacker;
				 */

			var isNativelyNullable =
				itemType == typeof( MessagePackObject )
				|| !itemType.GetIsValueType()
				|| Nullable.GetUnderlyingType( itemType ) != null;

			var nullableType = itemType;
			if ( !isNativelyNullable )
			{
				nullableType = typeof( Nullable<> ).MakeGenericType( itemType );
			}

			yield return
				this.EmitConditionalExpression(
					context,
					this.EmitNotExpression(
						context,
						this.EmitInvokeMethodExpression( context, unpacker, Metadata._Unpacker.Read )
					),
					this.EmitThrow(
						context, typeof( Unpacker ), SerializationExceptions.NewMissingItemMethod, itemIndex
					),
					unpacker
				);

			var nullable =
				this.DeclareLocal(
					context,
					nullableType,
					"nullable" + itemIndex,
					null
				);

			yield return nullable;

			var unpack =
				this.EmitAndConditionalExpression(
					context,
					new[]
					{
						this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.IsArrayHeader )
						),
						this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.IsMapHeader )
						)
					},
					this.EmitSequentialStatements(
						context,
						this.EmitDeserializeItemExpression( context, unpacker, nullableType ),
						this.EmitSetVariable( context, nullable )
					),
					this.EmitUsingExpression(
						context,
						typeof( Unpacker ),
						this.EmitInvokeMethodExpression(
							context,
							unpacker,
							Metadata._Unpacker.ReadSubtree
						),
						subtreeUnpacker =>
							this.EmitSequentialStatements(
								context,
								this.EmitDeserializeItemExpression( context, subtreeUnpacker, nullableType ),
								this.EmitSetVariable( context, nullable )
							)
					)
				);

			// Missing member is treated as nil
			if ( unpacked != null )
			{
#if DEBUG
				Contract.Assert( itemsCount != null );
#endif
				yield return
					this.EmitConditionalExpression(
						context,
						this.EmitLesserThanExpression(
							context,
							unpacked,
							itemsCount
						),
						unpack,
						this.EmitDefaultValueExpression( context, nullableType )
					);
			}
			else
			{
				yield return unpack;
			}

			// Nil Implication
			TConstruct expressionWhenNil = null;
			switch ( nilImplication )
			{
				case NilImplication.MemberDefault:
				{
					expressionWhenNil = this.Nop( context, typeof( void ) );
					break;
				}
				case NilImplication.Prohibit:
				{
					expressionWhenNil =
						this.EmitThrow(
							context,
							typeof( void ),
							SerializationExceptions.NewNullIsProhibitedMethod,
							memberName
						);
					break;
				}
				case NilImplication.Null:
				{
					if ( !isNativelyNullable )
					{
						expressionWhenNil =
							this.EmitThrow(
								context,
								typeof( void ),
								SerializationExceptions.NewValueTypeCannotBeNull3Method,
								memberName,
								this.EmitTypeOfExpression( context, itemType ),
								this.EmitTypeOfExpression( context, typeof( TObject ) )
							);
					}

					break;
				}
				default:
				{
					throw new SerializationException(
						String.Format( CultureInfo.CurrentCulture, "Unknown NilImplication value '{0}'.", ( int )nilImplication )
					);
				}
			}

			var unpackedItem = this.EmitGetValueWhenNullableExpression( context, itemType, nullable );

			yield return
				expressionWhenNil != null
					? this.EmitConditionalExpression(
						context,
						( !isNativelyNullable || Nullable.GetUnderlyingType( itemType ) != null )
							? this.EmitGetPropretyExpression( context, nullable, nullableType.GetProperty( "HasValue" ) )
							: itemType == typeof( MessagePackObject )
							? this.EmitNotExpression(
								context,
								this.EmitGetPropretyExpression( context, nullable, Metadata._MessagePackObject.IsNil )
							) : this.EmitNotEqualsExpression( context, nullable, this.MakeNullLiteral( context ) ),
						setValueExpression( unpacking, nilImplication, unpackedItem ),
						expressionWhenNil
					) : setValueExpression( unpacking, nilImplication, unpackedItem );

			if ( unpacked != null )
			{
#if DEBUG
				Contract.Assert( itemsCount != null );
#endif
				yield return
					this.EmitIncrementExpression(
						context,
						unpacked
					);
			}
		}

		private TConstruct EmitGetValueWhenNullableExpression( TContext context, Type targetType, TConstruct mayBeNullable )
		{
			if ( Nullable.GetUnderlyingType( mayBeNullable.ContextType ) != null
				&& Nullable.GetUnderlyingType( targetType ) == null )
			{
				return this.EmitGetPropretyExpression( context, mayBeNullable, mayBeNullable.ContextType.GetProperty( "Value" ) );
			}
			else
			{
				return mayBeNullable;
			}
		}

		/// <summary>
		///		Callback of EmitUnpackItemValueExpression.
		/// </summary>
		/// <param name="unpacking">The reference to the unpacking object.</param>
		/// <param name="nilImplication">The nil implication of setting member.</param>
		/// <param name="unpackedValueMaybeNull">The unpacked value for setting member. This value may be null for <see cref="NilImplication.Null"/>.</param>
		/// <returns>The statement which represents member setting.</returns>
		protected delegate TConstruct SetValueFunc(
			TConstruct unpacking, NilImplication nilImplication, TConstruct unpackedValueMaybeNull
		);

		protected TConstruct EmitUsingExpression(
			TContext context, Type disposableType, TConstruct instantiateIDisposableExpression, Func<TConstruct, TConstruct> usingBody
		)
		{
			var disposable =
				this.DeclareLocal(
					context,
					disposableType,
					"disposable",
					instantiateIDisposableExpression
				);
			return
				this.EmitSequentialStatements(
					context,
					disposable,
					this.EmitTryFinallyExpression(
						context,
						usingBody( disposable ),
						this.EmitConditionalExpression(
							context,
							this.EmitNotEqualsExpression(
								context,
								disposable,
								this.MakeNullLiteral( context )
							),
							this.EmitInvokeMethodExpression(
								context,
								disposable,
								disposableType.GetMethod( "Dispose", Type.EmptyTypes )
							),
							Unit
						)
					)
				);
		}

		private TConstruct EmitDeserializeItemExpression( TContext context, TConstruct unpacker, Type itemType )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					this.EmitGetSerializerExpression( context, itemType ),
					typeof( MessagePackSerializer<> ).MakeGenericType( itemType ).GetMethod( "UnpackFrom" ),
					unpacker
				);
		}

		/// <summary>
		///		Emits the conditional expression (cond?then:else).
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="conditionExpression">The expression which represents conditional.</param>
		/// <param name="thenExpression">The expression which is used when condition is true.</param>
		/// <param name="elseExpression">The expression which is used when condition is false.</param>
		/// <returns></returns>
		protected abstract TConstruct EmitConditionalExpression(
			TContext context,
			TConstruct conditionExpression,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		protected abstract TConstruct EmitAndConditionalExpression(
			TContext context,
			IList<TConstruct> conditionExpressions,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		protected abstract TConstruct EmitStringSwitchStatement(
			TContext context, TConstruct target, IDictionary<string, TConstruct> cases
		);

		/// <summary>
		/// 	Emits the for loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="count">The count reference for loop condition.</param>
		/// <param name="expressionContext">The expression context.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes for loop context then returns loop body construct.</param>
		/// <returns>The expression context after for loop.</returns>
		protected abstract TConstruct EmitForLoop( TContext context, TConstruct count, TConstruct expressionContext, Func<ForLoopContext, TConstruct> loopBodyEmitter );

		/// <summary>
		/// 	Emits the for-each loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="collectionTraits">The traits of the collection.</param>
		/// <param name="collection">The collection reference.</param>
		/// <param name="expressionContext">The expression context.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes item reference then returns loop body construct.</param>
		/// <returns>The expression context after for each loop.</returns>
		protected abstract TConstruct EmitForEachLoop( TContext context, CollectionTraits collectionTraits, TConstruct collection, TConstruct expressionContext, ForEachLoopBodyFunc loopBodyEmitter );

		protected delegate TConstruct ForEachLoopBodyFunc(
			TConstruct currentItem, TConstruct expressionContext
		);

		private static ConstructorInfo GetCollectionConstructor( Type instanceType )
		{
			var ctor =
				instanceType.GetConstructor( CollectionConstructorWithCapacityParameterTypes )
				?? instanceType.GetConstructor( Type.EmptyTypes );

			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( instanceType );
			}

			return ctor;
		}

		private static ConstructorInfo GetDefaultConstructor()
		{
			var ctor = typeof( TObject ).GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( typeof( TObject ) );
			}

			return ctor;
		}
	}
}
