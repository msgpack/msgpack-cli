#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
	///		Determines emitter strategy.
	/// </summary>
	internal enum EmitterFlavor
	{
#if !NETFX_CORE
		/// <summary>
		///		Uses <see cref="SerializationContext"/> in each case of the members (de)serialization.
		///		It may cause more contentions but is available in WP7.
		/// </summary>
		ContextBased,

		/// <summary>
		///		Caches serializers for the members (de)serialization.
		///		It is default.
		/// </summary>
		FieldBased,
#endif
#if !NETFX_35
		/// <summary>
		///		Uses expression tree to (de)serialization.
		///		It may have more overhead but is available in WinRT.
		/// </summary>
		ExpressionBased,
#endif
#if !NETFX_CORE
		/// <summary>
		///		Uses code DOM code generation to (de)serialization.
		///		It requires a long time but prevents runtime code generation at all.
		/// </summary>
		CodeDomBased,
#endif

		/// <summary>
		///		Uses reflection to (de)serialization.
		///		It requires additional resources but may work on most environment.
		/// </summary>
		ReflectionBased
	}
}
