#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if !SILVERLIGHT && !AOT
		/// <summary>
		///		Caches serializers for the members (de)serialization.
		///		It is default.
		/// </summary>
		FieldBased,

		/// <summary>
		///		Uses Reslyn SyntaxFactory code generation to (de)serialization.
		///		It requires a long time but prevents runtime code generation at all.
		/// </summary>
		CodeTreeBased,

#if !NETSTANDARD1_1
		/// <summary>
		///		Uses code DOM code generation to (de)serialization.
		///		It requires a long time but prevents runtime code generation at all.
		/// </summary>
		CodeDomBased,
#endif // !NETSTANDARD1_1
#endif // !SILVERLIGHT && !AOT

		/// <summary>
		///		Uses reflection to (de)serialization.
		///		It requires additional resources but may work on most environment.
		/// </summary>
		ReflectionBased
	}
}
