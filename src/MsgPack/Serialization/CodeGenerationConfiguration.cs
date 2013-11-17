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
using System.IO;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents configuration for code generation.
	/// </summary>
	public sealed class CodeGenerationConfiguration
	{
		private string _namespace;

		/// <summary>
		///		Gets or sets the namespace of generated classes.
		/// </summary>
		/// <value>
		///		The namespace of generated classes.
		///		The default is <c>"MsgPack.Serialization.GeneratedSerializers"</c>.
		/// </value>
		public string Namespace
		{
			get { return this._namespace; }
			set
			{
				Validation.ValidateNamespace( value, "value" );
				this._namespace = value;
			}
		}

		private string _outputDirectory;

		/// <summary>
		///		Gets or sets the output directory for generated source codes.
		/// </summary>
		/// <value>
		///		The output directory for generated source codes.
		///		The default is current directory.
		/// </value>
		/// <exception cref="ArgumentNullException">The specified value is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The specified value is empty or too long.</exception>
		/// <exception cref="NotSupportedException">The specified path format is not supported.</exception>
		public string OutputDirectory
		{
			get { return this._outputDirectory; }
			set { this._outputDirectory = Path.GetFullPath( value ); }
		}

		/// <summary>
		///		Gets or sets the language identifier for code generation.
		/// </summary>
		/// <value>
		///		The language identifier for code generation.
		///		This value must be registered identifier in CodeDOM configuration.
		///		The default is <c>"C#"</c>.
		/// </value>
		/// <remarks>
		///		This value will be passed as-is for an underlying code dom provider.
		/// </remarks>
		/// <see cref="System.CodeDom.Compiler.CodeDomProvider.CreateProvider(String)"/>
		public string Language { get; set; }

		/// <summary>
		///		Gets or sets the indentation string for code generation.
		/// </summary>
		/// <value>
		///		The indentation string for code generation.
		///		The default is <c>"    "</c>(4 U+0020 chars).
		/// </value>
		/// <remarks>
		///		This value will be passed as-is for an underlying code dom provider.
		/// </remarks>
		/// <see cref="System.CodeDom.Compiler.CodeGeneratorOptions"/>
		public string CodeIndentString { get; set; }

		/// <summary>
		///		Gets or sets the bracing style specifier for code generation.
		/// </summary>
		/// <value>
		///		The bracing style specifier for code generation.
		///		The default is <c>"Block"</c>.
		/// </value>
		/// <remarks>
		///		This value will be passed as-is for an underlying code dom provider.
		/// </remarks>
		/// <see cref="System.CodeDom.Compiler.CodeGeneratorOptions"/>
		public string CodeBracingStyle { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="CodeGenerationConfiguration"/> class.
		/// </summary>
		public CodeGenerationConfiguration()
		{
			this.OutputDirectory = Path.GetFullPath( "." );
			this.Language = "C#";
			this.CodeBracingStyle = "Block";
			this.Namespace = "MsgPack.Serialization.GeneratedSerializers";
			this.CodeIndentString = "    ";
		}
	}
}