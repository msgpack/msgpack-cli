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
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MsgPack.Serialization
{
#warning TODO: Property validation.
	internal interface ISerializerGeneratorConfiguration
	{
		string OutputDirectory { get; set; }
		SerializationMethod SerializationMethod { get; set; }
		void Validate();
	}

	public sealed class SerializerAssemblyGenerationConfiguration : ISerializerGeneratorConfiguration
	{
		public string OutputDirectory { get; set; }
		public SerializationMethod SerializationMethod { get; set; }
		public AssemblyName AssemblyName { get; set; }

		public SerializerAssemblyGenerationConfiguration()
		{
			this.OutputDirectory = ".";
			this.SerializationMethod = SerializationMethod.Array;
		}

		void ISerializerGeneratorConfiguration.Validate()
		{
			try
			{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				Path.GetFullPath( "." + Path.DirectorySeparatorChar + this.AssemblyName.Name );
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			}
			catch ( ArgumentException ex )
			{
				throw CreateValidationError( ex );
			}
			catch ( NotSupportedException ex )
			{
				throw CreateValidationError( ex );
			}
		}

		private Exception CreateValidationError( Exception innerException )
		{
			return
				new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture, "AssemblyName property is not set correctly. Detail: {0}", innerException.Message
					),
					innerException
				);
		}
	}

	/// <summary>
	///		Represents configuration for code generation.
	/// </summary>
	public sealed class SerializerCodeGenerationConfiguration : ISerializerGeneratorConfiguration
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
		///		Initializes a new instance of the <see cref="SerializerCodeGenerationConfiguration"/> class.
		/// </summary>
		public SerializerCodeGenerationConfiguration()
		{
			this.OutputDirectory = Path.GetFullPath( "." );
			this.Language = "C#";
			this.Namespace = "MsgPack.Serialization.GeneratedSerializers";
			this.CodeIndentString = "    ";
		}

		public SerializationMethod SerializationMethod { get; set; }

		void ISerializerGeneratorConfiguration.Validate()
		{
			// nop
		}
	}
}