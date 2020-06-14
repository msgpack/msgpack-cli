// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Internal
{
	/// <summary>
	///		Represents type code of extention types.
	/// </summary>
	public readonly struct MessagePackExtensionType : IEquatable<MessagePackExtensionType>
	{
		public byte TypeCode { get; }
		public bool IsSystemType => this.TypeCode < 0;

		internal MessagePackExtensionType(byte typeCode)
		{
			this.TypeCode = typeCode;
		}

		public static MessagePackExtensionType Define(byte typeCode)
			=> new MessagePackExtensionType(Ensure.IsNotGreaterThan(typeCode, (byte)SByte.MaxValue));

		/// <inheritdoc />
		public bool Equals(MessagePackExtensionType other)
			=> this.TypeCode == other.TypeCode;

		/// <inheritdoc />
		public override bool Equals(object? obj)
			=> (obj is MessagePackExtensionType other) ? this.Equals(other) : false;

		/// <inheritdoc />
		public override int GetHashCode()
			=> this.TypeCode.GetHashCode();

		public static bool operator ==(MessagePackExtensionType left, MessagePackExtensionType right)
			=> left.Equals(right);

		public static bool operator !=(MessagePackExtensionType left, MessagePackExtensionType right)
			=> !left.Equals(right);
	}
}
