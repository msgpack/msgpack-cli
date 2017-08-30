#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and constirbutors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

using System;
using System.Collections;
using System.Collections.Generic;
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct>
	{
		private static readonly TConstruct[] NoConstructs = new TConstruct[ 0 ];

		#region -- Literal --

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Well patterned" )]
		private TConstruct MakeDefaultParameterValueLiteral( TContext context, TConstruct targetVariable, Type literalType, object literal, bool hasDefault )
		{
			// Supports only C# literals
			if ( literalType == typeof( byte ) )
			{
				return this.MakeByteLiteral( context, !hasDefault ? default( byte ) : ( byte )literal );
			}

			if ( literalType == typeof( sbyte ) )
			{
				return this.MakeSByteLiteral( context, !hasDefault ? default( sbyte ) : ( sbyte )literal );
			}

			if ( literalType == typeof( short ) )
			{
				return this.MakeInt16Literal( context, !hasDefault ? default( short ) : ( short )literal );
			}

			if ( literalType == typeof( ushort ) )
			{
				return this.MakeUInt16Literal( context, !hasDefault ? default( ushort ) : ( ushort )literal );
			}

			if ( literalType == typeof( int ) )
			{
				return this.MakeInt32Literal( context, !hasDefault ? default( int ) : ( int )literal );
			}

			if ( literalType == typeof( uint ) )
			{
				return this.MakeUInt32Literal( context, !hasDefault ? default( uint ) : ( uint )literal );
			}

			if ( literalType == typeof( long ) )
			{
				return this.MakeInt64Literal( context, !hasDefault ? default( long ) : ( long )literal );
			}

			if ( literalType == typeof( ulong ) )
			{
				return this.MakeUInt64Literal( context, !hasDefault ? default( ulong ) : ( ulong )literal );
			}

			if ( literalType == typeof( float ) )
			{
				return this.MakeReal32Literal( context, !hasDefault ? default( float ) : ( float )literal );
			}

			if ( literalType == typeof( double ) )
			{
				return this.MakeReal64Literal( context, !hasDefault ? default( double ) : ( double )literal );
			}

			if ( literalType == typeof( decimal ) )
			{
				return this.MakeDecimalLiteral( context, targetVariable, !hasDefault ? default( decimal ) : ( decimal )literal );
			}

			if ( literalType == typeof( bool ) )
			{
				return this.MakeBooleanLiteral( context, hasDefault && ( bool )literal );
			}

			if ( literalType == typeof( char ) )
			{
				return this.MakeCharLiteral( context, !hasDefault ? default( char ) : ( char )literal );
			}

			if ( literalType.GetIsEnum() )
			{
				return this.MakeEnumLiteral( context, literalType, !hasDefault ? Enum.ToObject( literalType, 0 ) : literal );
			}

			if ( literal != null && hasDefault )
			{
				if ( literalType == typeof( string ) )
				{
					return this.MakeStringLiteral( context, literal as string );
				}

				// Unknown literal.
				if ( literalType.GetIsValueType() )
				{
					throw new NotSupportedException(
						String.Format( CultureInfo.CurrentCulture, "Literal for value type '{0}' is not supported.", literalType )
					);
				}
				else
				{
					throw new NotSupportedException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Literal for reference type '{0}' is not supported except null reference.",
							literalType
						)
					);
				}
			}
			else if ( literalType.GetIsValueType() )
			{
				return this.MakeDefaultLiteral( context, literalType );
			}
			else
			{
				return this.MakeNullLiteral( context, literalType );
			}
		}

		// for C# decimal opt parameter ... [DecimalConstant(...)]
		private TConstruct MakeDecimalLiteral( TContext context, TConstruct targetVariable, decimal constant )
		{
			var bits = Decimal.GetBits( constant );
			return
				this.EmitCreateNewObjectExpression(
					context,
					targetVariable,
					Metadata._Decimal.Constructor,
					this.MakeInt32Literal( context, bits[ 0 ] ), // lo
					this.MakeInt32Literal( context, bits[ 1 ] ), // mid
					this.MakeInt32Literal( context, bits[ 2 ] ), // high
					this.MakeBooleanLiteral( context, ( bits[ 3 ] & 0x80000000 ) != 0 ), // sign
					this.MakeByteLiteral( context, unchecked( ( byte )( bits[ 3 ] >> 16 & 0xFF ) ) ) // scale
				);
		}

		/// <summary>
		///		Emits anonymous <c>null</c> reference literal.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of null reference.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeNullLiteral( TContext context, TypeDefinition contextType );

		/// <summary>
		///		Emits the constant <see cref="Byte"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeByteLiteral( TContext context, byte constant );

		/// <summary>
		///		Emits the constant <see cref="SByte"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeSByteLiteral( TContext context, sbyte constant );

		/// <summary>
		///		Emits the constant <see cref="Int16"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt16Literal( TContext context, short constant );

		/// <summary>
		///		Emits the constant <see cref="UInt16"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt16Literal( TContext context, ushort constant );

		/// <summary>
		///		Emits the constant <see cref="Int32"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt32Literal( TContext context, int constant );

		/// <summary>
		///		Emits the constant <see cref="UInt32"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt32Literal( TContext context, uint constant );

		/// <summary>
		///		Emits the constant <see cref="Int64"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt64Literal( TContext context, long constant );

		/// <summary>
		///		Emits the constant <see cref="UInt64"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt64Literal( TContext context, ulong constant );

		/// <summary>
		///		Emits the constant <see cref="Single"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeReal32Literal( TContext context, float constant );

		/// <summary>
		///		Emits the constant <see cref="Double"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeReal64Literal( TContext context, double constant );

		/// <summary>
		///		Emits the constant <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeBooleanLiteral( TContext context, bool constant );

		/// <summary>
		///		Emits the constant <see cref="Char"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeCharLiteral( TContext context, char constant );

		/// <summary>
		///		Emits the constant <see cref="String"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeStringLiteral( TContext context, string constant );

		/// <summary>
		///		Emits the constant enum value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the enum.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		/// <exception cref="ArgumentException"><paramref name="type"/> is not enum.</exception>
		protected abstract TConstruct MakeEnumLiteral( TContext context, TypeDefinition type, object constant ); // boxing is better than complex unboxing issue

		/// <summary>
		///		Emits the constant default(T) value of value type.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the valueType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeDefaultLiteral( TContext context, TypeDefinition type );

		#endregion -- Literal --

		#region -- This --

		/// <summary>
		///		Emits the loading this reference expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitThisReferenceExpression( TContext context );

		#endregion -- This --

		#region -- Boxing/Unboxing/Cast --

		/// <summary>
		///		Emits the box expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="valueType">Type of the value to be boxed.</param>
		/// <param name="value">The value to be boxed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitBoxExpression( TContext context, TypeDefinition valueType, TConstruct value );

		/// <summary>
		///		Emits the cast or unbox expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="targetType">Type of the value to be casted or be unboxed.</param>
		/// <param name="value">The value to be casted or be unboxed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitUnboxAnyExpression( TContext context, TypeDefinition targetType, TConstruct value );

		private TConstruct BoxIfRequired( TContext context, TConstruct instance )
		{
			if ( instance.ContextType.ResolveRuntimeType().GetIsValueType() )
			{
				return this.EmitBoxExpression( context, instance.ContextType, instance );
			}
			else
			{
				return instance;
			}
		}

		#endregion -- Boxing/Unboxing/Cast --

		#region -- Operations --

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
		protected abstract TConstruct EmitIncrement( TContext context, TConstruct int32Value );

		/// <summary>
		///		Emits the elementType-of expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The elementType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitTypeOfExpression( TContext context, TypeDefinition type );

		/// <summary>
		///		Emits the 'methodof' expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The method to be retrieved.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitMethodOfExpression( TContext context, MethodBase method );

		/// <summary>
		///		Emits the 'fieldof' expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="field">The field to be retrieved.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitFieldOfExpression( TContext context, FieldInfo field );

		/// <summary>
		///		Emits the 'throw' statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="exception">The expression to returns an exception.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitThrowStatement( TContext context, TConstruct exception );

		#endregion -- Operations --

		#region -- Aggregation --

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected TConstruct EmitSequentialStatements( TContext context, TypeDefinition contextType, params TConstruct[] statements )
		{
			return this.EmitSequentialStatements( context, contextType, statements as IEnumerable<TConstruct> );
		}

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSequentialStatements( TContext context, TypeDefinition contextType, IEnumerable<TConstruct> statements );

		#endregion -- Aggregation --

		#region -- Args/Locals --

		/// <summary>
		///		Creates the argument reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the parameter for debugging puropose.</param>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="index">The index of the parameters.</param>
		/// <returns>
		///		The generated construct which represents an argument reference.
		/// </returns>
		protected abstract TConstruct ReferArgument( TContext context, TypeDefinition type, string name, int index );

		/// <summary>
		///		Declares the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the variable.</param>
		/// <param name="name">The name of the variable for debugging puropose.</param>
		/// <returns>
		///		The generated construct which represents local variable declaration AND initialization, and reference.
		/// </returns>
		protected abstract TConstruct DeclareLocal( TContext context, TypeDefinition type, string name );

		/// <summary>
		///		Emits the statement which loads value from the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be loaded.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitLoadVariableExpression( TContext context, TConstruct variable );

		/// <summary>
		///		Emits the statement which stores specified value to the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be stored.</param>
		/// <param name="value">The value to be stored. <c>null</c> for context value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitStoreVariableStatement( TContext context, TConstruct variable, TConstruct value );

		/// <summary>
		///		Emits the make ref instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="target">The target to be made its managed reference.</param>
		/// <returns>The managed reference of the <paramref name="target"/>.</returns>
		protected abstract TConstruct EmitMakeRef( TContext context, TConstruct target );

		#endregion -- Args/Locals --

		#region -- New --

		/// <summary>
		///		Emits the create new object expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable which will store created value type object.</param>
		/// <param name="constructor">The constructor.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that created object remains in context.
		/// </returns>
		protected abstract TConstruct EmitCreateNewObjectExpression(
			TContext context, TConstruct variable, ConstructorDefinition constructor, params TConstruct[] arguments
		);

		#endregion -- New --

		#region -- Array --

		/// <summary>
		///		Emits the create new array expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="elementType">The elementType of the array element.</param>
		/// <param name="length">The length of the array.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, TypeDefinition elementType, int length
		);

		/// <summary>
		///		Emits the create new array expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="elementType">The elementType of the array element.</param>
		/// <param name="length">The length of the array.</param>
		/// <param name="initialElements">The initial elements of the array.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, TypeDefinition elementType, int length, IEnumerable<TConstruct> initialElements
		);

		/// <summary>
		///		Emits the get array element expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="array">The array to be gotten.</param>
		/// <param name="index">The index of the array element to be gotten.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitGetArrayElementExpression( TContext context, TConstruct array, TConstruct index );

		/// <summary>
		/// 	Emits the set array element statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="array">The array to be set.</param>
		/// <param name="index">The index of the array element to be set.</param>
		/// <param name="value">The value to be set.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitSetArrayElementStatement( TContext context, TConstruct array, TConstruct index, TConstruct value );

		#endregion -- Array --

		#region -- Branch --

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

		#endregion -- Branch --

		#region -- Code Structures --

		/// <summary>
		/// 	Emits the return statement
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expression">The expression to be returned.</param>
		/// <returns>The return statement.</returns>
		protected virtual TConstruct EmitRetrunStatement( TContext context, TConstruct expression )
		{
			return expression;
		}

		/// <summary>
		///		Emits the try-finally expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="tryStatement">The try expression.</param>
		/// <param name="finallyStatement">The finally statement.</param>
		/// <returns>The generated construct which elementType is elementType of <paramref name="tryStatement"/>.</returns>
		protected abstract TConstruct EmitTryFinally(
			TContext context, TConstruct tryStatement, TConstruct finallyStatement
		);

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


		#endregion -- Code Structures --
	
		#region -- Invoke --

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
			TContext context, TConstruct instance, MethodDefinition method, params TConstruct[] arguments
		);

		/// <summary>
		///		Emits the invoke non-void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents method call instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected TConstruct EmitInvokeMethodExpression(
			TContext context, TConstruct instance, MethodDefinition method, params TConstruct[] arguments
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
		///		The generated construct which represents method call instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected abstract TConstruct EmitInvokeMethodExpression( TContext context, TConstruct instance, MethodDefinition method, IEnumerable<TConstruct> arguments );

		/// <summary>
		///		Emits the invoke non-void delegate.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="delegateReturnType">The return type of the delegate.</param>
		/// <param name="delegate">The delegate to be invocation.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents delegate invocation instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected abstract TConstruct EmitInvokeDelegateExpression( TContext context, TypeDefinition delegateReturnType, TConstruct @delegate, params TConstruct[] arguments );

		#endregion -- Invoke --

		#region -- Private Method --

		/// <summary>
		///		Emits specified body as individual private method and returns delegate for it. 
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="name">The name of the private method.</param>
		/// <param name="isStatic"><c>true</c> for static method.</param>
		/// <param name="returnType">The type of return value.</param>
		/// <param name="bodyFactory">The delegate to the factory which returns body of the private method.</param>
		/// <param name="parameters">The parameters of the private method.</param>
		/// <returns>
		///		The generated construct which represents delegate creation instruction to call the private method.
		///		Note that returned value remains in context.
		/// </returns>
		private TConstruct ExtractPrivateMethod( TContext context, string name, bool isStatic, TypeDefinition returnType, Func<TConstruct> bodyFactory, params TConstruct[] parameters )
		{
			return this.EmitGetPrivateMethodDelegateExpression( context, DefinePrivateMethod( context, name, isStatic, returnType, bodyFactory, parameters ) );
		}

		/// <summary>
		///		Emits specified body as individual private method and returns its metadata. 
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="name">The name of the private method.</param>
		/// <param name="isStatic"><c>true</c> for static method.</param>
		/// <param name="returnType">The type of return value.</param>
		/// <param name="bodyFactory">The delegate to the factory which returns body of the private method.</param>
		/// <param name="parameters">The parameters of the private method.</param>
		/// <returns>
		///		The generated metadata of the private method.
		/// </returns>
		private static MethodDefinition DefinePrivateMethod( TContext context, string name, bool isStatic, TypeDefinition returnType, Func<TConstruct> bodyFactory, params TConstruct[] parameters )
		{
			if ( context.IsDeclaredMethod( name ) )
			{
				return context.GetDeclaredMethod( name );
			}
			else
			{
				context.BeginPrivateMethod(
					name,
					isStatic,
					returnType,
					parameters
				);

				return context.EndPrivateMethod( name, bodyFactory() );
			}
		}

		protected virtual TConstruct EmitGetPrivateMethodDelegateExpression( TContext context, MethodDefinition method )
		{
			return
				this.EmitGetFieldExpression(
					context,
					this.EmitThisReferenceExpression( context ),
					context.GetCachedPrivateMethodDelegate( method, SerializerBuilderHelper.GetDelegateTypeDefinition( method.ReturnType, method.ParameterTypes ) )
				);
		}


		/// <summary>
		///		Emits delegate instantiation for specified named private instance or static method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The information of the private method.</param>
		/// <returns>
		///		The generated construct which represents delegate creation instruction to call the specified private method.
		///		Note that returned value remains in context.
		/// </returns>
		protected abstract TConstruct EmitNewPrivateMethodDelegateExpression( TContext context, MethodDefinition method );

		#endregion -- Private Method --

		#region -- Static Delegate --

		/// <summary>
		///		Emits getting cached delegate for specified named static method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The information of the static method.</param>
		/// <returns>
		///		The generated construct which represents delegate creation instruction to call the specified method.
		///		Note that returned value remains in context.
		/// </returns>
		protected virtual TConstruct EmitGetStaticDelegateExpression( TContext context, MethodDefinition method )
		{
			return 
				this.EmitGetFieldExpression(
					context,
					this.EmitThisReferenceExpression( context ),
					context.GetCachedStaticMethodDelegate(
						method,
						SerializerBuilderHelper.GetDelegateTypeDefinition( method.ReturnType, method.ParameterTypes )
					)
				);
		}

		#endregion -- Static Delegate --

		#region -- Get Helpers --

		/// <summary>
		///		Emits the get member(field or property) value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="member">The member to be accessed.</param>
		/// <returns>The generated construct.</returns>
		private TConstruct EmitGetMemberValueExpression( TContext context, TConstruct instance, MemberInfo member )
		{
			FieldInfo asField;
			if ( ( asField = member as FieldInfo ) != null )
			{
				return this.EmitGetField( context, instance, asField, !asField.GetHasPublicGetter() );
			}
			else
			{
				var asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				return this.EmitGetProperty( context, instance, asProperty, !asProperty.GetHasPublicGetter() );
			}
		}

		private TConstruct EmitGetProperty( TContext context, TConstruct instance, PropertyInfo property, bool withReflection )
		{
			if ( !withReflection )
			{
				return this.EmitGetPropertyExpression( context, instance, property );
			}

			/*
			 * return _method_of(m).Invoke( instance, null );
			 */
			return
				this.EmitUnboxAnyExpression(
					context,
					property.PropertyType,
					this.EmitInvokeMethodExpression(
						context,
						this.EmitMethodOfExpression( context, property.GetGetMethod( true ) ),
						Metadata._MethodBase.Invoke_2,
						instance,
						this.MakeNullLiteral( context, TypeDefinition.ObjectArrayType )
					)
				);
		}

		/// <summary>
		///		Emits the get property value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetPropertyExpression( TContext context, TConstruct instance, PropertyInfo property );

		private TConstruct EmitGetField( TContext context, TConstruct instance, FieldDefinition field, bool withReflection )
		{
			if ( !withReflection )
			{
				return this.EmitGetFieldExpression( context, instance, field );
			}

			/*
			 * _field_of(f).GetValue( instance );
			 */
			return
				this.EmitUnboxAnyExpression(
					context,
					field.ResolveRuntimeField().FieldType,
					this.EmitInvokeMethodExpression(
						context,
						this.EmitFieldOfExpression( context, field.ResolveRuntimeField() ),
						Metadata._FieldInfo.GetValue,
						this.BoxIfRequired( context, instance )
					)
				);
		}

		/// <summary>
		///		Emits the get field value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetFieldExpression( TContext context, TConstruct instance, FieldDefinition field );

		#endregion -- Get Helpers --

		#region -- Set Helpers --

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
			PropertyInfo asProperty = null;
			if ( ( asField = member as FieldInfo ) != null )
			{
				if ( !asField.IsInitOnly && asField.GetIsPublic() )
				{
					return this.EmitSetField( context, instance, asField, value, false );
				}

				getCollection = this.EmitGetField( context, instance, asField, !asField.GetHasPublicGetter() );
				traits = asField.FieldType.GetCollectionTraits( CollectionTraitOptions.Full, context.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			}
			else
			{
				asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				var setter = asProperty.GetSetMethod( true );
				if ( setter != null && setter.GetIsPublic() )
				{
					return this.EmitSetProperty( context, instance, asProperty, value, false );
				}

				getCollection = this.EmitGetProperty( context, instance, asProperty, !asProperty.GetHasPublicGetter() );
				traits = asProperty.PropertyType.GetCollectionTraits( CollectionTraitOptions.Full, context.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			}

			var existent = this.DeclareLocal( context, member.GetMemberValueType(), "existent" );
			var existentInitialization = this.EmitStoreVariableStatement( context, existent, getCollection );

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
					var store =
						this.EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
							context,
							instance,
							value,
							existent,
							member.GetMemberValueType(),
							asField,
							asProperty,
							traits.AddMethod == null
								? null
								: this.EmitForEachLoop(
									context,
									traits,
									value,
									current =>
										this.EmitAppendCollectionItem(
											context,
											member,
											traits,
											existent,
											current
											)
									)
							);
					return
						this.EmitSequentialStatements(
							context,
							store.ContextType,
							existent,
							existentInitialization,
							store
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

					var store =
							this.EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
								context,
								instance,
								value,
								existent,
								member.GetMemberValueType(),
								asField,
								asProperty,
								traits.AddMethod == null
									? null
									: this.EmitForEachLoop(
										context,
										traits,
										value,
										// ReSharper disable ImplicitlyCapturedClosure
										current =>
											this.EmitAppendDictionaryItem(
												context,
												traits,
												existent,
												keyType,
												this.EmitGetPropertyExpression(
													context,
													current,
#if !NETFX_CORE
													traits.ElementType == typeof( DictionaryEntry )
														? Metadata._DictionaryEntry.Key
														: traits.ElementType.GetProperty( "Key" )
#else
													traits.ElementType.GetProperty( "Key" )
#endif // !NETFX_CORE
												),
												valueType,
												this.EmitGetPropertyExpression(
													context,
													current,
#if !NETFX_CORE
													traits.ElementType == typeof( DictionaryEntry )
														? Metadata._DictionaryEntry.Value
														: traits.ElementType.GetProperty( "Value" )
#else
													traits.ElementType.GetProperty( "Value" )
#endif // !NETFX_CORE
												),
												false
											)
										// ReSharper restore ImplicitlyCapturedClosure
									)
							);

					return
						this.EmitSequentialStatements(
							context,
							store.ContextType,
							existent,
							existentInitialization,
							store
						);
				}
			}

			// Try use reflection
			if ( asField != null )
			{
				return this.EmitSetField( context, instance, asField, value, true );
			}

			if ( asProperty.GetSetMethod( true ) != null )
			{
				return this.EmitSetProperty( context, instance, asProperty, value, true );
			}

			throw new SerializationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Member '{0}' is read only and its elementType ('{1}') is not an appendable collection",
					member.Name,
					member.GetMemberValueType()
				)
			);
		}

		private TConstruct EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
			TContext context,
			TConstruct instance,
			TConstruct collection,
			TConstruct existent,
			TypeDefinition collectionType,
			FieldInfo asField,
			PropertyInfo asProperty,
			TConstruct storeCollectionItems // null for not appendable like String
		)
		{
			if ( storeCollectionItems != null && ( asField != null && asField.IsInitOnly ) || ( asProperty != null && asProperty.GetSetMethod( true ) == null ) )
			{
				return storeCollectionItems;
			}

			/*
			 * #if APPENDABLE
			 *	if ( instance.MEMBER == null )
			 *  {
			 *		instance.MEMBER = collection:
			 *  }
			 *  else
			 *  {
			 *		(APPANED)
			 *  }
			 * #else
			 *  instance.MEMBER = collection:
			 */

			var invokeSetter =
				asField != null
					? this.EmitSetField( context, instance, asField, collection, !asField.GetHasPublicSetter() )
					: this.EmitSetProperty( context, instance, asProperty, collection, !asProperty.GetHasPublicSetter() );

			if ( storeCollectionItems == null )
			{
				return invokeSetter;
			}

			return
				this.EmitConditionalExpression(
					context,
					this.EmitEqualsExpression(
						context,
						existent,
						this.MakeNullLiteral( context, collectionType )
					),
					invokeSetter, // then
					storeCollectionItems // else
				);
		}

		private TConstruct EmitSetProperty(
			TContext context,
			TConstruct instance,
			PropertyInfo property,
			TConstruct value,
			bool withReflection
		)
		{
			if ( !withReflection )
			{
				return this.EmitSetProperty( context, instance, property, value );
			}

			/*
			 * _method_of(m).Invoke( instance, new object[]{ value } );
			 */
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitMethodOfExpression( context, property.GetSetMethod( true ) ),
					Metadata._MethodBase.Invoke_2,
					instance,
					this.EmitCreateNewArrayExpression(
						context,
						TypeDefinition.ObjectType,
						1,
						new[]
						{
							value.ContextType.IsValueType
							? this.EmitBoxExpression( context, value.ContextType, value )
							: value
						}
					)
				);
		}

		/// <summary>
		///		Emits the set property value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetProperty( TContext context, TConstruct instance, PropertyInfo property, TConstruct value );

		/// <summary>
		///		Emits the set indexer property statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="declaringType">The type which defines the property.</param>
		/// <param name="proeprtyName">The name of the property to be accessed.</param>
		/// <param name="key">The key to be stored.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetIndexedProperty( TContext context, TConstruct instance, TypeDefinition declaringType, string proeprtyName, TConstruct key, TConstruct value );

		private TConstruct EmitSetField(
			TContext context,
			TConstruct instance,
			FieldDefinition field,
			TConstruct value,
			bool withReflection
		)
		{
			if ( !withReflection )
			{
				return this.EmitSetField( context, instance, field, value );
			}
			else if ( instance.ContextType.ResolveRuntimeType().GetIsValueType() )
			{
				return this.EmitSetFieldOnValueType( context, instance, field, value );
			}
			else
			{
				return this.EmitSetFieldOnReferenceType( context, instance, field, value );
			}
		}

		private TConstruct EmitSetFieldOnReferenceType(
			TContext context,
			TConstruct instance,
			FieldDefinition field,
			TConstruct value
		)
		{
			/*
			 * _field_of(f).SetValue( instance, value );
			 */
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitFieldOfExpression( context, field.ResolveRuntimeField() ),
					Metadata._FieldInfo.SetValue,
					instance,
					this.BoxIfRequired( context, value )
				);
		}

		private TConstruct EmitSetFieldOnValueType(
			TContext context,
			TConstruct instance,
			FieldDefinition field,
			TConstruct value
		)
		{
			// SetValue takes in an object for the first parameter, meaning that
			// the value gets boxed and, therefore, copied. Since it will set
			// the field value on the boxed copy, the changes won't be visible
			// after the call, hence the need to box/unbox it.
			string boxedName = context.GetUniqueVariableName( "boxed" );
			TConstruct boxed = this.DeclareLocal( context, typeof(object), boxedName );
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),

					// object boxed = instance;
					boxed,
					this.EmitStoreVariableStatement( context, boxed, this.EmitBoxExpression( context, instance.ContextType, instance ) ),

					// _field_of(f).SetValue( boxed, value );
					this.EmitInvokeVoidMethod(
						context,
						this.EmitFieldOfExpression( context, field.ResolveRuntimeField() ),
						Metadata._FieldInfo.SetValue,
						boxed,
						this.BoxIfRequired( context, value )),

					// instance = (T)boxed;
					this.EmitStoreVariableStatement( context, instance, this.EmitUnboxAnyExpression( context, instance.ContextType, boxed ) )
				);
		}

		/// <summary>
		///		Emits the set field value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetField( TContext context, TConstruct instance, FieldDefinition field, TConstruct value );

		/// <summary>
		///		Emits the set field value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="nestedType">The nested type definition of the instance which stores instance member value.</param>
		/// <param name="fieldName">The name of the field to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetField( TContext context, TConstruct instance, TypeDefinition nestedType, string fieldName, TConstruct value );

		#endregion -- Set Helpers --

		#region -- Pack Constructs --

		/// <summary>
		/// Emits the pack item statements.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="packer">The packer.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication of the member.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="item">The item to be packed.</param>
		/// <param name="memberInfo">The metadata of packing member. <c>null</c> for non-object member (collection or tuple items).</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <param name="isAsync"><c>true</c> for async.</param>
		/// <returns>The generated code construct.</returns>
		private IEnumerable<TConstruct> EmitPackItemStatements(
			TContext context,
			TConstruct packer,
			Type itemType,
			NilImplication nilImplication,
			string memberName,
			TConstruct item,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema,
			bool isAsync
		)
		{
			var nilImplicationConstruct =
				this._nilImplicationHandler.OnPacking(
					new SerializerBuilderOnPackingParameter( this, context, item, itemType, memberName ),
					nilImplication
				);
			if ( nilImplicationConstruct != null )
			{
				yield return nilImplicationConstruct;
			}

			/*
			 * this._serializerN.PackTo(packer, item);
			 */
			yield return this.EmitSerializeItemExpressionCore( context, packer, itemType, item, memberInfo, itemsSchema, isAsync );
		}

		/// <summary>
		/// Emits the pack item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="packer">The packer.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="item">The item to be packed.</param>
		/// <param name="memberInfo">The metadata of packing member. <c>null</c> for non-object member (collection or tuple items).</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <param name="isAsync"><c>true</c> for async.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitSerializeItemExpressionCore(
			TContext context,
			TConstruct packer,
			Type itemType,
			TConstruct item,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema,
			bool isAsync
		)
		{
			var methodName = isAsync ? "PackToAsync" : "PackTo";
			var arguments =
#if FEATURE_TAP
				isAsync ? new [] { packer, item, this.ReferCancellationToken( context, 3 ) } :
#endif // FEATURE_TAP
				new [] { packer, item };
			var serializerType = typeof( MessagePackSerializer<> ).MakeGenericType( itemType );
			var getSerializer = this.EmitGetSerializerExpression( context, itemType, memberInfo, itemsSchema );
			var method =
				serializerType
				.GetMethods()
				.Single( m =>
					m.Name == methodName
					&& m.DeclaringType == serializerType
					&& !m.IsStatic
					&& m.IsPublic
					&& m.GetParameters().Length == arguments.Length
				);
			return
				isAsync
					? this.EmitRetrunStatement( context, this.EmitInvokeMethodExpression( context, getSerializer, method, arguments ) )
					: this.EmitInvokeVoidMethod( context, getSerializer, method, arguments );
		}

		#endregion -- Pack Constructs --

		#region -- Unpack Constructs --

		private TConstruct EmitUnpackItemValueStatement(
			TContext context,
			Type memberType,
			TConstruct memberName,
			NilImplication nilImplication,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema,
			TConstruct unpacker,
			TConstruct unpackingContext,
			TConstruct indexOfItem,
			TConstruct countOfItem,
			TConstruct setterDelegate,
			bool isAsync
		)
		{
			var typeKind =
				memberType == typeof( MessagePackObject )
				? TypeKind.MessagePackObject
				: !memberType.GetIsValueType()
				? TypeKind.ReferenceType
				: Nullable.GetUnderlyingType( memberType ) == null
				? TypeKind.ValueType
				: TypeKind.NullableType;

			var unpackHelperParameterTypeDefinition = DetermineUnpackHelperMethodParameterTypeDefinition( typeKind, isAsync );
			var unpackHelperParameterType =
				typeKind == TypeKind.MessagePackObject
				? TypeDefinition.GenericValueType( unpackHelperParameterTypeDefinition, unpackingContext.ContextType )
				: typeKind == TypeKind.NullableType
				? TypeDefinition.GenericValueType( unpackHelperParameterTypeDefinition, unpackingContext.ContextType, Nullable.GetUnderlyingType( memberType ) )
				: TypeDefinition.GenericValueType( unpackHelperParameterTypeDefinition, unpackingContext.ContextType, memberType );

			var helperMethod =
				new MethodDefinition(
					AdjustName( "Unpack" + typeKind + "Value", isAsync ),
					typeKind == TypeKind.MessagePackObject
						? new [] { unpackingContext.ContextType }
						: typeKind == TypeKind.NullableType
						? new [] { unpackingContext.ContextType, Nullable.GetUnderlyingType( memberType ) }
						: new [] { unpackingContext.ContextType, memberType },
					typeof( UnpackHelpers ),// declaring type
					true, // isStatic
					unpackingContext.ContextType, // return type
					TypeDefinition.ManagedReference( unpackHelperParameterType )
				);

			IDictionary<string, TConstruct> unpackHelperArguments;
			if ( typeKind == TypeKind.MessagePackObject )
			{
				unpackHelperArguments =
					new Dictionary<string, TConstruct>
					{
						{ "Unpacker", unpacker },
						{ "UnpackingContext", unpackingContext },
						{ "ItemsCount", countOfItem },
						{ "Unpacked", indexOfItem },
						{ "MemberName", memberName },
						{ "NilImplication", this.MakeEnumLiteral( context, TypeDefinition.NilImplicationType, nilImplication ) },
						{ "Setter", setterDelegate }
					};
#if FEATURE_TAP
				if ( isAsync )
				{
					unpackHelperArguments.Add( "CancellationToken", this.ReferCancellationToken( context, 5 ) );
				}
#endif // FEATURE_TAP
			}
			else
			{
				unpackHelperArguments =
					new Dictionary<string, TConstruct>
					{
						{ "Unpacker", unpacker },
						{ "UnpackingContext", unpackingContext },
						{ "Serializer", this.EmitGetSerializerExpression( context, memberType, memberInfo, itemsSchema ) },
						{ "ItemsCount", countOfItem },
						{ "Unpacked", indexOfItem },
						{ "TargetObjectType", this.EmitTypeOfExpression( context, memberType ) },
						{ "MemberName", memberName }
					};

				if ( typeKind != TypeKind.ValueType )
				{
					unpackHelperArguments.Add( "NilImplication", this.MakeEnumLiteral( context, TypeDefinition.NilImplicationType, nilImplication ) );
				}

				var directReadMethod = Metadata._UnpackHelpers.GetDirectUnpackMethod( memberType, isAsync );
				var directReadDelegateType =
#if FEATURE_TAP
					isAsync
						? typeof( Func<,,,,> ).MakeGenericType( typeof( Unpacker ), typeof( Type ), typeof( String ), typeof( CancellationToken ), typeof( Task<> ).MakeGenericType( memberType ) ) :
#endif // FEATURE_TAP
						typeof( Func<,,,> ).MakeGenericType( typeof( Unpacker ), typeof( Type ), typeof( String ), memberType );
				unpackHelperArguments.Add(
					"DirectRead",
					directReadMethod == null
						? this.MakeNullLiteral( context, directReadDelegateType )
						: this.EmitGetStaticDelegateExpression( context, directReadMethod )
				);
				unpackHelperArguments.Add( "Setter", setterDelegate );

#if FEATURE_TAP
				if ( isAsync )
				{
					unpackHelperArguments.Add( "CancellationToken", this.ReferCancellationToken( context, 5 ) );
				}
#endif // FEATURE_TAP
			}

			var unpackHelperParameters = this.DeclareLocal( context, unpackHelperParameterType, "unpackHelperParameters" );

			return
				this.EmitSequentialStatements(
					context,
					TypeDefinition.VoidType,
					new [] { unpackHelperParameters }
					.Concat(
						this.CreatePackUnpackHelperArgumentInitialization( context, unpackHelperParameters, unpackHelperArguments )
					).Concat(
						new []
						{
							isAsync
								? this.EmitRetrunStatement( context, this.EmitInvokeMethodExpression( context, null, helperMethod, this.EmitMakeRef( context, unpackHelperParameters ) ) )
								: this.EmitInvokeVoidMethod( context, null, helperMethod, this.EmitMakeRef( context, unpackHelperParameters ) )
						}
					)
				);
		}

		private static Type DetermineUnpackHelperMethodParameterTypeDefinition( TypeKind typeKind, bool isAsync )
		{
			switch ( typeKind )
			{
				case TypeKind.MessagePackObject:
				{ 
					return
#if FEATURE_TAP
						isAsync
							? typeof( UnpackMessagePackObjectValueAsyncParameters<> ) :
#endif // FEATURE_TAP
							typeof( UnpackMessagePackObjectValueParameters<> );
				}
				case TypeKind.ReferenceType:
				{
					return
#if FEATURE_TAP
						isAsync
							? typeof( UnpackReferenceTypeValueAsyncParameters<,> ) :
#endif // FEATURE_TAP
							typeof( UnpackReferenceTypeValueParameters<,> );
				}
				case TypeKind.ValueType:
				{
					return
#if FEATURE_TAP
						isAsync
							? typeof( UnpackValueTypeValueAsyncParameters<,> ) :
#endif // FEATURE_TAP
							typeof( UnpackValueTypeValueParameters<,> );
				}
				default:
				{
					Contract.Assert( typeKind == TypeKind.NullableType, typeKind + " == TypeKind.NullableType" );
					return
#if FEATURE_TAP
						isAsync
							? typeof( UnpackNullableTypeValueAsyncParameters<,> ) :
#endif // FEATURE_TAP
							typeof( UnpackNullableTypeValueParameters<,> );
				}
			}
		}

		/// <summary>
		///		Emits the append collection item.
		/// </summary>
		/// <param name="context">The code generation context.</param>
		/// <param name="member">The read only collection member metadata. <c>null</c> for collection item.</param>
		/// <param name="traits">The traits of the collection.</param>
		/// <param name="collection">The collection to be appended.</param>
		/// <param name="unpackedItem">The unpacked item.</param>
		/// <returns></returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		/// </exception>
		private TConstruct EmitAppendCollectionItem(
			TContext context,
			MemberInfo member,
			CollectionTraits traits,
			TConstruct collection,
			TConstruct unpackedItem
		)
		{
			if ( traits.AddMethod == null )
			{
				if ( member != null )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Type '{0}' of read only member '{1}' does not have public 'Add' method.",
							member.GetMemberValueType(),
							member
						)
					);
				}
				else
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Type '{0}' does not have public 'Add' method.",
							collection.ContextType
						)
					);
				}
			}

#if DEBUG
			Contract.Assert( traits.AddMethod.DeclaringType != null, "traits.AddMethod.DeclaringType != null" );
#endif // DEBUG
			return
				this.EmitInvokeVoidMethod(
					context,
					traits.AddMethod.DeclaringType.IsAssignableFrom( collection.ContextType.ResolveRuntimeType() )
					? collection
					: this.EmitUnboxAnyExpression( context, traits.AddMethod.DeclaringType, collection ),
					traits.AddMethod,
					unpackedItem
				);
		}

		private TConstruct EmitAppendDictionaryItem( TContext context, CollectionTraits traits, TConstruct dictionary, Type keyType, TConstruct key, Type valueType, TConstruct value, bool withBoxing )
		{
#if DEBUG
			Contract.Assert( traits.AddMethod.DeclaringType != null, "traits.AddMethod.DeclaringType != null" );
#endif // DEBUG
			return
				this.EmitInvokeVoidMethod(
					context,
					traits.AddMethod.DeclaringType.IsAssignableFrom( dictionary.ContextType.ResolveRuntimeType() )
					? dictionary
					: this.EmitUnboxAnyExpression( context, traits.AddMethod.DeclaringType, dictionary ),
					traits.AddMethod,
					withBoxing
					? this.EmitBoxExpression( context, keyType, key )
					: key,
					withBoxing
					? this.EmitBoxExpression( context, valueType, value )
					: value
				);
		}

		private TConstruct EmitThrowCannotUnpackFrom( TContext context )
		{
			return
				this.EmitThrowStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						SerializationExceptions.NewUnpackFromIsNotSupportedMethod,
						this.EmitTypeOfExpression( context, this.TargetType )
					)
				);
		}

		private TConstruct EmitThrowCannotCreateInstance( TContext context )
		{
			return
				this.EmitThrowStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						SerializationExceptions.NewCreateInstanceIsNotSupportedMethod,
						this.EmitTypeOfExpression( context, this.TargetType )
					)
				);
		}

		#endregion -- Unpack Constructs --

		#region -- Helper Construcs --

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
					this.EmitGetPropertyExpression( context, null, Metadata._CultureInfo.InvariantCulture ),
					this.MakeStringLiteral( context, format ),
					this.EmitCreateNewArrayExpression(
						context,
						TypeDefinition.ObjectType,
						arguments.Length,
						arguments.Select( a => a.ContextType.IsValueType ? this.EmitBoxExpression( context, a.ContextType, a ) : a )
					)
				);
		}

		/// <summary>
		///		Emits the get serializer expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="targetType">Type of the target of the serializer.</param>
		/// <param name="memberInfo">The metadata of the packing/unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The generated code construct.</returns>
		/// <remarks>
		///		The serializer reference methodology is implication specific.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected virtual TConstruct EmitGetSerializerExpression(
			TContext context,
			Type targetType,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema
		)
		{
			if ( memberInfo != null && targetType.GetIsEnum() )
			{
				return
					this.EmitInvokeMethodExpression(
						context,
						context.Context,
						Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
						this.EmitBoxExpression(
							context,
							TypeDefinition.EnumSerializationMethodType,
							this.EmitInvokeMethodExpression(
								context,
								null,
								Metadata._EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethodMethod,
								context.Context,
								this.EmitTypeOfExpression( context, targetType ),
								this.MakeEnumLiteral(
									context,
									TypeDefinition.EnumMemberSerializationMethodType,
									memberInfo.Value.GetEnumMemberSerializationMethod()
								)
							)
						)
					);
			}
			else if ( memberInfo != null && DateTimeMessagePackSerializerHelpers.IsDateTime( targetType ) )
			{
				return
					this.EmitInvokeMethodExpression(
						context,
						context.Context,
						Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
						this.EmitBoxExpression(
							context,
							TypeDefinition.DateTimeConversionMethodType,
							this.EmitInvokeMethodExpression(
								context,
								null,
								Metadata._DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethodMethod,
								context.Context,
								this.MakeEnumLiteral(
									context,
									TypeDefinition.DateTimeMemberConversionMethodType,
									memberInfo.Value.GetDateTimeMemberConversionMethod()
								)
							)
						)
					);
			}
			else
			{
				// Check by try to get serializer now.
				var schemaForMember = itemsSchema ??
									( memberInfo != null
										? PolymorphismSchema.Create( targetType, memberInfo )
										: PolymorphismSchema.Default );
				context.SerializationContext.GetSerializer( targetType, schemaForMember );

				var schema = this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, "__schema" );
				return
					this.EmitSequentialStatements(
						context,
						typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
						new[] { schema }
						.Concat(
							this.EmitConstructPolymorphismSchema(
								context,
								schema,
								schemaForMember
							)
						).Concat(
							new[]
							{
								this.EmitInvokeMethodExpression(
									context,
									context.Context,
									Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
									schema
								)
							}
						)
					);
			}
		}

		#endregion -- Helper Construcs --

		#region -- Helper Funcs --

		private static void GetDictionaryKeyValueType( Type elementType, out Type keyType, out Type valueType )
		{
			if ( elementType == typeof( DictionaryEntry ) )
			{
				keyType = typeof( object );
				valueType = typeof( object );
			}
			else
			{
				keyType = elementType.GetGenericArguments()[ 0 ];
				valueType = elementType.GetGenericArguments()[ 1 ];
			}
		}

		/// <summary>
		///		Retrieves a default constructor of the specified elementType.
		/// </summary>
		/// <param name="instanceType">The target elementType.</param>
		/// <returns>A default constructor of the <paramref name="instanceType"/>.</returns>
		private ConstructorInfo GetDefaultConstructor( Type instanceType )
		{
#if DEBUG
			Contract.Assert( !instanceType.GetIsValueType() );
#endif

			var ctor = this.TargetType.GetConstructor( ReflectionAbstractions.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( instanceType );
			}

			return ctor;
		}

		private IEnumerable<TConstruct> CreatePackUnpackHelperArgumentInitialization( TContext context, TConstruct helperArguments, IDictionary<string, TConstruct> arguments )
		{
			var parameterType = helperArguments.ContextType;

#if DEBUG
			Contract.Assert( !parameterType.IsRef, parameterType + " is not ref" );
#endif // DEBUG
			Type parameterTypeDefinition;
			if ( ( parameterType.TryGetRuntimeType()?.GetIsGenericType() ).GetValueOrDefault() )
			{
				parameterTypeDefinition = parameterType.TryGetRuntimeType().GetGenericTypeDefinition();
			}
			else
			{
				Contract.Assert( parameterType.ElementType.TryGetRuntimeType() != null, parameterType + ".ElementType.ElementType(" + parameterType.ElementType.ElementType + " does not have runtime type" );
				Contract.Assert( parameterType.ElementType.TryGetRuntimeType().GetIsGenericTypeDefinition(), parameterType + ".ElementType.ElementType(" + parameterType.ElementType.ElementType + " is not generic type definition" );
				parameterTypeDefinition = parameterType.ElementType.TryGetRuntimeType();

			}

			foreach ( var argument in arguments )
			{
				var field = parameterTypeDefinition.GetField( argument.Key );
				Contract.Assert( field != null, parameterType + "." + argument.Key + " does not exist" );
				yield return this.EmitSetField( context, helperArguments, new FieldDefinition( parameterType, field.Name, field.FieldType ), argument.Value );
			}
		}
		
#endregion -- Helper Funcs --

#region -- Collection Helpers --

		/// <summary>
		///		Determines the collection constructor arguments.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="constructor">The constructor.</param>
		/// <returns>
		///		An array of constructs representing constructor arguments.
		/// </returns>
		/// <exception cref="System.NotSupportedException">
		///		The <paramref name="constructor"/> has unsupported signature.
		/// </exception>
		private TConstruct[] DetermineCollectionConstructorArguments( TContext context, ConstructorInfo constructor )
		{
			var parameters = constructor.GetParameters();
			switch ( parameters.Length )
			{
				case 0:
				{
					return NoConstructs;
				}
				case 1:
				{
					return new[] { this.GetConstructorArgument( context, parameters[ 0 ] ) };
				}
				case 2:
				{
					return new[] { this.GetConstructorArgument( context, parameters[ 0 ] ), this.GetConstructorArgument( context, parameters[ 1 ] ) };
				}
				default:
				{
					throw new NotSupportedException(
						String.Format( CultureInfo.CurrentCulture, "Constructor signature '{0}' is not supported.", constructor )
					);
				}
			}
		}


		/// <summary>
		///		Gets the construt for constructor argument.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="parameter">The parameter of the constructor parameter.</param>
		/// <returns>The construt for constructor argument.</returns>
		private TConstruct GetConstructorArgument( TContext context, ParameterInfo parameter )
		{
			return
				parameter.ParameterType == typeof( int )
				? context.InitialCapacity
				: this.EmitGetEqualityComparer( context );
		}

		/// <summary>
		///		Emits the construct to get equality comparer via <see cref="UnpackHelpers"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///		The construct to get equality comparer via <see cref="UnpackHelpers"/>.
		/// </returns>
		private TConstruct EmitGetEqualityComparer( TContext context )
		{
			Type comparisonType;
			switch ( this.CollectionTraits.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				case CollectionDetailedKind.GenericCollection:
				case CollectionDetailedKind.GenericEnumerable:
				case CollectionDetailedKind.GenericList:
#if !NET35
				case CollectionDetailedKind.GenericSet:
#if !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
#endif // !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#endif // !NET35
				{
					comparisonType = this.CollectionTraits.ElementType;
					break;
				}
				case CollectionDetailedKind.GenericDictionary:
#if !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
#endif // !NET35 && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				{
					comparisonType = this.CollectionTraits.ElementType.GetGenericArguments()[ 0 ];
					break;
				}
				default:
				{
					// non-generic
					comparisonType = typeof( object );
					break;
				}
			}

			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetEqualityComparer_1Method.MakeGenericMethod( comparisonType )
				);
		}

		/// <summary>
		///		Emits <see cref="PolymorphismSchema"/> construction sequence.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="storage">The local variable which the schema to be stored.</param>
		/// <param name="schema">The <see cref="PolymorphismSchema"/> which contains emitting data.</param>
		/// <returns>
		///		Constructs to emit construct a copy of <paramref name="schema"/>.
		/// </returns>
		protected IEnumerable<TConstruct> EmitConstructPolymorphismSchema(
			TContext context,
			TConstruct storage,
			PolymorphismSchema schema
		)
		{
			if ( schema == null )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
					);
				yield break;
			}

			switch ( schema.ChildrenType )
			{
				case PolymorphismSchemaChildrenType.CollectionItems:
				{
					/*
					 * __itemsTypeMap = new Dictionary<string, Type>();
					 * __itemsTypeMap.Add( b, t );
					 * :
					 * __itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
					 */
					var itemsSchemaVariableName = context.GetUniqueVariableName( "itemsSchema" );
					var itemsSchema =
						schema.ItemSchema.UseDefault
							? this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
							: this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, itemsSchemaVariableName );
					if ( !schema.ItemSchema.UseDefault )
					{
						yield return itemsSchema;

						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, itemsSchema, schema.ItemSchema, itemsSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					if ( schema.UseDefault )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForContextSpecifiedCollectionMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									itemsSchema
								)
							);
					}
					else if ( schema.UseTypeEmbedding )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicCollectionTypeEmbeddingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									itemsSchema
								)
							);
					}
					else
					{
						var typeMap =
							this.DeclareLocal(
								context,
								TypeDefinition.DictionaryOfStringAndTypeType,
								context.GetUniqueVariableName( "typeMap" )
							);

						yield return typeMap;
						yield return
							this.EmitStoreVariableStatement(
								context,
								typeMap,
								this.EmitCreateNewObjectExpression(
									context,
									typeMap,
									PolymorphismSchema.CodeTypeMapConstructor,
									this.MakeInt32Literal( context, schema.CodeTypeMapping.Count )
								)
							);


						foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, schema, typeMap ) )
						{
							yield return instruction;
						}

						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicCollectionCodeTypeMappingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									typeMap,
									itemsSchema
								)
							);
					}
					break;
				}
				case PolymorphismSchemaChildrenType.DictionaryKeyValues:
				{
					/*
					 * __keysTypeMap = new Dictionary<string, Type>();
					 * __keysTypeMap.Add( b, t );
					 * :
					 * __keysSchema = new PolymorphismSchema( __keyType, __keysTypeMap ); // OR null
					 * __valuesTypeMap = new Dictionary<string, Type>();
					 * __valuesTypeMap.Add( b, t );
					 * :
					 * __valuesSchema = new PolymorphismSchema( __valueType, __valuesTypeMap ); // OR null
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __keysSchema, __valuesSchema );
					 */
					var keysSchemaVariableName = context.GetUniqueVariableName( "keysSchema" );
					var keysSchema =
						schema.KeySchema.UseDefault
							? this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
							: this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, keysSchemaVariableName );
					if ( !schema.KeySchema.UseDefault )
					{
						yield return keysSchema;
						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, keysSchema, schema.KeySchema, keysSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					var valuesSchemaVariableName = context.GetUniqueVariableName( "valuesSchema" );
					var valuesSchema =
						schema.ItemSchema.UseDefault
							? this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
							: this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, valuesSchemaVariableName );
					if ( !schema.ItemSchema.UseDefault )
					{
						yield return valuesSchema;
						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, valuesSchema, schema.ItemSchema, valuesSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					if ( schema.UseDefault )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForContextSpecifiedDictionaryMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									keysSchema,
									valuesSchema
								)
							);
					}
					else if ( schema.UseTypeEmbedding )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicDictionaryTypeEmbeddingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									keysSchema,
									valuesSchema
								)
							);
					}
					else
					{
						var typeMap =
							this.DeclareLocal(
								context,
								TypeDefinition.DictionaryOfStringAndTypeType,
								context.GetUniqueVariableName( "typeMap" )
							);

						yield return typeMap;
						yield return
							this.EmitStoreVariableStatement(
								context,
								typeMap,
								this.EmitCreateNewObjectExpression(
									context,
									typeMap,
									PolymorphismSchema.CodeTypeMapConstructor,
									this.MakeInt32Literal( context, schema.CodeTypeMapping.Count )
								)
							);

						foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, schema, typeMap ) )
						{
							yield return instruction;
						}

						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicDictionaryCodeTypeMappingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									typeMap,
									keysSchema,
									valuesSchema
								)
							);
					}
					break;
				}
#if !WINDOWS_PHONE && !NET35
				case PolymorphismSchemaChildrenType.TupleItems:
				{
					if ( schema.ChildSchemaList.Count == 0 )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
							);
					}

					/*
					 * tupleItemsSchema = new PolymorphismSchema[__arity__];
					 * for(var i = 0; i < __arity__; i++ )
					 * {
					 *   __itemsTypeMap = new Dictionary<string, Type>();
					 *   __itemsTypeMap.Add( b, t );
					 *   :
					 *   itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
					 *   tupleItemsSchema[i] = itemsSchema;
					 * }
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
					 */
					var tupleItems = TupleItems.GetTupleItemTypes( schema.TargetType );
					var tupleItemsSchema =
						this.DeclareLocal(
							context,
							TypeDefinition.PolymorphismSchemaArrayType,
							context.GetUniqueVariableName( "tupleItemsSchema" )
						);

					yield return tupleItemsSchema;

					yield return
						this.EmitStoreVariableStatement(
							context,
							tupleItemsSchema,
							this.EmitCreateNewArrayExpression( context, TypeDefinition.PolymorphismSchemaType, tupleItems.Count )
						);
					for ( var i = 0; i < tupleItems.Count; i++ )
					{
						var variableName = context.GetUniqueVariableName( "tupleItemSchema" );
						var itemSchema = this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, variableName );
						yield return itemSchema;
						foreach ( var statement in this.EmitConstructLeafPolymorphismSchema( context, itemSchema, schema.ChildSchemaList[ i ], variableName ) )
						{
							yield return statement;
						}

						yield return this.EmitSetArrayElementStatement( context, tupleItemsSchema, this.MakeInt32Literal( context, i ), itemSchema );
					}

					yield return
						this.EmitStoreVariableStatement(
							context,
							storage,
							this.EmitInvokeMethodExpression(
								context,
								null,
								PolymorphismSchema.ForPolymorphicTupleMethod,
								this.EmitTypeOfExpression( context, schema.TargetType ),
								tupleItemsSchema
							)
						);
					break;
				}
#endif // !WINDOWS_PHONE && !NET35
				default:
				{
					foreach ( var instruction in
						this.EmitConstructLeafPolymorphismSchema( context, storage, schema, String.Empty ) )
					{
						yield return instruction;
					}

					break;
				}
			}
		}

		private IEnumerable<TConstruct> EmitConstructLeafPolymorphismSchema( TContext context, TConstruct storage, PolymorphismSchema currentSchema, string prefix )
		{
			/*
			 * __itemsTypeMap = new Dictionary<string, Type>();
			 * __itemsTypeMap.Add( b, t );
			 * :
			 * __itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
			 * __map = new Dictionary<string, Type>();
			 * __map.Add( b, t );
			 * :
			 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
			 */
			if ( currentSchema.UseDefault )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.MakeNullLiteral( context, TypeDefinition.PolymorphismSchemaType )
					);
			}
			else if ( currentSchema.UseTypeEmbedding )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.EmitInvokeMethodExpression(
							context,
							null,
							PolymorphismSchema.ForPolymorphicObjectTypeEmbeddingMethod,
							this.EmitTypeOfExpression( context, currentSchema.TargetType )
						)
					);
			}
			else
			{
				var typeMap =
					this.DeclareLocal(
						context,
						TypeDefinition.DictionaryOfStringAndTypeType,
						context.GetUniqueVariableName( String.IsNullOrEmpty( prefix ) ? "typeMap" : ( prefix + "TypeMap" ) )
					);

				yield return typeMap;

				foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, currentSchema, typeMap ) )
				{
					yield return instruction;
				}

				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.EmitInvokeMethodExpression(
							context,
							null,
							PolymorphismSchema.ForPolymorphicObjectCodeTypeMappingMethod,
							this.EmitTypeOfExpression( context, currentSchema.TargetType ),
							typeMap
						)
					);
			}
		}

		private IEnumerable<TConstruct> EmitConstructTypeCodeMappingForPolymorphismSchema(
			TContext context,
			PolymorphismSchema currentSchema,
			TConstruct typeMap )
		{
			yield return
				this.EmitStoreVariableStatement(
					context,
					typeMap,
					this.EmitCreateNewObjectExpression(
						context,
						typeMap,
						PolymorphismSchema.CodeTypeMapConstructor,
						this.MakeInt32Literal( context, currentSchema.CodeTypeMapping.Count )
					)
				);

			foreach ( var entry in currentSchema.CodeTypeMapping )
			{
				yield return
					this.EmitInvokeMethodExpression(
						context,
						typeMap,
						PolymorphismSchema.AddToCodeTypeMapMethod,
						this.MakeStringLiteral( context, entry.Key ),
						this.EmitTypeOfExpression( context, entry.Value )
					);
			}
		}

		private TConstruct EmitCheckIsArrayHeaderExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotExpression(
						context,
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.IsArrayHeader )
					),
					this.EmitInvokeVoidMethod(
						context,
						null,
						SerializationExceptions.ThrowIsNotArrayHeaderMethod,
						context.Unpacker
					),
					null
				);
		}

		private TConstruct EmitCheckIsMapHeaderExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotExpression(
						context,
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.IsMapHeader )
					),
					this.EmitInvokeVoidMethod(
						context,
						null,
						SerializationExceptions.ThrowIsNotMapHeaderMethod,
						context.Unpacker
					),
					null
				);
		}

#endregion -- Collection Helpers --

#region -- Async Helpers --

		private static string AdjustName( string methodName, bool isAsync )
		{
			return isAsync ? ( methodName + "Async" ) : methodName;
		}

#if !FEATURE_TAP

		private static void ThrowAsyncNotSupportedException()
		{
			throw new NotSupportedException( "Async operation is not supported in this platform." );
		}

#else
		protected virtual bool WithAsync( TContext context )
		{
			return context.SerializationContext.SerializerOptions.WithAsync;
		}

		private TConstruct ReferCancellationToken( TContext context, int index )
		{
			return this.ReferArgument( context, TypeDefinition.CancellationTokenType, "cancellationToken", index );
		}

#endif // !FEATURE_TAP

#endregion -- Async Helpers --

		private enum TypeKind
		{
			MessagePackObject = 0,
			ValueType,
			ReferenceType,
			NullableType
		}

		private sealed class UnpackingContextInfo
		{
			public TypeDefinition Type { get; private set; } // Might be System.Object
			public TypeDefinition VariableType { get; private set; } // SHould not be System.Object
			public ConstructorDefinition Constructor { get; private set; }
			public TConstruct Variable { get; set; }
			public TConstruct Factory { get; set; }

			private readonly List<TConstruct> _statements;

			public List<TConstruct> Statements { get { return this._statements; } }

			private readonly HashSet<string> _mappableConstructorArguments;

			public HashSet<string> MappableConstructorArguments { get { return this._mappableConstructorArguments; } }

			private UnpackingContextInfo( TypeDefinition type, TypeDefinition variableType, ConstructorDefinition constructor, HashSet<string> mappableConstructorArguments )
			{
				this.Type = type;
				this.VariableType = variableType;
				this.Constructor = constructor;
				this._statements = new List<TConstruct>();
				this._mappableConstructorArguments = mappableConstructorArguments;
			}

			public static UnpackingContextInfo Create( TypeDefinition type, ConstructorDefinition constructor, HashSet<string> mappableConstructorArguments )
			{
				return new UnpackingContextInfo( type, constructor.DeclaringType, constructor, mappableConstructorArguments );
			}
		}
	}
}
