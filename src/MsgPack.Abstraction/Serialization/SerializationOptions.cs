// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;

namespace MsgPack.Serialization
{
	public sealed class SerializationOptions
	{
		public static SerializationOptions Default { get; } = new SerializationOptionsBuilder().Create();

		public int MaxDepth { get; }

		public Encoding? StringEncoding { get; }

		public SerializationMethod? PreferredSerializationMethod { get; }

		/// <summary>
		///		Gets or sets a value indicating whether generated and/or reflection serializers should not access non public members via privileged reflection.
		/// </summary>
		/// <value>
		///		<c>true</c> if privileged reflection access is disabled; otherwise, <c>false</c>. Defaults to <c>false</c>.
		/// </value>
		/// <remarks>
		///		The privileged reflection means:
		///		<list type="bullet">
		///			<item>Access for non-public fields or property accessors via reflection. This operation requires <c>ReflectionPermission</c> of <c>MemberAccess</c> or <c>RestrictedMemberAccess</c>.</item>
		///			<item>Writing values for init only fields via reflection. This operation requires <c>SecurityPermission</c> of <c>SerializationFormatter</c>.</item>
		///		</list>
		///		If the program run on non-privileged Silverlight environment or restricted desktop CLR,
		///		serialization and deserialization should fail with <c>SecurityException</c>.
		/// </remarks>
		public bool DisablePrivilegedAccess { get; }


		internal SerializationOptions(
			SerializationOptionsBuilder builder
		)
		{
			this.MaxDepth = builder.MaxDepth;
			this.StringEncoding = builder.StringEncoding;
			this.PreferredSerializationMethod = builder.PreferredSerializationMethod;
			this.DisablePrivilegedAccess = builder.DisablePrivilegedAccess;
		}
	}
}
