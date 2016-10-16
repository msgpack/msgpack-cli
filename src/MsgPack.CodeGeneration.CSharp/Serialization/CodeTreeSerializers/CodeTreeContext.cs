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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
#if NETSTANDARD1_3
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETSTANDARD1_3
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
#if CSHARP
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using CodeTreeSerializerBuilder = MsgPack.Serialization.CodeTreeSerializers.CSharpCodeTreeSerializerBuilder;
#elif VISUAL_BASIC
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;
using CodeTreeSerializerBuilder = MsgPack.Serialization.CodeTreeSerializers.VisualBasicCodeTreeSerializerBuilder;
#endif

using MsgPack.Serialization.AbstractSerializers;
using static MsgPack.Serialization.CodeTreeSerializers.Syntax;
using static MsgPack.Serialization.CodeTreeSerializers.SyntaxCompatibilities;

#if VISUAL_BASIC
using AttributeArgumentSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.ArgumentSyntax;
using ClassDeclarationSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.ClassStatementSyntax;
using ConstructorDeclarationSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.SubNewStatementSyntax;
using MemberDeclarationSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.DeclarationStatementSyntax;
using TypeDeclarationSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.TypeBlockSyntax;
using UsingDirectiveSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.ImportsStatementSyntax;
#endif // VISUAL_BASIC

#warning Fix Samuel Cragg spreading

namespace MsgPack.Serialization.CodeTreeSerializers
{
	internal class CodeTreeContext : SerializerGenerationContext<CodeTreeConstruct>, ISerializerCodeGenerationContext
	{
#if VISUAL_BASIC

		private static ModifiedIdentifierSyntax Identifier( string identifier ) => ModifiedIdentifier( identifier );

#endif // VISUAL_BASIC

		private static readonly ParameterSyntax MessagePackObjectParameterSyntax = Parameter( Identifier( "messagePackObject" ) ).WithType( MessagePackObjectTypeSyntax );

		private static readonly ParameterSyntax InitialCapacityParameterSyntax = Parameter( Identifier( "initialCapacity" ) ).WithType( Int32TypeSyntax );

		private static readonly ParameterSyntax CancellationTokenParameterSyntax = Parameter( Identifier( "cancellationToken" ) ).WithType( CancellationTokenTypeSyntax );

		private static readonly SyntaxList<UsingDirectiveSyntax> KnownUsingDirectives =
			new SyntaxList<UsingDirectiveSyntax>().Add(
				UsingDirective( IdentifierName( "System" ) )
			).Add(
				UsingDirective( IdentifierName( "System.Collections.Generic" ) )
			).Add(
				UsingDirective( IdentifierName( "System.Threading" ) )
			).Add(
				UsingDirective( IdentifierName( "System.Threading.Tasks" ) )
			).Add(
				UsingDirective( IdentifierName( "MsgPack" ) ).WithLeadingTrivia( BlankLine )
			).Add(
				UsingDirective( IdentifierName( "MsgPack.Serialization" ) )
			);

		private static readonly SyntaxList<AttributeListSyntax> StandardTypeAttributes =
			new SyntaxList<AttributeListSyntax>().Add(
				AttributeList().AddAttributes(
					Attribute(
#if CSHARP
						IdentifierName( typeof( GeneratedCodeAttribute ).FullName ),
						AttributeArgumentList(
#elif VISUAL_BASIC
						Syntax.ToTypeSyntax( typeof( GeneratedCodeAttribute ) )
					).WithArgumentList(
						ArgumentList(
#endif
							new SeparatedSyntaxList<AttributeArgumentSyntax>().Add(
								AttributeArgument(
									LiteralExpression( SyntaxKind.StringLiteralExpression, Literal( typeof( CodeTreeSerializerBuilder ).FullName ) )
								)
							).Add(
								AttributeArgument(
									LiteralExpression( SyntaxKind.StringLiteralExpression, Literal( typeof( CodeTreeSerializerBuilder ).GetAssembly().GetName().Version.ToString() ) )
								)
							)
						)
					)
				)
			);

		private static readonly AttributeListSyntax DebuggerNonUserCodeAttributeSyntax =
			AttributeList().AddAttributes(
				Attribute(
					IdentifierName( typeof( DebuggerNonUserCodeAttribute ).FullName )
				)
			);

#if CSHARP
		private static readonly ClassDeclarationSyntax UnpackingContextTypeTemplate =
			ClassDeclaration( SerializerBuilderHelper.UnpackingContextTypeName );
#elif VISUAL_BASIC
		private static readonly ClassBlockSyntax UnpackingContextTypeTemplate =
			ClassBlock( ClassDeclaration( SerializerBuilderHelper.UnpackingContextTypeName ) );
#endif

		private static readonly ConstructorDeclarationSyntax UnpackingContextConstructorTemplate =
			ConstructorDeclaration( SerializerBuilderHelper.UnpackingContextTypeName, PublicKeyword );

		private static readonly CodeTreeConstruct SingletonPacker = CodeTreeConstruct.Parameter( typeof( Packer ), IdentifierName( "packer" ) );

		private static readonly CodeTreeConstruct SingletonUnpacker = CodeTreeConstruct.Parameter( typeof( Unpacker ), IdentifierName( "unpacker" ) );

		private static readonly CodeTreeConstruct SingletonIndexOfItem = CodeTreeConstruct.Parameter( typeof( int ), IdentifierName( "indexOfItem" ) );

		private static readonly CodeTreeConstruct SingletonItemsCount = CodeTreeConstruct.Parameter( typeof( int ), IdentifierName( "itemsCount" ) );

		private static readonly CodeTreeConstruct SingletonInitialCapacity = CodeTreeConstruct.Parameter( typeof( int ), IdentifierName( "initialCapacity" ) );


		private readonly Dictionary<SerializerFieldKey, string> _dependentSerializers;

		private readonly Dictionary<FieldInfo, CachedFieldInfo> _cachedTargetFields;

		private readonly Dictionary<MethodBase, CachedMethodBase> _cachedPropertyAccessors;

		private readonly Dictionary<Type, ClassDeclarationSyntaxBuilder> _declaringTypes;

		private readonly SerializerCodeGenerationConfiguration _configuration;

		private readonly string _extension;

		private Type _targetType;

		private bool IsDictionary => this.KeyToAdd != null;

		private ClassDeclarationSyntaxBuilder _buildingType;

		public string DeclaringTypeName => this._buildingType.Identifier.ValueText;

		private readonly Stack<MethodContext> _methodContextStack;

		/// <summary>
		///		Gets a value indicating whether the generated serializers will be internal to MsgPack library itself.
		/// </summary>
		/// <value>
		/// <c>true</c> if the generated serializers are internal to MsgPack library itself; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///		When you use MsgPack in Unity3D, you can import the library in source code form to your assets.
		///		And, you may also import generated serializers together, then the generated serializers and MsgPack library will be same assembly ultimately.
		///		It causes compilation error because some of overriding members have accessbility <c>FamilyOrAssembly</c>(<c>protected internal</c> in C#),
		///		so the generated source code must have the accessibility when and only when they will be same assembly as MsgPack library itself.
		/// </remarks>
		public bool IsInternalToMsgPackLibrary => this._configuration.IsInternalToMsgPackLibrary;

		public CodeTreeContext( SerializationContext context, SerializerCodeGenerationConfiguration configuration, string extension )
			: base( context )
		{
			this._configuration = configuration;
			this._extension = extension;
			this._methodContextStack = new Stack<MethodContext>();
			this._dependentSerializers = new Dictionary<SerializerFieldKey, string>();
			this._cachedTargetFields =
				new Dictionary<FieldInfo, CachedFieldInfo>( FieldInfoEqualityComparer.Instance );
			this._cachedPropertyAccessors =
				new Dictionary<MethodBase, CachedMethodBase>( MethodBaseEqualityComparer.Instance );
			this._declaringTypes = new Dictionary<Type, ClassDeclarationSyntaxBuilder>();
		}

		public string RegisterSerializer( Type targetType, EnumMemberSerializationMethod enumSerializationMethod, DateTimeMemberConversionMethod dateTimeConversionMethod, PolymorphismSchema polymorphismSchema )
		{
			var key = new SerializerFieldKey( targetType, enumSerializationMethod, dateTimeConversionMethod, polymorphismSchema );

			string fieldName;
			if ( !this._dependentSerializers.TryGetValue( key, out fieldName ) )
			{
				fieldName = "_serializer" + this._dependentSerializers.Count.ToString( CultureInfo.InvariantCulture );
				this._dependentSerializers.Add( key, fieldName );
				this._buildingType.AddMembers(
					FieldDeclaration(
						GenericName(
							"MessagePackSerializer",
							ToTypeSyntax( Type.GetTypeFromHandle( key.TypeHandle ) )
						),
						fieldName
					).WithModifiers( PrivateReadOnlyKeyword )
				);
			}

			return fieldName;
		}

		public Dictionary<SerializerFieldKey, String> GetDependentSerializers()
		{
			return this._dependentSerializers;
		}

		public string RegisterCachedFieldInfo( FieldInfo field )
		{
			CachedFieldInfo cachedField;
			if ( !this._cachedTargetFields.TryGetValue( field, out cachedField ) )
			{
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );

				cachedField =
					new CachedFieldInfo(
						field,
						"_field" + field.DeclaringType?.Name.Replace( '`', '_' ) + "_" + field.Name + this._cachedTargetFields.Count.ToString( CultureInfo.InvariantCulture )
					);
				this._cachedTargetFields.Add( field, cachedField );
				this._buildingType.AddMembers(
					FieldDeclaration( FieldInfoTypeSyntax, cachedField.StorageFieldName ).WithModifiers( PrivateReadOnlyKeyword )
				);
			}

			return cachedField.StorageFieldName;
		}

		protected override FieldDefinition DeclarePrivateFieldCore( string name, TypeDefinition type )
		{
			if ( !this._buildingType.ContainsField( name ) )
			{
				this._buildingType.AddMembers(
					FieldDeclaration( MethodInfoTypeSyntax, name ).WithModifiers( PrivateReadOnlyKeyword )
				);
			}

			return new FieldDefinition( null, name, type );
		}

		public Dictionary<FieldInfo, CachedFieldInfo> GetCachedFieldInfos()
		{
			return this._cachedTargetFields;
		}

		public string RegisterCachedMethodBase( MethodBase method )
		{
			CachedMethodBase cachedMethod;
			if ( !this._cachedPropertyAccessors.TryGetValue( method, out cachedMethod ) )
			{
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );

				cachedMethod =
					new CachedMethodBase(
						method,
						"_methodInfo" + method.DeclaringType?.Name.Replace( '`', '_' ) + "_" + method.Name + this._cachedPropertyAccessors.Count.ToString( CultureInfo.InvariantCulture )
					);
				this._cachedPropertyAccessors.Add( method, cachedMethod );
				this._buildingType.AddMembers(
					FieldDeclaration( MethodInfoTypeSyntax, cachedMethod.StorageFieldName ).WithModifiers( PrivateReadOnlyKeyword )
				);
			}

			return cachedMethod.StorageFieldName;
		}

		public Dictionary<MethodBase, CachedMethodBase> GetCachedMethodBases()
		{
			return this._cachedPropertyAccessors;
		}

		/// <summary>
		///		Gets a unique name of a local variable.
		/// </summary>
		/// <param name="prefix">The prefix of the variable.</param>
		/// <returns>A unique name of a local variable.</returns>
		public override string GetUniqueVariableName( string prefix )
		{
			var uniqueVariableSuffixes = this._methodContextStack.Peek().UniqueVariableSuffixes;
			int counter;
			if ( !uniqueVariableSuffixes.TryGetValue( prefix, out counter ) )
			{
				uniqueVariableSuffixes.Add( prefix, 0 );
				return prefix;
			}

			uniqueVariableSuffixes[ prefix ] = counter + 1;

			return prefix + counter.ToString( CultureInfo.InvariantCulture );
		}

		/// <summary>
		///		Resets internal states for new type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="baseClass">Type of base class of the target.</param>
		protected override void ResetCore( Type targetType, Type baseClass )
		{
			var attributes = StandardTypeAttributes;

			if ( !this._configuration.SuppressDebuggerNonUserCodeAttribute )
			{
				attributes = attributes.Add( DebuggerNonUserCodeAttributeSyntax );
			}

			this._targetType = targetType;
			this._dependentSerializers.Clear();
			this._cachedTargetFields.Clear();
			this._cachedPropertyAccessors.Clear();

			this._buildingType =
				new ClassDeclarationSyntaxBuilder(
					ClassDeclaration( IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" )
						.WithAttributeLists( attributes )
						.WithModifiers( PublicKeyword )
				).WithBaseList( baseClass );
			this._declaringTypes.Add( targetType, this._buildingType );

			this.PackToTarget = CodeTreeConstruct.Parameter( targetType, IdentifierName( "objectTree" ) );
			this.NullCheckTarget = this.PackToTarget;
			this.UnpackToTarget = CodeTreeConstruct.Parameter( targetType, IdentifierName( "collection" ) );
			this.Packer = SingletonPacker;
			this.Unpacker = SingletonUnpacker;
			this.IndexOfItem = SingletonIndexOfItem;
			this.ItemsCount = SingletonItemsCount;

			var traits = targetType.GetCollectionTraits( CollectionTraitOptions.Full, this.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = this.UnpackToTarget;
				this.ItemToAdd = CodeTreeConstruct.Parameter( traits.ElementType, IdentifierName( "item" ) );

				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
				)
				{
					this.KeyToAdd = CodeTreeConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 0 ], IdentifierName( "key" ) );
					this.ValueToAdd = CodeTreeConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 1 ], IdentifierName( "value" ) );
				}
				else
				{
					this.KeyToAdd = null;
					this.ValueToAdd = null;
				}

				this.InitialCapacity = SingletonInitialCapacity;
			}
		}

		public override void BeginMethodOverride( string name )
		{
			this._methodContextStack.Push( new MethodContext( name, false, typeof( object ), SerializerBuilderHelper.EmptyParameters ) );
		}

		public override void BeginPrivateMethod( string name, bool isStatic, TypeDefinition returnType, params CodeTreeConstruct[] parameters )
		{
			this._methodContextStack.Push(
				new MethodContext(
					name,
					isStatic,
					returnType,
					parameters
						.Select( p => new KeyValuePair<string, TypeDefinition>( p.AsParameter().GetIdentifierText(), p.ContextType ) )
						.ToArray()
				)
			);
		}

		protected override MethodDefinition EndMethodOverrideCore( string name, CodeTreeConstruct body )
		{

			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, "context.Name == name" );
#endif // DEBUG
			if ( body == null )
			{
				return null;
			}

			SyntaxTokenList modifiers;
			TypeSyntax returnType = VoidTypeSyntax;

			var parameters = new SeparatedSyntaxList<ParameterSyntax>();

			switch ( name )
			{
				case MethodName.PackToCore:
				{
					parameters =
						parameters.Add(
							this.Packer.AsParameter()
						).Add(
							this.PackToTarget.AsParameter()
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.UnpackFromCore:
				{
					parameters = parameters.Add( this.Unpacker.AsParameter() );
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.UnpackToCore:
				{
					returnType = ToTypeSyntax( this._targetType );
					parameters =
						parameters.Add(
							this.Unpacker.AsParameter()
						).Add(
							this.UnpackToTarget.AsParameter()
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.PackUnderlyingValueTo:
				{
					parameters =
						parameters.Add(
							this.Packer.AsParameter()
						).Add(
							Parameter( Identifier( "enumValue" ) ).WithType( ToTypeSyntax( this._targetType ) )
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.UnpackFromUnderlyingValue:
				{
					returnType = ToTypeSyntax( this._targetType );
					parameters = parameters.Add( MessagePackObjectParameterSyntax );
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;

					break;
				}
				case MethodName.AddItem:
				{
					parameters =
						parameters.Add(
							Parameter( Identifier( "collection" ) ).WithType( ToTypeSyntax( this._targetType ) )
						);
					if ( this.IsDictionary )
					{
						parameters =
							parameters.Add(
								Parameter( Identifier( "key" ) ).WithType( ToTypeSyntax( this.KeyToAdd.ContextType ) )
							).Add(
								Parameter( Identifier( "value" ) ).WithType( ToTypeSyntax( this.ValueToAdd.ContextType ) )
							);
					}
					else
					{
						parameters =
							parameters.Add(
								Parameter( Identifier( "item" ) ).WithType( ToTypeSyntax( this.ItemToAdd.ContextType ) )
							);
					}

					modifiers = ProtectedOverrideKeyword;
					break;
				}
				case MethodName.CreateInstance:
				{
					returnType = ToTypeSyntax( this._targetType );
					parameters = parameters.Add( InitialCapacityParameterSyntax );
					modifiers = ProtectedOverrideKeyword;
					break;
				}
				case MethodName.PackToAsyncCore:
				{
					returnType = TaskTypeSyntax;
					parameters =
						parameters.Add(
							this.Packer.AsParameter()
						).Add(
							this.PackToTarget.AsParameter()
						).Add(
							CancellationTokenParameterSyntax
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.UnpackFromAsyncCore:
				{
					returnType =
						GenericName(
							typeof( Task ).Name,
							ToTypeSyntax( this._targetType )
						);
					parameters =
						parameters.Add(
							this.Unpacker.AsParameter()
						).Add(
							CancellationTokenParameterSyntax
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.UnpackToAsyncCore:
				{
					returnType = TaskTypeSyntax;
					parameters =
						parameters.Add(
							this.Unpacker.AsParameter()
						).Add(
							this.UnpackToTarget.AsParameter()
						).Add(
							CancellationTokenParameterSyntax
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				case MethodName.PackUnderlyingValueToAsync:
				{
					returnType = TaskTypeSyntax;
					parameters =
						parameters.Add(
							this.Packer.AsParameter()
						).Add(
							Parameter( Identifier( "enumValue" ) ).WithType( ToTypeSyntax( this._targetType ) )
						).Add(
							CancellationTokenParameterSyntax
						);
					modifiers = this.IsInternalToMsgPackLibrary ? ProtectedInternalOverrideKeyword : ProtectedOverrideKeyword;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( nameof( name ), name );
				}
			}

			this._buildingType.AddMembers(
				MethodDeclaration( returnType, name )
				.WithModifiers( modifiers )
				.WithParameterList( ParameterList( parameters ) )
				.WithBody( Block( body.AsStatements() ) )
			);

			return
				new MethodDefinition(
					context.Name,
					null,
					null,
					context.IsStatic,
					context.ReturnType,
					context.Parameters.Select( kv => kv.Value ).ToArray()
				);
		}

		protected override MethodDefinition EndPrivateMethodCore( string name, CodeTreeConstruct body )
		{
			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, "context.Name == name" );
#endif // DEBUG
			if ( body == null )
			{
				return null;
			}

			this._buildingType.AddMembers(
					MethodDeclaration(
						context.ReturnType.TryGetRuntimeType() != typeof( void )
							? ToTypeSyntax( context.ReturnType )
							: VoidTypeSyntax,
						context.Name
					).WithModifiers(
						context.IsStatic ? PrivateStaticKeyword : PrivateInstanceKeyword
					).WithParameterList(
						ParameterList(
							new SeparatedSyntaxList<ParameterSyntax>().AddRange(
								context.Parameters.Select( kv =>
									Parameter( Identifier( kv.Key ) ).WithType( ToTypeSyntax( kv.Value ) )
								)
							)
						)
					).WithBody( Block( body.AsStatements() ) )
				);

			return
				new MethodDefinition(
					context.Name,
					null,
					null,
					context.IsStatic,
					context.ReturnType,
					context.Parameters.Select( kv => kv.Value ).ToArray()
				);
		}

		// For stack
		public void BeginConstructor()
		{
			this._methodContextStack.Push( new MethodContext( ".ctor", false, typeof( object ), SerializerBuilderHelper.EmptyParameters ) );
		}

		public void EndConstructor()
		{
			this._methodContextStack.Pop();
		}

		public void AddMember( MemberDeclarationSyntax member )
		{
			this._buildingType.AddMembers( member );
		}

		protected override void DefineUnpackingContextCore(
			IList<KeyValuePair<string, TypeDefinition>> fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor,
			out CodeTreeConstruct parameterInUnpackValueMethods,
			out CodeTreeConstruct parameterInSetValueMethods,
			out CodeTreeConstruct parameterInCreateObjectFromContext
		)
		{
			var fieldDeclarations = new SyntaxList<MemberDeclarationSyntax>();
			var parameters = new SeparatedSyntaxList<ParameterSyntax>();
			var statements = new SyntaxList<StatementSyntax>();
			foreach ( var kv in fields )
			{
				var fieldTypeSyntax = ToTypeSyntax( kv.Value );
				fieldDeclarations =
					fieldDeclarations.Add(
						FieldDeclaration( fieldTypeSyntax, kv.Key ).WithModifiers( PublicKeyword )
					);

				parameters =
					parameters.Add(
						Parameter( Identifier( kv.Key ) ).WithType( fieldTypeSyntax )
					);

				statements =
					statements.Add(
						SimpleAssignmentStatement(
							SimpleMemberAccessExpression(
								ThisExpression(),
								IdentifierName( kv.Key )
							),
							IdentifierName( kv.Key )
						)
					);
			}

			this._buildingType
					.WithMembers( fieldDeclarations )
					.AddMembers(
						UnpackingContextTypeTemplate
							.AddMembers(
								UnpackingContextConstructorTemplate
									.WithParameterList( ParameterList( parameters ) )
									.WithBody( Block( statements ) )
							)
					);
			type = TypeDefinition.Object( SerializerBuilderHelper.UnpackingContextTypeName );
			constructor = new ConstructorDefinition( type, fields.Select( kv => kv.Value ).ToArray() );
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		protected override void DefineUnpackingContextWithResultObjectCore(
			out TypeDefinition type,
			out CodeTreeConstruct parameterInUnpackValueMethods,
			out CodeTreeConstruct parameterInSetValueMethods,
			out CodeTreeConstruct parameterInCreateObjectFromContext
		)
		{
			type = TypeDefinition.Object( this._targetType );
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		private static void DefineUnpackValueMethodArguments( TypeDefinition type, out CodeTreeConstruct parameterInUnpackValueMethods, out CodeTreeConstruct parameterInSetValueMethods, out CodeTreeConstruct parameterInCreateObjectFromContext )
		{
			parameterInUnpackValueMethods = CodeTreeConstruct.Parameter( type, IdentifierName( "unpackingContext" ) );
			parameterInSetValueMethods = CodeTreeConstruct.Parameter( type, IdentifierName( "unpackingContext" ) );
			parameterInCreateObjectFromContext = CodeTreeConstruct.Parameter( type, IdentifierName( "unpackingContext" ) );
		}

		public override CodeTreeConstruct DefineUnpackedItemParameterInSetValueMethods( TypeDefinition itemType )
		{
			return CodeTreeConstruct.Parameter( itemType, IdentifierName( "unpackedValue" ) );
		}

		public IEnumerable<SerializerCodeGenerationResult> Generate()
		{
#warning TODO: indentation

			var directory =
					Path.Combine(
						this._configuration.OutputDirectory,
						this._configuration.Namespace.Replace( ReflectionAbstractions.TypeDelimiter, Path.DirectorySeparatorChar )
					);
			Directory.CreateDirectory( directory );

			var result = new List<SerializerCodeGenerationResult>( this._declaringTypes.Count );

			foreach ( var kv in this._declaringTypes )
			{
				var declaringType = kv.Value.Build();
				var declaringTypeIdentifier =
#if CSHARP
					declaringType.Identifier;
#elif VISUAL_BASIC
					declaringType.ClassStatement.Identifier;
#endif
				var typeFileName = declaringTypeIdentifier.ValueText;
				var genericArity =
#if CSHARP
					declaringType.TypeParameterList?.Parameters.Count;
#elif VISUAL_BASIC
					declaringType.ClassStatement.TypeParameterList?.Parameters.Count;
#endif
				if ( genericArity != null )
				{
					typeFileName += "`" + genericArity.Value.ToString( "D", CultureInfo.InvariantCulture );
				}

				typeFileName += "." + this._extension;

				var compilationUnit = this.CreateCompilationUnit( declaringType );
				var filePath = Path.Combine( directory, typeFileName );
				result.Add(
					new SerializerCodeGenerationResult(
						kv.Key,
						filePath,
						String.IsNullOrEmpty( this._configuration.Namespace )
						? declaringTypeIdentifier.ValueText
						: this._configuration.Namespace + "." + declaringTypeIdentifier.ValueText,
						this._configuration.Namespace,
						declaringTypeIdentifier.ValueText
					)
				);

				using ( var fileStream = new FileStream( filePath, FileMode.Create, FileAccess.Write, FileShare.Read ) )
				using ( var writer = new StreamWriter( fileStream, Encoding.UTF8 ) )
				{
					compilationUnit.WriteTo( writer );
				}
			}

			return result;
		}

		private CompilationUnitSyntax CreateCompilationUnit( TypeDeclarationSyntax targetType )
			=> CompilationUnit()
			.WithUsings( KnownUsingDirectives )
			.AddMembers(
				NamespaceDeclaration(
					IdentifierName( this._configuration.Namespace )
				).WithLeadingTrivia( BlankLine )
				.WithMembers(
#if CSHARP
					new SyntaxList<MemberDeclarationSyntax>()
#elif VISUAL_BASIC
					new SyntaxList<StatementSyntax>()
#endif
					.Add(
						targetType
					)
				)
			);

		public CompilationUnitSyntax CreateCompilationUnit()
			=> this.CreateCompilationUnit( this._buildingType.Build() );

		public struct CachedFieldInfo
		{
			public readonly string StorageFieldName;
			public readonly FieldInfo Target;

			public CachedFieldInfo( FieldInfo target, string storageFieldName )
			{
				this.Target = target;
				this.StorageFieldName = storageFieldName;
			}
		}

		public struct CachedMethodBase
		{
			public readonly string StorageFieldName;
			public readonly MethodBase Target;

			public CachedMethodBase( MethodBase target, string storageFieldName )
			{
				this.Target = target;
				this.StorageFieldName = storageFieldName;
			}
		}

		private sealed class MethodContext
		{
			public readonly IDictionary<string, int> UniqueVariableSuffixes;

			public readonly string Name;

			public readonly bool IsStatic;

			public readonly TypeDefinition ReturnType;

			public readonly KeyValuePair<string, TypeDefinition>[] Parameters;

			public MethodContext( string name, bool isStatic, TypeDefinition returnType, KeyValuePair<string, TypeDefinition>[] parameters )
			{
				this.Name = name;
				this.IsStatic = isStatic;
				this.ReturnType = returnType;
				this.Parameters = parameters;
				this.UniqueVariableSuffixes = new Dictionary<string, int>();
			}
		}

		private sealed class ClassDeclarationSyntaxBuilder
		{
#if CSHARP
			private ClassDeclarationSyntax _syntax;

			public SyntaxToken Identifier => this._syntax.Identifier;

#elif VISUAL_BASIC

			private ClassBlockSyntax _syntax;

			public SyntaxToken Identifier => this._syntax.ClassStatement.Identifier;

#endif

			public ClassDeclarationSyntaxBuilder( ClassDeclarationSyntax syntax )
			{
#if CSHARP
				this._syntax = syntax;
#elif VISUAL_BASIC
				this._syntax = ClassBlock( syntax );
#endif
			}

			public ClassDeclarationSyntaxBuilder WithBaseList( TypeDefinition baseClass )
			{
				this._syntax =
#if CSHARP
					this._syntax.WithBaseList(
						BaseList(
							new SeparatedSyntaxList<BaseTypeSyntax>().Add(
								SimpleBaseType( ToTypeSyntax( baseClass ) )
							)
						)
					);
#elif VISUAL_BASIC
					this._syntax.AddInherits(
						InheritsStatement( ToTypeSyntax( baseClass ) )
					);
#endif
				return this;
			}

			public ClassDeclarationSyntaxBuilder WithMembers( SyntaxList<MemberDeclarationSyntax> members )
			{
#if CSHARP
				this._syntax = this._syntax.WithMembers( members );
#elif VISUAL_BASIC
				this._syntax = this._syntax.WithMembers( new SyntaxList<StatementSyntax>().AddRange( members ) );
#endif
				return this;
			}

			// ReSharper disable once UnusedMethodReturnValue.Local
			public ClassDeclarationSyntaxBuilder AddMembers( params MemberDeclarationSyntax[] items )
			{
				this._syntax = this._syntax.AddMembers( items );
				return this;
			}

			public bool ContainsField( string name )
				=> this._syntax.Members.OfType<FieldDeclarationSyntax>()
#if CSHARP
					.Any( x => x.Declaration.Variables.Any( v => v.Identifier.ValueText == name ) );
#elif VISUAL_BASIC
					.Any( x => x.Declarators.Any( d => d.Names.Any( n => n.Identifier.ValueText == name ) ) );
#endif

#if CSHARP
			public ClassDeclarationSyntax Build() => this._syntax;
#elif VISUAL_BASIC
			public ClassBlockSyntax Build() => this._syntax;
#endif
		}
	}
}

