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
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.EmittingSerializers;

namespace MsgPack.Serialization.CodeGenerators
{
	internal sealed class SerializerAssemblyGenerationLogic : SerializerGenerationLogic<SerializerAssemblyGenerationConfiguration>
	{
		protected override EmitterFlavor EmitterFlavor
		{
			get { return EmitterFlavor.FieldBased; }
		}

		public SerializerAssemblyGenerationLogic() {}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ISerializerCodeGenerationContext CreateGenerationContext( SerializationContext context, SerializerAssemblyGenerationConfiguration configuration )
		{
			return
				new AssemblyBuilderCodeGenerationContext(
					context,
					AppDomain.CurrentDomain.DefineDynamicAssembly(
						configuration.AssemblyName,
						AssemblyBuilderAccess.RunAndSave,
						configuration.OutputDirectory
					),
					configuration
				);
		}

		protected override Func<Type, ISerializerCodeGenerator> CreateGeneratorFactory( SerializationContext context )
		{
			return type => new AssemblyBuilderSerializerBuilder( type, type.GetCollectionTraits( CollectionTraitOptions.Full, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) );
		}
	}
}