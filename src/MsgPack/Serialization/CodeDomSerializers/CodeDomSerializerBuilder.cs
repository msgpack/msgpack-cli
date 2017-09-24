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
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.CodeDomSerializers
{
	/// <summary>
	///		Code DOM based implementation of <see cref="SerializerBuilder{TContext,TConstruct}"/>.
	///		This type supports pre-generation.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
	internal class CodeDomSerializerBuilder : SerializerBuilder<CodeDomContext, CodeDomConstruct>
	{
		private static readonly CodeTypeReference[] EmptyGenericArguments = new CodeTypeReference[ 0 ];

		internal static CodeTypeReference ToCodeTypeReference( TypeDefinition type )
		{
			if ( type == null )
			{
				return null;
			}

			if ( type.IsArray )
			{
				return new CodeTypeReference( ToCodeTypeReference( type.ElementType ), 1 );
			}

			if ( type.HasRuntimeTypeFully() )
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return new CodeTypeReference( type.ResolveRuntimeType() );
				}
				else
				{
					return
						new CodeTypeReference(
							GetGenericTypeBaseName( type ),
							type.GenericArguments.Select( ToCodeTypeReference ).ToArray()
						);
				}
			}
			else
			{
				if ( type.GenericArguments.Length == 0 )
				{
					return new CodeTypeReference( type.TypeName );
				}
				else
				{
					return new CodeTypeReference( type.TypeName, type.GenericArguments.Select( ToCodeTypeReference ).ToArray() );
				}
			}
		}

		private static string GetGenericTypeBaseName( TypeDefinition genericType )
		{
			return
				genericType.HasRuntimeTypeFully()
				? genericType.ResolveRuntimeType().FullName.Remove( genericType.ResolveRuntimeType().FullName.IndexOf( '`' ) )
				: genericType.TypeName;
		}

		private static CodeTypeReferenceExpression ToCodeTypeReferenceExpression( TypeDefinition type )
		{
			return new CodeTypeReferenceExpression( ToCodeTypeReference( type ) );
		}
		
		public CodeDomSerializerBuilder( Type targetType, CollectionTraits collectionTraits )
			: base( targetType, collectionTraits ) { }

		protected override CodeDomConstruct MakeNullLiteral( CodeDomContext context, TypeDefinition contextType )
		{
			return CodeDomConstruct.Expression( contextType, new CodePrimitiveExpression( null ) );
		}

		protected override CodeDomConstruct MakeByteLiteral( CodeDomContext context, byte constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.ByteType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeSByteLiteral( CodeDomContext context, sbyte constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.SByteType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt16Literal( CodeDomContext context, short constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.Int16Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt16Literal( CodeDomContext context, ushort constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.UInt16Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt32Literal( CodeDomContext context, int constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.Int32Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt32Literal( CodeDomContext context, uint constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.UInt32Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt64Literal( CodeDomContext context, long constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.Int64Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt64Literal( CodeDomContext context, ulong constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.UInt64Type, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeReal32Literal( CodeDomContext context, float constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.SingleType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeReal64Literal( CodeDomContext context, double constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.DoubleType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeBooleanLiteral( CodeDomContext context, bool constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.BooleanType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeCharLiteral( CodeDomContext context, char constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.CharType, new CodePrimitiveExpression( constant ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct MakeEnumLiteral( CodeDomContext context, TypeDefinition type, object constant )
		{
			var asString = constant.ToString();
			if ( ( '0' <= asString[ 0 ] && asString[ 0 ] <= '9' ) || asString.Contains( ',' ) )
			{
				// Unrepresentable numeric or combined flags
				return
					CodeDomConstruct.Expression(
						type,
						new CodeCastExpression(
							ToCodeTypeReference( type ),
							// Only support integrals.
							new CodePrimitiveExpression(
								UInt64.Parse( ( ( Enum ) constant ).ToString( "D" ), CultureInfo.InvariantCulture ) 
							)
						)
					);
			}
			else
			{
				return
					CodeDomConstruct.Expression(
						type,
						new CodeFieldReferenceExpression(
							ToCodeTypeReferenceExpression( type ), constant.ToString() 
						)
					);
			}
		}

		protected override CodeDomConstruct MakeDefaultLiteral( CodeDomContext context, TypeDefinition type )
		{
			return CodeDomConstruct.Expression( type, new CodeDefaultValueExpression( ToCodeTypeReference( type ) ) );
		}

		protected override CodeDomConstruct MakeStringLiteral( CodeDomContext context, string constant )
		{
			return CodeDomConstruct.Expression( TypeDefinition.StringType, new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct EmitThisReferenceExpression( CodeDomContext context )
		{
			return CodeDomConstruct.Expression( typeof( MessagePackSerializer<> ).MakeGenericType( this.TargetType ), new CodeThisReferenceExpression() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitBoxExpression( CodeDomContext context, TypeDefinition valueType, CodeDomConstruct value )
		{
			return CodeDomConstruct.Expression( TypeDefinition.ObjectType, new CodeCastExpression( typeof( object ), value.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitUnboxAnyExpression( CodeDomContext context, TypeDefinition targetType, CodeDomConstruct value )
		{
			return
				CodeDomConstruct.Expression(
					targetType,
					new CodeCastExpression( ToCodeTypeReference( targetType ), value.AsExpression() )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitNotExpression( CodeDomContext context, CodeDomConstruct booleanExpression )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.BooleanType,
					new CodeBinaryOperatorExpression(
						booleanExpression.AsExpression(),
						CodeBinaryOperatorType.ValueEquality,
						new CodePrimitiveExpression( false )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitEqualsExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.BooleanType,
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.ValueEquality,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGreaterThanExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.BooleanType,
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.GreaterThan,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitLessThanExpression( CodeDomContext context, CodeDomConstruct left, CodeDomConstruct right )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.BooleanType,
					new CodeBinaryOperatorExpression(
						left.AsExpression(),
						CodeBinaryOperatorType.LessThan,
						right.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
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

		protected override CodeDomConstruct EmitTypeOfExpression( CodeDomContext context, TypeDefinition type )
		{
			return CodeDomConstruct.Expression( TypeDefinition.TypeType, new CodeTypeOfExpression( ToCodeTypeReference( type ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitFieldOfExpression( CodeDomContext context, FieldInfo field )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.FieldInfoType,
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.RegisterCachedFieldInfo(
							field
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitMethodOfExpression( CodeDomContext context, MethodBase method )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.MethodBaseType,
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.RegisterCachedMethodBase(
							method
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitThrowStatement( CodeDomContext context, CodeDomConstruct exception )
		{
			return CodeDomConstruct.Statement( new CodeThrowExceptionStatement( exception.AsExpression() ) );
		}

		protected override CodeDomConstruct EmitSequentialStatements( CodeDomContext context, TypeDefinition contextType, IEnumerable<CodeDomConstruct> statements )
		{
#if DEBUG
			statements = statements.ToArray();
			Contract.Assert( statements.All( c => c.IsStatement ) );
#endif
			return CodeDomConstruct.Statement( statements.SelectMany( s => s.AsStatements().ToArray() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct DeclareLocal( CodeDomContext context, TypeDefinition nestedType, string name )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeDomConstruct.Variable( nestedType, context.GetUniqueVariableName( name ) );
		}

		protected override CodeDomConstruct ReferArgument( CodeDomContext context, TypeDefinition type, string name, int index )
		{
#if DEBUG
			Contract.Assert( !name.Contains( "." ) );
#endif
			return CodeDomConstruct.Parameter( type, name );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitCreateNewObjectExpression( CodeDomContext context, CodeDomConstruct variable, ConstructorDefinition constructor, params CodeDomConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( constructor?.DeclaringType != null );
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeDomConstruct.Expression(
					constructor.DeclaringType,
					new CodeObjectCreateExpression( 
						ToCodeTypeReference( constructor.DeclaringType ), 
						arguments.Select( a => a.AsExpression() ).ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated" )]
		protected override CodeDomConstruct EmitMakeRef( CodeDomContext context, CodeDomConstruct target )
		{
			return CodeDomConstruct.Expression( target.ContextType, new CodeDirectionExpression( FieldDirection.Ref, target.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitInvokeVoidMethod( CodeDomContext context, CodeDomConstruct instance, MethodDefinition method, params CodeDomConstruct[] arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || method.DeclaringType != null );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeExpressionStatement(
						CreateMethodInvocation( method, instance, arguments )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitInvokeMethodExpression( CodeDomContext context, CodeDomConstruct instance, MethodDefinition method, IEnumerable<CodeDomConstruct> arguments )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || method.DeclaringType != null );
			arguments = arguments.Where( a => a != null ).ToArray();
			Contract.Assert( arguments.All( c => c.IsExpression ), String.Join( ",", arguments.Select( c => c.ToString() ).ToArray() ) );
#endif
			return
				CodeDomConstruct.Expression(
					method.ReturnType,
					CreateMethodInvocation( method, instance, arguments )
				);
		}

		private static CodeMethodInvokeExpression CreateMethodInvocation( MethodDefinition method, CodeDomConstruct instance, IEnumerable<CodeDomConstruct> arguments )
		{
			CodeExpression target;
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
					new CodeCastExpression(
						// Generics is not supported yet.
						method.Interface,
						instance.AsExpression()
					);
				methodName = method.MethodName.Substring( method.MethodName.LastIndexOf( '.' ) + 1 );
			}
			else
			{
				target =
					instance == null
						? new CodeTypeReferenceExpression( method.DeclaringType.TypeName )
						: instance.AsExpression();
			}

			return
				new CodeMethodInvokeExpression(
					new CodeMethodReferenceExpression(
						target,
						methodName,
						( method.TryGetRuntimeMethod() != null && method.TryGetRuntimeMethod().IsGenericMethod )
							? method.TryGetRuntimeMethod().GetGenericArguments().Select( t => new CodeTypeReference( t ) ).ToArray()
							: EmptyGenericArguments
						),
					arguments.Select( a => a.AsExpression() ).ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitInvokeDelegateExpression( CodeDomContext context, TypeDefinition delegateReturnType, CodeDomConstruct @delegate, params CodeDomConstruct[] arguments )
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
				CodeDomConstruct.Expression(
					delegateReturnType,
					new CodeDelegateInvokeExpression(
						@delegate.AsExpression(),
						arguments.Select( a => a.AsExpression() ).ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetPropertyExpression( CodeDomContext context, CodeDomConstruct instance, PropertyInfo property )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetFieldExpression( CodeDomContext context, CodeDomConstruct instance, FieldDefinition field )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || field.DeclaringType != null );
#endif
			return
				CodeDomConstruct.Expression(
					field.FieldType,
					new CodeFieldReferenceExpression(
						instance == null
						? ToCodeTypeReferenceExpression( field.DeclaringType )
						: instance.AsExpression(),
						field.FieldName
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitSetProperty( CodeDomContext context, CodeDomConstruct instance, PropertyInfo property, CodeDomConstruct value )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "5", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitSetIndexedProperty( CodeDomContext context, CodeDomConstruct instance, TypeDefinition declaringType, string proeprtyName, CodeDomConstruct key, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || declaringType.HasRuntimeTypeFully() );
			Contract.Assert( key.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeIndexerExpression(
							instance == null
							? new CodeTypeReferenceExpression( declaringType.ResolveRuntimeType() )
							: instance.AsExpression(),
							key.AsExpression()
						), 
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitSetField( CodeDomContext context, CodeDomConstruct instance, FieldDefinition field, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( instance != null || field.DeclaringType != null );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(
							instance == null
							? ToCodeTypeReferenceExpression( field.DeclaringType )
							: instance.AsExpression(),
							field.FieldName
						),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitSetField( CodeDomContext context, CodeDomConstruct instance, TypeDefinition nestedType, string fieldName, CodeDomConstruct value )
		{
#if DEBUG
			Contract.Assert( instance == null || instance.IsExpression );
			Contract.Assert( value.IsExpression );
#endif
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(
							instance == null
							? ToCodeTypeReferenceExpression( nestedType )
							: instance.AsExpression(),
							fieldName
						),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitLoadVariableExpression( CodeDomContext context, CodeDomConstruct variable )
		{
			return CodeDomConstruct.Expression( variable.ContextType, variable.AsExpression() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitCreateNewArrayExpression( CodeDomContext context, TypeDefinition elementType, int length )
		{
			return
				CodeDomConstruct.Expression(
					TypeDefinition.Array( elementType ),
					new CodeArrayCreateExpression(
						ToCodeTypeReference( elementType ),
						new CodePrimitiveExpression( length )
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitCreateNewArrayExpression( CodeDomContext context, TypeDefinition elementType, int length, IEnumerable<CodeDomConstruct> initialElements )
		{
#if DEBUG
			initialElements = initialElements.ToArray();
			Contract.Assert( initialElements.All( i => i.IsExpression ) );
#endif
			return
				CodeDomConstruct.Expression(
					TypeDefinition.Array( elementType ),
					new CodeArrayCreateExpression(
						ToCodeTypeReference( elementType ),
						initialElements.Select( i => i.AsExpression() ).ToArray()
					)
					{
						Size = length
					}
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetArrayElementExpression( CodeDomContext context, CodeDomConstruct array, CodeDomConstruct index )
		{
			return
				CodeDomConstruct.Expression(
					array.ContextType.ElementType,
					new CodeArrayIndexerExpression( array.AsExpression(), index.AsExpression() )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitSetArrayElementStatement( CodeDomContext context, CodeDomConstruct array, CodeDomConstruct index, CodeDomConstruct value )
		{
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeArrayIndexerExpression( array.AsExpression(), index.AsExpression() ),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetSerializerExpression( CodeDomContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			return
				CodeDomConstruct.Expression(
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						context.RegisterSerializer(
							targetType,
							memberInfo == null
								? EnumMemberSerializationMethod.Default
								: memberInfo.Value.GetEnumMemberSerializationMethod(),
							memberInfo == null
								? DateTimeMemberConversionMethod.Default
								: memberInfo.Value.GetDateTimeMemberConversionMethod(),
							itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo )
						)
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetActionsExpression( CodeDomContext context, ActionType actionType, bool isAsync )
		{
			TypeDefinition type;
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
						TypeDefinition.GenericReferenceType(
							typeof( IList<> ),
#if FEATURE_TAP
							isAsync ? 
							TypeDefinition.GenericReferenceType(
								typeof( Func<,,,,,> ),
								TypeDefinition.UnpackerType,
								context.UnpackingContextType ?? this.TargetType,
								TypeDefinition.Int32Type,
								TypeDefinition.Int32Type,
								TypeDefinition.CancellationTokenType,
								TypeDefinition.TaskType
							) :
#endif // FEATURE_TAP
							TypeDefinition.GenericReferenceType(
								typeof( Action<,,,> ),
								TypeDefinition.UnpackerType,
								context.UnpackingContextType ?? this.TargetType,
								TypeDefinition.Int32Type,
								TypeDefinition.Int32Type
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
							TypeDefinition.StringType,
#if FEATURE_TAP
							isAsync ? 
							TypeDefinition.GenericReferenceType(
								typeof( Func<,,,,,> ),
								TypeDefinition.UnpackerType,
								context.UnpackingContextType ?? this.TargetType,
								TypeDefinition.Int32Type,
								TypeDefinition.Int32Type,
								TypeDefinition.CancellationTokenType,
								TypeDefinition.TaskType
							) :
#endif // FEATURE_TAP
							TypeDefinition.GenericReferenceType(
								typeof( Action<,,,> ),
								TypeDefinition.UnpackerType,
								context.UnpackingContextType ?? this.TargetType,
								TypeDefinition.Int32Type,
								TypeDefinition.Int32Type
							)
						);
					name = FieldName.UnpackOperationTable;
					break;
				}
				case ActionType.UnpackTo:
				{
					type = 
#if FEATURE_TAP
						isAsync ? typeof( Func<,,,,> ).MakeGenericType( typeof( Unpacker ), this.TargetType, typeof( int ), typeof( CancellationToken ), typeof( Task ) ) :
#endif // FEATURE_TAP
						typeof( Action<,,> ).MakeGenericType( typeof( Unpacker ), this.TargetType, typeof( int ) );
					name = FieldName.UnpackTo;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "actionType" );
				}
			}

			if ( isAsync )
			{
				name += "Async";
			}

			var field = context.DeclarePrivateField( name, type );

			return
				CodeDomConstruct.Expression(
					type,
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						field.FieldName
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitGetMemberNamesExpression( CodeDomContext context )
		{
			var field = context.DeclarePrivateField( FieldName.MemberNames, TypeDefinition.IListOfStringType );

			return
				CodeDomConstruct.Expression(
					TypeDefinition.IListOfStringType,
					new CodeFieldReferenceExpression(
						new CodeThisReferenceExpression(),
						field.FieldName
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitFinishFieldInitializationStatement( CodeDomContext context, string name, CodeDomConstruct value )
		{
			return
				CodeDomConstruct.Statement(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(
							new CodeThisReferenceExpression(),
							name
						),
						value.AsExpression()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitConditionalExpression( CodeDomContext context, CodeDomConstruct conditionExpression, CodeDomConstruct thenExpression, CodeDomConstruct elseExpression )
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
				? CodeDomConstruct.Statement(
					new CodeConditionStatement(
						conditionExpression.AsExpression(),
						thenExpression.AsStatements().ToArray()
					)
				)
				: thenExpression.ContextType.TryGetRuntimeType() == typeof( void ) || thenExpression.IsStatement
				? CodeDomConstruct.Statement(
					new CodeConditionStatement(
						conditionExpression.AsExpression(),
						thenExpression.AsStatements().ToArray(),
						elseExpression.AsStatements().ToArray()
					)
				)
				: CreateConditionalExpression(
					context,
					conditionExpression,
					thenExpression,
					elseExpression
				);
		}

		private static CodeDomConstruct CreateConditionalExpression( CodeDomContext context, CodeDomConstruct conditionExpression, CodeDomConstruct thenExpression, CodeDomConstruct elseExpression )
		{
			context.IsConditionalExpressionUsed = true;
			return
				CodeDomConstruct.Expression(
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Validated internally" )]
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
							ToCodeTypeReference( enumerator.ContextType ),
							enumeratorName,
							new CodeMethodInvokeExpression(
								collection.AsExpression(),
								"GetEnumerator"
							)
						)
					),
					CodeDomConstruct.Statement( 
						new CodeVariableDeclarationStatement( 
							ToCodeTypeReference( current.ContextType ), 
							currentName
						)
					)
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
				this.EmitSequentialStatements(
					context,
					TypeDefinition.VoidType,
					statements.ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitEnumFromUnderlyingCastExpression(
			CodeDomContext context,
			Type enumType,
			CodeDomConstruct underlyingValue )
		{
			return CodeDomConstruct.Expression( enumType, new CodeCastExpression( enumType, underlyingValue.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitEnumToUnderlyingCastExpression(
			CodeDomContext context,
			Type underlyingType,
			CodeDomConstruct enumValue )
		{
			return CodeDomConstruct.Expression( underlyingType, new CodeCastExpression( underlyingType, enumValue.AsExpression() ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override CodeDomConstruct EmitNewPrivateMethodDelegateExpression( CodeDomContext context, MethodDefinition method )
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
				CodeDomConstruct.Expression(
					delegateType,
					new CodeDelegateCreateExpression(
						ToCodeTypeReference( delegateType ),
						method.IsStatic ? new CodeTypeReferenceExpression( context.DeclaringType.Name ) as CodeExpression : new CodeThisReferenceExpression(),
						method.MethodName
					)
				);
		}

		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			var asCodeDomContext = context as CodeDomContext;
			if ( asCodeDomContext == null )
			{
				throw new ArgumentException(
					"'context' was not created with CreateGenerationContextForCodeGeneration method.",
					"context"
				);
			}

			asCodeDomContext.Reset( this.TargetType, this.BaseClass );

			if ( !this.TargetType.GetIsEnum() )
			{
				SerializationTarget targetInfo;
				this.BuildSerializer( asCodeDomContext, concreteType, itemSchema, out targetInfo );
				this.Finish(
					asCodeDomContext,
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
				this.BuildEnumSerializer( asCodeDomContext );
				this.Finish( asCodeDomContext, null, true, ( SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) );
			}
		}

		protected override Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructor( CodeDomContext codeGenerationContext, SerializationTarget targetInfo, PolymorphismSchema schema, SerializerCapabilities? capabilities )
		{
#if DEBUG
			this.Finish( codeGenerationContext, targetInfo, false, capabilities );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );
			return targetType.GetConstructors().Single().CreateConstructorDelegate<Func<SerializationContext, MessagePackSerializer>>();
#else
			throw new NotSupportedException();
#endif // DEBUG
		}

		protected override Func<SerializationContext, MessagePackSerializer> CreateEnumSerializerConstructor( CodeDomContext codeGenerationContext )
		{
#if DEBUG
			this.Finish( codeGenerationContext, null, true, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );
			return targetType.GetConstructors().Single( c => c.GetParameters().Length == 1 ).CreateConstructorDelegate<Func<SerializationContext, MessagePackSerializer>>();
#else
			throw new NotSupportedException();
#endif // DEBUG
		}

#if DEBUG
#if !NET35
		[SecuritySafeCritical]
#endif // !NET35
		private static Type PrepareSerializerConstructorCreation( CodeDomContext codeGenerationContext )
		{
			if ( !SerializerDebugging.OnTheFlyCodeGenerationEnabled )
			{
				throw new NotSupportedException();
			}

			codeGenerationContext.Generate();
			Assembly assembly;
			IList<string> errors;
			IList<string> warnings;

			SerializerDebugging.CompileAssembly(
#if PERFORMANCE_TEST
				false,
#else
				true,
#endif // PERFORMANCE_TEST
				out assembly,
				out errors,
				out warnings
			);

			SerializerDebugging.ClearCodeBuffer();

			if ( errors.Any() )
			{
				if ( SerializerDebugging.TraceEnabled && !SerializerDebugging.DumpEnabled )
				{
					SerializerDebugging.ILTraceWriter.WriteLine( SerializerDebugging.CodeWriter.ToString() );
					SerializerDebugging.FlushTraceData();
				}

				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Failed to compile assembly. Details:{0}{1}",
						Environment.NewLine,
						String.Join( Environment.NewLine, errors.ToArray() )
					)
				);
			}
#if DEBUG
			// Check warning except ambigious type reference.
			Contract.Assert( !warnings.Any(), String.Join( Environment.NewLine, warnings.ToArray() ) );
#endif

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEmitEvent( "Build assembly '{0}' from dom.", assembly.ManifestModule.FullyQualifiedName );
			}

			var targetType =
				assembly.GetTypes()
					.SingleOrDefault(
						t =>
							t.Namespace == codeGenerationContext.Namespace
							&& t.Name == codeGenerationContext.DeclaringType.Name
					);

			Contract.Assert(
				targetType != null,
				String.Join(
					Environment.NewLine,
					assembly.GetTypes().Select( t => t.GetFullName() ).ToArray()
				)
			);
			return targetType;
		}
#endif // DEBUG

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
		private void Finish( CodeDomContext context, SerializationTarget targetInfo, bool isEnum, SerializerCapabilities? capabilities )
		{
			// ctor
			if ( isEnum )
			{
				var ctor1 =
					new CodeConstructor
					{
						Attributes = MemberAttributes.Public
					};
				ctor1.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
				ctor1.ChainedConstructorArgs.Add( new CodeArgumentReferenceExpression( "context" ) );
				ctor1.ChainedConstructorArgs.Add(
					new CodeFieldReferenceExpression(
						new CodeTypeReferenceExpression( typeof( EnumSerializationMethod ) ),
						EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
							context.SerializationContext,
							this.TargetType,
							EnumMemberSerializationMethod.Default
						).ToString()
					)
				);
				context.DeclaringType.Members.Add( ctor1 );

				var ctor2 =
					new CodeConstructor
					{
						Attributes = MemberAttributes.Public
					};
				ctor2.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
				ctor2.Parameters.Add( new CodeParameterDeclarationExpression( typeof( EnumSerializationMethod ), "enumSerializationMethod" ) );
				ctor2.BaseConstructorArgs.Add( new CodeArgumentReferenceExpression( "context" ) );
				ctor2.BaseConstructorArgs.Add( new CodeArgumentReferenceExpression( "enumSerializationMethod" ) );
				context.DeclaringType.Members.Add( ctor2 );
			}
			else
			{
				context.BeginConstructor();
				try
				{
					var ctor =
						new CodeConstructor
						{
							Attributes = MemberAttributes.Public
						};

					ctor.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
					var contextArgument = new CodeArgumentReferenceExpression( "context" );
					ctor.BaseConstructorArgs.Add( contextArgument );

					if ( this.BaseClass.GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance )
						.Any( c =>
							c.GetParameters()
							.Select( p => p.ParameterType )
							.SequenceEqual( CollectionSerializerHelpers.CollectionConstructorTypes ) 
						)
					)
					{
						ctor.BaseConstructorArgs.Add(
							new CodeMethodInvokeExpression(
								new CodeMethodReferenceExpression(
									new CodeTypeReferenceExpression( context.DeclaringType.Name ),
									MethodName.RestoreSchema
								)
							)
						);
					}

					if ( capabilities.HasValue )
					{
						var capabilitiesExpression = BuildCapabilitiesExpression( null, capabilities.Value, SerializerCapabilities.PackTo );
						capabilitiesExpression = BuildCapabilitiesExpression( capabilitiesExpression, capabilities.Value, SerializerCapabilities.UnpackFrom );
						capabilitiesExpression = BuildCapabilitiesExpression( capabilitiesExpression, capabilities.Value, SerializerCapabilities.UnpackTo );

						ctor.BaseConstructorArgs.Add(
							capabilitiesExpression
							?? new CodeFieldReferenceExpression( new CodeTypeReferenceExpression( typeof( SerializerCapabilities ) ), "None" )
						);
					}

					int schemaNumber = -1;
					foreach ( var dependentSerializer in context.GetDependentSerializers() )
					{
						var targetType = Type.GetTypeFromHandle( dependentSerializer.Key.TypeHandle );

						if ( targetType.GetIsEnum() )
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
											Metadata._EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethodMethod.Name,
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
						else if ( DateTimeMessagePackSerializerHelpers.IsDateTime( targetType ) )
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
											new CodeTypeReferenceExpression( typeof( DateTimeMessagePackSerializerHelpers ) ),
											Metadata._DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethodMethod.Name,
											contextArgument,
											new CodeFieldReferenceExpression(
												new CodeTypeReferenceExpression( typeof( DateTimeMemberConversionMethod ) ),
												dependentSerializer.Key.DateTimeConversionMethod.ToString()
											)
										)
									)
								)
							);
						}
						else
						{
							CodeExpression schemaExpression;
							if ( dependentSerializer.Key.PolymorphismSchema == null )
							{
								schemaExpression = new CodePrimitiveExpression( null );
							}
							else
							{
								schemaNumber++;
								var variableName = "schema" + schemaNumber;
								var schema = this.DeclareLocal( context, TypeDefinition.PolymorphismSchemaType, variableName );
								ctor.Statements.AddRange( schema.AsStatements().ToArray() );
								ctor.Statements.AddRange(
									this.EmitConstructPolymorphismSchema(
										context,
										schema,
										dependentSerializer.Key.PolymorphismSchema
									)
									// inner ToArray() is required for .net core app 2.0 LINQ
									.SelectMany( st => st.AsStatements().ToArray() )
									.ToArray()
								);

								schemaExpression = new CodeVariableReferenceExpression( variableName );
							}

							ctor.Statements.Add(
								new CodeAssignStatement(
									new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), dependentSerializer.Value ),
									new CodeMethodInvokeExpression(
										new CodeMethodReferenceExpression(
											contextArgument,
											"GetSerializer",
											new CodeTypeReference( targetType )
										),
										schemaExpression
									)
								)
							);
						}
					} // foreach ( in context.GetDependentSerializers() )

					foreach ( var cachedFieldInfo in context.GetCachedFieldInfos() )
					{
						var fieldInfo = cachedFieldInfo.Value.Target;
#if DEBUG
						Contract.Assert( fieldInfo != null, "fieldInfo != null" );
						Contract.Assert( fieldInfo.DeclaringType != null, "fieldInfo.DeclaringType != null" );
#endif // DEBUG
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedFieldInfo.Value.StorageFieldName ),
								new CodeMethodInvokeExpression(
									new CodeMethodReferenceExpression(
										new CodeTypeReferenceExpression( typeof( ReflectionHelpers ) ),
										"GetField"
									),
									new CodeTypeOfExpression( fieldInfo.DeclaringType ),
									new CodePrimitiveExpression( fieldInfo.Name )
								)
							)
						);
					} // foreach ( in context.GetCachedFieldInfos() )


					foreach ( var cachedMethodBase in context.GetCachedMethodBases() )
					{
						var methodBase = cachedMethodBase.Value.Target;
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedMethodBase.Value.StorageFieldName ),
								new CodeMethodInvokeExpression(
									new CodeMethodReferenceExpression(
										new CodeTypeReferenceExpression( typeof( ReflectionHelpers ) ), 
										"GetMethod"
									),
									// ReSharper disable once AssignNullToNotNullAttribute
									new CodeTypeOfExpression( methodBase.DeclaringType ),
									new CodePrimitiveExpression( methodBase.Name ),
									new CodeArrayCreateExpression(
										typeof( Type ),
										// ReSharper disable once CoVariantArrayConversion
										methodBase.GetParameters().Select( pi => new CodeTypeOfExpression( pi.ParameterType ) ).ToArray()
									)
								)
							)
						);
					} // foreach ( in context.GetCachedMethodBases() )

					if ( targetInfo != null && this.CollectionTraits.CollectionType == CollectionKind.NotCollection )
					{ 
						// For object only.
						if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType )
#if FEATURE_TAP
							|| !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType )
#endif // FEATURE_TAP
						)
						{

							if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
							{
								ctor.Statements.AddRange(
									this.EmitPackOperationListInitialization( context, targetInfo, false ).AsStatements().ToArray()
								);
							}
#if FEATURE_TAP
							if ( this.WithAsync( context ) && !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
							{
								ctor.Statements.AddRange(
									this.EmitPackOperationListInitialization( context, targetInfo, true ).AsStatements().ToArray()
								);
							}
#endif // FEATURE_TAP
							if ( targetInfo.Members.Any( m => m.Member != null ) ) // not Tuple
							{
								if ( !typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
								{
									ctor.Statements.AddRange(
										this.EmitPackOperationTableInitialization( context, targetInfo, false ).AsStatements().ToArray()
									);
								}
#if FEATURE_TAP
								if ( this.WithAsync( context ) && !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
								{
									ctor.Statements.AddRange(
										this.EmitPackOperationTableInitialization( context, targetInfo, true ).AsStatements().ToArray()
									);
								}
#endif // FEATURE_TAP

								if ( ( !typeof( IPackable ).IsAssignableFrom( this.TargetType )
#if FEATURE_TAP
									|| ( !typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) && this.WithAsync( context ) )
#endif // FEATURE_TAP
									)
#if DEBUG
									&& !SerializerDebugging.UseLegacyNullMapEntryHandling
#endif // DEBUG
								)
								{
									ctor.Statements.AddRange(
										this.EmitPackNullCheckerTableInitialization( context, targetInfo ).AsStatements().ToArray()
									);
								}
							}
						}

						if (
							targetInfo.CanDeserialize
							&& (
								!typeof( IUnpackable ).IsAssignableFrom( this.TargetType )
#if FEATURE_TAP
								|| !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType )
#endif // FEATURE_TAP
							)
						)
						{
							if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType ) )
							{
								ctor.Statements.AddRange(
									this.EmitUnpackOperationListInitialization( context, targetInfo, false ).AsStatements().ToArray()
								);
							}
#if FEATURE_TAP
							if ( this.WithAsync( context ) && !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
							{
								ctor.Statements.AddRange(
									this.EmitUnpackOperationListInitialization( context, targetInfo, true ).AsStatements().ToArray()
								);
							}
#endif // FEATURE_TAP

							if ( targetInfo.Members.Any( m => m.Member != null ) )
							{
								// Except tuples
								if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType ) )
								{
									ctor.Statements.AddRange(
										this.EmitUnpackOperationTableInitialization( context, targetInfo, false ).AsStatements().ToArray()
									);
								}
#if FEATURE_TAP
								if ( this.WithAsync( context ) && !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
								{
									ctor.Statements.AddRange(
										this.EmitUnpackOperationTableInitialization( context, targetInfo, true ).AsStatements().ToArray()
									);
								}
#endif // FEATURE_TAP
							}

							if ( !typeof( IUnpackable ).IsAssignableFrom( this.TargetType )
#if FEATURE_TAP
								|| ( !typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) && this.WithAsync( context ) )
#endif // FEATURE_TAP
							)
							{
								ctor.Statements.AddRange(
									this.EmitMemberListInitialization( context, targetInfo ).AsStatements().ToArray()
								);
							}
						}
					} // if( targetInfo != null )

					foreach ( var cachedDelegateInfo in context.GetCachedDelegateInfos() )
					{
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodeFieldReferenceExpression(
									new CodeThisReferenceExpression(),
									cachedDelegateInfo.BackingField.FieldName
								),
								new CodeDelegateCreateExpression(
									ToCodeTypeReference( cachedDelegateInfo.BackingField.FieldType ),
									cachedDelegateInfo.IsThisInstance
									? new CodeThisReferenceExpression() as CodeExpression
									: new CodeTypeReferenceExpression( ToCodeTypeReference( cachedDelegateInfo.TargetMethod.DeclaringType ) ),
									cachedDelegateInfo.TargetMethod.MethodName
								)
							)
						);
					}

					if ( context.IsUnpackToUsed )
					{
						ctor.Statements.AddRange( this.EmitUnpackToInitialization( context  ).AsStatements().ToArray() );
					}

					context.DeclaringType.Members.Add( ctor );
				}
				finally
				{
					context.EndConstructor();
				}
			} // else of isEnum

			// __Condition
			if ( context.IsConditionalExpressionUsed )
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

		private static CodeExpression BuildCapabilitiesExpression( CodeExpression expression, SerializerCapabilities capabilities, SerializerCapabilities value ) 
		{
			if ( ( capabilities & value ) != 0 )
			{
				var capabilityExpression =
					new CodeFieldReferenceExpression( new CodeTypeReferenceExpression( typeof( SerializerCapabilities ) ), value.ToString() );
				if ( expression == null )
				{
					return capabilityExpression;
				}
				else
				{
					return
						new CodeBinaryOperatorExpression(
							expression,
							CodeBinaryOperatorType.BitwiseOr,
							capabilityExpression
						);
				}
			}
			else
			{
				return expression;
			}
		}

		protected override CodeDomContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
#if DEBUG
			var result =
				new CodeDomContext(
					context,
					new SerializerCodeGenerationConfiguration
					{
						OutputDirectory = SerializerDebugging.DumpDirectory,
						CodeGenerationSink = CodeGenerationSink.ForSpecifiedTextWriter( SerializerDebugging.CodeWriter )
					}
				);
			result.Reset( this.TargetType, this.BaseClass );
			return result;
#else
			throw new NotSupportedException();
#endif // DEBUG
		}
	}
}
