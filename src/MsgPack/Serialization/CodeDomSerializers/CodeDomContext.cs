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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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

		private readonly Dictionary<Type, string> _dependentSerializers = new Dictionary<Type, string>();

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

		public string GetSerializerFieldName( Type targetType )
		{
			string fieldName;
			if ( !this._dependentSerializers.TryGetValue( targetType, out fieldName ) )
			{
				fieldName = "_serializer" + this._dependentSerializers.Count.ToString( CultureInfo.InvariantCulture );
				this._dependentSerializers.Add( targetType, fieldName );
			}

			return fieldName;
		}

		public Dictionary<string, Type> GetDependentSerializers()
		{
			return
				this._dependentSerializers.ToDictionary(
					kv => kv.Value,
					kv => typeof( MessagePackSerializer<> ).MakeGenericType( kv.Key )
				);
		}

		private readonly Dictionary<string, int> _uniqueVariableSuffixes = new Dictionary<string, int>();

		/// <summary>
		///		Determines that whether built-in serializer for specified type exists or not.
		/// </summary>
		/// <param name="type">The type for check.</param>
		/// <returns>
		///   <c>true</c> if built-in serializer for specified type exists; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool BuiltInSerializerExists( Type type )
		{
			if ( type == null )
			{
				throw new ArgumentNullException( "type" );
			}

			return type.IsArray || SerializerRepository.Default.Contains( type );
		}

		/// <summary>
		///		Gets a unique name of a local variable.
		/// </summary>
		/// <param name="prefix">The prefix of the variable.</param>
		/// <returns>A unique name of a local variable.</returns>
		public string GetUniqueVariableName( string prefix )
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
		///		Resets internal states for new type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		public override void Reset( Type targetType )
		{
			var declaringType = new CodeTypeDeclaration( IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" );
			declaringType.BaseTypes.Add( typeof( MessagePackSerializer<> ).MakeGenericType( targetType ) );
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
			this._buildingType = declaringType;

			this.Packer = CodeDomConstruct.Parameter( typeof( Packer ), "packer" );
			this.PackToTarget = CodeDomConstruct.Parameter( targetType, "objectTree" );
			this.Unpacker = CodeDomConstruct.Parameter( typeof( Unpacker ), "unpacker" );
			this.UnpackToTarget = CodeDomConstruct.Parameter( targetType, "collection" );
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
		/// <returns>The path of generated files.</returns>
		public IEnumerable<string> Generate()
		{
			var provider = CodeDomProvider.CreateProvider( this._configuration.Language );
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

			var result = new List<string>( _declaringTypes.Count );

			foreach ( var declaringType in _declaringTypes )
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
				result.Add( filePath );

				using ( var writer = new StreamWriter( filePath, false, Encoding.UTF8 ) )
				{
					provider.GenerateCodeFromCompileUnit( cu, writer, options );
				}
			}

			return result;
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
	}
}