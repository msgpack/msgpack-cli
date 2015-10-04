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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if !NETFX_CORE
using System.Reflection.Emit;
#endif // !NETFX_CORE
using MsgPack.Serialization.AbstractSerializers;

#if NETFX_CORE
using MsgPack.Serialization.Reflection;
#endif // NETFX_CORE

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{TContext,TConstruct,TObject}"/> using expression tree.
	/// </summary>
	/// <typeparam name="TObject">The type of the serializing object.</typeparam>
	internal sealed class ExpressionTreeSerializerBuilder<TObject> : SerializerBuilder<ExpressionTreeContext, ExpressionConstruct, TObject>
	{
		private static readonly Type SerializerClass =
			ExpressionTreeSerializerBuilderHelpers.GetSerializerClass( typeof( TObject ), CollectionTraitsOfThis );
#if !NETFX_CORE && !SILVERLIGHT
		private readonly TypeBuilder _typeBuilder;
#endif

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeSerializerBuilder{TObject}"/> class.
		/// </summary>
		public ExpressionTreeSerializerBuilder()
		{
#if !NETFX_CORE && !SILVERLIGHT
			if ( SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump();
				this._typeBuilder = SerializerDebugging.NewTypeBuilder( typeof( TObject ) );
			}
#endif
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodPrologue( ExpressionTreeContext context, SerializerMethod method )
		{
			context.Reset( typeof( TObject ), BaseClass );
			context.SetCurrentMethod( method );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodPrologue( ExpressionTreeContext context, EnumSerializerMethod method )
		{
			context.Reset( typeof( TObject ), BaseClass );
			context.SetCurrentMethod( typeof( TObject ), method );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodPrologue( ExpressionTreeContext context, CollectionSerializerMethod method, MethodInfo declaration )
		{
			context.Reset( typeof( TObject ), BaseClass );
			context.SetCurrentMethod( method, CollectionTraitsOfThis );
		}

		protected override void EmitMethodEpilogue( ExpressionTreeContext context, SerializerMethod method, ExpressionConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}

			context.SetDelegate( method, this.EmitMethodEpilogue( context, ExpressionTreeContext.CreateDelegateType<TObject>( method, SerializerClass ), method, construct ) );

		}

		protected override void EmitMethodEpilogue( ExpressionTreeContext context, EnumSerializerMethod method, ExpressionConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}

			context.SetDelegate( method, this.EmitMethodEpilogue( context, ExpressionTreeContext.CreateDelegateType<TObject>( method ), method, construct ) );
		}

		protected override void EmitMethodEpilogue( ExpressionTreeContext context, CollectionSerializerMethod method, ExpressionConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}

			context.SetDelegate( method, this.EmitMethodEpilogue( context, ExpressionTreeContext.CreateDelegateType<TObject>( method, SerializerClass, CollectionTraitsOfThis ), method, construct ) );
		}

#if NETFX_CORE || SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Must be instance method in other platforms" )]
#endif // NETFX_CORE || SILVERLIGHT
		private Delegate EmitMethodEpilogue<T>( ExpressionTreeContext context, Type delegateType, T method, ExpressionConstruct construct )
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "----{0}----", method );
				construct.ToString( SerializerDebugging.ILTraceWriter );
				SerializerDebugging.FlushTraceData();
			}

			var lambda =
				Expression.Lambda(
					delegateType,
					construct.Expression,
					method.ToString(),
					false,
					context.GetCurrentParameters()
				);

#if !NETFX_CORE && !SILVERLIGHT
			if ( SerializerDebugging.DumpEnabled )
			{
				var mb =
					this._typeBuilder.DefineMethod(
						method.ToString(),
						MethodAttributes.Public | MethodAttributes.Static,
						lambda.Type,
						lambda.Parameters.Select( e => e.Type ).ToArray()
						);
				lambda.CompileToMethod( mb );
			}
#endif
			return lambda.Compile();
		}

		protected override ExpressionConstruct MakeNullLiteral( ExpressionTreeContext context, Type contextType )
		{
			return Expression.Constant( null, contextType );
		}

		protected override ExpressionConstruct MakeByteLiteral( ExpressionTreeContext context, byte constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeSByteLiteral( ExpressionTreeContext context, sbyte constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt16Literal( ExpressionTreeContext context, short constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt16Literal( ExpressionTreeContext context, ushort constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt32Literal( ExpressionTreeContext context, int constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt32Literal( ExpressionTreeContext context, uint constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt64Literal( ExpressionTreeContext context, long constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt64Literal( ExpressionTreeContext context, ulong constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeReal32Literal( ExpressionTreeContext context, float constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeReal64Literal( ExpressionTreeContext context, double constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeBooleanLiteral( ExpressionTreeContext context, bool constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeCharLiteral( ExpressionTreeContext context, char constant )
		{
			return Expression.Constant( constant );
		}


		protected override ExpressionConstruct MakeStringLiteral( ExpressionTreeContext context, string constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeEnumLiteral( ExpressionTreeContext context, Type type, object constant )
		{
			return Expression.Constant( constant, type );
		}

		protected override ExpressionConstruct MakeDefaultLiteral( ExpressionTreeContext context, Type type )
		{
			return Expression.Default( type );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitThisReferenceExpression( ExpressionTreeContext context )
		{
			return context.This;
		}

		protected override ExpressionConstruct EmitBoxExpression( ExpressionTreeContext context, Type valueType, ExpressionConstruct value )
		{
			return Expression.Convert( value, typeof( object ) );
		}

		protected override ExpressionConstruct EmitUnboxAnyExpression( ExpressionTreeContext context, Type targetType, ExpressionConstruct value )
		{
			return Expression.Convert( value, targetType );
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

		protected override ExpressionConstruct EmitIncrement( ExpressionTreeContext context, ExpressionConstruct int32Value )
		{
			return Expression.Assign( int32Value, Expression.Increment( int32Value ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitTypeOfExpression( ExpressionTreeContext context, Type type )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( type );
			}
			else
			{
				// WinRT expression tree cannot handle Type constants, but handle RuntimeTypeHandle.
				return Expression.Call( Metadata._Type.GetTypeFromHandle, Expression.Constant( type.TypeHandle ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitMethodOfExpression( ExpressionTreeContext context, MethodBase method )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( method );
			}
			else
			{
#if !NETFX_CORE
				// WinRT expression tree cannot handle Type constants, but handle RuntimeTypeHandle.
#if DEBUG
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );
#endif // DEBUG
				return Expression.Call( Metadata._MethodBase.GetMethodFromHandle, Expression.Constant( method.MethodHandle ), Expression.Constant( method.DeclaringType.TypeHandle ) );
#else
				// WinRT expression tree cannot handle Type constants, and MethodHandle property is not exposed.
				// typeof( T ).GetRuntimeMethod( method.Name, ...{types}... );
				return
					Expression.Call(
						ReflectionHelpers.GetRuntimeMethodMethod,
						this.EmitTypeOfExpression( context, method.DeclaringType ).Expression,
						Expression.Constant( method.Name ),
						Expression.NewArrayInit(
							typeof( Type ),
							method.GetParameters().Select( p => this.EmitTypeOfExpression( context, p.ParameterType ).Expression )
						)
					);
#endif
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitFieldOfExpression( ExpressionTreeContext context, FieldInfo field )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( field );
			}
			else
			{
#if !NETFX_CORE
#if DEBUG
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );
#endif // DEBUG
				return Expression.Call( Metadata._FieldInfo.GetFieldFromHandle, Expression.Constant( field.FieldHandle ), Expression.Constant( field.DeclaringType.TypeHandle ) );
#else
				// WinRT expression tree cannot handle Type constants, and MethodHandle property is not exposed.
				// typeof( T ).GetRuntimeField( field.Name );
				return
					Expression.Call(
						ReflectionHelpers.GetRuntimeFieldMethod,
						this.EmitTypeOfExpression( context, field.DeclaringType ).Expression,
						Expression.Constant( field.Name )
					);
#endif
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ExpressionConstruct ReferArgument( ExpressionTreeContext context, Type type, string name, int index )
		{
			return context.GetCurrentParameters()[ index ];
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

		protected override ExpressionConstruct EmitGetPropertyExpression(
			ExpressionTreeContext context, ExpressionConstruct instance, PropertyInfo property
		)
		{
			return Expression.Property( instance, property );
		}

		protected override ExpressionConstruct EmitGetFieldExpression( ExpressionTreeContext context, ExpressionConstruct instance, FieldInfo field )
		{
			return Expression.Field( instance, field );
		}

		protected override ExpressionConstruct EmitSetProperty(
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

		protected override ExpressionConstruct EmitTryFinally(
			ExpressionTreeContext context, ExpressionConstruct tryStatement, ExpressionConstruct finallyStatement
		)
		{
			return Expression.TryFinally( tryStatement, finallyStatement );
		}

		protected override ExpressionConstruct EmitCreateNewArrayExpression(
			ExpressionTreeContext context, Type elementType, int length
		)
		{
			return Expression.NewArrayBounds( elementType, Expression.Constant( length ) );
		}

		protected override ExpressionConstruct EmitCreateNewArrayExpression(
			ExpressionTreeContext context, Type elementType, int length, IEnumerable<ExpressionConstruct> initialElements
		)
		{
			return Expression.NewArrayInit( elementType, initialElements.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitSetArrayElementStatement( ExpressionTreeContext context, ExpressionConstruct array, ExpressionConstruct index, ExpressionConstruct value )
		{
			return
				Expression.Assign(
					Expression.ArrayAccess( array, index ),
					value
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitConditionalExpression(
			ExpressionTreeContext context,
			ExpressionConstruct conditionExpression,
			ExpressionConstruct thenExpression,
			ExpressionConstruct elseExpression
		)
		{
#if DEBUG
			Contract.Assert(
				elseExpression == null
				|| thenExpression.ContextType == typeof( void )
				|| elseExpression.ContextType == typeof( void )
				|| thenExpression.ContextType == elseExpression.ContextType,
				thenExpression.ContextType + " != " + ( elseExpression == null ? "(null)" : elseExpression.ContextType.FullName )
			);
#endif

			return
				elseExpression == null
				? Expression.IfThen( conditionExpression, thenExpression )
				: ( thenExpression.ContextType == typeof( void ) || elseExpression.ContextType == typeof( void ) )
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

		protected override ExpressionConstruct EmitStringSwitchStatement( ExpressionTreeContext context, ExpressionConstruct target, IDictionary<string, ExpressionConstruct> cases, ExpressionConstruct defaultCase )
		{
			return
				Expression.Switch(
					typeof( void ),
					target,
					defaultCase,
					Metadata._String.op_Equality,
					cases.Select( kv => Expression.SwitchCase( kv.Value, Expression.Constant( kv.Key ) ) ).ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
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

		protected override ExpressionConstruct EmitEnumFromUnderlyingCastExpression(
			ExpressionTreeContext context,
			Type enumType,
			ExpressionConstruct underlyingValue )
		{
			return Expression.Convert( underlyingValue, enumType );
		}

		protected override ExpressionConstruct EmitEnumToUnderlyingCastExpression(
			ExpressionTreeContext context,
			Type underlyingType,
			ExpressionConstruct enumValue )
		{
			// ExpressionTree cannot handle enum to underlying type conversion...
			return Expression.Convert( enumValue, underlyingType );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( ExpressionTreeContext codeGenerationContext, PolymorphismSchema schema )
		{
#if !NETFX_CORE && !SILVERLIGHT
			if ( SerializerDebugging.DumpEnabled )
			{
				this._typeBuilder.CreateType();
			}
#endif
			return ExpressionTreeSerializerBuilderHelpers.CreateFactory<TObject>( codeGenerationContext, CollectionTraitsOfThis, schema );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor( ExpressionTreeContext codeGenerationContext )
		{
#if !NETFX_CORE && !SILVERLIGHT
			if ( SerializerDebugging.DumpEnabled )
			{
				this._typeBuilder.CreateType();
			}
#endif

			// Get at this point to prevent unexpected context change.
			var packUnderyingValueTo = codeGenerationContext.GetPackUnderyingValueTo();
			var unpackFromUnderlyingValue = codeGenerationContext.GetUnpackFromUnderlyingValue();

			var targetType = typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );

			return
				context =>
					ReflectionExtensions.CreateInstancePreservingExceptionType<MessagePackSerializer<TObject>>(
						targetType,
						context,
						EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
							context,
							typeof( TObject ),
							EnumMemberSerializationMethod.Default
						),
						packUnderyingValueTo,
						unpackFromUnderlyingValue
					);
		}

		protected override ExpressionTreeContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			return new ExpressionTreeContext( context );
		}
	}
}
