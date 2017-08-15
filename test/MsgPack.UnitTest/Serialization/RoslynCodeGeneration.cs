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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines Roslyn based code generation logic for unit testing.
	/// </summary>
	internal static class RoslynCodeGeneration
	{
		private static int _suffix = 0;

		public static void Compile( string code, bool isDebug, out Assembly compiledAssembly, out IList<string> errors, out IList<string> warnings )
		{
			var assemblyName = "CodeGenerationAssembly" + Interlocked.Increment( ref _suffix );
			var metadataList =
				SerializerDebugging.CodeSerializerDependentAssemblies.Select(
					a =>
						a is string
							? AssemblyMetadata.CreateFromFile( a as string )
							: AssemblyMetadata.CreateFromImage( a as byte[] )
				).ToArray();
			try
			{
				var compilation =
					CSharpCompilation.Create(
						assemblyName,
						new[] { CSharpSyntaxTree.ParseText( code ) },
						metadataList.Select( m => m.GetReference() ),
						new CSharpCompilationOptions(
							OutputKind.DynamicallyLinkedLibrary,
							optimizationLevel: isDebug ? OptimizationLevel.Debug : OptimizationLevel.Release,
							// Suppress CS0436 because gen/*.cs will conflict with testing serializers.
							specificDiagnosticOptions: new[] { new KeyValuePair<string, ReportDiagnostic>( "CS0436", ReportDiagnostic.Suppress ) }
						)
					);

				var emitOptions = new EmitOptions( runtimeMetadataVersion: "v4.0.30319" );
				EmitResult result;
				if ( SerializerDebugging.DumpEnabled )
				{
					var assemblyPath = Path.Combine( SerializerDebugging.DumpDirectory ?? Path.GetTempPath(), assemblyName + ".dll" );

					using ( var fileStream = File.OpenWrite( assemblyPath ) )
					{
						result = compilation.Emit( fileStream, options: emitOptions );
						fileStream.Flush();
					}

					if ( result.Success )
					{
						compiledAssembly = Assembly.LoadFrom( assemblyPath );
						SerializerDebugging.AddCompiledCodeAssembly( assemblyPath );
					}
					else
					{
						compiledAssembly = null;
					}
				}
				else
				{
					using ( var buffer = new MemoryStream() )
					{
						result = compilation.Emit( buffer, options: emitOptions );
						if ( result.Success )
						{
							var image = buffer.ToArray();
							compiledAssembly = Assembly.Load( image );
							SerializerDebugging.AddCompiledCodeAssembly( assemblyName, image );
						}
						else
						{
							compiledAssembly = null;
						}
					}
				}

				errors = BuildCompilationError( result.Diagnostics.Where( d => d.Severity == DiagnosticSeverity.Error ) );
				warnings = BuildCompilationError( result.Diagnostics.Where( d => d.Severity == DiagnosticSeverity.Warning ) );
			}
			finally
			{
				foreach ( var metadata in metadataList )
				{
					metadata.Dispose();
				}
			}
		}

		private static IList<string> BuildCompilationError( IEnumerable<Diagnostic> diagnostics )
		{
			return
				diagnostics.Select(
					( diagnostic, i ) =>
						String.Format(
							CultureInfo.InvariantCulture,
							"[{0}]{1}:{2}:(File:{3}, Line:{4}, Column:{5}):{6}",
							i,
							diagnostic.Severity,
							diagnostic.Id,
							diagnostic.Location.GetLineSpan().Path,
							diagnostic.Location.GetLineSpan().StartLinePosition.Line,
							diagnostic.Location.GetLineSpan().StartLinePosition.Character,
							diagnostic.GetMessage()
						)
				).ToArray();
		}
	}
}
