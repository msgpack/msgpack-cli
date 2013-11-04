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
using System.Reflection;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal abstract class ILEmittingSerializerBuilder<TObject> : SerializerBuilder<ILEmittingContext, ILConstruct, TObject>
	{
		protected ILEmittingSerializerBuilder()
			: this( "DynamicMethodHost", new Version() )
		{
		}

		protected ILEmittingSerializerBuilder( string assemblyName, Version version )
			: base( assemblyName, version )
		{
		}

		protected override void EmitMethodPrologue( ILEmittingContext context, MethodInfo metadata )
		{
			switch ( metadata.Name )
			{
				case "PackToCore":
				{
					context.IL = context.Emitter.GetPackToMethodILGenerator();
					break;
				}
				case "UnpackFromCore":
				{
					context.IL = context.Emitter.GetUnpackFromMethodILGenerator();
					break;
				}
				case "UnpackToCore":
				{
					context.IL = context.Emitter.GetUnpackToMethodILGenerator();
					break;
				}
				default:
				{
					throw new NotSupportedException( metadata.Name );
				}
			}
		}

		protected override void EmitMethodEpilogue( ILEmittingContext context, MethodInfo metadata, IList<ILConstruct> constructs )
		{
			try
			{
				foreach ( var construct in constructs )
				{
					if ( construct == null )
					{
						continue;
					}

					construct.Evaluate( context.IL );
				}

				//if ( metadata.ReturnType != typeof( void ) )
				//{
				//	var last = constructs.LastOrDefault( c => c != null );
				//	if ( last != null )
				//	{
				//		last.LoadValue( context.IL, false );
				//	}
				//}

				context.IL.EmitRet();
			}
			finally
			{
				context.IL.FlushTrace();
				context.Emitter.FlushTrace();
			}
		}

		protected override ILConstruct EmitSequentialStatements( ILEmittingContext context, IEnumerable<ILConstruct> statements )
		{
			return ILConstruct.Sequence( statements );
		}

		protected override ILConstruct EmitStatementExpression( ILEmittingContext context, ILConstruct statement, ILConstruct contextExpression )
		{
			return ILConstruct.Composite( statement, contextExpression );
		}

		protected override ILConstruct MakeNullLiteral( ILEmittingContext context )
		{
			return ILConstruct.Literal( typeof( ILConstruct.Any ), default( object ), il => il.EmitLdnull() );
		}

		protected override ILConstruct MakeInt32Literal( ILEmittingContext context, int constant )
		{
			switch ( constant )
			{
				case 0:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_0() );
				}
				case 1:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_1() );
				}
				case 2:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_2() );
				}
				case 3:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_3() );
				}
				case 4:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_4() );
				}
				case 5:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_5() );
				}
				case 6:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_6() );
				}
				case 7:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_7() );
				}
				case 8:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_8() );
				}
				case -1:
				{
					return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_M1() );
				}
				default:
				{
					// ReSharper disable RedundantIfElseBlock
					if ( 0 <= constant && constant <= 255 )
					{
						return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4_S( unchecked( ( byte )constant ) ) );
					}
					else
					{
						return ILConstruct.Literal( typeof( int ), constant, il => il.EmitLdc_I4( constant ) );
					}
					// ReSharper restore RedundantIfElseBlock
				}
			}
		}

		protected override ILConstruct MakeInt64Literal( ILEmittingContext context, long constant )
		{
			return ILConstruct.Literal( typeof( long ), constant, il => il.EmitLdc_I8( constant ) );
		}

		protected override ILConstruct MakeStringLiteral( ILEmittingContext context, string constant )
		{
			return ILConstruct.Literal( typeof( string ), constant, il => il.EmitLdstr( constant ) );
		}

		protected override ILConstruct EmitThisReferenceExpression( ILEmittingContext context )
		{
			return ILConstruct.Literal( context.SerializerType, "(this)", il => il.EmitLdarg_0() );
		}

		protected override ILConstruct EmitBoxExpression( ILEmittingContext context, Type valueType, ILConstruct value )
		{
			return
				ILConstruct.UnaryOperator(
					"box",
					value,
					( il, val ) =>
					{
						val.LoadValue( il, false );
						il.EmitBox( valueType );
					}
				);
		}

		protected override ILConstruct EmitNotExpression( ILEmittingContext context, ILConstruct booleanExpression )
		{
			if ( booleanExpression.ContextType != typeof( bool ) )
			{
				throw new ArgumentException(
					String.Format( CultureInfo.CurrentCulture, "Not expression must be Boolean elementType, but actual is '{0}'.", booleanExpression.ContextType ),
					"booleanExpression"
				);
			}

			return
				ILConstruct.UnaryOperator(
					"!",
					booleanExpression,
					( il, val ) =>
					{
						val.LoadValue( il, false );
						il.EmitNot();
					},
					( il, val, @else ) =>
					{
						val.LoadValue( il, false );
						il.EmitBrtrue( @else );
					}
				);
		}

		protected override ILConstruct EmitEqualsExpression( ILEmittingContext context, ILConstruct left, ILConstruct right )
		{
			var equality = left.ContextType.GetMethod( "op_Equality" );
			return
				ILConstruct.BinaryOperator(
					"==",
					typeof( bool ),
					left,
					right,
					( il, l, r ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( equality == null )
						{
							il.EmitCeq();
						}
						else
						{
							il.EmitAnyCall( equality );
						}
					},
					( il, l, r, @else ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( equality == null )
						{
							il.EmitCeq();
						}
						else
						{
							il.EmitAnyCall( equality );
						}

						il.EmitBrfalse( @else );
					}
				);
		}

		protected override ILConstruct EmitGreaterThanExpression( ILEmittingContext context, ILConstruct left, ILConstruct right )
		{
#if DEBUG
			Contract.Assert( left.ContextType.IsPrimitive && left.ContextType != typeof( string ) );
#endif
			var greaterThan = left.ContextType.GetMethod( "op_GreaterThan" );
			return
				ILConstruct.BinaryOperator(
					">",
					typeof( bool ),
					left,
					right,
					( il, l, r ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( greaterThan == null )
						{
							il.EmitCgt();
						}
						else
						{
							il.EmitAnyCall( greaterThan );
						}
					},
					( il, l, r, @else ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( greaterThan == null )
						{
							il.EmitCgt();
						}
						else
						{
							il.EmitAnyCall( greaterThan );
						}
						il.EmitBrfalse( @else );
					}
				);
		}

		protected override ILConstruct EmitLessThanExpression( ILEmittingContext context, ILConstruct left, ILConstruct right )
		{
#if DEBUG
			Contract.Assert( left.ContextType.IsPrimitive && left.ContextType != typeof( string ) );
#endif
			var lessThan = left.ContextType.GetMethod( "op_LessThan" );
			return
				ILConstruct.BinaryOperator(
					"<",
					typeof( bool ),
					left,
					right,
					( il, l, r ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( lessThan == null )
						{
							il.EmitClt();
						}
						else
						{
							il.EmitAnyCall( lessThan );
						}
					},
					( il, l, r, @else ) =>
					{
						l.LoadValue( il, false );
						r.LoadValue( il, false );
						if ( lessThan == null )
						{
							il.EmitClt();
						}
						else
						{
							il.EmitAnyCall( lessThan );
						}

						il.EmitBrfalse( @else );
					}
				);
		}

		protected override ILConstruct EmitIncrementExpression( ILEmittingContext context, ILConstruct int32Value )
		{
			return
				ILConstruct.UnaryOperator(
					"++",
					int32Value,
					( il, variable ) =>
					{
						variable.LoadValue( il, false );
						il.EmitLdc_I4_1();
						il.EmitAdd();
						variable.StoreValue( il );
					}
				);
		}

		protected override ILConstruct EmitTypeOfExpression( ILEmittingContext context, Type type )
		{
			return
				ILConstruct.Literal(
					typeof( Type ),
					type,
					il => il.EmitTypeOf( type )
				);
		}

		protected override ILConstruct DeclareLocal( ILEmittingContext context, Type type, string name, ILConstruct initExpression )
		{
			return
				ILConstruct.Variable(
					context,
					type,
					name,
					( il, variable ) =>
					{
						if ( initExpression != null )
						{
							initExpression.LoadValue( il, false );
							variable.StoreValue( il );
						}
					}
				);
		}

		protected override ILConstruct EmitInvokeVoidMethod( ILEmittingContext context, ILConstruct instance, MethodInfo method, params ILConstruct[] arguments )
		{
			return
				method.ReturnType == typeof( void )
					? ILConstruct.Invoke( instance, method, arguments )
					: ILConstruct.Sequence(
						new[]
						{
							ILConstruct.Invoke( instance, method, arguments ),
							ILConstruct.Instruction( "pop", typeof( void ), false, il => il.EmitPop() )
						}
					);
		}

		protected override ILConstruct EmitCreateNewObjectExpression( ILEmittingContext context, ConstructorInfo constructor, params ILConstruct[] arguments )
		{
			return ILConstruct.NewObject( constructor, arguments );
		}

		protected override ILConstruct EmitCreateNewArrayExpression( ILEmittingContext context, Type elementType, int length, IEnumerable<ILConstruct> initialElements )
		{
			return
				ILConstruct.Variable(
					context,
					elementType.MakeArrayType(),
					"array",
					( il, variable ) =>
					{
						il.EmitNewarr( elementType, length );
						variable.StoreValue( il );
						var index = 0;
						foreach ( var initialElement in initialElements )
						{
							variable.LoadValue( il, false );
							this.MakeInt32Literal( context, index ).LoadValue( il, false );
							initialElement.LoadValue( il, false );
							il.EmitStelem( elementType );
							index++;
						}
					}
				);
		}

		protected override ILConstruct EmitInvokeMethodExpression( ILEmittingContext context, ILConstruct instance, MethodInfo method, IEnumerable<ILConstruct> arguments )
		{
			return ILConstruct.Invoke( instance, method, arguments );
		}

		protected override ILConstruct EmitGetPropretyExpression( ILEmittingContext context, ILConstruct instance, PropertyInfo property )
		{
			return ILConstruct.Invoke( instance, property.GetGetMethod( true ), ILConstruct.NoArguments );
		}

		protected override ILConstruct EmitGetFieldExpression( ILEmittingContext context, ILConstruct instance, FieldInfo field )
		{
			return ILConstruct.LoadField( instance, field );
		}

		protected override ILConstruct EmitSetProprety( ILEmittingContext context, ILConstruct instance, PropertyInfo property, ILConstruct value )
		{
#if DEBUG
// ReSharper disable PossibleNullReferenceException
			Contract.Assert(
				property.GetSetMethod( true ) != null,
				property.DeclaringType.FullName + "::" + property.Name + ".set != null" 
			);
// ReSharper restore PossibleNullReferenceException
#endif
			return ILConstruct.Invoke( instance, property.GetSetMethod( true ), new[] { value } );
		}

		protected override ILConstruct EmitSetField( ILEmittingContext context, ILConstruct instance, FieldInfo field, ILConstruct value )
		{
			return ILConstruct.StoreField( instance, field, value );
		}

		protected override ILConstruct EmitSetVariableStatement( ILEmittingContext context, ILConstruct variable, ILConstruct value )
		{
			return ILConstruct.StoreLocal( variable, value );
		}

		protected override ILConstruct EmitThrowExpression( ILEmittingContext context, Type expressionType, ILConstruct exceptionExpression )
		{
			return
				ILConstruct.Instruction(
					"throw",
					expressionType,
					true,
					il =>
					{
						exceptionExpression.LoadValue( il, false );
						il.EmitThrow();
					}
				);
		}

		protected override ILConstruct EmitTryFinallyExpression( ILEmittingContext context, ILConstruct tryExpression, ILConstruct finallyStatement )
		{
			return
				ILConstruct.Instruction(
					"try-finally",
					tryExpression.ContextType,
					false,
					il =>
					{
						il.BeginExceptionBlock();
						tryExpression.Evaluate( il );
						il.BeginFinallyBlock();
						finallyStatement.Evaluate( il );
						il.EndExceptionBlock();
					}
				);
		}

		protected override ILConstruct EmitConditionalExpression( ILEmittingContext context, ILConstruct conditionExpression, ILConstruct thenExpression, ILConstruct elseExpression )
		{
			return
				ILConstruct.IfThenElse(
					conditionExpression,
					thenExpression,
					elseExpression
				);
		}

		protected override ILConstruct EmitAndConditionalExpression( ILEmittingContext context, IList<ILConstruct> conditionExpressions, ILConstruct thenExpression, ILConstruct elseExpression )
		{
			return
				ILConstruct.IfThenElse(
					ILConstruct.AndCondition( conditionExpressions ),
					thenExpression,
					elseExpression
				);
		}

		protected override ILConstruct EmitStringSwitchStatement( ILEmittingContext context, ILConstruct target, IDictionary<string, ILConstruct> cases )
		{
			// Simply if statements
			ILConstruct @else = null;
			foreach ( var @case in cases )
			{
				@else =
					this.EmitConditionalExpression(
						context,
						this.EmitInvokeMethodExpression(
							context,
							null,
							Metadata._String.op_Equality,
							target,
							this.MakeStringLiteral( context, @case.Key )
						),
						@case.Value,
						@else
					);
			}

			return @else;
		}

		protected override ILConstruct EmitForLoop( ILEmittingContext context, ILConstruct count, Func<ForLoopContext, ILConstruct> loopBodyEmitter )
		{
			var i =
				this.DeclareLocal(
					context,
					typeof( int ),
					"i",
					null
				);

			var loopContext = new ForLoopContext( i );
			return
				this.EmitSequentialStatements(
					context,
					i,
					ILConstruct.Instruction(
						"for",
						typeof( void ),
						false,
						il =>
						{
							var forCond = il.DefineLabel( "FOR_COND" );
							il.EmitBr( forCond );
							var body = il.DefineLabel( "BODY" );
							il.MarkLabel( body );
							loopBodyEmitter( loopContext ).Evaluate( il );
							// increment
							i.LoadValue( il, false );
							il.EmitLdc_I4_1();
							il.EmitAdd();
							i.StoreValue( il );
							// cond
							il.MarkLabel( forCond );
							i.LoadValue( il, false );
							count.LoadValue( il, false );
							il.EmitBlt( body );
						}
					)
				);
		}

		protected override ILConstruct EmitForEachLoop( ILEmittingContext context, CollectionTraits traits, ILConstruct collection, Func<ILConstruct, ILConstruct> loopBodyEmitter )
		{
			return
				ILConstruct.Instruction(
					"foreach",
					typeof( void ),
					false,
					il =>
					{
						var enumerator = il.DeclareLocal( traits.GetEnumeratorMethod.ReturnType, "enumerator" );
						var currentItem =
							this.DeclareLocal( 
								context,
								traits.ElementType,
								"item",
								null
							);

						// gets enumerator
						collection.LoadValue( il, true );

						il.EmitAnyCall( traits.GetEnumeratorMethod );
						il.EmitAnyStloc( enumerator );

						if ( typeof( IDisposable ).IsAssignableFrom( traits.GetEnumeratorMethod.ReturnType ) )
						{
							il.BeginExceptionBlock();
						}

						var startLoop = il.DefineLabel( "START_LOOP" );
						il.MarkLabel( startLoop );
						currentItem.Evaluate( il );

						var endLoop = il.DefineLabel( "END_LOOP" );
						var enumeratorType = traits.GetEnumeratorMethod.ReturnType;
						MethodInfo moveNextMethod = enumeratorType.GetMethod( "MoveNext", Type.EmptyTypes );
						PropertyInfo currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );

						if ( moveNextMethod == null )
						{
							moveNextMethod = Metadata._IEnumerator.MoveNext;
						}

						if ( currentProperty == null )
						{
							if ( enumeratorType == typeof( IDictionaryEnumerator ) )
							{
								currentProperty = Metadata._IDictionaryEnumerator.Current;
							}
							else if ( enumeratorType.IsInterface )
							{
								if ( enumeratorType.IsGenericType && enumeratorType.GetGenericTypeDefinition() == typeof( IEnumerator<> ) )
								{
									currentProperty = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetProperty( "Current" );
								}
								else
								{
									currentProperty = Metadata._IEnumerator.Current;
								}
							}
						}

						Contract.Assert( currentProperty != null, enumeratorType.ToString() );

						// iterates
						if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
						{
							il.EmitAnyLdloca( enumerator );
						}
						else
						{
							il.EmitAnyLdloc( enumerator );
						}

						il.EmitAnyCall( moveNextMethod );
						il.EmitBrfalse( endLoop );

						// get current item
						if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
						{
							il.EmitAnyLdloca( enumerator );
						}
						else
						{
							il.EmitAnyLdloc( enumerator );
						}
						il.EmitGetProperty( currentProperty );
						currentItem.StoreValue( il );

						// body
						loopBodyEmitter( currentItem ).Evaluate( il );

						// end loop
						il.EmitBr( startLoop );

						il.MarkLabel( endLoop );

						// Dispose
						if ( typeof( IDisposable ).IsAssignableFrom( traits.GetEnumeratorMethod.ReturnType ) )
						{
							il.BeginFinallyBlock();

							if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
							{
								var disposeMethod = traits.GetEnumeratorMethod.ReturnType.GetMethod( "Dispose" );
								if ( disposeMethod != null && disposeMethod.GetParameters().Length == 0 && disposeMethod.ReturnType == typeof( void ) )
								{
									il.EmitAnyLdloca( enumerator );
									il.EmitAnyCall( disposeMethod );
								}
								else
								{
									il.EmitAnyLdloc( enumerator );
									il.EmitBox( traits.GetEnumeratorMethod.ReturnType );
									il.EmitAnyCall( Metadata._IDisposable.Dispose );
								}
							}
							else
							{
								il.EmitAnyLdloc( enumerator );
								il.EmitAnyCall( Metadata._IDisposable.Dispose );
							}

							il.EndExceptionBlock();
						}
					}
				);
		}

		protected override ILConstruct EmitGetSerializerExpression( ILEmittingContext context, Type targetType )
		{
			var instructions = context.Emitter.RegisterSerializer( targetType );
			return
				ILConstruct.Instruction(
					"getserializer",
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( ILEmittingContext codeGenerationContext )
		{
			return context => codeGenerationContext.Emitter.CreateInstance<TObject>( context );
		}
	}
}