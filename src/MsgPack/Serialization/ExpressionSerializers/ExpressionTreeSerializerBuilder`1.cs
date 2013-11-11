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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{TContext,TConstruct,TObject}"/> using expression tree.
	/// </summary>
	/// <typeparam name="TObject">The type of the serializing object.</typeparam>
	internal sealed class ExpressionTreeSerializerBuilder<TObject> : SerializerBuilder<ExpressionTreeContext, ExpressionConstruct, TObject>
	{
		private readonly TypeBuilder _typeBuilder;

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeSerializerBuilder{TObject}"/> class.
		/// </summary>
		public ExpressionTreeSerializerBuilder()
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump();
				this._typeBuilder = SerializerDebugging.NewTypeBuilder( typeof( TObject ) );
			}
		}

		protected override void EmitMethodPrologue( ExpressionTreeContext context, MethodInfo metadata )
		{
			// nop
		}

		protected override void EmitMethodEpilogue( ExpressionTreeContext context, MethodInfo metadata, IList<ExpressionConstruct> constructs )
		{
			if ( constructs == null || constructs[ 0 ] == null )
			{
				return;
			}

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "----{0}----", metadata );
				constructs[ 0 ].ToString( SerializerDebugging.ILTraceWriter );
				SerializerDebugging.FlushTraceData();
			}

#if DEBUG
			Contract.Assert( constructs.Count == 1 );
#endif

			var lambda =
				Expression.Lambda(
					ExpressionTreeContext.CreateDelegateType<TObject>( metadata ),
					( constructs.Count > 1
						  ? Expression.Block( metadata.ReturnType, constructs.Select( c => c.Expression ) )
						  : constructs[ 0 ].Expression
					),
					metadata.Name,
					false,
					context.GetParameters( metadata )
				);

			if ( SerializerDebugging.DumpEnabled )
			{
				var mb =
					this._typeBuilder.DefineMethod(
						metadata.Name,
						MethodAttributes.Public | MethodAttributes.Static,
						lambda.Type,
						lambda.Parameters.Select( e => e.Type ).ToArray()
						);
				lambda.CompileToMethod( mb );
			}

			context.SetDelegate(
				metadata,
				lambda.Compile()
			);
		}

		protected override ExpressionConstruct EmitStatementExpression(
			ExpressionTreeContext context, ExpressionConstruct statement, ExpressionConstruct contextExpression
		)
		{
			var expressions = new Expression[] { statement, contextExpression };
			return
				Expression.Block(
					contextExpression.ContextType,
					expressions.OfType<ParameterExpression>(),
					expressions
				);
		}

		protected override ExpressionConstruct MakeNullLiteral( ExpressionTreeContext context, Type contextType )
		{
			return Expression.Constant( null, contextType );
		}

		protected override ExpressionConstruct MakeInt32Literal( ExpressionTreeContext context, int constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt64Literal( ExpressionTreeContext context, long constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeStringLiteral( ExpressionTreeContext context, string constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct EmitThisReferenceExpression( ExpressionTreeContext context )
		{
			return context.This;
		}

		protected override ExpressionConstruct EmitBoxExpression( ExpressionTreeContext context, Type valueType, ExpressionConstruct value )
		{
			return Expression.Convert( value, typeof( object ) );
		}

		protected override ExpressionConstruct EmitNotExpression( ExpressionTreeContext context, ExpressionConstruct booleanExpression )
		{
			return Expression.Not( booleanExpression );
		}

		protected override ExpressionConstruct EmitEqualsExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.Equal( left, right );
		}

		protected override ExpressionConstruct EmitNotEqualsExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.NotEqual( left, right );
		}

		protected override ExpressionConstruct EmitGreaterThanExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.GreaterThan( left, right );
		}

		protected override ExpressionConstruct EmitLessThanExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.LessThan( left, right );
		}

		protected override ExpressionConstruct EmitIncrementExpression( ExpressionTreeContext context, ExpressionConstruct int32Value )
		{
			return Expression.Assign( int32Value, Expression.Increment( int32Value ) );
		}

		protected override ExpressionConstruct EmitTypeOfExpression( ExpressionTreeContext context, Type type )
		{
// ReSharper disable RedundantIfElseBlock
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but handle Type constants.
				return Expression.Constant( type );
			}
			else
			{
				// WinRT expression tree cannot handle Type constants, but handle RuntimeTypeHandle.
				return Expression.Call( Metadata._Type.GetTypeFromHandle, Expression.Constant( type.TypeHandle ) );
			}
// ReSharper restore RedundantIfElseBlock
		}

		protected override ExpressionConstruct EmitSequentialStatements( ExpressionTreeContext context, Type contextType, IEnumerable<ExpressionConstruct> statements )
		{
			var sts = statements.Where( s => s != null ).ToArray();

			return
				Expression.Block(
					contextType,
					sts.Select( c => c.Expression ).OfType<ParameterExpression>().Distinct(), // For declare and re-refer pattern
					sts.Where( c => c.IsSignificantReference || !( c.Expression is ParameterExpression ) ).Select( c => c.Expression )
				);
		}

		protected override ExpressionConstruct DeclareLocal(
			ExpressionTreeContext context, Type type, string name
		)
		{
			return Expression.Variable( type, name );
		}

		protected override ExpressionConstruct EmitCreateNewObjectExpression(
			ExpressionTreeContext context, ExpressionConstruct variable, ConstructorInfo constructor, params ExpressionConstruct[] arguments
		)
		{
			return Expression.New( constructor, arguments.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitInvokeVoidMethod(
			ExpressionTreeContext context, ExpressionConstruct instance, MethodInfo method, params ExpressionConstruct[] arguments
		)
		{
			return this.EmitInvokeMethodExpression( context, instance, method, arguments );
		}

		protected override ExpressionConstruct EmitInvokeMethodExpression(
			ExpressionTreeContext context, ExpressionConstruct instance, MethodInfo method, IEnumerable<ExpressionConstruct> arguments
		)
		{
			return
				instance == null
					? Expression.Call( method, arguments.Select( c => c.Expression ) )
					: Expression.Call( instance, method, arguments.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitGetPropretyExpression(
			ExpressionTreeContext context, ExpressionConstruct instance, PropertyInfo property
		)
		{
			return Expression.Property( instance, property );
		}

		protected override ExpressionConstruct EmitGetFieldExpression( ExpressionTreeContext context, ExpressionConstruct instance, FieldInfo field )
		{
			return Expression.Field( instance, field );
		}

		protected override ExpressionConstruct EmitSetProprety(
			ExpressionTreeContext context, ExpressionConstruct instance, PropertyInfo property, ExpressionConstruct value
		)
		{
			return Expression.Assign( Expression.Property( instance, property ), value );
		}

		protected override ExpressionConstruct EmitSetField(
			ExpressionTreeContext context, ExpressionConstruct instance, FieldInfo field, ExpressionConstruct value
		)
		{
			return Expression.Assign( Expression.Field( instance, field ), value );
		}

		protected override ExpressionConstruct EmitLoadVariableExpression( ExpressionTreeContext context, ExpressionConstruct variable )
		{
			// Just use ParameterExpression.
#if DEBUG
			Contract.Assert(
				( variable.Expression is ParameterExpression ) && variable.ContextType != typeof( void ),
				variable.Expression.ToString()
			);
#endif
			return new ExpressionConstruct( variable, true );
		}

		protected override ExpressionConstruct EmitStoreVariableStatement(
			ExpressionTreeContext context, ExpressionConstruct variable, ExpressionConstruct value
		)
		{
			return Expression.Assign( variable, value );
		}

		protected override ExpressionConstruct EmitStoreVariableStatement( ExpressionTreeContext context, ExpressionConstruct variable )
		{
			// nop
			return null;
		}

		protected override ExpressionConstruct EmitThrowExpression(
			ExpressionTreeContext context, Type expressionType, ExpressionConstruct exceptionExpression
		)
		{
			return Expression.Throw( exceptionExpression, expressionType );
		}

		protected override ExpressionConstruct EmitTryFinallyExpression(
			ExpressionTreeContext context, ExpressionConstruct tryExpression, ExpressionConstruct finallyStatement
		)
		{
			return Expression.TryFinally( tryExpression, finallyStatement );
		}

		protected override ExpressionConstruct EmitCreateNewArrayExpression(
			ExpressionTreeContext context, Type elementType, int length, IEnumerable<ExpressionConstruct> initialElements
		)
		{
			return Expression.NewArrayInit( elementType, initialElements.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitGetSerializerExpression( ExpressionTreeContext context, Type targetType )
		{
			return
				Expression.Call(
					context.Context, Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType )
				);
		}

		protected override ExpressionConstruct EmitConditionalExpression(
			ExpressionTreeContext context,
			ExpressionConstruct conditionExpression,
			ExpressionConstruct thenExpression,
			ExpressionConstruct elseExpression
		)
		{
#if DEBUG
			Contract.Assert(
				elseExpression == null || ( thenExpression.ContextType == elseExpression.ContextType ),
				thenExpression.ContextType + " != " + ( elseExpression == null ? "(null)" : elseExpression.ContextType.FullName )
			);
#endif

			return
				elseExpression == null
				? Expression.IfThen( conditionExpression, thenExpression )
				: thenExpression.ContextType == typeof( void )
				? Expression.IfThenElse( conditionExpression, thenExpression, elseExpression )
				: Expression.Condition( conditionExpression, thenExpression, elseExpression );
		}

		protected override ExpressionConstruct EmitAndConditionalExpression(
			ExpressionTreeContext context,
			IList<ExpressionConstruct> conditionExpressions,
			ExpressionConstruct thenExpression,
			ExpressionConstruct elseExpression
		)
		{
			return
				Expression.IfThenElse(
					conditionExpressions.Aggregate( ( l, r ) => Expression.AndAlso( l, r ) ),
					thenExpression,
					elseExpression
				);
		}

		protected override ExpressionConstruct EmitStringSwitchStatement(
			ExpressionTreeContext context, ExpressionConstruct target, IDictionary<string, ExpressionConstruct> cases
		)
		{
			return
				Expression.Switch(
					typeof( void ),
					target,
					null,
					Metadata._String.op_Equality,
					cases.Select( kv => Expression.SwitchCase( kv.Value, Expression.Constant( kv.Key ) ) ).ToArray()
				);
		}

		protected override ExpressionConstruct EmitForLoop( ExpressionTreeContext context, ExpressionConstruct count, Func<ForLoopContext, ExpressionConstruct> loopBodyEmitter )
		{
			var counter = Expression.Variable( typeof( int ), "i" );
			var loopContext = new ForLoopContext( counter );
			var endFor = Expression.Label( "END_FOR" );
			return
				Expression.Block(
					new[] { counter },
					Expression.Loop(
						Expression.IfThenElse(
							Expression.LessThan( counter, count ),
							Expression.Block(
								loopBodyEmitter( loopContext ),
								Expression.Assign( counter, Expression.Increment( counter ) )
							),
							Expression.Break( endFor )
						),
						endFor
					)
				);
		}

		protected override ExpressionConstruct EmitForEachLoop(
			ExpressionTreeContext context, CollectionTraits collectionTraits, ExpressionConstruct collection, Func<ExpressionConstruct, ExpressionConstruct> loopBodyEmitter
		)
		{
			var enumerator = Expression.Variable( collectionTraits.GetEnumeratorMethod.ReturnType, "enumerator" );
			var current = Expression.Variable( collectionTraits.ElementType, "current" );
			var moveNextMethod = Metadata._IEnumerator.FindEnumeratorMoveNextMethod( enumerator.Type );
			var currentProperty = Metadata._IEnumerator.FindEnumeratorCurrentProperty( enumerator.Type, collectionTraits );

			var endForEach = Expression.Label( "END_FOREACH" );
			return
				Expression.Block(
					new[] { enumerator, current },
					Expression.Assign( enumerator, Expression.Call( collection, collectionTraits.GetEnumeratorMethod ) ),
					Expression.Loop(
						Expression.IfThenElse(
							Expression.Call( enumerator, moveNextMethod ),
							Expression.Block(
								Expression.Assign( current, Expression.Property( enumerator, currentProperty ) ),
								loopBodyEmitter( current )
							),
							Expression.Break( endForEach )
						),
						endForEach
					)
				);
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( ExpressionTreeContext codeGenerationContext )
		{
			var packTo = codeGenerationContext.GetPackToCore<TObject>();
			var unpackFrom = codeGenerationContext.GetUnpackFromCore<TObject>();
			var unpackTo = codeGenerationContext.GetUnpackToCore<TObject>();

			if ( SerializerDebugging.DumpEnabled )
			{
				this._typeBuilder.CreateType();
			}

			return context => new ExpressionCallbackMessagePackSerializer<TObject>( context, packTo, unpackFrom, unpackTo );
		}

		protected override ExpressionTreeContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			return
				new ExpressionTreeContext(
					context,
					Expression.Parameter( typeof( Packer ), "packer" ),
					Expression.Parameter( typeof( TObject ), "objectTree" ),
					Expression.Parameter( typeof( Unpacker ), "unpacker" ),
					Expression.Parameter( typeof( TObject ), "collection" )
				);
		}
	}
}
