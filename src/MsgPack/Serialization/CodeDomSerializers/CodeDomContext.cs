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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeDomSerializers
{
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

		public CodeDomContext( SerializationContext context, SerializerCodeGenerationConfiguration configuration )
			: base( context )
		{
			this._configuration = configuration;
		}

		public string GetSerializerFieldName( Type targetType, EnumMemberSerializationMethod enumSerializationMethod, DateTimeMemberConversionMethod dateTimeConversionMethod, PolymorphismSchema polymorphismSchema )
		{
			var key = new SerializerFieldKey( targetType, enumSerializationMethod, dateTimeConversionMethod, polymorphismSchema );

			string fieldName;
			if ( !this._dependentSerializers.TryGetValue( key, out fieldName ) )
			{
				fieldName = "_serializer" + this._dependentSerializers.Count.ToString( CultureInfo.InvariantCulture );
				this._dependentSerializers.Add( key, fieldName );
			}

			return fieldName;
		}

		public Dictionary<SerializerFieldKey, String> GetDependentSerializers()
		{
			return this._dependentSerializers;
		}

		public string GetCachedFieldInfoName( FieldInfo field )
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
			}

			return cachedField.StorageFieldName;
		}

		public Dictionary<RuntimeFieldHandle, CachedFieldInfo> GetCachedFieldInfos()
		{
			return this._cachedTargetFields;
		}

		public string GetCachedMethodBaseName( MethodBase method )
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
			}

			return cachedMethod.StorageFieldName;
		}

		public Dictionary<RuntimeMethodHandle, CachedMethodBase> GetCachedMethodBases()
		{
			return this._cachedPropertyAccessors;
		}

		private readonly Dictionary<string, int> _uniqueVariableSuffixes = new Dictionary<string, int>();

		/// <summary>
		///		Gets a unique name of a local variable.
		/// </summary>
		/// <param name="prefix">The prefix of the variable.</param>
		/// <returns>A unique name of a local variable.</returns>
		public override string GetUniqueVariableName( string prefix )
		{
			int counter;
			if ( !this._uniqueVariableSuffixes.TryGetValue( prefix, out counter ) )
			{
				this._uniqueVariableSuffixes.Add( prefix, 0 );
				return prefix;
			}

			this._uniqueVariableSuffixes[ prefix ] = counter + 1;

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

			this._declaringTypes.Add( targetType, declaringType );
			this._dependentSerializers.Clear();
			this._cachedTargetFields.Clear();
			this._cachedPropertyAccessors.Clear();
			this._buildingType = declaringType;

			this.Packer = CodeDomConstruct.Parameter( typeof( Packer ), "packer" );
			this.PackToTarget = CodeDomConstruct.Parameter( targetType, "objectTree" );
			this.Unpacker = CodeDomConstruct.Parameter( typeof( Unpacker ), "unpacker" );
			this.UnpackToTarget = CodeDomConstruct.Parameter( targetType, "collection" );
			var traits = targetType.GetCollectionTraits();
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = CodeDomConstruct.Parameter( targetType, "collection" );
				this.ItemToAdd = CodeDomConstruct.Parameter( traits.ElementType, "item" );
				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_35 && !UNITY && !NETFX_40
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_35 && !UNITY && !NETFX_40
				)
				{
					this.KeyToAdd = CodeDomConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = CodeDomConstruct.Parameter( traits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				this.InitialCapacity = CodeDomConstruct.Parameter( typeof( int ), "initialCapacity" );
			}
		}

		/// <summary>
		///		Resets internal states for new method.
		/// </summary>
		public void ResetMethodContext()
		{
			this._uniqueVariableSuffixes.Clear();
		}

		/// <summary>
		///		Generates codes for this context.
		/// </summary>
		/// <returns>A <see cref="SerializerCodeGenerationResult"/> collection which correspond to genereated codes.</returns>
#if !NETFX_35
		[SecuritySafeCritical]
#endif // !NETFX_35
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
				Directory.CreateDirectory( directory );

				var result = new List<SerializerCodeGenerationResult>( this._declaringTypes.Count );

				foreach ( var declaringType in this._declaringTypes )
				{
					var typeFileName = declaringType.Value.Name;
					if ( declaringType.Value.TypeParameters.Count > 0 )
					{
						typeFileName += "`" + declaringType.Value.TypeParameters.Count.ToString( CultureInfo.InvariantCulture );
					}

					typeFileName += "." + provider.FileExtension;

					var cn = new CodeNamespace( this._configuration.Namespace );
					cn.Types.Add( declaringType.Value );
					var cu = new CodeCompileUnit();
					cu.Namespaces.Add( cn );

					var filePath = Path.Combine( directory, typeFileName );
					result.Add( 
						new SerializerCodeGenerationResult( 
							declaringType.Key, 
							filePath, 
							String.IsNullOrEmpty( cn.Name )
							? declaringType.Value.Name
							: cn.Name + "." + declaringType.Value.Name, 
							cn.Name, 
							declaringType.Value.Name
						)
					);

					using ( var writer = new StreamWriter( filePath, false, Encoding.UTF8 ) )
					{
						provider.GenerateCodeFromCompileUnit( cu, writer, options );
					}
				}

				return result;
			}
		}

		/// <summary>
		///		Creates the <see cref="CodeCompileUnit"/> for on-the-fly code generation for execution.
		/// </summary>
		/// <returns>
		///		The newly created <see cref="CodeCompileUnit"/> for on-the-fly code generation for execution.
		/// </returns>
		public CodeCompileUnit CreateCodeCompileUnit()
		{
			var cn = new CodeNamespace( this._configuration.Namespace );
			cn.Types.Add( this._buildingType );
			var cu = new CodeCompileUnit();
			cu.Namespaces.Add( cn );
			return cu;
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
	}
}