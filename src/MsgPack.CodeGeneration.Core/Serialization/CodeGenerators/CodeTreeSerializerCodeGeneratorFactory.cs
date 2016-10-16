#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke and contributors
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
using System.Collections.Concurrent;
using System.Globalization;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeGenerators
{
	internal sealed class CodeTreeSerializerCodeGeneratorFactory : SerializerCodeGeneratorFactory
	{
		private static readonly string[] GeneratorKeys =
			new[] { "roslyn", "codetree", "codeanalyzer" };

		public static readonly CodeTreeSerializerCodeGeneratorFactory Instance = new CodeTreeSerializerCodeGeneratorFactory();

		private readonly ConcurrentDictionary<string, Factories> _factories = new ConcurrentDictionary<string, Factories>( StringComparer.OrdinalIgnoreCase );

		private CodeTreeSerializerCodeGeneratorFactory() { }

		public static void EnsureInitialized() => SerializerGenerator.RegisterGenerator( GeneratorKeys, Instance );

		public static void RegisterFactory(
			string[] languages,
			Func<SerializationContext, SerializerCodeGenerationConfiguration, ISerializerCodeGenerationContext> contextFactory,
			Func<SerializationContext, Func<Type, ISerializerCodeGenerator>> generatorFactory
		)
		{
			foreach ( var language in languages )
			{
				Instance._factories[ language ] = new Factories( contextFactory, generatorFactory );
			}
		}

		internal override SerializerGenerationLogic<SerializerCodeGenerationConfiguration> Create( SerializerCodeGenerationConfiguration configuration )
		{
			Factories factories;
			if ( !this._factories.TryGetValue( configuration.Language, out factories ) )
			{
				throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Code analyzer based code generator is not registered for language '{0}'",
							configuration.Language
						)
					);
			}
			return new CodeTreeSerializerGenerationLogic( factories.ContextFactory, factories.GeneratorFactory );
		}

		private struct Factories
		{
			public readonly Func<SerializationContext, SerializerCodeGenerationConfiguration, ISerializerCodeGenerationContext> ContextFactory;
			public readonly Func<SerializationContext, Func<Type, ISerializerCodeGenerator>> GeneratorFactory;

			public Factories(
				Func<SerializationContext, SerializerCodeGenerationConfiguration, ISerializerCodeGenerationContext> contextFactory,
				Func<SerializationContext, Func<Type, ISerializerCodeGenerator>> generatorFactory
			)
			{
				this.ContextFactory = contextFactory;
				this.GeneratorFactory = generatorFactory;
			}
		}
	}
}