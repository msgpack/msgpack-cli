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
	/// <summary>
	///		Defines common interfaces and features for context objects for serializer generation.
	/// </summary>
	/// <typeparam name="TConstruct">The contextType of the code construct for serializer builder.</typeparam>
	internal abstract class SerializerGenerationContext<TConstruct>
	{
		/// <summary>
		///		Gets the serialization context which holds various serialization configuration.
		/// </summary>
		/// <value>
		///		The serialization context. This value will not be <c>null</c>.
		/// </value>
		internal SerializationContext SerializationContext { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the packing target object tree root.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the packing target object tree root.
		///		This value will not be <c>null</c>.
		/// </value>
		internal TConstruct PackToTarget { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the packer.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the packer.
		///		This value will not be <c>null</c>.
		/// </value>
		internal TConstruct Packer { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the unpacker.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the unpacker.
		///		This value will not be <c>null</c>.
		/// </value>
		internal TConstruct Unpacker { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the collection which will hold unpacked items.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the collection which will hold unpacked items.
		///		This value will not be <c>null</c>.
		/// </value>
		internal TConstruct UnpackToTarget { get; private set; }

		/// <summary>
		///		Gets the configured nil-implication for collection items.
		/// </summary>
		/// <value>
		///		The configured nil-implication for collection items.
		/// </value>
		public NilImplication CollectionItemNilImplication { get; private set; }

		/// <summary>
		///		Gets the configured nil-implication for dictionary keys.
		/// </summary>
		/// <value>
		///		The configured nil-implication for dictionary keys.
		/// </value>
		public NilImplication DictionaryKeyNilImplication { get; private set; }

		// NOTE: Missing map value is MemberDefault

		/// <summary>
		///		Gets the configured nil-implication for tuple items.
		/// </summary>
		/// <value>
		///		The configured nil-implication for tuple items.
		/// </value>
		public NilImplication TupleItemNilImplication { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerGenerationContext{TConstruct}"/> class.
		/// </summary>
		/// <param name="serializationContext">The serialization context.</param>
		/// <param name="packer">
		///		The code construct which represents the argument for the packer.
		///	</param>
		/// <param name="packToTarget">
		///		The code construct which represents the argument for the packing target object tree root.
		/// </param>
		/// <param name="unpacker">
		///		The code construct which represents the argument for the unpacker.
		/// </param>
		/// <param name="unpackToTarget">
		///		The code construct which represents the argument for the collection which will hold unpacked items.
		/// </param>
		protected SerializerGenerationContext( SerializationContext serializationContext, TConstruct packer, TConstruct packToTarget, TConstruct unpacker, TConstruct unpackToTarget )
		{
			this.SerializationContext = serializationContext;
			this.Packer = packer;
			this.PackToTarget = packToTarget;
			this.Unpacker = unpacker;
			this.UnpackToTarget = unpackToTarget;
			this.CollectionItemNilImplication = NilImplication.Null;
			this.DictionaryKeyNilImplication = NilImplication.Prohibit;
			this.TupleItemNilImplication = NilImplication.Null;
		}
	}
}