#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		An <see cref="ISerializerCodeGenerationContext"/> for <see cref="AssemblyBuilderSerializerBuilder{TObject}"/>.
	/// </summary>
	internal class AssemblyBuilderCodeGenerationContext : ISerializerCodeGenerationContext
	{
		private readonly SerializationContext _context;
		private readonly SerializationMethodGeneratorManager _generatorManager;
		private readonly AssemblyBuilder _assemblyBuilder;
		private readonly string _directory;
		private readonly List<SerializerSpecification> _generatedSerializers;

		public AssemblyBuilderCodeGenerationContext( SerializationContext context, AssemblyBuilder assemblyBuilder, SerializerAssemblyGenerationConfiguration configuration )
		{
			this._context = context;
			this._assemblyBuilder = assemblyBuilder;

			DefaultSerializationMethodGeneratorManager.SetUpAssemblyBuilderAttributes( assemblyBuilder, false );
			this._generatorManager = SerializationMethodGeneratorManager.Get( assemblyBuilder );
			this._directory = configuration.OutputDirectory;
			this._generatedSerializers = new List<SerializerSpecification>();
		}

		/// <summary>
		///		Create new <see cref="AssemblyBuilderEmittingContext"/> for specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">The target type of the serializer.</param>
		/// <param name="targetTypeCollectionTraits">The collection traits of <paramref name="targetType"/>.</param>
		/// <param name="serializerBaseClass">The base class of the serializer.</param>
		/// <returns><see cref="AssemblyBuilderEmittingContext"/>.</returns>
		public AssemblyBuilderEmittingContext CreateEmittingContext( Type targetType, CollectionTraits targetTypeCollectionTraits, Type serializerBaseClass )
		{
			string serializerTypeName, serializerTypeNamespace;
			DefaultSerializerNameResolver.ResolveTypeName(
				this._assemblyBuilder == null,
				targetType,
				typeof( AssemblyBuilderCodeGenerationContext ).Namespace,
				out serializerTypeName,
				out serializerTypeNamespace 
			);
			var spec =
				new SerializerSpecification(
					targetType,
					targetTypeCollectionTraits,
					serializerTypeName,
					serializerTypeNamespace
				);

			this._generatedSerializers.Add( spec );

			return
				new AssemblyBuilderEmittingContext(
					this._context,
					targetType,
					() => this._generatorManager.CreateEmitter( spec, serializerBaseClass, EmitterFlavor.FieldBased ),
					() => this._generatorManager.CreateEnumEmitter( this._context, spec, EmitterFlavor.FieldBased ) 
				);
		}

		/// <summary>
		///		Generates codes for this context.
		/// </summary>
		/// <returns>A <see cref="SerializerCodeGenerationResult"/> collection which correspond to genereated codes.</returns>
		public IEnumerable<SerializerCodeGenerationResult> Generate()
		{
			var assemblyFileName = this._assemblyBuilder.GetName().Name + ".dll";
			this._assemblyBuilder.Save( assemblyFileName );
			var assemblyFilePath =
				Path.GetFullPath(
					Path.Combine(
						this._directory,
						assemblyFileName
					)
				);
			return
				this._generatedSerializers.Select(
					s =>
						new SerializerCodeGenerationResult(
							s.TargetType,
							assemblyFilePath,
							s.SerializerTypeFullName,
							s.SerializerTypeNamespace,
							s.SerializerTypeName
						) 
				);
		}
	}
}