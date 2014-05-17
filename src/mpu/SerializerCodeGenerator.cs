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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using MsgPack.Serialization;

namespace mpu
{
	/// <summary>
	///		Generates serializer asset codes.
	/// </summary>
	/// <remarks>
	///		This class is NOT formal API, so backward compatibility will not be maintained.
	/// </remarks>
	public sealed class SerializerCodeGenerator
	{
		private readonly SerializerCodeGenerationConfiguration _configuration;

		/// <summary>
		///		Gets the configuration of serializer generator.
		/// </summary>
		/// <value>
		///		The configuration object. This value will not be <c>null</c>.
		/// </value>
		public SerializerCodeGenerationConfiguration Configuration
		{
			get { return this._configuration; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerCodeGenerator"/> class.
		/// </summary>
		/// <param name="configuration">The configuration object.</param>
		public SerializerCodeGenerator( SerializerCodeGenerationConfiguration configuration )
		{
			this._configuration = configuration ?? new SerializerCodeGenerationConfiguration();
		}

		/// <summary>
		///		Generates the serializers using specified assembly file.
		/// </summary>
		/// <param name="sourceAssemblyFile">The source assembly file.</param>
		/// <param name="includingPattern">The including regex pattern. Omitting indicates all public concrete types are included.</param>
		/// <param name="excludingPattern">The excluding regex pattern. Omitting indicates all public concrete types are included.</param>
		/// <returns>File pathes to the generated source codes.</returns>
		/// <remarks>
		///		Specifying both of <paramref name="includingPattern"/> and <paramref name="excludingPattern"/> indicates AND condition. 
		/// </remarks>
		public IEnumerable<string> GenerateSerializers(
			string sourceAssemblyFile,
			string includingPattern,
			string excludingPattern
			)
		{
			return
				this.GenerateSerializers(
					Assembly.LoadFrom( sourceAssemblyFile ),
					includingPattern,
					excludingPattern
					);
		}

		/// <summary>
		///		Generates the serializers using specified assembly file.
		/// </summary>
		/// <param name="sourceAssembly">The source assembly.</param>
		/// <param name="includingPattern">The including regex pattern. Omitting indicates all public concrete types are included.</param>
		/// <param name="excludingPattern">The excluding regex pattern. Omitting indicates all public concrete types are included.</param>
		/// <returns>File pathes to the generated source codes.</returns>
		/// <remarks>
		///		Specifying both of <paramref name="includingPattern"/> and <paramref name="excludingPattern"/> indicates AND condition. 
		/// </remarks>
		public IEnumerable<string> GenerateSerializers(
			Assembly sourceAssembly,
			string includingPattern,
			string excludingPattern
			)
		{
			if ( sourceAssembly == null )
			{
				throw new ArgumentNullException( "sourceAssembly" );
			}

			if ( !String.IsNullOrEmpty( includingPattern ) )
			{
				includingPattern = includingPattern.Trim();
			}

			if ( !String.IsNullOrEmpty( excludingPattern ) )
			{
				excludingPattern = excludingPattern.Trim();
			}

			var includingRegex =
				String.IsNullOrEmpty( includingPattern )
					? null
					: new Regex( includingPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture );

			var excludingRegex =
				String.IsNullOrEmpty( excludingPattern )
					? null
					: new Regex( excludingPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture );

			return
				SerializerGenerator.GenerateCode(
					this._configuration,
					sourceAssembly.GetTypes()
						.Where(
							type =>
								type.IsPublic
								&& !type.IsAbstract
								&& !type.IsInterface
								&& ( includingRegex == null || includingRegex.IsMatch( type.FullName ) )
								&& ( excludingRegex == null || !excludingRegex.IsMatch( type.FullName ) )
						).ToArray()
					);

		}
	}
}