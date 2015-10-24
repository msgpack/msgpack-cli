#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace mpu
{
	/// <summary>
	///		Implements mpu build command implementation.
	/// </summary>
	/// <remarks>
	///		This class is NOT formal API, so backward compatibility will not be maintained.
	/// </remarks>
	public sealed class AssetFileImporter
	{
		/// <summary>
		///		Gets or sets a value indicating whether asset files will be copied even if the destination file will exist.
		/// </summary>
		/// <value>
		///		<c>true</c> if overwrite existent files; otherwise, <c>false</c>. 
		///		The default value is <c>false</c>.
		/// </value>
		public bool Overwrite { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="AssetFileImporter"/> class.
		/// </summary>
		public AssetFileImporter() { }

		/// <summary>
		///		Assembles the asset tree from MsgPack.Unity3D.csproj file.
		/// </summary>
		/// <param name="sourceProjectPath">
		///		The source project file path which points to MsgPack.Unity3D.csproj.
		///		If omitted, ./src/MsgPack.Unity3D/MsgPack.Unity3D.csproj will be used.
		/// </param>
		/// <param name="outputDirectoryPath">
		///		The output directory path the copied source file (assets file) will be placed.
		///		If omitted, ./Assets/MsgPack will be used.
		/// </param>
		/// <exception cref="ArgumentException">A format of passed argument is invalid.</exception>
		/// <exception cref="IOException">
		///		A specified file or directory is not valid, is not found, or conflicts with existent file system object.
		///		Or, unexpected I/O error is occurred.
		/// </exception>
		/// <remarks>
		///		The output files will be placed under &lt;<paramref name="outputDirectoryPath" /> directory
		///		as hierarchical file tree.
		/// </remarks>
		public void AssembleAssetTree( string sourceProjectPath, string outputDirectoryPath )
		{
			if ( String.IsNullOrEmpty( sourceProjectPath ) )
			{
				sourceProjectPath =
					String.Format(
						CultureInfo.InvariantCulture,
						".{0}src{0}MsgPack.Unity3D{0}MsgPack.Unity3D.csproj",
						Path.DirectorySeparatorChar
					);
			}

			if ( String.IsNullOrEmpty( outputDirectoryPath ) )
			{
				outputDirectoryPath =
					String.Format(
						CultureInfo.InvariantCulture,
						".{0}Assets{0}MsgPack",
						Path.DirectorySeparatorChar
					);
			}

			var sourceDirectoryPath = Path.GetDirectoryName( sourceProjectPath );
			var relativePrefix = ".." + Path.DirectorySeparatorChar;
			foreach ( var sourceFileRelativePath in this.ParseProjectFile( sourceProjectPath ).Select( p => p.Replace( '\\', Path.DirectorySeparatorChar )) )
			{
				var destinationFilePath = 
					Path.Combine( 
						outputDirectoryPath,
						new String(
							( sourceFileRelativePath.StartsWith( relativePrefix ) ? sourceFileRelativePath.Substring( 3 ) : sourceFileRelativePath ) // remove relative
							.SkipWhile( c => c != Path.DirectorySeparatorChar) // remove project name portion
							.Skip( 1 )
							.ToArray()
						)
					);
				// ReSharper disable once AssignNullToNotNullAttribute
				Directory.CreateDirectory( Path.GetDirectoryName( destinationFilePath ) );

				Debug.Print( "Copy {0} to {1}", sourceFileRelativePath, destinationFilePath );

				File.Copy(
					// ReSharper disable once AssignNullToNotNullAttribute
					Path.Combine( sourceDirectoryPath, sourceFileRelativePath ),
					destinationFilePath,
					this.Overwrite
				);
			}
		}

		internal IEnumerable<string> ParseProjectFile( string filePath )
		{
			if ( !filePath.EndsWith( ".csproj", StringComparison.OrdinalIgnoreCase ) )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Project file type must be .csproj (C# project file)." ), "filePath" );
			}

			XDocument projectXml;
			using ( var reader = new StreamReader( filePath ) )
			{
				projectXml = XDocument.Load( reader );
			}

			if ( projectXml.Root == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Project file '{0}' is empty.", filePath ) );
			}

			return
				projectXml.Root.Elements( "{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup" )
					.Elements( "{http://schemas.microsoft.com/developer/msbuild/2003}Compile" )
					.Attributes( "Include" )
					.Where( include =>
						include.Value.EndsWith( ".cs", StringComparison.OrdinalIgnoreCase )
						&& !include.Value.EndsWith( "AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase )
						&& !include.Value.EndsWith( "CommonAssemblyInfo.cs", StringComparison.OrdinalIgnoreCase )
						&& !include.Value.EndsWith( "CommonAssemblyInfo.Pack.cs", StringComparison.OrdinalIgnoreCase )
					).Select( include => include.Value );
		}
	}
}