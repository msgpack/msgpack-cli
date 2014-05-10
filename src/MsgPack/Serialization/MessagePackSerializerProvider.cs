#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines basic features and interfaces for serializer provider which is stored in repository and controlls returning serializer with its own parameter.
	/// </summary>
	internal abstract class MessagePackSerializerProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MessagePackSerializerProvider"/> class.
		/// </summary>
		protected MessagePackSerializerProvider() { }

		/// <summary>
		///		Gets a serializer instance for specified parameter.
		/// </summary>
		/// <param name="context">A serialization context which holds global settings.</param>
		/// <param name="providerParameter">A provider specific parameter.</param>
		/// <returns>A serializer object for specified parameter.</returns>
		public abstract object Get( SerializationContext context, object providerParameter );
	}
}