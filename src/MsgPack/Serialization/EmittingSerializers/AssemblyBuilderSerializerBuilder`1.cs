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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{AssemblyBuilderEmittingContext,TConstruct,TObject}"/> with <see cref="AssemblyBuilder"/>.
	/// </summary>
	/// <typeparam name="TObject">The type of the serializing object.</typeparam>
	internal sealed class AssemblyBuilderSerializerBuilder<TObject> : SerializerBuilder<AssemblyBuilderEmittingContext, ILConstruct, TObject>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="AssemblyBuilderSerializerBuilder{TObject}"/> class for instance creation.
		/// </summary>
		public AssemblyBuilderSerializerBuilder() { }

		protected override void EmitMethodPrologue( AssemblyBuilderEmittingContext context, SerializerMethod method )
		{
			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					context.IL = context.Emitter.GetPackToMethodILGenerator();
					break;
				}
				case SerializerMethod.UnpackFromCore:
				{
					context.IL = context.Emitter.GetUnpackFromMethodILGenerator();
					break;
				}
				case SerializerMethod.UnpackToCore:
				{
					context.IL = context.Emitter.GetUnpackToMethodILGenerator();
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		protected override void EmitMethodPrologue( AssemblyBuilderEmittingContext context, EnumSerializerMethod method )
		{
			switch ( method )
			{
				case EnumSerializerMethod.PackUnderlyingValueTo:
				{
					context.IL = context.EnumEmitter.GetPackUnderyingValueToMethodILGenerator();
					break;
				}
				case EnumSerializerMethod.UnpackFromUnderlyingValue:
				{
					context.IL = context.EnumEmitter.GetUnpackFromUnderlyingValueMethodILGenerator();
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		protected override void EmitMethodPrologue( AssemblyBuilderEmittingContext context, CollectionSerializerMethod method, MethodInfo declaration )
		{
			switch ( method )
			{
				case CollectionSerializerMethod.AddItem:
				{
					context.IL = context.Emitter.GetAddItemMethodILGenerator( declaration );
					break;
				}
				case CollectionSerializerMethod.CreateInstance:
				{
					context.IL = context.Emitter.GetCreateInstanceMethodILGenerator( declaration );
					break;
				}
				case CollectionSerializerMethod.RestoreSchema:
				{
					context.IL = context.Emitter.GetRestoreSchemaMethodILGenerator();
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		protected override void EmitMethodEpilogue( AssemblyBuilderEmittingContext context, SerializerMethod method, ILConstruct construct )
		{
			EmitMethodEpilogue( context, construct );
		}

		protected override void EmitMethodEpilogue( AssemblyBuilderEmittingContext context, EnumSerializerMethod method, ILConstruct construct )
		{
			EmitMethodEpilogue( context, construct );
		}

		protected override void EmitMethodEpilogue( AssemblyBuilderEmittingContext context, CollectionSerializerMethod method, ILConstruct construct )
		{
			EmitMethodEpilogue( context, construct );
		}

		private static void EmitMethodEpilogue( AssemblyBuilderEmittingContext context, ILConstruct construct )
		{
			try
			{
				if ( construct != null )
				{
					construct.Evaluate( context.IL );
				}

				context.IL.EmitRet();
			}
			finally
			{
				context.IL.FlushTrace();
				SerializerDebugging.FlushTraceData();
			}
		}

		protected override ILConstruct EmitSequentialStatements( AssemblyBuilderEmittingContext context, Type contextType, IEnumerable<ILConstruct> statements )
		{
			return ILConstruct.Sequence( contextType, statements );
		}

		protected override ILConstruct MakeNullLiteral( AssemblyBuilderEmittingContext context, Type contextType )
		{
			return ILConstruct.Literal( contextType, default( object ), il => il.EmitLdnull() );
		}

		protected override ILConstruct MakeByteLiteral( AssemblyBuilderEmittingContext context, byte constant )
		{
			return MakeIntegerLiteral( typeof( byte ), constant );
		}

		protected override ILConstruct MakeSByteLiteral( AssemblyBuilderEmittingContext context, sbyte constant )
		{
			return MakeIntegerLiteral( typeof( sbyte ), constant );
		}

		protected override ILConstruct MakeInt16Literal( AssemblyBuilderEmittingContext context, short constant )
		{
			return MakeIntegerLiteral( typeof( short ), constant );
		}

		protected override ILConstruct MakeUInt16Literal( AssemblyBuilderEmittingContext context, ushort constant )
		{
			return MakeIntegerLiteral( typeof( ushort ), constant );
		}

		protected override ILConstruct MakeInt32Literal( AssemblyBuilderEmittingContext context, int constant )
		{
			return MakeIntegerLiteral( typeof( int ), constant );
		}

		protected override ILConstruct MakeUInt32Literal( AssemblyBuilderEmittingContext context, uint constant )
		{
			return MakeIntegerLiteral( typeof( uint ), unchecked( ( int )constant ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Many case switch" )]
		private static ILConstruct MakeIntegerLiteral( Type contextType, int constant )
		{
			switch ( constant )
			{
				case 0:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_0() );
				}
				case 1:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_1() );
				}
				case 2:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_2() );
				}
				case 3:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_3() );
				}
				case 4:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_4() );
				}
				case 5:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_5() );
				}
				case 6:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_6() );
				}
				case 7:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_7() );
				}
				case 8:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_8() );
				}
				case -1:
				{
					return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_M1() );
				}
				default:
				{
					if ( SByte.MinValue <= constant && constant <= SByte.MaxValue )
					{
						return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4_S( unchecked( ( byte )constant ) ) );
					}
					else
					{
						return ILConstruct.Literal( contextType, constant, il => il.EmitLdc_I4( constant ) );
					}
				}
			}
		}

		protected override ILConstruct MakeInt64Literal( AssemblyBuilderEmittingContext context, long constant )
		{
			return ILConstruct.Literal( typeof( long ), constant, il => il.EmitLdc_I8( constant ) );
		}

		protected override ILConstruct MakeUInt64Literal( AssemblyBuilderEmittingContext context, ulong constant )
		{
			return ILConstruct.Literal( typeof( ulong ), constant, il => il.EmitLdc_I8( unchecked( ( long )constant ) ) );
		}

		protected override ILConstruct MakeReal32Literal( AssemblyBuilderEmittingContext context, float constant )
		{
			return ILConstruct.Literal( typeof( float ), constant, il => il.EmitLdc_R4( constant ) );
		}

		protected override ILConstruct MakeReal64Literal( AssemblyBuilderEmittingContext context, double constant )
		{
			return ILConstruct.Literal( typeof( double ), constant, il => il.EmitLdc_R8( constant ) );
		}

		protected override ILConstruct MakeBooleanLiteral( AssemblyBuilderEmittingContext context, bool constant )
		{
			return MakeIntegerLiteral( typeof( bool ), constant ? 1 : 0 );
		}

		protected override ILConstruct MakeCharLiteral( AssemblyBuilderEmittingContext context, char constant )
		{
			return MakeIntegerLiteral( typeof( char ), constant );
		}

		protected override ILConstruct MakeStringLiteral( AssemblyBuilderEmittingContext context, string constant )
		{
			return ILConstruct.Literal( typeof( string ), constant, il => il.EmitLdstr( constant ) );
		}

		protected override ILConstruct MakeEnumLiteral( AssemblyBuilderEmittingContext context, Type type, object constant )
		{
			var underyingType = Enum.GetUnderlyingType( type );

			switch ( Type.GetTypeCode( underyingType ) )
			{
				case TypeCode.Byte:
				{
					// tiny integrals are represented as int32 in IL operands.
					return this.MakeInt32Literal( context, ( byte )constant );
				}
				case TypeCode.SByte:
				{
					// tiny integrals are represented as int32 in IL operands.
					return this.MakeInt32Literal( context, ( sbyte )constant );
				}
				case TypeCode.Int16:
				{
					// tiny integrals are represented as int32 in IL operands.
					return this.MakeInt32Literal( context, ( short )constant );
				}
				case TypeCode.UInt16:
				{
					// tiny integrals are represented as int32 in IL operands.
					return this.MakeInt32Literal( context, ( ushort )constant );
				}
				case TypeCode.Int32:
				{
					return this.MakeInt32Literal( context, ( int )constant );
				}
				case TypeCode.UInt32:
				{
					// signeds and unsigneds are identical in IL operands.
					return this.MakeInt32Literal( context, unchecked( ( int )( uint )constant ) );
				}
				case TypeCode.Int64:
				{
					return this.MakeInt64Literal( context, ( long )constant );
				}
				case TypeCode.UInt64:
				{
					// signeds and unsigneds are identical in IL operands.
					return this.MakeInt64Literal( context, unchecked( ( long )( ulong )constant ) );
				}
				default:
				{
					// bool and char are not supported.
					// Of course these are very rare, and bool is not supported in ExpressionTree (it hurts portability),
					// and char is not supported by MsgPack protocol itself.
					throw new NotSupportedException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Underying type '{0}' is not supported.",
							underyingType
						)
					);
				}
			}
		}

		protected override ILConstruct MakeDefaultLiteral( AssemblyBuilderEmittingContext context, Type type )
		{
			return
				ILConstruct.Literal(
					type,
					"default(" + type + ")",
					il =>
					{
						var temp = il.DeclareLocal( type );
						il.EmitAnyLdloca( temp );
						il.EmitInitobj( type );
						il.EmitAnyLdloc( temp );
					}
				);
		}

		protected override ILConstruct EmitThisReferenceExpression( AssemblyBuilderEmittingContext context )
		{
			return ILConstruct.Literal( context.GetSerializerType( typeof( TObject ) ), "(this)", il => il.EmitLdarg_0() );
		}

		protected override ILConstruct EmitBoxExpression( AssemblyBuilderEmittingContext context, Type valueType, ILConstruct value )
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

		protected override ILConstruct EmitUnboxAnyExpression( AssemblyBuilderEmittingContext context, Type targetType, ILConstruct value )
		{
			return
				ILConstruct.UnaryOperator(
					"unbox.any",
					value,
					( il, val ) =>
					{
						val.LoadValue( il, false );
						il.EmitUnbox_Any( targetType );
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitNotExpression( AssemblyBuilderEmittingContext context, ILConstruct booleanExpression )
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
						il.EmitLdc_I4_0();
						il.EmitCeq();
					},
					( il, val, @else ) =>
					{
						val.LoadValue( il, false );
						il.EmitBrtrue( @else );
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitEqualsExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitGreaterThanExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitLessThanExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
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

		protected override ILConstruct EmitIncrement( AssemblyBuilderEmittingContext context, ILConstruct int32Value )
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

		protected override ILConstruct EmitTypeOfExpression( AssemblyBuilderEmittingContext context, Type type )
		{
			return
				ILConstruct.Literal(
					typeof( Type ),
					type,
					il => il.EmitTypeOf( type )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitMethodOfExpression( AssemblyBuilderEmittingContext context, MethodBase method )
		{
			var instructions =
				context.Emitter.RegisterMethod(
					method
				);

			return
				ILConstruct.Instruction(
					"getsetter",
					typeof( MethodBase ),
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitFieldOfExpression( AssemblyBuilderEmittingContext context, FieldInfo field )
		{
			var instructions =
				context.Emitter.RegisterField(
					field
				);

			return
				ILConstruct.Instruction(
					"getfield",
					typeof( FieldInfo ),
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		protected override ILConstruct DeclareLocal( AssemblyBuilderEmittingContext context, Type type, string name )
		{
			return
				ILConstruct.Variable(
					type,
					name
				);
		}

		protected override ILConstruct ReferArgument( AssemblyBuilderEmittingContext context, Type type, string name, int index )
		{
			return ILConstruct.Argument( index, type, name );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ILConstruct EmitInvokeVoidMethod( AssemblyBuilderEmittingContext context, ILConstruct instance, MethodInfo method, params ILConstruct[] arguments )
		{
			return
				method.ReturnType == typeof( void )
					? ILConstruct.Invoke( instance, method, arguments )
					: ILConstruct.Sequence(
						typeof( void ),
						new[]
						{
							ILConstruct.Invoke( instance, method, arguments ),
							ILConstruct.Instruction( "pop", typeof( void ), false, il => il.EmitPop() )
						}
					);
		}

		protected override ILConstruct EmitCreateNewObjectExpression( AssemblyBuilderEmittingContext context, ILConstruct variable, ConstructorInfo constructor, params ILConstruct[] arguments )
		{
			return ILConstruct.NewObject( variable, constructor, arguments );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override ILConstruct EmitCreateNewArrayExpression( AssemblyBuilderEmittingContext context, Type elementType, int length )
		{
			var array =
				ILConstruct.Variable(
					elementType.MakeArrayType(),
					"array"
				);

			return
				ILConstruct.Composite(
					ILConstruct.Sequence(
						array.ContextType,
						new[]
						{
							array,
							ILConstruct.Instruction( 
								"NewArray",
								array.ContextType,
								false,
								il =>
								{
									il.EmitNewarr( elementType, length );
									array.StoreValue( il );
								}
							)
						}
					),
					array
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override ILConstruct EmitCreateNewArrayExpression( AssemblyBuilderEmittingContext context, Type elementType, int length, IEnumerable<ILConstruct> initialElements )
		{
			var array =
				ILConstruct.Variable(
					elementType.MakeArrayType(),
					"array"
				);

			return
				ILConstruct.Composite(
					ILConstruct.Sequence(
						array.ContextType,
						new[]
						{
							array,
							ILConstruct.Instruction( 
								"CreateArray",
								array.ContextType,
								false,
								il =>
								{
									il.EmitNewarr( elementType, length );
									array.StoreValue( il );
									var index = 0;
									foreach ( var initialElement in initialElements )
									{
										array.LoadValue( il, false );
										this.MakeInt32Literal( context, index ).LoadValue( il, false );
										initialElement.LoadValue( il, false );
										il.EmitStelem( elementType );
										index++;
									}
								}
							)
						}
					),
					array
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitSetArrayElementStatement( AssemblyBuilderEmittingContext context, ILConstruct array, ILConstruct index, ILConstruct value )
		{
			return
				ILConstruct.Instruction(
					"SetArrayElement",
					array.ContextType,
					false,
					il =>
					{
						il.EmitAnyStelem(
							value.ContextType,
							il0 =>
							{
								array.LoadValue( il0, false );
							},
							il0 =>
							{
								index.LoadValue( il0, false );
							},
							il0 =>
							{
								value.LoadValue( il0, true );
							}
						);
					}
				);
		}

		protected override ILConstruct EmitInvokeMethodExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, MethodInfo method, IEnumerable<ILConstruct> arguments )
		{
			return ILConstruct.Invoke( instance, method, arguments );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ILConstruct EmitGetPropertyExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, PropertyInfo property )
		{
			return ILConstruct.Invoke( instance, property.GetGetMethod( true ), ILConstruct.NoArguments );
		}

		protected override ILConstruct EmitGetFieldExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, FieldInfo field )
		{
			return ILConstruct.LoadField( instance, field );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ILConstruct EmitSetProperty( AssemblyBuilderEmittingContext context, ILConstruct instance, PropertyInfo property, ILConstruct value )
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

		protected override ILConstruct EmitSetField( AssemblyBuilderEmittingContext context, ILConstruct instance, FieldInfo field, ILConstruct value )
		{
			return ILConstruct.StoreField( instance, field, value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitLoadVariableExpression( AssemblyBuilderEmittingContext context, ILConstruct variable )
		{
			return ILConstruct.Instruction( "load", variable.ContextType, false, il => variable.LoadValue( il, false ) );
		}

		protected override ILConstruct EmitStoreVariableStatement( AssemblyBuilderEmittingContext context, ILConstruct variable, ILConstruct value )
		{
			return ILConstruct.StoreLocal( variable, value );
		}

		protected override ILConstruct EmitThrowExpression( AssemblyBuilderEmittingContext context, Type expressionType, ILConstruct exceptionExpression )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ILConstruct EmitTryFinally( AssemblyBuilderEmittingContext context, ILConstruct tryStatement, ILConstruct finallyStatement )
		{
			return
				ILConstruct.Instruction(
					"try-finally",
					tryStatement.ContextType,
					false,
					il =>
					{
						il.BeginExceptionBlock();
						tryStatement.Evaluate( il );
						il.BeginFinallyBlock();
						finallyStatement.Evaluate( il );
						il.EndExceptionBlock();
					}
				);
		}

		protected override ILConstruct EmitConditionalExpression( AssemblyBuilderEmittingContext context, ILConstruct conditionExpression, ILConstruct thenExpression, ILConstruct elseExpression )
		{
			return
				ILConstruct.IfThenElse(
					conditionExpression,
					thenExpression,
					elseExpression
				);
		}

		protected override ILConstruct EmitAndConditionalExpression( AssemblyBuilderEmittingContext context, IList<ILConstruct> conditionExpressions, ILConstruct thenExpression, ILConstruct elseExpression )
		{
			return
				ILConstruct.IfThenElse(
					ILConstruct.AndCondition( conditionExpressions ),
					thenExpression,
					elseExpression
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ILConstruct EmitStringSwitchStatement( AssemblyBuilderEmittingContext context, ILConstruct target, IDictionary<string, ILConstruct> cases, ILConstruct defaultCase )
		{
			// Simple if statements
			ILConstruct @else = defaultCase;
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

		protected override ILConstruct EmitForLoop( AssemblyBuilderEmittingContext context, ILConstruct count, Func<ForLoopContext, ILConstruct> loopBodyEmitter )
		{
			var i =
				this.DeclareLocal(
					context,
					typeof( int ),
					"i"
				);

			var loopContext = new ForLoopContext( i );
			return
				this.EmitSequentialStatements(
					context,
					i.ContextType,
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

		protected override ILConstruct EmitForEachLoop( AssemblyBuilderEmittingContext context, CollectionTraits traits, ILConstruct collection, Func<ILConstruct, ILConstruct> loopBodyEmitter )
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
								"item"
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
						var moveNextMethod = Metadata._IEnumerator.FindEnumeratorMoveNextMethod( enumeratorType );
						var currentProperty = Metadata._IEnumerator.FindEnumeratorCurrentProperty( enumeratorType, traits );

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

		protected override ILConstruct EmitEnumFromUnderlyingCastExpression( AssemblyBuilderEmittingContext context, Type enumType, ILConstruct underlyingValue )
		{
			// No operations are needed in IL level.
			return underlyingValue;
		}

		protected override ILConstruct EmitEnumToUnderlyingCastExpression( AssemblyBuilderEmittingContext context, Type underlyingType, ILConstruct enumValue )
		{
			// No operations are needed in IL level.
			return enumValue;
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( AssemblyBuilderEmittingContext codeGenerationContext, PolymorphismSchema schema )
		{
			return context => codeGenerationContext.Emitter.CreateInstance<TObject>( context, schema );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor( AssemblyBuilderEmittingContext codeGenerationContext )
		{
			return context =>
				codeGenerationContext.EnumEmitter.CreateInstance<TObject>(
					context,
					EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod( context, typeof( TObject ), EnumMemberSerializationMethod.Default )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitGetSerializerExpression( AssemblyBuilderEmittingContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			var realSchema = itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo );
			var instructions =
				context.Emitter.RegisterSerializer(
					targetType,
					memberInfo == null
						? EnumMemberSerializationMethod.Default
						: memberInfo.Value.GetEnumMemberSerializationMethod(),
					memberInfo == null
						? DateTimeMemberConversionMethod.Default
						: memberInfo.Value.GetDateTimeMemberConversionMethod(),
					realSchema,
					() => this.EmitConstructPolymorphismSchema( context, realSchema ) 
				);

			return
				ILConstruct.Instruction(
					"getserializer",
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					false,
					// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		private IEnumerable<ILConstruct> EmitConstructPolymorphismSchema(
			AssemblyBuilderEmittingContext context,
			PolymorphismSchema currentSchema 
		)
		{
			var schema = this.DeclareLocal( context, typeof( PolymorphismSchema ), "schema" );
			
			yield return schema;
			
			foreach ( var construct in this.EmitConstructPolymorphismSchema( context, schema, currentSchema ) )
			{
				yield return construct;
			}

			yield return this.EmitLoadVariableExpression( context, schema );
		}

		protected override AssemblyBuilderEmittingContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			string serializerTypeName, serializerTypeNamespace;
			DefaultSerializerNameResolver.ResolveTypeName(
				true,
				typeof( TObject ),
				this.GetType().Namespace,
				out serializerTypeName,
				out serializerTypeNamespace 
			);
			var spec =
				new SerializerSpecification(
					typeof( TObject ),
					CollectionTraitsOfThis,
					serializerTypeName,
					serializerTypeNamespace
				);

			return
				new AssemblyBuilderEmittingContext(
					context,
					typeof( TObject ),
					() => SerializationMethodGeneratorManager.Get().CreateEmitter( spec, BaseClass, EmitterFlavor.FieldBased ),
					() => SerializationMethodGeneratorManager.Get().CreateEnumEmitter( context, spec, EmitterFlavor.FieldBased )
				);
		}

#if !SILVERLIGHT
		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			var asAssemblyBuilderCodeGenerationContext = context as AssemblyBuilderCodeGenerationContext;
			if ( asAssemblyBuilderCodeGenerationContext == null )
			{
				throw new ArgumentException(
					"'context' was not created with CreateGenerationContextForCodeGeneration method.",
					"context"
				);
			}

			var emittingContext =
				asAssemblyBuilderCodeGenerationContext.CreateEmittingContext(
					typeof( TObject ),
					CollectionTraitsOfThis,
					BaseClass
				);

			if ( !typeof( TObject ).GetIsEnum() )
			{
				this.BuildSerializer( emittingContext, concreteType, itemSchema );
				// Finish type creation, and discard returned ctor.
				emittingContext.Emitter.CreateConstructor<TObject>();
			}
			else
			{
				this.BuildEnumSerializer( emittingContext );
				// Finish type creation, and discard returned ctor.
				emittingContext.EnumEmitter.CreateConstructor<TObject>();
			}
		}
#endif
	}
}