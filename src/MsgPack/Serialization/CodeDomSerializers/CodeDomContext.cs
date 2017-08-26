#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
	internal class CodeDomContext : SerializerGenerationContext<CodeDomConstruct>, ISerializerCodeGenerationContext
	{
		public static readonly CodeCatchClause[] EmptyCatches = new CodeCatchClause[ 0 ];

		public const string ConditionalExpressionHelperMethodName = "__Conditional";
		public const string ConditionalExpressionHelperConditionParameterName = "condition";
		public const string ConditionalExpressionHelperWhenTrueParameterName = "whenTrue";
		public const string ConditionalExpressionHelperWhenFalseParameterName = "whenFalse";

		private readonly Dictionary<SerializerFieldKey, string> _dependentSerializers = new Dictionary<SerializerFieldKey, string>();
		private readonly Dictionary<RuntimeFieldHandle, CachedFieldInfo> _cachedTargetFields =
			new Dictionary<RuntimeFieldHandle, CachedFieldInfo>();
		private readonly Dictionary<RuntimeMethodHandle, CachedMethodBase> _cachedPropertyAccessors =
			new Dictionary<RuntimeMethodHandle, CachedMethodBase>();

		private readonly Dictionary<Type, CodeTypeDeclaration> _declaringTypes = new Dictionary<Type, CodeTypeDeclaration>();

		private readonly SerializerCodeGenerationConfiguration _configuration;

#if DEBUG
		internal string Namespace
		{
			get { return this._configuration.Namespace; }
		}
#endif // DEBUG

		private Type _targetType;

		private bool IsDictionary
		{
			get { return this.KeyToAdd != null; }
		}

		private CodeTypeDeclaration _buildingType;

		/// <summary>
		///		Gets the current <see cref="CodeTypeDeclaration"/>.
		/// </summary>
		/// <value>
		///		The current <see cref="CodeTypeDeclaration"/>.
		/// </value>
		public CodeTypeDeclaration DeclaringType
		{
			get { return this._buildingType; }
		}

		private readonly Stack<MethodContext> _methodContextStack;

		public CodeDomContext( SerializationContext context, SerializerCodeGenerationConfiguration configuration )
			: base( context )
		{
			this._configuration = configuration;
			this._methodContextStack = new Stack<MethodContext>();
		}

		public string RegisterSerializer( Type targetType, EnumMemberSerializationMethod enumSerializationMethod, DateTimeMemberConversionMethod dateTimeConversionMethod, PolymorphismSchema polymorphismSchema )
		{
			var key = new SerializerFieldKey( targetType, enumSerializationMethod, dateTimeConversionMethod, polymorphismSchema );

			string fieldName;
			if ( !this._dependentSerializers.TryGetValue( key, out fieldName ) )
			{
				fieldName = "_serializer" + this._dependentSerializers.Count.ToString( CultureInfo.InvariantCulture );
				this._dependentSerializers.Add( key, fieldName );
				this._buildingType.Members.Add(
					new CodeMemberField(
						typeof( MessagePackSerializer<> ).MakeGenericType( Type.GetTypeFromHandle( key.TypeHandle ) ),
						fieldName
					)
					{
						Attributes = MemberAttributes.Private
					}
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
			var key = field.FieldHandle;
			CachedFieldInfo cachedField;
			if ( !this._cachedTargetFields.TryGetValue( key, out cachedField ) )
			{
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );

				cachedField =
					new CachedFieldInfo(
						field,
						"_field" + field.DeclaringType.Name.Replace( '`', '_' ) + "_" + field.Name + this._cachedTargetFields.Count.ToString( CultureInfo.InvariantCulture )
					);
				this._cachedTargetFields.Add( key, cachedField );
				this._buildingType.Members.Add(
					new CodeMemberField(
						typeof( FieldInfo ),
						cachedField.StorageFieldName
					)
				);
			}

			return cachedField.StorageFieldName;
		}

		// For the field identified by its name.
		protected override FieldDefinition DeclarePrivateFieldCore( string name, TypeDefinition type )
		{
			var field = this._buildingType.Members.OfType<CodeMemberField>().SingleOrDefault( f => f.Name == name );
			if ( field == null )
			{
				this._buildingType.Members.Add(
					new CodeMemberField( CodeDomSerializerBuilder.ToCodeTypeReference( type ), name )
					{
						Attributes = MemberAttributes.Private
					}
				);
			}

			return new FieldDefinition( null, name, type );
		}

		public Dictionary<RuntimeFieldHandle, CachedFieldInfo> GetCachedFieldInfos()
		{
			return this._cachedTargetFields;
		}

		public string RegisterCachedMethodBase( MethodBase method )
		{
			var key = method.MethodHandle;
			CachedMethodBase cachedMethod;
			if ( !this._cachedPropertyAccessors.TryGetValue( key, out cachedMethod ) )
			{
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );

				cachedMethod =
					new CachedMethodBase(
						method,
						"_methodBase" + method.DeclaringType.Name.Replace( '`', '_' ) + "_" + method.Name + this._cachedPropertyAccessors.Count.ToString( CultureInfo.InvariantCulture )
					);
				this._cachedPropertyAccessors.Add( key, cachedMethod );
				this._buildingType.Members.Add(
					new CodeMemberField(
						typeof( MethodBase ),
						cachedMethod.StorageFieldName
					)
				);
			}

			return cachedMethod.StorageFieldName;
		}

		public Dictionary<RuntimeMethodHandle, CachedMethodBase> GetCachedMethodBases()
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
		public bool IsInternalToMsgPackLibrary { get { return this._configuration.IsInternalToMsgPackLibrary; } }

		/// <summary>
		///		Gets or sets a value indicating whether conditional expression is used, that is, helper method is required or not.
		/// </summary>
		/// <value>
		/// <c>true</c> if conditional expression is used; otherwise, <c>false</c>.
		/// </value>
		public bool IsConditionalExpressionUsed { get; set; }

		/// <summary>
		///		Resets internal states for new type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="baseClass">Type of base class of the target.</param>
		protected override void ResetCore( Type targetType, Type baseClass )
		{
			var declaringType = new CodeTypeDeclaration( IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" );
			declaringType.BaseTypes.Add( baseClass );
			declaringType.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeTypeReference( typeof( GeneratedCodeAttribute ) ),
					new CodeAttributeArgument( new CodePrimitiveExpression( "MsgPack.Serialization.CodeDomSerializers.CodeDomSerializerBuilder" ) ),
					new CodeAttributeArgument( new CodePrimitiveExpression( this.GetType().Assembly.GetName().Version.ToString() ) )
				)
			);
			declaringType.CustomAttributes.Add(
				new CodeAttributeDeclaration( new CodeTypeReference( typeof( DebuggerNonUserCodeAttribute ) ) )
			);

			this._targetType = targetType;
			this._declaringTypes.Add( targetType, declaringType );
			this._dependentSerializers.Clear();
			this._cachedTargetFields.Clear();
			this._cachedPropertyAccessors.Clear();
			this._buildingType = declaringType;

			var targetTypeDefinition = TypeDefinition.Object( targetType );
			this.Packer = CodeDomConstruct.Parameter( TypeDefinition.PackerType, "packer" );
			this.PackToTarget = CodeDomConstruct.Parameter( targetTypeDefinition, "objectTree" );
			this.NullCheckTarget = CodeDomConstruct.Parameter( targetTypeDefinition, "objectTree" );
			this.Unpacker = CodeDomConstruct.Parameter( TypeDefinition.UnpackerType, "unpacker" );
			this.IndexOfItem = CodeDomConstruct.Parameter( TypeDefinition.Int32Type, "indexOfItem" );
			this.ItemsCount = CodeDomConstruct.Parameter( TypeDefinition.Int32Type, "itemsCount" );
			this.UnpackToTarget = CodeDomConstruct.Parameter( targetTypeDefinition, "collection" );
			var traits = targetType.GetCollectionTraits( CollectionTraitOptions.Full, this.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = CodeDomConstruct.Parameter( targetTypeDefinition, "collection" );
				this.ItemToAdd = CodeDomConstruct.Parameter( traits.ElementType, "item" );
				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NET35 && !NET40
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NET35 && !NET40
				)
				{
					this.KeyToAdd = CodeDomConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = CodeDomConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				else
				{
					this.KeyToAdd = null;
					this.ValueToAdd = null;
				}
				this.InitialCapacity = CodeDomConstruct.Parameter( TypeDefinition.Int32Type, "initialCapacity" );
			}
		}

		public override void BeginMethodOverride( string name )
		{
			this._methodContextStack.Push( new MethodContext( name, false, TypeDefinition.ObjectType, SerializerBuilderHelper.EmptyParameters ) );
		}

		public override void BeginPrivateMethod( string name, bool isStatic, TypeDefinition returnType, params CodeDomConstruct[] parameters )
		{
			this._methodContextStack.Push(
				new MethodContext(
					name,
					isStatic,
					returnType,
					parameters
						.Select( p => new KeyValuePair<string, TypeDefinition>( p.AsParameter().Name, p.ContextType ) )
						.ToArray()
				)
			);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "CodeDOM" )]
		protected override MethodDefinition EndMethodOverrideCore( string name, CodeDomConstruct body )
		{
			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, "context.Name == name" );
#endif // DEBUG
			if ( body == null )
			{
				return null;
			}

			CodeMemberMethod codeMethod = new CodeMemberMethod { Name = name };
			switch ( name )
			{
				case MethodName.PackToCore:
				{
					codeMethod.Parameters.Add( this.Packer.AsParameter() );
					codeMethod.Parameters.Add( this.PackToTarget.AsParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.UnpackFromCore:
				{
					codeMethod.ReturnType = new CodeTypeReference( this._targetType );
					codeMethod.Parameters.Add( this.Unpacker.AsParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.UnpackToCore:
				{
					codeMethod.Parameters.Add( this.Unpacker.AsParameter() );
					codeMethod.Parameters.Add( this.UnpackToTarget.AsParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.PackUnderlyingValueTo:
				{
					codeMethod.Parameters.Add( this.Packer.AsParameter() );
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( this._targetType, "enumValue" ) );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.UnpackFromUnderlyingValue:
				{
					codeMethod.ReturnType = new CodeTypeReference( this._targetType );
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( MessagePackObject ), "messagePackObject" ) );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.AddItem:
				{
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( this._targetType, "collection" ) );
					if ( this.IsDictionary )
					{
						codeMethod.Parameters.Add(
							new CodeParameterDeclarationExpression(
								CodeDomSerializerBuilder.ToCodeTypeReference( this.KeyToAdd.ContextType ),
								"key"
							)
						);
						codeMethod.Parameters.Add(
							new CodeParameterDeclarationExpression(
								CodeDomSerializerBuilder.ToCodeTypeReference( this.ValueToAdd.ContextType ),
								"value"
							)
						);
					}
					else
					{
						codeMethod.Parameters.Add(
							new CodeParameterDeclarationExpression(
								CodeDomSerializerBuilder.ToCodeTypeReference( this.ItemToAdd.ContextType ),
								"item"
							)
						);
					}
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
					break;
				}
				case MethodName.CreateInstance:
				{
					codeMethod.ReturnType = new CodeTypeReference( this._targetType );
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( typeof( int ), "initialCapacity" ) );

					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
					break;
				}
#if FEATURE_TAP
				case MethodName.PackToAsyncCore:
				{
					codeMethod.ReturnType = new CodeTypeReference( typeof( Task ) );
					codeMethod.Parameters.Add( this.Packer.AsParameter() );
					codeMethod.Parameters.Add( this.PackToTarget.AsParameter() );
					codeMethod.Parameters.Add( CreateCancellationTokenParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.UnpackFromAsyncCore:
				{
					codeMethod.ReturnType = new CodeTypeReference( typeof( Task<> ).MakeGenericType( this._targetType ) );
					codeMethod.Parameters.Add( this.Unpacker.AsParameter() );
					codeMethod.Parameters.Add( CreateCancellationTokenParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.UnpackToAsyncCore:
				{
					codeMethod.ReturnType = new CodeTypeReference( typeof( Task ) );
					codeMethod.Parameters.Add( this.Unpacker.AsParameter() );
					codeMethod.Parameters.Add( this.UnpackToTarget.AsParameter() );
					codeMethod.Parameters.Add( CreateCancellationTokenParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
				case MethodName.PackUnderlyingValueToAsync:
				{
					codeMethod.ReturnType = new CodeTypeReference( typeof( Task ) );
					codeMethod.Parameters.Add( this.Packer.AsParameter() );
					codeMethod.Parameters.Add( new CodeParameterDeclarationExpression( this._targetType, "enumValue" ) );
					codeMethod.Parameters.Add( CreateCancellationTokenParameter() );
					// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
					codeMethod.Attributes = ( this.IsInternalToMsgPackLibrary ? MemberAttributes.FamilyOrAssembly : MemberAttributes.Family ) | MemberAttributes.Override;
					// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

					break;
				}
#endif // FEATURE_TAP
				default:
				{
					throw new ArgumentOutOfRangeException( "name", name );
				}
			}

			codeMethod.Statements.AddRange( body.AsStatements().ToArray() );

			this.DeclaringType.Members.Add( codeMethod );
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

#if FEATURE_TAP

		private static CodeParameterDeclarationExpression CreateCancellationTokenParameter()
		{
			return new CodeParameterDeclarationExpression( typeof( CancellationToken ), "cancellationToken" );
		}

#endif // FEATURE_TAP

		protected override MethodDefinition EndPrivateMethodCore( string name, CodeDomConstruct body )
		{
			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, "context.Name == name" );
#endif // DEBUG
			if ( body == null )
			{
				return null;
			}

			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			var codeMethod = new CodeMemberMethod { Name = context.Name, Attributes = MemberAttributes.Private | ( context.IsStatic ? MemberAttributes.Static : 0 ) };
			// ReSharper disable once ImpureMethodCallOnReadonlyValueField
			if ( context.ReturnType.TryGetRuntimeType() != typeof( void ) )
			{
				codeMethod.ReturnType = CodeDomSerializerBuilder.ToCodeTypeReference( context.ReturnType );
			}

			codeMethod.Parameters.AddRange(
				context.Parameters.Select( kv =>
					new CodeParameterDeclarationExpression( CodeDomSerializerBuilder.ToCodeTypeReference( kv.Value ), kv.Key )
				).ToArray()
			);

			codeMethod.Statements.AddRange( body.AsStatements().ToArray() );

			this.DeclaringType.Members.Add( codeMethod );
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
			this._methodContextStack.Push( new MethodContext( ".ctor", false, TypeDefinition.ObjectType, SerializerBuilderHelper.EmptyParameters ) );
		}

		public void EndConstructor()
		{
			this._methodContextStack.Pop();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected override void DefineUnpackingContextCore(
			IList<KeyValuePair<string, TypeDefinition>> fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor,
			out CodeDomConstruct parameterInUnpackValueMethods,
			out CodeDomConstruct parameterInSetValueMethods,
			out CodeDomConstruct parameterInCreateObjectFromContext
		)
		{
			var codeType = new CodeTypeDeclaration( SerializerBuilderHelper.UnpackingContextTypeName );
			var ctor =
				new CodeConstructor
				{
					Attributes = MemberAttributes.Public
				};
			foreach ( var kv in fields )
			{
				var field =
					new CodeMemberField( CodeDomSerializerBuilder.ToCodeTypeReference( kv.Value ), kv.Key )
					{
						Attributes = MemberAttributes.Public
					};
				codeType.Members.Add( field );

				var param = new CodeParameterDeclarationExpression( CodeDomSerializerBuilder.ToCodeTypeReference( kv.Value ), kv.Key );

				ctor.Parameters.Add( param );
				ctor.Statements.Add(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(
							new CodeThisReferenceExpression(),
							kv.Key
						),
						new CodeArgumentReferenceExpression( kv.Key )
					)
				);
			}

			codeType.Members.Add( ctor );
			this._buildingType.Members.Add( codeType );
			type = TypeDefinition.Object( codeType.Name );
			constructor = new ConstructorDefinition( type, fields.Select( kv => kv.Value ).ToArray() );
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		protected override void DefineUnpackingContextWithResultObjectCore(
			out TypeDefinition type,
			out CodeDomConstruct parameterInUnpackValueMethods,
			out CodeDomConstruct parameterInSetValueMethods,
			out CodeDomConstruct parameterInCreateObjectFromContext
		)
		{
			type = TypeDefinition.Object( this._targetType );
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		private static void DefineUnpackValueMethodArguments( TypeDefinition type, out CodeDomConstruct parameterInUnpackValueMethods, out CodeDomConstruct parameterInSetValueMethods, out CodeDomConstruct parameterInCreateObjectFromContext )
		{
			parameterInUnpackValueMethods = CodeDomConstruct.Parameter( type, "unpackingContext" );
			parameterInSetValueMethods = CodeDomConstruct.Parameter( type, "unpackingContext" );
			parameterInCreateObjectFromContext = CodeDomConstruct.Parameter( type, "unpackingContext" );
		}

		public override CodeDomConstruct DefineUnpackedItemParameterInSetValueMethods( TypeDefinition itemType )
		{
			return CodeDomConstruct.Parameter( itemType, "unpackedValue" );
		}

		/// <summary>
		///		Generates codes for this context.
		/// </summary>
		/// <returns>A <see cref="SerializerCodeGenerationResult"/> collection which correspond to genereated codes.</returns>
#if !NET35
		[SecuritySafeCritical]
#endif // !NET35
		public IEnumerable<SerializerCodeGenerationResult> Generate()
		{
			Contract.Assert( this._declaringTypes != null, "_declaringTypes != null" );

			using ( var provider = CodeDomProvider.CreateProvider( this._configuration.Language ) )
			{
				var options =
					new CodeGeneratorOptions
					{
						BlankLinesBetweenMembers = true,
						ElseOnClosing = false,
						IndentString = this._configuration.CodeIndentString,
						VerbatimOrder = false
					};

				var directory =
					Path.Combine(
						this._configuration.OutputDirectory,
						this._configuration.Namespace.Replace( Type.Delimiter, Path.DirectorySeparatorChar )
					);

				var sink = this._configuration.CodeGenerationSink ?? CodeGenerationSink.ForIndividualFile();

				var result = new List<SerializerCodeGenerationResult>( this._declaringTypes.Count );
				var extension = "." + provider.FileExtension;

				foreach ( var declaringType in this._declaringTypes )
				{
					Contract.Assert( declaringType.Value.TypeParameters.Count == 0, declaringType.Value.TypeParameters.Count + "!= 0" );

					var cn = new CodeNamespace( this._configuration.Namespace );
					cn.Types.Add( declaringType.Value );
					var cu = new CodeCompileUnit();
					cu.Namespaces.Add( cn );

					var codeInfo = new SerializerCodeInformation( declaringType.Value.Name, directory, extension );
					sink.AssignTextWriter( codeInfo );

					result.Add(
						new SerializerCodeGenerationResult(
							declaringType.Key,
							codeInfo.FilePath,
							String.IsNullOrEmpty( cn.Name )
							? declaringType.Value.Name
							: cn.Name + "." + declaringType.Value.Name,
							cn.Name,
							declaringType.Value.Name
						)
					);

#if DEBUG
					if ( SerializerDebugging.DumpEnabled )
					{
						SerializerDebugging.TraceEmitEvent( "Compile {0}", declaringType.Value.Name );
					}
#endif // DEBUG

					using ( var writer =
#if DEBUG
						SerializerDebugging.DumpEnabled
							? new TeeTextWriter( codeInfo.TextWriter ?? NullTextWriter.Instance, SerializerDebugging.ILTraceWriter ) :
#endif // DEBUG
							codeInfo.TextWriter ?? NullTextWriter.Instance
					)
					{
						provider.GenerateCodeFromCompileUnit( cu, writer, options );
						writer.WriteLine();
						writer.Flush();

#if DEBUG
						if ( SerializerDebugging.DumpEnabled )
						{
							SerializerDebugging.TraceEmitEvent( "Compile {0}", declaringType.Value.Name );
							SerializerDebugging.FlushTraceData();
						}
#endif // DEBUG
					}
				}

				return result;
			}
		}

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
	}
}