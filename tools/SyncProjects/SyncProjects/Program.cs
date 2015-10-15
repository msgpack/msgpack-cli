using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using NDesk.Options;

namespace SyncProjects
{
	// TODO: This tool might not work for Empty destination projects.
	class Program
	{
		private const string Ns = "{http://schemas.microsoft.com/developer/msbuild/2003}";

		static int Main( string[] args )
		{
			var file = "Sync.xml";
			var sourceBasePath = "src" + Path.DirectorySeparatorChar;
			var projectExtension = ".csproj";
			bool help = false;

			var options =
				new OptionSet
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

		private static readonly XElement EmptyExcludes = new XElement( "Excludes" );

		private static void SynchronizeProjects( string syncFilePath, string sourceBasePath, string projectFileExtension )
		{
			var sync = XDocument.Load( syncFilePath );
			if ( sync.Root == null || sync.Root.Name.LocalName != "ProjectSync" )
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
							Includes =
								p.Elements( "Include" )
								.Select( ToPattern ).Where( pattern => pattern != null ).ToArray(),
							Excludes =
								p.Elements( "Exclude" )
								.Select( ToPattern ).Where( pattern => pattern != null ).ToArray(),
							Preserves =
								p.Elements( "Preserve" )
								.Select( ToPattern ).Where( pattern => pattern != null ).ToArray()
						}
					) )
			{
				var projectFilePath = Path.Combine( sourceBasePath, project.Name.Value, project.Name.Value + projectFileExtension );
				var projectXml = XDocument.Load( projectFilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo );
				if ( projectXml.Root == null || projectXml.Root.Name.LocalName != "Project" )
				{
					throw new XmlException( "Invalid project file :" + projectFilePath );
				}

				var baseProjectFilePath = Path.Combine( sourceBasePath, project.Base.Value, project.Base.Value + projectFileExtension );
				var baseProjectXml = XDocument.Load( baseProjectFilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo );
				if ( baseProjectXml.Root == null || baseProjectXml.Root.Name.LocalName != "Project" )
				{
					throw new XmlException( "Invalid project file :" + baseProjectFilePath );
				}

				var relativePath =
					GetRelativePath(
						Path.GetDirectoryName( projectFilePath ),
						Path.GetDirectoryName( baseProjectFilePath )
					);

				var targetItemGroups =
					baseProjectXml.Root
					.Elements( Ns + "ItemGroup" )
					.Select( ig =>
						{
							ig.Elements()
								.Where(
									e =>
									e.Attribute( "Include" ) != null
									&& project.Excludes.Any( excludes =>
										e.Element( Ns + "Link" ) != null
										? Regex.IsMatch( e.Element( Ns + "Link" ).Value, excludes )
										: Regex.IsMatch( e.Attribute( "Include" ).Value, excludes )
									)
								).Remove();
							return ig;
						}
					).Where( ig => ig.Elements().Any() )
					.Select( e => new ItemGroup( e ) );
				var baseItemGroups = baseProjectXml.Root.Elements( Ns + "ItemGroup" ).SelectMany( ig => ig.Elements() ).ToLookup( e => e.Name.LocalName );

				var existingItemGroups =
					projectXml.Root.Elements( Ns + "ItemGroup" )
						.Where( e => e.HasElements )
						.Select( e => new ItemGroup( e ) )
						.ToDictionaryDebuggable( e => e.Key, projectFilePath );
				foreach ( var itemGroup in targetItemGroups )
				{
					switch ( itemGroup.Key )
					{
						case "Compile":
						case "Content":
						case "None":
						{
							// If there are no <ItemGroup> for current type, then create new type and register it to the document and bookkeeping.
							XElement destinationElement;
							ItemGroup destinationGroup;
							if ( existingItemGroups.TryGetValue( itemGroup.Key, out destinationGroup ) )
							{
								destinationElement = destinationGroup.Element;
							}
							else
							{
								destinationElement = new XElement( Ns + "ItemGroup" );
								// insert to the destination DOM
								projectXml.Root.Elements( Ns + "ItemGroup" ).Last().AddAfterSelf( destinationElement );
								// bookkeeping
								existingItemGroups.Add( itemGroup.Key, new ItemGroup( destinationElement, itemGroup.Key ) );
							}

							CopyItemGroup(
								itemGroup.Key,
								baseItemGroups[ itemGroup.Key ],
								destinationElement,
								relativePath,
								project.Includes,
								project.Excludes,
								project.Preserves
							);
							break;
						}
					}
				}

				// Avoid empty ItemGroups
				projectXml.Root.Elements( Ns + "ItemGroup" ).Where( ig => !ig.HasElements ).Remove();

				projectXml.Save( projectFilePath );
			}
		}

		private static void CopyItemGroup( string elementName, IEnumerable<XElement> sourceItems, XElement destinationItemGroup, string relativePath, IEnumerable<string> includings, IEnumerable<string> excludings, IEnumerable<string> preservings )
		{
			var remaining =
				destinationItemGroup.Elements( Ns + elementName )
				.Where( e =>
					e.Attribute( "Include" ) != null &&
					preservings.Any( preserves =>
						e.Element( Ns + "Link" ) != null
						? Regex.IsMatch( e.Element( Ns + "Link" ).Value, preserves )
						: Regex.IsMatch( e.Attribute( "Include" ).Value, preserves )
					)
				).Select( e => new XElement( e ) )
				.ToArray();

			var appendings =
				sourceItems
				.Where( e =>
					e.Attribute( "Include" ) != null &&
					includings.Any( includes =>
						Regex.IsMatch( e.Attribute( "Include" ).Value, includes )
					)
				).Select( e => CreateCopyingElement( relativePath, e ) )
				.ToArray();

			destinationItemGroup.Elements( Ns + elementName ).Remove();

			var adding = new Dictionary<string, XElement>();

			foreach ( var item in
				sourceItems.Where( e =>
					e.Attribute( "Include" ) != null &&
					!excludings.Any( excludes =>
						e.Element( Ns + "Link" ) != null
						? Regex.IsMatch( e.Element( Ns + "Link" ).Value, excludes )
						: Regex.IsMatch( e.Attribute( "Include" ).Value, excludes )
					) &&
					!preservings.Any( preserves =>
						e.Element( Ns + "Link" ) != null
						? Regex.IsMatch( e.Element( Ns + "Link" ).Value, preserves )
						: Regex.IsMatch( e.Attribute( "Include" ).Value, preserves )
					) 
				).Select( copying =>
					CreateCopyingElement( relativePath, copying )
				).Concat( remaining ).Concat( appendings ) )
			{
				var itemPath = item.Attribute( "Include" ).Value;
				if ( !adding.ContainsKey( itemPath ) )
				{
					adding.Add( itemPath, item );
				}
			}

			// To stable order, sort with their "display path".
			destinationItemGroup.Add(
				adding.OrderBy( kv =>
					( kv.Value.Element( "Link" ) == null ? kv.Key : kv.Value.Element( "Link" ).Value ),
					StringComparer.OrdinalIgnoreCase
				).Select( kv => kv.Value )
			);
		}

		private static XElement CreateCopyingElement( string relativePath, XElement copying )
		{
			return
				copying.Element( Ns + "Link" ) != null
				? new XElement( copying )
				: new XElement(
					copying.Name,
					new XAttribute(
						"Include",
						Path.Combine( relativePath, copying.Attribute( "Include" ).Value )
						),
					new XElement( Ns + "Link", copying.Attribute( "Include" ).Value )
				);
		}

		#region String Utlities

		private static readonly char[] DirectorySeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
		private static readonly string DirectorySeparatorPattern =
			"(" + Regex.Escape( Path.AltDirectorySeparatorChar.ToString() ) + "|" + Regex.Escape( Path.DirectorySeparatorChar.ToString() ) + ")";

		private static string ToPattern( XElement xml )
		{
			var file = xml.Attribute( "File" );
			var path = xml.Attribute( "Path" );
			if ( file == null && path == null )
			{
				return null;
			}

			if ( file == null )
			{
				return
					"^" +
					String.Join(
						DirectorySeparatorPattern,
						path.Value.Split( DirectorySeparators, StringSplitOptions.RemoveEmptyEntries ).Select( ToPathRegex )
					) +
					"$";
			}
			else
			{
				var filePattern = ToFileRegex( ( file.Value ) );
				return "(^" + filePattern + "|" + DirectorySeparatorPattern + filePattern + ")$";
			}
		}

		private static string ToPathRegex( string wildcard )
		{
			// Wildcard in path should not contain directory separator.
			return Regex.Escape( wildcard ).Replace( "\\*\\*", ".*" ).Replace( "\\*", @"[^\\]*" ).Replace( "\\?", @"[^\\]" );
		}

		private static string ToFileRegex( string wildcard )
		{
			return Regex.Escape( wildcard ).Replace( "\\*", ".*" ).Replace( "\\?", "." );
		}

		private static string GetRelativePath( string fromPath, string toPath )
		{
			var fromPathSegments = fromPath.Split( DirectorySeparators, StringSplitOptions.RemoveEmptyEntries );
			var toPathSegments = toPath.Split( DirectorySeparators, StringSplitOptions.RemoveEmptyEntries );
			var sameTo = 0;
			for ( ; sameTo < fromPathSegments.Length && sameTo < toPathSegments.Length; sameTo++ )
			{
				if ( !fromPathSegments[ sameTo ].Equals( toPathSegments[ sameTo ], StringComparison.OrdinalIgnoreCase ) )
				{
					break;
				}
			}

			return String.Join( Path.DirectorySeparatorChar.ToString( CultureInfo.InvariantCulture ), Enumerable.Repeat( "..", fromPathSegments.Length - sameTo ).Concat( toPathSegments.Skip( sameTo ) ) );
		}

		#endregion

		private class ItemGroup
		{
			public readonly XElement Element;
			public readonly string Key;

			public ItemGroup( XElement newItemGroup, string key )
			{
				this.Element = newItemGroup;
				this.Key = key;
			}

			public ItemGroup( XElement itemGroup )
			{
				this.Element = itemGroup;
				try
				{
					this.Key = itemGroup.Elements().Select( e => e.Name.LocalName ).Distinct().Single();
				}
				catch ( InvalidOperationException )
				{
					var lineInfo = ( ( itemGroup ) as IXmlLineInfo ?? NullXmlLineInfo.Instance );
					throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"ItemGroup at line {0} of xml file '{1}' contains hetro genious children [{2}].",
							lineInfo.LineNumber,
							itemGroup.Document.BaseUri,
							String.Join( ", ", itemGroup.Elements().Select( e => e.Name.LocalName ).Distinct() )
						)
					);
				}
			}
		}

		private sealed class NullXmlLineInfo : IXmlLineInfo
		{
			public static readonly NullXmlLineInfo Instance = new NullXmlLineInfo();

			public bool HasLineInfo()
			{
				return false;
			}

			public int LineNumber
			{
				get { return -1; }
			}

			public int LinePosition
			{
				get { return -1; }
			}
		}

	}

	internal static class EnumerableEx
	{
		public static Dictionary<TKey, T> ToDictionaryDebuggable<T, TKey>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			string filePath
		)
		{
			var dictionary = new Dictionary<TKey,T>();
			foreach ( var item in source )
			{
				var key = keySelector( item );
				try
				{
					dictionary.Add( key, item );
				}
				catch ( ArgumentException ex )
				{
					throw new InvalidOperationException(
						String.Format( 
							CultureInfo.CurrentCulture,
							"Failed to process file '{0}'. Key '{1}' is duplicated.",
							filePath,
							key
						),
						ex
					);
				}
			}

			return dictionary;
		}
	}
}
