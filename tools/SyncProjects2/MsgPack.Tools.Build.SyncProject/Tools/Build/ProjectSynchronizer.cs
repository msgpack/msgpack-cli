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
using System.Text;
using System.Xml.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace MsgPack.Tools.Build
{
	/// <summary>
	///		Implements project synchronization
	/// </summary>
	public sealed class ProjectSynchronizer
	{
		private const string Ns = "{http://schemas.microsoft.com/developer/msbuild/2003}";
		private static readonly XName ItemGroup = Ns + "ItemGroup";
		private static readonly XName Compile = Ns + "Compile";
		private static readonly XName Link = Ns + "Link";
		private static readonly XName DependentUpon = Ns + "DependentUpon";
		private static readonly XName AutoGen = Ns + "AutoGen";
		private static readonly XName DesignTime = Ns + "DesignTime";
		private static readonly XName Include = XName.Get( "Include" );

		private static readonly Encoding Utf8BomEncoding = new UTF8Encoding( encoderShouldEmitUTF8Identifier: true );

		private readonly Project _inMemory;
		private readonly XDocument _projectXml;
		private readonly ProjectItemGroupElement _targetItemGroup;

		/// <summary>
		///		Initializes a new instance of the <see cref="ProjectSynchronizer"/> class.
		/// </summary>
		/// <param name="project">The project XML.</param>
		/// <param name="projectPath">The project file path to solve pathes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="project"/> is <c>null</c>.</exception>
		public ProjectSynchronizer( XDocument project, string projectPath )
		{
			this._projectXml = project ?? throw new ArgumentNullException( nameof( project ) );
			this._inMemory = new Project { FullPath = projectPath };

			this._targetItemGroup = this._inMemory.Xml.CreateItemGroupElement();
			this._inMemory.Xml.AppendChild( this._targetItemGroup );
			this._inMemory.ReevaluateIfNecessary();
		}

		/// <summary>
		///		Evaluates the specified globs and updates project states.
		/// </summary>
		/// <param name="globs">The globs.</param>
		public void Evaluate( IEnumerable<Glob> globs )
		{
			foreach ( var glob in globs ?? Enumerable.Empty<Glob>() )
			{
				switch ( glob.Type )
				{
					case GlobType.Include:
					{
						var include = this._inMemory.Xml.CreateItemElement( Compile.LocalName, glob.Path );
						this._targetItemGroup.AppendChild( include );
						break;
					}
					case GlobType.Remove:
					{
						var remove = this._inMemory.Xml.CreateItemElement( Compile.LocalName );
						remove.Remove = glob.Path;
						this._targetItemGroup.AppendChild( remove );
						break;
					}
				}
			}
		}

		/// <summary>
		///		Saves this instance to original location.
		/// </summary>
		/// <param name="writer">The target <see cref="TextWriter"/>.</param>
		public void Save( TextWriter writer )
		{
			this._inMemory.ReevaluateIfNecessary();

			var itemGroup = default( XElement );
			var removings = new List<XElement>();
			var dependentUpons = new Dictionary<string, string>();
			var autoGens = new HashSet<string>();
			foreach ( var compile in this._projectXml.Root.Elements( ItemGroup ).Elements( Compile ) )
			{
				if ( itemGroup == null )
				{
					itemGroup = compile.Parent;
				}

				removings.Add( compile );
				var dependentUpon = compile.Element( DependentUpon );
				if ( dependentUpon != null )
				{
					dependentUpons[ compile.Attribute( Include ).Value ] = dependentUpon.Value;
				}

				var autoGen = compile.Element( AutoGen );
				if ( autoGen != null )
				{
					autoGens.Add( compile.Attribute( Include ).Value );
				}
			}

			foreach ( var removing in removings )
			{
				removing.Remove();
			}

			if ( itemGroup == null )
			{
				itemGroup = new XElement( ItemGroup );
				this._projectXml.Root.Add( itemGroup );
			}

			var projectDirectory = Path.GetDirectoryName( this._inMemory.FullPath );
			var added = new HashSet<string>();
			foreach ( var include in this._inMemory.GetItems( Compile.LocalName ).OrderBy( x => x.EvaluatedInclude ) )
			{

				// For old msbuild systems, replace '/' with '\'.
				var pathToItem = include.EvaluatedInclude.Replace( '/', '\\' );

				if ( !added.Add( pathToItem ) )
				{
					// Skip duplicated.
					continue;
				}

				if ( include.EvaluatedInclude.StartsWith( ".." ) )
				{
					itemGroup.Add(
						new XElement(
							Compile,
							new XAttribute( Include, pathToItem ),
							new XElement(
								Link,
								ToProjectRelativePath( pathToItem )
							)
						)
					);
				}
				else
				{
					var compile = new XElement( Compile, new XAttribute( Include, pathToItem ) );
					if ( autoGens.Contains( pathToItem ) )
					{
						compile.Add(
							new XElement( AutoGen, "True" ),
							new XElement( DesignTime, "True" )
						);
					}

					if ( dependentUpons.TryGetValue( pathToItem, out var dependentUpon ) )
					{
						compile.Add(
							new XElement( DependentUpon, dependentUpon )
						);
					}

					itemGroup.Add( compile );
				}
			}

			this._projectXml.Save( writer );
		}

		private static string ToProjectRelativePath( string maybeRelativePath )
		{
			if ( maybeRelativePath.EndsWith( "AssemblyInfo.cs" ) )
			{
				return "Properties\\" + Path.GetFileName( maybeRelativePath );
			}

			var firstSeparator = maybeRelativePath.IndexOf( '\\' );
			var secondSeparator = maybeRelativePath.IndexOf( '\\', firstSeparator + 1 );
			if ( secondSeparator < 0 )
			{
				return Path.GetFileName( maybeRelativePath );
			}

			return maybeRelativePath.Substring( secondSeparator + 1 );
		}

		/// <summary>
		///		Loads the base globs from specified project file.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <returns>Globs. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="project"/> is <c>null</c>.</exception>
		public static IEnumerable<Glob> LoadBaseGlobs( Project project )
			=> ( project ?? throw new ArgumentNullException( nameof( project ) ) )
			.GetItems( Compile.LocalName )
			.Where( x => !String.IsNullOrEmpty( x.Xml.Remove ) && !String.IsNullOrEmpty( x.Xml.Include ) )
			.Select( x =>
				!String.IsNullOrEmpty( x.Xml.Remove )
				? new Glob( x.Xml.Remove, GlobType.Remove )
				: new Glob( x.Xml.Include, GlobType.Include )
			);

		/// <summary>
		///		Do project synchronization with the specified definition.
		/// </summary>
		/// <param name="definition">The definition.</param>
		/// <param name="globalProperties">Global properties.</param>
		/// <exception cref="ArgumentNullException"><paramref name="definition"/> is <c>null</c>.</exception>
		public static void Synchronize( ProjectSynchronizationDefinition definition, IDictionary<string, string> globalProperties )
		{
			if ( definition == null )
			{
				throw new ArgumentNullException( nameof( definition ) );
			}

			var baseProject = new Project();
			foreach ( var property in globalProperties )
			{
				baseProject.SetGlobalProperty( property.Key, property.Value );
			}

			var baseGlobs = LoadBaseGlobs( baseProject );
			var synchronizer =
				new ProjectSynchronizer(
					XDocument.Load( definition.TargetProjectName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo ),
					Path.GetFullPath( definition.TargetProjectName )
				);
			synchronizer.Evaluate( baseGlobs.Concat( definition.GetResolvedGlobs() ) );

			using ( var stream = new MemoryStream() )
			using ( var writer = new StreamWriter( stream, Utf8BomEncoding ) )
			{
				synchronizer.Save( writer );
				writer.Flush();

				File.WriteAllBytes( definition.TargetProjectName, stream.ToArray() );
			}
		}
	}
}
