#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.CodeDom;

namespace MsgPack.Serialization.CodeDomSerializers
{
	/// <summary>
	///		Defines non-generic portion of <see cref="CodeDomSerializerBuilder{T}"/>.
	/// </summary>
	internal static class CodeDomSerializerBuilder
	{
		public static readonly CodeTypeReference[] EmptyGenericArguments = new CodeTypeReference[ 0 ];
	}
}