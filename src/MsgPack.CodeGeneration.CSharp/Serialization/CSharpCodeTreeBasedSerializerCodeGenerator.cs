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

using MsgPack.Serialization.CodeTreeSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Provides front-end for Roslyn Code Analyzer based, portable C# serializer source code generator.
	/// </summary>
	/// <remarks>
	///		You must call <see cref="Configure()"/> in advance to use Roslyn Code Analyzer based C# source code generation.
	///		If you miss it, Code DOM based legacy generator is used in desktop .NET Framework build (including Mono), or failed on other (.NET Core, etc.) based build.
	/// </remarks>
	public static class CSharpCodeTreeBasedSerializerCodeGenerator
	{
		// based on <system.codedom> in root web.config.
		private static readonly string[] LanguageIdentifiers =
			new[] { "C#", "CSharp", "CS" };

		// TODO: VisualBasic are vb;vbs;visualbasic;vbscript

		/// <summary>
		///		Configures C# generator and enables in <see cref="SerializerGenerator"/>.
		/// </summary>
		public static void Configure()
		{
			CodeTreeSerializerCodeGeneratorFactory.EnsureInitialized();
			CodeTreeSerializerCodeGeneratorFactory.RegisterFactory(
				LanguageIdentifiers,
				( context, configuration ) => new CodeTreeContext( context, configuration, "cs" ),
				context => type => new CodeTreeSerializerBuilder( type, type.GetCollectionTraits( CollectionTraitOptions.Full, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) )
			);
		}
	}
}