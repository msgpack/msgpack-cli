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
		/// 	Emits the method epiloigue.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="metadata">The metadata of the method.</param>
		/// <param name="constructs">The constructs which represent method statements in order. Null entry should be ignored.</param>
		protected abstract void EmitMethodEpilogue( TContext context, MethodInfo metadata, IList<TConstruct> constructs );

		/// <summary>
		/// 	Emits sequential statements and subsequent loadable expression which determines entire construct elementType.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="statement">The statement which is usually sequential statements.</param>
		/// <param name="contextExpression">The expresion which determines entire construct elementType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitStatementExpression( TContext context, TConstruct statement, TConstruct contextExpression );

		/// <summary>
		///		Emits anonymous <c>null</c> reference literal.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeNullLiteral( TContext context );

		/// <summary>
		///		Emits the constant <see cref="Int32"/> value reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt32Literal( TContext context, int constant );

		/// <summary>
		///		Emits the constant <see cref="Int64"/> value reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt64Literal( TContext context, long constant );

		/// <summary>
		///		Emits the constant <see cref="String"/> value reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeStringLiteral( TContext context, string constant );

		/// <summary>
		///		Emits the loading this reference expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitThisReferenceExpression( TContext context );

		/// <summary>
		///		Emits the box expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="valueType">Type of the value to be boxed.</param>
		/// <param name="value">The value to be boxed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitBoxExpression( TContext context, Type valueType, TConstruct value );

		/// <summary>
		///		Emits the not expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="booleanExpression">The boolean expression to be .</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitNotExpression( TContext context, TConstruct booleanExpression );

		/// <summary>
		///		Emits the equals expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitEqualsExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the not equals expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected virtual TConstruct EmitNotEqualsExpression( TContext context, TConstruct left, TConstruct right )
		{
			return this.EmitNotExpression( context, this.EmitEqualsExpression( context, left, right ) );
		}

		/// <summary>
		///		Emits the greater than expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGreaterThanExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the less than expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitLessThanExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the unary increment expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="int32Value">The int32 value to be incremented.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitIncrementExpression( TContext context, TConstruct int32Value );

		/// <summary>
		///		Emits the elementType-of expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The elementType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitTypeOfExpression( TContext context, Type type );

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected TConstruct EmitSequentialStatements( TContext context, params TConstruct[] statements )
		{
			return this.EmitSequentialStatements( context, statements as IEnumerable<TConstruct> );
		}

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSequentialStatements( TContext context, IEnumerable<TConstruct> statements );

		/// <summary>
		///		Declares the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The elementType of the variable.</param>
		/// <param name="name">The name of the variable for debugging puropose.</param>
		/// <param name="initExpression">The initilization expression.</param>
		/// <returns>
		///		The generated construct which represents local variable declaration AND initialization, and reference.
		/// </returns>
		protected abstract TConstruct DeclareLocal( TContext context, Type type, string name, TConstruct initExpression );

		/// <summary>
		///		Emits the create new object expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constructor">The constructor.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that created object remains in context.
		/// </returns>
		protected abstract TConstruct EmitCreateNewObjectExpression(
			TContext context, ConstructorInfo constructor, params TConstruct[] arguments
		);

		/// <summary>
		///		Emits the invoke void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct.
		/// </returns>
		/// <remarks>
		///		The derived class must emits codes which discard return non-void value.
		/// </remarks>
		protected abstract TConstruct EmitInvokeVoidMethod(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		);

		/// <summary>
		///		Emits the invoke non-void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected TConstruct EmitInvokeMethodExpression(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		)
		{
			return
				this.EmitInvokeMethodExpression(
					context, instance, method, ( arguments ?? new TConstruct[ 0 ] ) as IEnumerable<TConstruct>
				);
		}

		/// <summary>
		///		Emits the invoke non-void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected abstract TConstruct EmitInvokeMethodExpression( TContext context, TConstruct instance, MethodInfo method, IEnumerable<TConstruct> arguments );

		/// <summary>
		///		Emits the get member(field or property) value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="member">The member to be accessed.</param>
		/// <returns>The generated construct.</returns>
		private TConstruct EmitGetMemberValueExpression( TContext context, TConstruct instance, MemberInfo member )
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

		/// <summary>
		///		Emits the get property value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetPropretyExpression( TContext context, TConstruct instance, PropertyInfo property );

		/// <summary>
		///		Emits the get field value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetFieldExpression( TContext context, TConstruct instance, FieldInfo field );

		/// <summary>
		///		Emits the set member(property or field) value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="member">The member to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		This method generates <c>collection.Add(value)</c> constructs for a read-only member.
		/// </remarks>
		private TConstruct EmitSetMemberValueStatement( TContext context, TConstruct instance, MemberInfo member, TConstruct value )
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
				if ( asProperty.GetSetMethod( false ) != null )
				{
					return this.EmitSetProprety( context, instance, asProperty, value );
				}

				getCollection = this.EmitGetPropretyExpression( context, instance, asProperty );
				traits = asProperty.PropertyType.GetCollectionTraits();
			}

			// use Add(T) for appendable collection elementType read only member.

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
							current =>
								this.EmitAppendCollectionItem(
									context,
									traits,
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
							current =>
								this.EmitAppendDictionaryItem(
									context,
									traits,
									getCollection,
									keyType,
									this.EmitGetPropretyExpression(
										context,
										current,
										traits.ElementType == typeof( DictionaryEntry )
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
							"Member '{0}' is read only and its elementType ('{1}') is not an appendable collection",
							member.Name,
							member.GetMemberValueType()
						)
					);
				}
			}
		}

		/// <summary>
		///		Emits the set property value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetProprety( TContext context, TConstruct instance, PropertyInfo property, TConstruct value );

		/// <summary>
		///		Emits the set field value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetField( TContext context, TConstruct instance, FieldInfo field, TConstruct value );

		/// <summary>
		///		Emits the statement which stores context value to the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected TConstruct EmitSetVariableStatement( TContext context, TConstruct variable )
		{
			return this.EmitSetVariableStatement( context, variable, null );
		}

		/// <summary>
		///		Emits the statement which stores specified value to the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be stored.</param>
		/// <param name="value">The value to be stored. <c>null</c> for context value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetVariableStatement( TContext context, TConstruct variable, TConstruct value );

		/// <summary>
		///		Emits the throwing exception statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expressionType">Type of the entire expression elementType.</param>
		/// <param name="factoryMethod">The factory method which creates exception object with <paramref name="arguments"/>.</param>
		/// <param name="arguments">The arguments of <paramref name="factoryMethod"/>.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		The expression elementType is required to acomplish condition expression.
		/// </remarks>
		private TConstruct EmitThrowExpression( TContext context, Type expressionType, MethodInfo factoryMethod, params TConstruct[] arguments )
		{
			return this.EmitThrowExpression( context, expressionType, this.EmitInvokeMethodExpression( context, null, factoryMethod, arguments ) );
		}

		/// <summary>
		///		Emits the throwing exception statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expressionType">Type of the entire expression elementType.</param>
		/// <param name="exceptionExpression">The expression of throwing exception.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		The expression elementType is required to acomplish condition expression.
		/// </remarks>
		protected abstract TConstruct EmitThrowExpression( TContext context, Type expressionType, TConstruct exceptionExpression );

		/// <summary>
		///		Emits the try-finally expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="tryExpression">The try expression.</param>
		/// <param name="finallyStatement">The finally statement.</param>
		/// <returns>The generated construct which elementType is elementType of <paramref name="tryExpression"/>.</returns>
		protected abstract TConstruct EmitTryFinallyExpression(
			TContext context, TConstruct tryExpression, TConstruct finallyStatement
		);

		/// <summary>
		///		Emits the invariant <see cref="string.Format(IFormatProvider,string,object[])"/> with <see cref="CultureInfo.InvariantCulture"/>.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="format">The format string literal.</param>
		/// <param name="arguments">The arguments to be used.</param>
		/// <returns>The generated construct.</returns>
		private TConstruct EmitInvariantStringFormat( TContext context, string format, params TConstruct[] arguments )
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

		/// <summary>
		///		Emits the create new array expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="elementType">The elementType of the array element.</param>
		/// <param name="length">The length of the array.</param>
		/// <param name="initialElements">The initial elements of the array.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, Type elementType, int length, IEnumerable<TConstruct> initialElements
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
		/// <returns>The generated code construct.</returns>
		/// <remarks>
		///		The serializer reference methodology is implication specific.
		/// </remarks>
		protected abstract TConstruct EmitGetSerializerExpression( TContext context, Type targetType );

		/// <summary>
		/// Emits the pack item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="packer">The packer.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication of the member.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="item">The item to be packed.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitPackItemExpression( TContext context, TConstruct packer, Type itemType, NilImplication nilImplication, string memberName, TConstruct item )
		{
			var currentItem = item;

			switch ( nilImplication )
			{
				case NilImplication.Prohibit:
				{
					TConstruct condition = null;
					if ( itemType == typeof( MessagePackObject ) )
					{
						condition =
							this.EmitGetPropretyExpression( context, item, Metadata._MessagePackObject.IsNil );
					}
					else if ( !itemType.GetIsValueType() )
					{
						condition =
							this.EmitEqualsExpression(
								context,
								item,
								this.MakeNullLiteral( context )
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
								this.EmitThrowExpression(
									context,
									itemType,
									SerializationExceptions.NewNullIsProhibitedMethod,
									this.MakeStringLiteral( context, memberName )
								),
								item
							);
					}

					break;
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

		/// <summary>
		///		Emits the get items count expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="unpacker">The target unpacker.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitGetItemsCountExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetItemsCount,
					unpacker
				);
		}

		/// <summary>
		///		Emits the UnpackFrom body for collection which delegates UnpackTo.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="ctor">The constructor of the newly creating collection.</param>
		/// <param name="collectionCapacity">The newly creating collection capacity.</param>
		/// <param name="unpacker">The unpacker.</param>
		/// <returns>The generated code construct.</returns>
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
		///		Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="itemsCount">
		///		The reference to items count init-only local variable which remember unpackable items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="unpacked">
		///		The reference to unpacked items count local variable which remember unpacked items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="storeValueStatementEmitter">
		///		The delegate which generates statement for storing unpacked value.
		///		1st parameter is unpacked value, and return value is generated statement.
		/// </param>
		/// <returns>The sequential statement.</returns>
		protected TConstruct EmitUnpackItemValueExpression(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			Func<TConstruct, TConstruct> storeValueStatementEmitter
		)
		{
			return
				this.EmitSequentialStatements(
					context,
					this.EmitUnpackItemValueExpressionCore(
						context, itemType, nilImplication, unpacker, itemIndex, memberName, itemsCount, unpacked, storeValueStatementEmitter
					)
				);
		}

		/// <summary>
		///		Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="itemsCount">
		///		The reference to items count init-only local variable which remember unpackable items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="unpacked">
		///		The reference to unpacked items count local variable which remember unpacked items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="storeValueStatementEmitter">
		///		The delegate which generates statement for storing unpacked value.
		///		1st parameter is unpacked value, and return value is generated statement.
		/// </param>
		/// <returns>The elements of sequential statement.</returns>
		private IEnumerable<TConstruct> EmitUnpackItemValueExpressionCore(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			Func<TConstruct, TConstruct> storeValueStatementEmitter
		)
		{
			/*
				 *	T? nullable;
				 *	if ( unpacked < itemsCount )
				 *	{
				 *		if ( !unpacker.Read() )
				 *		{
				 *			throw SerializationExceptiuons.MissingItem(...);
				 *		}
				 *	
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

			// is nilable natually?
			var isNativelyNullable =
				itemType == typeof( MessagePackObject )
				|| !itemType.GetIsValueType()
				|| Nullable.GetUnderlyingType( itemType ) != null;

			// type of temp var for nil implication
			var nullableType = itemType;
			if ( !isNativelyNullable )
			{
				nullableType = typeof( Nullable<> ).MakeGenericType( itemType );
			}

			// temp var for nil implication
			var nullable =
				this.DeclareLocal(
					context,
					nullableType,
					"nullable" + itemIndex,
					null
				);

			// unpacking item instruction.
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
						this.EmitSetVariableStatement( context, nullable )
					),
					this.EmitUsingStatement(
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
								this.EmitSetVariableStatement( context, nullable )
							)
					)
				);

			// Unpacked item after nil implication.
			var unpackedItem =
				( Nullable.GetUnderlyingType( nullable.ContextType ) != null
				  && Nullable.GetUnderlyingType( itemType ) == null )
					? this.EmitGetPropretyExpression( context, nullable, nullable.ContextType.GetProperty( "Value" ) )
					: nullable;

			// Nil Implication
			TConstruct expressionWhenNil;
			switch ( nilImplication )
			{
				case NilImplication.MemberDefault:
				{
					expressionWhenNil = null;
					break;
				}
				case NilImplication.Prohibit:
				{
					expressionWhenNil =
						this.EmitThrowExpression(
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
							this.EmitThrowExpression(
								context,
								typeof( void ),
								SerializationExceptions.NewValueTypeCannotBeNull3Method,
								memberName,
								this.EmitTypeOfExpression( context, itemType ),
								this.EmitTypeOfExpression( context, typeof( TObject ) )
							);
					}
					else
					{
						expressionWhenNil =
							this.EmitSequentialStatements(
								context,
								storeValueStatementEmitter( unpackedItem )
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

			// compose read inst. and unpack inst. now.
			var readAndUnpack =
				this.EmitSequentialStatements(
					context,
					this.EmitConditionalExpression(
						context,
						this.EmitNotExpression(
							context,
							this.EmitInvokeMethodExpression( context, unpacker, Metadata._Unpacker.Read )
						),
						this.EmitThrowExpression(
							context, typeof( Unpacker ), SerializationExceptions.NewMissingItemMethod, itemIndex
						),
						unpacker
					),
					unpack
				);

			// actually declare local now.
			yield return nullable;

			// Missing member is treated as nil
			if ( unpacked != null )
			{
#if DEBUG
				Contract.Assert( itemsCount != null );
#endif
				yield return
					this.EmitConditionalExpression(
						context,
						this.EmitLessThanExpression(
							context,
							unpacked,
							itemsCount
						),
						readAndUnpack,
						null // if-then, no else -- nil is set to temp var.
					);
			}
			else
			{
				yield return readAndUnpack;
			}

			// compose nil implication and setting real var.
			yield return
				this.EmitConditionalExpression(
					context,
					( !isNativelyNullable || Nullable.GetUnderlyingType( itemType ) != null )
						? this.EmitGetPropretyExpression( context, nullable, nullableType.GetProperty( "HasValue" ) )
						: itemType == typeof( MessagePackObject )
						? this.EmitNotExpression(
							context,
							this.EmitGetPropretyExpression( context, nullable, Metadata._MessagePackObject.IsNil )
						) : this.EmitNotEqualsExpression( context, nullable, this.MakeNullLiteral( context ) ),
					storeValueStatementEmitter( unpackedItem ),
					expressionWhenNil
				);

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

		/// <summary>
		///		Emits the using expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="disposableType">Type of the disposable.</param>
		/// <param name="instantiateIDisposableExpression">The expression which instantiate <see cref="IDisposable"/> object.</param>
		/// <param name="usingBodyEmitter">The using body which takes disposable object and returns actual statement.</param>
		/// <returns>The using statement.</returns>
		private TConstruct EmitUsingStatement(
			TContext context, Type disposableType, TConstruct instantiateIDisposableExpression, Func<TConstruct, TConstruct> usingBodyEmitter
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
						usingBodyEmitter( disposable ),
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

		/// <summary>
		///		Emits the deserialize item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="unpacker">The unpacker expression.</param>
		/// <param name="itemType">Type of the item to be deserialized.</param>
		/// <returns>The expression which returns deserialized item.</returns>
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
		/// <returns>The conditional expression.</returns>
		protected abstract TConstruct EmitConditionalExpression(
			TContext context,
			TConstruct conditionExpression,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		/// <summary>
		///		Emits the conditional expression (cond?then:else) which has short circuit and expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="conditionExpressions">The expression which represents short circuit and expression.</param>
		/// <param name="thenExpression">The expression which is used when condition is true.</param>
		/// <param name="elseExpression">The expression which is used when condition is false.</param>
		/// <returns>The conditional expression.</returns>
		protected abstract TConstruct EmitAndConditionalExpression(
			TContext context,
			IList<TConstruct> conditionExpressions,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		/// <summary>
		///		Emits string switch statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="target">Target string expression.</param>
		/// <param name="cases">The case statements. The keys are case condition, and values are actual statement.</param>
		/// <returns>The switch statement.</returns>
		protected abstract TConstruct EmitStringSwitchStatement(
			TContext context, TConstruct target, IDictionary<string, TConstruct> cases
		);

		/// <summary>
		/// 	Emits the for loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="count">The count reference for loop terminiation condition.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes for loop context then returns loop body construct.</param>
		/// <returns>The for loop.</returns>
		protected abstract TConstruct EmitForLoop( TContext context, TConstruct count, Func<ForLoopContext, TConstruct> loopBodyEmitter );

		/// <summary>
		/// 	Emits the for-each loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="collectionTraits">The traits of the collection.</param>
		/// <param name="collection">The collection reference.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes item reference then returns loop body construct.</param>
		/// <returns>The for each loop.</returns>
		protected abstract TConstruct EmitForEachLoop(
			TContext context,
			CollectionTraits collectionTraits,
			TConstruct collection,
			Func<TConstruct, TConstruct> loopBodyEmitter
		);

		/// <summary>
		///		Retrieves a default constructor of the specified elementType.
		/// </summary>
		/// <param name="instanceType">The target elementType.</param>
		/// <returns>A default constructor of the <paramref name="instanceType"/>.</returns>
		private static ConstructorInfo GetDefaultConstructor( Type instanceType )
		{
			var ctor = typeof( TObject ).GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( instanceType );
			}

			return ctor;
		}

		/// <summary>
		///		Retrieves a constructor with <see cref="Int32"/> elementType capacity parameter or default constructor of the <typeparamref name="TObject"/>.
		/// </summary>
		/// <param name="instanceType">The target elementType.</param>
		/// <returns>A constructor of the <paramref name="instanceType"/>.</returns>
		private static ConstructorInfo GetCollectionConstructor( Type instanceType )
		{
			var ctor =
				instanceType.GetConstructor( SerializerBuilderConstants.CollectionConstructorWithCapacityParameterTypes )
				?? instanceType.GetConstructor( Type.EmptyTypes );

			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( instanceType );
			}

			return ctor;
		}
	}
}
