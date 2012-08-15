using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using NDesk.Options;
using System.Runtime.InteropServices;

namespace SyncProjects
{
	class Program
	{
		/*
		 * 		現状：
			.NET 4 -> Mono ＝すべて
			.NET 4.5 -> Mono ＝すべて
			.NET 4 -> SL4/SL5 = 分割、すべて
			.NET 4 -> WinRT = 分割、Emitting 以外
			.NET 4 -> WP7.5 = 分割、Expression 以外
		パターンファイル
			<ProjectSync
				>
				<Project 
					Name = "..."
					>
					<Excludes>
						<Exclude Path="Properties\AssemblyInfo.cs" />
						<Exclude Name="*.tt" />

			ItemGroup の Compile, Content, None 子要素をコピーする。Link があれば、丸ごと。そうでなければ、Link を生成する。
			出力は Include 属性のアルファベット順とする。
			Compile、Content、None それぞれで ItemGroup を作る。
		 * 
		 * 
		 * 
		 */

		static int Main( string[] args )
		{
			var file = "Sync.xml";
			var sourceBasePath = "src" + Path.DirectorySeparatorChar;
			var projectExtension = ".csproj";
			bool help = false;

			var options =
				new OptionSet()
				{
					{ "d|def=", "File path to synchronization definition. Default: Sync.xml", v => file = v },
					{ "s|src=", "File path to base directory of source tree. Default: src" + Path.DirectorySeparatorChar, v => sourceBasePath = v },
					{ "e|ext=", "Extension (including leading dot) of the project file. Default: .csproj", v => projectExtension = v },
					{ "h|?|help", "Show this help.", _ => help = true },
				};

			options.Parse( args );
			if ( help )
			{
				Console.WriteLine( "SyncProjects" );
				Console.WriteLine();
				Console.WriteLine( "Usage: SyncProjects.exe [<OPTIONS>]" );
				Console.WriteLine();
				Console.WriteLine( "Options:" );
				options.WriteOptionDescriptions( Console.Out );
				return 0;
			}

			try
			{
				SynchronizeProjects( file, sourceBasePath, projectExtension );
				return 0;
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( ex );
				return Marshal.GetHRForException( ex );
			}
		}

		private static void SynchronizeProjects( string syncFilePath, string sourceBasePath, string projectFileExtension )
		{
			var sync = XDocument.Load( syncFilePath );
			if ( sync.Root.Name.LocalName != "ProjectSync" )
			{
				throw new XmlException( "Invalid sync file." );
			}

			foreach ( var project in
				sync.Elements( "Project" )
					.Select( p =>
						new
						{
							Name = p.Attribute( "Name" ),
							Base = p.Attribute( "Base" ),
							Excludes =
								p.Element( "Excludes" )
								.Elements( "Exclude" )
								.Select( e => ToExcluding( e ) ).Where( ex => ex != null ).ToArray()
						}
					) )
			{
				var projectFilePath = Path.Combine( sourceBasePath, project.Name.Value, project.Name.Value + projectFileExtension );
				var projectXml = XDocument.Load( projectFilePath );
				if ( projectXml.Root.Name.LocalName != "Project" )
				{
					throw new XmlException( "Invalid project file :" + projectXml.BaseUri );
				}

				var baseProjectXml = XDocument.Load( Path.Combine( sourceBasePath, project.Base.Value, project.Base.Value + projectFileExtension ) );
				if ( baseProjectXml.Root.Name.LocalName != "Project" )
				{
					throw new XmlException( "Invalid project file :" + baseProjectXml.BaseUri );
				}

				var baseItemGroups = ToItemGroupLookup( baseProjectXml.Root );

				var itemGroups = ToItemGroupLookup( projectXml.Root );
				foreach ( var itemGroup in itemGroups )
				{
					switch ( itemGroup.Key )
					{
						case "Compile":
						case "Content":
						case "None":
						{
							CopyItemGroup( baseItemGroups[ itemGroup.Key ], itemGroup.Value, project.Excludes );
							break;
						}
					}
				}

				projectXml.Save( projectFilePath );
			}
		}

		private static IDictionary<string, XElement> ToItemGroupLookup( XElement projectXml )
		{
			try
			{
				return projectXml.Elements( "ItemGroup" ).ToDictionary( ig => ig.Elements().Distinct().Single().Name.LocalName, ig => ig );
			}
			catch ( InvalidOperationException ex )
			{
				throw new XmlException( "Unexpected <ItemGroup> grouping.", ex );
			}
		}

		private static void CopyItemGroup( XElement sourceItemGroup, XElement destinationItemGroup, IEnumerable<string> excluding )
		{
			var remaining =
				destinationItemGroup.Elements()
				.Where( e => excluding.Any( ex => Regex.IsMatch( e.Attribute( "Include" ).Value, ex ) ) )
				.Select( e => new XElement( e ) )
				.ToArray();

			destinationItemGroup.RemoveNodes();
			destinationItemGroup.Add( remaining );

			foreach ( var copying in
				sourceItemGroup.Elements()
				.Where( e => !excluding.Any( ex => Regex.IsMatch( e.Attribute( "Include" ).Value, ex ) ) ) )
			{
				if ( copying.Element( "Link" ) != null )
				{
					destinationItemGroup.Add( new XElement( copying ) );
				}
				else
				{
					destinationItemGroup.Add( new XElement( copying.Name, copying.Attribute( "Include" ), new XElement( "Link", Path.GetFileName( copying.Attribute( "Include" ).Value ) ) ) );
				}
			}
		}

		private static readonly char[] _directorySeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
		private static readonly string _directorySeparatorPattern =
			"(" + Regex.Escape( Path.AltDirectorySeparatorChar.ToString() ) + "|" + Regex.Escape( Path.DirectorySeparatorChar.ToString() ) + ")";

		private static string ToExcluding( XElement xml )
		{
			var name = xml.Attribute( "Name" );
			var path = xml.Attribute( "Path" );
			if ( name == null && path == null )
			{
				return null;
			}

			if ( name == null )
			{
				return
					"^" +
					String.Join(
						_directorySeparatorPattern,
						path.Value.Split( _directorySeparators, StringSplitOptions.RemoveEmptyEntries ).Select( segment => ToRegex( segment ) )
					) +
					"$";
			}
			else
			{
				return _directorySeparatorPattern + Regex.Escape( ToRegex( name.Value ) ) + "$";
			}
		}

		private static string ToRegex( string wildcard )
		{
			return Regex.Escape( wildcard ).Replace( "\\*", ".*" ).Replace( "\\?", "." );
		}
	}
}
