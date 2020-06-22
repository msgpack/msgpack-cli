// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks the field or the property should not be serialized/deserialized with MessagePack for CLI serialization mechanism.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class MessagePackIgnoreAttribute : Attribute
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackIgnoreAttribute"/> class.
		/// </summary>
		public MessagePackIgnoreAttribute() { }
	}
}
