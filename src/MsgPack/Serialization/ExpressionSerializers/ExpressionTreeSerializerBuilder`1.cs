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

// TODO: Flag of context
#define DUMP

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	internal sealed class ExpressionConstruct : ICodeConstruct
	{
		private readonly Expression _expression;

		public Expression Expression
		{
			get { return this._expression; }
		}

		private readonly bool _isSignificantReference;

		public bool IsSignificantReference
		{
			get { return this._isSignificantReference; }
		}

		public Type ContextType
		{
			get { return this._expression.Type; }
		}

		public ExpressionConstruct( Expression expression )
			: this( expression, false )
		{
		}

		public ExpressionConstruct( Expression expression, bool isSignificantReference )
		{
#if DEBUG
			Contract.Assert( expression != null );
#endif
			this._expression = expression;
			this._isSignificantReference = isSignificantReference;
		}

		public static implicit operator ExpressionConstruct( Expression expression )
		{
			return expression == null ? null : new ExpressionConstruct( expression );
		}

		public static implicit operator Expression( ExpressionConstruct construct )
		{
			return construct == null ? null : construct.Expression;
		}

		internal void ToString( System.IO.TextWriter textWriter )
		{
			this.ToString( textWriter, 0 );
		}

		private void ToString( System.IO.TextWriter textWriter, int indentLevel )
		{
			new ExpressionDumper( textWriter, indentLevel ).Visit( this.Expression );
		}
	}

	internal sealed class ExpressionTreeContext : SerializerGenerationContext<ExpressionConstruct>
	{
		private const string PackToCoreMethod = "PackToCore";
		private const string UnpackFromCoreMethod = "UnpackFromCore";
		private const string UnpackToCoreMethod = "UnpackToCore";

		private readonly ExpressionConstruct _context;

		public ExpressionConstruct Context
		{
			get { return this._context; }
		}

		private readonly ExpressionConstruct _this;

		public ExpressionConstruct This
		{
			get { return this._this; }
		}

		private Delegate _packToCore;
		private Delegate _unpackFromCore;
		private Delegate _unpackToCore;

		public ExpressionTreeContext( SerializationContext serializationContext, ExpressionConstruct packer, ExpressionConstruct packingTarget, ExpressionConstruct unpacker, ExpressionConstruct unpackToTarget )
			: base( serializationContext, packer, packingTarget, unpacker, unpackToTarget )
		{
			this._context = Expression.Parameter( typeof( SerializationContext ), "context" );
			this._this =
				Expression.Parameter(
					typeof( ExpressionCallbackMessagePackSerializer<> ).MakeGenericType( packingTarget.ContextType ), "this"
				);
		}

		public static Type CreateDelegateType<T>( MethodInfo method )
		{
			switch ( method.Name )
			{
				case PackToCoreMethod:
				{
					return typeof( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> );
				}
				case UnpackFromCoreMethod:
				{
					return typeof( Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> );
				}
				case UnpackToCoreMethod:
				{
					return typeof( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> );
				}
				default:
				{
					throw UnknownMethod( method );
				}
			}
		}

		private static Exception UnknownMethod( MethodInfo method )
		{
			return new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Unknown method '{0}'.", method ) );
		}

		public IEnumerable<ParameterExpression> GetParameters<T>( MethodInfo method )
		{
			yield return this._this.Expression as ParameterExpression;
			yield return this._context.Expression as ParameterExpression;

			switch ( method.Name )
			{
				case PackToCoreMethod:
				{
					yield return this.Packer.Expression as ParameterExpression;
					yield return this.PackingTarget.Expression as ParameterExpression;
					break;
				}
				case UnpackFromCoreMethod:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					break;
				}
				case UnpackToCoreMethod:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					yield return this.UnpackToTarget.Expression as ParameterExpression;
					break;
				}
				default:
				{
					throw UnknownMethod( method );
				}
			}
		}

		public void SetDelegate( MethodInfo method, Delegate @delegate )
		{
			switch ( method.Name )
			{
				case PackToCoreMethod:
				{
					this._packToCore = @delegate;
					break;
				}
				case UnpackFromCoreMethod:
				{
					this._unpackFromCore = @delegate;
					break;
				}
				case UnpackToCoreMethod:
				{
					this._unpackToCore = @delegate;
					break;
				}
				default:
				{
					throw UnknownMethod( method );
				}
			}
		}

		public Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> GetPackToCore<T>()
		{
			return this._packToCore as Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>;
		}

		public Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> GetUnpackFromCore<T>()
		{
			return this._unpackFromCore as Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T>;
		}

		public Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> GetUnpackToCore<T>()
		{
			return this._unpackToCore as Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T>;
		}
	}

	internal class ExpressionCallbackMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly SerializationContext _context;
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> _packToCore;
		private readonly Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackFromCore;
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackToCore;

		public ExpressionCallbackMessagePackSerializer(
			SerializationContext context,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> packToCore,
			Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackFromCore,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackToCore
		)
			: base( context == null ? PackerCompatibilityOptions.Classic : context.CompatibilityOptions.PackerCompatibilityOptions )
		{
			this._context = context;
			this._packToCore = packToCore;
			this._unpackFromCore = unpackFromCore;
			this._unpackToCore = unpackToCore;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( this, this._context, packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpackFromCore( this, this._context, unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._unpackToCore != null )
			{
				this._unpackToCore( this, this._context, unpacker, collection );
			}
			else
			{
				base.UnpackToCore( unpacker, collection );
			}
		}
	}

	class ExpressionTreeSerializerBuilder<TObject> : SerializerBuilder<ExpressionTreeContext, ExpressionConstruct, TObject>
	{
		private readonly TypeBuilder _typeBuilder;

		public ExpressionTreeSerializerBuilder()
			: base( "ETDynamicMethodHost", new Version() )
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
					context.GetParameters<TObject>( metadata )
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

		protected override ExpressionTreeContext CreateGenerationContextForSerializerCreation( SerializationContext context )
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
