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
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class CodeDomSerializerBuilder<TObject> : SerializerBuilder<CodeDomContext, CodeDomConstruct, TObject>
	{
		public CodeDomSerializerBuilder() { }

		protected override void EmitMethodPrologue( CodeDomContext context, SerializerMethod method )
		{
			context.ResetMethodContext();
		}

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
							// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
							Attributes = MemberAttributes.Family | MemberAttributes.Override
							// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
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
							// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
							Attributes = MemberAttributes.Family | MemberAttributes.Override,
							// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
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
							// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
							Attributes = MemberAttributes.Family | MemberAttributes.Override
							// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
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
			codeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
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

		protected override CodeDomConstruct MakeStringLiteral( CodeDomContext context, string constant )
		{
			return CodeDomConstruct.Expression( typeof( string ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct EmitThisReferenceExpression( CodeDomContext context )
		{
			return CodeDomConstruct.Expression( typeof( MessagePackSerializer<> ).MakeGenericType( typeof( TObject ) ), new CodeThisReferenceExpression() );
		}

		protected override CodeDomConstruct EmitBoxExpression( CodeDomContext context, Type valueType, CodeDomConstruct value )
		{
			return CodeDomConstruct.Expression( typeof( object ), new CodeCastExpression( typeof( object ), value.AsExpression() ) );
		}

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

		protected override CodeDomConstruct EmitSequentialStatements( CodeDomContext context, Type contextType, IEnumerable<CodeDomConstruct> statements )
		{
#if DEBUG
			statements = statements.ToArray();
			Contract.Assert( statements.All( c => c.IsStatement ) );
#endif
			return CodeDomConstruct.Statement( statements.SelectMany( s => s.AsStatements() ) );
		}

		protected override CodeDomConstruct DeclareLocal( CodeDomContext context, Type type, string name )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeDomConstruct.Variable( type, context.GetUniqueVariableName( name ) );
		}

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

		protected override CodeDomConstruct EmitLoadVariableExpression( CodeDomContext context, CodeDomConstruct variable )
		{
			return CodeDomConstruct.Expression( variable.ContextType, variable.AsExpression() );
		}

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

		protected override CodeDomConstruct EmitThrowExpression( CodeDomContext context, Type expressionType, CodeDomConstruct exceptionExpression )
		{
#if DEBUG
			Contract.Assert( exceptionExpression.IsExpression );
#endif
			return CodeDomConstruct.Statement( new CodeThrowExceptionStatement( exceptionExpression.AsExpression() ) );
		}

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

		protected override CodeDomConstruct EmitGetSerializerExpression( CodeDomContext context, Type targetType )
		{
			return
				CodeDomConstruct.Expression(
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.GetSerializerFieldName( targetType )
					)
				);
		}

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
			this.Finish( asCodeDomContext );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( CodeDomContext codeGenerationContext )
		{
#if NETFX_35
			throw new NotSupportedException();
#else
			if ( !SerializerDebugging.OnTheFlyCodeDomEnabled )
			{
				throw new NotSupportedException();
			}

			this.Finish( codeGenerationContext );
			var cu = codeGenerationContext.CreateCodeCompileUnit();
			var codeProvider = CodeDomProvider.CreateProvider( "cs" );
			if ( SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.TraceEvent( "Compile {0}", codeGenerationContext.DeclaringType.Name );
				codeProvider.GenerateCodeFromCompileUnit( cu, SerializerDebugging.ILTraceWriter, new CodeGeneratorOptions() );
				SerializerDebugging.FlushTraceData();
			}

			var cr =
				codeProvider.CompileAssemblyFromDom(
					new CompilerParameters( SerializerDebugging.CodeDomSerializerDependentAssemblies.ToArray() ),
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
				// ReSharper disable ImplicitlyCapturedClosure
						t => t.Namespace == cu.Namespaces[ 0 ].Name
				// ReSharper restore ImplicitlyCapturedClosure
					).Select( t => t.FullName )
				)
			);

			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<TObject>>>(
					Expression.New( targetType.GetConstructors().Single(), contextParameter ),
					contextParameter
				).Compile();
#endif
		}

#if !NETFX_35
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
					)
				);
		}
#endif

		private void Finish( CodeDomContext context )
		{
			// fields
			foreach ( var dependentSerializer in context.GetDependentSerializers() )
			{
				context.DeclaringType.Members.Add(
					new CodeMemberField(
						dependentSerializer.Value,
						dependentSerializer.Key
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
				var contextArgument = new CodeArgumentReferenceExpression( "context" );
				var contextExpression =
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression( context.DeclaringType.Name ),
						CodeDomContext.ConditionalExpressionHelperMethodName,
						new CodeBinaryOperatorExpression(
							contextArgument,
							CodeBinaryOperatorType.IdentityInequality,
							new CodePrimitiveExpression( null )
						),
						contextArgument,
						new CodePropertyReferenceExpression(
							new CodeTypeReferenceExpression( typeof( SerializationContext ) ),
							"Default"
						)
					);
				ctor.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
				ctor.BaseConstructorArgs.Add(
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(
							contextExpression,
							"CompatibilityOptions"
						),
						"PackerCompatibilityOptions"
					)
				);
				ctor.Statements.Add(
					new CodeVariableDeclarationStatement( typeof( SerializationContext ), "safeContext", contextExpression )
				);
				var contextVariable = new CodeVariableReferenceExpression( "safeContext" );

				foreach ( var dependentSerializer in context.GetDependentSerializers() )
				{
					ctor.Statements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), dependentSerializer.Key ),
							new CodeMethodInvokeExpression(
								new CodeMethodReferenceExpression(
									contextVariable,
									"GetSerializer",
									new CodeTypeReference( dependentSerializer.Value.GetGenericArguments()[ 0 ] )
								)
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
						// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
						Attributes = MemberAttributes.Static | MemberAttributes.Private,
						// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
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