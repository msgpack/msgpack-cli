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
using Newtonsoft.Json;

namespace MsgPack.Tools.Build
{
	/// <summary>
	///		Defines project synchronization inputs.
	/// </summary>
	public sealed class ProjectSynchronizationDefinition
	{
		/// <summary>
		///		Gets or sets the name of the target project.
		/// </summary>
		/// <value>
		///		The name of the target project. This value will be updated to full path when <see cref="ResolveProjectName(String, String)"/>.
		/// </value>
		[JsonProperty( "name" )]
		public string TargetProjectName { get; set; }

		/// <summary>
		///		Gets or sets the name of the base definition.
		/// </summary>
		/// <value>
		///		The name of the base definition.
		/// </value>
		[JsonProperty( "base" )]
		public string BaseDefinitionName { get; set; }

		private readonly IList<IList<Glob>> _baseGlobs = new List<IList<Glob>>();

		/// <summary>
		///		Gets the globs.
		/// </summary>
		/// <value>
		///		The globs. This value will not be <c>null</c>.
		/// </value>
		[JsonProperty( "globs" )]
		public IList<Glob> Globs { get; } = new List<Glob>();

		/// <summary>
		///		Initializes a new instance of the <see cref="ProjectSynchronizationDefinition"/> class.
		/// </summary>
		public ProjectSynchronizationDefinition() { }

		/// <summary>
		///		Creates new instances from specified JSON data.
		/// </summary>
		/// <param name="reader">The reader for JSON text.</param>
		/// <returns>A collection of deserialized <see cref="ProjectSynchronizationDefinition"/>.</returns>
		public static IEnumerable<ProjectSynchronizationDefinition> FromJson( TextReader reader )
			=> new JsonSerializer().Deserialize<ProjectSynchronizationDefinition[]>( new JsonTextReader( reader ) );

		/// <summary>
		///		Resolves the project names to full path with specified informations.
		/// </summary>
		/// <param name="baseDirectory">The base directory of resolution.</param>
		/// <param name="projectExtension">The project file extension including leading dot. If the leading dot is missing, dot should be prepended.</param>
		/// <remarks>
		///		If the original <see cref="TargetProjectName"/> matches mulitiple project files, the result is undefined.
		///		This method updates <see cref="TargetProjectName"/> properties.
		/// </remarks>
		public void ResolveProjectName( string baseDirectory, string projectExtension )
		{
			var realProjectExtension = projectExtension;
			if ( realProjectExtension.FirstOrDefault() != '.' )
			{
				realProjectExtension = '.' + realProjectExtension;
			}

			foreach ( var candidate in Directory.GetFiles( baseDirectory, "*" + realProjectExtension, SearchOption.AllDirectories ) )
			{
				var projectName = Path.GetFileNameWithoutExtension( candidate );
				if ( projectName == this.TargetProjectName )
				{
					this.TargetProjectName = candidate;
				}
			}
		}

		/// <summary>
		///		Gets resolved globs for base definition hiearchy.
		/// </summary>
		/// <returns>Resolved globs for base definition hiearchy.</returns>
		public IEnumerable<Glob> GetResolvedGlobs()
			=> this._baseGlobs.Reverse().SelectMany( x => x ).Concat( this.Globs );

		/// <summary>
		///		Resolves base definition hiearchy.
		/// </summary>
		/// <param name="baseResolver">A delegate to get base definition by its name.</param>
		/// <exception cref="ArgumentNullException"><paramref name="baseResolver"/> is <c>null</c>.</exception>
		public void ResolveBase( Func<string, ProjectSynchronizationDefinition> baseResolver )
		{
			if ( baseResolver == null )
			{
				throw new ArgumentNullException( nameof( baseResolver ) );
			}

			this._baseGlobs.Clear();

			var baseName = this.BaseDefinitionName;
			while ( !String.IsNullOrEmpty( baseName ) )
			{
				var baseDefinition = baseResolver( baseName );
				if ( baseDefinition != null )
				{
					this._baseGlobs.Add( baseDefinition.Globs );
				}

				baseName = baseDefinition?.BaseDefinitionName;
			}
		}
	}
}
