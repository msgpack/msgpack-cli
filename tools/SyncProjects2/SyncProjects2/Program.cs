#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Options;
using MsgPack.Tools.Build;

namespace SyncProject2
{
	internal static class Program
	{
		private static int Main( string[] args )
		{
			try
			{
				var file = "Sync.json";
				var sourceBasePath = "src" + Path.DirectorySeparatorChar;
				var projectExtension = ".csproj";
				var msbuildExtensionsPath = Environment.ExpandEnvironmentVariables( @"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild" );
				var help = false;

				var options =
					new OptionSet
					{
						{ "d|def=", "File path to synchronization definition. Default: Sync.xml", v => file = v },
						{ "s|src=", "File path to base directory of source tree. Default: src" + Path.DirectorySeparatorChar, v => sourceBasePath = v },
						{ "e|ext=", "Extension (including leading dot) of the project file. Default: .csproj", v => projectExtension = v },
						{ "msbuild-ext-path=", "Specify MSBuild extensions path. Default is <VSInstallDir>\\MSBuild.", v => msbuildExtensionsPath = v },
						{ "h|?|help", "Show this help.", _ => help = true },
					};

				options.Parse( args );
				if ( help )
				{
					Console.Error.WriteLine( "SyncProjects" );
					Console.Error.WriteLine();
					Console.Error.WriteLine( "Usage: SyncProjects2 [<OPTIONS>]" );
					Console.Error.WriteLine();
					Console.Error.WriteLine( "Options:" );
					options.WriteOptionDescriptions( Console.Out );
					return 1;
				}

				SynchronizeProjects( file, sourceBasePath, projectExtension, msbuildExtensionsPath );

				return 0;
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( ex );
				return ex.HResult;
			}
		}

		private static void SynchronizeProjects( string file, string sourceBasePath, string projectExtension, string msbuildExtensionsPath )
		{
			var globalProperties =
				new Dictionary<string, string>
				{
					[ "MSBuildExtensionsPath" ] = msbuildExtensionsPath,
					[ "EnableDefaultItems" ] = "true"
				};

			Dictionary<string, ProjectSynchronizationDefinition> definitions;
			using ( var reader = File.OpenText( file ) )
			{
				definitions = ProjectSynchronizationDefinition.FromJson( reader ).ToDictionary( x => x.TargetProjectName );
			}

			foreach ( var definition in definitions )
			{
				definition.Value.ResolveProjectName( sourceBasePath, projectExtension );
				definition.Value.ResolveBase( key => definitions.TryGetValue( key, out var found ) ? found : null );

				Console.Error.WriteLine( $"Process {definition.Value.TargetProjectName}" );
				ProjectSynchronizer.Synchronize( definition.Value, globalProperties );
			}
		}
	}
}