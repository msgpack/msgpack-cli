// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that this enum type has special characteristics on MessagePack serialization.
	/// </summary>
	/// <remarks>
	///		Enum types which are not marked with this attribute will be serialized as <see cref="SerializerProvider.EnumSerializationMethod"/> value.
	///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="EnumSerialization"]'/>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
	public sealed class MessagePackEnumAttribute : Attribute
	{
		/// <summary>
		///		Gets or sets the default serialization method for this enum type.
		/// </summary>
		/// <value>
		///		The default serialization method for this enum type.
		///		Note that the method for individual enum typed members will be overrided with <see cref="MessagePackEnumMemberAttribute"/>.
		/// </value>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="EnumSerialization"]'/>
		/// </remarks>
		public EnumSerializationMethod SerializationMethod { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackEnumAttribute"/> class.
		/// </summary>
		public MessagePackEnumAttribute() { }
	}
}
