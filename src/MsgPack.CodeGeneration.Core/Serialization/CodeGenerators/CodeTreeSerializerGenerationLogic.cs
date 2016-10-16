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

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeGenerators
{
	internal sealed class CodeTreeSerializerGenerationLogic : SerializerGenerationLogic<SerializerCodeGenerationConfiguration>
	{
		private readonly Func<SerializationContext, SerializerCodeGenerationConfiguration, ISerializerCodeGenerationContext> _contextFactory;
		private readonly Func<SerializationContext, Func<Type, ISerializerCodeGenerator>> _generatorFactory;

		public CodeTreeSerializerGenerationLogic(
			Func<SerializationContext, SerializerCodeGenerationConfiguration, ISerializerCodeGenerationContext> contextFactory,
			Func<SerializationContext, Func<Type, ISerializerCodeGenerator>> generatorFactory
		)
		{
			this._contextFactory = contextFactory;
			this._generatorFactory = generatorFactory;
		}

		// Just returns random stuff.
		protected override EmitterFlavor EmitterFlavor => EmitterFlavor.FieldBased;

		protected override ISerializerCodeGenerationContext CreateGenerationContext( SerializationContext context, SerializerCodeGenerationConfiguration configuration )
			=> this._contextFactory( context, configuration );

		protected override Func<Type, ISerializerCodeGenerator> CreateGeneratorFactory( SerializationContext context )
			=> this._generatorFactory( context );
	}
}