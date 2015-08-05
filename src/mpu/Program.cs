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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Mono.Options;

using MsgPack.Serialization;

namespace mpu
{
	static class Program
	{
		static int Main( string[] args )
		{
			try
			{
				return Execute( args );
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( ex );
				if ( ex is ArgumentException )
				{
					return 2;
				}

				int hResult = Marshal.GetHRForException( ex );
				return IsInWindows() ? hResult : ( unchecked( ( uint )hResult ) < 255 ) ? hResult : 255;
			}
		}

		private static bool IsInWindows()
		{
			switch ( Environment.OSVersion.Platform )
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
				{
					return true;
				}
				default:
				{
					return false;

				}
			}
		}

		private static int Execute( IEnumerable<string> args )
		{
			Action? action = null;
			var sourceProjectPath = default( string );
			var outputDirectoryPath = default( string );
			var overwrite = false;
			var sourceFileIsAssembly = false;
			var referenceAssemblies = new List<string>();
			var includingPattern = default( string );
			var excludingPattern = default( string );
			var treatWarningsAsErrors = false;
			var warningLevel = 4;
			var configuration =
				new SerializerCodeGenerationConfiguration
				{
					PreferReflectionBasedSerializer = true,
					IsRecursive = true
				};

			var options =
				new OptionSet( Localize )
				{
					{
						"?|h|help", "Show this help message.",
						_ => action = Action.ShowHelp
					},
					{
						"l|library", "[required] Copy MsgPack source as assets library tree.",
						_ => action = action != null ? Action.ShowHelp : Action.BuildAssetLibrary
					},
					{
						"s|serializer", "[required] Generate serializer sources as assets tree.",
						_ => action = action != null ? Action.ShowHelp : Action.GenerateSerializers
					},
					{
						"p|project=", "[library, optional] Specify MsgPack.Unity3D.csproj path. Default is './src/MsgPack.Unity3D/MsgPack.Unity3D.csproj'.",
						value => sourceProjectPath = value
					},
					{
						"o|out=", "[all, optional] Specify the root directory of the output file tree. Default is './Assets/MsgPack' for 'library', './Assets/MsgPackSerializers/' for 'serializer'.",
						value => outputDirectoryPath = value
					},
					{
						"w|overwrite", "[library, optional] Overwrite existent source file (you have to clean output directory in advance by default.)",
						_ => overwrite = true
					},
					{
						"a|assembly", "[serializer, optional] Specify source file is assembly file which contains all serialization target types instead of C# source code files.",
						_ => sourceFileIsAssembly = true
					},
					{
						"n|namespace=", "[serializer, optional] Specify namespace for generated serializer types.",
						value => configuration.Namespace = value
					},
					{
						"internal", "[serializer, optional] Specify generated source code will be internal to MsgPack library itself. This option is required if you import MsgPack sources instead of an assembly to your Assets.",
						_ => configuration.IsInternalToMsgPackLibrary = true
					},
					{
						"method=", "[serializer, optional] Specify serialization method for generated serializers. Valid value is Array or Map. Default is 'Array'.",
						(SerializationMethod value) => configuration.SerializationMethod = value
					},
					{
						"enum-method=", "[serializer, optional] Specify enum serialization method for generated enum serializers. Valid value is ByName or ByUnderlyingType. Default is 'ByName'.",
						(EnumSerializationMethod value) => configuration.EnumSerializationMethod = value
					},
					{
						"singular", "[serializer, optional] Specify avoid recursive serializer generation for target type(s).",
						_ => configuration.PreferReflectionBasedSerializer = false
					},
					{
						"avoid-reflection-based", "[serializer, optional] Specify avoid built-in reflection based serializer and generates alternative serializers.",
						_ => configuration.PreferReflectionBasedSerializer = false
					},
					{
						"indent=", "[serializer, optional] Specify indent string for generated serializers. Default is a horizontal tab charactor (U+0009).",
						value => configuration.CodeIndentString = value
					},
					{
						"r|references=", "[serializer, optional] Specify reference assemblies' file pathes (delimited by comma) to compile serialization target type source codes. './MsgPack.dll' will be added automatically when it exists.",
						value => referenceAssemblies.AddRange( value.Split( new []{','}, StringSplitOptions.RemoveEmptyEntries).Select( token => token.Trim() ))
					},
					{
						"includes=", "[serializer, optional] Specify additional regular expression to filter in serialization target types. This filter is used for type full name including its namespace.",
						value => includingPattern = value
					},
					{
						"excludes=", "[serializer, optional] Specify additional regular expression to filter in serialization target types. This filter is used for type full name including its namespace.",
						value => excludingPattern = value
					},
					{
						"treatWarningsAsErrors", "[serializer, optional] Specify to generate error for compiler warnings for serialization target types.",
						_ => treatWarningsAsErrors = true
					},
					{
						"warningLevel=", "[serializer, optional] Specify compiler warning level for serialization target types. Default is '4'.",
						(int value) => warningLevel = value
					}
				};

			var sourceFilePathes = options.Parse( args );

			switch ( action.GetValueOrDefault() )
			{
				case Action.BuildAssetLibrary:
				{
					BuildAssetLibrary( sourceProjectPath, outputDirectoryPath, overwrite );
					return 0;
				}
				case Action.GenerateSerializers:
				{
					configuration.OutputDirectory = outputDirectoryPath;
					if ( File.Exists( "./MsgPack.dll" ) )
					{
						referenceAssemblies.Add( "./MsgPack.dll" );
					}

					GenerateSerializers(
						sourceFilePathes,
						referenceAssemblies.Distinct( PathEqualityComparer.Instance ).ToArray(),
						sourceFileIsAssembly,
						includingPattern,
						excludingPattern,
						treatWarningsAsErrors,
						warningLevel,
						configuration
					);
					return 0;
				}
				default:
				{
					ShowHelp( options );
					return 1;
				}
			}
		}

		private static string Localize( string token )
		{
			return token;
		}

		private static void ShowHelp( OptionSet options )
		{
			Console.WriteLine( "MessagePack for Unity Utility ver. {0}",
				typeof( Program ).Assembly.GetCustomAttributes( typeof( AssemblyInformationalVersionAttribute ), false )
					.OfType<AssemblyInformationalVersionAttribute>().Single().InformationalVersion
			);
			Console.WriteLine( "  {0}",
				typeof( Program ).Assembly.GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false )
					.OfType<AssemblyCopyrightAttribute>().Single().Copyright
			);
			Console.WriteLine();
			Console.WriteLine( "Syntax:" );
			Console.WriteLine( "{0}{1} {{-l[ibrary]|-s[erializer]|-?|-h[elp]}} [other options] ['serializer' source assembly or codes]", IsInWindows() ? String.Empty : "mono ", Assembly.GetEntryAssembly().GetName().Name );
			Console.WriteLine();
			Console.WriteLine( "Options:" );
			options.WriteOptionDescriptions( Console.Out );
			Console.WriteLine();
			Console.WriteLine( "Exit code:" );
			Console.WriteLine( "           0: success" );
			Console.WriteLine( "           1: help is shown" );
			Console.WriteLine( "           2: invalid arguments" );
			if ( IsInWindows() )
			{
				Console.WriteLine( "   (hresult): other errors" );
			}
			else
			{
				Console.WriteLine( "   0x57-0xFE: other errors" );
				Console.WriteLine( "        0xFF: other errors its hresult is gerator than 0xFE" );
			}
		}

		private static void BuildAssetLibrary( string sourceProjectPath, string outputDirectoryPath, bool overwrite )
		{
			new AssetFileImporter
			{
				Overwrite = overwrite
			}.AssembleAssetTree(
				sourceProjectPath,
				outputDirectoryPath
			);
		}

		private static void GenerateSerializers(
			IList<string> sourceFilePathes,
			string[] referenceAssemblies,
			bool sourceFileIsAssembly,
			string includingPattern,
			string excludingPattern,
			bool treatWarningsAsErrors,
			int warningLevel,
			SerializerCodeGenerationConfiguration configuration
		)
		{
			if ( sourceFilePathes == null || sourceFilePathes.Count == 0 )
			{
				throw new ArgumentException( "Source files or a source assembly is required." );
			}
			var generator = new SerializerCodeGenerator( configuration );

			IEnumerable<string> result;
			if ( !sourceFileIsAssembly )
			{
				result =
					generator.GenerateSerializers(
						new SerializerTargetCompiler
						{
							TreatWarningsAsErrors = treatWarningsAsErrors,
							WarningLevel = warningLevel
						}.CompileTargetTypeAssembly(
							sourceFilePathes,
							referenceAssemblies ?? new string[ 0 ]
							),
						includingPattern,
						excludingPattern
						);
			}
			else
			{
				result =
					generator.GenerateSerializers(
						sourceFilePathes[ 0 ],
						includingPattern,
						excludingPattern
						);
			}

			foreach ( var outputFilePath in result )
			{
				Console.WriteLine( outputFilePath );
			}
		}

		private enum Action
		{
			ShowHelp = 0,
			BuildAssetLibrary,
			GenerateSerializers
		}
	}
}
