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

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines a common interface for serializer builder.
	/// </summary>
	/// <typeparam name="TObject">The type of the serializing object.</typeparam>
	internal interface ISerializerBuilder<TObject>
	{
		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information.</param>
		/// <param name="concreteType">The substitution type if <typeparamref name="TObject"/> is abstract type. <c>null</c> when <typeparamref name="TObject"/> is not abstract type.</param>
		/// <param name="schema">The schema which contains schema for collection items, dictionary keys, or tuple items. This value may be <c>null</c>.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		MessagePackSerializer<TObject> BuildSerializerInstance( SerializationContext context, Type concreteType, PolymorphismSchema schema );
	}
}