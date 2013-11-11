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

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Implements <see cref="SerializerGenerationContext{TConstruct}"/> for <see cref="ILEmittingSerializerBuilder{TContext,TObject}"/>.
	/// </summary>
	internal class ILEmittingContext : SerializerGenerationContext<ILConstruct>
	{
		/// <summary>
		///		Gets or sets the <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </summary>
		/// <value>
		///		The <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </value>
		internal TracingILGenerator IL { get; set; }

		/// <summary>
		///		Gets the type of the generating serializer.
		/// </summary>
		/// <value>
		///		The type of the generating serializer.
		/// </value>
		internal Type SerializerType { get; private set; }

		/// <summary>
		///		Gets the <see cref="SerializerEmitter"/>.
		/// </summary>
		/// <value>
		///		The <see cref="SerializerEmitter"/>.
		/// </value>
		internal SerializerEmitter Emitter { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ILEmittingContext"/> class.
		/// </summary>
		/// <param name="serializationContext">The serialization context.</param>
		/// <param name="targetType">
		///		The type of the serializer target.
		/// </param>
		/// <param name="emitter">
		///		The <see cref="SerializerEmitter"/> to be used.
		/// </param>
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
		public ILEmittingContext(
			SerializationContext serializationContext,
			Type targetType,
			SerializerEmitter emitter,
			ILConstruct packer,
			ILConstruct packToTarget,
			ILConstruct unpacker,
			ILConstruct unpackToTarget 
		) : base( serializationContext, packer, packToTarget, unpacker, unpackToTarget )
		{
			this.SerializerType = typeof( MessagePackSerializer<> ).MakeGenericType( targetType );
			this.Emitter = emitter;
		}
	}
}