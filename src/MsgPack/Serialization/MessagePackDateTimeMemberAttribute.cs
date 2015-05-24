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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that this <see cref="DateTime"/> or <see cref="DateTimeOffset"/> typed member has special characteristics on MessagePack serialization.
	/// </summary>
	/// <remarks>
	///		If this attributes is used for incompatible typed members, this attribute will be ignored.
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public sealed class MessagePackDateTimeMemberAttribute : Attribute
	{
		/// <summary>
		///		Gets or sets the default serialization method for this enum typed member.
		/// </summary>
		/// <value>
		///		The default serialization method for this enum typed member.
		///		Note that the method for the enum type will be overrided with this.
		/// </value>
		public DateTimeMemberConversionMethod DateTimeConversionMethod { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackDateTimeMemberAttribute"/> class.
		/// </summary>
		public MessagePackDateTimeMemberAttribute() { }
	}
}