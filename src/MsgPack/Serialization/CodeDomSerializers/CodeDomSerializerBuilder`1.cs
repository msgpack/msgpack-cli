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
using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	/// <summary>
	///		Code DOM based implementation of <see cref="SerializerBuilder{TContext,TConstruct,TObject}"/>.
	///		This type supports pre-generation.
	/// </summary>
	/// <typeparam name="TObject">Serialization target type.</typeparam>
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
		protected override void EmitMethodPrologue( CodeDomContext context, CollectionSerializerMethod method, MethodInfo declaration )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override void EmitMethodEpilogue( CodeDomContext context, CollectionSerializerMethod method, CodeDomConstruct construct )
		{
			if ( construct == null )
			{
				return;
			}
			CodeMemberMethod codeMethod;
			switch ( method )
			{
				case CollectionSerializerMethod.AddItem:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "AddItem",
						};
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( TObject ), "collection" ) );
					if ( CollectionTraitsOfThis.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_35 && !NETFX_40
						|| CollectionTraitsOfThis.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_35 && !NETFX_40 
					)
					{
						codeMethod.Parameters.Add(
							new CodeParameterDeclarationExpression( CollectionTraitsOfThis.ElementType.GetGenericArguments()[ 0 ], "key" ) 
						);
						codeMethod.Parameters.Add(
							new CodeParameterDeclarationExpression( CollectionTraitsOfThis.ElementType.GetGenericArguments()[ 1 ], "value" ) 
						);
					}
					else
					{
						codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( CollectionTraitsOfThis.ElementType, "item" ) );
					}
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
					break;
				}
				case CollectionSerializerMethod.CreateInstance:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "CreateInstance",
							ReturnType = new CodeTypeReference( typeof( TObject ) )
						};
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( int ), "initialCapacity" ) );

					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
					break;
				}
				case CollectionSerializerMethod.RestoreSchema:
				{
					codeMethod =
						new CodeMemberMethod
						{
							Name = "RestoreSchema",
							ReturnType = new CodeTypeReference( typeof( PolymorphismSchema ) )
						};

					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = MemberAttributes.Private | MemberAttributes.Static;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method" );
				}
			}

			codeMethod.Statements.AddRange( construct.AsStatements().ToArray() );

			context.DeclaringType.Members.Add( codeMethod );
		}

		protected override CodeDomConstruct MakeNullLiteral( CodeDomContext context, Type contextType )
		{
			return CodeDomConstruct.Expression( contextType, new CodePrimitiveExpression( null ) );
		}

		protected override CodeDomConstruct MakeByteLiteral( CodeDomContext context, byte constant )
		{
			return CodeDomConstruct.Expression( typeof( byte ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeSByteLiteral( CodeDomContext context, sbyte constant )
		{
			return CodeDomConstruct.Expression( typeof( sbyte ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt16Literal( CodeDomContext context, short constant )
		{
			return CodeDomConstruct.Expression( typeof( short ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt16Literal( CodeDomContext context, ushort constant )
		{
			return CodeDomConstruct.Expression( typeof( ushort ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt32Literal( CodeDomContext context, int constant )
		{
			return CodeDomConstruct.Expression( typeof( int ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt32Literal( CodeDomContext context, uint constant )
		{
			return CodeDomConstruct.Expression( typeof( uint ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeInt64Literal( CodeDomContext context, long constant )
		{
			return CodeDomConstruct.Expression( typeof( long ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeUInt64Literal( CodeDomContext context, ulong constant )
		{
			return CodeDomConstruct.Expression( typeof( ulong ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeReal32Literal( CodeDomContext context, float constant )
		{
			return CodeDomConstruct.Expression( typeof( float ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeReal64Literal( CodeDomContext context, double constant )
		{
			return CodeDomConstruct.Expression( typeof( double ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeBooleanLiteral( CodeDomContext context, bool constant )
		{
			return CodeDomConstruct.Expression( typeof( bool ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeCharLiteral( CodeDomContext context, char constant )
		{
			return CodeDomConstruct.Expression( typeof( char ), new CodePrimitiveExpression( constant ) );
		}

		protected override CodeDomConstruct MakeEnumLiteral( CodeDomContext context, Type type, object constant )
		{
			var asString = constant.ToString();
			if ( ( '0' <= asString[ 0 ] && asString[ 0 ] <= '9' ) || asString.Contains( ',' ) )
			{
				// Unrepresentable numeric or combined flags
				return
					CodeDomConstruct.Expression(
						type,
						new CodeCastExpression(
							type,
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
							new CodeTypeReferenceExpression( type ), constant.ToString() 
						)
					);
			}
		}

		protected override CodeDomConstruct MakeDefaultLiteral( CodeDomContext context, Type type )
		{
			return CodeDomConstruct.Expression( type, new CodeDefaultValueExpression( new CodeTypeReference( type ) ) );
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
							new CodeMethodReferenceExpression(
								instance == null
								? new CodeTypeReferenceExpression( method.DeclaringType )
								: instance.AsExpression(),
								method.Name,
								method.IsGenericMethod
								? method.GetGenericArguments().Select( t => new CodeTypeReference( t ) ).ToArray()
								: CodeDomSerializerBuilder.EmptyGenericArguments
							),
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
						new CodeMethodReferenceExpression(
							instance == null
							? new CodeTypeReferenceExpression( method.DeclaringType )
							: instance.AsExpression(),
							method.Name,
							method.IsGenericMethod
							? method.GetGenericArguments().Select( t => new CodeTypeReference(t)).ToArray()
							: CodeDomSerializerBuilder.EmptyGenericArguments
						),
						arguments.Select( a => a.AsExpression() ).ToArray()
					)
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
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
		protected override CodeDomConstruct EmitCreateNewArrayExpression( CodeDomContext context, Type elementType, int length )
		{
			return
				CodeDomConstruct.Expression(
					elementType.MakeArrayType(),
					new CodeArrayCreateExpression(
						elementType,
						new CodePrimitiveExpression( length )
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override CodeDomConstruct EmitGetSerializerExpression( CodeDomContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
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
								: memberInfo.Value.GetEnumMemberSerializationMethod(),
							memberInfo == null
								? DateTimeMemberConversionMethod.Default
								: memberInfo.Value.GetDateTimeMemberConversionMethod(),
							itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo )
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

		protected override CodeDomConstruct EmitStringSwitchStatement( CodeDomContext context, CodeDomConstruct target, IDictionary<string, CodeDomConstruct> cases, CodeDomConstruct defaultCase )
		{
#if DEBUG
			Contract.Assert( target.IsExpression );
			Contract.Assert( defaultCase.IsStatement );
			Contract.Assert( cases.Values.All( c => c.IsStatement ) );
#endif

			var statements = cases.Aggregate<KeyValuePair<string, CodeDomConstruct>, CodeConditionStatement>(
				null,
				( current, caseStatement ) =>
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						target.AsExpression(),
						CodeBinaryOperatorType.ValueEquality,
						new CodePrimitiveExpression( caseStatement.Key )
					),
					caseStatement.Value.AsStatements().ToArray(),
					current == null ? defaultCase.AsStatements().ToArray() : new CodeStatement[] { current }
				)
			);

			return CodeDomConstruct.Statement( statements );
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
				this.EmitSequentialStatements(
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

			asCodeDomContext.Reset( typeof( TObject ), BaseClass );

			if ( !typeof( TObject ).GetIsEnum() )
			{
				this.BuildSerializer( asCodeDomContext, concreteType, itemSchema );
			}
			else
			{
				this.BuildEnumSerializer( asCodeDomContext );
			}

			this.Finish( asCodeDomContext, typeof( TObject ).GetIsEnum() );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( CodeDomContext codeGenerationContext, PolymorphismSchema schema )
		{
			this.Finish( codeGenerationContext, false );
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
			this.Finish( codeGenerationContext, true );
			var targetType = PrepareSerializerConstructorCreation( codeGenerationContext );

			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<TObject>>>(
					Expression.New( targetType.GetConstructors().Single( c => c.GetParameters().Length == 1 ), contextParameter ),
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
		private void Finish( CodeDomContext context, bool isEnum )
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
						cachedFieldInfo.Value.StorageFieldName
					)
				);
			}


			foreach ( var cachedMethodBase in context.GetCachedMethodBases() )
			{
				context.DeclaringType.Members.Add(
					new CodeMemberField(
						typeof( MethodBase ),
						cachedMethodBase.Value.StorageFieldName
					)
				);
			}


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
							typeof( TObject ),
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
				var ctor =
					new CodeConstructor
					{
						Attributes = MemberAttributes.Public
					};

				ctor.Parameters.Add( new CodeParameterDeclarationExpression( typeof( SerializationContext ), "context" ) );
				var contextArgument = new CodeArgumentReferenceExpression( "context" );
				ctor.BaseConstructorArgs.Add( contextArgument );

				if (
					BaseClass.GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance )
						.Any( c => c.GetParameters().Select( p => p.ParameterType ).SequenceEqual( CollectionSerializerHelpers.CollectionConstructorTypes ) )
					)
				{
					ctor.BaseConstructorArgs.Add(
						new CodeMethodInvokeExpression(
							new CodeMethodReferenceExpression(
								new CodeTypeReferenceExpression( context.DeclaringType.Name ),
								"RestoreSchema"
							)
						)
					);

#if DEBUG
					Contract.Assert( context.GetDependentSerializers().Count == 0, "Dependent serializers are found in collection serializer." );
#endif // DEBUG
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
							var schema = this.DeclareLocal( context, typeof( PolymorphismSchema ), variableName );
							ctor.Statements.AddRange( schema.AsStatements().ToArray() );
							ctor.Statements.AddRange(
								this.EmitConstructPolymorphismSchema(
									context,
									schema,
									dependentSerializer.Key.PolymorphismSchema
								).SelectMany( st => st.AsStatements() ).ToArray()
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
				}

				foreach ( var cachedFieldInfo in context.GetCachedFieldInfos() )
				{
					var fieldInfo = cachedFieldInfo.Value.Target;
					ctor.Statements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedFieldInfo.Value.StorageFieldName ),
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
					var methodBase = cachedMethodBase.Value.Target;
					ctor.Statements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), cachedMethodBase.Value.StorageFieldName ),
							new CodeMethodInvokeExpression(
								new CodeMethodReferenceExpression(
									// ReSharper disable once AssignNullToNotNullAttribute
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
									// ReSharper disable once CoVariantArrayConversion
									methodBase.GetParameters().Select( pi => new CodeTypeOfExpression( pi.ParameterType ) ).ToArray()
								),
								new CodePrimitiveExpression( null )
							)
						)
					);
				}
				context.DeclaringType.Members.Add( ctor );
			} // else of isEnum

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
			result.Reset( typeof( TObject ), BaseClass );
			return result;
		}
	}
}