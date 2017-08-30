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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines CodeDOM based code generation logic for unit testing.
	/// </summary>
	internal static class CodeDomCodeGeneration
	{
		private static int _suffix = 0;

		public static void Compile( string code, bool isDebug, out Assembly compiledAssembly, out IList<string> errors, out IList<string> warnings )
		{
			var assemblyName = "CodeGenerationAssembly" + Interlocked.Increment( ref _suffix );
			var assemblyPath = Path.Combine( SerializerDebugging.DumpDirectory ?? Path.GetTempPath(), assemblyName + ".dll" );

			using ( var provider = CodeDomProvider.CreateProvider( "C#" ) )
			{
				var parameters =
					new CompilerParameters
					{
						GenerateInMemory = false,
						OutputAssembly = assemblyPath
					};

				foreach ( var assembly in SerializerDebugging.CodeSerializerDependentAssemblies )
				{
					parameters.ReferencedAssemblies.Add( ( string ) assembly );
				}

				var result =
					provider.CompileAssemblyFromSource(
						parameters,
						code
					);

				errors = BuildCompilationError( result.Errors.OfType<CompilerError>().Where( d => !d.IsWarning ) );
				warnings = BuildCompilationError( result.Errors.OfType<CompilerError>().Where( d => d.IsWarning ) );

				if ( !errors.Any() )
				{
					SerializerDebugging.AddCompiledCodeAssembly( result.PathToAssembly );
					compiledAssembly = result.CompiledAssembly;
				}
				else
				{
					compiledAssembly = null;
				}
			}
		}

		private static IList<string> BuildCompilationError( IEnumerable<CompilerError> diagnostics )
		{
			return
				diagnostics.Select(
					( diagnostic, i ) =>
						String.Format(
							CultureInfo.InvariantCulture,
							"[{0}]{1}:{2}:(File:{3}, Line:{4}, Column:{5}):{6}",
							i,
							diagnostic.IsWarning ? "Warn" : "Error",
							diagnostic.ErrorNumber,
							diagnostic.FileName,
							diagnostic.Line,
							diagnostic.Column,
							diagnostic.ErrorText
						)
				).ToArray();
		}
	}
}
