#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.Syntax.SyntaxFactory;
#endif

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.CollectionSerializers;
using System.Diagnostics;

using static MsgPack.Serialization.CodeTreeSerializers.Syntax;

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal sealed class CodeTreeSerializerBuilder :
		SerializerBuilder<CodeTreeContext, CodeTreeConstruct>,
		ISerializerCodeGenerator
	{
#if CSHARP
		private const string Extension = "cs";
#elif VISUAL_BASIC
		private const string Extension = "vb";
#else
#endif

		private static readonly int ProcessId = GetCurrentProcessId();

		private static int GetCurrentProcessId()
		{
			using ( var process = Process.GetCurrentProcess() )
			{
				return process.Id;
			}
		}

		private static readonly ParameterSyntax ContextParameterSyntax =
			Parameter( Identifier( "context" ) )
				.WithType( SerializationContextTypeSyntax );

		private static readonly ParameterListSyntax ContextParameterListSyntax =
			ParameterList(
				new SeparatedSyntaxList<ParameterSyntax>().Add(
					ContextParameterSyntax
				)
			);

		private static readonly SimpleNameSyntax ContextArgumentReferenceSyntax = IdentifierName( "context" );

		private static readonly ArgumentSyntax ContextArgumentSyntax = Argument( ContextArgumentReferenceSyntax );

		private static readonly SeparatedSyntaxList<ArgumentSyntax> ContextArgumentListTemplate =
			new SeparatedSyntaxList<ArgumentSyntax>().Add( ContextArgumentSyntax );

		private static readonly ParameterListSyntax ContextAndEnumSerializationMethodParameterListSyntax =
			ContextParameterListSyntax.AddParameters(
				Parameter( Identifier( "enumSerializationMethod" ) )
					.WithType( ToTypeSyntax( typeof( EnumSerializationMethod ) ) )
			);

		private static readonly ConstructorInitializerSyntax ContextAndEnumSerializationMethodConstructorInitializerSyntax =
			ConstructorInitializer(
				SyntaxKind.BaseConstructorInitializer,
				ArgumentList(
					// ReSharper disable once ImpureMethodCallOnReadonlyValueField
					ContextArgumentListTemplate.Add(
						Argument(
							IdentifierName( "enumSerializationMethod" )
						)
					)
				)
			);

		private static readonly SimpleNameSyntax EnumSerializationMethodTypeIdentifierSyntax = IdentifierName( typeof( EnumSerializationMethod ).FullName );

		private static readonly SimpleNameSyntax RestoreSchemaMethodIdentifierSyntax = IdentifierName( MethodName.RestoreSchema );

		private static readonly SimpleNameSyntax SerializerCapabilitiesTypeIdentifierSyntax = IdentifierName( typeof( SerializerCapabilities ).FullName );

		private static readonly ExpressionSyntax SerializerCapabilitiesNoneSyntax =
			MemberAccessExpression(
				SyntaxKind.SimpleMemberAccessExpression,
				SerializerCapabilitiesTypeIdentifierSyntax,
				IdentifierName( "None" )
			);

		private static readonly InvocationExpressionSyntax EnumMessagePackSerializerHelpersDetermineEnumSerializationMethodMethodTemplate =
			InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ToTypeSyntax( typeof( EnumMessagePackSerializerHelpers ) ),
					IdentifierName( nameof( EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod ) )
				)
			);

		private static readonly InvocationExpressionSyntax DateTimeMessagePackSerializerHelpersDetermineDateTimeConversionMethodTemplate =
			InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ToTypeSyntax( typeof( DateTimeMessagePackSerializerHelpers ) ),
					IdentifierName( nameof( DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethod ) )
				)
			);

		private static readonly SimpleNameSyntax DateTimeMemberConversionMethodIdentifierSyntax = IdentifierName( typeof( DateTimeMemberConversionMethod ).FullName );

		private static readonly InvocationExpressionSyntax ReflectionHelpersGetFieldMethodTemplate =
			InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ToTypeSyntax( typeof( ReflectionHelpers ) ),
					IdentifierName( nameof( ReflectionHelpers.GetField ) )
				)
			);

		private static readonly InvocationExpressionSyntax ReflectionHelpersGetMethodMethodTemplate =
			InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ToTypeSyntax( typeof( ReflectionHelpers ) ),
					IdentifierName( nameof( ReflectionHelpers.GetMethod ) )
				)
			);

		private static readonly ArrayCreationExpressionSyntax TypeArrayCreationTemplate =
			ArrayCreationExpression( ArrayType( ToTypeSyntax( TypeDefinition.TypeType ) ) );

		private static readonly SyntaxList<CatchClauseSyntax> EmptyCatches = new SyntaxList<CatchClauseSyntax>();


		private readonly TypeDefinition _thisType;

		public CodeTreeSerializerBuilder( Type targetType, CollectionTraits collectionTraits )
			: base( targetType, collectionTraits )
		{
			this._thisType = typeof( MessagePackSerializer<> ).MakeGenericType( this.TargetType );
		}

		protected override CodeTreeConstruct MakeNullLiteral( CodeTreeContext context, TypeDefinition contextType )
			=> CodeTreeConstruct.Expression( contextType, NullLiteralSyntax );

		private static CodeTreeConstruct MakeTinyNumberLiteral( TypeDefinition contextType, int value )
			=> CodeTreeConstruct.Expression(
				contextType,
				CastExpression(
					ToTypeSyntax( contextType ),
					LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( value ) )
				)
			);

		protected override CodeTreeConstruct MakeByteLiteral( CodeTreeContext context, byte constant )
		 => MakeTinyNumberLiteral( TypeDefinition.ByteType, constant );

		protected override CodeTreeConstruct MakeSByteLiteral( CodeTreeContext context, sbyte constant )
			=> MakeTinyNumberLiteral( TypeDefinition.SByteType, constant );

		protected override CodeTreeConstruct MakeInt16Literal( CodeTreeContext context, short constant )
			=> MakeTinyNumberLiteral( TypeDefinition.Int16Type, constant );

		protected override CodeTreeConstruct MakeUInt16Literal( CodeTreeContext context, ushort constant )
			=> MakeTinyNumberLiteral( TypeDefinition.UInt16Type, constant );

		protected override CodeTreeConstruct MakeInt32Literal( CodeTreeContext context, int constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.Int32Type, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeUInt32Literal( CodeTreeContext context, uint constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.UInt32Type, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeInt64Literal( CodeTreeContext context, long constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.Int64Type, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeUInt64Literal( CodeTreeContext context, ulong constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.UInt64Type, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeReal32Literal( CodeTreeContext context, float constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.SingleType, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeReal64Literal( CodeTreeContext context, double constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.DoubleType, LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeBooleanLiteral( CodeTreeContext context, bool constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.BooleanType, LiteralExpression( constant ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression ) );

		protected override CodeTreeConstruct MakeCharLiteral( CodeTreeContext context, char constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.CharType, LiteralExpression( SyntaxKind.CharacterLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeStringLiteral( CodeTreeContext context, string constant )
			=> CodeTreeConstruct.Expression( TypeDefinition.StringType, LiteralExpression( SyntaxKind.StringLiteralExpression, Literal( constant ) ) );

		protected override CodeTreeConstruct MakeEnumLiteral( CodeTreeContext context, TypeDefinition type, object constant )
		{
			var asString = constant.ToString();
			if ( ( '0' <= asString[ 0 ] && asString[ 0 ] <= '9' ) || asString.Contains( ',' ) )
			{
				// Unrepresentable numeric or combined flags
				return
					CodeTreeConstruct.Expression(
						type,
						CastExpression(
							ToTypeSyntax( type ),
							// Only support integrals.
							LiteralExpression(
								SyntaxKind.NumericLiteralExpression,
								Literal( UInt64.Parse( ( ( Enum )constant ).ToString( "D" ), CultureInfo.InvariantCulture ) )
							)
						)
					);
			}
			else
			{
				return
					CodeTreeConstruct.Expression(
						type,
						MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName( type.TypeName ),
							Token( SyntaxKind.DotToken ),
							IdentifierName( asString )
						)
					);
			}
		}

		protected override CodeTreeConstruct MakeDefaultLiteral( CodeTreeContext context, TypeDefinition type )
			=> CodeTreeConstruct.Expression( type, DefaultExpression( ToTypeSyntax( type ) ) );

		protected override CodeTreeConstruct EmitThisReferenceExpression( CodeTreeContext context )
			=> CodeTreeConstruct.Expression( this._thisType, ThisExpression() );

		protected override CodeTreeConstruct EmitBoxExpression( CodeTreeContext context, TypeDefinition valueType, CodeTreeConstruct value )
			=> CodeTreeConstruct.Expression( TypeDefinition.ObjectType, CastExpression( ObjectTypeSyntax, value.AsExpression() ) );

		protected override CodeTreeConstruct EmitUnboxAnyExpression( CodeTreeContext context, TypeDefinition targetType, CodeTreeConstruct value )
			=> CodeTreeConstruct.Expression( targetType, CastExpression( ToTypeSyntax( targetType ), value.AsExpression() ) );

		protected override CodeTreeConstruct EmitNotExpression( CodeTreeContext context, CodeTreeConstruct booleanExpression )
			=> CodeTreeConstruct.Expression( TypeDefinition.BooleanType, PrefixUnaryExpression( SyntaxKind.LogicalNotExpression, booleanExpression.AsExpression() ) );

		protected override CodeTreeConstruct EmitEqualsExpression( CodeTreeContext context, CodeTreeConstruct left, CodeTreeConstruct right )
			=> CodeTreeConstruct.Expression( TypeDefinition.BooleanType, BinaryExpression( SyntaxKind.EqualsEqualsToken, left.AsExpression(), right.AsExpression() ) );

		protected override CodeTreeConstruct EmitGreaterThanExpression( CodeTreeContext context, CodeTreeConstruct left, CodeTreeConstruct right )
			=> CodeTreeConstruct.Expression( TypeDefinition.BooleanType, BinaryExpression( SyntaxKind.GreaterThanToken, left.AsExpression(), right.AsExpression() ) );

		protected override CodeTreeConstruct EmitLessThanExpression( CodeTreeContext context, CodeTreeConstruct left, CodeTreeConstruct right )
			=> CodeTreeConstruct.Expression( TypeDefinition.BooleanType, BinaryExpression( SyntaxKind.LessThanToken, left.AsExpression(), right.AsExpression() ) );

		protected override CodeTreeConstruct EmitIncrement( CodeTreeContext context, CodeTreeConstruct int32Value )
			=> CodeTreeConstruct.Expression( TypeDefinition.Int32Type, PostfixUnaryExpression( SyntaxKind.PostIncrementExpression, int32Value.AsExpression() ) );

		protected override CodeTreeConstruct EmitTypeOfExpression( CodeTreeContext context, TypeDefinition type )
			=> CodeTreeConstruct.Expression( TypeDefinition.TypeType, TypeOfExpression( ToTypeSyntax( type ) ) );

		protected override CodeTreeConstruct EmitMethodOfExpression( CodeTreeContext context, MethodBase method )
			=> CodeTreeConstruct.Expression(
				TypeDefinition.MethodBaseType,
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ThisExpression(),
					Token( SyntaxKind.DotToken ),
					IdentifierName(
						context.RegisterCachedMethodBase(
							method
						)
					)
				)
			);

		protected override CodeTreeConstruct EmitFieldOfExpression( CodeTreeContext context, FieldInfo field )
			=> CodeTreeConstruct.Expression(
				TypeDefinition.FieldInfoType,
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ThisExpression(),
					Token( SyntaxKind.DotToken ),
					IdentifierName(
						context.RegisterCachedFieldInfo(
							field
						)
					)
				)
			);

		protected override CodeTreeConstruct EmitThrowStatement( CodeTreeContext context, CodeTreeConstruct exception )
			=> CodeTreeConstruct.Statement( ThrowStatement( exception.AsExpression() ) );

		protected override CodeTreeConstruct EmitSequentialStatements( CodeTreeContext context, TypeDefinition contextType, IEnumerable<CodeTreeConstruct> statements )
		{
#if DEBUG
			statements = statements.ToArray();
			Contract.Assert( statements.All( c => c.IsStatement ) );
#endif
			return CodeTreeConstruct.Statement( statements.SelectMany( s => s.AsStatements() ) );
		}

		protected override CodeTreeConstruct DeclareLocal( CodeTreeContext context, TypeDefinition type, string name )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return
				CodeTreeConstruct.Variable(
					type,
					VariableDeclaration(
						ToTypeSyntax( type ),
						new SeparatedSyntaxList<VariableDeclaratorSyntax>().Add( VariableDeclarator( context.GetUniqueVariableName( name ) ) )
					)
				);
		}

		protected override CodeTreeConstruct ReferArgument( CodeTreeContext context, TypeDefinition type, string name, int index )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeTreeConstruct.Parameter( type, IdentifierName( name ) );
		}

		protected override CodeTreeConstruct EmitCreateNewObjectExpression( CodeTreeContext context, CodeTreeConstruct variable, ConstructorDefinition constructor, params CodeTreeConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( constructor != null );
			Contract.Assert( constructor.DeclaringType != null );
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeTreeConstruct.Expression(
					constructor.DeclaringType,
					ObjectCreationExpression(
						ToTypeSyntax( constructor.DeclaringType )
					).AddArgumentListArguments(
						arguments.Select( a => Argument( a.AsExpression() ) ).ToArray()
					)
				);
		}

		protected override CodeTreeConstruct EmitMakeRef( CodeTreeContext context, CodeTreeConstruct target )
			=> CodeTreeConstruct.Expression( target.ContextType, MakeRefExpression( target.AsExpression() ) );

		protected override CodeTreeConstruct EmitInvokeVoidMethod( CodeTreeContext context, CodeTreeConstruct instance, MethodDefinition method, params CodeTreeConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || method.DeclaringType != null );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						CreateMethodInvocation( method, instance, arguments )
					)
				);
		}

		protected override CodeTreeConstruct EmitInvokeMethodExpression( CodeTreeContext context, CodeTreeConstruct instance, MethodDefinition method, IEnumerable<CodeTreeConstruct> arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || method.DeclaringType != null );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeTreeConstruct.Expression(
					method.ReturnType,
					CreateMethodInvocation( method, instance, arguments )
				);
		}


		private static ExpressionSyntax CreateMethodInvocation( MethodDefinition method, CodeTreeConstruct instance, IEnumerable<CodeTreeConstruct> arguments )
		{
			ExpressionSyntax target;
			var methodName = method.MethodName;

			if ( method.Interface != null )
			{
				// Explicit interface impl.
#if DEBUG
				Contract.Assert( instance != null, "instance != null" );
				Contract.Assert( method.TryGetRuntimeMethod() != null, "method.TryGetRuntimeMethod() != null" );
				Contract.Assert( !method.TryGetRuntimeMethod().GetIsPublic(), method.TryGetRuntimeMethod() + " is non public" );
#endif // DEBUG
				target =
					CastExpression(
						// Generics is not supported yet.
						ToTypeSyntax( method.Interface ),
						instance.AsExpression()
					);
				methodName = method.MethodName.Substring( method.MethodName.LastIndexOf( '.' ) + 1 );
			}
			else
			{
				target =
					instance == null
						? IdentifierName( method.DeclaringType.TypeName )
						: instance.AsExpression();
			}

			return
				InvocationExpression(
					MemberAccessExpression(
						SyntaxKind.SimpleAssignmentExpression,
						target,
						( method.TryGetRuntimeMethod() != null && method.TryGetRuntimeMethod().IsGenericMethod )
							? GenericName(
								Identifier( methodName ),
								TypeArgumentList(
									new SeparatedSyntaxList<TypeSyntax>().AddRange( method.TryGetRuntimeMethod().GetGenericArguments().Select( t => ToTypeSyntax( t ) ) )
								)
							) : IdentifierName( methodName ) as SimpleNameSyntax
					),
					ArgumentList(
						new SeparatedSyntaxList<ArgumentSyntax>().AddRange( arguments.Select( a => Argument( a.AsExpression() ) ) )
					)
				);
		}

		protected override CodeTreeConstruct EmitInvokeDelegateExpression( CodeTreeContext context, TypeDefinition delegateReturnType, CodeTreeConstruct @delegate, params CodeTreeConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( @delegate.IsExpression );
			Contract.Assert(
				@delegate.ContextType.TypeName.StartsWith( "System.Action" )
				|| @delegate.ContextType.TypeName.StartsWith( "System.Func" )
			);
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeTreeConstruct.Expression(
					delegateReturnType,
					InvocationExpression(
						@delegate.AsExpression(),
						ArgumentList( new SeparatedSyntaxList<ArgumentSyntax>().AddRange( arguments.Select( a => Argument( a.AsExpression() ) ) ) )
					)
				);
		}
		protected override CodeTreeConstruct EmitGetPropertyExpression( CodeTreeContext context, CodeTreeConstruct instance, PropertyInfo property )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( property.DeclaringType != null );
#endif
			return
				CodeTreeConstruct.Expression(
					property.PropertyType,
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						instance == null
							? ToTypeSyntax( property.DeclaringType )
							: instance.AsExpression(),
						IdentifierName( property.Name )
					)
				);
		}

		protected override CodeTreeConstruct EmitGetFieldExpression( CodeTreeContext context, CodeTreeConstruct instance, FieldDefinition field )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || field.DeclaringType != null );
#endif
			return
				CodeTreeConstruct.Expression(
					field.FieldType,
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						instance == null
							? ToTypeSyntax( field.DeclaringType )
							: instance.AsExpression(),
						IdentifierName( field.FieldName )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitSetProperty( CodeTreeContext context, CodeTreeConstruct instance, PropertyInfo property, CodeTreeConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( property.DeclaringType != null );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								instance == null
									? ToTypeSyntax( property.DeclaringType )
									: instance.AsExpression(),
								IdentifierName( property.Name )
							),
							value.AsExpression()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "5", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitSetIndexedProperty( CodeTreeContext context, CodeTreeConstruct instance, TypeDefinition declaringType, string proeprtyName, CodeTreeConstruct key, CodeTreeConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || declaringType.HasRuntimeTypeFully() );
			Contract.Assert( key.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							ElementAccessExpression(
								instance == null
									? ToTypeSyntax( declaringType.ResolveRuntimeType() )
									: instance.AsExpression()
							).WithExpression(
								key.AsExpression()
							),
							value.AsExpression()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitSetField( CodeTreeContext context, CodeTreeConstruct instance, FieldDefinition field, CodeTreeConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || field.DeclaringType != null );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								instance == null
									? ToTypeSyntax( field.DeclaringType )
									: instance.AsExpression(),
								IdentifierName( field.FieldName )
							),
							value.AsExpression()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitSetField( CodeTreeContext context, CodeTreeConstruct instance, TypeDefinition nestedType, string fieldName, CodeTreeConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								instance == null
									? ToTypeSyntax( nestedType )
									: instance.AsExpression(),
								IdentifierName( fieldName )
							),
							value.AsExpression()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitLoadVariableExpression( CodeTreeContext context, CodeTreeConstruct variable )
			=> CodeTreeConstruct.Expression( variable.ContextType, variable.AsExpression() );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitStoreVariableStatement( CodeTreeContext context, CodeTreeConstruct variable, CodeTreeConstruct value )
		{
#if DEBUG
			Contract.Assert( variable.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							variable.AsExpression(),
							value.AsExpression()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitTryFinally( CodeTreeContext context, CodeTreeConstruct tryStatement, CodeTreeConstruct finallyStatement )
		{
#if DEBUG
			Contract.Assert( tryStatement.IsStatement );
			Contract.Assert( finallyStatement.IsStatement );
#endif
			return
				CodeTreeConstruct.Statement(
					TryStatement(
						Block(
							tryStatement.AsStatements()
						),
						EmptyCatches,
						FinallyClause(
							Block(
								finallyStatement.AsStatements()
							)
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitCreateNewArrayExpression( CodeTreeContext context, TypeDefinition elementType, int length )
			=> CodeTreeConstruct.Expression(
				TypeDefinition.Array( elementType ),
				ArrayCreationExpression(
					ArrayType(
						ToTypeSyntax( elementType ),
						new SyntaxList<ArrayRankSpecifierSyntax>().Add(
							ArrayRankSpecifier(
								new SeparatedSyntaxList<ExpressionSyntax>().Add( LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( length ) ) )
							)
						)
					)
				)
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitCreateNewArrayExpression( CodeTreeContext context, TypeDefinition elementType, int length, IEnumerable<CodeTreeConstruct> initialElements )
		{
#if DEBUG
			initialElements = initialElements.ToArray();
			Contract.Assert( initialElements.All( i => i.IsExpression ) );
#endif
			return
				CodeTreeConstruct.Expression(
					TypeDefinition.Array( elementType ),
					ArrayCreationExpression(
						ArrayType(
							ToTypeSyntax( elementType ),
							new SyntaxList<ArrayRankSpecifierSyntax>().Add(
								ArrayRankSpecifier(
									new SeparatedSyntaxList<ExpressionSyntax>().Add( LiteralExpression( SyntaxKind.NumericLiteralExpression, Literal( length ) ) )
								)
							)
						),
						InitializerExpression(
							SyntaxKind.ArrayInitializerExpression,
							new SeparatedSyntaxList<ExpressionSyntax>().AddRange( initialElements.Select( i => i.AsExpression() ) )
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitGetArrayElementExpression( CodeTreeContext context, CodeTreeConstruct array, CodeTreeConstruct index )
			=> CodeTreeConstruct.Expression(
				array.ContextType.ElementType,
				ElementAccessExpression( array.AsExpression() )
					.WithExpression( index.AsExpression() )
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitSetArrayElementStatement( CodeTreeContext context, CodeTreeConstruct array, CodeTreeConstruct index, CodeTreeConstruct value )
			=> CodeTreeConstruct.Statement(
				ExpressionStatement(
					AssignmentExpression(
						SyntaxKind.SimpleAssignmentExpression,
						ElementAccessExpression(
							array.AsExpression()
						).WithExpression(
							index.AsExpression()
						),
						value.AsExpression()
					)
				)
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitGetSerializerExpression( CodeTreeContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
			=> CodeTreeConstruct.Expression(
				typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					ThisExpression(),
					IdentifierName(
						context.RegisterSerializer(
							targetType,
							memberInfo?.GetEnumMemberSerializationMethod() ?? EnumMemberSerializationMethod.Default,
							memberInfo?.GetDateTimeMemberConversionMethod() ?? DateTimeMemberConversionMethod.Default,
							itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo )
						)
					)
				)
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitGetActionsExpression( CodeTreeContext context, ActionType actionType, bool isAsync )
		{
			TypeDefinition type;
			string name;
			switch ( actionType )
			{
				case ActionType.PackToArray:
				{
					type =
						isAsync ? typeof( IList<> ).MakeGenericType( typeof( Func<,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( CancellationToken ), typeof( Task ) ) ) :
							typeof( IList<> ).MakeGenericType( typeof( Action<,> ).MakeGenericType( typeof( Packer ), this.TargetType ) );
					name = FieldName.PackOperationList;
					break;
				}
				case ActionType.PackToMap:
				{
					type =
						isAsync ? typeof( IDictionary<,> ).MakeGenericType( typeof( string ), typeof( Func<,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( CancellationToken ), typeof( Task ) ) ) :
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
						TypeDefinition.GenericReferenceType(
							typeof( IList<> ),
							isAsync ?
								TypeDefinition.GenericReferenceType(
									typeof( Func<,,,,,> ),
									typeof( Unpacker ),
									context.UnpackingContextType ?? this.TargetType,
									typeof( int ),
									typeof( int ),
									typeof( CancellationToken ),
									typeof( Task )
								) :
								TypeDefinition.GenericReferenceType(
									typeof( Action<,,,> ),
									typeof( Unpacker ),
									context.UnpackingContextType ?? this.TargetType,
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
						TypeDefinition.GenericReferenceType(
							typeof( IDictionary<,> ),
							typeof( string ),
							isAsync ?
								TypeDefinition.GenericReferenceType(
									typeof( Func<,,,,,> ),
									typeof( Unpacker ),
									context.UnpackingContextType ?? this.TargetType,
									typeof( int ),
									typeof( int ),
									typeof( CancellationToken ),
									typeof( Task )
								) :
								TypeDefinition.GenericReferenceType(
									typeof( Action<,,,> ),
									typeof( Unpacker ),
									context.UnpackingContextType ?? this.TargetType,
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
						isAsync ? typeof( Func<,,,,> ).MakeGenericType( typeof( Unpacker ), this.TargetType, typeof( int ), typeof( CancellationToken ), typeof( Task ) ) :
							typeof( Action<,,> ).MakeGenericType( typeof( Unpacker ), this.TargetType, typeof( int ) );
					name = FieldName.UnpackTo;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( nameof( actionType ) );
				}
			}

			if ( isAsync )
			{
				name += "Async";
			}

			var field = context.DeclarePrivateField( name, type );

			return
				CodeTreeConstruct.Expression(
					type,
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						ThisExpression(),
						IdentifierName( field.FieldName )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitGetMemberNamesExpression( CodeTreeContext context )
		{
			var field = context.DeclarePrivateField( FieldName.MemberNames, typeof( IList<string> ) );

			return
				CodeTreeConstruct.Expression(
					typeof( IList<string> ),
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						ThisExpression(),
						IdentifierName( field.FieldName )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitFinishFieldInitializationStatement( CodeTreeContext context, string name, CodeTreeConstruct value )
			=> CodeTreeConstruct.Statement(
				ExpressionStatement(
					AssignmentExpression(
						SyntaxKind.SimpleAssignmentExpression,
						MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							ThisExpression(),
							IdentifierName( name )
						),
						value.AsExpression()
					)
				)
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitConditionalExpression( CodeTreeContext context, CodeTreeConstruct conditionExpression, CodeTreeConstruct thenExpression, CodeTreeConstruct elseExpression )
		{
#if DEBUG
			Contract.Assert(
				elseExpression == null
				|| thenExpression.ContextType.TryGetRuntimeType() == typeof( void )
				|| ( thenExpression.ContextType.TryGetRuntimeType() == elseExpression.ContextType.TryGetRuntimeType() ),
				thenExpression.ContextType + " != " + ( elseExpression == null ? "(null)" : elseExpression.ContextType.TypeName )
			);
			Contract.Assert( conditionExpression.IsExpression );
			Contract.Assert(
				elseExpression == null
				|| thenExpression.ContextType.TryGetRuntimeType() == typeof( void )
				|| ( thenExpression.ContextType.TryGetRuntimeType() == elseExpression.ContextType.TryGetRuntimeType()
					&& thenExpression.IsExpression == elseExpression.IsExpression )
			);

#endif

			return
				elseExpression == null
					? CodeTreeConstruct.Statement(
						IfStatement(
							conditionExpression.AsExpression(),
							Block(
								thenExpression.AsStatements()
							)
						)
					)
					: thenExpression.ContextType.TryGetRuntimeType() == typeof( void ) || thenExpression.IsStatement
						? CodeTreeConstruct.Statement(
							IfStatement(
								conditionExpression.AsExpression(),
								Block(
									thenExpression.AsStatements()
								),
								ElseClause(
									Block(
										elseExpression.AsStatements()
									)
								)
							)
						)
						: CodeTreeConstruct.Expression(
							thenExpression.ContextType,
							ConditionalExpression(
								conditionExpression.AsExpression(),
								thenExpression.AsExpression(),
								elseExpression.AsExpression()
							)
						);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitAndConditionalExpression( CodeTreeContext context, IList<CodeTreeConstruct> conditionExpressions, CodeTreeConstruct thenExpression, CodeTreeConstruct elseExpression )
		{
#if DEBUG
			Contract.Assert( conditionExpressions.All( c => c.IsExpression ) );
			Contract.Assert( thenExpression.IsStatement );
			Contract.Assert( elseExpression.IsStatement );
#endif

			return
				CodeTreeConstruct.Statement(
					IfStatement(
						conditionExpressions
							.Select( c => c.AsExpression() )
							.Aggregate( ( l, r ) =>
												BinaryExpression( SyntaxKind.LogicalAndExpression, l, r )
							),
						Block(
							thenExpression.AsStatements()
						),
						ElseClause(
							Block(
								elseExpression.AsStatements()
							)
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitRetrunStatement( CodeTreeContext context, CodeTreeConstruct expression )
		{
#if DEBUG
			Contract.Assert( expression.IsExpression );
#endif
			return
				CodeTreeConstruct.Statement(
					ReturnStatement(
						expression.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitForEachLoop( CodeTreeContext context, CollectionTraits collectionTraits, CodeTreeConstruct collection, Func<CodeTreeConstruct, CodeTreeConstruct> loopBodyEmitter )
		{
#if DEBUG
			Contract.Assert( collection.IsExpression );
#endif

			var currentName = context.GetUniqueVariableName( "current" );
			var current = this.DeclareLocal( context, collectionTraits.ElementType, currentName );
			return
				CodeTreeConstruct.Statement(
					ForEachStatement(
						ToTypeSyntax( collectionTraits.ElementType ),
						currentName,
						collection.AsExpression(),
						Block(
							loopBodyEmitter( current ).AsStatements()
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitEnumFromUnderlyingCastExpression(
			CodeTreeContext context,
			Type enumType,
			CodeTreeConstruct underlyingValue
		) => CodeTreeConstruct.Expression( enumType, CastExpression( ToTypeSyntax( enumType ), underlyingValue.AsExpression() ) );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitEnumToUnderlyingCastExpression(
			CodeTreeContext context,
			Type underlyingType,
			CodeTreeConstruct enumValue
		) => CodeTreeConstruct.Expression( underlyingType, CastExpression( ToTypeSyntax( underlyingType ), enumValue.AsExpression() ) );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override CodeTreeConstruct EmitNewPrivateMethodDelegateExpression( CodeTreeContext context, MethodDefinition method )
		{
			var delegateBaseType = SerializerBuilderHelper.FindDelegateType( method.ReturnType, method.ParameterTypes );

			var delegateType =
				( method.ReturnType.TryGetRuntimeType() == typeof( void ) && method.ParameterTypes.Length == 0 )
					? delegateBaseType
					: TypeDefinition.GenericReferenceType(
						delegateBaseType,
						method.ReturnType.TryGetRuntimeType() == typeof( void )
							? method.ParameterTypes
							: method.ParameterTypes.Concat( new[] { method.ReturnType } ).ToArray()
					);

			return
				CodeTreeConstruct.Expression(
					delegateType,
					ObjectCreationExpression(
						ToTypeSyntax( delegateType )
					).AddArgumentListArguments(
						Argument(
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								method.IsStatic ? IdentifierName( context.DeclaringTypeName ) : ThisExpression() as ExpressionSyntax,
								IdentifierName( method.MethodName )
							)
						)
					)
				);
		}

		/// <summary>
		///		Builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		/// </param>
		/// <param name="concreteType">The substitution type if <see cref="TargetType"/> is abstract type. <c>null</c> when <see cref="TargetType"/> is not abstract type.</param>
		/// <param name="itemSchema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value must not be <c>null</c>.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		public void BuildSerializerCode( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( nameof( context ) );
			}

			var asCodeTreeContext = context as CodeTreeContext;
			if ( asCodeTreeContext == null )
			{
				throw new ArgumentException(
						"'context' was not created with CreateGenerationContextForCodeGeneration method.",
						nameof( context )
					);
			}

			asCodeTreeContext.Reset( this.TargetType, this.BaseClass );

			if ( !this.TargetType.GetIsEnum() )
			{
				SerializationTarget targetInfo;
				this.BuildSerializer( asCodeTreeContext, concreteType, itemSchema, out targetInfo );
				this.Finish(
					asCodeTreeContext,
					targetInfo,
					false,
					targetInfo == null
						? default( SerializerCapabilities? )
						: this.CollectionTraits.CollectionType == CollectionKind.NotCollection
							? targetInfo.GetCapabilitiesForObject()
							: targetInfo.GetCapabilitiesForCollection( this.CollectionTraits )
				);
			}
			else
			{
				this.BuildEnumSerializer( asCodeTreeContext );
				this.Finish( asCodeTreeContext, null, true, ( SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) );
			}
		}

		protected override Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructor( CodeTreeContext codeGenerationContext, SerializationTarget targetInfo, PolymorphismSchema schema, SerializerCapabilities? capabilities )
		{
			this.Finish( codeGenerationContext, targetInfo, false, capabilities );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );

			return targetType.CreateConstructorDelegate<MessagePackSerializer, SerializationContext>();
		}

		protected override Func<SerializationContext, MessagePackSerializer> CreateEnumSerializerConstructor( CodeTreeContext codeGenerationContext )
		{
			this.Finish( codeGenerationContext, null, true, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );

			return targetType.CreateConstructorDelegate<MessagePackSerializer, SerializationContext>();
		}

		private static Type PrepareSerializerConstructorCreation( CodeTreeContext codeGenerationContext )
		{
			if ( !SerializerDebugging.OnTheFlyCodeDomEnabled )
			{
				throw new NotSupportedException();
			}

			CompilationUnitSyntax cu = codeGenerationContext.CreateCompilationUnit();

			if ( SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.TraceEmitEvent( "Compile {0}", codeGenerationContext.DeclaringTypeName );
				cu.WriteTo( SerializerDebugging.ILTraceWriter );
				SerializerDebugging.FlushTraceData();
			}

			var assemblyName = $"CodeAssembly{DateTime.Now:yyyyMMddHHmmssfff}{ProcessId}{Environment.CurrentManagedThreadId}";

			var compilation =
				CSharpCompilation.Create(
					assemblyName,
					new[] { cu.SyntaxTree },
					SerializerDebugging.CodeDomSerializerDependentAssemblies.Select( x => MetadataReference.CreateFromFile( x ) ),
					new CSharpCompilationOptions(
						OutputKind.DynamicallyLinkedLibrary
#if !DEBUG || PERFORMANCE_TEST
						, optimizationLevel: OptimizationLevel.Release
#endif // !DEBUG || PERFORMANCE_TEST
					)
				);
			var outputPath = Path.Combine( Path.GetTempPath(), assemblyName + "." + Extension );

			var result = compilation.Emit( outputPath );
			if ( !result.Success )
			{
				if ( SerializerDebugging.TraceEnabled && !SerializerDebugging.DumpEnabled )
				{
					cu.WriteTo( SerializerDebugging.ILTraceWriter );
					SerializerDebugging.FlushTraceData();
				}

				throw new System.Runtime.Serialization.SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Failed to compile assembly. Details:{0}{1}",
							Environment.NewLine,
							BuildCompilationError( result.Diagnostics )
						)
					);
			}
#if DEBUG
			// Check warning except ambigious type reference.
			var warnings = 
				result.Diagnostics.Where( e => e.Id != "CS0436" ).ToArray();
			Contract.Assert( !warnings.Any(), BuildCompilationError( result.Diagnostics ) );
#endif

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEmitEvent( "Build assembly '{0}' from dom.", outputPath );
			}

			SerializerDebugging.AddCompiledCodeDomAssembly( outputPath );

			var rootNamespace =
				cu.SyntaxTree.GetCompilationUnitRoot().ChildNodesAndTokens()
					.First( x => x.AsNode() is NamespaceDeclarationSyntax ).ChildNodesAndTokens()
					.First( x => x.AsNode() is QualifiedNameSyntax ).ToString();

			var assembly = SerializerDebugging.LoadAssembly( outputPath );
			var targetType =
				assembly.ExportedTypes
					.SingleOrDefault(
						t =>
							t.Namespace == rootNamespace
							&& t.Name == codeGenerationContext.DeclaringTypeName
					);

			Contract.Assert(
				targetType != null,
				rootNamespace + " not in:" +
				String.Join(
					Environment.NewLine,
					assembly.ExportedTypes
						.Select( t => t.FullName ).ToArray()
				)
			);

			return targetType;
		}

		private static string BuildCompilationError( IEnumerable<Diagnostic> diagnostics )
			=> String.Join(
				Environment.NewLine,
				diagnostics
					.Select(
						( error, i ) =>
							String.Format(
								CultureInfo.InvariantCulture,
								"[{0}]{1}:{2}:({3}):{4}",
								i,
								error.Severity,
								error.Id,
								error.Location,
								error
							)
					).ToArray()
			);

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
		private void Finish( CodeTreeContext context, SerializationTarget targetInfo, bool isEnum, SerializerCapabilities? capabilities )
		{
			var publicConstructorTemplate =
				ConstructorDeclaration(
					Identifier( context.DeclaringTypeName )
				).WithModifiers( SyntaxTokenList.Create( Token( SyntaxKind.PublicKeyword ) ) );

			// ctor
			if ( isEnum )
			{
				// .ctor(SerializationContext context) : base(context, EnumSerializationMethod.Xxx)
				context.AddMember(
					publicConstructorTemplate.WithParameterList(
						ContextParameterListSyntax
					).WithInitializer(
						ConstructorInitializer(
							SyntaxKind.BaseConstructorInitializer,
							ArgumentList(
								ContextArgumentListTemplate.Add(
									Argument(
										MemberAccessExpression(
											SyntaxKind.SimpleMemberAccessExpression,
											EnumSerializationMethodTypeIdentifierSyntax,
											IdentifierName(
												EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
													context.SerializationContext,
													this.TargetType,
													EnumMemberSerializationMethod.Default
												).ToString()
											)
										)
									)
								)
							)
						)
					)
				);

				// .ctor(SerializationContext context, EnumSerializationMethod enumSerializationMethod) : base(context, enumSerializationMethod)
				context.AddMember(
					publicConstructorTemplate.WithParameterList(
						ContextAndEnumSerializationMethodParameterListSyntax
					).WithInitializer(
						ContextAndEnumSerializationMethodConstructorInitializerSyntax
					)
				);
			}
			else
			{
				// not enum

				context.BeginConstructor();
				try
				{
					var statements = this.BuildDependentSerializersInitialization( context );
					statements = BuildCachedFieldInfosInitialization( context, statements );
					statements = BuildCachedMethodInfosInitialization( context, statements );

					if ( targetInfo != null && this.CollectionTraits.CollectionType == CollectionKind.NotCollection )
					{
						// For object only.
						if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType )
							|| !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType )
						)
						{
							statements = this.BuildPackOperationDelegatesInitialization( context, targetInfo, statements );
						}

						if ( targetInfo.CanDeserialize
							&& ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType )
								|| !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
						)
						{
							statements = this.BuildUnpackOperationDelegatesInitialization( context, targetInfo, statements );
						}
					}

					statements = BuildCachedDelegatesInitialization( context, statements );

					if ( context.IsUnpackToUsed )
					{
						statements = statements.AddRange( this.EmitUnpackToInitialization( context ).AsStatements() );
					}

					context.AddMember(
						publicConstructorTemplate.WithParameterList(
							ContextParameterListSyntax
						).WithInitializer(
							ConstructorInitializer(
								SyntaxKind.BaseConstructorInitializer,
								ArgumentList(
									this.BuildBaseConstructorArgs( context, capabilities )
								)
							)
						).WithBody( Block( statements ) )
					);
				}
				finally
				{
					context.EndConstructor();
				}
			} // else of isEnum
		}

		private static SyntaxList<StatementSyntax> BuildCachedDelegatesInitialization( CodeTreeContext context, SyntaxList<StatementSyntax> statements )
		{
			foreach ( var cachedDelegateInfo in context.GetCachedDelegateInfos() )
			{
				statements =
					statements.Add(
						ExpressionStatement(
							AssignmentExpression(
								SyntaxKind.SimpleAssignmentExpression,
								MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									ThisExpression(),
									IdentifierName( cachedDelegateInfo.BackingField.FieldName )
								),
								ObjectCreationExpression(
									ToTypeSyntax( cachedDelegateInfo.BackingField.FieldType )
								).AddArgumentListArguments(
									Argument(
										MemberAccessExpression(
											SyntaxKind.SimpleMemberAccessExpression,
											cachedDelegateInfo.IsThisInstance
												? ThisExpression() as ExpressionSyntax
												: ToTypeSyntax( cachedDelegateInfo.TargetMethod.DeclaringType ),
											IdentifierName( cachedDelegateInfo.TargetMethod.MethodName )
										)
									)
								)
							)
						)
					);
			}

			return statements;
		}

		private SeparatedSyntaxList<ArgumentSyntax> BuildBaseConstructorArgs( CodeTreeContext context, SerializerCapabilities? capabilities )
		{
			var baseConstructorArgs = ContextArgumentListTemplate;

			if ( this.BaseClass.GetTypeInfo().DeclaredConstructors.Where( c => !c.IsPublic && !c.IsStatic )
					.Any(
						c =>
							c.GetParameterTypes()
								.SequenceEqual( CollectionSerializerHelpers.CollectionConstructorTypes )
					)
			)
			{
				baseConstructorArgs =
					baseConstructorArgs.Add(
						Argument(
							InvocationExpression(
								MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									IdentifierName( context.DeclaringTypeName ),
									RestoreSchemaMethodIdentifierSyntax
								)
							)
						)
					);
			}

			if ( capabilities.HasValue )
			{
				var capabilitiesExpression = BuildCapabilitiesExpression( null, capabilities.Value, SerializerCapabilities.PackTo );
				capabilitiesExpression = BuildCapabilitiesExpression( capabilitiesExpression, capabilities.Value, SerializerCapabilities.UnpackFrom );
				capabilitiesExpression = BuildCapabilitiesExpression( capabilitiesExpression, capabilities.Value, SerializerCapabilities.UnpackTo );

				baseConstructorArgs =
					baseConstructorArgs.Add(
						Argument( capabilitiesExpression ?? SerializerCapabilitiesNoneSyntax )
					);
			}

			return baseConstructorArgs;
		}

		private SyntaxList<StatementSyntax> BuildDependentSerializersInitialization( CodeTreeContext context )
		{
			var statements = new SyntaxList<StatementSyntax>();

			int schemaNumber = -1;
			foreach ( var dependentSerializer in context.GetDependentSerializers() )
			{
				var targetType = Type.GetTypeFromHandle( dependentSerializer.Key.TypeHandle );
				var targetTypeSyntax = ToTypeSyntax( targetType );
				ExpressionSyntax getSerializerArgument;

				if ( targetType.GetIsEnum() )
				{
					getSerializerArgument =
						EnumMessagePackSerializerHelpersDetermineEnumSerializationMethodMethodTemplate
							.AddArgumentListArguments(
								ContextArgumentSyntax,
								Argument( TypeOfExpression( targetTypeSyntax ) ),
								Argument(
									MemberAccessExpression(
										SyntaxKind.SimpleMemberAccessExpression,
										EnumSerializationMethodTypeIdentifierSyntax,
										IdentifierName( dependentSerializer.Key.EnumSerializationMethod.ToString() )
									)
								)
							);

				}
				else if ( DateTimeMessagePackSerializerHelpers.IsDateTime( targetType ) )
				{
					getSerializerArgument =
						DateTimeMessagePackSerializerHelpersDetermineDateTimeConversionMethodTemplate
							.AddArgumentListArguments(
								ContextArgumentSyntax,
								Argument(
									MemberAccessExpression(
										SyntaxKind.SimpleMemberAccessExpression,
										DateTimeMemberConversionMethodIdentifierSyntax,
										IdentifierName( dependentSerializer.Key.DateTimeConversionMethod.ToString() )
									)
								)
							);
				}
				else
				{
					if ( dependentSerializer.Key.PolymorphismSchema == null )
					{
						getSerializerArgument = NullLiteralSyntax;
					}
					else
					{
						schemaNumber++;
						var variableName = "schema" + schemaNumber;
						var schema = this.DeclareLocal( context, typeof( PolymorphismSchema ), variableName );
						statements =
							statements
								.AddRange( schema.AsStatements() )
								.AddRange(
									this.EmitConstructPolymorphismSchema(
										context,
										schema,
										dependentSerializer.Key.PolymorphismSchema
									).SelectMany( st => st.AsStatements() )
								);

						getSerializerArgument = IdentifierName( variableName );
					}
				}

				statements.Add(
					ExpressionStatement(
						AssignmentExpression(
							SyntaxKind.SimpleAssignmentExpression,
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								ThisExpression(),
								IdentifierName( dependentSerializer.Value )
							),
							InvocationExpression(
								MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									ContextArgumentReferenceSyntax,
									GenericName( "GetSerializer" )
										.AddTypeArgumentListArguments(
											targetTypeSyntax
										)
								)
							).AddArgumentListArguments( Argument( getSerializerArgument ) )
						)
					)
				);
			} // foreach ( in context.GetDependentSerializers() )

			return statements;
		}

		private static SyntaxList<StatementSyntax> BuildCachedFieldInfosInitialization( CodeTreeContext context, SyntaxList<StatementSyntax> statements )
		{
			foreach ( var cachedFieldInfo in context.GetCachedFieldInfos() )
			{
				var fieldInfo = cachedFieldInfo.Value.Target;
#if DEBUG
				Contract.Assert( fieldInfo != null, "fieldInfo != null" );
				Contract.Assert( fieldInfo.DeclaringType != null, "fieldInfo.DeclaringType != null" );
#endif // DEBUG
				statements =
					statements.Add(
						ExpressionStatement(
							AssignmentExpression(
								SyntaxKind.SimpleAssignmentExpression,
								MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									ThisExpression(),
									IdentifierName( cachedFieldInfo.Value.StorageFieldName )
								),
								ReflectionHelpersGetFieldMethodTemplate.AddArgumentListArguments(
									Argument( TypeOfExpression( ToTypeSyntax( fieldInfo.DeclaringType ) ) ),
									Argument( LiteralExpression( SyntaxKind.StringLiteralExpression, Literal( fieldInfo.Name ) ) )
								)
							)
						)
					);
			} // foreach ( in context.GetCachedFieldInfos() )

			return statements;
		}

		private static SyntaxList<StatementSyntax> BuildCachedMethodInfosInitialization( CodeTreeContext context, SyntaxList<StatementSyntax> statements )
		{
			foreach ( var cachedMethodBase in context.GetCachedMethodBases() )
			{
				var methodBase = cachedMethodBase.Value.Target;
				var parameterTypes = methodBase.GetParameterTypes();
				statements =
					statements.Add(
						ExpressionStatement(
							AssignmentExpression(
								SyntaxKind.SimpleAssignmentExpression,
								MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									ThisExpression(),
									IdentifierName( cachedMethodBase.Value.StorageFieldName )
								),
								ReflectionHelpersGetMethodMethodTemplate.AddArgumentListArguments(
									Argument( TypeOfExpression( ToTypeSyntax( methodBase.DeclaringType ) ) ),
									Argument( LiteralExpression( SyntaxKind.StringLiteralExpression, Literal( methodBase.Name ) ) ),
									Argument(
										TypeArrayCreationTemplate.WithInitializer(
											InitializerExpression(
												SyntaxKind.ArrayInitializerExpression,
												new SeparatedSyntaxList<ExpressionSyntax>().AddRange(
													parameterTypes.Select( t => TypeOfExpression( ToTypeSyntax( t ) ) )
												)
											)
										)
									)
								)
							)
						)
					);
			} // foreach ( in context.GetCachedMethodBases() )

			return statements;
		}

		private SyntaxList<StatementSyntax> BuildPackOperationDelegatesInitialization( CodeTreeContext context, SerializationTarget targetInfo, SyntaxList<StatementSyntax> statements )
		{
			if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
			{
				statements =
					statements.AddRange(
						this.EmitPackOperationListInitialization( context, targetInfo, isAsync: false ).AsStatements()
					);
			}

			if ( this.WithAsync( context ) && !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
			{
				statements =
					statements.AddRange(
						this.EmitPackOperationListInitialization( context, targetInfo, isAsync: true ).AsStatements()
					);
			}

			if ( targetInfo.Members.Any( m => m.Member != null ) ) // except Tuple
			{
				if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
				{
					statements =
						statements.AddRange(
							this.EmitPackOperationTableInitialization( context, targetInfo, isAsync: false ).AsStatements()
						);
				}

				if ( this.WithAsync( context ) && !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
				{
					statements =
						statements.AddRange(
							this.EmitPackOperationTableInitialization( context, targetInfo, isAsync: true ).AsStatements()
						);
				}

				if ( !SerializerDebugging.UseLegacyNullMapEntryHandling
					&& ( !typeof( IPackable ).IsAssignableFrom( this.TargetType )
						|| ( !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) && this.WithAsync( context ) )
					) )
				{
					statements =
						statements.AddRange(
							this.EmitPackNullCheckerTableInitialization( context, targetInfo ).AsStatements()
						);
				}
			}

			return statements;
		}

		private SyntaxList<StatementSyntax> BuildUnpackOperationDelegatesInitialization( CodeTreeContext context, SerializationTarget targetInfo, SyntaxList<StatementSyntax> statements )
		{
			if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType ) )
			{
				statements =
					statements.AddRange(
						this.EmitUnpackOperationListInitialization( context, targetInfo, false ).AsStatements()
					);
			}

			if ( this.WithAsync( context ) && !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
			{
				statements =
					statements.AddRange(
						this.EmitUnpackOperationListInitialization( context, targetInfo, true ).AsStatements()
					);
			}

			if ( targetInfo.Members.Any( m => m.Member != null ) )
			{
				// Except tuples
				if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType ) )
				{
					statements =
						statements.AddRange(
							this.EmitUnpackOperationTableInitialization( context, targetInfo, false ).AsStatements()
						);
				}

				if ( this.WithAsync( context ) && !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
				{
					statements =
						statements.AddRange(
							this.EmitUnpackOperationTableInitialization( context, targetInfo, true ).AsStatements()
						);
				}
			}

			if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType )
				|| ( !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) && this.WithAsync( context ) )
			)
			{
				statements =
					statements.AddRange(
						this.EmitMemberListInitialization( context, targetInfo ).AsStatements()
					);
			}

			return statements;
		}

		private static ExpressionSyntax BuildCapabilitiesExpression( ExpressionSyntax expression, SerializerCapabilities capabilities, SerializerCapabilities value )
		{
			if ( ( capabilities & value ) != 0 )
			{
				var capabilityExpression =
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						SerializerCapabilitiesTypeIdentifierSyntax,
						IdentifierName( value.ToString() )
					);

				if ( expression == null )
				{
					return capabilityExpression;
				}
				else
				{
					return
						BinaryExpression(
							SyntaxKind.BitwiseOrExpression,
							expression,
							capabilityExpression
						);
				}
			}
			else
			{
				return expression;
			}
		}

		protected override CodeTreeContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			var result = new CodeTreeContext( context, new SerializerCodeGenerationConfiguration(), Extension );
			result.Reset( this.TargetType, this.BaseClass );
			return result;
		}
	}
}