#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.AbstractSerializers
{
	internal abstract class SerializerGenerationContext<TConstruct>
	{
		internal SerializationContext SerializationContext { get; private set; }
		internal TConstruct PackingTarget { get; private set; }
		internal TConstruct Packer { get; private set; }
		internal TConstruct Unpacker { get; private set; }
		internal TConstruct UnpackToTarget { get; private set; }

		public NilImplication CollectionItemNilImplication { get; private set; }
		public NilImplication DictionaryKeyNilImplication { get; private set; }
		public NilImplication TupleItemNilImplication { get; private set; }
		// NOTE: Missing map value is MemberDefault

		protected SerializerGenerationContext( SerializationContext serializationContext, TConstruct packer, TConstruct packingTarget, TConstruct unpacker, TConstruct unpackToTarget )
		{
			this.SerializationContext = serializationContext;
			this.Packer = packer;
			this.PackingTarget = packingTarget;
			this.Unpacker = unpacker;
			this.UnpackToTarget = unpackToTarget;
			this.CollectionItemNilImplication = NilImplication.Null;
			this.DictionaryKeyNilImplication = NilImplication.Prohibit;
			this.TupleItemNilImplication = NilImplication.Null;
		}
	}

	internal interface ISerializerCodeGenerationContext
	{
#warning TODO: Versioning
		Version Version { get; set; }
	}
}