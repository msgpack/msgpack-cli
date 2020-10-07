// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that this enum typed member has special characteristics on MessagePack serialization.
	/// </summary>
	/// <remarks>
	///		If this attributes is used for non-enum typed members, this attribute will be ignored.
	///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="EnumSerialization"]'/>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class MessagePackEnumMemberAttribute : Attribute
	{
		/// <summary>
		///		Gets or sets the default serialization method for this enum typed member.
		/// </summary>
		/// <value>
		///		The default serialization method for this enum typed member.
		///		Note that the method for the enum type will be overrided with this.
		/// </value>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="EnumSerialization"]'/>
		/// </remarks>
		public EnumMemberSerializationMethod SerializationMethod { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackEnumMemberAttribute"/> class.
		/// </summary>
		public MessagePackEnumMemberAttribute() { }
	}
}
