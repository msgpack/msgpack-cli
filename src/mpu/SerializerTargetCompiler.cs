#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace mpu
{
	/// <summary>
	///		Compiles to specified assets files to the assembly for serializer generation.
	/// </summary>
	/// <remarks>
	///		This class is NOT formal API, so backward compatibility will not be maintained.
	/// </remarks>
	public sealed class SerializerTargetCompiler
	{
		/// <summary>
		///		Gets or sets a value indicating whether compiler warnings should be treated as errors or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if compiler warnings should be treated as errors; otherwise, <c>false</c>.
		///		The default is <c>false</c>.
		/// </value>
		public bool TreatWarningsAsErrors { get; set; }

		/// <summary>
		///		Gets or sets the warning level of the compiler.
		/// </summary>
		/// <value>
		///		The warning level  of the compiler.
		///		The default is <c>4</c>.
		/// </value>
		public int WarningLevel { get; set; }

		/// <summary>
		///		Gets or sets the <see cref="TextWriter"/> which will receive compiler output stream.
		/// </summary>
		/// <value>
		///		A <see cref="TextWriter"/> which will receive compiler output stream.
		///		A <c>null</c> indicates using standard output stream.
		///		The default is <c>null</c>.
		/// </value>
		public TextWriter OutputWriter { get; set; }

		/// <summary>
		///		Gets or sets the <see cref="TextWriter"/> which will receive compiler error stream.
		/// </summary>
		/// <value>
		///		A <see cref="TextWriter"/> which will receive compiler error stream.
		///		A <c>null</c> indicates using standard error stream.
		///		The default is <c>null</c>.
		/// </value>
		public TextWriter ErrorWriter { get; set; }

		public SerializerTargetCompiler()
		{
			this.WarningLevel = 4;
		}

		// TODO: rsp is used?
		/// <summary>
		///		Compiles specified source files to an assembly as serialization target type assembly.
		/// </summary>
		/// <param name="sourceFilePathes">The source code file pathes.</param>
		/// <param name="referenceAssemblies">The additional reference assembly file pathes to compile specified files.</param>
		/// <returns>An assembly which contains serialization target types.</returns>
		/// <exception cref="System.Exception">Failed to generate serializer source code because of compilation error.</exception>
		public Assembly CompileTargetTypeAssembly(
			IEnumerable<string> sourceFilePathes,
			IEnumerable<string> referenceAssemblies
		)
		{
			var sourceAssembly =
				this.CompileSourceFiles(
					sourceFilePathes,
					referenceAssemblies,
					this.OutputWriter ?? Console.Out,
					this.ErrorWriter == null
						? ColorizedTextWriter.ForConsoleError()
						: this.ErrorWriter == Console.Out
							? ColorizedTextWriter.ForConsoleOutput()
							: this.ErrorWriter == Console.Error
								? ColorizedTextWriter.ForConsoleError()
								: ColorizedTextWriter.ForTextWriter( this.ErrorWriter )
					);

			if ( sourceAssembly == null )
			{
				throw new Exception( "Failed to generate serializer source code because target type compilation." );
			}

			return sourceAssembly;
		}

		private Assembly CompileSourceFiles(
			IEnumerable<string> sourceFilePathes,
			IEnumerable<string> referenceAssemblies,
			TextWriter outputWriter,
			ColorizedTextWriter errorWriter
			)
		{
			var compilerParameters =
				new CompilerParameters
				{
					GenerateExecutable = false,
					IncludeDebugInformation = false,
					GenerateInMemory = true,
					OutputAssembly = "MsgPackSerializers",
					TreatWarningsAsErrors = this.TreatWarningsAsErrors,
					WarningLevel = this.WarningLevel
				};

			foreach ( var referenceAssembly in referenceAssemblies )
			{
				compilerParameters.ReferencedAssemblies.Add( referenceAssembly );
			}

			if (
				!typeof( CodeDomProvider ).Assembly.CodeBase.StartsWith(
					Environment.ExpandEnvironmentVariables( "file:///%SystemDrive%/Windows/" ),
					StringComparison.OrdinalIgnoreCase ) )
			{
				// may be mcs, so add C# 3.5 option.
				compilerParameters.CompilerOptions = "-langversion=3 -sdk=2";
			}

			var results =
				CodeDomProvider.CreateProvider( "C#" ).CompileAssemblyFromFile(
					compilerParameters,
					sourceFilePathes.ToArray()
					);

			foreach ( var stdout in results.Output )
			{
				outputWriter.WriteLine( stdout );
			}

			outputWriter.Flush();

			foreach ( CompilerError error in results.Errors )
			{
				string message =
					String.Format(
						CultureInfo.CurrentCulture,
						"Source '{1}'{0}  Line:{2}, Column:{3}{0}  {4}{0}{5}",
						Environment.NewLine,
						error.FileName,
						error.Line,
						error.Column,
						error.ErrorNumber,
						error.ErrorText
						);

				if ( error.IsWarning )
				{
					errorWriter.WriteWarning( message );
				}
				else
				{
					errorWriter.WriteError( message );
				}
			}

			errorWriter.Flush();

			return results.CompiledAssembly;
		}
	}
}