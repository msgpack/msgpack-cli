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
	///		Defines a common interface for code construct which abstracts code constructs used in serializer builders.
	/// </summary>
	internal interface ICodeConstruct
	{
		/// <summary>
		///		Gets the context type of this construct.
		/// </summary>
		/// <value>
		///		The context type of this construct.
		///		This value will not be <c>null</c>, but might be <see cref="Void"/>.
		/// </value>
		/// <remarks>
		///		A context type represents evaluation context for IL emitting or expression type for Expression Tree.
		/// </remarks>
		Type ContextType { get; }
	}
}