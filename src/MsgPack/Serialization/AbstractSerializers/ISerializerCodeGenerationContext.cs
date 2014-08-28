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
		///		Determines that whether built-in serializer for specified type exists or not.
		/// </summary>
		/// <param name="type">The type for check.</param>
		/// <returns>
		///		<c>true</c> if built-in serializer for specified type exists; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="type"/> is <c>null</c>.
		/// </exception>
		bool BuiltInSerializerExists( Type type );

		/// <summary>
		///		Generates codes for this context.
		/// </summary>
		/// <returns>The path of generated files.</returns>
#if !NETFX_35
		[SecuritySafeCritical]
#endif // !NETFX_35
		IEnumerable<string> Generate();
	}
}