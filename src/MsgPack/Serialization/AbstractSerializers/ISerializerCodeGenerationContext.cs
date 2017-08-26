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
using System.Security;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines common interface for context objects of serializer code generation.
	/// </summary>
	internal interface ISerializerCodeGenerationContext
	{
		/// <summary>
		///		Generates codes for this context.
		/// </summary>
		/// <returns>A <see cref="SerializerCodeGenerationResult"/> collection which correspond to genereated codes.</returns>
#if !NET35
		[SecuritySafeCritical]
#endif // !NET35
		IEnumerable<SerializerCodeGenerationResult> Generate();

		/// <summary>
		///		Gets the serialization context which holds various serialization configuration.
		/// </summary>
		/// <value>
		///		The serialization context. This value will not be <c>null</c>.
		/// </value>
		SerializationContext SerializationContext
		{
#if !NET35
			[SecuritySafeCritical]
#endif // !NET35
			get;
		}
	}
}