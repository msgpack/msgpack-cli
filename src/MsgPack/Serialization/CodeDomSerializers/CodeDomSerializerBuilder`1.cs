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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
	internal class CodeDomSerializerBuilder<TObject> : SerializerBuilder<CodeDomContext, CodeDomConstruct, TObject>
	{
		public CodeDomSerializerBuilder() { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodPrologue( CodeDomContext context, SerializerMethod method )
		{
			context.ResetMethodContext();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodPrologue( CodeDomContext context, EnumSerializerMethod method )
		{
			context.ResetMethodContext();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodEpilogue( CodeDomContext context, SerializerMethod method, CodeDomConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}
			CodeMemberMethod codeMethod;
			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "PackToCore",
						};
					codeMethod.Parameters.Add( context.Packer.AsParameter() );
					codeMethod.Parameters.Add( context.PackToTarget.AsParameter() );

					break;
				}
				case SerializerMethod.UnpackFromCore:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "UnpackFromCore",
							ReturnType = new CodeTypeReference( typeof( TObject ) )
						};
					codeMethod.Parameters.Add( context.Unpacker.AsParameter() );

					break;
				}
				case SerializerMethod.UnpackToCore:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "UnpackToCore",
						};
					codeMethod.Parameters.Add( context.Unpacker.AsParameter() );
					codeMethod.Parameters.Add( context.UnpackToTarget.AsParameter() );

					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method" );
				}
			}

			// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
			codeMethod.Attributes = ( context.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
			// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
			codeMethod.Statements.AddRange( construct.AsStatements().ToArray() );

			context.DeclaringType.Members.Add( codeMethod );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodEpilogue( CodeDomContext context, EnumSerializerMethod method, CodeDomConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}

			CodeMemberMethod codeMethod;
			switch ( method )
			{
				case EnumSerializerMethod.PackUnderlyingValueTo:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "PackUnderlyingValueTo",
						};
					codeMethod.Parameters.Add( context.Packer.AsParameter() );
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( TObject ), "enumValue" ) );

					break;
				}
				case EnumSerializerMethod.UnpackFromUnderlyingValue:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "UnpackFromUnderlyingValue",
							ReturnType = new CodeTypeReference( typeof( TObject ) )
						};
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( MessagePackObject ), "messagePackObject" ) );

					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method" );
				}
			}

			// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
			codeMethod.Attributes = ( context.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
			// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
			codeMethod.Statements.AddRange( construct.AsStatements().ToArray() );

			context.DeclaringType.Members.Add( codeMethod );
		}

		protected override CodeDomConstruct MakeNullLiteral( CodeDomContext context, Type contextType )
		{
			return CodeDomConstruct.Expression( contextType, new CodePrimitiveExpression( null ) );
		}

		protected override CodeDomConstruct MakeInt32Literal( CodeDomContext context, int constant )
		{
			return CodeDomConstruct.Expression( typeof( int ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt64Literal( CodeDomContext context, long constant )
		{
			return CodeDomConstruct.Expression( typeof( long ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeEnumLiteral( CodeDomContext context, Type type, object constant )
		{
			return CodeDomConstruct.Expression( type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeStringLiteral( CodeDomContext context, string constant )
		{
			return CodeDomConstruct.Expression( typeof( string ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct EmitThisReferenceExpression( CodeDomContext context )
		{
			return CodeDomConstruct.Expression( typeof( MessagePackSerializer<> ).MakeGenericType( typeof( TObject ) ), new CodeThisReferenceExpression() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitBoxExpression( CodeDomContext context, Type valueType, CodeDomConstruct value )
		{
			return CodeDomConstruct.Expression( typeof( object ), new CodeCastExpression( typeof( object ), value.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitUnboxAnyExpression( CodeDomContext context, Type targetType, CodeDomConstruct value )
		{
			return CodeDomConstruct.Expression( targetType, new CodeCastExpression( targetType, value.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitNotExpression( CodeDomContext context, CodeDomConstruct booleanExpression )
		{
			return
				CodeDomConstruct.Expression(
					typeof( bool ),
					new CodeBinaryOperatorExpression(
						booleanExpression.AsExpression(),
						CodeBinaryOperatorType.ValueEquality,
						new CodePrimitiveExpression( false )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitEqualsExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					typeof( bool ),
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.ValueEquality,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitGreaterThanExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					typeof( bool ),
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.GreaterThan,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitLessThanExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					typeof( bool ),
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.LessThan,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitIncrement( CodeDomContext context, CodeDomConstruct int32Value )
		{
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						int32Value.AsExpression(),
						new CodeBinaryOperatorExpression(
							int32Value.AsExpression(),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression( 1 )
						)
					)
				);
		}

		protected override CodeDomConstruct EmitTypeOfExpression( CodeDomContext context, Type type )
		{
			return CodeDomConstruct.Expression( typeof( Type ), new CodeTypeOfExpression( type ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitFieldOfExpression( CodeDomContext context, FieldInfo field )
		{
			return
				CodeDomConstruct.Expression(
					typeof( FieldInfo ),
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.GetCachedFieldInfoName(
							field
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitMethodOfExpression( CodeDomContext context, MethodBase method )
		{
			return
				CodeDomConstruct.Expression(
					typeof( MethodBase ),
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.GetCachedMethodBaseName(
							method
						)
					)
				);
		}

		protected override CodeDomConstruct EmitSequentialStatements( CodeDomContext context, Type contextType, IEnumerable<CodeDomConstruct> statements )
		{
#if DEBUG
			statements = statements.ToArray();
			Contract.Assert( statements.All( c => c.IsStatement ) );
#endif
			return CodeDomConstruct.Statement( statements.SelectMany( s => s.AsStatements() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeDomConstruct DeclareLocal( CodeDomContext context, Type type, string name )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeDomConstruct.Variable( type, context.GetUniqueVariableName( name ) );
		}

		protected override CodeDomConstruct ReferArgument( CodeDomContext context, Type type, string name, int index )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeDomConstruct.Parameter( type, name );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitCreateNewObjectExpression( CodeDomContext context, CodeDomConstruct variable, ConstructorInfo constructor, params CodeDomConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( constructor.DeclaringType != null );
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeDomConstruct.Expression(
					constructor.DeclaringType,
					new CodeObjectCreateExpression( constructor.DeclaringType, arguments.Select( a => a.AsExpression() ).ToArray() )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitInvokeVoidMethod( CodeDomContext context, CodeDomConstruct instance, MethodInfo method, params CodeDomConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
			Contract.Assert( method.DeclaringType != null );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeExpressionStatement(
						new CodeMethodInvokeExpression(
							instance == null
							? new CodeTypeReferenceExpression( method.DeclaringType )
							: instance.AsExpression(),
							method.Name,
							arguments.Select( a => a.AsExpression() ).ToArray()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitInvokeMethodExpression( CodeDomContext context, CodeDomConstruct instance, MethodInfo method, IEnumerable<CodeDomConstruct> arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
			Contract.Assert( method.DeclaringType != null );
#endif
			return
				CodeDomConstruct.Expression(
					method.ReturnType,
					new CodeMethodInvokeExpression(
						instance == null
						? new CodeTypeReferenceExpression( method.DeclaringType )
						: instance.AsExpression(),
						method.Name,
						arguments.Select( a => a.AsExpression() ).ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitGetPropretyExpression( CodeDomContext context, CodeDomConstruct instance, PropertyInfo property )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( property.DeclaringType != null );
#endif
			return
				CodeDomConstruct.Expression(
					property.PropertyType,
					new CodePropertyReferenceExpression(
						instance == null
						? new CodeTypeReferenceExpression( property.DeclaringType )
						: instance.AsExpression(),
						property.Name
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitGetFieldExpression( CodeDomContext context, CodeDomConstruct instance, FieldInfo field )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( field.DeclaringType != null );
#endif
			return
				CodeDomConstruct.Expression(
					field.FieldType,
					new CodeFieldReferenceExpression(
						instance == null
						? new CodeTypeReferenceExpression( field.DeclaringType )
						: instance.AsExpression(),
						field.Name
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitSetProprety( CodeDomContext context, CodeDomConstruct instance, PropertyInfo property, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( property.DeclaringType != null );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							instance == null
							? new CodeTypeReferenceExpression( property.DeclaringType )
							: instance.AsExpression(),
							property.Name
						),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitSetField( CodeDomContext context, CodeDomConstruct instance, FieldInfo field, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( field.DeclaringType != null );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(
							instance == null
							? new CodeTypeReferenceExpression( field.DeclaringType )
							: instance.AsExpression(),
							field.Name
						),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitLoadVariableExpression( CodeDomContext context, CodeDomConstruct variable )
		{
			return CodeDomConstruct.Expression( variable.ContextType, variable.AsExpression() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitStoreVariableStatement( CodeDomContext context, CodeDomConstruct variable, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( variable.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						variable.AsExpression(),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitThrowExpression( CodeDomContext context, Type expressionType, CodeDomConstruct exceptionExpression )
		{
#if DEBUG
			Contract.Assert( exceptionExpression.IsExpression );
#endif
			return CodeDomConstruct.Statement( new CodeThrowExceptionStatement( exceptionExpression.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitTryFinally( CodeDomContext context, CodeDomConstruct tryStatement, CodeDomConstruct finallyStatement )
		{
#if DEBUG
			Contract.Assert( tryStatement.IsStatement );
			Contract.Assert( finallyStatement.IsStatement );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeTryCatchFinallyStatement(
						tryStatement.AsStatements().ToArray(),
						CodeDomContext.EmptyCatches,
						finallyStatement.AsStatements().ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitCreateNewArrayExpression( CodeDomContext context, Type elementType, int length, IEnumerable<CodeDomConstruct> initialElements )
		{
#if DEBUG
			initialElements = initialElements.ToArray();
			Contract.Assert( initialElements.All( i => i.IsExpression ) );
#endif
			return
				CodeDomConstruct.Expression(
					elementType.MakeArrayType(),
					new CodeArrayCreateExpression(
						elementType,
						initialElements.Select( i => i.AsExpression() ).ToArray()
					)
					{
						Size = length
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitGetSerializerExpression( CodeDomContext context, Type targetType, SerializingMember? memberInfo )
		{
			return
				CodeDomConstruct.Expression(
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.GetSerializerFieldName(
							targetType,
							memberInfo == null
							? EnumMemberSerializationMethod.Default
							: memberInfo.Value.GetEnumMemberSerializationMethod()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitConditionalExpression( CodeDomContext context, CodeDomConstruct conditionExpression, CodeDomConstruct thenExpression, CodeDomConstruct elseExpression )
		{
#if DEBUG
			Contract.Assert(
				elseExpression == null ||
				thenExpression.ContextType == typeof( void ) ||
				( thenExpression.ContextType == elseExpression.ContextType ),
				thenExpression.ContextType + " != " + ( elseExpression == null ? "(null)" : elseExpression.ContextType.FullName )
			);
			Contract.Assert( conditionExpression.IsExpression );
			Contract.Assert(
				elseExpression == null ||
				thenExpression.ContextType == typeof( void ) ||
				( thenExpression.ContextType == elseExpression.ContextType &&
				  thenExpression.IsExpression == elseExpression.IsExpression )
			);

#endif

			return
				elseExpression == null
				? CodeDomConstruct.Statement(
					new CodeConditionStatement(
						conditionExpression.AsExpression(),
						thenExpression.AsStatements().ToArray()
					)
				)
				: thenExpression.ContextType == typeof( void ) || thenExpression.IsStatement
				? CodeDomConstruct.Statement(
					new CodeConditionStatement(
						conditionExpression.AsExpression(),
						thenExpression.AsStatements().ToArray(),
						elseExpression.AsStatements().ToArray()
					)
				) as CodeDomConstruct
				: CodeDomConstruct.Expression(
					thenExpression.ContextType,
					new CodeMethodInvokeExpression(
						null,
						CodeDomContext.ConditionalExpressionHelperMethodName,
						conditionExpression.AsExpression(),
						thenExpression.AsExpression(),
						elseExpression.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitAndConditionalExpression( CodeDomContext context, IList<CodeDomConstruct> conditionExpressions, CodeDomConstruct thenExpression, CodeDomConstruct elseExpression )
		{
#if DEBUG
			Contract.Assert( conditionExpressions.All( c => c.IsExpression ) );
			Contract.Assert( thenExpression.IsStatement );
			Contract.Assert( elseExpression.IsStatement );
#endif

			return
				CodeDomConstruct.Statement(
					new CodeConditionStatement(
						conditionExpressions
						.Select( c => c.AsExpression() )
						.Aggregate( ( l, r ) =>
							new CodeBinaryOperatorExpression(
								l,
								CodeBinaryOperatorType.BooleanAnd,
								r
							)
						),
						thenExpression.AsStatements().ToArray(),
						elseExpression.AsStatements().ToArray()
					)
				);
		}

		protected override CodeDomConstruct EmitStringSwitchStatement( CodeDomContext context, CodeDomConstruct target, IDictionary<string, CodeDomConstruct> cases )
		{
#if DEBUG
			Contract.Assert( target.IsExpression );
			Contract.Assert( cases.Values.All( c => c.IsStatement ) );
#endif
			return
				CodeDomConstruct.Statement(
					cases.Aggregate<KeyValuePair<string, CodeDomConstruct>, CodeConditionStatement>(
						null,
						( current, caseStatement ) =>
						new CodeConditionStatement(
							new CodeBinaryOperatorExpression(
								target.AsExpression(),
								CodeBinaryOperatorType.ValueEquality,
								new CodePrimitiveExpression( caseStatement.Key )
							),
							caseStatement.Value.AsStatements().ToArray(),
							current == null ? new CodeStatement[ 0 ] : new CodeStatement[] { current }
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitRetrunStatement( CodeDomContext context, CodeDomConstruct expression )
		{
#if DEBUG
			Contract.Assert( expression.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeMethodReturnStatement( expression.AsExpression() )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitForLoop( CodeDomContext context, CodeDomConstruct count, Func<ForLoopContext, CodeDomConstruct> loopBodyEmitter )
		{
#if DEBUG
			Contract.Assert( count.IsExpression );
#endif
			var counterName = context.GetUniqueVariableName( "i" );
			var counter = CodeDomConstruct.Variable( typeof( int ), counterName );
			var loopContext = new ForLoopContext( counter );

			return
				CodeDomConstruct.Statement(
					new CodeIterationStatement(
						new CodeVariableDeclarationStatement(
							typeof( int ), counterName, new CodePrimitiveExpression( 0 )
						),
						new CodeBinaryOperatorExpression(
							counter.AsExpression(),
							CodeBinaryOperatorType.LessThan,
							count.AsExpression()
						),
						new CodeAssignStatement(
							counter.AsExpression(),
							new CodeBinaryOperatorExpression(
								counter.AsExpression(),
								CodeBinaryOperatorType.Add,
								new CodePrimitiveExpression( 1 )
							)
						),
						loopBodyEmitter( loopContext ).AsStatements().ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitForEachLoop( CodeDomContext context, CollectionTraits collectionTraits, CodeDomConstruct collection, Func<CodeDomConstruct, CodeDomConstruct> loopBodyEmitter )
		{
#if DEBUG
			Contract.Assert( collection.IsExpression );
#endif
			var enumeratorName = context.GetUniqueVariableName( "enumerator" );
			var currentName = context.GetUniqueVariableName( "current" );
			bool isDisposable = typeof( IDisposable ).IsAssignableFrom( collectionTraits.GetEnumeratorMethod.ReturnType );
			var enumerator =
				CodeDomConstruct.Variable(
					collectionTraits.GetEnumeratorMethod.ReturnType, enumeratorName
				);
			var current = CodeDomConstruct.Variable( collectionTraits.ElementType, currentName );

			var loopBody =
				CodeDomConstruct.Statement(
					new CodeIterationStatement(
						new CodeSnippetStatement( String.Empty ),
						new CodeMethodInvokeExpression(
							enumerator.AsExpression(),
							"MoveNext"
						),
						new CodeSnippetStatement( String.Empty ),
						new CodeStatement[]
						{
							new CodeAssignStatement(
								current.AsExpression(),
								new CodePropertyReferenceExpression(
									enumerator.AsExpression(), 
									collectionTraits.GetEnumeratorMethod.ReturnType == typeof( IDictionaryEnumerator )
									? "Entry"
									: "Current" 
								)
							)
						}.Concat( loopBodyEmitter( current ).AsStatements() ).ToArray()
					)
				);

			var statements =
				new List<CodeDomConstruct>
				{
					CodeDomConstruct.Statement(
						new CodeVariableDeclarationStatement(
							enumerator.ContextType,
							enumeratorName,
							new CodeMethodInvokeExpression(
								collection.AsExpression(),
								"GetEnumerator"
							)
						)
					),
					CodeDomConstruct.Statement( new CodeVariableDeclarationStatement( current.ContextType, currentName ) )
				};

			if ( isDisposable )
			{
				statements.Add(
					CodeDomConstruct.Statement(
						new CodeTryCatchFinallyStatement(
							loopBody.AsStatements().ToArray(),
							CodeDomContext.EmptyCatches,
							new CodeStatement[]
							{
								new CodeExpressionStatement(
									new CodeMethodInvokeExpression(
										enumerator.AsExpression(),
										"Dispose"
									)
								)
							}
						)
					)
				);
			}
			else
			{
				statements.Add( loopBody );
			}

			return
				EmitSequentialStatements(
					context,
					typeof( void ),
					statements.ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitEnumFromUnderlyingCastExpression(
			CodeDomContext context,
			Type enumType,
			CodeDomConstruct underlyingValue )
		{
			return CodeDomConstruct.Expression( enumType, new CodeCastExpression( enumType, underlyingValue.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitEnumToUnderlyingCastExpression(
			CodeDomContext context,
			Type underlyingType,
			CodeDomConstruct enumValue )
		{
			return CodeDomConstruct.Expression( underlyingType, new CodeCastExpression( underlyingType, enumValue.AsExpression() ) );
		}

		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context )
		{
			var asCodeDomContext = context as CodeDomContext;
			if ( asCodeDomContext == null )
			{
				throw new ArgumentException(
					"'context' was not created with CreateGenerationContextForCodeGeneration method.",
					"context"
				);
			}

			asCodeDomContext.Reset( typeof( TObject ) );

			this.BuildSerializer( asCodeDomContext );
			Finish( asCodeDomContext, typeof( TObject ).GetIsEnum() );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( CodeDomContext codeGenerationContext )
		{
			Finish( codeGenerationContext, false );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );

			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<TObject>>>(
					Expression.New( targetType.GetConstructors().Single(), contextParameter ),
					contextParameter
				).Compile();
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor( CodeDomContext codeGenerationContext )
		{
			Finish( codeGenerationContext, true );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );

			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<TObject>>>(
					Expression.New( targetType.GetConstructors().Single(), contextParameter ),
					contextParameter
				).Compile();
		}

#if !NETFX_35
		[SecuritySafeCritical]
#endif // !NETFX_35
		private static Type PrepareSerializerConstructorCreation( CodeDomContext codeGenerationContext )
		{
			if ( !SerializerDebugging.OnTheFlyCodeDomEnabled )
			{
				throw new NotSupportedException();
			}

			var cu = codeGenerationContext.CreateCodeCompileUnit();
			CompilerResults cr;
			using ( var codeProvider = CodeDomProvider.CreateProvider( "cs" ) )
			{
				if ( SerializerDebugging.DumpEnabled )
				{
					SerializerDebugging.TraceEvent( "Compile {0}", codeGenerationContext.DeclaringType.Name );
					codeProvider.GenerateCodeFromCompileUnit( cu, SerializerDebugging.ILTraceWriter, new CodeGeneratorOptions() );
					SerializerDebugging.FlushTraceData();
				}

				cr =
					codeProvider.CompileAssemblyFromDom(
						new CompilerParameters( SerializerDebugging.CodeDomSerializerDependentAssemblies.ToArray() )
#if PERFORMANCE_TEST
					{
						IncludeDebugInformation = false,
						CompilerOptions = "/optimize+"
					}
#endif
						,
						cu
						);
				var errors = cr.Errors.OfType<CompilerError>().Where( e => !e.IsWarning ).ToArray();
				if ( errors.Length > 0 )
				{
					if ( SerializerDebugging.TraceEnabled )
					{
						codeProvider.GenerateCodeFromCompileUnit( cu, SerializerDebugging.ILTraceWriter, new CodeGeneratorOptions() );
						SerializerDebugging.FlushTraceData();
					}

					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Failed to compile assembly. Details:{0}{1}",
							Environment.NewLine,
							BuildCompilationError( cr )
							)
						);
				}
			}
#if DEBUG
			// Check warning except ambigious type reference.
			var warnings = cr.Errors.OfType<CompilerError>().Where( e => e.ErrorNumber != "CS0436" ).ToArray();
			Contract.Assert( !warnings.Any(), BuildCompilationError( cr ) );
#endif

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "Build assembly '{0}' from dom.", cr.PathToAssembly );
			}

			SerializerDebugging.AddCompiledCodeDomAssembly( cr.PathToAssembly );

			var targetType =
				cr.CompiledAssembly.GetTypes()
					.SingleOrDefault(
						t =>
							t.Namespace == cu.Namespaces[ 0 ].Name
							&& t.Name == codeGenerationContext.DeclaringType.Name
					);

			Contract.Assert(
				targetType != null,
				String.Join(
					Environment.NewLine,
					cr.CompiledAssembly.GetTypes()
						.Where(
				// ReSharper disable once ImplicitlyCapturedClosure
							t => t.Namespace == cu.Namespaces[ 0 ].Name
						).Select( t => t.FullName ).ToArray()
				)
			);
			return targetType;

		}

		private static string BuildCompilationError( CompilerResults cr )
		{
			return
				String.Join(
					Environment.NewLine,
					cr.Errors.OfType<CompilerError>()
					  .Select(
						( error, i ) =>
							String.Format(
								CultureInfo.InvariantCulture,
								"[{0}]{1}:{2}:(File:{3}, Line:{4}, Column:{5}):{6}",
								i,
								error.IsWarning ? "Warning" : "Error   ",
								error.ErrorNumber,
								error.FileName,
								error.Line,
								error.Column,
								error.ErrorText
							)
					).ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
		private static void Finish( CodeDomContext context, bool isEnum )
		{
			// fields
			foreach ( var dependentSerializer in context.GetDependentSerializers() )
			{
				var targetType = Type.GetTypeFromHandle( dependentSerializer.Key.TypeHandle );

				context.DeclaringType.Members.Add(
					new CodeMemberField(
						typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
						dependentSerializer.Value
					)
				);
			}

			foreach ( var cachedFieldInfo in context.GetCachedFieldInfos() )
			{
				context.DeclaringType.Members.Add(
					new CodeMemberField(
						typeof( FieldInfo ),
						cachedFieldInfo.Value
					)
				);
			}


			foreach ( var cachedMethodBase in context.GetCachedMethodBases() )
			{
				context.DeclaringType.Members.Add(
					new CodeMemberField(
						typeof( MethodBase ),
						cachedMethodBase.Value
					)
				);
			}


			// ctor
			{
				var ctor =
					new CodeConstructor
					{
						Attributes = MemberAttributes.Public
					};

				ctor.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
				var contextArgument = new CodeArgumentReferenceExpression( "context" );
				ctor.BaseConstructorArgs.Add( contextArgument );

				if ( isEnum )
				{
					ctor.BaseConstructorArgs.Add(
						new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression( typeof( EnumSerializationMethod ) ),
							EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
								context.SerializationContext,
								typeof( TObject ),
								EnumMemberSerializationMethod.Default
							).ToString()
						)
					);
				}

				foreach ( var dependentSerializer in context.GetDependentSerializers() )
				{
					var targetType = Type.GetTypeFromHandle( dependentSerializer.Key.TypeHandle );

					if ( !targetType.GetIsEnum() )
					{
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), dependentSerializer.Value ),
								new CodeMethodInvokeExpression(
									new CodeMethodReferenceExpression(
										contextArgument,
										"GetSerializer",
										new CodeTypeReference( targetType )
									)
								)
							)
						);
					}
					else
					{
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), dependentSerializer.Value ),
								new CodeMethodInvokeExpression(
									new CodeMethodReferenceExpression(
										contextArgument,
										"GetSerializer",
										new CodeTypeReference( targetType )
									),
									new CodeMethodInvokeExpression(
										new CodeTypeReferenceExpression( typeof( EnumMessagePackSerializerHelpers ) ),
										"DetermineEnumSerializationMethod",
										contextArgument,
										new CodeTypeOfExpression( @targetType ),
										new CodeFieldReferenceExpression(
											new CodeTypeReferenceExpression( typeof( EnumMemberSerializationMethod ) ),
											dependentSerializer.Key.EnumSerializationMethod.ToString()
										)
									)
								)
							)
						);
					}
				}

				foreach ( var cachedFieldInfo in context.GetCachedFieldInfos() )
				{
					var fieldInfo = FieldInfo.GetFieldFromHandle( cachedFieldInfo.Key );
					ctor.Statements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedFieldInfo.Value ),
							new CodeMethodInvokeExpression(
								new CodeMethodReferenceExpression(
									new CodeTypeOfExpression( fieldInfo.DeclaringType ),
									"GetField"
								),
								new CodePrimitiveExpression( fieldInfo.Name ),
								new CodeBinaryOperatorExpression(
									new CodeFieldReferenceExpression(
										new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
										"Instance"
									),
									CodeBinaryOperatorType.BitwiseOr,
									new CodeBinaryOperatorExpression(
										new CodeFieldReferenceExpression(
											new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
											"Public"
										),
										CodeBinaryOperatorType.BitwiseOr,
										new CodeFieldReferenceExpression(
											new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
											"NonPublic"
										)
									)
								)
							)
						)
					);
				}


				foreach ( var cachedMethodBase in context.GetCachedMethodBases() )
				{
					var methodBase = MethodBase.GetMethodFromHandle( cachedMethodBase.Key );
					ctor.Statements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedMethodBase.Value ),
							new CodeMethodInvokeExpression(
								new CodeMethodReferenceExpression(
									new CodeTypeOfExpression( methodBase.DeclaringType ),
									"GetMethod"
								),
								new CodePrimitiveExpression( methodBase.Name ),
								new CodeBinaryOperatorExpression(
									new CodeFieldReferenceExpression(
										new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
										"Instance"
									),
									CodeBinaryOperatorType.BitwiseOr,
									new CodeBinaryOperatorExpression(
										new CodeFieldReferenceExpression(
											new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
											"Public"
										),
										CodeBinaryOperatorType.BitwiseOr,
										new CodeFieldReferenceExpression(
											new CodeTypeReferenceExpression( typeof( BindingFlags ) ),
											"NonPublic"
										)
									)
								),
								new CodePrimitiveExpression( null ),
								new CodeArrayCreateExpression(
									typeof( Type ),
									methodBase.GetParameters().Select( pi => new CodeTypeOfExpression( pi.ParameterType ) ).ToArray()
								),
								new CodePrimitiveExpression( null )
							)
						)
					);
				}
				context.DeclaringType.Members.Add( ctor );
			}

			// __Condition
			{
				var t = new CodeTypeParameter( "T" );
				var tRef = new CodeTypeReference( t );
				var conditional =
					new CodeMemberMethod
					{
						Name = CodeDomContext.ConditionalExpressionHelperMethodName,
						// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
						Attributes = MemberAttributes.Static | MemberAttributes.Private,
						ReturnType = tRef
					};
				conditional.TypeParameters.Add( t );
				conditional.Parameters.Add( new CodeParameterDeclarationExpression( typeof( bool ), CodeDomContext.ConditionalExpressionHelperConditionParameterName ) );
				conditional.Parameters.Add( new CodeParameterDeclarationExpression( tRef, CodeDomContext.ConditionalExpressionHelperWhenTrueParameterName ) );
				conditional.Parameters.Add( new CodeParameterDeclarationExpression( tRef, CodeDomContext.ConditionalExpressionHelperWhenFalseParameterName ) );
				conditional.Statements.Add(
					new CodeConditionStatement(
						new CodeArgumentReferenceExpression( CodeDomContext.ConditionalExpressionHelperConditionParameterName ),
						new CodeStatement[] { new CodeMethodReturnStatement( new CodeArgumentReferenceExpression( CodeDomContext.ConditionalExpressionHelperWhenTrueParameterName ) ) },
						new CodeStatement[] { new CodeMethodReturnStatement( new CodeArgumentReferenceExpression( CodeDomContext.ConditionalExpressionHelperWhenFalseParameterName ) ) }
					)
				);

				context.DeclaringType.Members.Add( conditional );
			}
		}

		protected override CodeDomContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			var result = new CodeDomContext( context, new SerializerCodeGenerationConfiguration() );
			result.Reset( typeof( TObject ) );
			return result;
		}
	}
}