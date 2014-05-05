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
	///		Marks that this enum type has special characteristics on MessagePack serialization.
	/// </summary>
	/// <remarks>
	///		Enum types which are not marked with this attribute will be serialized as <see cref="SerializationContext.EnumSerializationMethod"/> value.
	/// </remarks>
	[AttributeUsage( AttributeTargets.Enum, Inherited = false, AllowMultiple = false )]
	public sealed class MessagePackEnumAttribute : Attribute
	{
		/// <summary>
		///		Gets or sets the default serialization method for this enum type.
		/// </summary>
		/// <value>
		///		The default serialization method for this enum type.
		///		Note that the method for individual enum typed members will be overrided with <see cref="MessagePackEnumMemberAttribute"/>.
		/// </value>
		public EnumSerializationMethod SerializationMethod { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackEnumAttribute"/> class.
		/// </summary>
		public MessagePackEnumAttribute() { }
	}
}