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
using MsgPack.Serialization.CodeDomSerializers;

namespace MsgPack.Serialization.CodeGenerators
{
	internal sealed class SerializerCodesGenerationLogic : SerializerGenerationLogic<SerializerCodeGenerationConfiguration>
	{
		public static readonly SerializerCodesGenerationLogic Instance = new SerializerCodesGenerationLogic();

		protected override EmitterFlavor EmitterFlavor
		{
			get { return EmitterFlavor.CodeDomBased; }
		}

		private SerializerCodesGenerationLogic() {}

		protected override ISerializerCodeGenerationContext CreateGenerationContext( SerializationContext context, SerializerCodeGenerationConfiguration configuration )
		{
			return new CodeDomContext( context, configuration );
		}

		protected override Func<Type, ISerializerCodeGenerator> CreateGeneratorFactory( SerializationContext context )
		{
			return type => new CodeDomSerializerBuilder( type, type.GetCollectionTraits( CollectionTraitOptions.Full, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) );
		}
	}
}