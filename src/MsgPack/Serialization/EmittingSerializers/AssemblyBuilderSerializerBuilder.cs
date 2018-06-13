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
using System.Collections.Generic;
#if NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{AssemblyBuilderEmittingContext,TConstruct}"/> with <see cref="AssemblyBuilder"/>.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Constracts" )]
	internal sealed class AssemblyBuilderSerializerBuilder : SerializerBuilder<AssemblyBuilderEmittingContext, ILConstruct>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="AssemblyBuilderSerializerBuilder"/> class for instance creation.
		/// </summary>
		/// <param name="targetType">The type of serialization target.</param>
		/// <param name="collectionTraits">The collection traits of the serialization target.</param>
		public AssemblyBuilderSerializerBuilder( Type targetType, CollectionTraits collectionTraits )
			: base( targetType, collectionTraits ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitSequentialStatements( AssemblyBuilderEmittingContext context, TypeDefinition contextType, IEnumerable<ILConstruct> statements )
		{
			return ILConstruct.Sequence( contextType.ResolveRuntimeType(), statements );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct MakeNullLiteral( AssemblyBuilderEmittingContext context, TypeDefinition contextType )
		{
			return ILConstruct.Literal( contextType.ResolveRuntimeType(), default( object ), il => il.EmitLdnull() );
		}

		protected override ILConstruct MakeByteLiteral( AssemblyBuilderEmittingContext context, byte constant )
		{
			return MakeIntegerLiteral( TypeDefinition.ByteType, constant );
		}

		protected override ILConstruct MakeSByteLiteral( AssemblyBuilderEmittingContext context, sbyte constant )
		{
			return MakeIntegerLiteral( TypeDefinition.SByteType, constant );
		}

		protected override ILConstruct MakeInt16Literal( AssemblyBuilderEmittingContext context, short constant )
		{
			return MakeIntegerLiteral( TypeDefinition.Int16Type, constant );
		}

		protected override ILConstruct MakeUInt16Literal( AssemblyBuilderEmittingContext context, ushort constant )
		{
			return MakeIntegerLiteral( TypeDefinition.UInt16Type, constant );
		}

		protected override ILConstruct MakeInt32Literal( AssemblyBuilderEmittingContext context, int constant )
		{
			return MakeIntegerLiteral( TypeDefinition.Int32Type, constant );
		}

		protected override ILConstruct MakeUInt32Literal( AssemblyBuilderEmittingContext context, uint constant )
		{
			return MakeIntegerLiteral( TypeDefinition.UInt32Type, unchecked( ( int )constant ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Many case switch" )]
		private static ILConstruct MakeIntegerLiteral( TypeDefinition contextType, int constant )
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
			return ILConstruct.Literal( TypeDefinition.Int64Type, constant, il => il.EmitLdc_I8( constant ) );
		}

		protected override ILConstruct MakeUInt64Literal( AssemblyBuilderEmittingContext context, ulong constant )
		{
			return ILConstruct.Literal( TypeDefinition.UInt64Type, constant, il => il.EmitLdc_I8( unchecked( ( long )constant ) ) );
		}

		protected override ILConstruct MakeReal32Literal( AssemblyBuilderEmittingContext context, float constant )
		{
			return ILConstruct.Literal( TypeDefinition.SingleType, constant, il => il.EmitLdc_R4( constant ) );
		}

		protected override ILConstruct MakeReal64Literal( AssemblyBuilderEmittingContext context, double constant )
		{
			return ILConstruct.Literal( TypeDefinition.DoubleType, constant, il => il.EmitLdc_R8( constant ) );
		}

		protected override ILConstruct MakeBooleanLiteral( AssemblyBuilderEmittingContext context, bool constant )
		{
			return MakeIntegerLiteral( TypeDefinition.BooleanType, constant ? 1 : 0 );
		}

		protected override ILConstruct MakeCharLiteral( AssemblyBuilderEmittingContext context, char constant )
		{
			return MakeIntegerLiteral( TypeDefinition.CharType, constant );
		}

		protected override ILConstruct MakeStringLiteral( AssemblyBuilderEmittingContext context, string constant )
		{
			return ILConstruct.Literal( TypeDefinition.StringType, constant, il => il.EmitLdstr( constant ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct MakeEnumLiteral( AssemblyBuilderEmittingContext context, TypeDefinition type, object constant )
		{
			var underyingType = Enum.GetUnderlyingType( type.ResolveRuntimeType() );

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			switch ( Type.GetTypeCode( underyingType ) )
#else
			switch ( NetStandardCompatibility.GetTypeCode( underyingType ) )
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct MakeDefaultLiteral( AssemblyBuilderEmittingContext context, TypeDefinition type )
		{
			return
				ILConstruct.Literal(
					type.ResolveRuntimeType(),
					"default(" + type + ")",
					il =>
					{
						var temp = il.DeclareLocal( type.ResolveRuntimeType(), context.GetUniqueVariableName( "tmp" ) );
						il.EmitAnyLdloca( temp );
						il.EmitInitobj( type.ResolveRuntimeType() );
						il.EmitAnyLdloc( temp );
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitThisReferenceExpression( AssemblyBuilderEmittingContext context )
		{
			return ILConstruct.Literal( context.GetSerializerType( this.TargetType ), "(this)", il => il.EmitLdarg_0() );
		}

		protected override ILConstruct EmitBoxExpression( AssemblyBuilderEmittingContext context, TypeDefinition valueType, ILConstruct value )
		{
			return
				ILConstruct.UnaryOperator(
					"box",
					value,
					( il, val ) =>
					{
						val.LoadValue( il, false );
						il.EmitBox( valueType.ResolveRuntimeType() );
					}
				);
		}

		protected override ILConstruct EmitUnboxAnyExpression( AssemblyBuilderEmittingContext context, TypeDefinition targetType, ILConstruct value )
		{
			return
				ILConstruct.UnaryOperator(
					"unbox.any",
					value,
					( il, val ) =>
					{
						val.LoadValue( il, false );
						il.EmitUnbox_Any( targetType.ResolveRuntimeType() );
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitNotExpression( AssemblyBuilderEmittingContext context, ILConstruct booleanExpression )
		{
			if ( booleanExpression.ContextType.ResolveRuntimeType() != typeof( bool ) )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitEqualsExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
		{
			var equality = left.ContextType.ResolveRuntimeType().GetMethod( "op_Equality" );
			return
				ILConstruct.BinaryOperator(
					"==",
					TypeDefinition.BooleanType,
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitGreaterThanExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
		{
#if DEBUG && !CORE_CLR
			Contract.Assert( left.ContextType.ResolveRuntimeType().GetIsPrimitive() && left.ContextType.ResolveRuntimeType() != typeof( string ) );
#endif // DEBUG && !CORE_CLR
			var greaterThan = left.ContextType.ResolveRuntimeType().GetMethod( "op_GreaterThan" );
			return
				ILConstruct.BinaryOperator(
					">",
					TypeDefinition.BooleanType,
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitLessThanExpression( AssemblyBuilderEmittingContext context, ILConstruct left, ILConstruct right )
		{
#if DEBUG && !CORE_CLR
			Contract.Assert( left.ContextType.ResolveRuntimeType().GetIsPrimitive() && left.ContextType.ResolveRuntimeType() != typeof( string ) );
#endif // DEBUG && !CORE_CLR
			var lessThan = left.ContextType.ResolveRuntimeType().GetMethod( "op_LessThan" );
			return
				ILConstruct.BinaryOperator(
					"<",
					TypeDefinition.BooleanType,
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

		protected override ILConstruct EmitTypeOfExpression( AssemblyBuilderEmittingContext context, TypeDefinition type )
		{
			return
				ILConstruct.Literal(
					TypeDefinition.TypeType,
					type,
					il => il.EmitTypeOf( type.ResolveRuntimeType() )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitMethodOfExpression( AssemblyBuilderEmittingContext context, MethodBase method )
		{
			var instructions =
				context.Emitter.RegisterMethodCache(
					method
				);

			return
				ILConstruct.Instruction(
					"getsetter",
					TypeDefinition.MethodBaseType,
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitFieldOfExpression( AssemblyBuilderEmittingContext context, FieldInfo field )
		{
			var instructions =
				context.Emitter.RegisterFieldCache(
					field
				);

			return
				ILConstruct.Instruction(
					"getfield",
					TypeDefinition.FieldInfoType,
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitThrowStatement( AssemblyBuilderEmittingContext context, ILConstruct exception )
		{
			return
				ILConstruct.Instruction(
					"throw",
					TypeDefinition.VoidType,
					true,
					il =>
					{
						exception.LoadValue( il, false );
						il.EmitThrow();
					}
				);
		}

		protected override ILConstruct DeclareLocal( AssemblyBuilderEmittingContext context, TypeDefinition nestedType, string name )
		{
			return
				ILConstruct.Variable(
					nestedType,
					name
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct ReferArgument( AssemblyBuilderEmittingContext context, TypeDefinition type, string name, int index )
		{
			return ILConstruct.Argument( index, type.ResolveRuntimeType(), name );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitInvokeVoidMethod( AssemblyBuilderEmittingContext context, ILConstruct instance, MethodDefinition method, params ILConstruct[] arguments )
		{
			return
				method.ResolveRuntimeMethod().ReturnType == typeof( void )
					? ILConstruct.Invoke( instance, method, arguments )
					: ILConstruct.Sequence(
						TypeDefinition.VoidType,
						new[]
						{
							ILConstruct.Invoke( instance, method, arguments ),
							ILConstruct.Instruction( "pop", TypeDefinition.VoidType, false, il => il.EmitPop() )
						}
					);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitCreateNewObjectExpression( AssemblyBuilderEmittingContext context, ILConstruct variable, ConstructorDefinition constructor, params ILConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( constructor?.ResolveRuntimeConstructor() != null );
#endif // DEBUG
			return ILConstruct.NewObject( variable, constructor.ResolveRuntimeConstructor(), arguments );
		}

		protected override ILConstruct EmitMakeRef( AssemblyBuilderEmittingContext context, ILConstruct target )
		{
			return ILConstruct.MakeRef( target );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
		protected override ILConstruct EmitCreateNewArrayExpression( AssemblyBuilderEmittingContext context, TypeDefinition elementType, int length )
		{
			var array =
				ILConstruct.Variable(
					elementType.ResolveRuntimeType().MakeArrayType(),
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
									il.EmitNewarr( elementType.ResolveRuntimeType(), length );
									array.StoreValue( il );
								}
							)
						}
					),
					array
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
		protected override ILConstruct EmitCreateNewArrayExpression( AssemblyBuilderEmittingContext context, TypeDefinition elementType, int length, IEnumerable<ILConstruct> initialElements )
		{
			var array =
				ILConstruct.Variable(
					elementType.ResolveRuntimeType().MakeArrayType(),
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
									il.EmitNewarr( elementType.ResolveRuntimeType(), length );
									array.StoreValue( il );
									var index = 0;
									foreach ( var initialElement in initialElements )
									{
										array.LoadValue( il, false );
										this.MakeInt32Literal( context, index ).LoadValue( il, false );
										initialElement.LoadValue( il, false );
										il.EmitStelem( elementType.ResolveRuntimeType() );
										index++;
									}
								}
							)
						}
					),
					array
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetArrayElementExpression( AssemblyBuilderEmittingContext context, ILConstruct array, ILConstruct index )
		{
			return
				ILConstruct.Instruction(
					"SetArrayElement",
					array.ContextType,
					false,
					il =>
					{
						il.EmitAnyLdelem(
							array.ContextType.ResolveRuntimeType().GetElementType(),
							il0 => array.LoadValue( il0, false ),
							il0 => index.LoadValue( il0, false )
						);
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
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
							value.ContextType.ResolveRuntimeType(),
							il0 => array.LoadValue( il0, false ),
							il0 => index.LoadValue( il0, false ),
							il0 => value.LoadValue( il0, true )
						);
					}
				);
		}

		protected override ILConstruct EmitInvokeMethodExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, MethodDefinition method, IEnumerable<ILConstruct> arguments )
		{
			return ILConstruct.Invoke( instance, method, arguments );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitInvokeDelegateExpression( AssemblyBuilderEmittingContext context, TypeDefinition delegateReturnType, ILConstruct @delegate, params ILConstruct[] arguments )
		{
			return ILConstruct.Invoke( @delegate, @delegate.ContextType.ResolveRuntimeType().GetMethod( "Invoke" ), arguments );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetPropertyExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, PropertyInfo property )
		{
			return ILConstruct.Invoke( instance, property.GetGetMethod( true ), ILConstruct.NoArguments );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetFieldExpression( AssemblyBuilderEmittingContext context, ILConstruct instance, FieldDefinition field )
		{
			return ILConstruct.LoadField( instance, field.ResolveRuntimeField() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "Validated by caller in base class" )]
		protected override ILConstruct EmitSetIndexedProperty( AssemblyBuilderEmittingContext context, ILConstruct instance, TypeDefinition declaringType, string proeprtyName, ILConstruct key, ILConstruct value )
		{
#if DEBUG
			Contract.Assert( declaringType.HasRuntimeTypeFully() );
			Contract.Assert( key.ContextType.HasRuntimeTypeFully() );
#endif
			var indexer = declaringType.ResolveRuntimeType().GetProperty( proeprtyName, new[] { key.ContextType.ResolveRuntimeType() } );
			return ILConstruct.Invoke( instance, indexer.GetSetMethod( true ), new[] { key, value } );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitSetField( AssemblyBuilderEmittingContext context, ILConstruct instance, FieldDefinition field, ILConstruct value )
		{
			return ILConstruct.StoreField( instance, field.ResolveRuntimeField(), value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitSetField( AssemblyBuilderEmittingContext context, ILConstruct instance, TypeDefinition nestedType, string fieldName, ILConstruct value )
		{
			return ILConstruct.StoreField( instance, nestedType.ResolveRuntimeType().GetField( fieldName ), value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitLoadVariableExpression( AssemblyBuilderEmittingContext context, ILConstruct variable )
		{
			return ILConstruct.Instruction( "load", variable.ContextType, false, il => variable.LoadValue( il, false ) );
		}

		protected override ILConstruct EmitStoreVariableStatement( AssemblyBuilderEmittingContext context, ILConstruct variable, ILConstruct value )
		{
			return ILConstruct.StoreLocal( variable, value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
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

		protected override ILConstruct EmitForEachLoop( AssemblyBuilderEmittingContext context, CollectionTraits traits, ILConstruct collection, Func<ILConstruct, ILConstruct> loopBodyEmitter )
		{
			return
				ILConstruct.Instruction(
					"foreach",
					TypeDefinition.VoidType,
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
						if ( traits.GetEnumeratorMethod.ReturnType.GetIsValueType() )
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
						if ( traits.GetEnumeratorMethod.ReturnType.GetIsValueType() )
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

							if ( traits.GetEnumeratorMethod.ReturnType.GetIsValueType() )
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

		protected override Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructor( 
			AssemblyBuilderEmittingContext codeGenerationContext,
			SerializationTarget targetInfo,
			PolymorphismSchema schema,
			SerializerCapabilities? capabilities
		)
		{
			return context => codeGenerationContext.Emitter.CreateObjectInstance( codeGenerationContext, this, targetInfo, schema, capabilities );
		}

		protected override Func<SerializationContext, MessagePackSerializer> CreateEnumSerializerConstructor( AssemblyBuilderEmittingContext codeGenerationContext )
		{
			return context =>
				codeGenerationContext.Emitter.CreateEnumInstance(
					context,
					EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod( context, this.TargetType, EnumMemberSerializationMethod.Default )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetSerializerExpression( AssemblyBuilderEmittingContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			var realSchema = itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo );
			var field =
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

			return this.EmitGetFieldExpression( context, this.EmitThisReferenceExpression( context ), field );
		}

		private IEnumerable<ILConstruct> EmitConstructPolymorphismSchema(
			AssemblyBuilderEmittingContext context,
			PolymorphismSchema currentSchema 
		)
		{
			var schema = this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, "schema" );
			
			yield return schema;
			
			foreach ( var construct in this.EmitConstructPolymorphismSchema( context, schema, currentSchema ) )
			{
				yield return construct;
			}

			yield return this.EmitLoadVariableExpression( context, schema );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetActionsExpression( AssemblyBuilderEmittingContext context, ActionType actionType, bool isAsync )
		{
			Type type;
			string name;
			switch ( actionType )
			{
				case ActionType.PackToArray:
				{
					type = 
#if FEATURE_TAP
						isAsync ? typeof( IList<> ).MakeGenericType( typeof( Func<,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( CancellationToken ), typeof( Task ) ) ) :
#endif // FEATURE_TAP
						typeof( IList<> ).MakeGenericType( typeof( Action<,> ).MakeGenericType( typeof( Packer ), this.TargetType ) );
					name = FieldName.PackOperationList;
					break;
				}
				case ActionType.PackToMap:
				{
					type =
#if FEATURE_TAP
						isAsync ? typeof( IDictionary<,> ).MakeGenericType( typeof( string ), typeof( Func<,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( CancellationToken ), typeof( Task ) ) ) :
#endif // FEATURE_TAP
						typeof( IDictionary<,> ).MakeGenericType( typeof( string ), typeof( Action<,> ).MakeGenericType( typeof( Packer ), this.TargetType ) );
					name = FieldName.PackOperationTable;
					break;
				}
				case ActionType.IsNull:
				{
					type = typeof( IDictionary<,> ).MakeGenericType( typeof( string ), typeof( Func<,> ).MakeGenericType( this.TargetType, typeof( bool ) ) );
					name = FieldName.NullCheckersTable;
					break;
				}
				case ActionType.UnpackFromArray:
				{
					type =
						typeof( IList<> ).MakeGenericType(
#if FEATURE_TAP
							isAsync ? 
							typeof( Func<,,,,,> ).MakeGenericType(
								typeof( Unpacker ),
								context.UnpackingContextType == null
									? this.TargetType
									: context.UnpackingContextType.ResolveRuntimeType(),
								typeof( int ),
								typeof( int ),
								typeof( CancellationToken ),
								typeof( Task )
							) :
#endif // FEATURE_TAP
							typeof( Action<,,,> ).MakeGenericType(
								typeof( Unpacker ),
								context.UnpackingContextType == null
									? this.TargetType
									: context.UnpackingContextType.ResolveRuntimeType(),
								typeof( int ),
								typeof( int )
							)
						);
					name = FieldName.UnpackOperationList;
					break;
				}
				case ActionType.UnpackFromMap:
				{
					type =
						typeof( IDictionary<,> ).MakeGenericType(
							typeof( string ),
#if FEATURE_TAP
							isAsync ? 
							typeof( Func<,,,,,> ).MakeGenericType(
								typeof( Unpacker ),
								context.UnpackingContextType == null
									? this.TargetType
									: context.UnpackingContextType.ResolveRuntimeType(),
								typeof( int ),
								typeof( int ),
								typeof( CancellationToken ),
								typeof( Task )
							) :
#endif // FEATURE_TAP
							typeof( Action<,,,> ).MakeGenericType( 
								typeof( Unpacker ), 
								context.UnpackingContextType == null
									? this.TargetType
									: context.UnpackingContextType.ResolveRuntimeType(),
								typeof( int ),
								typeof( int )
							)
						);
					name = FieldName.UnpackOperationTable;
					break;
				}
				case ActionType.UnpackTo:
				{
					type = 
#if FEATURE_TAP
						isAsync ? typeof( Func<,,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( int ), typeof( CancellationToken ), typeof( Task ) ) :
#endif // FEATURE_TAP
						typeof( Action<,,> ).MakeGenericType( typeof( Unpacker ), this.TargetType, typeof( int ) );
					name = FieldName.UnpackTo;
					break;
				}
				default: // UnpackFromMap
				{
					throw new ArgumentOutOfRangeException( "actionType" );
				}
			}

			if ( isAsync )
			{
				name += "Async";
			}

			var field = context.DeclarePrivateField( name, type );

			return this.EmitGetFieldExpression( context, this.EmitThisReferenceExpression( context ), field );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override ILConstruct EmitGetMemberNamesExpression( AssemblyBuilderEmittingContext context )
		{
			var field = context.DeclarePrivateField( FieldName.MemberNames, TypeDefinition.IListOfStringType );

			return this.EmitGetFieldExpression( context, this.EmitThisReferenceExpression( context ), field );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override ILConstruct EmitFinishFieldInitializationStatement( AssemblyBuilderEmittingContext context, string name, ILConstruct value )
		{
			var field = context.DeclarePrivateField( name, value.ContextType.ResolveRuntimeType() );
			return this.EmitSetField( context, this.EmitThisReferenceExpression( context ), field, value );
		}

		protected override AssemblyBuilderEmittingContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			string serializerTypeName, serializerTypeNamespace;
			DefaultSerializerNameResolver.ResolveTypeName(
				true,
				this.TargetType,
				this.GetType().Namespace + ".Generated",
				out serializerTypeName,
				out serializerTypeNamespace 
			);
			var spec =
				new SerializerSpecification(
					this.TargetType,
					this.CollectionTraits,
					serializerTypeName,
					serializerTypeNamespace
				);

			return
				new AssemblyBuilderEmittingContext(
					context,
					this.TargetType,
					this.TargetType.GetIsEnum()
						? new Func<SerializerEmitter>( () => SerializationMethodGeneratorManager.Get().CreateEnumEmitter( context, spec ) ) 
						: () => SerializationMethodGeneratorManager.Get().CreateObjectEmitter( spec, this.BaseClass )
				);
		}

#if FEATURE_ASMGEN

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
					this.TargetType,
					this.CollectionTraits,
					this.BaseClass
				);

			if ( !this.TargetType.GetIsEnum() )
			{
				SerializationTarget targetInfo;
				this.BuildSerializer( emittingContext, concreteType, itemSchema, out targetInfo );
				// Finish type creation, and discard returned ctor.
				emittingContext.Emitter.CreateObjectConstructor( emittingContext, this, targetInfo, targetInfo.GetCapabilitiesForObject() );
			}
			else
			{
				this.BuildEnumSerializer( emittingContext );
				// Finish type creation, and discard returned ctor.
				emittingContext.Emitter.CreateEnumConstructor();
			}
		}

#endif // FEATURE_ASMGEN

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override ILConstruct EmitNewPrivateMethodDelegateExpression( AssemblyBuilderEmittingContext context, MethodDefinition method )
		{
			var delegateType = SerializerBuilderHelper.GetResolvedDelegateType( method.ReturnType, method.ParameterTypes );
				
			return
				ILConstruct.Instruction(
					"CreateDelegate",
					delegateType,
					false,
					il =>
					{
						if ( method.IsStatic )
						{
							il.EmitLdnull();
						}
						else
						{
							il.EmitLdargThis();
						}
						// OK this should not be ldvirtftn because target is private.
						il.EmitLdftn( method.ResolveRuntimeMethod() );
						// call extern .ctor(Object, void*)
						il.EmitNewobj( delegateType.GetConstructors().Single() );
					}
				);
		}
	}
}
