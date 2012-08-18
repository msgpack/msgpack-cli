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
		private const string _ns = "{http://schemas.microsoft.com/developer/msbuild/2003}";

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

		private static readonly XElement _emptyExcludes = new XElement( "Excludes" );

		private static void SynchronizeProjects( string syncFilePath, string sourceBasePath, string projectFileExtension )
		{
			var sync = XDocument.Load( syncFilePath );
			if ( sync.Root.Name.LocalName != "ProjectSync" )
			{
				throw new XmlException( "Invalid sync file." );
			}

			foreach ( var project in
				sync.Root.Elements( "Project" )
					.Select( p =>
						new
						{
							Name = p.Attribute( "Name" ),
							Base = p.Attribute( "Base" ),
							Excludes =
								( p.Element( "Excludes" ) ?? _emptyExcludes )
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
							// まじめに組みなおさないとダメ。ItemGroup で全部まとめられてしまっている？
							CopyItemGroup( baseItemGroups[ itemGroup.Key ], itemGroup, project.Excludes );
							break;
						}
					}
				}

				projectXml.Save( projectFilePath );
			}
		}

		private static ILookup<string, XElement> ToItemGroupLookup( XElement projectXml )
		{
			try
			{
				return projectXml.Elements( _ns + "ItemGroup" ).SelectMany( ig => ig.Elements() ).ToLookup( e => e.Name.LocalName );
			}
			catch ( InvalidOperationException ex )
			{
				throw new XmlException( "Unexpected <ItemGroup> grouping. " + String.Join( ", ", projectXml.Elements( _ns + "ItemGroup" ).SelectMany( e => e.Elements() ).Select( e => e.Name.LocalName ).Distinct() ), ex );
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
				if ( copying.Element( _ns + "Link" ) != null )
				{
					destinationItemGroup.Add( new XElement( copying ) );
				}
				else
				{
					destinationItemGroup.Add( new XElement( copying.Name, copying.Attribute( "Include" ), new XElement( _ns + "Link", Path.GetFileName( copying.Attribute( "Include" ).Value ) ) ) );
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
